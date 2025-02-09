using Avalonia.Controls;
using YearInProgress.ViewLogic;
using YearInProgress.ViewModels;

namespace YearInProgress.Views;

public partial class Options : Window
{
    public Options()
    {
        this.InitializeComponent();
        this.DataContext = new OptionsViewModel();
        _= new WindowDragHandler(this);

        ((OptionsViewModel)this.DataContext).WindowInstance = this;
    }
}