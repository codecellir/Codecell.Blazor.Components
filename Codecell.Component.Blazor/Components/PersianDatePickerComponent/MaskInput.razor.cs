using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

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

        async Task HandleDayOnkeydown(KeyboardEventArgs e)
        {
            if (e.Key.Equals("ArrowRight", StringComparison.OrdinalIgnoreCase))
            {
                await monthInput.FocusAsync();
            }
            else if (e.Key.Equals("Backspace", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(day))
            {
                day = day[..^1];
            }
            else if (byte.TryParse(e.Key, out byte numbered))
            {
                day = day + numbered;

                if (!(byte.TryParse(day, out var numberedDay) && numberedDay >= 0 && numberedDay <= 31))
                {
                    day = string.Empty;
                }
            }

            if (day.Length == 2)
                await monthInput.FocusAsync();
        }

        async Task HandleMonthOnkeydown(KeyboardEventArgs e)
        {
            if (e.Key.Equals("ArrowRight", StringComparison.OrdinalIgnoreCase))
            {
                await yearInput.FocusAsync();
            }
            else if (e.Key.Equals("ArrowLeft", StringComparison.OrdinalIgnoreCase))
            {
                await dayInput.FocusAsync();
            }
            else if (e.Key.Equals("Backspace", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(month))
            {
                month = month[..^1];
            }
            else if (byte.TryParse(e.Key, out byte numbered))
            {
                month = month + numbered;

                if (!(byte.TryParse(month, out var numberedMonth) && numberedMonth > 0 && numberedMonth <= 12))
                {
                    month = string.Empty;
                }
            }

            if (month.Length == 2)
                await yearInput.FocusAsync();
        }

        async Task HandleYearOnkeydown(KeyboardEventArgs e)
        {
            if (e.Key.Equals("ArrowLeft", StringComparison.OrdinalIgnoreCase))
            {
                await monthInput.FocusAsync();
            }
            else if (e.Key.Equals("Backspace", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(year))
            {
                year = year[..^1];
            }
            else if (byte.TryParse(e.Key, out byte numbered) && numbered > 0 && year.Length < 4)
            {
                year = year + numbered;
            }
        }
    }
}
