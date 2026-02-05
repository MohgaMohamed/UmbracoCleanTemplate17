using MOHPortal.Core.Umbraco.Localization;
using MOHPortal.Core.Umbraco.NotificationHooks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;

namespace MOHPortal.Core.Umbraco.ProtectedContent
{
    internal class ProtectedContentSavingNotificationHandler : INotificationHandler<ContentSavingNotification>
    {
        private readonly LocalizationWrapper _localization;
        private readonly ProtectedContentHelper _protectedContentHelper;
        private readonly NotificationStateManager _notificationStateManager;
        public ProtectedContentSavingNotificationHandler(LocalizationWrapper localizationWrapper, ProtectedContentHelper protectedContentHelper, NotificationStateManager notificationStateManager)
        {
            _localization = localizationWrapper;
            _protectedContentHelper = protectedContentHelper;
            _notificationStateManager = notificationStateManager;
        }

        public void Handle(ContentSavingNotification notification)
        {
            if(_notificationStateManager.State == ExecutionState.Suppressed)
            {
                return;
            }

            foreach (IContent contentItem in notification.SavedEntities)
            {
                if (_protectedContentHelper.IsProtected(contentItem) &&
                    _protectedContentHelper.ProtectedContentExistsWithinParent(contentItem, out string? validationMessage))
                {
                    notification.CancelOperation( new EventMessage(_localization.CommonError, validationMessage, EventMessageType.Error) );
                }
            }
        }
    }
}
