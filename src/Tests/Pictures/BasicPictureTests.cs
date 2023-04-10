using System.Drawing;
using System.Reflection;

namespace Tests.Pictures;

public class BasicPictureTests
{
    private readonly PDFWriter _writer = new PDFWriter();

    public BasicPictureTests()
    {
        _writer.ScaleMode = ScaleModes.Inches;
        _writer.NewPage(System.Drawing.Printing.PaperKind.Letter, false, 0.5f, 0.5f);
        _writer.PrepForTests();
    }

    [Fact]
    public void Alignment()
    {
        // get the embedded resource
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "Tests.logo.64x64.png";
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException("Could not find resource");
        // read all bytes from stream into byte array
        var bytes = new byte[stream.Length];
        if (stream.Read(bytes, 0, bytes.Length) != bytes.Length)
            throw new InvalidOperationException("Could not read resource");
        // create image from byte array - supports JPEG, PNG, GIF, BMP, TIFF
        var image = iTextSharp.text.Image.GetInstance(bytes);

        Draw(0, 0, PictureAlignment.LeftTop);
        Draw(7.5f / 2, 0, PictureAlignment.CenterTop);
        Draw(7.5f, 0, PictureAlignment.RightTop);

        Draw(0, 4, PictureAlignment.LeftCenter);
        Draw(7.5f / 2, 4, PictureAlignment.CenterCenter);
        Draw(7.5f, 4, PictureAlignment.RightCenter);

        Draw(0, 8, PictureAlignment.LeftBottom);
        Draw(7.5f / 2, 8, PictureAlignment.CenterBottom);
        Draw(7.5f, 8, PictureAlignment.RightBottom);

        void Draw(float x, float y, PictureAlignment alignment)
        {
            _writer.PictureAlignment = alignment;
            _writer.MoveTo(x, y).PaintPicture(image, width: 1.5f);
            _writer.CurrentX.ShouldBe(x);
            _writer.CurrentY.ShouldBe(y);
            CrossHair(1.75f);
        }

        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    [Fact]
    public void Jpeg()
    {
        // get the embedded resource
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "Tests.logo.jpg";
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException("Could not find resource");
        // read all bytes from stream into byte array
        var bytes = new byte[stream.Length];
        if (stream.Read(bytes, 0, bytes.Length) != bytes.Length)
            throw new InvalidOperationException("Could not read resource");
        // create image from byte array - supports JPEG, PNG, GIF, BMP, TIFF
        var image = iTextSharp.text.Image.GetInstance(bytes);

        _writer.PictureAlignment = PictureAlignment.CenterTop;
        _writer.MoveTo(7.5f / 2, 0).PaintPicture(image, height: 10f);
        
        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
    }

    [Fact]
    public void ActualSize()
    {
        // get the embedded resource
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "Tests.logo.64x64.png";
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException("Could not find resource");
        // read all bytes from stream into byte array
        var bytes = new byte[stream.Length];
        if (stream.Read(bytes, 0, bytes.Length) != bytes.Length)
            throw new InvalidOperationException("Could not read resource");
        // create image from byte array - supports JPEG, PNG, GIF, BMP, TIFF
        var image = iTextSharp.text.Image.GetInstance(bytes);

        _writer.PaintPicture(image);
        
        _writer.ToArray().SaveAsPdf().ToASCIIString().RemoveID().ShouldMatchApproved(o => o.NoDiff());
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
}
