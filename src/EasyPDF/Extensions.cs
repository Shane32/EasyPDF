using System;
using iTextSharp.text;
using iTextFont = iTextSharp.text.Font;

namespace Shane32.EasyPDF
{
    internal static class Extensions
    {
        /// <summary>
        /// Returns a shallow copy of an array.
        /// </summary>
        public static T[] Duplicate<T>(this T[] array)
        {
            var newArray = new T[array.Length];
            array.CopyTo(newArray, 0);
            return newArray;
        }

        /// <summary>
        /// Converts a <see cref="System.Drawing.Color"/> to <see cref="iTextSharp.text.BaseColor"/>.
        /// </summary>
        public static BaseColor ToiTextSharpColor(this System.Drawing.Color color)
            => new BaseColor(color.R, color.G, color.B, color.A);

        /// <summary>
        /// Converts a <see cref="Font"/> to <see cref="iTextFont"/>.
        /// </summary>
        public static iTextFont ToiTextSharpFont(this Font font, System.Drawing.Color color)
        {
            if (font == null)
                throw new ArgumentNullException(nameof(font));

            int s = 0;
            if (font.Bold)
                s |= iTextFont.BOLD;
            if (font.Italic)
                s |= iTextFont.ITALIC;
            if (font.Underline)
                s |= iTextFont.UNDERLINE;
            if (font.Strikeout)
                s |= iTextFont.STRIKETHRU;
            
            //if font doesnt exist then throws error
            if (!FontFactory.IsRegistered(font.FamilyName)) {
                throw new InvalidOperationException($"'{font.FamilyName}' doesn't exist; please register font using RegisterFont.");
            }

            return FontFactory.GetFont(font.FamilyName, iTextSharp.text.pdf.BaseFont.IDENTITY_H, true, font.Size, s, color.ToiTextSharpColor());
        }
    }
}
