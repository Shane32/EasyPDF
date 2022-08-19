using iTextSharp.text.pdf;

namespace Shane32.EasyPDF
{
    public partial class PDFWriter
    {
        /// <summary>
        /// Gets or sets the font to use when printing text.
        /// </summary>
        public Font Font { get; set; } = new Font(StandardFonts.Helvetica, 12);

        /// <summary>
        /// Gets or sets the alignment of printed text.
        /// This alignment is relative to the current position when printing.
        /// </summary>
        public TextAlignment TextAlignment { get; set; } = TextAlignment.LeftBaseline;

        /// <summary>
        /// Writes one or more lines of text, with word wrapping if specified.
        /// The current position is set to the position immediately after the last character written.
        /// </summary>
        /// <param name="text">The text to print.</param>
        /// <param name="maxWidth">The maximum width to print before word wrapping.</param>
        public PDFWriter Write(string? text, float? maxWidth = null)
        {
            FinishLineAndUpdateLineStyle();

            if (text == null)
                return this;

            bool lastIndent = false;
            var ret = WriteInternalAndReturnRemaining(text, maxWidth ?? 0f, maxWidth.HasValue, lastIndent);
            while (ret.NextLine != null) {
                ret = WriteInternalAndReturnRemaining(ret.NextLine, maxWidth ?? 0f, maxWidth.HasValue, ret.IndentNextLine);
            }

            return this;
        }

        /// <summary>
        /// Writes one or more lines of text, with word wrapping if specified.
        /// The current position is positioned below the last line of text.
        /// </summary>
        /// <param name="text">The text to print.</param>
        /// <param name="maxWidth">The maximum width to print before word wrapping.</param>
        public PDFWriter WriteLine(string? text = null, float? maxWidth = null)
        {
            if (text == null) {
                _currentY += TextHeightPoints() + Font.ParagraphSpacing;
                return this;
            }
            Write(text + "\r\n", maxWidth);
            return this;
        }

        /// <summary>
        /// Prints a line of text, returning the next line of text or <see langword="null"/> if there are no more lines to be printed.
        /// Supports CR, LF, or CRLF within the text for line breaks.
        /// If a CR/LF/CRLF is detected, additional spacing according to <see cref="Font.ParagraphSpacing"/> is added.
        /// </summary>
        /// <param name="text">The text to print.</param>
        /// <param name="width">Maximum width of text when <paramref name="wordWrap"/> is <see langword="true"/>.</param>
        /// <param name="wordWrap">Enables word-wrapping if the text extends past the specified width; wraps at spaces.</param>
        /// <param name="indent">Indent this line according to <see cref="Font.HangingIndent"/>.</param>
        /// <returns></returns>
        private (string? NextLine, bool IndentNextLine) WriteInternalAndReturnRemaining(string text, float width, bool wordWrap, bool indent)
        {
            if (text == "")
                return (null, false);
            var i = text.IndexOf('\r');
            var j = text.IndexOf('\n');
            var k = text.IndexOf("\r\n", StringComparison.Ordinal);
            if (k >= 0 && k <= i && k <= j) {
                var ret = WriteInternalAndReturnRemaining2(text.Substring(0, k), true, width, wordWrap, indent);
                if (ret == null)
                    CurrentY += _TranslateRev(Font.ParagraphSpacing);
                return (ret == null ? text.Substring(k + 2) : ret + text.Substring(k), ret != null);
            } else if (i >= 0 && (j == -1 || i < j)) {
                var ret = WriteInternalAndReturnRemaining2(text.Substring(0, i), true, width, wordWrap, indent);
                if (ret == null)
                    CurrentY += _TranslateRev(Font.ParagraphSpacing);
                return (ret == null ? text.Substring(i + 1) : ret + text.Substring(i), ret != null);
            } else if (j >= 0) {
                var ret = WriteInternalAndReturnRemaining2(text.Substring(0, j), true, width, wordWrap, indent);
                if (ret == null)
                    CurrentY += _TranslateRev(Font.ParagraphSpacing);
                return (ret == null ? text.Substring(j + 1) : ret + text.Substring(j), ret != null);
            }
            var ret2 = WriteInternalAndReturnRemaining2(text, false, width, wordWrap, indent);
            return (ret2, ret2 != null);
        }

