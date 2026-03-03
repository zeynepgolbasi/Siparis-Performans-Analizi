namespace WebApiProject.Models
{
    public class OrderItem
    {
        public string urunAdi { get; set; }
        public decimal alisFiyati { get; set; }      // alışFiyat
        public decimal satisFiyati { get; set; }      // satışFiyat
        public decimal komisyonOrani { get; set; } // komisyonOrani
        public decimal kargoBedeli { get; set; }   // kargoBedeli
        public int adet { get; set; }           // adet
    }
}
