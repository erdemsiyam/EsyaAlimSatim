using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlAbiVerAbi
{
    public class Adres
    {
        // il ilçe mahalle verileri liste olarak buradan döndürülür.
        public static List<İl> iller = new List<İl>()
            {
                new İl(){ id=34,Ad="İstanbul" },
                new İl(){ id=06,Ad="Ankara" },
                new İl(){ id=81,Ad="Düzce" }
            };
        public static List<İlçe> ilçeler = new List<İlçe>()
            {
                new İlçe(){ il_id=34,id=1,Ad="Fatih" },
                new İlçe(){ il_id=06,id=2,Ad="Kızılay" },
                new İlçe(){ il_id=81,id=3,Ad="Akçakoca"}
            };
        public static List<Mahalle> mahalleler = new List<Mahalle>()
            {
                new Mahalle(){ ilce_id=1,id=1,Ad="mahalle1" },
                new Mahalle(){ ilce_id=2,id=2,Ad="mahalle2" },
                new Mahalle(){ ilce_id=3,id=3,Ad="mahalle3" }
            };
    }

}