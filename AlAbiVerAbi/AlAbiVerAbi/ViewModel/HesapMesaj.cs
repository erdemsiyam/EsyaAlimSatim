using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlAbiVerAbi.ViewModel
{
    public class HesapMesaj
    {
        public bool sahibiyim { get; set; }
        public Kullanici alici { get; set; }
        public Kullanici satici { get; set; }
        public ilan ilani { get; set; }
        public List<ilanMesaj> mesajlar;
    }
}