using System.Text.Json;
using System.Text.Json.Serialization;

namespace Optivem.EShop.Monolith.Core.Converters;

public class EmptyStringToNullIntConverter : JsonConverter<int?>
{
    public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            
            // Convert empty string to null to trigger Required validation
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return null;
            }
            
            // Try to parse the string as an integer
            if (int.TryParse(stringValue, out var result))
            {
                return result;
            }
            
            // Invalid format - return null to trigger validation
            return null;
        }
        
        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt32();
        }
        
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }
        
        throw new JsonException($"Unable to convert to int?");
    }

    public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteNumberValue(value.Value);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
