namespace Project2IdentityEmail.Entities
{
    public class Kategori
    {
        public int KategoriId { get; set; }
        public string Ad { get; set; }
        public string? Icon { get; set; }
        public string? Renk { get; set; }

        public ICollection<EpostaKutusu> Epostalar { get; set; }
    }
}
