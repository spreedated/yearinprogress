using System;

namespace YearInProgress.Models
{
    internal sealed class Configuration
    {
        public DateTime Birthday { get; set; } = new DateTime(1987, 7, 1, 0, 0, 0, DateTimeKind.Local);
        public bool CompactView { get; set; }
        public bool DisplayClock { get; set; } = true;
        public bool DisplayDate { get; set; } = true;
        public bool DisplayRetirement { get; set; } = true;
        public int LocationX { get; set; }
        public int LocationY { get; set; }
        /// <summary>
        /// The refresh rate of the random retirement string in seconds
        /// </summary>
        public int RefreshrateOfRandomRetirementString { get; set; } = 600;
        public int RetirementWithYear { get; set; } = 67;
        public bool TopMost { get; set; } = true;
        public bool UseAdvancedProgress { get; set; } = true;
    }
}
