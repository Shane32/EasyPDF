using System.Drawing;
using Font = Shane32.EasyPDF.Font;

namespace Tests.Lines;

public class BasicLineTests
{
    private readonly PDFWriter _writer = new PDFWriter();

    public BasicLineTests()
    {
        _writer.ScaleMode = ScaleModes.Inches;
        _writer.NewPage(PageKind.Letter, false, 1f, 1f);
        _writer.PrepForTests();
    }

    [Fact]
    public void FillStyles()
    {
        _writer.FillColor.ShouldBe(Color.Black);

        DrawGeometricFigure(0.5f, false, false, false, false, 0.1f, LineJoinStyle.Miter, 0, LineCapStyle.None);

        _writer.MoveTo(3.75f, 0);
        DrawGeometricFigure(0.5f, true, true, false, false, 0.1f, LineJoinStyle.Miter, 0, LineCapStyle.None);

        _writer.MoveTo(0, 2);
        DrawGeometricFigure(0.5f, true, false, true, false, 0.1f, LineJoinStyle.Miter, 0, LineCapStyle.None);

        _writer.MoveTo(3.75f, 2);
        DrawGeometricFigure(0.5f, true, false, true, true, 0.1f, LineJoinStyle.Miter, 0, LineCapStyle.None);

        _writer.MoveTo(0, 4);
        DrawGeometricFigure(0.5f, true, true, true, true, 0.1f, LineJoinStyle.Miter, 0, LineCapStyle.None);

        _writer.MoveTo(3.75f, 4);
        DrawGeometricFigure(0.5f, true, true, true, false, 0.1f, LineJoinStyle.Miter, 0, LineCapStyle.None);

        _writer.MoveTo(0, 6);
        DrawGeometricFigure(0.5f, true, false, false, false, 0.1f, LineJoinStyle.Miter, 0, LineCapStyle.None);

        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }


