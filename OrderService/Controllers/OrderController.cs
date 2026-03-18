using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using OrderService.Services;
using OrderService.ViewModels;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrderController(IOrdersService ordersService, IMapper mapper, ILogger<OrderController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderVM>>> GetAll()
        {
            var orders = await ordersService.GetAllOrdersAsync();
            return Ok(mapper.Map<IEnumerable<OrderVM>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderVM>> GetById(int id)
        {
            var order = await ordersService.GetOrderByIdAsync(id);

            if (order == null)
                return NotFound();

            return Ok(mapper.Map<OrderVM>(order));
        }

        [HttpPost]
        public async Task<ActionResult<OrderVM>> Create([FromBody] OrderVM orderVM)
        {
            var order = mapper.Map<Order>(orderVM);
            var created = await ordersService.CreateOrderAsync(order);
            var result = mapper.Map<OrderVM>(created);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderVM>> Update(int id, [FromBody] OrderVM orderVM)
        {
            var order = mapper.Map<Order>(orderVM);
            var updated = await ordersService.UpdateOrderAsync(id, order);

            if (updated == null)
                return NotFound();

            return Ok(mapper.Map<OrderVM>(updated));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await ordersService.DeleteOrderAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
