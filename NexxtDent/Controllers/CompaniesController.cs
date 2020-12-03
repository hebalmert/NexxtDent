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
    [Authorize(Roles = "Admin")]

    public class CompaniesController : Controller
    {
        private NexxtDentContext db = new NexxtDentContext();

        // GET: Companies
        public ActionResult Index()
        {
            return View(db.Companies.ToList());
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            var compania = new Company
            {
                DateDesde = DateTime.Today,
                DateHasta = DateTime.Today,
                Activo = true
            };
            return View(compania);
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Companies.Add(company);
                    db.SaveChanges();

                    if (company.LogoFile != null)
                    {
                        var folder = "~/Content/Logos";
                        var file = string.Format("{0}.jpg", company.CompanyId);
                        var response = FilesHelper.UploadPhoto(company.LogoFile, folder, file);

                        if (response)
                        {
                            var pic = string.Format("{0}/{1}", folder, file);
                            company.Logo = pic;
                            db.Entry(company).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    // Se crea el Registro de Control de Consecutivos de la Compania
                    var db2 = new NexxtDentContext();
                    var registro = new Register
                    {
                        CompanyId = company.CompanyId,
                        Recepcion = 0,
                        PagoTecnicos = 0,
                        NotaCobro = 0,
                        Factura = 0,
                        Egresos = 0
                    };
                    db2.Registers.Add(registro);
                    db2.SaveChanges();
                    db2.Dispose();
                    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

                    // Se crea el Tipo Documento por defecto
                    var db5 = new NexxtDentContext();
                    var identification = new Identification
                    {
                        CompanyId = company.CompanyId,
                        TipoDocumento = "Nit/CI"
                    };

                    db5.Identifications.Add(identification);
                    db5.SaveChanges();
                    db5.Dispose();
                    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

                    // Se crea el HeadText de Control de Consecutivos de la Compania
                    var db3 = new NexxtDentContext();
                    var headtext = new HeadText
                    {
                        CompanyId = company.CompanyId,
                        TextoEncabezado = @Resources.Resource.TextHead_Default
                    };

                    db3.HeadTexts.Add(headtext);
                    db3.SaveChanges();
                    db3.Dispose();
                    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

                    // Se crea el SupportText de Control de Consecutivos de la Compania
                    var db4 = new NexxtDentContext();
                    var supporttext = new SupportText
                    {
                        CompanyId = company.CompanyId,
                        TextoCondiciones = @Resources.Resource.SupportText_Default
                    };

                    db4.SupportTexts.Add(supporttext);
                    db4.SaveChanges();
                    db4.Dispose();
                    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


                    // Se crea el Impuesto por defecto en Cero (0)
                    var db6 = new NexxtDentContext();
                    var tax = new Tax
                    {
                        CompanyId = company.CompanyId,
                        Impuesto = "IVA 0 %",
                        Rate = 0
                    };

                    db6.Taxes.Add(tax);
                    db6.SaveChanges();
                    db6.Dispose();

                    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
                    // Si El tecnico cobra por porcentaje
                    var porcentajetecnicos = new TechnicalPorcentage
                    {
                        CompanyId = company.CompanyId,
                        CobraPorcentaje = true
                    };

                    db.TechnicalPorcentages.Add(porcentajetecnicos);
                    db.SaveChanges();
                    db.Dispose();
                    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::



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


                return RedirectToAction("Index");
            }

            return View(company);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                try
                {
                    if (company.LogoFile != null)
                    {
                        var pic = string.Empty;
                        var folder = "~/Content/Logos";
                        var file = string.Format("{0}.jpg", company.CompanyId);
                        var response = FilesHelper.UploadPhoto(company.LogoFile, folder, file);

                        if (response)
                        {
                            pic = string.Format("{0}/{1}", folder, file);
                            company.Logo = pic;
                        }
                    }

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
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
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
            return View(company);
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
