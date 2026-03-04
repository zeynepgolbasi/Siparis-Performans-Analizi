namespace WebApiProject.Models.DTOs
{
    public class AnomalyReportDto
    {
        public string Urun { get; set; }
        public decimal OrtalamaSatisFiyat { get; set; }
        public decimal MevcutSatisFiyat { get; set; }
        public decimal SapmaYuzde { get; set; }
    }
}
