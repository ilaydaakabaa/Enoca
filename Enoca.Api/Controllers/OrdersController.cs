using Enoca.Application.DTOs;
using Enoca.Application.Interfaces;
using Enoca.Application.Services;
using Enoca.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Enoca.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IShippingCostCalculator _calculator;

    public OrdersController(IUnitOfWork unitOfWork, IShippingCostCalculator calculator)
    {
        _unitOfWork = unitOfWork;
        _calculator = calculator;
    }

   
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _unitOfWork.Orders.GetAllAsync();
        var carriers = await _unitOfWork.Carriers.GetAllAsync();

        var result = orders
            .OrderByDescending(x => x.OrderDate)
            .Select(x => new OrderListDto
            {
                OrderId = x.OrderId,
                CarrierId = x.CarrierId,
                CarrierName = carriers.FirstOrDefault(c => c.CarrierId == x.CarrierId)?.CarrierName ?? "Bilinmiyor",
                OrderDesi = x.OrderDesi,
                OrderDate = x.OrderDate,
                OrderCarrierCost = x.OrderCarrierCost
            })
            .ToList();

        return Ok(result);
    }

   
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
    {
        

        var result = await _calculator.CalculateAsync(dto.OrderDesi);

        var order = new Order
        {
            CarrierId = result.CarrierId,
            OrderDesi = dto.OrderDesi,
            OrderDate = dto.OrderDate,
            OrderCarrierCost = result.CalculatedCost
        };

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return Ok("Sipariş eklendi");
    }

    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);
        if (order is null)
            return NotFound($"{id} ID'li kayıt bulunamadı");

        _unitOfWork.Orders.Remove(order);
        await _unitOfWork.SaveChangesAsync();

        return Ok($"{id} ID'li kayıt silindi");
    }
}
