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
    [Authorize(Roles = "Boss, HR, EduCenter, DaoTao, View")]
    public class LichKiemTrasController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: LichKiemTras
        public ActionResult Index()
        {
            int year = DateTime.Now.Year;           
            var lichKiemTras = db.LichKiemTras.Where(x => x.Nam ==year).Include(l => l.DIC_DEPARTMENT).Include(l => l.LoaiKiemTra);            
            return View(lichKiemTras.ToList());
        }

        // GET: LichKiemTras/Details/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichKiemTra lichKiemTra = db.LichKiemTras.Find(id);
            if (lichKiemTra == null)
            {
                return HttpNotFound();
            }
            return View(lichKiemTra);
        }

        // GET: LichKiemTras/Create
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName");
            ViewBag.LoaiKiemTraID = new SelectList(db.LoaiKiemTras, "LoaiKiemTraID", "TenLoaiKiemTra");
            return PartialView();
        }

        // POST: LichKiemTras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Create([Bind(Include = "LichKiemTraID,DepartmentID,LoaiKiemTraID,Thang,Nam,Ngay")] LichKiemTra lichKiemTra)
        {
            if (ModelState.IsValid)
            {
                db.LichKiemTras.Add(lichKiemTra);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", lichKiemTra.DepartmentID);
            ViewBag.LoaiKiemTraID = new SelectList(db.LoaiKiemTras, "LoaiKiemTraID", "TenLoaiKiemTra", lichKiemTra.LoaiKiemTraID);
            return PartialView(lichKiemTra);
        }

        // GET: LichKiemTras/Edit/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichKiemTra lichKiemTra = db.LichKiemTras.Find(id);
            if (lichKiemTra == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", lichKiemTra.DepartmentID);
            ViewBag.LoaiKiemTraID = new SelectList(db.LoaiKiemTras, "LoaiKiemTraID", "TenLoaiKiemTra", lichKiemTra.LoaiKiemTraID);
            return View(lichKiemTra);
        }

        // POST: LichKiemTras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit([Bind(Include = "LichKiemTraID,DepartmentID,LoaiKiemTraID,Thang,Nam,Ngay")] LichKiemTra lichKiemTra)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lichKiemTra).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", lichKiemTra.DepartmentID);
            ViewBag.LoaiKiemTraID = new SelectList(db.LoaiKiemTras, "LoaiKiemTraID", "TenLoaiKiemTra", lichKiemTra.LoaiKiemTraID);
            return View(lichKiemTra);
        }

        // GET: LichKiemTras/Delete/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichKiemTra lichKiemTra = db.LichKiemTras.Find(id);
            if (lichKiemTra == null)
            {
                return HttpNotFound();
            }
            return View(lichKiemTra);
        }

        // POST: LichKiemTras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult DeleteConfirmed(int id)
        {
            LichKiemTra lichKiemTra = db.LichKiemTras.Find(id);
            db.LichKiemTras.Remove(lichKiemTra);
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
