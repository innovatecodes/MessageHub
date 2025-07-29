using Common.Constants;
using Common.Exceptions;

namespace Common.Options
{
    public sealed class SmtpInfo
    {
        public string From { get; init; } = string.Empty; // Sender
        public string To { get; init; } = string.Empty; // Recipient
        public string SmtpServer { get; init; } = string.Empty;
        public int SmtpPort { get; init; } = 587;
        public string SmtpUser { get; init; } = string.Empty;
        public string SmtpPass { get; init; } = string.Empty;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(From) ||
                string.IsNullOrWhiteSpace(To) ||
                string.IsNullOrWhiteSpace(SmtpServer) ||
                (SmtpPort < 1 || SmtpPort > 65535) ||
                string.IsNullOrWhiteSpace(SmtpUser) ||
                string.IsNullOrWhiteSpace(SmtpPass))
            {
                throw new ConfigurationException(AppConstants.CONFIG_LOAD_FAILURE_ERROR); 
            }
        }
    }
}
