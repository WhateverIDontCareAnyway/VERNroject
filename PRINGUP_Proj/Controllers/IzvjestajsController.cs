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
using Rotativa;


namespace PRINGUP_Proj.Controllers
{
    public class IzvjestajsController : Controller
    {
        private IzvjestajiEntities1 db = new IzvjestajiEntities1();

        // GET: Izvjestajs
        [AuthLog(Roles = "Admin")]
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.UserSortParm = sortOrder == "User" ? "user_desc" : "User";
            var izvjestajs = from i in db.Izvjestajs
                             select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                izvjestajs = izvjestajs.Where(s => s.Name.Contains(searchString)
                                       || s.Username.Contains(searchString)
                                       || s.Date_Posted.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    izvjestajs = izvjestajs.OrderByDescending(i => i.Name);
                    break;
                case "Date":
                    izvjestajs = izvjestajs.OrderBy(i => i.Date_Posted);
                    break;
                case "date_desc":
                    izvjestajs = izvjestajs.OrderByDescending(i => i.Date_Posted);
                    break;
                case "User":
                    izvjestajs = izvjestajs.OrderBy(i => i.Username);
                    break;
                case "user_desc":
                    izvjestajs = izvjestajs.OrderByDescending(i => i.Username);
                    break;
                default:
                    izvjestajs = izvjestajs.OrderBy(i => i.Name);
                    break;
            }

            return View(izvjestajs.ToList());
        }

        [Authorize]
        public ActionResult IndexUser(string sortOrder, string searchString)
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

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.DateEditSortParm = sortOrder == "DateEdit" ? "dateEdit_desc" : "DateEdit";
            var izvjestajs = from i in items
                             select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                izvjestajs = izvjestajs.Where(s => s.Name.Contains(searchString)
                                       || s.Date_Posted.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    izvjestajs = izvjestajs.OrderByDescending(i => i.Name);
                    break;
                case "Date":
                    izvjestajs = izvjestajs.OrderBy(i => i.Date_Posted);
                    break;
                case "date_desc":
                    izvjestajs = izvjestajs.OrderByDescending(i => i.Date_Posted);
                    break;
                case "User":
                    izvjestajs = izvjestajs.OrderBy(i => i.Username);
                    break;
                case "user_desc":
                    izvjestajs = izvjestajs.OrderByDescending(i => i.Username);
                    break;
                case "dateEdit":
                    izvjestajs = izvjestajs.OrderBy(i => i.Date_Edited);
                    break;
                case "dateEdit_desc":
                    izvjestajs = izvjestajs.OrderByDescending(i => i.Date_Edited);
                    break;
                default:
                    izvjestajs = izvjestajs.OrderBy(i => i.Name);
                    break;
            }
            return View(izvjestajs.ToList());
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

        public ActionResult GeneratePDF(int? id)
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
            return new Rotativa.MVC.ViewAsPdf("GeneratePDF",izvjestaj);           
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
        public ActionResult Create([Bind(Include = "ID,Name,Content,Members_present,Location,Duration,Date_Posted,Date_Edited")] Izvjestaj izvjestaj)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.Identity.GetUserId();
                izvjestaj.User_ID = currentUserId;
                var creatorUserName = User.Identity.Name;
                izvjestaj.Username = creatorUserName;
                
                izvjestaj.Date_Posted = DateTime.Now.ToString("dd/M/yyyy - h:mm:ss tt");
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
        public ActionResult Edit([Bind(Include = "ID,Name,Content,Members_present,Location,Duration,Date_Posted,Date_Edited,Username,User_ID")] Izvjestaj izvjestaj)
        {
            if (ModelState.IsValid)
            {
                db.Entry(izvjestaj).State = EntityState.Modified;
                izvjestaj.Date_Edited = DateTime.Now.ToString("dd/M/yyyy - h:mm:ss tt");        
                db.SaveChanges();
                if (this.User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("IndexUser");
                }
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
            if (this.User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("IndexUser");
            }
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
