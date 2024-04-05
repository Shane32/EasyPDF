using Font = Shane32.EasyPDF.Font;

namespace Tests.Text;

public class BasicFontTestsTests
{
    private readonly PDFWriter _writer = new PDFWriter();

    static BasicFontTestsTests()
    {
        TestExtensions.RegisterFont("Righteous-Regular.ttf", "Righteous");
    }
    
    public BasicFontTestsTests()
    {
        _writer.ScaleMode = ScaleModes.Inches;
        _writer.NewPage(PageKind.Letter, false, 1f, 1f);
        _writer.PrepForTests();
    }

    [Fact]
    public void ShowBasicFonts()
    {
        Show(StandardFonts.Helvetica);
        Show(StandardFonts.Times);
        Show(StandardFonts.Courier);

        void Show(StandardFonts standardFont)
        {
            _writer.Font = new Font(standardFont, 12f);
            _writer.Font.Size.ShouldBe(12f, 0.001f);
            _writer.Font.FamilyName.ShouldBe(standardFont.ToString());
            _writer.Font.Embedded.ShouldBeFalse();
            _writer.Font.Bold.ShouldBeFalse();
            _writer.Font.Italic.ShouldBeFalse();
            _writer.Font.Underline.ShouldBeFalse();
            _writer.Font.Strikeout.ShouldBeFalse();
            _writer.WriteLine(standardFont.ToString());
            _writer.Font.Bold = true;
            _writer.WriteLine(standardFont.ToString() + " Bold");
            _writer.Font.Bold = false;
            _writer.Font.Italic = true;
            _writer.WriteLine(standardFont.ToString() + " Italic");
            _writer.Font.Bold = true;
            _writer.Font.Italic.ShouldBeTrue();
            _writer.WriteLine(standardFont.ToString() + " Bold Italic");
            _writer.Font.Bold = false;
            _writer.Font.Italic = false;
            _writer.Font.Underline = true;
            _writer.WriteLine(standardFont.ToString() + " Underline");
            _writer.Font.Underline = false;
            _writer.Font.Strikeout = true;
            _writer.WriteLine(standardFont.ToString() + " Strikeout");
            _writer.Font.Bold = true;
            _writer.Font.Italic = true;
            _writer.Font.Underline = true;
            _writer.Font.Strikeout = true;
            _writer.WriteLine(standardFont.ToString() + " Bold Italic Underline Strikeout");
            _writer.WriteLine();
        }

        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    [Fact]
    public void KerningTests()
    {
        _writer.Font = new Font(StandardFonts.Helvetica, 12f);
        _writer.WriteLine("This is a test with no kerning");
        _writer.Font.CharacterSpacing = 2f;
        _writer.WriteLine("This is a test with 2pt kerning");
        _writer.Font.CharacterSpacing = 4f;
        _writer.Write("ABCD");
        _writer.WriteLine("EFGH");
        _writer.WriteLine();
        _writer.CurrentX = 0;
        _writer.WriteLine("THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG", 2.5f);
        _writer.WriteLine();
        _writer.Font.Justify = true;
        _writer.WriteLine("THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG", 2.5f);
        _writer.WriteLine();

        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    [Fact]
    public void SpacingTests()
    {
        _writer.Font = new Font(StandardFonts.Helvetica, 12f);
        Loop(2f);

        void Loop(float width)
        {
            _writer.WriteLine("This is a jolly test with standard spacing.", width);
            _writer.WriteLine("This is line 2.", width);
            _writer.WriteLine();
            _writer.Write("This is a jolly test with standard spacing.\nThis is line 2\n\n", width);
            _writer.CurrentY.ShouldBe(1.233333f, 0.001f);

            _writer.Font.LineSpacing = 1.5f;
            _writer.WriteLine("This is a jolly test with 1.5x line spacing.", width);
            _writer.WriteLine("This is line 2.", width);
            _writer.WriteLine();
            _writer.Write("This is a jolly test with 1.5x line spacing.\nThis is line 2\n\n", width);
            _writer.CurrentY.ShouldBe(3.08333f, 0.001f);

            _writer.Font.LineSpacing = 1f;
            _writer.Font.ParagraphSpacing = 6f;
            _writer.WriteLine("This is a jolly test with 6pt paragraph spacing.", width);
            _writer.WriteLine("This is line 2.", width);
            _writer.WriteLine();
            _writer.Write("This is a jolly test with 6pt paragraph spacing.\nThis is line 2\n\n", width);
            _writer.CurrentY.ShouldBe(4.816667f, 0.001f);

            _writer.Font.LineSpacing = 1.5f;
            _writer.Font.ParagraphSpacing = 6f;
            _writer.WriteLine("This is a jolly test with 6pt paragraph and 1.5x line spacing.", width);
            _writer.WriteLine("This is line 2.", width);
            _writer.WriteLine();
            _writer.Write("This is a jolly test with 6pt paragraph and 1.5x line spacing.\nThis is line 2\n\n", width);
            _writer.CurrentY.ShouldBe(7.62916667f, 0.001f);
        }

        _writer.MoveTo(_writer.Size.Width / 2, 0f);
        _writer.Font = new Font(StandardFonts.Helvetica, 6f) { StretchY = 2f };
        Loop(1f);

        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    [Fact]
    public void ParagraphTests()
    {
        _writer.Font = new Font(StandardFonts.Times, 12f);
        _writer.Font.HangingIndent.ShouldBe(0f, 0.001f);
        _writer.Font.CharacterSpacing.ShouldBe(0f, 0.001f);
        _writer.Font.LineSpacing.ShouldBe(1f, 0.001f);
        _writer.Font.LineSpacing = 1.2f;
        _writer.Font.ParagraphSpacing.ShouldBe(0f, 0.001f);
        _writer.Font.ParagraphSpacing = 12f;
        _writer.Font.StretchX.ShouldBe(1f, 0.001f);
        _writer.Font.StretchY.ShouldBe(1f, 0.001f);
        _writer.Font.Justify.ShouldBeFalse();
        _writer.WriteLine("This is a jolly test with standard spacing.  The quick brown fox jumps over the lazy dog.", 2f);
        _writer.Font.Justify = true;
        _writer.WriteLine("This is a jolly test with standard spacing.  The quick brown fox jumps over the lazy dog.", 2f);
        _writer.Font.StretchX = 1.2f;
        _writer.WriteLine("This is a jolly test with standard spacing.  The quick brown fox jumps over the lazy dog.", 2f);
        _writer.WriteLine("This is a jolly test with standard spacing #2.  The quick brown fox jumps over the lazy dog.", 2f);
        
        _writer.Font.HangingIndent = 0.25f;
        _writer.Font.StretchX = 1f;
        _writer.Font.Justify = false;
        _writer.WriteLine("This is a jolly test with standard spacing.  The quick brown fox jumps over the lazy dog.", 2f);
        _writer.Font.Justify = true;
        _writer.WriteLine("This is a jolly test with standard spacing.  The quick brown fox jumps over the lazy dog.", 2f);

        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    [Fact]
    public void BoundingBoxTests()
    {
        _writer.CurrentX = -0.5f;
        _writer.Font = new Font(StandardFonts.Times, 12f);
        _writer.TextAlignment = TextAlignment.LeftTop;
        TestMe("Standard");
        _writer.Font.LineSpacing = 1.5f;
        TestMe("LineSpacing 1.5x");
        _writer.Font.LineSpacing = 1f;
        _writer.Font.ParagraphSpacing = 6f;
        TestMe("ParagraphSpacing 6pt");
        _writer.Font.ParagraphSpacing = 0f;
        _writer.Font.StretchY = 2f;
        TestMe("StretchY 2x");
        _writer.Font.StretchX = 2f;
        _writer.Font.StretchY = 1f;
        TestMe("StretchX 2x");
        _writer.Font.StretchX = 1f;
        TestMe("Ends with 2 spaces  ");
        _writer.Font.CharacterSpacing = 1f;
        TestMe("With 1pt kerning");

        void TestMe(string str)
        {
            var pos = _writer.Position;
            str = "The quick brown fox jumps over the lazy dog. " + str;
            var w = _writer.TextWidth(str);
            var h = _writer.TextHeight();
            _writer.Rectangle(w, h);
            _writer.MoveTo(pos).Write(str);
            _writer.CurrentX.ShouldBe(pos.X + w, 0.001f);
            _writer.CurrentY.ShouldBe(pos.Y, 0.001f);
            _writer.MoveTo(pos).WriteLine();
            _writer.CurrentY.ShouldBe(pos.Y + h + _writer.Font.ParagraphSpacing / 72, 0.001f);
            _writer.OffsetTo(0, 0.125f);
        }

        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    [Fact]
    public void FontCanBeCloned()
    {
        var font = new Font("Righteous", 12f, bold: true);
        var font2 = font with { };
        font.ShouldNotBeSameAs(font2);
        font.ShouldBe(font2);

        font = new Font(StandardFonts.Helvetica, 12, italic: true);
        font2 = font with { };
        font.ShouldNotBeSameAs(font2);
        font.ShouldBe(font2);
    }
}
