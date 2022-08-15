namespace Shane32.EasyPDF
{
    public partial class PDFWriter
    {
        /// <summary>
        /// Default box size for QR codes in points (0.03" dot pitch)
        /// </summary>
        private const float DEFAULT_BOX_SIZE = 72f / 33;

        /// <summary>
        /// Prints a QR code in the color specified by <see cref="FillColor"/>.
        /// If <paramref name="size"/> is not specified, QR code prints with 0.03" dot pitch;
        /// otherwise the overall size of the QR code is the specified size.
        /// The border, if enabled, prints with the color specified by <see cref="ForeColor"/>.
        /// </summary>
        public void QRCode(QRCoder.QRCodeData data, float? size = null, bool border = false)
        {
            var x = CurrentX;
            var y = CurrentY;
            var boxSize = size.HasValue ? size.Value / data.ModuleMatrix.Count : _TranslateRev(DEFAULT_BOX_SIZE);
            size ??= boxSize * data.ModuleMatrix.Count;

            switch (PictureAlignment) {
                case PictureAlignment.LeftTop:
                case PictureAlignment.LeftCenter:
                case PictureAlignment.LeftBottom:
                    break;
                case PictureAlignment.CenterTop:
                case PictureAlignment.CenterCenter:
                case PictureAlignment.CenterBottom:
                    x -= size.Value / 2f;
                    break;
                case PictureAlignment.RightTop:
                case PictureAlignment.RightCenter:
                case PictureAlignment.RightBottom:
                    x -= size.Value;
                    break;
            }

            switch (PictureAlignment) {
                case PictureAlignment.LeftTop:
                case PictureAlignment.CenterTop:
                case PictureAlignment.RightTop:
                    break;
                case PictureAlignment.LeftCenter:
                case PictureAlignment.CenterCenter:
                case PictureAlignment.RightCenter:
                    y -= size.Value / 2f;
                    break;
                case PictureAlignment.LeftBottom:
                case PictureAlignment.CenterBottom:
                case PictureAlignment.RightBottom:
                    y -= size.Value;
                    break;
            }

            for (int row = 0; row < data.ModuleMatrix.Count; row++) {
                var rowData = data.ModuleMatrix[row];
                for (int col = 0; col < rowData.Length; col++) {
                    if (rowData[col]) {
                        Rectangle(false, x + col * boxSize, y + row * boxSize, true, boxSize, boxSize, 0, true, false);
                    }
                }
            }

            if (border) {
                Rectangle(false, x, y, true, size.Value, size.Value);
            }

            MoveTo(false, x + size.Value, y + size.Value);
        }

        /// <inheritdoc cref="QRCode(QRCoder.QRCodeData, float?, bool)"/>
        public void QRCode(string code, QRCoder.QRCodeGenerator.ECCLevel eccLevel = QRCoder.QRCodeGenerator.ECCLevel.L, float? size = null, bool border = false)
        {
            var generator = new QRCoder.QRCodeGenerator();
            var qrCode = generator.CreateQrCode(code, eccLevel);
            QRCode(qrCode, size, border);
        }

        /// <summary>
        /// Returns the default size of the specified QR code (uses 0.03" dot pitch).
        /// </summary>
        public float QRCodeSize(QRCoder.QRCodeData code)
        {
            return _TranslateRev(DEFAULT_BOX_SIZE * code.ModuleMatrix.Count);
        }

        /// <summary>
        /// Returns the default size of the specified QR code (uses 0.03" dot pitch).
        /// </summary>
        public float QRCodeSize(string code, QRCoder.QRCodeGenerator.ECCLevel eccLevel = QRCoder.QRCodeGenerator.ECCLevel.L)
        {
            var generator = new QRCoder.QRCodeGenerator();
            var qrCode = generator.CreateQrCode(code, eccLevel);
            return QRCodeSize(qrCode);
        }

        /// <summary>
        /// Prints a CODE-128 barcode
        /// </summary>
        public void Barcode(string text, float? width = null, float? height = null)
        {
            FinishLine();
            var c = new iTextSharp.text.pdf.Barcode128();
            c.Code = text;
            c.CodeType = iTextSharp.text.pdf.Barcode128.CODE128;
            c.Font = null;
            c.BarHeight = height == null ? 72f * 0.5f : _Translate(height.Value);
            _content.SaveState();
            try {
                //_content.Transform(new System.Drawing.Drawing2D.Matrix()
                c.PlaceBarcode(_content, null, _GetColor(_foreColor));
            }
            finally {
                _content.RestoreState();
            }
        }
    }
}
