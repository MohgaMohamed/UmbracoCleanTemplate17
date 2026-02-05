using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MOHPortal.Core.Umbraco.GoogleRecaptcha.Contracts;
using MOHPortal.Core.Umbraco.GoogleRecaptcha.Models;
using MOHPortal.Core.Umbraco.GoogleRecaptcha;
using MOHPortal.Core.Umbraco.Localization;
using MOHPortal.Core.Umbraco.SEO.Contracts;
using MOHPortal.Core.Umbraco.SEO;
using MOHPortal.Core.Umbraco.Validation.Configuration;
using MOHPortal.Core.Umbraco.Validation.Contracts;
using MOHPortal.Core.Umbraco.Validation;
using Umbraco.Cms.Core.Notifications;
using MOHPortal.Core.Umbraco.SiteSettings;
using MOHPortal.Core.Umbraco.ContentSearch.Configurations;
using MOHPortal.Core.Umbraco.StaticAssetBundling.Settings;
using MOHPortal.Core.Umbraco.StaticAssetBundling.Configuration;
using MOHPortal.Core.Umbraco.ContentSearch;
using MOHPortal.Core.Umbraco.DocumentValidator;
using MOHPortal.Core.Umbraco.MediaFiles;
using MOHPortal.Core.Umbraco.PageServices;
using MOHPortal.Core.Umbraco.MediaFiles.Helpers;
using MOHPortal.Core.Umbraco.MediaFiles.Models;
using MOHPortal.Core.Umbraco.ProtectedContent;
//using MOHPortal.Core.Umbraco.Authentication;
using MOHPortal.Core.Umbraco.NotificationHooks.NotificationHandlers;
using MOHPortal.Core.Umbraco.NotificationHooks;
using MOHPortal.Core.Umbraco.uSync;
using uSync.BackOffice;


namespace MOHPortal.Core.Umbraco
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
