using DershaneTakipSistemi.Data; // DbContext i�in
using DershaneTakipSistemi.Models; // ViewModel i�in
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // async metotlar ve CountAsync, SumAsync i�in
using System.Diagnostics; // ErrorViewModel i�in
using System.Linq; // Where, Sum vb. i�in
using System.Threading.Tasks; // Task i�in
using System; // DateTime i�in

namespace DershaneTakipSistemi.Controllers
{
    // HomeController'� da yetkilendirebiliriz veya public b�rakabiliriz.
    // �imdilik public b�rakal�m ki giri� yapmadan da ana sayfa g�r�lebilsin.
    // [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // DbContext eklendi

        // Constructor g�ncellendi: ILogger yan�na ApplicationDbContext eklendi
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context; // Inject edilen DbContext atan�yor
        }

        // Index metodu async yap�ld� ve ViewModel kullanacak �ekilde g�ncellendi
        public async Task<IActionResult> Index()
        {
            // Ba�lang��ta veritaban� bo� olabilir, null kontrolleri ekleyelim
            if (_context.Ogrenciler == null || _context.Odemeler == null)
            {
                // Veritaban� setleri null ise bo� bir model veya hata d�nd�r
                // �imdilik bo� view d�nd�relim veya basit bir ViewModel olu�tural�m
                // return Problem("Veritaban� tablolar� bulunamad�.");
                return View(new DashboardViewModel()); // Bo� modelle View'� d�nd�r
            }


            // ViewModel nesnesini olu�tur
            var viewModel = new DashboardViewModel
            {
                // Toplam ��renci say�s�n� asenkron olarak al
                ToplamOgrenciSayisi = await _context.Ogrenciler.CountAsync(),

                // Aktif ��renci say�s�n� asenkron olarak al (AktifMi == true olanlar)
                AktifOgrenciSayisi = await _context.Ogrenciler.CountAsync(o => o.AktifMi),

                // Toplam �deme tutar�n� asenkron olarak al
                // E�er hi� �deme yoksa SumAsync hata verebilir, bu y�zden �nce kontrol edelim veya DefaultIfEmpty kullanal�m
                ToplamOdemeTutari = await _context.Odemeler.AnyAsync() ? await _context.Odemeler.SumAsync(p => p.Tutar) : 0,

                // Bug�nk� �deme say�s�n� asenkron olarak al
                BugunkuOdemeSayisi = await _context.Odemeler.CountAsync(p => p.OdemeTarihi.Date == DateTime.Today)
            };

            // Doldurulmu� ViewModel'i View'a g�nder
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}