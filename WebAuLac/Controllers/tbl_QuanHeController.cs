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
    [Authorize(Roles = "Admin, Boss, HR")]
    [ValidateAntiForgeryToken]

    public class tbl_QuanHeController : Controller
    {

        private AuLacEntities db = new AuLacEntities();
        // GET: tbl_QuanHe
        public ActionResult Index()
        {
            return View(db.tbl_QuanHe.ToList());
        }

        // GET: tbl_QuanHe/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_QuanHe tbl_QuanHe = db.tbl_QuanHe.Find(id);
            if (tbl_QuanHe == null)
            {
                return HttpNotFound();
            }
            return PartialView(tbl_QuanHe);
        }

        // GET: tbl_QuanHe/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: tbl_QuanHe/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuanHeID,HoTen")] tbl_QuanHe tbl_QuanHe)
        {
            if (ModelState.IsValid)
            {
                db.tbl_QuanHe.Add(tbl_QuanHe);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView(tbl_QuanHe);
        }

        // GET: tbl_QuanHe/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_QuanHe tbl_QuanHe = db.tbl_QuanHe.Find(id);
            if (tbl_QuanHe == null)
            {
                return HttpNotFound();
            }
            return PartialView(tbl_QuanHe);
        }

        // POST: tbl_QuanHe/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuanHeID,HoTen")] tbl_QuanHe tbl_QuanHe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_QuanHe).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(tbl_QuanHe);
        }

        // GET: tbl_QuanHe/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_QuanHe tbl_QuanHe = db.tbl_QuanHe.Find(id);
            if (tbl_QuanHe == null)
            {
                return HttpNotFound();
            }
            return PartialView(tbl_QuanHe);
        }

        // POST: tbl_QuanHe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_QuanHe tbl_QuanHe = db.tbl_QuanHe.Find(id);
            db.tbl_QuanHe.Remove(tbl_QuanHe);
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
