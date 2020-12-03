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

    public class CxCBillsController : Controller
    {
        private NexxtDentContext db = new NexxtDentContext();


        // GET: Anular/Anular
        public ActionResult Anular(int id)
        {
            var db4 = new NexxtDentContext();
            var db3 = new NexxtDentContext();
            var db2 = new NexxtDentContext();
            var cxcbill = db.CxCBills.Find(id);
            cxcbill.Anulado = true;
            cxcbill.DateAnulado = DateTime.Today;

            var cxcbillcancel = new CxCCanceled
            { 
                CompanyId = cxcbill.CompanyId,
                Date = DateTime.Today,
                SellId = cxcbill.SellId,
                CxCBillId = cxcbill.CxCBillId,
                NotaCobro = cxcbill.NotaCobro,
                ClientId = cxcbill.ClientId,
                ReceptionId = cxcbill.ReceptionId,
                Total = cxcbill.Total,
                Abono = cxcbill.Abono,
                Saldo = cxcbill.Saldo
            };
            db2.CxCCanceleds.Add(cxcbillcancel);
            db2.SaveChanges();

            var sells = db3.Sells.Find(cxcbill.SellId);
            sells.AnuladaFactura = true;
            sells.DateAnulada = DateTime.Today;
            db3.Entry(sells).State = EntityState.Modified;
            db3.SaveChanges();
            db3.Dispose();

            var db8 = new NexxtDentContext();
            var recep = db8.Receptions.Find(cxcbill.ReceptionId);
            recep.StateId = 9;
            recep.DateAnulado = DateTime.Today;
            db8.Entry(recep).State = EntityState.Modified;
            db8.SaveChanges();
            db8.Dispose();

            var db9 = new NexxtDentContext();
            var deli = db9.Deliveries.Where(c=> c.ReceptionId == cxcbill.ReceptionId).FirstOrDefault();
            deli.StateId = 9;
            db9.Entry(deli).State = EntityState.Modified;
            db9.SaveChanges();
            db9.Dispose();


            var technicalpaies = db4.TechnicalPays.Where(r=> r.ReceptionId == cxcbill.ReceptionId).ToList();
            if (technicalpaies != null)
            {
                var db5 = new NexxtDentContext();
                foreach (var item in technicalpaies)
                {
                    var technicalpay2 = db5.TechnicalPays.Find(item.TechnicalPayId);
                    technicalpay2.Anulado = true;
                    technicalpay2.DateAnulado = DateTime.Today;
                    db5.Entry(technicalpay2).State = EntityState.Modified;
                    db5.SaveChanges();
                }
                db5.Dispose();
                db4.Dispose();
            }
            else
            {
                db4.Dispose();
            }
            db.Entry(cxcbill).State = EntityState.Modified;
            db.SaveChanges();
            db2.Dispose();

            return RedirectToAction("Index");
        }


        // GET: CxCBills
        public ActionResult PrintReport(DateTime fechaInicio, DateTime fechafin, int? page = null)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            page = (page ?? 1);

            var cxCBills = db.CxCBills.Where(c => c.CompanyId == user.CompanyId && c.Date >= fechaInicio && c.Date <= fechafin && c.Anulado == false)
                 .Include(c => c.Client)
                 .Include(c => c.Reception)
                 .Include(c => c.Sell);

            return View(cxCBills.OrderBy(c => c.Client.Cliente).ThenByDescending(t => t.Date).ToList().ToPagedList((int)page, 5));
        }

        //Get: PrintSell: PrintCxCReport/Create
        public ActionResult PrintCxCReport()
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

        // Post: PrintCxCReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrintCxCReport(PrintViewDate printViewdate)
        {

            return RedirectToAction("PrintReport", new { fechaInicio = printViewdate.DateInicio, fechafin = printViewdate.DateFin });
        }

        // PrintSell: PrintFactSell/Create
        public ActionResult PrintFactSell(int id)
        {
            var cxcbill = db.CxCBills.Find(id);
            int idVenta = cxcbill.SellId;
            int Recep = 0;
            int sum = 0;
            var db2 = new NexxtDentContext();
            var db3 = new NexxtDentContext();
            var venta = db3.Sells.Find(idVenta);

            if (venta.NFactura == null)
            {
                var register = db2.Registers.Where(c => c.CompanyId == venta.CompanyId).FirstOrDefault();
                Recep = register.Factura;
                sum = Recep + 1;
                register.Recepcion = sum;
                db2.Entry(register).State = EntityState.Modified;
                db2.SaveChanges();
                

                venta.NFactura = Convert.ToString(sum);
                venta.Control = Convert.ToString(sum);
                venta.Impresa = true;
                //venta.DatePagado = DateTime.Today;
                db3.Entry(venta).State = EntityState.Modified;
                db3.SaveChanges();
            }
            db2.Dispose();
            //db3.Dispose();

            return View(venta);
        }

        // PrintSell: PrintSell/Create
        public ActionResult PrintSell(int id)
        {
            var cxcbill = db.CxCBills.Find(id);
            int idVenta = cxcbill.SellId;

            var db2 = new NexxtDentContext();
            var venta = db2.Sells.Find(idVenta);

            return View(venta);
        }

        // GET: AnularContract/Create
        public ActionResult AddPago(int id, decimal deuda)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var cxcbilldetail = new CxCBillDetail
            {
                CompanyId = user.CompanyId,
                CxCBillId = id,
                ModoPago = @Resources.Resource.ModoPago_Efectivo,
                Date = DateTime.Today,
                Deuda = deuda,
                Abono = 0,
                Saldo = deuda
            };

            return PartialView(cxcbilldetail);
        }

        // POST: AnularContract/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPago(CxCBillDetail cxcbilldetail)
        {
            if (ModelState.IsValid)
            {
                db.CxCBillDetails.Add(cxcbilldetail);
                try
                {
                    if (cxcbilldetail.Abono > 0)
                    {
                        decimal Tabono = 0;
                        decimal Vabono = 0;
                        var db2 = new NexxtDentContext();
                        var cxcbill = db2.CxCBills.Find(cxcbilldetail.CxCBillId);
                        Vabono = cxcbill.Abono;
                        Tabono = Vabono + cxcbilldetail.Abono;

                        cxcbill.Abono = Tabono;
                        cxcbill.Saldo = cxcbilldetail.Saldo;
                        if (cxcbilldetail.Saldo == 0)
                        {
                            cxcbill.Pagado = true;
                        }
                        else
                        {
                            cxcbill.Pagado = false;
                        }
                        cxcbill.DatePagado = DateTime.Today;
                        db2.Entry(cxcbill).State = EntityState.Modified;
                        db2.SaveChanges();


                        //SeAgrega a Ingresos Diarios el valor del contrato
                        var db41 = new NexxtDentContext();
                        var ingreso = new Income
                        {
                            CompanyId = cxcbill.CompanyId,
                            Date = DateTime.Today,                            
                            Source = @Resources.Resource.CxC_ViewIndex_CuentasXcobrar,
                            Control = cxcbill.NotaCobro,
                            Detalle = @Resources.Resource.CxC_ViewIndex_CuentasXcobrar,
                            Monto = cxcbilldetail.Abono
                        };
                        db41.Incomes.Add(ingreso);
                        db41.SaveChanges();

                        //Se termina el registro nuevo de ingresos

                        db.SaveChanges();
                        db2.Dispose();
                        //db41.Dispose();

                        return RedirectToAction("Details", new { id = cxcbilldetail.CxCBillId });
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

            return PartialView(cxcbilldetail);
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

        // GET: CxCBills
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
                var cxCBills = db.CxCBills.Where(c => c.CompanyId == user.CompanyId && c.Anulado == false && c.ClientId == clientid)
                .Include(c => c.Client)
                .Include(c => c.Reception)
                .Include(c => c.Sell);
                return View(cxCBills.OrderBy(c => c.Client.Cliente).ThenByDescending(t => t.Date).ToList().ToPagedList((int)page, 25));
            }
            else 
            {
                var cxCBills = db.CxCBills.Where(c => c.CompanyId == user.CompanyId && c.Anulado == false)
                .Include(c => c.Client)
                .Include(c => c.Reception)
                .Include(c => c.Sell);
                return View(cxCBills.OrderBy(c => c.Client.Cliente).ThenByDescending(t => t.Date).ToList().ToPagedList((int)page, 25));
            }
            
        }

        // GET: CxCBills/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CxCBill cxCBill = db.CxCBills.Find(id);
            if (cxCBill == null)
            {
                return HttpNotFound();
            }
            return View(cxCBill);
        }

        // GET: CxCBills/Create
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "Cliente");
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Compania");
            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente");
            ViewBag.SellId = new SelectList(db.Sells, "SellId", "NotaCobro");
            return View();
        }

        // POST: CxCBills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CxCBillId,CompanyId,SellId,NotaCobro,Date,ClientId,ReceptionId,Total,Abono,Saldo,Pagado,DatePagado,Anulado")] CxCBill cxCBill)
        {
            if (ModelState.IsValid)
            {
                db.CxCBills.Add(cxCBill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "Cliente", cxCBill.ClientId);
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Compania", cxCBill.CompanyId);
            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente", cxCBill.ReceptionId);
            ViewBag.SellId = new SelectList(db.Sells, "SellId", "NotaCobro", cxCBill.SellId);
            return View(cxCBill);
        }

        // GET: CxCBills/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CxCBill cxCBill = db.CxCBills.Find(id);
            if (cxCBill == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "Cliente", cxCBill.ClientId);
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Compania", cxCBill.CompanyId);
            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente", cxCBill.ReceptionId);
            ViewBag.SellId = new SelectList(db.Sells, "SellId", "NotaCobro", cxCBill.SellId);
            return View(cxCBill);
        }

        // POST: CxCBills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CxCBillId,CompanyId,SellId,NotaCobro,Date,ClientId,ReceptionId,Total,Abono,Saldo,Pagado,DatePagado,Anulado")] CxCBill cxCBill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cxCBill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "Cliente", cxCBill.ClientId);
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Compania", cxCBill.CompanyId);
            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente", cxCBill.ReceptionId);
            ViewBag.SellId = new SelectList(db.Sells, "SellId", "NotaCobro", cxCBill.SellId);
            return View(cxCBill);
        }

        // GET: CxCBills/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CxCBill cxCBill = db.CxCBills.Find(id);
            if (cxCBill == null)
            {
                return HttpNotFound();
            }
            return View(cxCBill);
        }

        // POST: CxCBills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CxCBill cxCBill = db.CxCBills.Find(id);
            db.CxCBills.Remove(cxCBill);
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
