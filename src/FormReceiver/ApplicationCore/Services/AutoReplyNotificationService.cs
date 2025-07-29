using Common.Constants;
using Common.Enums;
using Common.Events;
using Common.Interfaces;
using Common.Options;
using Common.Utils;
using FormReceiver.ApplicationCore.DTOs.Request;
using FormReceiver.ApplicationCore.DTOs.Response;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace FormReceiver.ApplicationCore.Services
{
    public class AutoReplyNotificationService : EmailService, IAutoReplyNotificationService<Response>
    {
        public AutoReplyNotificationService(IConfiguration configuration, 
            IOptions<ApplicationInfo> applicationInfo, 
            SmtpInfo smtpInfo, 
            IHostEnvironment hostEnvironment) 
            : base(configuration, applicationInfo, smtpInfo, hostEnvironment)
        {
        }

        public async Task<Response> SendAsync(/*InputRequest request*/object? sender, NotificationEventArgs e)
        {
            if (e.Request is InputRequest request)
                return await base.ExecuteAsync(request, AppConstants.EMAIL_AUTOREPLY_FAILURE_ERROR, e.NotificationMessage);
            
            return new Response(Common.Enums.Status.Failed, AppConstants.EMAIL_AUTOREPLY_FAILURE_ERROR);
        }

        protected override MailMessage Create(InputRequest request, string from, string to, string appName)
        {
            var message = base.Create(request, from, request.Email, appName);

            message.ReplyToList.Add(new MailAddress(to));

            // Permite ao cliente de e-mail exibir uma opção de "cancelar inscrição" (boas práticas de e-mail marketing/spam)
            message.Headers.Add("List-Unsubscribe", $"<mailto:{from}>");
            // Informa que o e-mail foi gerado automaticamente (ajuda a evitar respostas automáticas)
            message.Headers.Add("Auto-Submitted", "auto-replied");
            // Indica que a mensagem é enviada em massa (ou automática); evita auto-replies e confirmações de leitura
            message.Headers.Add("Precedence", "bulk");
            // Solicita ao servidor do destinatário (como Outlook/Exchange) que **não envie nenhuma resposta automática**
            message.Headers.Add("X-Auto-Response-Suppress", "All");

            return message;
        }

        protected override string GetSubject(InputRequest request) => $"Re: {base.GetSubject(request)}";

        protected override (string, string) GetBodies(InputRequest request, string? subject = null, string? from = null, string? appName = null)
        {

            var applicationInfo = new ApplicationInfo {AppName = _applicationInfo.AppName, WhatsApp = _applicationInfo.WhatsApp, Site = _applicationInfo.Site};

            var templates = TemplateUtils.CustomTemplateRenderer(request, _hostEnvironment, new BodyRenderOptions { From = from!, ApplicationInfo = applicationInfo });

            return (templates[0], templates[1]);
        }

        public string GetMessageTemplateType(string templateType)
        {
            return templateType;
        }
    }
}
