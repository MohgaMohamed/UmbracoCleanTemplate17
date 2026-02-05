using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.Extensions.DependencyInjection;
using MOHPortal.Core.Umbraco.DocumentValidator.Contracts;
using MOHPortal.Core.Umbraco.DocumentValidator.Extensions;
using MOHPortal.Core.Umbraco.DocumentValidator.Models;
using MOHPortal.Core.Umbraco.Localization;
using MOHPortal.Core.Umbraco.NotificationHooks;
using NUglify.Helpers;
using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;

namespace MOHPortal.Core.Umbraco.DocumentValidator
{
    internal class DocumentValidationNotificationHandler : INotificationAsyncHandler<ContentSavingNotification>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly LocalizationWrapper _localization;
        private readonly NotificationStateManager _notificationStateManager;

        public DocumentValidationNotificationHandler(IServiceProvider serviceProvider, LocalizationWrapper localization, NotificationStateManager notificationStateManager)
        {
            _serviceProvider = serviceProvider;
            _localization = localization;
            _notificationStateManager = notificationStateManager;
        }

        public Task HandleAsync(ContentSavingNotification notification, CancellationToken cancellationToken)
        {
            if (!notification.SavedEntities.Any() || _notificationStateManager.State == ExecutionState.Suppressed)
            {
                return Task.CompletedTask;
            }

            IEnumerable<IDocumentValidator> validators = _serviceProvider.GetServices<IDocumentValidator>();
            foreach (IContent contentModel in notification.SavedEntities)
            {
                List<DocumentValidationResult> results = [];
                IDocumentValidator? contentValidator = validators.FirstOrDefault(service => service.DocumentTypeAlias == contentModel.ContentType.Alias);
                if (contentValidator is not null)
                {
                    results.Add(contentValidator.Validate(contentModel));
                }

                if(contentModel.GetPropertiesByEditor(Constants.PropertyEditors.Aliases.Tags).Any())
                {
                    results.Add(ValidateKeywordsProperty(contentModel));
                }

                if (results.Any(x => !x.Success))
                {
                    results.Where(x => !x.Success)
                        .ForEach(x =>
                        {
                            string errorProperty = !string.IsNullOrWhiteSpace(x.ErrorProperty)
                                ? x.ErrorProperty
                                : _localization.CommonError;

                            EventMessage msg = new(errorProperty, x.ErrorMessage, EventMessageType.Error)
                            {
                                IsDefaultEventMessage = true
                            };

                            notification.Messages.Add(msg);
                        });
                    notification.Cancel = true;
                    continue;
                }

                //Knowing that if the ErrorMessage is not empty here, it would represent a successMessage
                IEnumerable<string> successMessages = results
                    .Where(x => !string.IsNullOrWhiteSpace(x.ErrorMessage))
                    .Select(x => x.ErrorMessage);
                if (successMessages.Any())
                {
                    successMessages.ForEach(x =>
                    {
                        notification.Messages.Add(new EventMessage(_localization.CommonSuccess, x, EventMessageType.Success));
                    });
                }
            }

            //if(string.IsNullOrWhiteSpace(content.Name) || content.Name.Length > 100)
            //{
            //    notification.CancelOperation(new EventMessage(_localization.CommonTitle, _localization.ValidationNodeNameExceededLimit, EventMessageType.Error));
            //}

            return Task.CompletedTask;
        }

        private DocumentValidationResult ValidateKeywordsProperty(IContent contentModel)
        {
            IEnumerable<IProperty> keywordsProperties = contentModel.Properties.Where(x => x.PropertyType.PropertyEditorAlias == Constants.PropertyEditors.Aliases.Tags);
            if (!keywordsProperties.Any())
            {
                return DocumentValidationResult.Successful();
            }

            foreach (IProperty? property in keywordsProperties)
            {
                string[] values = property.GetJsonTagsPropertyValue();
                if (property.PropertyType.Mandatory && values.Length == default)
                {
                    return DocumentValidationResult.Failure(
                        _localization.GetLocalizedPropertyName(property.PropertyType.Name),
                        property.PropertyType.MandatoryMessage ?? _localization.ValidationRequired
                    );
                }

                if (values.Any(x => x.Length > 50))
                {
                    return DocumentValidationResult.Failure(
                        _localization.GetLocalizedPropertyName(property.PropertyType.Name),
                        _localization.ReplacePlaceholderIfExist(
                            _localization.ValidationKeywordValueCannotExceedLimit,
                            values.FirstOrDefault(x => x.Length > 50) ?? string.Empty,
                            50
                        )
                    );
                }
            }

            return DocumentValidationResult.Successful();
        }
    }
}
