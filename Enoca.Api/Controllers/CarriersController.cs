using Enoca.Domain.Entities;
using Enoca.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Enoca.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarriersController : ControllerBase
{
    private readonly AppDbContext _context;

    public CarriersController(AppDbContext context)
    {
        _context = context;
    }

   
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var carriers = await _context.Carriers
            .AsNoTracking()
            .ToListAsync();

        return Ok(carriers);
    }

    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Carrier carrier)
    {
        if (carrier is null)
            return BadRequest("Geçersiz istek.");

        if (string.IsNullOrWhiteSpace(carrier.CarrierName))
            return BadRequest("CarrierName zorunludur.");

        await _context.Carriers.AddAsync(carrier);
        await _context.SaveChangesAsync();

        return Ok("Kargo firması eklendi");
    }

    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Carrier carrier)
    {
        var existing = await _context.Carriers.FindAsync(id);
        if (existing is null)
            return NotFound($"{id} ID'li kayıt bulunamadı");

        if (string.IsNullOrWhiteSpace(carrier.CarrierName))
            return BadRequest("CarrierName zorunludur.");

        existing.CarrierName = carrier.CarrierName;
        existing.CarrierIsActive = carrier.CarrierIsActive;
        existing.CarrierPlusDesiCost = carrier.CarrierPlusDesiCost;

        await _context.SaveChangesAsync();

        return Ok("Kargo bilgileri güncellendi");
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var carrier = await _context.Carriers.FindAsync(id);
        if (carrier is null)
            return NotFound($"{id} ID'li kayıt bulunamadı");

        var hasConfigs = await _context.CarrierConfigurations.AnyAsync(x => x.CarrierId == id);
        if (hasConfigs)
            return BadRequest("Bu kargo firmasına ait konfigürasyonlar var. Önce konfigürasyonları silmelisiniz.");

        var hasOrders = await _context.Orders.AnyAsync(x => x.CarrierId == id);
        if (hasOrders)
            return BadRequest("Bu kargo firmasına ait siparişler var. Önce siparişleri silmelisiniz.");

        _context.Carriers.Remove(carrier);
        await _context.SaveChangesAsync();

        return Ok($"{id} ID'li kayıt silindi");
    }
}
