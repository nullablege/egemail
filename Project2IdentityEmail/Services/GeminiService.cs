using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Project2IdentityEmail.Context;

namespace Project2IdentityEmail.Services
{
    public interface IGeminiService
    {
        Task<int?> KategorizasyonYapAsync(string gonderenEmail, string aliciEmail, string konu, string icerik);
    }

    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly EmailContext _context;
        private readonly ILogger<GeminiService> _logger;

        public GeminiService(
            HttpClient httpClient, 
            IConfiguration configuration, 
            EmailContext context,
            ILogger<GeminiService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        public async Task<int?> KategorizasyonYapAsync(string gonderenEmail, string aliciEmail, string konu, string icerik)
        {
            try
            {
                var geminiEnabled = _configuration.GetValue<bool>("Gemini:Enabled");
                if (!geminiEnabled)
                {
                    _logger.LogInformation("Gemini entegrasyonu devre dışı.");
                    return null;
                }

                var apiKey = _configuration["Gemini:ApiKey"];
                var model = _configuration["Gemini:Model"] ?? "gemini-2.0-flash";

                if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_GEMINI_API_KEY_HERE")
                {
                    _logger.LogWarning("Gemini API anahtarı ayarlanmamış.");
                    return null;
                }

                var kategoriler = await _context.Kategoriler.ToListAsync();
                if (!kategoriler.Any())
                {
                    _logger.LogWarning("Veritabanında kategori bulunamadı.");
                    return null;
                }

                var kategoriListesi = string.Join(", ", kategoriler.Select(k => $"{k.KategoriId}:{k.Ad}"));

                var systemPrompt = $@"Sen bir mail sisteminin kategorizayon sistemisin. Mail bilgilerini ve sistemde olan kategorileri veriyorum sana.  Kategoriler: {kategoriListesi}
                Bu bilgilere dayanarak bir kateogri belirlemen gerekiyor.
                SADECE şu JSON formatında yanıt ver: {{""kategoriId"": ID_veya_null}}
                Uygun kategori yoksa null döndür.";

                                var userPrompt = $@"Aşağıdaki e-postayı kategorize et:

                Gönderen: {gonderenEmail}
                Alıcı: {aliciEmail}
                Konu: {konu}
                İçerik: {StripHtml(icerik)}";

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = systemPrompt + "\n\n" + userPrompt }
                            }
                        }
                    },
                    generationConfig = new
                    {
                        temperature = 0.1,
                        maxOutputTokens = 5000
                    }
                };

                var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";
                
                var response = await _httpClient.PostAsync(
                    url,
                    new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
                );

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Gemini API hatası: {StatusCode} - {Error}", response.StatusCode, errorContent);
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Gemini yanıtı: {Response}", responseContent);

                using var doc = JsonDocument.Parse(responseContent);
                
                var candidate = doc.RootElement.GetProperty("candidates")[0];
                if (candidate.TryGetProperty("finishReason", out var finishReason))
                {
                    var reason = finishReason.GetString();
                    if (reason == "MAX_TOKENS" || reason == "SAFETY")
                    {
                        _logger.LogWarning("Gemini yanıtı tamamlanamadı: {Reason}", reason);
                        return null;
                    }
                }

                if (!candidate.TryGetProperty("content", out var content) ||
                    !content.TryGetProperty("parts", out var parts) ||
                    parts.GetArrayLength() == 0)
                {
                    _logger.LogWarning("Gemini yanıtında content/parts bulunamadı.");
                    return null;
                }

                var textResponse = parts[0].GetProperty("text").GetString();

                if (string.IsNullOrEmpty(textResponse))
                {
                    _logger.LogWarning("Gemini boş yanıt döndürdü.");
                    return null;
                }

                var cleanedResponse = textResponse.Trim();
                if (cleanedResponse.StartsWith("```"))
                {
                    cleanedResponse = cleanedResponse.Replace("```json", "").Replace("```", "").Trim();
                }

                using var kategoriDoc = JsonDocument.Parse(cleanedResponse);
                if (kategoriDoc.RootElement.TryGetProperty("kategoriId", out var kategoriIdElement))
                {
                    if (kategoriIdElement.ValueKind == JsonValueKind.Null)
                    {
                        _logger.LogInformation("Gemini uygun kategori bulamadı.");
                        return null;
                    }

                    var kategoriId = kategoriIdElement.GetInt32();
                    
                    if (kategoriler.Any(k => k.KategoriId == kategoriId))
                    {
                        _logger.LogInformation("E-posta kategori ID {KategoriId} olarak belirlendi.", kategoriId);
                        return kategoriId;
                    }
                    else
                    {
                        _logger.LogWarning("Gemini geçersiz kategori ID döndürdü: {KategoriId}", kategoriId);
                        return null;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gemini kategorizasyon hatası");
                return null;
            }
        }

        private static string StripHtml(string html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;
            
            var result = System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", " ");
            result = System.Text.RegularExpressions.Regex.Replace(result, @"\s+", " ");
            return result.Trim();
        }
    }
}
