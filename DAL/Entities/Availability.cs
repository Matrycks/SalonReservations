using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalonReservations.Data
{
    public class AvailabilityEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AvailabilityId { get; set; }
        public int StylistId { get; set; }

        [ForeignKey("StylistId")]
        public StylistEntity Stylist { get; set; } = null!;
        public ICollection<TimeSlotEntity>? AvailableSlots { get; set; }
    }
}