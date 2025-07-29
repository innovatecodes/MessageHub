using Common.Constants;

namespace Common.Options
{
    public sealed class ApplicationInfo
    {
        public string AppName { get; init; } = string.Empty;
        public string WhatsApp { get; init; } = ApplicationInfoDefaults.Whatsapp;
        public string Site { get; init; } = ApplicationInfoDefaults.Site;   
    }
}
