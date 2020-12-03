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
    public class ReceptionLogsController : Controller
    {
        private NexxtDentContext db = new NexxtDentContext();

        // GET: ReceptionLogs
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var receptionLogs = db.ReceptionLogs.Where(c => c.CompanyId == user.CompanyId);

            return View(receptionLogs.OrderBy(o => o.ReceptionId).ToList());
        }

        // GET: ReceptionLogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceptionLog receptionLog = db.ReceptionLogs.Find(id);
            if (receptionLog == null)
            {
                return HttpNotFound();
            }
            return View(receptionLog);
        }

        // GET: ReceptionLogs/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var receptionlog = new ReceptionLog
            {
                CompanyId = user.CompanyId,
                Date = DateTime.Today,
            };

            ViewBag.ReceptionId = new SelectList(db.Receptions.Where(c => c.CompanyId == user.CompanyId), "ReceptionId", "Paciente");
            return View(receptionlog);
        }

        // POST: ReceptionLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReceptionLog receptionLog)
        {
            if (ModelState.IsValid)
            {
                db.ReceptionLogs.Add(receptionLog);
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

            ViewBag.ReceptionId = new SelectList(db.Receptions.Where(c => c.CompanyId == receptionLog.CompanyId), "ReceptionId", "Paciente", receptionLog.ReceptionId);
            return View(receptionLog);
        }

        // GET: ReceptionLogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceptionLog receptionLog = db.ReceptionLogs.Find(id);
            if (receptionLog == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReceptionId = new SelectList(db.Receptions.Where(c => c.CompanyId == receptionLog.CompanyId), "ReceptionId", "Paciente", receptionLog.ReceptionId);
            return View(receptionLog);
        }

        // POST: ReceptionLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReceptionLog receptionLog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(receptionLog).State = EntityState.Modified;
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

            ViewBag.ReceptionId = new SelectList(db.Receptions.Where(c => c.CompanyId == receptionLog.CompanyId), "ReceptionId", "Paciente", receptionLog.ReceptionId);
            return View(receptionLog);
        }

        // GET: ReceptionLogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceptionLog receptionLog = db.ReceptionLogs.Find(id);
            if (receptionLog == null)
            {
                return HttpNotFound();
            }
            return View(receptionLog);
        }

        // POST: ReceptionLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReceptionLog receptionLog = db.ReceptionLogs.Find(id);
            db.ReceptionLogs.Remove(receptionLog);
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
            return View(receptionLog);
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
