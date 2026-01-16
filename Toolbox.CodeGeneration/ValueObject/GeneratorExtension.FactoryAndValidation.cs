namespace Toolbox.CodeGeneration.ValueObject;

using System.Text;

internal static partial class GeneratorExtension
{
    internal static void AppendFactoryAndValidation(this StringBuilder sb, ValueObjectModel model)
    {
        var type = model.TypeName;
        var raw  = model.UnderlyingTypeFullName;

        // =========================
        // Constructor
        // =========================
        sb.AppendLine($"    private {type}({raw} value)");
        sb.AppendLine("    {");
        sb.AppendLine("        Value = value;");
        sb.AppendLine("    }");
        sb.AppendLine();

        // =========================
        // Validate declaration
        // =========================
        sb.AppendLine("    static partial void Validate(");
        sb.AppendLine($"        {raw} value,");
        sb.AppendLine("        ref bool isValid,");
        sb.AppendLine("        ref string? error);");
        sb.AppendLine();

        // =========================
        // TryCreate
        // =========================
        sb.AppendLine($"    public static bool TryCreate({raw} value, out {type} result)");
        sb.AppendLine("    {");
        sb.AppendLine("        var isValid = true;");
        sb.AppendLine("        string? error = null;");
        sb.AppendLine();
        sb.AppendLine("        Validate(value, ref isValid, ref error);");
        sb.AppendLine();
        sb.AppendLine("        if (!isValid)");
        sb.AppendLine("        {");
        sb.AppendLine("            result = default;");
        sb.AppendLine("            return false;");
        sb.AppendLine("        }");
        sb.AppendLine();
        sb.AppendLine($"        result = new {type}(value);");
        sb.AppendLine("        return true;");
        sb.AppendLine("    }");
        sb.AppendLine();

        // =========================
        // Create
        // =========================
        sb.AppendLine($"    public static {type} Create({raw} value)");
        sb.AppendLine("    {");
        sb.AppendLine("        var isValid = true;");
        sb.AppendLine("        string? error = null;");
        sb.AppendLine();
        sb.AppendLine("        Validate(value, ref isValid, ref error);");
        sb.AppendLine();
        sb.AppendLine("        if (!isValid)");
        sb.AppendLine("            throw new System.ArgumentOutOfRangeException(");
        sb.AppendLine("                nameof(value),");
        sb.AppendLine("                value,");
        sb.AppendLine($"                error ?? \"Invalid {type} value.\");");
        sb.AppendLine();
        sb.AppendLine($"        return new {type}(value);");
        sb.AppendLine("    }");
        sb.AppendLine();
    }
}