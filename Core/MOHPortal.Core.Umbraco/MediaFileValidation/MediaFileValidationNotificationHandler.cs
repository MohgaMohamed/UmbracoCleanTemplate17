using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using System.Text.Json;
using MOHPortal.Core.Umbraco.Localization;
using Umbraco.Cms.Core;
using MOHPortal.Core.Umbraco.MediaFileValidation.Models;

namespace MOHPortal.Core.Umbraco.MediaFileValidation
{
    public class MediaFileValidationNotificationHandler : INotificationHandler<MediaSavingNotification>
    {
        private readonly LocalizationWrapper _localization;
        private readonly HashSet<string> _allowedExtensions;
        private readonly List<string> _allowedFileExtensions;
        private readonly Dictionary<string, int> _mediaSizes;
        private readonly IMediaService _mediaService;
        private readonly MediaFileManager _mediaFileManager;

        public MediaFileValidationNotificationHandler(MediaNotificationHandlerSettings mediaNotificationHandlerSettings, LocalizationWrapper localization, IMediaService mediaService, MediaFileManager mediaFileManager)
        {
            _localization = localization;
            _mediaService = mediaService;
            _mediaFileManager = mediaFileManager;

            _allowedExtensions = mediaNotificationHandlerSettings.AllowedExtensions?
                .Split(',')
                .Select(e => e.Trim().ToLower())
                .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? [];

            _allowedFileExtensions = mediaNotificationHandlerSettings.AllowedFileExtensions?
                .Split(',')
                .Select(e => e.Trim().ToLower())?.ToList() ?? [];

            _mediaSizes = new Dictionary<string, int>
            {
                { Constants.Conventions.MediaTypes.Image, ParseSizeToBytes(mediaNotificationHandlerSettings.ImageSizeInMegaByte) },
                { Constants.Conventions.MediaTypes.VideoAlias, ParseSizeToBytes(mediaNotificationHandlerSettings.VideoSizeInMegaByte) },
                { Constants.Conventions.MediaTypes.VectorGraphicsAlias, ParseSizeToBytes(mediaNotificationHandlerSettings.SVgsizeInMegaByte) },
                { Constants.Conventions.MediaTypes.File, ParseSizeToBytes(mediaNotificationHandlerSettings.FileSizeInMegaByte) },
                { Constants.Conventions.MediaTypes.ArticleAlias, ParseSizeToBytes(mediaNotificationHandlerSettings.ArticleSizeInMegaByte) },
                { Constants.Conventions.MediaTypes.AudioAlias, ParseSizeToBytes(mediaNotificationHandlerSettings.AudioSizeInMegaByte) },
            };
        }

        public void Handle(MediaSavingNotification notification)
        {
            foreach (var mediaItem in notification.SavedEntities)
            {
                var contentTypeAlias = mediaItem.ContentType.Alias;
                if (contentTypeAlias == Constants.Conventions.MediaTypes.Folder)
                {
                    continue; // Skip folders
                }

                string? filePath = mediaItem.GetValue<string>(Constants.Conventions.Media.File);

                UmbracoFilePathProperty? umbracoFilePathProperty = !string.IsNullOrWhiteSpace(filePath) && contentTypeAlias != Constants.Conventions.MediaTypes.File
                ? JsonSerializer.Deserialize<UmbracoFilePathProperty>(filePath)
                : new UmbracoFilePathProperty(filePath!);

                string? fileExtension = Path.GetExtension(umbracoFilePathProperty?.Source)?.ToLower();
                if (contentTypeAlias == Constants.Conventions.MediaTypes.File &&
                    (string.IsNullOrWhiteSpace(fileExtension) || !_allowedFileExtensions.Contains(fileExtension)))
                {
                    CancelOperation(notification, _localization.CommonFileType, _localization.ValidationFileFormatPDF, umbracoFilePathProperty);
                    continue;
                }

                if (_allowedExtensions.Count > default(int))
                {
                    if (string.IsNullOrWhiteSpace(fileExtension) || !_allowedExtensions.Contains(fileExtension))
                    {
                        CancelOperation(notification, _localization.CommonFileType, _localization.ValidationInvalidFileFormat, umbracoFilePathProperty);
                        continue;
                    }
                }

                if (_mediaSizes.TryGetValue(contentTypeAlias, out int maxSize))
                {
                    if (mediaItem.GetValue<int>(Constants.Conventions.Media.Bytes) > maxSize)
                    {
                        CancelOperation(notification, _localization.CommonFileSize, _localization.ValidationLargeFileSize, umbracoFilePathProperty);
                    }
                }
                else
                {
                    CancelOperation(notification, _localization.CommonFileType, _localization.ValidationInvalidFileFormat, umbracoFilePathProperty);
                }
            }
        }

        private static int ParseSizeToBytes(int size) => size * 1024 * 1024;
        private void CancelOperation(MediaSavingNotification notification, string title, string message, UmbracoFilePathProperty? umbracoFilePath)
        {
            if (umbracoFilePath != null)
            {
                _mediaService.DeleteMediaFile(umbracoFilePath.Source);
                _mediaFileManager.FileSystem.DeleteDirectory(Path.GetDirectoryName(umbracoFilePath.Source) ?? string.Empty);
            }
            notification.CancelOperation(new EventMessage(title, message, EventMessageType.Error));
        }
    }
}
