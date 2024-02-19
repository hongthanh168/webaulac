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
    [Authorize(Roles = "HR, Boss")]
    public class HuyKetQuaTuyenDungController : Controller
    {
        private int m_loaiQuyetDinhID = 6;

        private AuLacEntities db = new AuLacEntities();

        [Authorize(Roles = "HR, Boss")]
        public ActionResult IndexHuyKetQuaTuyenDung()
        {            
            TuNgayDenNgay obj = new TuNgayDenNgay();
            obj.TuNgay = new DateTime(DateTime.Now.Year, 1, 1);
            obj.DenNgay = new DateTime(DateTime.Now.Year, 12, 31);
            var result = db.sp_LayDSThuyenVienHuyTuyenDung(obj.TuNgay, obj.DenNgay).Where(p=> p.ParentID == 8 || p.ParentID == 17).ToList();
            ViewBag.BangKe = result;
            return View(obj);
        }

        [Authorize(Roles = "HR, Boss")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IndexHuyKetQuaTuyenDung([Bind(Include = "TuNgay,DenNgay")] TuNgayDenNgay obj)
        {
            if (ModelState.IsValid)
            {
                var result = db.sp_LayDSThuyenVienHuyTuyenDung(obj.TuNgay, obj.DenNgay).Where(p => p.ParentID == 8 || p.ParentID == 17).ToList();
                ViewBag.BangKe = result;
                return View(obj);
            }
            return View();
        }

        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        // GET: QuyetDinhNghiViec/Details/5
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

        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        // GET: QuyetDinhNghiViec/Create
        public ActionResult Create(int loaiNV)
        {
            HRM_EMPLOYMENTHISTORY item = new HRM_EMPLOYMENTHISTORY();

            int DecisionNo = 0;
            try
            {
                //kiểm tra danh sách để lấy theo năm hiện tại trước               

                if (loaiNV == 1)
                {
                    var query = db.HRM_EMPLOYMENTHISTORY.Where(x => x.DecisionDate.Value.Year == DateTime.Now.Year
                                && x.CategoryDecisionID == m_loaiQuyetDinhID && x.XacNhan == true && x.DIC_DEPARTMENT.ParentID != 1).OrderByDescending(x => x.DecisionNo).First();
                    DecisionNo = (int)query.DecisionNo;
                }
                else
                {
                    var query = db.HRM_EMPLOYMENTHISTORY.Where(x => x.DecisionDate.Value.Year == DateTime.Now.Year
                                && x.CategoryDecisionID == m_loaiQuyetDinhID && x.XacNhan == true && x.DIC_DEPARTMENT.ParentID == 1).OrderByDescending(x => x.DecisionNo).First();
                    DecisionNo = (int)query.DecisionNo;
                }
                    
            }
            catch (Exception) { }


            item.DecisionNo = DecisionNo + 1;

            item.CategoryDecisionID = m_loaiQuyetDinhID;
            var listCategoryDecision = from p in db.DIC_CATEGORYDECISION
                                       where p.CategoryDecisionID == m_loaiQuyetDinhID
                                       select p;

            ViewBag.CategoryDecisionID = new SelectList(listCategoryDecision, "CategoryDecisionID", "CategoryDecisionName");

            // Lọc nhân viên vừa có quyết định tuyển dụng
            if (loaiNV == 1)
            {
                var listNhanVien = from p in db.HRM_EMPLOYEE
                                   where (from o in db.viewHRM_EMPLOYMENTHISTORY
                                          where o.ParentID != 1 && o.CategoryDecisionID == 2 && o.StatusID == 1
                                          select o.EmployeeID).Contains(p.EmployeeID) 
                                   select p;

                ViewBag.EmployeeID = new SelectList(listNhanVien, "EmployeeID", "HoTen");
            }
            else
            {
                var listNhanVien = from p in db.HRM_EMPLOYEE
                                   where (from o in db.viewHRM_EMPLOYMENTHISTORY
                                          where o.ParentID == 1 && o.CategoryDecisionID == 2 && o.StatusID == 1
                                          select o.EmployeeID).Contains(p.EmployeeID) 
                                   select p;

                ViewBag.EmployeeID = new SelectList(listNhanVien, "EmployeeID", "HoTen");
            }
                
            //ViewBag.LoaiTauID = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau");

            //ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name");
            //var departmentID = from a in db.DIC_DEPARTMENT
            //                   where a.IsLast == true
            //                   select a;
            //var positionID = from a in db.DIC_POSITION
            //                 select a;
            //ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName");
            //ViewBag.DepartmentID = new SelectList(departmentID, "DepartmentID", "DepartmentName");
            //ViewBag.PositionID = new SelectList(positionID, "PositionID", "PositionName");
            //ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName");
            //ViewBag.DepartmentPluralityID = new SelectList(departmentID, "DepartmentID", "DepartmentName");
            //ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");
            //ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");

            //var rankID = from a in db.DIC_SALARY
            //             select a;

            //ViewBag.SalaryPositionID = new SelectList(rankID, "SalaryID", "RankID");
            //ViewBag.SalaryPluralityID = new SelectList(rankID, "SalaryID", "RankID");

            //item.SalaryPositionID = 1;
            item.PerPosition = 100;
            item.PerPlurality = 0;
            item.Salary = 0;
            item.XacNhan = true;

            return PartialView(item);
        }

        // POST: QuyetDinhNghiViec/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmploymentHistoryID,DecisionDate,EffectiveDate,ContentDecision,CategoryDecisionID,EmployeeID,DepartmentID,PositionID,InternshipPosition,PluralityID,IntershipPlurality,Note,PerPosition,PerPlurality,SalaryPositionID,SalaryPluralityID,DepartmentName,LoaiTauID,Gross,Power,NgayXuongTau,XacNhan,DecisionNo,DepartmentPluralityID,LyDoNghiViec_ID,Salary,AllowanceSalary,Bonus,AllowanceBonus,SalaryPlurality,AllowanceSalaryPlurality,BonusPlurality,AllowanceBonusPlurality,ThoiGianThucTap")] HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                //Update Phòng ban, chức vụ tại chỗ
                viewHRM_EMPLOYMENTHISTORY objHRM = (from p in db.viewHRM_EMPLOYMENTHISTORY
                                                    where p.EmployeeID == hRM_EMPLOYMENTHISTORY.EmployeeID
                                                    select p).FirstOrDefault();

                hRM_EMPLOYMENTHISTORY.DepartmentID = objHRM.DepartmentID;
                hRM_EMPLOYMENTHISTORY.PositionID = objHRM.PositionID;

                db.HRM_EMPLOYMENTHISTORY.Add(hRM_EMPLOYMENTHISTORY);
                db.SaveChanges();

                // Set tình trạng ứng viên = 6
                HRM_EMPLOYEE em = (from p in db.HRM_EMPLOYEE
                                  where p.EmployeeID == hRM_EMPLOYMENTHISTORY.EmployeeID
                                  select p).FirstOrDefault();

                em.StatusID = 6;
                db.SaveChanges();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView(hRM_EMPLOYMENTHISTORY);
        }

        // GET: QuyetDinhNghiViec/Edit/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Find(id);
            if (hRM_EMPLOYMENTHISTORY == null)
            {
                return HttpNotFound();
            }

            var listCategoryDecision = from p in db.DIC_CATEGORYDECISION
                                       where p.CategoryDecisionID == m_loaiQuyetDinhID
                                       select p;

            var listNhanVien = from p in db.HRM_EMPLOYEE
                               where p.EmployeeID == hRM_EMPLOYMENTHISTORY.EmployeeID
                               select p;

            ViewBag.CategoryDecisionID = new SelectList(listCategoryDecision, "CategoryDecisionID", "CategoryDecisionName", hRM_EMPLOYMENTHISTORY.CategoryDecisionID);
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
            ViewBag.EmployeeID = new SelectList(listNhanVien, "EmployeeID", "HoTen", hRM_EMPLOYMENTHISTORY.EmployeeID);
            

            return PartialView(hRM_EMPLOYMENTHISTORY);

        }

        // POST: QuyetDinhNghiViec/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmploymentHistoryID,DecisionDate,EffectiveDate,ContentDecision,CategoryDecisionID,EmployeeID,DepartmentID,PositionID,InternshipPosition,PluralityID,IntershipPlurality,Note,PerPosition,PerPlurality,SalaryPositionID,SalaryPluralityID,DepartmentName,LoaiTauID,Gross,Power,NgayXuongTau,XacNhan,DecisionNo,DepartmentPluralityID,LyDoNghiViec_ID,Salary,AllowanceSalary,Bonus,AllowanceBonus,SalaryPlurality,AllowanceSalaryPlurality,BonusPlurality,AllowanceBonusPlurality,ThoiGianThucTap")] HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRM_EMPLOYMENTHISTORY).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "HoTen", hRM_EMPLOYMENTHISTORY.EmployeeID);           

            return PartialView(hRM_EMPLOYMENTHISTORY);
        }

        // GET: QuyetDinhNghiViec/Delete/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Find(id);
            if (hRM_EMPLOYMENTHISTORY == null)
            {
                return HttpNotFound();
            }
            return PartialView(hRM_EMPLOYMENTHISTORY);
        }

        // POST: QuyetDinhNghiViec/Delete/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Find(id);
            
            // Set tình trạng về làm việc = 1
            HRM_EMPLOYEE em = (from p in db.HRM_EMPLOYEE
                               where p.EmployeeID == hRM_EMPLOYMENTHISTORY.EmployeeID
                               select p).FirstOrDefault();

            em.StatusID = 1;
            db.SaveChanges();

            db.HRM_EMPLOYMENTHISTORY.Remove(hRM_EMPLOYMENTHISTORY);
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
