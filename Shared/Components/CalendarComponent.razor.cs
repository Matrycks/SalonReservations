using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace SalonReservations.Shared.Components
{
    public partial class CalendarComponent : ComponentBase
    {
        [Inject] private ILogger<CalendarComponent> _logger { get; set; } = default!;
        [Inject] private IJSRuntime JS { get; set; } = default!;

        private DateTime Date { get; set; } = DateTime.Now;
        private DateTime SelectedDate { get; set; } = DateTime.Now;
        [Parameter] public EventCallback<DateTime> OnDateChanged { get; set; }

        protected override void OnInitialized()
        {

        }

        private void MonthChanged(string indicator)
        {
            if (indicator == "prev")
                Date = Date.AddMonths(-1);
            else
                Date = Date.AddMonths(1);

            DateTime currentDate = DateTime.Now;
            if (Date.Month == currentDate.Month && Date.Year == currentDate.Year)
                SelectedDate = Date;

            OnDateChanged.InvokeAsync(Date);
        }

        private void DayClicked(int day)
        {
            SelectedDate = new DateTime(Date.Year, Date.Month, day);

            OnDateChanged.InvokeAsync(SelectedDate);
        }

        private bool IsSelectedDate(DateTime date)
        {
            return SelectedDate.Date == date.Date;
        }
    }
}