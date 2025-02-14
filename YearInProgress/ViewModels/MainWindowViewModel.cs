using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using YearInProgress.Logic;
using YearInProgress.ViewElements;
using YearInProgress.Views;

namespace YearInProgress.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private Grid workingGrid = null;
        private readonly ProgressBoxAdvanced[,] progressBoxesAdvanced = new ProgressBoxAdvanced[10, 10];
        private readonly DispatcherTimer reinitTimer = new();
        private bool isFirstRun = true;
        private int lastRunDay = 0;
        private int refreshRetirementStringInSecondsLeft;
        private string currentRetirementString = null;

        #region BindableProperties
        [ObservableProperty]
        private Window windowInstance = null;

        [ObservableProperty]
        private string versionInfo = null;

        [ObservableProperty]
        private string text = "1987 is 100% complete";

        [ObservableProperty]
        private string currentDate = "July, 1 1987";

        [ObservableProperty]
        private string currentTimeString = "07:56:24";

        [ObservableProperty]
        private string retirementString = "88 days, 50 min, 24 sec";

        [ObservableProperty]
        private bool displayClock = Globals.Configuration.RuntimeConfiguration.DisplayClock;
        partial void OnDisplayClockChanged(bool value)
        {
            this.SaveSettingsToConfig();
        }

        [ObservableProperty]
        private bool displayDate = Globals.Configuration.RuntimeConfiguration.DisplayDate;
        partial void OnDisplayDateChanged(bool value)
        {
            this.SaveSettingsToConfig();
        }

        [ObservableProperty]
        private bool displayRetirement = Globals.Configuration.RuntimeConfiguration.DisplayRetirement;
        partial void OnDisplayRetirementChanged(bool value)
        {
            this.SaveSettingsToConfig();
        }

        [ObservableProperty]
        private bool compactView = Globals.Configuration.RuntimeConfiguration.CompactView;
        partial void OnCompactViewChanged(bool value)
        {
            this.SaveSettingsToConfig();
            this.ReInitialize();
        }

        [ObservableProperty]
        private DateTime setBirthday;

        [ObservableProperty]
        private bool topMost = Globals.Configuration.RuntimeConfiguration.TopMost;
        partial void OnTopMostChanged(bool value)
        {
            this.SaveSettingsToConfig();
        }
        #endregion

        #region Ctor
        public MainWindowViewModel()
        {
            this.currentRetirementString = HelperFunctions.LoadRandomMotivationalRetirementText();
            this.refreshRetirementStringInSecondsLeft = Globals.Configuration.RuntimeConfiguration.RefreshrateOfRandomRetirementString;
            this.versionInfo = $"Version: {typeof(MainWindowViewModel).Assembly.GetName().Version}";
            this.lastRunDay = DateTime.Now.Day;
            this.setBirthday = Globals.Configuration.RuntimeConfiguration.Birthday;

            TimeSpan timeUntilNextSecond = TimeSpan.FromSeconds(1) - TimeSpan.FromMilliseconds(DateTime.Now.Millisecond);
            this.reinitTimer.Interval = timeUntilNextSecond;
            this.reinitTimer.Start();

            this.reinitTimer.Tick += this.TimerElapsed;
        }
        #endregion

        #region Commands
        [RelayCommand]
        private void CheckBoxClick(MenuItem menuItem)
        {
            if (menuItem == null || string.IsNullOrEmpty(menuItem.Tag.ToString()))
            {
                return;
            }

            PropertyInfo prop = typeof(MainWindowViewModel).GetProperties().FirstOrDefault(x => x.Name == menuItem.Tag.ToString());
            prop?.SetValue(this, !(bool)prop.GetValue(this));
        }

        [RelayCommand]
        private static void CloseApplication()
        {
            Globals.Configuration.Save();
            Environment.Exit(0);
        }

        [RelayCommand]
        private void OpenOptions()
        {
            Options o = new();
            o.ShowDialog(this.WindowInstance);
        }

        [RelayCommand]
        private void GetNewMotivation()
        {
            this.refreshRetirementStringInSecondsLeft = 0;
        }
        #endregion

        private void TimerElapsed(object sender, EventArgs e)
        {
            if (this.isFirstRun)
            {
                this.reinitTimer.Interval = TimeSpan.FromSeconds(1);
            }

            if (this.DisplayClock)
            {
                this.CurrentTimeString = DateTime.Now.ToString("HH:mm:ss");
            }

            if (this.lastRunDay != DateTime.Now.Day)
            {
                this.ReInitialize();
                this.lastRunDay = DateTime.Now.Day;
            }

            if (this.DisplayRetirement && Globals.Configuration.RuntimeConfiguration.Birthday != default)
            {
                TimeSpan substracted = Globals.Configuration.RuntimeConfiguration.Birthday.AddYears(Globals.Configuration.RuntimeConfiguration.RetirementWithYear) - Globals.Configuration.RuntimeConfiguration.Birthday;
#pragma warning disable S6561
                TimeSpan calculated = substracted - (DateTime.Now - Globals.Configuration.RuntimeConfiguration.Birthday);
#pragma warning restore S6561

                this.DisplayRandomRetirementString(calculated);
            }

            if (this.isFirstRun)
            {
                this.isFirstRun = false;
            }
        }

        private void DisplayRandomRetirementString(TimeSpan time)
        {
            this.refreshRetirementStringInSecondsLeft--;

            this.RetirementString = this.currentRetirementString.Replace("{Placeholder}", $"{(int)time.TotalDays:N0} days,\n{time.Hours} hrs, {time.Minutes} mins, {time.Seconds} sec").Trim('\n');

            if (this.refreshRetirementStringInSecondsLeft <= 0)
            {
                this.refreshRetirementStringInSecondsLeft = Globals.Configuration.RuntimeConfiguration.RefreshrateOfRandomRetirementString;
                this.currentRetirementString = HelperFunctions.LoadRandomMotivationalRetirementText();
            }
        }

        private void SaveSettingsToConfig()
        {
            Globals.Configuration.RuntimeConfiguration.DisplayClock = this.DisplayClock;
            Globals.Configuration.RuntimeConfiguration.DisplayDate = this.DisplayDate;
            Globals.Configuration.RuntimeConfiguration.DisplayRetirement = this.DisplayRetirement;
            Globals.Configuration.RuntimeConfiguration.Birthday = this.SetBirthday;
            Globals.Configuration.RuntimeConfiguration.CompactView = this.CompactView;
            Globals.Configuration.RuntimeConfiguration.TopMost = this.TopMost;

            Globals.Configuration.Save();
        }

        private void CreateGridAdvanced()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int ii = 0; ii < 10; ii++)
                {
                    ProgressBoxAdvanced progressBox = new()
                    {
                        Margin = new(0, 0, 0, 4),
                        [Grid.RowProperty] = i,
                        [Grid.ColumnProperty] = ii,
                        Opacity = 0.8
                    };

                    this.progressBoxesAdvanced[i, ii] = progressBox;
                }
            }
        }

        /// <summary>
        /// Initializes and calculate
        /// </summary>
        public void Initialize(Grid grid)
        {
            this.workingGrid ??= grid;
            this.DisplayClock = Globals.Configuration.RuntimeConfiguration.DisplayClock;
            this.DisplayDate = Globals.Configuration.RuntimeConfiguration.DisplayDate;
            this.DisplayRetirement = Globals.Configuration.RuntimeConfiguration.DisplayRetirement;

            this.CreateGridAdvanced();

            int daycountOftheYear = DateTime.IsLeapYear(DateTime.Now.Year) ? 366 : 365;
#pragma warning disable S6561
            int daysPassed = (DateTime.Now - new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0, DateTimeKind.Local)).Days;
