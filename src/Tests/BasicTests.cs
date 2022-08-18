namespace Tests
{
    public class BasicTests
    {
        [Fact]
        public void ItWorks()
        {
            using var writer = new PDFWriter();
            writer.ScaleMode = ScaleModes.Inches;
            writer.NewPage(System.Drawing.Printing.PaperKind.Letter, false, 1, 1);
            writer.WriteLine("Hello");
            writer.QRCode("Test");
            writer.CurrentX = 0;
            writer.WriteLine("Testing");
            writer.MoveTo(1, 1).LineTo(1, 0).LineTo(0, 1).LineTo(-1, 0).FinishPolygon(false, true);
            _ = writer.ToArray();
        }
    }
}
