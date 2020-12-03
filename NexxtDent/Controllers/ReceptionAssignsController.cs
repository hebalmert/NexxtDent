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

    public class ReceptionAssignsController : Controller
    {
        private readonly NexxtDentContext db = new NexxtDentContext();

        // GET: Receptions/Activate
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var receptionassign = db.ReceptionAssigns.Find(id);
            if (receptionassign == null)
            {
                return HttpNotFound();
            }

            receptionassign.StateId = 8;
            db.Entry(receptionassign).State = EntityState.Modified;
            db.SaveChanges();

            var reception = db.Receptions.Find(receptionassign.ReceptionId);
            reception.StateId = 8;
            db.Entry(reception).State = EntityState.Modified;
            db.SaveChanges();

            var receptionassingdetail = db.ReceptionAssignDetails.Where(c => c.ReceptionAssignId == id).ToList();
            if (receptionassingdetail != null)
            {
                var db2 = new NexxtDentContext();
                foreach (var item in receptionassingdetail)
                {
                    
                    var technicalwork = new TechnicalWork
                    {
                        CompanyId = reception.CompanyId,
                        TechnicalId = item.TechnicalId,
                        ReceptionId = reception.ReceptionId,
                        ReceptionAssignId = receptionassign.ReceptionAssignId,
                        Servicio = item.Service.Servicio,
                        Precio = item.Precio,
                        Cantidad = item.Cantidad,
                        Total = item.Total,
                        Tasa = item.Tasa,
                        StateId = 8 //Estado Proceso
                    };
                    db2.TechnicalWorks.Add(technicalwork);
                }
                db2.SaveChanges();
                db2.Dispose();
            }

            return RedirectToAction("Details", new { id = receptionassign.ReceptionAssignId });
        }

        // GET: AddDelete/Borrar
        public ActionResult AddDelete(int id)
        {

            {
                var receptionassigndetail = db.ReceptionAssignDetails.Find(id);
                db.ReceptionAssignDetails.Remove(receptionassigndetail);
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = receptionassigndetail.ReceptionAssignId });
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                    {
                        ModelState.AddModelError(string.Empty, ("El Registo no puede ser Eliminado, porque tiene relacion con otros Datos"));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
                return RedirectToAction("Details", new { id = receptionassigndetail.ReceptionAssignId });
            }
        }

        // GET: AddTechnical/Create
        public ActionResult AddTechnical(int id)
        {

            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var taxes = db.Taxes.FirstOrDefault();

            var receptionassigndetail = new ReceptionAssignDetail
            {
                ReceptionAssignId = id,
                Tasa = taxes.Rate,
                Cantidad = 0,
                StateId = 1
            };

            ViewBag.ServiceCategoryId = new SelectList(ComboHelper.GetCategoryService(user.CompanyId), "ServiceCategoryId", "Categoria");
            ViewBag.ServiceId = new SelectList(ComboHelper.GetService(user.CompanyId), "ServiceId", "Servicio");
            ViewBag.LevelPriceId = new SelectList(ComboHelper.GetLevelPrice(), "LevelPriceId", "NivelPrecio");
            ViewBag.TechnicalId = new SelectList(ComboHelper.GetTechnical(user.CompanyId), "TechnicalId", "FullName");

            return PartialView(receptionassigndetail);
        }

        // POST: AddWork/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTechnical(ReceptionAssignDetail receptionAssignDetail)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var servi = db.Services.Find(receptionAssignDetail.ServiceId);
                    receptionAssignDetail.Servicio = servi.Servicio;

                    db.ReceptionAssignDetails.Add(receptionAssignDetail);
                    db.SaveChanges();

                    return RedirectToAction("Details", new { id = receptionAssignDetail.ReceptionAssignId });
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("_Index"))
                    {
                        ModelState.AddModelError(string.Empty, ("Existe un Registro con el mismo Valor"));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }

            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            ViewBag.ServiceCategoryId = new SelectList(ComboHelper.GetCategoryService(user.CompanyId), "ServiceCategoryId", "Categoria", receptionAssignDetail.ServiceCategoryId);
            ViewBag.ServiceId = new SelectList(ComboHelper.GetService(user.CompanyId), "ServiceId", "Servicio", receptionAssignDetail.ServiceId);
            ViewBag.LevelPriceId = new SelectList(ComboHelper.GetLevelPrice(), "LevelPriceId", "NivelPrecio", receptionAssignDetail.LevelPriceId);
            ViewBag.TechnicalId = new SelectList(ComboHelper.GetTechnical(user.CompanyId), "TechnicalId", "FullName", receptionAssignDetail.TechnicalId);

            return PartialView(receptionAssignDetail);
        }

        // GET: ReceptionAssigns
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var receptionAssigns = db.ReceptionAssigns.Where(c=> c.CompanyId == user.CompanyId && (c.StateId == 2 || c.StateId == 3 || c.StateId == 4 || c.StateId == 8))
                .Include(r => r.Client)
                .Include(r => r.Reception)
                .Include(r => r.State);
            return View(receptionAssigns.ToList());
        }

        // GET: ReceptionAssigns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceptionAssign receptionAssign = db.ReceptionAssigns.Find(id);
            if (receptionAssign == null)
            {
                return HttpNotFound();
            }
            return View(receptionAssign);
        }

        // GET: ReceptionAssigns/Create
        public ActionResult Create()
        {

            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "Cliente");
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Compania");
            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente");
            ViewBag.StateId = new SelectList(db.States, "StateId", "Estado");
            return View();
        }

        // POST: ReceptionAssigns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReceptionAssign receptionAssign)
        {
            if (ModelState.IsValid)
            {
                db.ReceptionAssigns.Add(receptionAssign);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "Cliente", receptionAssign.ClientId);
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Compania", receptionAssign.CompanyId);
            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente", receptionAssign.ReceptionId);
            ViewBag.StateId = new SelectList(db.States, "StateId", "Estado", receptionAssign.StateId);
            return View(receptionAssign);
        }

        // GET: ReceptionAssigns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var receptionAssign = db.ReceptionAssigns.Find(id);
            if (receptionAssign == null)
            {
                return HttpNotFound();
            }

            ViewBag.ClientId = new SelectList(ComboHelper.GetClient(receptionAssign.CompanyId), "ClientId", "Cliente", receptionAssign.ClientId);
            //ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente", receptionAssign.ReceptionId);
            //ViewBag.StateId = new SelectList(db.States, "StateId", "Estado", receptionAssign.StateId);
            return View(receptionAssign);
        }

        // POST: ReceptionAssigns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReceptionAssign receptionAssign)
        {
            if (ModelState.IsValid)
            {
                db.Entry(receptionAssign).State = EntityState.Modified;
                try
                {
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
            ViewBag.ClientId = new SelectList(ComboHelper.GetClient(receptionAssign.CompanyId), "ClientId", "Cliente", receptionAssign.ClientId);
            //ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente", receptionAssign.ReceptionId);
            //ViewBag.StateId = new SelectList(db.States, "StateId", "Estado", receptionAssign.StateId);
            return View(receptionAssign);
        }

        // GET: ReceptionAssigns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceptionAssign receptionAssign = db.ReceptionAssigns.Find(id);
            if (receptionAssign == null)
            {
                return HttpNotFound();
            }
            return View(receptionAssign);
        }

        // POST: ReceptionAssigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReceptionAssign receptionAssign = db.ReceptionAssigns.Find(id);
            db.ReceptionAssigns.Remove(receptionAssign);
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
            return View(receptionAssign);
        }

        public JsonResult GetService(int ServiceCategoryId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var services = db.Services.Where(c => c.ServiceCategoryId == ServiceCategoryId);
            return Json(services);
        }

        public JsonResult GetPrecio(int LevelPriceId, int ServiceId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var servicesprice = db.Services.Find(ServiceId);
            decimal precio = 0;

            if (LevelPriceId == 1)
            {
                precio = servicesprice.Precio1;
            }
            if (LevelPriceId == 2)
            {
                precio = servicesprice.Precio2;
            }
            if (LevelPriceId == 3)
            {
                precio = servicesprice.Precio3;
            }

            return Json(precio);
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
