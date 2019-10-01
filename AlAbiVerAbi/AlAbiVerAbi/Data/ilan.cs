namespace AlAbiVerAbi
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ilan")]
    public partial class ilan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ilan()
        {
            Favoriilan = new HashSet<Favoriilan>();
            ilanMesaj = new HashSet<ilanMesaj>();
            ilanResim = new HashSet<ilanResim>();
            OneCikar = new HashSet<OneCikar>();
        }
        [Display(Name ="�lan")]
        public Guid id { get; set; }

        [Display(Name = "�lan Sahibi")]
        public Guid kullanici_id { get; set; }

        [Display(Name = "�lan Kategorisi")]
        public Guid kategori_id { get; set; }

        public Guid? acikartirma_id { get; set; }

        public bool? satildimi { get; set; }

        public bool? ilanacikmi { get; set; }

        public DateTime? ilantarih { get; set; }

        [Required()]
        [Display(Name = "Ba�l�k")]
        [StringLength(50)]
        [MinLength(5, ErrorMessage = "{0} Alan� En Az {1} Karakter Olmal�")]
        [MaxLength(50, ErrorMessage = "{0} Alan� En Fazla {1} Karakter Olmal�")]
        public string baslik { get; set; }

        [Required()]
        [Display(Name = "A��klama")]
        [StringLength(500)]
        [MinLength(5, ErrorMessage = "{0} Alan� En Az {1} Karakter Olmal�")]
        [MaxLength(500, ErrorMessage = "{0} Alan� En Fazla {1} Karakter Olmal�")]
        public string aciklama { get; set; }

        [Required()]
        [Display(Name = "Sorun")]
        [StringLength(500)]
        [MinLength(5, ErrorMessage = "{0} Alan� En Az {1} Karakter Olmal�")]
        [MaxLength(500, ErrorMessage = "{0} Alan� En Fazla {1} Karakter Olmal�")]
        public string sorunu { get; set; }

        [Required()]
        [Display(Name = "Kullan�m S�resi")]
        public int? kullanimsuresi { get; set; }

        [StringLength(50)]
        public string il { get; set; }

        [StringLength(50)]
        public string ilce { get; set; }

        [StringLength(50)]
        public string mahalle { get; set; }

        [StringLength(50)]
        public string konum { get; set; }

        [Required()]
        [Display(Name = "Fiyat�")]
        public int? fiyat { get; set; }

        public virtual AcikArtirma AcikArtirma { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Favoriilan> Favoriilan { get; set; }

        public virtual Kategori Kategori { get; set; }

        public virtual Kullanici Kullanici { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ilanMesaj> ilanMesaj { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ilanResim> ilanResim { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OneCikar> OneCikar { get; set; }
    }
}
