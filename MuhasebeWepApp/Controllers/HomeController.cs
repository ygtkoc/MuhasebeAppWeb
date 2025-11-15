using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MuhasebeWepApp.Models;

namespace MuhasebeWepApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private static readonly IReadOnlyList<Bank> Banks = new List<Bank>
        {
            new("001", "Türkiye Cumhuriyet Merkez Bankasý A.Þ."),
            new("004", "Ýller Bankasý"),
            new("010", "Türkiye Cumhuriyeti Ziraat Bankasý A.Þ."),
            new("012", "Türkiye Halk Bankasý A.Þ."),
            new("013", "Denizbank"),
            new("014", "Türkiye Sýnai Kalkýnma Bankasý A.Þ."),
            new("015", "Türkiye Vakýflar Bankasý T.A.O."),
            new("016", "Türkiye Ýhracat Kredi Bankasý A.Þ. (Eximbank)"),
            new("017", "Türkiye Kalkýnma Bankasý A.Þ."),
            new("029", "Birleþik Fon Bankasý A.Þ. (Bayýndýrbank A.Þ.)"),
            new("032", "Türk Ekonomi Bankasý A.Þ."),
            new("034", "Aktif Yatýrým Bankasý A.Þ."),
            new("046", "Akbank T.A.Þ."),
            new("048", "HSBC Bank A.Þ."),
            new("058", "Sýnai Yatýrým Bankasý A.Þ."),
            new("059", "Þekerbank T.A.Þ."),
            new("062", "Türkiye Garanti Bankasý A.Þ."),
            new("064", "Türkiye Ýþ Bankasý A.Þ."),
            new("067", "Yapý ve Kredi Bankasý A.Þ."),
            new("071", "Fortis Bank (TEB)"),
            new("087", "Banca di Roma"),
            new("088", "The Royal Bank of Scotland PLC Merkezi Amsterdam Ýstanbul Merkez Þubesi"),
            new("091", "Arap Türk Bankasý A.Þ."),
            new("092", "Citibank N.A."),
            new("094", "Bank Mellat"),
            new("095", "BCCI"),
            new("096", "Turkish Bank A.Þ."),
            new("097", "Habib Bank Limited"),
            new("098", "JP Morgan Chase Bank Ýstanbul Türkiye Þubesi"),
            new("099", "Oyak Bank A.Þ. - ING BANK"),
            new("100", "Adabank A.Þ."),
            new("101", "Türk Sakura Bank A.Þ."),
            new("103", "Fiba Bank A.Þ."),
            new("104", "IMPEXBANK"),
            new("106", "PORTIGON A.G."),
            new("107", "BNP-Ak-Dresdner Bank A.Þ."),
            new("108", "Turkland Bank A.Þ."),
            new("109", "Tekstil Bankasý A.Þ."),
            new("110", "Credit Lyonnais"),
            new("111", "Finansbank A.Þ."),
            new("113", "Marbank"),
            new("115", "Deutsche Bank A.Þ."),
            new("116", "TAÝB Yatýrým Bank A.Þ."),
            new("117", "Turizm Yatýrým ve Ticaret Bank A.Þ."),
            new("118", "Kýbrýs Kredi Bankasý"),
            new("119", "Birleþik Yatýrým"),
            new("121", "Standard Chartered Yatýrým Bankasý Türk A.Þ."),
            new("122", "Societe Generale"),
            new("123", "HSBC Bank A.Þ."),
            new("124", "Alternatifbank A.Þ."),
            new("125", "Burganbank A.Þ."),
            new("127", "KentBank"),
            new("128", "Park Yatýrým Bankasý"),
            new("129", "Tat Yatýrým Bankasý A.Þ."),
            new("132", "IMKB Takas ve Saklama Bankasý A.Þ."),
            new("133", "ING BANK"),
            new("134", "Denizbank A.Þ."),
            new("135", "Anadolubank A.Þ."),
            new("136", "Okan Yatýrým Bankasý A. Þ"),
            new("137", "Rabobank Nederland Ýstanbul Merkez Þubesi"),
            new("138", "Diler Yatýrým Bankasý A.Þ."),
            new("139", "GSD Yatýrým Bankasý A.Þ."),
            new("140", "Credit Suisse First Boston Ýstanbul Þubesi"),
            new("141", "Nurol Yatýrým Bankasý A.Þ."),
            new("142", "Bank Pozitif Kredi ve Kalkýnma Bankasý A.Þ."),
            new("144", "Atlas Yatýrým Bankasý A.Þ."),
            new("145", "Morgan Guarenty Trusy Company"),
            new("146", "OdeaBank A.Þ."),
            new("147", "Bank of Tokyo -Mitsubishi UFJ Turkey A.Þ."),
            new("148", "Intesa SanPaolo SPA Ýtalya-Ýstanbul Merkez Þubesi"),
            new("203", "Al Baraka Türk Katýlým Bankasý A.Þ."),
            new("204", "Family Finans Kurumu"),
            new("205", "Kuveyt Türk Katýlým Bankasý A.Þ."),
            new("206", "Türkiye Finans Katýlým Bankasý A.Þ."),
            new("208", "Asya Katýlým Bankasý A.Þ."),
            new("210", "Vakýf Katýlým Bankasý A.Þ."),
            new("223", "Al Baraka Türk Katýlým Bankasý A.Þ.")
        };

        public IActionResult Index()
        {
            var model = new BankSelectionViewModel
            {
                Banks = Banks
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadStatement(IFormFile? statementFile)
        {
            if (statementFile == null || statementFile.Length == 0)
            {
                return BadRequest("Geçerli bir dosya seçiniz.");
            }

            if (!Path.GetExtension(statementFile.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Sadece .csv uzantılı dosyalar desteklenmektedir.");
            }

            var entries = new List<BankStatementEntry>();
            var turkishCulture = CultureInfo.GetCultureInfo("tr-TR");

            using var stream = statementFile.OpenReadStream();
            using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);

            var isHeader = true;
            while (!reader.EndOfStream)
            {
                var rawLine = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(rawLine))
                {
                    continue;
                }

                if (isHeader)
                {
                    isHeader = false;
                    continue;
                }

                var segments = SplitCsvLine(rawLine);
                if (segments.Length < 3)
                {
                    continue;
                }

                if (!DateTime.TryParse(segments[0], turkishCulture, DateTimeStyles.None, out var documentDate))
                {
                    continue;
                }

                var description = segments[1];

                decimal.TryParse(segments[2], NumberStyles.Any, turkishCulture, out var amount);
                var entry = new BankStatementEntry
                {
                    DocumentDate = documentDate,
                    Description = description,
                    Debt = amount > 0 ? amount : 0,
                    Credit = amount < 0 ? Math.Abs(amount) : 0
                };

                entries.Add(entry);
            }

            return PartialView("_StatementTable", entries);
        }

        private static string[] SplitCsvLine(string line)
        {
            var separators = new[] { ';', ',', '\t' };
            foreach (var separator in separators)
            {
                var parts = line.Split(separator);
                if (parts.Length >= 3)
                {
                    return parts.Select(p => p.Trim('"')).ToArray();
                }
            }

            return new[] { line };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
