using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalonReservations.Data;

namespace SalonReservations.Data
{
    public class StylistEntity : User
    {
        public string Bio { get; set; } = null!;
        public ICollection<StylistServiceEntity>? Specialties { get; set; }
        public ICollection<TimeSlotEntity>? Availability { get; set; }
    }
}