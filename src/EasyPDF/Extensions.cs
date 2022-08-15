using System;
using iTextSharp.text;
using iTextFont = iTextSharp.text.Font;
using BaseFont = iTextSharp.text.pdf.BaseFont;

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

            if (font.Embedded) {
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
            } else {
                BaseFont baseFont;
                int s = 0;
                if (font.Underline)
                    s |= iTextFont.UNDERLINE;
                if (font.Strikeout)
                    s |= iTextFont.STRIKETHRU;
                switch (font.FamilyName) {
                    case nameof(StandardFonts.Times) when !font.Bold && !font.Italic:
                        baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.WINANSI, false);
                        break;
                    case nameof(StandardFonts.Times) when font.Bold && !font.Italic:
                        baseFont = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.WINANSI, false);
                        break;
                    case nameof(StandardFonts.Times) when !font.Bold && font.Italic:
                        baseFont = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.WINANSI, false);
                        break;
                    case nameof(StandardFonts.Times) when font.Bold && font.Italic:
                        baseFont = BaseFont.CreateFont(BaseFont.TIMES_BOLDITALIC, BaseFont.WINANSI, false);
                        break;
                    case nameof(StandardFonts.Courier) when !font.Bold && !font.Italic:
                        baseFont = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.WINANSI, false);
                        break;
                    case nameof(StandardFonts.Courier) when font.Bold && !font.Italic:
                        baseFont = BaseFont.CreateFont(BaseFont.COURIER_BOLD, BaseFont.WINANSI, false);
                        break;
                    case nameof(StandardFonts.Courier) when !font.Bold && font.Italic:
                        baseFont = BaseFont.CreateFont(BaseFont.COURIER_OBLIQUE, BaseFont.WINANSI, false);
                        break;
                    case nameof(StandardFonts.Courier) when font.Bold && font.Italic:
                        baseFont = BaseFont.CreateFont(BaseFont.COURIER_BOLDOBLIQUE, BaseFont.WINANSI, false);
                        break;
                    case nameof(StandardFonts.Helvetica) when !font.Bold && !font.Italic:
                        baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, false);
                        break;
                    case nameof(StandardFonts.Helvetica) when font.Bold && !font.Italic:
                        baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.WINANSI, false);
                        break;
                    case nameof(StandardFonts.Helvetica) when !font.Bold && font.Italic:
                        baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_OBLIQUE, BaseFont.WINANSI, false);
                        break;
                    case nameof(StandardFonts.Helvetica) when font.Bold && font.Italic:
                        baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLDOBLIQUE, BaseFont.WINANSI, false);
                        break;
                    case nameof(StandardFonts.Symbol):
                        baseFont = BaseFont.CreateFont(BaseFont.SYMBOL, "Symbol", false);
                        if (font.Bold)
                            s |= iTextFont.BOLD;
                        if (font.Italic)
                            s |= iTextFont.ITALIC;
                        break;
                    case nameof(StandardFonts.ZapfDingbats):
                        baseFont = BaseFont.CreateFont(BaseFont.ZAPFDINGBATS, "ZapfDingbats", false);
                        if (font.Bold)
                            s |= iTextFont.BOLD;
                        if (font.Italic)
                            s |= iTextFont.ITALIC;
                        break;
                    default:
                        throw new InvalidOperationException("Invalid built-in font.");
                }
                return new iTextFont(baseFont, font.Size, s, color.ToiTextSharpColor());
            }
        }
    }
}
