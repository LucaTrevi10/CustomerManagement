namespace CustomerUI.Models
{
    public class NotificationItem
    {
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = "info"; // info, success, warning, danger
        public string Icon { get; set; } = "bi-info-circle"; // Bootstrap icon class
    }
}
