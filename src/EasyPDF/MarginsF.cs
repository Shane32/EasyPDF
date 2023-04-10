namespace Shane32.EasyPDF
{
    /// <summary>
    /// Represents the current margins of the page.
    /// </summary>
    public struct MarginsF
    {
        /// <summary>
        /// Initializes a new instance with no margins.
        /// </summary>
        public MarginsF()
        {
            Left = 0f;
            Top = 0f;
            Right = 0f;
            Bottom = 0f;
        }

        /// <summary>
        /// Initializes a new instance with the specified margins, in the scale mode of the page.
        /// Right and bottom margins are mirrored from the left and top margins.
        /// </summary>
        public MarginsF(float left, float top)
        {
            Left = left;
            Top = top;
            Right = left;
            Bottom = top;
        }

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
