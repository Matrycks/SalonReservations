using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using SalonReservations.Models;

namespace SalonReservations.Data
{
    public class AppointmentEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentId { get; set; }
        public int ClientId { get; set; }
        public int StylistId { get; set; }
        [ForeignKey("StylistId")]
        public StylistEntity Stylist { get; set; } = null!;
        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public ServiceEntity Service { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}