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
pdf.WriteLine("The quick brown fox jumps over the lazy dog");
```

5. Save the result and/or close the document

```cs
var data = pdf.ToArray();   // supported only by the default constructor
pdf.Close();
```

## Properties

| Property                 | Description |
|--------------------------|-------------|
| CurrentX                 | The current X position |
| CurrentY                 | The current Y position |
| FillColor                | The color for filling polygons and drawing barcodes |
| Font                     | The font name, size, and style used when writing text |
| Font.FamilyName          | The font family name |
| Font.Embedded            | Indicates if the font is embedded into the document; false if it is a built-in font |
| Font.FontStyle           | A flag enumeration value representing the bold, italic, underline and strikeout properties |
| Font.Size                | The size of the font measured in points |
| Font.Bold                | Specifies if the font has the Bold style |
| Font.Italic              | Specifies if the font has the Italic style |
| Font.Underline           | Specifies if the font has the Underline style |
| Font.Strikeout           | Specifies if the font has the Strikeout style |
| Font.Justify             | Specifies if the text is justified when word wrapping |
| Font.LineSpacing         | A multiplier to adjust line spacing; defaults to 1.0 |
| Font.HangingIndent       | The amount of indent for word-wrapped lines |
| Font.ParagraphSpacing    | The amount of space between paragraphs measured in points |
| Font.StretchX            | The amount that text is stretched along the X axis |
| Font.StretchY            | The amount that text is stretched along the Y axis |
| ForeColor                | The color for printing text, lines and borders |
| LineStyle                | The style used to draw lines and borders |
| LineStyle.CapStyle       | The style used to draw the end of a line |
| LineStyle.DashStyle      | The dash style used to draw lines and borders |
| LineStyle.JoinStyle      | The style used to draw joined line segments and borders |
| LineStyle.Width          | The pen width when drawing lines and borders |
| MarginOffset             | Returns the offset of the left and top margins |
| PageSize                 | Returns the size of the current page including margins |
| PictureAlignment         | The alignment of pictures and barcodes in relation to the current position |
| Position                 | The current position |
| ScaleMode                | The scaling mode for the coordinates used by all other commands |
| Size                     | Returns the size of the current page excluding margins |
| TextAlignment            | The alignment of text in relation to the current position |

Note that drawing coordinates are always specified or returned based on the current
`ScaleMode` setting, and coordinate (0, 0) is the top-left corner of the page/margin.

## Methods

Note that most methods may be chained, allowing to adjust position and execute a command in one line of code.

Example:

```cs
pdf.MoveTo(1, 1).WriteLine("Hello there!");
pdf.OffsetTo(0, 0.5f).WriteLine("Added a half inch gap between the lines");
```

## Text drawing commands

| Method           | Description |
|------------------|-------------|
| RegisterFont     | Registers a font contained within a file to be able to be used                   |
| TextAscent       | Returns the distance between the baseline and the top of the highest letters     |
| TextCapHeight    | Returns the distance between the baseline and the top of capital letters         |
| TextDescent      | Returns the distance between the baseline and the bottom of the lowest letters (j's, etc) |
| TextHeight       | Returns the height of a single line of text, including space between rows (ascent + descent + leading), adjusting for LineSpacing and StretchY |
| TextLeading      | Returns the amount of additional line spacing besides the ascent and descent, adjusting for LineSpacing |
| TextWidth        | Returns the width of the specified text, adjusted for StretchX                                          |
| Write            | Writes one or more lines of text, leaving the current position positioned after the last character printed |
| WriteLine        | Writes one or more lines of text, leaving the current position below the last line of text |

Examples below:

```cs
// Write "Hello world!" and advance to the next line
pdf.WriteLine("Hello world!");

// Write "Hello world!" except with "world" in red
pdf.Write("Hello ");
pdf.ForeColor = Color.Red;
pdf.Write("world");
pdf.ForeColor = Color.Black;
pdf.WriteLine("!");
pdf.CurrentX = 0;  // reposition cursor; otherwise it would be underneath the exclamation mark

// Center a header on the page
pdf.TextAlignment = TextAlignment.CenterTop;
pdf.Font = new Font(StandardFonts.Times, 20);
pdf.MoveTo(pdf.Size.Width / 2, 0.5f).WriteLine("Hello there!");

// Write a paragraph of text over 4" of horizontal space with justification
pdf.Font.Justify = true;
pdf.WriteLine("Hello there!  The quick brown fox jumps over the lazy dog.  Did you know that?", 4);
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
| LineTo              | Starts or continues a line or polygon to another specified coordinate |
| Rectangle           | Draws a rectangle of the specified size, optionally filling it (with/without border) and/or rounding the corners |
| RectangleDualOffset | Draws a rectangle of the specified size, and then draws another rectangle inset by a specified amount |

Note that once a line or polygon has begun, any call to a method other than `LineTo`, `CornerTo`,
`BezierTo`, `FinishLine` or `FinishPolygon` will implicitly call `FinishLine` to draw the line on the page.

Lines will be 0.1 points in width if not specified by the `LineStyle.Width` property.

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
