using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAuLac.Models;

namespace WebAuLac.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DIC_NATIONALITYController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: DIC_NATIONALITY
        public ActionResult Index()
        {
            return View(db.DIC_NATIONALITY.ToList());
        }

        // GET: DIC_NATIONALITY/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_NATIONALITY dIC_NATIONALITY = db.DIC_NATIONALITY.Find(id);
            if (dIC_NATIONALITY == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_NATIONALITY);
        }

        // GET: DIC_NATIONALITY/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: DIC_NATIONALITY/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NationalityID,NationalityName,Description")] DIC_NATIONALITY dIC_NATIONALITY)
        {
            if (ModelState.IsValid)
            {
                db.DIC_NATIONALITY.Add(dIC_NATIONALITY);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView(dIC_NATIONALITY);
        }

        // GET: DIC_NATIONALITY/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_NATIONALITY dIC_NATIONALITY = db.DIC_NATIONALITY.Find(id);
            if (dIC_NATIONALITY == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_NATIONALITY);
        }

        // POST: DIC_NATIONALITY/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NationalityID,NationalityName,Description")] DIC_NATIONALITY dIC_NATIONALITY)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dIC_NATIONALITY).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(dIC_NATIONALITY);
        }

        // GET: DIC_NATIONALITY/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_NATIONALITY dIC_NATIONALITY = db.DIC_NATIONALITY.Find(id);
            if (dIC_NATIONALITY == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_NATIONALITY);
        }

        // POST: DIC_NATIONALITY/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DIC_NATIONALITY dIC_NATIONALITY = db.DIC_NATIONALITY.Find(id);
            db.DIC_NATIONALITY.Remove(dIC_NATIONALITY);
            db.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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
