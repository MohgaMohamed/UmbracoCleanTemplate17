using System.Text.Json.Serialization;

namespace MOHPortal.Core.Umbraco.DocumentValidator.Base
{
    public sealed record MediaPickerEntry
    {
        [JsonPropertyName("key")]
        public Guid Key { get; set; }

        [JsonPropertyName("mediaKey")]
        public Guid MediaKey { get; set; }
    }

}