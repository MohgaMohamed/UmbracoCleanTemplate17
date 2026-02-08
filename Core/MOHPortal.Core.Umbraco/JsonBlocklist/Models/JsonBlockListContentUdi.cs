using System.Text.Json.Serialization;

namespace FayoumGovPortal.Core.Umbraco.JsonBlocklist.Models
{
    public class JsonBlockListContentUdi
    {
        [JsonPropertyName("contentUdi")]
        public required string ContentUdi { get; set; }
    }
}
