using Bloggy.Shared.Config;
using Microsoft.Extensions.Localization;

namespace Bloggy.Service.AlertService
{
    public class AlertService  : IAlertService , ISingletonDependency
    {
        private readonly IStringLocalizer localizer;
        public AlertService(IStringLocalizer localizer)
        {
            this.localizer = localizer;
        }

        public  AlertModel CreateSuccessAlert()
        {
            return new AlertModel
            {
                Title = "",
                Text = localizer["success-message-text"],
                icon = AlertIcons.success
            };
        }
        public  AlertModel CreateNotSuccessAlert()
        {
            return new AlertModel
            {
                Title = "",
                Text = localizer["not-success-message-text"],
                icon = AlertIcons.error
            };
        }
        public  AlertModel CreateAlert(string Title, string Text, AlertIcons icon)
        {
            return new AlertModel
            {
                Title = Title,
                Text = Text,
                icon = icon
            };
        }
    }
}
