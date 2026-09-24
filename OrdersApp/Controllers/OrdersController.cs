using Microsoft.AspNetCore.Mvc;
using OrdersApp.DTOs;
using OrdersApp.Exceptions;
using OrdersApp.Services;

namespace OrdersApp.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IDbService _dbService;

    public OrdersController(IDbService dbService)
    {
        _dbService = dbService;
    }

    // GET api/orders/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        try
        {
            var result = await _dbService.GetOrderByIdAsync(id);
            return Ok(result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    // PUT api/orders/{id}/fulfill
    [HttpPut("{id}/fulfill")]
    public async Task<IActionResult> FulfillOrder(int id, FulfillOrderRequestDto dto)
    {
        try
        {
            await _dbService.FulfillOrderAsync(id, dto);
            return Ok();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
    }
}
