using System.Text.Json.Serialization;

namespace MOHPortal.Core.Umbraco.JsonBlocklist.Models
{
    public class JsonBlockListContentUdi
    {
        [JsonPropertyName("contentUdi")]
        public required string ContentUdi { get; set; }
    }
}
