using Microsoft.AspNetCore.Components.Forms;
using FayoumGovPortal.Core.Umbraco.Localization;
using FayoumGovPortal.Core.Umbraco.Model.Gen;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;

namespace  FayoumGovPortal.Core.Umbraco.ProtectedContent
{
    internal class ProtectedContentDeletingNotificationHandler : INotificationHandler<ContentMovingToRecycleBinNotification>
    {
        private readonly LocalizationWrapper _localization;
        private readonly ProtectedContentHelper _protectedContentHelper;
        public ProtectedContentDeletingNotificationHandler(LocalizationWrapper localization, ProtectedContentHelper protectedContentHelper)
        {
            _localization = localization;
            _protectedContentHelper = protectedContentHelper;
        }

        public void Handle(ContentMovingToRecycleBinNotification notification)
        {
            foreach (IContent contentItem in notification.MoveInfoCollection.Select(x => x.Entity))
            {
                if (_protectedContentHelper.IsProtected(contentItem) &&
                    !_protectedContentHelper.ProtectedContentExistsWithinParent(contentItem, out string? validationMessage))
                {
                    notification.CancelOperation(new EventMessage(_localization.CommonError, _localization.ValidationCannotDeleteProtectedContent, EventMessageType.Error));
                }
            }
        }
    }
}
