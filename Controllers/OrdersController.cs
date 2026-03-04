using Microsoft.AspNetCore.Mvc;
using WebApiProject.Models.DTOs;

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
    public async Task<IActionResult> CreateOrder(Order order)
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

    [HttpGet("/api/report/platform")]
    public async Task<IActionResult> GetPlatformReport()
    {
        var report = await _orderService.GetPlatformReportAsync();
        return Ok(report);
    }
    [HttpGet("/api/report/loss")]
    public async Task<IActionResult> GetLossReport()
    {
        var report = await _orderService.GetLossReportAsync();
        return Ok(report);
    }
    [HttpGet("/api/report/anomaly")]
    public async Task<IActionResult> GetAnomalyReport()
    {
        var report = await _orderService.GetAnomalyReportAsync();
        return Ok(report);
    }
    [HttpGet("/api/report/trend")]
    public async Task<IActionResult> GetTrendReport()
    {
        var report = await _orderService.GetTrendReportAsync();
        return Ok(report);
    }
    [HttpGet("/api/report/risk")]
    public async Task<IActionResult> GetRiskReport()
    {
        var report = await _orderService.GetRiskReportAsync();
        return Ok(report);
    }
}