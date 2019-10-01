using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace AlAbiVerAbi
{
    public static class MailAt
    {
        private const string MAIL = "erdemsiyam@gmail.com";
        private const string PASS = "komut81ai";

        private static bool mailGonder(string hedefMailAdres, string icerik , string baslik)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.To.Add(new MailAddress(hedefMailAdres));
                message.From = new MailAddress(MAIL); 
                message.Subject = baslik;
                message.Body = icerik;
                message.IsBodyHtml = true;
                NetworkCredential credential = new NetworkCredential()
                    {
                        UserName = MAIL,
                        Password = PASS
                    };
                SmtpClient smtp = new SmtpClient()
                {
                        Credentials = credential,
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true
                };
                smtp.Send(message);
                return true;
            }
            catch (Exception e)
            {
                int x = 1;
                return false;
            }
        }

        public static CacheOnayItem onay(Kullanici k)
        {
            string domainName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            Guid guid = Guid.NewGuid();
            string link = domainName + "/Login/Onay/" + guid.ToString();
            string mesaj = "<p>Hesabınızın Onaylanması İçin <a target=\"_blank\" href=\"" + link + "\">Buraya Tıklayınız</a></p>";
            string baslik = "Al Abi Ver Abi (Onay Mail)";

            if (mailGonder(k.mail, mesaj, baslik))
            {
                return new CacheOnayItem() { kullanici = k, guid = guid };
            }
            else
            {
                return null;
            }
        }

        public static CacheResetItem sifreReset(Kullanici k)
        {
            string domainName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            Guid guid = Guid.NewGuid();
            string link = domainName + "/Login/SifreReset/" + guid.ToString();
            string mesaj = "<p>Hesabınızın Şifresini Değiştirmek İçin <a target=\"_blank\" href=\"" + link + "\">Buraya Tıklayınız</a></p>";
            string baslik = "Al Abi Ver Abi (Şifre Reset)";

            if (mailGonder(k.mail, mesaj, baslik))
            {
                return new CacheResetItem() { kullanici = k, guid = guid };
            }
            else
            {
                return null;
            }
        }

    }

}