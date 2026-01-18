using Enoca.Application.Interfaces;
using Enoca.Domain.Entities;

namespace Enoca.Application.Services;

public class CarrierReportJob : ICarrierReportJob
{
    private readonly IUnitOfWork _unitOfWork;

    public CarrierReportJob(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task RunAsync()
    {
        var orders = await _unitOfWork.Orders.GetAllAsync();

        var grouped = orders
            .GroupBy(o => new { o.CarrierId, Date = o.OrderDate.Date })
            .Select(g => new
            {
                g.Key.CarrierId,
                ReportDate = g.Key.Date,
                Total = g.Sum(x => x.OrderCarrierCost)
            })
            .ToList();

        foreach (var item in grouped)
        {
            var existing = (await _unitOfWork.CarrierReports.WhereAsync(r =>
                r.CarrierId == item.CarrierId &&
                r.CarrierReportDate.Date == item.ReportDate.Date
            )).FirstOrDefault();

            if (existing is null)
            {
                await _unitOfWork.CarrierReports.AddAsync(new CarrierReport
                {
                    CarrierId = item.CarrierId,
                    CarrierReportDate = item.ReportDate,
                    CarrierCost = item.Total
                });
            }
            else
            {
                existing.CarrierCost = item.Total;
                _unitOfWork.CarrierReports.Update(existing);
            }
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
