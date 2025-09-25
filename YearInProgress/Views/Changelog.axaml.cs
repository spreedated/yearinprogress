using Avalonia.Controls;
using neXn.Ui.Avalonia;
using YearInProgress.ViewModels;

namespace YearInProgress.Views
{
    public partial class Changelog : Window
    {
        public Changelog()
        {
            this.InitializeComponent();
            this.DataContext = new ChangelogViewModel();
            _ = new WindowDragHandler(this)
            {
                IsEnabled = true
            };
            ((ChangelogViewModel)this.DataContext).WindowInstance = this;
        }
    }
}