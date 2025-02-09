using Avalonia;
using Avalonia.Controls;

namespace YearInProgress.ViewElements;

public partial class ProgressBoxAdvanced : UserControl
{
    public static readonly StyledProperty<double> ValueProperty = AvaloniaProperty.Register<ProgressBoxAdvanced, double>(nameof(Value));
    public double Value
    {
        get { return base.GetValue(ValueProperty); }
        set
        {
            base.SetValue(ValueProperty, value);
        }
    }

    public ProgressBoxAdvanced()
    {
        this.InitializeComponent();
        this.DataContext = this;
    }
}