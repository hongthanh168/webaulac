using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAuLac.Models;

namespace WebAuLac.Controllers
{
    [Authorize(Roles = "Luong, Boss")]
    public class HRM_SALARYController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: HRM_SALARY
        [Authorize(Roles = "Luong, Boss")]
        public ActionResult Index()
        {
            return View(db.HRM_SALARY.OrderByDescending(x =>x.Years).ThenByDescending(x => x.Months).ToList());
        }

        // GET: HRM_SALARY/Details/5
        [Authorize(Roles = "Luong, Boss")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_SALARY hRM_SALARY = db.HRM_SALARY.Find(id);
            if (hRM_SALARY == null)
            {
                return HttpNotFound();
            }
            ViewBag.bangLuong = hRM_SALARY;

            var dp = db.DIC_DEPARTMENT.Where(x => (x.ParentID == 8 || x.ParentID == 17) && x.DepartmentID != 11 && x.DepartmentID != 24).OrderBy(x => x.ParentID);
            ViewBag.DepartmentID = new SelectList(dp, "DepartmentID", "DepartmentName");
            ViewBag.CurrentDeptID = 0;
            //mặc định lấy ra khối phòng ban
            var phongBan = db.DIC_DEPARTMENT.Where(x => x.ParentID == 1).Select(x => x.DepartmentID).ToList();

            db.sp_Luong_1(id);
            //var q = db.sp_Luong_3(id).ToList();
            var q = db.viewLuongThangs.ToList();
            return View(q);
            //return View(db.viewLuongThangs.Where(x => x.HRMSalaryID==id && phongBan.Contains(x.DepartmentSalaryID.Value)).ToList().OrderBy(x=> x.ThuTuPhongBan).ThenBy(x => x.PositionID));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Luong, Boss")]
        public ActionResult Details(int? id, int? DepartmentID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            HRM_SALARY hRM_SALARY = db.HRM_SALARY.Find(id);
            if (hRM_SALARY == null)
            {
                return HttpNotFound();
            }
            ViewBag.bangLuong = hRM_SALARY;
            var dp = db.DIC_DEPARTMENT.Where(x => (x.ParentID == 8 || x.ParentID == 17) && x.DepartmentID != 11 && x.DepartmentID != 24).OrderBy(x => x.ParentID);
            ViewBag.DepartmentID = new SelectList(dp, "DepartmentID", "DepartmentName");
            if (DepartmentID.HasValue && DepartmentID.Value > 0)
            {
                ViewBag.CurrentDeptID = DepartmentID;

                int parentID = Convert.ToInt32((from p in db.DIC_DEPARTMENT
                                where p.DepartmentID == DepartmentID

                                select p.ParentID).FirstOrDefault());

                if (parentID == 17)
                {                    
                    //var q = db.sp_Luong_3(id).Where(x => x.DepartmentSalaryID == DepartmentID.Value).ToList().OrderBy(x => x.DepartmentLuongID).ThenBy(x => x.ThuTuChucVu);
                    var q = db.viewLuongThangs.Where(x => x.DepartmentLuongID == 1000).ToList().OrderBy(x => x.ThuTuChucVu);//.ThenBy(x => x.ThuTuPhongBan)

                    return View(q);
                }
                else
                {
                    //var q = db.sp_Luong_3(id).Where(x => x.DepartmentSalaryID == DepartmentID.Value).ToList().OrderBy(x => x.DepartmentLuongID).ThenBy(x => x.ThuTuChucVu);
                    var q = db.viewLuongThangs.Where(x => x.DepartmentLuongID == DepartmentID.Value).ToList().OrderBy(x => x.ThuTuPhongBan).ThenBy(x => x.ThuTuChucVu);
                    return View(q);
                }                
            }
            else
            {
                ViewBag.CurrentDeptID = 0;
                var phongBan = db.DIC_DEPARTMENT.Where(x => x.ParentID == 1).Select(x => x.DepartmentID).ToList();
                //return View(db.viewLuongThangs.Where(x => x.HRMSalaryID == id && phongBan.Contains(x.DepartmentSalaryID.Value)).ToList().OrderBy(x => x.ThuTuPhongBan).ThenBy(x => x.PositionID));
                //db.sp_Luong_1(id);
                //var q = db.sp_Luong_3(id).Where(x => phongBan.Contains(x.DepartmentLuongID.Value)).ToList().OrderBy(x => x.DepartmentLuongID).ThenBy(x => x.ThuTuChucVu);
                var q = db.viewLuongThangs.Where(x => phongBan.Contains(x.DepartmentLuongID.Value)).ToList().OrderBy(x => x.ThuTuPhongBan).ThenBy(x => x.ThuTuChucVu);
                return View(q);
            }            
        }
               
        public ActionResult Refresh(int id)
        {
            db.HRM_DETAILSALARY.RemoveRange(db.HRM_DETAILSALARY.Where(x => x.HRMSalaryID == id));
            db.SaveChanges();
            db.sp_InsertBangLuong(id);
            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult LoadTienLuong(int id)
        {
            //db.HRM_DETAILSALARY.RemoveRange(db.HRM_DETAILSALARY.Where(x => x.HRMSalaryID == id));
            //db.SaveChanges();

            //Update tiền lương
            db.sp_LayTienLuong(id);
            return RedirectToAction("Details", new { id = id });
        }
      
        // GET: HRM_SALARY/Create
        [Authorize(Roles = "Luong")]
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: HRM_SALARY/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Luong")]
        [Authorize(Roles = "Create")]
        public ActionResult Create([Bind(Include = "HRMSalaryID,HRMSalaryName,Years,Months,PerBHXH,PerBHYT,PerBHTN,PerCongDoan,TotalDay,HeSo")] HRM_SALARY hRM_SALARY)
        {
            if (ModelState.IsValid)
            {
                db.HRM_SALARY.Add(hRM_SALARY);
                db.SaveChanges();
                //chạy script tạo bảng lương
                db.sp_InsertBangLuong(hRM_SALARY.HRMSalaryID);
                return Json(new { success =true}, JsonRequestBehavior.AllowGet);
            }

            return PartialView(hRM_SALARY);
        }

        // GET: HRM_SALARY/Edit/5
        [Authorize(Roles = "Luong")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_SALARY hRM_SALARY = db.HRM_SALARY.Find(id);
            if (hRM_SALARY == null)
            {
                return HttpNotFound();
            }
            return PartialView(hRM_SALARY);
        }

        // POST: HRM_SALARY/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Luong")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit([Bind(Include = "HRMSalaryID,HRMSalaryName,Years,Months,PerBHXH,PerBHYT,PerBHTN,PerCongDoan,TotalDay,HeSo")] HRM_SALARY hRM_SALARY)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRM_SALARY).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(hRM_SALARY);
        }

        // GET: HRM_SALARY/Delete/5
        [Authorize(Roles = "Luong")]
        [Authorize(Roles = "Create")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_SALARY hRM_SALARY = db.HRM_SALARY.Find(id);
            if (hRM_SALARY == null)
            {
                return HttpNotFound();
            }
            return PartialView(hRM_SALARY);
        }

        // POST: HRM_SALARY/Delete/5
        [Authorize(Roles = "Luong")]
        [Authorize(Roles = "Create")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Xóa chi tiết lương
            db.HRM_DETAILSALARY.RemoveRange(db.HRM_DETAILSALARY.Where(x =>x.HRMSalaryID==id));
            HRM_SALARY hRM_SALARY = db.HRM_SALARY.Find(id);
            db.HRM_SALARY.Remove(hRM_SALARY);
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

        //XUAT EXCEL LUONG
        [Authorize(Roles = "Luong, Boss")]
        public ActionResult XuatExcelLuong(int? id, int? DepartmentID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (DepartmentID == 0)
            {
                return DanhSachLuongKhoiVanPhong(id.Value);
            }
            else
            {
                //return DanhSachLuongDuTru(DepartmentID.Value, id.Value);
                return DanhSachLuongDuTru(DepartmentID.Value, id.Value);
            }
        }

        public FileResult DanhSachLuongTau(int departmentID, int hrm_salaryID)
        {
            //Bỏ
            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = Server.MapPath("~/App_Data");

            FileInfo templateFile = new FileInfo(server + "//Mau_LuongTau.xltx");
            FileInfo newFile = new FileInfo(outputDir.FullName + @"\LuongTau.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\LuongTau.xlsx");
            }


            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];


                //****************************************              
                //****************************************                
                int indexRow = 9;

                int i = 0;
                int stt = 1;
                int nhom = 1;

                //var listLuong = db.sp_Luong_3(hrm_salaryID).Where(x => x.DepartmentSalaryID == departmentID).ToList().OrderBy(x => x.DepartmentLuongID).ThenBy(x => x.ThuTuChucVu);
                var listLuong = from p in db.viewLuongThangs
                                where p.DepartmentLuongID == departmentID && p.HRMSalaryID == hrm_salaryID
                                orderby p.ThuTuPhongBan, p.ThuTuChucVu
                                select p;

                int parentID = Convert.ToInt32((from p in db.DIC_DEPARTMENT
                                                where p.DepartmentID == departmentID
                                                select p.ParentID).FirstOrDefault());

                //if (parentID == 17)
                //    listLuong = from p in db.viewLuongThangs
                //                where p.DepartmentLuongID == 1000 && p.HRMSalaryID == hrm_salaryID
                //                orderby p.ThuTuChucVu
                //                select p;


                //var listLuong = from p in db.sp_Luong_3(hrm_salaryID)
                //                where p.DepartmentSalaryID == departmentID && p.HRMSalaryID == hrm_salaryID
                //                orderby p.DepartmentLuongID, p.PositionID
                //                select p;

                var tentau = (from p in db.DIC_DEPARTMENT
                              where p.DepartmentID == departmentID
                              select p.DepartmentName).SingleOrDefault();
                worksheet.Cells[2, 1].Value = "BẢNG LƯƠNG TÀU " + tentau.ToUpper();

                double tong_tongthunhap = 0;
                double tong_luongcoban = 0;
                double tong_phucap = 0;
                double tong_thunhapthoathuan = 0;
                double tong_songay = 0;
                double tong_bhxh = 0;
                double tong_bhyt = 0;
                double tong_congdoan = 0;
                double tong_thatnghiep = 0;
                double tong_conlai = 0;

                int m_departmentID = 0;

                foreach (var luong in listLuong)
                {

                    if (m_departmentID != 0)
                    {
                        if (m_departmentID != luong.DepartmentID)
                        {
                            stt = 1;
                            i++;
                        }                        
                    }
                    m_departmentID = Convert.ToInt32(luong.DepartmentID);

                    worksheet.Cells[indexRow + i, 1].Value = stt;


                    worksheet.Cells[indexRow + i, 2].Value = luong.FirstName;
                    worksheet.Cells[indexRow + i, 3].Value = luong.LastName;
                    worksheet.Cells[indexRow + i, 4].Value = luong.ChucVu;
                    if (luong.BankCode != null)
                        worksheet.Cells[indexRow + i, 5].Value = luong.BankCode;

                    worksheet.Cells[indexRow + i, 6].Value = luong.Salary.Value;//.ToString("#,##0");
                    tong_luongcoban += luong.Salary.Value;

                    if (luong.AllowanceSalary > 0)
                    {
                        worksheet.Cells[indexRow + i, 7].Value = luong.AllowanceSalary;//.Value.ToString("#,##0");
                        tong_phucap += luong.AllowanceSalary.Value;
                    }

                    if (luong.Bonus > 0)
                    {
                        worksheet.Cells[indexRow + i, 8].Value = luong.Bonus.Value;//.ToString("#,##0");

                        tong_thunhapthoathuan += luong.Bonus.Value;
                    }


                    worksheet.Cells[indexRow + i, 9].Value = luong.NumOfDay.Value;//.ToString("#,##0");
                    tong_songay += luong.NumOfDay.Value;

                    worksheet.Cells[indexRow + i, 11].Value = luong.TongThuNhap.Value;//.ToString("#,##0");
                    tong_tongthunhap += luong.TongThuNhap.Value;

                    worksheet.Cells[indexRow + i, 12].Value = luong.BHXH.Value;//.ToString("#,##0");
                    tong_bhxh += luong.BHXH.Value;

                    worksheet.Cells[indexRow + i, 13].Value = luong.BHYT.Value;//.ToString("#,##0");
                    tong_bhyt += luong.BHYT.Value;

                    worksheet.Cells[indexRow + i, 14].Value = luong.CongDoan.Value;//.ToString("#,##0");
                    tong_congdoan += luong.CongDoan.Value;

                    worksheet.Cells[indexRow + i, 15].Value = luong.BHTN.Value;//.ToString("#,##0");
                    tong_thatnghiep += luong.BHTN.Value;

                    worksheet.Cells[indexRow + i, 16].Value = luong.ConLai.Value;//.ToString("#,##0");
                    tong_conlai += luong.ConLai.Value;

                    //worksheet.Cells[indexRow + i, 17].Value = luong.TongThuNhap;
                    worksheet.Cells[indexRow + i, 18].Value = stt;
                    worksheet.Cells[indexRow + i, 19].Value = luong.FirstName.Trim() + " " + luong.LastName.Trim();
                    worksheet.Cells[indexRow + i, 21].Value = luong.ChucVu;
                    //Lấy tất cả quyết định trong tháng theo thứ tự đưa vào
                    var list_quyetdinh = from p in db.HRM_EMPLOYMENTHISTORY
                                         where p.EmployeeID == luong.EmployeeID && p.NgayXuongTau.Value.Year == luong.Years.Value && p.NgayXuongTau.Value.Month == luong.Months.Value && p.XacNhan == true
                                         orderby p.NgayXuongTau, p.EmploymentHistoryID
                                         select p;
                    String ghiChu = "";//"Theo QĐ số 357/17/QĐ-ĐĐ hiệu lực ngày 28/07/2017, Theo QĐ số 372/17/QĐ-ĐĐ hiệu lực ngày 28/07/2017"

                    if (list_quyetdinh != null)
                    {
                        foreach (HRM_EMPLOYMENTHISTORY quyetdinh in list_quyetdinh)
                        {
                            ghiChu += "Theo QĐ số " + quyetdinh.DecisionNo.ToString() + "/" + quyetdinh.NgayXuongTau.Value.Year.ToString() + "/QĐ-ĐĐ" + "hiệu lực ngày " + quyetdinh.NgayXuongTau.Value.ToString("dd/MM/yyyy") + " ";
                        }
                    }
                    
                    worksheet.Cells[indexRow + i, 22].Value = ghiChu;
                    if (luong.NumOfDay != luong.TotalDay)
                    {
                        ExcelRange rd = worksheet.Cells[indexRow + i, 2, indexRow + i, 22];
                        rd.Style.Font.Color.SetColor(System.Drawing.Color.Red);
                    }                    

                    i++;
                    stt++;
                }
                // Tong cong
                i++;

                worksheet.Cells[indexRow + i, 2].Value = "TỔNG CỘNG";
                worksheet.Cells[indexRow + i, 6].Value = tong_luongcoban;
                worksheet.Cells[indexRow + i, 7].Value = tong_phucap;
                worksheet.Cells[indexRow + i, 8].Value = tong_thunhapthoathuan;
                worksheet.Cells[indexRow + i, 9].Value = tong_songay;
                worksheet.Cells[indexRow + i, 11].Value = tong_tongthunhap;
                worksheet.Cells[indexRow + i, 12].Value = tong_bhxh;
                worksheet.Cells[indexRow + i, 13].Value = tong_bhyt;
                worksheet.Cells[indexRow + i, 14].Value = tong_congdoan; 
                worksheet.Cells[indexRow + i, 15].Value = tong_thatnghiep;
                worksheet.Cells[indexRow + i, 16].Value = tong_conlai;

                ExcelRange rt = worksheet.Cells[indexRow + i, 2, indexRow + i, 16];            
                rt.Style.Font.Bold = true;

                i++;

                for (int col = 1; col <= 21; col++)
                {
                    if (col != 2 && col != 9 && col != 20)
                    {
                        ExcelRange rcol = worksheet.Cells[9, col, indexRow + i - 1, col];
                        {
                            rcol.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        }
                    }
                }

                ExcelRange rcolP = worksheet.Cells[9, 33, indexRow + i - 1, 33];
                rcolP.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                ExcelRange rcolT = worksheet.Cells[9, 1, indexRow + i - 1, 1];
                rcolT.Style.Border.Left.Style = ExcelBorderStyle.Thin;


                using (ExcelRange rb = worksheet.Cells[indexRow + i - 1, 1, indexRow + i - 1, 33])
                {
                    rb.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }


                //3. END
                ExcelRange r = worksheet.Cells[indexRow + i + 2, 2, indexRow + i + 2, 3];
                r.Merge = true;
                r.Style.Font.Bold = true;
                r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                r.Value = "NGƯỜI LẬP BẢNG";

                ExcelRange r1 = worksheet.Cells[indexRow + i + 2, 6, indexRow + i + 2, 11];
                r1.Merge = true;
                r1.Style.Font.Bold = true;
                r1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                r1.Value = "TRƯỞNG PHÒNG HC-NS";

                ExcelRange r2 = worksheet.Cells[indexRow + i + 2, 12, indexRow + i + 2, 14];
                r2.Merge = true;
                r2.Style.Font.Bold = true;
                r2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                r2.Value = "KẾ TOÁN TRƯỞNG";

                ExcelRange r3 = worksheet.Cells[indexRow + i + 2, 16, indexRow + i + 2, 19];
                r3.Merge = true;
                r3.Style.Font.Bold = true;
                r3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                r3.Value = "TGĐ DUYỆT CHI";
                ExcelRange r4 = worksheet.Cells[indexRow + i + 1, 16, indexRow + i + 1, 19];
                r4.Merge = true;
                r4.Style.Font.Italic = true;
                r4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                r4.Value = "TpHCM, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm" + DateTime.Now.Year.ToString();


                //}
                //Switch the PageLayoutView back to normal
                worksheet.View.PageLayoutView = false;
                // save our new workbook and we are done!
                package.Save();
            }
            string absolutePath = newFile.FullName;// = _environment.WebRootPath + @"\" + relativePath;
            FileResult result = null;

