using Avalonia.Controls;
using System.Linq;
using YearInProgress.Logic;
using YearInProgress.ViewLogic;
using YearInProgress.ViewModels;

namespace YearInProgress.Views
{
    public partial class MainWindow : Window
    {
        private readonly WindowDragHandler dragHandler;

        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = new MainWindowViewModel();
            this.dragHandler = new(this);

            this.dragHandler.PointerReleased += (s, e) =>
            {
                Globals.Configuration.RuntimeConfiguration.LocationX = this.Position.X;
                Globals.Configuration.RuntimeConfiguration.LocationY = this.Position.Y;
                Globals.Configuration.Save();
            };

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

            for (int i = 0; i < 10; i++)
            {
                this.GridYear.ColumnDefinitions.Add(new ColumnDefinition());
                this.GridYear.RowDefinitions.Add(new RowDefinition());
            }

            ((MainWindowViewModel)this.DataContext).Initialize(this.GridYear);
            ((MainWindowViewModel)this.DataContext).WindowInstance = this;
        }
    }
}