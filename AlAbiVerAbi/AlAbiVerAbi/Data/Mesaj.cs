namespace AlAbiVerAbi
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Mesaj")]
    public partial class Mesaj
    {
        public Guid id { get; set; }

        public Guid? ilanmesaj_id { get; set; }

        [Column("mesaj", TypeName = "text")]
        public string mesaj1 { get; set; }

        public bool? mesajalicininmi { get; set; }

        public bool? gordumu { get; set; }

        public DateTime? tarih { get; set; }

        public virtual ilanMesaj ilanMesaj { get; set; }
    }
}
