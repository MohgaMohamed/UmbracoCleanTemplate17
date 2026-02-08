using FayoumGovPortal.Core.Umbraco.Localization;
using NUglify.Helpers;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Extensions;

namespace  FayoumGovPortal.Core.Umbraco.NotificationHandlers

{
    internal class ContentTreeRulesValidationNotificationHandler(
        ILocalizedTextService localizedTextService,
        LocalizationWrapper localizationWrapper,
        IContentService contentService
        ) : INotificationHandler<ContentSavingNotification>
    {
        private readonly string[] _documentsInScope = [

   //         nameof(WebSiteRoot),

			////Arhive Document			
			//nameof(ArchiveDocumentsListing),
            

   //         // Frequently Asked Question
   //         nameof(FrequentlyAskedQuestionListing),

   //         // Media Center
   //         nameof(VideoGalleries),
   //         nameof(PhotoGalleries),
   //         nameof(EventsAndSeminarsListing),
   //         nameof(MediaCenter),

			////Pages
			//nameof(HomePage),
   //         nameof(HistoryListing),
   //         nameof(NewsListing),
   //         nameof(SearchPage),
   //         nameof(NewsletterList),
   //         nameof(AboutUs),
   //         nameof(WhoUs),
   //         nameof(ListingSubmissions),
   //         nameof(BookLibrary),
   //         nameof(Departments),
   //         nameof(PublicationsListing),
           

			////Sitemap
			//nameof(SiteMap),

			////Lookups
			//nameof(FAqtopics),
   //         nameof(ArchiveSubjectsContainer),
	

			////RootElements
			//nameof(ContentNotFoundPage),
   //         nameof(ContactUs),
   //         nameof(SiteSettings),
        ];

        public ILocalizedTextService LocalizedTextService { get; } = localizedTextService;
        public LocalizationWrapper Localization { get; } = localizationWrapper;
        public IContentService ContentService { get; } = contentService;

        public void Handle(ContentSavingNotification notification)
        {
            IEnumerable<IContent>? currentlySavingTargetEntities =
                notification.SavedEntities.Where(
                    entity => _documentsInScope.Any(docName => docName.Equals(entity.ContentType.Alias, StringComparison.OrdinalIgnoreCase))
                );

            if (currentlySavingTargetEntities != null && currentlySavingTargetEntities.Any())
            {
                currentlySavingTargetEntities.ForEach(currentlySavingEntity =>
                {
                    if (currentlySavingEntity != null)
                    {
                        //We Expect to find only One Document
                        IContent? contentOfSameType = ContentService.GetPagedOfTypes([currentlySavingEntity.ContentTypeId], 0, int.MaxValue, out long totalRecords, null).FirstOrDefault();

                        //We're trying to save an Entity Type which already exists.
                        if (contentOfSameType != null && contentOfSameType!.Id != currentlySavingEntity.Id)
                        {
                            string validationMessage = string.Format(Localization.ValidationSingleEntityAlreadyExists, GetLocalizedDocumentName(contentOfSameType),
                                        contentOfSameType.Name);

                            notification.CancelOperation(new EventMessage(Localization.CommonError, validationMessage, EventMessageType.Error));
                        }
                    }
                });
            }
        }

        private string GetLocalizedDocumentName(IContent document)
        {
            string[] documentName = document.ContentType.Name!.Replace("#", "").Split('_');
            return documentName.Length != 2
                ? "Document" : LocalizedTextService.Localize(documentName[0], documentName[1]);
        }
    }
}
