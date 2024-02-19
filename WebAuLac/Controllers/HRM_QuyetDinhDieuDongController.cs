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
using System.Web.Services;
using System.IO;
using Novacode;

namespace WebAuLac.Controllers
{
    [Authorize(Roles = "Boss, HR, EduCenter")]
    public class HRM_QuyetDinhDieuDongController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: HRM_EMPLOYEE1
        //public async Task<ActionResult> Index()
        //{
        //    //Trả lại dữ liệu có kèm cảnh báo bằng cấp
        //    var hRM_EMPLOYEE = db.fn_IsLimitDegree() as HRM_EMPLOYEE;
        //    //  var hRM_EMPLOYEE = db.HRM_EMPLOYEE.Include(h => h.DIC_DEPARTMENT_POSITION).Include(h => h.DIC_EDUCATION).Include(h => h.DIC_ETHNIC).Include(h => h.DIC_NATIONALITY).Include(h => h.DIC_RELIGION).Include(h => h.DIC_STATUS);
        //    return View(await hRM_EMPLOYEE.ToListAsync());
        //}
        public FileResult XuatQuyetDinhDieuDongCaNhan(int ID_QTCT)
        {   
            AuLacEntities dc = new AuLacEntities();

            MemoryStream stream = new MemoryStream();
            string server = Server.MapPath("~/App_Data");
            string fileNameTemplate = "";
            DocX doc = DocX.Create(stream);

            view_quatrinhcongtacFull vemhis = (from p in dc.view_quatrinhcongtacFull
                                               where p.EmploymentHistoryID == ID_QTCT
                                               select p).SingleOrDefault();

            //Bổ sung trường hợp điều xuống Dự trữ trong kế hoạch nhưng thực tế lập quyết định vẫn ở trên tàu
            //Nghiên cứu lại in quyết định
            //1. Nếu quyết định về Dự trữ thì kiểm tra xem là có thay thế hay không
            //2. Nếu lên tàu thì chia ra các trường hợp

            Boolean is_on = false;
            int departmentID = Convert.ToInt32(vemhis.DepartmentID);
            int parentID = Convert.ToInt32(vemhis.ParentID);

            if (parentID == 8)
                is_on = true;//Lên tàu


            HRM_DETAILPLAN dtplan = (from p in dc.HRM_DETAILPLAN
                                     where p.CrewOffHistoryID == ID_QTCT
                                     select p).SingleOrDefault();
            if (dtplan == null)
            {
                dtplan = (from p in dc.HRM_DETAILPLAN
                          where p.CrewOnHistoryID == ID_QTCT
                          select p).SingleOrDefault();
            }

            String donvi, chucvu, tentau, nguoinhanbangiao;
            nguoinhanbangiao = "";
            tentau = dtplan.DIC_DEPARTMENT.DepartmentName;
            

            if (is_on)
            {
                //Chia làm 2 từ Dự trữ lên tàu hoặc bổ nhiệm từ tàu lên tàu
                if (dtplan.CrewOffID == vemhis.EmployeeID)
                {
                    //Từ tàu lên tàu, bổ nhiệm
                    chucvu = dtplan.CrewOffPosition;
                    donvi = "TÀU " + dtplan.DIC_DEPARTMENT.DepartmentName;
                }
                else
                {
                    // Từ dự trữ lên tàu
                    chucvu = dtplan.CrewOnPosition;
                    donvi = "BỘ PHẬN DỰ TRỮ THUYỀN VIÊN";
                }
            }
            else
            {
                if (dtplan.CrewOnID == vemhis.EmployeeID)
                {
                    //Từ dự trữ bổ nhiệm, ... ở dự trữ
                    chucvu = dtplan.CrewOnPosition;
                    donvi = "BỘ PHẬN DỰ TRỮ THUYỀN VIÊN";
                }
                else
                {
                    //Từ dự trữ lên tàu
                    chucvu = dtplan.CrewOffPosition;
                    donvi = "TÀU " + dtplan.DIC_DEPARTMENT.DepartmentName;
                    try
                    {
                        nguoinhanbangiao = (from p in dc.HRM_EMPLOYEE
                                            where p.EmployeeID == dtplan.CrewOnID
                                            select p.FirstName + " " + p.LastName).Single();
                    }
                    catch { }
                }
            }

            if (is_on)// từ dự trữ lên trên tàu
            {
                // X? lý các tru?ng h?p, d?m nh?n ch?c danh, th?c t?p, ...
                //1. Th?c t?p, 2. Tang cu?ng, 3. Làm quen, 4. Overlap, 5. null Ð?m nh?n ch?c danh
                if (vemhis.IntershipPlurality == 1)
                {
                    int cvknid = Convert.ToInt32(vemhis.PluralityID);
                    int nhomcv = (int)(from p in dc.DIC_POSITION
                                 where p.PositionID == cvknid
                                 select p.GroupPositionID).SingleOrDefault();

                    if(nhomcv ==1 || vemhis.PluralityID == 33 || vemhis.PluralityID == 34|| vemhis.PluralityID == 35)
                        fileNameTemplate = server + @"//Mau_QDDD_KiemNhiemSQBoong.docx";
                    else if(nhomcv == 2 || vemhis.PluralityID == 29 || vemhis.PluralityID == 30 || vemhis.PluralityID == 31 || vemhis.PluralityID == 32)
                        fileNameTemplate = server + @"//Mau_QDDD_KiemNhiemSQMay.docx";
                    else
                        fileNameTemplate = server + @"//Mau_QDDD_KiemNhiemSQBoong.docx";
                }
                else
                {
                    if (vemhis.InternshipPosition != null)
                    {
                        if (vemhis.InternshipPosition == 1)
                            fileNameTemplate = server + @"//Mau_QDDD_ThucTap.docx";
                        else if (vemhis.InternshipPosition == 2)
                            fileNameTemplate = server + @"//Mau_QDDD_TangCuong.docx";
                        else if (vemhis.InternshipPosition == 3)
                            fileNameTemplate = server + @"//Mau_QDDD_LamQuen.docx";
                        else
                            fileNameTemplate = server + @"//Mau_QDDD_Overlap.docx";
                    }
                    else
                    {
                        fileNameTemplate = server + @"//Mau_QDDD_DamNhanChucDanh.docx";
                    }
                }
            }
            else
            {
                //Từ tàu về dự trữ
                if (dtplan.CrewOnHistoryID != null)
                    fileNameTemplate = server + @"//Mau_QDDD_DuTru11.docx";
                else
                    fileNameTemplate = server + @"//Mau_QDDD_DuTru.docx";
            }


            doc = DocX.Load(fileNameTemplate);
            if (vemhis.DecisionNo != null)
                doc.ReplaceText("%SoQD%", vemhis.DecisionNo.ToString().ToUpper() + "/" + vemhis.DecisionDate.Value.Year.ToString().Substring(2, 2) + "/QĐ-ĐĐ");



            //Chuc vu kiem nhiem
            String ngayQD, thangQD, namQD;

            if (vemhis.DecisionDate.Value.Day < 10)
                ngayQD = "0" + vemhis.DecisionDate.Value.Day.ToString();
            else
                ngayQD = vemhis.DecisionDate.Value.Day.ToString();

            if (vemhis.DecisionDate.Value.Month < 3)
                thangQD = "0" + vemhis.DecisionDate.Value.Month.ToString();
            else
                thangQD = vemhis.DecisionDate.Value.Month.ToString();

            namQD = vemhis.DecisionDate.Value.Year.ToString();

            doc.ReplaceText("%Ngay%", ngayQD);
            doc.ReplaceText("%Thang%", thangQD);
            doc.ReplaceText("%Nam%", namQD);

            doc.ReplaceText("%Name%", vemhis.FirstName.ToUpper() + " " + vemhis.LastName.ToUpper());
            doc.ReplaceText("%LastName%", vemhis.LastName.ToUpper());

            string ngaySinh;
            if (vemhis.BirthDay.Value.Day < 10)
                ngaySinh = "0" + vemhis.BirthDay.Value.Day.ToString();
            else
                ngaySinh = vemhis.BirthDay.Value.Day.ToString();

            if (vemhis.BirthDay.Value.Month < 3)
                ngaySinh += "/0" + vemhis.BirthDay.Value.Month.ToString() + "/" + vemhis.BirthDay.Value.Year.ToString();
            else
                ngaySinh += "/" + vemhis.BirthDay.Value.Month.ToString() + "/" + vemhis.BirthDay.Value.Year.ToString();

            if (vemhis.BirthDay != null)
                doc.ReplaceText("%NamSinh%", ngaySinh);

            if (donvi != null)
                doc.ReplaceText("%DonVi%", donvi.ToUpper());

            if (chucvu != null)
                doc.ReplaceText("%ChucVu%", chucvu.ToUpper());

            if (vemhis.PluralityName != null)
                doc.ReplaceText("%ChucVuKiemNhiem%", vemhis.PluralityName.ToUpper());

            if (parentID == 17)
                doc.ReplaceText("%DonViMoi%", "BỘ PHẬN DỰ TRỮ THUYỀN VIÊN");
            else
                doc.ReplaceText("%DonViMoi%", "TÀU " + vemhis.DepartmentName.ToUpper());

            if (vemhis.ChucVu != null)
                doc.ReplaceText("%ChucVuMoi%", vemhis.ChucVu.ToUpper());

            String ngayHieuLuc;
            if (vemhis.EffectiveDate.Value.Day < 10)
                ngayHieuLuc = "0" + vemhis.EffectiveDate.Value.Day.ToString();
            else
                ngayHieuLuc = vemhis.EffectiveDate.Value.Day.ToString();

            if (vemhis.EffectiveDate.Value.Month < 3)
                ngayHieuLuc += "/0" + vemhis.EffectiveDate.Value.Month.ToString() + "/" + vemhis.EffectiveDate.Value.Year.ToString();
            else
                ngayHieuLuc += "/" + vemhis.EffectiveDate.Value.Month.ToString() + "/" + vemhis.EffectiveDate.Value.Year.ToString();

            if (vemhis.EffectiveDate != null)
                doc.ReplaceText("%NgayQD%", ngayHieuLuc);

            if (nguoinhanbangiao != null)
                doc.ReplaceText("%NguoiNhanBanGiao%", nguoinhanbangiao);

            if (tentau != null)
                doc.ReplaceText("%TenTau%", tentau);

            if (vemhis.ContentDecision != null)
                doc.ReplaceText("%NoiDungQuyetDinh%", vemhis.ContentDecision.ToUpper());

            doc.SaveAs(stream);
            return File(stream.ToArray(), "application/octet-stream", "QDDD.docx");
        }

