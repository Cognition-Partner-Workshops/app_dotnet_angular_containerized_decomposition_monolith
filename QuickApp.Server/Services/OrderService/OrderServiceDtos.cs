namespace QuickApp.Server.Services.OrderService;

public record OrderServiceDto(
    int Id,
    decimal Discount,
    string? Comments,
    string? CashierId,
    int CustomerId,
    DateTime CreatedDate,
    DateTime UpdatedDate,
    ICollection<OrderDetailServiceDto> OrderDetails
);

public record OrderDetailServiceDto(
    int Id,
    decimal UnitPrice,
    int Quantity,
    decimal Discount,
    int ProductId,
    int OrderId
);

public record CreateOrderServiceDto(
    decimal Discount,
    string? Comments,
    string? CashierId,
    int CustomerId,
    ICollection<CreateOrderDetailServiceDto> OrderDetails
);

public record CreateOrderDetailServiceDto(
    decimal UnitPrice,
    int Quantity,
    decimal Discount,
    int ProductId
);

public record UpdateOrderServiceDto(
    decimal Discount,
    string? Comments,
    string? CashierId,
    int CustomerId
);
