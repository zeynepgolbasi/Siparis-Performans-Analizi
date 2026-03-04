using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApiProject.Models.DTOs
{
    public class TrendReportDto
    {
        public DateTime Tarih { get; set; }
        public decimal GunlukCiro { get; set; }
        public decimal GunlukNetKar { get; set; }
        public decimal OncekiGuneGoreDegisimYuzde { get; set; }
    }
}