using System;
using UnityEngine;

namespace FluffyDuck.Util
{
    public static class StringDefine
    {
        public static string WithColorTag(this string message, Color c)
        {
            return message.WithColorTag(c.ToRGBHex());
        }

        public static string WithColorTag(this string message, string color_code)
        {
            return $"<color={color_code}>{message}</color>";
        }

        public static Color ToRGBFromHex(this string hex)
        {
            if (hex.StartsWith("#"))
            {
                hex = hex.Substring(1); // "#" 문자 제거
            }

            if (hex.Length != 6)
            {
                throw new ArgumentException("Hex color code must be 6 characters long.", nameof(hex));
            }

            Color c = Color.white;
            string r = hex.Substring(0, 2);
            string g = hex.Substring(2, 2);
            string b = hex.Substring(4, 2);
            c.r = (float)int.Parse(r, System.Globalization.NumberStyles.AllowHexSpecifier) / 255.0f;
            c.g = (float)int.Parse(g, System.Globalization.NumberStyles.AllowHexSpecifier) / 255.0f;
            c.b = (float)int.Parse(b, System.Globalization.NumberStyles.AllowHexSpecifier) / 255.0f;
            return c;
        }
    }
}
