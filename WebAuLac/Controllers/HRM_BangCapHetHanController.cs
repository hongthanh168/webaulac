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
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace WebAuLac.Controllers
{
    [Authorize]
    public class HRM_BangCapHetHanController : Controller
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

        public ActionResult Index()
        {
            //Trả lại dữ liệu có kèm cảnh báo bằng cấp
            var hRM_EMPLOYEE = db.fn_DegreeLimitList().OrderByDescending(a => a.TH);
            //  var hRM_EMPLOYEE = db.HRM_EMPLOYEE.Include(h => h.DIC_DEPARTMENT_POSITION).Include(h => h.DIC_EDUCATION).Include(h => h.DIC_ETHNIC).Include(h => h.DIC_NATIONALITY).Include(h => h.DIC_RELIGION).Include(h => h.DIC_STATUS);
            return View(hRM_EMPLOYEE.ToArray());

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int? soThangHetHan)
        {
            //lấy ra danh sách khối tàu

            var bchh = db.sp_LayDSChungChiHetHanTheoSoThang(1, 1).ToList();
            if (soThangHetHan.HasValue)
            {
                bchh = db.sp_LayDSChungChiHetHanTheoSoThang(soThangHetHan, 1).ToList();
                //bchh = db.fn_DegreeLimitList_Loc().AsEnumerable().Where(x => (ThanhUtilities.MonthDifference(DateTime.Now, Convert.ToDateTime(x.ExpirationDate)) <= soThangHetHan.Value) && (x.ParentID==17 || x.ParentID==8)).ToList();
            }
                 

            return View(bchh.OrderBy(x => x.TH));
        }
        public ActionResult Index_NhanVien()
        {
            //Trả lại dữ liệu có kèm cảnh báo bằng cấp
            var hRM_EMPLOYEE = db.fn_DegreeLimitList_NhanVien().OrderByDescending(a => a.TH);
            //  var hRM_EMPLOYEE = db.HRM_EMPLOYEE.Include(h => h.DIC_DEPARTMENT_POSITION).Include(h => h.DIC_EDUCATION).Include(h => h.DIC_ETHNIC).Include(h => h.DIC_NATIONALITY).Include(h => h.DIC_RELIGION).Include(h => h.DIC_STATUS);
            return View(hRM_EMPLOYEE.ToArray());

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index_NhanVien(int? soThangHetHan)
        {
            //lấy ra danh sách khối tàu

            var bchh = db.sp_LayDSChungChiHetHanTheoSoThang(1, 2).ToList();
            if (soThangHetHan.HasValue)
            {
                bchh = db.sp_LayDSChungChiHetHanTheoSoThang(soThangHetHan,2).ToList();
                //bchh = db.fn_DegreeLimitList_Loc().AsEnumerable().Where(x => (ThanhUtilities.MonthDifference(DateTime.Now, Convert.ToDateTime(x.ExpirationDate)) <= soThangHetHan.Value) && (x.ParentID==17 || x.ParentID==8)).ToList();
            }


            return View(bchh.OrderBy(x => x.TH));
        }
        public ActionResult IndexPassport()
        {
            //Trả lại dữ liệu có kèm cảnh báo bằng cấp
            var hRM_EMPLOYEE = db.sp_LayDSKhongDatPassport().OrderByDescending(a => a.TH).ThenBy(x =>x.ChucVu).ThenBy(x=>x.DepartmentName).ThenBy(x => x.LastName). ThenBy(x =>x.FirstName);
            return View(hRM_EMPLOYEE.ToList());

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

        // GET: HRM_EMPLOYEE1/Create
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
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HRM_EMPLOYEE hRM_EMPLOYEE = await db.HRM_EMPLOYEE.FindAsync(id);
            db.HRM_EMPLOYEE.Remove(hRM_EMPLOYEE);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //HAM LAY THONG TIN TIEN LUONG THAM KHAO
        [WebMethod]
        public JsonResult XuatBaoCao(string[] function_param)
        {
            CModuleSy cUtitlies = new CModuleSy();
            cUtitlies.DanhSachDoiBangCap(Server.MapPath("~/App_Data"), function_param);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

            //return new JsonResult { Data = 1 };
        }
        [HttpPost]
        public JsonResult XuatBaoCaoTest(string[] function_param)
        {
            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = Server.MapPath("~/App_Data");

            var fileName = "Excel_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";

            //save the file to server temp folder
            string fullPath = Path.Combine(server, fileName);

            FileInfo templateFile = new FileInfo(server + "//Mau_DSBC.xltx");
            //FileInfo newFile = new FileInfo(outputDir.FullName + @"\LLTN.xlsx");
            FileInfo newFile = new FileInfo(fullPath);//Tạo file tạm lưu ra
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\Mau_DSBC.xlsx");
            }
            ExcelPackage package;
            using (package = new ExcelPackage(newFile, templateFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int i = 14;
                //Duyệt qua danh sách để in ra
                int stt = 1;

                foreach (string item in function_param)
                {
                    char[] delimiterChars = { '|' };
                    string[] list = item.Split(delimiterChars);
                    int employeeID = Convert.ToInt32(list[0].Trim());
                    int degreeID = Convert.ToInt32(list[1].Trim());
                    HRM_EMPLOYEE_DEGREE emdre = (from a in db.HRM_EMPLOYEE_DEGREE
                                                 where a.EmployeeID == employeeID && a.DegreeID == degreeID
                                                 select a).First();

                    HRM_EMPLOYEE thuyenvien = (from p in db.HRM_EMPLOYEE
                                               where p.EmployeeID == emdre.EmployeeID
                                               select p).SingleOrDefault();

                    DIC_DEGREE dre = (from a in db.DIC_DEGREE
                                      where a.DegreeID == emdre.DegreeID
                                      select a).First();
                    if (thuyenvien != null)
                    {
                        //1. Số thứ tự
                        worksheet.Cells[i, 1].Value = stt++;
                        //2. Họ và tên thuyền viên
                        worksheet.Cells[i, 2].Value = thuyenvien.FirstName + " " + thuyenvien.LastName;

                        if (thuyenvien.BirthDay != null)
                            worksheet.Cells[i, 3].Value = thuyenvien.BirthDay.Value.ToString("dd/MM/yyyy");

                        if (thuyenvien.BirthPlace != null)
                            worksheet.Cells[i, 4].Value = thuyenvien.BirthPlace;

                        //3. Bằng cấp
                        if (dre.DegreeName != null)
                            worksheet.Cells[i, 5].Value = dre.DegreeName;

                        //3. Số Bằng cấp
                        if (emdre.DegreeNo != null)
                            worksheet.Cells[i, 6].Value = emdre.DegreeNo;

                        //4. Ngày cấp
                        if (emdre.DegreeDate != null)
                            worksheet.Cells[i, 7].Value = Convert.ToDateTime(emdre.DegreeDate).ToString("dd/MM/yyyy");

                        //5. Ngày hết hạn
                        if (emdre.ExpirationDate != null)
                            worksheet.Cells[i, 8].Value = Convert.ToDateTime(emdre.ExpirationDate).ToString("dd/MM/yyyy");

                        if (emdre.GhiChu != null)
                            worksheet.Cells[i, 9].Value = emdre.GhiChu;

                    }

                    i++;
                }

                using (ExcelRange r = worksheet.Cells[14, 1, i - 1, 9])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                using (ExcelRange r = worksheet.Cells[i + 1, 6, i + 1, 9])
                {
                    r.Style.Font.Italic = true;
                    r.Merge = true;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[i + 1, 6].Value = "Ngày " + DateTime.Now.Day.ToString() + " tháng " + DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();
                }

                using (ExcelRange r = worksheet.Cells[i + 2, 6, i + 2, 9])
                {
                    r.Style.Font.Italic = true;
                    r.Merge = true;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[i + 2, 6].Value = "TRUNG TÂM THUYỀN VIÊN";
                }

                package.Save();
            }

            
            //using (var exportData = new MemoryStream())
            //{
            //    //I don't show the detail how to create the Excel, this is not the point of this article,
            //    //I just use the NPOI for Excel handler
               
            //    FileStream file = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            //    exportData.WriteTo(file);
            //    file.Close();
            //}

            var errorMessage = "you can return the errors in here!";

            //return the Excel file name
            return Json(new { fileName = fileName, errorMessage = "" });
        }
        [HttpGet]
        [DeleteFileAttribute] //Action Filter, it will auto delete the file after download, 
                              //I will explain it later
        public ActionResult Download(string file)
        {
            //get the temp folder and file path in server
            string fullPath = Path.Combine(Server.MapPath("~/App_Data"), file);

            //return the file for download, this is an Excel 
            //so I set the file content type to "application/vnd.ms-excel"
            return File(fullPath, "application/vnd.ms-excel", file);
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
    public class DeleteFileAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Flush();

            //convert the current filter context to file and get the file path
            string filePath = (filterContext.Result as FilePathResult).FileName;

            //delete the file after download
            System.IO.File.Delete(filePath);
        }
    }
}