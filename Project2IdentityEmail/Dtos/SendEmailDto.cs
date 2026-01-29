using System.ComponentModel.DataAnnotations;

namespace Project2IdentityEmail.Dtos
{
    public class SendEmailDto
    {
        [Required(ErrorMessage = "Alıcı e-posta adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        public string AliciEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Konu zorunludur")]
        public string Konu { get; set; } = string.Empty;

        [Required(ErrorMessage = "İçerik zorunludur")]
        public string Icerik { get; set; } = string.Empty;
    }
}
