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
using HRM.Controllers;
using System.IO;
using Novacode;
using System.Web.Services;

namespace WebAuLac.Controllers
{
    [Authorize(Roles = "HR, Boss")]
    public class QuyetDinhTuyenDungController : Controller
    {        
        private AuLacEntities db = new AuLacEntities();       

        [Authorize(Roles = "HR, Boss")]
        public ActionResult IndexTuyenDung()
        {
            int loaibaocao = 100;
            TuNgayDenNgay obj = new TuNgayDenNgay();
            obj.TuNgay = new DateTime(DateTime.Now.Year, 1, 1);
            obj.DenNgay = new DateTime(DateTime.Now.Year, 12, 31);
            var result = db.sp_LayDSTuyenDung(obj.TuNgay, obj.DenNgay, loaibaocao).ToList();
            ViewBag.BangKe = result;
            return View(obj);
        }

        [Authorize(Roles = "HR, Boss")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IndexTuyenDung([Bind(Include = "TuNgay,DenNgay")] TuNgayDenNgay obj)
        {
            if (ModelState.IsValid)
            {
                var result = db.sp_LayDSTuyenDung(obj.TuNgay, obj.DenNgay, 1).ToList();
                ViewBag.BangKe = result;
                return View(obj);
            }

            return View();
        }

        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        // GET: QuyetDinhTuyenDung/Details/5
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
                                       where p.CategoryDecisionID == 2
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
            ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name", hRM_EMPLOYMENTHISTORY.LyDoNghiViec_ID);

            return PartialView(hRM_EMPLOYMENTHISTORY);
        }

        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public FileResult XuatQuyetDinhTuyenDung(int QTCT_ID)
        {
            //XuatWordExcelController ctrl = new XuatWordExcelController();
            //ctrl.ServerPath = Server.MapPath("~/App_Data");
            //ctrl.AppUser = User;
            //return ctrl.XuatQuyetDinhTuyenDung(QTCT_ID);
            AuLacEntities dc = new AuLacEntities();

            MemoryStream stream = new MemoryStream();
            string server = Server.MapPath("~/App_Data");
            string fileNameTemplate = "";
            DocX doc = DocX.Create(stream);

            view_quatrinhcongtacFull vemhis = (from p in dc.view_quatrinhcongtacFull
                                               where p.EmploymentHistoryID == QTCT_ID
                                               select p).SingleOrDefault();

            
            if(vemhis.ParentID == 1)
                fileNameTemplate = server + @"//Mau_QDTDVanPhong.docx";
            else
                fileNameTemplate = server + @"//Mau_QDTD.docx";
           

            doc = DocX.Load(fileNameTemplate);
            if (vemhis.DecisionNo != null)
                doc.ReplaceText("%SoQD%", vemhis.DecisionNo.ToString().ToUpper() + "/" + vemhis.DecisionDate.Value.Year.ToString().Substring(2, 2) + "/QĐ-TD");

            doc.ReplaceText("%Ngay%", vemhis.DecisionDate.Value.Day.ToString());
            doc.ReplaceText("%Thang%", vemhis.DecisionDate.Value.Month.ToString());
            doc.ReplaceText("%Nam%", vemhis.DecisionDate.Value.Year.ToString());
            doc.ReplaceText("%Name%", vemhis.FirstName.ToUpper() + " " + vemhis.LastName.ToUpper());

            if (vemhis.BirthDay != null)
                doc.ReplaceText("%NamSinh%", vemhis.BirthDay.Value.ToString("dd/MM/yyyy"));

            if (vemhis.MainAddress != null)
                doc.ReplaceText("%DiaChi%", vemhis.MainAddress);

            if (vemhis.ChucVu != null)
                if(vemhis.ParentID == 1)
                    doc.ReplaceText("%ChucVu%", vemhis.ChucVu.ToUpper() + " - " + vemhis.DepartmentName.ToUpper());
                else
                    doc.ReplaceText("%ChucVu%", vemhis.ChucVu.ToUpper());

            if (vemhis.DepartmentName != null)
            {
                doc.ReplaceText("%DonVi%", vemhis.DepartmentName);
                doc.ReplaceText("%TruongPhong%", "Trưởng " + vemhis.DepartmentName);
            } 

            if(vemhis.NgayXuongTau !=null)
                doc.ReplaceText("%NgayXuongTau%", vemhis.NgayXuongTau.Value.ToString("dd/MM/yyyy"));

            Boolean gioiTinh = true;
            try {
                gioiTinh = Convert.ToBoolean((from p in dc.HRM_EMPLOYEE
                                              where p.EmployeeID == vemhis.EmployeeID
                                              select p.Sex).FirstOrDefault());
            }
            catch { };
            
            if(gioiTinh)
                doc.ReplaceText("%GioiTinh%", "Ông");
            else
                doc.ReplaceText("%GioiTinh%", "Bà");

            int salary = Convert.ToInt32((from p in dc.HRM_EMPLOYMENTHISTORY
                          where p.EmploymentHistoryID == vemhis.EmploymentHistoryID
                          select p.Salary).FirstOrDefault());

            if(salary != 0)
                doc.ReplaceText("%LuongCoBan%", salary.ToString("#,###"));

            doc.SaveAs(stream);
            return File(stream.ToArray(), "application/octet-stream", "QDTD.docx");
        }
        
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Create(int loaiNV)
        {
            HRM_EMPLOYMENTHISTORY item = new HRM_EMPLOYMENTHISTORY();

            int DecisionNo = 0;
            int departmentID = 0;

            //try
            //{
            //    //kiểm tra danh sách để lấy theo năm hiện tại trước

            //    if (loaiNV == 1)
            //    {
            //        var query = db.HRM_EMPLOYMENTHISTORY.Where(x => x.DecisionDate.Value.Year == DateTime.Now.Year && x.CategoryDecisionID == 2 && x.XacNhan == true && x.DIC_DEPARTMENT.ParentID != 1).OrderByDescending(x => x.DecisionNo).First();
            //        DecisionNo = (int)query.DecisionNo;

            //        var listPhongBan = db.DIC_DEPARTMENT.Where(x => 1 == 1 && (x.ParentID == 8 || x.ParentID == 17) && x.DepartmentID != 11 && x.DepartmentID != 24 && x.DepartmentID != 17).OrderBy(x => x.ParentID).ThenBy(x => x.DepartmentID);
            //        ViewBag.DepartmentID = new SelectList(listPhongBan, "DepartmentID", "DepartmentName");

            //        departmentID = 21;


            //        var listChucDanh = db.DIC_POSITION.Where(x => x.GroupPositionID != 4).OrderBy(x => x.PositionID);
            //        ViewBag.PositionID = new SelectList(listChucDanh, "PositionID", "PositionName");

            //        ViewBag.DepartmentPluralityID = new SelectList(listPhongBan, "DepartmentID", "DepartmentName");
            //        ViewBag.PluralityID = new SelectList(listChucDanh, "PositionID", "PositionName");

            //        item.PerPosition = 100;
            //        item.Salary = 0;
            //        item.AllowanceSalary = 0;
            //        item.Bonus = 0;
            //        item.AllowanceBonus = 0;
            //        item.PerPlurality = 0;
            //        item.SalaryPlurality = 0;
            //        item.AllowanceBonusPlurality = 0;
            //        item.BonusPlurality = 0;
            //        item.AllowanceBonusPlurality = 0;

            //    }
            //    else
            //    {
            //        var query = db.HRM_EMPLOYMENTHISTORY.Where(x => x.DecisionDate.Value.Year == DateTime.Now.Year && x.CategoryDecisionID == 2 && x.XacNhan == true && x.DIC_DEPARTMENT.ParentID == 1).OrderByDescending(x => x.DecisionNo).First();
            //        DecisionNo = (int)query.DecisionNo;

            //        var listPhongBan = db.DIC_DEPARTMENT.Where(x => x.ParentID == 1).OrderBy(x => x.DepartmentID);
            //        ViewBag.DepartmentID = new SelectList(listPhongBan, "DepartmentID", "DepartmentName");

            //        var listChucDanh = db.DIC_POSITION.Where(x => x.GroupPositionID == 4).OrderBy(x => x.PositionID);
            //        ViewBag.PositionID = new SelectList(listChucDanh, "PositionID", "PositionName");

            //        ViewBag.DepartmentPluralityID = new SelectList(listPhongBan, "DepartmentID", "DepartmentName");
            //        ViewBag.PluralityID = new SelectList(listChucDanh, "PositionID", "PositionName");

            //        departmentID = 2;

            //        item.PerPosition = 100;
            //        item.Salary = 0;
            //        item.AllowanceSalary = 0;
            //        item.Bonus = 0;
            //        item.AllowanceBonus = 0;
            //        item.PerPlurality = 0;
            //        item.SalaryPlurality = 0;
            //        item.AllowanceBonusPlurality = 0;

            //        item.BonusPlurality = 0;
            //        item.AllowanceBonusPlurality = 0;
            //    }
            //}
            //catch (Exception) { }


            var query = db.HRM_EMPLOYMENTHISTORY.Where(x => x.DecisionDate.Value.Year == DateTime.Now.Year && x.CategoryDecisionID == 2 && x.XacNhan == true && x.DIC_DEPARTMENT.ParentID != 1).OrderByDescending(x => x.DecisionNo).FirstOrDefault();
            if(loaiNV == 0)
                query = db.HRM_EMPLOYMENTHISTORY.Where(x => x.DecisionDate.Value.Year == DateTime.Now.Year && x.CategoryDecisionID == 2 && x.XacNhan == true && x.DIC_DEPARTMENT.ParentID == 1).OrderByDescending(x => x.DecisionNo).FirstOrDefault();

            if (query != null)
                try
                {
                    DecisionNo = (int)query.DecisionNo;
                }
                catch { DecisionNo = 1; }

            else
                DecisionNo = 1;

            var listPhongBan = db.DIC_DEPARTMENT.Where(x => 1 == 1 && (x.ParentID == 8 || x.ParentID == 17) && x.DepartmentID != 11 && x.DepartmentID != 24 && x.DepartmentID != 17).OrderBy(x => x.ParentID).ThenBy(x => x.DepartmentID);
            if(loaiNV == 0)
                listPhongBan = db.DIC_DEPARTMENT.Where(x => 1 == 1 && (x.ParentID == 1)).OrderBy(x => x.ParentID).ThenBy(x => x.DepartmentID);

            ViewBag.DepartmentID = new SelectList(listPhongBan, "DepartmentID", "DepartmentName");
           

            var listChucDanh = db.DIC_POSITION.Where(x => x.GroupPositionID != 4).OrderBy(x => x.PositionID);
            if (loaiNV == 0)
                listChucDanh = db.DIC_POSITION.Where(x => x.GroupPositionID == 4).OrderBy(x => x.PositionID);

            ViewBag.PositionID = new SelectList(listChucDanh, "PositionID", "PositionName");

            ViewBag.DepartmentPluralityID = new SelectList(listPhongBan, "DepartmentID", "DepartmentName");
            ViewBag.PluralityID = new SelectList(listChucDanh, "PositionID", "PositionName");

                //item.PerPosition = 100;
                //item.Salary = 0;
                //item.AllowanceSalary = 0;
                //item.Bonus = 0;
                //item.AllowanceBonus = 0;
                //item.PerPlurality = 0;
                //item.SalaryPlurality = 0;
                //item.AllowanceBonusPlurality = 0;
                //item.BonusPlurality = 0;
                //item.AllowanceBonusPlurality = 0;


            item.DecisionNo = DecisionNo + 1;            
            item.XacNhan = true;
            item.DepartmentID = departmentID;

            //if (loaiNV == 1)
            //    item.DepartmentID = 21; // Thuyen vien moi
            //item.DepartmentPluralityID = 21;
            //item.DepartmentID = 21;
            //item.SalaryPositionID = 1;
            //item.PerPosition = 100;
            //item.PerPlurality = 0;
            //item.Salary = 0;

            var listCategoryDecision = from p in db.DIC_CATEGORYDECISION
                                       where p.CategoryDecisionID == 2                                       
                                       select p;

            ViewBag.CategoryDecisionID = new SelectList(listCategoryDecision, "CategoryDecisionID", "CategoryDecisionName");         

            var listNhanVien = from p in db.HRM_EMPLOYEE
                               where p.StatusID != 1 || !(from o in db.HRM_EMPLOYMENTHISTORY
                                                          select o.EmployeeID).Contains(p.EmployeeID)
                               orderby p.LastName
                               select p;

            ViewBag.EmployeeID = new SelectList(listNhanVien, "EmployeeID", "HoTen");
            //ViewBag.LoaiTauID = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau");           
            //ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name");            
            //ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName");
            //ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");
            //ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");

            //var rankID = from a in db.DIC_SALARY                           
            //             select a;
            
            //ViewBag.SalaryPositionID = new SelectList(rankID, "SalaryID", "RankID");           
            //ViewBag.SalaryPluralityID = new SelectList(rankID, "SalaryID", "RankID");

            item.SalaryPositionID = 1;
            item.PerPosition = 100;
            item.PerPlurality = 0;            

            return PartialView(item);
        }

        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmploymentHistoryID,DecisionNo,DecisionDate,EffectiveDate,ContentDecision,CategoryDecisionID,EmployeeID,DepartmentID,PositionID,InternshipPosition,DepartmentPluralityID,PluralityID,IntershipPlurality,Note,PerPosition,PerPlurality,Salary,AllowanceSalary,Bonus,AllowanceBonus,SalaryPositionID,SalaryPluralityID,DepartmentName,LoaiTauID,Gross,Power,NgayXuongTau,XacNhan,LyDoNghiViec_ID")] HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {               
                db.HRM_EMPLOYMENTHISTORY.Add(hRM_EMPLOYMENTHISTORY);                
                db.SaveChanges();

