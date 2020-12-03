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
    [Authorize(Roles = "User, Technical")]

    public class TechnicalWorksController : Controller
    {
        private NexxtDentContext db = new NexxtDentContext();

        // GET: AddComment/Create
        public ActionResult AddComment(int id)
        {

            var technicalworkdetail = new TechnicalWorkDetail
            {
                TechnicalWorkId = id,
                Date = DateTime.Today
            };

            ViewBag.StateTechnicalId = new SelectList(ComboHelper.GetStateTechnical(), "StateTechnicalId", "EstadoTecnico");

            return View(technicalworkdetail);
        }

        // POST: AddComment/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(TechnicalWorkDetail technicalworkdetail)
        {
            if (ModelState.IsValid)            
            {                
                try
                {
                    db.TechnicalWorkDetails.Add(technicalworkdetail);
                    db.SaveChanges();

                    int EstadoReal = 0;
                    switch (technicalworkdetail.StateTechnicalId)
                    {
                        case 1:
                            EstadoReal = 8;
                            break;
                        case 2:
                            EstadoReal = 3;
                            break;
                        case 3:
                            EstadoReal = 4;
                            break;
                        case 4:
                            EstadoReal = 5;
                            break;
                    }

                    var Nreception = db.TechnicalWorks.Find(technicalworkdetail.TechnicalWorkId);

                    int RecepNumero = Nreception.ReceptionId;
                    int RepAssingNumero = Nreception.ReceptionAssignId;
                    int TechnicalNumero = technicalworkdetail.TechnicalWorkId;

                    Nreception.StateId = EstadoReal;
                    db.Entry(Nreception).State = EntityState.Modified;
                    db.SaveChanges();

                    if (EstadoReal == 5)
                    {
                        var db6 = new NexxtDentContext();
                        var asignados = db6.TechnicalWorks.Where(c => c.ReceptionId == RecepNumero).ToList();
                        if (asignados != null)
                        {
                            int CanAsignados = asignados.Count;
                            int sum = 0;
                            foreach (var item in asignados)
                            {
                                if (item.StateId == 5)
                                {
                                    sum += 1;
                                }
                            }
                            if (CanAsignados == sum)
                            {

                                var db3 = new NexxtDentContext();
                                var reception = db3.Receptions.Find(RecepNumero);
                                reception.StateId = EstadoReal;
                                db3.Entry(reception).State = EntityState.Modified;
                                db3.SaveChanges();
                                db3.Dispose();

                                var db4 = new NexxtDentContext();
                                var receptionassign = db4.ReceptionAssigns.Find(RepAssingNumero);
                                receptionassign.StateId = EstadoReal;
                                db4.Entry(receptionassign).State = EntityState.Modified;
                                db4.SaveChanges();
                                db4.Dispose();

                                var db5 = new NexxtDentContext();
                                var despachos = new Delivery
                                {
                                    CompanyId = reception.CompanyId,
                                    Date = DateTime.Today,
                                    ReceptionId = RecepNumero,
                                    ReceptionAssignId = RepAssingNumero,
                                    TechnicalWorkId = TechnicalNumero,
                                    ClientId = reception.ClientId,
                                    StateId = 5
                                };
                                db5.Deliveries.Add(despachos);
                                db5.SaveChanges();
                                db5.Dispose();
                            }
                        }
                        else
                        { 
                            db6.Dispose();
                        }

                    }

                    return RedirectToAction("Details", new { id = technicalworkdetail.TechnicalWorkId });
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

            ViewBag.StateTechnicalId = new SelectList(ComboHelper.GetStateTechnical(), "StateTechnicalId", "EstadoTecnico", technicalworkdetail.StateTechnicalId);

            return View(technicalworkdetail);
        }


        // GET: TechnicalWorks
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var technicalWorks = db.TechnicalWorks.Where(c=> c.CompanyId == user.CompanyId && (c.StateId == 8 || c.StateId == 3 || c.StateId == 4))
                .Include(t => t.Reception)
                .Include(t => t.ReceptionAssign)
                .Include(t => t.State)
                .Include(t => t.Technical);
            return View(technicalWorks.ToList());
        }

        // GET: TechnicalWorks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TechnicalWork technicalWork = db.TechnicalWorks.Find(id);
            if (technicalWork == null)
            {
                return HttpNotFound();
            }
            return View(technicalWork);
        }

        // GET: TechnicalWorks/Create
        public ActionResult Create()
        {

            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente");
            ViewBag.ReceptionAssignId = new SelectList(db.ReceptionAssigns, "ReceptionAssignId", "ReceptionAssignId");
            ViewBag.StateId = new SelectList(db.States, "StateId", "Estado");
            ViewBag.TechnicalId = new SelectList(db.Technicals, "TechnicalId", "FirstName");
            return View();
        }

        // POST: TechnicalWorks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TechnicalWork technicalWork)
        {
            if (ModelState.IsValid)
            {
                db.TechnicalWorks.Add(technicalWork);
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

            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente", technicalWork.ReceptionId);
            ViewBag.ReceptionAssignId = new SelectList(db.ReceptionAssigns, "ReceptionAssignId", "ReceptionAssignId", technicalWork.ReceptionAssignId);
            ViewBag.StateId = new SelectList(db.States, "StateId", "Estado", technicalWork.StateId);
            ViewBag.TechnicalId = new SelectList(db.Technicals, "TechnicalId", "FirstName", technicalWork.TechnicalId);

            return View(technicalWork);
        }

        // GET: TechnicalWorks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TechnicalWork technicalWork = db.TechnicalWorks.Find(id);
            if (technicalWork == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente", technicalWork.ReceptionId);
            ViewBag.ReceptionAssignId = new SelectList(db.ReceptionAssigns, "ReceptionAssignId", "ReceptionAssignId", technicalWork.ReceptionAssignId);
            ViewBag.StateId = new SelectList(db.States, "StateId", "Estado", technicalWork.StateId);
            ViewBag.TechnicalId = new SelectList(db.Technicals, "TechnicalId", "FirstName", technicalWork.TechnicalId);

            return View(technicalWork);
        }

        // POST: TechnicalWorks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TechnicalWork technicalWork)
        {
            if (ModelState.IsValid)
            {
                db.Entry(technicalWork).State = EntityState.Modified;
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

            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente", technicalWork.ReceptionId);
            ViewBag.ReceptionAssignId = new SelectList(db.ReceptionAssigns, "ReceptionAssignId", "ReceptionAssignId", technicalWork.ReceptionAssignId);
            ViewBag.StateId = new SelectList(db.States, "StateId", "Estado", technicalWork.StateId);
            ViewBag.TechnicalId = new SelectList(db.Technicals, "TechnicalId", "FirstName", technicalWork.TechnicalId);

            return View(technicalWork);
        }

        // GET: TechnicalWorks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TechnicalWork technicalWork = db.TechnicalWorks.Find(id);
            if (technicalWork == null)
            {
                return HttpNotFound();
            }
            return View(technicalWork);
        }

        // POST: TechnicalWorks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TechnicalWork technicalWork = db.TechnicalWorks.Find(id);
            db.TechnicalWorks.Remove(technicalWork);
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
            return View(technicalWork);
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
