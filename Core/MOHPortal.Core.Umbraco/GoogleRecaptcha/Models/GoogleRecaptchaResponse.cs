using System.Text.Json.Serialization;

namespace MOHPortal.Core.Umbraco.GoogleRecaptcha.Models
{
    internal record GoogleRecaptchaResponse
    {
        [JsonPropertyName("success")]
        public bool Success {get; set;}
        
        [JsonPropertyName("score")]
        public double? Score {get; set;}
        
        [JsonPropertyName("action")]
        public string Action {get; set;} = string.Empty;
        
        [JsonPropertyName("challenge_ts")]
        public DateTime ChallengeTS {get; set;}

        [JsonPropertyName("hostname")]
        public string Hostname { get; set; } = string.Empty;
        
        public override string ToString()
        {
            return $"Response Success: {Success} for action {Action} \n Score: {Score}, validated at {ChallengeTS}";
        }
    }   
}
