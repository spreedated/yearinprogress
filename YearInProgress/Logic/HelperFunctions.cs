using Avalonia;
using Avalonia.Styling;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace YearInProgress.Logic
{
    internal static class HelperFunctions
    {
        internal readonly static Assembly assembly = typeof(HelperFunctions).Assembly;
        private static string[] motivationalLines = null;
        private static string changelogText = null;
        private static readonly Random rnd = new(BitConverter.ToInt32(Guid.NewGuid().ToByteArray()));

        public static string LoadRandomMotivationalRetirementText()
        {
            if (motivationalLines == null || motivationalLines.Length <= 0)
            {
                using (Stream s = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Resources.MotivationalRetirementTexts.txt"))
                {
                    using (StreamReader r = new(s))
                    {
                        motivationalLines = r.ReadToEnd().Split('\n');
                    }
                }
            }

            return motivationalLines[rnd.Next(motivationalLines.Length)].Replace("\\n", "\n");
        }

        public static string ReadEmbeddedChangelog()
        {
            if (string.IsNullOrEmpty(changelogText))
            {
                using (Stream s = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Resources.Changelog.txt"))
                {
                    using (StreamReader r = new(s))
                    {
                        changelogText = r.ReadToEnd();
                    }
                }
            }

            return changelogText;
        }

        public static void OpenWebsite(string url)
        {
            try
            {
                if (OperatingSystem.IsWindows())
                {
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (OperatingSystem.IsMacOS())
                {
                    Process.Start("open", url);
                }
                else if (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
                {
                    Process.Start("xdg-open", url);
                }
            }
            catch (Exception)
            {
                //noop
            }
        }

        public static void SetTheme()
        {
            if (Globals.Configuration.RuntimeConfiguration.DarkTheme)
            {
                Application.Current.RequestedThemeVariant = ThemeVariant.Dark;
                return;
            }

            Application.Current.RequestedThemeVariant = ThemeVariant.Light;
        }
    }
}
