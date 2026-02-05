namespace MOHPortal.Core.Umbraco.GoogleRecaptcha.Models
{
    public class GoogleRecaptchaOptions
    {
        public const string GoogleRecaptchaSettings = "GoogleRecaptchaSettings";
        public string? ApiEndpoint { get; set; }
        public string? SiteKey { get; set; }
        public string? ClientSecret { get; set; }
        public double ScoreThreshold { get; set; } = 0.5;
    }
}
