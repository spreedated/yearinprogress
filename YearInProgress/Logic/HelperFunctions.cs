﻿using System;
using System.IO;
using System.Reflection;

namespace YearInProgress.Logic
{
    internal static class HelperFunctions
    {
        private readonly static Assembly assembly = typeof(HelperFunctions).Assembly;
        private static string[] motivationalLines = null;
        private static string changelogText = null;

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

            Random rnd = new(BitConverter.ToInt32(Guid.NewGuid().ToByteArray()));
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
    }
}
