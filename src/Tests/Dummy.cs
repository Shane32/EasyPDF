using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Moq;
using Shane32.EasyPDF;
using Shouldly;
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
            _ = writer.ToArray();
        }
    }
}
