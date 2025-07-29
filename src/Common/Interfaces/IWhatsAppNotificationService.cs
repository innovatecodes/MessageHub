using Common.Events;

namespace Common.Interfaces
{
    public interface IWhatsAppNotificationService<TResponse>
    {
        public Task<TResponse> SendAsync(object? sender, NotificationEventArgs e);
    }
}
