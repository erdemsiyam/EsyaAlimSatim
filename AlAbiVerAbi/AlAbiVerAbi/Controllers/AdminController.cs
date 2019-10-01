using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlAbiVerAbi.Controllers
{
    public class AdminController : Controller
    {
        Context ctx = new Context();
        [HttpGet]
        public ActionResult Index()
        {
            //istatistikler
            return View();
        }
        [HttpGet]
        public ActionResult Kullanicilar()
        {
            var kullanıcılar = ctx.Kullanici.OrderByDescending(x=>x.olusturulmatarih).ToList();
            return View(kullanıcılar);
        }
        [HttpPost]
        public ActionResult Kullanicilar(string kelimeArama)
        {
            var kullanıcılar = ctx.Kullanici.Where(x=>x.ad.Contains(kelimeArama) || x.soyad.Contains(kelimeArama) || x.telefon.Contains(kelimeArama) || x.mail.Contains(kelimeArama)).OrderByDescending(x=>x.olusturulmatarih).ToList();
            return View(kullanıcılar);
        }
        [HttpGet]
        public ActionResult KullaniciDetay(string id)
        {
            //kullanıcının ilanları, favorileri, acikartirmaları gelsin
            Kullanici kullanici = ctx.Kullanici.Where(x => x.id == new Guid(id)).FirstOrDefault();

            if (kullanici == null)
            {
                TempData["Mesaj"] = "Kullanici Bulunamadı.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Kullanicilar");
            }
            return View(kullanici);
        }
        [HttpGet]
        public ActionResult KullaniciDuzenle(string id)
        {

            Kullanici model = ctx.Kullanici.Where(x => x.id == new Guid(id)).FirstOrDefault();

            if (model == null)
            {
                TempData["Mesaj"] = "Kullanici Bulunamadı.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Kullanicilar");
            }
            ViewBag.Roller = new SelectList(ctx.Rol.ToList(), "id", "adi");
            return View(model);
        }
        [HttpPost]
        public ActionResult KullaniciDuzenle(Kullanici model, HttpPostedFileBase gelenResim)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("KullaniciDuzenle");
            }
            Kullanici kullanici = ctx.Kullanici.FirstOrDefault(x => x.id == model.id);
            if (kullanici == null) // kullanıcı yoksa geriye at
            {
                TempData["Mesaj"] = "Kullanici Bulunamadı.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Kullanicilar");
            }
            kullanici.ad = model.ad;
            kullanici.soyad = model.soyad;
            kullanici.engellimi = model.engellimi;
            kullanici.mail = model.mail;
            kullanici.Rol = model.Rol;
            kullanici.onaylimi = model.onaylimi;
            kullanici.telefon = model.telefon;
            string sonuc1, sonuc2;
            if (gelenResim != null)  // yeni resim eklediysek
            {
                if (kullanici.KullaniciResim != null) // önceden resmi varsa silinir..
                {
                    sonuc1 = ResimIslem.Sil(kullanici.KullaniciResim.ad, ResimIslemTip.Kullanici);
                    if (sonuc1 == "Silindi")
                    {
                        KullaniciResim kr = ctx.KullaniciResim.FirstOrDefault(x => x.id == kullanici.kullaniciresim_id);
                        kullanici.KullaniciResim = null;
                        ctx.KullaniciResim.Remove(kr);
                        ctx.SaveChanges();
                    }

                }
                sonuc2 = ResimIslem.Ekle(gelenResim, ResimIslemTip.Kullanici);
                if (sonuc2 != "boyut" || sonuc2 != "uzanti") // ikisi de değilse resim eklendi demektir.
                {
                    KullaniciResim kResim = new KullaniciResim();
                    kResim.id = Guid.NewGuid();
                    kResim.ad = sonuc2;
                    ctx.KullaniciResim.Add(kResim);
                    kullanici.KullaniciResim = kResim;
                }

            }
            ctx.SaveChanges();
            TempData["Mesaj"] = "Kullanıcı Düzenlendi.";
            TempData["MesajTip"] = "success"; // success info warning danger
            return RedirectToAction("Kullanicilar");
        }
        [HttpPost]
        public ActionResult KullaniciRol(string kullanici_id, string yeni_rol)
        {
            Kullanici kullanici = ctx.Kullanici.FirstOrDefault(x => x.id == new Guid(kullanici_id));
            if (kullanici == null)
            {
                TempData["Mesaj"] = "Kullanıcı Bulunamadı.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Kullanicilar");
            }
            if (kullanici == (Kullanici)Session["Kullanici"])
            {
                TempData["Mesaj"] = "Kendi Rolünüzü Değiştiremezsiniz.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Kullanicilar");
            }
            if ((bool)kullanici.engellimi)
            {
                TempData["Mesaj"] = "Engelli Kullanıcının Rolü Değişemez.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Kullanicilar");
            }
            if (!(bool)kullanici.onaylimi)
            {
                TempData["Mesaj"] = "Onaysız Kullanıcının Rolü Değişemez.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Kullanicilar");
            }
            Rol rol = ctx.Rol.FirstOrDefault(x => x.adi == yeni_rol);
            if (rol == null)
            {
                TempData["Mesaj"] = "Rol Bulunamadı.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Kullanicilar");
            }
            kullanici.Rol = rol;
            ctx.SaveChanges();
            TempData["Mesaj"] = kullanici.ad+" " + kullanici.soyad+", "+rol.adi+" Yapıldı.";
            TempData["MesajTip"] = "success"; // success info warning danger
            return RedirectToAction("Kullanicilar");
        }
        [HttpPost]
        public ActionResult KullaniciOnay(string kullanici_id, bool onay)
        {
            Kullanici kullanici = ctx.Kullanici.FirstOrDefault(x => x.id == new Guid(kullanici_id));
            if (kullanici == null)
            {
                TempData["Mesaj"] = "Kullanıcı Bulunamadı.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Kullanicilar");
            }
            kullanici.onaylimi = onay;
            ctx.SaveChanges();
            TempData["Mesaj"] = kullanici.ad + " " + kullanici.soyad + ", Onayı \"" + onay + "\" Yapıldı.";
            if (onay)
            {
                TempData["MesajTip"] = "success"; // success info warning danger
            }
            else
            {
                TempData["MesajTip"] = "warning"; // success info warning danger
            }
            return RedirectToAction("Kullanicilar");
        }
        [HttpPost]
        public ActionResult KullaniciEngel(string kullanici_id, bool engel)
        {
            Kullanici kullanici = ctx.Kullanici.FirstOrDefault(x => x.id == new Guid(kullanici_id));
            if (kullanici == null)
            {
                TempData["Mesaj"] = "Rol Bulunamadı.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Kullanicilar");
            }
            kullanici.engellimi = engel;
            ctx.SaveChanges();

            TempData["Mesaj"] = kullanici.ad + " " + kullanici.soyad + ", Engeli \"" + engel + "\" Yapıldı.";
            if (engel)
            {
                TempData["MesajTip"] = "warning"; // success info warning danger
            }
            else
            {
                TempData["MesajTip"] = "success"; // success info warning danger
            }
            return RedirectToAction("Kullanicilar");
        }




        [HttpGet]
        public ActionResult Kategoriler()
        {
            List<Kategori> model = ctx.Kategori.ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult KategoriEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KategoriEkle(Kategori model)
        {
            model.id = Guid.NewGuid();
            ctx.Kategori.Add(model);
            ctx.SaveChanges();
            TempData["Mesaj"] = "Kategori '" + model.ad + "' Eklendi.";
            TempData["MesajTip"] = "success"; // success info warning danger
            return RedirectToAction("Kategoriler");
        }
        [HttpGet]
        public ActionResult KategoriDuzenle(string id)
        {
            Kategori model = ctx.Kategori.FirstOrDefault(x => x.id == new Guid(id));
            if (model == null)
            {
                TempData["Mesaj"] = "Kategori Bulunamadı";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Kategoriler");
            }

            return View(model);
        }
        [HttpPost]
        public ActionResult KategoriDuzenle(Kategori model)
        {
            Kategori kategori = ctx.Kategori.FirstOrDefault(x => x.id == model.id);
            if (kategori == null) // kategori yoksa geriye at
            {
                TempData["Mesaj"] = "Kategori Bulunamadı.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Kategoriler");
            }
            kategori.ad = model.ad;
            ctx.SaveChanges();

            return RedirectToAction("Kategoriler");
        }
        [HttpGet]
        public ActionResult KategoriSil(string id)
        {
            Kategori model = ctx.Kategori.FirstOrDefault(x => x.id == new Guid(id));
            if (model == null)
            {
                TempData["Mesaj"] = "Kategori Bulunamadı.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Kategoriler");
            }
            if (model.ilan.Count > 0)
            {
                TempData["Mesaj"] = "Kategoriye Bağlı İlan Olduğu İçin Silinemedi.";
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Kategoriler");
            }
            ctx.Kategori.Remove(model);
            ctx.SaveChanges();
            TempData["Mesaj"] = "Kategori Silindi.";
            TempData["MesajTip"] = "success"; // success info warning danger
            return RedirectToAction("Kategoriler");

        }



        [HttpGet]
        public ActionResult İlanlar()
        {
            List<ilan> ilanlar = ctx.ilan.ToList();
            return View(ilanlar);
        }
        [HttpPost]
        public ActionResult İlanlar(string kelimeArama)
        {
            List<ilan> ilanlar = ctx.ilan.Where(x=> x.baslik.Contains(kelimeArama) || x.aciklama.Contains(kelimeArama) || x.Kullanici.ad.Contains(kelimeArama)).ToList();
            return View(ilanlar);
        }
        [HttpGet] // DENEE
        public ActionResult İlanDuzenle(string id)
        {
            ilan ilan = ctx.ilan.FirstOrDefault(x => x.id == new Guid(id));
            if (ilan == null)
            {
                TempData["Mesaj"] = "İlan Bulunamadı.";
                return RedirectToAction("İlanlar");
            }
            ViewBag.Kategoriler = new SelectList(ctx.Kategori.ToList(), "id", "ad");
            ViewBag.İller = new SelectList(Adres.iller, "id", "Ad");
            ViewBag.İlçeler = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "İlçe Seçiniz", Value = "0" } }, "Value", "Text");
            ViewBag.Mahalleler = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "Mahalle Seçiniz", Value = "0" } }, "Value", "Text");
            return View(ilan);
        }
        [HttpPost] // DENEE
        public ActionResult İlanDuzenle(ilan model, IEnumerable<HttpPostedFileBase> gelenResim)
        {
            ilan ilan = ctx.ilan.FirstOrDefault(x => x.id == model.id);
            if (ilan == null)
            {
                ViewBag.Mesaj = "İlan Bulunamadı";
                return View();
            }
            ilan.aciklama = model.aciklama;
            ilan.baslik = model.baslik;
            ilan.ilanacikmi = model.ilanacikmi;
            ilan.il = model.il;
            if (model.ilce != "0")
            {
                ilan.ilce = model.ilce;
            }
            if (model.mahalle != "0")
            {
                ilan.mahalle = model.mahalle;
            }
            ilan.kullanimsuresi = model.kullanimsuresi;
            ilan.satildimi = model.satildimi;
            ilan.sorunu = model.sorunu;
            ilan.fiyat = model.fiyat;
            ilan.kategori_id = model.kategori_id;

            bool hicresimyok = true;
            foreach (HttpPostedFileBase resim in gelenResim)
            {
                if (resim != null)
                {
                    hicresimyok = false;
                    break;
                }
            }

            if (!hicresimyok) // gelen resim varsa resim eklenir.
            {
                int resim_sirasi = 1;
                foreach (HttpPostedFileBase resim in gelenResim)
                {
                    if (resim != null)
                    {
                        // önceden ilanda o SIRA NUMARASINDA resim varsa o silinir.
                        ilanResim ilanResim = ctx.ilanResim.FirstOrDefault(x => x.ilan_id == ilan.id && x.sirasi == resim_sirasi);
                        if (ilanResim != null)
                        {
                            string sonuc = ResimIslem.Sil(ilanResim.ad, ResimIslemTip.İlan);
                            if (sonuc == "Silindi")
                            {
                                ctx.ilanResim.Remove(ilanResim);
                            }
                            else
                            {
                                ViewBag.Mesaj = "Eski Resim Silinemedi // Bulunamadı";
                                return İlanDuzenle(model.id.ToString());
                            }
                        }
                        // resim ekleme
                        ilanResim ir = new ilanResim();
                        ir.ad = ResimIslem.Ekle(resim, ResimIslemTip.İlan);
                        if (ir.ad == "uzanti")
                        {
                            TempData["Mesaj"] = "Resimin Uzantisi Farklı";
                            return İlanDuzenle(model.id.ToString()); // get olan kısım çalışsın aynı sayfaya gitsin giderken de viewbag ile istenilen listeleri versin
                        }
                        else if (ir.ad == "boyut")
                        {
                            TempData["Mesaj"] = "Resimin Boyutu Fazla";
                            return İlanDuzenle(model.id.ToString());// get olan kısım çalışsın aynı sayfaya gitsin giderken de viewbag ile istenilen listeleri versin
                        }
                        ir.id = Guid.NewGuid();
                        ir.ilan_id = ilan.id;
                        ir.sirasi = resim_sirasi;
                        ctx.ilanResim.Add(ir);
                    }
                    resim_sirasi++;
                }
            }
            ctx.SaveChanges();
            return RedirectToAction("İlanlar");
        }
        [HttpGet]
        public ActionResult İlanSil(string id)
        {
            ilan ilan = ctx.ilan.FirstOrDefault(x => x.id == new Guid(id));
            if (ilan == null)
            {
                TempData["Mesaj"] = "ilan bulunamadı";
                return RedirectToAction("İlanlar");
            }
            //ilanın resmi var
            List<ilanResim> ilanResims = ctx.ilanResim.Where(x=>x.ilan_id == new Guid(id)).ToList();
            foreach (ilanResim item in ilanResims)
            {
                ResimIslem.Sil(item.ad, ResimIslemTip.İlan);
                ctx.ilanResim.Remove(item);
            }
            //ilanın acik artirması var : acik artirmanın teklifi var
            if (ilan.AcikArtirma != null)
            {
                AcikArtirma acikArtirma = ilan.AcikArtirma;
                List<Teklif> teklifler = ctx.Teklif.Where(x => x.acikartirma_id == acikArtirma.id).ToList();
                foreach (Teklif item in teklifler)
                {
                    ctx.Teklif.Remove(item);
                }
                ctx.AcikArtirma.Remove(acikArtirma);
            }
            //ilanın favorisi var
            List<Favoriilan> favorileri = ctx.Favoriilan.Where(x => x.ilan_id == ilan.id).ToList();
            foreach (Favoriilan item in favorileri)
            {
                ctx.Favoriilan.Remove(item);
            }
            //ilanın ilan mesajı var
            List<ilanMesaj> mesajlar = ctx.ilanMesaj.Where(x => x.ilan_id == ilan.id).ToList();
            foreach (ilanMesaj item in mesajlar)
            {
                ctx.ilanMesaj.Remove(item);
            }
            //ilanın önce çıkar'ı var
            List<OneCikar> oneCikars = ctx.OneCikar.Where(x => x.ilan_id == ilan.id).ToList();
            foreach (OneCikar item in oneCikars)
            {
                ctx.OneCikar.Remove(item);
            }
            ctx.ilan.Remove(ilan);
            ctx.SaveChanges();
            TempData["Mesaj"] = "İlan Silindi";
            TempData["MesajTip"] = "info"; // success info warning danger
            return RedirectToAction("İlanlar");
        }

        [HttpPost]
        public ActionResult İlanÖneÇıkarGünEkle(string ilan_id,string gun_ekle)
        {
            ilan ilan = ctx.ilan.FirstOrDefault(x => x.id == new Guid(ilan_id));
            if (ilan == null)
            {
                TempData["Mesaj"] = "İlan Bulunamadı.";
                return RedirectToAction("İlanlar");
            }
            OneCikar oc = ilan.OneCikar.OrderByDescending(x => x.bitistarih).FirstOrDefault();
            if (oc == null)
            {
                TempData["Mesaj"] = "İlan Öne Çıkarması Bulunamadı.";
                return RedirectToAction("İlanlar");
            }
            oc.bitistarih = oc.bitistarih.AddDays(Convert.ToDouble(gun_ekle));
            ctx.SaveChanges();
            TempData["Mesaj"] = "İlan Öne Çıkar Özelliğine, "+gun_ekle.ToString()+" Gün Eklendi.";
            return RedirectToAction("İlanlar");
        }
        [HttpGet]
        public ActionResult İlanÖneÇıkarSil(string id) //id : ilan_id
        {
            ilan ilan = ctx.ilan.FirstOrDefault(x => x.id == new Guid(id));
            if (ilan == null)
            {
                TempData["Mesaj"] = "İlan Bulunamadı.";
                return RedirectToAction("İlanlar");
            }
            OneCikar oc = ilan.OneCikar.OrderByDescending(x => x.bitistarih).FirstOrDefault();
            if (oc == null)
            {
                TempData["Mesaj"] = "Zaten İlanın Öne Çıkarması Bulunamadı.";
                return RedirectToAction("İlanlar");
            }
            ctx.OneCikar.Remove(oc);
            ctx.SaveChanges();
            TempData["Mesaj"] = "İlanın Öne Çıkarma Özelliği Silindi.";
            return RedirectToAction("İlanlar");
        }
        [HttpPost]
        public ActionResult İlanGizle(string ilan_id, bool gizle)
        {
            ilan ilan = ctx.ilan.FirstOrDefault(x => x.id == new Guid(ilan_id));
            if (ilan == null)
            {
                TempData["Mesaj"] = "İlan Bulunamadı.";
                return RedirectToAction("İlanlar");
            }
            ilan.ilanacikmi = gizle;
            ctx.SaveChanges();
            return RedirectToAction("İlanlar");
        }
    }

}