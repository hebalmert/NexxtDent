using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NexxtDent.Classes;
using NexxtDent.Models;

namespace NexxtDent.Controllers
{
    [Authorize(Roles = "User")]

    public class ServicesController : Controller
    {
        private NexxtDentContext db = new NexxtDentContext();

        // GET: Services
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var services = db.Services.Where(c => c.CompanyId == user.CompanyId)
                .Include(s => s.ServiceCategory)
                .Include(s => s.Tax);

            return View(services.ToList());
        }

        // GET: Services/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var service = new Service { CompanyId = user.CompanyId };

            ViewBag.ServiceCategoryId = new SelectList(ComboHelper.GetCategoryService(user.CompanyId), "ServiceCategoryId", "Categoria");
            ViewBag.TaxId = new SelectList(ComboHelper.GetTax(user.CompanyId), "TaxId", "Impuesto");

            return View(service);
        }

        // POST: Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Service service)
        {
            if (ModelState.IsValid)              
            {                           
                try
                {
                    var porcentajetecnicos = db.TechnicalPorcentages.Where(c => c.CompanyId == service.CompanyId).FirstOrDefault();
                    if (porcentajetecnicos != null)
                    {
                        if (porcentajetecnicos.CobraPorcentaje == true)
                        {
                            if (service.Precio1 == 0 || service.Precio2 == 0 || service.Precio3 == 0)
                            {
                                ModelState.AddModelError(string.Empty, (@Resources.Resource.Msg_TecnicoPorcentaje));
                                ViewBag.ServiceCategoryId = new SelectList(ComboHelper.GetCategoryService(service.CompanyId), "ServiceCategoryId", "Categoria", service.ServiceCategoryId);
                                ViewBag.TaxId = new SelectList(ComboHelper.GetTax(service.CompanyId), "TaxId", "Impuesto", service.TaxId);
                                return View(service);
                            }
                        }
                        else
                        {
                            service.Precio1 = 0;
                            service.Precio2 = 0;
                            service.Precio3 = 0;
                        }
                    }

                    db.Services.Add(service);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("_Index"))
                    {
                        ModelState.AddModelError(string.Empty, (@Resources.Resource.Msg_DoubleData));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }

            ViewBag.ServiceCategoryId = new SelectList(ComboHelper.GetCategoryService(service.CompanyId), "ServiceCategoryId", "Categoria", service.ServiceCategoryId);
            ViewBag.TaxId = new SelectList(ComboHelper.GetTax(service.CompanyId), "TaxId", "Impuesto", service.TaxId);
            return View(service);
        }

        // GET: Services/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }

            ViewBag.ServiceCategoryId = new SelectList(ComboHelper.GetCategoryService(service.CompanyId), "ServiceCategoryId", "Categoria", service.ServiceCategoryId);
            ViewBag.TaxId = new SelectList(ComboHelper.GetTax(service.CompanyId), "TaxId", "Impuesto", service.TaxId);
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Service service)
        {
            if (ModelState.IsValid)
            {              
                try
                {
                    var porcentajetecnicos = db.TechnicalPorcentages.Where(c => c.CompanyId == service.CompanyId).FirstOrDefault();
                    if (porcentajetecnicos != null)
                    {
                        if (porcentajetecnicos.CobraPorcentaje == true)
                        {
                            if (service.Precio1 == 0 || service.Precio2 == 0 || service.Precio3 == 0)
                            {
                                ModelState.AddModelError(string.Empty, (@Resources.Resource.Msg_TecnicoPorcentaje));
                                ViewBag.ServiceCategoryId = new SelectList(ComboHelper.GetCategoryService(service.CompanyId), "ServiceCategoryId", "Categoria", service.ServiceCategoryId);
                                ViewBag.TaxId = new SelectList(ComboHelper.GetTax(service.CompanyId), "TaxId", "Impuesto", service.TaxId);
                                return View(service);
                            }
                        }
                        else
                        {
                            service.Precio1 = 0;
                            service.Precio2 = 0;
                            service.Precio3 = 0;
                        }
                    }

                    db.Entry(service).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("_Index"))
                    {
                        ModelState.AddModelError(string.Empty, (@Resources.Resource.Msg_DoubleData));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }

            ViewBag.ServiceCategoryId = new SelectList(ComboHelper.GetCategoryService(service.CompanyId), "ServiceCategoryId", "Categoria", service.ServiceCategoryId);
            ViewBag.TaxId = new SelectList(ComboHelper.GetTax(service.CompanyId), "TaxId", "Impuesto", service.TaxId);
            return View(service);
        }

        // GET: Services/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Service service = db.Services.Find(id);
            db.Services.Remove(service);
            try
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    ModelState.AddModelError(string.Empty, (@Resources.Resource.Msg_Relationship));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(service);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
