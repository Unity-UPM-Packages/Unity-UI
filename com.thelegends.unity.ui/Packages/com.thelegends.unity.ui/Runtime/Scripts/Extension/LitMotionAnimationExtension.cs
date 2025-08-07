using LitMotion;
using LitMotion.Animation;

namespace TheLegends.Base.UI
{
    public static class LitMotionAnimationExtension
    {
        /// summary>
        /// Complete the motion. If it is not played, play it and then complete. The motion remains playing.
        /// <summary>
        public static void Complete(this LitMotionAnimation animation)
        {
            if (!animation.IsPlaying) animation.Play();

            foreach (var component in animation.Components)
            {
                var handle = component.TrackedHandle;
                handle.TryComplete();
                component.TrackedHandle = handle;
            }
        }

        /// <summary>
        /// Cancels the motion that is playing. If it is a built-in Stop, the motion state is destroyed and the object returns to its original state.
        /// Cancel leaves the state when it was canceled.
        /// </summary>
        public static void Cancel(this LitMotionAnimation animation)
        {
            foreach (var component in animation.Components)
            {
                var handle = component.TrackedHandle;
                handle.TryCancel();
                component.TrackedHandle = handle;
            }
        }

        /// <summary>
        /// Specify the playback speed and playback. The argument is the multiplier
        /// </summary>
        public static void Play(this LitMotionAnimation animation, float playbackSpeed)
        {
            if (!animation.IsPlaying) animation.Play();

            foreach (var component in animation.Components)
            {
                var handle = component.TrackedHandle;
                handle.PlaybackSpeed = playbackSpeed;
                component.TrackedHandle = handle;
            }
        }

        /// <summary>
        /// Transitions to the first state of the motion. Cancels playback, so it becomes unplayed.
        /// <summary>
        public static void Rewind(this LitMotionAnimation animation)
        {
            if (!animation.IsPlaying) animation.Play();

            foreach (var component in animation.Components)
            {
                var handle = component.TrackedHandle;
                handle.Time = 0;
                component.TrackedHandle = handle;
            }
            animation.Cancel();
        }

    }
}
