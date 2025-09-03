using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalonReservations.Models
{
    public class Availability
    {
        public int StylistId { get; set; }
        public List<TimeSlot> AvailableSlots { get; set; } = new();
    }
}