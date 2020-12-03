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

    public class WorksController : Controller
    {
        private NexxtDentContext db = new NexxtDentContext();


        [HttpPost]
        public JsonResult Search(string Prefix)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var Iwork = (from work in db.Works
                           where work.Trabajo.StartsWith(Prefix) && work.CompanyId == user.CompanyId
                           select new
                           {
                               label = work.Trabajo,
                               val = work.WorkId
                           }).ToList();

            return Json(Iwork);

        }


        // GET: Works
        public ActionResult Index(int? workid)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (workid != null)
            {
                var works = db.Works.Where(c => c.CompanyId == user.CompanyId && c.WorkId == workid)
                .Include(w => w.Tax)
                .Include(w => w.WorkCategory);

                return View(works.OrderBy(o => o.Trabajo).ToList());
            }
            else
            {
                var works = db.Works.Where(c => c.CompanyId == user.CompanyId)
                .Include(w => w.Tax)
                .Include(w => w.WorkCategory);

                return View(works.OrderBy(o => o.Trabajo).ToList());
            }
        }

        // GET: Works/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = db.Works.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        // GET: Works/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var works = new Work { CompanyId = user.CompanyId };

            ViewBag.TaxId = new SelectList(ComboHelper.GetTax(user.CompanyId), "TaxId", "Impuesto");
            ViewBag.WorkCategoryId = new SelectList(ComboHelper.GetCategoryWork(user.CompanyId), "WorkCategoryId", "Categoria");

            return View(works);
        }

        // POST: Works/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Work work)
        {
            if (ModelState.IsValid)
            {
                db.Works.Add(work);
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

            ViewBag.TaxId = new SelectList(ComboHelper.GetTax(work.CompanyId), "TaxId", "Impuesto", work.TaxId);
            ViewBag.WorkCategoryId = new SelectList(ComboHelper.GetCategoryWork(work.CompanyId), "WorkCategoryId", "Categoria", work.WorkCategoryId);

            return View(work);
        }

        // GET: Works/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = db.Works.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }

            ViewBag.TaxId = new SelectList(ComboHelper.GetTax(work.CompanyId), "TaxId", "Impuesto", work.TaxId);
            ViewBag.WorkCategoryId = new SelectList(ComboHelper.GetCategoryWork(work.CompanyId), "WorkCategoryId", "Categoria", work.WorkCategoryId);

            return View(work);
        }

        // POST: Works/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Work work)
        {
            if (ModelState.IsValid)
            {
                db.Entry(work).State = EntityState.Modified;
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

            ViewBag.TaxId = new SelectList(ComboHelper.GetTax(work.CompanyId), "TaxId", "Impuesto", work.TaxId);
            ViewBag.WorkCategoryId = new SelectList(ComboHelper.GetCategoryWork(work.CompanyId), "WorkCategoryId", "Categoria", work.WorkCategoryId);

            return View(work);
        }

        // GET: Works/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = db.Works.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        // POST: Works/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Work work = db.Works.Find(id);
            db.Works.Remove(work);
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
            return View(work);
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
