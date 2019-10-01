using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlAbiVerAbi.Controllers
{
    public class HesapController : Controller
    {
        //_SecurityFilter' sınıfı sayesinde , oturum açan kullanıcıların geldiğini varsayıyoruz.
        Context ctx = new Context();

        [HttpGet]
        public ActionResult Index()
        {
            Kullanici kullanici = (Kullanici)Session["Kullanici"];
            if (!(bool)kullanici.onaylimi)
            {
                TempData["Mesaj"] = "Hesap Onaylı Olmadığından İlan Eklenemez. Lütfen Hesabımdan Onay Yapınız.";
                TempData["MesajTip"] = "warning"; // success info warning danger
            }
            List<ilan> ilanlarim = ctx.ilan.Where(x => x.kullanici_id == kullanici.id && x.AcikArtirma != null).ToList();
            List<AcikArtirma> model = new List<AcikArtirma>(); // açık artırmaları view'e gönderiyoruz.
            foreach (ilan item in ilanlarim)
            {
                model.Add(item.AcikArtirma);
            }
            ViewBag.Baslik = "Hesabım";
            ViewBag.Title = kullanici.ad + " " + kullanici.soyad;
            return View(model);
        }

        [HttpGet]
        public ActionResult İlanEkle()
        {
            Kullanici oturum = (Kullanici)Session["Kullanici"];
            if (!(bool)oturum.onaylimi)
            {
                TempData["Mesaj"] = "Hesap Onaylı Olmadığından İlan Eklenemez. Lütfen Hesabımdan Telefon Onayı Yapınız.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Index");
            }
            ViewBag.Kategoriler = new SelectList(ctx.Kategori.ToList(), "id", "ad");
            ViewBag.İller = new SelectList(Adres.iller, "id", "Ad");
            ViewBag.İlçeler = new SelectList(new List<SelectListItem>{new SelectListItem { Text = "İlçe Seçiniz", Value ="0"}}, "Value", "Text");
            ViewBag.Mahalleler = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "Mahalle Seçiniz", Value = "0" } }, "Value", "Text");

            ViewBag.Baslik = "İlanEkle";
            ViewBag.Title = "İlan Ekle";
            return View();
        }
        [HttpPost]
        public ActionResult İlanEkle(ilan ilan, IEnumerable<HttpPostedFileBase> gelenResim)
        {
            if (!ModelState.IsValid)
            {
                return İlanEkle();
            }
            if(ilan.baslik == "" || ilan.aciklama == "" || ilan.sorunu == "")
            {
                TempData["Mesaj"] = "Boş Bırakılan Yerleri Doldurunuz";
                TempData["MesajTip"] = "danger"; // success info warning danger
                return İlanEkle(); // get olan kısım çalışsın aynı sayfaya gitsin giderken de viewbag ile istenilen listeleri versin
            }

            bool hicresimyok = true;
            foreach (HttpPostedFileBase resim in gelenResim)
            {
                if(resim != null)
                {
                    hicresimyok = false;
                    break;
                }
            }

            if (hicresimyok)
            {
                TempData["Mesaj"] = "En Az 1 Resim Olmalı";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return İlanEkle(); // get olan kısım çalışsın aynı sayfaya gitsin giderken de viewbag ile istenilen listeleri versin
            }

            ilan.id = Guid.NewGuid();
            ilan.ilanacikmi = true;
            ilan.ilantarih = DateTime.Now;
            ilan.satildimi = false;
            ilan.kullanici_id =((Kullanici) Session["Kullanici"]).id;
            ilan.konum = null; // yapılabilir.

            int resim_sirasi= 1;
            foreach (HttpPostedFileBase resim in gelenResim)
            {
                if (resim != null)
                {
                    ilanResim ir = new ilanResim();
                    ir.ad = ResimIslem.Ekle(resim, ResimIslemTip.İlan);
                    if (ir.ad == "uzanti")
                    {
                        TempData["Mesaj"] = "Resimin Uzantisi Farklı";
                        TempData["MesajTip"] = "danger"; // success info warning danger
                        return İlanEkle(); // get olan kısım çalışsın aynı sayfaya gitsin giderken de viewbag ile istenilen listeleri versin
                    }
                    else if (ir.ad == "boyut")
                    {
                        TempData["Mesaj"] = "Resimin Boyutu Fazla";
                        TempData["MesajTip"] = "danger"; // success info warning danger
                        return İlanEkle();// get olan kısım çalışsın aynı sayfaya gitsin giderken de viewbag ile istenilen listeleri versin
                    }
                    ir.id = Guid.NewGuid();
                    ir.ilan_id = ilan.id;
                    ir.sirasi = resim_sirasi;
                    ctx.ilanResim.Add(ir);
                }
                resim_sirasi++;
            }
            ctx.ilan.Add(ilan);
            ctx.SaveChanges();
            return RedirectToAction("ilanlar");
        }

        [HttpGet]
        public ActionResult ilanlar() // ilanlarım 
        {
            Kullanici oturum = (Kullanici)Session["Kullanici"];
            List<ilan> ilanlari = ctx.ilan.Where(x => x.kullanici_id == oturum.id).ToList();

            ViewBag.Baslik = "İlanlarım";
            ViewBag.Title = "İlanlarım";
            return View(ilanlari);
        }

        [HttpGet]
        public ActionResult İlanDetay(string id)
        {
            Guid kullanici_id = ((Kullanici)Session["Kullanici"]).id;
            ilan model = ctx.ilan.FirstOrDefault(x => x.id == new Guid(id) && x.kullanici_id == kullanici_id);
            if (model == null)
            {
                TempData["Mesaj"] = "Belirtilen İlan Bulunamadı";
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Index");
            }
            ViewBag.Baslik = "Diğer";
            ViewBag.Title = "İlanim - "+ model.baslik;
            return View(model);
        }
        [HttpGet]
        public ActionResult İlanSil(string id)
        {

            //ÖNCESİNDE BAĞLI OLDUĞU HER ŞEYİ SİLMELİSİN !!!!
            Guid kullanici_id = ((Kullanici)Session["Kullanici"]).id;
            ilan ilanim = ctx.ilan.FirstOrDefault(x => x.id == new Guid(id) && x.kullanici_id == kullanici_id);
            if (ilanim == null)
            {
                TempData["Mesaj"] = "Belirtilen İlan Bulunamadı";
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Index");
            }

            List<ilanMesaj> ilanMesajs = ctx.ilanMesaj.Where(x => x.ilan_id == ilanim.id).ToList();
            if (ilanMesajs != null)
            {
                foreach (ilanMesaj item in ilanMesajs)
                {
                    List<Mesaj> mesajs = ctx.Mesaj.Where(x => x.ilanmesaj_id == item.id).ToList();
                    if (mesajs != null)
                    {
                        foreach (Mesaj item2 in mesajs)
                        {
                            ctx.Mesaj.Remove(item2);//Mesaj
                        }
                    }
                    ctx.ilanMesaj.Remove(item);//ilanMesaj 
                }
            }

            List<Favoriilan> favoriilans = ctx.Favoriilan.Where(x => x.ilan_id == ilanim.id).ToList();
            if (favoriilans != null)
            {
                foreach (Favoriilan item in favoriilans)
                {
                    ctx.Favoriilan.Remove(item);//Favori
                }
            }

            AcikArtirma acikArtirma = ilanim.AcikArtirma;
            if (acikArtirma != null)
            {
                List<Teklif> teklifs = ctx.Teklif.Where(x => x.acikartirma_id == acikArtirma.id).ToList();
                if (teklifs != null)
                {
                    foreach (Teklif item in teklifs)
                    {
                        ctx.Teklif.Remove(item);//Teklif
                    }
                }
                ctx.AcikArtirma.Remove(acikArtirma);//AcikArtirma
            }

            List<OneCikar> oneCikars = ctx.OneCikar.Where(x => x.ilan_id == ilanim.id).ToList();
            if (oneCikars != null)
            {
                foreach (OneCikar item in oneCikars)
                {
                    ctx.OneCikar.Remove(item);//ÖneCikarma
                }
            }

            List<ilanResim> ilanResims = ctx.ilanResim.Where(x => x.ilan_id == ilanim.id).ToList();
            foreach (ilanResim item in ilanResims)
            {
                ResimIslem.Sil(item.ad, ResimIslemTip.İlan);//ilanResim
                ctx.ilanResim.Remove(item);//ilanResim
            }
            ctx.ilan.Remove(ilanim);
            ctx.SaveChanges();
            TempData["Mesaj"] = "İlan Silindi";
            TempData["MesajTip"] = "info"; // success info warning danger
            return RedirectToAction("ilanlar");
        }
        [HttpPost]
        public ActionResult İlanSatildi(string id) // ilan_id
        {
            Kullanici oturum = (Kullanici)Session["Kullanici"];

            ilan ilanim = ctx.ilan.FirstOrDefault(x => x.id == new Guid(id));
            if (ilanim == null)
            {
                return RedirectToAction("Index");
            }


            //acik artirmalari sil.
            if (ilanim.AcikArtirma != null)
            {
                AcikArtirma acikArtirma = ilanim.AcikArtirma;
                foreach (Teklif item in acikArtirma.Teklif.ToList())
                {
                    ctx.Teklif.Remove(item);
                }
                ctx.AcikArtirma.Remove(acikArtirma);
            }

            //ilanı satıldı işaretle.
            ilanim.satildimi = true;
            ctx.SaveChanges();
            return RedirectToAction("ilanlar");
        }
        [HttpPost]
        public ActionResult İlanSatildiGeriAl(string id) // ilan_id
        {
            Kullanici oturum = (Kullanici)Session["Kullanici"];

            ilan ilanim = ctx.ilan.FirstOrDefault(x => x.id == new Guid(id));
            if (ilanim == null)
            {
                return RedirectToAction("Index");
            }
            //ilanı satılmadı işaretle.
            ilanim.satildimi = false;
            ctx.SaveChanges();
            return RedirectToAction("ilanlar");
        }
        
        [HttpGet]
        public ActionResult İlanıGizle(string id)
        {
            Guid kullanici_id = ((Kullanici)Session["Kullanici"]).id;
            ilan ilanim = ctx.ilan.FirstOrDefault(x => x.id == new Guid(id) && x.kullanici_id == kullanici_id);
            if (ilanim == null)
            {
                TempData["Mesaj"] = "Belirtilen İlan Bulunamadı.";
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Index");
            }
            ilanim.ilanacikmi = false;
            ctx.SaveChanges();
            return RedirectToAction("ilanlar");
        }
        [HttpGet]
        public ActionResult İlanıAç(string id)
        {
            Guid kullanici_id = ((Kullanici)Session["Kullanici"]).id;
            ilan ilanim = ctx.ilan.FirstOrDefault(x => x.id == new Guid(id) && x.kullanici_id == kullanici_id);
            if (ilanim == null)
            {
                TempData["Mesaj"] = "Belirtilen İlan Bulunamadı.";
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Index");
            }
            ilanim.ilanacikmi = true;
            ctx.SaveChanges();
            return RedirectToAction("ilanlar");
        }


        [HttpGet] 
        public ActionResult Favoriİlanlar() // favori ilanlarım
        {
            Guid kullanici_id = ((Kullanici)Session["Kullanici"]).id;
            List<Favoriilan> favoriler = ctx.Favoriilan.Where(x => x.kullanici_id == kullanici_id).ToList();
            
            List<ilan> favori_ilanlar = new List<ilan>();

            foreach (Favoriilan item in favoriler)
            {
                ilan ilan = ctx.ilan.FirstOrDefault(x => x.id == item.ilan_id);
                if (ilan != null)
                {
                    favori_ilanlar.Add(ilan);
                }
            }
            
            ViewBag.Baslik = "Favorilerim";
            ViewBag.Title = "Favoriler";
            return View(favori_ilanlar);
        }
        [HttpGet]
        public ActionResult FavoriSil(string id)
        {
            Guid kullanici_id = ((Kullanici)Session["Kullanici"]).id;

            Favoriilan fav = ctx.Favoriilan.FirstOrDefault(x => x.ilan_id == new Guid(id) && x.kullanici_id == kullanici_id);
            if (fav != null)
            {
                TempData["Mesaj"] = "Favori İlan Silindi";
                TempData["MesajTip"] = "success"; // success info warning danger
                ctx.Favoriilan.Remove(fav);
                ctx.SaveChanges();
            }
            else
            {
                TempData["Mesaj"] = "Favori İlan Bulunamadı";
                TempData["MesajTip"] = "danger"; // success info warning danger
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult FavoriEkle(string id)
        {
            Guid kullanici_id = ((Kullanici)Session["Kullanici"]).id;
            ilan ila = ctx.ilan.FirstOrDefault(x => x.id == new Guid(id));
            if (ila == null)
            {
                TempData["Mesaj"] = "İlan Bulunamadı";
                TempData["MesajTip"] = "danger"; // success info warning danger
            }
            else if (ila.kullanici_id == kullanici_id)
            {
                TempData["Mesaj"] = "Kendi İlanınızı Favori Ekleyemezsiniz";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Index");
            }
            else
            {
                Favoriilan fa = new Favoriilan();
                fa.id = Guid.NewGuid();
                fa.ilan_id = ila.id;
                fa.kullanici_id = kullanici_id;
                ctx.Favoriilan.Add(fa);
                ctx.SaveChanges();
                TempData["Mesaj"] = "İlan Favorilere Eklendi";
                TempData["MesajTip"] = "success"; // success info warning danger
            }
            return RedirectToAction("Index");
        }

        [HttpGet] // DENE
        public ActionResult Tekliflerim() // benim başkasına yaptığım açık artırmalar 
        {
            Guid kullanici_id = ((Kullanici)Session["Kullanici"]).id;

            List<Teklif> teklifList = ctx.Teklif.Where(x => x.alici_kullanici_id == kullanici_id).ToList();

            List<HesapTekliflerim> model = new List<HesapTekliflerim>(); // teklif ve sırasını tutacağız
            foreach (Teklif item in teklifList)
            {
                Guid acikAritrmaId = item.acikartirma_id;
                List<Teklif> oilanTeklifleri = ctx.Teklif.Where(x => x.acikartirma_id == acikAritrmaId).OrderByDescending(x=>x.teklif1).ToList();
                model.Add(new HesapTekliflerim() { teklif = item, sirasi= oilanTeklifleri.IndexOf(item)+1});
            }

            ViewBag.Baslik = "Tekliflerim";
            ViewBag.Title = "Tekliflerim";
            return View(model);
        }
        [HttpGet] // DENE
        public ActionResult TeklifSil(string id)
        {
            Guid kullanici_id = ((Kullanici)Session["Kullanici"]).id;
            Teklif teklifim = ctx.Teklif.FirstOrDefault(x => x.id == new Guid(id) && x.alici_kullanici_id == kullanici_id);
            if (teklifim == null)
            {
                TempData["Mesaj"] = "Öyle Bir Teklifiniz Bulunamadı.";
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Tekliflerim", "Hesap");
            }
            ctx.Teklif.Remove(teklifim);
            ctx.SaveChanges();
            TempData["Mesaj"] = "Teklif Silindi";
            TempData["MesajTip"] = "info"; // success info warning danger
            return RedirectToAction("Tekliflerim", "Hesap");
        }


        [HttpGet]
        public ActionResult KullaniciDuzenle() // ayarlarım 
        {
            Kullanici model = ((Kullanici)Session["Kullanici"]);
            //Resim ekle çıkart.
            ViewBag.Baslik = "Diğer";
            ViewBag.Title = "Kullanıcı Düzenle";
            return View(model);
        }
        [HttpPost]
        public ActionResult KullaniciDuzenle(Kullanici model, HttpPostedFileBase gelenResim) // ayarlarım 
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("KullaniciDuzenle");
            }
            if (model.id != ((Kullanici)Session["Kullanici"]).id)
            {
                TempData["Mesaj"] = "Başka Kullanıcının Bilgileri Değiştirilemez.";
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("KullaniciDuzenle");
            }
            Kullanici kullanici = ctx.Kullanici.FirstOrDefault(x => x.id == model.id);
            kullanici.ad = model.ad;
            kullanici.soyad = model.soyad;
            kullanici.sifre = model.sifre;
            kullanici.telefon = model.telefon;
            kullanici.mail = model.mail;

            if (gelenResim != null) // yeni resim gelmişse
            {
                KullaniciResim kr = kullanici.KullaniciResim;
                if (kr != null) // önceden resmi varsa sil
                {
                    string sonuc = ResimIslem.Sil(kr.ad, ResimIslemTip.Kullanici);
                    ctx.KullaniciResim.Remove(kr);
                }

                //yeni resmi ekle
                string sonuc2 = ResimIslem.Ekle(gelenResim, ResimIslemTip.Kullanici);
                if (sonuc2 == "uzanti")
                {
                    TempData["Mesaj"] = "Yüklenen Resmin Uzzantısı Farklı";
                    TempData["MesajTip"] = "danger"; // success info warning danger
                    return RedirectToAction("KullaniciDuzenle");

                }
                else if (sonuc2 == "boyut")
                {
                    TempData["Mesaj"] = "Yüklenen Resmin Boyutu Fazla";
                    TempData["MesajTip"] = "danger"; // success info warning danger
                    return RedirectToAction("KullaniciDuzenle");

                }
                KullaniciResim kResim = new KullaniciResim();
                kResim.id = Guid.NewGuid();
                kResim.ad = sonuc2;
                ctx.KullaniciResim.Add(kResim);
                kullanici.KullaniciResim = kResim;
            }
            ctx.SaveChanges();
            TempData["Mesaj"] = "Bilgiler Başarıyla Güncellendi";
            TempData["MesajTip"] = "success"; // success info warning danger
            return RedirectToAction("Index");
        }

 
        [HttpGet]
        public ActionResult ilanDuzenle(string id) // ilanDuzenle 
        {
            ilan ilan = ctx.ilan.FirstOrDefault(x => x.id == new Guid(id));
            if (ilan == null)
            {
                TempData["Mesaj"] = "İlan Bulunamadı";
                TempData["MesajTip"] = "danger"; // success info warning danger
                return View();
            }
            ViewBag.Kategoriler = new SelectList(ctx.Kategori.ToList(), "id", "ad");
            ViewBag.İller = new SelectList(Adres.iller, "id", "Ad");
            ViewBag.İlçeler = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "İlçe Seçiniz", Value = "0" } }, "Value", "Text");
            ViewBag.Mahalleler = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "Mahalle Seçiniz", Value = "0" } }, "Value", "Text");
            ViewBag.Baslik = "Diğer";
            ViewBag.Title = "İlan Düzenle";
            return View(ilan);
        }
        [HttpPost]
        public ActionResult ilanDuzenle(ilan model, IEnumerable<HttpPostedFileBase> gelenResim) // ilanDuzenle 
        {
            if (!ModelState.IsValid)
            {
                return ilanDuzenle(model.id.ToString());
            }
            ilan ilan = ctx.ilan.FirstOrDefault(x => x.id == model.id);
            if (ilan == null)
            {
                TempData["Mesaj"] = "İlan Bulunamadı";
                TempData["MesajTip"] = "danger"; // success info warning danger
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
                                TempData["Mesaj"] = "Eski Resim Silinemedi // Bulunamadı";
                                TempData["MesajTip"] = "danger"; // success info warning danger
                                return ilanDuzenle(model.id.ToString());
                            }
                        }
                        // resim ekleme
                        ilanResim ir = new ilanResim();
                        ir.ad = ResimIslem.Ekle(resim, ResimIslemTip.İlan);
                        if (ir.ad == "uzanti")
                        {
                            TempData["Mesaj"] = "Resimin Uzantisi Farklı";
                            TempData["MesajTip"] = "danger"; // success info warning danger
                            return ilanDuzenle(model.id.ToString()); // get olan kısım çalışsın aynı sayfaya gitsin giderken de viewbag ile istenilen listeleri versin
                        }
                        else if (ir.ad == "boyut")
                        {
                            TempData["Mesaj"] = "Resimin Boyutu Fazla";
                            TempData["MesajTip"] = "danger"; // success info warning danger
                            return ilanDuzenle(model.id.ToString());// get olan kısım çalışsın aynı sayfaya gitsin giderken de viewbag ile istenilen listeleri versin
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
            return RedirectToAction("ilanlar");
        }


        [HttpPost]
        public ActionResult AcikArtirmaEkle(string id,int gun) // id = ilan id
        {

            Guid kullanici_id = ((Kullanici)Session["Kullanici"]).id;

            ilan il = ctx.ilan.FirstOrDefault(x => x.kullanici_id == kullanici_id && x.id == new Guid(id)); // onun ilanı mı kontrol
            if (il == null)
            {
                TempData["Mesaj"] = "İlan Bulunamadı.";
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Index");
            }
            if (gun <= 0)
            {
                TempData["Mesaj"] = "Açık Artırma En Az 1 Gün Olur";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Index");
            }
            if ((bool)il.satildimi)
            {
                TempData["Mesaj"] = "Satılmış İlana Açık Artırma Uygulanamaz.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Index");
            }
            AcikArtirma acik = new AcikArtirma();
            acik.id = Guid.NewGuid();
            acik.ilan = new List<ilan>() { il };
            acik.olusturulmatarih = DateTime.Now;
            acik.bitistarih = DateTime.Now.AddDays(gun);
            ctx.AcikArtirma.Add(acik);
            ctx.SaveChanges();
            return RedirectToAction("ilanlar");
        }
        [HttpPost]
        public ActionResult AcikArtirmaSil(string id) // id = ilan id
        {
            Guid kullanici_id = ((Kullanici)Session["Kullanici"]).id;
            ilan il = ctx.ilan.FirstOrDefault(x => x.kullanici_id == kullanici_id && x.id == new Guid(id)); // onun ilanı mı kontrol
            if (il == null)
            {
                TempData["Mesaj"] = "İlan Bulunamadı.";
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Index");
            }
            AcikArtirma acik = il.AcikArtirma;
            List<Teklif> teklifler = ctx.Teklif.Where(x => x.acikartirma_id == acik.id).ToList();
            foreach (Teklif item in teklifler)
            {
                ctx.Teklif.Remove(item);
            }
            ctx.AcikArtirma.Remove(acik);
            ctx.SaveChanges();
            return RedirectToAction("ilanlar");
        }


        [HttpGet]
        public ActionResult Mesajlar() // mesajlarım 
        {
            Guid kullanici_id = ((Kullanici)Session["Kullanici"]).id;
            List<ilanMesaj> model = ctx.ilanMesaj.Where(x => x.alici_kullanici_id == kullanici_id || x.satici_kullanici_id == kullanici_id).ToList(); // alici veya satici oldugu mesajlari tutan ilanMesaj listesini getir.
            ViewBag.Baslik = "Mesajlar";
            ViewBag.Title = "Mesajlar";
            return View(model);
        }
        [HttpPost]
        public ActionResult MesajSil(string ilanMesaj_id) // Kişiyle Olan o ilan için tüm konuşma silinir.
        {
            ilanMesaj im = ctx.ilanMesaj.FirstOrDefault(x => x.id == new Guid(ilanMesaj_id));
            if (im == null)
            {
                TempData["Mesaj"] = "Mesaj Bulunamadı.";
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Index");
            }
            foreach (Mesaj item in im.Mesaj.ToList())
            {
                ctx.Mesaj.Remove(item);
            }
            ctx.ilanMesaj.Remove(im);
            ctx.SaveChanges();
            return RedirectToAction("Mesajlar");
        }
        [HttpPost]
        public ActionResult MesajAt(string ilanMesaj_id,string mesaj) // id = ilanMesaj_id , string mesaj // Hesap/MEsaj'dan, zaten var olan ilanMesaj'a, ALICI VEYA SATICI mesajı atma
        {
            Kullanici oturum = (Kullanici)Session["Kullanici"];
            ilanMesaj im = ctx.ilanMesaj.FirstOrDefault(x => x.id == new Guid(ilanMesaj_id));
            if (im == null)
            {
                TempData["Mesaj"] = "Mesaj Bulunamadı.";
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Index");
            }

                Mesaj m = new Mesaj();
                m.id = Guid.NewGuid();
                m.gordumu = false;
                m.ilanMesaj = im;
                m.mesaj1 = mesaj;
                m.mesajalicininmi = im.alici_kullanici_id == oturum.id; // alıcıysa true , değilse false(satıcı)
                m.tarih = DateTime.Now;
            ctx.Mesaj.Add(m);
            ctx.SaveChanges();
            return RedirectToAction("Mesajlar");
        }
        [HttpPost]
        public ActionResult AcikArtirmaKazananaMesajAt(string ilan_id,string kazanan_id)
        {
            ilan ilanim = ctx.ilan.FirstOrDefault(x => x.id == new Guid(ilan_id));
            Kullanici oturum = (Kullanici)Session["Kullanici"];
            Kullanici kazanan = ctx.Kullanici.FirstOrDefault(x => x.id == new Guid(kazanan_id));

            ilanMesaj im = ctx.ilanMesaj.FirstOrDefault(x => x.alici_kullanici_id == kazanan.id && x.satici_kullanici_id == oturum.id && x.ilan_id == ilanim.id);
            if (im == null)
            {
                im = new ilanMesaj();
                im.id = Guid.NewGuid();
                im.ilan_id = ilanim.id;
                im.alici_kullanici_id = kazanan.id;
                im.satici_kullanici_id = oturum.id;
                ctx.ilanMesaj.Add(im);
            }

            Mesaj m = new Mesaj();
            m.id = Guid.NewGuid();
            m.gordumu = false;
            m.ilanMesaj = im;
            m.mesaj1 = "Merhaba, "+kazanan.ad+" "+kazanan.soyad+". \""+ ilanim.baslik + "\"  İlanının Açık Artırmasını Kazandınız. İletişime Geçebilir Miyiz?";
            m.mesajalicininmi = false;
            m.tarih = DateTime.Now;
            ctx.Mesaj.Add(m);
            ctx.SaveChanges();
            return RedirectToAction("Mesajlar");
        }
        [HttpPost]
        public ActionResult AcikArtirmaKazandimMesajAt(string ilan_id)
        {
            ilan ilani = ctx.ilan.FirstOrDefault(x => x.id == new Guid(ilan_id));
            Kullanici oturum = (Kullanici)Session["Kullanici"];
            Kullanici ilanSahibi = ilani.Kullanici;

            ilanMesaj im = ctx.ilanMesaj.FirstOrDefault(x => x.alici_kullanici_id == oturum.id && x.satici_kullanici_id == ilanSahibi.id && x.ilan_id == ilani.id);
            if (im == null)
            {
                im = new ilanMesaj();
                im.id = Guid.NewGuid();
                im.ilan_id = ilani.id;
                im.alici_kullanici_id = oturum.id;
                im.satici_kullanici_id = ilanSahibi.id;
                ctx.ilanMesaj.Add(im);
            }

            Mesaj m = new Mesaj();
            m.id = Guid.NewGuid();
            m.gordumu = false;
            m.ilanMesaj = im;
            m.mesaj1 = "Merhaba, " + ilanSahibi.ad + " " + ilanSahibi.soyad + ". \"" + ilani.baslik + "\"  İlanının Açık Artırmasını Kazandım. İletişime Geçebilir Miyiz?";
            m.mesajalicininmi = true;
            m.tarih = DateTime.Now;
            ctx.Mesaj.Add(m);
            ctx.SaveChanges();
            return RedirectToAction("Mesajlar");
        }


        [HttpPost]
        public ActionResult HesapOnay(string telefon_onay)
        {
            Kullanici oturum = (Kullanici)Session["Kullanici"];
            if (oturum.telefon == telefon_onay)
            {
                TempData["Mesaj"] = "Hesap Onaylandı";
                TempData["MesajTip"] = "success"; // success info warning danger
                oturum.onaylimi = true;
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Mesaj"] = "Onay Kodu Yanlış";
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Index");
            }

        }

        [HttpGet]
        public ActionResult ilanOneCikar(string id) // ilan sayfasından buna gelebilsin.
        {
            Guid kullanici_id = ((Kullanici)Session["Kullanici"]).id;

            ilan il = ctx.ilan.FirstOrDefault(x => x.kullanici_id == kullanici_id && x.id == new Guid(id)); // onun ilanı mı kontrol
            if (il == null)
            {
                TempData["Mesaj"] = "İlan Bulunamadı.";
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Index");
            }

            ViewBag.Baslik = "Diğer";
            ViewBag.Title = "Öne Çıkar";
            return View(il);
        }
        [HttpPost]
        public ActionResult ilanOneCikar(string ilan_id, OneCikarPaket secilen) 
        {
            Guid kullanici_id = ((Kullanici)Session["Kullanici"]).id;

            ilan il = ctx.ilan.FirstOrDefault(x => x.kullanici_id == kullanici_id && x.id == new Guid(ilan_id)); // onun ilanı mı kontrol
            if (il == null)
            {
                TempData["Mesaj"] = "İlan Bulunamadı.";
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Index");
            }
            OneCikar oneCikar = new OneCikar();
            oneCikar.id = Guid.NewGuid();
            switch (secilen.secilenPaket)
            {
                case 0:
                    TempData["Mesaj"] = "Uygun Öne Çıkarma Paketi Seçilemedi.";
                    TempData["MesajTip"] = "warning"; // success info warning danger
                    return RedirectToAction("Index");
                case 1:
                    TempData["Mesaj"] = "\""+il.baslik+"\" ilanı için 1 Haftalık Öne Çıkarma Uygulandı. 10 TL Ücret Kesimi Yapıldı.";
                    TempData["MesajTip"] = "success"; // success info warning danger
                    oneCikar.bitistarih = DateTime.Now.AddDays(7);
                    break;
                case 2:
                    TempData["Mesaj"] = "\"" + il.baslik + "\" ilanı için 1 Aylık Öne Çıkarma Uygulandı. 30 TL Ücret Kesimi Yapıldı.";
                    TempData["MesajTip"] = "success"; // success info warning danger
                    oneCikar.bitistarih = DateTime.Now.AddMonths(1);
                    break;
                case 3:
                    TempData["Mesaj"] = "\"" + il.baslik + "\" ilanı için 3 Aylık Öne Çıkarma Uygulandı. 70 TL Ücret Kesimi Yapıldı.";
                    TempData["MesajTip"] = "success"; // success info warning danger
                    oneCikar.bitistarih = DateTime.Now.AddMonths(3);
                    break;
                case 4:
                    TempData["Mesaj"] = "\"" + il.baslik + "\" ilanı için 1 Yıllık Öne Çıkarma Uygulandı. 240 TL Ücret Kesimi Yapıldı.";
                    TempData["MesajTip"] = "success"; // success info warning danger
                    oneCikar.bitistarih = DateTime.Now.AddYears(1);
                    break;
            }
            oneCikar.ilan_id = il.id;
            ctx.OneCikar.Add(oneCikar);
            ctx.SaveChanges();
            return RedirectToAction("Index"); ;
        }

        [HttpPost]
        public ActionResult onayIstek()
        {
            Kullanici kullanici = (Kullanici)Session["Kullanici"];

            if ((bool)kullanici.onaylimi)
            {
                TempData["Mesaj"] = "Kullanıcı Zaten Onaylı.";
                TempData["MesajTip"] = "info"; // success info warning danger
                return RedirectToAction("Index");
            }

            if (HttpContext.Cache["onayList"] == null)
            {
                HttpContext.Cache.Add(
                                        key: "onayList",
                                        value: new Dictionary<Kullanici, Guid>(),
                                        dependencies: null,
                                        absoluteExpiration: System.Web.Caching.Cache.NoAbsoluteExpiration,
                                        slidingExpiration: new TimeSpan(0,1,0), // 1 dk sonra öl
                                        priority: System.Web.Caching.CacheItemPriority.Low,
                                        onRemoveCallback: null
                                );
            }

            Dictionary<Kullanici, Guid> onayList = (Dictionary<Kullanici,Guid>)HttpContext.Cache["onayList"];
            Guid listedeVarMi = onayList[kullanici];

            if(listedeVarMi != null)
            {
                TempData["Mesaj"] = "Onay Kodu Mailinize Zaten Gönderilmiştir.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Index");
            }

            //KeyValuePair<Kullanici, Guid> sonuc = MailAt.onay(kullanici);
            // onayList.Add(sonuc.Key, sonuc.Value);

            TempData["Mesaj"] = "Onay Kodu Mailinize Gönderilmiştir.";
            TempData["MesajTip"] = "info"; // success info warning danger
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult onay(string id)
        {

            Guid guid = new Guid(id);

            if (HttpContext.Cache["onayList"] != null)
            {
                Dictionary<Kullanici, Guid> onayList = (Dictionary<Kullanici, Guid>)HttpContext.Cache["onayList"];

                KeyValuePair<Kullanici, Guid> sonuc = onayList.FirstOrDefault(x => x.Value == guid);
                if (sonuc.Key == null)
                {
                    Kullanici onaylanacakKullanici = sonuc.Key;
                    onaylanacakKullanici.onaylimi = true;
                    ctx.SaveChanges();

                    TempData["Mesaj"] = "Kullanıcı Onaylandı.";
                    TempData["MesajTip"] = "success"; // success info warning danger
                    Session["Kullanici"] = onaylanacakKullanici;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Mesaj"] = "Onay İsteği Zaman Aşımına Uğradı. Hesabınızdan Tekrar Onay İsteği Yapınız.";
                    TempData["MesajTip"] = "warning"; // success info warning danger
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Mesaj"] = "Onay İsteği Zaman Aşımına Uğradı. Hesabınızdan Tekrar Onay İsteği Yapınız.";
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Index");
            }
        }

        ///////////////////////////////////////////////////// JSON RESULTS

        public JsonResult Get_ilce(int id)
        {

            List<İlçe> result = Adres.ilçeler.Where(x => x.il_id== id).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Get_Mahalle(int id)
        {

            List<Mahalle> result = Adres.mahalleler.Where(x => x.ilce_id == id).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult MesajlariGordu() 
        {
            Kullanici kullanici = (Kullanici)Session["Kullanici"];
            foreach (ilanMesaj item1 in ctx.ilanMesaj.Where(x=> x.satici_kullanici_id == kullanici.id).ToList()) // satıcı olduğu yerleri getir
            {
                foreach (Mesaj item2 in ctx.Mesaj.Where(x=>x.ilanmesaj_id == item1.id).ToList())
                {
                    if ((bool)item2.mesajalicininmi) // mesaj alıcının değil satıcınınsa
                    {
                        item2.gordumu = true;
                    }
                }
            }
            foreach (ilanMesaj item1 in ctx.ilanMesaj.Where(x => x.alici_kullanici_id == kullanici.id)) // alıcı olduğu mesajları getir
            {
                foreach (Mesaj item2 in ctx.Mesaj.Where(x => x.ilanmesaj_id == item1.id).ToList())
                {
                    if (!(bool)item2.mesajalicininmi) // mesaj alıcıya yani buna aitse
                    {
                        item2.gordumu = true;
                    }
                }
            }
            ctx.SaveChanges();
            string result = "Mesajlar Silindi";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}