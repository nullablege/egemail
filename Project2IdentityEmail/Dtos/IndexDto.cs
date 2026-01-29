namespace Project2IdentityEmail.Dtos
{
    public class IndexDto
    {
        public int ToplamMesaj { get; set; }
        public int SayfaSayisi { get; set; }
        public List<MailDto> Mailler { get; set; } = new();
    }
}
