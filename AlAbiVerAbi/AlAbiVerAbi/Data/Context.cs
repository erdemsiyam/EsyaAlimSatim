namespace AlAbiVerAbi
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Context : DbContext
    {
        public Context()
            : base("name=Context")
        {
        }

        public virtual DbSet<AcikArtirma> AcikArtirma { get; set; }
        public virtual DbSet<Favoriilan> Favoriilan { get; set; }
        public virtual DbSet<ilan> ilan { get; set; }
        public virtual DbSet<ilanMesaj> ilanMesaj { get; set; }
        public virtual DbSet<ilanResim> ilanResim { get; set; }
        public virtual DbSet<Kategori> Kategori { get; set; }
        public virtual DbSet<Kullanici> Kullanici { get; set; }
        public virtual DbSet<KullaniciResim> KullaniciResim { get; set; }
        public virtual DbSet<Mesaj> Mesaj { get; set; }
        public virtual DbSet<OneCikar> OneCikar { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Teklif> Teklif { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcikArtirma>()
                .HasMany(e => e.ilan)
                .WithOptional(e => e.AcikArtirma)
                .HasForeignKey(e => e.acikartirma_id);

            modelBuilder.Entity<AcikArtirma>()
                .HasMany(e => e.Teklif)
                .WithRequired(e => e.AcikArtirma)
                .HasForeignKey(e => e.acikartirma_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ilan>()
                .HasMany(e => e.Favoriilan)
                .WithRequired(e => e.ilan)
                .HasForeignKey(e => e.ilan_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ilan>()
                .HasMany(e => e.ilanMesaj)
                .WithRequired(e => e.ilan)
                .HasForeignKey(e => e.ilan_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ilan>()
                .HasMany(e => e.ilanResim)
                .WithRequired(e => e.ilan)
                .HasForeignKey(e => e.ilan_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ilan>()
                .HasMany(e => e.OneCikar)
                .WithRequired(e => e.ilan)
                .HasForeignKey(e => e.ilan_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ilanMesaj>()
                .HasMany(e => e.Mesaj)
                .WithOptional(e => e.ilanMesaj)
                .HasForeignKey(e => e.ilanmesaj_id);

            modelBuilder.Entity<Kategori>()
                .HasMany(e => e.ilan)
                .WithRequired(e => e.Kategori)
                .HasForeignKey(e => e.kategori_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.Favoriilan)
                .WithRequired(e => e.Kullanici)
                .HasForeignKey(e => e.kullanici_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.ilan)
                .WithRequired(e => e.Kullanici)
                .HasForeignKey(e => e.kullanici_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.ilanMesaj)
                .WithRequired(e => e.Kullanici)
                .HasForeignKey(e => e.satici_kullanici_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.ilanMesaj1)
                .WithRequired(e => e.Kullanici1)
                .HasForeignKey(e => e.alici_kullanici_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.Teklif)
                .WithRequired(e => e.Kullanici)
                .HasForeignKey(e => e.alici_kullanici_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KullaniciResim>()
                .HasMany(e => e.Kullanici)
                .WithOptional(e => e.KullaniciResim)
                .HasForeignKey(e => e.kullaniciresim_id);

            modelBuilder.Entity<Mesaj>()
                .Property(e => e.mesaj1)
                .IsUnicode(false);

            modelBuilder.Entity<Rol>()
                .HasMany(e => e.Kullanici)
                .WithOptional(e => e.Rol)
                .HasForeignKey(e => e.rol_id);
        }
    }
}