                // Set tình trạng đang làm việc
                HRM_EMPLOYEE em = (from p in db.HRM_EMPLOYEE
                                   where p.EmployeeID == hRM_EMPLOYMENTHISTORY.EmployeeID
                                   select p).FirstOrDefault();

                em.StatusID = 1;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView(hRM_EMPLOYMENTHISTORY);
        }

        // POST: QuyetDinhTuyenDung/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.



        // POST: QuyetDinhTuyenDung/Edit/5
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
            ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name", hRM_EMPLOYMENTHISTORY.LyDoNghiViec_ID);

            return PartialView(hRM_EMPLOYMENTHISTORY);
        }

        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        // GET: QuyetDinhTuyenDung/Delete/5
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

        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        // POST: QuyetDinhTuyenDung/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Find(id);

            // Set tình trạng nghỉ việc
            HRM_EMPLOYEE em = (from p in db.HRM_EMPLOYEE
                               where p.EmployeeID == hRM_EMPLOYMENTHISTORY.EmployeeID
                               select p).FirstOrDefault();

            em.StatusID = 6;
            db.SaveChanges();


            
            db.HRM_EMPLOYMENTHISTORY.Remove(hRM_EMPLOYMENTHISTORY);            
            db.SaveChanges();            

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        //HAM LAY THONG TIN TIEN LUONG THAM KHAO
        [WebMethod]
        public JsonResult GetRank(int? DepartmentID, int PositionID)
        {
            if (DepartmentID.HasValue)
            {
                var rank = db.DIC_SALARY.Where(x => x.DepartmentID == DepartmentID && x.PositionID == PositionID && x.RankID == 1)
                            .Select(a => new
                            {
                                SalaryID = a.SalaryID,                                
                                Salary = a.Salary
                            });
                return Json(rank);
            }
            else return Json(null);

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
