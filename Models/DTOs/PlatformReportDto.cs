using WebApiProject.Models.Enums;

namespace WebApiProject.Models.DTOs
{
    public class PlatformReportDto
    {
        public PlatformType Platform { get; set; }
        public decimal ToplamCiro { get; set; }
        public decimal ToplamNetKar { get; set; }
        public decimal KarMarjiYuzde { get; set; }
    }
}
