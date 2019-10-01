using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlAbiVerAbi
{
    public class _SecurityFilter : ActionFilterAttribute
    {
        /*
         Global asax'a bunu ekle :

            GlobalFilters.Filters.Add(new _SecurityFilter());

             */
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Context ctx = new Context();
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            Kullanici kullanici = (Kullanici)  HttpContext.Current.Session["Kullanici"];
            string kullaniciRolu;
            if (kullanici != null)
                kullaniciRolu = kullanici.Rol.adi;
            else
                kullaniciRolu = "";

            if (controllerName == "Admin")
            {
                if (kullaniciRolu != "Admin")
                {
                    filterContext.Result = new RedirectResult("/Login/Giris"); // admin paneline girmeye çalışıp admin değilse login sayfasına at
                    return;
                }
            }
            else if (controllerName == "Hesap")
            {
                if (!(kullaniciRolu == "Kullanici" || kullaniciRolu == "Admin"))
                {
                    filterContext.Result = new RedirectResult("/Login/Giris"); // hesap sayfasına girmeye çalışıp admin veya kullanici değilse login sayfasına at
                    return;
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}