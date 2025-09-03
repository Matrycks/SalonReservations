using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalonReservations.Data
{
    public class ClientEntity : User
    {
        public string? Phone { get; set; }
    }
}