using System.Text.RegularExpressions;

namespace Common.Utils
{
    public static class StringUtils
    {
        public static string RemoveNonDigitCharacters(string value) => new System.String(value.Where(Char.IsDigit).ToArray());

        public static string RemovePattern(string value, string pattern) => Regex.Replace(value, pattern, "");

        public static bool IsDigitsOnly(string value)
        {

            return value.All(Char.IsDigit);

            /*
            foreach (char c in value)
                if (!Char.IsDigit(c))
                    return false;
            return true;
            GetFormattedDate(new CultureInfo("pt-BR"));
            */
        }

        public static void GetSanitizedApplicationName(dynamic hostEnvironment, out string appName, out int hasDot)
        {
            appName = hostEnvironment.ApplicationName;
            hasDot = appName.IndexOf('.');
        }
    }
}
