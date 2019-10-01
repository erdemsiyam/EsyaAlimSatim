using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlAbiVerAbi
{
    public class Arama
    {
        public bool boolKategori { get; set; }
        public string kategoriId { get; set; }

        public bool boolKelimeAra { get; set; }
        public string kelimeAra { get; set; }

        public bool boolEnAzFiyat { get; set; }
        public int enAzFiyat { get; set; }

        public bool boolEnFazlaFiyat { get; set; }
        public int enfazlaFiyat { get; set; }

        public bool boolYayinTarihi { get; set; }
        public int yayinTarihi { get; set; }

        public bool boolSiralama { get; set; }
        public int siralama { get; set; }


        public SelectList yayinTarihiList = new SelectList(YayinTarihi.yayinTarihiList(), "value", "text");  
        public SelectList siralamaList = new SelectList(Siralama.siralamaList(), "value", "text");

    }
    public class YayinTarihi
    {
        public int value { get; set; }
        public string text { get; set; }
        public static List<YayinTarihi> yayinTarihiList()
        {
            return new List<YayinTarihi>()
            {
                new YayinTarihi(){ text="Bugün", value=1},
                new YayinTarihi(){ text="Bu Hafta", value=2},
                new YayinTarihi(){ text="Bu Ay", value=3},
                new YayinTarihi(){ text="Bu Yıl", value=4}
            };
        }
    }

    public class Siralama
    {
        public int value { get; set; }
        public string text { get; set; }
        public static List<Siralama> siralamaList()
        {
            return new List<Siralama>()
            {
                new Siralama(){ text="Son Yayınlanan", value=1},
                new Siralama(){ text="Önce Düşük Fiyat", value=2},
                new Siralama(){ text="Önce Yüksek Fiyat", value=3}
            };
        }
    }

}