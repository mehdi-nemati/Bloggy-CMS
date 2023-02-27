namespace Bloggy.Service.AlertService
{
    public class AlertModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public AlertIcons icon { get; set; }
    }

    public enum AlertIcons
    {
        warning,
        error,
        success,
        info
    }
}
