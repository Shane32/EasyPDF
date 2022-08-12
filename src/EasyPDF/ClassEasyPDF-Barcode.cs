namespace Shane32.EasyPDF
{
    public partial class PDFWriter
    {
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
            var boxSize = size.HasValue ? size.Value / data.ModuleMatrix[0].Length : _TranslateRev(72f / 33);

            for (int row = 0; row < data.ModuleMatrix.Count; row++) {
                var rowData = data.ModuleMatrix[row];
                for (int col = 0; col < rowData.Length; col++) {
                    if (rowData[col]) {
                        Rectangle(false, x + col * boxSize, y + row * boxSize, true, boxSize, boxSize, 0, true, false);
                    }
                }
            }

            size ??= boxSize * data.ModuleMatrix.Count;
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
    }
}
