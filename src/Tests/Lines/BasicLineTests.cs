using System.Drawing;
using Font = Shane32.EasyPDF.Font;

namespace Tests.Lines;

public class BasicLineTests
{
    private readonly PDFWriter _writer = new PDFWriter();

    public BasicLineTests()
    {
        _writer.ScaleMode = ScaleModes.Inches;
        _writer.NewPage(System.Drawing.Printing.PaperKind.Letter, false, 1f, 1f);
        _writer.PrepForTests();
    }

    [Fact]
    public void FillStyles()
    {
        _writer.FillColor.ShouldBe(Color.Black);

        DrawGeometricFigure(0.5f, false, false, false, false, null, LineJoinStyle.Miter, 0, LineCapStyle.None);

        _writer.MoveTo(0, 2f);
        DrawGeometricFigure(0.5f, true, true, false, false, null, LineJoinStyle.Miter, 0, LineCapStyle.None);

        _writer.MoveTo(0, 4f);
        DrawGeometricFigure(0.5f, true, false, true, false, null, LineJoinStyle.Miter, 0, LineCapStyle.None);

        _writer.MoveTo(0, 6f);
        DrawGeometricFigure(0.5f, true, false, true, true, null, LineJoinStyle.Miter, 0, LineCapStyle.None);

        _writer.MoveTo(0, 8f);
        DrawGeometricFigure(0.5f, true, true, true, true, null, LineJoinStyle.Miter, 0, LineCapStyle.None);

        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }


    [Fact]
    public void LineStyles()
    {
        _writer.ForeColor.ShouldBe(Color.Black);
        _writer.LineStyle.JoinStyle.ShouldBe(LineJoinStyle.Miter);
        _writer.LineStyle.CapStyle.ShouldBe(LineCapStyle.None);
        _writer.LineStyle.DashStyle.ShouldBe(LineDashStyle.Solid);
        _writer.LineStyle.Width.ShouldBeNull();

        _writer.MoveTo(0, 0f);
        DrawGeometricFigure(0.5f, false, true, false, false, null, LineJoinStyle.Miter, 0, LineCapStyle.None);
        _writer.MoveTo(3.75f, 0f);
        DrawGeometricFigure(0.5f, true, true, false, false, null, LineJoinStyle.Miter, 0, LineCapStyle.None);

        _writer.MoveTo(0, 2f);
        DrawGeometricFigure(0.5f, false, true, false, false, 0.125f, LineJoinStyle.Miter, 0, LineCapStyle.None);
        _writer.MoveTo(3.75f, 2f);
        DrawGeometricFigure(0.5f, true, true, false, false, 0.125f, LineJoinStyle.Miter, 0, LineCapStyle.None);

        _writer.MoveTo(0, 4f);
        //DrawGeometricFigure(0.5f, false, false, false, false, null, LineJoinStyle.Miter, 0, LineCapStyle.None);

        _writer.MoveTo(0, 6f);
        //DrawGeometricFigure(0.5f, false, false, false, false, null, LineJoinStyle.Miter, 0, LineCapStyle.None);

        _writer.MoveTo(0, 8f);
        //DrawGeometricFigure(0.5f, false, false, false, false, null, LineJoinStyle.Miter, 0, LineCapStyle.None);

        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    private void DrawGeometricFigure(float scale, bool close, bool border, bool fill, bool eofill, float? lineWidth, LineJoinStyle joinStyle, int dashStyle, LineCapStyle capStyle)
    {
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
            .LineTo(-1f, 0f);

        if (close) {
            _writer.FinishPolygon(border, fill, eofill);
        } else {
            _writer.FinishLine();
        }
    }
}
