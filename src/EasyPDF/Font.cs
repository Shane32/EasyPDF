using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Shane32.EasyPDF
{
    /// <summary>
    /// Represents a specific font family, size and style.
    /// </summary>
    public class Font
    {
        /// <summary>
        /// Gets the family name of this font.
        /// </summary>
        public string FamilyName { get; }

        /// <summary>
        /// Gets the em-size of the font measured in points.
        /// </summary>
        public float Size { get; }

        /// <summary>
        /// Gets the font style for this font.
        /// </summary>
        public FontStyle FontStyle { get; }

        /// <summary>
        /// Indicates if this font is embedded or a built-in font.
        /// </summary>
        public bool Embedded { get; }

        /// <summary>
        /// Indicates if the font has a bold style.
        /// </summary>
        public bool Bold => FontStyle.HasFlag(FontStyle.Bold);

        /// <summary>
        /// Indicates if the font has a italic style.
        /// </summary>
        public bool Italic => FontStyle.HasFlag(FontStyle.Italic);

        /// <summary>
        /// Indicates if the font has a underline style.
        /// </summary>
        public bool Underline => FontStyle.HasFlag(FontStyle.Underline);

        /// <summary>
        /// Indicates if the font has a strikeout style.
        /// </summary>
        public bool Strikeout => FontStyle.HasFlag(FontStyle.Strikeout);

        /// <summary>
        /// Initializes a new instance with the specified variables.
        /// </summary>
        /// <param name="familyName">The family name of this font.</param>
        /// <param name="size">The font em-size of the font.</param>
        /// <param name="fontStyle">The style of the font measured in points.</param>
        public Font(string familyName, float size, FontStyle fontStyle = FontStyle.Regular)
        {
            FamilyName = familyName;
            Size = size;
            FontStyle = fontStyle;
            Embedded = true;
        }

        /// <summary>
        /// Initializes a new instance with the specified variables.
        /// </summary>
        /// <param name="family">The family name of this font.</param>
        /// <param name="size">The font em-size of the font.</param>
        /// <param name="fontStyle">The style of the font measured in points.</param>
        public Font(StandardFonts family, float size, FontStyle fontStyle = FontStyle.Regular)
        {
            switch (family) {
                case StandardFonts.Times:
                    FamilyName = "Times";
                    break;
                case StandardFonts.Helvetica:
                    FamilyName = "Helvetica";
                    break;
                case StandardFonts.Courier:
                    FamilyName = "Courier";
                    break;
                case StandardFonts.Symbol:
                    if (fontStyle.HasFlag(FontStyle.Bold) || fontStyle.HasFlag(FontStyle.Italic))
                        throw new ArgumentOutOfRangeException(nameof(fontStyle), "Cannot specify bold or italic for the built-in Symbol font.");
                    FamilyName = "Symbol";
                    break;
                case StandardFonts.ZapfDingbats:
                    if (fontStyle.HasFlag(FontStyle.Bold) || fontStyle.HasFlag(FontStyle.Italic))
                        throw new ArgumentOutOfRangeException(nameof(fontStyle), "Cannot specify bold or italic for the built-in ZapfDingbats font.");
                    FamilyName = "ZapfDingbats";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(family));
            }
            Size = size;
            FontStyle = fontStyle;
            Embedded = false;
        }
    }

    /// <summary>
    /// Specifies constants that define the built-in PDF font to be used.
    /// </summary>
    public enum StandardFonts
    {
        /// <summary>
        /// The Times font, which has bold, italic and bold-italic variations.
        /// </summary>
        Times = 1,

        /// <summary>
        /// The Helvetica font, which has bold, italic and bold-italic variations.
        /// </summary>
        Helvetica = 2,

        /// <summary>
        /// The Courier font, which has bold, italic and bold-italic variations.
        /// </summary>
        Courier = 3,

        /// <summary>
        /// The Symbol font.
        /// </summary>
        Symbol = 4,

        /// <summary>
        /// The ZapfDingbats font.
        /// </summary>
        ZapfDingbats = 5,
    }
}
