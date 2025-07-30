using Common.Constants;
using Common.Interfaces;
using Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace FormReceiver.DTOs.Request
{
    public  record InputRequest : IInputRequest
    {
        [MinLength(4, ErrorMessage = AppConstants.NAME_LENGTH_ERROR)]
        public string? Name { get; init; }

        [Required(ErrorMessage = AppConstants.REQUIRED_EMAIL_ERROR)]
        [EmailFormat(ErrorMessage = AppConstants.INVALID_EMAIL_ERROR)]
        public string Email { get; init; } = string.Empty;

        [StringLength(11, ErrorMessage = AppConstants.WHATSAPP_LENGTH_ERROR)]
        public string? WhatsApp { get; init; }

        [StringLength(40, MinimumLength = 6, ErrorMessage = AppConstants.SUBJECT_LENGTH_ERROR)]
        public string? Subject { get; init; }

        [Required(ErrorMessage = AppConstants.REQUIRED_MESSAGE_ERROR)]
        [MinLength(6, ErrorMessage = AppConstants.MESSAGE_LENGTH_ERROR)]
        public string Message { get; init; } = string.Empty;
    }
}

