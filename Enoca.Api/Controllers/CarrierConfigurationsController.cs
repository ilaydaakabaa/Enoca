using Enoca.Application.DTOs;
using Enoca.Application.Interfaces;
using Enoca.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Enoca.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarrierConfigurationsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public CarrierConfigurationsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var configs = await _unitOfWork.CarrierConfigurations.GetAllAsync();
        return Ok(configs);
    }

    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CarrierConfigurationCreateDto dto)
    {
        if (dto.CarrierMinDesi > dto.CarrierMaxDesi)
            return BadRequest("CarrierMinDesi, CarrierMaxDesi'den büyük olamaz.");

        if (dto.CarrierCost <= 0)
            return BadRequest("CarrierCost 0'dan büyük olmalıdır.");
        var carrier = await _unitOfWork.Carriers.GetByIdAsync(dto.CarrierId);
        if (carrier is null)
            return BadRequest("CarrierId geçersiz. Böyle bir kargo firması yok.");


        var config = new CarrierConfiguration
        {
            CarrierId = dto.CarrierId,
            CarrierMinDesi = dto.CarrierMinDesi,
            CarrierMaxDesi = dto.CarrierMaxDesi,
            CarrierCost = dto.CarrierCost
        };

        await _unitOfWork.CarrierConfigurations.AddAsync(config);
        await _unitOfWork.SaveChangesAsync();

        return Ok("Kargo firma konfigürasyonu eklendi");
    }

    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CarrierConfigurationUpdateDto dto)
    {
        var existing = await _unitOfWork.CarrierConfigurations.GetByIdAsync(id);
        if (existing is null)
            return NotFound($"{id} ID'li kayıt bulunamadı");
        if (dto.CarrierMinDesi > dto.CarrierMaxDesi)
            return BadRequest("CarrierMinDesi, CarrierMaxDesi'den büyük olamaz.");
        if (dto.CarrierCost <= 0)
            return BadRequest("CarrierCost 0'dan büyük olmalıdır.");

        var carrier = await _unitOfWork.Carriers.GetByIdAsync(dto.CarrierId);
        if (carrier is null)
            return BadRequest("CarrierId geçersiz. Böyle bir kargo firması yok.");

        existing.CarrierId = dto.CarrierId;
        existing.CarrierMinDesi = dto.CarrierMinDesi;
        existing.CarrierMaxDesi = dto.CarrierMaxDesi;
        existing.CarrierCost = dto.CarrierCost;

        _unitOfWork.CarrierConfigurations.Update(existing);
        await _unitOfWork.SaveChangesAsync();

        return Ok("Kargo firma konfigürasyonu güncellendi");
    }
        
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var config = await _unitOfWork.CarrierConfigurations.GetByIdAsync(id);
        if (config is null)
            return NotFound($"{id} ID'li kayıt bulunamadı");

        _unitOfWork.CarrierConfigurations.Remove(config);
        await _unitOfWork.SaveChangesAsync();

        return Ok($"{id} ID'li kayıt silindi");
    }
}
