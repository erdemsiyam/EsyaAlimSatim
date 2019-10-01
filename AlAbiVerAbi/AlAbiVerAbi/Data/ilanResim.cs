namespace AlAbiVerAbi
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ilanResim")]
    public partial class ilanResim
    {
        public Guid id { get; set; }

        public Guid ilan_id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name ="Ýlan Resim")]
        public string ad { get; set; }

        public int? sirasi { get; set; }

        public virtual ilan ilan { get; set; }
    }
}
