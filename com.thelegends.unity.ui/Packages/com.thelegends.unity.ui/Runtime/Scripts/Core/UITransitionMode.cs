namespace TheLegends.Base.UI
{
    /// <summary>
    /// Defines how multiple <see cref="IUIMotion"/> components execute during a transition.
    /// </summary>
    public enum UITransitionMode
    {
        /// <summary>All motions play at the same time.</summary>
        Parallel,

        /// <summary>Motions play one after another, ordered by <see cref="IUIMotion.Order"/>.</summary>
        Sequential
    }
}
