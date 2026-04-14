using System;

namespace TheLegends.Base.UI
{
    /// <summary>
    /// Contract for a composable UI motion animation component.
    /// Each implementation handles a single visual property (fade, scale, slide, etc.)
    /// and can be combined with others on the same GameObject for complex transitions.
    /// </summary>
    public interface IUIMotion
    {
        /// <summary>Whether this motion is currently playing.</summary>
        bool IsPlaying { get; }

        /// <summary>
        /// Execution priority for <see cref="UITransitionMode.Sequential"/> mode.
        /// Lower values execute first.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Total time this motion takes to complete, including delay.
        /// Calculated as <c>Delay + Duration</c>.
        /// </summary>
        float TotalDuration { get; }

        /// <summary>
        /// Plays the motion in the forward direction (From → To).
        /// Used during screen show transitions.
        /// </summary>
        /// <param name="onComplete">Optional callback invoked when the motion finishes.</param>
        void Play(Action onComplete = null);

        /// <summary>
        /// Plays the motion in the reverse direction (To → From).
        /// Used during screen hide transitions.
        /// </summary>
        /// <param name="onComplete">Optional callback invoked when the motion finishes.</param>
        void PlayReverse(Action onComplete = null);

        /// <summary>Immediately snaps to the starting value (From). No animation.</summary>
        void ResetToStart();

        /// <summary>Immediately snaps to the ending value (To). No animation.</summary>
        void ResetToEnd();

        /// <summary>Cancels any active tween and releases the motion handle.</summary>
        void Kill();
    }
}
