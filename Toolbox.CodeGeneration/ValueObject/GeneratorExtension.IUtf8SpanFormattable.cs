namespace Toolbox.CodeGeneration.ValueObject;

using System.Text;

internal static partial class GeneratorExtension
{
    internal static void AppendUtf8SpanFormattable(this StringBuilder sb, ValueObjectModel model)
    {
        var raw        = model.RawValueIsNullable ? "Value!" : "Value";
        var underlying = model.UnderlyingTypeFullName;

        sb.AppendLine("    #region IUtf8SpanFormattable");

        sb.AppendLine("    public bool TryFormat(");
        sb.AppendLine("        System.Span<byte> utf8Destination,");
        sb.AppendLine("        out int bytesWritten,");
        sb.AppendLine("        System.ReadOnlySpan<char> format,");
        sb.AppendLine("        System.IFormatProvider? provider)");
        sb.AppendLine("    {");

        if (underlying == "string" || underlying == "System.String")
        {
            // STRING
            sb.AppendLine("        if (!format.IsEmpty)");
            sb.AppendLine(
                "            throw new System.FormatException(\"Format string is not supported for string-based ValueObject.\");");
            sb.AppendLine();
            sb.AppendLine("        return System.Text.Encoding.UTF8.TryGetBytes(");
            sb.AppendLine($"            {raw},");
            sb.AppendLine("            utf8Destination,");
            sb.AppendLine("            out bytesWritten);");
        }
        else
        {
            // NUMERIC
            sb.AppendLine($"        return {raw}.TryFormat(");
            sb.AppendLine("            utf8Destination,");
            sb.AppendLine("            out bytesWritten,");
            sb.AppendLine("            format,");
            sb.AppendLine("            System.Globalization.CultureInfo.InvariantCulture);");
        }

        sb.AppendLine("    }");
        sb.AppendLine("    #endregion");
        sb.AppendLine();
    }
}