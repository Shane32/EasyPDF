using System.Drawing;
using System.Drawing.Printing;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Versioning;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextPdfWriter = iTextSharp.text.pdf.PdfWriter;

namespace Shane32.EasyPDF;

/// <summary>
/// Creates a PDF page and writes it to a stream or byte array.
/// </summary>
public partial class PDFWriter : IDisposable
{
    private readonly Stream _stream;
    private readonly bool _privateStream;
    private readonly bool _disposeStream;
    private Document? _document;
    private iTextPdfWriter? _writer;
    private PdfMetadata? _metadata;
    private PdfContentByte? _content2;
    private PdfStamper? _stamper;
    private PdfContentByte _content => _content2 ?? throw new InvalidOperationException("Create a page first!");
    private ScaleModes _scaleMode = ScaleModes.Hundredths;
    private SizeF _pageSize;
    private SizeF _marginSize;
    private PointF _marginOffset;

    static PDFWriter()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        _ = FontFactory.RegisterDirectories();
    }

    /// <summary>
    /// Creates a new document; use <see cref="ToArray"/> to retrieve the PDF data.
    /// </summary>
    public PDFWriter()
    {
        _stream = new MemoryStream();
        _privateStream = true;
    }

    /// <summary>
    /// Creates a new document with the specified filename.
    /// </summary>
    public PDFWriter(string path)
    {
        _stream = System.IO.File.Create(path);
        _disposeStream = true;
    }

    /// <summary>
    /// Creates a new document which will save to the specified stream.
    /// </summary>
    public PDFWriter(Stream stream)
    {
        _stream = stream;
    }

    /// <summary>
    /// Returns the underlying <see cref="PdfContentByte"/>.
    /// </summary>
    public PdfContentByte GetDirectContent() => _content;

    /// <summary>
    /// Returns the underlying <see cref="iTextPdfWriter"/>.
    /// </summary>
    public iTextPdfWriter GetWriter() => _writer ?? throw new InvalidOperationException("Create a page first!");

    /// <summary>
    /// Returns the underlying <see cref="Document"/>.
    /// </summary>
    public Document GetDocument() => _document ?? throw new InvalidOperationException("Create a page first!");
    
    /// <summary>
    /// Gets the metadata associated with the current PDF document.
    /// </summary>
    public PdfMetadata Metadata => _metadata ?? throw new InvalidOperationException("Create a page first!");

    [SupportedOSPlatform("windows")]
    [Obsolete]
    private static PaperSize _GetPaperSize(PaperKind paperKind) => paperKind switch {
        PaperKind.Letter => new PaperSize(paperKind.ToString(), 850, 1100),
        PaperKind.Legal => new PaperSize(paperKind.ToString(), 850, 1400),
        PaperKind.Ledger => new PaperSize(paperKind.ToString(), 1100, 1700),
        _ => throw new NotImplementedException(),
    };

    /// <summary>
    /// Returns the size of the page including margins.
    /// </summary>
    public SizeF PageSize => new SizeF(_TranslateRev(_pageSize.Width), _TranslateRev(_pageSize.Height));

    /// <inheritdoc cref="NewPage(float, float, bool, float, float, float?, float?)"/>
    [SupportedOSPlatform("windows")]
    [Obsolete("Please use the constructor with PageKind instead.")]
    public PDFWriter NewPage(PaperKind paperKind, bool landscape, float marginLeft = 0f, float marginTop = 0f, float? marginRight = null, float? marginBottom = null)
        => NewPage(_GetPaperSize(paperKind), landscape, marginLeft, marginTop, marginRight, marginBottom);

    /// <inheritdoc cref="NewPage(float, float, bool, float, float, float?, float?)"/>
    [SupportedOSPlatform("windows")]
    [Obsolete("Please use the constructor with PageKind instead.")]
    public PDFWriter NewPage(PaperKind paperKind, bool landscape, Margins margins)
        => NewPage(_GetPaperSize(paperKind), landscape, margins);

    /// <inheritdoc cref="NewPage(float, float, bool, float, float, float?, float?)"/>
    [SupportedOSPlatform("windows")]
    [Obsolete("Please use the constructor with PageKind instead.")]
    public PDFWriter NewPage(PaperKind paperKind, bool landscape, MarginsF margins)
        => NewPage(_GetPaperSize(paperKind), landscape, margins.Left, margins.Top, margins.Right, margins.Bottom);

    /// <summary>
    /// Creates a document or adds a new page to an existing document as the specified size.
    /// </summary>
    public PDFWriter NewPage(float width, float height, bool landscape, float marginLeft = 0f, float marginTop = 0f, float? marginRight = null, float? marginBottom = null)
        => NewPageAbs(_Translate(width), _Translate(height), landscape, _Translate(marginLeft), _Translate(marginTop), _Translate(marginRight ?? marginLeft), _Translate(marginBottom ?? marginTop));

    /// <inheritdoc cref="NewPage(float, float, bool, float, float, float?, float?)"/>
    [SupportedOSPlatform("windows")]
    [Obsolete("Please use the constructor with SizeF and MarginsF instead.")]
    public PDFWriter NewPage(PaperSize paperSize, bool landscape, float marginLeft = 0f, float marginTop = 0f, float? marginRight = null, float? marginBottom = null)
        => NewPageAbs(_Translate(paperSize.Width, ScaleModes.Hundredths), _Translate(paperSize.Height, ScaleModes.Hundredths), landscape, _Translate(marginLeft), _Translate(marginTop), _Translate(marginRight ?? marginLeft), _Translate(marginBottom ?? marginTop));

    /// <inheritdoc cref="NewPage(float, float, bool, float, float, float?, float?)"/>
    [SupportedOSPlatform("windows")]
    [Obsolete("Please use the constructor with SizeF and MarginsF instead.")]
    public PDFWriter NewPage(PaperSize paperSize, bool landscape, Margins margins)
        => NewPageAbs(_Translate(paperSize.Width, ScaleModes.Hundredths), _Translate(paperSize.Height, ScaleModes.Hundredths), landscape, _Translate(margins.Left, ScaleModes.Hundredths), _Translate(margins.Top, ScaleModes.Hundredths), _Translate(margins.Right, ScaleModes.Hundredths), _Translate(margins.Bottom, ScaleModes.Hundredths));

    /// <inheritdoc cref="NewPage(float, float, bool, float, float, float?, float?)"/>
    public PDFWriter NewPage(SizeF pageSize, bool landscape, MarginsF margins)
        => NewPageAbs(_Translate(pageSize.Width), _Translate(pageSize.Height), landscape, _Translate(margins.Left), _Translate(margins.Top), _Translate(margins.Right), _Translate(margins.Bottom));

    /// <inheritdoc cref="NewPage(float, float, bool, float, float, float?, float?)"/>
    public PDFWriter NewPage(PageKind pageKind, bool landscape, MarginsF margins)
    {
        var sizePoints = pageKind.GetSizeInPoints();
        return NewPageAbs(sizePoints.Width, sizePoints.Height, landscape, _Translate(margins.Left), _Translate(margins.Top), _Translate(margins.Right), _Translate(margins.Bottom));
    }

    /// <inheritdoc cref="NewPage(float, float, bool, float, float, float?, float?)"/>
    public PDFWriter NewPage(PageKind pageKind, bool landscape, float marginLeft = 0f, float marginTop = 0f, float? marginRight = null, float? marginBottom = null)
        => NewPage(pageKind, landscape, new(marginLeft, marginTop, marginRight ?? marginLeft, marginBottom ?? marginTop));

    /// <summary>
    /// Allows for editing a existing pdf; will have to save under a different file name
    /// </summary>
    public void AnnotatePage(string originalFile)
    {
        var reader = new PdfReader(originalFile);
        //todo: is the file stream held open by the PdfReader?
        //  if so, it should be disposed by Close/Dispose
        //  if not, we're good
        AnnotatePage(reader);
    }

    /// <inheritdoc cref="AnnotatePage(string)"/>
    public void AnnotatePage(PdfReader reader)
    {
        var stamper = new PdfStamper(reader, _stream);
        _content2 = stamper.GetOverContent(1);

        var rect = reader.GetPageSizeWithRotation(1);
        _content.ConcatCtm(1, 0, 0, -1, 0, rect.Height);

        _stamper = stamper;
    }

    /// <summary>
    /// Creates a new PDF page with the specified dimensions and margins and returns a PDFWriter object
    /// </summary>
    /// <param name="pageWidth">Width of the page in points</param>
    /// <param name="pageHeight">Height of the page in points</param>
    /// <param name="landscape">Whether the page is in landscape orientation</param>
    /// <param name="marginLeft">Left margin of the page in points</param>
    /// <param name="marginTop">Top margin of the page in points</param>
    /// <param name="marginRight">Right margin of the page in points, or null to mirror left margin</param>
    /// <param name="marginBottom">Bottom margin of the page in points, or null to mirror top margin</param>
    private PDFWriter NewPageAbs(float pageWidth, float pageHeight, bool landscape, float marginLeft = 0f, float marginTop = 0f, float? marginRight = null, float? marginBottom = null)
    {
        // Set the right and bottom margins to be the same as the left and top margins, respectively,
        // if they were not explicitly specified
        marginRight ??= marginLeft;
        marginBottom ??= marginTop;

        // Finish any pending line operations
        FinishLine();

        // If the document has not been initialized yet,
        // create a new Document object and set up a PDFWriter and DirectContent objects
        if (_document is null) {
            // If the page is in landscape orientation, create a rotated Rectangle object
            if (landscape) {
                var rotated = new iTextSharp.text.Rectangle(pageWidth, pageHeight).Rotate();
                _document = new Document(rotated);
            } else {
                _document = new Document(new iTextSharp.text.Rectangle(pageWidth, pageHeight));
            }
            _writer = iTextPdfWriter.GetInstance(_document, _stream);
            _metadata = new();
            var producer = _writer.Info.Get(PdfName.Producer);
            if (producer != null && producer.IsString() && producer is PdfString producerString) {
                _metadata.Producer = producerString.ToUnicodeString();
            }
            _writer.Info.Remove(PdfName.Producer);
            _writer.Info.Remove(PdfName.Creationdate);
            _writer.Info.Remove(PdfName.Moddate);
            _document.Open();
            _content2 = _writer.DirectContent;
        }
        // If the document has already been initialized,
        // set the page size to the specified dimensions and create a new page
        else {
            if (landscape) {
                _ = _document.SetPageSize(new iTextSharp.text.Rectangle(pageWidth, pageHeight).Rotate());
            } else {
                _ = _document.SetPageSize(new iTextSharp.text.Rectangle(pageWidth, pageHeight));
            }
            _ = _document.NewPage();
        }

        // Ensure that the page is added to the document
        _writer!.PageEmpty = false;

        // Set the pageSize field to the specified dimensions, taking into account the landscape orientation
        if (landscape) {
            _pageSize = new SizeF(pageHeight, pageWidth);
        } else {
            _pageSize = new SizeF(pageWidth, pageHeight);
        }

        // Concatenate the current transformation matrix (CTM) with a new matrix that translates the origin to the
        // specified margin location and flips the y-axis orientation.  This is necessary so that (0, 0) represents
        // the top-left corner of the page rather than the bottom-left corner.
        _content.ConcatCtm(1, 0, 0, -1, marginLeft, _pageSize.Height - marginTop);

        // Set the marginOffset and marginSize fields based on the specified margins
        _marginOffset = new PointF(marginLeft, marginTop);
        _marginSize = new SizeF(_pageSize.Width - marginLeft - marginRight.Value, _pageSize.Height - marginTop - marginBottom.Value);

        // Reset line-related variables within the PDF
        _InitLineVars();

        // Set the current (x, y) coordinates to (0, 0)
        _currentX = 0;
        _currentY = 0;

        // Return the current PDFWriter object
        return this;
    }

    /// <summary>
    /// When using the default constructor, this closes the document and returns the PDF data as a byte array.
    /// Cannot be used with other constructors.
    /// </summary>
    public byte[] ToArray() => ((MemoryStream)ToStream()).ToArray();

    /// <summary>
    /// When using the default constructor, this closes the document and returns the PDF data as a <see cref="Stream"/>.
    /// Cannot be used with other constructors.
    /// </summary>
    public Stream ToStream()
    {
        if (!_privateStream)
            throw new InvalidOperationException("This method is only available when using the default constructor.");
        Close();
        _stream.Position = 0;
        return _stream;
    }

    /// <summary>
    /// Prints to the specified network-attached printer; only supported when the printer supports PDF printing.
    /// </summary>
    public async Task PrintAsync(string host, int port = 9100, CancellationToken token = default)
    {
        if (!_privateStream)
            throw new InvalidOperationException("This method is only available when using the default constructor.");
        Close();

        using var tcpClient = new TcpClient();
#if NET5_0_OR_GREATER
        await tcpClient.ConnectAsync(host, port, token);
#else
        await tcpClient.ConnectAsync(host, port);
#endif
        using var stream = tcpClient.GetStream();
        _stream.Position = 0;
        await _stream.CopyToAsync(stream, 81920, token);
        await stream.FlushAsync(token);
    }

    /// <summary>
    /// Prints to the specified network-attached printer; only supported when the printer supports PDF printing.
    /// </summary>
    public async Task PrintAsync(IPAddress address, int port = 9100, CancellationToken token = default)
    {
        if (!_privateStream)
            throw new InvalidOperationException("This method is only available when using the default constructor.");
        Close();

        using var tcpClient = new TcpClient();
#if NET5_0_OR_GREATER
        await tcpClient.ConnectAsync(address, port, token);
#else
        await tcpClient.ConnectAsync(address, port);
#endif
        using var stream = tcpClient.GetStream();
        _stream.Position = 0;
        await _stream.CopyToAsync(stream, 81920, token);
        await stream.FlushAsync(token);
    }

    /// <summary>
    /// Closes the document and finishes writing it to the underlying stream.
    /// </summary>
    public void Close()
    {
        if (_content2 != null) {
            FinishLineAndUpdateLineStyle();
            _content2 = null;
        }

        if (_metadata != null && _writer != null) {
            _metadata.Save(_writer.Info);
        }
        _metadata = null;

        if (_document != null) {
            _document.Close();
            _document = null;
        }

        if (_writer != null) {
            _writer.Close();
            _writer = null;
        }

        if (_stamper != null) {
            _stamper.Close();
            _stamper = null;
        }

        if (_disposeStream && _stream != null) {
            _stream.Dispose();
        }
    }

    /// <summary>
    /// Disposes of the underlying stream if necessary.
    /// Use <see cref="Close"/> to save the file data prior to closing.
    /// </summary>
    void IDisposable.Dispose()
    {
        if (_disposeStream)
            _stream.Dispose();
        GC.SuppressFinalize(this);
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
                    throw new ArgumentOutOfRangeException(nameof(value));
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
                return num * 0.72f;
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
            ScaleModes.Hundredths => num / 0.72f,
            ScaleModes.Inches => num / 72f,
            _ => throw new ArgumentOutOfRangeException(nameof(scaleMode)),
        };
    }

    /// <summary>
    /// Returns the size of the page excluding margins.
    /// </summary>
    public SizeF Size => new(_TranslateRev(_marginSize.Width), _TranslateRev(_marginSize.Height));

    /// <summary>
    /// Gets or sets the margins of the current page.
    /// </summary>
    public MarginsF Margins {
        get => new(
            _TranslateRev(_marginOffset.X),
            _TranslateRev(_marginOffset.Y),
            _TranslateRev(_pageSize.Width - _marginSize.Width - _marginOffset.X),
            _TranslateRev(_pageSize.Height - _marginSize.Height - _marginOffset.Y));
        set {
            var newMarginOffset = new PointF(_Translate(value.Left), _Translate(value.Top));
            var difference = new PointF(newMarginOffset.X - _marginOffset.X, newMarginOffset.Y - _marginOffset.Y);
            if (difference.X != 0 && difference.Y != 0) {
                _content.ConcatCtm(1f, 0f, 0f, 1f, difference.X, difference.Y);
                _marginOffset = newMarginOffset;
            }

            _marginSize = new SizeF(_pageSize.Width - newMarginOffset.X - _Translate(value.Right), _pageSize.Height - newMarginOffset.Y - _Translate(value.Bottom));
        }
    }

    /// <summary>
    /// Returns the top and left margin offsets on the page.
    /// </summary>
    public PointF MarginOffset => new(_TranslateRev(_marginOffset.X), _TranslateRev(_marginOffset.Y));

    /// <summary>
    /// Offsets the margins on the current page, as measured in the current scale mode.
    /// </summary>
    public PDFWriter OffsetMargins(float left, float top, float right = 0f, float bottom = 0f)
    {
        var margins = Margins;
        Margins = new MarginsF(margins.Left + left, margins.Top + top, margins.Right + right, margins.Bottom + bottom);
        return this;
    }

    /// <summary>
    /// Saves the current state of the graphics context (fonts, colors, position, scale mode, etc).
    /// Dispose the returned instance to restore the graphics context.
    /// </summary>
    public IDisposable SaveState() => new SaveState(this);
}
