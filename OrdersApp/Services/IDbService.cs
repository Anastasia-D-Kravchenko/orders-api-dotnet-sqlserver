using OrdersApp.DTOs;

namespace OrdersApp.Services;

public interface IDbService
{
    Task<OrderResponseDto> GetOrderByIdAsync(int id);
    Task FulfillOrderAsync(int id, FulfillOrderRequestDto dto);
}
