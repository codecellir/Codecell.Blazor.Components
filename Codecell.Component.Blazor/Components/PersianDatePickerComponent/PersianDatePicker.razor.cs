using Codecell.Component.Blazor.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Linq.Expressions;

namespace Codecell.Component.Blazor.Components.PersianDatePickerComponent;

public partial class PersianDatePicker : IDisposable
{
    [Inject] public CodecellJsInterop JsInterop { get; set; }

    [Parameter] public bool DarkMode { get; set; }
    [Parameter] public bool Immediate { get; set; }
    [Parameter] public string PlaceHolder { get; set; }
    [Parameter] public string Label { get; set; } = "تاریخ";
    [Parameter] public DateTime? Date { get; set; }
    [Parameter] public Expression<Func<DateTime?>> For { get; set; }
    [Parameter] public EventCallback<DateTime?> DateChanged { get; set; }
    [Parameter] public EventCallback<DateTime?> ValueChanged { get; set; }

    DotNetObjectReference<PersianDatePicker>? objRef;
    CustomValidationMessage<DateTime?> validation;
    DateTime? selectedDate;
    string selectedDateInPersianFormat;
    System.Globalization.PersianCalendar pc = new();
    List<DateCellModel> cells = new();

    private const string ComponentId = "codecell_persian_date_picker_component";
    string fullMonthName;
    int currentMonth;
    int currentYear;
    int currentDay;

    string componentClass = "persian-date-input";
    string pickerClass = "persian-date-wrapper d-none";
    string monthClass = "month-select d-none";
    string yearClass = "year-select d-none";
    string calendarClass = "calendar d-none";
    protected override void OnInitialized()
    {
        objRef = DotNetObjectReference.Create(this);

        if (Date.HasValue)
        {
            SetPersianFormatText(Date.Value);
        }

        if (DarkMode)
        {
            componentClass = "persian-date-input dark";
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JsInterop.AddOutSideClickHandler(ComponentId, objRef);
        }
    }


    [JSInvokable]
    public void InvokeClickOutside()
    {
        pickerClass = "persian-date-wrapper d-none";
        monthClass = "month-select d-none";
        yearClass = "year-select d-none";
        calendarClass = "calendar d-none";
        StateHasChanged();
    }


    void PrepareCells(DateTime date)
    {
        cells = new();
        currentYear = pc.GetYear(date);
        currentMonth = pc.GetMonth(date);
        currentDay = pc.GetDayOfMonth(date);
        var isLeapYear = pc.IsLeapYear(currentYear);

        fullMonthName = $"{currentMonth.GetMonthName()} {currentYear}";

        var firstDayOfDate = new DateTime(currentYear, currentMonth, 1, pc);
        int weekSpan = firstDayOfDate.DayOfWeek.GetWeekSpan();
        var maxDay = currentMonth <= 6 ? 31 : 30;
        if (!isLeapYear && currentMonth == 12 && maxDay == 30)
        {
            maxDay = 29;
        }
        for (int i = 1; i <= weekSpan; i++)
        {
            cells.Add(new DateCellModel
            {
                Day = 0
            });
        }

        for (int i = 1; i <= maxDay; i++)
        {
            cells.Add(new DateCellModel
            {
                Day = i,
                Date = firstDayOfDate.AddDays(i - 1).Date
            });
        }

        var remain = 42 - cells.Count();

        if (remain > 0)
        {
            for (int i = 1; i <= remain; i++)
            {
                cells.Add(new DateCellModel
                {
                    Day = 0
                });
            }
        }
    }

    void SelectDate(DateTime date)
    {
        selectedDate = Date = date;
        pickerClass = "persian-date-wrapper d-none";
        calendarClass = "calendar d-none";
        DateChanged.InvokeAsync(selectedDate);
        ValueChanged.InvokeAsync(selectedDate);
        SetPersianFormatText(Date.Value);
        if (Immediate)
        {
            validation.Immediate();
        }
    }

    void PrevMonth()
    {
        if (currentMonth > 1)
        {
            currentMonth--;
        }
        else if (currentMonth == 1)
        {
            currentMonth = 12;
            currentYear--;
        }
        if (currentMonth == 12 && !pc.IsLeapYear(currentYear) && currentDay > 29)
            currentDay = 1;

        selectedDate = new DateTime(currentYear, currentMonth, currentDay, pc);
        PrepareCells(selectedDate.Value);
    }

    void NextMonth()
    {
        if (currentMonth < 12)
        {
            currentMonth++;
        }
        else if (currentMonth == 12)
        {
            currentMonth = 1;
            currentYear++;
        }
        selectedDate = new DateTime(currentYear, currentMonth, currentDay, pc);
        if (currentMonth == 12 && !pc.IsLeapYear(currentYear) && currentDay > 29)
            currentDay = 1;
        PrepareCells(selectedDate.Value);
    }

    void MonthMode()
    {
        monthClass = "month-select";
        calendarClass = "calendar d-none";
    }

    void YearMode()
    {
        yearClass = "year-select";
        calendarClass = "calendar d-none";
    }

    void GoToMonth(int month)
    {
        currentMonth = month;
        if (currentMonth == 12 && !pc.IsLeapYear(currentYear) && currentDay > 29)
            currentDay = 1;

        var date = new DateTime(currentYear, currentMonth, currentDay, pc);
        monthClass = "month-select d-none";
        calendarClass = "calendar";
        PrepareCells(date);

    }

    void GoToYear(int year)
    {
        currentYear = year;
        if (currentMonth == 12 && !pc.IsLeapYear(currentYear) && currentDay > 29)
            currentDay = 1;

        var date = new DateTime(currentYear, currentMonth, currentDay, pc);
        yearClass = "year-select d-none";
        calendarClass = "calendar";
        PrepareCells(date);

    }

    void OpenPicker()
    {
        if (pickerClass.Equals("persian-date-wrapper"))
        {
            pickerClass = "persian-date-wrapper d-none";
            calendarClass = "calendar d-none";
            return;
        }
        selectedDate = Date.HasValue ? Date : DateTime.Now;
        PrepareCells(selectedDate.Value);
        pickerClass = "persian-date-wrapper";
        calendarClass = "calendar";
    }

    void SetPersianFormatText(DateTime date)
    {
        currentYear = pc.GetYear(date);
        currentMonth = pc.GetMonth(date);
        currentDay = pc.GetDayOfMonth(date);
        selectedDateInPersianFormat = $"{currentYear}/{currentMonth.ToString("D2")}/{currentDay.ToString("D2")}";
    }

    void Clear()
    {
        Date = selectedDate = null;
        selectedDateInPersianFormat = string.Empty;
        pickerClass = "persian-date-wrapper d-none";
        calendarClass = "calendar d-none";
        DateChanged.InvokeAsync(null);
        ValueChanged.InvokeAsync(null);
    }

    void OnValidationChanged(bool status)
    {
        var hasError = !status;

        componentClass = hasError ? $"persian-date-input error" : "persian-date-input";
    }

    public void Dispose()
    {
        JsInterop.RemoveOutSideClickHandler(ComponentId);
    }
}
