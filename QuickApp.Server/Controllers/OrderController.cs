// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QuickApp.Core.Models.Shop;
using QuickApp.Core.Services.Shop;
using QuickApp.Server.ViewModels.Shop;

namespace QuickApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseApiController
    {
        private readonly IOrdersService _ordersService;

        public OrderController(IOrdersService ordersService, ILogger<BaseApiController> logger, IMapper mapper)
            : base(logger, mapper)
        {
            _ordersService = ordersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var orders = await _ordersService.GetAllOrdersAsync(cancellationToken);
            return Ok(orders.Select(MapToVM));
        }

        [HttpGet("customer/{customerId:int}")]
        public async Task<IActionResult> GetByCustomer(int customerId, CancellationToken cancellationToken)
        {
            var orders = await _ordersService.GetOrdersByCustomerAsync(customerId, cancellationToken);
            return Ok(orders.Select(MapToVM));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var order = await _ordersService.GetOrderByIdAsync(id, cancellationToken);
            if (order is null)
                return NotFound();
            return Ok(MapToVM(order));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderVM vm, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                CustomerId = vm.CustomerId,
                CashierId = vm.CashierId,
                Discount = vm.Discount,
                Comments = vm.Comments,
                Customer = new Customer { Name = string.Empty, Email = string.Empty }
            };

            foreach (var d in vm.OrderDetails)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    ProductId = d.ProductId,
                    UnitPrice = d.UnitPrice,
                    Quantity = d.Quantity,
                    Discount = d.Discount,
                    Order = order,
                    Product = new Product
                    {
                        Name = d.ProductName ?? string.Empty,
                        ProductCategory = new ProductCategory { Name = "Unknown" }
                    }
                });
            }

            var created = await _ordersService.CreateOrderAsync(order, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToVM(created));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderVM vm, CancellationToken cancellationToken)
        {
            var existing = await _ordersService.GetOrderByIdAsync(id, cancellationToken);
            if (existing is null)
                return NotFound();

            var updated = await _ordersService.UpdateOrderAsync(id, vm.Discount, vm.Comments, cancellationToken);
            return Ok(MapToVM(updated));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var existing = await _ordersService.GetOrderByIdAsync(id, cancellationToken);
            if (existing is null)
                return NotFound();

            await _ordersService.DeleteOrderAsync(id, cancellationToken);
            return NoContent();
        }

        private static OrderVM MapToVM(Order o) => new()
        {
            Id = o.Id,
            CustomerId = o.CustomerId,
            CashierId = o.CashierId,
            Discount = o.Discount,
            Comments = o.Comments,
            CreatedDate = o.CreatedDate,
            UpdatedDate = o.UpdatedDate,
            OrderDetails = o.OrderDetails.Select(d => new OrderDetailVM
            {
                Id = d.Id,
                ProductId = d.ProductId,
                ProductName = d.Product?.Name,
                UnitPrice = d.UnitPrice,
                Quantity = d.Quantity,
                Discount = d.Discount
            }).ToList()
        };
    }
}
