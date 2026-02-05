using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Net.Http.Headers;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Extensions;
using IHostingEnvironment = Umbraco.Cms.Core.Hosting.IHostingEnvironment;
using MOHPortal.Core.Umbraco.StaticAssetBundling.Settings;
using Microsoft.Extensions.Configuration;


namespace MOHPortal.Core.Umbraco.StaticAssetBundling.Configuration
{
    public class StaticFilesCacheConfiguration : IConfigureOptions<StaticFileOptions>
    {
        private readonly string _backOfficePath;
        private readonly StaticFilesCacheSettings _cacheSettings;
        private readonly IConfiguration _configurations;

        //public StaticFilesCacheConfiguration(
        //    IOptions<GlobalSettings> globalSettings,
        //    IHostingEnvironment hostingEnvironment,
        //    IOptions<StaticFilesCacheSettings> cacheSettings)
        //{
        //    _backOfficePath = globalSettings.Value.GetBackOfficePath(hostingEnvironment);
        //    _cacheSettings = cacheSettings.Value;
        //}

        public StaticFilesCacheConfiguration(
    IOptions<GlobalSettings> globalSettings,
    IHostingEnvironment hostingEnvironment, // You can likely remove this parameter now if unused elsewhere
    IOptions<StaticFilesCacheSettings> cacheSettings, IConfiguration configurations)
        {
            _configurations = configurations;
            _backOfficePath = _configurations.GetValue<string>("Umbraco:CMS:Global:UmbracoPath") ?? "/umbraco";
            _cacheSettings = cacheSettings.Value;
        }

        public void Configure(StaticFileOptions options)
            => options.OnPrepareResponse = ctx =>
            {
                // Exclude Umbraco backoffice assets
                if (ctx.Context.Request.Path.StartsWithSegments(_backOfficePath))
                {
                    return;
                }

                // Set headers for specific file extensions
                string fileExtension = Path.GetExtension(ctx.File.Name);
                if (_cacheSettings.CacheExtensions.Contains(fileExtension))
                {
                    ResponseHeaders headers = ctx.Context.Response.GetTypedHeaders();

                    CacheControlHeaderValue cacheControl = headers.CacheControl ?? new CacheControlHeaderValue();
                    cacheControl.Public = true;
                    cacheControl.MaxAge = TimeSpan.FromDays(_cacheSettings.MaxAgeInDays);
                    headers.CacheControl = cacheControl;
                }
            };
    }
}
