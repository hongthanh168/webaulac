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
    public class DIC_RELIGIONController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: DIC_RELIGION
        public ActionResult Index()
        {
            return View(db.DIC_RELIGION.ToList());
        }

        // GET: DIC_RELIGION/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_RELIGION dIC_RELIGION = db.DIC_RELIGION.Find(id);
            if (dIC_RELIGION == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_RELIGION);
        }

        // GET: DIC_RELIGION/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: DIC_RELIGION/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReligionID,ReligionName,Description")] DIC_RELIGION dIC_RELIGION)
        {
            if (ModelState.IsValid)
            {
                db.DIC_RELIGION.Add(dIC_RELIGION);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView(dIC_RELIGION);
        }

        // GET: DIC_RELIGION/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_RELIGION dIC_RELIGION = db.DIC_RELIGION.Find(id);
            if (dIC_RELIGION == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_RELIGION);
        }

        // POST: DIC_RELIGION/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReligionID,ReligionName,Description")] DIC_RELIGION dIC_RELIGION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dIC_RELIGION).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(dIC_RELIGION);
        }

        // GET: DIC_RELIGION/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_RELIGION dIC_RELIGION = db.DIC_RELIGION.Find(id);
            if (dIC_RELIGION == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_RELIGION);
        }

        // POST: DIC_RELIGION/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DIC_RELIGION dIC_RELIGION = db.DIC_RELIGION.Find(id);
            db.DIC_RELIGION.Remove(dIC_RELIGION);
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
