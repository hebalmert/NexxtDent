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
using PagedList;

namespace NexxtDent.Controllers
{
    [Authorize(Roles = "User")]

    public class ReceptionsController : Controller
    {
        private NexxtDentContext db = new NexxtDentContext();

        // GET: Print/Print
        public ActionResult Print(int id)
        {

            var receptionss = db.Receptions.Find(id);

            return View(receptionss);
        }

        // GET: Receptions/Activate
        public ActionResult Activate(int? id)  //id = receptionId
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Actualiza el estatus de la Recepcion
            var reception = db.Receptions.Find(id);
            if (reception != null)
            {
                reception.StateId = 2;
                db.Entry(reception).State = EntityState.Modified;
                db.SaveChanges();
            }

            var repassign = new ReceptionAssign
            {
                CompanyId = reception.CompanyId,
                ReceptionId = reception.ReceptionId,
                Recepcion = reception.Recepcion,
                Date = DateTime.Today,
                ClientId = reception.ClientId,
                Total = reception.TotalValue,
                StateId = 2 //Estado Proceso
            };
            db.ReceptionAssigns.Add(repassign);
            db.SaveChanges();
            db.Dispose();


            return RedirectToAction("Details", new { id = reception.ReceptionId });
        }

        // GET: AddDelete/Borrar
        public ActionResult AddDelete(int id)
        {

            {
                var receptionwork = db.ReceptionWorks.Find(id);
                db.ReceptionWorks.Remove(receptionwork);
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = receptionwork.ReceptionId });
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
                return RedirectToAction("Details", new { id = receptionwork.ReceptionId });
            }
        }

        // GET: AddWork/Create
        public ActionResult AddWork(int id)
        {

            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var taxes = db.Taxes.FirstOrDefault();

            var receptionworks = new ReceptionWork
            {
                ReceptionId = id,
                Tasa = taxes.Rate,
                Cantidad = 0
            };

            ViewBag.WorkCategoryId = new SelectList(ComboHelper.GetCategoryWork(user.CompanyId), "WorkCategoryId", "Categoria");
            ViewBag.WorkId = new SelectList(ComboHelper.GetWork(user.CompanyId), "WorkId", "Trabajo");

            return PartialView(receptionworks);
        }

        // POST: AddWork/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddWork(ReceptionWork receptionwork)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.ReceptionWorks.Add(receptionwork);
                    db.SaveChanges();

                    var db2 = new NexxtDentContext();
                    var receptionworkUpte = db2.ReceptionWorks.Find(receptionwork.ReceptionWorkId);
                    receptionworkUpte.Trabajo = receptionworkUpte.Work.Trabajo;
                    db2.Entry(receptionworkUpte).State = EntityState.Modified;
                    db2.SaveChanges();
                    db2.Dispose();


                    return RedirectToAction("Details", new { id = receptionwork.ReceptionId });
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

            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            ViewBag.CategoryWorkId = new SelectList(ComboHelper.GetCategoryWork(user.CompanyId), "WorkCategoryId", "Categoria", receptionwork.WorkCategoryId);
            ViewBag.WorkId = new SelectList(ComboHelper.GetWork(user.CompanyId), "WorkId", "Trabajo", receptionwork.WorkId);

            return PartialView(receptionwork);
        }

                [HttpPost]
        public JsonResult Search(string Prefix)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var Iclient = (from cliente in db.Clients
                           where cliente.Cliente.StartsWith(Prefix) && cliente.CompanyId == user.CompanyId
                           select new
                           {
                               label = cliente.Cliente,
                               val = cliente.ClientId
                           }).ToList();

            return Json(Iclient);

        }

        // GET: Receptions
        public ActionResult Index(int? clientid, int? page = null)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            page = (page ?? 1);

            if (clientid != null)
            {
                var receptionss = db.Receptions.Where(c => c.CompanyId == user.CompanyId && c.ClientId == clientid)
               .Include(r => r.Client)
               .Include(r => r.State);
                return View(receptionss.OrderByDescending(o => o.Recepcion).ToList().ToPagedList((int)page, 25));
            }
            else
            {
                var receptionss = db.Receptions.Where(c => c.CompanyId == user.CompanyId)
                    .Include(r => r.Client)
                    .Include(r => r.State);
                return View(receptionss.OrderByDescending(o => o.Recepcion).ToList().ToPagedList((int)page, 25));
            }
        }

        // GET: Receptions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reception = db.Receptions.Find(id);
            if (reception == null)
            {
                return HttpNotFound();
            }
            return View(reception);
        }

        // GET: Receptions/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var db2 = new NexxtDentContext();
            var headtexts = db2.HeadTexts.Where(c => c.CompanyId == user.CompanyId).FirstOrDefault();

            var receptions = new Reception
            {
                Recepcion = 0,
                CompanyId = user.CompanyId,
                Date = DateTime.Today,
                DateEntrega = DateTime.Today,
                DatePrueba = DateTime.Today,
                DateAnulado = DateTime.Today,
                StateId = 1,
                HeadTextId = headtexts.HeadTextId
            };
            db2.Dispose();


            ViewBag.ClientId = new SelectList(ComboHelper.GetClient(user.CompanyId), "ClientId", "Cliente");
            ViewBag.GumColorId = new SelectList(ComboHelper.GetGumcolor(user.CompanyId), "GumColorId", "ColorEncia");
            ViewBag.ToothColorId = new SelectList(ComboHelper.GetToothcolor(user.CompanyId), "ToothColorId", "ColorDiente");

            return View(receptions);
        }

        // POST: Receptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reception reception)
        {
            if (ModelState.IsValid)
            {             
                try
                {                    
                    var db2 = new NexxtDentContext();
                    int sum = 0;
                    int Recep = 0;

                    var register = db2.Registers.Where(c => c.CompanyId == reception.CompanyId).FirstOrDefault();
                    Recep = register.Recepcion;
                    sum = Recep + 1;
                    register.Recepcion = sum;
                    db2.Entry(register).State = EntityState.Modified;

                    reception.Recepcion = sum;
                    db.Receptions.Add(reception);
                    db.SaveChanges();

                    db2.SaveChanges();
                    db2.Dispose();

                    // Se crea el Registro de Control de Consecutivos de la Compania
                    var receptionlog = new ReceptionLog
                    {
                        CompanyId = reception.CompanyId,
                        ReceptionId = reception.ReceptionId,
                        Date = DateTime.Today,
                        Estado = @Resources.Resource.State_Recibido,
                        Detalle = reception.Detalles
                    };
                    db.ReceptionLogs.Add(receptionlog);
                    db.SaveChanges();
                    db.Dispose();
                    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

                    return RedirectToAction("Details", new { id = reception.ReceptionId });
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

            ViewBag.ClientId = new SelectList(ComboHelper.GetClient(reception.CompanyId), "ClientId", "Cliente", reception.ClientId);
            ViewBag.GumColorId = new SelectList(ComboHelper.GetGumcolor(reception.CompanyId), "GumColorId", "ColorEncia", reception.GumColorId);
            ViewBag.ToothColorId = new SelectList(ComboHelper.GetToothcolor(reception.CompanyId), "ToothColorId", "ColorDiente", reception.ToothColorId);
            return View(reception);
        }

        // GET: Receptions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reception = db.Receptions.Find(id);
            if (reception == null)
            {
                return HttpNotFound();
            }

            ViewBag.ClientId = new SelectList(ComboHelper.GetClient(reception.CompanyId), "ClientId", "Cliente", reception.ClientId);
            ViewBag.GumColorId = new SelectList(ComboHelper.GetGumcolor(reception.CompanyId), "GumColorId", "ColorEncia", reception.GumColorId);
            ViewBag.ToothColorId = new SelectList(ComboHelper.GetToothcolor(reception.CompanyId), "ToothColorId", "ColorDiente", reception.ToothColorId);
            return View(reception);
        }

        // POST: Receptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Reception reception)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reception).State = EntityState.Modified;
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

            ViewBag.ClientId = new SelectList(ComboHelper.GetClient(reception.CompanyId), "ClientId", "Cliente", reception.ClientId);
            ViewBag.GumColorId = new SelectList(ComboHelper.GetGumcolor(reception.CompanyId), "GumColorId", "ColorEncia", reception.GumColorId);
            ViewBag.ToothColorId = new SelectList(ComboHelper.GetToothcolor(reception.CompanyId), "ToothColorId", "ColorDiente", reception.ToothColorId);
            return View(reception);
        }

        // GET: Receptions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reception reception = db.Receptions.Find(id);
            if (reception == null)
            {
                return HttpNotFound();
            }
            return View(reception);
        }

        // POST: Receptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var reception = db.Receptions.Find(id);
            db.Receptions.Remove(reception);
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
            return View(reception);
        }

        public JsonResult GetWork(int WorkCategoryId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var work = db.Works.Where(c => c.WorkCategoryId == WorkCategoryId);
            return Json(work);
        }

        public JsonResult GetPrecio(int WorkId, int ReceptionId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var receptions = db.Receptions.Find(ReceptionId);
            int Clin = receptions.ClientId;
            var clinic = db.Clients.Find(Clin);
            int Lprice = clinic.LevelPriceId;

            var work = db.Works.Find(WorkId);
            decimal precio = 0;

            if (Lprice == 1)
            {
                precio = work.Precio1;
            }
            if (Lprice == 2)
            {
                precio = work.Precio2;
            }
            if (Lprice == 3)
            {
                precio = work.Precio3;
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
