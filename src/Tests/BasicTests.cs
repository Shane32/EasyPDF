using QRCoder;
using static QRCoder.QRCodeGenerator;

namespace Tests;

public class BasicTests
{
    static BasicTests()
    {
        TestExtensions.RegisterFont("Righteous-Regular.ttf", "Righteous");
    }

    [Fact]
    public void ItWorks()
    {
        var page = new PDFWriter();
        page.NewPage(PageKind.Letter, false);
        page.PrepForTests();
        page.ScaleMode = ScaleModes.Inches;
        page.TextAlignment = TextAlignment.LeftTop;

        page.CurrentX = 1;
        page.CurrentY = 1;
        page.Circle(0.05f);
        page.Font = new Font(StandardFonts.Courier, 10) { LineSpacing = 1.2f };
        page.WriteLine("testing\r\nline2 this is a very long long line of text\nnew line\r  some more text");
        page.WriteLine();

        page.Font = new Font(StandardFonts.Helvetica, 12) { LineSpacing = 1.2f, StretchX = 2f, Justify = true };
        page.WriteLine("This is a very long line of text; the quick brown fox jumps over the lazy dog and hello world a few times over", 3);

        page.Font = new Font("Righteous", 20, italic: true);
        page.WriteLine("This is a test");

        page.Font = page.Font with { StretchX = 3f, StretchY = 3f };
        page.WriteLine("This is a test");

        page.Font = new Font(StandardFonts.Times, 10);
        page.WriteLine("Times Regular");
        page.Font = new Font(StandardFonts.Times, 10, bold: true);
        page.WriteLine("Times Bold");
        page.Font = new Font(StandardFonts.Times, 10, italic: true);
        page.WriteLine("Times Italic");
        page.Font = new Font(StandardFonts.Times, 10, true, true);
        page.WriteLine("Times Bold Italic");

        page.Font = new Font(StandardFonts.Helvetica, 10);
        page.WriteLine("Helvetica Regular");
        page.Font = new Font(StandardFonts.Helvetica, 10, bold: true);
        page.WriteLine("Helvetica Bold");
        page.Font = new Font(StandardFonts.Helvetica, 10, italic: true);
        page.WriteLine("Helvetica Italic");
        page.Font = new Font(StandardFonts.Helvetica, 10, true, true);
        page.WriteLine("Helvetica Bold Italic");

        page.Font = new Font(StandardFonts.Courier, 10);
        page.WriteLine("Courier Regular");
        page.Font = new Font(StandardFonts.Courier, 10, bold: true);
        page.WriteLine("Courier Bold");
        page.Font = new Font(StandardFonts.Courier, 10, italic: true);
        page.WriteLine("Courier Italic");
        page.Font = new Font(StandardFonts.Courier, 10, true, true);
        page.WriteLine("Courier Bold Italic");

        page.Font = new Font(StandardFonts.Symbol, 8);
        page.WriteLine(
            @"!	∀	#	∃	%	&	∍	(	)	*	+	,	−	.	/
3x	0	1	2	3	4	5	6	7	8	9	:	;	<	=	>	?
4x	≅	Α	Β	Χ	Δ	Ε	Φ	Γ	Η	Ι	ϑ	Κ	Λ	Μ	Ν	Ο
5x	Π	Θ	Ρ	Σ	Τ	Υ	ς	Ω	Ξ	Ψ	Ζ	[	∴	]	⊥	_
6x	‾	α	β	χ	δ	ε	φ	γ	η	ι	ϕ	κ	λ	μ	ν	ο
7x	π	θ	ρ	σ	τ	υ	ϖ	ω	ξ	ψ	ζ	{	|	}	~	
8x																
9x																
Ax	€	ϒ	′	≤	⁄	∞	ƒ	♣	♦	♥	♠	↔	←	↑	→	↓
Bx	°	±	″	≥	×	∝	∂	•	÷	≠	≡	≈	…	⏐	⎯	↵
Cx	ℵ	ℑ	ℜ	℘	⊗	⊕	∅	∩	∪	⊃	⊇	⊄	⊂	⊆	∈	∉
Dx	∠	∇	®	©	™	∏	√	⋅	¬	∧	∨	⇔	⇐	⇑	⇒	⇓
Ex	◊	〈	®	©	™	∑	⎛	⎜	⎝	⎡	⎢	⎣	⎧	⎨	⎩	⎪
Fx		〉	∫	⌠	⎮	⌡	⎞	⎟	⎠	⎤	⎥	⎦	⎫	⎬	⎭	".Replace("\t", "  "));

        page.Font = new Font(StandardFonts.Times, 10);
        page.WriteLine("------------------------------------");

        page.Font = new Font(StandardFonts.ZapfDingbats, 8);
        page.WriteLine(@"
2x	SP	✁	✂	✃	✄	☎	✆	✇	✈	✉	☛	☞	✌	✍	✎	✏
3x	✐	✑	✒	✓	✔	✕	✖	✗	✘	✙	✚	✛	✜	✝	✞	✟
4x	✠	✡	✢	✣	✤	✥	✦	✧	★	✩	✪	✫	✬	✭	✮	✯
5x	✰	✱	✲	✳	✴	✵	✶	✷	✸	✹	✺	✻	✼	✽	✾	✿
6x	❀	❁	❂	❃	❄	❅	❆	❇	❈	❉	❊	❋	●	❍	■	❏
7x	❐	❑	❒	▲	▼	◆	❖	◗	❘	❙	❚	❛	❜	❝	❞	
8x	❨	❩	❪	❫	❬	❭	❮	❯	❰	❱	❲	❳	❴	❵		
9x																
Ax		❡	❢	❣	❤	❥	❦	❧	♣	♦	♥	♠	①	②	③	④
Bx	⑤	⑥	⑦	⑧	⑨	⑩	❶	❷	❸	❹	❺	❻	❼	❽	❾	❿
Cx	➀	➁	➂	➃	➄	➅	➆	➇	➈	➉	➊	➋	➌	➍	➎	➏
Dx	➐	➑	➒	➓	➔	→	↔	↕	➘	➙	➚	➛	➜	➝	➞	➟
Ex	➠	➡	➢	➣	➤	➥	➦	➧	➨	➩	➪	➫	➬	➭	➮	➯
Fx		➱	➲	➳	➴	➵	➶	➷	➸	➹	➺	➻	➼	➽	➾	".Replace("\t", "  "));

        page.FillColor = System.Drawing.Color.Red;

        var qrCodeGenerator = new QRCodeGenerator();
        var qrData = qrCodeGenerator.CreateQrCode("www.zbox.com", ECCLevel.L);

        page.MoveTo(3, 3);
        page.QRCode(qrData);
        page.MoveTo(3, 3);
        page.ForeColor = System.Drawing.Color.Blue;
        page.Rectangle(qrData.ModuleMatrix.Count * 0.03f, qrData.ModuleMatrix.Count * 0.03f);
        page.OffsetTo(qrData.ModuleMatrix.Count * 0.03f, qrData.ModuleMatrix.Count * 0.03f);
        page.ForeColor = System.Drawing.Color.Red;
        //page.MoveTo(5, 3);
        //page.QRCode(qrData, 2, true);
        page.MoveTo(5, 3);
        page.PictureAlignment = PictureAlignment.LeftCenter;
        page.QRCode(qrData, 2, false);
        page.ForeColor = System.Drawing.Color.Blue;
        page.MoveTo(5, 2).Rectangle(2, 2).OffsetTo(2, 2);
        page.ForeColor = System.Drawing.Color.Red;

        page.Barcode("Hello this is a test", BarcodeType.Code128, 1, .5f);
        page.ForeColor = System.Drawing.Color.Blue;
        page.MoveTo(7, 3.75f).Rectangle(1, 0.5f);

        page.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public void LedgerLandscape(int method)
    {
        var page = new PDFWriter();
        page.ScaleMode = ScaleModes.Inches;
        switch (method) {
            case 1:
                page.NewPage(PageKind.Ledger, true, 0.75f, 0.5f);
                break;
            case 2:
                page.NewPage(PageKind.Ledger, true, 0.75f, 0.5f, 0.75f, 0.5f);
                break;
            case 3:
                page.NewPage(PageKind.Ledger, true, new MarginsF(0.75f, 0.5f, 0.75f, 0.5f));
                break;
            case 4:
                page.NewPage(new System.Drawing.SizeF(11, 17), true, new MarginsF(0.75f, 0.5f));
                break;
            case 5:
                page.NewPage(new System.Drawing.SizeF(11, 17), true, new MarginsF(0.75f, 0.5f, 0.75f, 0.5f));
                break;
            case 7:
                page.NewPage(11, 17, true, 0.75f, 0.5f);
                break;
            case 8:
                page.NewPage(11, 17, true, 0.75f, 0.5f, 0.75f);
                break;
            case 9:
                page.NewPage(11, 17, true, 0.75f, 0.5f, 0.75f, 0.5f);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(method));
        }
        page.PrepForTests();
        page.TextAlignment = TextAlignment.LeftTop;
        page.PageSize.Width.ShouldBe(17f, 0.01);
        page.PageSize.Height.ShouldBe(11f, 0.01);
        page.Size.Width.ShouldBe(15.5f, 0.01);
        page.Size.Height.ShouldBe(10f, 0.01);
        page.MarginOffset.X.ShouldBe(0.75f, 0.01);
        page.MarginOffset.Y.ShouldBe(0.5f, 0.01);

        page.Font = new Font(StandardFonts.Helvetica, 12) { LineSpacing = 1.2f };
        page.MoveTo(0f, 0f).WriteLine("This is Helvetica 12pt on landscape ledger paper with 0.5\" margins on the top/bottom and 0.75\" margins on the sides");
        page.MoveTo(0f, 0f).Rectangle(page.Size.Width, page.Size.Height);

        page.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    [Fact]
    public void MultiplePages()
    {
        var page = new PDFWriter();
        page.ScaleMode = ScaleModes.Inches;
        page.NewPage(PageKind.Letter, false, 0.75f, 0.5f);
        page.PrepForTests();
        page.Margins.Left.ShouldBe(0.75f, 0.001f);
        page.Margins.Top.ShouldBe(0.5f, 0.001f);
        page.Margins.Right.ShouldBe(0.75f, 0.001f);
        page.Margins.Bottom.ShouldBe(0.5f, 0.001f);

        page.WriteLine("Hello");
        page.NewPage(PageKind.Legal, true, 0.75f, 0.5f);
        page.WriteLine("there!");

        page.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    [Fact]
    public void SaveState()
    {
        var page = new PDFWriter();
        page.ScaleMode = ScaleModes.Inches;
        page.NewPage(PageKind.Letter, false, 1f, 1f);
        page.PrepForTests();
        page.TextAlignment = TextAlignment.LeftTop;
        page.PictureAlignment = PictureAlignment.LeftTop;
        page.Rectangle(page.Size.Width, page.Size.Height);
        page.MoveTo(0.125f, 0.125f);
        page.Write("Line 1");
        var oldX = page.CurrentX;

        using (page.SaveState()) {
            page.ForeColor = System.Drawing.Color.Red;
            page.WriteLine();
            page.CurrentX = 0;
            page.ScaleMode = ScaleModes.Points;
            page.CurrentY += 6;
            page.Font.Size = 20;
            page.Font.Italic = true;
            page.TextAlignment = TextAlignment.CenterTop;
            page.PictureAlignment = PictureAlignment.CenterCenter;
            page.LineStyle = new LineStyle(2f, LineCapStyle.Square, LineJoinStyle.Rounded, LineDashStyle.Dot);
            page.Write("Line 2");
            page.OffsetMargins(72f, 72f, 36f, 36f);
            page.MoveTo(0, 0).Rectangle(page.Size.Width, page.Size.Height);
        }

        page.PictureAlignment.ShouldBe(PictureAlignment.LeftTop);
        page.TextAlignment.ShouldBe(TextAlignment.LeftTop);
        page.ScaleMode.ShouldBe(ScaleModes.Inches);
        page.CurrentX.ShouldBe(oldX, 0.001f);
        page.CurrentY.ShouldBe(0.125f, 0.001f);
        page.Font.Size.ShouldBe(12f, 0.001f);
        page.Font.Italic.ShouldBeFalse();
        page.ForeColor.ShouldBe(System.Drawing.Color.Black);
        page.LineStyle.ShouldBe(new LineStyle());
        page.Margins.ShouldBe(new MarginsF(1f, 1f, 1f, 1f));

        page.WriteLine(" continued");

        page.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    [Fact]
    public void Metadata()
    {
        var page = new PDFWriter();
        page.NewPage(PageKind.Letter, false);
        page.Metadata.Author.ShouldBeNull();
        page.Metadata.Author = "Test Author";
        page.Metadata.Title.ShouldBeNull();
        page.Metadata.Title = "Test Title";
        page.Metadata.Subject.ShouldBeNull();
        page.Metadata.Subject = "Test Subject";
        page.Metadata.Keywords.ShouldBeNull();
        page.Metadata.Keywords = "Test Keywords";
        page.Metadata.Creator.ShouldBeNull();
        page.Metadata.Creator = "Test Creator";
        page.Metadata.Producer.ShouldStartWith("iTextSharp.LGPLv2.Core");
        page.Metadata.Producer = "Test Producer";
        page.Metadata.CreationDate.ShouldNotBeNull().ShouldBeInRange(DateTime.Now.AddMinutes(-5), DateTime.Now);
        page.Metadata.ModificationDate.ShouldBe(page.Metadata.CreationDate);
        page.Metadata.CreationDate = new DateTime(2018, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        page.Metadata.ModificationDate = new DateTime(2019, 2, 3, 4, 5, 6, DateTimeKind.Utc);
        page.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    [Fact]
    public void Margins()
    {
        var page = new PDFWriter();
        page.ScaleMode = ScaleModes.Inches;
        page.NewPage(PageKind.Letter, false, new MarginsF(0.5f, 1f, 1.5f, 2f));
        page.PrepForTests();
        page.Margins.Left.ShouldBe(0.5f, 0.001f);
        page.Margins.Top.ShouldBe(1f, 0.001f);
        page.Margins.Right.ShouldBe(1.5f, 0.001f);
        page.Margins.Bottom.ShouldBe(2f, 0.001f);
        page.PageSize.Width.ShouldBe(8.5f, 0.01);
        page.PageSize.Height.ShouldBe(11f, 0.01);
        page.Size.Width.ShouldBe(6.5f, 0.01);
        page.Size.Height.ShouldBe(8f, 0.01);
        page.ForeColor = System.Drawing.Color.Red;
        page.LineStyle.Width = 0.03f;
        page.MoveTo(0, 0).Rectangle(page.Size.Width, page.Size.Height);

        page.OffsetMargins(0.5f, 1f, 1.5f, 2f);
        page.Margins.Left.ShouldBe(1f, 0.001f);
        page.Margins.Top.ShouldBe(2f, 0.001f);
        page.Margins.Right.ShouldBe(3f, 0.001f);
        page.Margins.Bottom.ShouldBe(4f, 0.001f);
        page.PageSize.Width.ShouldBe(8.5f, 0.01);
        page.PageSize.Height.ShouldBe(11f, 0.01);
        page.Size.Width.ShouldBe(4.5f, 0.01);
        page.Size.Height.ShouldBe(5f, 0.01);
        page.ForeColor = System.Drawing.Color.Blue;
        page.MoveTo(0, 0).Rectangle(page.Size.Width, page.Size.Height);

        page.OffsetMargins(0, 1f);
        page.Margins.Left.ShouldBe(1f, 0.001f);
        page.Margins.Top.ShouldBe(3f, 0.001f);
        page.Margins.Right.ShouldBe(3f, 0.001f);
        page.Margins.Bottom.ShouldBe(4f, 0.001f);
        page.PageSize.Width.ShouldBe(8.5f, 0.01);
        page.PageSize.Height.ShouldBe(11f, 0.01);
        page.Size.Width.ShouldBe(4.5f, 0.01);
        page.Size.Height.ShouldBe(4f, 0.01);
        page.ForeColor = System.Drawing.Color.Black;
        page.MoveTo(0, 0).WriteLine("Testing");

        page.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }
}
