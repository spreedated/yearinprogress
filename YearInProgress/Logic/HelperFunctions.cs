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
        internal readonly static Assembly _assembly = typeof(HelperFunctions).Assembly;
        private static string[] _motivationalLines = null;
        private static string _changelogText = null;
        private static readonly Random _rnd = new(BitConverter.ToInt32(Guid.NewGuid().ToByteArray()));

        public static string LoadRandomMotivationalRetirementText()
        {
            if (_motivationalLines == null || _motivationalLines.Length <= 0)
            {
                using (Stream s = _assembly.GetManifestResourceStream($"{_assembly.GetName().Name}.Resources.MotivationalRetirementTexts.txt"))
                {
                    using (StreamReader r = new(s))
                    {
                        _motivationalLines = r.ReadToEnd().Split('\n');
                    }
                }
            }

            return _motivationalLines[_rnd.Next(_motivationalLines.Length)].Replace("\\n", "\n");
        }

        public static string ReadEmbeddedChangelog()
        {
            if (string.IsNullOrEmpty(_changelogText))
            {
                using (Stream s = _assembly.GetManifestResourceStream($"{_assembly.GetName().Name}.Resources.Changelog.txt"))
                {
                    using (StreamReader r = new(s))
                    {
                        _changelogText = r.ReadToEnd();
                    }
                }
            }

            return _changelogText;
        }

        public static void OpenWebsite(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                url = "https://" + url.Trim();
            }

            try
            {
                ProcessStartInfo psi = new()
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception)
            {
                return;
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
