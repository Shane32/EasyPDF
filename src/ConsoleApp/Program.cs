using System.Net.Sockets;
using QRCoder;
using static QRCoder.PayloadGenerator;
using static QRCoder.QRCodeGenerator;

Console.WriteLine("Creating PDF");

var page = new Shane32.EasyPDF.PDFWriter();
page.NewPage(System.Drawing.Printing.PaperKind.Letter, false);
page.ScaleMode = Shane32.EasyPDF.ScaleModes.Inches;
page.CurrentX = 1;
page.CurrentY = 1;

var qrCodeGenerator = new QRCodeGenerator();
var qrData = qrCodeGenerator.CreateQrCode("www.zbox.com", ECCLevel.L);

page.QRCode(qrData);
page.MoveTo(false, 1, 3);
page.QRCode(qrData, 2, true);

//page.Font = new Shane32.EasyPDF.Font("Courier New", 10);
//page.Write("Testing", true);

//foreach (var row in qrData.ModuleMatrix) {
//    page.Write(string.Join("", Enumerable.Range(0, row.Length).Select(index => row[index]).Select(x => x ? "XX" : "  ")), true);
//    Console.WriteLine("b" + string.Join("", Enumerable.Range(0, row.Length).Select(index => row[index]).Select(x => x ? "XX" : "  ")));
//}

var data = page.ToArray();

using var fil = new FileStream("testbarcode.pdf", FileMode.Create, FileAccess.ReadWrite, FileShare.None);
await fil.WriteAsync(data);
fil.Close();

Console.WriteLine("Saved to testbarcode.pdf");

//return;

// 172.18.0.50
Console.WriteLine("Printer hostname/ip:");
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
