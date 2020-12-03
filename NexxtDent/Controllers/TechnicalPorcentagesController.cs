using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NexxtDent.Models;

namespace NexxtDent.Controllers
{
    [Authorize(Roles = "User")]

    public class TechnicalPorcentagesController : Controller
    {
        private NexxtDentContext db = new NexxtDentContext();

        // GET: TechnicalPorcentages
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var technicalPorcentages = db.TechnicalPorcentages.Where(c => c.CompanyId == user.CompanyId)
                .Include(t => t.Company);

            return View(technicalPorcentages.ToList());
        }

        // GET: TechnicalPorcentages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var technicalPorcentage = db.TechnicalPorcentages.Find(id);
            if (technicalPorcentage == null)
            {
                return HttpNotFound();
            }
            return View(technicalPorcentage);
        }

        // GET: TechnicalPorcentages/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var tecnicoporcentaje = new TechnicalPorcentage { CompanyId = user.CompanyId };

            return View(tecnicoporcentaje);
        }

        // POST: TechnicalPorcentages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TechnicalPorcentage technicalPorcentage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.TechnicalPorcentages.Add(technicalPorcentage);
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

            return View(technicalPorcentage);
        }

        // GET: TechnicalPorcentages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var technicalPorcentage = db.TechnicalPorcentages.Find(id);
            if (technicalPorcentage == null)
            {
                return HttpNotFound();
            }

            return View(technicalPorcentage);
        }

        // POST: TechnicalPorcentages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TechnicalPorcentage technicalPorcentage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(technicalPorcentage).State = EntityState.Modified;
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

            return View(technicalPorcentage);
        }

        // GET: TechnicalPorcentages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TechnicalPorcentage technicalPorcentage = db.TechnicalPorcentages.Find(id);
            if (technicalPorcentage == null)
            {
                return HttpNotFound();
            }
            return View(technicalPorcentage);
        }

        // POST: TechnicalPorcentages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var technicalPorcentage = db.TechnicalPorcentages.Find(id);

            try
            {
                db.TechnicalPorcentages.Remove(technicalPorcentage);
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
            return View(technicalPorcentage);
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
