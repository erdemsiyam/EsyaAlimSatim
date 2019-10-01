namespace AlAbiVerAbi
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kullanici")]
    public partial class Kullanici
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kullanici()
        {
            Favoriilan = new HashSet<Favoriilan>();
            ilan = new HashSet<ilan>();
            ilanMesaj = new HashSet<ilanMesaj>();
            ilanMesaj1 = new HashSet<ilanMesaj>();
            Teklif = new HashSet<Teklif>();
        }

        [Display(Name = "Kullanýcý")]
        public Guid id { get; set; }

        [Required()]
        [Display(Name = "Adý")]
        [StringLength(50)]
        [MaxLength(50, ErrorMessage = "{0} Alaný En Fazla {1} Karakter Olmalý")]
        public string ad { get; set; }

        [Required()]
        [Display(Name = "Soyadý")]
        [StringLength(50)]
        [MaxLength(50, ErrorMessage = "{0} Alaný En Fazla {1} Karakter Olmalý")]
        public string soyad { get; set; }

        [Required()]
        [Display(Name = "Þifre")]
        [DataType(DataType.Password)]
        [StringLength(50)]
        [MinLength(3,ErrorMessage ="{0} Alaný En Az {1} Karakter Olmalý")]
        [MaxLength(50, ErrorMessage = "{0} Alaný En Fazla {1} Karakter Olmalý")]
        public string sifre { get; set; }

        [Required()]
        [Display(Name = "E-Mail")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage ="Uygun Bir {0} Giriniz.")]
        [MinLength(5, ErrorMessage = "{0} Alaný En Az {1} Karakter Olmalý")]
        [MaxLength(50, ErrorMessage = "{0} Alaný En Fazla {1} Karakter Olmalý")]
        public string mail { get; set; }

        [Required()]
        [Display(Name = "Telefon")]
        [StringLength(50)]
        [DataType(DataType.PhoneNumber)]
        [Phone()]
        public string telefon { get; set; }

        public Guid? rol_id { get; set; }

        public Guid? kullaniciresim_id { get; set; }

        [Display(Name = "Oluþturulma Tarihi")]
        public DateTime? olusturulmatarih { get; set; }


        [Display(Name = "Onaylý Mý?")]
        public bool? onaylimi { get; set; }

        [Display(Name = "Engelli Mi?")]
        public bool? engellimi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Favoriilan> Favoriilan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ilan> ilan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ilanMesaj> ilanMesaj { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ilanMesaj> ilanMesaj1 { get; set; }

        public virtual KullaniciResim KullaniciResim { get; set; }

        public virtual Rol Rol { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Teklif> Teklif { get; set; }
    }
}
