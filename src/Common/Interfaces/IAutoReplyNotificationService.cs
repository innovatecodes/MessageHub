using Common.Events;

namespace Common.Interfaces
{
    public interface IAutoReplyNotificationService<TResponse> 
    {
        public Task<TResponse> SendAsync(object? sender, NotificationEventArgs e);
    }
}
