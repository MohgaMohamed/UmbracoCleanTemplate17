namespace MOHPortal.Core.Umbraco.StaticAssetBundling.Settings
{
    public class StaticFilesCacheSettings
    {
        public int MaxAgeInDays { get; set; }
        public List<string> CacheExtensions { get; set; } = [];
    }
}
