namespace Shane32.EasyPDF
{
    /// <summary>
    /// Specifies a text alignment when printing text.
    /// Alignment is applied on a per-line basis; blocks of text cannot be aligned as a group.
    /// </summary>
    public enum TextAlignment
    {
        /// <summary>
        /// Text is aligned to the left and the top of the bounding box.
        /// </summary>
        LeftTop,

        /// <summary>
        /// Text is aligned to the left and centered vertically along the height of the capital letters.
        /// </summary>
        LeftCenter,

        /// <summary>
        /// Text is aligned to the left and aligned to the baseline of the text.
        /// </summary>
        LeftBaseline,
        
        /// <summary>
        /// Text is aligned to the left and aligned to the bottom of the bounding box, including any line spacing.
        /// </summary>
        LeftBottom,

        /// <summary>
        /// Text is horizontally centered and aligned to the top of the bounding box.
        /// </summary>
        CenterTop,

        /// <summary>
        /// Text is horizontally centered and centered vertically along the height of the capital letters.
        /// </summary>
        CenterCenter,

        /// <summary>
        /// Text is horizontally centered and aligned to the baseline of the text.
        /// </summary>
        CenterBaseline,

        /// <summary>
        /// Text is horizontally centered and aligned to the bottom of the bounding box, including any line spacing.
        /// </summary>
        CenterBottom,

        /// <summary>
        /// Text is aligned to the right and aligned to the top of the bounding box.
        /// </summary>
        RightTop,

        /// <summary>
        /// Text is aligned to the right and centered vertically along the height of the capital letters.
        /// </summary>
        RightCenter,

        /// <summary>
        /// Text is aligned to the right and aligned to the baseline of the text.
        /// </summary>
        RightBaseline,

        /// <summary>
        /// Text is aligned to the right and aligned to the bottom of the bounding box, including any line spacing.
        /// </summary>
        RightBottom,
    }
}
