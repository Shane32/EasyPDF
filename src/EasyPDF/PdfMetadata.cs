using System.Globalization;
using iTextSharp.text.pdf;

namespace Shane32.EasyPDF;

/// <summary>
/// Represents the metadata of a PDF document.
/// </summary>
public class PdfMetadata
{
    /// <summary>
    /// Gets or sets the title of the PDF document.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the author of the PDF document.
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    /// Gets or sets the subject of the PDF document.
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// Gets or sets the keywords associated with the PDF document.
    /// </summary>
    public string? Keywords { get; set; }

    /// <summary>
    /// Gets or sets the creator of the PDF document.
    /// </summary>
    /// <remarks>
    /// If the document was converted into a PDF document from another
    /// form, this is the name of the application that created the original document.
    /// </remarks>
    public string? Creator { get; set; }

    /// <summary>
    /// Gets or sets the creation date of the PDF document.
    /// </summary>
    public DateTime? CreationDate { get; set; }

    /// <summary>
    /// Gets or sets the last modification date of the PDF document.
    /// </summary>
    public DateTime? ModificationDate { get; set; }

    /// <summary>
    /// Gets or sets the producer of the PDF document.
    /// </summary>
    public string? Producer { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfMetadata"/> class.
    /// </summary>
    /// <remarks>
    /// Sets the creation date and modification date to the current date and time.
    /// </remarks>
    internal PdfMetadata()
    {
        CreationDate = DateTime.Now;
        ModificationDate = CreationDate;
    }

    internal void Save(PdfDictionary info)
    {
        if (Title != null)
            info.Put(PdfName.Title, new PdfString(Title, PdfObject.TEXT_UNICODE));
        if (Author != null)
            info.Put(PdfName.Author, new PdfString(Author, PdfObject.TEXT_UNICODE));

        if (Subject != null)
            info.Put(PdfName.Subject, new PdfString(Subject, PdfObject.TEXT_UNICODE));

        if (Keywords != null)
            info.Put(PdfName.Keywords, new PdfString(Keywords, PdfObject.TEXT_UNICODE));

        if (Creator != null)
            info.Put(PdfName.Creator, new PdfString(Creator, PdfObject.TEXT_UNICODE));

        if (CreationDate.HasValue)
            info.Put(PdfName.Creationdate, PdfDate(CreationDate.Value));

        if (ModificationDate.HasValue)
            info.Put(PdfName.Moddate, PdfDate(ModificationDate.Value));

        if (Producer != null)
            info.Put(PdfName.Producer, new PdfString(Producer, PdfObject.TEXT_UNICODE));

        static PdfString PdfDate(DateTime d)
        {
            var ret = d.ToString("\\D\\:yyyyMMddHHmmss", DateTimeFormatInfo.InvariantInfo);
            var timezone = d.Kind == DateTimeKind.Utc ? "+00'00" : d.ToString("zzz", DateTimeFormatInfo.InvariantInfo).Replace(':','\'');
            ret += timezone + "'";
            return new PdfString(ret, PdfObject.TEXT_UNICODE);
        }
    }
}
