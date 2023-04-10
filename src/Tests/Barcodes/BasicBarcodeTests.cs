using System.Drawing;

namespace Tests.Barcodes;

public class BasicBarcodeTests
{
    private readonly PDFWriter _writer = new PDFWriter();

    public BasicBarcodeTests()
    {
        _writer.ScaleMode = ScaleModes.Inches;
        _writer.NewPage(System.Drawing.Printing.PaperKind.Letter, false, 1f, 1f);
        _writer.PrepForTests();
    }

    [Fact]
    public void QRAlignment()
    {
        using var generator = new QRCoder.QRCodeGenerator();
        using var qr = generator.CreateQrCode("https://github.com/Shane32/EasyPDF", QRCoder.QRCodeGenerator.ECCLevel.L);

        PrintPage(true);
        _writer.NewPage(System.Drawing.Printing.PaperKind.Letter, false, 1f, 1f);
        PrintPage(false);
        
        void PrintPage(bool quietZone)
        {
            _writer.PictureAlignment = PictureAlignment.LeftTop;
            _writer.MoveTo(0, 0).QRCode(qr, quietZone: quietZone);
            _writer.Position.ShouldBe(new PointF(0, 0));
            Box(_writer.QRCodeSize(qr, quietZone: quietZone));
            _writer.Position.ShouldBe(new PointF(0, 0));
            CrossHair(2f);

            _writer.PictureAlignment = PictureAlignment.CenterTop;
            _writer.MoveTo(6.5f / 2, 0).QRCode(qr, quietZone: quietZone);
            _writer.Position.ShouldBe(new PointF(6.5f / 2, 0));
            CrossHair(2f);

            _writer.PictureAlignment = PictureAlignment.RightTop;
            _writer.MoveTo(6.5f, 0).QRCode(qr, quietZone: quietZone);
            _writer.Position.ShouldBe(new PointF(6.5f, 0));
            CrossHair(2f);

            _writer.PictureAlignment = PictureAlignment.LeftCenter;
            _writer.MoveTo(0, 8f / 2).QRCode(qr, quietZone: quietZone);
            _writer.Position.ShouldBe(new PointF(0, 8f / 2));
            CrossHair(2f);

            _writer.PictureAlignment = PictureAlignment.CenterCenter;
            _writer.MoveTo(6.5f / 2, 8f / 2).QRCode(qr, quietZone: quietZone);
            _writer.Position.ShouldBe(new PointF(6.5f / 2, 8f / 2));
            CrossHair(2f);

            _writer.PictureAlignment = PictureAlignment.RightCenter;
            _writer.MoveTo(6.5f, 8f / 2).QRCode(qr, quietZone: quietZone);
            _writer.Position.ShouldBe(new PointF(6.5f, 8f / 2));
            CrossHair(2f);

            _writer.PictureAlignment = PictureAlignment.LeftBottom;
            _writer.MoveTo(0, 8f).QRCode(qr, quietZone: quietZone);
            _writer.Position.ShouldBe(new PointF(0, 8f));
            CrossHair(2f);

            _writer.PictureAlignment = PictureAlignment.CenterBottom;
            _writer.MoveTo(6.5f / 2, 8f).QRCode(qr, quietZone: quietZone);
            _writer.Position.ShouldBe(new PointF(6.5f / 2, 8f));
            CrossHair(2f);

            _writer.PictureAlignment = PictureAlignment.RightBottom;
            _writer.MoveTo(6.5f, 8f).QRCode(qr, quietZone: quietZone);
            _writer.Position.ShouldBe(new PointF(6.5f, 8f));
            CrossHair(2f);
        }

        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    [Fact]
    public void BarcodeAlignment()
    {
        var str = "https://github.com";

        _writer.PictureAlignment = PictureAlignment.LeftTop;
        _writer.MoveTo(0, 0).Barcode(str);
        _writer.Position.ShouldBe(new PointF(0, 0));
        Box(_writer.BarcodeSize(str), 0.5f);
        _writer.Position.ShouldBe(new PointF(0, 0));
        CrossHair(2f);

        _writer.PictureAlignment = PictureAlignment.CenterTop;
        _writer.MoveTo(6.5f / 2, 1).Barcode(str);
        _writer.Position.ShouldBe(new PointF(6.5f / 2, 1));
        CrossHair(2f);

        _writer.PictureAlignment = PictureAlignment.RightTop;
        _writer.MoveTo(6.5f, 2).Barcode(str);
        _writer.Position.ShouldBe(new PointF(6.5f, 2));
        CrossHair(2f);

        _writer.PictureAlignment = PictureAlignment.LeftCenter;
        _writer.MoveTo(0, 8f / 2 - 1).Barcode(str);
        _writer.Position.ShouldBe(new PointF(0, 8f / 2 - 1));
        CrossHair(2f);

        _writer.PictureAlignment = PictureAlignment.CenterCenter;
        _writer.MoveTo(6.5f / 2, 8f / 2).Barcode(str);
        _writer.Position.ShouldBe(new PointF(6.5f / 2, 8f / 2));
        CrossHair(2f);

        _writer.PictureAlignment = PictureAlignment.RightCenter;
        _writer.MoveTo(6.5f, 8f / 2 + 1).Barcode(str);
        _writer.Position.ShouldBe(new PointF(6.5f, 8f / 2 + 1));
        CrossHair(2f);

        _writer.PictureAlignment = PictureAlignment.LeftBottom;
        _writer.MoveTo(0, 8f - 2).Barcode(str);
        _writer.Position.ShouldBe(new PointF(0, 8f - 2));
        CrossHair(2f);

        _writer.PictureAlignment = PictureAlignment.CenterBottom;
        _writer.MoveTo(6.5f / 2, 8f - 1).Barcode(str);
        _writer.Position.ShouldBe(new PointF(6.5f / 2, 8f - 1));
        CrossHair(2f);

        _writer.PictureAlignment = PictureAlignment.RightBottom;
        _writer.MoveTo(6.5f, 8f).Barcode(str);
        _writer.Position.ShouldBe(new PointF(6.5f, 8f));
        CrossHair(2f);

        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    [Fact]
    public void ColorsSizes()
    {
        _writer.FillColor = Color.Red;
        _writer.QRCode("https://github.com/Shane32/EasyPDF", quietZone: false);
        _writer.QRCodeSize("https://github.com/Shane32/EasyPDF", quietZone: false).ShouldBe(0.8788f, 0.001f);

        _writer.MoveTo(2, 0);
        _writer.LineStyle.JoinStyle = LineJoinStyle.Rounded;
        _writer.LineStyle.Width = 0.0625f * 72;
        _writer.QRCode("https://github.com/Shane32/EasyPDF", quietZone: false);
        _writer.Position.ShouldBe(new PointF(2, 0));

        _writer.MoveTo(0, 2);
        _writer.QRCode("https://github.com/Shane32/EasyPDF", size: 2f, quietZone: false);
        _writer.Position.ShouldBe(new PointF(0, 2));

        _writer.MoveTo(3, 2);
        _writer.QRCode("https://github.com/Shane32/EasyPDF", eccLevel: QRCoder.QRCodeGenerator.ECCLevel.H, size: 2f, quietZone: false);
        _writer.Position.ShouldBe(new PointF(3, 2));

        _writer.MoveTo(0, 4.5f);
        _writer.Barcode("https://github.com");
        _writer.Position.ShouldBe(new PointF(0, 4.5f));

        _writer.MoveTo(0, 5.5f);
        _writer.Barcode("https://github.com", width: 6.5f);
        _writer.Position.ShouldBe(new PointF(0, 5.5f));

        _writer.MoveTo(0, 6.5f);
        _writer.Barcode("https://github.com", height: 1f);
        _writer.Position.ShouldBe(new PointF(0, 6.5f));

        _writer.MoveTo(0, 8);
        _writer.Barcode("https://github.com", width: 6f, height: 1f);
        _writer.Position.ShouldBe(new PointF(0, 8));

        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    private void Box(float w, float? h = null)
    {
        using var _ = _writer.SaveState();
        _writer.ForeColor = Color.Blue;
        _writer.LineStyle = new LineStyle(0.02f * 72f, dashStyle: LineDashStyle.Dash);
        _writer.Rectangle(w, h ?? w);
    }
    
    private void CrossHair(float w)
    {
        using var _ = _writer.SaveState();
        _writer.ScaleMode = ScaleModes.Inches;
        _writer.ForeColor = Color.Red;
        _writer.LineStyle = new LineStyle(0.03f * 72f, dashStyle: LineDashStyle.Dash);
        _writer.OffsetTo(-w / 2, 0f).LineTo(w, 0);
        _writer.OffsetTo(-w / 2, -w / 2).LineTo(0, w);
    }

    [Fact]
    public void InvalidBarcodeType()
    {
        Should.Throw<ArgumentOutOfRangeException>(() => _writer.Barcode("test", (BarcodeType)1));
        Should.Throw<ArgumentOutOfRangeException>(() => _writer.BarcodeSize("test", (BarcodeType)1));
    }
}
