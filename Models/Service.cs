using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalonReservations.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public string Name { get; set; } = null!;
        public double Duration { get; set; } //in minutes
        public float Price { get; set; }
    }
}