using System;
using System.Drawing;
using iTextSharp.text;

namespace Shane32.EasyPDF
{
    public partial class PDFWriter
    {
        /// <summary>
        /// The current X position measured in points.
        /// </summary>
        private float _currentX;
        /// <summary>
        /// The current Y position measured in points.
        /// </summary>
        private float _currentY;

        /// <summary>
        /// The current line width setting in points.
        /// </summary>
        private float _lineWidth = 0.1f;
        private LineCapStyle _lineCap = LineCapStyle.None;
        private LineJoinStyle _lineJoin = LineJoinStyle.Miter;
        private LineDashStyle _lineDash = LineDashStyle.Solid;
        private bool _inLine;
        private Color _foreColor = Color.Black;
        private Color _fillColor = Color.Black;

        /// <summary>
        /// When a document is initially created, this initializes <see cref="_content"/>
        /// with the proper settings to draw lines.
        /// </summary>
        private void _InitLineVars()
        {
            ForeColor = _foreColor;
            FillColor = _fillColor;
            LineCap = _lineCap;
            LineJoin = _lineJoin;
            LineDash = _lineDash;
#pragma warning disable CA2245 // Do not assign a property to itself
            LineWidth = LineWidth;
#pragma warning restore CA2245 // Do not assign a property to itself
        }

        /// <summary>
        /// Completes the current line being drawn.
        /// </summary>
        public PDFWriter FinishLine()
        {
            if (_inLine) {
                try {
                    _content.Stroke();
                } finally {
                    _inLine = false;
                }
            }

            return this;
        }

        /// <summary>
        /// Completes the current polygon being drawn.
        /// </summary>
        /// <param name="border">Draws a border around the polygon with the current <see cref="ForeColor"/>.</param>
        /// <param name="fill">Fills the polygon with the current <see cref="FillColor"/>.</param>
        /// <param name="eoFill">Uses the even-odd fill rule to fill polygons with overlapping boundaries.</param>
        public PDFWriter FinishPolygon(bool border, bool fill, bool eoFill = false)
        {
            if (_inLine) {
                try {
                    if (border) {
                        if (eoFill) {
                            _content.ClosePathEoFillStroke();
                        } else if (fill) {
                            _content.ClosePathFillStroke();
                        } else {
                            _content.ClosePathStroke();
                        }
                    } else if (eoFill) {
                        _content.EoFill();
                    } else if (fill) {
                        _content.Fill();
                    } else {
                        _content.NewPath();
                    }
                } finally {
                    _inLine = false;
                }
            }

            return this;
        }

        /// <summary>
        /// Gets or sets the color used for printing text, lines and borders.
        /// </summary>
        public Color ForeColor {
            get => _foreColor;

            set {
                FinishLine();
                _foreColor = value;
                _content2?.SetColorStroke(value.ToiTextSharpColor());
            }
        }

        /// <summary>
        /// Gets or sets the color used for filling polygons.
        /// </summary>
        public Color FillColor {
            get => _fillColor;

            set {
                FinishLine();
                _fillColor = value;
                _content2?.SetColorFill(value.ToiTextSharpColor());
            }
        }