        private string? WriteInternalAndReturnRemaining2(string text, bool newLine, float width, bool wordWrap, bool indent)
        {
            if (text == "") {
                if (newLine)
                    _currentY += TextHeightPoints();
                return null;
            }

            bool justify = Font.Justify;
            bool isLeftAligned = TextAlignment == TextAlignment.LeftTop || TextAlignment == TextAlignment.LeftCenter || TextAlignment == TextAlignment.LeftBaseline || TextAlignment == TextAlignment.LeftBottom;
            var widthPoints = _Translate(width - (isLeftAligned && indent ? Font.HangingIndent : 0f)) / Font.StretchX;
            string? remainingText = default;
            FinishLineAndUpdateLineStyle();
            if (string.IsNullOrEmpty(text) && !newLine)
                return null;
            var f = Font.ToiTextSharpFont(ForeColor);
            var bf = f.GetCalculatedBaseFont(true);
            var textWidthPoints = 0f;
            if (!string.IsNullOrEmpty(text)) {
                _content.SaveState();
                try {
                    _content.BeginText();
                    _content.SetFontAndSize(bf, f.CalculatedSize);
                    float XOffset = default, YOffset = default;
                    if (wordWrap) {
                        int I = text!.IndexOf(' ');
                        if (I == -1) {
                            wordWrap = false; // use newLine variable and don't kern text
                            remainingText = null;
                        } else {
                            int J, spaces;
                            float K, strlen, spacelen;
                            string str2;
                            J = 0;
                            strlen = _content.GetEffectiveStringWidth(text.Substring(0, I), false);
                            spacelen = _content.GetEffectiveStringWidth(" ", false);
                            spaces = 0;
                            do {
                                J = text.IndexOf(' ', I + 1); //no error thrown when startIndex = str.Length, but always returns -1 (perfect!)
                                if (J == -1) {
                                    str2 = text;
                                } else {
                                    str2 = text.Substring(0, J);

                                }

                                K = _content.GetEffectiveStringWidth(str2.TrimEnd(), false);
                                if (K <= widthPoints) {
                                    // enough room; continue with loop
                                    if (J == -1) {
                                        // enough room for entire string
                                        wordWrap = false; // use newLine variable and don't kern text
                                        remainingText = null;
                                        break;
                                    } else {
                                        I = J;
                                        strlen = K;
                                        spaces += 1;
                                    }
                                } else {
                                    // not enough room; go back to last valid string
                                    remainingText = text.Substring(I + 1); // everything after the space
                                    text = text.Substring(0, I).TrimEnd(); // everything before the space, trimmed in case of extra spaces
                                                                         // wordWrap = True 'write a new line and kern text
                                    if (justify)
                                        _content.SetWordSpacing((widthPoints - strlen) / spaces);
                                    break;
                                }
                            }
                            while (true);
                        }
                    } else {
                        remainingText = null;
                    }

                    switch (TextAlignment) {
                        case TextAlignment.LeftTop:
                        case TextAlignment.LeftCenter:
                        case TextAlignment.LeftBaseline:
                        case TextAlignment.LeftBottom: {
                            XOffset += indent ? _Translate(Font.HangingIndent) : 0f;
                            break;
                        }

                        case TextAlignment.CenterTop:
                        case TextAlignment.CenterCenter:
                        case TextAlignment.CenterBaseline:
                        case TextAlignment.CenterBottom: {
                            XOffset = -_content.GetEffectiveStringWidth(text, false) / 2 * Font.StretchX;
                            break;
                        }

                        case TextAlignment.RightTop:
                        case TextAlignment.RightCenter:
                        case TextAlignment.RightBaseline:
                        case TextAlignment.RightBottom: {
                            XOffset = -_content.GetEffectiveStringWidth(text, false) * Font.StretchX;
                            break;
                        }
                    }

                    switch (TextAlignment) {
                        case TextAlignment.LeftTop:
                        case TextAlignment.CenterTop:
                        case TextAlignment.RightTop: {
                            YOffset += bf.GetFontDescriptor(BaseFont.AWT_ASCENT, f.CalculatedSize) * Font.StretchY;
                            break;
                        }

                        case TextAlignment.LeftCenter:
                        case TextAlignment.CenterCenter:
                        case TextAlignment.RightCenter: {
                            YOffset += bf.GetFontDescriptor(BaseFont.CAPHEIGHT, f.CalculatedSize) / 2 * Font.StretchY;
                            break;
                        }

                        case TextAlignment.LeftBaseline:
                        case TextAlignment.CenterBaseline:
                        case TextAlignment.RightBaseline: {
                            break;
                        }

                        case TextAlignment.LeftBottom:
                        case TextAlignment.CenterBottom:
                        case TextAlignment.RightBottom: {
                            YOffset += TextHeightPoints() - bf.GetFontDescriptor(BaseFont.AWT_ASCENT, f.CalculatedSize) * Font.StretchY;
                            //YOffset += bf.GetFontDescriptor(BaseFont.AWT_DESCENT, f.CalculatedSize) * Font.StretchY; // negative value
                            //YOffset -= bf.GetFontDescriptor(BaseFont.AWT_LEADING, f.CalculatedSize) * Font.StretchY;
                            break;
                        }
                    }

                    if ((f.CalculatedStyle & iTextSharp.text.Font.ITALIC) == iTextSharp.text.Font.ITALIC) {
                        _content.SetTextMatrix(Font.StretchX, 0, 0.21256f * Font.StretchX, -Font.StretchY, _currentX + XOffset, _currentY + YOffset);
                    } else {
                        _content.SetTextMatrix(Font.StretchX, 0, 0, -Font.StretchY, _currentX + XOffset, _currentY + YOffset);
                    }

                    if ((f.CalculatedStyle & iTextSharp.text.Font.BOLD) == iTextSharp.text.Font.BOLD) {
                        _content.SetTextRenderingMode(PdfContentByte.TEXT_RENDER_MODE_FILL_STROKE);
                        _content.SetColorStroke(f.Color);
                        _content.SetLineCap(PdfContentByte.LINE_CAP_PROJECTING_SQUARE);
                        _content.SetLineDash(0);
                        _content.SetLineJoin(PdfContentByte.LINE_JOIN_MITER);
                        _content.SetLineWidth(f.Size / 30);
                    }

                    _content.SetColorFill(f.Color);
                    _content.MoveText(0, 0);
                    _content.ShowText(text);
                    // If NewLine Then _Content.NewlineText() 'unused; newline code is below.  (doesn't update CurrentY)
                    if ((f.CalculatedStyle & iTextSharp.text.Font.BOLD) == iTextSharp.text.Font.BOLD) {
                        _content.SetTextRenderingMode(PdfContentByte.TEXT_RENDER_MODE_FILL);
                    }

                    textWidthPoints = _content.GetEffectiveStringWidth(text, false) * Font.StretchX;
                    _content.EndText();
                    if ((f.CalculatedStyle & iTextSharp.text.Font.UNDERLINE) == iTextSharp.text.Font.UNDERLINE) {
                        _content.Rectangle(_currentX + XOffset, _currentY + YOffset + f.CalculatedSize / 4, textWidthPoints, -f.CalculatedSize / 15);
                        _content.Fill();
                    }

                    if ((f.CalculatedStyle & iTextSharp.text.Font.STRIKETHRU) == iTextSharp.text.Font.STRIKETHRU) {
                        _content.Rectangle(_currentX + XOffset, _currentY + YOffset - f.CalculatedSize / 3, textWidthPoints, -f.CalculatedSize / 15);
                        _content.Fill();
                    }
                } finally {
                    _content.RestoreState();
                }
            } else {
                remainingText = null;
            }

            if (newLine || wordWrap) // justify, at this point, is equilavent to (Write2 != null) -- indicating whether there is text to kern or not
            {
                //float textHeight = bf.GetFontDescriptor(BaseFont.AWT_ASCENT, f.CalculatedSize);
                //textHeight -= bf.GetFontDescriptor(BaseFont.AWT_DESCENT, f.CalculatedSize);
                //textHeight += bf.GetFontDescriptor(BaseFont.AWT_LEADING, f.CalculatedSize);
                _currentY += TextHeightPoints(); //textHeight * Font.LineSpacing * Font.StretchY;
            } else {
                switch (TextAlignment) {
                    case TextAlignment.LeftTop:
                    case TextAlignment.LeftCenter:
                    case TextAlignment.LeftBaseline:
                    case TextAlignment.LeftBottom:
                        _currentX += textWidthPoints;
                        break;

                    case TextAlignment.CenterTop:
                    case TextAlignment.CenterCenter:
                    case TextAlignment.CenterBaseline:
                    case TextAlignment.CenterBottom:
                        _currentX += textWidthPoints / 2f;
                        break;

                    case TextAlignment.RightTop:
                    case TextAlignment.RightCenter:
                    case TextAlignment.RightBaseline:
                    case TextAlignment.RightBottom:
                        _currentX -= textWidthPoints;
                        break;
                }
            }

            return remainingText;
        }

