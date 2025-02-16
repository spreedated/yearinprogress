#pragma warning disable S1075

using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Diagnostics;
using YearInProgress.Logic;
using YearInProgress.ViewLogic;

namespace YearInProgress.ViewModels
{
    internal partial class OptionsViewModel : ObservableObject
    {
        #region BindableProperties
        [ObservableProperty]
        private Window windowInstance = null;

        [ObservableProperty]
        private DateTimeOffset? birthdayDate;
        partial void OnBirthdayDateChanged(DateTimeOffset? value)
        {
            this.CalculateMinimumRetirementYear();
            Globals.Configuration.RuntimeConfiguration.Birthday = new(value.Value.Year, value.Value.Month, value.Value.Day, 0, 0, 0, DateTimeKind.Local);
            Globals.Configuration.Save();
        }

        [ObservableProperty]
        private int retirementYear = default;
        partial void OnRetirementYearChanged(int value)
        {
            Globals.Configuration.RuntimeConfiguration.RetirementWithYear = value;
            Globals.Configuration.Save();
        }

        [ObservableProperty]
        private int retirementYearMinimum = default;

        [ObservableProperty]
        private int refreshrateOfRandomRetirementStringInMinutes = default;
        partial void OnRefreshrateOfRandomRetirementStringInMinutesChanged(int value)
        {
            Globals.Configuration.RuntimeConfiguration.RefreshrateOfRandomRetirementString = value * 60;
            Globals.Configuration.Save();
        }
        #endregion

        #region Ctor
        public OptionsViewModel()
        {
            this.CalculateMinimumRetirementYear();

            this.BirthdayDate = Globals.Configuration.RuntimeConfiguration.Birthday;
            this.RetirementYear = Globals.Configuration.RuntimeConfiguration.RetirementWithYear;
            this.RefreshrateOfRandomRetirementStringInMinutes = (int)Math.Round(Globals.Configuration.RuntimeConfiguration.RefreshrateOfRandomRetirementString / 60d);
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
            this.WindowInstance.Close();
        }

        [RelayCommand]
        private static void BuyMeACoffee()
        {
            StaticCommands.BuyMeACoffee();
        }
        #endregion
    }
}
