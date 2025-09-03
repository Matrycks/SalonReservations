using Microsoft.AspNetCore.Components;
using SalonReservations.Models;
using SalonReservations.Services;

namespace SalonReservations.Pages
{
    public partial class Appointments
    {
        [Inject] private ILogger<Appointments> _logger { get; set; } = default!;
        [Inject] private AppointmentService _appointmentService { get; set; } = default!;

        private List<Appointment> _appoinments { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {
            _appoinments = await _appointmentService.GetAll();
        }
    }
}