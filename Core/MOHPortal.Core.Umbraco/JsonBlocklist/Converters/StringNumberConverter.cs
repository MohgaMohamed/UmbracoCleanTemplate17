using System.Text.Json;
using System.Text.Json.Serialization;

namespace MOHPortal.Core.Umbraco.JsonBlocklist.Converters
{
    public class StringNumberConverter : JsonConverter<int>
    {   
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType switch
        {
            JsonTokenType.True => 1,
            JsonTokenType.False => 0,
            JsonTokenType.String => int.TryParse(reader.GetString(), out var b) ? b : throw new JsonException(),
            JsonTokenType.Number => reader.TryGetInt32(out int value) ? value : default,
            _ => throw new JsonException(),
        };

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
