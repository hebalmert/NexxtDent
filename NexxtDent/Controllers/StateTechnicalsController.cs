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
    [Authorize(Roles = "Admin")]

    public class StateTechnicalsController : Controller
    {
        private NexxtDentContext db = new NexxtDentContext();

        // GET: StateTechnicals
        public ActionResult Index()
        {
            return View(db.StateTechnicals.ToList());
        }

        // GET: StateTechnicals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StateTechnical stateTechnical = db.StateTechnicals.Find(id);
            if (stateTechnical == null)
            {
                return HttpNotFound();
            }
            return View(stateTechnical);
        }

        // GET: StateTechnicals/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: StateTechnicals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StateTechnicalId,EstadoTecnico")] StateTechnical stateTechnical)
        {
            if (ModelState.IsValid)
            {
                db.StateTechnicals.Add(stateTechnical);
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

            return View(stateTechnical);
        }

        // GET: StateTechnicals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StateTechnical stateTechnical = db.StateTechnicals.Find(id);
            if (stateTechnical == null)
            {
                return HttpNotFound();
            }
            return View(stateTechnical);
        }

        // POST: StateTechnicals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StateTechnicalId,EstadoTecnico")] StateTechnical stateTechnical)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stateTechnical).State = EntityState.Modified;
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
            return View(stateTechnical);
        }

        // GET: StateTechnicals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StateTechnical stateTechnical = db.StateTechnicals.Find(id);
            if (stateTechnical == null)
            {
                return HttpNotFound();
            }
            return View(stateTechnical);
        }

        // POST: StateTechnicals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StateTechnical stateTechnical = db.StateTechnicals.Find(id);
            db.StateTechnicals.Remove(stateTechnical);
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
            return View(stateTechnical);
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
