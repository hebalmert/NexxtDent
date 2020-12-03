using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using NexxtDent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NexxtDent.Controllers
{
    public class HomeController : Controller
    {
        private readonly NexxtDentContext db = new NexxtDentContext();

        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (user != null)
            {
                var db2 = new NexxtDentContext();
                var companyUp = db2.Companies.Find(user.CompanyId);
                bool comActivo = companyUp.Activo;
                db2.Dispose();

                DateTime hoy = DateTime.Today;
                DateTime current = companyUp.DateHasta;
                if (comActivo == false)   //Verificacion de la Comañia si esta activa o no
                {
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    ModelState.AddModelError("Error", "La Cuenta Caduco o esta Bloqueada !!!");

                    return RedirectToAction("Login", "Account");
                }

                if (current <= hoy)  //Verificacion de la compañia si la Fecha esta Vencida
                {
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    ModelState.AddModelError("Error", "La Cuenta Caduco o esta Bloqueada !!!");

                    return RedirectToAction("Login", "Account");
                }

                //if (User.IsInRole("User"))
                //{
                //    return RedirectToAction("DashBoardView", "DashBoards");
                //}
                //else
                //{
                //    return View(user);
                //}
            }

            return View(user);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}