namespace WebApiProject.Repositories
{
    public interface IOrderService
    {
        Task CreateOrderAsync(Order order);

    }
}
