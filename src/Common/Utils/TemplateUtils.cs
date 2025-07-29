using Common.Enums;
using Common.Interfaces;
using Common.Options;

namespace Common.Utils 
{
    public class TemplateUtils
    {
        public static Dictionary<string, string> PlaceholderBuilder(IInputRequest request, string sender, string appName, string whatsapp, string site)
        {
            var placeholders = new Dictionary<string, string>()
            {
                { "AppName", appName },
                { "Name", request.Name ?? request.Email},
                { "FormattedDate", DateTimeUtils.GetCurrentFormattedDate() },
                { "Sender", sender },
                { "Site", site },
                { "Whatsapp", FormatWhatsappAnchor(whatsapp) }
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

        private static string FormatWhatsappAnchor(string whatsapp)
        {
            string whatsappAnchor;

            if (!string.IsNullOrEmpty(whatsapp))
            {
                whatsapp = StringUtils.RemoveNonDigitCharacters(whatsapp);

                if (whatsapp.Length == 11)
                {
                    var ddd = whatsapp.Substring(0, 2);
                    var whatsappHref = $"https://api.whatsapp.com/send/?phone=55{whatsapp}";
                    var formattedNumber = $"({ddd}) {whatsapp.Substring(2, 5)}-{whatsapp.Substring(7)}";
                    whatsappAnchor = $"<a href=\"{whatsappHref}\" target=\"_blank\">{formattedNumber}</a>";
                }
                else return whatsapp;
            }
            else
                return whatsapp;
            return whatsappAnchor;

            /*
            if (string.IsNullOrWhiteSpace(whatsapp))
                return whatsapp;

            var digits = RemoveNonDigitCharacters(whatsapp);

            if (digits.Length != 11)
                return whatsapp;

            var ddd = digits.Substring(0, 2);
            var number = digits[2..]; // Pega todos os caracteres da string 'digits' a partir do índice 2 até o final (igual a digits.Substring(2))
            string whatsappHref = $"https://api.whatsapp.com/send/?phone=55{digits}";
            string formattedNumber = $"({ddd}) {number.Insert(5, "-")}"; // Insere o hífen após o quinto dígito

            return $"<a href=\"{whatsappHref}\" target=\"_blank\">{formattedNumber}</a>";
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
                var friendlyName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);

                if (!string.IsNullOrWhiteSpace(friendlyName))
                    return friendlyName;
            }
            catch { }

            return !string.IsNullOrEmpty(applicationInfo.AppName)
                         ? applicationInfo.AppName
                         : (hasDot > 0 ? appName.Substring(0, hasDot) : appName);
        }

        public static string[] CustomTemplateRenderer(IInputRequest request, dynamic hostEnvironment, BodyRenderOptions bodyRenderOptions = null!)
        {
            var htmlTemplatePath = Path.Combine(hostEnvironment.ContentRootPath, "Templates", "HtmlTemplate.html");
            var plainTextTemplatePath = Path.Combine(hostEnvironment.ContentRootPath, "Templates", "PlainTextTemplate.txt");
            var htmlContent = File.ReadAllText(htmlTemplatePath);
            var plainTextContent = File.ReadAllText(plainTextTemplatePath);
            var placeholders = TemplateUtils.PlaceholderBuilder(request, bodyRenderOptions.From!, bodyRenderOptions.ApplicationInfo.AppName, bodyRenderOptions.ApplicationInfo.WhatsApp, bodyRenderOptions.ApplicationInfo.Site);

            string[] templates = [TemplateUtils.ReplacePlaceholders(htmlContent, placeholders), TemplateUtils.ReplacePlaceholders(plainTextContent, placeholders)];
            return templates;
        }
    }
}
