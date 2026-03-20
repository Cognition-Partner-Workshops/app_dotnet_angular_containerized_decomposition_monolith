using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopService.API.ViewModels;
using ShopService.Core.Services;

namespace ShopService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<OrderController> _logger;
        private readonly IOrdersService _ordersService;

        public OrderController(IMapper mapper, ILogger<OrderController> logger,
            IOrdersService ordersService)
        {
            _mapper = mapper;
            _logger = logger;
            _ordersService = ordersService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allOrders = await _ordersService.GetAllOrdersAsync();
            return Ok(_mapper.Map<IEnumerable<OrderVM>>(allOrders));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var order = await _ordersService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(_mapper.Map<OrderVM>(order));
        }

        [HttpGet("by-cashier/{cashierId}/exists")]
        [AllowAnonymous]
        public async Task<IActionResult> HasOrdersByCashier(string cashierId)
        {
            var exists = await _ordersService.HasOrdersByCashierAsync(cashierId);
            return Ok(exists);
        }
    }
}
