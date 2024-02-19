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
using System.Web.Services;

namespace WebAuLac.Controllers
{
    [Authorize(Roles = "HR, Boss")]
    public class QuyetDinhNghiViecController : Controller
    {
        private int m_loaiQuyetDinhID = 3;

        private AuLacEntities db = new AuLacEntities();

        [Authorize(Roles = "HR, Boss")]
        public ActionResult IndexNghiViec()
        {
            int loaibaocao = 100;
            TuNgayDenNgay obj = new TuNgayDenNgay();
            obj.TuNgay = new DateTime(DateTime.Now.Year, 1, 1);
            obj.DenNgay = new DateTime(DateTime.Now.Year, 12, 31);
            var result = db.sp_LayDSNghiViec(obj.TuNgay, obj.DenNgay, loaibaocao).ToList();
            ViewBag.BangKe = result;
            return View(obj);
        }

        [Authorize(Roles = "HR, Boss")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IndexNghiViec([Bind(Include = "TuNgay,DenNgay")] TuNgayDenNgay obj)
        {
            if (ModelState.IsValid)
            {
                var result = db.sp_LayDSNghiViec(obj.TuNgay, obj.DenNgay, 1).ToList();
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

            if (loaiNV == 1)
            {
                var listNhanVien = from p in db.HRM_EMPLOYEE
                                   where (from o in db.viewHRM_EMPLOYMENTHISTORY
                                          where o.ParentID != 1 
                                          select o.EmployeeID).Contains(p.EmployeeID) && p.StatusID ==1 
                                   select p;

                ViewBag.EmployeeID = new SelectList(listNhanVien, "EmployeeID", "HoTen");
            }
            else
            {
                var listNhanVien = from p in db.HRM_EMPLOYEE
                                   where (from o in db.viewHRM_EMPLOYMENTHISTORY
                                          where o.ParentID == 1
                                          select o.EmployeeID).Contains(p.EmployeeID) && p.StatusID == 1
                                   select p;

                ViewBag.EmployeeID = new SelectList(listNhanVien, "EmployeeID", "HoTen");
            }

            // Chọn quyết định gần nhất để lấy phòng ban, chức vụ mặc định

                
            ViewBag.LoaiTauID = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau");

            ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name");
            var departmentID = from a in db.DIC_DEPARTMENT
                               where a.IsLast == true
                               select a;
            var positionID = from a in db.DIC_POSITION
                             select a;


            ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName");
            ViewBag.DepartmentID = new SelectList(departmentID, "DepartmentID", "DepartmentName");
            ViewBag.PositionID = new SelectList(positionID, "PositionID", "PositionName");
            ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName");
            ViewBag.DepartmentPluralityID = new SelectList(departmentID, "DepartmentID", "DepartmentName");
            ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");
            ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");

            var rankID = from a in db.DIC_SALARY
                         select a;

            ViewBag.SalaryPositionID = new SelectList(rankID, "SalaryID", "RankID");
            ViewBag.SalaryPluralityID = new SelectList(rankID, "SalaryID", "RankID");

            item.SalaryPositionID = 1;
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
                db.HRM_EMPLOYMENTHISTORY.Add(hRM_EMPLOYMENTHISTORY);
                db.SaveChanges();

                // Set tình trạng nghỉ việc = 5
                HRM_EMPLOYEE em = (from p in db.HRM_EMPLOYEE
                                  where p.EmployeeID == hRM_EMPLOYMENTHISTORY.EmployeeID
                                  select p).FirstOrDefault();

                em.StatusID = 5;
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
            ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name", hRM_EMPLOYMENTHISTORY.LyDoNghiViec_ID);

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
            ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name", hRM_EMPLOYMENTHISTORY.LyDoNghiViec_ID);

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

        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public FileResult XuatQuyetDinhNghiViec(int QTCT_ID)
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


            if (vemhis.ParentID == 1)
                fileNameTemplate = server + @"//Mau_QDTVVanPhong.docx";
            else
                fileNameTemplate = server + @"//Mau_QDTV.docx";


            doc = DocX.Load(fileNameTemplate);
            string SoQD = "";
            if (vemhis.DecisionNo < 10)
                SoQD = "00" + vemhis.DecisionNo.ToString();
            else if (vemhis.DecisionNo < 100)
                SoQD = "0" + vemhis.DecisionNo.ToString();
            else
                SoQD = vemhis.DecisionNo.ToString();

            if (vemhis.DecisionNo != null)
                doc.ReplaceText("%SoQD%", SoQD + "/" + vemhis.DecisionDate.Value.Year.ToString().Substring(2, 2) + "/QĐ-TV");

            doc.ReplaceText("%Ngay%", vemhis.DecisionDate.Value.Day.ToString());
            doc.ReplaceText("%Thang%", vemhis.DecisionDate.Value.Month.ToString());
            doc.ReplaceText("%Nam%", vemhis.DecisionDate.Value.Year.ToString());
            doc.ReplaceText("%Name%", vemhis.FirstName.ToUpper() + " " + vemhis.LastName.ToUpper());

            if (vemhis.BirthDay != null)
                doc.ReplaceText("%NamSinh%", vemhis.BirthDay.Value.Year.ToString());

            //if (vemhis.MainAddress != null)
            //    doc.ReplaceText("%DiaChi%", vemhis.MainAddress);

            if (vemhis.ChucVu != null)
                doc.ReplaceText("%ChucVu%", vemhis.ChucVu);
          
            if (vemhis.DepartmentName != null)
            {
                doc.ReplaceText("%DonVi%", vemhis.DepartmentName);
                //doc.ReplaceText("%TruongPhong%", "Trưởng " + vemhis.DepartmentName);
            }

            if (vemhis.NgayXuongTau != null)
                doc.ReplaceText("%NgayHieuLuc%", vemhis.NgayXuongTau.Value.ToString("dd/MM/yyyy"));

            Boolean gioiTinh = true;
            try
            {
                gioiTinh = Convert.ToBoolean((from p in dc.HRM_EMPLOYEE
                                              where p.EmployeeID == vemhis.EmployeeID
                                              select p.Sex).FirstOrDefault());
            }
            catch { };

            if (gioiTinh)
                doc.ReplaceText("%GioiTinh%", "ông");
            else
                doc.ReplaceText("%GioiTinh%", "bà");

            HRM_CONTRACTHISTORY objHopDong;
            string ngayHD, thangHD, namHD;
            ngayHD = "";
            thangHD = "";
            namHD = "";
            try
            {
                objHopDong = (from p in dc.HRM_CONTRACTHISTORY
                              where p.EmployeeID == vemhis.EmployeeID
                              select p).FirstOrDefault();
                DateTime dayContract = Convert.ToDateTime(objHopDong.ContractDate);

                var listngay = dc.sp_GetYMD(vemhis.DecisionDate.Value, dayContract).ToList();
                string ngay = listngay[0];
                string[] ngaythangnam = ngay.Split(' ');
                for (int i = 0; i < ngaythangnam.Length; i++)
                {
                    string cuoi = ngaythangnam[i].Substring(ngaythangnam[i].Length -1,1);
                    switch (cuoi)
                    {
                        case "y":
                            namHD = ngaythangnam[i].Substring(0, ngaythangnam[i].Length - 1);
                            break;
                        case "m":                           
                            thangHD = ngaythangnam[i].Substring(0, ngaythangnam[i].Length - 1);
                            break;
                        case "d":
                            ngayHD = ngaythangnam[i].Substring(0, ngaythangnam[i].Length - 1);
                            break;
                        default:                           
                            break;
                    }
                }
            }
            catch { };

            //view_quatrinhcongtacFull vemhis = (from p in dc.view_quatrinhcongtacFull
            //                                   where p.EmploymentHistoryID == QTCT_ID
            //                                   select p).SingleOrDefault();

            if(namHD != "")
                doc.ReplaceText("%NamLV%", namHD + " năm");
            else
                doc.ReplaceText("%NamLV%", "");

            if (thangHD != "")
                doc.ReplaceText("%ThangLV%", thangHD + " tháng");
            else
                doc.ReplaceText("%ThangLV%", "");

            if (ngayHD != "")
                doc.ReplaceText("%NgayLV%", ngayHD + " ngày");
            else
                doc.ReplaceText("%NgayLV%", "");

            string LyDoNghiViec = (from p in dc.tbl_LyDoNghiViec
                                   where p.LyDoNghiViec_ID == vemhis.LyDoNghiViec_ID
                                   select p.LyDoNghiViec_Name).SingleOrDefault();

            if(LyDoNghiViec != null)
                doc.ReplaceText("%LyDo%", LyDoNghiViec);

            int salary = Convert.ToInt32((from p in dc.DIC_SALARY_DEPARTMENT
                                          where p.DepartmentID == 18 && p.PositionID == vemhis.PositionID
                                          select p.Salary).FirstOrDefault());

            if (salary != 0)
                doc.ReplaceText("%LuongCoBan%", salary.ToString("#,###").Replace(',', '.'));

            var listHDLD = from p in dc.HRM_CONTRACTHISTORY
                           where p.EmployeeID == vemhis.EmployeeID
                           orderby p.ContractDate
                           select p;

            string HDLD = "";
            foreach (HRM_CONTRACTHISTORY obj in listHDLD)
            {
                //Hợp đồng lao động thuyền viên số 026 / 18 / HĐLĐ - TV ngày 16 / 03 / 2018;
                if(obj.ContractNo != null)
                    HDLD += " Hợp đồng lao động thuyền viên số " + obj.ContractNo + " ngày " + obj.ContractDate.Value.ToString("dd/MM/yyyy") + ";";
                else
                    HDLD += " Hợp đồng lao động thuyền viên số " +  " ngày " + obj.ContractDate.Value.ToString("dd/MM/yyyy") + ";";

            }
            HDLD = HDLD.Substring(0, HDLD.Length - 1); //Bỏ dấu ; cuối cùng
            HDLD = HDLD.Substring(1, HDLD.Length - 1); //Bỏ khoảng trắng " " đầu tiên

            HDLD += " và các phụ lục hợp đồng;";
            doc.ReplaceText("%HopDongLaoDong%", HDLD);


            doc.SaveAs(stream);
            return File(stream.ToArray(), "application/octet-stream", "QDTV.docx");
        }

        //HAM LAY THONG TIN TIEN LUONG THAM KHAO
        [WebMethod]
        public JsonResult GetDepartmentPosition(int? EmployeeID)
        {
            if (EmployeeID.HasValue)
            {
                var rank = db.view_FirstEMPLOYMENTHISTORY.Where(x => x.EmployeeID == EmployeeID)
                            .Select(a => new
                            {
                                DepartmentID = a.DepartmentID,
                                PositionID = a.PositionID
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
