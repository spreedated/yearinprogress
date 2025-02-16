using System;
using System.Diagnostics;
using YearInProgress.Logic;

namespace YearInProgress.ViewLogic
{
    public static class StaticCommands
    {
        public static void BuyMeACoffee()
        {
            string url = Constants.BUY_ME_A_COFFEE_URL;
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
    }
}
