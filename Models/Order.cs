using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WebApiProject.Models;
using WebApiProject.Models.Enums;

public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault] // Id JSON'da yoksa Mongo otomatik üretir
    public string Id { get; set; }

    [BsonRepresentation(BsonType.String)]
    public PlatformType Platform { get; set; }

    public DateTime OrderDate { get; set; }

    public List<OrderItem> Items { get; set; }
}