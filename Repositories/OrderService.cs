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
}