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
    public class HRM_DETAILSALARYController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: HRM_DETAILSALARY
        public ActionResult Index()
        {
            return View(db.HRM_DETAILSALARY.ToList());
        }

        // GET: HRM_DETAILSALARY/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_DETAILSALARY hRM_DETAILSALARY = db.HRM_DETAILSALARY.Find(id);
            if (hRM_DETAILSALARY == null)
            {
                return HttpNotFound();
            }
            return View(hRM_DETAILSALARY);
        }

        // GET: HRM_DETAILSALARY/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HRM_DETAILSALARY/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HRMDetailSalaryID,HRMSalaryID,EmployeeID,EmploymentHistoryID,DepartmentID,PositionID,Position,Salary,AllowanceSalary,Bonus,AllowanceBonus,NumOfDay,TruKhac,PhuThuoc,GhiChu")] HRM_DETAILSALARY hRM_DETAILSALARY)
        {
            if (ModelState.IsValid)
            {
                db.HRM_DETAILSALARY.Add(hRM_DETAILSALARY);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hRM_DETAILSALARY);
        }

        // GET: HRM_DETAILSALARY/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_DETAILSALARY hRM_DETAILSALARY = db.HRM_DETAILSALARY.Find(id);
            if (hRM_DETAILSALARY == null)
            {
                return HttpNotFound();
            }
            return PartialView(hRM_DETAILSALARY);
        }

        // POST: HRM_DETAILSALARY/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int HRMDetailSalaryID, float? ThuLao, float? Thuong, float? ThuNhapTinhThueTNCN, int? PhuThuoc, int? TruKhac, bool? IsBHXH, bool? IsBHYT, bool? IsCongDoan, bool? IsBHTN, bool? IsGiamTruCaNhan, string GhiChu )
        {
            HRM_DETAILSALARY obj = db.HRM_DETAILSALARY.Find(HRMDetailSalaryID);
            if (ModelState.IsValid)
            {
                obj.ThuLao = ThuLao;
                obj.Thuong = Thuong;
                obj.ThuNhapTinhThueTNCN = ThuNhapTinhThueTNCN;
                obj.PhuThuoc = PhuThuoc;                
                obj.TruKhac = TruKhac;
                obj.IsBHXH = IsBHXH;
                obj.IsBHYT = IsBHYT;
                obj.IsCongDoan = IsCongDoan;
                obj.IsBHTN = IsBHTN;
                obj.IsGiamTruCaNhan = IsGiamTruCaNhan;
                obj.GhiChu = GhiChu;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success= true}, JsonRequestBehavior.AllowGet);
            }
            return View(obj);
        }

        // GET: HRM_DETAILSALARY/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_DETAILSALARY hRM_DETAILSALARY = db.HRM_DETAILSALARY.Find(id);
            if (hRM_DETAILSALARY == null)
            {
                return HttpNotFound();
            }
            return View(hRM_DETAILSALARY);
        }

        // POST: HRM_DETAILSALARY/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HRM_DETAILSALARY hRM_DETAILSALARY = db.HRM_DETAILSALARY.Find(id);
            db.HRM_DETAILSALARY.Remove(hRM_DETAILSALARY);
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
