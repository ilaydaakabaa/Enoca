using Enoca.Application.DTOs;
using Enoca.Domain.Entities;
using Enoca.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Enoca.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarrierConfigurationsController : ControllerBase
{
    private readonly AppDbContext _context;

    public CarrierConfigurationsController(AppDbContext context)
    {
        _context = context;
    }

    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var configs = await _context.CarrierConfigurations
            .AsNoTracking()
            .Include(x => x.Carrier)
            .ToListAsync();

        return Ok(configs);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CarrierConfigurationCreateDto dto)
    {
        if (dto is null)
            return BadRequest("Geçersiz istek.");

        var carrierExists = await _context.Carriers.AnyAsync(x => x.CarrierId == dto.CarrierId);
        if (!carrierExists)
            return BadRequest("CarrierId geçersiz. Böyle bir kargo firması yok.");

        if (dto.CarrierMinDesi <= 0) return BadRequest("CarrierMinDesi 0'dan büyük olmalıdır.");
        if (dto.CarrierMaxDesi <= 0) return BadRequest("CarrierMaxDesi 0'dan büyük olmalıdır.");
        if (dto.CarrierMinDesi > dto.CarrierMaxDesi) return BadRequest("CarrierMinDesi, CarrierMaxDesi'den büyük olamaz.");
        if (dto.CarrierCost <= 0) return BadRequest("CarrierCost 0'dan büyük olmalıdır.");

        var config = new CarrierConfiguration
        {
            CarrierId = dto.CarrierId,
            CarrierMinDesi = dto.CarrierMinDesi,
            CarrierMaxDesi = dto.CarrierMaxDesi,
            CarrierCost = dto.CarrierCost
        };

        await _context.CarrierConfigurations.AddAsync(config);
        await _context.SaveChangesAsync();

        return Ok("Kargo firma konfigürasyonu eklendi");
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CarrierConfiguration config)
    {
        var existing = await _context.CarrierConfigurations.FindAsync(id);
        if (existing is null)
            return NotFound($"{id} ID'li kayıt bulunamadı");

        // Carrier var mı kontrol
        var carrierExists = await _context.Carriers.AnyAsync(x => x.CarrierId == config.CarrierId);
        if (!carrierExists)
            return BadRequest("CarrierId geçersiz. Böyle bir kargo firması yok.");

        if (config.CarrierMinDesi <= 0)
            return BadRequest("CarrierMinDesi 0'dan büyük olmalıdır.");

        if (config.CarrierMaxDesi <= 0)
            return BadRequest("CarrierMaxDesi 0'dan büyük olmalıdır.");

        if (config.CarrierMinDesi > config.CarrierMaxDesi)
            return BadRequest("CarrierMinDesi, CarrierMaxDesi'den büyük olamaz.");

        if (config.CarrierCost <= 0)
            return BadRequest("CarrierCost 0'dan büyük olmalıdır.");

        existing.CarrierId = config.CarrierId;
        existing.CarrierMinDesi = config.CarrierMinDesi;
        existing.CarrierMaxDesi = config.CarrierMaxDesi;
        existing.CarrierCost = config.CarrierCost;

        await _context.SaveChangesAsync();

        return Ok("Kargo firma konfigürasyonu güncellendi");
    }

    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var config = await _context.CarrierConfigurations.FindAsync(id);
        if (config is null)
            return NotFound($"{id} ID'li kayıt bulunamadı");

        _context.CarrierConfigurations.Remove(config);
        await _context.SaveChangesAsync();

        return Ok($"{id} ID'li kayıt silindi");
    }
}
