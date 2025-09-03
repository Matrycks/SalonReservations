using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalonReservations.Data;

namespace SalonReservations.Models
{
    public class Stylist : User
    {
        public string Bio { get; set; } = null!;
        public List<Service> Specialties { get; set; } = new();
    }
}