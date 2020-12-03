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

    public class SupportTextsController : Controller
    {
        private NexxtDentContext db = new NexxtDentContext();

        // GET: SupportTexts
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var supportTexts = db.SupportTexts.Where(c => c.CompanyId == user.CompanyId);

            return View(supportTexts.ToList());
        }

        // GET: SupportTexts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupportText supportText = db.SupportTexts.Find(id);
            if (supportText == null)
            {
                return HttpNotFound();
            }
            return View(supportText);
        }

        // GET: SupportTexts/Create
        public ActionResult Create()
        {
                var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                if (user == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                var supportext = new SupportText
                {
                    CompanyId = user.CompanyId
                };

                return View(supportext);
        }

        // POST: SupportTexts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SupportText supportText)
        {
            if (ModelState.IsValid)
            {
                db.SupportTexts.Add(supportText);
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

            return View(supportText);
        }

        // GET: SupportTexts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupportText supportText = db.SupportTexts.Find(id);
            if (supportText == null)
            {
                return HttpNotFound();
            }

            return View(supportText);
        }

        // POST: SupportTexts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SupportText supportText)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supportText).State = EntityState.Modified;
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

            return View(supportText);
        }

        // GET: SupportTexts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupportText supportText = db.SupportTexts.Find(id);
            if (supportText == null)
            {
                return HttpNotFound();
            }
            return View(supportText);
        }

        // POST: SupportTexts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SupportText supportText = db.SupportTexts.Find(id);
            db.SupportTexts.Remove(supportText);
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
            return View(supportText);
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
