# 📧 EgeMail  
## Yeni Nesil Yapay Zeka Destekli E-Posta Yönetim ve Otomasyon Sistemi

**EgeMail**, M&Y Akademi Full Stack C# Bootcamp kapsamında **“Case 2”** olarak geliştirilmiş; modern web geliştirme standartlarını, yüksek güvenlik protokollerini ve **yapay zeka destekli otomasyonu** bir araya getiren kapsamlı bir e-posta yönetim simülasyonudur.

Sistem, geleneksel e-posta altyapılarını modern teknolojilerle harmanlayarak **kullanıcı deneyimini** ve **veri yönetimini** en üst seviyeye çıkarmayı hedefler.

---

## 🎯 Proje Vizyonu ve Mimari Yol Haritası

Proje mevcut haliyle işlevsel bir bütünlüğe sahip olsa da, **kurumsal ölçekte sürdürülebilirlik** ve **genişletilebilirlik** hedeflenerek ciddi bir mühendislik yol haritası üzerine konumlandırılmıştır.

Amaç yalnızca çalışan bir uygulama geliştirmek değil; **doğru yazılım mimarisi**, **test edilebilirlik** ve **uzun vadeli bakım maliyetlerini düşüren** bir yapı inşa etmektir.

### Teknik Evrim Hedefleri

- **Katmanlı Mimari (N-Tier Architecture)**  
  Sunum, iş mantığı ve veri erişim katmanlarının ayrıştırılmasıyla düşük bağımlılığa (Loose Coupling) sahip profesyonel bir yapı.

- **SOLID Prensipleri**  
  - SRP (Single Responsibility Principle)  
  - OCP (Open/Closed Principle)  
  - DIP (Dependency Inversion Principle)  

- **Fat Controller Anti-Pattern’inden Kaçınma**  
  İş mantığının controller sınıflarından tamamen ayrılarak **service katmanına** taşınması.

- **DTO & AutoMapper Kullanımı**  
  Entity’lerin dış dünyaya doğrudan açılmasının engellenmesi ve güvenli veri transferi.

- **Gelişmiş Önbellekleme Stratejileri**  
  - In-Memory Cache  
  - Distributed Cache (Redis vb.)

- **API Odaklı Mimari**  
  Web, mobil ve masaüstü istemcilerle haberleşebilecek **RESTful API** altyapısı.

---

## 🛠️ Teknik Özellikler ve Kullanılan Teknolojiler

- **.NET 8 Core**
- **ASP.NET Core Identity**
- **Entity Framework Core (EF Core)**
- **Google Gemini AI Entegrasyonu**
- **QuillJS**
- **Responsive UI (Bootstrap)**

---

## 🤖 Yapay Zeka Entegrasyonu – Otomatik Kategorizasyon

Sistem, **Google Gemini** modelleri kullanarak e-posta içeriklerini anlamsal olarak analiz eder ve otomatik olarak kategorilere ayırır:

- İş  
- Sosyal  
- Tanıtım  
- Diğer  

---

## ⚙️ Yapılandırma (Configuration)

```json
"Gemini": {
  "ApiKey": "BURAYA_GOOGLE_GEMINI_API_KEY_GIRILECEK",
  "Url": "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent"
}
```

> Not: API Key için **Google AI Studio** kullanılmalıdır.

---

## 🗄️ Veritabanı ve Seed Data

- **Code-First** yaklaşımı
- Yapay zeka ile üretilmiş **SeedData.sql**
- Binlerce kullanıcı, e-posta ve log verisi

---

## 🚀 Kurulum ve Çalıştırma

### Repoyu Klonlayın
```bash
git clone https://github.com/nullablege/egemail
```

### Veritabanını Güncelleyin
```powershell
Update-Database
```

### Konfigürasyon
`appsettings.json` içindeki bağlantı ayarlarını düzenleyin.

### Uygulamayı Çalıştırın
Visual Studio veya CLI üzerinden başlatın.


Proje görselleri : 
<img width="1914" height="914" alt="image" src="https://github.com/user-attachments/assets/1cd63e16-ee10-48dd-9386-a66ac651e743" />
<img width="1910" height="914" alt="image" src="https://github.com/user-attachments/assets/0bf11bf0-dfa1-4407-9a17-8edcd3a6b726" />
<img width="1905" height="911" alt="image" src="https://github.com/user-attachments/assets/a7039f0b-97df-4ad0-8f88-a5bedc4dd2a1" />
<img width="1911" height="906" alt="image" src="https://github.com/user-attachments/assets/e8458741-1746-48fb-bb16-1c6488c3d6c0" />
<img width="1906" height="909" alt="image" src="https://github.com/user-attachments/assets/364a559d-9c68-4aac-b638-a6b94da102d0" />
<img width="1898" height="906" alt="image" src="https://github.com/user-attachments/assets/e74dc1d1-9c8f-454b-9c18-8536151d524b" />
<img width="1897" height="905" alt="image" src="https://github.com/user-attachments/assets/d686dede-96f6-444c-b9ae-7bc217148c8f" />

