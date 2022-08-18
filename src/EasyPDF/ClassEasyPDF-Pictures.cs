using iTextImage = iTextSharp.text.Image;

namespace Shane32.EasyPDF
{
    public partial class PDFWriter
    {
        /// <summary>
        /// Gets or sets the alignment of printed images.
        /// This alignment is relative to the current position when printing.
        /// </summary>
        public PictureAlignment PictureAlignment { get; set; }

        /// <summary>
        /// Prints the specified image at the current position with the specified size.
        /// </summary>
        public PDFWriter PaintPicture(iTextImage img, float? width = null, float? height = null)
        {
            if (width == null && height == null) {
                if (img.DpiX == 0 || img.DpiY == 0) {
                    return PaintPictureAbs(img, _Translate(img.Width / 96, ScaleModes.Inches), _Translate(img.Height / 96, ScaleModes.Inches));
                } else {
                    return PaintPictureAbs(img, _Translate(img.Width / img.DpiX, ScaleModes.Inches), _Translate(img.Height / img.DpiY, ScaleModes.Inches));
                }
            }
            width ??= height / img.Height * img.Width;
            height ??= width / img.Width * img.Height;
            return PaintPictureAbs(img, _Translate(width!.Value), _Translate(height!.Value));
        }

        private PDFWriter PaintPictureAbs(iTextImage img, float widthPoints, float heightPoints)
        {
            FinishLineAndUpdateLineStyle();
            float offsetX = default, offsetY = default;
            switch (PictureAlignment) {
                case PictureAlignment.LeftTop:
                case PictureAlignment.LeftCenter:
                case PictureAlignment.LeftBottom: {
                    break;
                }

                case PictureAlignment.CenterTop:
                case PictureAlignment.CenterCenter:
                case PictureAlignment.CenterBottom: {
                    offsetX -= widthPoints / 2f;
                    break;
                }

                case PictureAlignment.RightTop:
                case PictureAlignment.RightCenter:
                case PictureAlignment.RightBottom: {
                    offsetX -= widthPoints;
                    break;
                }
            }

            switch (PictureAlignment) {
                case PictureAlignment.LeftTop:
                case PictureAlignment.CenterTop:
                case PictureAlignment.RightTop: {
                    offsetY += heightPoints;
                    break;
                }

                case PictureAlignment.LeftCenter:
                case PictureAlignment.CenterCenter:
                case PictureAlignment.RightCenter: {
                    offsetY += heightPoints / 2f;
                    break;
                }

                case PictureAlignment.LeftBottom:
                case PictureAlignment.CenterBottom:
                case PictureAlignment.RightBottom: {
                    break;
                }
            }

            _content.AddImage(img, widthPoints, 0, 0, -heightPoints, _currentX + offsetX, _currentY + offsetY);

            return this;
        }
    }
}
