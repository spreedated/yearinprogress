using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace YearInProgress.Logic
{
    public static class AutoStartManager
    {
        private const string RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private static string AppPath => Process.GetCurrentProcess().MainModule?.FileName ?? "";

        public static void EnableAutoStart()
        {
            if (OperatingSystem.IsWindows())
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath, true))
                {
                    key?.SetValue(HelperFunctions.assembly.GetName().Name, $"\"{AppPath}\"");
                }
            }
        }

        public static void DisableAutoStart()
        {
            if (OperatingSystem.IsWindows())
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath, true))
                {
                    key?.DeleteValue(HelperFunctions.assembly.GetName().Name, false);
                }
            }
        }

        public static bool IsAutostartEnabled()
        {
            if (OperatingSystem.IsWindows())
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath, true))
                {
                    string val = key.GetValue(HelperFunctions.assembly.GetName().Name)?.ToString();

                    return val != null && val.Trim('"') == AppPath;
                }
            }

            return false;
        }

        public static void ToggleAutoStart()
        {
            if (IsAutostartEnabled())
            {
                DisableAutoStart();
                return;
            }

            EnableAutoStart();
        }
    }
}
