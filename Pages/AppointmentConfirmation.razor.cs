using Microsoft.AspNetCore.Components;
using SalonReservations.Models;
using SalonReservations.Services;

namespace SalonReservations.Pages
{
    public partial class AppointmentConfirmation : ComponentBase
    {
        [Inject] AppointmentService _appointmentService { get; set; } = default!;

        Appointment? Appointment { get; set; }
        public string StylistImgSrc { get; set; } = string.Empty;

        protected override void OnInitialized()
        {
            Appointment = _appointmentService.NewAppointment;
            if (Appointment == null) return;

            StylistImgSrc = $"images/stylist{Appointment.Stylist!.UserId}.jpeg";
        }
    }
}