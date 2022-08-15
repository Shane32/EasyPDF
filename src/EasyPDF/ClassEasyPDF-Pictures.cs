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
        /// Prints the specified image at the current position.
        /// </summary>
        public void PaintPicture(iTextImage img)
        {
            if (img.DpiX == 0 | img.DpiY == 0) {
                PaintPictureAbs(img, _Translate(img.Width / 96, ScaleModes.Inches), _Translate(img.Height / 96, ScaleModes.Inches));
            } else {
                PaintPictureAbs(img, _Translate(img.Width / img.DpiX, ScaleModes.Inches), _Translate(img.Height / img.DpiY, ScaleModes.Inches));
            }
        }

        /// <summary>
        /// Prints the specified image at the current position with the specified size.
        /// </summary>
        public void PaintPicture(iTextImage img, float width, float height)
        {
            PaintPictureAbs(img, _Translate(width), _Translate(height));
        }

        /// <summary>
        /// Prints the specified image at the specified position.
        /// <paramref name="step"/> can be used to indicate the coordinates are offsets to the current position.
        /// </summary>
        public void PaintPicture(iTextImage img, bool step, float x, float y)
        {
            if (step) {
                _currentX += _Translate(x);
                _currentY += _Translate(y);
            } else {
                _currentX = _Translate(x);
                _currentY = _Translate(y);
            }

            PaintPicture(img);
        }

        /// <summary>
        /// Prints the specified image at the specified position with the specified size.
        /// <paramref name="step"/> can be used to indicate the coordinates are offsets to the current position.
        /// </summary>
        public void PaintPicture(iTextImage img, bool step, float X, float Y, float width, float height)
        {
            if (step) {
                _currentX += _Translate(X);
                _currentY += _Translate(Y);
            } else {
                _currentX = _Translate(X);
                _currentY = _Translate(Y);
            }

            PaintPicture(img, width, height);
        }

        private void PaintPictureAbs(iTextImage img, float width, float height)
        {
            FinishLine();
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
                    offsetX -= width / 2f;
                    break;
                }

                case PictureAlignment.RightTop:
                case PictureAlignment.RightCenter:
                case PictureAlignment.RightBottom: {
                    offsetX -= width;
                    break;
                }
            }

            switch (PictureAlignment) {
                case PictureAlignment.LeftTop:
                case PictureAlignment.CenterTop:
                case PictureAlignment.RightTop: {
                    offsetY += height;
                    break;
                }

                case PictureAlignment.LeftCenter:
                case PictureAlignment.CenterCenter:
                case PictureAlignment.RightCenter: {
                    offsetY += height / 2f;
                    break;
                }

                case PictureAlignment.LeftBottom:
                case PictureAlignment.CenterBottom:
                case PictureAlignment.RightBottom: {
                    break;
                }
            }

            _content.AddImage(img, width, 0, 0, -height, _currentX + offsetX, _currentY + offsetY);
        }
    }
}
