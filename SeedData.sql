-- =============================================
-- Project2IdentityEmail - Test Verileri Oluşturma Script'i
-- 3 Kullanıcı + Her biri için 10 E-posta
-- =============================================

USE Project2EmailNightDb;
GO

-- Önce mevcut test verilerini temizle (opsiyonel - dikkatli kullanın)
-- DELETE FROM EpostaKutulari WHERE SahibiId IN (SELECT Id FROM AspNetUsers WHERE Email LIKE '%@testmail.com');
-- DELETE FROM Mesajlar WHERE GonderenId IN (SELECT Id FROM AspNetUsers WHERE Email LIKE '%@testmail.com');
-- DELETE FROM AspNetUsers WHERE Email LIKE '%@testmail.com';

-- =============================================
-- 1. KULLANICILARI OLUŞTUR (3 Kullanıcı)
-- =============================================
-- Not: ASP.NET Identity kullandığınız için şifreler hash'lenmiş olmalı
-- Aşağıdaki hash'ler "Test123!" şifresi için oluşturulmuştur

DECLARE @User1Id NVARCHAR(450) = NEWID();
DECLARE @User2Id NVARCHAR(450) = NEWID();
DECLARE @User3Id NVARCHAR(450) = NEWID();

-- Kullanıcı 1: Ahmet Yılmaz
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, 
    PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, 
    LockoutEnabled, AccessFailedCount, Name, Surname, ImageUrl, About)
VALUES (
    @User1Id,
    'ahmet.yilmaz@testmail.com',
    'AHMET.YILMAZ@TESTMAIL.COM',
    'ahmet.yilmaz@testmail.com',
    'AHMET.YILMAZ@TESTMAIL.COM',
    1,
    'AQAAAAIAAYagAAAAELEqkbXvQ3x9VzRwKYqFsZXxOQyKzBVZqRJE2K8hWKQj6gZvM8pzqN7FxLmH2sG5Kw==', -- Test123!
    NEWID(),
    NEWID(),
    0,
    0,
    1,
    0,
    'Ahmet',
    'Yılmaz',
    '/vertical/assets/images/avatars/avatar-1.png',
    'Yazılım geliştirici, İstanbul.'
);

-- Kullanıcı 2: Elif Demir
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, 
    PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, 
    LockoutEnabled, AccessFailedCount, Name, Surname, ImageUrl, About)
VALUES (
    @User2Id,
    'elif.demir@testmail.com',
    'ELIF.DEMIR@TESTMAIL.COM',
    'elif.demir@testmail.com',
    'ELIF.DEMIR@TESTMAIL.COM',
    1,
    'AQAAAAIAAYagAAAAELEqkbXvQ3x9VzRwKYqFsZXxOQyKzBVZqRJE2K8hWKQj6gZvM8pzqN7FxLmH2sG5Kw==', -- Test123!
    NEWID(),
    NEWID(),
    0,
    0,
    1,
    0,
    'Elif',
    'Demir',
    '/vertical/assets/images/avatars/avatar-2.png',
    'Proje yöneticisi, Ankara.'
);

-- Kullanıcı 3: Mehmet Kaya
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, 
    PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, 
    LockoutEnabled, AccessFailedCount, Name, Surname, ImageUrl, About)
VALUES (
    @User3Id,
    'mehmet.kaya@testmail.com',
    'MEHMET.KAYA@TESTMAIL.COM',
    'mehmet.kaya@testmail.com',
    'MEHMET.KAYA@TESTMAIL.COM',
    1,
    'AQAAAAIAAYagAAAAELEqkbXvQ3x9VzRwKYqFsZXxOQyKzBVZqRJE2K8hWKQj6gZvM8pzqN7FxLmH2sG5Kw==', -- Test123!
    NEWID(),
    NEWID(),
    0,
    0,
    1,
    0,
    'Mehmet',
    'Kaya',
    '/vertical/assets/images/avatars/avatar-3.png',
    'UI/UX tasarımcı, İzmir.'
);

-- =============================================
-- 2. KATEGORİLERİ OLUŞTUR
-- =============================================
-- Önce mevcut kategorileri kontrol et
IF NOT EXISTS (SELECT 1 FROM Kategoriler WHERE Ad = 'İş')
    INSERT INTO Kategoriler (Ad) VALUES ('İş');
