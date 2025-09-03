using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SalonReservations.DAL;
using SalonReservations.DAL.Interfaces;
using SalonReservations.Data;
using SalonReservations.Models;

namespace SalonReservations.Services
{
    public class SalonServices
    {
        private readonly UnitOfWork _unitOfWork = default!;
        private readonly IRepository<ServiceEntity> _servicesRepo = default!;
        private readonly IRepository<StylistServiceEntity> _stylistServicesRepo = default!;

        public SalonServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _servicesRepo = _unitOfWork.Repository<ServiceEntity>();
            _stylistServicesRepo = _unitOfWork.Repository<StylistServiceEntity>();
        }

        public IQueryable<Service> Find(Expression<Func<ServiceEntity, bool>> exp)
        {
            var query = _servicesRepo.Find(exp);
            return query.ProjectToType<Service>();
        }

        public async Task<ServiceEntity?> GetById(int userId)
        {
            return await _servicesRepo.GetByIdAsync(userId);
        }

        public async Task<IEnumerable<ServiceEntity>> ListServices()
        {
            return await _servicesRepo.GetAllAsync();
        }

        public async Task<IEnumerable<ServiceEntity>> AddServices(List<ServiceEntity> services)
        {
            await _servicesRepo.AddRangeAsync(services);
            await _unitOfWork.SaveAsync();
            return services; //TODO: confirm this returns created entities
        }

        public async Task AddStylistServices(int userId, List<int> serviceIds)
        {
            foreach (var serviceId in serviceIds)
                await _stylistServicesRepo.AddAsync(new StylistServiceEntity
                {
                    UserId = userId,
                    ServiceId = serviceId
                });

            await _unitOfWork.SaveAsync();
        }

        public async Task<List<ServiceEntity>> GetStylistServices(int userId)
        {
            var stylistServices = await _stylistServicesRepo.Find(x => x.UserId == userId).Select(x => x.ServiceId).ToListAsync();
            var services = await _servicesRepo.Find(x => stylistServices.Contains(x.ServiceId)).ToListAsync();

            return services ?? new();
        }
    }
}