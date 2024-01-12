using UnityEngine;

namespace FluffyDuck.Util
{
    public static class FloatDefine
    {
        public static string ToPercentage(this float value)
        {
            return $"{value * 100}%";
        }

        public static byte ToByte(this float f)
        {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }
    }
}
