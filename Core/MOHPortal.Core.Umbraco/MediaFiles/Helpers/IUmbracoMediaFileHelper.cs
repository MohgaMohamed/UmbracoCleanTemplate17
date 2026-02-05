using Microsoft.AspNetCore.Http;
using Umbraco.Cms.Core.Models;

namespace MOHPortal.Core.Umbraco.MediaFiles.Helpers
{
    public interface IUmbracoMediaFileHelper
    {
        /// <summary>
        /// Saves a stream-based file to the Umbraco media library.
        /// </summary>
        /// <param name="fileStream">The stream containing the file data.</param>
        /// <param name="fileName">The name to be used for the saved file.</param>
        /// <param name="mediaTypeAlias">The Umbraco media type (e.g., "Image", "File").</param>
        /// <param name="fileDirectory">Optional root directory for the file.</param>
        /// <returns>The saved media item, or null if the operation fails.</returns>
        IMedia? SaveMediaFile(Stream fileStream, string fileName, string mediaTypeAlias, string? fileDirectory = null, bool force = false);

        /// <summary>
        /// Saves an uploaded file (IFormFile) to the Umbraco media library.
        /// </summary>
        /// <param name="formFile">The uploaded file from an HTTP request.</param>
        /// <param name="mediaTypeAlias">The Umbraco media type (e.g., "Image", "File").</param>
        /// <param name="fileDirectory">Optional root directory for the file.</param>
        /// <returns>The saved media item, or null if the operation fails.</returns>
        IMedia? SaveMediaFile(IFormFile formFile, string mediaTypeAlias, string? fileDirectory = null, bool force = false);
    }
}
