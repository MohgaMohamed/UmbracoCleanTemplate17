using MOHPortal.Core.Umbraco.Validation.Contracts;
using Umbraco.Cms.Core.Services;

namespace MOHPortal.Core.Umbraco.Localization
{
    public class LocalizationWrapper
    {
        private ILocalizedTextService LocalizedTextService { get; }
        private IValidationHelper ValidationHelper { get; }

        public LocalizationWrapper(ILocalizedTextService localizedTextService, IValidationHelper validationHelper)
        {
            LocalizedTextService = localizedTextService;
            ValidationHelper = validationHelper;
        }

        #region Common
        private readonly string _commonMessagesAreaName = "Common";
        public string CommonKeywords => GetLocalizedValue(_commonMessagesAreaName, "Keywords");
        public string CommonSelection => GetLocalizedValue(_commonMessagesAreaName, "Selection");
        public string CommonShow => GetLocalizedValue(_commonMessagesAreaName, "Show");
        public string CommonSource => GetLocalizedValue(_commonMessagesAreaName, "Source");
        public string CommonNext => GetLocalizedValue(_commonMessagesAreaName, "Next");
        public string CommonPrevious => GetLocalizedValue(_commonMessagesAreaName, "Previous");
        public string CommonNoResults => GetLocalizedValue(_commonMessagesAreaName, "NoResults");
        public string CommonSearch => GetLocalizedValue(_commonMessagesAreaName, "Search");
        public string CommonLink => GetLocalizedValue(_commonMessagesAreaName, "Link");
        public string CommonSettings => GetLocalizedValue(_commonMessagesAreaName, "Settings");
        public string CommonShowMore => GetLocalizedValue(_commonMessagesAreaName, "ShowMore");
        public string CommonShowDetails => GetLocalizedValue(_commonMessagesAreaName, "ShowDetails");
        public string CommonWebsiteRoot => GetLocalizedValue(_commonMessagesAreaName, "WebsiteRoot");
        public string CommonContentNotFoundPage => GetLocalizedValue(_commonMessagesAreaName, "ContentNotFoundPage");
        public string CommonTitle => GetLocalizedValue(_commonMessagesAreaName, "Title");
        public string CommonSearchResults => GetLocalizedValue(_commonMessagesAreaName, "SearchResults");
        public string CommonSearchPage => GetLocalizedValue(_commonMessagesAreaName, "SearchPage");
        public string CommonSearchResultsMessage => GetLocalizedValue(_commonMessagesAreaName, "SearchResultsMessage");
        public string CommonSomethingWentWrong => GetLocalizedValue(_commonMessagesAreaName, "SomethingWentWrong");
        public string CommonFrom => GetLocalizedValue(_commonMessagesAreaName, "From");
        public string CommonAll => GetLocalizedValue(_commonMessagesAreaName, "All");
        public string CommonFileType => GetLocalizedValue(_commonMessagesAreaName, "FileType");
        public string CommonFileSize => GetLocalizedValue(_commonMessagesAreaName, "FileSize");
        public string CommonError => GetLocalizedValue(_commonMessagesAreaName, "Error");
        public string CommonOpen => GetLocalizedValue(_commonMessagesAreaName, "Open");
        public string CommonDownload => GetLocalizedValue(_commonMessagesAreaName, "Download");
        public string CommonSuccess => GetLocalizedValue(_commonMessagesAreaName, "Success");
        public string ValidationUnpublishedTopicInLanguage => GetLocalizedValue(_commonMessagesAreaName, "LanguageValidation");
        public string CommonError404 => GetLocalizedValue(_commonMessagesAreaName, "Error404");
        public string CommonBackToHomePage => GetLocalizedValue(_commonMessagesAreaName, "BackToHomePage");
        #endregion

