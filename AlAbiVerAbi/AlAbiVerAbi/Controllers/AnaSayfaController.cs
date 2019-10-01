using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlAbiVerAbi.Controllers
{
    public class AnaSayfaController : Controller
    {
        Context ctx = new Context();
        [HttpGet]
        public ActionResult Index()
        {
            AnaSayfaIndexModel model = new AnaSayfaIndexModel();
            model.kategoriler = ctx.Kategori.ToList();
            model.arama = new Arama();
            /**/
            List<ilan> gonderilecekİlanlar = new List<ilan>();
            List<ilan> gelenİlanlar = ctx.ilan.Where(x => x.ilanacikmi == true && x.satildimi == false).OrderByDescending(x => x.ilantarih).ToList(); // öne çıkaranları daha önde getirt
            List<ilan> oneCikarilanlar = new List<ilan>();
            foreach (ilan item in gelenİlanlar)
            {
                if (item.OneCikar != null && item.OneCikar.Count >0 )
                {
                    if (item.OneCikar.FirstOrDefault().bitistarih > DateTime.Now)
                    {
                        oneCikarilanlar.Add(item);
                    }
                }
            }
            List<ilan> oneCikarilmayanlar = gelenİlanlar.Except(oneCikarilanlar).ToList(); // one çıkarılmayanlar = gelenList - one cıkarılanlar

            gonderilecekİlanlar.AddRange(oneCikarilanlar); // ilk öne çıkarılanlar alındı.
            gonderilecekİlanlar.AddRange(oneCikarilmayanlar); // sonra öne çıkarılmayanlar alındı.
            
            model.ilanlar = gonderilecekİlanlar;
            ViewBag.Kategori = "Genel";
            ViewBag.Title = "Ana Sayfa";
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(Arama arama)
        {
            if (arama == null)
            {
                TempData["Mesaj"] = "Arama Kriterleri Bulunamadı";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("Index", "AnaSayfa");
            }

            AnaSayfaIndexModel model = new AnaSayfaIndexModel();
            model.kategoriler = ctx.Kategori.ToList();
            model.arama = arama;

            List<ilan> gonderilecekİlanlar = new List<ilan>();
            List<ilan> gelenİlanlar = ctx.ilan.Where(x => x.ilanacikmi == true && x.satildimi == false).OrderByDescending(x => x.ilantarih).ToList(); // öne çıkaranları daha önde getirt

            if (arama.boolKategori)
            {
                Kategori kat = ctx.Kategori.FirstOrDefault(x => x.id == new Guid(arama.kategoriId));
                if (kat == null)
                {
                    TempData["Mesaj"] = "Seçilen Kategori Bulunamadı";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                    TempData["MesajTip"] = "warning"; // success info warning danger
                    return RedirectToAction("Index", "AnaSayfa");
                }
                gelenİlanlar = gelenİlanlar.Where(x => x.kategori_id == kat.id).ToList();
            }
            if (arama.boolKelimeAra)
            {
                if (arama.kelimeAra == null || arama.kelimeAra.Trim() == "")
                {
                    TempData["Mesaj"] = "Aranacak Kelime Boş Geçilemez";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                    TempData["MesajTip"] = "warning"; // success info warning danger
                    return RedirectToAction("Index", "AnaSayfa");
                }
                gelenİlanlar = gelenİlanlar.Where(x => x.baslik.Contains(arama.kelimeAra)).ToList();
            }
            if (arama.boolEnAzFiyat)
            {
                if (arama.enAzFiyat < 0)
                {
                    TempData["Mesaj"] = "En Az Fiyat Negatif Sayı Olamaz";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                    TempData["MesajTip"] = "danger"; // success info warning danger
                    return RedirectToAction("Index", "AnaSayfa");
                }
                gelenİlanlar = gelenİlanlar.Where(x => x.fiyat >= arama.enAzFiyat).ToList();
            }
            if (arama.boolEnFazlaFiyat)
            {
                if (arama.enfazlaFiyat < 0)
                {
                    TempData["Mesaj"] = "En Fazla Fiyat Negatif Sayı Olamaz";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                    TempData["MesajTip"] = "danger"; // success info warning danger
                    return RedirectToAction("Index", "AnaSayfa");
                }
                gelenİlanlar = gelenİlanlar.Where(x => x.fiyat <= arama.enfazlaFiyat).ToList();

            }
            if (arama.boolYayinTarihi)
            {
                switch (arama.yayinTarihi)
                {
                    case 1: // Bugün
                        gelenİlanlar = gelenİlanlar.Where(x => x.ilantarih.Value.Date.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy")).ToList();
                        break;
                    case 2:
                        gelenİlanlar = gelenİlanlar.Where(x => x.ilantarih > DateTime.Now.AddDays(-7)).ToList();
                        break;
                    case 3:
                        gelenİlanlar = gelenİlanlar.Where(x => x.ilantarih > DateTime.Now.AddMonths(-1)).ToList();
                        break;
                    case 4:
                        gelenİlanlar = gelenİlanlar.Where(x => x.ilantarih > DateTime.Now.AddYears(-1)).ToList();
                        break;
                }

            }

             List<ilan> oneCikarilanlar = new List<ilan>();
            foreach (ilan item in gelenİlanlar)
            {
                if (item.OneCikar != null && item.OneCikar.Count > 0)
                {
                    if (item.OneCikar.FirstOrDefault().bitistarih > DateTime.Now)
                    {
                        oneCikarilanlar.Add(item);
                    }
                }
            }
            List<ilan> oneCikarilmayanlar = gelenİlanlar.Except(oneCikarilanlar).ToList(); // one çıkarılmayanlar = gelenList - one cıkarılanlar

            gonderilecekİlanlar.AddRange(oneCikarilanlar); // ilk öne çıkarılanlar alındı.
            gonderilecekİlanlar.AddRange(oneCikarilmayanlar); // sonra öne çıkarılmayanlar alındı.

            if (arama.boolSiralama)
            {
                switch (arama.siralama)
                {
                    case 1: // Bugün
                        gonderilecekİlanlar = gonderilecekİlanlar.OrderByDescending(x=>x.ilantarih).ToList();
                        break;
                    case 2:
                        gonderilecekİlanlar = gonderilecekİlanlar.OrderBy(x=>x.fiyat).ToList();
                        break;
                    case 3:
                        gonderilecekİlanlar = gonderilecekİlanlar.OrderByDescending(x => x.fiyat).ToList();
                        break;
                }
            }
            model.ilanlar = gonderilecekİlanlar;

            ViewBag.Kategori = "Genel";
            ViewBag.Title = "Ana Sayfa";
            return View(model);
        }
        [HttpGet]
        public ActionResult KategoriGetir(string id) // o kategori ile ilgili ilanlar gelir
        {
            if (id == null || id.Trim() == "")
            {
                TempData["Mesaj"] = "Kategori Bulunamadı";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Index", "AnaSayfa");
            }
            Kategori kategori = ctx.Kategori.FirstOrDefault(x => x.id == new Guid(id));
            if (kategori == null)
            {
                TempData["Mesaj"] = "Kategori Bulunamadı";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Index", "AnaSayfa");
            }

            AnaSayfaIndexModel model = new AnaSayfaIndexModel();
            model.kategoriler = ctx.Kategori.ToList();
            model.arama = new Arama();
            /**/
            List<ilan> gonderilecekİlanlar = new List<ilan>();
            List<ilan> gelenİlanlar = ctx.ilan.Where(x => x.ilanacikmi == true && x.satildimi == false && x.kategori_id == kategori.id).OrderByDescending(x => x.ilantarih).ToList(); // öne çıkaranları daha önde getirt
            List<ilan> oneCikarilanlar = new List<ilan>();
            foreach (ilan item in gelenİlanlar)
            {
                if (item.OneCikar != null && item.OneCikar.Count > 0)
                {
                    if (item.OneCikar.FirstOrDefault().bitistarih > DateTime.Now)
                    {
                        oneCikarilanlar.Add(item);
                    }
                }
            }
            List<ilan> oneCikarilmayanlar = gelenİlanlar.Except(oneCikarilanlar).ToList(); // one çıkarılmayanlar = gelenList - one cıkarılanlar

            gonderilecekİlanlar.AddRange(oneCikarilanlar); // ilk öne çıkarılanlar alındı.
            gonderilecekİlanlar.AddRange(oneCikarilmayanlar); // sonra öne çıkarılmayanlar alındı.

            model.ilanlar = gonderilecekİlanlar;
            ViewBag.Kategori = kategori.ad;
            ViewBag.Title = kategori.ad;
            return View(model);
            //Redirect To Action ' ile index metodu çalıştırmış oluyor. oyüzden kendisinin .cshtml'ini oluşturduk
        }
        [HttpGet]
        public ActionResult İlanDetay(string id)
        {

            ilan model = ctx.ilan.FirstOrDefault(x => x.id == new Guid(id));
            if (model == null)
            {
                TempData["Mesaj"] = "İlan Bulunamadı";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Index");
            }
            ViewBag.Kategori = "Genel";
            ViewBag.Title = model.baslik;
            return View(model);
        }
        [HttpPost]
        public ActionResult TeklifYap(string ilanid,int tekliffiyat)
        {
            ilan il = ctx.ilan.FirstOrDefault(x => x.id == new Guid(ilanid));
            if (il == null)
            {
                TempData["Mesaj"] = "İlan Bulunamadı";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Index");
            }
            else if (il.AcikArtirma == null)
            {
                TempData["Mesaj"] = "İlanın Açık Artırması Bulunamadı";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("İlanDetay", il); // yanında ilan modeli verilir.
            }

            if (tekliffiyat <= 0)
            {
                TempData["Mesaj"] = "Teklif Fiyatı Sıfırdan Büyük Olmalı";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("İlanDetay",il); // yanında ilan modeli verilir.
            }
            Kullanici oturum = ((Kullanici)Session["Kullanici"]);
            if (oturum == null)
            {
                TempData["Mesaj"] = "Önce Giriş Yapınız";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("İlanDetay", il); // yanında ilan modeli verilir.
            }
            if (oturum.id == il.kullanici_id)
            {
                TempData["Mesaj"] = "Kendi İlanınıza Teklif Yapamazsınız.";
                TempData["MesajTip"] = "warning";// success info warning danger
                return RedirectToAction("İlanDetay", il); // yanında ilan modeli verilir.
            }

            Teklif oncedenVarmi = ctx.Teklif.FirstOrDefault(x => x.alici_kullanici_id == oturum.id && x.acikartirma_id == il.acikartirma_id);

            if (oncedenVarmi != null)
            {
                oncedenVarmi.teklif1 = tekliffiyat;
                oncedenVarmi.tekliftarih = DateTime.Now;
                ctx.SaveChanges();
            }
            else
            {
                Teklif teklif = new Teklif();
                teklif.id = Guid.NewGuid();
                teklif.alici_kullanici_id = ((Kullanici)Session["Kullanici"]).id;
                teklif.tekliftarih = DateTime.Now;
                teklif.AcikArtirma = il.AcikArtirma;
                teklif.teklif1 = tekliffiyat;
                ctx.Teklif.Add(teklif);
                ctx.SaveChanges();
            }

            TempData["Mesaj"] = "Açık Artırmaya Teklif Fiyatınız Eklendi.";
            TempData["MesajTip"] = "success";// success info warning danger
            return RedirectToAction("İlanDetay", il); // yanında ilan modeli verilir.

        }
        [HttpPost]
        public ActionResult İlanaMesajAt(string ilanid, string mesaj) // Alıcı Olarak Mesaj At (İLAN DETAYDAN)
        {
            ilan il = ctx.ilan.FirstOrDefault(x => x.id == new Guid(ilanid));
            if (il == null)
            {
                TempData["Mesaj"] = "İlan Bulunamadı";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Index");
            }
            Kullanici oturum = ((Kullanici)Session["Kullanici"]);
            if (oturum == null)
            {
                TempData["Mesaj"] = "Önce Giriş Yapınız";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("İlanDetay", il); // yanında ilan modeli verilir.
            }
            if (oturum.id == il.kullanici_id)
            {
                TempData["Mesaj"] = "Kendi İlanınıza Mesaj Atamazsınız.";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("İlanDetay", il); // yanında ilan modeli verilir.
            }
            if (mesaj.Trim() == "")
            {
                TempData["Mesaj"] = "Mesaj İçeriğini Boş Geçmeyiniz";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "warning"; // success info warning danger
                return RedirectToAction("İlanDetay", il); // yanında ilan modeli verilir.
            }
            ilanMesaj im = ctx.ilanMesaj.FirstOrDefault(x => x.ilan_id == il.id && x.alici_kullanici_id == oturum.id);
            if (im != null) // null değilse zamanında konuşuldu demektir.
            {
                Mesaj me = new Mesaj();
                me.id = Guid.NewGuid();
                me.gordumu = false;
                me.ilanmesaj_id = im.id;
                me.mesaj1 = mesaj;
                me.mesajalicininmi = true;
                me.tarih = DateTime.Now;
                ctx.Mesaj.Add(me);
                ctx.SaveChanges();
            }
            else // null sa önceden konuşulmamış ozaman yeni ilanMesaj açarız
            {
                ilanMesaj im2 = new ilanMesaj();
                im2.id = Guid.NewGuid();
                im2.ilan_id = il.id;
                im2.satici_kullanici_id = il.kullanici_id;
                im2.alici_kullanici_id = oturum.id;
                ctx.ilanMesaj.Add(im2);
                Mesaj me = new Mesaj();
                me.id = Guid.NewGuid();
                me.gordumu = false;
                me.ilanmesaj_id = im2.id;
                me.mesaj1 = mesaj;
                me.mesajalicininmi = true;
                me.tarih = DateTime.Now;
                ctx.Mesaj.Add(me);
                ctx.SaveChanges();
            }
            TempData["Mesaj"] = "Mesaj Gönderildi.";// redirect yapılan sayfa için temp data ile hata mesajı atılır
            TempData["MesajTip"] = "success"; // success info warning danger
            return RedirectToAction("Mesajlar","Hesap");
        }
        [HttpGet]
        public ActionResult Kullaniciİlanlari(string id) // kullanıcı id
        {
            Kullanici kul = ctx.Kullanici.FirstOrDefault(x => x.id == new Guid(id));
            if (kul == null)
            {
                TempData["Mesaj"] = "Kullanıcı Bulunamadı.";// redirect yapılan sayfa için temp data ile hata mesajı atılır
                TempData["MesajTip"] = "danger"; // success info warning danger
                return RedirectToAction("Index", "AnaSayfa");
            }
            List<ilan> ilanlari = ctx.ilan.Where(x => x.kullanici_id == kul.id).ToList();
            AnaSayfaKullaniciİlanlari model = new AnaSayfaKullaniciİlanlari();
            model.kullanici = kul;
            model.ilanlari = ilanlari;
            ViewBag.Kategori = "Genel";
            ViewBag.Title = kul.ad + " " + kul.soyad;
            return View(model);
        }
    }
}