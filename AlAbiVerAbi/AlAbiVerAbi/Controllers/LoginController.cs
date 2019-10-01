using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlAbiVerAbi.Controllers
{
    public class LoginController : Controller
    {
        Context ctx = new Context();

        [HttpGet]
        public ActionResult Giris()
        {
            //eğer sessionda zaten kullanıcı varsa, login sayfası açılmadan anasayfaya yönlendirilir
            Kullanici kullanici = (Kullanici)Session["Kullanici"];
            if (kullanici != null)
            {
                TempData["Mesaj"] = "Zaten Giriş Yaptınız."; // redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Index", "AnaSayfa");
            }
            ViewBag.LoginTitle = "Giriş";
            return View();
        }
        [HttpPost]
        public ActionResult Giris(Kullanici model)
        {
            Kullanici kulVarMi = ctx.Kullanici.FirstOrDefault(x => x.mail == model.mail);
            if (kulVarMi == null)
            {
                TempData["Mesaj"] = "Kullanıcı Bulunamadı.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return View();
            }
            Kullanici kullanici = ctx.Kullanici.FirstOrDefault(x => x.mail == model.mail && x.sifre == model.sifre);
            if (kullanici == null)
            {
                TempData["Mesaj"] = "Şifrenizi Kontrol Ediniz.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return View();
            }

            if ((bool)kullanici.engellimi)
            {
                TempData["Mesaj"] = "Kullanıcı Engellenmiştir."; ;
                TempData["MesajTip"] = "danger"; // success info warning danger
                return View();
            }
            if (!(bool)kullanici.onaylimi)
            {
                TempData["Mesaj"] = "Kullanıcı Onaylı Değildir. Bu Süreçte İlan Veremezsiniz. Bizden Gelen Maildeki Linki Açınız.";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "warning"; // success info warning danger
                Session["Kullanici"] = kullanici; // kişi sessiona'a atılır
                return RedirectToAction("Index", "AnaSayfa");
                //burada kişinin mailine onay maili atılır.
            }
            TempData["Mesaj"] = "Başarıyla Giriş Yapıldı";// redirect yapılan sayfa için temp data ile hata mesajı atılır
            TempData["MesajTip"] = "success"; // success info warning danger
            Session["Kullanici"] = kullanici; // kişi sessiona'a atılır
            return RedirectToAction("Index", "AnaSayfa"); // başarılı Giriş olduğundan anasayfaya yönlendirilir.
        }
        [HttpGet]
        public ActionResult Cikis()
        {
            TempData["Mesaj"] = "Başarıyla Çıkış Yapıldı.";// redirect yapılan sayfa için temp data ile hata mesajı atılır
            TempData["MesajTip"] = "info"; // success info warning danger
            Session.Abandon(); // bu kullanıcıya ait tüm sessionları siler.
            return RedirectToAction("Index", "AnaSayfa");
        }
        [HttpGet]
        public ActionResult KayitOl()
        {

            ViewBag.LoginTitle = "Kayıt Ol";
            return View();
        }
        [HttpPost]
        public ActionResult KayitOl(Kullanici model, HttpPostedFileBase gelenResim, string sifre2)
        {
            if (!ModelState.IsValid)
            {
                return KayitOl();
            }
            if (model.sifre != sifre2)
            {
                TempData["Mesaj"] = "Şifreler Aynı Olmalı";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "warning"; // success info warning danger
                return View(model);
            }
            if (model.ad == "" || model.soyad == "" || model.mail == "" || model.telefon == "" || model.sifre == "")
            {
                TempData["Mesaj"] = "Boş Alanları Doldurun";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "warning"; // success info warning danger
                return View(model);
            }

            Kullanici kontrol = ctx.Kullanici.FirstOrDefault(x => x.mail == model.mail || x.telefon == model.telefon);//kullanıcı aynı mail aynı telefonla kayıt olamaz
            if (kontrol != null)
            {
                TempData["Mesaj"] = "Bu Bilgilerle Kayıtlı Kullanıcı Bulunmaktadır.";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "danger"; // success info warning danger
                return View();
            }
            if (gelenResim == null)
            {
                model.kullaniciresim_id = null;
            }
            else
            {
                string cevap = ResimIslem.Ekle(gelenResim, ResimIslemTip.Kullanici);
                if (cevap == "uzanti")
                {
                    TempData["Mesaj"] = "Yüklenen Dosya Resim Değildir.";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                    TempData["MesajTip"] = "warning"; // success info warning danger
                    return View(model);
                }
                else if (cevap == "boyut")
                {
                    TempData["Mesaj"] = "Resim Dosyasının Boyutu Belirlenenden Büyük";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                    TempData["MesajTip"] = "warning"; // success info warning danger
                    return View(model);
                }
                KullaniciResim kResim = new KullaniciResim();
                kResim.id = Guid.NewGuid();
                kResim.ad = cevap;
                ctx.KullaniciResim.Add(kResim);
                model.KullaniciResim = kResim;
            }
            model.id = Guid.NewGuid();
            model.olusturulmatarih = DateTime.Now;
            model.Rol = ctx.Rol.FirstOrDefault(x => x.adi == "Kullanici");
            model.engellimi = false;
            model.onaylimi = false;

            ctx.Kullanici.Add(model);
            ctx.SaveChanges();

            TempData["Mesaj"] = "Kullanıcı Kayıtı Tamamlandı.(Mail'inize Hesap Onay Linki Gitmiştir. Hesabı Aktifleştirmek İçin Linke Tıklayınız.)";// redirect yapılan sayfa için temp data ile hata mesajı atılır
            TempData["MesajTip"] = "success"; // success info warning danger
            return RedirectToAction("Giris");
        }

        [HttpGet]
        public ActionResult SifremiUnuttum()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SifreResetIstek(string mail)
        {
            Kullanici kullanici = ctx.Kullanici.FirstOrDefault(x => x.mail == mail);
            if (kullanici == null)
            {
                TempData["Mesaj"] = "Kayıtlı Kullanıcı Bulunamadı.";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Giris");
            }

            if (HttpContext.Cache["mailList"] == null)
            {
                HttpContext.Cache.Add(
                                        key: "mailList",
                                        value: new List<CacheResetItem>(),
                                        dependencies: null,
                                        absoluteExpiration: System.Web.Caching.Cache.NoAbsoluteExpiration,
                                        slidingExpiration: new TimeSpan(0, 2, 0), // 2 dk sonra öl
                                        priority: System.Web.Caching.CacheItemPriority.Low,
                                        onRemoveCallback: null
                                );
            }

            List<CacheResetItem> mailList = (List < CacheResetItem >)HttpContext.Cache["mailList"];

            CacheResetItem kulVarMi = mailList.FirstOrDefault(x => x.kullanici.id == kullanici.id);

            if (kulVarMi != null) // kullanıcı listede zaten var mı
            {
                TempData["Mesaj"] = "Şifre Reset Linki Zaten Mailinize Gönderilmiştir.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Giris");
            }

            CacheResetItem sonuc;
            try
            {
                sonuc = MailAt.sifreReset(kullanici);
                if (sonuc == null)
                {
                    TempData["Mesaj"] = "Sunucu Hatası";
                    TempData["MesajTip"] = "danger"; // success info warning danger
                    return RedirectToAction("Giris");
                }
                mailList.Add(sonuc); //new CacheResetItem() { kullanici = kullanici, guid = Guid.NewGuid() }
                HttpContext.Cache["mailList"] = mailList;

                TempData["Mesaj"] = "Şifre Reset Linki Mailinize Gönderilmiştir.";
                TempData["MesajTip"] = "info"; // success info warning danger
                return RedirectToAction("Giris");
            }
            catch (Exception)
            {
                TempData["Mesaj"] = "Şifre Değiştirilemedi.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Giris");
            }

        }
        [HttpGet]
        public ActionResult SifreReset(string id)
        {
            Guid guid = new Guid(id);

            if (HttpContext.Cache["mailList"] != null)
            {
                List<CacheResetItem> mailList = (List<CacheResetItem>)HttpContext.Cache["mailList"];

                CacheResetItem kullaniciListedeVarMi = mailList.FirstOrDefault(x=>x.guid.ToString() == guid.ToString());

                if (kullaniciListedeVarMi != null)
                {
                    TempData["sifreResetId"] = id;
                    return View(kullaniciListedeVarMi.kullanici);
                }
                else
                {
                    TempData["Mesaj"] = "Şifre Değiştirme Talebi Bulunamadı.";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                    TempData["MesajTip"] = "warning"; // success info warning danger
                    return RedirectToAction("Giris");
                }
            }
            else
            {
                TempData["Mesaj"] = "Şifre Değiştirme Talebi Zaman Aşımına Uğradı";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Giris");
            }

        }
        [HttpPost]
        public ActionResult SifreResetOnay(string kulid, string sifre1,string sifre2)
        {
            if (sifre1 != sifre2)
            {
                TempData["Mesaj"] = "Şifreler Farklı";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "danger"; // success info warning danger
                string g = (string)TempData["sifreResetId"];
                return RedirectToAction("SifreReset/"+ g);
            }

            Kullanici k = ctx.Kullanici.FirstOrDefault(x => x.id == new Guid(kulid));
            if (k == null)
            {
                TempData["Mesaj"] = "Kullanıcı Bulunamadı.";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Giris");
            }

            k.sifre = sifre1;
            ctx.SaveChanges();

            TempData["Mesaj"] = "Şifre Değiştirildi";// redirect yapılan sayfa için temp data ile hata mesajı atılır
            TempData["MesajTip"] = "success"; // success info warning danger
            return RedirectToAction("Giris");
        }


        [HttpGet]
        public ActionResult OnayIstek()
        {

            Kullanici kullanici = (Kullanici)Session["Kullanici"];
            if (kullanici == null)
            {
                TempData["Mesaj"] = "Onaydan Önce Giriş Yapınız.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Giris");
            }

            if (HttpContext.Cache["onayList"] == null)
            {
                HttpContext.Cache.Add(
                                        key: "onayList",
                                        value: new List<CacheOnayItem>(),
                                        dependencies: null,
                                        absoluteExpiration: System.Web.Caching.Cache.NoAbsoluteExpiration,
                                        slidingExpiration: new TimeSpan(0, 2, 0), // 2 dk sonra öl
                                        priority: System.Web.Caching.CacheItemPriority.Low,
                                        onRemoveCallback: null
                );
            }

            List<CacheOnayItem> onayList = (List<CacheOnayItem>)HttpContext.Cache["onayList"];

            if (onayList.Where(x => x.kullanici.id == kullanici.id).Count() > 0)
            {
                TempData["Mesaj"] = "Onay İsteği Zaten Yapıldı. Mailinize Bakınız.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Giris");
            }

            onayList.Add(MailAt.onay(kullanici));
            HttpContext.Cache["onayList"] = onayList;

            return View();
        }
        [HttpGet]
        public ActionResult Onay(string id)
        {
            if (HttpContext.Cache["onayList"] == null)
            {
                HttpContext.Cache.Add(
                                        key: "onayList",
                                        value: new List<CacheOnayItem>(),
                                        dependencies: null,
                                        absoluteExpiration: System.Web.Caching.Cache.NoAbsoluteExpiration,
                                        slidingExpiration: new TimeSpan(0, 2, 0), // 2 dk sonra öl
                                        priority: System.Web.Caching.CacheItemPriority.Low,
                                        onRemoveCallback: null
                );
            }


            List<CacheOnayItem> onayList = (List<CacheOnayItem>)HttpContext.Cache["onayList"];

            CacheOnayItem onay = onayList.FirstOrDefault(x => x.guid == new Guid(id));

            if (onay == null)
            {
                TempData["Mesaj"] = "Onay İsteği Zaman Aşımına Uğradı. Tekrar İstekte Bulunun";
                TempData["MesajTip"] = "info"; // success info warning danger
                return RedirectToAction("Giris");
            }

            Kullanici kul = onay.kullanici;
            kul.onaylimi = true;
            ctx.SaveChanges();


            return View();

        }
    }
}