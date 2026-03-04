using WebApiProject.Models.DTOs;
using WebApiProject.Repositories;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task CreateOrderAsync(Order order)
    {
        await _orderRepository.CreateAsync(order);
    }
    public async Task<SummaryReportDto> GetSummaryReportAsync()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();

        var toplamSiparisSayisi = orders.Count;

        var toplamUrunAdedi = orders.Sum(o => o.Items.Sum(i => i.adet));

        var toplamCiro = orders.Sum(o =>
            o.Items.Sum(i => i.satisFiyati * i.adet)
        );

        var toplamNetKar = orders.Sum(o =>
            o.Items.Sum(i =>
                (i.satisFiyati - i.alisFiyati - i.komisyonOrani - i.kargoBedeli)
                * i.adet
            )
        );

        return new SummaryReportDto
        {
            ToplamSiparisSayisi = toplamSiparisSayisi,
            ToplamUrunAdedi = toplamUrunAdedi,
            ToplamCiro = toplamCiro,
            ToplamNetKar = toplamNetKar
        };
    }
}