IF NOT EXISTS (SELECT 1 FROM Kategoriler WHERE Ad = 'Kişisel')
    INSERT INTO Kategoriler (Ad) VALUES ('Kişisel');
IF NOT EXISTS (SELECT 1 FROM Kategoriler WHERE Ad = 'Sosyal')
    INSERT INTO Kategoriler (Ad) VALUES ('Sosyal');
IF NOT EXISTS (SELECT 1 FROM Kategoriler WHERE Ad = 'Promosyon')
    INSERT INTO Kategoriler (Ad) VALUES ('Promosyon');

DECLARE @KatIs INT = (SELECT KategoriId FROM Kategoriler WHERE Ad = 'İş');
DECLARE @KatKisisel INT = (SELECT KategoriId FROM Kategoriler WHERE Ad = 'Kişisel');
DECLARE @KatSosyal INT = (SELECT KategoriId FROM Kategoriler WHERE Ad = 'Sosyal');
DECLARE @KatPromosyon INT = (SELECT KategoriId FROM Kategoriler WHERE Ad = 'Promosyon');

-- =============================================
-- 3. MESAJLARI OLUŞTUR (Her kullanıcı için 10 mesaj)
-- =============================================

-- Kullanıcı 1'in gönderdiği mesajlar (Ahmet Yılmaz)
DECLARE @Mesaj1 INT, @Mesaj2 INT, @Mesaj3 INT, @Mesaj4 INT, @Mesaj5 INT;
DECLARE @Mesaj6 INT, @Mesaj7 INT, @Mesaj8 INT, @Mesaj9 INT, @Mesaj10 INT;

-- Ahmet'in gönderdiği 5 mesaj
INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Proje Toplantısı Hakkında', 'Merhaba, yarınki proje toplantısı için hazırlıklar tamamlandı. Toplantı saat 14:00''da gerçekleşecek. Katılımınızı bekliyorum.', DATEADD(DAY, -1, GETDATE()), 1, @User1Id);
SET @Mesaj1 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Haftalık Rapor', 'Bu hafta tamamlanan işler ve önümüzdeki hafta için planlanan görevler ektedir. Lütfen inceleyip geri dönüş yapın.', DATEADD(DAY, -2, GETDATE()), 0, @User1Id);
SET @Mesaj2 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Yeni Özellik Talebi', 'Müşteriden gelen talep doğrultusunda yeni bir özellik eklememiz gerekiyor. Detayları toplantıda görüşelim.', DATEADD(DAY, -3, GETDATE()), 1, @User1Id);
SET @Mesaj3 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Doğum Günü Kutlaması', 'Doğum günün kutlu olsun! Nice mutlu yıllara. 🎂🎉', DATEADD(DAY, -5, GETDATE()), 1, @User1Id);
SET @Mesaj4 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Kod İnceleme Talebi', 'Geliştirdiğim modülün kod incelemesini yapabilir misin? PR linki ekte.', DATEADD(DAY, -7, GETDATE()), 0, @User1Id);
SET @Mesaj5 = SCOPE_IDENTITY();

-- Elif'in gönderdiği 5 mesaj
INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Sprint Planlama', 'Yeni sprint için görev dağılımını yaptım. Lütfen atanan görevleri kontrol edin ve onaylayın.', DATEADD(DAY, -1, GETDATE()), 0, @User2Id);
SET @Mesaj6 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Tatil Planları', 'Bu yaz için tatil planları yapmayı düşünüyor musun? Belki birlikte bir yer ayarlayabiliriz.', DATEADD(DAY, -4, GETDATE()), 1, @User2Id);
SET @Mesaj7 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Sunum Hazırlığı', 'Müşteri sunumu için hazırlıklar devam ediyor. Senin bölümünü eklememiz gerekiyor.', DATEADD(DAY, -6, GETDATE()), 1, @User2Id);
SET @Mesaj8 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Yeni Ekip Üyesi', 'Ekibimize yeni bir geliştirici katılıyor. Yarın tanışma toplantısı yapacağız.', DATEADD(DAY, -8, GETDATE()), 0, @User2Id);
SET @Mesaj9 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Eğitim Fırsatı', 'Şirket dışından bir eğitim fırsatı var. İlgilenirsen detayları paylaşabilirim.', DATEADD(DAY, -10, GETDATE()), 1, @User2Id);
SET @Mesaj10 = SCOPE_IDENTITY();

