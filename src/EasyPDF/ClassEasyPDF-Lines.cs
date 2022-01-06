using System;
using System.Drawing;
using iTextSharp.text;

namespace Shane32.EasyPDF
{
    public partial class PDFWriter
    {
        private float _currentX = 0f;
        private float _currentY = 0f;
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
            LineWidth = LineWidth;
        }

        /// <summary>
        /// Completes the current line being drawn.
        /// </summary>
        public void FinishLine()
        {
            if (_inLine) {
                try {
                    _content.Stroke();
                } finally {
                    _inLine = false;
                }
            }
        }

        /// <summary>
        /// Completes the current polygon being drawn.
        /// </summary>
        /// <param name="border">Draws a border around the polygon with the current <see cref="ForeColor"/>.</param>
        /// <param name="fill">Fills the polygon with the current <see cref="FillColor"/>.</param>
        /// <param name="eoFill">I can't remember what this does...</param>
        public void FinishPolygon(bool border, bool fill, bool eoFill = false)
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
        }

        /// <summary>
        /// Gets or sets the color used for printing text, lines and borders.
        /// </summary>
        public Color ForeColor {
            get => _foreColor;

            set {
                FinishLine();
                _foreColor = value;
                _content2?.SetColorStroke(_GetColor(value));
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
                _content2?.SetColorFill(new BaseColor(value.R, value.G, value.B, value.A));
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
                        throw new ArgumentOutOfRangeException();
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
                        throw new ArgumentOutOfRangeException();
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
                    throw new ArgumentNullException();
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
        public PointF Pos {
            get => new PointF(_TranslateRev(_currentX), _TranslateRev(_currentY));

            set {
                FinishLine();
                _currentX = _Translate(value.X);
                _currentY = _Translate(value.Y);
            }
        }

        /// <summary>
        /// Moves the current position to the specified coordinates.
        /// <paramref name="step"/> indiates that <paramref name="x"/> and <paramref name="y"/> are offsets from the current position.
        /// </summary>
        public void MoveTo(bool step, float x, float y)
        {
            FinishLine();
            if (step) {
                _currentX += _Translate(x);
                _currentY += _Translate(y);
            } else {
                _currentX = _Translate(x);

                _currentY = _Translate(y);
            }
        }

        /// <summary>
        /// Draws or continues a line from the current position to the specified coordinates.
        /// <paramref name="step"/> indiates that <paramref name="x"/> and <paramref name="y"/> are offsets from the current position.
        /// </summary>
        public void LineTo(bool step, float x, float y)
        {
            x = _Translate(x);
            y = _Translate(y);
            if (step) {
                x += _currentX;
                y += _currentY;
            }

            if (!_inLine)
                _content.MoveTo(_currentX, _currentY);
            _content.LineTo(x, y);
            _inLine = true;
            _currentX = x;
            _currentY = y;
        }

        /// <summary>
        /// Draws a new line from and to the specified coordinates.
        /// <paramref name="step1"/> indiates that <paramref name="x1"/> and <paramref name="y1"/> are offsets from the current position.
        /// <paramref name="step2"/> indiates that <paramref name="x2"/> and <paramref name="y2"/> are offsets from the start of the line.
        /// </summary>
        public void Line(bool step1, float x1, float y1, bool step2, float x2, float y2)
        {
            FinishLine();
            MoveTo(step1, x1, y1);
            LineTo(step2, x2, y2);
        }

        /// <summary>
        /// Draws a rectangle at the specified coordinates, and then draws another rectangle inset by the specified amount.
        /// <paramref name="step1"/> indiates that <paramref name="x1"/> and <paramref name="y1"/> are offsets from the current position.
        /// <paramref name="step2"/> indiates that <paramref name="x2"/> and <paramref name="y2"/> are offsets from the start of the line.
        /// </summary>
        public void RectangleDualOffset(bool step1, float x1, float y1, bool step2, float x2, float y2, float inset) //, float radius = 0f, bool Fill = false, bool Border = true)
        {
            Rectangle(step1, x1, y1, step2, x2, y2);
            Rectangle(step1, x1 + inset, y1 + inset, step2, x2 - inset * 2, y2 - inset * 2);
        }

        /// <summary>
        /// Draws a rectangle at the specified coordinates, optionally with rounded corners.
        /// </summary>
        /// <param name="step1">Indiates that <paramref name="x1"/> and <paramref name="y1"/> are offsets from the current position.</param>
        /// <param name="x1">The X coordinate or offset of the top-left corner of the rectangle.</param>
        /// <param name="y1">The Y coordinate or offset of the top-left corner of the rectangle.</param>
        /// <param name="step2">Indiates that <paramref name="x2"/> and <paramref name="y2"/> are offsets from the start of the line.</param>
        /// <param name="x2">The X coordinate or offset of the bottom-right corner of the rectangle.</param>
        /// <param name="y2">The Y coordinate or offset of the bottom-right corner of the rectangle.</param>
        /// <param name="borderRadius">The amount of radius to apply to the corners.</param>
        /// <param name="fill">Fills the rectange with the current <see cref="FillColor"/>.</param>
        /// <param name="border">Draws a border on the rectangle with the current <see cref="ForeColor"/>.</param>
        public void Rectangle(bool step1, float x1, float y1, bool step2, float x2, float y2, float borderRadius = 0f, bool fill = false, bool border = true)
        {
            FinishLine();
            x1 = _Translate(x1);
            y1 = _Translate(y1);
            if (step1) {
                x1 += _currentX;
                y1 += _currentY;
            }

            x2 = _Translate(x2);
            y2 = _Translate(y2);
            if (step2) {
                x2 += x1;
                y2 += y1;
            }

            _currentX = _TranslateRev(x2);
            _currentY = _TranslateRev(y2);
            if (!(fill | border))
                return;
            if (borderRadius == 0f) {
                _content.Rectangle(x1, y1, x2 - x1, y2 - y1);
            } else {
                _content.RoundRectangle(x1, y1, x2 - x1, y2 - y1, _Translate(borderRadius));
            }

            if (border) {
                if (fill)
                    _content.FillStroke();
                else
                    _content.Stroke();
            } else {
                _content.Fill();
            }
        }

        // Public Sub Arc(ByVal X1!, ByVal Y1!, ByVal X2!, ByVal Y2!, ByVal startAng!, ByVal extent!)
        // _Content.Arc(_Translate(X1), _Translate(Y1), _Translate(X2), _Translate(Y2), startAng, extent)
        // End Sub

        /// <summary>
        /// Draws or continues a line as a curve from the current position to the specified coordinates.
        /// </summary>
        /// <param name="step">Indicates that the <paramref name="x"/> and <paramref name="y"/> coordinates are offsets to the current position.</param>
        /// <param name="x">The X coordinate or offset.</param>
        /// <param name="y">The Y coordinate or offset.</param>
        /// <param name="fromSide">Controls if this is a inner or outer curve.</param>
        /// <param name="bulge">Controls the amount of curvature. The default value is for a 90 degree curve and equals <c>(float)(4d * (Math.Pow(2d, 0.5d) - 1d) / 3d)</c>.</param>
        public void CornerTo(bool step, float x, float y, bool fromSide, float bulge = 0.5522847498307936f) // 0.5522847498307936f = (float)(4d * (Math.Pow(2d, 0.5d) - 1d) / 3d);
            => CornerTo(step, x, y, fromSide, bulge, bulge);

        /// <summary>
        /// Draws or continues a line as a curve from the current position to the specified coordinates.
        /// </summary>
        /// <param name="step">Indicates that the <paramref name="x"/> and <paramref name="y"/> coordinates are offsets to the current position.</param>
        /// <param name="x">The X coordinate or offset.</param>
        /// <param name="y">The Y coordinate or offset.</param>
        /// <param name="fromSide">Controls if this is a inner or outer curve.</param>
        /// <param name="bulgeHorizontal">Controls the amount of curvature.</param>
        /// <param name="bulgeVertical">Controls the amount of curvature.</param>
        public void CornerTo(bool step, float x, float y, bool fromSide, float bulgeHorizontal, float bulgeVertical)
        {
            x = _Translate(x);
            y = _Translate(y);
            if (step) {
                x += _currentX;
                y += _currentY;
            }

            if (!_inLine)
                _content.MoveTo(_currentX, _currentY);
            if (fromSide) {
                _content.CurveTo(_currentX, (y - _currentY) * bulgeVertical + _currentY, x - (x - _currentX) * bulgeHorizontal, y, x, y);
            } else {
                _content.CurveTo((x - _currentX) * bulgeHorizontal + _currentX, _currentY, x, y - (y - _currentY) * bulgeVertical, x, y);
            }

            _inLine = true;
            _currentX = x;
            _currentY = y;
        }

        /// <summary>
        /// Draws a bezier curve starting at the current position to (<paramref name="x4"/>, <paramref name="y4"/>) with
        /// (<paramref name="x2"/>, <paramref name="y2"/>) and (<paramref name="x3"/>, <paramref name="y3"/>) as control points.
        /// </summary>
        public void BezierTo(float x2, float y2, float x3, float y3, float x4, float y4)
        {
            if (!_inLine)
                _content.MoveTo(_currentX, _currentY);
            _content.CurveTo(_Translate(x2), _Translate(y2), _Translate(x3), _Translate(y3), _Translate(x4), _Translate(y4));
            _inLine = true;
            _currentX = x4;
            _currentY = y4;
        }

        /// <summary>
        /// Draws a bezier curve starting at the current position to (<paramref name="x4"/>, <paramref name="y4"/>) with
        /// (<paramref name="x2"/>, <paramref name="y2"/>) as a control point.
        /// </summary>
        public void BezierTo(float x2, float y2, float x4, float y4)
        {
            if (!_inLine)
                _content.MoveTo(_currentX, _currentY);
            _content.CurveTo(_Translate(x2), _Translate(y2), _Translate(x4), _Translate(y4));
            _inLine = true;
            _currentX = x4;
            _currentY = y4;
        }

        /// <summary>
        /// Draws a circle centered at the specified position with a specified radius.
        /// </summary>
        /// <param name="step">Indicates that the <paramref name="x"/> and <paramref name="y"/> coordinates are offsets to the current position.</param>
        /// <param name="x">The X coordinate or offset.</param>
        /// <param name="y">The Y coordinate or offset.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="fill">Fills the circle with the current <see cref="FillColor"/>.</param>
        /// <param name="border">Draws a border on the circle with the current <see cref="ForeColor"/>.</param>
        public void Circle(bool step, float x, float y, float radius, bool border = true, bool fill = false)
        {
            FinishLine();
            if (!(border | fill))
                return;
            x = _Translate(x);
            y = _Translate(y);
            if (step) {
                x += _currentX;
                y += _currentY;
            }

            _currentX = x;
            _currentY = y;
            _content.Circle(x, y, _Translate(radius));
            if (border) {
                if (fill)
                    _content.FillStroke();
                else
                    _content.Stroke();
            } else {
                _content.Fill();
            }
        }
    }
}
