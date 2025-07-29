using Common.Constants;
using Common.Enums;
using Common.Events;
using Common.Interfaces;
using FormReceiver.ApplicationCore.DTOs.Request;
using FormReceiver.ApplicationCore.DTOs.Response;

namespace FormReceiver.ApplicationCore.Services
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
                    var result = await EventHandler(request, notificationMessage);

                    if (result.Status == Status.Failed) 
                        _logger.LogError(result.Message, "*** EventHandler ***");

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

            return new Response(Common.Enums.Status.Failed, AppConstants.EVENT_FAILURE_ERROR);
        }

        private void SubscribeEventHandlers()
        {
            OnFormSubmitted += _autoReplyNotificationService.SendAsync;
            OnFormSubmitted += _whatsAppNotificationService.SendAsync;
        }

        private void UnsubscribeEventHandlers()
        {
            OnFormSubmitted -= _autoReplyNotificationService.SendAsync;
            OnFormSubmitted -= _whatsAppNotificationService.SendAsync;
        }
    }
}
