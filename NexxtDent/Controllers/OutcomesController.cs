using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NexxtDent.Models;
using PagedList;

namespace NexxtDent.Controllers
{
    [Authorize(Roles = "User")]

    public class OutcomesController : Controller
    {
        private NexxtDentContext db = new NexxtDentContext();


        // GET: CxCBills
        public ActionResult PrintReport(DateTime fechaInicio, DateTime fechafin, int? page = null)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            page = (page ?? 1);

            var outcomes = db.Outcomes.Where(c => c.CompanyId == user.CompanyId && c.Date >= fechaInicio && c.Date <= fechafin);

            return View(outcomes.OrderBy(o => o.Date).ThenBy(o => o.Egreso).ToList().ToPagedList((int)page, 10));
        }

        //Get: PrintSell: PrintOutcomeReport/Create
        public ActionResult PrintOutComeReport()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var printviewdate = new PrintViewDate
            {
                CompanyId = user.CompanyId,
                DateFin = DateTime.Today,
                DateInicio = DateTime.Today
            };

            return PartialView(printviewdate);
        }

        // Post: PrintOutcomeReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrintOutComeReport(PrintViewDate printViewdate)
        {

            return RedirectToAction("PrintReport", new { fechaInicio = printViewdate.DateInicio, fechafin = printViewdate.DateFin });
        }

        // GET: AddItems/Create
        public ActionResult AddItems(int id)
        {

            var additems = new OutcomeDetail { OutcomeId = id };

            return PartialView(additems);
        }

        // POST: AddItems/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddItems(OutcomeDetail outcomedetail)
        {

            if (ModelState.IsValid)
            {
                db.OutcomeDetails.Add(outcomedetail);
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = outcomedetail.OutcomeId });
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

            return PartialView(outcomedetail);
        }

        // Post: DeleteItems/Borrar
        public ActionResult DeleteItems(int id)
        {

            {
                var outcomdetail = db.OutcomeDetails.Find(id);
                db.OutcomeDetails.Remove(outcomdetail);
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = outcomdetail.OutcomeId });
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
                return RedirectToAction("Details", new { id = outcomdetail.OutcomeId });
            }
        }

        // GET: Outcomes
        public ActionResult Index(string searchString, int? page = null)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            page = (page ?? 1);

            if (!String.IsNullOrEmpty(searchString))
            {
                var outcomes = db.Outcomes.Where(c => c.CompanyId == user.CompanyId && c.Beneficiario.Contains(searchString));
                return View(outcomes.OrderByDescending(o => o.Date).ThenBy(o => o.Egreso).ToList().ToPagedList((int)page, 10));
            }
            else
            {
                var outcomes = db.Outcomes.Where(c => c.CompanyId == user.CompanyId);
                return View(outcomes.OrderByDescending(o => o.Date).ThenBy(o => o.Egreso).ToList().ToPagedList((int)page, 10));
            }

        }

        // GET: Outcomes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Outcome outcome = db.Outcomes.Find(id);
            if (outcome == null)
            {
                return HttpNotFound();
            }
            return View(outcome);
        }

        // GET: Outcomes/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var outcome = new Outcome
            {
                CompanyId = user.CompanyId,
                Date = DateTime.Today
            };

            return View(outcome);
        }

        // POST: Outcomes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Outcome outcome)
        {
            if (ModelState.IsValid)
            {
                db.Outcomes.Add(outcome);
                try
                {
                    db.SaveChanges();
                    //Se busca en register el Contrato Actual
                    var db2 = new NexxtDentContext();
                    var db3 = new NexxtDentContext();
                    int sum = 0;
                    int Contra = 0;

                    var register = db2.Registers.Where(c => c.CompanyId == outcome.CompanyId).FirstOrDefault();
                    Contra = register.Egresos;
                    sum = Contra + 1;
                    register.Egresos = sum;
                    db2.Entry(register).State = EntityState.Modified;
                    db2.SaveChanges();
                    db2.Dispose();

                    var salida = db3.Outcomes.Find(outcome.OutcomeId);

                    salida.Egreso = Convert.ToString(sum);
                    db3.Entry(salida).State = EntityState.Modified;
                    db3.SaveChanges();
                    db3.Dispose();
                    //Se guardar el nuevo Contrato en Register y Contracts

                    return RedirectToAction("Details", new { id = outcome.OutcomeId });
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

            return View(outcome);
        }

        // GET: Outcomes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Outcome outcome = db.Outcomes.Find(id);
            if (outcome == null)
            {
                return HttpNotFound();
            }

            return View(outcome);
        }

        // POST: Outcomes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Outcome outcome)
        {
            if (ModelState.IsValid)
            {
                db.Entry(outcome).State = EntityState.Modified;
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

            return View(outcome);
        }

        // GET: Outcomes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Outcome outcome = db.Outcomes.Find(id);
            if (outcome == null)
            {
                return HttpNotFound();
            }
            return View(outcome);
        }

        // POST: Outcomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Outcome outcome = db.Outcomes.Find(id);
            db.Outcomes.Remove(outcome);
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
            return View(outcome);
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
