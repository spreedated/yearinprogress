using Avalonia.Controls;
using neXn.Ui.Avalonia;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using YearInProgress.Logic;
using YearInProgress.ViewModels;

namespace YearInProgress.Views
{
    public partial class MainWindow : Window
    {
        private readonly WindowDragHandler dragHandler;

        public MainWindow()
        {
            HelperFunctions.SetTheme();

            this.InitializeComponent();
            this.DataContext = new MainWindowViewModel();
            this.dragHandler = new(this)
            {
                IsEnabled = true
            };

            this.dragHandler.PointerReleased += (s, e) =>
            {
                Globals.Configuration.RuntimeConfiguration.LocationX = this.Position.X;
                Globals.Configuration.RuntimeConfiguration.LocationY = this.Position.Y;
                Globals.Configuration.Save();
            };

            for (int i = 0; i < 10; i++)
            {
                this.GridYear.ColumnDefinitions.Add(new ColumnDefinition());
                this.GridYear.RowDefinitions.Add(new RowDefinition());
            }

            ((MainWindowViewModel)this.DataContext).Initialize(this.GridYear);
            ((MainWindowViewModel)this.DataContext).WindowInstance = this;
        }

        private void Window_Loaded(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if ((Globals.Configuration.RuntimeConfiguration.LocationX == default && Globals.Configuration.RuntimeConfiguration.LocationY == default) ||
                Globals.Configuration.RuntimeConfiguration.LocationX > base.Screens.All.Sum(x => x.Bounds.Size.Width) ||
                Globals.Configuration.RuntimeConfiguration.LocationY > base.Screens.All.Sum(x => x.Bounds.Size.Height))
            {
                this.Position = new((int)((base.Screens.Primary.Bounds.Size.Width / 2) - (this.Width / 2)),
                    (int)((base.Screens.Primary.Bounds.Size.Height / 2) - (this.Height / 2)));
            }
            else
            {
                this.Position = new(Globals.Configuration.RuntimeConfiguration.LocationX, Globals.Configuration.RuntimeConfiguration.LocationY);
            }
        }

        private void Window_Opened(object sender, EventArgs e)
        {
            if (OperatingSystem.IsWindows())
            {
                var platformImpl = this.TryGetPlatformHandle();
                var handle = platformImpl?.Handle ?? IntPtr.Zero;
                if (handle != IntPtr.Zero)
                {
                    const int GWL_EXSTYLE = -20;
                    const int WS_EX_TOOLWINDOW = 0x00000080;
                    const int WS_EX_APPWINDOW = 0x00040000;

                    int exStyle = (int)GetWindowLong(handle, GWL_EXSTYLE);
                    exStyle |= WS_EX_TOOLWINDOW;
                    exStyle &= ~WS_EX_APPWINDOW;
                    _ = SetWindowLong(handle, GWL_EXSTYLE, exStyle);
                }
            }
        }

        [LibraryImport("user32.dll", EntryPoint = "GetWindowLongPtrW")]
        private static partial IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [LibraryImport("user32.dll", EntryPoint = "SetWindowLongPtrW")]
        private static partial IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
    }
}