using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace YearInProgress.Logic
{
    public static class AutoStartManager
    {
        private const string REGISTRY_PATH = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private static string AppPath
        {
            get
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    return p.MainModule?.FileName ?? string.Empty;
                }
            }
        }

        public static void EnableAutoStart()
        {
            if (OperatingSystem.IsWindows())
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true))
                {
                    key?.SetValue(HelperFunctions._assembly.GetName().Name, $"\"{AppPath}\"");
                }
            }
        }

        public static void DisableAutoStart()
        {
            if (OperatingSystem.IsWindows())
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true))
                {
                    key?.DeleteValue(HelperFunctions._assembly.GetName().Name, false);
                }
            }
        }

        public static bool IsAutostartEnabled()
        {
            if (OperatingSystem.IsWindows())
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true))
                {
                    string val = key.GetValue(HelperFunctions._assembly.GetName().Name)?.ToString();

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
