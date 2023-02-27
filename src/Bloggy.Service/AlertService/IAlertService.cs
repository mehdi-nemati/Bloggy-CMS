namespace Bloggy.Service.AlertService
{
    public interface IAlertService
    {
        AlertModel CreateSuccessAlert();
        AlertModel CreateNotSuccessAlert();
        AlertModel CreateAlert(string Title, string Text, AlertIcons icon);
    }
}
