using Common.Constants;
using Common.Enums;
using Common.Events;
using Common.Interfaces;
using FormReceiver.DTOs.Request;
using FormReceiver.DTOs.Response;

namespace FormReceiver.Services
{
    public class NotificationService : INotificationService<InputRequest, Response> , IDisposable
    {
        private readonly ILogger<NotificationService> _logger;
        public event NotifyEventHandler<Response>? OnFormSubmitted;
        private readonly IAutoReplyNotificationService<Response> _autoReplyNotificationService;
        private readonly WhatsAppNotificationService _whatsAppNotificationService;


        public NotificationService(
            IAutoReplyNotificationService<Response> autoReplyNotificationService,
            ILogger<NotificationService> logger,
            WhatsAppNotificationService whatsAppNotificationService
            )
        {
            _logger = logger;
            _autoReplyNotificationService = autoReplyNotificationService;
            _whatsAppNotificationService = whatsAppNotificationService;
            SubscribeEventHandlers();
        }

        public void Dispose()
        {
            UnsubscribeEventHandlers();
        }

        public Task RunNotificationEventHandler(InputRequest request, string notificationMessage)
        {
            _ = Task.Run(async () => {
                try
                {
                    var eventResult = await EventHandler(request, notificationMessage);

                    if (eventResult.Status == Status.Failed) 
                        _logger.LogError(eventResult.Message, "*** EventHandler ***");

                }
                catch (Exception ex) 
                {
                    _logger.LogError(ex, "*** EventHandler ***");
                }
            });

            return Task.CompletedTask;
        }

        public async Task<Response> EventHandler(InputRequest request, string? notificationMessage = null)
        {
            if (OnFormSubmitted != null)
                return await OnFormSubmitted.Invoke(this, new NotificationEventArgs(request, notificationMessage));

            return new Response(Status.Failed, AppConstants.EVENT_FAILURE_ERROR);
        }

        private void SubscribeEventHandlers()
        {
            OnFormSubmitted += _autoReplyNotificationService.SendAsync;
            //OnFormSubmitted += _whatsAppNotificationService.SendAsync;
        }

        private void UnsubscribeEventHandlers()
        {
            OnFormSubmitted -= _autoReplyNotificationService.SendAsync;
            //OnFormSubmitted -= _whatsAppNotificationService.SendAsync;
        }
    }
}
