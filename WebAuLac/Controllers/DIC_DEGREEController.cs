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
    public class DIC_DEGREEController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: DIC_DEGREE
        public ActionResult Index()
        {
            return View(db.DIC_DEGREE.ToList());
        }

        // GET: DIC_DEGREE/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_DEGREE dIC_DEGREE = db.DIC_DEGREE.Find(id);
            if (dIC_DEGREE == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_DEGREE);
        }

        // GET: DIC_DEGREE/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: DIC_DEGREE/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DegreeID,DegreeName,Description,Unclos,WarningBefore")] DIC_DEGREE dIC_DEGREE)
        {
            if (ModelState.IsValid)
            {
                db.DIC_DEGREE.Add(dIC_DEGREE);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView(dIC_DEGREE);
        }

        // GET: DIC_DEGREE/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_DEGREE dIC_DEGREE = db.DIC_DEGREE.Find(id);
            if (dIC_DEGREE == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_DEGREE);
        }

        // POST: DIC_DEGREE/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DegreeID,DegreeName,Description,Unclos,WarningBefore")] DIC_DEGREE dIC_DEGREE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dIC_DEGREE).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(dIC_DEGREE);
        }

        // GET: DIC_DEGREE/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_DEGREE dIC_DEGREE = db.DIC_DEGREE.Find(id);
            if (dIC_DEGREE == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_DEGREE);
        }

        // POST: DIC_DEGREE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DIC_DEGREE dIC_DEGREE = db.DIC_DEGREE.Find(id);
            db.DIC_DEGREE.Remove(dIC_DEGREE);
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