-- Mehmet'in gönderdiği 5 mesaj
DECLARE @Mesaj11 INT, @Mesaj12 INT, @Mesaj13 INT, @Mesaj14 INT, @Mesaj15 INT;

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('UI Tasarım Güncellemesi', 'Yeni tasarımları Figma''ya yükledim. Lütfen inceleyip geri bildirim verin.', DATEADD(HOUR, -5, GETDATE()), 0, @User3Id);
SET @Mesaj11 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Logo Revizyonu', 'Müşterinin istediği logo değişikliklerini tamamladım. Onay için bekliyorum.', DATEADD(DAY, -2, GETDATE()), 1, @User3Id);
SET @Mesaj12 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Renk Paleti Önerisi', 'Yeni proje için hazırladığım renk paleti önerilerini ekte bulabilirsiniz.', DATEADD(DAY, -4, GETDATE()), 0, @User3Id);
SET @Mesaj13 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Hafta Sonu Etkinliği', 'Ekip olarak hafta sonu bir aktivite yapmayı düşünüyoruz. Katılır mısın?', DATEADD(DAY, -6, GETDATE()), 1, @User3Id);
SET @Mesaj14 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Mobil Uygulama Tasarımı', 'Mobil uygulama için wireframe''leri hazırladım. İnceleme toplantısı ayarlayalım mı?', DATEADD(DAY, -9, GETDATE()), 0, @User3Id);
SET @Mesaj15 = SCOPE_IDENTITY();

-- Ek mesajlar (toplam 30 mesaj olması için her kullanıcıya 5 mesaj daha)
DECLARE @Mesaj16 INT, @Mesaj17 INT, @Mesaj18 INT, @Mesaj19 INT, @Mesaj20 INT;
DECLARE @Mesaj21 INT, @Mesaj22 INT, @Mesaj23 INT, @Mesaj24 INT, @Mesaj25 INT;
DECLARE @Mesaj26 INT, @Mesaj27 INT, @Mesaj28 INT, @Mesaj29 INT, @Mesaj30 INT;

-- Ahmet'in ek mesajları
INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Bug Raporu', 'Sistemde kritik bir hata tespit ettim. Hemen düzeltilmesi gerekiyor.', DATEADD(HOUR, -3, GETDATE()), 0, @User1Id);
SET @Mesaj16 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Veritabanı Optimizasyonu', 'Performans iyileştirmesi için veritabanı sorgularını optimize ettim.', DATEADD(DAY, -11, GETDATE()), 1, @User1Id);
SET @Mesaj17 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('API Dokümantasyonu', 'Yeni API endpoint''leri için dokümantasyonu tamamladım. Wiki''ye ekledim.', DATEADD(DAY, -12, GETDATE()), 1, @User1Id);
SET @Mesaj18 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Kahve Molası?', 'Bugün öğleden sonra kahve içmeye ne dersin? Yeni açılan kafeyi deneyelim.', DATEADD(DAY, -13, GETDATE()), 1, @User1Id);
SET @Mesaj19 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Güvenlik Güncellemesi', 'Kritik güvenlik yamalarını uyguladım. Lütfen test ortamında kontrol edin.', DATEADD(DAY, -14, GETDATE()), 0, @User1Id);
SET @Mesaj20 = SCOPE_IDENTITY();

-- Elif'in ek mesajları
INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Bütçe Raporu', 'Q1 bütçe raporunu hazırladım. Yönetim toplantısından önce incelemenizi rica ederim.', DATEADD(HOUR, -8, GETDATE()), 0, @User2Id);
SET @Mesaj21 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Müşteri Geri Bildirimi', 'Son demoda müşteriden çok olumlu geri bildirimler aldık! Tebrikler ekip!', DATEADD(DAY, -11, GETDATE()), 1, @User2Id);
SET @Mesaj22 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Deadline Hatırlatması', 'Proje teslim tarihine 1 hafta kaldı. Lütfen tüm görevleri tamamlayın.', DATEADD(DAY, -12, GETDATE()), 1, @User2Id);
SET @Mesaj23 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Yemek Daveti', 'Cumartesi akşamı evde yemek yapıyorum. Gelebilir misin?', DATEADD(DAY, -15, GETDATE()), 1, @User2Id);
SET @Mesaj24 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Konferans Katılımı', 'Gelecek ay düzenlenecek tech konferansına katılmak ister misin? Biletler şirketten.', DATEADD(DAY, -16, GETDATE()), 0, @User2Id);
SET @Mesaj25 = SCOPE_IDENTITY();

