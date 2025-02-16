using Avalonia.Controls;
using YearInProgress.ViewLogic;
using YearInProgress.ViewModels;

namespace YearInProgress.Views
{
    public partial class Changelog : Window
    {
        public Changelog()
        {
            this.InitializeComponent();
            this.DataContext = new ChangelogViewModel();
            _ = new WindowDragHandler(this);
            ((ChangelogViewModel)this.DataContext).WindowInstance = this;
        }
    }
}