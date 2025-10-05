using Avalonia.Controls;
using neXn.Ui.Avalonia;
using System;
using YearInProgress.Logic;
using YearInProgress.ViewModels;

namespace YearInProgress.Views
{
    public partial class MainWindow : Window
    {
        private readonly WindowDragHandler dragHandler;
        private readonly WindowManager wm;

        public MainWindow()
        {
            HelperFunctions.SetTheme();

            this.InitializeComponent();
            this.DataContext = new MainWindowViewModel();
            this.wm = new(this);
            this.dragHandler = new(this)
            {
                IsEnabled = true
            };

            this.dragHandler.PointerReleased += (s, e) =>
            {
                Globals.Configuration.RuntimeConfiguration.WindowLocation = new(this.Position.X, this.Position.Y);
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
            this.wm.RestoreWindowLocation(Globals.Configuration.RuntimeConfiguration.WindowLocation);
        }

        private void Window_Opened(object sender, EventArgs e)
        {
            this.wm.HideFromAltTab();
        }
    }
}