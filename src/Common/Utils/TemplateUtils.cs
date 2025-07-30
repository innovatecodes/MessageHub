using Common.Enums;
using Common.Interfaces;
using Common.Options;

namespace Common.Utils
{
    public class TemplateUtils
    {
        public static Dictionary<string, string> PlaceholderBuilder(IInputRequest request, string from, string appName, string whatsapp, string site)
        {
            var placeholders = new Dictionary<string, string>()
            {
                { "AppName", appName },
                { "Name", request.Name ?? request.Email},
                { "FormattedDate", DateTimeUtils.GetCurrentFormattedDate() },
                { "From", from },
                { "Site", site },
                { "WhatsApp", FormatWhatsappAnchor(whatsapp) }
            };

            return placeholders;
        }

        public static string ReplacePlaceholders(string template, Dictionary<string, string> templateCredentials)
        {
            foreach (var pair in templateCredentials)
            {
                string placeholder = $"{{{pair.Key}}}"; // "{ {" + pair.Key +"} }"
                template = template.Replace(placeholder, pair.Value);
            }
            return template;
        }

        private static string FormatWhatsappAnchor(string whatsApp)
        {
            string whatsappAnchor;

            if (!string.IsNullOrEmpty(whatsApp))
            {
                whatsApp = StringUtils.RemoveNonDigitCharacters(whatsApp);

                if (whatsApp.Length == 11)
                {
                    var ddd = whatsApp.Substring(0, 2);
                    var whatsappHref = $"https://api.whatsapp.com/send/?phone=55{whatsApp}";
                    var formattedNumber = $"({ddd}) {whatsApp.Substring(2, 5)}-{whatsApp.Substring(7)}";
                    whatsappAnchor = $"<a href=\"{whatsappHref}\" target=\"_blank\">{formattedNumber}</a>";
                }
                else return whatsApp;
            }
            else
                return whatsApp;
            return whatsappAnchor;

            /*
            if (string.IsNullOrWhiteSpace(whatsApp))
                return whatsApp;

            var digits = RemoveNonDigitCharacters(whatsApp);

            if (digits.Length != 11)
                return whatsApp;

            var ddd = digits.Substring(0, 2);
            var number = digits[2..]; // Pega todos os caracteres da string 'digits' a partir do índice 2 até o final (igual a digits.Substring(2))
            string whatsAppHref = $"https://api.whatsapp.com/send/?phone=55{digits}";
            string formattedNumber = $"({ddd}) {number.Insert(5, "-")}"; // Insere o hífen após o quinto dígito

            return $"<a href=\"{whatsAppHref}\" target=\"_blank\">{formattedNumber}</a>";
            */
        }

        public static string GetTemplateType(TemplateType type)
        {
            return type switch
            {
                TemplateType.TextPlain => "text/plain", // Mapeia (associa/transforma) o enum TextPlain para o MIME "text/plain"
                TemplateType.Html => "text/html", // Mapeia (associa/transforma) o enum Html para o MIME "text/html"
                _ => "text/html"
            };
        }

        public static string ResolveAppName(string appName, int hasDot, dynamic applicationInfo)
        {
            try
            {
                if (!string.IsNullOrEmpty(applicationInfo.AppName))
                    return applicationInfo.AppName;

                if (!string.IsNullOrWhiteSpace(appName) && hasDot > 0) return appName.Substring(0, hasDot);
                if (string.IsNullOrWhiteSpace(appName))
                {
                    var friendlyName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);

                    if (!string.IsNullOrWhiteSpace(friendlyName)) return friendlyName;
                }
            }
            catch
            {
            }

            return appName;
        }

        public static string[] CustomTemplateRenderer(IInputRequest request, dynamic hostEnvironment, BodyRenderOptions? bodyRenderOptions)
        {
            var htmlTemplatePath = Path.Combine(hostEnvironment.ContentRootPath, "Templates", "HtmlTemplate.html");
            var plainTextTemplatePath = Path.Combine(hostEnvironment.ContentRootPath, "Templates", "PlainTextTemplate.txt");
            var htmlContent = File.ReadAllText(htmlTemplatePath);
            var plainTextContent = File.ReadAllText(plainTextTemplatePath);
            var placeholders = PlaceholderBuilder(request, bodyRenderOptions?.From!, bodyRenderOptions?.ApplicationInfo.AppName!, bodyRenderOptions?.ApplicationInfo.WhatsApp!, bodyRenderOptions?.ApplicationInfo.Site!);

            string[] templates = [TemplateUtils.ReplacePlaceholders(htmlContent, placeholders), TemplateUtils.ReplacePlaceholders(plainTextContent, placeholders)];
            return templates;
        }
    }
}
