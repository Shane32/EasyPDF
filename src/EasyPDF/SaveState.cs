using System.Drawing;

namespace Shane32.EasyPDF;

internal class SaveState : IDisposable
{
    private PDFWriter? _writer;
    private readonly Font _font;
    private readonly ScaleModes _scaleMode;
    private readonly PointF _position;
    private readonly Color _fillColor;
    private readonly Color _foreColor;
    private readonly LineStyle _lineStyle;
    private readonly PictureAlignment _pictureAlignment;
    private readonly TextAlignment _textAlignment;
    private readonly MarginsF _margins;

    public SaveState(PDFWriter writer)
    {
        _writer = writer;
        _font = writer.Font with { };
        _scaleMode = writer.ScaleMode;
        _position = writer.Position;
        _fillColor = writer.FillColor;
        _foreColor = writer.ForeColor;
        _lineStyle = writer.LineStyle with { };
        _pictureAlignment = writer.PictureAlignment;
        _textAlignment = writer.TextAlignment;
        _margins = writer.Margins;
    }

    public void Dispose()
    {
        var writer = _writer;
        _writer = null;
        if (writer == null)
            return;
        writer.ScaleMode = _scaleMode;
        writer.Position = _position;
        writer.Font = _font;
        writer.ForeColor = _foreColor;
        writer.FillColor = _fillColor;
        writer.LineStyle = _lineStyle;
        writer.PictureAlignment = _pictureAlignment;
        writer.TextAlignment = _textAlignment;
        writer.Margins = _margins;
    }
}