        #region Validation
        private readonly string _validationAreaName = "Validation";
        public string ValidationRequired => GetLocalizedValue(_validationAreaName, "Required");
        public string ValidationLettersOnly => GetLocalizedValue(_validationAreaName, "LettersOnly");
        public string ValidationNumbersOrLettersOnly => GetLocalizedValue(_validationAreaName, "NumbersOrLettersOnly");
        public string ValidationInvalidLinkFormat => GetLocalizedValue(_validationAreaName, "InvalidLinkFormat");
        public string ValidationInvalidEmailFormat => GetLocalizedValue(_validationAreaName, "InvalidEmailFormat");
        public string ValidationInvalidLink => GetLocalizedValue(_validationAreaName, "InvalidLink");
        public string ValidationFileFormatPDF => GetLocalizedValue(_validationAreaName, "FileFormatPDF");
        public string ValidationNumbersOnly => GetLocalizedValue(_validationAreaName, "NumbersOnly");
        public string ValidationSuccessfullySubscribed => GetLocalizedValue(_validationAreaName, "SuccessfullySubscribed");
        public string ValidationDateAfterTheCurrentDate => GetLocalizedValue(_validationAreaName, "DateAfterTheCurrentDate");
        public string ValidationMailboxCheckForUnsubscribingProcess => GetLocalizedValue(_validationAreaName, "MailboxCheckForUnsubscribingProcess");
        public string ValidationSuccessfullyUnsubscribed => GetLocalizedValue(_validationAreaName, "SuccessfullyUnsubscribed");
        public string ValidationGeneralError => GetLocalizedValue(_validationAreaName, "GeneralError");
        public string ValidationNegativeNumbersNotAllowed => GetLocalizedValue(_validationAreaName, "NegativeNumbersNotAllowed");
        public string ValidationMustEnterFourDigits => GetLocalizedValue(_validationAreaName, "MustEnterFourDigits");
        public string ValidationUnpublishedTopic => GetLocalizedValue(_validationAreaName, "UnpublishedTopic");
        public string ValidationNullTopic => GetLocalizedValue(_validationAreaName, "NullTopic");
        public string ValidationUnpublishedEventsChosing => GetLocalizedValue(_validationAreaName, "UnpublishedEventsChosing");
        public string ValidationNullEventsChosing => GetLocalizedValue(_validationAreaName, "NullEventsChosing");
        public string ValidationUnpublishedContactUsForm => GetLocalizedValue(_validationAreaName, "UnpublishedContactUsForm");
        public string ValidationNullContactUsForm => GetLocalizedValue(_validationAreaName, "NullContactUsForm");
        public string ValidationUnpublishedRelatedNews => GetLocalizedValue(_validationAreaName, "UnpublishedRelatedNews");
        public string ValidationNullRelatedNews => GetLocalizedValue(_validationAreaName, "NullRelatedNews");
        public string ValidationInvalidFileType => GetLocalizedValue(_validationAreaName, "FileFormatPDF");
        public string ValidationEndDateSmallerThanStartDate => GetLocalizedValue(_validationAreaName, "EndDateSmallerThanStartDate");
        public string ValidationInvalidFileFormat => GetLocalizedValue(_validationAreaName, "InvalidFileFormat");
        public string ValidationLargeFileSize => GetLocalizedValue(_validationAreaName, "LargeFileSize");
        public string ValidationInvalidPhoneNumber => GetLocalizedValue(_validationAreaName, "InvalidPhoneNumber");
        public string ValidationSingleEntityAlreadyExists => GetLocalizedValue(_validationAreaName, "SingleEntityAlreadyExists");
        public string ValidationContentCultureNotFound => GetLocalizedValue(_validationAreaName, "ContentCultureNotFound");
        public string ValidationContentNotFound => GetLocalizedValue(_validationAreaName, "ContentNotFound");
        public string ValidationUnselectedSourceValue => GetLocalizedValue(_validationAreaName, "UnselectedSourceValue");
        public string ValidationUnselectedManualSelection => GetLocalizedValue(_validationAreaName, "UnselectedManualSelection");
        public string ValidationUnselectedKeywords => GetLocalizedValue(_validationAreaName, "UnselectedKeywords");
        public string ValidationEmailRequired => GetLocalizedValue(_validationAreaName, "EmailRequired");
        public string ValidationKeywordValueCannotExceedLimit => GetLocalizedValue(_validationAreaName, "KeywordValueCannotExceedLimit");
        public string ValidationCannotDeleteProtectedContent => GetLocalizedValue(_validationAreaName, "CannotDeleteProtectedContent");

        #endregion


        /// <summary>
        /// If message has a placeholder replace placeholders inside message with values; else return the original message
        /// </summary>
        /// <param name="messageToReplacePlaceholder">Message with Placeholders</param>
        /// <param name="valuesToInject">placeholder values</param>
        /// <returns>message after replacing place holders or the original message</returns>
        public string ReplacePlaceholderIfExist(string messageToReplacePlaceholder, params object[] valuesToInject)
            => ValidationHelper.HasPlaceholders(messageToReplacePlaceholder) && valuesToInject != null && valuesToInject.Length > default(int)
                ? string.Format(messageToReplacePlaceholder, valuesToInject)
                : messageToReplacePlaceholder;

        /// <summary>
        /// Get a localized umbraco content property name, 
        /// Property name comes in the format of "#area_key"
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>localized value for property</returns>
        public string GetLocalizedPropertyName(string propertyName)
        {
            string[] cultureKeys = propertyName.Replace("#", "").Split('_');
            if (cultureKeys.Length != 2)
            {
                return $"[{propertyName}]";
            }

            return LocalizedTextService.Localize(cultureKeys[0], cultureKeys[1]);
        }

        protected string GetLocalizedValue(string area, string key, string altValue = "")
        {
            string localizedValue = LocalizedTextService.Localize(area, key);
            return !string.IsNullOrWhiteSpace(localizedValue) ? localizedValue : altValue;
        }
    }
}
