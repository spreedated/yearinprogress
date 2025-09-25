using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using System;
using System.IO;
using YearInProgress.Logic;
using YearInProgress.Views;

namespace YearInProgress
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (OperatingSystem.IsWindows())
            {
                Globals.AppLocalBaseUserPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "neXn-Systems", "YearInProgress");
            }
            else
            {
                Globals.AppLocalBaseUserPath = AppContext.BaseDirectory;
            }

            Globals.Configuration = new(new(Path.Combine(Globals.AppLocalBaseUserPath, "config.json"))
            {
                Autoload = false
            });
            Globals.Configuration.Load();

            if (base.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                BindingPlugins.DataValidators.RemoveAt(0);

                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}