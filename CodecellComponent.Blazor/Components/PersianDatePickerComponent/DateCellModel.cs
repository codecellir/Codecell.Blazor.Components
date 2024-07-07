namespace CodecellComponent.Blazor.Components.PersianDatePickerComponent
{
    public class DateCellModel
    {
        public int Day { get; set; }
        public DateTime Date { get; set; }
        public bool Show => Day > 0;
        public string PersianDateFormat => PersianDate();
        string PersianDate()
        {
            if (Day <= 0) return string.Empty;
            System.Globalization.PersianCalendar pc = new();
            var year = pc.GetYear(Date);
            var month = pc.GetMonth(Date);
            var day = pc.GetDayOfMonth(Date);

            return $"{year}/{month.ToString("D2")}/{day.ToString("D2")}";
        }
    }
}
