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
    [Authorize(Roles = "HoSoPhapLy")]
    [Authorize(Roles = "Create")]
    public class ChungChiTauController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: ChungChiTau
        public ActionResult Index()
        {
            return View(db.ChungChiTaus.OrderBy(x => x.STT).ToList());
        }
        public ActionResult SapXep(int id, int lenXuong)
        {
            db.sp_T_UpdateSTTLenXuong_ChungChiTau(id, lenXuong);
            return RedirectToAction("Index");
        }
        // GET: ChungChiTau/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChungChiTau chungChiTau = db.ChungChiTaus.Find(id);
            if (chungChiTau == null)
            {
                return HttpNotFound();
            }
            return View(chungChiTau);
        }

        // GET: ChungChiTau/Create
        [Authorize(Roles = "HR")]
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: ChungChiTau/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        public ActionResult Create([Bind(Include = "ChungChiID,TenChungChi,SoNgayCanhBao")] ChungChiTau chungChiTau)
        {
            if (ModelState.IsValid)
            {
                db.ChungChiTaus.Add(chungChiTau);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return View(chungChiTau);
        }

        // GET: ChungChiTau/Edit/5
        [Authorize(Roles = "HR")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChungChiTau chungChiTau = db.ChungChiTaus.Find(id);
            if (chungChiTau == null)
            {
                return HttpNotFound();
            }
            return PartialView(chungChiTau);
        }

        // POST: ChungChiTau/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        public ActionResult Edit([Bind(Include = "ChungChiID,TenChungChi,SoNgayCanhBao")] ChungChiTau chungChiTau)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chungChiTau).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(chungChiTau);
        }

        // GET: ChungChiTau/Delete/5
        [Authorize(Roles = "HR")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChungChiTau chungChiTau = db.ChungChiTaus.Find(id);
            if (chungChiTau == null)
            {
                return HttpNotFound();
            }
            return PartialView(chungChiTau);
        }

        // POST: ChungChiTau/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        public ActionResult DeleteConfirmed(int id)
        {
            ChungChiTau chungChiTau = db.ChungChiTaus.Find(id);
            db.ChungChiTaus.Remove(chungChiTau);
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
