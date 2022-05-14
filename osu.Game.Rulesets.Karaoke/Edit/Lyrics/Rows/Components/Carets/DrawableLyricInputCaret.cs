﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Carets
{
    public class DrawableLyricInputCaret : DrawableLyricTextCaret
    {
        private const float caret_move_time = 60;
        private const float caret_width = 3;

        private readonly Box drawableCaret;
        private readonly InputCaretTextBox inputCaretTextBox;

        private TextCaretPosition caretPosition;

        [Resolved]
        private EditorKaraokeSpriteText karaokeSpriteText { get; set; }

        [Resolved]
        private ILyricTextChangeHandler lyricTextChangeHandler { get; set; }

        [Resolved]
        private ILyricCaretState lyricCaretState { get; set; }

        public DrawableLyricInputCaret(bool preview)
            : base(preview)
        {
            Width = caret_width;

            InternalChild = drawableCaret = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.White,
                Alpha = preview ? 0.5f : 1
            };

            if (!preview)
            {
                AddInternal(inputCaretTextBox = new InputCaretTextBox
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopLeft,
                    Width = 50,
                    Height = 20,
                    ReleaseFocusOnCommit = false,
                    NewCommitText = text =>
                    {
                        if (caretPosition == null)
                            throw new ArgumentNullException(nameof(caretPosition));

                        int index = caretPosition.Index;
                        lyricTextChangeHandler.InsertText(index, text);

                        moveCaret(text.Length);
                    },
                    DeleteText = () =>
                    {
                        if (caretPosition == null)
                            throw new ArgumentNullException(nameof(caretPosition));

                        int index = caretPosition.Index;
                        if (index == 0)
                            return;

                        lyricTextChangeHandler.DeleteLyricText(index);

                        moveCaret(-1);
                    }
                });
            }

            void moveCaret(int offset)
            {
                if (caretPosition == null)
                    throw new ArgumentNullException(nameof(caretPosition));

                // calculate new caret position.
                var lyric = caretPosition.Lyric;
                int index = caretPosition.Index + offset;
                caretPosition = new TextCaretPosition(lyric, index);
                lyricCaretState.MoveCaretToTargetPosition(caretPosition);
            }
        }

        public override void Hide()
        {
            this.FadeOut(200);
        }

        protected override void Apply(TextCaretPosition caret)
        {
            caretPosition = caret;

            Height = karaokeSpriteText.LineBaseHeight;
            var position = GetPosition(caret);

            bool displayAnimation = Alpha > 0;
            int time = displayAnimation ? 60 : 0;

            this.MoveTo(new Vector2(position.X - caret_width / 2, position.Y), time, Easing.Out);
            this.ResizeWidthTo(caret_width, caret_move_time, Easing.Out);

            drawableCaret
                .FadeColour(Color4.White, 200, Easing.Out)
                .Loop(c => c.FadeTo(0.7f).FadeTo(0.4f, 500, Easing.InOutSine));

            if (inputCaretTextBox == null)
                return;

            // should focus the caret if change the state.
            Schedule(() =>
            {
                inputCaretTextBox.Text = string.Empty;
                GetContainingInputManager().ChangeFocus(inputCaretTextBox);
            });
        }

        private class InputCaretTextBox : BasicTextBox
        {
            // should not accept tab event because all focus/unfocus should be controlled by caret.
            public override bool CanBeTabbedTo => false;
            public Action<string> NewCommitText;

            public Action DeleteText;

            // should not allow cursor index change because press left/right event is handled by parent caret.
            protected override bool AllowWordNavigation => false;

            public InputCaretTextBox()
            {
                OnCommit += submitMessage;

                void submitMessage(TextBox textBox, bool newText)
                {
                    string text = textBox.Text;

                    if (string.IsNullOrEmpty(text))
                        return;

                    NewCommitText?.Invoke(text);

                    textBox.Text = string.Empty;
                }
            }

            public override bool OnPressed(KeyBindingPressEvent<PlatformAction> e)
            {
                bool triggerDeleteText = processTriggerDeleteText(e.Action);
                bool triggerMoveTextCaretIndex = processTriggerMoveText(e.Action);

                // should trigger delete the main text in lyric if there's not pending text.
                if (triggerDeleteText && string.IsNullOrEmpty(Text))
                {
                    DeleteText?.Invoke();
                    return true;
                }

                // should not block the move left/right event if there's on text in the text box.
                if (triggerMoveTextCaretIndex && string.IsNullOrEmpty(Text)) return false;

                return base.OnPressed(e);

                static bool processTriggerDeleteText(PlatformAction action) =>
                    action switch
                    {
                        // Deletion
                        PlatformAction.DeleteBackwardChar => true,
                        _ => false
                    };

                static bool processTriggerMoveText(PlatformAction action) =>
                    action switch
                    {
                        // Move left/right actions.
                        PlatformAction.MoveBackwardChar => true,
                        PlatformAction.MoveForwardChar => true,
                        PlatformAction.MoveBackwardWord => true,
                        PlatformAction.MoveForwardWord => true,
                        PlatformAction.MoveBackwardLine => true,
                        PlatformAction.MoveForwardLine => true,
                        _ => false
                    };
            }
        }
    }
}
