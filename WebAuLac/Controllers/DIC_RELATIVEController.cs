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
    public class DIC_RELATIVEController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: DIC_RELATIVE
        public ActionResult Index()
        {
            return View(db.DIC_RELATIVE.ToList());
        }

        // GET: DIC_RELATIVE/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_RELATIVE dIC_RELATIVE = db.DIC_RELATIVE.Find(id);
            if (dIC_RELATIVE == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_RELATIVE);
        }

        // GET: DIC_RELATIVE/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: DIC_RELATIVE/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RelativeID,RelativeName,Description")] DIC_RELATIVE dIC_RELATIVE)
        {
            if (ModelState.IsValid)
            {
                db.DIC_RELATIVE.Add(dIC_RELATIVE);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView(dIC_RELATIVE);
        }

        // GET: DIC_RELATIVE/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_RELATIVE dIC_RELATIVE = db.DIC_RELATIVE.Find(id);
            if (dIC_RELATIVE == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_RELATIVE);
        }

        // POST: DIC_RELATIVE/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RelativeID,RelativeName,Description")] DIC_RELATIVE dIC_RELATIVE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dIC_RELATIVE).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(dIC_RELATIVE);
        }

        // GET: DIC_RELATIVE/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_RELATIVE dIC_RELATIVE = db.DIC_RELATIVE.Find(id);
            if (dIC_RELATIVE == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_RELATIVE);
        }

        // POST: DIC_RELATIVE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DIC_RELATIVE dIC_RELATIVE = db.DIC_RELATIVE.Find(id);
            db.DIC_RELATIVE.Remove(dIC_RELATIVE);
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
