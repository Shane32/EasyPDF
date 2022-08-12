using Shane32.EasyPDF;
using Xunit;

namespace EasyPDFTests
{
    public class DummyTests
    {
        [Fact]
        public void ItWorks()
        {
            using var writer = new PDFWriter();
            writer.ScaleMode = ScaleModes.Inches;
            writer.NewPage(System.Drawing.Printing.PaperKind.Letter, false, 1, 1);
            writer.Write("Hello");
            writer.QRCode("Test");
            writer.CurrentX = 0;
            writer.WriteLinesAt(true, 0f, 0f, 2f, 0f, 0f, "Testing", false);
            _ = writer.ToArray();
        }
    }
}
