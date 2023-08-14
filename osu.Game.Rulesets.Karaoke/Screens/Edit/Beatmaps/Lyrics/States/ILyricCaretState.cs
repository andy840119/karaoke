// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

public interface ILyricCaretState
{
    /// <summary>
    /// The hover caret position.
    /// </summary>
    ICaretPosition? HoverCaretPosition => BindableHoverCaretPosition.Value;

    /// <summary>
    /// The caret position.
    /// </summary>
    ICaretPosition? CaretPosition => BindableCaretPosition.Value;

    /// <summary>
    /// The range caret position.
    /// </summary>
    RangeCaretPosition? RangeCaretPosition => BindableRangeCaretPosition.Value;

    /// <summary>
    /// The hover caret position.
    /// Will have value when user hover the lyric.
    /// </summary>
    IBindable<ICaretPosition?> BindableHoverCaretPosition { get; }

    /// <summary>
    /// The caret position.
    /// Will have value when user click the lyric.
    /// And will not have value when user drag to select the range of index in the lyric.
    /// </summary>
    IBindable<ICaretPosition?> BindableCaretPosition { get; }

    /// <summary>
    /// The range caret position.
    /// Will have value when user drag to select the range of index in the lyric.
    /// </summary>
    IBindable<RangeCaretPosition?> BindableRangeCaretPosition { get; }

    /// <summary>
    /// Current focused lyric.
    /// Will have value if <see cref="BindableCaretPosition"/> has the value.
    /// </summary>
    IBindable<Lyric?> BindableFocusedLyric { get; }

    /// <summary>
    /// Move the caret by action.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    bool MoveCaret(MovingCaretAction action);

    /// <summary>
    /// Used to predict the caret position by <see cref="MovingCaretAction"/>.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    ICaretPosition? GetCaretPositionByAction(MovingCaretAction action);

    /// <summary>
    /// Move the hover caret position by target of <see cref="Lyric"/>.
    /// </summary>
    /// <param name="lyric"></param>
    /// <returns></returns>
    bool MoveHoverCaretToTargetPosition(Lyric lyric);

    /// <summary>
    /// Move the hover caret position by target of <see cref="Lyric"/> and index.
    /// </summary>
    /// <param name="lyric"></param>
    /// <param name="index"></param>
    /// <typeparam name="TIndex"></typeparam>
    /// <returns></returns>
    bool MoveHoverCaretToTargetPosition<TIndex>(Lyric lyric, TIndex index) where TIndex : notnull;

    /// <summary>
    /// Change the hover caret position to the current caret position.
    /// Will clear the hover caret position.
    /// </summary>
    /// <returns></returns>
    bool ConfirmHoverCaretPosition();

    /// <summary>
    /// Clear the hover caret position.
    /// </summary>
    /// <returns></returns>
    bool ClearHoverCaretPosition();

    /// <summary>
    /// Move the caret position by target of <see cref="Lyric"/>.
    /// </summary>
    /// <param name="lyric"></param>
    /// <returns></returns>
    bool MoveCaretToTargetPosition(Lyric lyric);

    /// <summary>
    /// Move the caret position by target of <see cref="Lyric"/> and index.
    /// </summary>
    /// <param name="lyric"></param>
    /// <param name="index"></param>
    /// <typeparam name="TIndex"></typeparam>
    /// <returns></returns>
    bool MoveCaretToTargetPosition<TIndex>(Lyric lyric, TIndex index) where TIndex : notnull;

    /// <summary>
    /// Start dragging.
    /// </summary>
    /// <returns></returns>
    bool StartDragging();

    /// <summary>
    /// Will update the range of caret position by end index.
    /// And notice that will clear the <see cref="BindableCaretPosition"/> after assigned the end caret index.
    /// </summary>
    /// <param name="index"></param>
    /// <typeparam name="TIndex"></typeparam>
    /// <returns></returns>
    bool MoveDraggingCaretIndex<TIndex>(TIndex index) where TIndex : notnull;

    /// <summary>
    /// Ebd dragging.
    /// </summary>
    /// <returns></returns>
    bool EndDragging();

    /// <summary>
    /// Update the <see cref="CaretPosition"/> or <see cref="RangeCaretPosition"/> to the selected <see cref="Lyric"/> list in the<see cref="EditorBeatmap"/>
    /// </summary>
    void SyncSelectedHitObjectWithCaret();

    /// <summary>
    /// Confirm is able to change <see cref="HoverCaretPosition"/> or <see cref="CaretPosition"/> or not.
    /// </summary>
    bool CaretEnabled { get; }

    /// <summary>
    /// Confirm is able to change <see cref="RangeCaretPosition"/> or not.
    /// </summary>
    bool CaretDraggable { get; }

    ICaretPositionAlgorithm? CaretPositionAlgorithm => BindableCaretPositionAlgorithm.Value;

    IBindable<ICaretPositionAlgorithm?> BindableCaretPositionAlgorithm { get; }
}
