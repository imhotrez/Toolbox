namespace Toolbox.CodeGeneration.ValueObject;

using System;
using System.Text;

internal static partial class GeneratorExtension
{
    internal static void AppendSpanParsable(this StringBuilder sb, ValueObjectModel model)
    {
        var type = model.TypeName;
        var raw  = model.UnderlyingTypeFullName;

        sb.AppendLine("    #region ISpanParsable");

        // TryParse
        sb.AppendLine($"    public static bool TryParse(");
        sb.AppendLine("        System.ReadOnlySpan<char> s,");
        sb.AppendLine("        System.IFormatProvider? provider,");
        sb.AppendLine($"        out {type} result)");
        sb.AppendLine("    {");

        if (raw == "string" || raw == "System.String")
        {
            sb.AppendLine("        var value = s.ToString();");
            sb.AppendLine();
            sb.AppendLine($"        return {type}.TryCreate(value, out result);");
        }
        else
        {
            sb.AppendLine($"        if (!{raw}.TryParse(");
            sb.AppendLine("                s,");
            sb.AppendLine($"                {GetNumberStyles(raw)},");
            sb.AppendLine("                System.Globalization.CultureInfo.InvariantCulture,");
            sb.AppendLine("                out var value))");
            sb.AppendLine("        {");
            sb.AppendLine($"            result = default;");
            sb.AppendLine("            return false;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine($"        return {type}.TryCreate(value, out result);");
        }

        sb.AppendLine("    }");
        sb.AppendLine();

        // Parse
        sb.AppendLine($"    public static {type} Parse(");
        sb.AppendLine("        System.ReadOnlySpan<char> s,");
        sb.AppendLine("        System.IFormatProvider? provider)");
        sb.AppendLine("    {");
        sb.AppendLine($"        if (!TryParse(s, provider, out var result))");
        sb.AppendLine($"            throw new System.FormatException(\"Invalid {type} format.\");");
        sb.AppendLine();
        sb.AppendLine("        return result;");
        sb.AppendLine("    }");

        sb.AppendLine("    #endregion");
        sb.AppendLine();
    }

    private static string GetNumberStyles(string underlying)
        => underlying switch
        {
            "byte" or "short" or "int" or "long" or
                "uint" or "ulong"
                => "System.Globalization.NumberStyles.Integer",

            "float" or "double" or "decimal"
                => "System.Globalization.NumberStyles.Float",

            _ => throw new NotSupportedException(
                $"NumberStyles not supported for {underlying}")
        };
}