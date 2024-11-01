namespace Shane32.EasyPDF;

/// <summary>
/// The line style to use when drawing lines or borders.
/// </summary>
public record LineStyle
{
    private LineCapStyle _lineCap;
    private LineJoinStyle _lineJoin;
    private LineDashStyle _lineDash;
    private float _lineWidth;

    /// <summary>
    /// Initializes a new instance with the specified parameters.
    /// </summary>
    /// <param name="width">The line width used when drawing lines or borders, specified in points.</param>
    /// <param name="capStyle">The <see cref="LineCapStyle"/> used when drawing lines.</param>
    /// <param name="joinStyle">The <see cref="LineJoinStyle"/> used when drawing lines or borders.</param>
    /// <param name="dashStyle">The <see cref="LineDashStyle"/> used when drawing lines or borders. Defaults to <see cref="LineDashStyle.Solid"/> when null.</param>
    public LineStyle(float width = 0.1f, LineCapStyle capStyle = LineCapStyle.None, LineJoinStyle joinStyle = LineJoinStyle.Miter, LineDashStyle? dashStyle = null)
    {
        _lineWidth = width;
        _lineCap = capStyle;
        _lineJoin = joinStyle;
        _lineDash = dashStyle ?? LineDashStyle.Solid;
    }

    /// <summary>
    /// Gets or sets the current <see cref="LineCapStyle"/> used when drawing lines.
    /// </summary>
    public LineCapStyle CapStyle {
        get => _lineCap;

        set {
            switch (value) {
                case LineCapStyle.None:
                case LineCapStyle.Round:
                case LineCapStyle.Square:
                    _lineCap = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
    }

    /// <summary>
    /// Gets or sets the current <see cref="LineJoinStyle"/> used when drawing lines or borders.
    /// </summary>
    public LineJoinStyle JoinStyle {
        get => _lineJoin;

        set {
            switch (value) {
                case LineJoinStyle.Bevel:
                case LineJoinStyle.Miter:
                case LineJoinStyle.Rounded:
                    _lineJoin = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
    }

    /// <summary>
    /// Gets or sets the current <see cref="LineDashStyle"/> used when drawing lines or borders.
    /// </summary>
    public LineDashStyle DashStyle {
        get => _lineDash;

        set {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            _lineDash = value;
        }
    }

    /// <summary>
    /// Gets or sets the current line width used when drawing lines or borders, specified in the scale of the document. Defaults to 0.1 points when null.
    /// </summary>
    public float Width {
        get => _lineWidth;
        set {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            _lineWidth = value;
        }
    }
}
