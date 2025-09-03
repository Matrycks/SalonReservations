using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SalonReservations.DAL;
using SalonReservations.DAL.Interfaces;
using SalonReservations.Data;
using SalonReservations.Models;

namespace SalonReservations.Services
{
    public class AvailabilityService
    {
        private StylistService _stylistService { get; set; }
        private IRepository<TimeSlotEntity> _availabilitySlotsRepo { get; set; }
        private readonly UnitOfWork _unitOfWork;

        public AvailabilityService(UnitOfWork unitOfWork, StylistService stylistService)
        {
            _unitOfWork = unitOfWork;
            _stylistService = stylistService;
            _availabilitySlotsRepo = unitOfWork.Repository<TimeSlotEntity>();
        }

        public async Task AddAvailability(int userId, List<TimeSlot> timeSlots)
        {
            bool stylistExist = await _stylistService.AnyAsync(x => x.UserId == userId);
            if (stylistExist)
            {
                var nTimeSlots = timeSlots.Adapt<List<TimeSlotEntity>>();
                await _availabilitySlotsRepo.AddRangeAsync
                (
                    nTimeSlots
                );
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> IsTimeSlotAvailable(int userId, DateTime reqDatetime, double serviceDuration)
        {
            DateTime endDatetime = reqDatetime.AddMinutes(serviceDuration);
            var isAvailable = await _availabilitySlotsRepo.AnyAsync(x => x.UserId == userId
                && x.Start >= reqDatetime && x.End <= endDatetime);

            return isAvailable;
        }

        public async Task<List<TimeSlot>?> GetAvailability(int userId)
        {
            var slots = await _availabilitySlotsRepo.Find(x => x.UserId == userId).ToListAsync();
            return slots.OrderBy(x => x.Start).Adapt<List<TimeSlot>>();
        }

        public async Task<List<TimeSlot>?> GetAvailability(int userId, DateTime date)
        {
            var slots = await _availabilitySlotsRepo.Find(x => x.UserId == userId && x.Start.Date == date.Date).ToListAsync();
            return slots.OrderBy(x => x.Start).Adapt<List<TimeSlot>>();
        }

        public async Task UpdateAvailability(int userId, DateTime appointmentStart, double serviceDuration)
        {
            var availableSlots = await _availabilitySlotsRepo.Find(x => x.UserId == userId
                && x.Start.Date == appointmentStart.Date).ToListAsync();
            if (availableSlots == null) return;

            DateTime appointmentEnd = appointmentStart.AddMinutes(serviceDuration);
            int numOfAvailSlots = availableSlots.Count();

            foreach (var slot in availableSlots)
            {
                //Check if appointment falls within slot
                if (appointmentStart >= slot.Start && appointmentEnd <= slot.End)
                {
                    //Remove original slot
                    _availabilitySlotsRepo.Remove(slot);

                    //Create left-over slot times
                    if (appointmentStart > slot.Start)
                    {
                        await _availabilitySlotsRepo.AddAsync(new TimeSlotEntity
                        {
                            Start = slot.Start,
                            End = appointmentStart
                        });
                        continue; //move to next slot (handles appointments that spans multiple slots)
                    }

                    if (appointmentEnd < slot.End)
                    {
                        await _availabilitySlotsRepo.AddAsync(new TimeSlotEntity
                        {
                            Start = appointmentEnd,
                            End = slot.End
                        });
                    }

                    break; //Only one matching slot should exist
                }
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task GenerateStylistAvailability(DateTime start, DateTime end)
        {
            var stylists = await _stylistService.GetStylistsAsync();
            foreach (var stylist in stylists)
            {
                List<TimeSlotEntity> availSlots = new List<TimeSlotEntity>();
                DateTime current = start;

                //NOTE: skip if temp data already exist
                var hasSlots = await _availabilitySlotsRepo.AnyAsync(x => x.Start.Date == current.Date);
                if (hasSlots) return;

                while (current < end)
                {
                    availSlots.Add(new TimeSlotEntity
                    {
                        UserId = stylist.UserId,
                        Start = current,
                        End = current.AddHours(1)
                    });

                    current = current.AddHours(1);
                }

                await _availabilitySlotsRepo.AddRangeAsync(availSlots);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAvailability_Old(int userId, DateTime appointmentStart, double serviceDuration)
        {
            var availableSlots = await _availabilitySlotsRepo.Find(x => x.UserId == userId
                && x.Start.Date == appointmentStart.Date).ToListAsync();
            if (availableSlots == null) return;

            DateTime appointmentEnd = appointmentStart.AddMinutes(serviceDuration);
            int numOfAvailSlots = availableSlots.Count();

            for (int i = 0; i < numOfAvailSlots; i++)
            {
                var slot = availableSlots[i];

                //Check if appointment falls within slot
                if (appointmentStart >= slot.Start && appointmentEnd <= slot.End)
                {
                    //Remove original slot
                    availableSlots.RemoveAt(i);

                    //Create left-over slot times
                    if (appointmentStart > slot.Start)
                    {
                        availableSlots.Insert(i, new TimeSlotEntity
                        {
                            Start = slot.Start,
                            End = appointmentStart
                        });
                        i++; //move to next slot (handles appointments that spans multiple slots)
                    }

                    if (appointmentEnd < slot.End)
                    {
                        availableSlots.Insert(i, new TimeSlotEntity
                        {
                            Start = appointmentEnd,
                            End = slot.End
                        });
                    }

                    break; //Only one matching slot should exist
                }
            }

            await _unitOfWork.SaveAsync();
        }
    }
}