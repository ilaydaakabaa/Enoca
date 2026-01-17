using Enoca.Application.DTOs;
using Enoca.Application.Interfaces;
using Enoca.Domain.Entities;
using Enoca.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Enoca.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarriersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public CarriersController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var carriers = await _unitOfWork.Carriers
            .GetAllAsync();

        return Ok(carriers);
    }

    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CarrierCreateDto dto)
    {
       
        var carrier = new Carrier
        {
            CarrierName = dto.CarrierName,
            CarrierIsActive = dto.CarrierIsActive,
            CarrierPlusDesiCost = dto.CarrierPlusDesiCost
        };

        await _unitOfWork.Carriers.AddAsync(carrier);
        await _unitOfWork.SaveChangesAsync();

        return Ok("Kargo firması eklendi");
    }

    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CarrierUpdateDto dto)
    {
        var existing = await _unitOfWork.Carriers.GetByIdAsync(id);
        if (existing is null)
            return NotFound($"{id} ID'li kayıt bulunamadı");

        existing.CarrierName = dto.CarrierName;
        existing.CarrierIsActive = dto.CarrierIsActive;
        existing.CarrierPlusDesiCost = dto.CarrierPlusDesiCost;

        _unitOfWork.Carriers.Update(existing);
        await _unitOfWork.SaveChangesAsync();

        return Ok("Kargo bilgileri güncellendi");
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var carrier = await _unitOfWork.Carriers.GetByIdAsync(id);
        if (carrier is null)
            return NotFound($"{id} ID'li kayıt bulunamadı");

        var hasConfigs = (await _unitOfWork.CarrierConfigurations.WhereAsync(x => x.CarrierId == id)).Any();
        if (hasConfigs)
            return BadRequest("Bu kargo firmasına ait konfigürasyonlar var. Önce konfigürasyonları silmelisiniz.");

        var hasOrders = (await _unitOfWork.Orders.WhereAsync(x => x.CarrierId == id)).Any();
        if (hasOrders)
            return BadRequest("Bu kargo firmasına ait siparişler var. Önce siparişleri silmelisiniz.");

        _unitOfWork.Carriers.Remove(carrier);
        await _unitOfWork.SaveChangesAsync();

        return Ok($"{id} ID'li kayıt silindi");
    }
}
