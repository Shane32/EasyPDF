namespace Shane32.EasyPDF
{
    /// <summary>
    /// Specifies a value representation for coordinates passed to/from <see cref="PDFWriter"/>.
    /// </summary>
    public enum ScaleModes
    {
        /// <summary>
        /// Inches.
        /// </summary>
        Inches,

        /// <summary>
        /// Hundredths of an inch.
        /// </summary>
        Hundredths,

        /// <summary>
        /// 72/100 of an inch.
        /// </summary>
        Points,
    }
}
