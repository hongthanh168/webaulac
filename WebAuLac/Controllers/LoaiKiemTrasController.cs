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
    public class LoaiKiemTrasController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: LoaiKiemTras
        public ActionResult Index()
        {
            return View(db.LoaiKiemTras.ToList());
        }

        // GET: LoaiKiemTras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiKiemTra loaiKiemTra = db.LoaiKiemTras.Find(id);
            if (loaiKiemTra == null)
            {
                return HttpNotFound();
            }
            return PartialView(loaiKiemTra);
        }

        // GET: LoaiKiemTras/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: LoaiKiemTras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LoaiKiemTraID,TenLoaiKiemTra,VietTat")] LoaiKiemTra loaiKiemTra)
        {
            if (ModelState.IsValid)
            {
                db.LoaiKiemTras.Add(loaiKiemTra);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView(loaiKiemTra);
        }

        // GET: LoaiKiemTras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiKiemTra loaiKiemTra = db.LoaiKiemTras.Find(id);
            if (loaiKiemTra == null)
            {
                return HttpNotFound();
            }
            return PartialView(loaiKiemTra);
        }

        // POST: LoaiKiemTras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoaiKiemTraID,TenLoaiKiemTra,VietTat")] LoaiKiemTra loaiKiemTra)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loaiKiemTra).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(loaiKiemTra);
        }

        // GET: LoaiKiemTras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiKiemTra loaiKiemTra = db.LoaiKiemTras.Find(id);
            if (loaiKiemTra == null)
            {
                return HttpNotFound();
            }
            return PartialView(loaiKiemTra);
        }

        // POST: LoaiKiemTras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoaiKiemTra loaiKiemTra = db.LoaiKiemTras.Find(id);
            db.LoaiKiemTras.Remove(loaiKiemTra);
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
