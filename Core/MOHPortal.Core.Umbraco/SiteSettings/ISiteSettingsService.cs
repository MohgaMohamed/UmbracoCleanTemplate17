using Umbraco.Cms.Core.Models;

namespace  FayoumGovPortal.Core.Umbraco.SiteSettings
{
    public interface ISiteSettingsService
    {
        MediaWithCrops? DefaultWebsiteImage { get; }
        string? DefaultWebsiteImageUrl { get; }
        string? GoogleAnalyticsTrackingCode { get; }
    }
}