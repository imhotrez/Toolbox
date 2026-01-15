namespace Toolbox.CodeGeneration.ValueObject;

using System;
using System.Text;

internal static partial class GeneratorExtension
{
    internal static void AppendSystemTextJsonWriteMethods(this StringBuilder sb, ValueObjectModel model)
    {
        var raw          = model.RawValueIsNullable ? "Value!" : "Value";
        var allowedToken = GetAllowedJsonTokenType(model.UnderlyingTypeFullName);
        var dataType     = allowedToken == "System.Text.Json.JsonTokenType.String" ? "String" : "Number";
        
        sb.AppendLine("    #region System.Text.Json");
        sb.AppendLine(
            "    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]");
        sb.AppendLine($"    public void WriteValue(System.Text.Json.Utf8JsonWriter writer) =>");
        sb.AppendLine($"        writer.Write{dataType}Value({raw});");
        sb.AppendLine();

        sb.AppendLine(
            "    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]");
        sb.AppendLine($"    public void Write(System.Text.Json.Utf8JsonWriter writer, string propertyName) =>");
        sb.AppendLine($"        writer.Write{dataType}(propertyName, {raw});");
        sb.AppendLine();

        sb.AppendLine(
            "    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]");
        sb.AppendLine(
            $"    public void Write(System.Text.Json.Utf8JsonWriter writer, System.ReadOnlySpan<char> propertyName) =>");
        sb.AppendLine($"        writer.Write{dataType}(propertyName, {raw});");
        sb.AppendLine();

        sb.AppendLine(
            "    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]");
        sb.AppendLine(
            $"    public void Write(System.Text.Json.Utf8JsonWriter writer, System.Text.Json.JsonEncodedText propertyName) =>");
        sb.AppendLine($"        writer.Write{dataType}(propertyName, {raw});");
        sb.AppendLine("    #endregion");
        sb.AppendLine();
    }

