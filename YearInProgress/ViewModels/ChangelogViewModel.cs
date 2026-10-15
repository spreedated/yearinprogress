using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using YearInProgress.Logic;

namespace YearInProgress.ViewModels
{
    internal partial class ChangelogViewModel : ViewModelBase
    {
        #region BindableProperties
        [ObservableProperty]
        public partial string Changelog { get; set; }
        #endregion

        #region Ctor
        public ChangelogViewModel() : base("Changelog")
        {
            Task.Run(() =>
            {
                Dispatcher.UIThread.Invoke(() => this.Changelog = HelperFunctions.ReadEmbeddedChangelog());
            });
        }
        #endregion

        #region Commmands
        [RelayCommand]
        private void Exit()
        {
            base.Instance.Close();
        }

        [RelayCommand]
        private static void BuyMeACoffee()
        {
            HelperFunctions.OpenWebsite(Constants.BUY_ME_A_COFFEE_URL);
        }
        #endregion
    }
}
