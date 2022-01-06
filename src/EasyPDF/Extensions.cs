using System;
using iTextSharp.text;

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
        /// Converts a <see cref="System.Drawing.Font"/> to <see cref="Font"/>.
        /// </summary>
        public static Font ToiTextSharpFont(this System.Drawing.Font font, System.Drawing.Color color)
        {
            if (font == null)
                throw new ArgumentNullException(nameof(font));

            Font f;
            int s = 0;
            if (font.Bold)
                s |= Font.BOLD;
            if (font.Italic)
                s |= Font.ITALIC;
            if (font.Underline)
                s |= Font.UNDERLINE;
            if (font.Strikeout)
                s |=Font.STRIKETHRU;

            //if font doesnt exist then throws error
            if (!FontFactory.IsRegistered(font.OriginalFontName)) {
                throw new InvalidOperationException($"'{font.OriginalFontName}' doesn't exist; please register font using RegisterFont.");
            }

            f = FontFactory.GetFont(font.OriginalFontName, iTextSharp.text.pdf.BaseFont.CP1252, true, font.SizeInPoints, s, color.ToiTextSharpColor());
            return f;
        }
    }
}
