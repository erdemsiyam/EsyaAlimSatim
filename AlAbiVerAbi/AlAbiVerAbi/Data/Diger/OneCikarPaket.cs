using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlAbiVerAbi
{

    public class OneCikarPaket
    {
        public int secilenPaket { get; set; }
        public static SelectList paketSelectList = new SelectList(Paket.paketList(), "value", "text");
    }


    public class Paket
    {
        public int value { get; set; }
        public string text { get; set; }
        public static List<Paket> paketList()
        {
            return new List<Paket>()
            {
                new Paket(){ text="1 Hafta(10 TL)", value=1},
                new Paket(){ text="1 Ay(30 TL)", value=2},
                new Paket(){ text="3 Ay(70 TL)", value=3},
                new Paket(){ text="1 Yıl(240 TL)", value=4}
            };
        }
    }


}