namespace Project2IdentityEmail.Dtos
{
    public class StatisticsDto
    {
        public int ToplamMail { get; set; }
        public int OkunmusMail { get; set; }
        public int OkunmamisMail { get; set; }
        public int YildizliMail { get; set; }
        public int SilinenMail { get; set; }
        
        public Dictionary<string, int> KategoriDagilimi { get; set; } = new();
        public Dictionary<string, int> AylikMailSayisi { get; set; } = new();
        public Dictionary<string, int> GunlukMailSayisi { get; set; } = new();
    }
}
