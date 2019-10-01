namespace AlAbiVerAbi
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OneCikar")]
    public partial class OneCikar
    {
        public Guid id { get; set; }

        public Guid ilan_id { get; set; }

        public DateTime bitistarih { get; set; }

        public virtual ilan ilan { get; set; }
    }
}
