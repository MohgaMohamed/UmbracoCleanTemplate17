using Umbraco.Cms.Core.Models;

namespace MOHPortal.Core.Umbraco.SiteSettings
{
    public interface ISiteSettingsService
    {
        MediaWithCrops? DefaultWebsiteImage { get; }
        string? DefaultWebsiteImageUrl { get; }
        string? GoogleAnalyticsTrackingCode { get; }
    }
}