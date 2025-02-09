using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System;

namespace YearInProgress.ViewLogic
{
    public class WindowDragHandler
    {
        private readonly Window window;
        private bool mouseDownForWindowMoving = false;
        private PointerPoint originalPoint;

        public event EventHandler PointerPressed;
        public event EventHandler PointerReleased;
        public event EventHandler PointerMoved;

        #region Ctor
        /// <summary>
        /// Handles dragging of a window.<br/>
        /// Subscribes to<br/><b>PointerPressed</b>,<br/><b>PointerReleased</b><br/>and<br/><b>PointerMoved</b><br/>
        /// events of the window
        /// </summary>
        /// <param name="window"></param>
        public WindowDragHandler(Window window)
        {
            this.window = window;
            this.window.PointerPressed += this.Window_PointerPressed;
            this.window.PointerReleased += this.Window_PointerReleased;
            this.window.PointerMoved += this.Window_PointerMoved;
        }
        #endregion

        private void Window_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (window.WindowState == WindowState.Maximized || window.WindowState == WindowState.FullScreen)
            {
                return;
            }

            if (e.Pointer.Type == PointerType.Mouse && !e.GetCurrentPoint(window).Properties.IsLeftButtonPressed)
            {
                return;
            }

            mouseDownForWindowMoving = true;
            originalPoint = e.GetCurrentPoint(window);
            this.PointerPressed?.Invoke(this, EventArgs.Empty);
        }

        private void Window_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            mouseDownForWindowMoving = false;

            this.PointerReleased?.Invoke(this, EventArgs.Empty);
        }

        private void Window_PointerMoved(object sender, PointerEventArgs e)
        {
            if (!mouseDownForWindowMoving)
            {
                return;
            }

            PointerPoint currentPoint = e.GetCurrentPoint(window);
            window.Position = new PixelPoint(window.Position.X + (int)(currentPoint.Position.X - originalPoint.Position.X), window.Position.Y + (int)(currentPoint.Position.Y - originalPoint.Position.Y));

            this.PointerMoved?.Invoke(this, EventArgs.Empty);
        }
    }
}