            if (System.IO.File.Exists(absolutePath))
            {
                string fileName = System.IO.Path.GetFileName(absolutePath);
                byte[] fileBytes = System.IO.File.ReadAllBytes(absolutePath);
                result = File(fileBytes, "application/x-msdownload", fileName);
            }
            return result;
        }

        public FileResult DanhSachLuongKhoiVanPhong(int hrm_salaryID)
        {
            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = Server.MapPath("~/App_Data");

            FileInfo templateFile = new FileInfo(server + "//Mau_LuongVanPhong.xltx");
            FileInfo newFile = new FileInfo(outputDir.FullName + @"\LuongVanPhong.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\LuongVanPhong.xlsx");
            }
            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];


                //****************************************              
                //****************************************                
                int indexRow = 8;

                int i = 0;
                int stt = 1;
                int nhom = 1;

                var listvp = (from p in db.DIC_DEPARTMENT
                              where p.ParentID == 1
                              select p.DepartmentID).ToList();


                var listLuong = from p in db.viewLuongThangs
                                where p.ParentID == 1 && p.HRMSalaryID == hrm_salaryID
                                orderby p.ThuTuPhongBan, p.ThuTuChucVu
                                select p;

                int slhang = (from p in db.viewLuongThangs
                              where p.ParentID == 1 && p.HRMSalaryID == hrm_salaryID
                              orderby p.ThuTuPhongBan, p.ThuTuChucVu
                              select p).Count();

                string pb = "";
                int vitripb = indexRow;

                foreach (var luong in listLuong)
                {

                    worksheet.Cells[indexRow + i, 1].Value = stt;



                    var tenpb = (from p in db.DIC_DEPARTMENT
                                 where p.DepartmentID == luong.DepartmentID
                                 select p.DepartmentName).SingleOrDefault();

                    worksheet.Cells[indexRow + i, 2].Value = tenpb;

                    //Xử lý merge cell phòng ban
                    if (pb == "")
                    {
                        pb = tenpb;

                    }
                    else
                        if (pb != tenpb)
                    {
                        ExcelRange rpb = worksheet.Cells[vitripb, 2, indexRow + i - 1, 2];
                        rpb.Merge = true;
                        rpb.Style.WrapText = true;
                        rpb.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        pb = tenpb;
                        vitripb = i + indexRow;
                    }

                    if (i == slhang - 1)
                    {
                        ExcelRange rpb = worksheet.Cells[vitripb, 2, indexRow + i, 2];
                        rpb.Merge = true;
                        rpb.Style.WrapText = true;
                        rpb.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //vitripb = i + indexRow;
                    }

                    worksheet.Cells[indexRow + i, 3].Value = luong.FirstName;
                    worksheet.Cells[indexRow + i, 4].Value = luong.LastName;
                    worksheet.Cells[indexRow + i, 5].Value = luong.ChucVu;

                    if (luong.BankCode != null)
                        worksheet.Cells[indexRow + i, 6].Value = luong.BankCode;

                    if (luong.Salary != null)
                         worksheet.Cells[indexRow + i, 7].Value = luong.Salary.Value.ToString("#,##0");

                    if (luong.AllowanceSalary != null)
                        worksheet.Cells[indexRow + i, 8].Value = luong.AllowanceSalary.Value.ToString("#,##0");
                    if (luong.Bonus != null)
                    {
                        worksheet.Cells[indexRow + i, 9].Value = luong.Bonus.Value.ToString("#,##0");
                        worksheet.Cells[indexRow + i, 11].Value = luong.Bonus.Value.ToString("#,##0");
                    }

                    if (luong.AllowanceBonus != null)
                    {
                        worksheet.Cells[indexRow + i, 10].Value = luong.AllowanceBonus.Value.ToString("#,##0");
                        worksheet.Cells[indexRow + i, 12].Value = luong.AllowanceBonus.Value.ToString("#,##0");
                    }


                    //worksheet.Cells[indexRow + i, 9].Value = luong.NumOfDay.Value.ToString("#,##0");

                    if (luong.TongThuNhap != null)
                    {
                        worksheet.Cells[indexRow + i, 13].Value = luong.TongThuNhap.Value.ToString("#,##0");
                        worksheet.Cells[indexRow + i, 14].Value = luong.BHXH.Value.ToString("#,##0");
                        worksheet.Cells[indexRow + i, 15].Value = luong.BHYT.Value.ToString("#,##0");
                        worksheet.Cells[indexRow + i, 16].Value = luong.CongDoan.Value.ToString("#,##0");
                        worksheet.Cells[indexRow + i, 17].Value = luong.BHTN.Value.ToString("#,##0");

                    }
                    

                    if (luong.PhuThuoc > 0)
                        worksheet.Cells[indexRow + i, 19].Value = luong.PhuThuoc.Value.ToString("#,##0");
                    if (luong.TruKhac > 0)
                        worksheet.Cells[indexRow + i, 20].Value = luong.TruKhac.Value.ToString("#,##0");
                    if (luong.ThueTNCN > 0)
                        worksheet.Cells[indexRow + i, 21].Value = luong.ThueTNCN.Value.ToString("#,##0");

                    if(luong.ConLai != null)
                        worksheet.Cells[indexRow + i, 22].Value = luong.ConLai.Value.ToString("#,##0");

                    worksheet.Cells[indexRow + i, 24].Value = stt;
                    worksheet.Cells[indexRow + i, 25].Value = luong.FirstName.Trim() + " " + luong.LastName.Trim();
                    worksheet.Cells[indexRow + i, 27].Value = luong.ChucVu;
                    //Lấy tất cả quyết định trong tháng theo thứ tự đưa vào
                    var list_quyetdinh = from p in db.HRM_EMPLOYMENTHISTORY
                                         where p.EmployeeID == luong.EmployeeID && p.NgayXuongTau.Value.Year == luong.Years.Value && p.NgayXuongTau.Value.Month == luong.Months.Value && p.XacNhan == true
                                         orderby p.NgayXuongTau, p.EmploymentHistoryID
                                         select p;
                    String ghiChu = "";//"Theo QĐ số 357/17/QĐ-ĐĐ hiệu lực ngày 28/07/2017, Theo QĐ số 372/17/QĐ-ĐĐ hiệu lực ngày 28/07/2017"

                    if (list_quyetdinh != null)
                    {
                        foreach (HRM_EMPLOYMENTHISTORY quyetdinh in list_quyetdinh)
                        {
                            ghiChu += "Theo QĐ số " + quyetdinh.DecisionNo.ToString() + "/" + quyetdinh.NgayXuongTau.Value.Year.ToString() + "/QĐ-ĐĐ" + "hiệu lực ngày " + quyetdinh.NgayXuongTau.Value.ToString("dd/MM/yyyy") + " ";
                        }
                    }



                    worksheet.Cells[indexRow + i, 28].Value = ghiChu;

                    i++;
                    stt++;
                }

                for (int col = 1; col <= 27; col++)
                {
                    if (col != 3 && col != 25)
                    {
                        ExcelRange rcol = worksheet.Cells[indexRow, col, indexRow + i - 1, col];
                        {
                            rcol.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        }
                    }
                }

                ExcelRange rcolP = worksheet.Cells[indexRow, 37, indexRow + i - 1, 37];
                rcolP.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                ExcelRange rcolT = worksheet.Cells[indexRow, 1, indexRow + i - 1, 1];
                rcolT.Style.Border.Left.Style = ExcelBorderStyle.Thin;


                using (ExcelRange rb = worksheet.Cells[indexRow + i - 1, 1, indexRow + i - 1, 37])
                {
                    rb.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }


                //3. END
                ExcelRange r = worksheet.Cells[indexRow + i + 2, 2, indexRow + i + 2, 3];
                r.Merge = true;
                r.Style.Font.Bold = true;
                r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                r.Value = "NGƯỜI LẬP BẢNG";

                ExcelRange r1 = worksheet.Cells[indexRow + i + 2, 6, indexRow + i + 2, 11];
                r1.Merge = true;
                r1.Style.Font.Bold = true;
                r1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                r1.Value = "TRƯỞNG PHÒNG HC-NS";

                ExcelRange r2 = worksheet.Cells[indexRow + i + 2, 12, indexRow + i + 2, 14];
                r2.Merge = true;
                r2.Style.Font.Bold = true;
                r2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                r2.Value = "KẾ TOÁN TRƯỞNG";

                ExcelRange r3 = worksheet.Cells[indexRow + i + 2, 16, indexRow + i + 2, 19];
                r3.Merge = true;
                r3.Style.Font.Bold = true;
                r3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                r3.Value = "TGĐ DUYỆT CHI";
                ExcelRange r4 = worksheet.Cells[indexRow + i + 1, 16, indexRow + i + 1, 19];
                r4.Merge = true;
                r4.Style.Font.Italic = true;
                r4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                r4.Value = "TpHCM, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm" + DateTime.Now.Year.ToString();


                //}
                //Switch the PageLayoutView back to normal
                worksheet.View.PageLayoutView = false;
                // save our new workbook and we are done!
                package.Save();
            }
            string absolutePath = newFile.FullName;// = _environment.WebRootPath + @"\" + relativePath;
            FileResult result = null;

            if (System.IO.File.Exists(absolutePath))
            {
                string fileName = System.IO.Path.GetFileName(absolutePath);
                byte[] fileBytes = System.IO.File.ReadAllBytes(absolutePath);
                result = File(fileBytes, "application/x-msdownload", fileName);
            }
            return result;
        }

        public FileResult DanhSachLuongDuTru(int departmentID, int hrm_salaryID)
        {
            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = Server.MapPath("~/App_Data");

            FileInfo templateFile = new FileInfo(server + "//Mau_LuongTau.xltx");
            FileInfo newFile = new FileInfo(outputDir.FullName + @"\LuongTau.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\LuongTau.xlsx");
            }

            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];


                //****************************************              
                //****************************************                
                int indexRow = 9;

                int i = 0;
                int stt = 1;
                int nhom = 1;

                //var listLuong = db.sp_Luong_3(hrm_salaryID).Where(x => x.DepartmentLuongID == departmentID).ToList().OrderBy(x => x.DepartmentLuongID).ThenBy(x => x.ThuTuChucVu);
                var listLuong = from p in db.viewLuongThangs
                                where p.DepartmentLuongID == departmentID && p.HRMSalaryID == hrm_salaryID
                                orderby p.ThuTuPhongBan, p.ThuTuChucVu
                                select p;

                int parentID = Convert.ToInt32((from p in db.DIC_DEPARTMENT
                                                where p.DepartmentID == departmentID
                                                select p.ParentID).FirstOrDefault());

                if (parentID == 17)
                    listLuong = from p in db.viewLuongThangs
                                where p.DepartmentLuongID == 1000 && p.HRMSalaryID == hrm_salaryID
                                orderby p.ThuTuChucVu
                                select p;


                //var listLuong = from p in db.sp_Luong_3(hrm_salaryID)
                //                where p.DepartmentSalaryID == departmentID && p.HRMSalaryID == hrm_salaryID
                //                orderby p.DepartmentLuongID, p.PositionID
                //                select p;

                var tentau = (from p in db.DIC_DEPARTMENT
                              where p.DepartmentID == departmentID
                              select p.DepartmentName).SingleOrDefault();
                worksheet.Cells[2, 1].Value = "BẢNG LƯƠNG TÀU " + tentau.ToUpper();

                var obj_salary = (from p in db.HRM_SALARY
                                  where p.HRMSalaryID == hrm_salaryID
                                  select p).SingleOrDefault();
                if(obj_salary.Months <10)
                    worksheet.Cells[3, 1].Value = "THÁNG 0" + obj_salary.Months.ToString() +"/" + obj_salary.Years.ToString() + worksheet.Cells[3, 1].Value.ToString();
                else
                    worksheet.Cells[3, 1].Value = "THÁNG " + obj_salary.Months.ToString() + "/" + obj_salary.Years.ToString() + worksheet.Cells[3, 1].Value.ToString();
                
                double tong_tongthunhap = 0;
                double tong_luongcoban = 0;
                double tong_phucap = 0;
                double tong_thunhapthoathuan = 0;
                double tong_songay = 0;
                double tong_bhxh = 0;
                double tong_bhyt = 0;
                double tong_congdoan = 0;
                double tong_thatnghiep = 0;
                double tong_conlai = 0;

                int m_departmentID = 0;

                foreach (var luong in listLuong)
                {
                    m_departmentID = Convert.ToInt32(luong.DepartmentID);

                    worksheet.Cells[indexRow + i, 1].Value = stt;
                    worksheet.Cells[indexRow + i, 2].Value = luong.FirstName;
                    worksheet.Cells[indexRow + i, 3].Value = luong.LastName;
                    worksheet.Cells[indexRow + i, 4].Value = luong.ChucVu;
                    if (luong.BankCode != null)
                        worksheet.Cells[indexRow + i, 5].Value = luong.BankCode;

                    worksheet.Cells[indexRow + i, 6].Value = luong.Salary.Value;
                    tong_luongcoban += luong.Salary.Value;

                    if (luong.AllowanceSalary > 0)
                    {
                        worksheet.Cells[indexRow + i, 7].Value = luong.AllowanceSalary;
                        tong_phucap += luong.AllowanceSalary.Value;
                    }

                    if (luong.Bonus > 0)
                    {
                        worksheet.Cells[indexRow + i, 8].Value = luong.Bonus.Value;

                        tong_thunhapthoathuan += luong.Bonus.Value;
                    }


                    worksheet.Cells[indexRow + i, 9].Value = luong.NumOfDay.Value;
                    tong_songay += luong.NumOfDay.Value;

                    worksheet.Cells[indexRow + i, 11].Value = luong.TongThuNhap.Value;
                    tong_tongthunhap += luong.TongThuNhap.Value;

                    worksheet.Cells[indexRow + i, 12].Value = luong.BHXH.Value;
                    tong_bhxh += luong.BHXH.Value;

                    worksheet.Cells[indexRow + i, 13].Value = luong.BHYT.Value;
                    tong_bhyt += luong.BHYT.Value;

                    worksheet.Cells[indexRow + i, 14].Value = luong.CongDoan.Value;
                    tong_congdoan += luong.CongDoan.Value;                

                    worksheet.Cells[indexRow + i, 15].Value = luong.BHTN.Value;
                    tong_thatnghiep += luong.BHTN.Value;

                    worksheet.Cells[indexRow + i, 16].Value = luong.ConLai.Value;
                    tong_conlai += luong.ConLai.Value;

                    //worksheet.Cells[indexRow + i, 17].Value = luong.TongThuNhap;
                    worksheet.Cells[indexRow + i, 18].Value = stt;
                    worksheet.Cells[indexRow + i, 19].Value = luong.FirstName.Trim() + " " + luong.LastName.Trim();
                    worksheet.Cells[indexRow + i, 21].Value = luong.ChucVu;
                    //Lấy tất cả quyết định trong tháng theo thứ tự đưa vào
                    var list_quyetdinh = from p in db.HRM_EMPLOYMENTHISTORY
                                         where p.EmployeeID == luong.EmployeeID && p.NgayXuongTau.Value.Year == luong.Years.Value && p.NgayXuongTau.Value.Month == luong.Months.Value && p.XacNhan == true
                                         orderby p.NgayXuongTau, p.EmploymentHistoryID
                                         select p;
                    String ghiChu = "";//"Theo QĐ số 357/17/QĐ-ĐĐ hiệu lực ngày 28/07/2017, Theo QĐ số 372/17/QĐ-ĐĐ hiệu lực ngày 28/07/2017"

                    if (list_quyetdinh != null)
                    {
                        foreach (HRM_EMPLOYMENTHISTORY quyetdinh in list_quyetdinh)
                        {
                            ghiChu += "Theo QĐ số " + quyetdinh.DecisionNo.ToString() + "/" + quyetdinh.NgayXuongTau.Value.Year.ToString() + "/QĐ-ĐĐ" + " hiệu lực ngày " + quyetdinh.NgayXuongTau.Value.ToString("dd/MM/yyyy") + " ";
                        }
                    }

                    worksheet.Cells[indexRow + i, 22].Value = ghiChu;
                    if (luong.NumOfDay != luong.TotalDay)
                    {
                        ExcelRange rd = worksheet.Cells[indexRow + i, 2, indexRow + i, 22];
                        rd.Style.Font.Color.SetColor(System.Drawing.Color.Red);
                    }


                    i++;
                    stt++;
                }
                // Tong cong
                i++;

                worksheet.Cells[indexRow + i, 2].Value = "TỔNG CỘNG";
                worksheet.Cells[indexRow + i, 6].Value = tong_luongcoban;
                worksheet.Cells[indexRow + i, 7].Value = tong_phucap;
                worksheet.Cells[indexRow + i, 8].Value = tong_thunhapthoathuan;
                worksheet.Cells[indexRow + i, 9].Value = tong_songay;
                worksheet.Cells[indexRow + i, 11].Value = tong_tongthunhap;
                worksheet.Cells[indexRow + i, 12].Value = tong_bhxh;
                worksheet.Cells[indexRow + i, 13].Value = tong_bhyt;
                worksheet.Cells[indexRow + i, 14].Value = tong_congdoan;
                worksheet.Cells[indexRow + i, 15].Value = tong_thatnghiep;
                worksheet.Cells[indexRow + i, 16].Value = tong_conlai;

                ExcelRange rt = worksheet.Cells[indexRow + i, 2, indexRow + i, 16];
                rt.Style.Font.Bold = true;

                i++;

                for (int col = 1; col <= 21; col++)
                {
                    if (col != 2 && col != 9 && col != 20)
                    {
                        ExcelRange rcol = worksheet.Cells[9, col, indexRow + i - 1, col];
                        {
                            rcol.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        }
                    }
                }

                ExcelRange rcolP = worksheet.Cells[9, 33, indexRow + i - 1, 33];
                rcolP.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                ExcelRange rcolT = worksheet.Cells[9, 1, indexRow + i - 1, 1];
                rcolT.Style.Border.Left.Style = ExcelBorderStyle.Thin;


                using (ExcelRange rb = worksheet.Cells[indexRow + i - 1, 1, indexRow + i - 1, 33])
                {
                    rb.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }


                //3. END
                ExcelRange r = worksheet.Cells[indexRow + i + 2, 2, indexRow + i + 2, 3];
                r.Merge = true;
                r.Style.Font.Bold = true;
                r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                r.Value = "NGƯỜI LẬP BẢNG";

                ExcelRange r1 = worksheet.Cells[indexRow + i + 2, 6, indexRow + i + 2, 11];
                r1.Merge = true;
                r1.Style.Font.Bold = true;
                r1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                r1.Value = "TRƯỞNG PHÒNG HC-NS";

                ExcelRange r2 = worksheet.Cells[indexRow + i + 2, 12, indexRow + i + 2, 14];
                r2.Merge = true;
                r2.Style.Font.Bold = true;
                r2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                r2.Value = "KẾ TOÁN TRƯỞNG";

                ExcelRange r3 = worksheet.Cells[indexRow + i + 2, 16, indexRow + i + 2, 19];
                r3.Merge = true;
                r3.Style.Font.Bold = true;
                r3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                r3.Value = "TGĐ DUYỆT CHI";
                ExcelRange r4 = worksheet.Cells[indexRow + i + 1, 16, indexRow + i + 1, 19];
                r4.Merge = true;
                r4.Style.Font.Italic = true;
                r4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                r4.Value = "TpHCM, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm" + DateTime.Now.Year.ToString();


                //}
                //Switch the PageLayoutView back to normal
                worksheet.View.PageLayoutView = false;
                // save our new workbook and we are done!
                package.Save();
            }
            string absolutePath = newFile.FullName;// = _environment.WebRootPath + @"\" + relativePath;
            FileResult result = null;

            if (System.IO.File.Exists(absolutePath))
            {
                string fileName = System.IO.Path.GetFileName(absolutePath);
                byte[] fileBytes = System.IO.File.ReadAllBytes(absolutePath);
                result = File(fileBytes, "application/x-msdownload", fileName);
            }
            return result;
        }

    }
}
