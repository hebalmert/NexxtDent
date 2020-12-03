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

    public class TechnicalPaysController : Controller
    {
        private NexxtDentContext db = new NexxtDentContext();


        // PrintSell: PrintSell/Create
        public ActionResult PrintSell(int id)
        {
            //var cxcbill = db.CxCBills.Find(id);
            //int idVenta = cxcbill.SellId;
            int Recep = 0;
            int sum = 0;
            var db2 = new NexxtDentContext();
            var db3 = new NexxtDentContext();
            var technicalpays = db3.TechnicalPays.Find(id);

            if (technicalpays.NFactura == null)
            {
                var register = db2.Registers.Where(c => c.CompanyId == technicalpays.CompanyId).FirstOrDefault();
                Recep = register.Factura;
                sum = Recep + 1;
                register.Recepcion = sum;
                db2.Entry(register).State = EntityState.Modified;
                db2.SaveChanges();

                technicalpays.NFactura = Convert.ToString(sum);

                //venta.DatePagado = DateTime.Today;
                db3.Entry(technicalpays).State = EntityState.Modified;
                db3.SaveChanges();

            }
            db2.Dispose();

            return View(technicalpays);
        }

        // GET: AnularContract/Create
        public ActionResult AddPago(int id, decimal deuda)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var technicalpaydetails = new TechnicalPayDetail
            {
                CompanyId = user.CompanyId,
                TechnicalPayId = id,
                ModoPago = @Resources.Resource.ModoPago_Efectivo,
                Date = DateTime.Today,
                Deuda = deuda,
                Abono = 0,
                Saldo = deuda
            };

            return PartialView(technicalpaydetails);
        }

        // POST: AnularContract/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPago(TechnicalPayDetail technicalPayDetail)
        {
            if (ModelState.IsValid)
            {
                db.TechnicalPayDetails.Add(technicalPayDetail);
                try
                {
                    if (technicalPayDetail.Abono > 0)
                    {
                        decimal Tabono = 0;
                        decimal Vabono = 0;
                        var db2 = new NexxtDentContext();
                        var technicalpay = db2.TechnicalPays.Find(technicalPayDetail.TechnicalPayId);
                        Vabono = technicalpay.Abono;
                        Tabono = Vabono + technicalPayDetail.Abono;

                        technicalpay.Abono = Tabono;
                        technicalpay.Saldo = technicalPayDetail.Saldo;
                        if (technicalPayDetail.Saldo == 0)
                        {
                            technicalpay.Pagado = true;
                        }
                        else
                        {
                            technicalpay.Pagado = false;
                        }
                        technicalpay.DatePagado = DateTime.Today;
                        db2.Entry(technicalpay).State = EntityState.Modified;
                        db2.SaveChanges();


                        ////Se Agrega a Ingresos Diarios el valor del contrato
                        //var db41 = new NexxtDentContext();
                        //var ingreso = new Income
                        //{
                        //    CompanyId = technicalpay.CompanyId,
                        //    Date = DateTime.Today,
                        //    Source = @Resources.Resource.TechnicalPay_PagoTecnicos,
                        //    Control = Convert.ToString(technicalpay.Reception.Recepcion),
                        //    Detalle = @Resources.Resource.TechnicalPay_PagoTecnicos,
                        //    Monto = technicalPayDetail.Abono
                        //};
                        //db41.Incomes.Add(ingreso);
                        //db41.SaveChanges();
                        //db41.Dispose();
                        ////Se termina el registro nuevo de ingresos

                        db.SaveChanges();
                        db2.Dispose();
                        //db41.Dispose();

                        return RedirectToAction("Details", new { id = technicalPayDetail.TechnicalPayId });
                    }

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

            ModelState.AddModelError(string.Empty, (@Resources.Resource.Msg_SaldoMayorCero));

            return PartialView(technicalPayDetail);
        }

        // GET: TechnicalPays
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
                var technicalPays = db.TechnicalPays.Where(c => c.CompanyId == user.CompanyId && c.Anulado == false && c.Technical.FullName.Contains(searchString))
                .Include(t => t.Reception)
                .Include(t => t.Technical);
                return View(technicalPays.ToList().ToPagedList((int)page, 10));
            }
            else
            {
                var technicalPays = db.TechnicalPays.Where(c => c.CompanyId == user.CompanyId && c.Anulado == false)
                .Include(t => t.Reception)
                .Include(t => t.Technical);
                return View(technicalPays.ToList().ToPagedList((int)page, 10));
            }
        }

        // GET: TechnicalPays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TechnicalPay technicalPay = db.TechnicalPays.Find(id);
            if (technicalPay == null)
            {
                return HttpNotFound();
            }
            return View(technicalPay);
        }

        // GET: TechnicalPays/Create
        public ActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Compania");
            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente");
            ViewBag.TechnicalId = new SelectList(db.Technicals, "TechnicalId", "FirstName");
            return View();
        }

        // POST: TechnicalPays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TechnicalPayId,CompanyId,ReceptionId,TechnicalId,Servicio,Precio,Cantidad,Total,Tasa,Facturado,NotaPago")] TechnicalPay technicalPay)
        {
            if (ModelState.IsValid)
            {
                db.TechnicalPays.Add(technicalPay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Compania", technicalPay.CompanyId);
            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente", technicalPay.ReceptionId);
            ViewBag.TechnicalId = new SelectList(db.Technicals, "TechnicalId", "FirstName", technicalPay.TechnicalId);
            return View(technicalPay);
        }

        // GET: TechnicalPays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TechnicalPay technicalPay = db.TechnicalPays.Find(id);
            if (technicalPay == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Compania", technicalPay.CompanyId);
            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente", technicalPay.ReceptionId);
            ViewBag.TechnicalId = new SelectList(db.Technicals, "TechnicalId", "FirstName", technicalPay.TechnicalId);
            return View(technicalPay);
        }

        // POST: TechnicalPays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TechnicalPayId,CompanyId,ReceptionId,TechnicalId,Servicio,Precio,Cantidad,Total,Tasa,Facturado,NotaPago")] TechnicalPay technicalPay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(technicalPay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Compania", technicalPay.CompanyId);
            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente", technicalPay.ReceptionId);
            ViewBag.TechnicalId = new SelectList(db.Technicals, "TechnicalId", "FirstName", technicalPay.TechnicalId);
            return View(technicalPay);
        }

        // GET: TechnicalPays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TechnicalPay technicalPay = db.TechnicalPays.Find(id);
            if (technicalPay == null)
            {
                return HttpNotFound();
            }
            return View(technicalPay);
        }

        // POST: TechnicalPays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TechnicalPay technicalPay = db.TechnicalPays.Find(id);
            db.TechnicalPays.Remove(technicalPay);
            db.SaveChanges();
            return RedirectToAction("Index");
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
