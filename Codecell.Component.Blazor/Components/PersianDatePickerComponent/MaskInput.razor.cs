using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Globalization;

namespace Codecell.Component.Blazor.Components.PersianDatePickerComponent
{
    public partial class MaskInput
    {
        string day = string.Empty;
        string month = string.Empty;
        string year = string.Empty;

        ElementReference dayInput;
        ElementReference monthInput;
        ElementReference yearInput;

        [Parameter] public DateTime? Date { get; set; }
        [Parameter] public EventCallback<DateTime?> DateChanged { get; set; }
        async Task HandleDayOnkeydown(KeyboardEventArgs e)
        {
            if (e.Key.Equals("ArrowLeft", StringComparison.OrdinalIgnoreCase))
            {
                await monthInput.FocusAsync();
                return;
            }
            else if (e.Key.Equals("Backspace", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(day))
            {
                day = day[..^1];
            }
            else if (byte.TryParse(e.Key, out byte numbered))
            {
                if (numbered == 0)
                {
                    if (day.Length == 0)
                        day = day + numbered;
                    else if (day[0] != '0')
                        day = day + numbered;
                }
                else
                    day = day + numbered;

                int.TryParse(day, out var numberedDay);

                if (numberedDay > 31)
                {
                    day = string.Empty;
                }

                await GetDate();
            }

            if (day.Length == 2)
                await monthInput.FocusAsync();
        }
        async Task HandleMonthOnkeydown(KeyboardEventArgs e)
        {
            if (e.Key.Equals("ArrowLeft", StringComparison.OrdinalIgnoreCase))
            {
                await yearInput.FocusAsync();
                return;
            }
            else if (e.Key.Equals("ArrowRight", StringComparison.OrdinalIgnoreCase))
            {
                await dayInput.FocusAsync();
                return;
            }
            else if (e.Key.Equals("Backspace", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(month))
            {
                month = month[..^1];
            }
            else if (byte.TryParse(e.Key, out byte numbered))
            {
                if (numbered == 0)
                {
                    if (month.Length == 0)
                        month = month + numbered;
                    else if (month[0] != '0')
                        month = month + numbered;
                }
                else
                    month = month + numbered;

                int.TryParse(month, out var numberedMonth);

                if (numberedMonth > 12)
                {
                    month = string.Empty;
                }

                await GetDate();
            }

            if (month.Length == 2)
                await yearInput.FocusAsync();
        }

        async Task HandleYearOnkeydown(KeyboardEventArgs e)
        {
            if (e.Key.Equals("ArrowRight", StringComparison.OrdinalIgnoreCase))
            {
                await monthInput.FocusAsync();
                return;
            }
            else if (e.Key.Equals("Backspace", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(year))
            {
                year = year[..^1];
            }
            else if (byte.TryParse(e.Key, out byte numbered) && year.Length < 4)
            {
                if (string.IsNullOrEmpty(year) && numbered == 0)
                    return;

                year = year + numbered;
                await GetDate();
            }
        }

        async Task GetDate()
        {
            FixDate();

            var persianDate = $"{year}/{month}/{day}";

            if (persianDate.TryGetDateFromString(out var date))
                await DateChanged.InvokeAsync(date);
        }

        void FixDate()
        {
            var pc = new PersianCalendar();

            var dayIsValid = byte.TryParse(day, out var numberedDay);
            var monthIsValid = byte.TryParse(month, out var numberedMonth);
            var yearIsValid = int.TryParse(year, out var numberedYear) && numberedYear.ToString().Length==4;

            if (dayIsValid && monthIsValid && yearIsValid)
            {
                if (numberedDay == 31 && numberedMonth >= 7)
                {
                    numberedDay = 30;
                    day = "30";
                }

                if (numberedMonth == 12 && numberedDay == 30)
                {
                    bool isLeapYear = pc.IsLeapYear(numberedYear);
                    if (!isLeapYear)
                    {
                        numberedDay = 29;
                        day = "29";
                    }
                }
            }
        }
    }

}
