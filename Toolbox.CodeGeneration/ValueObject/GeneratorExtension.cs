namespace Toolbox.CodeGeneration.ValueObject;

using System.Text;

internal static partial class GeneratorExtension
{
    internal static void AppendRawValue(this StringBuilder sb, ValueObjectModel model)
    {
        // 1. Подложка (обёртка над одним primitive)
        sb.Append("    public ").Append(model.UnderlyingTypeFullName).AppendLine(" Value { get; }");
        sb.AppendLine();
    }

    internal static void AppendEqualityMethods(this StringBuilder sb, ValueObjectModel model)
    {
        // 5. Equality / IEquatable / GetHashCode
        sb.AppendLine("    public override bool Equals(object? obj)");
        sb.AppendLine("        => obj is " + model.TypeName + " other && Equals(other);");
        sb.AppendLine();

        sb.AppendLine("    public bool Equals(" + model.TypeName + " other)");
        sb.AppendLine("        => System.Collections.Generic.EqualityComparer<" + model.UnderlyingTypeFullName +
                      ">.Default.Equals(Value, other.Value);");
        sb.AppendLine();

        sb.AppendLine("    public override int GetHashCode()");
        sb.AppendLine("        => System.Collections.Generic.EqualityComparer<" + model.UnderlyingTypeFullName +
                      ">.Default.GetHashCode(Value);");
        sb.AppendLine();
    }

    internal static void AppendComparisonExpressions(this StringBuilder sb, ValueObjectModel model)
    {
        // 6. Операторы ==, !=
        sb.AppendLine("    public static bool operator ==(" + model.TypeName + " left, " + model.TypeName +
                      " right) => left.Equals(right);");
        sb.AppendLine("    public static bool operator !=(" + model.TypeName + " left, " + model.TypeName +
                      " right) => !left.Equals(right);");
        sb.AppendLine();
    }

    internal static void AppendCompareMethods(this StringBuilder sb, ValueObjectModel model)
    {
        // 7. Сравнение и IComparable (опционально)
        if (model.ImplementComparable)
        {
            sb.AppendLine("    public int CompareTo(" + model.TypeName + " other)");
            sb.AppendLine("        => System.Collections.Generic.Comparer<" + model.UnderlyingTypeFullName +
                          ">.Default.Compare(Value, other.Value);");
            sb.AppendLine();

            sb.AppendLine("    public static bool operator <(" + model.TypeName + " left, " + model.TypeName +
                          " right) => left.CompareTo(right) < 0;");
            sb.AppendLine("    public static bool operator >(" + model.TypeName + " left, " + model.TypeName +
                          " right) => left.CompareTo(right) > 0;");
            sb.AppendLine("    public static bool operator <=(" + model.TypeName + " left, " + model.TypeName +
                          " right) => left.CompareTo(right) <= 0;");
            sb.AppendLine("    public static bool operator >=(" + model.TypeName + " left, " + model.TypeName +
                          " right) => left.CompareTo(right) >= 0;");
            sb.AppendLine();
        }
    }

    internal static void AppendExplicitAndImplicitOperators(this StringBuilder sb, ValueObjectModel model)
    {
        // 8. Операторы преобразования (explicit/implicit)
        // Из primitive в VO — всегда explicit (чтобы не плодить surprise-обёртки)
        sb.AppendLine("    public static explicit operator " + model.TypeName + "(" + model.UnderlyingTypeFullName +
                      " value)");
        sb.AppendLine("        => Create(value);");
        sb.AppendLine();

        // В primitive — по флагу (AllowImplicitToPrimitive)
        if (model.AllowImplicitToPrimitive)
        {
            sb.AppendLine("    public static implicit operator " + model.UnderlyingTypeFullName + "(" + model.TypeName +
                          " vo)");
            sb.AppendLine("        => vo.Value;");
            sb.AppendLine();
        }
        else
        {
            sb.AppendLine("    public static explicit operator " + model.UnderlyingTypeFullName + "(" + model.TypeName +
                          " vo)");
            sb.AppendLine("        => vo.Value;");
            sb.AppendLine();
        }
    }

    internal static void AppendIsDefault(this StringBuilder sb, ValueObjectModel model)
    {
        // 9. Безопасность default(T)
        // default(T) разрешаем, но считаем, что Value = default(primitive)
        sb.AppendLine("    public bool IsDefault => System.Collections.Generic.EqualityComparer<" +
                      model.UnderlyingTypeFullName + ">.Default.Equals(Value, default);");
        sb.AppendLine();
    }
}