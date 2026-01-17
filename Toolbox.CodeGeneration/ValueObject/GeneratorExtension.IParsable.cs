namespace Toolbox.CodeGeneration.ValueObject;

using System.Text;

internal static partial class GeneratorExtension
{
    internal static void AppendParsable(this StringBuilder sb, ValueObjectModel model)
    {
        var type = model.TypeName;

        sb.AppendLine("    #region IParsable");

        // TryParse(string)
        sb.AppendLine($"    public static bool TryParse(");
        sb.AppendLine("        string? s,");
        sb.AppendLine("        System.IFormatProvider? provider,");
        sb.AppendLine($"        out {type} result)");
        sb.AppendLine("    {");
        sb.AppendLine("        if (s is null)");
        sb.AppendLine("        {");
        sb.AppendLine("            result = default;");
        sb.AppendLine("            return false;");
        sb.AppendLine("        }");
        sb.AppendLine();
        sb.AppendLine("        return TryParse(s.AsSpan(), provider, out result);");
        sb.AppendLine("    }");
        sb.AppendLine();

        // Parse(string)
        sb.AppendLine($"    public static {type} Parse(");
        sb.AppendLine("        string s,");
        sb.AppendLine("        System.IFormatProvider? provider)");
        sb.AppendLine("    {");
        sb.AppendLine("        if (s is null)");
        sb.AppendLine("            throw new System.ArgumentNullException(nameof(s));");
        sb.AppendLine();
        sb.AppendLine("        return Parse(s.AsSpan(), provider);");
        sb.AppendLine("    }");

        sb.AppendLine("    #endregion");
        sb.AppendLine();
    }
}