        /// <summary>
        /// Gets or sets the current <see cref="LineCapStyle"/> used when drawing lines.
        /// </summary>
        public LineCapStyle LineCap {
            get => _lineCap;

            set {
                FinishLine();
                switch (value) {
                    case LineCapStyle.None:
                    case LineCapStyle.Round:
                    case LineCapStyle.Square:
                        _lineCap = value;
                        _content2?.SetLineCap((int)_lineCap);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value));
                }
            }
        }

        /// <summary>
        /// Gets or sets the current <see cref="LineJoinStyle"/> used when drawing lines or borders.
        /// </summary>
        public LineJoinStyle LineJoin {
            get => _lineJoin;

            set {
                FinishLine();
                switch (value) {
                    case LineJoinStyle.Bevel:
                    case LineJoinStyle.Miter:
                    case LineJoinStyle.Rounded:
                        _lineJoin = value;
                        _content2?.SetLineJoin((int)_lineJoin);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value));
                }
            }
        }

        /// <summary>
        /// Gets or sets the current <see cref="LineDashStyle"/> used when drawing lines or borders.
        /// </summary>
        public LineDashStyle LineDash {
            get => _lineDash;

            set {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
                FinishLine();
                _lineDash = value;
                _content2?.SetLineDash(_lineDash.MultipliedArray(_lineWidth), _lineDash.MultipliedPhase(_lineWidth));
            }
        }

        /// <summary>
        /// Gets or sets the current X coordinate of the page, with 0 representing the left edge of the page's margins.
        /// </summary>
        public float CurrentX {
            get => _TranslateRev(_currentX);

            set {
                FinishLine();
                _currentX = _Translate(value);
            }
        }

        /// <summary>
        /// Gets or sets the current Y coordinate of the page, with 0 representing the top edge of the page's margins.
        /// </summary>
        public float CurrentY {
            get => _TranslateRev(_currentY);

            set {
                FinishLine();
                _currentY = _Translate(value);
            }
        }

        /// <summary>
        /// Gets or sets the current line width used when drawing lines or borders.
        /// </summary>
        public float LineWidth {
            get => _TranslateRev(_lineWidth);

            set {
                FinishLine();
                _lineWidth = _Translate(value);
                _content2?.SetLineWidth(_lineWidth);
                _content2?.SetLineDash(_lineDash.MultipliedArray(_lineWidth), _lineDash.MultipliedPhase(_lineWidth));
            }
        }

        /// <summary>
        /// Gets or sets the current position as an offset from the page's margins.
        /// </summary>
        public PointF Position {
            get => new PointF(_TranslateRev(_currentX), _TranslateRev(_currentY));

            set {
                FinishLine();
                _currentX = _Translate(value.X);
                _currentY = _Translate(value.Y);
            }
        }

        /// <summary>
        /// Moves the current position to the specified coordinates relative to the current position.
        /// </summary>
        public PDFWriter OffsetTo(float offsetX, float offsetY)
        {
            FinishLine();
            _currentX += _Translate(offsetX);
            _currentY += _Translate(offsetY);
            return this;
        }

        /// <summary>
        /// Moves the current position to the specified coordinates.
        /// </summary>
        public PDFWriter MoveTo(float x, float y)
        {
            FinishLine();
            _currentX = _Translate(x);
            _currentY = _Translate(y);
            return this;
        }

        /// <summary>
        /// Draws or continues a line from the current position to the specified coordinates.
        /// </summary>
        public PDFWriter LineTo(float offsetX, float offsetY)
        {
            var x2 = _currentX + _Translate(offsetX);
            var y2 = _currentY + _Translate(offsetY);

            if (!_inLine)
                _content.MoveTo(_currentX, _currentY);
            _content.LineTo(x2, y2);
            _inLine = true;
            _currentX = x2;
            _currentY = y2;
            return this;
        }

        /// <summary>
        /// Draws a rectangle at the specified coordinates, and then draws another rectangle inset by the specified amount.
        /// </summary>
        public PDFWriter RectangleDualOffset(float width, float height, float inset, float borderRadius = 0f)
        {
            var currentX = _currentX;
            var currentY = _currentY;
            Rectangle(width, height, borderRadius);
            var insetPoints = _Translate(inset);
            _currentX = currentX + insetPoints;
            _currentY = currentY + insetPoints;
            Rectangle(width - inset * 2, height - inset * 2, borderRadius);
            _currentX += insetPoints;
            _currentY += insetPoints;
            return this;
        }

        /// <summary>
        /// Draws a rectangle at the specified coordinates, optionally with rounded corners.
        /// </summary>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="borderRadius">The amount of radius to apply to the corners.</param>
        /// <param name="fill">Fills the rectange with the current <see cref="FillColor"/>.</param>
        /// <param name="border">Draws a border on the rectangle with the current <see cref="ForeColor"/>.</param>
        public PDFWriter Rectangle(float width, float height, float borderRadius = 0f, bool fill = false, bool border = true)
        {
            FinishLine();

            var widthPoints = _Translate(width);
            var heightPoints = _Translate(height);

            if (!(fill || border))
                return this;
            if (borderRadius == 0f) {
                _content.Rectangle(_currentX, _currentY, widthPoints, heightPoints);
            } else {
                _content.RoundRectangle(_currentX, _currentY, widthPoints, heightPoints, _Translate(borderRadius));
            }

            if (border) {
                if (fill)
                    _content.FillStroke();
                else
                    _content.Stroke();
            } else {
                _content.Fill();
            }

            _currentX += widthPoints;
            _currentY += heightPoints;

            return this;
        }

        /// <summary>
        /// Draws or continues a line as a curve from the current position to the specified coordinates.
        /// </summary>
        /// <param name="offsetX">The X coordinate or offset.</param>
        /// <param name="offsetY">The Y coordinate or offset.</param>
        /// <param name="fromSide">Controls if this is a inner or outer curve.</param>
        /// <param name="bulge">Controls the amount of curvature. The default value is for a 90 degree curve and equals <c>(float)(4d * (Math.Pow(2d, 0.5d) - 1d) / 3d)</c>.</param>
        public PDFWriter CornerTo(float offsetX, float offsetY, bool fromSide, float bulge = 0.5522847498307936f) // 0.5522847498307936f = (float)(4d * (Math.Pow(2d, 0.5d) - 1d) / 3d);
            => CornerTo(offsetX, offsetY, fromSide, bulge, bulge);

        /// <summary>
        /// Draws or continues a line as a curve from the current position to the specified coordinates.
        /// </summary>
        /// <param name="offsetX">The X coordinate or offset.</param>
        /// <param name="offsetY">The Y coordinate or offset.</param>
        /// <param name="fromSide">Controls if this is a inner or outer curve.</param>
        /// <param name="bulgeHorizontal">Controls the amount of curvature.</param>
        /// <param name="bulgeVertical">Controls the amount of curvature.</param>
        public PDFWriter CornerTo(float offsetX, float offsetY, bool fromSide, float bulgeHorizontal, float bulgeVertical)
        {
            var x2 = _currentX + _Translate(offsetX);
            var y2 = _currentY + _Translate(offsetY);

            if (!_inLine)
                _content.MoveTo(_currentX, _currentY);
            if (fromSide) {
                _content.CurveTo(_currentX, (y2 - _currentY) * bulgeVertical + _currentY, x2 - (x2 - _currentX) * bulgeHorizontal, y2, x2, y2);
            } else {
                _content.CurveTo((x2 - _currentX) * bulgeHorizontal + _currentX, _currentY, x2, y2 - (y2 - _currentY) * bulgeVertical, x2, y2);
            }

            _inLine = true;
            _currentX = x2;
            _currentY = y2;
            return this;
        }

        /// <summary>
        /// Draws a bezier curve starting at the current position to (<paramref name="x4"/>, <paramref name="y4"/>) with
        /// (<paramref name="x2"/>, <paramref name="y2"/>) and (<paramref name="x3"/>, <paramref name="y3"/>) as control points.
        /// </summary>
        public PDFWriter BezierTo(float x2, float y2, float x3, float y3, float x4, float y4)
        {
            if (!_inLine)
                _content.MoveTo(_currentX, _currentY);
            _content.CurveTo(_Translate(x2), _Translate(y2), _Translate(x3), _Translate(y3), _Translate(x4), _Translate(y4));
            _inLine = true;
            _currentX = _Translate(x4);
            _currentY = _Translate(y4);
            return this;
        }

        /// <summary>
        /// Draws a bezier curve starting at the current position to (<paramref name="x4"/>, <paramref name="y4"/>) with
        /// (<paramref name="x2"/>, <paramref name="y2"/>) as a control point.
        /// </summary>
        public PDFWriter BezierTo(float x2, float y2, float x4, float y4)
        {
            if (!_inLine)
                _content.MoveTo(_currentX, _currentY);
            _content.CurveTo(_Translate(x2), _Translate(y2), _Translate(x4), _Translate(y4));
            _inLine = true;
            _currentX = _Translate(x4);
            _currentY = _Translate(y4);
            return this;
        }

        /// <summary>
        /// Draws a circle centered at the specified position with a specified radius.
        /// </summary>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="fill">Fills the circle with the current <see cref="FillColor"/>.</param>
        /// <param name="border">Draws a border on the circle with the current <see cref="ForeColor"/>.</param>
        public PDFWriter Circle(float radius, bool border = true, bool fill = false)
        {
            FinishLine();
            if (!(border || fill))
                return this;

            _content.Circle(_currentX, _currentY, _Translate(radius));
            if (border) {
                if (fill)
                    _content.FillStroke();
                else
                    _content.Stroke();
            } else {
                _content.Fill();
            }

            return this;
        }
    }
}
