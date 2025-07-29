using Common.Events;


namespace Common.Interfaces
{
    public interface INotificationService<TRequest, TResponse> 
    {
        event NotifyEventHandler<TResponse>? OnFormSubmitted;
        Task<TResponse> EventHandler(TRequest request, string? notificationMessage = null);
        Task RunNotificationEventHandler(TRequest request, string notificationMessage);
    }
}
