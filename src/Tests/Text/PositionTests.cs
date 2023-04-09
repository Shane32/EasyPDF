using System.Drawing;
using Font = Shane32.EasyPDF.Font;

namespace Tests.Text;

public class PositionTests
{
    private readonly PDFWriter _writer = new PDFWriter();

    public PositionTests()
    {
        _writer.ScaleMode = ScaleModes.Inches;
        _writer.NewPage(System.Drawing.Printing.PaperKind.Letter, false, 1f, 1f);
        _writer.PrepForTests();
    }

    [Theory]
    [InlineData(1f, 0f, 1f, 0.1103f, 0.1138f, 0.036f, 0.15f, 0f, 0f, 1f, 0f, 0.875f)]
    [InlineData(1.2f, 0f, 1f, 0.1103f, 0.1138f, 0.036f, 0.18f, 0.03f, 0f, 1f, 0f, 0.875f)]
    [InlineData(1f, 6f, 1f, 0.1103f, 0.1138f, 0.036f, 0.15f, 0f, 0.083f, 1f, 0f, 0.875f)]
    [InlineData(1f, 0f, 1.2f, 0.1324f, 0.1366f, 0.043f, 0.18f, 0f, 0f, 1f, 0f, 0.875f)]
    [InlineData(1.2f, 6f, 1.2f, 0.1324f, 0.1366f, 0.043f, 0.216f, 0.036f, 0.083f, 1f, 0f, 0.875f)]
    [InlineData(1f, 0f, 1f, 0.1103f, 0.1138f, 0.036f, 0.15f, 0f, 0f, 1.2f, 0f, 1.05f)]
    [InlineData(1f, 0f, 1f, 0.1103f, 0.1138f, 0.036f, 0.15f, 0f, 0f, 1f, 1f, 1.027f)]
    [InlineData(1f, 0f, 1f, 0.1103f, 0.1138f, 0.036f, 0.15f, 0f, 0f, 1.2f, 1f, 1.233f)]
    public void TextSizes(float lineSpacing, float paragraphSpacing, float stretchY, float capHeight, float ascent, float descent, float height, float leading, float paragraphLeading, float stretchX, float characterSpacing, float width)
    {
        _writer.Font = new Font(StandardFonts.Times, 12f) {
            LineSpacing = lineSpacing,
            ParagraphSpacing = paragraphSpacing,
            StretchY = stretchY,
            CharacterSpacing = characterSpacing,
            StretchX = stretchX,
        };
        _writer.TextCapHeight().ShouldBe(capHeight, 0.001f);
        _writer.TextAscent().ShouldBe(ascent, 0.001f);
        _writer.TextDescent().ShouldBe(descent, 0.001f);
        _writer.TextHeight().ShouldBe(height, 0.001f);
        _writer.TextLeading().ShouldBe(leading, 0.001f);
        _writer.TextHeight().ShouldBe(_writer.TextAscent() + _writer.TextDescent() + _writer.TextLeading(), 0.001f);
        _writer.TextParagraphLeading().ShouldBe(paragraphLeading, 0.001f);
        _writer.TextWidth("Hello World!").ShouldBe(width, 0.001f);
    }
    
    [Fact]
    public void Standard()
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
    public void StretchedY()
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
    public void Spacing()
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
    public void StretchedY_Spacing()
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
    public void StretchedX()
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
    public void CharacterSpacing()
    {
        _writer.Font = new Font(StandardFonts.Helvetica, 10f) { CharacterSpacing = 2f };
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
    public void MultiLine()
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
