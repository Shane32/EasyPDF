using System.Drawing;
using System.Text;
using Font = Shane32.EasyPDF.Font;

namespace Tests;

public class TextTests
{
    private readonly PDFWriter _writer = new PDFWriter();

    public TextTests()
    {
        _writer.ScaleMode = ScaleModes.Inches;
        _writer.NewPage(System.Drawing.Printing.PaperKind.Letter, false, 1f, 1f);
        _writer.PrepForTests();
    }

    [Fact]
    public void PositioningTests()
    {
        _writer.Font = new Font(StandardFonts.Helvetica, 10f);
        for (int i = 0; i < 12; i++) {
            _writer.TextAlignment = (TextAlignment)i;
            var pos = new PointF(3f, i * 0.25f);
            var r = 0.0625f;
            _writer.MoveTo(pos).Circle(r);
            _writer.MoveTo(pos).OffsetTo(-r, 0f).LineTo(r + r, 0f);
            _writer.MoveTo(pos).OffsetTo(0f, -r).LineTo(0f, r + r);
            _writer.MoveTo(pos).WriteLine("Alignment: " + _writer.TextAlignment);
        }

        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    [Fact]
    public void PositioningTests_StretchedY()
    {
        _writer.Font = new Font(StandardFonts.Helvetica, 5f) { StretchY = 2f };
        for (int i = 0; i < 12; i++) {
            _writer.TextAlignment = (TextAlignment)i;
            var pos = new PointF(3f, i * 0.25f);
            var r = 0.0625f;
            _writer.MoveTo(pos).Circle(r);
            _writer.MoveTo(pos).OffsetTo(-r, 0f).LineTo(r + r, 0f);
            _writer.MoveTo(pos).OffsetTo(0f, -r).LineTo(0f, r + r);
            _writer.MoveTo(pos).WriteLine("Alignment: " + _writer.TextAlignment);
        }

        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    [Fact]
    public void PositioningTests_Spacing()
    {
        _writer.Font = new Font(StandardFonts.Helvetica, 10f) { LineSpacing = 1.2f };
        for (int i = 0; i < 12; i++) {
            _writer.TextAlignment = (TextAlignment)i;
            var pos = new PointF(3f, i * 0.25f);
            var r = 0.0625f;
            _writer.MoveTo(pos).Circle(r);
            _writer.MoveTo(pos).OffsetTo(-r, 0f).LineTo(r + r, 0f);
            _writer.MoveTo(pos).OffsetTo(0f, -r).LineTo(0f, r + r);
            _writer.MoveTo(pos).WriteLine("Alignment: " + _writer.TextAlignment);
        }

        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    [Fact]
    public void PositioningTests_StretchedY_Spacing()
    {
        _writer.Font = new Font(StandardFonts.Helvetica, 5f) { StretchY = 2f, LineSpacing = 1.2f };
        for (int i = 0; i < 12; i++) {
            _writer.TextAlignment = (TextAlignment)i;
            var pos = new PointF(3f, i * 0.25f);
            var r = 0.0625f;
            _writer.MoveTo(pos).Circle(r);
            _writer.MoveTo(pos).OffsetTo(-r, 0f).LineTo(r + r, 0f);
            _writer.MoveTo(pos).OffsetTo(0f, -r).LineTo(0f, r + r);
            _writer.MoveTo(pos).WriteLine("Alignment: " + _writer.TextAlignment);
        }

        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    [Fact]
    public void PositioningTests_StretchedX()
    {
        _writer.Font = new Font(StandardFonts.Helvetica, 10f) { StretchX = 2f };
        for (int i = 0; i < 12; i++) {
            _writer.TextAlignment = (TextAlignment)i;
            var pos = new PointF(3f, i * 0.25f);
            var r = 0.0625f;
            _writer.MoveTo(pos).Circle(r);
            _writer.MoveTo(pos).OffsetTo(-r, 0f).LineTo(r + r, 0f);
            _writer.MoveTo(pos).OffsetTo(0f, -r).LineTo(0f, r + r);
            _writer.MoveTo(pos).WriteLine("Alignment: " + _writer.TextAlignment);
        }

        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    [Fact]
    public void PositioningTests_MultiLine()
    {
        _writer.Font = new Font(StandardFonts.Helvetica, 10f);
        for (int i = 0; i < 12; i++) {
            _writer.TextAlignment = (TextAlignment)i;
            var pos = new PointF(3f, i * 0.5f);
            var r = 0.0625f;
            _writer.MoveTo(pos).Circle(r);
            _writer.MoveTo(pos).OffsetTo(-r, 0f).LineTo(r + r, 0f);
            _writer.MoveTo(pos).OffsetTo(0f, -r).LineTo(0f, r + r);
            _writer.MoveTo(pos).WriteLine("Alignment: " + _writer.TextAlignment + "\nLine 2");
        }

        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

}
