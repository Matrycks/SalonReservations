using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalonReservations.Models;
using SalonReservations.Services;

namespace SalonReservations.Data
{
    public static class SeedDatabase
    {
        public static async Task Init(SalonDbContext dbContext, AvailabilityService availService)
        {
            await InitServices(dbContext);
            await InitStylists(dbContext);
            await GenerateStylistAvailability(availService);
        }

        public static async Task InitServices(SalonDbContext dbContext)
        {
            dbContext.Services.AddRange
            (
                new List<ServiceEntity>
                {
                    new ServiceEntity
                    {
                        Name = "Haircut & Styling",
                        Desc = "Includes shampoo, cut, blow-dry, and basic styling. Customized to suit face shape and preference.",
                        Duration = 60,
                        Price = 120
                    },
                    new ServiceEntity
                    {
                        Name = "Blowout",
                        Desc = "Wash, blow-dry, and style for smooth, voluminous hair. Ideal for special occasions.",
                        Duration = 60,
                        Price = 85
                    },
                    new ServiceEntity
                    {
                        Name = "Hair Coloring",
                        Desc = "Full color application, root touch-up, or grey coverage using permanent or semi-permanent dye.",
                        Duration = 60,
                        Price = 120
                    },
                    new ServiceEntity
                    {
                        Name = "Deep Conditioning",
                        Desc = "Intensive moisture and repair treatment for dry or damaged hair.",
                        Duration = 30,
                        Price = 80
                    }
                }
            );
            await dbContext.SaveChangesAsync();
        }

        public static async Task InitStylists(SalonDbContext dbContext)
        {
            string bio = "I’m a stylist who loves helping people feel their best. Whether it’s a fresh cut or a bold color, I’m all about creating looks that fit your vibe.";
            string email = "user@salon.com";

            dbContext.Stylists.AddRange
            (
                new List<StylistEntity>
                {
                    new StylistEntity
                    {
                        Name = "Aaliyah",
                        Bio = bio,
                        Email = email,
                        Specialties = new List<StylistServiceEntity>
                        {
                            new StylistServiceEntity { ServiceId = 1 },
                            new StylistServiceEntity { ServiceId = 2 },
                            new StylistServiceEntity { ServiceId = 3 }
                        }
                    },
                    new StylistEntity
                    {
                        Name = "Angel",
                        Bio = bio,
                        Email = email,
                        Specialties = new List<StylistServiceEntity>
                        {
                            new StylistServiceEntity { ServiceId = 1 },
                            new StylistServiceEntity { ServiceId = 3 },
                            new StylistServiceEntity { ServiceId = 4 }
                        }
                    },
                    new StylistEntity
                    {
                        Name = "Janee",
                        Bio = bio,
                        Email = email,
                        Specialties = new List<StylistServiceEntity>
                        {
                            new StylistServiceEntity { ServiceId = 1 },
                            new StylistServiceEntity { ServiceId = 2 },
                            new StylistServiceEntity { ServiceId = 3 },
                            new StylistServiceEntity { ServiceId = 4 }
                        }
                    },
                    new StylistEntity
                    {
                        Name = "Princess",
                        Bio = bio,
                        Email = email,
                        Specialties = new List<StylistServiceEntity>
                        {
                            new StylistServiceEntity { ServiceId = 3 },
                            new StylistServiceEntity { ServiceId = 4 }
                        }
                    }
                }
            );

            await dbContext.SaveChangesAsync();
        }

        public static async Task GenerateStylistAvailability(AvailabilityService availService)
        {
            await availService.GenerateStylistAvailability(DateTime.Today.AddHours(8), DateTime.Today.AddHours(16));
        }
    }
}