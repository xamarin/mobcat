using System;
using Foundation;
namespace Microsoft.MobCAT.iOS.Extensions
{
    public static class NSUserDefaultsExtensions
    {
        public static bool BoolForKey(this NSUserDefaults defaults, bool defaultValue, string key)
        {
            if (defaults.ValueForKey(new NSString(key)) == null)
                return defaultValue;
            return defaults.BoolForKey(key);
        }

        public static int IntForKey(this NSUserDefaults defaults, int defaultValue, string key)
        {
            if (defaults.ValueForKey(new NSString(key)) == null)
                return defaultValue;
            return (int)defaults.IntForKey(key);
        }

        public static double DoubleForKey(this NSUserDefaults defaults, double defaultValue, string key)
        {
            if (defaults.ValueForKey(new NSString(key)) == null)
                return defaultValue;
            return defaults.DoubleForKey(key);
        }

        public static float FloatForKey(this NSUserDefaults defaults, float defaultValue, string key)
        {
            if (defaults.ValueForKey(new NSString(key)) == null)
                return defaultValue;
            return defaults.FloatForKey(key);
        }
    }
}