        public ActionResult Index(int planID, int? TauID)
        {
            //-----THANH SỬA NGÀY 6/2/2018
            var hRM_EMPLOYEE = db.fn_ListCrewTemp(planID);
            //lấy ra danh sách các department có trong kế hoạch
            var listDept= hRM_EMPLOYEE.Select(x => x.OnDepartment).ToList();
            //var list2 = hRM_EMPLOYEE.Select(x => x.OffDepartment).ToList();
            //listDept.AddRange(list2);
            var dp = db.DIC_DEPARTMENT.Where(x => listDept.Contains(x.DepartmentName)).OrderBy(x => x.DepartmentID);                  
            //----HẾT PHẦN THANH SỬA NGÀY 6/2/2018            
            if (TauID.HasValue)
            {
                DIC_DEPARTMENT dept = db.DIC_DEPARTMENT.Find(TauID);
                if (dept != null)
                {
                    hRM_EMPLOYEE = hRM_EMPLOYEE.Where(x => x.OnDepartment==dept.DepartmentName);
                    ViewBag.TauID = new SelectList(dp, "DepartmentID", "DepartmentName", TauID);
                }
                else
                {
                    ViewBag.TauID = new SelectList(dp, "DepartmentID", "DepartmentName");
                }
            }else
            {
                ViewBag.TauID = new SelectList(dp, "DepartmentID", "DepartmentName");
            }
            ViewBag.PlanID = planID;
            hRM_EMPLOYEE = hRM_EMPLOYEE.OrderBy(a => a.DetailPlanID).ThenBy(a => a.PositionID);
            return View(hRM_EMPLOYEE.ToArray());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LocTau(int PlanID, int? TauID)
        {
            Session["idkh"] = PlanID;
            if (TauID.HasValue)
            {
                Session["idTau"] = TauID.Value;
            }
            else
            {
                Session["idTau"] = 0;
            }
            
            return RedirectToAction("Index", new { PlanID = PlanID, TauID = TauID });
        }

        
        public ActionResult XacnhanQD(int planID, int? TauID)
        {
            //-----THANH SỬA NGÀY 6/2/2018
            var hRM_EMPLOYEE = db.fn_ListCrewTemp(planID);
            //lấy ra danh sách các department có trong kế hoạch
            var listDept = hRM_EMPLOYEE.Select(x => x.OnDepartment).ToList();
            //var list2 = hRM_EMPLOYEE.Select(x => x.OffDepartment).ToList();
            //listDept.AddRange(list2);
            var dp = db.DIC_DEPARTMENT.Where(x => listDept.Contains(x.DepartmentName)).OrderBy(x => x.DepartmentID);
            //----HẾT PHẦN THANH SỬA NGÀY 6/2/2018            
            if (TauID.HasValue)
            {
                DIC_DEPARTMENT dept = db.DIC_DEPARTMENT.Find(TauID);
                if (dept != null)
                {
                    hRM_EMPLOYEE = hRM_EMPLOYEE.Where(x => x.OnDepartment == dept.DepartmentName);
                    ViewBag.TauID = new SelectList(dp, "DepartmentID", "DepartmentName", TauID);
                }
                else
                {
                    ViewBag.TauID = new SelectList(dp, "DepartmentID", "DepartmentName");
                }
            }
            else
            {
                ViewBag.TauID = new SelectList(dp, "DepartmentID", "DepartmentName");
            }
            Session["idkh_xacnhan"] = planID;
            if (TauID.HasValue)
            {
                Session["idTau_xacnhan"] = TauID.Value;
            }
            else
            {
                Session["idTau_xacnhan"] = 0;
            }
            ViewBag.PlanID = planID;
            hRM_EMPLOYEE = hRM_EMPLOYEE.OrderBy(a => a.DetailPlanID).ThenBy(a => a.PositionID);
            return View(hRM_EMPLOYEE.ToArray());

        }
        // GET: HRM_EMPLOYEE1/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE hRM_EMPLOYEE = await db.HRM_EMPLOYEE.FindAsync(id);
            if (hRM_EMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(hRM_EMPLOYEE);
        }
        //hàm nhập quyết định theo danh sách chọn
        [WebMethod]
        public ActionResult XuatBaoCao(string[] function_param)
        {

            Session["list"] = function_param;
            Session["index"] = 0;
            char[] delimiterChars = { '|' };
            string[] list = function_param[0].Split(delimiterChars);
            return new JsonResult { Data = new { e = list[0], p = list[1], d = list[2] } };
            //foreach (var item in function_param)
            //{
            //    //với mỗi người hiển thị nhập bằng cấp
            //    char[] delimiterChars = { '|' };
            //    string[] list = item.Split(delimiterChars);

            //    return View("~/Views/hRM_EMPLOYMENTHISTORY/CreateOne?EmployeeID=341&PlanID=2&detailPlanID=19");
            //    //RedirectToAction("CreateOne", "hRM_EMPLOYMENTHISTORY", new { EmployeeID = list[0], PlanID = list[1], detailPlanID = list[2] });
            //}
            //return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        //hàm lấy thông tin các quyết định cần xác nhận
        [WebMethod]
        [Authorize(Roles = "EduCenter")]
        [Authorize(Roles = "Create")]
        public ActionResult XacNhanQuyetDinh(string[] function_param)
        {
           
            Session["listHistory"] = function_param;
            Session["index"] = 0;
            char[] delimiterChars = { '|' };
            string[] list = function_param[0].Split(delimiterChars);
            Session["planID"] = list[1];
            return new JsonResult { Data = new { e = list[0] } };
            //foreach (var item in function_param)
            //{
            //    //với mỗi người hiển thị nhập bằng cấp
            //    char[] delimiterChars = { '|' };
            //    string[] list = item.Split(delimiterChars);

            //    return View("~/Views/hRM_EMPLOYMENTHISTORY/CreateOne?EmployeeID=341&PlanID=2&detailPlanID=19");
            //    //RedirectToAction("CreateOne", "hRM_EMPLOYMENTHISTORY", new { EmployeeID = list[0], PlanID = list[1], detailPlanID = list[2] });
            //}
            //return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [WebMethod]
        [Authorize(Roles = "EduCenter")]
        [Authorize(Roles = "Create")]
        public ActionResult KhongThucHien(string[] function_param)
        {

            Session["listHistory"] = function_param;
            Session["index"] = 0;
            char[] delimiterChars = { '|' };
            string[] list = function_param[0].Split(delimiterChars);
            //Session["planID"] = list[1];
            //Xác nhận  = 0 cho các quyết định đã chọn
            for (int i = 0; i < function_param.Length; i++)
            {
                list = function_param[i].Split(delimiterChars);
                HRM_EMPLOYMENTHISTORY qd = db.HRM_EMPLOYMENTHISTORY.Find(Convert.ToInt32(list[0]));
                qd.XacNhan = false;
                qd.NgayXuongTau = null;
                db.Entry(qd).State = EntityState.Modified;
                db.SaveChanges();
                HRM_EMPLOYEE nv = db.HRM_EMPLOYEE.Find(qd.EmployeeID);
                nv.SSDD = null;
                nv.NgaySSDD = null;
                nv.GhiChuSSDD = null;
                db.Entry(nv).State = EntityState.Modified;
                db.SaveChanges();
            }
            return new JsonResult { Data = new { e = list[0] } };
            //foreach (var item in function_param)
            //{
            //    //với mỗi người hiển thị nhập bằng cấp
            //    char[] delimiterChars = { '|' };
            //    string[] list = item.Split(delimiterChars);

            //    return View("~/Views/hRM_EMPLOYMENTHISTORY/CreateOne?EmployeeID=341&PlanID=2&detailPlanID=19");
            //    //RedirectToAction("CreateOne", "hRM_EMPLOYMENTHISTORY", new { EmployeeID = list[0], PlanID = list[1], detailPlanID = list[2] });
            //}
            //return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        // GET: HRM_EMPLOYEE1/Create
        [Authorize(Roles = "EduCenter")]
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            ViewBag.Department_PositionID = new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Decription");
            ViewBag.EducationID = new SelectList(db.DIC_EDUCATION, "EducationID", "EducationName");
            ViewBag.EthnicID = new SelectList(db.DIC_ETHNIC, "EthnicID", "EthnicName");
            ViewBag.NationalityID = new SelectList(db.DIC_NATIONALITY, "NationalityID", "NationalityName");
            ViewBag.ReligionID = new SelectList(db.DIC_RELIGION, "ReligionID", "ReligionName");
            ViewBag.StatusID = new SelectList(db.DIC_STATUS, "StatusID", "StatusName");
            return View();
        }

        // POST: HRM_EMPLOYEE1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EduCenter")]
        [Authorize(Roles = "Create")]
        public async Task<ActionResult> Create([Bind(Include = "EmployeeID,EmployeeCode,CardNo,FirstName,LastName,Alias,Sex,Marriage,BirthDay,BirthPlace,MainAddress,ContactAddress,CellPhone,HomePhone,Email,Skype,Yahoo,Facebook,IDCard,IDCardDate,IDCardPlace,TaxNo,BankCode,BankName,InsuranceCode,InsuranceDate,Photo,EducationID,DegreeID,EthnicID,ReligionID,NationalityID,Department_PositionID,StatusID,IsDaiDuong")] HRM_EMPLOYEE hRM_EMPLOYEE)
        {
            if (ModelState.IsValid)
            {
                db.HRM_EMPLOYEE.Add(hRM_EMPLOYEE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Department_PositionID = new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Decription", hRM_EMPLOYEE.Department_PositionID);
            ViewBag.EducationID = new SelectList(db.DIC_EDUCATION, "EducationID", "EducationName", hRM_EMPLOYEE.EducationID);
            ViewBag.EthnicID = new SelectList(db.DIC_ETHNIC, "EthnicID", "EthnicName", hRM_EMPLOYEE.EthnicID);
            ViewBag.NationalityID = new SelectList(db.DIC_NATIONALITY, "NationalityID", "NationalityName", hRM_EMPLOYEE.NationalityID);
            ViewBag.ReligionID = new SelectList(db.DIC_RELIGION, "ReligionID", "ReligionName", hRM_EMPLOYEE.ReligionID);
            ViewBag.StatusID = new SelectList(db.DIC_STATUS, "StatusID", "StatusName", hRM_EMPLOYEE.StatusID);
            return View(hRM_EMPLOYEE);
        }

        // GET: HRM_EMPLOYEE1/Edit/5
        [Authorize(Roles = "EduCenter")]
        [Authorize(Roles = "Create")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE hRM_EMPLOYEE = await db.HRM_EMPLOYEE.FindAsync(id);
            if (hRM_EMPLOYEE == null)
            {
                return HttpNotFound();
            }
            ViewBag.Department_PositionID = new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Decription", hRM_EMPLOYEE.Department_PositionID);
            ViewBag.EducationID = new SelectList(db.DIC_EDUCATION, "EducationID", "EducationName", hRM_EMPLOYEE.EducationID);
            ViewBag.EthnicID = new SelectList(db.DIC_ETHNIC, "EthnicID", "EthnicName", hRM_EMPLOYEE.EthnicID);
            ViewBag.NationalityID = new SelectList(db.DIC_NATIONALITY, "NationalityID", "NationalityName", hRM_EMPLOYEE.NationalityID);
            ViewBag.ReligionID = new SelectList(db.DIC_RELIGION, "ReligionID", "ReligionName", hRM_EMPLOYEE.ReligionID);
            ViewBag.StatusID = new SelectList(db.DIC_STATUS, "StatusID", "StatusName", hRM_EMPLOYEE.StatusID);
            return View(hRM_EMPLOYEE);
        }

        // POST: HRM_EMPLOYEE1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "EduCenter")]
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmployeeID,EmployeeCode,CardNo,FirstName,LastName,Alias,Sex,Marriage,BirthDay,BirthPlace,MainAddress,ContactAddress,CellPhone,HomePhone,Email,Skype,Yahoo,Facebook,IDCard,IDCardDate,IDCardPlace,TaxNo,BankCode,BankName,InsuranceCode,InsuranceDate,Photo,EducationID,DegreeID,EthnicID,ReligionID,NationalityID,Department_PositionID,StatusID,IsDaiDuong")] HRM_EMPLOYEE hRM_EMPLOYEE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRM_EMPLOYEE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Department_PositionID = new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Decription", hRM_EMPLOYEE.Department_PositionID);
            ViewBag.EducationID = new SelectList(db.DIC_EDUCATION, "EducationID", "EducationName", hRM_EMPLOYEE.EducationID);
            ViewBag.EthnicID = new SelectList(db.DIC_ETHNIC, "EthnicID", "EthnicName", hRM_EMPLOYEE.EthnicID);
            ViewBag.NationalityID = new SelectList(db.DIC_NATIONALITY, "NationalityID", "NationalityName", hRM_EMPLOYEE.NationalityID);
            ViewBag.ReligionID = new SelectList(db.DIC_RELIGION, "ReligionID", "ReligionName", hRM_EMPLOYEE.ReligionID);
            ViewBag.StatusID = new SelectList(db.DIC_STATUS, "StatusID", "StatusName", hRM_EMPLOYEE.StatusID);
            return View(hRM_EMPLOYEE);
        }

        // GET: HRM_EMPLOYEE1/Delete/5
        [Authorize(Roles = "EduCenter")]
        [Authorize(Roles = "Create")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE hRM_EMPLOYEE = await db.HRM_EMPLOYEE.FindAsync(id);
            if (hRM_EMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(hRM_EMPLOYEE);
        }

        // POST: HRM_EMPLOYEE1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EduCenter")]
        [Authorize(Roles = "Create")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HRM_EMPLOYEE hRM_EMPLOYEE = await db.HRM_EMPLOYEE.FindAsync(id);
            db.HRM_EMPLOYEE.Remove(hRM_EMPLOYEE);
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