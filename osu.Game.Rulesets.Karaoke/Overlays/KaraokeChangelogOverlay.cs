﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Octokit;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Game.Input.Bindings;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;
using osu.Game.Rulesets.Karaoke.Overlays.Changelog;
using osu.Game.Rulesets.Karaoke.Overlays.Changelog.Sidebar;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Overlays
{
    public class KaraokeChangelogOverlay : OnlineOverlay<ChangelogHeader>
    {
        public override bool IsPresent => base.IsPresent || Scheduler.HasPendingTasks;

        [Cached]
        public readonly Bindable<APIChangelogBuild> Current = new();

        protected override Color4 BackgroundColour => ColourProvider.Background4;

        private readonly Container sidebarContainer;
        private readonly ChangelogSidebar sidebar;
        private readonly Container content;

        private readonly string organizationName;
        private readonly string branchName;

        private string projectName => $"{organizationName}.github.io";

        private Sample sampleBack;

        private List<APIChangelogBuild> builds;

        private Task initialFetchTask;

        private CancellationTokenSource loadContentCancellation;

        public KaraokeChangelogOverlay(string organization, string branch = "master")
            : base(OverlayColourScheme.Purple, false)
        {
            organizationName = organization;
            branchName = branch;

            Child = new GridContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.AutoSize)
                },
                ColumnDimensions = new[]
                {
                    new Dimension(GridSizeMode.AutoSize),
                    new Dimension()
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        sidebarContainer = new Container
                        {
                            AutoSizeAxes = Axes.X,
                            Child = sidebar = new ChangelogSidebar()
                        },
                        content = new Container
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y
                        }
                    }
                }
            };
        }

        public void ShowListing()
        {
            Current.Value = null;
            Show();
        }

        /// <summary>
        ///     Fetches and shows a specific build from a specific update stream.
        /// </summary>
        /// <param name="build"> Singer <see cref="APIChangelogBuild" />.</param>
        public void ShowBuild([NotNull] APIChangelogBuild build)
        {
            if (build == null) throw new ArgumentNullException(nameof(build));

            Current.Value = build;
            Show();
        }

        public override bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
        {
            switch (e.Action)
            {
                case GlobalAction.Back:
                    if (Current.Value == null)
                        Hide();
                    else
                    {
                        Current.Value = null;
                        sampleBack?.Play();
                    }

                    return true;

                default:
                    return false;
            }
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();
            sidebarContainer.Height = DrawHeight;
            sidebarContainer.Y = Math.Clamp(ScrollFlow.Current - Header.DrawHeight, 0, Math.Max(ScrollFlow.ScrollContent.DrawHeight - DrawHeight - Header.DrawHeight, 0));
        }

        protected override ChangelogHeader CreateHeader()
        {
            return new()
            {
                ListingSelected = ShowListing
            };
        }

        protected override void PopIn()
        {
            base.PopIn();

            // fetch and refresh to show listing, if no other request was made via Show methods
            if (initialFetchTask == null)
            {
                performAfterFetch(() =>
                {
                    Current.TriggerChange();

                    int[] years = builds.Select(x => x.PublishedAt.Year).Distinct().ToArray();
                    sidebar.Year.Value = years.Max();
                    sidebar.Metadata.Value = new APIChangelogSidebar
                    {
                        Changelogs = builds,
                        Years = years
                    };
                });
            }
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            Header.Build.BindTo(Current);

            sampleBack = audio.Samples.Get(@"UI/generic-select-soft");

            Current.BindValueChanged(e =>
            {
                if (e.NewValue != null)
                    loadContent(new ChangelogSingleBuild(e.NewValue));
                else
                    loadContent(new ChangelogListing(builds));
            });
        }

        private void performAfterFetch(Action action)
        {
            fetchListing()?.ContinueWith(_ =>
                Schedule(action), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private Task fetchListing()
        {
            if (initialFetchTask != null)
                return initialFetchTask;

            return initialFetchTask = Task.Run(async () =>
            {
                var tcs = new TaskCompletionSource<bool>();

                var client = new GitHubClient(new ProductHeaderValue(organizationName));
                var reposAscending = await client.Repository.Content.GetAllContentsByRef(organizationName, projectName, "content/changelog", branchName).ConfigureAwait(false);

                if (reposAscending.Any())
                {
                    builds = reposAscending.Reverse().Where(x => x.Type == ContentType.Dir).Select(x => new APIChangelogBuild(organizationName, projectName, branchName)
                    {
                        RootUrl = x.HtmlUrl,
                        Path = x.Path,
                        DisplayVersion = x.Name,
                        PublishedAt = getPublishDateFromName(x.Name)
                    }).ToList();

                    foreach (var build in builds)
                    {
                        build.Versions.Previous = builds.GetPrevious(build);
                        build.Versions.Next = builds.GetNext(build);
                    }

                    tcs.SetResult(true);
                }
                else
                    tcs.SetResult(false);

                await tcs.Task.ConfigureAwait(false);
            });

            static DateTimeOffset getPublishDateFromName(string name)
            {
                var regex = new Regex("(?<year>[-0-9]+).(?<month>[-0-9]{2})(?<day>[-0-9]{2})");
                var result = regex.Match(name);
                if (!result.Success)
                    return DateTimeOffset.MaxValue;

                int year = result.GetGroupValue<int>("year");
                int month = result.GetGroupValue<int>("month");
                int day = result.GetGroupValue<int>("day");

                return new DateTimeOffset(new DateTime(year, month, day));
            }
        }

        private void loadContent(ChangelogContent newContent)
        {
            content.FadeTo(0.2f, 300, Easing.OutQuint);

            loadContentCancellation?.Cancel();

            LoadComponentAsync(newContent, c =>
            {
                content.FadeIn(300, Easing.OutQuint);

                // if content changed view version
                c.BuildSelected = ShowBuild;
                content.Child = c;
            }, (loadContentCancellation = new CancellationTokenSource()).Token);
        }
    }
}
