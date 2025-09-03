using Microsoft.EntityFrameworkCore;

namespace SalonReservations.Data
{
    public class SalonDbContext : DbContext
    {
        public DbSet<AvailabilityEntity> Availabilities { get; set; }
        public DbSet<AppointmentEntity> Appointments { get; set; }
        public DbSet<StylistEntity> Stylists { get; set; }
        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<StylistServiceEntity> StylistServices { get; set; }
        public DbSet<ServiceEntity> Services { get; set; }
        public DbSet<TimeSlotEntity> TimeSlots { get; set; }

        public SalonDbContext(DbContextOptions<SalonDbContext> options)
            : base(options)
        {

        }
    }
}