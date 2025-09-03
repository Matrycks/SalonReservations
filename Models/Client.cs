using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalonReservations.Data;

namespace SalonReservations.Models
{
    public class Client : User
    {
        public string? Phone { get; set; }
    }
}