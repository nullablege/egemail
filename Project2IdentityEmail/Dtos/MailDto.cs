namespace Project2IdentityEmail.Dtos
{
    public class MailDto
    {
        public string MailId { get; set; }
        public string MailBaslik { get; set; }
        public string MailIcerik { get; set; }
        public DateTime MailSaat { get; set; }
        public bool OkunduMu { get; set; }
        public bool YildizliMi { get; set; }
        public string GonderenAdi { get; set; }
        public string GonderenEmail { get; set; }
    }
}
