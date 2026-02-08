using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using FayoumGovPortal.Core.Umbraco.StaticAssetBundling.Settings;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using Smidge; // Replacement for Umbraco.Cms.Core.WebAssets
using System.IO;
using System.Linq;

namespace  FayoumGovPortal.Core.Umbraco.StaticAssetBundling
{
    internal class AssetsBundlingAndMinificationNotificationHandler
        : INotificationHandler<UmbracoApplicationStartingNotification>
    {
        private readonly IBundleManager _bundleManager; // Native Smidge Service
        private readonly IRuntimeState _runtimeState;
        private readonly IWebHostEnvironment _environment;
        private readonly AssetBundlingSettings _bundlingSettings;

        public AssetsBundlingAndMinificationNotificationHandler(
            IBundleManager bundleManager,
            IRuntimeState runtimeState,
            IWebHostEnvironment environment,
            IOptions<AssetBundlingSettings> bundlingSettings)
        {
            _bundleManager = bundleManager;
            _runtimeState = runtimeState;
            _environment = environment;
            _bundlingSettings = bundlingSettings.Value;
        }

        public void Handle(UmbracoApplicationStartingNotification notification)
        {
            // Note: Smidge handles Dev/Prod optimization automatically based on environment
            if (_runtimeState.Level is RuntimeLevel.Run)
            {
                // Register CSS Bundle
                string[] cssFiles = GetAssetFiles(".css", _bundlingSettings.StylesheetsPath);
                if (cssFiles.Any())
                {
                    _bundleManager.CreateCss("site-css-bundle", cssFiles);
                }

                // Register JS Bundle
                string[] jsFiles = GetAssetFiles(".js", _bundlingSettings.ScriptsPath);
                if (jsFiles.Any())
                {
                    _bundleManager.CreateJs("site-js-bundle", jsFiles);
                }
            }
        }

        private string[] GetAssetFiles(string extension, string rootPath)
        {
            // Ensure path exists and adjust extension check (Path.GetExtension includes the dot)
            if (!Directory.Exists(rootPath)) return System.Array.Empty<string>();

            return Directory
                .GetFiles(rootPath)
                .Where(f => Path.GetExtension(f).Equals(extension, System.StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }
    }
}
