using System;

namespace SI.Discord.Webhooks
{
    public static class ColorExtensions
    {
        public static int ToInteger(this System.Drawing.Color color)
        {
            int argb = color.ToArgb();
            string x8 = argb.ToString("X8");
            string hexcode = x8.Substring(2, 6);
            return hexcode.HexToInteger();
        }

        public static int HexToInteger(this string hexColor)
        {
            if (hexColor.StartsWith("#"))
            {
                hexColor = hexColor.Substring(1);
            }

            if (hexColor.Length != 6)
            {
                throw new ArgumentException("Hex color must be in the format RRGGBB");
            }

            int red = Convert.ToInt32(hexColor.Substring(0, 2), 16);
            int green = Convert.ToInt32(hexColor.Substring(2, 2), 16);
            int blue = Convert.ToInt32(hexColor.Substring(4, 2), 16);

            return red * 65536 + green * 256 + blue;
        }
    }
}