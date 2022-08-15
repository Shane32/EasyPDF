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

        private static readonly string _zapfDingbatsTranslationFrom = "✁✂✃✄☎✆✇✈✉☛☞✌✍✎✏✐✑✒✓✔✕✖✗✘✙✚✛✜✝✞✟✠✡✢✣✤✥✦✧★✩✪✫✬✭✮✯✰✱✲✳✴✵✶✷✸✹✺✻✼✽✾✿❀❁❂❃❄❅❆❇❈❉❊❋●❍■❏❐❑❒▲▼◆❖◗❘❙❚❛❜❝❞❨❩❪❫❬❭❮❯❰❱❲❳❴❵";
        private static readonly string _zapfDingbatsTranslationTo = "\u0021\u0022\u0023\u0024\u0025\u0026\u0027\u0028\u0029\u002a\u002b\u002c\u002d\u002e\u002f\u0030\u0031\u0032\u0033\u0034\u0035\u0036\u0037\u0038\u0039\u003a\u003b\u003c\u003d\u003e\u003f\u0040\u0041\u0042\u0043\u0044\u0045\u0046\u0047\u0048\u0049\u004a\u004b\u004c\u004d\u004e\u004f\u0050\u0051\u0052\u0053\u0054\u0055\u0056\u0057\u0058\u0059\u005a\u005b\u005c\u005d\u005e\u005f\u0060\u0061\u0062\u0063\u0064\u0065\u0066\u0067\u0068\u0069\u006a\u006b\u006c\u006d\u006e\u006f\u0070\u0071\u0072\u0073\u0074\u0075\u0076\u0077\u0078\u0079\u007a\u007b\u007c\u007d\u007e\u0080\u0081\u0082\u0083\u0084\u0085\u0086\u0087\u0088\u0089\u008a\u008b\u008c\u008d";
        public static string Translate(this string text, Font font)
        {
            return text;
            if (font.Embedded == false && font.FamilyName == nameof(StandardFonts.ZapfDingbats)) {
                var text2 = text.ToCharArray();
                for (var i = 0; i < text2.Length; i++) {
                    var j = _zapfDingbatsTranslationFrom.IndexOf(text2[i]);
                    if (j >= 0)
                        text2[i] = _zapfDingbatsTranslationTo[j];
                }
                return new string(text2);
            }
            return text;
        }
    }
}
