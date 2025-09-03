using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalonReservations.DAL.Interfaces;
using SalonReservations.Data;
using SalonReservations.Repos;

namespace SalonReservations.DAL
{
    public class UnitOfWork
    {
        private readonly SalonDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(SalonDbContext context)
        {
            _context = context;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
                return (IRepository<T>)_repositories[typeof(T)];

            var repo = new GenericRepo<T>(_context);
            _repositories[typeof(T)] = repo;
            return repo;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}