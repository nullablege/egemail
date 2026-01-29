using Microsoft.AspNetCore.Identity;
using Project2IdentityEmail.Context;
using Project2IdentityEmail.Entities;
using Project2IdentityEmail.Enums;

namespace Project2IdentityEmail
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var context = scope.ServiceProvider.GetRequiredService<EmailContext>();

            var users = new List<(string Email, string Password, string Name, string Surname, string ImageUrl, string About)>
            {
                ("ahmet.yilmaz@testmail.com", "Test123!", "Ahmet", "Yılmaz", "/vertical/assets/images/avatars/avatar-1.png", "Yazılım geliştirici, İstanbul."),
                ("elif.demir@testmail.com", "Test123!", "Elif", "Demir", "/vertical/assets/images/avatars/avatar-2.png", "Proje yöneticisi, Ankara."),
                ("mehmet.kaya@testmail.com", "Test123!", "Mehmet", "Kaya", "/vertical/assets/images/avatars/avatar-3.png", "UI/UX tasarımcı, İzmir.")
            };

            var createdUsers = new List<AppUser>();

            foreach (var (email, password, name, surname, imageUrl, about) in users)
            {
                var existingUser = await userManager.FindByEmailAsync(email);
                if (existingUser == null)
                {
                    var user = new AppUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,
                        Name = name,
                        Surname = surname,
                        ImageUrl = imageUrl,
                        About = about
                    };

                    var result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        createdUsers.Add(user);
                        Console.WriteLine($"✓ Kullanıcı oluşturuldu: {email}");
                    }
                    else
                    {
                        Console.WriteLine($"✗ Kullanıcı oluşturulamadı: {email} - {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    createdUsers.Add(existingUser);
                    Console.WriteLine($"○ Kullanıcı zaten mevcut: {email}");
                }
            }

            var kategoriler = new[] { "İş", "Kişisel", "Sosyal", "Promosyon" };
            foreach (var kategoriAd in kategoriler)
            {
                if (!context.Kategoriler!.Any(k => k.Ad == kategoriAd))
                {
                    context.Kategoriler!.Add(new Kategori { Ad = kategoriAd });
                }
            }
            await context.SaveChangesAsync();

            var katIs = context.Kategoriler!.First(k => k.Ad == "İş");
            var katKisisel = context.Kategoriler!.First(k => k.Ad == "Kişisel");
            var katSosyal = context.Kategoriler!.First(k => k.Ad == "Sosyal");

            if (createdUsers.Count < 3)
            {
                Console.WriteLine("Yeterli kullanıcı bulunamadı, seed işlemi durduruluyor.");
                return;
            }

            var ahmet = createdUsers[0];
            var elif = createdUsers[1];
            var mehmet = createdUsers[2];

            var mesajlar = new List<Mesaj>();

            var ahmetMesajlari = new[]
            {
                ("Proje Toplantısı Hakkında", "Merhaba, yarınki proje toplantısı için hazırlıklar tamamlandı. Toplantı saat 14:00'da gerçekleşecek. Katılımınızı bekliyorum.", -1),
                ("Haftalık Rapor", "Bu hafta tamamlanan işler ve önümüzdeki hafta için planlanan görevler ektedir. Lütfen inceleyip geri dönüş yapın.", -2),
                ("Yeni Özellik Talebi", "Müşteriden gelen talep doğrultusunda yeni bir özellik eklememiz gerekiyor. Detayları toplantıda görüşelim.", -3),
                ("Doğum Günü Kutlaması", "Doğum günün kutlu olsun! Nice mutlu yıllara. 🎂🎉", -5),
                ("Kod İnceleme Talebi", "Geliştirdiğim modülün kod incelemesini yapabilir misin? PR linki ekte.", -7),
                ("Bug Raporu", "Sistemde kritik bir hata tespit ettim. Hemen düzeltilmesi gerekiyor.", 0),
                ("Veritabanı Optimizasyonu", "Performans iyileştirmesi için veritabanı sorgularını optimize ettim.", -11),
                ("API Dokümantasyonu", "Yeni API endpoint'leri için dokümantasyonu tamamladım. Wiki'ye ekledim.", -12),
                ("Kahve Molası?", "Bugün öğleden sonra kahve içmeye ne dersin? Yeni açılan kafeyi deneyelim.", -13),
                ("Güvenlik Güncellemesi", "Kritik güvenlik yamalarını uyguladım. Lütfen test ortamında kontrol edin.", -14)
            };

            foreach (var (konu, icerik, gunOnce) in ahmetMesajlari)
            {
                var mesaj = new Mesaj
                {
                    Konu = konu,
                    Icerik = icerik,
                    GonderimTarihi = DateTime.Now.AddDays(gunOnce),
                    OkunduMu = false,
                    GonderenId = ahmet.Id
                };
                context.Mesajlar!.Add(mesaj);
                mesajlar.Add(mesaj);
            }

            var elifMesajlari = new[]
            {
                ("Sprint Planlama", "Yeni sprint için görev dağılımını yaptım. Lütfen atanan görevleri kontrol edin ve onaylayın.", -1),
                ("Tatil Planları", "Bu yaz için tatil planları yapmayı düşünüyor musun? Belki birlikte bir yer ayarlayabiliriz.", -4),
                ("Sunum Hazırlığı", "Müşteri sunumu için hazırlıklar devam ediyor. Senin bölümünü eklememiz gerekiyor.", -6),
                ("Yeni Ekip Üyesi", "Ekibimize yeni bir geliştirici katılıyor. Yarın tanışma toplantısı yapacağız.", -8),
                ("Eğitim Fırsatı", "Şirket dışından bir eğitim fırsatı var. İlgilenirsen detayları paylaşabilirim.", -10),
                ("Bütçe Raporu", "Q1 bütçe raporunu hazırladım. Yönetim toplantısından önce incelemenizi rica ederim.", 0),
                ("Müşteri Geri Bildirimi", "Son demoda müşteriden çok olumlu geri bildirimler aldık! Tebrikler ekip!", -11),
                ("Deadline Hatırlatması", "Proje teslim tarihine 1 hafta kaldı. Lütfen tüm görevleri tamamlayın.", -12),
                ("Yemek Daveti", "Cumartesi akşamı evde yemek yapıyorum. Gelebilir misin?", -15),
                ("Konferans Katılımı", "Gelecek ay düzenlenecek tech konferansına katılmak ister misin? Biletler şirketten.", -16)
            };

            foreach (var (konu, icerik, gunOnce) in elifMesajlari)
            {
                var mesaj = new Mesaj
                {
                    Konu = konu,
                    Icerik = icerik,
                    GonderimTarihi = DateTime.Now.AddDays(gunOnce),
                    OkunduMu = false,
                    GonderenId = elif.Id
                };
                context.Mesajlar!.Add(mesaj);
                mesajlar.Add(mesaj);
            }

            var mehmetMesajlari = new[]
            {
                ("UI Tasarım Güncellemesi", "Yeni tasarımları Figma'ya yükledim. Lütfen inceleyip geri bildirim verin.", 0),
                ("Logo Revizyonu", "Müşterinin istediği logo değişikliklerini tamamladım. Onay için bekliyorum.", -2),
                ("Renk Paleti Önerisi", "Yeni proje için hazırladığım renk paleti önerilerini ekte bulabilirsiniz.", -4),
                ("Hafta Sonu Etkinliği", "Ekip olarak hafta sonu bir aktivite yapmayı düşünüyoruz. Katılır mısın?", -6),
                ("Mobil Uygulama Tasarımı", "Mobil uygulama için wireframe'leri hazırladım. İnceleme toplantısı ayarlayalım mı?", -9),
                ("İkon Seti", "Projeye özel ikon seti hazırladım. SVG formatında paylaşıyorum.", 0),
                ("Dark Mode Tasarımı", "Uygulamanın dark mode versiyonunu tamamladım. Çok şık oldu!", -10),
                ("Font Önerisi", "Yeni projeler için kullanabileceğimiz güzel fontlar buldum. Listeliyorum.", -13),
                ("Fotoğraf Gezisi", "Hafta sonu fotoğraf çekmeye gidelim mi? Güzel manzaralar var.", -14),
                ("Animasyon Desteği", "Landing page için mikro animasyonlar ekledim. Canlı demo linki ekte.", -17)
            };

            foreach (var (konu, icerik, gunOnce) in mehmetMesajlari)
            {
                var mesaj = new Mesaj
                {
                    Konu = konu,
                    Icerik = icerik,
                    GonderimTarihi = DateTime.Now.AddDays(gunOnce),
                    OkunduMu = false,
                    GonderenId = mehmet.Id
                };
                context.Mesajlar!.Add(mesaj);
                mesajlar.Add(mesaj);
            }

            await context.SaveChangesAsync();
            Console.WriteLine($"✓ {mesajlar.Count} mesaj oluşturuldu.");

            var random = new Random(42); 
            var kullanicilar = new[] { ahmet, elif, mehmet };

            foreach (var kullanici in kullanicilar)
            {
                var gelenMesajlar = mesajlar
                    .Where(m => m.GonderenId != kullanici.Id)
                    .OrderByDescending(m => m.GonderimTarihi)
                    .Take(10)
                    .ToList();

                foreach (var mesaj in gelenMesajlar)
                {
                    var kategori = random.Next(4) switch
                    {
                        0 => katIs,
                        1 => katKisisel,
                        2 => katSosyal,
                        _ => katIs
                    };

                    context.EpostaKutulari!.Add(new EpostaKutusu
                    {
                        SahibiId = kullanici.Id,
                        MesajId = mesaj.MesajId,
                        OkunduMu = random.Next(2) == 1,
                        YildizliMi = random.Next(4) == 0, 
                        SilindiMi = false,
                        klasorTipi = klasorTipi.GelenKutusu,
                        KategoriId = kategori.KategoriId
                    });
                }

                var gidenMesajlar = mesajlar
                    .Where(m => m.GonderenId == kullanici.Id)
                    .ToList();

                foreach (var mesaj in gidenMesajlar)
                {
                    context.EpostaKutulari!.Add(new EpostaKutusu
                    {
                        SahibiId = kullanici.Id,
                        MesajId = mesaj.MesajId,
                        OkunduMu = true,
                        YildizliMi = false,
                        SilindiMi = false,
                        klasorTipi = klasorTipi.GidenKutusu,
                        KategoriId = katIs.KategoriId
                    });
                }
            }

            await context.SaveChangesAsync();
            Console.WriteLine("✓ Posta kutuları oluşturuldu.");

            Console.WriteLine("\n=== SEED DATA TAMAMLANDI ===");
            Console.WriteLine("\nKullanıcı Bilgileri:");
            Console.WriteLine("────────────────────────────────────────");
            Console.WriteLine("1. Ahmet Yılmaz");
            Console.WriteLine("   E-posta: ahmet.yilmaz@testmail.com");
            Console.WriteLine("   Şifre:   Test123!");
            Console.WriteLine();
            Console.WriteLine("2. Elif Demir");
            Console.WriteLine("   E-posta: elif.demir@testmail.com");
            Console.WriteLine("   Şifre:   Test123!");
            Console.WriteLine();
            Console.WriteLine("3. Mehmet Kaya");
            Console.WriteLine("   E-posta: mehmet.kaya@testmail.com");
            Console.WriteLine("   Şifre:   Test123!");
            Console.WriteLine("────────────────────────────────────────");
        }
    }
}
