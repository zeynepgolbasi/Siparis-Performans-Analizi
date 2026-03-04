using WebApiProject.Models.Enums;

namespace WebApiProject.Models.DTOs
{
    public class RiskReportDto
    {
        public string Urun { get; set; }
        public decimal RiskSkoru { get; set; }
        public string RiskSeviyesi { get; set; }
    }
}