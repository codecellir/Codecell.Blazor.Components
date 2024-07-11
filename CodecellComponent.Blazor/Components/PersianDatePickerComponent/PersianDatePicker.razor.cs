using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CodecellComponent.Blazor.Components.PersianDatePickerComponent
{
    public partial class PersianDatePicker : IDisposable
    {
        [Inject] public CodecellJsInterop JsInterop { get; set; }

        [Parameter] public DateTime? Date { get; set; }

        [Parameter] public EventCallback<DateTime?> DateChanged { get; set; }

        DotNetObjectReference<PersianDatePicker>? objRef;

        DateTime? selectedDate;
        string selectedDateInPersianFormat;
        System.Globalization.PersianCalendar pc = new();
        List<DateCellModel> cells = new();

        private const string ComponentId = "codecell_persian_date_picker_component";
        string fullMonthName;
        int currentMonth;
        int currentYear;
        int currentDay;
        bool monthMode, yearMode, openPicker;
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
            openPicker = false;
            pickerClass = "persian-date-wrapper d-none";
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
            openPicker = false;
            pickerClass = "persian-date-wrapper d-none";
            calendarClass = "calendar d-none";
            DateChanged.InvokeAsync(selectedDate);

            SetPersianFormatText(Date.Value);
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
            monthMode = true;
            yearMode = false;
            monthClass = "month-select";
            calendarClass = "calendar d-none";
        }

        void YearMode()
        {
            yearMode = true;
            monthMode = false;
            yearClass = "year-select";
            calendarClass = "calendar d-none";
        }

        void GoToMonth(int month)
        {
            currentMonth = month;
            if (currentMonth == 12 && !pc.IsLeapYear(currentYear) && currentDay > 29)
                currentDay = 1;

            var date = new DateTime(currentYear, currentMonth, currentDay, pc);
            monthMode = yearMode = false;
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
            monthMode = yearMode = false;
            yearClass = "year-select d-none";
            calendarClass = "calendar";
            PrepareCells(date);

        }

        void OpenPicker()
        {
            if (openPicker)
            {
                openPicker = false;
                pickerClass = "persian-date-wrapper d-none";
                calendarClass = "calendar d-none";
                return;
            }
            selectedDate = Date.HasValue ? Date : DateTime.Now;
            PrepareCells(selectedDate.Value);
            openPicker = true;
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
            openPicker = false;
            pickerClass = "persian-date-wrapper d-none";
            calendarClass = "calendar d-none";
            DateChanged.InvokeAsync(null);
        }

        public void Dispose()
        {
            JsInterop.RemoveOutSideClickHandler(ComponentId);
        }
    }
}
