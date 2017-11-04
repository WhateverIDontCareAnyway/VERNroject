using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PRINGUP_Proj.Models;
using PRINGUP_Proj.CustomFilters;
using Microsoft.AspNet.Identity;

namespace PRINGUP_Proj.Controllers
{
    public class IzvjestajsController : Controller
    {
        private IzvjestajiEntities1 db = new IzvjestajiEntities1();

        // GET: Izvjestajs
        [AuthLog(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Izvjestajs.ToList());
        }

        [Authorize]
        public ActionResult IndexUser()
        {         
            var user = User.Identity.GetUserId();
            var items = new List<Izvjestaj>();

            foreach (var item in db.Izvjestajs.ToList())
            {
                if (item.User_ID == user)
                {
                    items.Add(item);
                }
            }           
            return View(items);
        }

        // GET: Izvjestajs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Izvjestaj izvjestaj = db.Izvjestajs.Find(id);
            if (izvjestaj == null)
            {
                return HttpNotFound();
            }
            return View(izvjestaj);
        }

        // GET: Izvjestajs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Izvjestajs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Opis,Date_Posted,Time_Posted,Date_Edited,Time_Edited")] Izvjestaj izvjestaj)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.Identity.GetUserId();
                izvjestaj.User_ID = currentUserId;
                var creatorUserName = User.Identity.Name;
                izvjestaj.Username = creatorUserName;
                
                izvjestaj.Date_Posted = DateTime.Now.ToString("M/dd/yyyy");
                izvjestaj.Time_Posted = DateTime.Now.ToString("h:mm:ss tt");
                db.Izvjestajs.Add(izvjestaj);
                db.SaveChanges();
                return RedirectToAction("IndexUser");
            }

            return View(izvjestaj);
        }

        // GET: Izvjestajs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Izvjestaj izvjestaj = db.Izvjestajs.Find(id);
            if (izvjestaj == null)
            {
                return HttpNotFound();
            }
            return View(izvjestaj);
        }

        // POST: Izvjestajs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Opis,Date_Posted,Time_Posted,Date_Edited,Time_Edited")] Izvjestaj izvjestaj)
        {
            if (ModelState.IsValid)
            {
                db.Entry(izvjestaj).State = EntityState.Modified;
                izvjestaj.Date_Edited = DateTime.Now.ToString("M/dd/yyyy");
                izvjestaj.Time_Edited = DateTime.Now.ToString("h:mm:ss tt");
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(izvjestaj);
        }

        // GET: Izvjestajs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Izvjestaj izvjestaj = db.Izvjestajs.Find(id);
            if (izvjestaj == null)
            {
                return HttpNotFound();
            }
            return View(izvjestaj);
        }

        // POST: Izvjestajs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Izvjestaj izvjestaj = db.Izvjestajs.Find(id);
            db.Izvjestajs.Remove(izvjestaj);
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
