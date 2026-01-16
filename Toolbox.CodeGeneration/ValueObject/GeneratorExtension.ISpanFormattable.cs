namespace Toolbox.CodeGeneration.ValueObject;

using System.Text;

internal static partial class GeneratorExtension
{
    internal static void AppendSpanFormattable(
        this StringBuilder sb,
        ValueObjectModel model)
    {
        var raw        = model.RawValueIsNullable ? "Value!" : "Value";
        var underlying = model.UnderlyingTypeFullName;

        sb.AppendLine("    #region ISpanFormattable");

        // ---------- ToString() ----------
        sb.AppendLine("    public override string ToString()");
        if (underlying == "string" || underlying == "System.String")
        {
            sb.AppendLine($"        => {raw};");
        }
        else
        {
            sb.AppendLine(
                $"        => {raw}.ToString(System.Globalization.CultureInfo.InvariantCulture);");
        }

        sb.AppendLine();

        // ---------- IFormattable ----------
        sb.AppendLine(
            "    public string ToString(string? format, System.IFormatProvider? formatProvider)");
        if (underlying is "string" or "System.String")
        {
            sb.AppendLine($"        => {raw};");
        }
        else
        {
            sb.AppendLine(
                $"        => {raw}.ToString(format, formatProvider ?? System.Globalization.CultureInfo.InvariantCulture);");
        }

        sb.AppendLine();

        // ---------- TryFormat (Span<char>) ----------
        sb.AppendLine(
            "    public bool TryFormat(");
        sb.AppendLine(
            "        System.Span<char> destination,");
        sb.AppendLine(
            "        out int charsWritten,");
        sb.AppendLine(
            "        System.ReadOnlySpan<char> format,");
        sb.AppendLine(
            "        System.IFormatProvider? provider)");
        sb.AppendLine("    {");

        if (underlying == "string" || underlying == "System.String")
        {
            sb.AppendLine($"        if ({raw}.Length > destination.Length)");
            sb.AppendLine("        {");
            sb.AppendLine("            charsWritten = 0;");
            sb.AppendLine("            return false;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine($"        {raw}.AsSpan().CopyTo(destination);");
            sb.AppendLine($"        charsWritten = {raw}.Length;");
            sb.AppendLine("        return true;");
        }
        else
        {
            sb.AppendLine(
                $"        return {raw}.TryFormat(");
            sb.AppendLine(
                "            destination,");
            sb.AppendLine(
                "            out charsWritten,");
            sb.AppendLine(
                "            format,");
            sb.AppendLine(
                "            System.Globalization.CultureInfo.InvariantCulture);");
        }

        sb.AppendLine("    }");
        sb.AppendLine("    #endregion");
        sb.AppendLine();
    }
}