namespace Toolbox.CodeGeneration.ValueObject;

using System.Text;

internal static partial class GeneratorExtension
{
    internal static void AppendUtf8SpanParsable(this StringBuilder sb, ValueObjectModel model)
    {
        var type = model.TypeName;
        var raw  = model.UnderlyingTypeFullName;

        sb.AppendLine("    #region IUtf8SpanParsable");
        sb.AppendLine($"    public static {type} Parse(");
        sb.AppendLine("        System.ReadOnlySpan<byte> utf8Text,");
        sb.AppendLine("        System.IFormatProvider? provider)");
        sb.AppendLine("    {");
        sb.AppendLine("        if (utf8Text.IsEmpty)");
        sb.AppendLine("            throw new System.FormatException(\"Input is empty.\");");
        sb.AppendLine();

        if (raw == "string" || raw == "System.String")
        {
            sb.AppendLine("        string value;");
            sb.AppendLine("        try");
            sb.AppendLine("        {");
            sb.AppendLine("            value = System.Text.Encoding.UTF8.GetString(utf8Text);");
            sb.AppendLine("        }");
            sb.AppendLine("        catch (System.Exception ex)");
            sb.AppendLine("        {");
            sb.AppendLine("            throw new System.FormatException(\"Invalid UTF8 format.\", ex);");
            sb.AppendLine("        }");
        }
        else
        {
            sb.AppendLine(
                $"        if (!System.Buffers.Text.Utf8Parser.TryParse(utf8Text, out {raw} value, out var consumed)");
            sb.AppendLine("            || consumed != utf8Text.Length)");
            sb.AppendLine("            throw new System.FormatException(\"Invalid UTF8 format.\");");
        }

        sb.AppendLine();
        sb.AppendLine($"        if (!{type}.TryCreate(value, out var result))");
        sb.AppendLine($"            throw new System.FormatException(\"Invalid {type} value.\");");
        sb.AppendLine();
        sb.AppendLine("        return result;");
        sb.AppendLine("    }");
        sb.AppendLine();

        sb.AppendLine($"    public static bool TryParse(");
        sb.AppendLine("        System.ReadOnlySpan<byte> utf8Text,");
        sb.AppendLine("        System.IFormatProvider? provider,");
        sb.AppendLine($"        out {type} result)");
        sb.AppendLine("    {");

        if (raw == "string" || raw == "System.String")
        {
            sb.AppendLine("        string value;");
            sb.AppendLine("        try");
            sb.AppendLine("        {");
            sb.AppendLine("            value = System.Text.Encoding.UTF8.GetString(utf8Text);");
            sb.AppendLine("        }");
            sb.AppendLine("        catch");
            sb.AppendLine("        {");
            sb.AppendLine("            result = default;");
            sb.AppendLine("            return false;");
            sb.AppendLine("        }");
        }
        else
        {
            sb.AppendLine(
                $"        if (!System.Buffers.Text.Utf8Parser.TryParse(utf8Text, out {raw} value, out var consumed)");
            sb.AppendLine("            || consumed != utf8Text.Length)");
            sb.AppendLine("        {");
            sb.AppendLine("            result = default;");
            sb.AppendLine("            return false;");
            sb.AppendLine("        }");
        }

        sb.AppendLine();
        sb.AppendLine($"        return {type}.TryCreate(value, out result);");
        sb.AppendLine("    }");
        sb.AppendLine("    #endregion");
        sb.AppendLine();
    }
}