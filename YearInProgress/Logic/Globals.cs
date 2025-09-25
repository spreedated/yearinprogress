using neXn.Lib.ConfigurationHandler;
using YearInProgress.Models;

namespace YearInProgress.Logic
{
    internal static class Globals
    {
        public static string AppLocalBaseUserPath { get; set; }
        public static ConfigurationHandler<Configuration> Configuration { get; set; }
    }
}
