namespace WebApiProject.Repositories
{
    public interface IOrderRepository
    {
        Task CreateAsync(Order order);
        Task<List<Order>> GetAllOrdersAsync();
    }
}
