using System;

namespace SI.Discord.Webhooks.Utilities
{
    /// <summary>
    /// Utility methods for working with colors.
    /// </summary>
    public static class ColorUtils
    {
        /// <summary>
        /// Converts a System.Drawing.Color object to an integer representation.
        /// </summary>
        /// <param name="color">The System.Drawing.Color object to convert.</param>
        /// <returns>An integer representation of the color.</returns>
        public static int ToInteger(this System.Drawing.Color color)
        {
            // Convert the color to an ARGB value
            int argb = color.ToArgb();

            // Convert the ARGB value to a hex string
            string x8 = argb.ToString("X8");

            // Extract the RGB hexcode
            string hexcode = x8.Substring(2, 6);

            // Convert the hexcode to an integer value
            return hexcode.HexToInteger();
        }

        /// <summary>
        /// Converts a hex color code (RRGGBB format) to an integer representation.
        /// </summary>
        /// <param name="hexColor">The hex color code to convert.</param>
        /// <returns>An integer representation of the color.</returns>
        /// <exception cref="ArgumentException">Thrown when the hex color code is not in the format RRGGBB.</exception>
        public static int HexToInteger(this string hexColor)
        {
            // Remove '#' if present at the start of the hex color code
            if (hexColor.StartsWith("#"))
            {
                hexColor = hexColor[1..];
            }

            // Ensure the hex color code is in the correct format (RRGGBB)
            if (hexColor.Length != 6)
            {
                throw new ArgumentException("Hex color must be in the format RRGGBB");
            }

            // Convert the individual R, G, and B components from hex to decimal
            int red = Convert.ToInt32(hexColor.Substring(0, 2), 16);
            int green = Convert.ToInt32(hexColor.Substring(2, 2), 16);
            int blue = Convert.ToInt32(hexColor.Substring(4, 2), 16);

            // Combine the components to get the final integer representation
            return red * 65536 + green * 256 + blue;
        }
    }
}
