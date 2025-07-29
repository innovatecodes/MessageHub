using Common.Interfaces;

namespace Common.Events
{
    public delegate Task<TResponse> NotifyEventHandler<TResponse>(object? sender, NotificationEventArgs e);

    public sealed class NotificationEventArgs : EventArgs
    {
        public NotificationEventArgs(
            IInputRequest request,
            string? notificationMessage = null
            )
        {
            Request = request;
            NotificationMessage = notificationMessage;
        }

        public IInputRequest Request { get; }
        public string? NotificationMessage { get; }
    }
}