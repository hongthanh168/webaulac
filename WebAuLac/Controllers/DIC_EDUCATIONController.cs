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
    public class DIC_EDUCATIONController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: DIC_EDUCATION
        public ActionResult Index()
        {
            return View(db.DIC_EDUCATION.ToList());
        }

        // GET: DIC_EDUCATION/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_EDUCATION dIC_EDUCATION = db.DIC_EDUCATION.Find(id);
            if (dIC_EDUCATION == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_EDUCATION);
        }

        // GET: DIC_EDUCATION/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: DIC_EDUCATION/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EducationID,EducationName,Description")] DIC_EDUCATION dIC_EDUCATION)
        {
            if (ModelState.IsValid)
            {
                db.DIC_EDUCATION.Add(dIC_EDUCATION);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView(dIC_EDUCATION);
        }

        // GET: DIC_EDUCATION/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_EDUCATION dIC_EDUCATION = db.DIC_EDUCATION.Find(id);
            if (dIC_EDUCATION == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_EDUCATION);
        }

        // POST: DIC_EDUCATION/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EducationID,EducationName,Description")] DIC_EDUCATION dIC_EDUCATION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dIC_EDUCATION).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(dIC_EDUCATION);
        }

        // GET: DIC_EDUCATION/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_EDUCATION dIC_EDUCATION = db.DIC_EDUCATION.Find(id);
            if (dIC_EDUCATION == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_EDUCATION);
        }

        // POST: DIC_EDUCATION/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DIC_EDUCATION dIC_EDUCATION = db.DIC_EDUCATION.Find(id);
            db.DIC_EDUCATION.Remove(dIC_EDUCATION);
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
