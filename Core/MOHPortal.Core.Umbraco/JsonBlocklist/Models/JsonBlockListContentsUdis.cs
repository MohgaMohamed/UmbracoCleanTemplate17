using System.Text.Json.Serialization;

namespace FayoumGovPortal.Core.Umbraco.JsonBlocklist.Models
{
    public class JsonBlockListContentsUdis
    {
        [JsonPropertyName("Umbraco.BlockList")]
        public List<JsonBlockListContentUdi> UmbracoBlockList { get; set; } = [];

        [JsonPropertyName("Umbraco.BlockGrid")]
        public List<JsonBlockListContentUdi> UmbracoBlockGrid { get; set; } = [];
    }
}
