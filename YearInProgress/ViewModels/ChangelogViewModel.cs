using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using YearInProgress.Logic;

namespace YearInProgress.ViewModels
{
    internal partial class ChangelogViewModel : ObservableObject
    {
        #region BindableProperties
        [ObservableProperty]
        private Window windowInstance = null;

        [ObservableProperty]
        private string changelog = null;
        #endregion

        #region Ctor
        public ChangelogViewModel()
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
            this.WindowInstance.Close();
        }

        [RelayCommand]
        private static void BuyMeACoffee()
        {
            HelperFunctions.OpenWebsite(Constants.BUY_ME_A_COFFEE_URL);
        }
        #endregion
    }
}
