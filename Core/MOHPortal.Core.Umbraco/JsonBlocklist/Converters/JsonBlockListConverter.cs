using System.Text.Json;
using System.Text.Json.Serialization;
using FayoumGovPortal.Core.Umbraco.JsonBlocklist.Base;
using FayoumGovPortal.Core.Umbraco.JsonBlocklist.Models;

namespace FayoumGovPortal.Core.Umbraco.JsonBlocklist.Converters
{
    public class JsonBlockListConverter<TContentData> : JsonConverter<JsonBlockList<TContentData>> 
        where TContentData : JsonBlockListContentData
    {
        public override JsonBlockList<TContentData>? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (reader.TokenType != JsonTokenType.String)
            {
                // Direct JSON object - deserialize normally
                return JsonSerializer.Deserialize<JsonBlockList<TContentData>>(
                    ref reader,
                    new JsonSerializerOptions 
                    { 
                        PropertyNameCaseInsensitive = true 
                    });
            }

            // String value - use factory to parse
            string? rawValue = reader.GetString();
            if (string.IsNullOrWhiteSpace(rawValue))
                return null;

            JsonBlocklistFactory<TContentData> factory = JsonBlocklistFactory<TContentData>.CreateFromRawValue(rawValue);
            return factory.CanBuild ? factory.Build() : null;
        }

        public override void Write(
            Utf8JsonWriter writer,
            JsonBlockList<TContentData> value,
            JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            string json = JsonSerializer.Serialize(value, options);
            writer.WriteStringValue(json);
        }
    }
} 