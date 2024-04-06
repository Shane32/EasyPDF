using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Tests;

internal static class TestExtensions
{
    public static string ToASCIIString(this byte[] data)
    {
        var sb = new StringBuilder();
        foreach (var b in data) {
            if (b == '\\') {
                sb.Append("\\\\");
            } else if (b == 10 || b == 13 || (b >= 32 && b <= 126)) {
                sb.Append((char)b);
            } else {
                sb.Append($"\\x{b:X2}");
            }
        }

        return sb.ToString();
    }

    public static string RemoveID(this string data)
    {
        var index = data.LastIndexOf("R/ID");
        if (index > 0) {
            data = data.Substring(0, index);
        }

        // also remove font names

        // Define the regular expression pattern
        string pattern = @"/FontName/([^/]+)/";
        
        // Use Regex.Match to find the first match of the pattern in the input string
        var match = Regex.Match(data, pattern);

        // If a match is found
        if (match.Success) {
            // Get the captured group from the match (the {anything} part)
            string fontName = match.Groups[1].Value;

            // Replace all instances of the font name in the input string with "Dummy"
            data = data.Replace(fontName, "Dummy");
        }

        // Define the regular expression pattern for ITXT version numbers
        string itxtPattern = @"ITXT\(\d+\.\d+\.\d+\.\d+\)";

        // Replace all matches of the ITXT version pattern in the input string with "ITXT(3.3.2.0)"
        data = Regex.Replace(data, itxtPattern, "ITXT(3.3.2.0)");

        return data;
    }

    public static void PrepForTests(this PDFWriter writer)
    {
        writer.GetWriter().CompressionLevel = PdfStream.NO_COMPRESSION; // <-- does not seem to do anything; the document is not fully compressed, but nor is it fully uncompressed
        writer.GetWriter().FullCompression.ShouldBeFalse();
        writer.Metadata.Producer = null;
        writer.Metadata.CreationDate = null;
        writer.Metadata.ModificationDate = null;
    }

    private static readonly object _saveSync = new();
    public static byte[] SaveAsPdf(this byte[] byteArray, string? extraData = null)
    {
        lock (_saveSync) {
            // Get the calling method name and class name using the stack trace
            var stackTrace = new StackTrace(true);
            var callingFrame = stackTrace.GetFrame(1) ?? throw new InvalidOperationException("Calling stack frame not found");
            var callingMethod = callingFrame.GetMethod() ?? throw new InvalidOperationException("Calling method not found");
            string callingMethodName = callingMethod.Name;
            string callingClassName = (callingMethod.DeclaringType ?? throw new InvalidOperationException("Could not get calling method's delcaring type")).Name;

            // Generate the file name based on the calling method and class names
            string fileName = $"{callingClassName}.{callingMethodName}.pdf";

            // Get the location of the test's source file
            string directory = Path.GetDirectoryName(callingFrame.GetFileName()) ?? throw new InvalidOperationException("Could not get directory name");
            string filePath = Path.Combine(directory, fileName);

            // Write the byte array to the file
            File.WriteAllBytes(filePath, byteArray);

            // Return the original array for chaining
            return byteArray;
        }
    }

    private static readonly HashSet<string> _registeredFonts = new();
    public static void RegisterFont(string fileName, string fontName)
    {
        var stackTrace = new StackTrace(true);
        var callingFrame = stackTrace.GetFrame(0) ?? throw new InvalidOperationException("This stack frame not found");
        var directory = Path.GetDirectoryName(callingFrame.GetFileName()) ?? throw new InvalidOperationException("Could not get directory name");

        if (!_registeredFonts.Add(fontName))
            return;
        var fontPath = Path.Combine(directory, "Fonts", fileName);
        FontFactory.Register(fontPath, fontName);
    }
}
