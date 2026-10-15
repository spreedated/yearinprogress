using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using YearInProgress.Logic;

namespace YearInProgress.ViewModels
{
    internal partial class OptionsViewModel : ViewModelBase
    {
        #region BindableProperties
        [ObservableProperty]
        private DateTimeOffset? _birthdayDate;
        partial void OnBirthdayDateChanged(DateTimeOffset? value)
        {
            this.CalculateMinimumRetirementYear();
            Globals.Configuration.RuntimeConfiguration.Birthday = new(value.Value.Year, value.Value.Month, value.Value.Day, 0, 0, 0, DateTimeKind.Local);
            Globals.Configuration.SaveAsync().Wait();
        }

        [ObservableProperty]
        public partial int RetirementYear { get; set; } = default;
        partial void OnRetirementYearChanged(int value)
        {
            Globals.Configuration.RuntimeConfiguration.RetirementWithYear = value;
            Globals.Configuration.SaveAsync().Wait();
        }

        [ObservableProperty]
        public partial int RetirementYearMinimum { get; set; } = default;

        [ObservableProperty]
        public partial int RefreshrateOfRandomRetirementStringInMinutes { get; set; } = default;
        partial void OnRefreshrateOfRandomRetirementStringInMinutesChanged(int value)
        {
            Globals.Configuration.RuntimeConfiguration.RefreshrateOfRandomRetirementString = value * 60;
            Globals.Configuration.SaveAsync().Wait();
        }

        [ObservableProperty]
        public partial bool IsDarkModeActive { get; set; } = false;
        partial void OnIsDarkModeActiveChanged(bool value)
        {
            Globals.Configuration.RuntimeConfiguration.DarkTheme = value;
            Task.Run(async () => await Globals.Configuration.SaveAsync());
            HelperFunctions.SetTheme();
        }
        #endregion

        #region Ctor
        public OptionsViewModel() : base("Options")
        {
            if (!Design.IsDesignMode)
            {
                this.IsDarkModeActive = Globals.Configuration.RuntimeConfiguration.DarkTheme;
                this.CalculateMinimumRetirementYear();

                this.BirthdayDate = Globals.Configuration.RuntimeConfiguration.Birthday;
                this.RetirementYear = Globals.Configuration.RuntimeConfiguration.RetirementWithYear;
                this.RefreshrateOfRandomRetirementStringInMinutes = (int)Math.Round(Globals.Configuration.RuntimeConfiguration.RefreshrateOfRandomRetirementString / 60d);
            }
        }
        #endregion

        private void CalculateMinimumRetirementYear()
        {
#pragma warning disable S6561
            this.RetirementYearMinimum = ((DateTime.Now - Globals.Configuration.RuntimeConfiguration.Birthday).Days / 365) + 1;
#pragma warning restore S6561
        }

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
