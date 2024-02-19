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
using System.IO;
using Novacode;

namespace WebAuLac.Controllers
{

    [Authorize(Roles = "HR, Boss")]
    public class QuyetDinhKyLuatController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            HRM_EMPLOYEE_DISCIPLINE item = new HRM_EMPLOYEE_DISCIPLINE();

            int DecisionNo = 0;
            try
            {
                //kiểm tra danh sách để lấy theo năm hiện tại trước
                var query = db.HRM_EMPLOYEE_DISCIPLINE.Where(x => x.DisciplineDate.Value.Year == DateTime.Now.Year).OrderByDescending(x => x.DisciplineNo).First();
                DecisionNo = (int)query.DisciplineNo;
            }
            catch (Exception) { }

            item.DisciplineNo = DecisionNo + 1;
            item.DisciplineDate = DateTime.Now;

            item.CatThuong = false;

            var listKyLuat = from p in db.DIC_TYPE_OF_DISCIPLINE
                             where p.TypeOfDisciplineID != 5
                             select p;

            ViewBag.TypeOfDisciplineID = new SelectList(listKyLuat, "TypeOfDisciplineID", "TypeOfDisciplineName");
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "HoTen");
            return PartialView(item);
        }

        // GET: QuyetDinhKyLuat
        [Authorize(Roles = "HR, Boss")]       
        public ActionResult Index()
        {
            //TuNgayDenNgay obj = new TuNgayDenNgay();
            //obj.TuNgay = new DateTime(DateTime.Now.Year, 1, 1);
            //obj.DenNgay = new DateTime(DateTime.Now.Year, 12, 31);
            var result = db.HRM_EMPLOYEE_DISCIPLINE.OrderByDescending(x=> x.DisciplineDate).ThenByDescending(x=>x.DisciplineNo).ToList();
            ViewBag.BangKe = result;
            return View(result);
        }

        [Authorize(Roles = "HR, Boss")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "TuNgay,DenNgay")] TuNgayDenNgay obj)
        {
            if (ModelState.IsValid)
            {
                var result = db.HRM_EMPLOYEE_DISCIPLINE.ToList();
                ViewBag.BangKe = result;
                return View(obj);
            }
            return View();
        }

        // GET: QuyetDinhKyLuat/Details/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE_DISCIPLINE hRM_EMPLOYEE_DISCIPLINE = await db.HRM_EMPLOYEE_DISCIPLINE.FindAsync(id);
            if (hRM_EMPLOYEE_DISCIPLINE == null)
            {
                return HttpNotFound();
            }
            return View(hRM_EMPLOYEE_DISCIPLINE);
        }

        // GET: QuyetDinhKyLuat/Create
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult CreateGiamChucDanh()
        {
            HRM_EMPLOYEE_DISCIPLINE item = new HRM_EMPLOYEE_DISCIPLINE();

            int DecisionNo = 0;
            try
            {
                //kiểm tra danh sách để lấy theo năm hiện tại trước
                var query = db.HRM_EMPLOYEE_DISCIPLINE.Where(x => x.DisciplineDate.Value.Year == DateTime.Now.Year).OrderByDescending(x => x.DisciplineNo).First();
                DecisionNo = (int)query.DisciplineNo;
            }
            catch (Exception) { }

            item.DisciplineNo = DecisionNo + 1;
            item.DisciplineDate = DateTime.Now;

            item.CatThuong = false;

            var listKyLuat = from p in db.DIC_TYPE_OF_DISCIPLINE
                             where p.TypeOfDisciplineID == 5
                             select p;

            ViewBag.TypeOfDisciplineID = new SelectList(listKyLuat, "TypeOfDisciplineID", "TypeOfDisciplineName");
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "HoTen");

            var listPB = from p in db.DIC_DEPARTMENT
                         select p;
            ViewBag.DepartmentID = new SelectList(listPB, "DepartmentID", "DepartmentName");

            var listCD = from p in db.DIC_POSITION
                         select p;
            ViewBag.PositionID = new SelectList(listCD, "PositionID", "PositionName");

            return PartialView(item);


            //HRM_EMPLOYMENTHISTORY item = new HRM_EMPLOYMENTHISTORY();

            //int DecisionNo = 0;
            //try
            //{
            //    //kiểm tra danh sách để lấy theo năm hiện tại trước
            //    var query = db.HRM_EMPLOYEE_DISCIPLINE.Where(x => x.DisciplineDate.Value.Year == DateTime.Now.Year).OrderByDescending(x => x.DisciplineNo).First();
            //    DecisionNo = (int)query.DisciplineNo;
            //}
            //catch (Exception) { }

            //item.DecisionNo = DecisionNo + 1;
            //item.PerPosition = 100;
            //item.PerPlurality = 0;
            //item.XacNhan = true;

            //item.CategoryDecisionID = 5;
            //var listCategoryDecision = from p in db.DIC_CATEGORYDECISION
            //                           where p.CategoryDecisionID == 5
            //                           select p;

            //item.DecisionDate = DateTime.Now;

            //ViewBag.CategoryDecisionID = new SelectList(listCategoryDecision, "CategoryDecisionID", "CategoryDecisionName");
            //ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "HoTen");
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
            //IQueryable<DIC_TYPE_OF_DISCIPLINE> danhSach = db.DIC_TYPE_OF_DISCIPLINE;
            //ViewBag.TypeOfDisciplineID = new SelectList(danhSach, "TypeOfDisciplineID", "TypeOfDisciplineName");
            //return PartialView(item);
        }

        // POST: QuyetDinhKyLuat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGiamChucDanh([Bind(Include = "EmployeeDisciplineID,EmployeeID,TypeOfDisciplineID,DisciplineNo,DisciplineDate,Reason,EmploymentHistoryID,NgayHopHoiDong,HinhThucKyLuat,CatThuong")] HRM_EMPLOYEE_DISCIPLINE hRM_EMPLOYEE_DISCIPLINE, FormCollection frm)
        {
            //string LyDo = (frm["Reason"].ToString());
            //string HinhThucKyLuat = (frm["HinhThucKyLuat"].ToString());
            //DateTime ngayHopHoiDong = Convert.ToDateTime(frm["NgayHopHoiDong"]);
            //Boolean CatThuong = Convert.ToBoolean(frm["CatThuong"]);

            int IDPhongBan = int.Parse(frm["DepartmentID"]);
            int IDChucVu = int.Parse(frm["PositionID"]);

            if (ModelState.IsValid)
            {

                //HRM_EMPLOYMENTHISTORY qtct = new HRM_EMPLOYMENTHISTORY();
                //qtct.EmployeeID = 2;
                //qtct.DecisionNo = 100;
                //qtct.DecisionDate = DateTime.Now.Date;
                //qtct.DepartmentID = 10;
                //qtct.PositionID = 20;
                //qtct.Salary = 0;
                //qtct.PerPlurality = 0;
                //qtct.PerPosition = 100;
                //qtct.SalaryPositionID = 1;

                //qtct.XacNhan = true;
                //qtct.CategoryDecisionID = 1;

                //db.HRM_EMPLOYMENTHISTORY.Add(qtct);
                //db.SaveChanges();


                //Them moi QuyetDinh Ky luat vào QTCT
                HRM_EMPLOYMENTHISTORY qd = new HRM_EMPLOYMENTHISTORY();
                qd.DecisionNo = Convert.ToInt32(hRM_EMPLOYEE_DISCIPLINE.DisciplineNo);
                qd.DecisionDate = hRM_EMPLOYEE_DISCIPLINE.DisciplineDate;
                qd.EffectiveDate = hRM_EMPLOYEE_DISCIPLINE.DisciplineDate;
                qd.NgayXuongTau = hRM_EMPLOYEE_DISCIPLINE.DisciplineDate;
                qd.CategoryDecisionID = 5;
                qd.DepartmentID = IDPhongBan;
                qd.PositionID = IDChucVu;
                qd.Salary = 0;
                qd.PerPlurality = 0;
                qd.PerPosition = 100;
                qd.SalaryPositionID = 1;


                qd.EmployeeID = hRM_EMPLOYEE_DISCIPLINE.EmployeeID;
                qd.XacNhan = true;


                db.HRM_EMPLOYMENTHISTORY.Add(qd);
                db.SaveChanges();

                // Bổ sung QTCT để xóa
                hRM_EMPLOYEE_DISCIPLINE.EmploymentHistoryID = qd.EmploymentHistoryID;
                db.HRM_EMPLOYEE_DISCIPLINE.Add(hRM_EMPLOYEE_DISCIPLINE);
                db.SaveChanges();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView(hRM_EMPLOYEE_DISCIPLINE);
        }

        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeDisciplineID,EmployeeID,TypeOfDisciplineID,DisciplineNo,DisciplineDate,Reason,EmploymentHistoryID,NgayHopHoiDong,HinhThucKyLuat,CatThuong")] HRM_EMPLOYEE_DISCIPLINE hRM_EMPLOYEE_DISCIPLINE)
        {
            if (ModelState.IsValid)
            {
                db.HRM_EMPLOYEE_DISCIPLINE.Add(hRM_EMPLOYEE_DISCIPLINE);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            //var listTypeDiscipline = from p in db.DIC_TYPE_OF_DISCIPLINE
            //                         where p.TypeOfDisciplineID == hRM_EMPLOYEE_DISCIPLINE.TypeOfDisciplineID
            //                         select p;

            //ViewBag.TypeOfDisciplineID = new SelectList(listTypeDiscipline, "TypeOfDisciplineID", "TypeOfDisciplineName", hRM_EMPLOYEE_DISCIPLINE.TypeOfDisciplineID);

            //var listNhanVien = from p in db.HRM_EMPLOYEE
            //                   where p.EmployeeID == hRM_EMPLOYEE_DISCIPLINE.EmployeeID
            //                   select p;

            //ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "HoTen", hRM_EMPLOYEE_DISCIPLINE.EmployeeID);
            return View(hRM_EMPLOYEE_DISCIPLINE);
        }

        // GET: QuyetDinhKyLuat/Edit/5    
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE_DISCIPLINE hRM_EMPLOYEE_DISCIPLINE =db.HRM_EMPLOYEE_DISCIPLINE.Find(id);
            if (hRM_EMPLOYEE_DISCIPLINE == null)
            {
                return HttpNotFound();
            }

            var listTypeDiscipline = from p in db.DIC_TYPE_OF_DISCIPLINE
                                     where p.TypeOfDisciplineID == hRM_EMPLOYEE_DISCIPLINE.TypeOfDisciplineID
                                     select p;

            var listEmployee = from p in db.HRM_EMPLOYEE
                                     where p.EmployeeID == hRM_EMPLOYEE_DISCIPLINE.EmployeeID
                                     select p;

            ViewBag.TypeOfDisciplineID = new SelectList(listTypeDiscipline, "TypeOfDisciplineID", "TypeOfDisciplineName", hRM_EMPLOYEE_DISCIPLINE.TypeOfDisciplineID);
            ViewBag.EmployeeID = new SelectList(listEmployee, "EmployeeID", "HoTen", hRM_EMPLOYEE_DISCIPLINE.EmployeeID);
            return PartialView(hRM_EMPLOYEE_DISCIPLINE);
        }

        // POST: QuyetDinhKyLuat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeDisciplineID,EmployeeID,TypeOfDisciplineID,DisciplineNo,DisciplineDate,Reason,EmploymentHistoryID,NgayHopHoiDong,HinhThucKyLuat,CatThuong")] HRM_EMPLOYEE_DISCIPLINE hRM_EMPLOYEE_DISCIPLINE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRM_EMPLOYEE_DISCIPLINE).State = EntityState.Modified;               
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.TypeOfDisciplineID = new SelectList(db.DIC_TYPE_OF_DISCIPLINE, "TypeOfDisciplineID", "TypeOfDisciplineName", hRM_EMPLOYEE_DISCIPLINE.TypeOfDisciplineID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_DISCIPLINE.EmployeeID);
            return PartialView(hRM_EMPLOYEE_DISCIPLINE);
        }

        // GET: QuyetDinhKyLuat/Delete/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE_DISCIPLINE hRM_EMPLOYEE_DISCIPLINE = db.HRM_EMPLOYEE_DISCIPLINE.Find(id);
            if (hRM_EMPLOYEE_DISCIPLINE == null)
            {
                return HttpNotFound();
            }
            return PartialView(hRM_EMPLOYEE_DISCIPLINE);            
        }

        // POST: QuyetDinhKyLuat/Delete/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {            
            HRM_EMPLOYEE_DISCIPLINE hRM_EMPLOYEE_DISCIPLINE = db.HRM_EMPLOYEE_DISCIPLINE.Find(id);
            if (hRM_EMPLOYEE_DISCIPLINE.EmploymentHistoryID != null)
            {
                HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Find(hRM_EMPLOYEE_DISCIPLINE.EmploymentHistoryID);
                db.HRM_EMPLOYMENTHISTORY.Remove(hRM_EMPLOYMENTHISTORY);
                db.SaveChanges();
            }

            db.HRM_EMPLOYEE_DISCIPLINE.Remove(hRM_EMPLOYEE_DISCIPLINE);
            db.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        //InQuyetDinhKyLuat
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public FileResult InQuyetDinhKyLuat(int id)
        {
            AuLacEntities dc = new AuLacEntities();

            MemoryStream stream = new MemoryStream();
            string server = Server.MapPath("~/App_Data");
            string fileNameTemplate = "";
            DocX doc = DocX.Create(stream);

            HRM_EMPLOYEE_DISCIPLINE emDis = (from p in db.HRM_EMPLOYEE_DISCIPLINE
                                             where p.EmployeeDisciplineID == id
                                             select p).FirstOrDefault();

            int EmployeeID = Convert.ToInt32(emDis.EmployeeID);

            viewHRM_EMPLOYMENTHISTORY vemhis = (from p in dc.viewHRM_EMPLOYMENTHISTORY
                                                where p.EmployeeID == EmployeeID 
                                           select p).SingleOrDefault();

            fileNameTemplate = server + @"//Mau_QDCanhCao.docx";

            doc = DocX.Load(fileNameTemplate);

            String soQD;
            if (emDis.DisciplineNo < 10)
                soQD = "0" + emDis.DisciplineNo.ToString();
            else
                soQD = emDis.DisciplineNo.ToString();

            if (vemhis.DecisionNo != null)
                doc.ReplaceText("%SoQD%", soQD + "/" + emDis.DisciplineDate.Value.Year.ToString().Substring(2, 2) + "/QĐ-KL");

            doc.ReplaceText("%Ngay%", emDis.DisciplineDate.Value.Day.ToString());
            doc.ReplaceText("%Thang%", emDis.DisciplineDate.Value.Month.ToString());
            doc.ReplaceText("%Nam%", emDis.DisciplineDate.Value.Year.ToString());
            doc.ReplaceText("%Name%", vemhis.FirstName.ToUpper() + " " + vemhis.LastName.ToUpper());

            if (vemhis.BirthDay != null)
                doc.ReplaceText("%NamSinh%", vemhis.BirthDay.Value.ToString("dd/MM/yyyy"));

            if (vemhis.DepartmentName != null)
                doc.ReplaceText("%DonVi%", vemhis.DepartmentName);

            if (vemhis.ChucVu != null)
                doc.ReplaceText("%ChucVu%", vemhis.ChucVu.ToUpper());

            if (emDis.Reason != null)
                doc.ReplaceText("%HanhViViPham%", emDis.Reason);

            if (emDis.HinhThucKyLuat != null)
                doc.ReplaceText("%HinhThucKyLuat%", emDis.HinhThucKyLuat);

            if (emDis.NgayHopHoiDong != null)
                doc.ReplaceText("%NgayHopHD%", emDis.NgayHopHoiDong.Value.ToString("dd/MM/yyyy"));

            if (emDis.CatThuong == true)
                doc.ReplaceText("%CatThuong%", "Cắt khen thưởng năm " + DateTime.Now.Year.ToString());
            else
                doc.ReplaceText("%CatThuong%", "");

            doc.SaveAs(stream);
            return File(stream.ToArray(), "application/octet-stream", "QDCanhCao.docx");
            
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
