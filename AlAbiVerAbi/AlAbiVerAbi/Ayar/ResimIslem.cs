using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace AlAbiVerAbi
{
    public class ResimIslem
    {
            public static string Ekle(HttpPostedFileBase orjResim,ResimIslemTip resimIslemTip)
            {
                string uzanti = Path.GetExtension(orjResim.FileName);
                if (!(uzanti == ".jpg" || uzanti == ".png"))
                {
                    return "uzanti";
                }

                if (orjResim.ContentLength > 100000000)
                {
                    return "boyut";
                }

                string ad = Guid.NewGuid().ToString() + uzanti;
                Bitmap res = new Bitmap(Image.FromStream(orjResim.InputStream));


                if(resimIslemTip == ResimIslemTip.Kullanici)
                    res.Save(HttpContext.Current.Server.MapPath("/Image/kullanici/" + ad));
                else if (resimIslemTip == ResimIslemTip.İlan)
                    res.Save(HttpContext.Current.Server.MapPath("/Image/ilan/" + ad));


            return ad;
            }

            public static string Sil(string resimAdi, ResimIslemTip resimIslemTip)
            {
                string yol = "";
                if (resimIslemTip == ResimIslemTip.Kullanici)
                    yol = HttpContext.Current.Server.MapPath("/Image/kullanici/" + resimAdi);
                else if (resimIslemTip == ResimIslemTip.İlan)
                    yol = HttpContext.Current.Server.MapPath("/Image/ilan/" + resimAdi);

                if (System.IO.File.Exists(yol)) // belirtilen kalasörde o dosya var mı
                {
                    System.IO.File.Delete(yol); // eski resmi sil
                }
                else
                {
                    return "Bulunamadı";
                }
                return "Silindi";
            }
    }
    public enum ResimIslemTip
    {
        Kullanici,İlan
    }

}