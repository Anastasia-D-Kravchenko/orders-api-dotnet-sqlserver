using Microsoft.EntityFrameworkCore;
using OrdersApp.Data;
using OrdersApp.DTOs;
using OrdersApp.Exceptions;

namespace OrdersApp.Services;

public class DbService : IDbService
{
    private readonly AppDbContext _context;

    public DbService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OrderResponseDto> GetOrderByIdAsync(int id)
    {
        // 1. Check order exists → 404 if not
        var order = await _context.Orders
            .AsNoTracking()
            .Include(o => o.Client)
            .Include(o => o.Status)
            .Include(o => o.ProductOrders)
                .ThenInclude(po => po.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order is null)
            throw new NotFoundException($"Order with ID {id} was not found.");

        // 2. Project into response DTO
        return new OrderResponseDto
        {
            Id = order.Id,
            CreatedAt = order.CreatedAt,
            FulfilledAt = order.FulfilledAt,
            Status = order.Status.Name,
            Client = new ClientDto
            {
                FirstName = order.Client.FirstName,
                LastName = order.Client.LastName
            },
            Products = order.ProductOrders.Select(po => new ProductDto
            {
                Name = po.Product.Name,
                Price = po.Product.Price,
                Amount = po.Amount
            }).ToList()
        };
    }

    public async Task FulfillOrderAsync(int id, FulfillOrderRequestDto dto)
    {
        // 1. Check order exists → 404 if not
        var order = await _context.Orders
            .Include(o => o.ProductOrders)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order is null)
            throw new NotFoundException($"Order with ID {id} was not found.");

        // 2. Check status exists → 404 if not
        var status = await _context.Statuses
            .FirstOrDefaultAsync(s => s.Name == dto.StatusName);

        if (status is null)
            throw new NotFoundException($"Status '{dto.StatusName}' was not found.");

        // 3. Check order is not already completed → 409 if it is
        if (order.FulfilledAt is not null)
            throw new ConflictException($"Order with ID {id} is already completed.");

        // 4. Apply changes: set new status, set FulfilledAt, clear product orders
        order.StatusId = status.Id;
        order.FulfilledAt = DateTime.UtcNow;
        _context.ProductOrders.RemoveRange(order.ProductOrders);

        await _context.SaveChangesAsync();
    }
}
