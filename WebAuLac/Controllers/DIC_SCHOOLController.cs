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
    [Authorize(Roles = "Admin, EduCenter")]
    public class DIC_SCHOOLController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: DIC_SCHOOL
        public ActionResult Index()
        {
            return View(db.DIC_SCHOOL.ToList());
        }

        // GET: DIC_SCHOOL/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_SCHOOL dIC_SCHOOL = db.DIC_SCHOOL.Find(id);
            if (dIC_SCHOOL == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_SCHOOL);
        }

        // GET: DIC_SCHOOL/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: DIC_SCHOOL/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SchoolID,SchoolName,Description")] DIC_SCHOOL dIC_SCHOOL)
        {
            if (ModelState.IsValid)
            {
                db.DIC_SCHOOL.Add(dIC_SCHOOL);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView(dIC_SCHOOL);
        }

        // GET: DIC_SCHOOL/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_SCHOOL dIC_SCHOOL = db.DIC_SCHOOL.Find(id);
            if (dIC_SCHOOL == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_SCHOOL);
        }

        // POST: DIC_SCHOOL/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SchoolID,SchoolName,Description")] DIC_SCHOOL dIC_SCHOOL)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dIC_SCHOOL).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(dIC_SCHOOL);
        }

        // GET: DIC_SCHOOL/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_SCHOOL dIC_SCHOOL = db.DIC_SCHOOL.Find(id);
            if (dIC_SCHOOL == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_SCHOOL);
        }

        // POST: DIC_SCHOOL/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DIC_SCHOOL dIC_SCHOOL = db.DIC_SCHOOL.Find(id);
            db.DIC_SCHOOL.Remove(dIC_SCHOOL);
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
