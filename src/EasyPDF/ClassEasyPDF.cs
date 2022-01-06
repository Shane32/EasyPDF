using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextColor = iTextSharp.text.BaseColor;
using iTextFont = iTextSharp.text.Font;
using iTextPdfWriter = iTextSharp.text.pdf.PdfWriter;

namespace Shane32.EasyPDF
{
    public partial class PDFWriter
    {
        private readonly Stream _stream;
        private readonly bool _privateStream;
        private Document? _document;
        private iTextPdfWriter? _writer;
        private PdfContentByte? _content2;
        private PdfStamper? _stamper;
        private PdfContentByte _content => _content2 ?? throw new InvalidOperationException("Create a page first!");
        private ScaleModes _scaleMode = ScaleModes.Hundredths;
        private SizeF _pageSize;

        static PDFWriter()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _ = FontFactory.RegisterDirectories();
        }

        /// <summary>
        /// Creates a document for writing to
        /// </summary>
        public PDFWriter()
        {
            _stream = new MemoryStream();
            _privateStream = true;
        }

        /// <inheritdoc cref="PDFWriter(string)"/>
        public PDFWriter(string path)
        {
            _stream = System.IO.File.Create(path);
        }

        /// <inheritdoc cref="PDFWriter(Stream)"/>
        public PDFWriter(Stream s)
        {
            _stream = s;
        }

        public PdfContentByte GetDirectContent() => _content;

        public iTextPdfWriter GetWriter() => _writer ?? throw new InvalidOperationException("Create a page first!");

        public Document GetDocument() => _document ?? throw new InvalidOperationException("Create a page first!");

        private PaperSize _GetPaperSize(PaperKind paperKind) => paperKind switch {
            PaperKind.Letter => new PaperSize(paperKind.ToString(), 850, 1100),
            PaperKind.Legal => new PaperSize(paperKind.ToString(), 850, 1400),
            PaperKind.Ledger => new PaperSize(paperKind.ToString(), 1100, 1700),
            _ => throw new NotImplementedException(),
        };

        public SizeF PageSize => new SizeF(_TranslateRev(_pageSize.Width), _TranslateRev(_pageSize.Height));

        /// <summary>
        /// Creates a document or adds a new page to an existing document as the specified size.
        /// </summary>
        public void NewPage(PaperKind paperKind, bool landscape)
            => NewPage(_GetPaperSize(paperKind), landscape);

        /// <inheritdoc cref="NewPage(PaperKind, bool)"/>
        public void NewPage(PaperKind paperKind, bool landscape, float marginLeft, float marginTop)
            => NewPage(_GetPaperSize(paperKind), landscape, marginLeft, marginTop);

        /// <inheritdoc cref="NewPage(PaperKind, bool)"/>
        public void NewPage(PaperKind paperKind, bool landscape, Margins margins)
            => NewPage(_GetPaperSize(paperKind), landscape, margins);

        /// <inheritdoc cref="NewPage(PaperKind, bool)"/>
        public void NewPage(float width, float height, bool landscape)
            => NewPageAbs(_Translate(width), _Translate(height), 0f, 0f, landscape);

        /// <inheritdoc cref="NewPage(PaperKind, bool)"/>
        public void NewPage(float width, float height, bool landscape, float marginLeft, float marginTop)
            => NewPageAbs(_Translate(width), _Translate(height), _Translate(marginLeft), _Translate(marginTop), landscape);

        /// <inheritdoc cref="NewPage(PaperKind, bool)"/>
        public void NewPage(PaperSize paperSize, bool landscape)
            => NewPageAbs(_Translate(paperSize.Width, ScaleModes.Hundredths), _Translate(paperSize.Height, ScaleModes.Hundredths), 0, 0, landscape);

        /// <inheritdoc cref="NewPage(PaperKind, bool)"/>
        public void NewPage(PaperSize paperSize, bool landscape, float marginLeft, float marginTop)
            => NewPageAbs(_Translate(paperSize.Width, ScaleModes.Hundredths), _Translate(paperSize.Height, ScaleModes.Hundredths), _Translate(marginLeft), _Translate(marginTop), landscape);

        /// <inheritdoc cref="NewPage(PaperKind, bool)"/>
        public void NewPage(PaperSize paperSize, bool landscape, Margins margins)
            => NewPageAbs(_Translate(paperSize.Width, ScaleModes.Hundredths), _Translate(paperSize.Height, ScaleModes.Hundredths), _Translate(margins.Left, ScaleModes.Hundredths), _Translate(margins.Top, ScaleModes.Hundredths), landscape);

        /// <summary>
        /// Allows for Editing a existing pdf will half to save under a different file name
        /// </summary>
        public void AnnotatePage(string originalFile)
        {
            var reader = new PdfReader(originalFile);
            AnnotatePage(reader);
        }

