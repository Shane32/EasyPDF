# Shane32.EasyPDF

[![NuGet](https://img.shields.io/nuget/v/Shane32.EasyPDF.svg)](https://www.nuget.org/packages/Shane32.EasyPDF) [![Coverage Status](https://coveralls.io/repos/github/Shane32/EasyPDF/badge.svg?branch=master)](https://coveralls.io/github/Shane32/EasyPDF?branch=master)

## Basic setup

1. Create a new instance with one of the constructors:

```cs
// Create a PDF file which will be saved to a byte array via ToArray()
var pdf = new PDFWriter();

// Create a PDF file at the specified path on the hard drive
var pdf = new PDFWriter(filename);

// Create a PDF file that will write to the specified Stream
var pdf = new PDFWriter(stream);
```

2. Set the desired scaling mode

```cs
pdf.ScaleMode = ScaleModes.Inches;  // points, inches and hundredths are supported; hundredths are default
```

3. Add a new page via `NewPage()`

```cs
pdf.NewPage(PaperKind.Letter, false);        // create a Letter sized page in portrait mode
pdf.NewPage(11, 17, true);                   // create a 11" x 17" page in landscape mode
pdf.NewPage(PaperKind.Letter, false, 1, 1);  // create a Letter sized page with 1" margins
```

4. Add drawing instructions

```cs
pdf.Write("The quick brown fox jumps over the lazy dog", true);
```

5. Save the result and/or close the document

```cs
var data = pdf.ToArray();   // supported only by the default constructor
pdf.Close();
```

## Properties

| Property         | Description |
|------------------|-------------|
| CurrentX         | The current X position |
| CurrentY         | The current Y position |
| FillColor        | The color for filling polygons and drawing barcodes |
| Font             | The font name, size, and style used when writing text |
| FontAlignment    | The alignment of text in relation to the current position |
| FontLineSpacing  | The line spacing multiple when printing multiple lines of text |
| ForeColor        | The color for printing text, lines and borders |
| LineCap          | The style used to draw the end of a line |
| LineDash         | The dash style used to draw lines and borders |
| LineJoin         | The style used to draw joined line segments and borders |
| LineWidth        | The pen width when drawing lines and borders |
| PageSize         | Returns the size of the current page ignoring margins |
| PictureAlignment | The alignment of pictures and barcodes in relation to the current position |
| Pos              | The current position |
| ScaleMode        | The scaling mode for the coordinates used by all other commands |

Note that drawing coordinates are always specified or returned based on the current
`ScaleMode` setting, and coordinate (0, 0) is the top-left corner of the page/margin.

## Text drawing commands

| Method           | Description |
|------------------|-------------|
| RegisterFont     | Registers a font contained within a file to be able to be used                   |
| TextAscent       | Returns the distance between the baseline and the top of the highest letters     |
| TextCapHeight    | Returns the distance between the baseline and the top of capital letters         |
| TextDescent      | Returns the distance between the baseline and the bottom of the lowest letters (j's, etc) |
| TextHeight       | Returns the height of a single line of text, including space between rows (ascent + descent + leading) |
| TextLeading      | Returns the amount of additional line spacing besides the ascent and descent     |
| TextWidth        | Returns the width of the specified text                                          |
| Write            | Writes one or more lines of text at the current position with no word wrapping   |
| WriteAt          | Writes one or more lines of text at the specified position with no word wrapping |
| WriteLine        | Writes a single line of text, stopping where necessary due to word wrapping      |
| WriteLines       | Writes one or more lines of text at the current position with word wrapping      |
| WriteLinesAt     | Writes one or more lines of text at the specified position with word wrapping    |

Examples below:

```cs
// Write "Hello world!" and advance to the next line
pdf.Write("Hello world!", true);

// Write "Hello world!" except with "world" in red
pdf.Write("Hello ");
pdf.ForeColor = Color.Red;
pdf.Write("world");
pdf.ForeColor = Color.Black;
pdf.Write("!", true);
pdf.CurrentY = 0;  // reposition cursor; otherwise it would be underneath the exclamation mark

// Center a header on the page
pdf.TextAlignment = TextAlignment.CenterTop;
pdf.Font = new Font(StandardFonts.Times, 20);
pdf.WriteAt(false, pdf.PageSize.Width / 2, 0.5f, "Hello there!");

// Write a paragraph of text over 4" of space with no indentation and with justification
pdf.WriteLines(4, 0, 0, "Hello there!  The quick brown fox jumps over the lazy dog.  Did you know that?", true);
```

Note that by default, all system fonts are registered and can be used (embedded) into the PDF.
There are 5 standard fonts available to PDF files which do not require embedding; they are
Times, Helvetica, Courier, Symbol and ZapfDingbats.

## Picture drawing commands

There is a single command, `PaintPicture` which can print a `ITextImage` instance
to the PDF file.  Pictures are positioned according to the `PictureAlignment` property.

## Line drawing commands

| Method              | Description |
|---------------------|-------------|
| BezierTo            | Starts or continues a line or polygon around a bezier curve |
| Circle              | Draws a circle at the specified coordinates |
| CornerTo            | Starts or continues a line or polygon around a rounded corner to another coordinate (draws an arc) |
| FinishLine          | Ends the line and draws it |
| FinishPolygon       | Ends a polygon and draws it |
| Line                | Starts a new line or polygon from a specified coordinate to another specified coordinate |
| LineTo              | Starts or continues a line or polygon to another specified coordinate |
| Rectangle           | Draws a rectangle at the specified coordinates, optionally filling it (with/without border) and/or rounding the corners |
| RectangleDualOffset | Draws a rectangle at the specified coordinates, and then draws another rectangle inset by the specified amount |

Note that once a line or polygon has begun, any call to a method other than `LineTo`, `CornerTo`,
`BezierTo`, `FinishLine` or `FinishPolygon` will implicitly call `FinishLine` to draw the line on the page.

## Barcode / QR code commands

| Method              | Description |
|---------------------|-------------|
| Barcode             | Draws a barcode at the current position of the specified size |
| BarcodeSize         | Returns the default width of a specified barcode              |
| QRCode              | Draws a QR code at the current position of the specified size |
| QRCodeSize          | Returns the default size of a specified QR code               |

Barcodes are positioned according to the `PictureAlignment` property and colored via the `FillColor` property.

## Setup and miscellaneous commands

| Method              | Description |
|---------------------|-------------|
| AnnotatePage        | Can be used to take an existing PDF and overlay drawing actions on top of it |
| Close               | Close/saves the PDF |
| GetDirectContent    | Returns the underlying `iTextSharp.text.pdf.PdfContentByte` which can be used to perform nearly any drawing action on the PDF |
| GetDocument         | Returns the underlying `iTextSharp.text.Document` instance |
| GetWriter           | Returns the underlying `iTextSharp.text.pdf.iTextPdfWriter` instance |
| NewPage             | Creates or appends a new page of the specified size; required before writing to the document |
| PrintAsync          | Prints the page to a specified network-attached printer that supports direct PDF printing |
| ToArray             | Returns the PDF as a byte array |

## Credits

Glory to Jehovah, Lord of Lords and King of Kings, creator of Heaven and Earth, who through his Son Jesus Christ,
has reedemed me to become a child of God. -Shane32
