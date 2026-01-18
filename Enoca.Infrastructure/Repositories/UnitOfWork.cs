using Enoca.Application.Interfaces;
using Enoca.Domain.Entities;
using Enoca.Infrastructure.Data;


namespace Enoca.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Carriers = new GenericRepository<Carrier>(_context);
            CarrierConfigurations = new GenericRepository<CarrierConfiguration>(_context);
            Orders = new GenericRepository<Order>(_context);
            CarrierReports = new GenericRepository<CarrierReport>(_context);

        }

        public IGenericRepository<Carrier> Carriers { get; }
        public IGenericRepository<CarrierConfiguration> CarrierConfigurations { get; }
        public IGenericRepository<Order> Orders { get; }

        public IGenericRepository<CarrierReport> CarrierReports { get; private set; }


        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
