using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalonReservations.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int ClientId { get; set; }
        public int StylistId { get; set; }
        public Stylist? Stylist { get; set; }
        public int ServiceId { get; set; }
        public Service? Service { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}