        /// <inheritdoc cref="AnnotatePage(PdfReader)"/>
        public void AnnotatePage(PdfReader reader)
        {
            var stamper = new PdfStamper(reader, _stream);
            _content2 = stamper.GetOverContent(1);

            var rect = reader.GetPageSizeWithRotation(1);
            _content.ConcatCtm(1, 0, 0, -1, 0, rect.Height);

            _stamper = stamper;
        }

        private void NewPageAbs(float pageWidth, float pageHeight, float marginLeft, float marginTop, bool landscape)
        {
            FinishLine();
            if (_document is null) {
                if (landscape) {
                    var rotated = new iTextSharp.text.Rectangle(pageWidth, pageHeight).Rotate();
                    _document = new Document(rotated);
                } else {
                    _document = new Document(new iTextSharp.text.Rectangle(pageWidth, pageHeight));
                }

                _writer = iTextPdfWriter.GetInstance(_document, _stream);
                _document.Open();
                _content2 = _writer.DirectContent;
            } else {
                if (landscape) {
                    _ = _document.SetPageSize(new iTextSharp.text.Rectangle(pageWidth, pageHeight).Rotate());
                } else {
                    _ = _document.SetPageSize(new iTextSharp.text.Rectangle(pageWidth, pageHeight));
                }
                _ = _document.NewPage();
            }

            _writer!.PageEmpty = false;
            if (landscape) {
                _content.ConcatCtm(1, 0, 0, -1, marginLeft, pageWidth - marginTop);
                _pageSize = new SizeF(pageHeight, pageWidth);
            } else {
                _content.ConcatCtm(1, 0, 0, -1, marginLeft, pageHeight - marginTop);
                _pageSize = new SizeF(pageWidth, pageHeight);
            }

            _InitLineVars();
            _currentX = 0;
            _currentY = 0;
        }

        /// <summary>
        /// When using the default constructor, this closes the document and returns the PDF data as a byte array.
        /// Cannot be used with other constructors.
        /// </summary>
        public byte[] ToArray()
        {
            if (!_privateStream)
                throw new InvalidOperationException("This method is only available when using the default constructor.");
            Close();
            return ((MemoryStream)_stream).ToArray();
        }

        /// <summary>
        /// Closes the document and finishes writing it to the underlying stream.
        /// </summary>
        public void Close()
        {
            if (_content2 != null) {
                FinishLine();
                _content2 = null;
            }
            if (_document != null) {
                _document.Close();
                _document = null;
            }

            if (_writer != null) {
                _writer.Close();
                _writer = null;
            }

            if (_stamper is object) {
                _stamper.Close();
                _stamper = null;
            }
        }


        /// <summary>
        /// Gets or sets the scale of coordinate system.
        /// </summary>
        public ScaleModes ScaleMode {
            get => _scaleMode;

            set {
                switch (value) {
                    case ScaleModes.Hundredths:
                    case ScaleModes.Inches:
                    case ScaleModes.Points:
                        _scaleMode = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Registers a specific font file to be able to be used while creating PDFs.
        /// </summary>
        public static void RegisterFont(string name, string path)
        {
            if (!FontFactory.IsRegistered(name)) {
                FontFactory.Register(path, name);
            }
        }

        /// <summary>
        /// Translates a value from the selected scale mode to points.
        /// </summary>
        private float _Translate(float num) => _Translate(num, _scaleMode);

        /// <summary>
        /// Translates a value from a specified scale mode to points.
        /// </summary>
        private static float _Translate(float num, ScaleModes scaleMode)
        {
            switch (scaleMode) {
                case ScaleModes.Points: {
                    return num;
                }

                case ScaleModes.Hundredths: {
                    return num * 72f / 100f;
                }

                case ScaleModes.Inches: {
                    return num * 72f;
                }

                default: {
                    throw new ArgumentOutOfRangeException(nameof(scaleMode));
                }
            }
        }

        /// <summary>
        /// Translates a value from points to the selected scale mode.
        /// </summary>
        private float _TranslateRev(float num) => _TranslateRev(num, ScaleMode);

        /// <summary>
        /// Translates a value from points to a specified scale mode.
        /// </summary>
        private static float _TranslateRev(float num, ScaleModes scaleMode)
        {
            return scaleMode switch {
                ScaleModes.Points => num,
                ScaleModes.Hundredths => num * 100f / 72f,
                ScaleModes.Inches => num / 72f,
                _ => throw new ArgumentOutOfRangeException(nameof(scaleMode)),
            };
        }

        /// <summary>
        /// Converts a <see cref="Color"/> to <see cref="iTextColor"/>.
        /// </summary>
        private static iTextColor _GetColor(Color color)
            => new iTextColor(color.R, color.G, color.B, color.A);
    }
}
