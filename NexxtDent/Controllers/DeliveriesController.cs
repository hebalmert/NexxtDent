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

    public class DeliveriesController : Controller
    {
        private NexxtDentContext db = new NexxtDentContext();

        // GET: AddDelivery/Create
        public ActionResult AddDelivery(int id)
        {

            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var deliverydetail = new DeliveryDetail
            {
                DeliveryId = id,
                Date = DateTime.Today
            };

            return View(deliverydetail);
        }

        // POST: AddDelivery/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDelivery(DeliveryDetail deliverydetail)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.DeliveryDetails.Add(deliverydetail);
                    db.SaveChanges();

                    int EstadoReal = 6;

                    var delivery = db.Deliveries.Find(deliverydetail.DeliveryId);
                    var technicalwork = delivery.TechnicalWorkId;
                    int RecepNumero = delivery.ReceptionId;
                    int RepAssingNumero = delivery.ReceptionAssignId;
                    delivery.StateId = EstadoReal;
                    db.Entry(delivery).State = EntityState.Modified;
                    db.SaveChanges();

                    var Nreception = db.TechnicalWorks.Where(c=> c.ReceptionId == RecepNumero).ToList();
                    if (Nreception != null)
                    {
                        var db777 = new NexxtDentContext();
                        foreach (var item in Nreception)
                        {
                            
                            var technicalreceptionUpDate = db777.TechnicalWorks.Find(item.TechnicalWorkId);
                            technicalreceptionUpDate.StateId = EstadoReal;
                            db777.Entry(technicalreceptionUpDate).State = EntityState.Modified;                           
                        }
                        db777.SaveChanges();
                        db777.Dispose();
                    }

                    var reception = db.Receptions.Find(RecepNumero);
                    var clienteNumero = reception.ClientId;
                    var clienteNombre = reception.Client.Cliente;
                    var Nrecepcion = reception.Recepcion;
                    var telefono = reception.Client.Phone;
                    var direccion = reception.Client.Address;
                    var zona = reception.Client.Zone.Zona;
                    var TipoDocu = reception.Client.Identification.TipoDocumento;
                    var NDocu = reception.Client.IdentificationNumber;
                    reception.StateId = EstadoReal;
                    db.Entry(reception).State = EntityState.Modified;
                    db.SaveChanges();

                    var receptionassign = db.ReceptionAssigns.Find(RepAssingNumero);
                    receptionassign.StateId = EstadoReal;
                    db.Entry(receptionassign).State = EntityState.Modified;
                    db.SaveChanges();

                    //.......................//
                    //...Se culmino la actualizacion de los StateId en los Reception, Asignacion y Trabajo Tecnico.
                    //...Se constinua con Agregar el Trabajo a Cuentas por Cobrar del Cliente.

                    int VentaId = 0;

                    int CompanyUser = reception.CompanyId;  //Buscamos Directamente el Company Id para el Sell.CompanyId           

                    decimal SumSubTotal = 0;
                    decimal SumImpuesto = 0;
                    decimal SumTotal = 0;
                    decimal subTotal = 0;
                    decimal Impuesto = 0;
                    decimal TotalPrecio = 0;
                    decimal Abono = 0;
                    decimal Saldo = 0;
                    int SumNota = 0;            //Contador de Notas Cobros para consecutivo

                    //Se prepara el numero de correlativo Nota Cobro para agregarlos en las tablas
                    var register = db.Registers.Where(c => c.CompanyId == CompanyUser).FirstOrDefault();
                    int nota = register.NotaCobro;
                    SumNota = nota + 1;
                    register.NotaCobro = SumNota;
                    db.Entry(register).State = EntityState.Modified;
                    db.SaveChanges();

                    //..................................................................................

                    //Se Ingresa el Texto del Encabezado para Imprimir la Factura y tener como vincular el Texto de Encabezado
                    var encabezado = db.HeadTexts.Where(c => c.CompanyId == CompanyUser).FirstOrDefault();
                    int cabezaId = encabezado.HeadTextId;
                    //..................................................................................

                    // Ingresar las Ventas con Saldos en Cero (0) al final solo se actualizan los montos
                    //TODO: Hay que reviar el Reception, se cierra y no pasa de este punto.
                    var db21 = new NexxtDentContext();  // Nuevo Sell
                    var venta = new Sell
                    {
                        CompanyId = CompanyUser,
                        NotaCobro = Convert.ToString(SumNota),
                        HeadTextId = cabezaId,
                        Date = DateTime.Today,
                        ReceptionId = RecepNumero,
                        Recepcion = Nrecepcion,
                        ClientId = clienteNumero,
                        Cliente = clienteNombre,
                        TipoDocumento = TipoDocu,
                        IdentificationNumber = NDocu,
                        Phone = telefono,
                        Address = direccion,
                        Zona = zona,
                        DateVencimiento = DateTime.Today, //DateTime.Today.AddDays(5),
                        Condicion = "Credito",
                        SubTotal = 0,
                        Iva = 0,
                        Total = 0,
                        DateAnulada = DateTime.Today
                    };
                    db21.Sells.Add(venta);
                    db21.SaveChanges();
                    VentaId = venta.SellId;
                    db21.Dispose();
                    //  fin ingreso de Ventas con saldo sen Ceros(0)
                    //......................................................................


                    //......................................................................
                    //.... Se revisan los Trabajos Contratados por el cliente en Reception Work
                    var db22 = new NexxtDentContext();  
                    var receptionwork = db22.ReceptionWorks.Where(c => c.ReceptionId == RecepNumero).ToList();
                    if (receptionwork != null)
                    {
                        foreach (var item in receptionwork)
                        {
                            decimal tasa = item.Tasa;
                            decimal Precio = item.Precio;
                            decimal total = item.Total;
                            int cantidad = item.Cantidad;
                            decimal iva = 0;
                            
                            if (tasa == 0)
                            {
                                iva = 0;
                                subTotal = Precio;
                                Impuesto = 0;
                                TotalPrecio = total;


                                SumSubTotal += total;
                                SumImpuesto += Impuesto;
                                SumTotal += TotalPrecio;

                            }
                            else
                            {
                                iva = tasa + 1;
                                //Detalle de Unitario, Cantidad y Total de cada Item de la Factura

                                subTotal = (Precio / iva);                      //Precio Unitario del Servicioi
                                Impuesto = (Precio - subTotal) * cantidad;      //Impuesto Total generado del Unitario * Cantidad
                                TotalPrecio = subTotal * cantidad;              // Total por Item en la Factura

                                SumSubTotal += TotalPrecio;
                                SumImpuesto += Impuesto;
                                SumTotal += total;
                            }
                            
                            //Agregamos los detalles de Items a Ventas
                            //.............................................
                            var db24 = new NexxtDentContext();  // Nuevo Sell
                            var ventaDetalle = new SellDetail
                            {
                                SellId =Convert.ToInt32(VentaId),
                                Codigo = Convert.ToString(item.WorkId),
                                Concepto = item.Trabajo,
                                Cantidad = cantidad,
                                Unitario = subTotal,
                                Total = TotalPrecio,
                                Tasa = tasa
                            };
                            db24.SellDetails.Add(ventaDetalle);
                            db24.SaveChanges();
                            db24.Dispose();
                            //....FIN
                            //........................................... 
                        }
                        db22.Dispose();
                    }
                    else
                    {
                        db22.Dispose();
                    }

                    var db30 = new NexxtDentContext();  // Nuevo Sell
                    var cuentasxcobrar = new CxCBill
                    {
                        CompanyId = CompanyUser,
                        SellId = VentaId,
                        NotaCobro = Convert.ToString(SumNota),
                        Date = DateTime.Today,
                        ClientId = clienteNumero,
                        ReceptionId = RecepNumero,
                        Total = SumTotal,
                        Abono = Abono,
                        Saldo = SumTotal, //Saldo,
                        Pagado = false,
                        DatePagado = DateTime.Today,
                        DateAnulado = DateTime.Today
                    };
                    db30.CxCBills.Add(cuentasxcobrar);
                    db30.SaveChanges();
                    db30.Dispose();
                    //....FIN
                    //...........................................

                    var pagoporcentaje = db.TechnicalPorcentages.Where(c => c.CompanyId == CompanyUser).FirstOrDefault();
                    if (pagoporcentaje.CobraPorcentaje == true)
                    {
                        var db91 = new NexxtDentContext();
                        var techwork = db91.TechnicalWorks.Where(c => c.ReceptionId == RecepNumero).ToList();
                        if (techwork != null)
                        {

                            var db90 = new NexxtDentContext();  // Nuevo Servicios de los Tecnicos para pagar
                            foreach (var item in techwork)
                            {
                                var technicalpay = new TechnicalPay
                                {
                                    CompanyId = CompanyUser,
                                    ReceptionId = RecepNumero,
                                    TechnicalId = item.TechnicalId,
                                    Servicio = item.Servicio,
                                    Precio = item.Precio,
                                    Cantidad = item.Cantidad,
                                    Total = item.Total,
                                    Abono = 0,
                                    Saldo = item.Total,
                                    Tasa = item.Tasa,
                                    DatePagado = DateTime.Today,
                                    DateAnulado = DateTime.Today
                                };
                                db90.TechnicalPays.Add(technicalpay);
                                db90.SaveChanges();
                            }
                            db90.Dispose();
                            db91.Dispose();
                            //....FIN
                            //...........................................
                        }
                        else
                        {
                            db91.Dispose();
                        }
                    }


                    var db70 = new NexxtDentContext();
                    var UpSell = db70.Sells.Find(VentaId);
                    UpSell.SubTotal = subTotal;
                    UpSell.Iva = Impuesto;
                    UpSell.Total = TotalPrecio;
                    db70.Entry(UpSell).State = EntityState.Modified;
                    db70.SaveChanges();
                    db70.Dispose();                   
                    
                    return RedirectToAction("Details", new { id = deliverydetail.DeliveryId });
                }

                catch (Exception ex)
                {
                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("_Index"))
                    {
                        ModelState.AddModelError(string.Empty, ("Existe un Registro con el mismo Valor"));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }

            return View(deliverydetail);
        }

        // GET: Deliveries
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var deliveries = db.Deliveries.Where(c=> c.CompanyId == user.CompanyId)
                .Include(d => d.Client)
                .Include(d => d.Reception)
                .Include(d => d.ReceptionAssign)
                .Include(d => d.State)
                .Include(d => d.TechnicalWork);
            return View(deliveries.ToList());
        }

        // GET: Deliveries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = db.Deliveries.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return View(delivery);
        }

        // GET: Deliveries/Create
        public ActionResult Create()
        {

            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "Cliente");
            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente");
            ViewBag.ReceptionAssignId = new SelectList(db.ReceptionAssigns, "ReceptionAssignId", "ReceptionAssignId");
            ViewBag.StateId = new SelectList(db.States, "StateId", "Estado");
            ViewBag.TechnicalWorkId = new SelectList(db.TechnicalWorks, "TechnicalWorkId", "Servicio");
            return View();
        }

        // POST: Deliveries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Delivery delivery)
        {
            if (ModelState.IsValid)
            {
                db.Deliveries.Add(delivery);
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

            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "Cliente", delivery.ClientId);
            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente", delivery.ReceptionId);
            ViewBag.ReceptionAssignId = new SelectList(db.ReceptionAssigns, "ReceptionAssignId", "ReceptionAssignId", delivery.ReceptionAssignId);
            ViewBag.StateId = new SelectList(db.States, "StateId", "Estado", delivery.StateId);
            ViewBag.TechnicalWorkId = new SelectList(db.TechnicalWorks, "TechnicalWorkId", "Servicio", delivery.TechnicalWorkId);
            return View(delivery);
        }

        // GET: Deliveries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = db.Deliveries.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "Cliente", delivery.ClientId);
            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente", delivery.ReceptionId);
            ViewBag.ReceptionAssignId = new SelectList(db.ReceptionAssigns, "ReceptionAssignId", "ReceptionAssignId", delivery.ReceptionAssignId);
            ViewBag.StateId = new SelectList(db.States, "StateId", "Estado", delivery.StateId);
            ViewBag.TechnicalWorkId = new SelectList(db.TechnicalWorks, "TechnicalWorkId", "Servicio", delivery.TechnicalWorkId);
            return View(delivery);
        }

        // POST: Deliveries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Delivery delivery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(delivery).State = EntityState.Modified;
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
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "Cliente", delivery.ClientId);
            ViewBag.ReceptionId = new SelectList(db.Receptions, "ReceptionId", "Paciente", delivery.ReceptionId);
            ViewBag.ReceptionAssignId = new SelectList(db.ReceptionAssigns, "ReceptionAssignId", "ReceptionAssignId", delivery.ReceptionAssignId);
            ViewBag.StateId = new SelectList(db.States, "StateId", "Estado", delivery.StateId);
            ViewBag.TechnicalWorkId = new SelectList(db.TechnicalWorks, "TechnicalWorkId", "Servicio", delivery.TechnicalWorkId);
            return View(delivery);
        }

        // GET: Deliveries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = db.Deliveries.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return View(delivery);
        }

        // POST: Deliveries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Delivery delivery = db.Deliveries.Find(id);
            db.Deliveries.Remove(delivery);
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
            return View(delivery);
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
