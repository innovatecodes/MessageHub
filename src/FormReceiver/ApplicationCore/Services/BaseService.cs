using Common.Constants;
using Common.Enums;
using Common.Exceptions;
using Common.Extensions;
using Common.Options;
using Common.Utils;
using FormReceiver.ApplicationCore.DTOs.Request;
using FormReceiver.ApplicationCore.DTOs.Response;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;

namespace FormReceiver.ApplicationCore.Services
{
    public abstract class BaseService 
    {
        private readonly IConfiguration _configuration;
        protected readonly ApplicationInfo _applicationInfo;
        protected readonly SmtpInfo _smtpInfo;
        protected readonly IHostEnvironment _hostEnvironment;
        protected readonly string _appName;
        protected readonly string _from;
        protected readonly string _to;

        protected BaseService( 
             IConfiguration configuration,
            IOptions<ApplicationInfo> applicationInfo,
            SmtpInfo smtpInfo,
            IHostEnvironment hostEnvironment
            )
        {
            _configuration = configuration;
            _applicationInfo = applicationInfo.Value;
            _smtpInfo = smtpInfo;
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
            StringUtils.GetSanitizedApplicationName(_hostEnvironment, out string appName, out int hasDot);
            _appName = TemplateUtils.ResolveAppName(appName, hasDot, _applicationInfo);
            _from = _smtpInfo.From; /*_configuration["SmtpInfo:From"]*/
            _to = _smtpInfo.To; /*_configuration["SmtpInfo:To"]*/
        }

        public async Task<Response> ExecuteAsync(InputRequest request, string fallbackErrorMessage, string? notificationMessage = null)
        {

            var isAutoReply = notificationMessage == AppConstants.EMAIL_SENT; 

            try
            {
                using var smtpClient = CreateStmpClient(_smtpInfo.SmtpServer, _smtpInfo.SmtpPort, _smtpInfo.SmtpUser.ValidateEmail(), _smtpInfo.SmtpPass);
                using var message = this.Create(request, _from.ValidateEmail(), _to.ValidateEmail(), _appName);

                await smtpClient.SendMailAsync(message);

                if (isAutoReply)
                    return new Response(Status.Success, AppConstants.EMAIL_AUTOREPLY_SENT);

                return new Response(Status.Success, AppConstants.EMAIL_SENT);
            }
            catch (CustomValidationException)
            {
                throw;
            }
            catch (SmtpException ex) when (ex.InnerException is InvalidOperationException invalidOperationException)
            {
                throw new CustomSmtpException(ex.Message, invalidOperationException /*ex.InnerException*/);
            }
            catch (SmtpException ex) when (ex.InnerException is SocketException socketException)
            {
                throw new CustomSmtpException(fallbackErrorMessage, socketException /*ex.InnerException*/);
            }
            catch (SmtpException ex) 
            {
                throw new CustomSmtpException(fallbackErrorMessage, ex);
            }
            catch (Exception) 
            {
                throw;
            }
        }

        protected virtual MailMessage Create(InputRequest request, string from, string to, string appName)
        {
            var subject = GetSubject(request);
            var (htmlContent, plainTextContent) = GetBodies(request, subject);

            MailAddress From = new(from, appName); // Sender
            MailAddress To = new(to); // Recipient
            MailMessage message = new(From, To)
            {
                Subject = subject,
                BodyEncoding = System.Text.Encoding.UTF8,
                //Body = htmlContent, 
                IsBodyHtml = true,
                Priority = MailPriority.High,
            };

            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(plainTextContent, System.Text.Encoding.UTF8, TemplateUtils.GetTemplateType(TemplateType.TextPlain)));
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(htmlContent, System.Text.Encoding.UTF8, TemplateUtils.GetTemplateType(TemplateType.Html)));

            return message;
        }

        protected virtual string GetSubject(InputRequest request) => $"{(!string.IsNullOrEmpty(request.Subject) ? request.Subject : $"Sem assunto")}";

        protected virtual (string, string) GetBodies(InputRequest request, string? subject = null, string? from = null, string? appName = null)
        {
            subject ??= GetSubject(request);

            var htmlContent = MessageTemplates.Html(request, subject);
            var plainTextContent = MessageTemplates.PlainText(request, subject);

            return (htmlContent, plainTextContent);
        }

        protected virtual SmtpClient CreateStmpClient(string smtpServer, int smtpPort, string smtpUser, string smtpPass)
        {
            return new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }
    }
}
