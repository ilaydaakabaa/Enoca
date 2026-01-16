

namespace Enoca.Application.Services
{
    public interface  IShippingCostCalculator
    {
        Task<ShippingResult> CalculateAsync(int orderDesi);
    }
}
