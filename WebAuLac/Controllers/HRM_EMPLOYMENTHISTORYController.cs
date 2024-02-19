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
using System.Web.Routing;
using System.Web.Services;
using System.Collections;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace WebAuLac.Controllers
{
    [Authorize]
    public class HRM_EMPLOYMENTHISTORYController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: HRM_EMPLOYMENTHISTORY
        public async Task<ActionResult> Index()
        {
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");
            var hRM_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Include(h => h.DIC_CATEGORYDECISION).Include(h => h.DIC_DEPARTMENT).Include(h => h.HRM_EMPLOYEE);
            return View(await hRM_EMPLOYMENTHISTORY.ToListAsync());
        }
        public ActionResult ErrorNotExistsView()
        {
            return View("ErrorNotExistsView");
        }
        //set cache để nó load lại cái mới cập nhật
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult EmploymentHistoryofEmloyee(int EmployeeID)
        {
            // ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");
            ViewBag.EmployeeID = EmployeeID;
            var hRM_EMPLOYMENTHISTORY = db.view_quatrinhcongtac.Where(p => p.EmployeeID == EmployeeID).OrderBy(p=>p.DecisionDate).ThenBy(p=>p.EmploymentHistoryID);
            return PartialView("_EmploymentHistoryofEmloyee", hRM_EMPLOYMENTHISTORY.ToList());
        }
        //set cache để nó load lại cái mới cập nhật
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult EmploymentHistoryofEmloyee_goc(int EmployeeID)
        {
            // ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");
            ViewBag.EmployeeID = EmployeeID;
            var hRM_EMPLOYMENTHISTORY = db.view_qtct_goc.Where(p => p.EmployeeID == EmployeeID).OrderBy(p => p.DecisionDate).ThenBy(p => p.EmploymentHistoryID);
            return PartialView("_EmploymentHistoryofEmloyee_goc", hRM_EMPLOYMENTHISTORY.ToList());
        }
        public ActionResult XacNhanQuyetDinh(int planID)
        {
            //Trả lại dữ liệu danh sách quyết định của một kế hoạch
            //var hRM_EMPLOYEE = db.fn_IsLimitDegree().OrderByDescending(a =>a.ISLIMITDEGREE);
            var hRM_EMPLOYEE = db.fn_XacNhanQuyetDinh(planID);
            //  var hRM_EMPLOYEE = db.HRM_EMPLOYEE.Include(h => h.DIC_DEPARTMENT_POSITION).Include(h => h.DIC_EDUCATION).Include(h => h.DIC_ETHNIC).Include(h => h.DIC_NATIONALITY).Include(h => h.DIC_RELIGION).Include(h => h.DIC_STATUS);
            return View(hRM_EMPLOYEE.ToArray());

        }
        public ActionResult Employees(string EmployeeID)
        {
            string[] values = EmployeeID.Split('|');
            int idtv = Convert.ToInt32(values[0]);
            var hRM_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Include(h => h.DIC_CATEGORYDECISION).Include(h => h.DIC_DEPARTMENT).Where(h => h.HRM_EMPLOYEE.EmployeeID == idtv);
            ViewBag.idThuyenVien = idtv;

            ViewBag.idKeHoach = Convert.ToInt32(values[1]);
            ViewBag.idChitietKeHoach = Convert.ToInt32(values[2]);

            return View("Employees", hRM_EMPLOYMENTHISTORY.ToList());
            //return View(await hRM_EMPLOYEE_ACCIDENT.ToListAsync());
        }
        // GET: HRM_EMPLOYMENTHISTORY/Details/5
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
            return PartialView(hRM_EMPLOYMENTHISTORY);
        }

        // GET: HRM_EMPLOYMENTHISTORY/Create
        public ActionResult Create()
        {

            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName");
            //   ViewBag.Department_PositionID = new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName");
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");
            ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName");
            return View();
        }

        // POST: HRM_EMPLOYMENTHISTORY/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmploymentHistoryID,DecisionNo,DecisionDate,EffectiveDate,ContentDecision,CategoryDecisionID,EmployeeID,DepartmentID,PositionID,InternshipPosition,DepartmentPluralityID,PluralityID,IntershipPlurality,Note,PerPosition,PerPlurality,SalaryPositionID,SalaryPluralityID,DepartmentName,LoaiTauID,Gross,Power,NgayXuongTau,XacNhan,LyDoNghiViec_ID, Salary, AllowanceSalary, Bonus, AllowanceBonus, SalaryPlurality, AllowanceSalaryPlurality, BonusPlurality, AllowanceBonusPlurality")] HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY)

        {

            if (ModelState.IsValid)

            {
                //Lấy ngày thnasg hiện tại nếu k nhập vào 
                if (hRM_EMPLOYMENTHISTORY.DecisionDate == null)
                {
                    hRM_EMPLOYMENTHISTORY.DecisionDate = DateTime.Now;
                }
                //-----------THANH KHÓA ĐOẠN NÀY NGÀY 07/02/2018
                //---------CHỈ HIỂN THỊ DECISIONNO BAN ĐẦU THÔI, SAU ĐÓ NGƯỜI TA NHẬP GÌ THÌ KỆ HỌ
                //int DecisionNo = 0;
                //try
                //{
                //    //kiểm tra danh sách để lấy theo năm hiện tại trước
                //    var query = db.HRM_EMPLOYMENTHISTORY.Where(x => x.DecisionDate.Value.Year == hRM_EMPLOYMENTHISTORY.DecisionDate.Value.Year).OrderByDescending(x => x.DecisionNo).First();
                //    DecisionNo = (int)query.DecisionNo;

                //}
                //catch (Exception)
                //{
                //}
                //hRM_EMPLOYMENTHISTORY.DecisionNo = DecisionNo + 1;
                //4.	Đánh số quyết định theo loại quyết định (bổ nhiệm, điều đồng, thanh lý hợp đồng, kỷ luật) 
                //int DecisionNo = 0;
                //try
                //{
                //    //kiểm tra danh sách để lấy theo năm hiện tại trước
                //    var query = db.HRM_EMPLOYMENTHISTORY.Where(x => x.DecisionDate.Value.Year == hRM_EMPLOYMENTHISTORY.DecisionDate.Value.Year && x.CategoryDecisionID == hRM_EMPLOYMENTHISTORY.CategoryDecisionID).OrderByDescending(x => x.DecisionNo).First();
                //    DecisionNo = (int)query.DecisionNo;

                //}
                //catch (Exception)
                //{
                //}
                //hRM_EMPLOYMENTHISTORY.DecisionNo = DecisionNo + 1;



                db.HRM_EMPLOYMENTHISTORY.Add(hRM_EMPLOYMENTHISTORY);
                db.SaveChanges();

                var nhanvien = db.HRM_EMPLOYEE.Where(e => e.EmployeeID == hRM_EMPLOYMENTHISTORY.EmployeeID).First();
                //Setup Status ID của Employee là 5(nghỉ việc) nếu loại quyết định là 3(thanh lý hợp đồng)
               
                if (hRM_EMPLOYMENTHISTORY.CategoryDecisionID == 2) // ký hợp đồng tuyển dụng
                {                
                    nhanvien.StatusID = 1;
                    db.Entry(nhanvien).State = EntityState.Modified;
                    db.SaveChanges();
                   // return RedirectToAction("EmploymentHistoryofEmloyee", "hRM_EMPLOYMENTHISTORY", new { EmployeeID = hRM_EMPLOYMENTHISTORY.EmployeeID });
                }
                else
                {
                    if (hRM_EMPLOYMENTHISTORY.CategoryDecisionID == 3)
                    {
                        nhanvien.StatusID = 5;
                        db.Entry(nhanvien).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                 }
                //Cập nhật bảng nhân viên
                var nv = db.HRM_EMPLOYEE.Where(e => e.EmployeeID == hRM_EMPLOYMENTHISTORY.EmployeeID).First();
                nv.SSDD = false;
                db.Entry(nv).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EmploymentHistoryofEmloyee", "hRM_EMPLOYMENTHISTORY", new { EmployeeID = hRM_EMPLOYMENTHISTORY.EmployeeID });

            }
            //Loại tàu
            ViewBag.LoaiTauID = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau");
            //Loại quyết định
            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName");
            ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name");
            //    ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des"); //new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            //   ViewBag.Department_PositionID =  new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Coeff");
            var departmentID = from a in db.DIC_DEPARTMENT
                               where (a.ParentID == 8 || a.ParentID == 17) && a.IsLast == true
                               select a;
            var positionID = from a in db.DIC_POSITION
                                 //where a.GroupPositionID != 4
                             select a;
            ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName");
            //   ViewBag.EmployeeID = EmployeeID;// new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");

            ViewBag.DepartmentID = new SelectList(departmentID, "DepartmentID", "DepartmentName");
            ViewBag.PositionID = new SelectList(positionID, "PositionID", "PositionName");
            ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName");

            //Phòng ban kiêm nhiệm
            //ViewBag.DepartmentPluralityID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName");
            ViewBag.DepartmentPluralityID = new SelectList(departmentID, "DepartmentID", "DepartmentName");//new
            //Thực tập chức vụ?
            ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");
            //Thực tập chức vụ kiêm nhiệm
            ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");
            //Chức vụ kiêm nhiệm

            //ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PluralityID);//new
            //LẤY THÔNG TIN PHÒNG BAN VÀ CHỨC VỤ ĐỂ LẤY ĐƯỢC THÔNG TIN LƯƠNG
            var rankID = from a in db.DIC_SALARY
                             //where a.DepartmentID == hRM_EMPLOYMENTHISTORY.DepartmentID && a.PositionID == hRM_EMPLOYMENTHISTORY.PositionID
                         select a;

            //ViewBag.SalaryPositionID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPositionID = new SelectList(rankID, "SalaryID", "RankID");
            // ViewBag.SalaryPluralityID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPluralityID = new SelectList(rankID, "SalaryID", "RankID");

            var hrm_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Where(h => h.EmployeeID == hRM_EMPLOYMENTHISTORY.EmployeeID);
            return PartialView("CreateWithEmployee", hrm_EMPLOYMENTHISTORY);
        }
        // GET: HRM_EMPLOYMENTHISTORY/Create
        [Authorize(Roles = "HR, EduCenter")]
        [Authorize(Roles = "Create")]        
        public ActionResult CreateWithEmployee(int EmployeeID)
        {
            HRM_EMPLOYMENTHISTORY item = new HRM_EMPLOYMENTHISTORY();
            //        var departmentPositions =   db.DIC_DEPARTMENT_POSITION.ToList()
            //.Select(s => new
            //{
            //    Department_PositionID = s.Department_PositionID,
            //    Des = string.Format("{0} --- {1} ----{2}", s.DIC_DEPARTMENT.DepartmentName, s.DIC_POSITION.PositionName, s.Coeff)
            //});
            //Lấy số Quyết định tự tăng để hiển thị lên
            //Tự động tăng id DecisionNo
            //Lấy max DecisionNo + 1
            //Chủ động hiển thị số quyết định theo năm hiện tại cho dễ xử lý

            int DecisionNo = 0;
            try
            {
                //kiểm tra danh sách để lấy theo năm hiện tại trước
                var query = db.HRM_EMPLOYMENTHISTORY.Where(x=>x.DecisionDate.Value.Year== DateTime.Now.Year && x.CategoryDecisionID==4).OrderByDescending(x => x.DecisionNo).First();
                DecisionNo = (int)query.DecisionNo;

            }
            catch (Exception)
            {


            }
            //if (DateTime.Now.Day==1 && DateTime.Now.Month ==1) //Ngày đầu năm
            //{
            //    DecisionNo = 0;
            //}
            item.DecisionNo = DecisionNo + 1;
            item.EmployeeID = EmployeeID;
            item.PerPosition = 100;
            item.PerPlurality = 0;
            item.XacNhan = true;
            //THêm thông tin lương tham khảo


            //var departmentID = from a in db.DIC_DEPARTMENT
            //                   where a.ParentID == 8 || a.ParentID == 17
            //                   select a;

            //ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName");
            ////    ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des"); //new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            ////   ViewBag.Department_PositionID =  new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Coeff");

            ////ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName");
            //ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName");
            //ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName");
            //   ViewBag.EmployeeID = EmployeeID;// new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");
            //Loại tàu
            ViewBag.LoaiTauID = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau");
            //Loại quyết định
            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName");
            ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name");
            //    ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des"); //new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            //   ViewBag.Department_PositionID =  new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Coeff");
            var departmentID = from a in db.DIC_DEPARTMENT
                               where a.IsLast== true
                               select a;
            var positionID = from a in db.DIC_POSITION
                                 //where a.GroupPositionID != 4
                             select a;
            ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName");
            //   ViewBag.EmployeeID = EmployeeID;// new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");

            ViewBag.DepartmentID = new SelectList(departmentID, "DepartmentID", "DepartmentName");
            ViewBag.PositionID = new SelectList(positionID, "PositionID", "PositionName");
            ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName");

            //Phòng ban kiêm nhiệm
            //ViewBag.DepartmentPluralityID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName");
            ViewBag.DepartmentPluralityID = new SelectList(departmentID, "DepartmentID", "DepartmentName");//new
            //Thực tập chức vụ?
            ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");
            //Thực tập chức vụ kiêm nhiệm
            ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");
            //Chức vụ kiêm nhiệm

            //ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PluralityID);//new
            //LẤY THÔNG TIN PHÒNG BAN VÀ CHỨC VỤ ĐỂ LẤY ĐƯỢC THÔNG TIN LƯƠNG
            var rankID = from a in db.DIC_SALARY
                         //where a.DepartmentID == hRM_EMPLOYMENTHISTORY.DepartmentID && a.PositionID == hRM_EMPLOYMENTHISTORY.PositionID
                         select a;

            //ViewBag.SalaryPositionID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPositionID = new SelectList(rankID, "SalaryID", "RankID");
            // ViewBag.SalaryPluralityID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPluralityID = new SelectList(rankID, "SalaryID", "RankID");
            return PartialView(item);//Trả về create

        }

        //create one        
        public async Task<ActionResult> CreateOne(int EmployeeID, int PlanID, int detailPlanID)
        {
            HRM_EMPLOYMENTHISTORY item = new HRM_EMPLOYMENTHISTORY();
            ViewBag.EmployeeID = EmployeeID;
            item.EmployeeID = EmployeeID;
            ViewBag.idThuyenVien = EmployeeID;
            ViewBag.idKeHoach = PlanID;
            ViewBag.idChitietKeHoach = detailPlanID;
            //Lấy thông tin của người đang chọn để biết những thông tin có sẵn
            HRM_DETAILPLAN hRM_DETAILPLAN = db.HRM_DETAILPLAN.Where(h => h.PlanID == PlanID && h.DetailPlanID == detailPlanID && (h.CrewOffID == EmployeeID || h.CrewOnID == EmployeeID)).First();
            //Lưu thông tin có sẵn
            //thuyền viên xuống tàu
            if (hRM_DETAILPLAN.CrewOffID == EmployeeID)
            {
                item.PositionID = hRM_DETAILPLAN.OffPositionID;
                //Chuyển lên dự trữ
                item.DepartmentID = 17;
            }
            else
            {
                //Thuyền viên lên tàu
                item.PositionID = hRM_DETAILPLAN.OnPositionID;
                item.DepartmentID = hRM_DETAILPLAN.CrewOffDepartmentID;
            }

            Session["idtv"] = EmployeeID;
            Session["idkh"] = PlanID;
            Session["idctkh"] = detailPlanID;
            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", 4);
            //    ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des"); //new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            //   ViewBag.Department_PositionID =  new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Coeff");
            var departmentID = from a in db.DIC_DEPARTMENT
                               where a.ParentID == 8 || a.ParentID == 17
                               select a;
            var positionID = from a in db.DIC_POSITION
                             where a.GroupPositionID != 4
                             select a;
            ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName");
            //   ViewBag.EmployeeID = EmployeeID;// new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");

            ViewBag.DepartmentID = new SelectList(departmentID, "DepartmentID", "DepartmentName", item.DepartmentID);
            ViewBag.PositionID = new SelectList(positionID, "PositionID", "PositionName", item.PositionID);
            ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName");
            return PartialView("_CreateOne", item);
        }

        //create one
        [WebMethod]
        public async Task<ActionResult> CreateEachList()
        {

            string[] list1 = (string[])Session["list"];

            int index = (int)Session["index"];
            if (index > list1.Length - 1)
            {
                //Trả lại
                return RedirectToAction("Index", "HRM_QuyetDinhDieuDong", new { planID = (int)Session["idkh"] });
            }
            char[] delimiterChars = { '|' };
            string[] list = list1[index].Split(delimiterChars);
            int EmployeeID = Convert.ToInt32(list[0]);
            int PlanID = Convert.ToInt32(list[1]);
            int detailPlanID = Convert.ToInt32(list[2]);
            //Xóa phần tử đầu tiên trong list???
            index++;
            Session["list"] = list1;
            Session["index"] = index;
            HRM_EMPLOYMENTHISTORY item = new HRM_EMPLOYMENTHISTORY();
            ViewBag.EmployeeID = EmployeeID;
            item.EmployeeID = EmployeeID;
            ViewBag.idThuyenVien = EmployeeID;
            ViewBag.idKeHoach = PlanID;
            ViewBag.idChitietKeHoach = detailPlanID;
            try
            {
                HRM_EMPLOYEE eploy = (from a in db.HRM_EMPLOYEE
                                      where a.EmployeeID == EmployeeID
                                      select a).First();
                ViewBag.TenTV = eploy.FirstName + " " + eploy.LastName;
            }
            catch (Exception)
            {

                //Không tồn tại thuyền viên

            }


            //Lấy thông tin của người đang chọn để biết những thông tin có sẵn
            HRM_DETAILPLAN hRM_DETAILPLAN = db.HRM_DETAILPLAN.Where(h => h.PlanID == PlanID && h.DetailPlanID == detailPlanID && (h.CrewOffID == EmployeeID || h.CrewOnID == EmployeeID)).First();
            //Lưu thông tin có sẵn
            //thuyền viên xuống tàu
            if (hRM_DETAILPLAN.CrewOffID == EmployeeID)
            {
                item.PositionID = hRM_DETAILPLAN.OffPositionID;
                item.PluralityID = hRM_DETAILPLAN.OffPositionID;
                //Chuyển lên dự trữ
                item.DepartmentID = 17;
                item.DepartmentPluralityID = 17;
            }
            else
            {
                //Thuyền viên lên tàu
                item.PositionID = hRM_DETAILPLAN.OnPositionID;
                item.PluralityID = hRM_DETAILPLAN.OnPositionID;
                item.DepartmentID = hRM_DETAILPLAN.CrewOffDepartmentID;
                item.DepartmentPluralityID = hRM_DETAILPLAN.CrewOffDepartmentID;
                // về thực tập thêm một id null vào 

            }

            Session["idtv"] = EmployeeID;
            Session["idkh"] = PlanID;
            Session["idctkh"] = detailPlanID;
            //Lấy số Quyết định tự tăng để hiển thị lên
            //Tự động tăng id DecisionNo
            //Lấy max DecisionNo + 1
            int DecisionNo = 0;
            try
            {
                var query = db.HRM_EMPLOYMENTHISTORY.OrderByDescending(x => x.DecisionNo).First();
                DecisionNo = (int)query.DecisionNo;

            }
            catch (Exception)
            {


            }
            item.DecisionNo = DecisionNo + 1;

            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", 4);
            //    ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des"); //new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            //   ViewBag.Department_PositionID =  new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Coeff");
            var departmentID = from a in db.DIC_DEPARTMENT
                               where a.ParentID == 8 || a.ParentID == 17
                               select a;
            var positionID = from a in db.DIC_POSITION
                             where a.GroupPositionID != 4
                             select a;
            ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName");
            //   ViewBag.EmployeeID = EmployeeID;// new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");

            ViewBag.DepartmentID = new SelectList(departmentID, "DepartmentID", "DepartmentName", item.DepartmentID);
            ViewBag.PositionID = new SelectList(positionID, "PositionID", "PositionName", item.PositionID);
           // ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName");

            //Phòng ban kiêm nhiệm
            //ViewBag.DepartmentPluralityID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName");
            ViewBag.DepartmentPluralityID = new SelectList(departmentID, "DepartmentID", "DepartmentName");//new
            //Thực tập chức vụ?
            ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");
            //Thực tập chức vụ kiêm nhiệm
            ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");
            //Chức vụ kiêm nhiệm

            ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName");//new
            //LẤY THÔNG TIN PHÒNG BAN VÀ CHỨC VỤ ĐỂ LẤY ĐƯỢC THÔNG TIN LƯƠNG
            var rankID = from a in db.DIC_SALARY
                         where a.DepartmentID == item.DepartmentID && a.PositionID == item.PositionID
                         select a;

            //ViewBag.SalaryPositionID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPositionID = new SelectList(rankID, "SalaryID", "RankID", item.SalaryPositionID);
            // ViewBag.SalaryPluralityID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPluralityID = new SelectList(rankID, "SalaryID", "RankID", item.SalaryPluralityID);

            return PartialView("CreateEachList", item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEachList([Bind(Include = "EmploymentHistoryID,DecisionNo,DecisionDate,EffectiveDate,ContentDecision,CategoryDecisionID,EmployeeID,DepartmentID,PositionID,InternshipPosition,DepartmentPluralityID,PluralityID,IntershipPlurality,Note,PerPosition,PerPlurality,SalaryPositionID,SalaryPluralityID,DepartmentName,LoaiTauID,Gross,Power,NgayXuongTau,XacNhan,LyDoNghiViec_ID")] HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY)

        {
            if (ModelState.IsValid)
            {
                //Tự động tăng id DecisionNo
                //Lấy max DecisionNo + 1
                int DecisionNo = 0;
                try
                {
                    var query = db.HRM_EMPLOYMENTHISTORY.OrderByDescending(x => x.DecisionNo).First();
                    DecisionNo = (int)query.DecisionNo;

                }
                catch (Exception)
                {


                }
                if (hRM_EMPLOYMENTHISTORY.DecisionNo ==null)
                {
                    hRM_EMPLOYMENTHISTORY.DecisionNo = DecisionNo + 1;
                }
                
                if (hRM_EMPLOYMENTHISTORY.DecisionDate ==null)
                {
                    //Thêm ngày hiện tại
                    hRM_EMPLOYMENTHISTORY.DecisionDate = DateTime.Now;
                }
                db.HRM_EMPLOYMENTHISTORY.Add(hRM_EMPLOYMENTHISTORY);
                db.SaveChanges();
                //Lưu thành công thì cập nhật thông tin vào bảng detailplan
                try
                {
                    int idtv = Convert.ToInt32(Session["idtv"]);
                    int idkh = Convert.ToInt32(Session["idkh"]);
                    int idctkh = Convert.ToInt32(Session["idctkh"]);
                    // HRM_DETAILPLAN hRM_DETAILPLAN = db.HRM_DETAILPLAN.Where(h => (h.PlanID == ViewBag.idKeHoach) && (h.DetailPlanID == ViewBag.idChitietKeHoach) && (h.CrewOffID == ViewBag.idThuyenVien) || (h.CrewOnID == ViewBag.idThuyenVien));
                    HRM_DETAILPLAN hRM_DETAILPLAN = db.HRM_DETAILPLAN.Where(h => (h.CrewOffID == idtv || h.CrewOnID == idtv) && h.DetailPlanID == idctkh && h.PlanID == idkh).First();
                    //hRM_EMPLOYEE.Department_PositionID = hRM_EMPLOYMENTHISTORY.Department_PositionID;
                    //Lấy id mới nhất được tạo ra để chèn vào
                    HRM_EMPLOYMENTHISTORY item = db.HRM_EMPLOYMENTHISTORY.OrderByDescending(h => h.EmploymentHistoryID).First();
                    if (hRM_DETAILPLAN.CrewOffID == item.EmployeeID)
                    {
                        hRM_DETAILPLAN.CrewOffHistoryID = item.EmploymentHistoryID;
                    }
                    else hRM_DETAILPLAN.CrewOnHistoryID = item.EmploymentHistoryID;


                    db.Entry(hRM_DETAILPLAN).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch { }

                return RedirectToAction("CreateEachList", "HRM_EMPLOYMENTHISTORY");
                //return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                // return  RedirectToAction("Index", "HRM_EMPLOYEE2");
                //return RedirectToAction("Index");
            }
            //var departmentPositions =
            // db.DIC_DEPARTMENT_POSITION
            //   .ToList()
            //   .Select(s => new
            //   {
            //       Department_PositionID = s.Department_PositionID,
            //       Des = string.Format("{0} --- {1} ----{2}", s.DIC_DEPARTMENT.DepartmentName, s.DIC_POSITION.PositionName, s.Coeff)
            //   });
            //var departmentID = from a in db.DIC_DEPARTMENT
            //                   where a.ParentID == 8 && a.ParentID == 17
            //                   select a;
            //var positionID = from a in db.DIC_POSITION
            //                 where a.GroupPositionID != 4
            //                 select a;
            //ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", hRM_EMPLOYMENTHISTORY.CategoryDecisionID);
            ////ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des");
            //// new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            ////ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName", hRM_EMPLOYMENTHISTORY.SalaryStepID);
            //ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYMENTHISTORY.EmployeeID);
            //ViewBag.DepartmentID = new SelectList(departmentID, "DepartmentID", "DepartmentName");
            //ViewBag.PositionID = new SelectList(positionID, "PositionID", "PositionName");
            //ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName");
            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", 4);
            //    ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des"); //new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            //   ViewBag.Department_PositionID =  new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Coeff");
            var departmentID = from a in db.DIC_DEPARTMENT
                               where a.ParentID == 8 || a.ParentID == 17
                               select a;
            var positionID = from a in db.DIC_POSITION
                             where a.GroupPositionID != 4
                             select a;
            ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName");
            //   ViewBag.EmployeeID = EmployeeID;// new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");

            ViewBag.DepartmentID = new SelectList(departmentID, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentID);
            ViewBag.PositionID = new SelectList(positionID, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName");

            //Phòng ban kiêm nhiệm
            //ViewBag.DepartmentPluralityID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName");
            ViewBag.DepartmentPluralityID = new SelectList(departmentID, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentPluralityID);//new
            //Thực tập chức vụ?
            ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");
            //Thực tập chức vụ kiêm nhiệm
            ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");
            //Chức vụ kiêm nhiệm

            ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PluralityID);//new
            //LẤY THÔNG TIN PHÒNG BAN VÀ CHỨC VỤ ĐỂ LẤY ĐƯỢC THÔNG TIN LƯƠNG
            var rankID = from a in db.DIC_SALARY
                         where a.DepartmentID == hRM_EMPLOYMENTHISTORY.DepartmentID && a.PositionID == hRM_EMPLOYMENTHISTORY.PositionID
                         select a;

            //ViewBag.SalaryPositionID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPositionID = new SelectList(rankID, "SalaryID", "RankID", hRM_EMPLOYMENTHISTORY.SalaryPositionID);
            // ViewBag.SalaryPluralityID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPluralityID = new SelectList(rankID, "SalaryID", "RankID", hRM_EMPLOYMENTHISTORY.SalaryPluralityID);
            return PartialView(hRM_EMPLOYMENTHISTORY);
        }
        //create one
        [WebMethod]
        public async Task<ActionResult> CreateDefault()//Tự động tạo quyết định mới cho tất cả người trong danh sách
        {             
            HRM_EMPLOYMENTHISTORY item = new HRM_EMPLOYMENTHISTORY();
            return PartialView("CreateDefault", item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDefault([Bind(Include = "EmploymentHistoryID,DecisionNo,DecisionDate,EffectiveDate,ContentDecision,CategoryDecisionID,EmployeeID,DepartmentID,PositionID,InternshipPosition,DepartmentPluralityID,PluralityID,IntershipPlurality,Note,PerPosition,PerPlurality,SalaryPositionID,SalaryPluralityID,DepartmentName,LoaiTauID,Gross,Power,NgayXuongTau,XacNhan,LyDoNghiViec_ID")] HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY)

        {
            if (ModelState.IsValid)
            {
                //Nút mới đều tự động lấy giá trị để tạo, ngoại trừ ngày quyết định và ngày hiệu lực chọn từ form
                string[] list1 = (string[])Session["list"];//lấy danh sách các nhân viên đã chọn
                for (int index = (int)Session["index"]; index <= list1.Length - 1; index++)
                {
                    //lặp qua các giá trị
                    //Phân tích dữ liệu từ session
                    char[] delimiterChars = { '|' };
                    string[] list = list1[index].Split(delimiterChars);
                    int EmployeeID = Convert.ToInt32(list[0]);
                    int PlanID = Convert.ToInt32(list[1]);
                    int detailPlanID = Convert.ToInt32(list[2]);

                    //Session["list"] = list1;
                    //Session["index"] = index;

                    //Lấy được các thông tin bắt đầu tạo mới quyết định từ các giá trị đã có


                    //
                    //int index = (int)Session["index"];//Giá trị đầu tiên
                    //if (index > list1.Length - 1)
                    //{
                    //    //Trả lại
                    //    return RedirectToAction("Index", "HRM_QuyetDinhDieuDong", new { planID = (int)Session["idkh"] });
                    //}


                    //Lấy thông tin của người đang chọn để biết những thông tin có sẵn
                    HRM_DETAILPLAN hRM_DETAILPLAN = db.HRM_DETAILPLAN.Where(h => h.PlanID == PlanID && h.DetailPlanID == detailPlanID && (h.CrewOffID == EmployeeID || h.CrewOnID == EmployeeID)).First();
                    //Lấy thông tin hiện tại của nhân viên
                    viewHRM_EMPLOYMENTHISTORY currentInfo = db.viewHRM_EMPLOYMENTHISTORY.Where(e => e.EmployeeID == EmployeeID).OrderByDescending(e => e.EmploymentHistoryID).First();
                    hRM_EMPLOYMENTHISTORY.CategoryDecisionID = 4; //Mặc định là điều động thuyền viên
                    hRM_EMPLOYMENTHISTORY.DepartmentPluralityID = null;//currentInfo.DepartmentPluralityID;//Phòng kiêm nhiệm                    

                    hRM_EMPLOYMENTHISTORY.LoaiTauID = currentInfo.LoaiTauID;
                    hRM_EMPLOYMENTHISTORY.LyDoNghiViec_ID = null; // currentInfo.LyDoNghiViec_ID;
                    hRM_EMPLOYMENTHISTORY.SalaryPluralityID = null;//currentInfo.SalaryPluralityID;
                    hRM_EMPLOYMENTHISTORY.SalaryPositionID = null; // currentInfo.SalaryPositionID;
                    hRM_EMPLOYMENTHISTORY.SalaryPositionID = null; // currentInfo.SalaryPositionID;
                    hRM_EMPLOYMENTHISTORY.SalaryPositionID = null; // currentInfo.SalaryPositionID;

                    hRM_EMPLOYMENTHISTORY.InternshipPosition = currentInfo.InternshipPosition; // chức vụ thực tập
                    hRM_EMPLOYMENTHISTORY.IntershipPlurality = currentInfo.IntershipPlurality; // chức vụ thực tập kiêm nhiệm
                    hRM_EMPLOYMENTHISTORY.PositionID = currentInfo.PositionID; // chức vụ kiêm nhiệm
                    hRM_EMPLOYMENTHISTORY.PluralityID = currentInfo.PluralityID; // chức vụ kiêm nhiệm

                    //Lưu thông tin có sẵn
                    //thuyền viên xuống tàu
                    hRM_EMPLOYMENTHISTORY.EmployeeID = EmployeeID;

                    if (hRM_DETAILPLAN.CrewOffID == EmployeeID)
                    {
                        //Chuyển lên dự trữ
                        hRM_EMPLOYMENTHISTORY.DepartmentID = 18;
                        //hRM_EMPLOYMENTHISTORY.DepartmentPluralityID = 18;
                    }
                    else
                    {
                        //Thuyền viên lên tàu
                        //hRM_EMPLOYMENTHISTORY.PositionID = hRM_DETAILPLAN.OnPositionID;
                        hRM_EMPLOYMENTHISTORY.DepartmentID = hRM_DETAILPLAN.CrewOffDepartmentID;//thay thế chỗ cũ
                        // về thực tập thêm một id null vào
                    }



                    Session["idtv"] = EmployeeID;
                    Session["idkh"] = PlanID;
                    Session["idctkh"] = detailPlanID;

                    if (hRM_EMPLOYMENTHISTORY.DecisionDate == null)
                    {
                        //Thêm ngày hiện tại
                        hRM_EMPLOYMENTHISTORY.DecisionDate = DateTime.Now;
                    }
                    //Lấy số Quyết định tự tăng để hiển thị lên
                    //Tự động tăng id DecisionNo
                    //Lấy max DecisionNo + 1
                    //Lấy giá trị DecisionNo tăng tự động theo năm (18/1)
                    //Kiểm tra ngày lập quyết định để lấy thông tin

                    int DecisionNo = 0;
                    //if (hRM_EMPLOYMENTHISTORY.DecisionDate.Value.Day == 1 && hRM_EMPLOYMENTHISTORY.DecisionDate.Value.Month == 1)
                    //{
                    //    DecisionNo = 0;

                    //}
                    try
                    {
                        var query = db.HRM_EMPLOYMENTHISTORY.Where(x => x.DecisionDate.Value.Year == hRM_EMPLOYMENTHISTORY.DecisionDate.Value.Year).OrderByDescending(x => x.DecisionNo).First();
                        DecisionNo = (int)query.DecisionNo;

                    }
                    catch (Exception)
                    {

                        if (hRM_EMPLOYMENTHISTORY.DecisionNo == null)
                        {
                            hRM_EMPLOYMENTHISTORY.DecisionNo = DecisionNo + 1;
                        }

                    }

                    hRM_EMPLOYMENTHISTORY.DecisionNo = DecisionNo + 1;


                    //Lấy thông tin ngày quyết định và ngày hiệu lực

                    db.HRM_EMPLOYMENTHISTORY.Add(hRM_EMPLOYMENTHISTORY);
                    db.SaveChanges();
                    //Lưu thành công thì cập nhật thông tin vào bảng detailplan
                    try
                    {
                        int idtv = Convert.ToInt32(Session["idtv"]);
                        int idkh = Convert.ToInt32(Session["idkh"]);
                        int idctkh = Convert.ToInt32(Session["idctkh"]);
                        // HRM_DETAILPLAN hRM_DETAILPLAN = db.HRM_DETAILPLAN.Where(h => (h.PlanID == ViewBag.idKeHoach) && (h.DetailPlanID == ViewBag.idChitietKeHoach) && (h.CrewOffID == ViewBag.idThuyenVien) || (h.CrewOnID == ViewBag.idThuyenVien));
                        hRM_DETAILPLAN = db.HRM_DETAILPLAN.Where(h => (h.CrewOffID == idtv || h.CrewOnID == idtv) && h.DetailPlanID == idctkh && h.PlanID == idkh).First();
                        //hRM_EMPLOYEE.Department_PositionID = hRM_EMPLOYMENTHISTORY.Department_PositionID;
                        //Lấy id mới nhất được tạo ra để chèn vào
                        HRM_EMPLOYMENTHISTORY item = db.HRM_EMPLOYMENTHISTORY.OrderByDescending(h => h.EmploymentHistoryID).First();
                        if (hRM_DETAILPLAN.CrewOffID == item.EmployeeID)
                        {
                            hRM_DETAILPLAN.CrewOffHistoryID = item.EmploymentHistoryID;
                        }
                        else hRM_DETAILPLAN.CrewOnHistoryID = item.EmploymentHistoryID;


                        db.Entry(hRM_DETAILPLAN).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch { }

                }
                if (Session["idTau"] != null)
                {
                    return RedirectToAction("Index", "HRM_QuyetDinhDieuDong", new { planID = (int)Session["idkh"], TauID = (int)Session["idTau"] });
                }
                else
                {
                    return RedirectToAction("Index", "HRM_QuyetDinhDieuDong", new { planID = (int)Session["idkh"] });
                }
            }
            //var departmentPositions =
            // db.DIC_DEPARTMENT_POSITION
            //   .ToList()
            //   .Select(s => new
            //   {
            //       Department_PositionID = s.Department_PositionID,
            //       Des = string.Format("{0} --- {1} ----{2}", s.DIC_DEPARTMENT.DepartmentName, s.DIC_POSITION.PositionName, s.Coeff)
            //   });
            //var departmentID = from a in db.DIC_DEPARTMENT
            //                   where a.ParentID == 8 && a.ParentID == 17
            //                   select a;
            //var positionID = from a in db.DIC_POSITION
            //                 where a.GroupPositionID != 4
            //                 select a;
            //ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", hRM_EMPLOYMENTHISTORY.CategoryDecisionID);
            ////ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des");
            //// new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            ////ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName", hRM_EMPLOYMENTHISTORY.SalaryStepID);
            //ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYMENTHISTORY.EmployeeID);
            //ViewBag.DepartmentID = new SelectList(departmentID, "DepartmentID", "DepartmentName");
            //ViewBag.PositionID = new SelectList(positionID, "PositionID", "PositionName");
            //ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName");
            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", 4);
            //    ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des"); //new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            //   ViewBag.Department_PositionID =  new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Coeff");
            var departmentID = from a in db.DIC_DEPARTMENT
                               where a.ParentID == 8 || a.ParentID == 17
                               select a;
            var positionID = from a in db.DIC_POSITION
                             where a.GroupPositionID != 4
                             select a;
            ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName");
            //   ViewBag.EmployeeID = EmployeeID;// new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");

            ViewBag.DepartmentID = new SelectList(departmentID, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentID);
            ViewBag.PositionID = new SelectList(positionID, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName");

            //Phòng ban kiêm nhiệm
            //ViewBag.DepartmentPluralityID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName");
            ViewBag.DepartmentPluralityID = new SelectList(departmentID, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentPluralityID);//new
            //Thực tập chức vụ?
            ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");
            //Thực tập chức vụ kiêm nhiệm
            ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");
            //Chức vụ kiêm nhiệm

            ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PluralityID);//new
            //LẤY THÔNG TIN PHÒNG BAN VÀ CHỨC VỤ ĐỂ LẤY ĐƯỢC THÔNG TIN LƯƠNG
            var rankID = from a in db.DIC_SALARY
                         where a.DepartmentID == hRM_EMPLOYMENTHISTORY.DepartmentID && a.PositionID == hRM_EMPLOYMENTHISTORY.PositionID
                         select a;

            //ViewBag.SalaryPositionID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPositionID = new SelectList(rankID, "SalaryID", "RankID", hRM_EMPLOYMENTHISTORY.SalaryPositionID);
            // ViewBag.SalaryPluralityID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPluralityID = new SelectList(rankID, "SalaryID", "RankID", hRM_EMPLOYMENTHISTORY.SalaryPluralityID);
            return PartialView(hRM_EMPLOYMENTHISTORY);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOne([Bind(Include = "EmploymentHistoryID,DecisionNo,DecisionDate,EffectiveDate,ContentDecision,CategoryDecisionID,SalaryStepID,EmployeeID,DepartmentID,PositionID,InternshipPosition,PluralityID,IntershipPlurality,Coefficient,Salary,CoefficientIns,SalaryIns,BHXH,BHYT,BHTN")] HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY)

        {
            if (ModelState.IsValid)
            {
                db.HRM_EMPLOYMENTHISTORY.Add(hRM_EMPLOYMENTHISTORY);
                db.SaveChanges();
                //Lưu thành công thì cập nhật thông tin vào bảng detailplan
                try
                {
                    int idtv = Convert.ToInt32(Session["idtv"]);
                    int idkh = Convert.ToInt32(Session["idkh"]);
                    int idctkh = Convert.ToInt32(Session["idctkh"]);
                    // HRM_DETAILPLAN hRM_DETAILPLAN = db.HRM_DETAILPLAN.Where(h => (h.PlanID == ViewBag.idKeHoach) && (h.DetailPlanID == ViewBag.idChitietKeHoach) && (h.CrewOffID == ViewBag.idThuyenVien) || (h.CrewOnID == ViewBag.idThuyenVien));
                    HRM_DETAILPLAN hRM_DETAILPLAN = db.HRM_DETAILPLAN.Where(h => (h.CrewOffID == idtv || h.CrewOnID == idtv) && h.DetailPlanID == idctkh && h.PlanID == idkh).First();
                    //hRM_EMPLOYEE.Department_PositionID = hRM_EMPLOYMENTHISTORY.Department_PositionID;
                    //Lấy id mới nhất được tạo ra để chèn vào
                    HRM_EMPLOYMENTHISTORY item = db.HRM_EMPLOYMENTHISTORY.OrderByDescending(h => h.EmploymentHistoryID).First();
                    if (hRM_DETAILPLAN.CrewOffID == item.EmployeeID)
                    {
                        hRM_DETAILPLAN.CrewOffHistoryID = item.EmploymentHistoryID;
                    }
                    else hRM_DETAILPLAN.CrewOnHistoryID = item.EmploymentHistoryID;
                    db.Entry(hRM_DETAILPLAN).State = EntityState.Modified;
                    db.SaveChanges();
            
                }
                catch { }

                //   return RedirectToAction("EmploymentHistoryofEmloyee", new { EmployeeID = hRM_EMPLOYMENTHISTORY.EmployeeID });
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                // return  RedirectToAction("Index", "HRM_EMPLOYEE2");
                //return RedirectToAction("Index");
            }
            //var departmentPositions =
            // db.DIC_DEPARTMENT_POSITION
            //   .ToList()
            //   .Select(s => new
            //   {
            //       Department_PositionID = s.Department_PositionID,
            //       Des = string.Format("{0} --- {1} ----{2}", s.DIC_DEPARTMENT.DepartmentName, s.DIC_POSITION.PositionName, s.Coeff)
            //   });
            var departmentID = from a in db.DIC_DEPARTMENT
                               where a.ParentID == 8 && a.ParentID == 17
                               select a;
            var positionID = from a in db.DIC_POSITION
                             where a.GroupPositionID != 4
                             select a;
            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", hRM_EMPLOYMENTHISTORY.CategoryDecisionID);
            //ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des");
            // new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            //ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName", hRM_EMPLOYMENTHISTORY.SalaryStepID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYMENTHISTORY.EmployeeID);
            ViewBag.DepartmentID = new SelectList(departmentID, "DepartmentID", "DepartmentName");
            ViewBag.PositionID = new SelectList(positionID, "PositionID", "PositionName");
            ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName");
            return PartialView(hRM_EMPLOYMENTHISTORY);
        }
        // POST: HRM_EMPLOYMENTHISTORY/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //HAM LAY THONG TIN TIEN LUONG THAM KHAO
        [WebMethod]
        public JsonResult GetCoef(int SalaryStepID)
        {
            ArrayList arl = new ArrayList();


            var ret = db.DIC_SALARY_STEP.Where(a => a.SalaryStepID == SalaryStepID);

            foreach (DIC_SALARY_STEP rg in ret)
            {
                arl.Add(new { Coefficient = rg.Coefficient.ToString(), Salary = rg.Salary, rg.CoefficientIns, rg.SalaryIns });
            }

            return new JsonResult { Data = arl };
        }
        //HAM LAY THONG TIN TIEN LUONG THAM KHAO
        [WebMethod]
        public JsonResult GetRank(int? DepartmentID, int PositionID)
        {
            if (DepartmentID.HasValue)
            {
                var rank = db.DIC_SALARY.Where(x => x.DepartmentID == DepartmentID && x.PositionID == PositionID)
                            .Select(a => new
                            {
                                SalaryID = a.SalaryID,
                                RankID = a.RankID
                            });
                return Json(rank);
            }
            else return Json(null);
            
        }
        //HAM LAY THONG TIN TIEN LUONG THAM KHAO
        [WebMethod]
        public JsonResult GetChucVu(int? DepartmentID)
        {
            if (DepartmentID.HasValue)
            {
                var chucvu = from a in db.DIC_DEPARTMENT_POSITION
                         join b in db.DIC_POSITION
                         on a.PositionID equals b.PositionID
                         where a.DepartmentID == DepartmentID.Value
                         select new { PositionID = b.PositionID, PositionName = b.PositionName };
                return Json(chucvu);

            }
            else
            {
                var chucvu2 = from a in db.DIC_POSITION
                         select new { a.PositionID, a.PositionName };
                return Json(chucvu2);
            }  
            
          
            
        }
        [WebMethod]
        public JsonResult GetSoQD(int? CategoryDecisionID, DateTime? DecisionDate)
        {
            //if (CategoryDecisionID.HasValue)
            //{
                //int DecisionNo = 0;
                //try
                //{
                //    //kiểm tra danh sách để lấy theo năm hiện tại trước
                //    var query = db.HRM_EMPLOYMENTHISTORY.Where(x => x.DecisionDate.Value.Year == hRM_EMPLOYMENTHISTORY.DecisionDate.Value.Year && x.CategoryDecisionID == hRM_EMPLOYMENTHISTORY.CategoryDecisionID).OrderByDescending(x => x.DecisionNo).First();
                //    DecisionNo = (int)query.DecisionNo;

                //}
                //catch (Exception)
                //{
                //}
                //hRM_EMPLOYMENTHISTORY.DecisionNo = DecisionNo + 1;
                //var query = db.HRM_EMPLOYMENTHISTORY.Where(x => x.DecisionDate.Value.Year == hRM_EMPLOYMENTHISTORY.DecisionDate.Value.Year && x.CategoryDecisionID == hRM_EMPLOYMENTHISTORY.CategoryDecisionID).OrderByDescending(x => x.DecisionNo).First();
                //DecisionNo = (int)query.DecisionNo;
                var soqd = from a in db.HRM_EMPLOYMENTHISTORY 
                             
                             where a.DecisionDate.Value.Year == DecisionDate.Value.Year && a.CategoryDecisionID == CategoryDecisionID
                             orderby a.DecisionNo descending
                             select  new { DecisionNo = (a.DecisionNo + 1) };
                return Json(soqd);

            //}
            //else
            //{
            //    var chucvu2 = from a in db.DIC_POSITION
            //                  select new { a.PositionID, a.PositionName };
            //    return Json(chucvu2);
            //}



        }
        [Authorize(Roles = "HR, EduCenter")]
        //[Authorize(Roles = "Create")]
        [HttpPost, ActionName("CreateWithEmployee")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWithEmployee(int EmployeeID, [Bind(Include = "EmploymentHistoryID,DecisionNo,DecisionDate,EffectiveDate,ContentDecision,CategoryDecisionID,EmployeeID,DepartmentID,PositionID,InternshipPosition,DepartmentPluralityID,PluralityID,IntershipPlurality,Note,PerPosition,PerPlurality,SalaryPositionID,SalaryPluralityID,DepartmentName,LoaiTauID,Gross,Power,NgayXuongTau,XacNhan,LyDoNghiViec_ID")] HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY)
        {
            if (ModelState.IsValid)
            {
                db.HRM_EMPLOYMENTHISTORY.Add(hRM_EMPLOYMENTHISTORY);
                db.SaveChangesAsync();
                //Setup Status ID của Employee là 5(nghỉ việc) nếu loại quyết định là 3(thanh lý hợp đồng)
                if (hRM_EMPLOYMENTHISTORY.CategoryDecisionID==3)
                {
                    var nhanvien = db.HRM_EMPLOYEE.Where(e => e.EmployeeID == hRM_EMPLOYMENTHISTORY.EmployeeID).First();
                    nhanvien.StatusID = 5;
                    db.Entry(nhanvien).State = EntityState.Modified;
                    db.SaveChanges();
                }
      
                return RedirectToAction("EmploymentHistoryofEmloyee", "hRM_EMPLOYMENTHISTORY", new { EmployeeID = EmployeeID });
                //   return RedirectToAction("EmploymentHistoryofEmloyee",new { EmployeeID = EmployeeID });
            }
            //           var departmentPositions =
            //db.DIC_DEPARTMENT_POSITION
            //  .ToList()
            //  .Select(s => new
            //  {
            //      Department_PositionID = s.Department_PositionID,
            //      Des = string.Format("{0} --- {1} ----{2}", s.DIC_DEPARTMENT.DepartmentName, s.DIC_POSITION.PositionName, s.Coeff)
            //  });

            //ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", hRM_EMPLOYMENTHISTORY.CategoryDecisionID);
            ////      ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des");// new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            ////   ViewBag.Department_PositionID = new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Coeff");

            ////ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName", hRM_EMPLOYMENTHISTORY.SalaryStepID);
            //ViewBag.EmployeeID = EmployeeID;// new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYMENTHISTORY.EmployeeID);
            //ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentID);
            //ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            //Loại tàu
            ViewBag.LoaiTauID = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau");
            //Loại quyết định
            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName",4);
            //    ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des"); //new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            //   ViewBag.Department_PositionID =  new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Coeff");
            var departmentID = from a in db.DIC_DEPARTMENT
                               where (a.ParentID == 8 || a.ParentID == 17) && a.IsLast == true
                               select a;
            var positionID = from a in db.DIC_POSITION
                                 //where a.GroupPositionID != 4
                             select a;
            ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName");
            //   ViewBag.EmployeeID = EmployeeID;// new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");

            ViewBag.DepartmentID = new SelectList(departmentID, "DepartmentID", "DepartmentName");
            ViewBag.PositionID = new SelectList(positionID, "PositionID", "PositionName");
            ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName");

            //Phòng ban kiêm nhiệm
            //ViewBag.DepartmentPluralityID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName");
            ViewBag.DepartmentPluralityID = new SelectList(departmentID, "DepartmentID", "DepartmentName");//new
            //Thực tập chức vụ?
            ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");
            //Thực tập chức vụ kiêm nhiệm
            ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");
            //Chức vụ kiêm nhiệm

            //ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PluralityID);//new
            //LẤY THÔNG TIN PHÒNG BAN VÀ CHỨC VỤ ĐỂ LẤY ĐƯỢC THÔNG TIN LƯƠNG
            var rankID = from a in db.DIC_SALARY
                             //where a.DepartmentID == hRM_EMPLOYMENTHISTORY.DepartmentID && a.PositionID == hRM_EMPLOYMENTHISTORY.PositionID
                         select a;

            //ViewBag.SalaryPositionID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPositionID = new SelectList(rankID, "SalaryID", "RankID");
            // ViewBag.SalaryPluralityID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPluralityID = new SelectList(rankID, "SalaryID", "RankID");
            var hrm_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Where(h => h.EmployeeID == EmployeeID);
            return PartialView("CreateWithEmployee", hrm_EMPLOYMENTHISTORY);
        }

        // GET: HRM_EMPLOYMENTHISTORY/Edit/5
        [Authorize(Roles = "HR, EduCenter")]
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
 
            ViewBag.LoaiTauID = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau", hRM_EMPLOYMENTHISTORY.LoaiTauID);
            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", hRM_EMPLOYMENTHISTORY.CategoryDecisionID);
            ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name",hRM_EMPLOYMENTHISTORY.LyDoNghiViec_ID);
            //    ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des"); //new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            //   ViewBag.Department_PositionID =  new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Coeff");
            var departmentID = from a in db.DIC_DEPARTMENT
                               where  a.IsLast == true
                               orderby a.ParentID,a.DepartmentID
                               select a;
            var positionID = from a in db.DIC_POSITION
                             //where a.GroupPositionID != 4
                             select a;
            ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName");
            //   ViewBag.EmployeeID = EmployeeID;// new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");

            ViewBag.DepartmentID = new SelectList(departmentID, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentID);
            ViewBag.PositionID = new SelectList(positionID, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PluralityID);

            //Phòng ban kiêm nhiệm
            //ViewBag.DepartmentPluralityID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName");
            ViewBag.DepartmentPluralityID = new SelectList(departmentID, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentPluralityID);//new
            //Thực tập chức vụ?
            ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName",hRM_EMPLOYMENTHISTORY.InternshipPosition);
            //Thực tập chức vụ kiêm nhiệm
            ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName", hRM_EMPLOYMENTHISTORY.IntershipPlurality);
            //Chức vụ kiêm nhiệm

            //ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PluralityID);//new
            //LẤY THÔNG TIN PHÒNG BAN VÀ CHỨC VỤ ĐỂ LẤY ĐƯỢC THÔNG TIN LƯƠNG
            var rankID = from a in db.DIC_SALARY
                         where a.DepartmentID == hRM_EMPLOYMENTHISTORY.DepartmentID && a.PositionID == hRM_EMPLOYMENTHISTORY.PositionID
                         select a;

            //ViewBag.SalaryPositionID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPositionID = new SelectList(rankID, "SalaryID", "RankID", hRM_EMPLOYMENTHISTORY.SalaryPositionID);
            // ViewBag.SalaryPluralityID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPluralityID = new SelectList(rankID, "SalaryID", "RankID", hRM_EMPLOYMENTHISTORY.SalaryPluralityID);
            return PartialView(hRM_EMPLOYMENTHISTORY);
        }
        // GET: HRM_EMPLOYMENTHISTORY/Edit/5
        [Authorize(Roles = "HR, EduCenter")]
        [Authorize(Roles = "Create")]
        public ActionResult EditDefault(int? id, int planID)
        {
            Session["planID"] = planID;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Find(id);
            if (hRM_EMPLOYMENTHISTORY == null)
            {
                return HttpNotFound();
            }
            //            var departmentPositions =
            //db.DIC_DEPARTMENT_POSITION
            // .ToList()
            // .Select(s => new
            // {
            //     Department_PositionID = s.Department_PositionID,
            //     Des = string.Format("{0} --- {1} ----{2}", s.DIC_DEPARTMENT.DepartmentName, s.DIC_POSITION.PositionName, s.Coeff)
            // });

            //ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", hRM_EMPLOYMENTHISTORY.CategoryDecisionID);
            //// ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des",hRM_EMPLOYMENTHISTORY.Department_PositionID);// new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            ////   ViewBag.Department_PositionID = new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Coeff");

            ////ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName", hRM_EMPLOYMENTHISTORY.SalaryStepID);
            //ViewBag.EmployeeID = hRM_EMPLOYMENTHISTORY.EmployeeID;// new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYMENTHISTORY.EmployeeID);

            //ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentID);
            //ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            ////  var hrm_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Where(h => h.EmployeeID == EmployeeID);
            ViewBag.LoaiTauID = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau");
            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", hRM_EMPLOYMENTHISTORY.CategoryDecisionID);
            ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name", hRM_EMPLOYMENTHISTORY.LyDoNghiViec_ID);
            //    ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des"); //new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            //   ViewBag.Department_PositionID =  new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Coeff");
            var departmentID = from a in db.DIC_DEPARTMENT
                               where a.DepartmentID != 17 && a.DepartmentID!=11 && a.DepartmentID!=24 && a.ParentID!=1     //bỏ các phòng ban dự trữ, aulac angel, aulac01
                               orderby a.ParentID, a.DepartmentID
                               select a;
            var positionID = from a in db.DIC_POSITION
                                 //where a.GroupPositionID != 4
                             select a;
            ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName");
            //   ViewBag.EmployeeID = EmployeeID;// new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");

            ViewBag.DepartmentID = new SelectList(departmentID, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentID);
            ViewBag.PositionID = new SelectList(positionID, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PluralityID);

            //Phòng ban kiêm nhiệm
            //ViewBag.DepartmentPluralityID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName");
            ViewBag.DepartmentPluralityID = new SelectList(departmentID, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentPluralityID);//new
            //Thực tập chức vụ?
            ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName", hRM_EMPLOYMENTHISTORY.InternshipPosition);
            //Thực tập chức vụ kiêm nhiệm
            ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName", hRM_EMPLOYMENTHISTORY.IntershipPlurality);
            //Chức vụ kiêm nhiệm

            //ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PluralityID);//new
            //LẤY THÔNG TIN PHÒNG BAN VÀ CHỨC VỤ ĐỂ LẤY ĐƯỢC THÔNG TIN LƯƠNG
            var rankID = from a in db.DIC_SALARY
                         where a.DepartmentID == hRM_EMPLOYMENTHISTORY.DepartmentID && a.PositionID == hRM_EMPLOYMENTHISTORY.PositionID
                         select a;

            //ViewBag.SalaryPositionID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPositionID = new SelectList(rankID, "SalaryID", "RankID", hRM_EMPLOYMENTHISTORY.SalaryPositionID);
            // ViewBag.SalaryPluralityID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPluralityID = new SelectList(rankID, "SalaryID", "RankID", hRM_EMPLOYMENTHISTORY.SalaryPluralityID);

            hRM_EMPLOYMENTHISTORY.PerPosition = 100;
            hRM_EMPLOYMENTHISTORY.PerPlurality = 0;
            return PartialView(hRM_EMPLOYMENTHISTORY);
        }
        // POST: HRM_EMPLOYMENTHISTORY/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDefault([Bind(Include = "EmploymentHistoryID,DecisionNo,DecisionDate,EffectiveDate,ContentDecision,CategoryDecisionID,EmployeeID,DepartmentID,PositionID,InternshipPosition,DepartmentPluralityID,PluralityID,IntershipPlurality,Note,PerPosition,PerPlurality,SalaryPositionID,SalaryPluralityID,DepartmentName,LoaiTauID,Gross,Power,NgayXuongTau,XacNhan,LyDoNghiViec_ID, Salary, AllowanceSalary, Bonus, AllowanceBonus, SalaryPlurality, AllowanceSalaryPlurality, BonusPlurality, AllowanceBonusPlurality")] HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRM_EMPLOYMENTHISTORY).State = EntityState.Modified;
                db.SaveChanges();
                //Kiểm tra nếu là dòng dữ liệu cuối và có sửa chữa thì mới cập nhật
                //Tìm id của dữ liệu dòng cuối
                int getLastID = int.Parse(db.HRM_EMPLOYMENTHISTORY
                            .OrderByDescending(p => p.EmploymentHistoryID)
                            .Select(r => r.EmploymentHistoryID)
                            .First().ToString());
                if (getLastID == hRM_EMPLOYMENTHISTORY.EmploymentHistoryID)//dòng cuối
                {
                    //Lưu thành công thì cập nhật cho employeeID
                    try
                    {
                        HRM_EMPLOYEE hRM_EMPLOYEE = db.HRM_EMPLOYEE.Find(hRM_EMPLOYMENTHISTORY.EmployeeID);
                        //hRM_EMPLOYEE.Department_PositionID = hRM_EMPLOYMENTHISTORY.Department_PositionID;
                        db.Entry(hRM_EMPLOYEE).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch { }
                }
                int planID = (int)Session["planID"];
                //return RedirectToAction("Index", "HRM_QuyetDinhDieuDong", new { planID = planID });
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("EmploymentHistoryofEmloyee", new { EmployeeID = hRM_EMPLOYMENTHISTORY.EmployeeID });
                //return RedirectToAction("EmploymentHistoryofEmloyee", new { EmployeeID = hRM_EMPLOYMENTHISTORY.EmployeeID });
                //return RedirectToAction("Index");
            }

           
            //          var departmentPositions =
            //db.DIC_DEPARTMENT_POSITION
            //  .ToList()
            //  .Select(s => new
            //  {
            //      Department_PositionID = s.Department_PositionID,
            //      Des = string.Format("{0} --- {1} ----{2}", s.DIC_DEPARTMENT.DepartmentName, s.DIC_POSITION.PositionName, s.Coeff)
            //  });

            //ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", hRM_EMPLOYMENTHISTORY.CategoryDecisionID);
            //// ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des",hRM_EMPLOYMENTHISTORY.Department_PositionID);// new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            ////   ViewBag.Department_PositionID = new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Coeff");

            ////ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName", hRM_EMPLOYMENTHISTORY.SalaryStepID);
            //ViewBag.EmployeeID = hRM_EMPLOYMENTHISTORY.EmployeeID;// new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYMENTHISTORY.EmployeeID);
            ViewBag.LoaiTauID = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau");
            ////var hrm_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Where(h => h.EmployeeID == EmployeeID);
            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", hRM_EMPLOYMENTHISTORY.CategoryDecisionID);
            ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name", hRM_EMPLOYMENTHISTORY.LyDoNghiViec_ID);
            //    ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des"); //new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            //   ViewBag.Department_PositionID =  new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Coeff");
            var departmentID = from a in db.DIC_DEPARTMENT
                               where (a.ParentID == 8 || a.ParentID == 17) && a.IsLast == true
                               select a;
            var positionID = from a in db.DIC_POSITION
                                 //where a.GroupPositionID != 4
                             select a;
            ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName");
            //   ViewBag.EmployeeID = EmployeeID;// new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");

            ViewBag.DepartmentID = new SelectList(departmentID, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentID);
            ViewBag.PositionID = new SelectList(positionID, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PluralityID);

            //Phòng ban kiêm nhiệm
            //ViewBag.DepartmentPluralityID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName");
            ViewBag.DepartmentPluralityID = new SelectList(departmentID, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentPluralityID);//new
            //Thực tập chức vụ?
            ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName", hRM_EMPLOYMENTHISTORY.InternshipPosition);
            //Thực tập chức vụ kiêm nhiệm
            ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName", hRM_EMPLOYMENTHISTORY.IntershipPlurality);
            //Chức vụ kiêm nhiệm

            //ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PluralityID);//new
            //LẤY THÔNG TIN PHÒNG BAN VÀ CHỨC VỤ ĐỂ LẤY ĐƯỢC THÔNG TIN LƯƠNG
            var rankID = from a in db.DIC_SALARY
                         where a.DepartmentID == hRM_EMPLOYMENTHISTORY.DepartmentID && a.PositionID == hRM_EMPLOYMENTHISTORY.PositionID
                         select a;

            //ViewBag.SalaryPositionID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPositionID = new SelectList(rankID, "SalaryID", "RankID", hRM_EMPLOYMENTHISTORY.SalaryPositionID);
            // ViewBag.SalaryPluralityID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPluralityID = new SelectList(rankID, "SalaryID", "RankID", hRM_EMPLOYMENTHISTORY.SalaryPluralityID);
            return PartialView(hRM_EMPLOYMENTHISTORY);
        }
        // GET: HRM_EMPLOYMENTHISTORY/Edit/5
        [Authorize(Roles = "HR, EduCenter")]
        [Authorize(Roles = "Create")]
        public ActionResult EditXacNhan(int? id)
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
            //            var departmentPositions =
            //db.DIC_DEPARTMENT_POSITION
            // .ToList()
            // .Select(s => new
            // {
            //     Department_PositionID = s.Department_PositionID,
            //     Des = string.Format("{0} --- {1} ----{2}", s.DIC_DEPARTMENT.DepartmentName, s.DIC_POSITION.PositionName, s.Coeff)
            // });

            //ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", hRM_EMPLOYMENTHISTORY.CategoryDecisionID);
            // ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des",hRM_EMPLOYMENTHISTORY.Department_PositionID);// new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            //   ViewBag.Department_PositionID = new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Coeff");

            //ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName", hRM_EMPLOYMENTHISTORY.SalaryStepID);
            //ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYMENTHISTORY.EmployeeID);
            ViewData["EmployeeID"] =
  new SelectList((from s in db.HRM_EMPLOYEE.ToList()
                  select new
                  {
                      EmployeeID = s.EmployeeID,
                      FullName = s.FirstName + " " + s.LastName
                  }),
      "EmployeeID",
      "FullName",
      hRM_EMPLOYMENTHISTORY.EmployeeID);
            //ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentID);
            //ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            //  var hrm_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Where(h => h.EmployeeID == EmployeeID);
            return PartialView(hRM_EMPLOYMENTHISTORY);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR, EduCenter")]
        [Authorize(Roles = "Create")]
        public ActionResult EditXacNhan([Bind(Include = "EmploymentHistoryID,DecisionNo,DecisionDate,EffectiveDate,ContentDecision,CategoryDecisionID,EmployeeID,DepartmentID,PositionID,InternshipPosition,DepartmentPluralityID,PluralityID,IntershipPlurality,Note,PerPosition,PerPlurality,SalaryPositionID,SalaryPluralityID,DepartmentName,LoaiTauID,Gross,Power,NgayXuongTau,XacNhan,LyDoNghiViec_ID")] HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY)
        {
            if (ModelState.IsValid)
            {

                db.Entry(hRM_EMPLOYMENTHISTORY).State = EntityState.Modified;
              
                db.SaveChanges();
                //Kiểm tra nếu là dòng dữ liệu cuối và có sửa chữa thì mới cập nhật
                //Tìm id của dữ liệu dòng cuối
                int getLastID = int.Parse(db.HRM_EMPLOYMENTHISTORY
                            .OrderByDescending(p => p.EmploymentHistoryID)
                            .Select(r => r.EmploymentHistoryID)
                            .First().ToString());
                if (getLastID == hRM_EMPLOYMENTHISTORY.EmploymentHistoryID)//dòng cuối
                {
                    //Lưu thành công thì cập nhật cho employeeID
                    try
                    {
                        HRM_EMPLOYEE hRM_EMPLOYEE = db.HRM_EMPLOYEE.Find(hRM_EMPLOYMENTHISTORY.EmployeeID);
                        //hRM_EMPLOYEE.Department_PositionID = hRM_EMPLOYMENTHISTORY.Department_PositionID;
                        hRM_EMPLOYEE.SSDD = false;
                        hRM_EMPLOYEE.NgaySSDD = null;
                        db.Entry(hRM_EMPLOYEE).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch { }
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("EmploymentHistoryofEmloyee", new { EmployeeID = hRM_EMPLOYMENTHISTORY.EmployeeID });
                //return RedirectToAction("Index");
            }
            //          var departmentPositions =
            //db.DIC_DEPARTMENT_POSITION
            //  .ToList()
            //  .Select(s => new
            //  {
            //      Department_PositionID = s.Department_PositionID,
            //      Des = string.Format("{0} --- {1} ----{2}", s.DIC_DEPARTMENT.DepartmentName, s.DIC_POSITION.PositionName, s.Coeff)
            //  });

            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", hRM_EMPLOYMENTHISTORY.CategoryDecisionID);
            // ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des",hRM_EMPLOYMENTHISTORY.Department_PositionID);// new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            //   ViewBag.Department_PositionID = new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Coeff");

            //ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName", hRM_EMPLOYMENTHISTORY.SalaryStepID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYMENTHISTORY.EmployeeID);

            //var hrm_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Where(h => h.EmployeeID == EmployeeID);
            return PartialView(hRM_EMPLOYMENTHISTORY);
        }

        // GET: HRM_EMPLOYMENTHISTORY/Edit/5
        public ActionResult EditXacNhanList()
        {
            HRM_EMPLOYMENTHISTORY item = new HRM_EMPLOYMENTHISTORY();
            return PartialView("EditXacNhanList", item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditXacNhanList([Bind(Include = "EmploymentHistoryID,DecisionNo,DecisionDate,EffectiveDate,ContentDecision,CategoryDecisionID,EmployeeID,DepartmentID,PositionID,InternshipPosition,DepartmentPluralityID,PluralityID,IntershipPlurality,Note,PerPosition,PerPlurality,SalaryPositionID,SalaryPluralityID,DepartmentName,LoaiTauID,Gross,Power,NgayXuongTau,XacNhan,LyDoNghiViec_ID")] HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY)
        {
            if (Session["listHistory"] == null)
            {
                return RedirectToAction("IndexXacNhan", "HRM_PLAN");
            }
            int planID = 0;
            if (ModelState.IsValid)
            {
                //Nút mới đều tự động lấy giá trị để tạo, ngoại trừ ngày quyết định và ngày hiệu lực chọn từ form
                string[] list1 = (string[])Session["listHistory"];//lấy danh sách các nhân viên đã chọn
                for (int index = (int)Session["index"]; index <= list1.Length - 1; index++)
                {
                    //lặp qua các giá trị
                    //Phân tích dữ liệu từ session
                    //lặp qua các giá trị
                    //Phân tích dữ liệu từ session
                    char[] delimiterChars = { '|' };
                    string[] list = list1[index].Split(delimiterChars);
                     
                    int HistoryID = Convert.ToInt32(list[0]); //Lấy giá trị và tìm ra giá trị hiện tại để sửa
                    planID = Convert.ToInt32(list[1]);
                    HRM_EMPLOYMENTHISTORY quyetdinh = db.HRM_EMPLOYMENTHISTORY.Find(HistoryID);
                    if (hRM_EMPLOYMENTHISTORY == null)
                    {
                        return HttpNotFound();
                    }
                    //Lấy được giá trị rồi thì chèn 2 giá trị mới vào
                    quyetdinh.NgayXuongTau = hRM_EMPLOYMENTHISTORY.NgayXuongTau;
                    quyetdinh.XacNhan = true;
                    db.Entry(quyetdinh).State = EntityState.Modified;
                    db.SaveChanges();
                    //Lưu cả phần SSĐ
                    //Lưu thành công thì cập nhật cho employeeID
                    try
                    {
                        HRM_EMPLOYEE hRM_EMPLOYEE = db.HRM_EMPLOYEE.Find(quyetdinh.EmployeeID);
                        //hRM_EMPLOYEE.Department_PositionID = hRM_EMPLOYMENTHISTORY.Department_PositionID;
                        //hRM_EMPLOYEE.SSDD = false;
                        //hRM_EMPLOYEE.NgaySSDD = null;
                        db.Entry(hRM_EMPLOYEE).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch { }
                }
                return RedirectToAction("XacnhanQD", "HRM_QuyetDinhDieuDong", new { planID = planID, TauID = (int)Session["idTau_xacnhan"] });
            }
                 
            return PartialView(hRM_EMPLOYMENTHISTORY);
        }
        // POST: HRM_EMPLOYMENTHISTORY/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR, EduCenter")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit([Bind(Include = "EmploymentHistoryID,DecisionNo,DecisionDate,EffectiveDate,ContentDecision,CategoryDecisionID,EmployeeID,DepartmentID,PositionID,InternshipPosition,DepartmentPluralityID,PluralityID,IntershipPlurality,Note,PerPosition,PerPlurality,SalaryPositionID,SalaryPluralityID,DepartmentName,LoaiTauID,Gross,Power,NgayXuongTau,XacNhan,LyDoNghiViec_ID,  Salary, AllowanceSalary, Bonus, AllowanceBonus, SalaryPlurality, AllowanceSalaryPlurality, BonusPlurality, AllowanceBonusPlurality")] HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRM_EMPLOYMENTHISTORY).State = EntityState.Modified;
                db.SaveChanges();
                //Kiểm tra nếu là dòng dữ liệu cuối và có sửa chữa thì mới cập nhật
                //Tìm id của dữ liệu dòng cuối
                int getLastID = int.Parse(db.HRM_EMPLOYMENTHISTORY
                            .OrderByDescending(p => p.EmploymentHistoryID)
                            .Select(r => r.EmploymentHistoryID)
                            .First().ToString());
                if (getLastID == hRM_EMPLOYMENTHISTORY.EmploymentHistoryID)//dòng cuối
                {
                    //Lưu thành công thì cập nhật cho employeeID
                    try
                    {
                        HRM_EMPLOYEE hRM_EMPLOYEE = db.HRM_EMPLOYEE.Find(hRM_EMPLOYMENTHISTORY.EmployeeID);
                        //hRM_EMPLOYEE.Department_PositionID = hRM_EMPLOYMENTHISTORY.Department_PositionID;
                        db.Entry(hRM_EMPLOYEE).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch { }
                }
                //return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                return RedirectToAction("EmploymentHistoryofEmloyee", new { EmployeeID = hRM_EMPLOYMENTHISTORY.EmployeeID });
                //return RedirectToAction("EmploymentHistoryofEmloyee", new { EmployeeID = hRM_EMPLOYMENTHISTORY.EmployeeID });
                //return RedirectToAction("Index");
            }
            //          var departmentPositions =
            //db.DIC_DEPARTMENT_POSITION
            //  .ToList()
            //  .Select(s => new
            //  {
            //      Department_PositionID = s.Department_PositionID,
            //      Des = string.Format("{0} --- {1} ----{2}", s.DIC_DEPARTMENT.DepartmentName, s.DIC_POSITION.PositionName, s.Coeff)
            //  });

            //ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", hRM_EMPLOYMENTHISTORY.CategoryDecisionID);
            //// ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des",hRM_EMPLOYMENTHISTORY.Department_PositionID);// new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            ////   ViewBag.Department_PositionID = new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Coeff");

            ////ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName", hRM_EMPLOYMENTHISTORY.SalaryStepID);
            //ViewBag.EmployeeID = hRM_EMPLOYMENTHISTORY.EmployeeID;// new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYMENTHISTORY.EmployeeID);
            ViewBag.LoaiTauID = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau");
            ////var hrm_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Where(h => h.EmployeeID == EmployeeID);
            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", hRM_EMPLOYMENTHISTORY.CategoryDecisionID);
            ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name", hRM_EMPLOYMENTHISTORY.LyDoNghiViec_ID);
            //    ViewBag.Department_PositionID = new SelectList(departmentPositions, "Department_PositionID", "Des"); //new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "PositionName");
            //   ViewBag.Department_PositionID =  new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Coeff");
            var departmentID = from a in db.DIC_DEPARTMENT
                               where (a.ParentID == 8 || a.ParentID == 17) && a.IsLast == true
                               select a;
            var positionID = from a in db.DIC_POSITION
                                 //where a.GroupPositionID != 4
                             select a;
            ViewBag.SalaryStepID = new SelectList(db.DIC_SALARY_STEP, "SalaryStepID", "StepName");
            //   ViewBag.EmployeeID = EmployeeID;// new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");

            ViewBag.DepartmentID = new SelectList(departmentID, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentID);
            ViewBag.PositionID = new SelectList(positionID, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PluralityID);

            //Phòng ban kiêm nhiệm
            //ViewBag.DepartmentPluralityID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName");
            ViewBag.DepartmentPluralityID = new SelectList(departmentID, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentPluralityID);//new
            //Thực tập chức vụ?
            ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName", hRM_EMPLOYMENTHISTORY.InternshipPosition);
            //Thực tập chức vụ kiêm nhiệm
            ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName", hRM_EMPLOYMENTHISTORY.IntershipPlurality);
            //Chức vụ kiêm nhiệm

            //ViewBag.PluralityID = new SelectList(positionID, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PluralityID);//new
            //LẤY THÔNG TIN PHÒNG BAN VÀ CHỨC VỤ ĐỂ LẤY ĐƯỢC THÔNG TIN LƯƠNG
            var rankID = from a in db.DIC_SALARY
                         where a.DepartmentID == hRM_EMPLOYMENTHISTORY.DepartmentID && a.PositionID == hRM_EMPLOYMENTHISTORY.PositionID
                         select a;

            //ViewBag.SalaryPositionID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPositionID = new SelectList(rankID, "SalaryID", "RankID", hRM_EMPLOYMENTHISTORY.SalaryPositionID);
            // ViewBag.SalaryPluralityID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPluralityID = new SelectList(rankID, "SalaryID", "RankID", hRM_EMPLOYMENTHISTORY.SalaryPluralityID);
            return PartialView(hRM_EMPLOYMENTHISTORY);
        }

        // GET: HRM_EMPLOYMENTHISTORY/Delete/5
        [Authorize(Roles = "HR, EduCenter")]
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

        // POST: HRM_EMPLOYMENTHISTORY/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR, EduCenter")]
        [Authorize(Roles = "Create")]
        public ActionResult DeleteConfirmed(int id)
        {
            HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Find(id);
            int EmployeeID = (int)hRM_EMPLOYMENTHISTORY.EmployeeID;
            //Setup Status ID của Employee là 1(đang làm việc) nếu loại quyết định là 3(thanh lý hợp đồng)
            if (hRM_EMPLOYMENTHISTORY.CategoryDecisionID == 3)
            {
                var nhanvien = db.HRM_EMPLOYEE.Where(e => e.EmployeeID == hRM_EMPLOYMENTHISTORY.EmployeeID).First();
                nhanvien.StatusID = 1;
                db.Entry(nhanvien).State = EntityState.Modified;
                db.SaveChanges();
            }
            try
            {
                //Kiểm tra giá trị id có trong danh sách của detailplan hay không để xóa đi
                var chitiet = db.HRM_DETAILPLAN.Where(e => e.CrewOffHistoryID == hRM_EMPLOYMENTHISTORY.EmploymentHistoryID).First();
                chitiet.CrewOffHistoryID = null;
                db.Entry(chitiet).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception)
            {

                
            }

            try
            {
                var chitiet = db.HRM_DETAILPLAN.Where(e => e.CrewOnHistoryID == hRM_EMPLOYMENTHISTORY.EmploymentHistoryID).First();
                chitiet.CrewOnHistoryID = null;
                db.Entry(chitiet).State = EntityState.Modified;
                db.SaveChanges();

            }
            catch (Exception)
            {
 
            }
          

            db.HRM_EMPLOYMENTHISTORY.Remove(hRM_EMPLOYMENTHISTORY);
            db.SaveChanges();
           
            return RedirectToAction("EmploymentHistoryofEmloyee", new { EmployeeID = EmployeeID });

        }
        public ActionResult GetPdf(string fileName)
        {
            try
            {
                var fileStream = new FileStream(Server.MapPath("~/App_Data/quyetdinh/") + fileName,
                                            FileMode.Open,
                                            FileAccess.Read
                                          );
                var fsResult = new FileStreamResult(fileStream, "application/pdf");
                return fsResult;
            }
            catch (Exception)
            {

                //throw; 

            }
            return View("ErrorNotExistsView");
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