namespace TheLegends.Base.UI
{
    /// <summary>
    /// Interface for all playable UI animation components and groups
    /// </summary>
    public interface IUIPlayable
    {
        /// <summary>
        /// Start playing the animation(s)
        /// </summary>
        void Play();
        
        /// <summary>
        /// Stop the animation(s)
        /// </summary>
        void Stop();
        
        /// <summary>
        /// Pause the animation(s)
        /// </summary>
        void Pause();
        
        /// <summary>
        /// Restart the animation(s) from the beginning
        /// </summary>
        void Restart();
        
        /// <summary>
        /// Whether the animation(s) are currently active
        /// </summary>
        bool IsActive { get; }
        
        /// <summary>
        /// Whether the animation(s) are currently playing
        /// </summary>
        bool IsPlaying { get; }
    }
}