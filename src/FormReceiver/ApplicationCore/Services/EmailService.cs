using Common.Constants;
using Common.Interfaces;
using Common.Options;
using FormReceiver.ApplicationCore.DTOs.Request;
using FormReceiver.ApplicationCore.DTOs.Response;
using Microsoft.Extensions.Options;

namespace FormReceiver.ApplicationCore.Services
{
    public class EmailService : BaseService, IEmailService<InputRequest, Response> 
    {
        public EmailService( 
            IConfiguration configuration, 
            IOptions<ApplicationInfo> applicationInfo, 
            SmtpInfo smtpInfo, 
            IHostEnvironment hostEnvironment
            ) : base(configuration, applicationInfo, smtpInfo, hostEnvironment)
        {
        }

        public async Task<Response> SendEmailAsync(InputRequest request)
        {
            return await base.ExecuteAsync(request, AppConstants.EMAIL_SEND_FAILURE_ERROR);
        }
    }
}
