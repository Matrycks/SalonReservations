using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SalonReservations.DAL;
using SalonReservations.DAL.Interfaces;
using SalonReservations.Data;
using SalonReservations.Models;

namespace SalonReservations.Services
{
    public class StylistService
    {
        private readonly UnitOfWork _unitOfWork = default!;
        private readonly IRepository<StylistEntity> _stylistRepo = default!;
        private readonly IRepository<StylistServiceEntity> _stylistServicesRepo = default!;
        private readonly IRepository<ServiceEntity> _servicesRepo = default!;
        private readonly IRepository<TimeSlotEntity> _timeSlotsRepo = default!;

        public StylistService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _stylistRepo = _unitOfWork.Repository<StylistEntity>();
            _stylistServicesRepo = _unitOfWork.Repository<StylistServiceEntity>();
            _servicesRepo = _unitOfWork.Repository<ServiceEntity>();
            _timeSlotsRepo = _unitOfWork.Repository<TimeSlotEntity>();
        }

        public async Task<Stylist> GetStylistAsync(int stylistId)
        {
            var stylist = await _stylistRepo.GetByIdAsync(stylistId);
            return stylist.Adapt<Stylist>();
        }

        public async Task<List<Stylist>> GetStylistsAsync()
        {
            var entities = await _stylistRepo.Query().Include(x => x.Specialties).ToListAsync();
            return entities.Adapt<List<Stylist>>();
        }

        public async Task<List<Stylist>> GetStylistsAsync(string service)
        {
            var entities = await (from stylists in _stylistRepo.Query()
                                  join ss in _stylistServicesRepo.Query() on stylists.UserId equals ss.UserId
                                  join services in _servicesRepo.Query() on ss.ServiceId equals services.ServiceId
                                  where services.Name.ToLower() == service.ToLower()
                                  select stylists)
                                  .ToListAsync();
            return entities.Adapt<List<Stylist>>();
        }

        public async Task<List<TimeSlot>> GetTimeSlotsAsync(int stylistId)
        {
            var slots = await _timeSlotsRepo.Query().Where(x => x.UserId == stylistId).ToListAsync();
            return slots.Adapt<List<TimeSlot>>();
        }

        public async Task<bool> AnyAsync(Expression<Func<TimeSlotEntity, bool>> exp)
        {
            return await _timeSlotsRepo.AnyAsync(exp);
        }
    }
}