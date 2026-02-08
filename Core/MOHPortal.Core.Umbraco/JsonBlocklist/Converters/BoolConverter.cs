using System.Text.Json;
using System.Text.Json.Serialization;

namespace FayoumGovPortal.Core.Umbraco.JsonBlocklist.Converters
{
    public class BoolConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            bool val = reader.TokenType switch
            {
                JsonTokenType.True => true,
                JsonTokenType.False => false,
                JsonTokenType.String => bool.TryParse(reader.GetString(), out bool b) ? b : this.HandleStringValue(reader.GetString()),
                JsonTokenType.Number => reader.TryGetInt64(out long l) ? Convert.ToBoolean(l) : reader.TryGetDouble(out double d) && Convert.ToBoolean(d),
                _ => throw new JsonException(),
            };
            return val;
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteBooleanValue(value);
        }

        private bool HandleStringValue(string? s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return false;
            }


            return s.Equals("1");
        }
    }
}
