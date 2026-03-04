using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApiProject.Repositories;
using WebApiProject.Settings;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<Order> _orderCollection;

    public OrderRepository(IOptions<MongoDbSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(settings.Value.DatabaseName);

        _orderCollection = database.GetCollection<Order>(
            settings.Value.OrderCollectionName);
    }

    public async Task CreateAsync(Order order)
    {
        await _orderCollection.InsertOneAsync(order);
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _orderCollection.Find(_ => true).ToListAsync();
    }
}