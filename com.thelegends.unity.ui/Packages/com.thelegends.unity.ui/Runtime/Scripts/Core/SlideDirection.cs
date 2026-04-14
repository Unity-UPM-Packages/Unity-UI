namespace TheLegends.Base.UI
{
    /// <summary>
    /// Specifies the direction for <see cref="UISlideMotion"/> enter and exit transitions.
    /// <see cref="Custom"/> falls back to a manual Vector2 offset value.
    /// </summary>
    public enum SlideDirection
    {
        /// <summary>Slides from or to the left of the screen.</summary>
        Left,

        /// <summary>Slides from or to the right of the screen.</summary>
        Right,

        /// <summary>Slides from or to the top of the screen.</summary>
        Up,

        /// <summary>Slides from or to the bottom of the screen.</summary>
        Down,

        /// <summary>Uses a manually specified Vector2 offset instead of a screen-relative calculation.</summary>
        Custom
    }
}
