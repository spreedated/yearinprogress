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
        private readonly WindowDragHandler _dragHandler;
        private readonly WindowManager _wm;
        private readonly MainWindowViewModel _vm;

        public MainWindow()
        {
            this.InitializeComponent();
            _vm = new(this.GridYear);

            this.DataContext = _vm;
            _wm = new(this);

            _dragHandler = new(this)
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

                _dragHandler.PointerReleased += (s, e) =>
                {
                    Globals.Configuration.RuntimeConfiguration.WindowLocation = new(this.Position.X, this.Position.Y);
                    Task.Run(async () => await Globals.Configuration.SaveAsync());
                };

                _wm.RestoreWindowLocation(Globals.Configuration.RuntimeConfiguration.WindowLocation);
            }

            _vm.Instance = this;
        }

        private void Window_Opened(object sender, EventArgs e)
        {
            _wm.HideFromAltTab();
        }
    }
}