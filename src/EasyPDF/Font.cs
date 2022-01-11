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
        /// <param name="size">The font em-size of the font measured in points.</param>
        public Font(string familyName, float size)
        {
            FamilyName = familyName;
            Size = size;
        }

        /// <summary>
        /// Initializes a new instance with the specified variables.
        /// </summary>
        /// <param name="familyName">The family name of this font.</param>
        /// <param name="size">The font em-size of the font.</param>
        /// <param name="fontStyle">The style of the font measured in points.</param>
        public Font(string familyName, float size, FontStyle fontStyle)
            : this(familyName, size)
        {
            FontStyle = fontStyle;
        }
    }
}
