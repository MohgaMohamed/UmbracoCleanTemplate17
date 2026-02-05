using MOHPortal.Core.Umbraco.Localization;
using MOHPortal.Core.Umbraco.Model.Gen;
using System.Diagnostics.CodeAnalysis;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;

namespace MOHPortal.Core.Umbraco.ProtectedContent
{
    internal class ProtectedContentHelper
    {
        private const string ProtectedContentCompositionAlias = ProtectedContentComposition.ModelTypeAlias;
        private IContentTypeService ContentTypeService { get; }
        private IContentService ContentService {get;}
        private IPublishedContentQuery PublishedContentQuery {get; }
        private LocalizationWrapper Localization { get; }

        public ProtectedContentHelper(IContentTypeService contentTypeService, IContentService contentService, LocalizationWrapper localization, IPublishedContentQuery publishedContentQuery)
        {
            ContentTypeService = contentTypeService;
            ContentService = contentService;
            Localization = localization;
            PublishedContentQuery = publishedContentQuery;
        }

        /// <summary>
        /// Determines whether the passed IContent item is inherits the 
        /// ProtectedContentComposition
        /// </summary>
        /// <param name="content"></param>
        /// <returns>true if the content item implements ProtectedContentComposition</returns>
        public bool IsProtected(IContent content)
        {
            IContentType? contentType = ContentTypeService.Get(content.ContentTypeId);
            return contentType is not null && contentType.ContentTypeCompositionExists(ProtectedContentCompositionAlias);
        }

        public bool ProtectedContentExists(int contentId, int contentTypeId, [NotNullWhen(true)] out string? validationMessage)
        {
            validationMessage = default;
            IContent? existingProtectedContent = ContentService.GetPagedOfType(contentTypeId, default, int.MaxValue, out long totalRecords, default!).FirstOrDefault();
            if (existingProtectedContent != null && existingProtectedContent!.Id != contentId)
            {
                validationMessage = Localization.ReplacePlaceholderIfExist(
                    Localization.ValidationSingleEntityAlreadyExists,
                    Localization.GetLocalizedPropertyName(existingProtectedContent.ContentType.Name!),
                    existingProtectedContent.Name!
                );
                return true;
            }

            return false;
        }

        public bool ProtectedContentExistsWithinParent(IContent content, [NotNullWhen(true)] out string? validationMessage)
        {
            validationMessage = default;
            if(content.ParentId == -1)
            {
                if(PublishedContentQuery.ContentAtRoot().FirstOrDefault(x => x.Id != content.Id && x.ContentType.Alias == content.ContentType.Alias)
                    is IPublishedContent existingRootContent)
                {
                    IContentType? contentType = ContentTypeService.Get(existingRootContent.ContentType.Id);
                    validationMessage = Localization.ReplacePlaceholderIfExist(
                        Localization.ValidationSingleEntityAlreadyExists,
                        Localization.GetLocalizedPropertyName(contentType?.Name ?? string.Empty),
                        existingRootContent.Name
                    );
                    return true;
                }

                return false;
            }


            global::Umbraco.Cms.Core.Models.PublishedContent.IPublishedContent? publishedParent = PublishedContentQuery.Content(content.ParentId);
            if(publishedParent is null)
            {
                validationMessage = Localization.CommonSomethingWentWrong;
                return true;
            }

            if(publishedParent.Children.FirstOrDefault(x => x.Id != content.Id && x.ContentType.Alias == content.ContentType.Alias) is IPublishedContent existingProtectedContent)
            {
                IContentType? contentType = ContentTypeService.Get(existingProtectedContent.ContentType.Id);
                validationMessage = Localization.ReplacePlaceholderIfExist(
                    Localization.ValidationSingleEntityAlreadyExists,
                    Localization.GetLocalizedPropertyName(contentType?.Name ?? string.Empty),
                    existingProtectedContent.Name
                );
                return true;
            }

            return false;
        }
    }
}