        /// <summary>
        /// Returns the width of the specified text.
        /// </summary>
        public float TextWidth(string text)
        {
            var f = Font.ToiTextSharpFont(ForeColor);
            return _TranslateRev(f.GetCalculatedBaseFont(false).GetWidthPoint(text, f.CalculatedSize)) * Font.StretchX;
        }

        /// <summary>
        /// Returns the height of a single line of text, including space between rows (ascent + descent + leading).
        /// Also accounts for the current <see cref="Font.LineSpacing">LineSpacing</see> and <see cref="Font.StretchY"/> settings.
        /// </summary>
        public float TextHeight() => _TranslateRev(TextHeightPoints());

        private float TextHeightPoints()
        {
            var f = Font.ToiTextSharpFont(ForeColor);
            var bf = f.GetCalculatedBaseFont(false);
            float s = bf.GetFontDescriptor(BaseFont.AWT_ASCENT, f.CalculatedSize);
            s -= bf.GetFontDescriptor(BaseFont.AWT_DESCENT, f.CalculatedSize);
            s += bf.GetFontDescriptor(BaseFont.AWT_LEADING, f.CalculatedSize);
            s *= Font.LineSpacing;
            return s * Font.StretchY;
        }

        /// <summary>
        /// Returns the distance between the baseline and the top of capital letters.
        /// </summary>
        public float TextCapHeight()
        {
            var f = Font.ToiTextSharpFont(ForeColor);
            var bf = f.GetCalculatedBaseFont(false);
            return _TranslateRev(bf.GetFontDescriptor(BaseFont.CAPHEIGHT, f.CalculatedSize)) * Font.StretchY;
        }

