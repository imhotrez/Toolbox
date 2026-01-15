namespace Toolbox.ValueObjects;

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using CodeGeneration.Attributes;

[ValueObject(typeof(int))]
public readonly partial struct OrderId
{
    public class OrderIdConverter : JsonConverter<OrderId>
    {
        private readonly NumberFormatInfo _numberFormatInfo = new NumberFormatInfo { NumberGroupSeparator = "" };

        public override OrderId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                int value;
                if (!int.TryParse(reader.GetString(), out value))
                    throw new JsonException("Invalid OrderId format.");

                return OrderId.Create(value);
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                int value = reader.GetInt32();
                return OrderId.Create(value);
            }

            throw new JsonException("Unexpected token type when reading OrderId.");
        }

        public override void Write(Utf8JsonWriter writer, OrderId value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value.ToString(_numberFormatInfo));
        }
    }
}