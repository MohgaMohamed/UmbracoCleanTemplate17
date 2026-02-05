using Umbraco.Cms.Web.Common.AspNetCore;

namespace MOHPortal.Core.Umbraco.TemplateConstants
{
    public static class MediaCrops
    {
        #region General
        public const string SmallSquare = nameof(SmallSquare);
        public const string MediumSquare = nameof(MediumSquare);
        public const string LargeSquare = nameof(LargeSquare);
        
        public const string Listing = nameof(Listing);
        public const string Details = nameof(Details);
        public const string Thumbnail = nameof(Thumbnail);
        #endregion

        #region Social media Icons
        public const string SocialMediaIconSmall = nameof(SocialMediaIconSmall);
        public const string SocialMediaIconMedium = nameof(SocialMediaIconMedium);
        public const string SocialMediaIconLarge = nameof(SocialMediaIconLarge);
        #endregion

        #region Website Logo
        public const string LogoWide = nameof(LogoWide);
        public const string LogoSquare = nameof(LogoSquare);
        #endregion

    }
}
