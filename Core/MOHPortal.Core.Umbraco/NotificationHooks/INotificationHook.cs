using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;

namespace  FayoumGovPortal.Core.Umbraco.NotificationHooks
{
    public interface INotificationHook
    {

    }

    public interface INotificationHook<TNotification> : INotificationHook
        where TNotification : INotification
    {
        public string ContentTypeAlias { get; }
        public Task ProcessEntities(IEnumerable<IContent> entities);
    }
}