-- Mehmet'in ek mesajları
INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('İkon Seti', 'Projeye özel ikon seti hazırladım. SVG formatında paylaşıyorum.', DATEADD(HOUR, -2, GETDATE()), 0, @User3Id);
SET @Mesaj26 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Dark Mode Tasarımı', 'Uygulamanın dark mode versiyonunu tamamladım. Çok şık oldu!', DATEADD(DAY, -10, GETDATE()), 1, @User3Id);
SET @Mesaj27 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Font Önerisi', 'Yeni projeler için kullanabileceğimiz güzel fontlar buldum. Listeliyorum.', DATEADD(DAY, -13, GETDATE()), 1, @User3Id);
SET @Mesaj28 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Fotoğraf Gezisi', 'Hafta sonu fotoğraf çekmeye gidelim mi? Güzel manzaralar var.', DATEADD(DAY, -14, GETDATE()), 1, @User3Id);
SET @Mesaj29 = SCOPE_IDENTITY();

INSERT INTO Mesajlar (Konu, Icerik, GonderimTarihi, OkunduMu, GonderenId)
VALUES ('Animasyon Desteği', 'Landing page için mikro animasyonlar ekledim. Canlı demo linki ekte.', DATEADD(DAY, -17, GETDATE()), 0, @User3Id);
SET @Mesaj30 = SCOPE_IDENTITY();

-- =============================================
-- 4. EPOSTA KUTULARINI OLUŞTUR
-- Her kullanıcının gelen ve giden kutusuna mesajları ekle
-- =============================================

-- Ahmet'in posta kutusu (Gelen: 10 mesaj, Giden: kendi gönderdiği mesajlar)
-- Gelen mesajlar (Elif ve Mehmet'ten)
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User1Id, @Mesaj6, 0, 1, 0, 1, @KatIs); -- Sprint Planlama - GelenKutusu
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User1Id, @Mesaj7, 1, 0, 0, 1, @KatKisisel); -- Tatil Planları
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User1Id, @Mesaj8, 1, 1, 0, 1, @KatIs); -- Sunum Hazırlığı
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User1Id, @Mesaj11, 0, 1, 0, 1, @KatIs); -- UI Tasarım
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User1Id, @Mesaj12, 1, 0, 0, 1, @KatIs); -- Logo Revizyonu
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User1Id, @Mesaj14, 1, 0, 0, 1, @KatSosyal); -- Hafta Sonu Etkinliği
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User1Id, @Mesaj21, 0, 1, 0, 1, @KatIs); -- Bütçe Raporu
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User1Id, @Mesaj26, 0, 0, 0, 1, @KatIs); -- İkon Seti
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User1Id, @Mesaj27, 1, 1, 0, 1, @KatIs); -- Dark Mode
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User1Id, @Mesaj29, 1, 0, 0, 1, @KatKisisel); -- Fotoğraf Gezisi

-- Ahmet'in giden kutusu
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User1Id, @Mesaj1, 1, 0, 0, 2, @KatIs);
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User1Id, @Mesaj2, 1, 0, 0, 2, @KatIs);
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User1Id, @Mesaj3, 1, 0, 0, 2, @KatIs);
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User1Id, @Mesaj4, 1, 1, 0, 2, @KatKisisel);
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User1Id, @Mesaj5, 1, 0, 0, 2, @KatIs);

-- Elif'in posta kutusu (Gelen: 10 mesaj)
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User2Id, @Mesaj1, 1, 1, 0, 1, @KatIs); -- Proje Toplantısı
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User2Id, @Mesaj2, 0, 0, 0, 1, @KatIs); -- Haftalık Rapor
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User2Id, @Mesaj3, 1, 1, 0, 1, @KatIs); -- Yeni Özellik
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User2Id, @Mesaj11, 0, 0, 0, 1, @KatIs); -- UI Tasarım
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User2Id, @Mesaj13, 0, 1, 0, 1, @KatIs); -- Renk Paleti
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User2Id, @Mesaj15, 0, 0, 0, 1, @KatIs); -- Mobil Uygulama
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User2Id, @Mesaj16, 0, 1, 0, 1, @KatIs); -- Bug Raporu
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User2Id, @Mesaj19, 1, 0, 0, 1, @KatKisisel); -- Kahve Molası
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User2Id, @Mesaj28, 1, 0, 0, 1, @KatIs); -- Font Önerisi
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User2Id, @Mesaj30, 0, 1, 0, 1, @KatIs); -- Animasyon

