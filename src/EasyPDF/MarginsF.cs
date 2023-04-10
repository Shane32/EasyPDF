namespace Shane32.EasyPDF
{
    /// <summary>
    /// Represents the current margins of the page.
    /// </summary>
    public struct MarginsF
    {
        /// <summary>
        /// Initializes a new instance with the specified margins, in the scale mode of the page.
        /// </summary>
        public MarginsF(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        /// <summary>
        /// Gets or sets the left margin, in the scale mode of the page.
        /// </summary>
        public float Left { get; set; }

        /// <summary>
        /// Gets or sets the top margin, in the scale mode of the page.
        /// </summary>
        public float Top { get; set; }

        /// <summary>
        /// Gets or sets the right margin, in the scale mode of the page.
        /// </summary>
        public float Right { get; set; }

        /// <summary>
        /// Gets or sets the bottom margin, in the scale mode of the page.
        /// </summary>
        public float Bottom { get; set; }
    }
}
