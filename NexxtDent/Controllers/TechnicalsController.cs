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

    public class TechnicalsController : Controller
    {
        private NexxtDentContext db = new NexxtDentContext();

        // GET: Technicals
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var technicals = db.Technicals.Where(c => c.CompanyId == user.CompanyId)
                .Include(t => t.City)
                .Include(t => t.Identification)
                .Include(t => t.WorkStation)
                .Include(t => t.Zone);
            return View(technicals.ToList());
        }

        // GET: Technicals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Technical technical = db.Technicals.Find(id);
            if (technical == null)
            {
                return HttpNotFound();
            }
            return View(technical);
        }

        // GET: Technicals/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var technicals = new Technical
            {
                CompanyId = user.CompanyId,
                Date = DateTime.Today,
                Activo = true
            };

            ViewBag.CityId = new SelectList(ComboHelper.GetCities(user.CompanyId), "CityId", "Ciudad");
            ViewBag.IdentificationId = new SelectList(ComboHelper.GetIdentifications(user.CompanyId), "IdentificationId", "TipoDocumento");
            ViewBag.WorkStationId = new SelectList(ComboHelper.GetWorkstation(user.CompanyId), "WorkStationId", "Estacion");
            ViewBag.ZoneId = new SelectList(ComboHelper.GetZone(user.CompanyId), "ZoneId", "Zona");
            return View(technicals);
        }

        // POST: Technicals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Technical technical)
        {
            if (ModelState.IsValid)
            {
                db.Technicals.Add(technical);
                try
                {
                    technical.FullName = technical.FirstName + " " + technical.LastName;
                    db.SaveChanges();

                    UsersHelper.CreateUserASP(technical.UserName, "Technical");


                    var usuario = new User
                    {
                        UserName = technical.UserName,
                        FirstName = technical.FirstName,
                        LastName = technical.LastName,
                        Phone = technical.Phone,
                        Address = technical.Address,
                        Puesto = "Tecnico",
                        CompanyId = technical.CompanyId
                    };
                    db.Users.Add(usuario);
                    db.SaveChanges();
                    db.Dispose();

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

            ViewBag.CityId = new SelectList(ComboHelper.GetCities(technical.CompanyId), "CityId", "Ciudad", technical.CityId);
            ViewBag.IdentificationId = new SelectList(ComboHelper.GetIdentifications(technical.CompanyId), "IdentificationId", "TipoDocumento", technical.IdentificationId);
            ViewBag.WorkStationId = new SelectList(ComboHelper.GetWorkstation(technical.CompanyId), "WorkStationId", "Estacion", technical.WorkStationId);
            ViewBag.ZoneId = new SelectList(ComboHelper.GetZone(technical.CompanyId), "ZoneId", "Zona", technical.ZoneId);
            return View(technical);
        }

        // GET: Technicals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Technical technical = db.Technicals.Find(id);
            if (technical == null)
            {
                return HttpNotFound();
            }

            ViewBag.CityId = new SelectList(ComboHelper.GetCities(technical.CompanyId), "CityId", "Ciudad", technical.CityId);
            ViewBag.IdentificationId = new SelectList(ComboHelper.GetIdentifications(technical.CompanyId), "IdentificationId", "TipoDocumento", technical.IdentificationId);
            ViewBag.WorkStationId = new SelectList(ComboHelper.GetWorkstation(technical.CompanyId), "WorkStationId", "Estacion", technical.WorkStationId);
            ViewBag.ZoneId = new SelectList(ComboHelper.GetZone(technical.CompanyId), "ZoneId", "Zona", technical.ZoneId);
            return View(technical);
        }

        // POST: Technicals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Technical technical)
        {
            if (ModelState.IsValid)
            {
                db.Entry(technical).State = EntityState.Modified;
                try
                {
                    if (technical.Activo == true)
                    {
                        var db2 = new NexxtDentContext();
                        var currentTech = db2.Technicals.Find(technical.TechnicalId);
                        if (currentTech.UserName != technical.UserName)
                        {
                            var db3 = new NexxtDentContext();
                            var usuarios = db3.Users.Where(c => c.UserName == currentTech.UserName && c.FirstName == currentTech.FirstName && c.LastName == currentTech.LastName).FirstOrDefault();
                            if (usuarios != null)
                            {
                                usuarios.UserName = technical.UserName;
                                db3.Entry(usuarios).State = EntityState.Modified;
                                db3.SaveChanges();
                                db3.Dispose();
                            }
                            else
                            {
                                var db4 = new NexxtDentContext();
                                var usuario = new User
                                {
                                    UserName = technical.UserName,
                                    FirstName = technical.FirstName,
                                    LastName = technical.LastName,
                                    Phone = technical.Phone,
                                    Address = technical.Address,
                                    Puesto = "Tecnico",
                                    CompanyId = technical.CompanyId
                                };
                                db4.Users.Add(usuario);
                                db4.SaveChanges();
                                db4.Dispose();
                                UsersHelper.CreateUserASP(technical.UserName, "Technical");
                            }
                            UsersHelper.UpdateUserName(currentTech.UserName, technical.UserName);
                        }
                        else
                        {
                            var db3 = new NexxtDentContext();
                            var usuarios = db3.Users.Where(c => c.UserName == currentTech.UserName && c.FirstName == currentTech.FirstName && c.LastName == currentTech.LastName).FirstOrDefault();
                            if (usuarios != null)
                            {
                                usuarios.UserName = technical.UserName;
                                db3.Entry(usuarios).State = EntityState.Modified;
                                db3.SaveChanges();
                                db3.Dispose();
                            }
                            else
                            {
                                var db4 = new NexxtDentContext();
                                var usuario = new User
                                {
                                    UserName = technical.UserName,
                                    FirstName = technical.FirstName,
                                    LastName = technical.LastName,
                                    Phone = technical.Phone,
                                    Address = technical.Address,
                                    Puesto = "Tecnico",
                                    CompanyId = technical.CompanyId
                                };
                                db4.Users.Add(usuario);
                                db4.SaveChanges();
                                db4.Dispose();
                                UsersHelper.CreateUserASP(technical.UserName, "Technical");
                            }
                            UsersHelper.UpdateUserName(currentTech.UserName, technical.UserName);
                        }
                    }

                    if (technical.Activo == false)
                    {
                        var db5 = new NexxtDentContext();
                        var currentTech2 = db5.Technicals.Find(technical.TechnicalId);

                        var db6 = new NexxtDentContext();
                        var usuarios = db6.Users.Where(c => c.UserName == currentTech2.UserName && c.FirstName == currentTech2.FirstName && c.LastName == currentTech2.LastName).FirstOrDefault();
                        if (usuarios != null)
                        {
                            db6.Users.Remove(usuarios);
                            db6.SaveChanges();
                            db6.Dispose();
                        }
                        UsersHelper.DeleteUser(technical.UserName);
                        //if (currentTech2.UserName != technical.UserName)
                        //{
                        //    UsersHelper.UpdateUserName(currentTech2.UserName, technical.UserName);
                        //}

                        db5.Dispose();
                    }
                    db.SaveChanges();

                    //Unir el primer y ultimo nombre para nombre completo
                    var db33 = new NexxtDentContext();
                    var tecnicos = db33.Technicals.Find(technical.TechnicalId);
                    tecnicos.FullName = tecnicos.FirstName + " " + tecnicos.LastName;
                    db33.Entry(tecnicos).State = EntityState.Modified;
                    db33.SaveChanges();
                    db33.Dispose();
                    //--------------------------------------------------------------
                    //..............................................................

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

            ViewBag.CityId = new SelectList(ComboHelper.GetCities(technical.CompanyId), "CityId", "Ciudad", technical.CityId);
            ViewBag.IdentificationId = new SelectList(ComboHelper.GetIdentifications(technical.CompanyId), "IdentificationId", "TipoDocumento", technical.IdentificationId);
            ViewBag.WorkStationId = new SelectList(ComboHelper.GetWorkstation(technical.CompanyId), "WorkStationId", "Estacion", technical.WorkStationId);
            ViewBag.ZoneId = new SelectList(ComboHelper.GetZone(technical.CompanyId), "ZoneId", "Zona", technical.ZoneId);
            return View(technical);
        }

        // GET: Technicals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Technical technical = db.Technicals.Find(id);
            if (technical == null)
            {
                return HttpNotFound();
            }
            return View(technical);
        }

        // POST: Technicals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Technical technical = db.Technicals.Find(id);
            db.Technicals.Remove(technical);
            try
            {
                db.SaveChanges();
                UsersHelper.DeleteUser(technical.UserName);

                var db6 = new NexxtDentContext();
                var usuarios = db6.Users.Where(c => c.UserName == technical.UserName && c.FirstName == technical.FirstName && c.LastName == technical.LastName).FirstOrDefault();
                if (usuarios != null)
                {
                    db6.Users.Remove(usuarios);
                    db6.SaveChanges();
                    db6.Dispose();
                }

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
            return View(technical);
        }

        public JsonResult GetZone(int cityId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var zones = db.Zones.Where(c => c.CityId == cityId);
            return Json(zones);
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
