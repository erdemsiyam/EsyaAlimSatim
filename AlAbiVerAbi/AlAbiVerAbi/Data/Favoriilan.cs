namespace AlAbiVerAbi
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Favoriilan")]
    public partial class Favoriilan
    {
        public Guid id { get; set; }

        public Guid kullanici_id { get; set; }

        public Guid ilan_id { get; set; }

        public virtual ilan ilan { get; set; }

        public virtual Kullanici Kullanici { get; set; }
    }
}
