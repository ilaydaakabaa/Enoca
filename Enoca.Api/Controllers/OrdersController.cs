using Enoca.Application.DTOs;
using Enoca.Application.Services;
using Enoca.Domain.Entities;
using Enoca.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Enoca.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IShippingCostCalculator _calculator;

        public OrdersController(AppDbContext context, IShippingCostCalculator calculator)
        {
            _context = context;
            _calculator = calculator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync();

            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
        {
            if (dto is null)
                return BadRequest("Geçersiz istek.");

            if (dto.OrderDesi <= 0)
                return BadRequest("OrderDesi 0'dan büyük olmalıdır.");

            if (dto.OrderDate == default)
                return BadRequest("OrderDate zorunludur.");

            var result = await _calculator.CalculateAsync(dto.OrderDesi);

            var order = new Order
            {
                CarrierId = result.CarrierId,
                OrderDesi = dto.OrderDesi,
                OrderDate = dto.OrderDate,
                OrderCarrierCost = result.CalculatedCost
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return Ok("Sipariş eklendi");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order is null)
                return NotFound($"{id} ID'li kayıt bulunamadı");

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok($"{id} ID'li kayıt silindi");
        }
    }
}
