using Lucene.Net.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Infrastructure.Scoping;

namespace FayoumGovPortal.Core.Umbraco.MediaFiles.Helpers
{
    /// <summary>
    /// Helper class for managing media files in Umbraco's media library.
    /// Handles file uploads, directory creation, and media storage.
    /// </summary>
    internal class UmbracoMediaFileHelper : IUmbracoMediaFileHelper
    {
        #region Private Fields

        private readonly IMediaService _mediaService;
        private readonly IMediaTypeService _mediaTypeService;
        private readonly MediaFileManager _mediaFileManager;
        private readonly MediaUrlGeneratorCollection _mediaUrlGeneratorCollection;
        private readonly IShortStringHelper _shortStringHelper;
        private readonly IContentTypeBaseServiceProvider _contentTypeBaseServiceProvider;
        private readonly ILogger<UmbracoMediaFileHelper> _logger;
        private readonly IScopeProvider _scopeProvider;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the UmbracoMediaFileHelper class.
        /// </summary>
        /// <param name="mediaService">Service for managing media items.</param>
        /// <param name="mediaTypeService">Service for managing media types.</param>
        /// <param name="mediaFileManager">Manager for media file operations.</param>
        /// <param name="mediaUrlGeneratorCollection">Collection of URL generators for media.</param>
        /// <param name="shortStringHelper">Helper for string operations.</param>
        /// <param name="contentTypeBaseServiceProvider">Provider for content type services.</param>
        /// <param name="logger">Logger for recording operations and errors.</param>
        /// <param name="scopeProvider">Provider for database scopes.</param>
        public UmbracoMediaFileHelper(
            IMediaService mediaService,
            IMediaTypeService mediaTypeService,
            MediaFileManager mediaFileManager,
            MediaUrlGeneratorCollection mediaUrlGeneratorCollection,
            IShortStringHelper shortStringHelper,
            IContentTypeBaseServiceProvider contentTypeBaseServiceProvider,
            ILogger<UmbracoMediaFileHelper> logger,
            IScopeProvider scopeProvider)
        {
            _mediaService = mediaService;
            _mediaTypeService = mediaTypeService;
            _mediaFileManager = mediaFileManager;
            _mediaUrlGeneratorCollection = mediaUrlGeneratorCollection;
            _shortStringHelper = shortStringHelper;
            _contentTypeBaseServiceProvider = contentTypeBaseServiceProvider;
            _logger = logger;
            _scopeProvider = scopeProvider;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves a stream-based file to the Umbraco media library within a database scope.
        /// </summary>
        /// <param name="fileStream">The stream containing the file data.</param>
        /// <param name="fileName">The name to be used for the saved file.</param>
        /// <param name="mediaTypeAlias">The Umbraco media type (e.g., "Image", "File").</param>
        /// <param name="fileDirectory">Optional directory path in format "folder1/folder2/folder3".</param>
        /// <returns>The saved media item, or null if the operation fails.</returns>
        /// <remarks>
        /// The method will:
        /// - Create any missing directories in the specified path
        /// - Save the file with the provided name
        /// - Return the media item for further processing
        /// </remarks>
        public IMedia? SaveMediaFile(Stream fileStream, string fileName, string mediaTypeAlias, string? fileDirectory = null, bool force = false)
        {
            if (fileStream == null || 
                string.IsNullOrWhiteSpace(mediaTypeAlias) || 
                string.IsNullOrWhiteSpace(fileName))
            {
                _logger.LogWarning("SaveMediaFile called with invalid parameters. FileName: {FileName}, MediaTypeAlias: {MediaTypeAlias}", 
                    fileName ?? "null", 
                    mediaTypeAlias ?? "null");
                return null;
            }

            try
            {
                using (IScope scope = _scopeProvider.CreateScope())
                {
                    int parentId = EnsureDirectoryStructure(fileDirectory);
                    IMedia media = _mediaService.CreateMedia(fileName, parentId, mediaTypeAlias);
                    media.SetValue(
                        _mediaFileManager, 
                        _mediaUrlGeneratorCollection, 
                        _shortStringHelper,
                        _contentTypeBaseServiceProvider, 
                        Constants.Conventions.Media.File, 
                        fileName, 
                        fileStream
                    );
                    media.SetValue(Constants.Conventions.Media.Bytes, fileStream.Length);

                    if (force)
                    {
                        using IDisposable suppressor = scope.Notifications.Suppress();
                        SaveMediaModel(media, fileName, mediaTypeAlias, fileDirectory);
                    }
                    else
                    {
                        SaveMediaModel(media, fileName, mediaTypeAlias, fileDirectory);
                    }
                    
                    scope.Complete();
                    _logger.LogInformation(
                        "Media file successfully saved. FileName: {FileName}, MediaTypeAlias: {MediaTypeAlias}, Directory: {FileDirectory}, MediaId: {MediaId}", 
                        fileName, 
                        mediaTypeAlias, 
                        fileDirectory ?? "root", 
                        media.Id
                    );

                    return media;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error saving media file. FileName: {FileName}, MediaTypeAlias: {MediaTypeAlias}, Directory: {FileDirectory}. Error: {ErrorMessage}", 
                    fileName, 
                    mediaTypeAlias, 
                    fileDirectory ?? "root", 
                    ex.Message);
                
                return null;
            }
        }

        /// <summary>
        /// Saves an uploaded file (IFormFile) to the Umbraco media library.
        /// </summary>
        /// <param name="formFile">The uploaded file from an HTTP request.</param>
        /// <param name="mediaTypeAlias">The Umbraco media type (e.g., "Image", "File").</param>
        /// <param name="fileDirectory">Optional directory path in format "folder1/folder2/folder3".</param>
        /// <param name="force">.</param>
        /// <returns>The saved media item, or null if the operation fails.</returns>
        /// <remarks>
        /// This method:
        /// - Validates the uploaded file
        /// - Opens a stream to read the file
        /// - Delegates to the stream-based SaveMediaFile method
        /// - Ensures proper disposal of resources
        /// </remarks>
        public IMedia? SaveMediaFile(IFormFile formFile, string mediaTypeAlias, string? fileDirectory = null, bool force = false)
        {
            if (formFile == null || formFile.Length == 0)
            {
                _logger.LogWarning("SaveMediaFile called with invalid IFormFile. FormFile is null or empty");
                return null;
            }

            try
            {
                using Stream fileStream = formFile.OpenReadStream();
                return SaveMediaFile(fileStream, formFile.FileName, mediaTypeAlias, fileDirectory, force);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error processing IFormFile for media upload. FileName: {FileName}, MediaTypeAlias: {MediaTypeAlias}, Directory: {FileDirectory}. Error: {ErrorMessage}",
                    formFile.FileName,
                    mediaTypeAlias,
                    fileDirectory ?? "root",
                    ex.Message);
                    
                return null;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Ensures the existence of a directory structure in the media library.
        /// </summary>
        /// <param name="fileDirectory">Directory path in format "folder1/folder2/folder3".</param>
        /// <returns>The ID of the deepest folder in the hierarchy, or -1 if no directory is specified.</returns>
        /// <remarks>
        /// This method:
        /// - Splits the directory path into segments
        /// - Creates each folder if it doesn't exist
        /// - Returns the ID of the final folder for file placement
        /// - Logs creation of new folders
        /// </remarks>
        private int EnsureDirectoryStructure(string? fileDirectory)
        {
            if (string.IsNullOrWhiteSpace(fileDirectory))
                return -1;

            try
            {
                int parentId = -1;
                string[] pathSegments = fileDirectory.Split('/', StringSplitOptions.RemoveEmptyEntries);
                
                foreach (string segment in pathSegments)
                {
                    if (string.IsNullOrWhiteSpace(segment))
                        continue;

                    IEnumerable<IMedia> children = _mediaService.GetPagedChildren(parentId, 0, int.MaxValue, out long totalRecords);
                    IMedia? existingFolder = children
                        .FirstOrDefault(m => m.Name!.Equals(segment, StringComparison.OrdinalIgnoreCase));

                    if (existingFolder == null)
                    {
                        IMedia folder = _mediaService.CreateMedia(
                            segment, 
                            parentId, 
                            Constants.Conventions.MediaTypes.Folder);
                        
                        _mediaService.Save(folder);
                        parentId = folder.Id;
                        
                        _logger.LogInformation(
                            "Created new media folder. Name: {FolderName}, ParentId: {ParentId}, NewFolderId: {FolderId}", 
                            segment, 
                            parentId, 
                            folder.Id);
                    }
                    else
                    {
                        parentId = existingFolder.Id;
                    }
                }

                return parentId;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error ensuring directory structure for path: {FileDirectory}. Error: {ErrorMessage}", 
                    fileDirectory, 
                    ex.Message);
                
                throw; // Re-throw to be handled by calling method
            }
        }


        private bool SaveMediaModel(IMedia media, string fileName, string mediaTypeAlias, string? fileDirectory)
        {
            Attempt<OperationResult?> result = _mediaService.Save(media);
            if (!result.Success)
            {
                _logger.LogError(
                    JsonSerializer.Serialize(result.Result),
                    "Error saving media file. FileName: {FileName}, MediaTypeAlias: {MediaTypeAlias}, Directory: {FileDirectory}. Error: {ErrorMessage}",
                    fileName,
                    mediaTypeAlias,
                    fileDirectory ?? "root",
                    result.Result?.EventMessages?.GetAll().Select(x => $"{x.Category}: {x.Message}")
                );
            }

            return result.Success;
        }
        #endregion
    }
}
