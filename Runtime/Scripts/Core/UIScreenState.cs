namespace TheLegends.Base.UI
{
    /// <summary>
    /// Represents the lifecycle state of a UI screen.
    /// Transitions: Hidden → Showing → Visible → Hiding → Hidden.
    /// </summary>
    public enum UIScreenState
    {
        /// <summary>Screen is fully hidden and inactive.</summary>
        Hidden,

        /// <summary>Screen is playing its show transition.</summary>
        Showing,

        /// <summary>Screen is fully visible and interactive.</summary>
        Visible,

        /// <summary>Screen is playing its hide transition.</summary>
        Hiding
    }
}
