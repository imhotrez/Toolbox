namespace Toolbox.CodeGeneration.ValueObject;

using System.Text;

internal static partial class GeneratorExtension
{
    internal static void AppendNewtonsoftJsonConverter(this StringBuilder sb, ValueObjectModel model)
    {
        var typeName   = model.TypeName;
        var underlying = model.UnderlyingTypeFullName;
        var tokenCheck = underlying == "string"
            ? "reader.TokenType != Newtonsoft.Json.JsonToken.String"
            : "reader.TokenType != Newtonsoft.Json.JsonToken.Integer && reader.TokenType != Newtonsoft.Json.JsonToken.Float";

        sb.AppendLine($"    internal sealed class NewtonsoftJsonConverter : Newtonsoft.Json.JsonConverter<{typeName}>");
        sb.AppendLine("    {");

        // READ
        sb.AppendLine($"        public override {typeName} ReadJson(");
        sb.AppendLine("            Newtonsoft.Json.JsonReader reader,");
        sb.AppendLine("            System.Type objectType,");
        sb.AppendLine($"            {typeName} existingValue,");
        sb.AppendLine("            bool hasExistingValue,");
        sb.AppendLine("            Newtonsoft.Json.JsonSerializer serializer)");
        sb.AppendLine("        {");
        sb.AppendLine("            if (reader.TokenType == Newtonsoft.Json.JsonToken.Null)");
        sb.AppendLine(
            "                throw new Newtonsoft.Json.JsonSerializationException(\"Null is not allowed.\");");
        sb.AppendLine();
        sb.AppendLine($"            if ({tokenCheck})");
        sb.AppendLine("                throw new Newtonsoft.Json.JsonSerializationException(");
        sb.AppendLine($"                    \"Invalid JSON token for {typeName}.\");");
        sb.AppendLine();
        sb.AppendLine($"            var primitive = serializer.Deserialize<{underlying}>(reader);");
        sb.AppendLine($"            if (!{typeName}.TryCreate(primitive!, out var result))");
        sb.AppendLine(
            $"                throw new Newtonsoft.Json.JsonSerializationException(\"Invalid {typeName} value.\");");
        sb.AppendLine();
        sb.AppendLine("            return result;");
        sb.AppendLine("        }");
        sb.AppendLine();

        // WRITE
        sb.AppendLine("        public override void WriteJson(");
        sb.AppendLine("            Newtonsoft.Json.JsonWriter writer,");
        sb.AppendLine($"            {typeName} value,");
        sb.AppendLine("            Newtonsoft.Json.JsonSerializer serializer)");
        sb.AppendLine("        {");
        sb.AppendLine("            serializer.Serialize(writer, value.Value);");
        sb.AppendLine("        }");

        sb.AppendLine("    }");
    }
}