        /// <summary>
        /// Returns the distance between the baseline and the top of the highest letters.
        /// </summary>
        public float TextAscent()
        {
            var f = Font.ToiTextSharpFont(ForeColor);
            var bf = f.GetCalculatedBaseFont(false);
            return _TranslateRev(bf.GetFontDescriptor(BaseFont.AWT_ASCENT, f.CalculatedSize)) * Font.StretchY;
        }

        /// <summary>
        /// Returns the distance between the baseline and the bottom of the lowest letters (j's, etc).
        /// </summary>
        /// <returns></returns>
        public float TextDescent()
        {
            var f = Font.ToiTextSharpFont(ForeColor);
            var bf = f.GetCalculatedBaseFont(false);
            return _TranslateRev(-bf.GetFontDescriptor(BaseFont.AWT_DESCENT, f.CalculatedSize)) * Font.StretchY;
        }

        /// <summary>
        /// Returns the amount of additional line spacing besides the ascent and descent,
        /// adjusted for the current <see cref="Font.LineSpacing">LineSpacing</see> setting.
        /// </summary>
        public float TextLeading()
        {
            var f = Font.ToiTextSharpFont(ForeColor);
            var bf = f.GetCalculatedBaseFont(false);
            float leading = bf.GetFontDescriptor(BaseFont.AWT_LEADING, f.CalculatedSize);
            float fullHeight = bf.GetFontDescriptor(BaseFont.AWT_ASCENT, f.CalculatedSize);
            fullHeight -= bf.GetFontDescriptor(BaseFont.AWT_DESCENT, f.CalculatedSize);
            fullHeight += leading;
            float heightAdjustment = fullHeight * (Font.LineSpacing - 1f);
            leading += heightAdjustment;
            return _TranslateRev(leading) * Font.StretchY;
        }

        /// <summary>
        /// Returns the amount of additional line spacing between paragraphs.
        /// </summary>
        public float TextParagraphLeading() => _TranslateRev(Font.ParagraphSpacing);
    }
}
