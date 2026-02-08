using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;

namespace  FayoumGovPortal.Core.Umbraco.NotificationHooks.NotificationHandlers
{
    internal class ContentSavingNotificationHandler : INotificationAsyncHandler<ContentSavingNotification>
    {
        private readonly IEnumerable<INotificationHook<ContentSavingNotification>> _hooks;
        private readonly NotificationStateManager _notificationStateManager;
        public ContentSavingNotificationHandler(IEnumerable<INotificationHook<ContentSavingNotification>> hooks, NotificationStateManager notificationStateManager)
        {
            _hooks = hooks;
            _notificationStateManager = notificationStateManager;
        }

        public async Task HandleAsync(ContentSavingNotification notification, CancellationToken cancellationToken)
        {
            IEnumerable<IContent> savingEntities = notification.SavedEntities;
            if (!notification.SavedEntities.Any() || _notificationStateManager.State == ExecutionState.Suppressed) 
            {
                return;
            }

            foreach (INotificationHook<ContentSavingNotification> hook in _hooks)
            {
                IEnumerable<IContent> relatedEntities = savingEntities.Where(x => x.ContentType.Alias == hook.ContentTypeAlias);
                if(relatedEntities.Any())
                {
                    await hook.ProcessEntities(relatedEntities);
                }
            }
        }
    }
}
