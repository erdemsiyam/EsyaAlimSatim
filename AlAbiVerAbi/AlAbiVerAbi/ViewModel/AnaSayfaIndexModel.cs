using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlAbiVerAbi
{
    public class AnaSayfaIndexModel
    {
        public List<Kategori> kategoriler { get; set; }
        public List<ilan> ilanlar { get; set; }
        public Arama arama { get; set; }
    }
}