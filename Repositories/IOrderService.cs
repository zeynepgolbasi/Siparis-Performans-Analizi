using WebApiProject.Models.DTOs;

public interface IOrderService
{
    Task CreateOrderAsync(Order order);
    Task<SummaryReportDto> GetSummaryReportAsync();
    Task<List<PlatformReportDto>> GetPlatformReportAsync();
    Task<List<LossReportDto>> GetLossReportAsync();
    Task<List<AnomalyReportDto>> GetAnomalyReportAsync();
    Task<List<TrendReportDto>> GetTrendReportAsync();
    Task<List<RiskReportDto>> GetRiskReportAsync();

}