using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ClausaComm
{
    public static class Config
    {
        public enum Property { LaunchAtStartup }
        private const string PropertyValueSeparator = ": ";
        
        private static readonly Dictionary<Property, string> PropertyName = new()
        {
            { Property.LaunchAtStartup, "launch at startup" },
        };

        public static string? GetValueOrNull(Property property)
        {
            string propName = PropertyName[property];
            
            return File.ReadAllLines(ProgramDirectory.ConfigPath)
                .Where(line => line.Contains(propName))
                .Select(line => line.Replace(propName + PropertyValueSeparator, ""))
                .FirstOrDefault();
        }
        
        public static int? GetIntOrNull(Property property) => int.TryParse(GetValueOrNull(property), out int value) ? value : null;
        public static long? GetLongOrNull(Property property) => long.TryParse(GetValueOrNull(property), out long value) ? value : null;
        public static bool? GetBoolOrNull(Property property) => bool.TryParse(GetValueOrNull(property), out bool value) ? value : null;
        public static float? GetFloatOrNull(Property property) => float.TryParse(GetValueOrNull(property), out float value) ? value : null;

        public static void SetValue(Property property, object value)
        {
            string propName = PropertyName[property];
            string configLine = propName + PropertyValueSeparator + value;
            string[] lines = File.ReadAllLines(ProgramDirectory.ConfigPath);
            
            bool set = false;
            for (int i = 0; i < lines.Length; ++i)
            {
                if (!lines[i].Contains(propName))
                    continue;
                
                lines[i] = configLine;
                set = true;
                break;
            }

            if (set)
                File.WriteAllLines(ProgramDirectory.ConfigPath, lines);
            else
                File.AppendAllText(ProgramDirectory.ConfigPath, configLine + "\n");
        }
    }
}