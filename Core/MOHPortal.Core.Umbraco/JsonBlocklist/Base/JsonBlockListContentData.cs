using System.Text.Json.Serialization;

namespace MOHPortal.Core.Umbraco.JsonBlocklist.Base
{
    public abstract class JsonBlockListContentData
    {
        [JsonPropertyName("contentTypeKey")]
        public required Guid ContentTypeKey { get; set; }

        [JsonPropertyName("udi")]
        public required string Udi { get; set; }
    }
}