-- Elif'in giden kutusu
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User2Id, @Mesaj6, 1, 0, 0, 2, @KatIs);
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User2Id, @Mesaj7, 1, 0, 0, 2, @KatKisisel);
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User2Id, @Mesaj8, 1, 0, 0, 2, @KatIs);
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User2Id, @Mesaj9, 1, 0, 0, 2, @KatIs);
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User2Id, @Mesaj10, 1, 0, 0, 2, @KatIs);

-- Mehmet'in posta kutusu (Gelen: 10 mesaj)
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User3Id, @Mesaj1, 1, 0, 0, 1, @KatIs); -- Proje Toplantısı
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User3Id, @Mesaj4, 1, 1, 0, 1, @KatKisisel); -- Doğum Günü
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User3Id, @Mesaj5, 0, 0, 0, 1, @KatIs); -- Kod İnceleme
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User3Id, @Mesaj6, 0, 1, 0, 1, @KatIs); -- Sprint Planlama
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User3Id, @Mesaj9, 0, 0, 0, 1, @KatIs); -- Yeni Ekip Üyesi
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User3Id, @Mesaj10, 1, 0, 0, 1, @KatIs); -- Eğitim Fırsatı
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User3Id, @Mesaj17, 1, 1, 0, 1, @KatIs); -- Veritabanı Optimizasyonu
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User3Id, @Mesaj22, 1, 1, 0, 1, @KatIs); -- Müşteri Geri Bildirimi
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User3Id, @Mesaj23, 1, 0, 0, 1, @KatIs); -- Deadline Hatırlatması
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User3Id, @Mesaj24, 1, 0, 0, 1, @KatKisisel); -- Yemek Daveti

-- Mehmet'in giden kutusu
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User3Id, @Mesaj11, 1, 0, 0, 2, @KatIs);
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User3Id, @Mesaj12, 1, 0, 0, 2, @KatIs);
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User3Id, @Mesaj13, 1, 0, 0, 2, @KatIs);
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User3Id, @Mesaj14, 1, 0, 0, 2, @KatSosyal);
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User3Id, @Mesaj15, 1, 0, 0, 2, @KatIs);

-- Çöp kutusuna bazı mesajlar ekle
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User1Id, @Mesaj9, 1, 0, 1, 3, @KatIs); -- Çöp kutusunda
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User2Id, @Mesaj20, 1, 0, 1, 3, @KatIs); -- Çöp kutusunda
INSERT INTO EpostaKutulari (SahibiId, MesajId, OkunduMu, YildizliMi, SilindiMi, klasorTipi, KategoriId)
VALUES (@User3Id, @Mesaj18, 1, 0, 1, 3, @KatIs); -- Çöp kutusunda

-- =============================================
-- 5. ÖZET BİLGİLER
-- =============================================
PRINT '=== TEST VERİLERİ BAŞARIYLA OLUŞTURULDU ===';
PRINT '';
PRINT 'KULLANICI BİLGİLERİ:';
PRINT '--------------------------------------------';
PRINT '1. Ahmet Yılmaz';
PRINT '   E-posta: ahmet.yilmaz@testmail.com';
PRINT '   Şifre: Test123!';
PRINT '';
PRINT '2. Elif Demir';
PRINT '   E-posta: elif.demir@testmail.com';
PRINT '   Şifre: Test123!';
PRINT '';
PRINT '3. Mehmet Kaya';
PRINT '   E-posta: mehmet.kaya@testmail.com';
PRINT '   Şifre: Test123!';
PRINT '--------------------------------------------';
PRINT '';

-- Kontrol sorguları
SELECT 'Kullanıcı Sayısı' AS Bilgi, COUNT(*) AS Sayi FROM AspNetUsers WHERE Email LIKE '%@testmail.com';
SELECT 'Toplam Mesaj Sayısı' AS Bilgi, COUNT(*) AS Sayi FROM Mesajlar;
SELECT 'Toplam Posta Kutusu Kaydı' AS Bilgi, COUNT(*) AS Sayi FROM EpostaKutulari;
SELECT 'Kategori Sayısı' AS Bilgi, COUNT(*) AS Sayi FROM Kategoriler;

GO
