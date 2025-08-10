using System.Globalization;

namespace Codecell.Component.Blazor.Components.PersianDatePickerComponent;

public static class PersianDateHelper
{
    private static PersianCalendar pc = new();
    public static string[] WeekNames => ["ش", "ی", "د", "س", "چ", "پ", "ج"];
    public static int GetWeekSpan(this DayOfWeek week)
    {
        return week switch
        {
            DayOfWeek.Saturday => 0,
            DayOfWeek.Sunday => 1,
            DayOfWeek.Monday => 2,
            DayOfWeek.Tuesday => 3,
            DayOfWeek.Wednesday => 4,
            DayOfWeek.Thursday => 5,
            DayOfWeek.Friday => 6,
            _ => 0
        };
    }
    public static string GetMonthName(this int month) =>
          month switch
          {
              1 => "فروردین",
              2 => "اردیبهشت",
              3 => "خرداد",
              4 => "تیر",
              5 => "مرداد",
              6 => "شهریور",
              7 => "مهر",
              8 => "آبان",
              9 => "آذر",
              10 => "دی",
              11 => "بهمن",
              12 => "اسفند",
              _ => "نامشخص",
          };
    public static (int MonthNumber, string MonthName)[] GetMonths() =>
    [
        (1, "فروردین"),
        (2, "اردیبهشت"),
        (3, "خرداد"),
        (4, "تیر"),
        (5, "مرداد"),
        (6, "شهریور"),
        (7, "مهر"),
        (8, "آبان"),
        (9, "آذر"),
        (10, "دی"),
        (11, "بهمن"),
        (12, "اسفند")
    ];
    public static string ToPersianDate(this DateTime date)
    {
        var year = pc.GetYear(date);
        var month = pc.GetMonth(date);
        var day = pc.GetDayOfMonth(date);
        return $"{year}/{month.ToString("D2")}/{day.ToString("D2")}";
    }
    public static DateTime? ToGeorgianDate(this string persianDateText)
    {

        try
        {
            if (string.IsNullOrWhiteSpace(persianDateText))
                return null;

            var splited = persianDateText.Split('/');

            var year = int.Parse(splited[0]);
            var month = int.Parse(splited[1]);
            var day = int.Parse(splited[2]);

            return new DateTime(year, month, day, pc);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

}
