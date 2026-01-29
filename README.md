EgeMail - Yeni Nesil Yapay Zeka Destekli E-Posta Yönetim ve Otomasyon Sistemi
Bu proje, M&Y Akademi Full Stack C# Bootcamp kapsamında "Case 2" olarak geliştirilmiş; modern web geliştirme standartlarını, yüksek güvenlik protokollerini ve yapay zeka destekli otomasyonu bir araya getiren kapsamlı bir e-posta yönetim simülasyonudur.

Sistem, geleneksel e-posta altyapılarını modern teknolojilerle harmanlayarak, kullanıcı deneyimini ve veri yönetimini en üst seviyeye çıkarmayı hedeflemektedir.

Proje Vizyonu ve Mimari Yol Haritası
Bu proje, mevcut haliyle işlevsel bir bütünlüğe sahip olmakla birlikte, kurumsal ölçekte sürdürülebilirliği ve genişletilebilirliği sağlamak adına ciddi bir mühendislik yol haritasına sahiptir. Geliştirme süreci, sadece kod yazmayı değil, yazılım mimarisini en doğru prensiplerle kurgulamayı amaçlamaktadır.

Projenin teknik evrimi için belirlenen temel hedefler şunlardır:

Katmanlı Mimari (N-Tier Architecture): Mevcut yapının; sunum, iş mantığı ve veri erişim katmanlarına ayrılarak, bağımlılıkların minimize edildiği (Loose Coupling) profesyonel bir yapıya dönüştürülmesi.

SOLID Prensipleri: Kod tabanının her satırında Tek Sorumluluk (SRP), Açık/Kapalı (OCP) ve Bağımlılıkların Tersine Çevrilmesi (DIP) gibi prensiplere tam uyum sağlanması.

Fat Controller Anti-Pattern'inden Kaçınma: İş mantığının (Business Logic) controller sınıflarından tamamen arındırılıp servis katmanlarına taşınması ile daha temiz ve test edilebilir bir yapı kurulması.

Veri Transfer Nesneleri (DTO) ve AutoMapper: Veritabanı varlıklarının (Entities) doğrudan dış dünyaya açılmasını engelleyerek, AutoMapper kütüphanesi ile güvenli ve optimize edilmiş veri transferi sağlanması.

Gelişmiş Önbellekleme Stratejileri: Performansı maksimize etmek adına Memory Cache ve dağıtık sistemler için Distributed Cache (Redis vb.) mekanizmalarının entegrasyonu.

API Odaklı Yaklaşım: Sistemin sadece web arayüzü ile sınırlı kalmayıp; mobil ve masaüstü uygulamalarla haberleşebilecek güçlü, dokümante edilmiş RESTful API servislerinin sunulması.

Teknik Özellikler ve Kullanılan Teknolojiler
Proje, endüstri standardı teknolojiler üzerine inşa edilmiştir:

.NET 8 Core: Yüksek performanslı ve platformlar arası çalışabilen backend altyapısı.

ASP.NET Core Identity: Kullanıcı kimlik doğrulama, yetkilendirme ve güvenliğin sağlanması için endüstri standardı kütüphane entegrasyonu.

Entity Framework Core (EF Core): Veritabanı işlemleri için modern ORM yaklaşımı.

Google Gemini AI Entegrasyonu: Gelen e-postaların içeriğini analiz ederek otomatik kategorizasyon yapan yapay zeka servisi.

QuillJS: Kullanıcılara zengin metin düzenleme (Rich Text Editor) imkanı sunan, esnek ve modern metin editörü.

Responsive UI: Bootstrap ve özel tasarımlar ile her cihazda kusursuz çalışan kullanıcı arayüzü.

Yapay Zeka Entegrasyonu: Otomatik Kategorizasyon
Projenin en dikkat çekici özelliklerinden biri, GeminiService aracılığıyla sağlanan yapay zeka desteğidir. Sistem, Google Gemini modellerini kullanarak e-posta içeriklerini anlamsal olarak analiz eder ve ilgili kategorilere (İş, Sosyal, Tanıtım vb.) otomatik olarak atar. Bu özellik, kullanıcıların posta kutusu yönetimini otonom hale getirmektedir.

Yapılandırma (Configuration)
Gemini AI servisinin aktif olarak çalışabilmesi için appsettings.json dosyasına ilgili API anahtarlarının girilmesi gerekmektedir. Sistem, konfigürasyonu aşağıdaki formatta beklemektedir:

JSON
  "Gemini": {
    "ApiKey": "BURAYA_GOOGLE_GEMINI_API_KEY_GIRILECEK",
    "Url": "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent"
  }
Not: API Key temini için Google AI Studio platformunu kullanabilirsiniz.

Veritabanı ve Seed Data Yönetimi
Proje, Code-First yaklaşımı ile geliştirilmiştir. Ancak, sistemin performansını test etmek ve gerçekçi bir veri seti üzerinde çalışmak amacıyla yapay zeka destekli bir SQL tohumlama (seeding) stratejisi izlenmiştir.

SeedData.sql: Proje içerisinde yer alan bu dosya, yapay zekaya özel olarak ürettirilmiş olup, veritabanını test amaçlı binlerce anlamlı veri ile (Kullanıcılar, E-postalar, Loglar) doldurmak için kullanılır. Bu sayede uygulamanın yüksek veri yükü altındaki davranışı simüle edilebilmektedir.

Kurulum ve Çalıştırma
Projeyi yerel ortamınızda çalıştırmak için aşağıdaki adımları izleyin:

Repoyu Klonlayın:

Bash
git clone https://github.com/nullablege/egemail
Veritabanını Güncelleyin: Package Manager Console üzerinden migration işlemlerini uygulayın:

PowerShell
Update-Database
Konfigürasyon: appsettings.json dosyasındaki veritabanı bağlantı dizesini (Connection String) ve Gemini API ayarlarını kendi ortamınıza göre düzenleyin.

Uygulamayı Başlatın: Projeyi Visual Studio veya CLI üzerinden derleyip ayağa kaldırın.

Bu proje, modern web teknolojilerinin yeteneklerini sergileyen ve sürekli gelişime açık bir mimari ile tasarlanmış profesyonel bir çalışmadır.

Proje görselleri : 
<img width="1914" height="914" alt="image" src="https://github.com/user-attachments/assets/1cd63e16-ee10-48dd-9386-a66ac651e743" />
<img width="1910" height="914" alt="image" src="https://github.com/user-attachments/assets/0bf11bf0-dfa1-4407-9a17-8edcd3a6b726" />
<img width="1905" height="911" alt="image" src="https://github.com/user-attachments/assets/a7039f0b-97df-4ad0-8f88-a5bedc4dd2a1" />
<img width="1911" height="906" alt="image" src="https://github.com/user-attachments/assets/e8458741-1746-48fb-bb16-1c6488c3d6c0" />
<img width="1906" height="909" alt="image" src="https://github.com/user-attachments/assets/364a559d-9c68-4aac-b638-a6b94da102d0" />
<img width="1898" height="906" alt="image" src="https://github.com/user-attachments/assets/e74dc1d1-9c8f-454b-9c18-8536151d524b" />
<img width="1897" height="905" alt="image" src="https://github.com/user-attachments/assets/d686dede-96f6-444c-b9ae-7bc217148c8f" />

