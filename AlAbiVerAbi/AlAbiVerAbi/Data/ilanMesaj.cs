namespace AlAbiVerAbi
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ilanMesaj")]
    public partial class ilanMesaj
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ilanMesaj()
        {
            Mesaj = new HashSet<Mesaj>();
        }

        public Guid id { get; set; }

        public Guid ilan_id { get; set; }

        public Guid satici_kullanici_id { get; set; }

        public Guid alici_kullanici_id { get; set; }

        public virtual ilan ilan { get; set; }

        public virtual Kullanici Kullanici { get; set; }

        public virtual Kullanici Kullanici1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mesaj> Mesaj { get; set; }
    }
}
