using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project2IdentityEmail.Dtos;
using Project2IdentityEmail.Entities;

namespace Project2IdentityEmail.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RegisterController(UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateUserRegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            if (dto.Password != dto.ConfirmPassword)
            {
                ViewBag.Error = "Şifreler eşleşmiyor!";
                return View(dto);
            }

            string? imageUrl = null;

            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(dto.ImageFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ViewBag.Error = "Geçersiz dosya formatı! Sadece JPG, PNG, GIF veya WEBP dosyaları yükleyebilirsiniz.";
                    return View(dto);
                }

                if (dto.ImageFile.Length > 5 * 1024 * 1024)
                {
                    ViewBag.Error = "Dosya boyutu 5MB'dan büyük olamaz!";
                    return View(dto);
                }

                var fileName = Guid.NewGuid().ToString() + extension;
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "avatars");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }

                imageUrl = "/uploads/avatars/" + fileName;
            }

            AppUser appUser = new AppUser()
            {
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                UserName = dto.Username,
                ImageUrl = imageUrl
            };

            var result = await _userManager.CreateAsync(appUser, dto.Password);

            if (result.Succeeded)
            {
                TempData["Success"] = "Kayıt başarılı! Giriş yapabilirsiniz.";
                return RedirectToAction("Index", "Login");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(dto);
            }
        }
    }
}
