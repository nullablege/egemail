using Microsoft.AspNetCore.Mvc;

namespace Project2IdentityEmail.Controllers
{
    public class SeedController : Controller
    {
        private readonly IServiceProvider _serviceProvider;

        public SeedController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public async Task<IActionResult> Initialize()
        {
            try
            {
                await SeedData.InitializeAsync(_serviceProvider);
                return Content(@"
                    <html>
                    <head>
                        <meta charset='UTF-8'>
                        <style>
                            body { font-family: 'Segoe UI', Arial, sans-serif; padding: 40px; background: #1a1a2e; color: #eee; }
                            .success { color: #4ade80; font-size: 24px; margin-bottom: 20px; }
                            .info { background: #16213e; padding: 20px; border-radius: 10px; margin: 10px 0; }
                            .user { border-left: 4px solid #6366f1; padding-left: 15px; margin: 15px 0; }
                            .email { color: #60a5fa; font-weight: bold; }
                            .password { color: #f472b6; }
                            h2 { color: #6366f1; }
                        </style>
                    </head>
                    <body>
                        <h1 class='success'>✅ Test Verileri Başarıyla Oluşturuldu!</h1>
                        
                        <h2>📧 Kullanıcı Bilgileri</h2>
                        
                        <div class='user'>
                            <strong>1. Ahmet Yılmaz</strong><br>
                            E-posta: <span class='email'>ahmet.yilmaz@testmail.com</span><br>
                            Şifre: <span class='password'>Test123!</span>
                        </div>
                        
                        <div class='user'>
                            <strong>2. Elif Demir</strong><br>
                            E-posta: <span class='email'>elif.demir@testmail.com</span><br>
                            Şifre: <span class='password'>Test123!</span>
                        </div>
                        
                        <div class='user'>
                            <strong>3. Mehmet Kaya</strong><br>
                            E-posta: <span class='email'>mehmet.kaya@testmail.com</span><br>
                            Şifre: <span class='password'>Test123!</span>
                        </div>
                        
                        <div class='info'>
                            <strong>📊 Oluşturulan Veriler:</strong><br>
                            • 3 Kullanıcı<br>
                            • 30 E-posta mesajı (her kullanıcı için 10)<br>
                            • 4 Kategori (İş, Kişisel, Sosyal, Promosyon)<br>
                            • Gelen ve Giden kutuları
                        </div>
                        
                        <p><a href='/Login/Index' style='color: #60a5fa;'>🔐 Giriş Sayfasına Git</a></p>
                    </body>
                    </html>
                ", "text/html");
            }
            catch (Exception ex)
            {
                return Content($@"
                    <html>
                    <head><meta charset='UTF-8'></head>
                    <body style='font-family: Arial; padding: 40px; background: #1a1a2e; color: #eee;'>
                        <h1 style='color: #f87171;'>❌ Hata Oluştu</h1>
                        <p>{ex.Message}</p>
                        <pre style='background: #16213e; padding: 20px; border-radius: 10px; overflow-x: auto;'>{ex.StackTrace}</pre>
                    </body>
                    </html>
                ", "text/html");
            }
        }
    }
}
