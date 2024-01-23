using UnityEngine;

namespace FluffyDuck.Util
{
    public static class IntDefine
    {
        public static string ToPercentage(this int value)
        {
            return $"{value}%";
        }

        public static float ToPercentageFloat(this int value)
        {
            return (float)value / 100;
        }
    }
}
