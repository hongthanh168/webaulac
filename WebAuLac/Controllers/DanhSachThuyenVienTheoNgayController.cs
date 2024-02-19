using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAuLac.Models;

namespace WebAuLac.Controllers
{
    public class DanhSachThuyenVienTheoNgayController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: DanhSachThuyenVienTheoNgay
        public ActionResult Index()
        {
           
            TuNgayDenNgay obj = new TuNgayDenNgay();
            obj.TuNgay = new DateTime(DateTime.Now.Year, 1, 1);
            obj.DenNgay = DateTime.Now;
            var result = db.sp_LayDSNhanVienTheoNgay(obj.TuNgay).ToList();
            ViewBag.BangKe = result;
            return View(obj);
        }

        [Authorize(Roles = "HR, Boss")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "TuNgay,DenNgay")] TuNgayDenNgay obj)
        {
            if (ModelState.IsValid)
            {
                var result = db.sp_LayDSNhanVienTheoNgay(obj.TuNgay).ToList();
                ViewBag.BangKe = result;
                return View(obj);
            }
            return View();
        }

        // GET: DanhSachThuyenVienTheoNgay/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY = await db.HRM_EMPLOYMENTHISTORY.FindAsync(id);
            if (hRM_EMPLOYMENTHISTORY == null)
            {
                return HttpNotFound();
            }
            return View(hRM_EMPLOYMENTHISTORY);
        }

        // GET: DanhSachThuyenVienTheoNgay/Create
        public ActionResult Create()
        {
            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName");
            ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName");
            ViewBag.DepartmentPluralityID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName");
            ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");
            ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");
            ViewBag.LoaiTauID = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau");
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName");
            ViewBag.PluralityID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName");
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName");
            ViewBag.SalaryPositionID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPluralityID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");
            ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name");
            return View();
        }

        // POST: DanhSachThuyenVienTheoNgay/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmploymentHistoryID,DecisionDate,EffectiveDate,ContentDecision,CategoryDecisionID,EmployeeID,DepartmentID,PositionID,InternshipPosition,PluralityID,IntershipPlurality,Note,PerPosition,PerPlurality,SalaryPositionID,SalaryPluralityID,DepartmentName,LoaiTauID,Gross,Power,NgayXuongTau,XacNhan,DecisionNo,DepartmentPluralityID,LyDoNghiViec_ID,Salary,AllowanceSalary,Bonus,AllowanceBonus,SalaryPlurality,AllowanceSalaryPlurality,BonusPlurality,AllowanceBonusPlurality,ThoiGianThucTap")] HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY)
        {
            if (ModelState.IsValid)
            {
                db.HRM_EMPLOYMENTHISTORY.Add(hRM_EMPLOYMENTHISTORY);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", hRM_EMPLOYMENTHISTORY.CategoryDecisionID);
            ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentID);
            ViewBag.DepartmentPluralityID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentPluralityID);
            ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName", hRM_EMPLOYMENTHISTORY.InternshipPosition);
            ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName", hRM_EMPLOYMENTHISTORY.IntershipPlurality);
            ViewBag.LoaiTauID = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau", hRM_EMPLOYMENTHISTORY.LoaiTauID);
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            ViewBag.PluralityID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PluralityID);
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            ViewBag.SalaryPositionID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID", hRM_EMPLOYMENTHISTORY.SalaryPositionID);
            ViewBag.SalaryPluralityID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID", hRM_EMPLOYMENTHISTORY.SalaryPluralityID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYMENTHISTORY.EmployeeID);
            ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name", hRM_EMPLOYMENTHISTORY.LyDoNghiViec_ID);
            return View(hRM_EMPLOYMENTHISTORY);
        }

        // GET: DanhSachThuyenVienTheoNgay/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY = await db.HRM_EMPLOYMENTHISTORY.FindAsync(id);
            if (hRM_EMPLOYMENTHISTORY == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", hRM_EMPLOYMENTHISTORY.CategoryDecisionID);
            ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentID);
            ViewBag.DepartmentPluralityID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentPluralityID);
            ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName", hRM_EMPLOYMENTHISTORY.InternshipPosition);
            ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName", hRM_EMPLOYMENTHISTORY.IntershipPlurality);
            ViewBag.LoaiTauID = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau", hRM_EMPLOYMENTHISTORY.LoaiTauID);
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            ViewBag.PluralityID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PluralityID);
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            ViewBag.SalaryPositionID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID", hRM_EMPLOYMENTHISTORY.SalaryPositionID);
            ViewBag.SalaryPluralityID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID", hRM_EMPLOYMENTHISTORY.SalaryPluralityID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYMENTHISTORY.EmployeeID);
            ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name", hRM_EMPLOYMENTHISTORY.LyDoNghiViec_ID);
            return View(hRM_EMPLOYMENTHISTORY);
        }

        // POST: DanhSachThuyenVienTheoNgay/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmploymentHistoryID,DecisionDate,EffectiveDate,ContentDecision,CategoryDecisionID,EmployeeID,DepartmentID,PositionID,InternshipPosition,PluralityID,IntershipPlurality,Note,PerPosition,PerPlurality,SalaryPositionID,SalaryPluralityID,DepartmentName,LoaiTauID,Gross,Power,NgayXuongTau,XacNhan,DecisionNo,DepartmentPluralityID,LyDoNghiViec_ID,Salary,AllowanceSalary,Bonus,AllowanceBonus,SalaryPlurality,AllowanceSalaryPlurality,BonusPlurality,AllowanceBonusPlurality,ThoiGianThucTap")] HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRM_EMPLOYMENTHISTORY).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", hRM_EMPLOYMENTHISTORY.CategoryDecisionID);
            ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentID);
            ViewBag.DepartmentPluralityID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentPluralityID);
            ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName", hRM_EMPLOYMENTHISTORY.InternshipPosition);
            ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName", hRM_EMPLOYMENTHISTORY.IntershipPlurality);
            ViewBag.LoaiTauID = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau", hRM_EMPLOYMENTHISTORY.LoaiTauID);
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            ViewBag.PluralityID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PluralityID);
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            ViewBag.SalaryPositionID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID", hRM_EMPLOYMENTHISTORY.SalaryPositionID);
            ViewBag.SalaryPluralityID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID", hRM_EMPLOYMENTHISTORY.SalaryPluralityID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYMENTHISTORY.EmployeeID);
            ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name", hRM_EMPLOYMENTHISTORY.LyDoNghiViec_ID);
            return View(hRM_EMPLOYMENTHISTORY);
        }

        // GET: DanhSachThuyenVienTheoNgay/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY = await db.HRM_EMPLOYMENTHISTORY.FindAsync(id);
            if (hRM_EMPLOYMENTHISTORY == null)
            {
                return HttpNotFound();
            }
            return View(hRM_EMPLOYMENTHISTORY);
        }

        // POST: DanhSachThuyenVienTheoNgay/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY = await db.HRM_EMPLOYMENTHISTORY.FindAsync(id);
            db.HRM_EMPLOYMENTHISTORY.Remove(hRM_EMPLOYMENTHISTORY);
            await db.SaveChangesAsync();
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
