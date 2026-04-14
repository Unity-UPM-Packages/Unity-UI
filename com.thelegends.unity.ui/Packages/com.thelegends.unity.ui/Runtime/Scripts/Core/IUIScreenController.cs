using System;

namespace TheLegends.Base.UI
{
    /// <summary>
    /// Contract for the main UI screen controller.
    /// Manages screen visibility lifecycle and fires transition events.
    /// </summary>
    public interface IUIScreenController
    {
        /// <summary>Current lifecycle state of the screen.</summary>
        UIScreenState State { get; }

        /// <summary>
        /// Shows the screen by playing all configured motions in the forward direction.
        /// Fires <see cref="OnShowing"/> immediately, then <see cref="OnShown"/> when all motions complete.
        /// </summary>
        void Show();

        /// <summary>
        /// Hides the screen by playing all configured motions in reverse.
        /// Fires <see cref="OnHiding"/> immediately, then <see cref="OnHidden"/> when all motions complete.
        /// </summary>
        void Hide();

        /// <summary>Fires when the screen begins its show transition. Motions are starting.</summary>
        event Action OnShowing;

        /// <summary>Fires when the screen has fully appeared. All show motions have completed.</summary>
        event Action OnShown;

        /// <summary>Fires when the screen begins its hide transition. Motions are starting.</summary>
        event Action OnHiding;

        /// <summary>Fires when the screen has fully disappeared. All hide motions have completed.</summary>
        event Action OnHidden;
    }
}
