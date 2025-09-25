using Avalonia.Controls;
using neXn.Ui.Avalonia;
using YearInProgress.ViewModels;

namespace YearInProgress.Views;

public partial class Options : Window
{
    public Options()
    {
        this.InitializeComponent();
        this.DataContext = new OptionsViewModel();
        _ = new WindowDragHandler(this)
        {
            IsEnabled = true
        };

        ((OptionsViewModel)this.DataContext).WindowInstance = this;
    }
}