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

    public class IncomesController : Controller
    {
        private NexxtDentContext db = new NexxtDentContext();

        // GET: Incomes
        public ActionResult PrintReport(DateTime fechaInicio, DateTime fechafin, int? page = null)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            page = (page ?? 1);

            var incomes = db.Incomes.Where(c => c.CompanyId == user.CompanyId && c.Date >= fechaInicio && c.Date <= fechafin);

            return View(incomes.OrderBy(o => o.Date).ToList().ToPagedList((int)page, 5));
        }

        //Get: PrintSell: PrintIncomeReport/Create
        public ActionResult PrintInComeReport()
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

        // Post: PrintIncomeReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrintInComeReport(PrintViewDate printViewdate)
        {

            return RedirectToAction("PrintReport", new { fechaInicio = printViewdate.DateInicio, fechafin = printViewdate.DateFin });
        }


        // GET: Incomes
        public ActionResult Index(int? page = null)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            page = (page ?? 1);

            var incomes = db.Incomes.Where(c => c.CompanyId == user.CompanyId);
            return View(incomes.OrderByDescending(o => o.Date).ToList().ToPagedList((int)page, 10));
        }

        // GET: Incomes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Income income = db.Incomes.Find(id);
            if (income == null)
            {
                return HttpNotFound();
            }
            return View(income);
        }

        // GET: Incomes/Create
        public ActionResult Create()
        {

            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var income = new Income
            {
                CompanyId = user.CompanyId,
                Date = DateTime.Today
            };

            return View(income);
        }

        // POST: Incomes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Income income)
        {
            if (ModelState.IsValid)
            {
                db.Incomes.Add(income);
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

            return View(income);
        }

        // GET: Incomes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Income income = db.Incomes.Find(id);
            if (income == null)
            {
                return HttpNotFound();
            }

            return View(income);
        }

        // POST: Incomes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Income income)
        {
            if (ModelState.IsValid)
            {
                db.Entry(income).State = EntityState.Modified;
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

            return View(income);
        }

        // GET: Incomes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Income income = db.Incomes.Find(id);
            if (income == null)
            {
                return HttpNotFound();
            }
            return View(income);
        }

        // POST: Incomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Income income = db.Incomes.Find(id);
            db.Incomes.Remove(income);
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
            return View(income);
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
