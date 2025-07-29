using Common.Constants;
using Common.Enums;
using Common.Exceptions;
using Common.Interfaces;
using FormReceiver.ApplicationCore.DTOs.Request;
using FormReceiver.ApplicationCore.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FormReceiver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormSubmissionController : ControllerBase
    {
        #region POST /api/contactform/send
        //[EnableRateLimiting("fixed-window")]
        [HttpPost("send", Name = "Enviar dados")]
        public async Task<ActionResult<Response>> Send(
            [FromBody] InputRequest request,
            [FromServices]
            IEmailService<InputRequest, Response> emailService,
            INotificationService<InputRequest, Response> notificationService
            )
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var emailResponse = await emailService.SendEmailAsync(request);

                if (emailResponse.Status == Status.Failed)
                    return BadRequest(emailResponse);

                _ = notificationService.RunNotificationEventHandler(request, emailResponse.Message); 

                return emailResponse;

            }
            catch (CustomValidationException ex)
            {
                return BadRequest(new Response(Status.Failed, AppConstants.INVALID_DATA_ERROR, ex.Message));
            }
            catch (CustomSmtpException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    new Response(Status.Failed, AppConstants.UNEXPECTED_ERROR, ex.Message)
                    );
            }
        }
        #endregion
    }
}
