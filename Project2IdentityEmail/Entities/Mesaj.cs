namespace Project2IdentityEmail.Entities
{
    public class Mesaj
    {
        public int MesajId { get; set; }
        public string Konu { get; set; }
        public string Icerik { get; set; }
        public DateTime GonderimTarihi { get; set; }
        public bool OkunduMu { get; set; }
        public string GonderenId { get; set; }
        public AppUser Gonderen { get; set; }

        public ICollection<EpostaKutusu> PostaKutulari { get; set; }

    }
}
