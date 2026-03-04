using WebApiProject.Models.DTOs;
using WebApiProject.Repositories;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task CreateOrderAsync(Order order)
    {
        await _orderRepository.CreateAsync(order);
    }

    // GENEL ÖZET
    public async Task<SummaryReportDto> GetSummaryReportAsync()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();

        var toplamSiparisSayisi = orders.Count;

        var toplamUrunAdedi = orders
            .Where(o => o.Items != null)
            .Sum(o => o.Items.Sum(i => i.adet));

        var toplamCiro = orders
            .Where(o => o.Items != null)
            .Sum(o => o.Items.Sum(i => i.satisFiyati * i.adet));

        var toplamNetKar = orders
            .Where(o => o.Items != null)
            .Sum(o => o.Items.Sum(i =>
                (i.satisFiyati - i.alisFiyati - i.komisyonOrani - i.kargoBedeli)
                * i.adet));

        return new SummaryReportDto
        {
            ToplamSiparisSayisi = toplamSiparisSayisi,
            ToplamUrunAdedi = toplamUrunAdedi,
            ToplamCiro = toplamCiro,
            ToplamNetKar = toplamNetKar
        };
    }

    // PLATFORM RAPORU
    public async Task<List<PlatformReportDto>> GetPlatformReportAsync()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();

        var result = orders
            .Where(o => o.Items != null)
            .GroupBy(o => o.Platform)
            .Select(group =>
            {
                var toplamCiro = group.Sum(o =>
                    o.Items.Sum(i => i.satisFiyati * i.adet));

                var toplamNetKar = group.Sum(o =>
                    o.Items.Sum(i =>
                        (i.satisFiyati - i.alisFiyati - i.komisyonOrani - i.kargoBedeli)
                        * i.adet));

                var karMarji = toplamCiro == 0
                    ? 0
                    : (toplamNetKar / toplamCiro) * 100;

                return new PlatformReportDto
                {
                    Platform = group.Key,
                    ToplamCiro = toplamCiro,
                    ToplamNetKar = toplamNetKar,
                    KarMarjiYuzde = Math.Round(karMarji, 2)
                };
            })
            .ToList();

        return result;
    }

    // Zararda Olan Ürünler
    public async Task<List<LossReportDto>> GetLossReportAsync()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();

        var result = orders
            .Where(o => o.Items != null)
            .SelectMany(o => o.Items) // tüm ürünleri tek liste yap
            .GroupBy(i => i.urunAdi)
            .Select(group =>
            {
                var toplamZarar = group.Sum(i =>
                {
                    var net = (i.satisFiyati - i.alisFiyati - i.komisyonOrani - i.kargoBedeli)
                              * i.adet;

                    return net < 0 ? net : 0;
                });

                return new LossReportDto
                {
                    Urun = group.Key,
                    ToplamZarar = Math.Abs(toplamZarar)
                };
            })
            .Where(x => x.ToplamZarar > 0) // sadece zararda olanlar
            .ToList();

        return result;
    }

    //Anormal Fiyat Tespiti
    public async Task<List<AnomalyReportDto>> GetAnomalyReportAsync()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();

        var result = orders
            .Where(o => o.Items != null)
            .SelectMany(o => o.Items)
            .GroupBy(i => i.urunAdi)
            .Select(group =>
            {
                var ortalama = group.Average(i => i.satisFiyati);
                var mevcut = group.Last().satisFiyati; // en son eklenen satış fiyatı

                var sapma = ortalama == 0 ? 0 : ((mevcut - ortalama) / ortalama) * 100;

                return new AnomalyReportDto
                {
                    Urun = group.Key,
                    OrtalamaSatisFiyat = Math.Round(ortalama, 2),
                    MevcutSatisFiyat = mevcut,
                    SapmaYuzde = Math.Round(sapma, 2)
                };
            })
            .Where(x => Math.Abs(x.SapmaYuzde) >= 20) // %20’lik anomaliler
            .ToList();

        return result;
    }
    //Trend Ürün Tespiti
    public async Task<List<TrendReportDto>> GetTrendReportAsync()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();

        var dailyGroups = orders
            .Where(o => o.Items != null)
            .GroupBy(o => o.OrderDate.Date)
            .OrderBy(g => g.Key)
            .Select(g =>
            {
                var gunlukCiro = g.Sum(o => o.Items.Sum(i => i.satisFiyati * i.adet));
                var gunlukNetKar = g.Sum(o => o.Items.Sum(i =>
                    (i.satisFiyati - i.alisFiyati - i.komisyonOrani - i.kargoBedeli) * i.adet));

                return new TrendReportDto
                {
                    Tarih = g.Key,
                    GunlukCiro = gunlukCiro,
                    GunlukNetKar = gunlukNetKar
                };
            })
            .ToList();

        // Önceki güne göre değişim
        for (int i = 1; i < dailyGroups.Count; i++)
        {
            var onceki = dailyGroups[i - 1].GunlukCiro;
            dailyGroups[i].OncekiGuneGoreDegisimYuzde = onceki == 0
                ? 0
                : Math.Round(((dailyGroups[i].GunlukCiro - onceki) / onceki) * 100, 2);
        }

        // İlk gün değişim %0
        if (dailyGroups.Count > 0)
            dailyGroups[0].OncekiGuneGoreDegisimYuzde = 0;

        return dailyGroups;
    }

    //Risk Analizi
    public async Task<List<RiskReportDto>> GetRiskReportAsync()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();

        var result = orders
            .Where(o => o.Items != null)
            .SelectMany(o => o.Items)
            .GroupBy(i => i.urunAdi)
            .Select(group =>
            {
                var toplamAdet = group.Sum(i => i.adet);
                var toplamSatis = group.Sum(i => i.satisFiyati * i.adet);
                var toplamKar = group.Sum(i => (i.satisFiyati - i.alisFiyati - i.komisyonOrani - i.kargoBedeli) * i.adet);

                var ortalamaKarMarji = toplamSatis == 0 ? 0 : (decimal)toplamKar / toplamSatis * 100;

                // Risk skoru formülü: düşük kar marjı ve az satış → risk artar
                var riskSkoru = 100 - ortalamaKarMarji;
                riskSkoru = Math.Clamp(riskSkoru, 0, 100);

                string riskSeviyesi;
                if (riskSkoru <= 40) riskSeviyesi = "Dusuk";
                else if (riskSkoru <= 70) riskSeviyesi = "Orta";
                else riskSeviyesi = "Yuksek";

                return new RiskReportDto
                {
                    Urun = group.Key,
                    RiskSkoru = Math.Round(riskSkoru, 2),
                    RiskSeviyesi = riskSeviyesi
                };
            })
            .ToList();

        return result;
    }
}