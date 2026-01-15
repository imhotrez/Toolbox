namespace Toolbox.CodeGeneration.ValueObject;

using System.Text;

internal static partial class GeneratorExtension
{
    internal static void AppendNewtonsoftJsonConverter(this StringBuilder sb, ValueObjectModel model)
    {
        var typeName   = model.TypeName;
        var underlying = model.UnderlyingTypeFullName;

        sb.AppendLine($"    internal sealed class {typeName}NewtonsoftJsonConverter");
        sb.AppendLine($"        : Newtonsoft.Json.JsonConverter");
        sb.AppendLine("    {");

        sb.AppendLine("        public override bool CanConvert(System.Type objectType)");
        sb.AppendLine($"            => objectType == typeof({typeName});");
        sb.AppendLine();

        sb.AppendLine("        public override object ReadJson(");
        sb.AppendLine("            Newtonsoft.Json.JsonReader reader,");
        sb.AppendLine("            System.Type objectType,");
        sb.AppendLine("            object? existingValue,");
        sb.AppendLine("            Newtonsoft.Json.JsonSerializer serializer)");
        sb.AppendLine("        {");

        sb.AppendLine("            if (reader.TokenType == Newtonsoft.Json.JsonToken.Null)");
        sb.AppendLine(
            "                throw new Newtonsoft.Json.JsonSerializationException(\"Null is not allowed.\");");

        sb.AppendLine($"            var primitive = serializer.Deserialize<{underlying}>(reader);");
        sb.AppendLine(
            $"            if (!{typeName}.TryCreate(primitive, out var result))");
        sb.AppendLine(
            $"                throw new Newtonsoft.Json.JsonSerializationException(\"Invalid {typeName} value.\");");
        sb.AppendLine("            return result;");
        sb.AppendLine("        }");
        sb.AppendLine();

        sb.AppendLine("        public override void WriteJson(");
        sb.AppendLine("            Newtonsoft.Json.JsonWriter writer,");
        sb.AppendLine("            object? value,");
        sb.AppendLine("            Newtonsoft.Json.JsonSerializer serializer)");
        sb.AppendLine("        {");
        sb.AppendLine($"            serializer.Serialize(writer, (({typeName})value!).Value);");
        sb.AppendLine("        }");

        sb.AppendLine("    }");
    }
}