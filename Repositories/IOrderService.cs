using WebApiProject.Models.DTOs;

namespace WebApiProject.Repositories
{
    public interface IOrderService
    {
        Task CreateOrderAsync(Order order);
        Task<SummaryReportDto> GetSummaryReportAsync();
    }
}
