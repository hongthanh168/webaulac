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
    public class TieuChuanCrewMatricesController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: TieuChuanCrewMatrices
        public ActionResult Index()
        {
            return View(db.TieuChuanCrewMatrices.ToList());
        }

        // GET: TieuChuanCrewMatrices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TieuChuanCrewMatrix tieuChuanCrewMatrix = db.TieuChuanCrewMatrices.Find(id);
            if (tieuChuanCrewMatrix == null)
            {
                return HttpNotFound();
            }
            return PartialView(tieuChuanCrewMatrix);
        }

        // GET: TieuChuanCrewMatrices/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: TieuChuanCrewMatrices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TieuChuanID,TieuChuanName,WithOperator_QuanLyBoong,InRank_QuanLyBoong,ThisTypeOfTanker_QuanLyBoong,AllTypeOfTanker_QuanLyBoong,WithOperator_ThuyenTruong,InRank_ThuyenTruong,ThisTypeOfTanker_ThuyenTruong,AllTypeOfTanker_ThuyenTruong,WithOperator_DaiPho,InRank_DaiPho,ThisTypeOfTanker_DaiPho,AllTypeOfTanker_DaiPho,WithOperator_QuanLyMay,InRank_QuanLyMay,ThisTypeOfTanker_QuanLyMay,AllTypeOfTanker_QuanLyMay,WithOperator_MayTruong,InRank_MayTruong,ThisTypeOfTanker_MayTruong,AllTypeOfTanker_MayTruong,WithOperator_May2,InRank_May2,ThisTypeOfTanker_May2,AllTypeOfTanker_May2,WithOperator_VanHanhBoong,InRank_VanHanhBoong,ThisTypeOfTanker_VanHanhBoong,AllTypeOfTanker_VanHanhBoong,WithOperator_VanHanhMay,InRank_VanHanhMay,ThisTypeTanker_VanHanhMay,AllTypeOfTanker_VanHanhMay")] TieuChuanCrewMatrix tieuChuanCrewMatrix)
        {
            if (ModelState.IsValid)
            {
                db.TieuChuanCrewMatrices.Add(tieuChuanCrewMatrix);
                db.SaveChanges();
                return Json(new { success=true}, JsonRequestBehavior.AllowGet);
            }

            return PartialView(tieuChuanCrewMatrix);
        }

        // GET: TieuChuanCrewMatrices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TieuChuanCrewMatrix tieuChuanCrewMatrix = db.TieuChuanCrewMatrices.Find(id);
            if (tieuChuanCrewMatrix == null)
            {
                return HttpNotFound();
            }
            return PartialView(tieuChuanCrewMatrix);
        }

        // POST: TieuChuanCrewMatrices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TieuChuanID,TieuChuanName,WithOperator_QuanLyBoong,InRank_QuanLyBoong,ThisTypeOfTanker_QuanLyBoong,AllTypeOfTanker_QuanLyBoong,WithOperator_ThuyenTruong,InRank_ThuyenTruong,ThisTypeOfTanker_ThuyenTruong,AllTypeOfTanker_ThuyenTruong,WithOperator_DaiPho,InRank_DaiPho,ThisTypeOfTanker_DaiPho,AllTypeOfTanker_DaiPho,WithOperator_QuanLyMay,InRank_QuanLyMay,ThisTypeOfTanker_QuanLyMay,AllTypeOfTanker_QuanLyMay,WithOperator_MayTruong,InRank_MayTruong,ThisTypeOfTanker_MayTruong,AllTypeOfTanker_MayTruong,WithOperator_May2,InRank_May2,ThisTypeOfTanker_May2,AllTypeOfTanker_May2,WithOperator_VanHanhBoong,InRank_VanHanhBoong,ThisTypeOfTanker_VanHanhBoong,AllTypeOfTanker_VanHanhBoong,WithOperator_VanHanhMay,InRank_VanHanhMay,ThisTypeTanker_VanHanhMay,AllTypeOfTanker_VanHanhMay")] TieuChuanCrewMatrix tieuChuanCrewMatrix)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tieuChuanCrewMatrix).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(tieuChuanCrewMatrix);
        }

        // GET: TieuChuanCrewMatrices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TieuChuanCrewMatrix tieuChuanCrewMatrix = db.TieuChuanCrewMatrices.Find(id);
            if (tieuChuanCrewMatrix == null)
            {
                return HttpNotFound();
            }
            return PartialView(tieuChuanCrewMatrix);
        }

        // POST: TieuChuanCrewMatrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TieuChuanCrewMatrix tieuChuanCrewMatrix = db.TieuChuanCrewMatrices.Find(id);
            db.TieuChuanCrewMatrices.Remove(tieuChuanCrewMatrix);
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
