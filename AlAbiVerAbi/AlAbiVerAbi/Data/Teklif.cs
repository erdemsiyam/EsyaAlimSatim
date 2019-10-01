namespace AlAbiVerAbi
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Teklif")]
    public partial class Teklif
    {
        public Guid id { get; set; }

        public Guid acikartirma_id { get; set; }

        public Guid alici_kullanici_id { get; set; }

        [Column("teklif")]
        public int? teklif1 { get; set; }

        public DateTime? tekliftarih { get; set; }

        public virtual AcikArtirma AcikArtirma { get; set; }

        public virtual Kullanici Kullanici { get; set; }
    }
}
