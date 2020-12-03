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

    public class CxCCanceledsController : Controller
    {
        private NexxtDentContext db = new NexxtDentContext();

        // GET: CxCCanceleds
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var cxCCanceleds = db.CxCCanceleds.Where(c => c.CompanyId == user.CompanyId)
                .Include(c => c.Client)
                .Include(c => c.CxCBill)
                .Include(c => c.Reception)
                .Include(c => c.Sell);
            return View(cxCCanceleds.OrderByDescending(o=> o.Date).ToList());
        }

        // GET: CxCCanceleds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CxCCanceled cxCCanceled = db.CxCCanceleds.Find(id);
            if (cxCCanceled == null)
            {
                return HttpNotFound();
            }
            return View(cxCCanceled);
        }

        // GET: CxCCanceleds/Create
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "Cliente");
            ViewBag.CxCBillId = new SelectList(db.CxCBills, "CxCBillId", "NotaCobro");
            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente");
            ViewBag.SellId = new SelectList(db.Sells, "SellId", "NotaCobro");
            return View();
        }

        // POST: CxCCanceleds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CxCCanceled cxCCanceled)
        {
            if (ModelState.IsValid)
            {
                db.CxCCanceleds.Add(cxCCanceled);
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

            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "Cliente", cxCCanceled.ClientId);
            ViewBag.CxCBillId = new SelectList(db.CxCBills, "CxCBillId", "NotaCobro", cxCCanceled.CxCBillId);
            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente", cxCCanceled.ReceptionId);
            ViewBag.SellId = new SelectList(db.Sells, "SellId", "NotaCobro", cxCCanceled.SellId);
            return View(cxCCanceled);
        }

        // GET: CxCCanceleds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CxCCanceled cxCCanceled = db.CxCCanceleds.Find(id);
            if (cxCCanceled == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "Cliente", cxCCanceled.ClientId);
            ViewBag.CxCBillId = new SelectList(db.CxCBills, "CxCBillId", "NotaCobro", cxCCanceled.CxCBillId);
            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente", cxCCanceled.ReceptionId);
            ViewBag.SellId = new SelectList(db.Sells, "SellId", "NotaCobro", cxCCanceled.SellId);
            return View(cxCCanceled);
        }

        // POST: CxCCanceleds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CxCCanceled cxCCanceled)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cxCCanceled).State = EntityState.Modified;
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
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "Cliente", cxCCanceled.ClientId);
            ViewBag.CxCBillId = new SelectList(db.CxCBills, "CxCBillId", "NotaCobro", cxCCanceled.CxCBillId);
            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente", cxCCanceled.ReceptionId);
            ViewBag.SellId = new SelectList(db.Sells, "SellId", "NotaCobro", cxCCanceled.SellId);
            return View(cxCCanceled);
        }

        // GET: CxCCanceleds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CxCCanceled cxCCanceled = db.CxCCanceleds.Find(id);
            if (cxCCanceled == null)
            {
                return HttpNotFound();
            }
            return View(cxCCanceled);
        }

        // POST: CxCCanceleds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CxCCanceled cxCCanceled = db.CxCCanceleds.Find(id);
            db.CxCCanceleds.Remove(cxCCanceled);
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
            return View(cxCCanceled);
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
