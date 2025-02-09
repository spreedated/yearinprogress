using Avalonia.Controls;
using YearInProgress.ViewLogic;
using YearInProgress.ViewModels;

namespace YearInProgress.Views;

public partial class Options : Window
{
#pragma warning disable IDE0052,S4487
    private readonly WindowDragHandler dragHandler;
#pragma warning restore IDE0052,S4487

    public Options()
    {
        this.InitializeComponent();
        this.DataContext = new OptionsViewModel();
        this.dragHandler = new WindowDragHandler(this);

        ((OptionsViewModel)this.DataContext).WindowInstance = this;
    }
}