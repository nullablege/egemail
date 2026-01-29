using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project2IdentityEmail.Dtos;
using Project2IdentityEmail.Entities;

namespace Project2IdentityEmail.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            user.Name = dto.Name ?? user.Name;
            user.Surname = dto.Surname ?? user.Surname;
            user.About = dto.About ?? user.About;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] = "Profil bilgileriniz güncellendi!";
            }
            else
            {
                TempData["Error"] = "Profil güncellenirken bir hata oluştu!";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (dto.NewPassword != dto.ConfirmNewPassword)
            {
                TempData["Error"] = "Yeni şifreler eşleşmiyor!";
                return RedirectToAction("Index");
            }

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["Success"] = "Şifreniz başarıyla değiştirildi!";
            }
            else
            {
                TempData["Error"] = "Şifre değiştirilirken bir hata oluştu! Mevcut şifrenizi kontrol edin.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAvatar(IFormFile imageFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (imageFile == null || imageFile.Length == 0)
            {
                TempData["Error"] = "Lütfen bir dosya seçin!";
                return RedirectToAction("Index");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                TempData["Error"] = "Geçersiz dosya formatı! Sadece JPG, PNG, GIF veya WEBP dosyaları yükleyebilirsiniz.";
                return RedirectToAction("Index");
            }

            if (imageFile.Length > 5 * 1024 * 1024)
            {
                TempData["Error"] = "Dosya boyutu 5MB'dan büyük olamaz!";
                return RedirectToAction("Index");
            }

            if (!string.IsNullOrEmpty(user.ImageUrl) && !user.ImageUrl.Contains("avatar-"))
            {
                var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, user.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
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
                await imageFile.CopyToAsync(stream);
            }

            user.ImageUrl = "/uploads/avatars/" + fileName;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] = "Profil resminiz güncellendi!";
            }
            else
            {
                TempData["Error"] = "Profil resmi güncellenirken bir hata oluştu!";
            }

            return RedirectToAction("Index");
        }
    }
}
