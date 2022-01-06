namespace Shane32.EasyPDF
{
    /// <summary>
    /// Specifies a line join style when drawing polygons or connected line segments.
    /// </summary>
    public enum LineJoinStyle
    {
        /// <summary>
        /// Draws a mitered corner.
        /// </summary>
        Miter = iTextSharp.text.pdf.PdfContentByte.LINE_JOIN_MITER,

        /// <summary>
        /// Draws a rounded corner.
        /// </summary>
        Rounded = iTextSharp.text.pdf.PdfContentByte.LINE_JOIN_ROUND,

        /// <summary>
        /// Draws a beveled corner.
        /// </summary>
        Bevel = iTextSharp.text.pdf.PdfContentByte.LINE_JOIN_BEVEL
    }
}
