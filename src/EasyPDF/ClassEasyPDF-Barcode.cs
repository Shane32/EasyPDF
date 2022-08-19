using System;
using iTextSharp.text.pdf;

namespace Shane32.EasyPDF
{
    public partial class PDFWriter
    {
        /// <summary>
        /// Default box size for QR codes in points (0.03" dot pitch)
        /// </summary>
        private const float DEFAULT_BOX_SIZE = 72f / 33;

        /// <summary>
        /// Default barcode height in points (0.5")
        /// </summary>
        private const float DEFAULT_BARCODE_HEIGHT = 72f * 0.5f;

        /// <summary>
        /// Prints a QR code in the color specified by <see cref="FillColor"/> in the current
        /// position, adjusted based on the <see cref="PictureAlignment"/> setting.
        /// If <paramref name="size"/> is not specified, QR code prints with 0.03" dot pitch;
        /// otherwise the overall size of the QR code is the specified size.
        /// The border, if enabled, prints with the color specified by <see cref="ForeColor"/>
        /// and current line style selections.
        /// </summary>
        public PDFWriter QRCode(QRCoder.QRCodeData data, float? size = null, bool quietZone = true)
        {
            int quietBorder = 0;
            if (!quietZone) {
                quietBorder = data.ModuleMatrix.Count(row => {
                    for (int col = 0; col < row.Length; col++) {
                        if (row[col])
                            return false;
                    }
                    return true;
                }) / 2;
            }

            var count = data.ModuleMatrix.Count - quietBorder - quietBorder;
            var x = CurrentX;
            var y = CurrentY;
            var boxSize = size.HasValue ? size.Value / count : _TranslateRev(DEFAULT_BOX_SIZE);
            size ??= boxSize * count;

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

            for (int row = quietBorder; row < (count + quietBorder); row++) {
                var rowData = data.ModuleMatrix[row];
                for (int col = quietBorder; col < (count + quietBorder); col++) {
                    if (rowData[col]) {
                        MoveTo(x + (col - quietBorder) * boxSize, y + (row - quietBorder) * boxSize);
                        Rectangle(boxSize, boxSize, 0, true, false);
                    }
                }
            }

            MoveTo(x + size.Value, y + size.Value);
            //if (border) {
            //    MoveTo(x, y);
            //    Rectangle(size.Value, size.Value);
            //} else {
            //    MoveTo(x + size.Value, y + size.Value);
            //}

            return this;
        }

        /// <inheritdoc cref="QRCode(QRCoder.QRCodeData, float?, bool)"/>
        public PDFWriter QRCode(string code, QRCoder.QRCodeGenerator.ECCLevel eccLevel = QRCoder.QRCodeGenerator.ECCLevel.L, float? size = null, bool quietZone = true)
        {
            var generator = new QRCoder.QRCodeGenerator();
            var qrCode = generator.CreateQrCode(code, eccLevel);
            return QRCode(qrCode, size, quietZone);
        }

        /// <summary>
        /// Returns the default size of the specified QR code (uses 0.03" dot pitch).
        /// </summary>
        public float QRCodeSize(QRCoder.QRCodeData code, bool quietZone = true)
        {
            int quietBorder = 0;
            if (!quietZone) {
                quietBorder = code.ModuleMatrix.Count(row => {
                    for (int col = 0; col < row.Length; col++) {
                        if (row[col])
                            return false;
                    }
                    return true;
                }) / 2;
            }

            return _TranslateRev(DEFAULT_BOX_SIZE * (code.ModuleMatrix.Count - quietBorder - quietBorder));
        }

        /// <inheritdoc cref="QRCodeSize(QRCoder.QRCodeData, bool)"/>
        public float QRCodeSize(string code, QRCoder.QRCodeGenerator.ECCLevel eccLevel = QRCoder.QRCodeGenerator.ECCLevel.L, bool quietZone = true)
        {
            var generator = new QRCoder.QRCodeGenerator();
            var qrCode = generator.CreateQrCode(code, eccLevel);
            return QRCodeSize(qrCode, quietZone);
        }

        /// <summary>
        /// Prints a CODE-128 barcode in the selected <see cref="FillColor"/> at the current position, adjusted
        /// depending on the <see cref="PictureAlignment"/> value.
        /// </summary>
        public PDFWriter Barcode(string text, BarcodeType type = BarcodeType.Code128, float? width = null, float? height = null)
        {
            if (type != BarcodeType.Code128)
                throw new ArgumentOutOfRangeException(nameof(type));

            FinishLineAndUpdateLineStyle();
            var x = CurrentX;
            var y = CurrentY;
            width ??= BarcodeSize(text, type);
            height ??= _TranslateRev(DEFAULT_BARCODE_HEIGHT);

            switch (PictureAlignment) {
                case PictureAlignment.LeftTop:
                case PictureAlignment.LeftCenter:
                case PictureAlignment.LeftBottom:
                    break;
                case PictureAlignment.CenterTop:
                case PictureAlignment.CenterCenter:
                case PictureAlignment.CenterBottom:
                    x -= width.Value / 2f;
                    break;
                case PictureAlignment.RightTop:
                case PictureAlignment.RightCenter:
                case PictureAlignment.RightBottom:
                    x -= width.Value;
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
                    y -= height.Value / 2f;
                    break;
                case PictureAlignment.LeftBottom:
                case PictureAlignment.CenterBottom:
                case PictureAlignment.RightBottom:
                    y -= height.Value;
                    break;
            }

            var c = new Barcode128();
            c.Code = text;
            c.CodeType = Barcode128.CODE128;
            c.Font = null;
            c.BarHeight = _Translate(height.Value);
            _content.SaveState();
            try {
                var matrix = new System.Drawing.Drawing2D.Matrix();
                matrix.Translate(_Translate(x), _Translate(y));
                if (width.HasValue)
                    matrix.Scale(width.Value / BarcodeSize(text), 1f);
                _content.Transform(matrix);
                c.PlaceBarcode(_content, null, null);
                _currentX = x + width.Value;
                _currentY = y + height.Value;
            }
            finally {
                _content.RestoreState();
            }

            return this;
        }

        /// <summary>
        /// Returns the default width of the specified barcode.
        /// </summary>
        public float BarcodeSize(string text, BarcodeType type = BarcodeType.Code128)
        {
            if (type != BarcodeType.Code128)
                throw new ArgumentOutOfRangeException(nameof(type));

            var c = new Barcode128();
            c.Code = text;
            c.CodeType = Barcode128.CODE128;
            c.Font = null;
            return _TranslateRev(c.BarcodeSize.Width);
        }
    }
}
