using System.Drawing;

namespace Shane32.EasyPDF;

/// <summary>
/// Represents a specific page size.
/// </summary>
public enum PageKind
{
    /// <summary>
    /// Letter paper (8.5 in. x 11 in.).
    /// </summary>
    Letter = 1,

    /// <summary>
    /// Legal paper (8.5 in. x 14 in.).
    /// </summary>
    Legal = 2,

    /// <summary>
    /// Ledger paper (11 in. x 17 in.).
    /// </summary>
    Ledger = 3,
}

internal static class PageSizeExtensions
{
    public static (float Width, float Height) GetSizeInPoints(this PageKind kind)
    {
        return kind switch {
            PageKind.Letter => (8.5f * 72f, 11f * 72f),
            PageKind.Legal => (8.5f * 72f, 14f * 72f),
            PageKind.Ledger => (11f * 72f, 17f * 72f),
            _ => throw new ArgumentOutOfRangeException(nameof(kind)),
        };
    }
}
