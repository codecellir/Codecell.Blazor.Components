using Codecell.Component.Blazor.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Linq.Expressions;

namespace Codecell.Component.Blazor.Components.PersianDatePickerComponent;

public partial class PersianDatePicker : IDisposable
{
    [Inject] public PersianDatePickerJsInterop JsInterop { get; set; }

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
    System.Globalization.PersianCalendar pc = new();
    List<DateCellModel> cells = new();

    private string ComponentId = "codecell_persian_date_picker_component";
    private string InputId = "codecell-p-date";
    string componentClass = "persian-date-input";
    string pickerClass = "persian-date-wrapper d-none";
    string monthClass = "month-select d-none";
    string yearClass = "year-select d-none";
    string calendarClass = "calendar d-none";
    string persianDateFormat = string.Empty;
    string fullMonthName = string.Empty;
    int currentMonth;
    int currentYear;
    int currentDay;

    protected override void OnInitialized()
    {
        ComponentId = $"{ComponentId}_{Guid.NewGuid()}";
        InputId = $"{InputId}_{Guid.NewGuid()}";
        objRef = DotNetObjectReference.Create(this);

        var intitialDate = Date;

        Clear();

        if (intitialDate.HasValue)
        {
            SetPersianFormatText(intitialDate.Value);
            Date = intitialDate;
            DateChanged.InvokeAsync(Date);
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
            await JsInterop.AddDateMask(InputId, objRef);
        }
        else if (Date.HasValue)
            SetPersianFormatText(Date.Value);
        else
            ClearWithoutInvoke();

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

    [JSInvokable]
    public async Task InvokeMakComplete()
    {
        var persiandateValue = await JsInterop.GetValue(InputId);
        var date = persiandateValue.ToGeorgianDate();

        if (date.HasValue)
        {
            SelectDate(date.Value);
            StateHasChanged();
        }

    }
    void OnKeyupHandler(KeyboardEventArgs e)
    {
        var date = persianDateFormat.ToGeorgianDate();

        if (!date.HasValue)
        {
            Date = null;
            DateChanged.InvokeAsync(Date);
            ValueChanged.InvokeAsync(Date);
            StateHasChanged();
        }
    }
    void OnKeydownHandler(KeyboardEventArgs e)
    {
        if (e.CtrlKey && e.Key == "Enter" && !Date.HasValue)
        {
            SelectDate(DateTime.Now);
        }
    }
    void GoToToday() => SelectDate(DateTime.Now);
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
        if (Immediate && validation is not null)
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

        var date = new DateTime(currentYear, currentMonth, currentDay, pc);
        PrepareCells(date);
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
        var date = new DateTime(currentYear, currentMonth, currentDay, pc);
        if (currentMonth == 12 && !pc.IsLeapYear(currentYear) && currentDay > 29)
            currentDay = 1;
        PrepareCells(date);
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
        persianDateFormat = $"{currentYear}/{currentMonth.ToString("D2")}/{currentDay.ToString("D2")}";
    }
    void Clear()
    {
        Date = selectedDate = null;
        persianDateFormat = "1___/__/__";
        pickerClass = "persian-date-wrapper d-none";
        calendarClass = "calendar d-none";
        monthClass = "month-select d-none";
        yearClass = "year-select d-none";
        DateChanged.InvokeAsync(null);
        ValueChanged.InvokeAsync(null);
        JsInterop.ResetMask(InputId);
    }
    void ClearWithoutInvoke()
    {
        Date = selectedDate = null;
        persianDateFormat = "1___/__/__";
        pickerClass = "persian-date-wrapper d-none";
        calendarClass = "calendar d-none";
        monthClass = "month-select d-none";
        yearClass = "year-select d-none";
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
