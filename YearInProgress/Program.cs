using Avalonia;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using System;
using System.IO;
using YearInProgress.Logic;

namespace YearInProgress
{
    internal static class Program
    {
        private readonly static LogEventLevel _minimumLevel = LogEventLevel.Verbose;
        private static Microsoft.Extensions.Logging.ILogger _logger;

        [STAThread]
        public static void Main(string[] args)
        {
            SetupLogger();
            _logger.LogTrace("Logger established");

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
            Globals.Configuration.LoadAsync().Wait();
            _logger.LogTrace("Config loaded");

            _logger.LogTrace("Loading/building app...");
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                        .UsePlatformDetect()
                        .WithInterFont()
                        .LogToTrace();
        }

        private static void SetupLogger()
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.FromLogContext()
            .WriteTo.Debug(restrictedToMinimumLevel: _minimumLevel)
            .Enrich.WithProperty("Application", typeof(Program).Assembly.GetName().Name)
            .CreateLogger();

            _logger = new SerilogLoggerProvider().CreateLogger("Program");
        }
    }
}
