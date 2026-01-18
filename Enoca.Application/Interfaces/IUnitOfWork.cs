using Enoca.Domain.Entities;


namespace Enoca.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Carrier> Carriers { get; }
        IGenericRepository<CarrierConfiguration> CarrierConfigurations { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<CarrierReport> CarrierReports { get; }


        Task<int> SaveChangesAsync();
    }
}
