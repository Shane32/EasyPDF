namespace Shane32.EasyPDF
{
    /// <summary>
    /// Specifies a line cap style used when drawing lines.
    /// </summary>
    public enum LineCapStyle
    {
        /// <summary>
        /// The line will end abruptly at the start/end coordinate, without extending past the coordinate.
        /// A line that starts and ends at the same coordinate will be invisible.
        /// </summary>
        /// 
        None = iTextSharp.text.pdf.PdfContentByte.LINE_CAP_BUTT,

        /// <summary>
        /// The line will extend past the start/end coordinate by the amount of half the line width.
        /// A line that starts and ends at the same coordinate will appear to be a square.
        /// </summary>
        Square = iTextSharp.text.pdf.PdfContentByte.LINE_CAP_PROJECTING_SQUARE,

        /// <summary>
        /// The line will extend past the start/end coordinate with a half circle the radius of the line width.
        /// A line that starts and ends at the same coordinate will appear to be a circle.
        /// </summary>
        Round = iTextSharp.text.pdf.PdfContentByte.LINE_CAP_ROUND,
    }
}
