using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalonReservations.Data
{
    public class ServiceEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceId { get; set; }
        public string Name { get; set; } = null!;
        public string? Desc { get; set; }
        public double Duration { get; set; } //in minutes
        public float Price { get; set; }
    }

    public class StylistServiceEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StylistServiceId { get; set; }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public StylistEntity Stylist { get; set; } = null!;
        public int ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        public ServiceEntity Service { get; set; } = null!;
    }
}