    [Fact]
    public void LineStyles()
    {
        _writer.ForeColor.ShouldBe(Color.Black);
        _writer.LineStyle.JoinStyle.ShouldBe(LineJoinStyle.Miter);
        _writer.LineStyle.CapStyle.ShouldBe(LineCapStyle.None);
        _writer.LineStyle.DashStyle.ShouldBe(LineDashStyle.Solid);
        _writer.LineStyle.Width.ShouldBe(0.1f, 0.001f);

        _writer.MoveTo(0, 0f);
        DrawGeometricFigure(0.5f, false, true, false, false, 0.1f, LineJoinStyle.Miter, 0, LineCapStyle.None);
        _writer.MoveTo(3.75f, 0f);
        DrawGeometricFigure(0.5f, true, true, false, false, 0.1f, LineJoinStyle.Miter, 0, LineCapStyle.None);

        _writer.MoveTo(0, 2f);
        DrawGeometricFigure(0.5f, false, true, false, false, 0.0625f * 72, LineJoinStyle.Miter, 0, LineCapStyle.None);
        _writer.MoveTo(3.75f, 2f);
        DrawGeometricFigure(0.5f, true, true, false, false, 0.0625f * 72, LineJoinStyle.Miter, 0, LineCapStyle.None);

        _writer.MoveTo(0, 4f);
        DrawGeometricFigure(0.5f, false, true, false, false, 0.0625f * 72, LineJoinStyle.Rounded, 0, LineCapStyle.Round);
        _writer.MoveTo(3.75f, 4f);
        DrawGeometricFigure(0.5f, true, true, false, false, 0.0625f * 72, LineJoinStyle.Rounded, 0, LineCapStyle.Round);

        _writer.MoveTo(0, 6f);
        DrawGeometricFigure(0.5f, false, true, false, false, 0.0625f * 72, LineJoinStyle.Bevel, 0, LineCapStyle.Square);
        _writer.MoveTo(3.75f, 6f);
        DrawGeometricFigure(0.5f, true, true, false, false, 0.0625f * 72, LineJoinStyle.Bevel, 0, LineCapStyle.Square);

        _writer.LineStyle.Width = 0.1f;
        _writer.MoveTo(0, 0f);
        _writer.ForeColor = Color.Blue;
        _writer.LineTo(0, 8f);

        _writer.NewPage(PageKind.Letter, false, 1f, 1f);

        _writer.MoveTo(0, 0f);
        DrawGeometricFigure(0.5f, false, true, false, false, 0.02f * 72, LineJoinStyle.Miter, 0, LineCapStyle.None);
        _writer.MoveTo(3.75f, 0f);
        DrawGeometricFigure(0.5f, false, true, false, false, 0.02f * 72, LineJoinStyle.Miter, 1, LineCapStyle.None);

        _writer.MoveTo(0, 2f);
        DrawGeometricFigure(0.5f, false, true, false, false, 0.02f * 72, LineJoinStyle.Miter, 2, LineCapStyle.None);
        _writer.MoveTo(3.75f, 2f);
        DrawGeometricFigure(0.5f, false, true, false, false, 0.02f * 72, LineJoinStyle.Miter, 3, LineCapStyle.None);

        _writer.MoveTo(0, 4f);
        DrawGeometricFigure(0.5f, false, true, false, false, 0.02f * 72, LineJoinStyle.Miter, 4, LineCapStyle.None);

        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    private void DrawGeometricFigure(float scale, bool close, bool border, bool fill, bool eofill, float lineWidth, LineJoinStyle joinStyle, int dashStyle, LineCapStyle capStyle)
    {
        var pos = _writer.Position;
        var len = 0.25f * scale;
        var leg = (float)Math.Sqrt(Math.Pow(len, 2) / 2);

        _writer.ForeColor = Color.Red;
        _writer.LineStyle.Width = lineWidth;
        _writer.LineStyle.JoinStyle = joinStyle;
        if (dashStyle == 0) {
            _writer.LineStyle.DashStyle = LineDashStyle.Solid;
        } else if (dashStyle == 1) {
            _writer.LineStyle.DashStyle = LineDashStyle.Dash;
        } else if (dashStyle == 2) {
            _writer.LineStyle.DashStyle = LineDashStyle.DashDotDot;
        } else if (dashStyle == 3) {
            _writer.LineStyle.DashStyle = LineDashStyle.Dot;
        } else if (dashStyle == 4) {
            _writer.LineStyle.DashStyle = new LineDashStyle(new float[] { 1f, 1f, 2f, 1f, 3f, 5f }, 5f);
        }
        _writer.LineStyle.CapStyle = capStyle;

        _writer
            .LineTo(1f * scale, 0f)
            .CornerTo(.25f * scale, .25f * scale, false)
            .LineTo(0f, 0.5f * scale)
            .BezierTo(0f, 0.25f * scale, 0.25f * scale, 0.25f * scale)
            .LineTo(0.5f * scale, 0)
            .BezierTo(0.25f * scale, 0f, 0.25f * scale + leg, leg)
            .LineTo(0.5f * scale, 0.5f * scale)
            .BezierTo(.25f * scale, .25f * scale, .75f * scale, -0.25f * scale, 1f * scale, 0f)
            .LineTo(0f, 1f * scale)
            .LineTo(-2f * scale, 0f)
            .LineTo(0f, .5f * scale)
            .LineTo(.5f * scale, 0f)
            .LineTo(0f, -1f * scale)
            .LineTo(0.5f * scale, 0f)
            .LineTo(0f, 1.5f * scale)
            .LineTo(-1f * scale, 0f)
            .CornerTo(-0.25f * scale, -0.25f * scale, true);

        if (close) {
            _writer.FinishPolygon(border, fill, eofill);
            _writer.Position.ShouldBe(pos);
        } else {
            _writer.FinishLine();
            _writer.Position.ShouldNotBe(pos);
        }

        _writer.MoveTo(pos);
        _writer.OffsetTo(4.5f * scale, 0).Rectangle(1f * scale, 0.4f * scale, 0, fill, border);
        _writer.OffsetTo(0, 0.7f * scale).Rectangle(1f * scale, 0.5f * scale, 0.2f * scale, fill, border);
        _writer.OffsetTo(0, 0.8f * scale).RectangleDualOffset(1f * scale, 0.7f * scale, 0.15f * scale, 0);
        _writer.OffsetTo(0, 1f * scale).RectangleDualOffset(1f * scale, 0.7f * scale, 0.15f * scale, 0.05f);
    }

    [Fact]
    public void LineDashStylesCompare()
    {
        var style1 = new LineDashStyle(new float[] { 1f, 1f, 2f, 1f, 3f, 5f }, 5f);
        style1.Array.ShouldBe(new float[] { 1f, 1f, 2f, 1f, 3f, 5f });
        style1.Array[0] = 50f;
        style1.Array.ShouldBe(new float[] { 1f, 1f, 2f, 1f, 3f, 5f });
        var style2 = new LineDashStyle(new float[] { 1f, 1f, 2f, 1f, 3f, 5f }, 5f);
        (style1 == style2).ShouldBeTrue();
        style1.Equals(style2).ShouldBeTrue();
        style2.Equals(style1).ShouldBeTrue();
        var style3 = new LineDashStyle(new float[] { 1f, 1f, 2f, 1f, 3f, 5f }, 6f);
        (style1 == style3).ShouldBeFalse();
        style1.Equals(style3).ShouldBeFalse();
        style3.Equals(style1).ShouldBeFalse();
        var style4 = new LineDashStyle(new float[] { 1f, 1f, 2f, 1f, 3f, 5f, 6f }, 5f);
        (style1 == style4).ShouldBeFalse();
        style1.Equals(style4).ShouldBeFalse();
        style4.Equals(style1).ShouldBeFalse();
    }

    [Fact]
    public void InvalidLineDashStyleThrows()
    {
        Should.Throw<ArgumentNullException>(() => new LineDashStyle(null!, 0f));
    }
}
