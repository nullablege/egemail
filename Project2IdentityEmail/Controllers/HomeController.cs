using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project2IdentityEmail.Context;
using Project2IdentityEmail.Dtos;
using Project2IdentityEmail.Entities;
using Project2IdentityEmail.Services;
using System.Threading.Tasks;

namespace Project2IdentityEmail.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGeminiService _geminiService;

        public HomeController(EmailContext context, UserManager<AppUser> userManager, IGeminiService geminiService)
        {
            _userManager = userManager;
            _context = context;
            _geminiService = geminiService;
        }

        public async Task<IActionResult> Index(int page = 1, string search = null)
        {
            const int pageSize = 50;

            var user = await _userManager.GetUserAsync(User);
            ViewData["ActivePage"] = "Inbox";
            ViewData["CurrentSearch"] = search;

            var baseQuery = _context.EpostaKutulari
                .Include(x => x.Mesaj)
                .ThenInclude(m => m.Gonderen)
                .Where(x => x.SahibiId == user.Id 
                    && x.klasorTipi == Enums.klasorTipi.GelenKutusu
                    && !x.SilindiMi);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                baseQuery = baseQuery.Where(x =>
                    x.Mesaj.Konu.ToLower().Contains(search) ||
                    x.Mesaj.Icerik.ToLower().Contains(search) ||
                    x.Mesaj.Gonderen.Email.ToLower().Contains(search) ||
                    x.Mesaj.Gonderen.Name.ToLower().Contains(search) ||
                    x.Mesaj.Gonderen.Surname.ToLower().Contains(search)
                );
            }

            var toplamMesaj = await baseQuery.CountAsync();

            var totalPages = (int)Math.Ceiling(toplamMesaj / (double)pageSize);
            if (totalPages == 0) totalPages = 1;
            page = Math.Clamp(page, 1, totalPages);

            var mailler = await baseQuery
                .OrderByDescending(x => x.Mesaj.GonderimTarihi)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new MailDto
                {
                    MailId = x.EpostaKutusuId.ToString(),
                    MailBaslik = x.Mesaj.Konu,
                    MailIcerik = x.Mesaj.Icerik,
                    MailSaat = x.Mesaj.GonderimTarihi,
                    OkunduMu = x.OkunduMu,
                    YildizliMi = x.YildizliMi,
                    GonderenAdi = x.Mesaj.Gonderen.Name + " " + x.Mesaj.Gonderen.Surname,
                    GonderenEmail = x.Mesaj.Gonderen.Email
                })
                .ToListAsync();

            var dto = new IndexDto
            {
                ToplamMesaj = toplamMesaj,
                SayfaSayisi = page,    
                Mailler = mailler
            };

            return View(dto);
        }

        public async Task<IActionResult> EmailRead(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            var epostaKutusu = await _context.EpostaKutulari
                .Include(x => x.Mesaj)
                .ThenInclude(m => m.Gonderen)
                .FirstOrDefaultAsync(x => x.EpostaKutusuId.ToString() == id 
                    && x.SahibiId == user.Id);

            if (epostaKutusu == null)
            {
                return NotFound();
            }

            if (!epostaKutusu.OkunduMu)
            {
                epostaKutusu.OkunduMu = true;
                await _context.SaveChangesAsync();
            }

            return View(epostaKutusu);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStar(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var epostaKutusu = await _context.EpostaKutulari
                .FirstOrDefaultAsync(x => x.EpostaKutusuId == id && x.SahibiId == user.Id);

            if (epostaKutusu == null)
            {
                return Json(new { success = false, message = "E-posta bulunamadı" });
            }

            epostaKutusu.YildizliMi = !epostaKutusu.YildizliMi;
            await _context.SaveChangesAsync();

            return Json(new { success = true, yildizliMi = epostaKutusu.YildizliMi });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmails([FromBody] List<int> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return Json(new { success = false, message = "Silinecek e-posta seçilmedi" });
            }

            var user = await _userManager.GetUserAsync(User);

            var mailsToDelete = await _context.EpostaKutulari
                .Where(x => ids.Contains(x.EpostaKutusuId) && x.SahibiId == user.Id)
                .ToListAsync();

            foreach (var mail in mailsToDelete)
            {
                mail.SilindiMi = true;
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, deletedCount = mailsToDelete.Count });
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var epostaKutusu = await _context.EpostaKutulari
                .FirstOrDefaultAsync(x => x.EpostaKutusuId == id && x.SahibiId == user.Id);

            if (epostaKutusu == null)
            {
                return Json(new { success = false, message = "E-posta bulunamadı" });
            }

            epostaKutusu.OkunduMu = true;
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        public IActionResult Refresh()
        {
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Kategori(int id, int page = 1)
        {
            const int pageSize = 50;

            var user = await _userManager.GetUserAsync(User);
            
            var kategori = await _context.Kategoriler.FindAsync(id);
            if (kategori == null)
            {
                return NotFound();
            }

            ViewData["ActivePage"] = "Kategori";
            ViewData["AktifKategoriId"] = id;
            ViewData["KategoriAdi"] = kategori.Ad;

            var baseQuery = _context.EpostaKutulari
                .Include(x => x.Mesaj)
                .ThenInclude(m => m.Gonderen)
                .Where(x => x.SahibiId == user.Id 
                    && x.KategoriId == id
                    && x.klasorTipi == Enums.klasorTipi.GelenKutusu
                    && !x.SilindiMi);

            var toplamMesaj = await baseQuery.CountAsync();

            var totalPages = (int)Math.Ceiling(toplamMesaj / (double)pageSize);
            if (totalPages == 0) totalPages = 1;
            page = Math.Clamp(page, 1, totalPages);

            var mailler = await baseQuery
                .OrderByDescending(x => x.Mesaj.GonderimTarihi)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new MailDto
                {
                    MailId = x.EpostaKutusuId.ToString(),
                    MailBaslik = x.Mesaj.Konu,
                    MailIcerik = x.Mesaj.Icerik,
                    MailSaat = x.Mesaj.GonderimTarihi,
                    OkunduMu = x.OkunduMu,
                    YildizliMi = x.YildizliMi,
                    GonderenAdi = x.Mesaj.Gonderen.Name + " " + x.Mesaj.Gonderen.Surname,
                    GonderenEmail = x.Mesaj.Gonderen.Email
                })
                .ToListAsync();

            var dto = new IndexDto
            {
                ToplamMesaj = toplamMesaj,
                SayfaSayisi = page,    
                Mailler = mailler
            };

            return View("Index", dto);
        }

        [HttpPost]
        public async Task<IActionResult> SetCategory(int mailId, int? kategoriId)
        {
            var user = await _userManager.GetUserAsync(User);

            var epostaKutusu = await _context.EpostaKutulari
                .FirstOrDefaultAsync(x => x.EpostaKutusuId == mailId && x.SahibiId == user.Id);

            if (epostaKutusu == null)
            {
                return Json(new { success = false, message = "E-posta bulunamadı" });
            }

            Kategori kategori = null;
            if (kategoriId.HasValue)
            {
                kategori = await _context.Kategoriler.FindAsync(kategoriId.Value);
                if (kategori == null)
                {
                    return Json(new { success = false, message = "Kategori bulunamadı" });
                }
            }

            epostaKutusu.KategoriId = kategoriId;
            await _context.SaveChangesAsync();

            return Json(new { 
                success = true, 
                kategoriId = kategoriId,
                kategoriAdi = kategori?.Ad ?? "Kategorisiz"
            });
        }

        public async Task<IActionResult> Statistics()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewData["ActivePage"] = "Statistics";

            var userMails = _context.EpostaKutulari
                .Include(x => x.Mesaj)
                .Include(x => x.Kategori)
                .Where(x => x.SahibiId == user.Id && x.klasorTipi == Enums.klasorTipi.GelenKutusu);

            var dto = new StatisticsDto
            {
                ToplamMail = await userMails.Where(x => !x.SilindiMi).CountAsync(),
                OkunmusMail = await userMails.Where(x => x.OkunduMu && !x.SilindiMi).CountAsync(),
                OkunmamisMail = await userMails.Where(x => !x.OkunduMu && !x.SilindiMi).CountAsync(),
                YildizliMail = await userMails.Where(x => x.YildizliMi && !x.SilindiMi).CountAsync(),
                SilinenMail = await userMails.Where(x => x.SilindiMi).CountAsync()
            };

            var kategoriGruplari = await userMails
                .Where(x => !x.SilindiMi && x.KategoriId != null)
                .GroupBy(x => x.Kategori.Ad)
                .Select(g => new { Kategori = g.Key, Sayi = g.Count() })
                .ToListAsync();

            foreach (var g in kategoriGruplari)
            {
                dto.KategoriDagilimi[g.Kategori ?? "Diğer"] = g.Sayi;
            }

            var son7Gun = DateTime.Now.Date.AddDays(-6);
            var gunlukGruplar = await userMails
                .Where(x => !x.SilindiMi && x.Mesaj.GonderimTarihi >= son7Gun)
                .GroupBy(x => x.Mesaj.GonderimTarihi.Date)
                .Select(g => new { Tarih = g.Key, Sayi = g.Count() })
                .ToListAsync();

            for (int i = 0; i < 7; i++)
            {
                var tarih = son7Gun.AddDays(i);
                var gun = tarih.ToString("dd MMM");
                dto.GunlukMailSayisi[gun] = gunlukGruplar.FirstOrDefault(g => g.Tarih == tarih)?.Sayi ?? 0;
            }

            var son6Ay = DateTime.Now.Date.AddMonths(-5);
            var aylikGruplar = await userMails
                .Where(x => !x.SilindiMi && x.Mesaj.GonderimTarihi >= son6Ay)
                .GroupBy(x => new { x.Mesaj.GonderimTarihi.Year, x.Mesaj.GonderimTarihi.Month })
                .Select(g => new { Yil = g.Key.Year, Ay = g.Key.Month, Sayi = g.Count() })
                .ToListAsync();

            for (int i = 0; i < 6; i++)
            {
                var tarih = DateTime.Now.AddMonths(-5 + i);
                var ayAdi = tarih.ToString("MMM yyyy");
                dto.AylikMailSayisi[ayAdi] = aylikGruplar.FirstOrDefault(g => g.Yil == tarih.Year && g.Ay == tarih.Month)?.Sayi ?? 0;
            }

            return View(dto);
        }

        public async Task<IActionResult> Durum(string tip, int page = 1)
        {
            const int pageSize = 50;
            var user = await _userManager.GetUserAsync(User);
            
            string pageTitle = "";
            IQueryable<EpostaKutusu> baseQuery;

            switch (tip?.ToLower())
            {
                case "yildizli":
                    ViewData["ActivePage"] = "Yildizli";
                    pageTitle = "Yıldızlı E-postalar";
                    baseQuery = _context.EpostaKutulari
                        .Include(x => x.Mesaj).ThenInclude(m => m.Gonderen)
                        .Where(x => x.SahibiId == user.Id && x.YildizliMi && !x.SilindiMi);
                    break;

                case "gonderilen":
                    ViewData["ActivePage"] = "Gonderilen";
                    pageTitle = "Gönderilen E-postalar";
                    baseQuery = _context.EpostaKutulari
                        .Include(x => x.Mesaj).ThenInclude(m => m.Gonderen)
                        .Where(x => x.SahibiId == user.Id && x.klasorTipi == Enums.klasorTipi.GidenKutusu && !x.SilindiMi);
                    break;

                case "cop":
                    ViewData["ActivePage"] = "Cop";
                    pageTitle = "Çöp Kutusu";
                    baseQuery = _context.EpostaKutulari
                        .Include(x => x.Mesaj).ThenInclude(m => m.Gonderen)
                        .Where(x => x.SahibiId == user.Id && x.SilindiMi);
                    break;

                default:
                    return RedirectToAction("Index");
            }

            ViewData["PageTitle"] = pageTitle;
            ViewData["DurumTip"] = tip;

            var toplamMesaj = await baseQuery.CountAsync();
            var mailler = await baseQuery
                .OrderByDescending(x => x.Mesaj.GonderimTarihi)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new MailDto
                {
                    MailId = x.EpostaKutusuId.ToString(),
                    MailBaslik = x.Mesaj.Konu,
                    MailIcerik = x.Mesaj.Icerik,
                    MailSaat = x.Mesaj.GonderimTarihi,
                    OkunduMu = x.OkunduMu,
                    YildizliMi = x.YildizliMi,
                    GonderenAdi = x.Mesaj.Gonderen.Name + " " + x.Mesaj.Gonderen.Surname,
                    GonderenEmail = x.Mesaj.Gonderen.Email
                })
                .ToListAsync();

            var dto = new IndexDto
            {
                ToplamMesaj = toplamMesaj,
                SayfaSayisi = page,
                Mailler = mailler
            };

            return View("Index", dto);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailDto model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Geçersiz form verisi" });
            }

            var gonderen = await _userManager.GetUserAsync(User);
            if (gonderen == null)
            {
                return Json(new { success = false, message = "Oturum hatası" });
            }

            var alici = await _userManager.FindByEmailAsync(model.AliciEmail);
            if (alici == null)
            {
                return Json(new { success = false, message = "Alıcı bulunamadı. Lütfen geçerli bir e-posta adresi giriniz." });
            }

            if (gonderen.Id == alici.Id)
            {
                return Json(new { success = false, message = "Kendinize e-posta gönderemezsiniz." });
            }

            var mesaj = new Mesaj
            {
                Konu = model.Konu,
                Icerik = model.Icerik,
                GonderimTarihi = DateTime.Now,
                GonderenId = gonderen.Id,
                OkunduMu = false
            };

            _context.Mesajlar.Add(mesaj);
            await _context.SaveChangesAsync();

            int? kategoriId = null;
            try
            {
                kategoriId = await _geminiService.KategorizasyonYapAsync(
                    gonderen.Email ?? "",
                    alici.Email ?? "",
                    model.Konu,
                    model.Icerik
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gemini kategorizasyon hatası: {ex.Message}");
            }

            var gonderenKutusu = new EpostaKutusu
            {
                SahibiId = gonderen.Id,
                MesajId = mesaj.MesajId,
                OkunduMu = true, 
                YildizliMi = false,
                SilindiMi = false,
                klasorTipi = Enums.klasorTipi.GidenKutusu
            };

            var aliciKutusu = new EpostaKutusu
            {
                SahibiId = alici.Id,
                MesajId = mesaj.MesajId,
                OkunduMu = false,
                YildizliMi = false,
                SilindiMi = false,
                klasorTipi = Enums.klasorTipi.GelenKutusu,
                KategoriId = kategoriId
            };

            _context.EpostaKutulari.Add(gonderenKutusu);
            _context.EpostaKutulari.Add(aliciKutusu);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "E-posta başarıyla gönderildi!" });
        }
    }
}
