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

    public class ClientsController : Controller
    {
        private NexxtDentContext db = new NexxtDentContext();


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


        // GET: Clients
        public ActionResult Index(int? clientid)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (clientid != null)
            {
                var clients = db.Clients.Where(c => c.CompanyId == user.CompanyId && c.ClientId == clientid)
                     .Include(c => c.Identification)
                     .Include(c => c.City)
                     .Include(c => c.Zone);
                return View(clients.OrderBy(o => o.Cliente).ToList());
            }
            else
            {
                var clients = db.Clients.Where(c => c.CompanyId == user.CompanyId)
                     .Include(c => c.Identification)
                     .Include(c => c.City)
                     .Include(c => c.Zone);
                return View(clients.OrderBy(o => o.Cliente).ToList());
            }

        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var client = new Client
            {
                CompanyId = user.CompanyId,
                Date = DateTime.Today
            };

            ViewBag.LevelPriceId = new SelectList(ComboHelper.GetLevelPrice(), "LevelPriceId", "NivelPrecio");
            ViewBag.CityId = new SelectList(ComboHelper.GetCity(user.CompanyId), "CityId", "Ciudad");
            ViewBag.IdentificationId = new SelectList(ComboHelper.GetIdentifications(user.CompanyId), "IdentificationId", "TipoDocumento");
            ViewBag.ZoneId = new SelectList(ComboHelper.GetZone(user.CompanyId), "ZoneId", "Zona");

            return View(client);
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                try
                {
                    db.SaveChanges();
                    UsersHelper.CreateUserASP(client.UserName, "Client");
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

            ViewBag.LevelPriceId = new SelectList(ComboHelper.GetLevelPrice(), "LevelPriceId", "NivelPrecio", client.LevelPriceId);
            ViewBag.CityId = new SelectList(ComboHelper.GetCity(client.CompanyId), "CityId", "Ciudad", client.CityId);
            ViewBag.IdentificationId = new SelectList(ComboHelper.GetIdentifications(client.CompanyId), "IdentificationId", "TipoDocumento", client.IdentificationId);
            ViewBag.ZoneId = new SelectList(ComboHelper.GetZone(client.CompanyId), "ZoneId", "Zona", client.ZoneId);
            return View(client);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }

            ViewBag.LevelPriceId = new SelectList(ComboHelper.GetLevelPrice(), "LevelPriceId", "NivelPrecio", client.LevelPriceId);
            ViewBag.CityId = new SelectList(ComboHelper.GetCity(client.CompanyId), "CityId", "Ciudad", client.CityId);
            ViewBag.IdentificationId = new SelectList(ComboHelper.GetIdentifications(client.CompanyId), "IdentificationId", "TipoDocumento", client.IdentificationId);
            ViewBag.ZoneId = new SelectList(ComboHelper.GetZone(client.CompanyId), "ZoneId", "Zona", client.ZoneId);
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                try
                {
                    var db2 = new NexxtDentContext();
                    var currentClient = db2.Clients.Find(client.ClientId);
                    if (currentClient.UserName != client.UserName)
                    {
                        UsersHelper.UpdateUserName(currentClient.UserName, client.UserName);
                    }
                    db2.Dispose();

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

            ViewBag.LevelPriceId = new SelectList(ComboHelper.GetLevelPrice(), "LevelPriceId", "NivelPrecio", client.LevelPriceId);
            ViewBag.CityId = new SelectList(ComboHelper.GetCity(client.CompanyId), "CityId", "Ciudad", client.CityId);
            ViewBag.IdentificationId = new SelectList(ComboHelper.GetIdentifications(client.CompanyId), "IdentificationId", "TipoDocumento", client.IdentificationId);
            ViewBag.ZoneId = new SelectList(ComboHelper.GetZone(client.CompanyId), "ZoneId", "Zona", client.ZoneId);
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
            try
            {
                db.SaveChanges();
                UsersHelper.DeleteUser(client.UserName);

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
            return View(client);
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
