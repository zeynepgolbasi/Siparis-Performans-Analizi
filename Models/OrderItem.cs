using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApiProject.Models
{
    public class OrderItem
    {
        public string urunAdi { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal alisFiyati { get; set; }      // alışFiyat
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal satisFiyati { get; set; }      // satışFiyat
        public decimal komisyonOrani { get; set; } // komisyonOrani
        public decimal kargoBedeli { get; set; }   // kargoBedeli
        public int adet { get; set; }           // adet
    }
}
