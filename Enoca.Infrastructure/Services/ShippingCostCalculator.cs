using Enoca.Application.Services;
using Enoca.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Enoca.Infrastructure.Services
{
    public class ShippingCostCalculator : IShippingCostCalculator
    {
        private readonly AppDbContext _context;

        public ShippingCostCalculator(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ShippingResult> CalculateAsync(int orderDesi)
        {
            if (orderDesi <= 0)
                throw new ArgumentException("OrderDesi 0'dan büyük olmalıdır.");

            // 1) Desi aralığına giren konfigürasyonları bul (aktif kargolar)
            var inRangeConfigs = await _context.CarrierConfigurations
                .AsNoTracking()
                .Include(x => x.Carrier)
                .Where(x =>
                    x.Carrier.CarrierIsActive &&
                    orderDesi >= x.CarrierMinDesi &&
                    orderDesi <= x.CarrierMaxDesi)
                .ToListAsync();

            // 1.a) Aralığa giriyorsa en düşük CarrierCost seç
            if (inRangeConfigs.Any())
            {
                var cheapest = inRangeConfigs
                    .OrderBy(x => x.CarrierCost)
                    .First();

                return new ShippingResult
                {
                    CarrierId = cheapest.CarrierId,
                    CalculatedCost = cheapest.CarrierCost
                };
            }

            // 2) Aralığa girmiyorsa: en yakın config bul
            // Örneğe uygun yaklaşım: orderDesi'den küçük/eşit MaxDesi'ler içinde en büyük MaxDesi
            var nearestConfig = await _context.CarrierConfigurations
                .AsNoTracking()
                .Include(x => x.Carrier)
                .Where(x => x.Carrier.CarrierIsActive && x.CarrierMaxDesi <= orderDesi)
                .OrderByDescending(x => x.CarrierMaxDesi)
                .FirstOrDefaultAsync();

            if (nearestConfig is null)
            {
                // Fallback: aktif config yoksa hata
                nearestConfig = await _context.CarrierConfigurations
                    .AsNoTracking()
                    .Include(x => x.Carrier)
                    .Where(x => x.Carrier.CarrierIsActive)
                    .OrderBy(x => x.CarrierMaxDesi)
                    .FirstOrDefaultAsync();

                if (nearestConfig is null)
                    throw new InvalidOperationException("Aktif kargo konfigürasyonu bulunamadı.");
            }

            var diff = orderDesi - nearestConfig.CarrierMaxDesi;
            if (diff < 0) diff = 0;

            var calculatedCost = nearestConfig.CarrierCost + (nearestConfig.Carrier.CarrierPlusDesiCost * diff);

            return new ShippingResult
            {
                CarrierId = nearestConfig.CarrierId,
                CalculatedCost = calculatedCost
            };
        }
    }
}
