using Microsoft.AspNetCore.Mvc;
using WebApiProject.Repositories;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        await _orderService.CreateOrderAsync(order);
        return Ok("Order created successfully");
    }

    [HttpGet("/api/report/summary")]
    public async Task<IActionResult> GetSummaryReport()
    {
        var report = await _orderService.GetSummaryReportAsync();
        return Ok(report);
    }
}