    internal static void AppendSystemTextJsonConverter(this StringBuilder sb, ValueObjectModel model)
    {
        var type = model.TypeName;
        var raw  = model.UnderlyingTypeFullName;

        sb.AppendLine("    #region System.Text.Json Converter");
        sb.AppendLine($"    private sealed class SystemTextJsonConverter : System.Text.Json.Serialization.JsonConverter<{type}>");
        sb.AppendLine("    {");

        // READ VALUE
        /*sb.AppendLine($"        public override {type} Read(");
        sb.AppendLine("            ref System.Text.Json.Utf8JsonReader reader,");
        sb.AppendLine("            System.Type typeToConvert,");
        sb.AppendLine("            System.Text.Json.JsonSerializerOptions options)");
        sb.AppendLine("        {");
        sb.AppendLine("            if (reader.TokenType == System.Text.Json.JsonTokenType.Null)");
        sb.AppendLine("                throw new System.Text.Json.JsonException(\"Null is not allowed.\");");
        sb.AppendLine();
        sb.AppendLine("            if (reader.TokenType == System.Text.Json.JsonTokenType.String)");
        sb.AppendLine("            {");
        sb.AppendLine($"                {raw} rawValue;");
        sb.AppendLine($"                if (!{raw}.TryParse(reader.GetString(), out rawValue))");
        sb.AppendLine($"                    throw new System.Text.Json.JsonException(\"Invalid {type} format.\");");
        sb.AppendLine();
        sb.AppendLine("                return Create(rawValue);");
        sb.AppendLine("            }");
        sb.AppendLine();
        sb.AppendLine($"            var value = reader.Get{GetSystemTextJsonGetter(raw)}();");
        sb.AppendLine($"            if (!{type}.TryCreate(value, out var result))");
        sb.AppendLine($"                throw new System.Text.Json.JsonException(\"Invalid {type} value.\");");
        sb.AppendLine("            return result;");
        sb.AppendLine("        }");*/
        
        var allowedToken = GetAllowedJsonTokenType(raw);

        sb.AppendLine($"        public override {type} Read(");
        sb.AppendLine("            ref System.Text.Json.Utf8JsonReader reader,");
        sb.AppendLine("            System.Type typeToConvert,");
        sb.AppendLine("            System.Text.Json.JsonSerializerOptions options)");
        sb.AppendLine("        {");

        // NULL
        sb.AppendLine("            if (reader.TokenType == System.Text.Json.JsonTokenType.Null)");
        sb.AppendLine("                throw new System.Text.Json.JsonException(\"Null is not allowed.\");");
        sb.AppendLine();

        // STRICT TOKEN CHECK
        sb.AppendLine($"            if (reader.TokenType != {allowedToken})");
        sb.AppendLine("                throw new System.Text.Json.JsonException(");
        sb.AppendLine(
            $"                    $\"Invalid JSON token for {type}. Expected {allowedToken}, got {{reader.TokenType}}.\");");
        sb.AppendLine();

        // READ RAW VALUE
        sb.AppendLine($"            var value = reader.Get{GetSystemTextJsonGetter(raw)}();");
        sb.AppendLine();

        // FACTORY
        sb.AppendLine($"            if (!{type}.TryCreate(value, out var result))");
        sb.AppendLine($"                throw new System.Text.Json.JsonException(\"Invalid {type} value.\");");
        sb.AppendLine();

        sb.AppendLine("            return result;");
        sb.AppendLine("        }");

        sb.AppendLine();

        // WRITE VALUE
        sb.AppendLine($"        public override void Write(");
        sb.AppendLine("            System.Text.Json.Utf8JsonWriter writer,");
        sb.AppendLine($"            {type} value,");
        sb.AppendLine("            System.Text.Json.JsonSerializerOptions options)");
        sb.AppendLine("        {");
        sb.AppendLine("            value.WriteValue(writer);");
        sb.AppendLine("        }");
        sb.AppendLine();

        // READ PROPERTY NAME
        sb.AppendLine($"        public override {type} ReadAsPropertyName(");
        sb.AppendLine("            ref System.Text.Json.Utf8JsonReader reader,");
        sb.AppendLine("            System.Type typeToConvert,");
        sb.AppendLine("            System.Text.Json.JsonSerializerOptions options)");
        sb.AppendLine("        {");
        sb.AppendLine("            var bytes = reader.HasValueSequence");
        sb.AppendLine("                ? reader.ValueSequence.FirstSpan.ToArray()");
        sb.AppendLine("                : reader.ValueSpan;");
        sb.AppendLine();

        if (GetAllowedJsonTokenType(model.UnderlyingTypeFullName) == "System.Text.Json.JsonTokenType.String")
        {
            sb.AppendLine("            string value;");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                value = System.Text.Encoding.UTF8.GetString(bytes);");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                throw new System.Text.Json.JsonException(\"Could not decode UTF-8 string.\", ex);");
            sb.AppendLine("            }");
            sb.AppendLine();
        }
        else
        {
            sb.AppendLine();

            sb.AppendLine(
                $"            if (!System.Buffers.Text.Utf8Parser.TryParse(bytes, out {raw} value, out var consumed)");
            sb.AppendLine("                || consumed != bytes.Length)");
            sb.AppendLine("                throw new System.Text.Json.JsonException(\"Could not parse property name.\");");
            sb.AppendLine();
        }
        
        sb.AppendLine($"            if (!{type}.TryCreate(value, out var result))");
        sb.AppendLine($"                throw new System.Text.Json.JsonException(\"Invalid {type} value.\");");
        sb.AppendLine();

        sb.AppendLine("            return result;");
        sb.AppendLine("        }");
        sb.AppendLine();

        // WRITE PROPERTY NAME
        sb.AppendLine($"        public override void WriteAsPropertyName(");
        sb.AppendLine("            System.Text.Json.Utf8JsonWriter writer,");
        sb.AppendLine($"            {type} value,");
        sb.AppendLine("            System.Text.Json.JsonSerializerOptions options)");
        sb.AppendLine("        {");
        sb.AppendLine(
            "            writer.WritePropertyName(value.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));");
        sb.AppendLine("        }");

        sb.AppendLine("    }");
        sb.AppendLine("    #endregion");
        sb.AppendLine();
    }

    private static string GetSystemTextJsonGetter(string underlying)
        => underlying switch
        {
            "byte"    => "Byte",
            "short"   => "Int16",
            "int"     => "Int32",
            "long"    => "Int64",
            "uint"    => "UInt32",
            "ulong"   => "UInt64",
            "float"   => "Single",
            "double"  => "Double",
            "decimal" => "Decimal",
            "string" => "String",
            _         => throw new NotSupportedException($"SystemTextJson getter not supported for {underlying}")
        };
    
    private static string GetAllowedJsonTokenType(string underlying)
        => underlying switch
        {
            "string" or "System.String"
                => "System.Text.Json.JsonTokenType.String",

            "byte" or "short" or "int" or "long" or
                "uint" or "ulong" or
                "float" or "double" or "decimal"
                => "System.Text.Json.JsonTokenType.Number",

            _ => throw new NotSupportedException(
                $"JSON token mapping not supported for {underlying}")
        };

}