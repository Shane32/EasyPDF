using System.Net.Sockets;
using QRCoder;
using Shane32.EasyPDF;
using static QRCoder.QRCodeGenerator;
using Font = Shane32.EasyPDF.Font;

Console.WriteLine("Creating PDF");

var page = new PDFWriter();
page.NewPage(System.Drawing.Printing.PaperKind.Letter, false);
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

page.Font = new Font("Algerian", 20, System.Drawing.FontStyle.Italic);
page.WriteLine("This is a test");

page.Font = page.Font with { StretchX = 3f, StretchY = 3f };
page.WriteLine("This is a test");

page.Font = new Font(StandardFonts.Times, 10);
page.WriteLine("Times Regular");
page.Font = new Font(StandardFonts.Times, 10, System.Drawing.FontStyle.Bold);
page.WriteLine("Times Bold");
page.Font = new Font(StandardFonts.Times, 10, System.Drawing.FontStyle.Italic);
page.WriteLine("Times Italic");
page.Font = new Font(StandardFonts.Times, 10, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
page.WriteLine("Times Bold Italic");

page.Font = new Font(StandardFonts.Helvetica, 10);
page.WriteLine("Helvetica Regular");
page.Font = new Font(StandardFonts.Helvetica, 10, System.Drawing.FontStyle.Bold);
page.WriteLine("Helvetica Bold");
page.Font = new Font(StandardFonts.Helvetica, 10, System.Drawing.FontStyle.Italic);
page.WriteLine("Helvetica Italic");
page.Font = new Font(StandardFonts.Helvetica, 10, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
page.WriteLine("Helvetica Bold Italic");

page.Font = new Font(StandardFonts.Courier, 10);
page.WriteLine("Courier Regular");
page.Font = new Font(StandardFonts.Courier, 10, System.Drawing.FontStyle.Bold);
page.WriteLine("Courier Bold");
page.Font = new Font(StandardFonts.Courier, 10, System.Drawing.FontStyle.Italic);
page.WriteLine("Courier Italic");
page.Font = new Font(StandardFonts.Courier, 10, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
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
page.MoveTo(5, 3);
//page.QRCode(qrData, 2, true);
page.MoveTo(5, 3);
page.PictureAlignment = PictureAlignment.LeftCenter;
page.QRCode(qrData, 2, true);

page.Barcode("Hello this is a test", BarcodeType.Code128, 1, .5f);

var data = page.ToArray();

var filename = Path.ChangeExtension(Path.GetTempFileName(), "pdf");
using var fil = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
await fil.WriteAsync(data);
fil.Close();

Console.WriteLine("Saved");

var processInfo = new System.Diagnostics.ProcessStartInfo(filename);
processInfo.UseShellExecute = true;
System.Diagnostics.Process.Start(processInfo);

// 172.18.0.50
Console.WriteLine("Printer hostname/ip: (or enter to quit)");
var ip = Console.ReadLine();
if (ip == "" || ip == null)
    return;
using var client = new TcpClient(ip, 9100);
using var stream = client.GetStream();
await stream.WriteAsync(data);
await stream.FlushAsync();
Console.WriteLine("Printed to " + ip);

return;

Console.WriteLine("Hello World!");

Console.WriteLine("H");
Console.WriteLine(AsciiQrCode("qr.hrecycling.com/afdsfsk", QRCodeGenerator.ECCLevel.H));
Console.WriteLine("Q");
Console.WriteLine(AsciiQrCode("qr.hrecycling.com/afdsfsk", QRCodeGenerator.ECCLevel.Q));
Console.WriteLine("M");
Console.WriteLine(AsciiQrCode("qr.hrecycling.com/afdsfsk", QRCodeGenerator.ECCLevel.M));
Console.WriteLine("L");
Console.WriteLine(AsciiQrCode("qr.hrecycling.com/afdsfsk", QRCodeGenerator.ECCLevel.L));
Console.WriteLine("H");
Console.WriteLine(AsciiQrCode("qr.hrecycling.com/afdsfsk".ToUpper(), QRCodeGenerator.ECCLevel.H));
Console.WriteLine("Q");
Console.WriteLine(AsciiQrCode("qr.hrecycling.com/afdsfsk".ToUpper(), QRCodeGenerator.ECCLevel.Q));
Console.WriteLine("M");
Console.WriteLine(AsciiQrCode("qr.hrecycling.com/afdsfsk".ToUpper(), QRCodeGenerator.ECCLevel.M));
Console.WriteLine("L");
Console.WriteLine(AsciiQrCode("qr.hrecycling.com/afdsfsk".ToUpper(), QRCodeGenerator.ECCLevel.L));
Console.WriteLine("short H");
Console.WriteLine(AsciiQrCode("qr.zbox.com/abc".ToUpper(), QRCodeGenerator.ECCLevel.H));
Console.WriteLine("short Q");
Console.WriteLine(AsciiQrCode("qr.zbox.com/abc".ToUpper(), QRCodeGenerator.ECCLevel.Q));
Console.WriteLine("short M");
Console.WriteLine(AsciiQrCode("qr.zbox.com/abc".ToUpper(), QRCodeGenerator.ECCLevel.M));
Console.WriteLine("short L");
Console.WriteLine(AsciiQrCode("qr.zbox.com/abc".ToUpper(), QRCodeGenerator.ECCLevel.L));
Console.WriteLine("micro L");
Console.WriteLine(AsciiQrCode("abc".ToUpper(), QRCodeGenerator.ECCLevel.L));


static string AsciiQrCode(string url, QRCodeGenerator.ECCLevel eccLevel)
{
    var qrCodeGenerator = new QRCodeGenerator();
    var qrData = qrCodeGenerator.CreateQrCode(url, eccLevel);
    var asciiQrCode = new AsciiQRCode(qrData);
    var str = asciiQrCode.GetGraphic(1);
    return str;
}