#pragma warning restore S6561
            float percentagePassed = (daysPassed * 100f) / daycountOftheYear;

            this.Text = $"{DateTime.Now.Year} is {Math.Round(percentagePassed, 2)}% complete";
            this.CurrentDate = DateTime.Now.ToString("MMMM dd, yyyy", CultureInfo.InvariantCulture);

            float markedDays = percentagePassed;

            int currentIteration = 0;
            int currentRow = 0;

            foreach (ProgressBoxAdvanced pb in this.progressBoxesAdvanced)
            {
                if (markedDays >= 1f)
                {
                    pb.Value = 100d;
                }
                if (markedDays < 1 && markedDays > 0f)
                {
                    pb.Value = markedDays * 100d;
                }
                if (markedDays > 0f)
                {
                    markedDays--;
                }

                if (Globals.Configuration.RuntimeConfiguration.CompactView && pb.Value <= 0 && this.progressBoxesAdvanced[currentRow, 0].Value <= 0)
                {
                    continue;
                }

                grid.Children.Add(pb);

                currentIteration++;

                if (currentIteration % 10 == 0)
                {
                    currentRow++;
                }
            }
        }

        public void ReInitialize()
        {
            this.workingGrid.Children.Clear();
            this.Initialize(this.workingGrid);
        }
    }
}
