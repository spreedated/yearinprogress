using System;
using System.IO;
using System.Reflection;

namespace YearInProgress.Logic
{
    internal static class HelperFunctions
    {
        private readonly static Assembly assembly = typeof(HelperFunctions).Assembly;

        public static string LoadRandomMotivationalRetirementText()
        {
            string[] strings = null;

            using (Stream s = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Resources.MotivationalRetirementTexts.txt"))
            {
                using (StreamReader r = new(s))
                {
                    strings = r.ReadToEnd().Split('\n');
                }
            }

            Random rnd = new(BitConverter.ToInt32(Guid.NewGuid().ToByteArray()));
            return strings[rnd.Next(strings.Length)].Replace("\\n", "\n");
        }
    }
}
