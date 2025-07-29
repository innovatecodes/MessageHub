using Common.Constants;
using Common.Enums;
using Common.Events;
using Common.Interfaces;
using Common.Options;
using Common.Utils;
using FormReceiver.ApplicationCore.DTOs.Request;
using FormReceiver.ApplicationCore.DTOs.Response;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace FormReceiver.ApplicationCore.Services
{
    public class WhatsAppNotificationService : BaseService, IWhatsAppNotificationService<Response>
    {
        private readonly HttpClient _httpClient;

        public WhatsAppNotificationService(IConfiguration configuration,
            IOptions<ApplicationInfo> applicationInfo,
            SmtpInfo smtpInfo,
            IHostEnvironment hostEnvironment,
            HttpClient httpClient
            )
            : base(configuration, applicationInfo, smtpInfo, hostEnvironment)
        {
            _httpClient = httpClient;
        }

        public async Task<Response> SendAsync(object? sender, NotificationEventArgs e)
        {
            if (e.NotificationMessage != AppConstants.EMAIL_SENT) return new Response(Status.Failed, AppConstants.WHATSAPP_SEND_FAILURE_ERROR);

            try
            {
                if (e.Request is InputRequest request && !string.IsNullOrWhiteSpace(request.WhatsApp))
                {
                    var whatsAppMessageRequest = new WhatsAppMessageRequest
                    {
                        To = $"{(int)CountryCode.BR}{request.WhatsApp}",
                        Text = new TextContent
                        {
                            Body = FormattedMessage(request)
                        }
                    };

                    using StringContent requestContent = new(
                      JsonSerializer.Serialize(whatsAppMessageRequest),
                        Encoding.UTF8,
                        "application/json");

                    using var httpResponse = await _httpClient.PostAsync(_httpClient.BaseAddress, requestContent);

                    httpResponse.EnsureSuccessStatusCode();
                
                    var content = await httpResponse.Content.ReadAsStringAsync();
                
                    return new Response(Status.Success, content);
                }

                return new Response(Status.Failed, AppConstants.WHATSAPP_SEND_FAILURE_ERROR);
            }
            catch (HttpRequestException ex)
            {
                return new Response(Status.Failed, AppConstants.EXTERNAL_API_FAILURE_ERROR, ex.Message);
            }
            catch (Exception ex)
            {
                return new Response(Status.Failed, AppConstants.WHATSAPP_SEND_FAILURE_ERROR, ex.Message);
            }
        }

        private string FormattedMessage(InputRequest request)
        {
            var name = !string.IsNullOrWhiteSpace(request.Name) ? request.Name : request.Email;
            return $"*Prezado(a)* _{name}_, agradecemos seu contato.\r\n" +
                   $"Esta é uma resposta automática para confirmar o recebimento da sua mensagem em {DateTimeUtils.GetCurrentFormattedDate()}.\r\n\r\n" +
                   $"*Atenciosamente*,\r\n_{_appName}_\r\n\r\n" +
                   $"*Site*: _{_applicationInfo.Site}_\r\n" +
                   $"*E-mail*: _{_smtpInfo.From}_\r\n" +
                   $"*WhatsApp*: _{_applicationInfo.WhatsApp}_\r\n";
        }
    }
}
