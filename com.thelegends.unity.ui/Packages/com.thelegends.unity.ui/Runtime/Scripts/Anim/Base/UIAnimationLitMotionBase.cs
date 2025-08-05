using LitMotion;
using UnityEngine;

namespace TheLegends.Base.UI
{
    /// <summary>
    /// Base class for all LitMotion-based animations
    /// Handles common Play/Stop/Pause logic to reduce code duplication
    /// </summary>
    public abstract class UIAnimationLitMotionBase<T> : UIAnimationBase where T : unmanaged
    {
        protected MotionHandle _motionHandle;

        public override void Play()
        {
            if (_motionHandle.IsActive())
            {
                _motionHandle.PlaybackSpeed = 1f;
                return;
            }

            InvokeOnStart();
            _motionHandle = CreateMotion();
        }

        public override void Stop()
        {
            if (_motionHandle.IsActive())
            {
                _motionHandle.TryCancel();
            }
            base.Stop();
        }

        public override void Pause()
        {
            if (_motionHandle.IsActive())
            {
                _motionHandle.PlaybackSpeed = 0f;
            }
            base.Pause();
        }

        /// <summary>
        /// Create the specific motion for this animation type
        /// Must be implemented by derived classes
        /// </summary>
        protected abstract MotionHandle CreateMotion();

        /// <summary>
        /// Get the motion settings for this animation
        /// Must be implemented by derived classes
        /// </summary>
        protected abstract SerializableMotionSettings<T, NoOptions> GetMotionSettings();
    }
}