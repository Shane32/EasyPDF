using System.Drawing;

namespace Shane32.EasyPDF;

/// <summary>
/// Represents a specific font family, size and style.
/// </summary>
public record Font
{
    private string _familyName = null!;

    private static readonly IEnumerable<string> _builtInFonts = new[] { nameof(StandardFonts.Times), nameof(StandardFonts.Helvetica), nameof(StandardFonts.Courier), nameof(StandardFonts.Symbol), nameof(StandardFonts.ZapfDingbats) };

    /// <summary>
    /// Gets the family name of this font.
    /// </summary>
    public string FamilyName {
        get => _familyName;
        set {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            var builtInFont = _builtInFonts.FirstOrDefault(x => string.Equals(x, value, StringComparison.OrdinalIgnoreCase));
            if (builtInFont != null) {
                _familyName = builtInFont;
                Embedded = false;
            } else {
                _familyName = value;
                Embedded = true;
            }
        }
    }

    /// <summary>
    /// Indicates if this font is embedded or a built-in font.
    /// </summary>
    public bool Embedded { get; private set; }

    /// <summary>
    /// Gets the em-size of the font measured in points.
    /// </summary>
    public float Size { get; set; }

    /// <summary>
    /// Gets the font style for this font.
    /// </summary>
    public FontStyle FontStyle {
        get {
            FontStyle f = 0;
            if (Bold)
                f |= FontStyle.Bold;
            if (Italic)
                f |= FontStyle.Italic;
            if (Underline)
                f |= FontStyle.Underline;
            if (Strikeout)
                f |= FontStyle.Strikeout;
            return f;
        }
        set {
            Bold = (value & FontStyle.Bold) == FontStyle.Bold;
            Italic = (value & FontStyle.Italic) == FontStyle.Italic;
            Underline = (value & FontStyle.Underline) == FontStyle.Underline;
            Strikeout = (value & FontStyle.Strikeout) == FontStyle.Strikeout;
        }
    }

    /// <summary>
    /// Indicates if the font has a bold style.
    /// </summary>
    public bool Bold { get; set; }

    /// <summary>
    /// Indicates if the font has a italic style.
    /// </summary>
    public bool Italic { get; set; }

    /// <summary>
    /// Indicates if the font has a underline style.
    /// </summary>
    public bool Underline { get; set; }

    /// <summary>
    /// Indicates if the font has a strikeout style.
    /// </summary>
    public bool Strikeout { get; set; }

    /// <summary>
    /// Indicates if word-wrapped text is justified.
    /// </summary>
    public bool Justify { get; set; }

    /// <summary>
    /// The line spacing multiple to use.
    /// Line height calculations including <see cref="PDFWriter.TextHeight"/> and
    /// <see cref="PDFWriter.TextLeading"/> take this value into account.
    /// </summary>
    public float LineSpacing { get; set; } = 1f;

    /// <summary>
    /// The amount of indentation for word-wrapped lines measured in the document's
    /// current <see cref="PDFWriter.ScaleMode">ScaleMode</see> setting.
    /// </summary>
    public float HangingIndent { get; set; }

    /// <summary>
    /// The additional amount of spacing after a carriage return (but not after
    /// word-wrapped text) measured in points.
    /// </summary>
    public float ParagraphSpacing { get; set; }

    /// <summary>
    /// A multiplier that stretches the text along the X axis.
    /// </summary>
    public float StretchX { get; set; } = 1f;

    /// <summary>
    /// A multipler that stretches the height of the text.
    /// </summary>
    public float StretchY { get; set; } = 1f;

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
    }

    /// <summary>
    /// Initializes a new instance with the specified variables.
    /// </summary>
    /// <param name="family">The family name of this font.</param>
    /// <param name="size">The font em-size of the font.</param>
    /// <param name="fontStyle">The style of the font measured in points.</param>
    public Font(StandardFonts family, float size, FontStyle fontStyle = FontStyle.Regular)
        : this(family.ToString(), size, fontStyle)
    {
    }
}
