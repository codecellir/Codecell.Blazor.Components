namespace CodecellComponent.Blazor.Components.PersianDatePickerComponent
{
    public static class PersianDateHelper
    {

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

        public static string GetWeekName(this DayOfWeek week) =>
             week switch
             {
                 DayOfWeek.Saturday => "شنبه",
                 DayOfWeek.Sunday => "یکشنبه",
                 DayOfWeek.Monday => "دوشنبه",
                 DayOfWeek.Tuesday => "سه شنبه",
                 DayOfWeek.Wednesday => "چهارشنبه",
                 DayOfWeek.Thursday => "پنجشنبه",
                 DayOfWeek.Friday => "جمعه",
                 _ => "نامشخص"
             };

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

    }
}
