using FayoumGovPortal.Core.Umbraco.NotificationHooks;
using Umbraco.Cms.Core.Events;
using uSync.BackOffice;
using uSync.Core;
using uSync.BackOffice.Services;

namespace  FayoumGovPortal.Core.Umbraco.uSync
{
    internal class uSyncNotificationManager : 
        INotificationAsyncHandler<uSyncImportStartingNotification>,
        INotificationAsyncHandler<uSyncImportCompletedNotification>
    {
        private readonly ISyncService _uSyncEventService;
        //private readonly uSyncEventService _uSyncEventService;
        private readonly NotificationStateManager _notificationStateManager;
        public uSyncNotificationManager(ISyncService uSyncEventService, NotificationStateManager notificationStateManager)
        {
            _uSyncEventService = uSyncEventService;
            _notificationStateManager = notificationStateManager;
        }

        public Task HandleAsync(uSyncImportStartingNotification notification, CancellationToken cancellationToken)
        {
            _notificationStateManager.SupressNotifications();
            return Task.CompletedTask;
        }

        public Task HandleAsync(uSyncImportCompletedNotification notification, CancellationToken cancellationToken)
        {
            _notificationStateManager.ResetState();
            return Task.CompletedTask;
        }
    }
}
