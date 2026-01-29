using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project2IdentityEmail.Entities;

namespace Project2IdentityEmail.Context
{
    public class EmailContext : IdentityDbContext<AppUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;initial catalog=Project2EmailNightDb;integrated security=true;TrustServerCertificate=true");
        }
        public DbSet<Mesaj>? Mesajlar { get; set; }
        public DbSet<EpostaKutusu>? EpostaKutulari { get; set; }
        public DbSet<Kategori>? Kategoriler { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.Entity<EpostaKutusu>()
                    .HasOne(x => x.Sahibi)
                    .WithMany(u => u.PostaKutusu)
                    .HasForeignKey(x => x.SahibiId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<EpostaKutusu>()
                    .HasOne(x => x.Mesaj)
                    .WithMany(m => m.PostaKutulari)
                    .HasForeignKey(x => x.MesajId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Mesaj>()
                    .HasOne(x => x.Gonderen)
                    .WithMany()
                    .HasForeignKey(x => x.GonderenId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}


