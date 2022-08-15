using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextFont = iTextSharp.text.Font;

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
        public TextAlignment FontAlignment { get; set; } = TextAlignment.LeftBaseline;

        /// <summary>
        /// Gets or sets the line spacing multiple; defaults to 1.0.
        /// </summary>
        public float FontLineSpacing { get; set; } = 1f;

        /// <summary>
        /// Writes multiple lines of text with word wrapping at a specified position and returns the text remaining
        /// to be written (subsequent lines). Indentation for subsequent lines can be specified.
        /// <para>
        /// The current position is set to the line below the written block of text.
        /// If no text was written, the Y offset is still incremented.
        /// </para>
        /// </summary>
        /// <param name="step">Indicates that the <paramref name="x"/> and <paramref name="y"/> coordinates are offsets to the current position.</param>
        /// <param name="x">The X coordinate or offset.</param>
        /// <param name="y">The Y coordinate or offset.</param>
        /// <param name="width">The maximum width of a single line of text.</param>
        /// <param name="nextLineLeftIndent">The amount of left indent to apply to subsequent lines of text.</param>
        /// <param name="nextLineRightIndent">The amount of right indent to apply to subsequent lines of text.</param>
        /// <param name="text">The text to print.</param>
        /// <param name="justify">Adds word spacing to have each line besides the last fill the width.</param>
        public void WriteLinesAt(bool step, float x, float y, float width, float nextLineLeftIndent, float nextLineRightIndent, string? text, bool justify)
        {
            MoveTo(step, x, y);
            WriteLines(width, nextLineLeftIndent, nextLineRightIndent, text, justify);
        }

        /// <summary>
        /// Writes multiple lines of text with word wrapping at the current position and returns the text remaining
        /// to be written (subsequent lines). Indentation for subsequent lines can be specified.
        /// <para>
        /// The current position is set to the line below the written block of text.
        /// If no text was written, the Y offset is still incremented.
        /// </para>
        /// </summary>
        /// <param name="width">The maximum width of a single line of text.</param>
        /// <param name="nextLineLeftIndent">The amount of left indent to apply to subsequent lines of text.</param>
        /// <param name="nextLineRightIndent">The amount of right indent to apply to subsequent lines of text.</param>
       /// <param name="text">The text to print.</param>
        /// <param name="justify">Adds word spacing to have each line besides the last fill the width.</param>
        public void WriteLines(float width, float nextLineLeftIndent, float nextLineRightIndent, string? text, bool justify)
        {
            FinishLine();
            switch (FontAlignment) {
                case TextAlignment.CenterBaseline:
                case TextAlignment.CenterBottom:
                case TextAlignment.CenterCenter:
                case TextAlignment.CenterTop:
                case TextAlignment.RightBaseline:
                case TextAlignment.RightBottom:
                case TextAlignment.RightCenter:
                case TextAlignment.RightTop:
                    throw new InvalidOperationException("Invalid FontAlignment; use a left-aligned alignment");
            }

            if (string.IsNullOrEmpty(text)) {
                Write(null, true);
                return;
            }

            width = _Translate(width);
            text = WriteLine(width, text, justify);
            if (text is null)
                return;
            var oldX = _currentX;
            width = width - _Translate(nextLineLeftIndent) - _Translate(nextLineRightIndent);
            _currentX += _Translate(nextLineLeftIndent);
            do
                text = WriteLine(width, text, justify);
            while (text != null);
            _currentX = oldX;
        }

        /// <summary>
        /// Writes a single line of text with word wrapping at the current position and returns the text remaining
        /// to be written (subsequent lines); returns <see langword="null"/> if there are no more lines remaining
        /// to be printed.
        /// <para>
        /// The current position's Y offset is incremented by the line height.
        /// </para>
        /// </summary>
        /// <param name="width">The maximum width of a single line of text.</param>
        /// <param name="text">The text to print.</param>
        /// <param name="justify">Adds word spacing to have each line besides the last fill the width.</param>
        public string? WriteLine(float width, string? text, bool justify)
        {
            // Always goes to new line if there is more text to be written
            switch (FontAlignment) {
                case TextAlignment.CenterBaseline:
                case TextAlignment.CenterBottom:
                case TextAlignment.CenterCenter:
                case TextAlignment.CenterTop:
                case TextAlignment.RightBaseline:
                case TextAlignment.RightBottom:
                case TextAlignment.RightCenter:
                case TextAlignment.RightTop:
                    throw new InvalidOperationException("Invalid FontAlignment; use a left-aligned alignment");
            }

            return Write2(text ?? "", true, width, true, justify);
        }

        /// <summary>
        /// Prints specified text in the specified font at a specified position.
        /// No word wrapping is applied.
        /// <para>
        /// When <paramref name="newLine"/> is <see langword="false"/>, the current position is set to the end of the line of text.
        /// Otherwise, the current position is set to the specified positon with the Y offset incremented by the line height.
        /// </para>
        /// <para>
        /// When <paramref name="step"/> is <see langword="true"/>, the <paramref name="x"/> and <paramref name="y"/> coordinates
        /// are interpreted as an offset to the current position.
        /// </para>
        /// </summary>
        public void WriteAt(bool step, float x, float y, string? text, bool newLine = false)
        {
            FinishLine();
            if (step) {
                _currentX += _Translate(x);
                _currentY += _Translate(y);
            } else {
                _currentX = _Translate(x);
                _currentY = _Translate(y);
            }

            Write(text, newLine);
        }

        /// <summary>
        /// Prints specified text in the specified font at the current position.
        /// No word wrapping is applied.
        /// The current position is set to the end of the line of text.
        /// </summary>
        public void WriteFont(string? text, iTextFont f)
        {
            _content.SetFontAndSize(f.BaseFont, 12);
            Write(text);
        }

        /// <summary>
        /// Prints specified text in the specified font at the current position.
        /// No word wrapping or justification is applied.
        /// <para>
        /// When <paramref name="newLine"/> is <see langword="false"/>, the current position is set to the end of the line of text.
        /// Otherwise, the current position's Y offset is incremented by the line height.
        /// </para>
        /// </summary>
        public void Write(string? text, bool newLine = false)
        {
            text ??= "";
            while (text != null) {
                text = Write2(text, newLine, 0, false, false);
            }
        }

        /// <summary>
        /// Prints a line of text, returning the next line of text or <see langword="null"/> if there are no more lines to be printed.
        /// Supports CR, LF, or CRLF within the text for line breaks.
        /// </summary>
        /// <param name="text">The text to print.</param>
        /// <param name="newLine">If, for the last line, the position should be positioned below the starting position; otherwise it will be at the end of the line of text.</param>
        /// <param name="width">Maximum width of text when <paramref name="wordWrap"/> is <see langword="true"/>.</param>
        /// <param name="wordWrap">Enables word-wrapping if the text extends past the specified width; wraps at spaces.</param>
        /// <param name="justify">Justifies text if word wrapping was necessary (but not when due to CR/LF).</param>
        /// <returns></returns>
        private string? Write2(string text, bool newLine, float width, bool wordWrap, bool justify)
        {
            var i = text.IndexOf('\r');
            var j = text.IndexOf('\n');
            var k = text.IndexOf("\r\n");
            if (k >= 0 && k <= i && k <= j) {
                var ret = Write3(text.Substring(0, k), true, width, wordWrap, justify);
                return ret == null ? text.Substring(k + 2) : ret + text.Substring(k);
            } else if (i >= 0 && (j == -1 || i < j)) {
                var ret = Write3(text.Substring(0, i), true, width, wordWrap, justify);
                return ret == null ? text.Substring(i + 1) : ret + text.Substring(i);
            } else if (j >= 0) {
                var ret = Write3(text.Substring(0, j), true, width, wordWrap, justify);
                return ret == null ? text.Substring(j + 1) : ret + text.Substring(j);
            }
            return Write3(text, newLine, width, wordWrap, justify);
        }

        private string? Write3(string text, bool newLine, float width, bool wordWrap, bool justify)
        {
            System.Diagnostics.Debug.WriteLine("Print: " + (text ?? "(null)").Replace("\r", "\\r").Replace("\n", "\\n"));
            string? remainingText = default;
            FinishLine();
            if (string.IsNullOrEmpty(text) && !newLine)
                return null;
            var f = Font.ToiTextSharpFont(ForeColor);
            var bf = f.GetCalculatedBaseFont(true);
            var TextWidth = default(float);
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
                                if (K <= width) {
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
                                        _content.SetWordSpacing((width - strlen) / spaces);
                                    break;
                                }
                            }
                            while (true);
                        }
                    } else {
                        remainingText = null;
                    }

                    switch (FontAlignment) {
                        case TextAlignment.LeftTop:
                        case TextAlignment.LeftCenter:
                        case TextAlignment.LeftBaseline:
                        case TextAlignment.LeftBottom: {
                            break;
                        }

                        case TextAlignment.CenterTop:
                        case TextAlignment.CenterCenter:
                        case TextAlignment.CenterBaseline:
                        case TextAlignment.CenterBottom: {
                            XOffset = -_content.GetEffectiveStringWidth(text, false) / 2;
                            break;
                        }

                        case TextAlignment.RightTop:
                        case TextAlignment.RightCenter:
                        case TextAlignment.RightBaseline:
                        case TextAlignment.RightBottom: {
                            XOffset = -_content.GetEffectiveStringWidth(text, false);
                            break;
                        }
                    }

                    switch (FontAlignment) {
                        case TextAlignment.LeftTop:
                        case TextAlignment.CenterTop:
                        case TextAlignment.RightTop: {
                            YOffset += bf.GetFontDescriptor(BaseFont.AWT_ASCENT, f.CalculatedSize);
                            break;
                        }

                        case TextAlignment.LeftCenter:
                        case TextAlignment.CenterCenter:
                        case TextAlignment.RightCenter: {
                            YOffset += bf.GetFontDescriptor(BaseFont.CAPHEIGHT, f.CalculatedSize) / 2;
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
                            YOffset += bf.GetFontDescriptor(BaseFont.AWT_DESCENT, f.CalculatedSize); // negative value
                            YOffset -= bf.GetFontDescriptor(BaseFont.AWT_LEADING, f.CalculatedSize);
                            break;
                        }
                    }

                    if ((f.CalculatedStyle & iTextSharp.text.Font.ITALIC) == iTextSharp.text.Font.ITALIC) {
                        _content.SetTextMatrix(1, 0, 0.21256f, -1, _currentX + XOffset, _currentY + YOffset);
                    } else {
                        _content.SetTextMatrix(1, 0, 0, -1, _currentX + XOffset, _currentY + YOffset);
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
                    _content.ShowText(text!.Translate(Font));
                    // If NewLine Then _Content.NewlineText() 'unused; newline code is below.  (doesn't update CurrentY)
                    if ((f.CalculatedStyle & iTextSharp.text.Font.BOLD) == iTextSharp.text.Font.BOLD) {
                        _content.SetTextRenderingMode(PdfContentByte.TEXT_RENDER_MODE_FILL);
                    }

                    TextWidth = _content.GetEffectiveStringWidth(text, false);
                    _content.EndText();
                    if ((f.CalculatedStyle & iTextSharp.text.Font.UNDERLINE) == iTextSharp.text.Font.UNDERLINE) {
                        _content.Rectangle(_currentX + XOffset, _currentY + YOffset + f.CalculatedSize / 4, TextWidth, -f.CalculatedSize / 15);
                        _content.Fill();
                    }

                    if ((f.CalculatedStyle & iTextSharp.text.Font.STRIKETHRU) == iTextSharp.text.Font.STRIKETHRU) {
                        _content.Rectangle(_currentX + XOffset, _currentY + YOffset - f.CalculatedSize / 3, TextWidth, -f.CalculatedSize / 15);
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
                float TextHeight = bf.GetFontDescriptor(BaseFont.AWT_ASCENT, f.CalculatedSize);
                TextHeight -= bf.GetFontDescriptor(BaseFont.AWT_DESCENT, f.CalculatedSize);
                TextHeight += bf.GetFontDescriptor(BaseFont.AWT_LEADING, f.CalculatedSize);
                _currentY += TextHeight * FontLineSpacing;
            } else {
                switch (FontAlignment) {
                    case TextAlignment.LeftTop:
                    case TextAlignment.LeftCenter:
                    case TextAlignment.LeftBaseline:
                    case TextAlignment.LeftBottom:
                        _currentX += TextWidth;
                        break;

                    case TextAlignment.CenterTop:
                    case TextAlignment.CenterCenter:
                    case TextAlignment.CenterBaseline:
                    case TextAlignment.CenterBottom:
                        _currentX += TextWidth / 2f;
                        break;

                    case TextAlignment.RightTop:
                    case TextAlignment.RightCenter:
                    case TextAlignment.RightBaseline:
                    case TextAlignment.RightBottom:
                        _currentX -= TextWidth;
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
            return _TranslateRev(f.GetCalculatedBaseFont(false).GetWidthPoint(text, f.CalculatedSize));
        }

        /// <summary>
        /// Returns the height of a single line of text, including space between rows (ascent + descent + leading).
        /// </summary>
        public float TextHeight()
        {
            var f = Font.ToiTextSharpFont(ForeColor);
            var bf = f.GetCalculatedBaseFont(false);
            float s = bf.GetFontDescriptor(BaseFont.AWT_ASCENT, f.CalculatedSize);
            s -= bf.GetFontDescriptor(BaseFont.AWT_DESCENT, f.CalculatedSize);
            s += bf.GetFontDescriptor(BaseFont.AWT_LEADING, f.CalculatedSize);
            return _TranslateRev(s);
        }

        /// <summary>
        /// Returns the distance between the baseline and the top of capital letters.
        /// </summary>
        public float TextCapHeight()
        {
            var f = Font.ToiTextSharpFont(ForeColor);
            var bf = f.GetCalculatedBaseFont(false);
            return _TranslateRev(bf.GetFontDescriptor(BaseFont.CAPHEIGHT, f.CalculatedSize));
        }

        /// <summary>
        /// Returns the distance between the baseline and the top of the highest letters.
        /// </summary>
        public float TextAscent()
        {
            var f = Font.ToiTextSharpFont(ForeColor);
            var bf = f.GetCalculatedBaseFont(false);
            return _TranslateRev(bf.GetFontDescriptor(BaseFont.AWT_ASCENT, f.CalculatedSize));
        }

        /// <summary>
        /// Returns the distance between the baseline and the bottom of the lowest letters (j's, etc).
        /// </summary>
        /// <returns></returns>
        public float TextDescent()
        {
            var f = Font.ToiTextSharpFont(ForeColor);
            var bf = f.GetCalculatedBaseFont(false);
            return _TranslateRev(-bf.GetFontDescriptor(BaseFont.AWT_DESCENT, f.CalculatedSize));
        }

        /// <summary>
        /// Returns the amount of additional line spacing besides the ascent and descent.
        /// </summary>
        /// <returns></returns>
        public float TextLeading()
        {
            var f = Font.ToiTextSharpFont(ForeColor);
            var bf = f.GetCalculatedBaseFont(false);
            return _TranslateRev(bf.GetFontDescriptor(BaseFont.AWT_LEADING, f.CalculatedSize));
        }
    }
}
