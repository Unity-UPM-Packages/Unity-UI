namespace TheLegends.Base.UI
{
    /// <summary>
    /// Specifies the direction for <see cref="UISlideMotion"/> enter and exit transitions.
    /// All values produce screen-relative offsets computed from the root Canvas and element dimensions.
    /// For arbitrary position tweening, use <see cref="UITranslateMotion"/> instead.
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
        Down
    }
}
