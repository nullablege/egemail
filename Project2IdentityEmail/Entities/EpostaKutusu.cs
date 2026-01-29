using Project2IdentityEmail.Enums;

namespace Project2IdentityEmail.Entities
{
    public class EpostaKutusu
    {
        public int EpostaKutusuId { get; set; }

        public string SahibiId { get; set; }
        public AppUser Sahibi { get; set; }

        public int MesajId { get; set; }
        public Mesaj Mesaj { get; set; }

        public bool OkunduMu { get; set; } = false;
        public bool YildizliMi { get; set; } = false;
        public bool SilindiMi { get; set; } = false;

        public klasorTipi klasorTipi { get; set; }

        public int? KategoriId { get; set; }
        public Kategori? Kategori { get; set; }

    }
}
