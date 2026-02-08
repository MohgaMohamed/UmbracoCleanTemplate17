using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using FayoumGovPortal.Core.Umbraco.GoogleRecaptcha.Contracts;
using FayoumGovPortal.Core.Umbraco.GoogleRecaptcha.Models;
using FayoumGovPortal.Core.Umbraco.GoogleRecaptcha;
using FayoumGovPortal.Core.Umbraco.Localization;
using FayoumGovPortal.Core.Umbraco.SEO.Contracts;
using FayoumGovPortal.Core.Umbraco.SEO;
using FayoumGovPortal.Core.Umbraco.Validation.Configuration;
using FayoumGovPortal.Core.Umbraco.Validation.Contracts;
using FayoumGovPortal.Core.Umbraco.Validation;
using Umbraco.Cms.Core.Notifications;
using FayoumGovPortal.Core.Umbraco.SiteSettings;
using FayoumGovPortal.Core.Umbraco.ContentSearch.Configurations;
using FayoumGovPortal.Core.Umbraco.StaticAssetBundling.Settings;
using FayoumGovPortal.Core.Umbraco.StaticAssetBundling.Configuration;
using FayoumGovPortal.Core.Umbraco.ContentSearch;
using FayoumGovPortal.Core.Umbraco.DocumentValidator;
using FayoumGovPortal.Core.Umbraco.MediaFiles;
using FayoumGovPortal.Core.Umbraco.PageServices;
using FayoumGovPortal.Core.Umbraco.MediaFiles.Helpers;
using FayoumGovPortal.Core.Umbraco.MediaFiles.Models;
using FayoumGovPortal.Core.Umbraco.ProtectedContent;
//using FayoumGovPortal.Core.Umbraco.Authentication;
using FayoumGovPortal.Core.Umbraco.NotificationHooks.NotificationHandlers;
using FayoumGovPortal.Core.Umbraco.NotificationHooks;
using FayoumGovPortal.Core.Umbraco.uSync;
using uSync.BackOffice;


namespace  FayoumGovPortal.Core.Umbraco
{
    public static class UmbracoLibraryBuilderExtensions
    {
        public static IUmbracoBuilder AddUmbracoLibrary(this IUmbracoBuilder builder, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                builder.AddCompiledRazorFiles(environment);
            }

            builder.AddCoreNotificationHandlers();

            if (environment.IsProduction()) 
            {
                //TODO [AS]: fix for running on server
                //builder.AddCachingAndBundling(builder.Config);
            }
            
            builder.Services
                .AddApplicationServices(builder.Config)
                .AddValidationHelper(builder.Config)
                .AddMediaNotificationHelper(builder.Config);

            return builder;
        }

        private static IUmbracoBuilder AddCoreNotificationHandlers(this IUmbracoBuilder builder)
        {
            builder
                .AddNotificationAsyncHandler<ContentSavingNotification, DocumentValidationNotificationHandler>()
                .AddNotificationAsyncHandler<ContentSavingNotification, ContentSavingNotificationHandler>()
                .AddNotificationAsyncHandler<uSyncImportStartingNotification, uSyncNotificationManager>()
                .AddNotificationAsyncHandler<uSyncImportCompletedNotification, uSyncNotificationManager>()
                .AddNotificationHandler<ContentSavingNotification, ProtectedContentSavingNotificationHandler>()
                .AddNotificationHandler<ContentMovingToRecycleBinNotification, ProtectedContentDeletingNotificationHandler>()
                .AddNotificationHandler<MediaSavingNotification, MediaFileValidationNotificationHandler>()
                .Services
                .AddSingleton<NotificationStateManager>();

            return builder;
        }

        private static IUmbracoBuilder AddCompiledRazorFiles(this IUmbracoBuilder builder, IWebHostEnvironment environment)
        {
            string[] dirs = [
                   "Core/MOHPortal.Core.Umbraco"
            ];

            IMvcBuilder mvcBuilder = builder.Services.AddRazorPages();
            mvcBuilder.AddRazorRuntimeCompilation(options =>
            {
                // add each to options file providers
                foreach (var dir in dirs)
                {
                    string libraryPath = Path.GetFullPath(Path.Combine(environment.ContentRootPath, "../..", dir));
                    options.FileProviders.Add(new PhysicalFileProvider(libraryPath));
                }
            });

            return builder;
        }
        
        private static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                //.AddSingleton<IEmailHelper, EmailHelper>()
                .AddSingleton<LocalizationWrapper>();

            services
                .AddScoped<ProtectedContentHelper>()
                .AddScoped<IGoogleRecaptchaHelper, GoogleRecaptchaHelper>()
                .AddScoped<ISiteSettingsService, SiteSettingsService>()
                .AddScoped<ISEOService, SEOService>()
                .AddScoped<IUmbracoPageService, UmbracoPageService>()
                //.AddScoped<IMemberAuthenticationService, MemberAuthenticationService>()
                .AddScoped<IUmbracoMediaFileHelper, UmbracoMediaFileHelper>()
                .AddScoped<ISearchService, SearchService>();

            return services
                .ConfigureOptions<ExamineExternalIndexConfigurations>()
                .Configure<GoogleRecaptchaOptions>(configuration.GetSection(GoogleRecaptchaOptions.GoogleRecaptchaSettings));
        }

        private static IServiceCollection AddValidationHelper(this IServiceCollection services, IConfiguration configuration)
        {
            ValidationHelperSettings validationHelperSettings = new();
            configuration.Bind(nameof(ValidationHelperSettings), validationHelperSettings);

            return services
                .AddSingleton<IValidationHelper, ValidationHelper>()
                .AddSingleton(validationHelperSettings);
        }

        private static IServiceCollection AddMediaNotificationHelper(this IServiceCollection services, IConfiguration configuration)
        {
            MediaNotificationHandlerSettings mediaNotificationHandlerSettings = new();
            configuration.Bind(nameof(MediaNotificationHandlerSettings), mediaNotificationHandlerSettings);

            return services.AddSingleton(mediaNotificationHandlerSettings);
        }

        private static IUmbracoBuilder AddCachingAndBundling(this IUmbracoBuilder builder, IConfiguration configuration)
        {
            //Static Files Cache Policy
            builder.Services.Configure<StaticFilesCacheSettings>(configuration.GetSection(nameof(StaticFilesCacheSettings)));
            builder.Services.AddTransient<IConfigureOptions<StaticFileOptions>, StaticFilesCacheConfiguration>();

            //Asset Bundling and Minification
            //builder.AddNotificationHandler<UmbracoApplicationStartingNotification, AssetsBundlingAndMinificationNotificationHandler>();

            return builder;
        }
    }
}
