using Avalonia.Controls;
using neXn.Ui.Avalonia;
using System;
using System.Threading.Tasks;
using YearInProgress.Logic;
using YearInProgress.ViewModels;

namespace YearInProgress.Views
{
    public partial class MainWindow : Window
    {
        private WindowDragHandler dragHandler;
        private WindowManager wm;

        public MainWindow()
        {
            this.InitializeComponent();

            this.DataContext = new MainWindowViewModel();
            this.wm = new(this);

            this.dragHandler = new(this)
            {
                IsEnabled = true
            };

            for (int i = 0; i < 10; i++)
            {
                this.GridYear.ColumnDefinitions.Add(new ColumnDefinition());
                this.GridYear.RowDefinitions.Add(new RowDefinition());
            }

            if (!Design.IsDesignMode)
            {
                Logic.HelperFunctions.SetTheme();
                ((MainWindowViewModel)this.DataContext).Initialize(this.GridYear);

                this.dragHandler.PointerReleased += (s, e) =>
                {
                    Globals.Configuration.RuntimeConfiguration.WindowLocation = new(this.Position.X, this.Position.Y);
                    Task.Run(async () => await Globals.Configuration.SaveAsync());
                };

                this.wm.RestoreWindowLocation(Globals.Configuration.RuntimeConfiguration.WindowLocation);
            }

            ((MainWindowViewModel)this.DataContext).WindowInstance = this;
        }

        private void Window_Opened(object sender, EventArgs e)
        {
            this.wm.HideFromAltTab();
        }
    }
}