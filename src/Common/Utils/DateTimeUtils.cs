using Common.Enums;
using System.Globalization;

namespace Common.Utils
{
    public class DateTimeUtils
    {
        public static string GetCurrentFormattedDate(DateFormatType format = DateFormatType.Long) 
        {
            CultureInfo cultureInfo = new("pt-BR");
            DateTime date = DateTime.UtcNow;

            var fullYear = date.Year.ToString("D4");
            var month = date.Month.ToString("D2"); // date.Month.ToString().PadLeft(2, '0');
            var day = date.Day.ToString("D2"); // date.Day.ToString().PadLeft(2, '0');
            var dayOfweek = cultureInfo.DateTimeFormat.GetDayName(date.DayOfWeek);
            var monthOfYear = cultureInfo.DateTimeFormat.GetMonthName(date.Month);
            //var cultureName = !string.IsNullOrWhiteSpace(cultureInfo.Name) ? cultureInfo.Name.ToLowerInvariant() : string.Empty;

            // dayOfweek[1..] usa o range operator (C# 8.0+) e faz o mesmo que dayOfWeek.Substring(1), mas é mais moderno e legível
            dayOfweek = $"{Char.ToUpper(dayOfweek[0])}{dayOfweek[1..]}";

            switch (format)
            {
                case DateFormatType.Long:
                    return $"{dayOfweek}, {day} de {monthOfYear}, às {date.ToLocalTime():HH:mm}"; // {date.ToLocalTime().ToString("HH:mm")}"; 

                default:
                    return $"{day}/{month}/{fullYear}, às {date.ToLocalTime():HH:mm}";
            }

        }
    }
}
