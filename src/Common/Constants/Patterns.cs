using System.Text.RegularExpressions;

namespace Common.Constants
{
    public static class Patterns
    {
        public const string NON_DIGIT_PATTERN = @"[^\d]"; // @"\D"; 
        
        /// <summary>
        /// Expressão regular para validar e-mails
        /// RegexOptions.Compiled: Melhora o desempenho quando a regex é utilizada muitas vezes, já que evita a necessidade de recompilação da expressão a cada uso
        /// </summary>
        public static readonly Regex EMAIL_PATTERN = new(@"^[a-zA-Z0-9](?:[a-zA-Z0-9._-]*[a-zA-Z0-9_])?@(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}$", RegexOptions.Compiled);
    }
}
