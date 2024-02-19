using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAuLac.Models;
using System.IO;
using System.Web.UI.WebControls;
using System.Text;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Security.Principal;
using Novacode;

namespace HRM.Controllers
{
    [Authorize]
    public class XuatWordExcelController : Controller
    {
        private AuLacEntities db = new AuLacEntities();
        private string _serverPath;
        private IPrincipal _user;

        public string ServerPath
        {
            get
            {
                return _serverPath;
            }
            set
            {
                _serverPath = value;
            }
        }

        public IPrincipal AppUser
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        public FileResult XuatDanhSachThuyenVien(List<viewNhanVien> dsThuyenVien)
        {
            //IQueryable<viewHRM_EMPLOYMENTHISTORY> ds
            //int departmentID = 10; 

            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = ServerPath;
            FileInfo templateFile;
            FileInfo newFile;
            templateFile = new FileInfo(server + "//Mau_DSTV.xltx");
            newFile = new FileInfo(outputDir.FullName + @"\DSTV.xlsx");

            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\DSTV.xlsx");
            }

            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                worksheet.Cells[2, 12].Value = "Cập nhật ngày " + DateTime.Now.ToString("dd/MM/yyyy");

                //****************************************
                //2. NOI DUNG
                int i = 4;
                int stt = 1;
                int departmentID = 0;

                var listTV = dsThuyenVien;

                foreach (var tv in listTV)
                {
                    if (departmentID != tv.DepartmentID || departmentID == 0)
                    {
                        stt = 1;
                        worksheet.Cells[i, 1].Value = tv.DepartmentName.ToUpper(); //"DANH SÁCH THUYỀN VIÊN "
                        departmentID = Convert.ToInt32(tv.DepartmentID);
                        using (var range = worksheet.Cells[i, 1, i, 12])
                        {
                            // Format text đỏ và đậm
                            range.Style.Font.Color.SetColor(System.Drawing.Color.Red);
                            range.Style.Font.Size = 18;
                            range.Style.Font.Bold = true;
                            range.Merge = true;
                            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }

                        i++;
                    }

                    worksheet.Cells[i, 1].Value = stt;
                    stt++;

                    worksheet.Cells[i, 2].Value = tv.FirstName + ' ' + tv.LastName;

                    if (tv.QuanHeID != null) //&& AppUser.IsInRole("Boss")
                        worksheet.Cells[i, 3].Value = tv.QuanHeID;

                    if (tv.ChucVu != null)
                        worksheet.Cells[i, 4].Value = tv.ChucVu;

                    if (tv.BirthDay != null)
                        worksheet.Cells[i, 5].Value = tv.BirthDay.Value.ToString("dd/MM/yy");

                    //6.GCNKNCM
                    if (tv.Qualification != null)
                        worksheet.Cells[i, 6].Value = tv.Qualification;

                    //7. TDCM
                    if (tv.EducationName != null)
                        worksheet.Cells[i, 7].Value = tv.EducationName;

                    //8. NgayXuongTau
                    if (tv.NgayXuongTau.HasValue)
                        worksheet.Cells[i, 8].Value = tv.NgayXuongTau.Value.ToString("dd/MM/yy");

                    //9. So ngay tai tau
                    if (tv.TGDT != null)
                        worksheet.Cells[i, 9].Value = tv.TGDT;

                    //10. Tau da cong tac
                    if (tv.QTDT != null)
                        worksheet.Cells[i, 10].Value = tv.QTDT;

                    //11. Ghi chu
                    if (tv.NoteQTCT != null)
                        worksheet.Cells[i, 11].Value = tv.NoteQTCT;

                    //12. Ghi chu SSDD
                    if (tv.GhiChuSSDD != null)
                        worksheet.Cells[i, 12].Value = tv.GhiChuSSDD;

                    var thoigianlamviec = (from p in db.sp_ThoiGianLamViecTaiCongTy(tv.EmployeeID)
                                           select p).FirstOrDefault();
                    //13. NgayHopDong
                    if (thoigianlamviec.NgayHopDong != null)
                        worksheet.Cells[i, 13].Value = thoigianlamviec.NgayHopDong.Value.ToString("dd/MM/yy");

                    //14. ThoiGianLamViecTaiCongTy
                    worksheet.Cells[i, 14].Value = thoigianlamviec.ThoiGianTaiCongTy;

                    //15. DiaChiThuongTru
                    if (tv.MainAddress != null)
                        worksheet.Cells[i, 15].Value = tv.MainAddress;

                    //16.DiaChiLienLac
                    if (tv.ContactAddress != null)
                        worksheet.Cells[i, 16].Value = tv.ContactAddress;

                    //17. DienThoai
                    if (tv.CellPhone != null)
                        worksheet.Cells[i, 17].Value = tv.CellPhone;

                    //18. DienThoaiNguoiThan
                    if (tv.SDTNguoiThan != null)
                        worksheet.Cells[i, 18].Value = tv.SDTNguoiThan;

                    //19. DS Nguoi Than
                    var listNguoiThan = from p in db.HRM_EMPLOYEE_RELATIVE
                                        where p.EmployeeID == tv.EmployeeID
                                        select p;

                    String dsNguoiThan = "";
                    foreach (HRM_EMPLOYEE_RELATIVE nguoithan in listNguoiThan)
                    {
                        String loaiQH = (from p in db.DIC_RELATIVE
                                         where p.RelativeID == nguoithan.RelativeID
                                         select p.RelativeName).SingleOrDefault();
                        if(nguoithan.Phone != null)
                            dsNguoiThan = loaiQH + ": " + nguoithan.PersonName + " - " + nguoithan.Phone + "\n";
                        else
                            dsNguoiThan = loaiQH + ": " + nguoithan.PersonName + "\n";
                    }
                    worksheet.Cells[i, 19].Value = dsNguoiThan;

                    i++;
                }

                using (ExcelRange r = worksheet.Cells[3, 1, i - 1, 14])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

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

        public FileResult XuatDanhSachThuyenVien_MauDung(List<viewNhanVien> dsThuyenVien)
        {
            //IQueryable<viewHRM_EMPLOYMENTHISTORY> ds
            //int departmentID = 10;
            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = ServerPath;
            FileInfo templateFile;
            FileInfo newFile;
            templateFile = new FileInfo(server + "//Mau_DSTVDung.xltx");
            newFile = new FileInfo(outputDir.FullName + @"\DSTVDung.xlsx");

            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\DSTVDung.xlsx");
            }

            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                worksheet.Cells[2, 10].Value = "Cập nhật ngày " + DateTime.Now.ToString("dd/MM/yyyy");

                //****************************************
                //2. NOI DUNG
                int i = 4;
                int stt = 1;
                int departmentID = 0;

                var listTV = dsThuyenVien;

                foreach (var tv in listTV)
                {
                    if (departmentID != tv.DepartmentID || departmentID == 0)
                    {
                        stt = 1;
                        worksheet.Cells[i, 1].Value = tv.DepartmentName.ToUpper(); //"DANH SÁCH THUYỀN VIÊN "
                        departmentID = Convert.ToInt32(tv.DepartmentID);
                        using (var range = worksheet.Cells[i, 1, i, 10])
                        {
                            // Format text đỏ và đậm
                            range.Style.Font.Color.SetColor(System.Drawing.Color.Red);
                            range.Style.Font.Size = 18;
                            range.Style.Font.Bold = true;
                            range.Merge = true;
                            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }

                        i++;
                    }

                    worksheet.Cells[i, 1].Value = stt;
                    stt++;

                    worksheet.Cells[i, 2].Value = tv.FirstName + ' ' + tv.LastName;
                    if (tv.QuanHeID != null && AppUser.IsInRole("Boss"))
                        worksheet.Cells[i, 3].Value = tv.QuanHeID;
                    if (tv.ChucVu != null)
                        worksheet.Cells[i, 4].Value = tv.ChucVu;
                    if (tv.BirthDay != null)
                        worksheet.Cells[i, 5].Value = tv.BirthDay.Value.ToString("dd/MM/yy");
                    //6.GCNKNCM
                    if (tv.Qualification != null)
                        worksheet.Cells[i, 6].Value = tv.Qualification;
                    //7. TDCM
                    if (tv.EducationName != null)
                        worksheet.Cells[i, 7].Value = tv.EducationName;
                    //8. NgayXuongTau
                    if (tv.NgayXuongTau.HasValue)
                        worksheet.Cells[i, 8].Value = tv.NgayXuongTau.Value.ToString("dd/MM/yy");
                    //9. So ngay tai tau
                    worksheet.Cells[i, 9].Value = tv.TGDT;
                    //10. Tau da cong tac
                    worksheet.Cells[i, 10].Value = tv.QTDT;

                    i++;
                }

                using (ExcelRange r = worksheet.Cells[4, 1, i - 1, 10])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

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

        public FileResult XuatDanhSachThuyenVien_MauNgang(int[] listEmployeeID)
        {
            //IQueryable<viewHRM_EMPLOYMENTHISTORY> ds
            //int departmentID = 10;
            List<viewNhanVien> dsThuyenVien = (from p in db.viewNhanViens
                                               where listEmployeeID.Contains(p.EmployeeID)
                                               select p).ToList();


            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = ServerPath;
            FileInfo templateFile;
            FileInfo newFile;
            templateFile = new FileInfo(server + "//Mau_DSTV_Ngang.xltx");
            newFile = new FileInfo(outputDir.FullName + @"\DSTV_Ngang.xlsx");

            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\DSTV_Ngang.xlsx");
            }

            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                worksheet.Cells[2, 12].Value = "Cập nhật ngày " + DateTime.Now.ToString("dd/MM/yyyy");

                //****************************************
                //2. NOI DUNG
                int i = 4;
                int stt = 1;
                int departmentID = 0;

                var listTV = dsThuyenVien;

                foreach (var tv in listTV)
                {
                    worksheet.Cells[i, 1].Value = stt;
                    stt++;

                    worksheet.Cells[i, 2].Value = tv.FirstName + ' ' + tv.LastName;

                    if (tv.QuanHeID != null) //&& AppUser.IsInRole("Boss")
                        worksheet.Cells[i, 3].Value = tv.QuanHeID;

                    if (tv.DepartmentID != null)
                        worksheet.Cells[i, 4].Value = tv.DepartmentName;

                    if (tv.ChucVu != null)
                        worksheet.Cells[i, 5].Value = tv.ChucVu;

                    if (tv.BirthDay != null)
                        worksheet.Cells[i, 6].Value = tv.BirthDay.Value.ToString("dd/MM/yy");

                    //6.GCNKNCM
                    if (tv.Qualification != null)
                        worksheet.Cells[i, 7].Value = tv.Qualification;

                    //7. TDCM
                    if (tv.EducationName != null)
                        worksheet.Cells[i, 8].Value = tv.EducationName;

                    //8. NgayXuongTau
                    if (tv.NgayXuongTau.HasValue)
                        worksheet.Cells[i, 9].Value = tv.NgayXuongTau.Value.ToString("dd/MM/yy");

                    //9. So ngay tai tau
                    if (tv.TGDT != null)
                        worksheet.Cells[i, 10].Value = tv.TGDT;

                    //10. Tau da cong tac
                    if (tv.QTDT != null)
                        worksheet.Cells[i, 11].Value = tv.QTDT;

                    //11. Ghi chu
                    if (tv.NoteQTCT != null)
                        worksheet.Cells[i, 12].Value = tv.NoteQTCT;

                    //12. Ghi chu SSDD

                    if (tv.GhiChuSSDD != null)
                        worksheet.Cells[i, 13].Value = tv.GhiChuSSDD;

                    var thoigianlamviec = (from p in db.sp_ThoiGianLamViecTaiCongTy(tv.EmployeeID)
                                           select p).FirstOrDefault();
                    //13. NgayHopDong
                    if (thoigianlamviec.NgayHopDong != null)
                        worksheet.Cells[i, 14].Value = thoigianlamviec.NgayHopDong.Value.ToString("dd/MM/yy");

                    //14. ThoiGianLamViecTaiCongTy
                    worksheet.Cells[i, 15].Value = thoigianlamviec.ThoiGianTaiCongTy;
                    i++;
                }

                using (ExcelRange r = worksheet.Cells[4, 1, i - 1, 15])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

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

        public FileResult XuatDanhSachNhanVien(List<sp_T_LayDanhSachNhanVien_Result> dsNhanVien)
        {
            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = ServerPath;

            FileInfo templateFile = new FileInfo(server + "//Mau_LLTN.xltx");
            FileInfo newFile = new FileInfo(outputDir.FullName + @"\LLTN.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\LLTN.xlsx");
            }
            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xu?t d? li?u
                for (int sheet = 1; sheet <= 2; sheet++)
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[sheet];

                    //****************************************
                    //2. NOI DUNG - KHỐI VĂN PHÒNG
                    int i = 4;
                    int stt = 1;
                    //int departmentID = 0;

                    IEnumerable<sp_T_LayDanhSachNhanVien_Result> listTV;

                    if (sheet == 1)
                        listTV = dsNhanVien.Where(x => x.ParentDepartment == 1).OrderBy(x => x.DepartmentID).ThenBy(x => x.PositionID);
                    else
                        listTV = dsNhanVien.Where(x => x.ParentDepartment == 8 || x.ParentDepartment == 17).OrderBy(x => x.DepartmentID).ThenBy(x => x.PositionID);


                    foreach (var item in listTV)
                    {

                        worksheet.Cells[i, 1].Value = stt;
                        stt++;
                        HRM_EMPLOYEE tv = db.HRM_EMPLOYEE.Where(x => x.EmployeeID == item.EmployeeID).FirstOrDefault();
                        if (tv != null)
                        {
                            if (tv.BirthPlace != null)
                                worksheet.Cells[i, 4].Value = tv.BirthPlace;
                            if (tv.Origin != null)
                                worksheet.Cells[i, 5].Value = tv.Origin;
                            if (tv.IDCard != null)
                                worksheet.Cells[i, 6].Value = tv.IDCard;
                            if (tv.IDCardDate != null)
                                worksheet.Cells[i, 7].Value = tv.IDCardDate.Value.ToString("dd/MM/yyyy");
                            if (tv.IDCardPlace != null)
                                worksheet.Cells[i, 8].Value = tv.IDCardPlace;
                            if (tv.MainAddress != null && (AppUser.IsInRole("HR") || AppUser.IsInRole("Boss")))
                                worksheet.Cells[i, 9].Value = tv.MainAddress;
                            if (tv.EducationID != null)
                                if (tv.SchoolID != null)
                                    worksheet.Cells[i, 11].Value = tv.DIC_EDUCATION.EducationName + "\n" + tv.DIC_SCHOOL.SchoolName;
                                else
                                    worksheet.Cells[i, 11].Value = tv.DIC_EDUCATION.EducationName;
                            if (tv.Qualification != null)
                                worksheet.Cells[i, 12].Value = tv.Qualification;
                            if (tv.CellPhone != null)
                                worksheet.Cells[i, 15].Value = tv.CellPhone;
                        }
                        worksheet.Cells[i, 2].Value = item.FirstName + ' ' + item.LastName;
                        if (item.BirthDay != null)
                            worksheet.Cells[i, 3].Value = item.BirthDay.Value.ToString("dd/MM/yyyy");

                        if (item.ContactAddress != null && (AppUser.IsInRole("HR") || AppUser.IsInRole("Boss")))
                            worksheet.Cells[i, 10].Value = item.ContactAddress;

                        if (item.ChucVu != null)
                            worksheet.Cells[i, 13].Value = item.ChucVu;

                        HRM_CONTRACTHISTORY hopDong = db.HRM_CONTRACTHISTORY.Where(x => x.EmployeeID == item.EmployeeID).FirstOrDefault();
                        if (hopDong != null)
                        {
                            worksheet.Cells[i, 14].Value = hopDong.ContractDate.Value.ToString("dd/MM/yyyy");
                        }

                        //18. DienThoaiNguoiThan
                        if (tv.SDTNguoiThan != null)
                            worksheet.Cells[i, 18].Value = tv.SDTNguoiThan;

                        //19. DS Nguoi Than
                        var listNguoiThan = from p in db.HRM_EMPLOYEE_RELATIVE
                                            where p.EmployeeID == tv.EmployeeID
                                            orderby p.RelativeID
                                            select p;

                        String dsNguoiThan = "";
                        foreach (HRM_EMPLOYEE_RELATIVE nguoithan in listNguoiThan)
                        {
                            String loaiQH = (from p in db.DIC_RELATIVE
                                             where p.RelativeID == nguoithan.RelativeID
                                             select p.RelativeName).SingleOrDefault();
                            string NoiO = "";
                            string congviec = "";

                            if (nguoithan.Address != null)
                                NoiO = nguoithan.Address;
                            if(nguoithan.Job!= null)
                                congviec = nguoithan.Job;

                            if (nguoithan.Phone != null)
                                if(nguoithan.Birthday != null)
                                    dsNguoiThan += loaiQH + ": " + nguoithan.PersonName + " - " + nguoithan.Birthday + " - " + nguoithan.Phone + " - " + NoiO + " - " + congviec + "\n";
                                else
                                    dsNguoiThan += loaiQH + ": " + nguoithan.PersonName + " - " + nguoithan.Phone + " - " + NoiO + " - " + congviec + "\n";
                            else
                                if (nguoithan.Birthday != null)
                                    dsNguoiThan += loaiQH + ": " + nguoithan.PersonName + " - " + nguoithan.Birthday + " - " + NoiO + " - " + congviec + "\n";
                                else
                                    dsNguoiThan += loaiQH + ": " + nguoithan.PersonName + " - " + NoiO + " - " + congviec + "\n";
                        }
                        if(dsNguoiThan.Length >0)
                            worksheet.Cells[i, 19].Value = dsNguoiThan.Substring(0,dsNguoiThan.Length -1);

                        //20. DS Bang cap
                        var listBangCap = from p in db.HRM_EMPLOYEE_DEGREE
                                            where p.EmployeeID == tv.EmployeeID                                            
                                            select p;

                        String dsBangCap = "";

                        foreach (HRM_EMPLOYEE_DEGREE bangcap in listBangCap)
                        {
                            String degreeName = (from p in db.DIC_DEGREE
                                             where p.DegreeID == bangcap.DegreeID
                                             select p.DegreeName).SingleOrDefault();

                            String truongName = (from p in db.DIC_SCHOOL
                                                 where p.SchoolID == bangcap.SchoolID
                                                 select p.SchoolName).SingleOrDefault();

                            string SoBang = "";
                            string ngaycap = "";
                            string ngayhethan = "";


                            if (bangcap.DegreeNo != null)
                                SoBang = bangcap.DegreeNo;

                            if (bangcap.DegreeDate != null)
                                ngaycap = bangcap.DegreeDate.Value.ToString("dd/MM/yyyy");

                            if (bangcap.ExpirationDate != null)
                                ngayhethan = bangcap.ExpirationDate.Value.ToString("dd/MM/yyyy");

                            dsBangCap += degreeName + " - " + truongName + " - " + SoBang + " - " + ngaycap + " - " + ngayhethan + "\n";                            
                        }

                        if (dsBangCap.Length > 0)
                            worksheet.Cells[i, 20].Value = dsBangCap.Substring(0, dsBangCap.Length - 1);

                        //HRM_EMPLOYEE_DEGREE empassport = (from p in db.HRM_EMPLOYEE_DEGREE
                        //                            where p.EmployeeID == tv.EmployeeID && p.DegreeID == 9
                        //                            select p).SingleOrDefault();

                        //if (empassport.DegreeNo != null)
                        //    worksheet.Cells[i, 16].Value = empassport.DegreeNo;
                        //if (empassport.DegreeDate != null)
                        //    worksheet.Cells[i, 17].Value = empassport.DegreeDate.Value.ToString("dd/MM/yyyy");

                        //HRM_EMPLOYEE_DEGREE emseapassport = (from p in db.HRM_EMPLOYEE_DEGREE
                        //                                  where p.EmployeeID == tv.EmployeeID && p.DegreeID == 7
                        //                                  select p).SingleOrDefault();

                        //if (emseapassport != null)
                        //{                        
                        //if(emseapassport.DegreeNo != null)
                        //    worksheet.Cells[i, 18].Value = emseapassport.DegreeNo;
                        //if (emseapassport.DegreeDate != null)
                        //    worksheet.Cells[i, 19].Value = emseapassport.DegreeDate.Value.ToString("dd/MM/yyyy");
                        //}

                        //HRM_EMPLOYEE_DEGREE emseabook = (from p in db.HRM_EMPLOYEE_DEGREE
                        //                                     where p.EmployeeID == tv.EmployeeID && p.DegreeID == 8
                        //                                     select p).SingleOrDefault();
                        //if (emseabook.DegreeNo != null)                      
                        //    worksheet.Cells[i, 20].Value = emseabook.DegreeNo;
                        //if (emseabook.DegreeDate != null)
                        //    worksheet.Cells[i, 21].Value = emseabook.DegreeDate.Value.ToString("dd/MM/yyyy");

                        //if(tv.ReligionID != null)
                        //    worksheet.Cells[i, 22].Value = tv.DIC_RELIGION.ReligionName;
                        //if (tv.EthnicID != null)
                        //    worksheet.Cells[i, 23].Value = tv.DIC_ETHNIC.EthnicName;


                        worksheet.Row(i).Height = 55;
                        i++;
                    }

                    using (ExcelRange r = worksheet.Cells[3, 1, i - 1, 15])
                    {
                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }

                    worksheet.View.PageLayoutView = false;
                    //                    
                }



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

        public FileResult XuatDanhSachNhanVienTongHop(List<sp_T_LayDanhSachNhanVien_Result> dsNhanVien)
        {
            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = ServerPath;

            FileInfo templateFile = new FileInfo(server + "//Mau_NhanVienTongHop.xltx");
            FileInfo newFile = new FileInfo(outputDir.FullName + @"\NhanVienTongHop.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\NhanVienTongHop.xlsx");
            }
            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xu?t d? li?u
                for (int sheet = 1; sheet <= 1; sheet++)
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[sheet];

                    //****************************************
                    //2. NOI DUNG - KHỐI VĂN PHÒNG
                    int i = 5;
                    int stt = 1;
                    //int departmentID = 0;

                    //IEnumerable<sp_T_LayDanhSachNhanVien_Result> listTV;

                    //if (sheet == 1)
                    //    listTV = dsNhanVien.Where(x => x.ParentDepartment == 1).OrderBy(x => x.DepartmentID).ThenBy(x => x.PositionID);
                    //else
                    //    listTV = dsNhanVien.Where(x => x.ParentDepartment == 8 || x.ParentDepartment == 17).OrderBy(x => x.DepartmentID).ThenBy(x => x.PositionID);

                    var listIDNhanVien = (from p in dsNhanVien
                                         select (int?)p.EmployeeID).ToList();                    

                    var listTV = from p in db.viewNhanVienTongHops
                                 where listIDNhanVien.Contains(p.EmployeeID)
                                 orderby p.ThuTuPhongBan, p.PositionID
                                 select p;

                    foreach (var item in listTV)
                    {

                        worksheet.Cells[i, 1].Value = stt;
                        stt++;
                        HRM_EMPLOYEE tv = db.HRM_EMPLOYEE.Where(x => x.EmployeeID == item.EmployeeID).FirstOrDefault();
                        if (tv != null)
                        {
                            worksheet.Cells[i, 2].Value = item.FirstName;
                            worksheet.Cells[i, 3].Value = item.LastName;
                            worksheet.Cells[i, 4].Value = item.DepartmentName;
                            worksheet.Cells[i, 5].Value = item.ChucVu;

                            if (item.Sex != null)
                                if (item.Sex == true)
                                    worksheet.Cells[i, 6].Value = "Nam";
                                else
                                    worksheet.Cells[i, 6].Value = "Nữ";

                            if (item.QuanHeID != null)
                                worksheet.Cells[i, 7].Value = item.QuanHeID;

                            if (item.BirthDay != null)
                                worksheet.Cells[i, 8].Value = item.BirthDay.Value.ToString("dd/MM/yyyy");

                            if (item.BirthPlace != null)
                                worksheet.Cells[i, 9].Value = item.BirthPlace;

                            if (item.KhuVucID != null)
                                worksheet.Cells[i, 10].Value = item.TenKhuVuc;

                            if (item.Origin != null)
                                worksheet.Cells[i, 11].Value = item.Origin;
                            if (item.OriginQuanHuyen != null)
                                worksheet.Cells[i, 12].Value = item.OriginQuanHuyen;
                            if (item.OriginTinhThanhPho != null)
                                worksheet.Cells[i, 13].Value = item.OriginTinhThanhPho;

                            if (item.MainAddress != null)
                                worksheet.Cells[i, 14].Value = item.ContactAddress;
                            if (item.MainAddressQuanHuyen != null)
                                worksheet.Cells[i, 15].Value = item.ContactAddressQuanHuyen;
                            if (item.ContactAddressTinhThanhPho != null)
                                worksheet.Cells[i, 16].Value = item.ContactAddressTinhThanhPho;

                            if (item.ContactAddress != null)
                                worksheet.Cells[i, 17].Value = item.ContactAddress;
                            if (item.ContactAddressQuanHuyen != null)
                                worksheet.Cells[i, 18].Value = item.ContactAddressQuanHuyen;
                            if (item.ContactAddressTinhThanhPho != null)
                                worksheet.Cells[i, 19].Value = item.ContactAddressTinhThanhPho;                            

                            if (item.IDCard != null)
                                worksheet.Cells[i, 20].Value = item.IDCard;
                            if (item.IDCardDate != null)
                                worksheet.Cells[i, 21].Value = item.IDCardDate.Value.ToString("dd/MM/yyyy");
                            if (item.IDCardPlace != null)
                                worksheet.Cells[i, 22].Value = item.IDCardPlace;
                            
                            if (item.CellPhone != null)
                                worksheet.Cells[i, 23].Value = item.CellPhone;
                            if (item.HomePhone != null)
                                worksheet.Cells[i, 24].Value = item.HomePhone;
                            if (item.Email != null)
                                worksheet.Cells[i, 25].Value = item.Email;                            
                            if (item.Skype != null) // BHXH
                                worksheet.Cells[i, 26].Value = item.Skype;                            
                            if (item.TaxNo != null)
                                worksheet.Cells[i, 27].Value = item.TaxNo;
                            if (item.BankCode != null)
                                worksheet.Cells[i, 28].Value = item.BankCode;
                            if (item.BankName != null)
                                worksheet.Cells[i, 29].Value = item.BankName;
                            if (item.InsuranceCode != null)
                                worksheet.Cells[i, 30].Value = item.InsuranceCode;
                            if (item.InsuranceDate != null)
                                worksheet.Cells[i, 31].Value = item.InsuranceDate.Value.ToString("dd/MM/yyyy");
                            
                            if (item.EducationID != null)
                                worksheet.Cells[i, 32].Value = item.EducationName;
                            if (item.SchoolName != null)
                                worksheet.Cells[i, 33].Value = item.SchoolName;                            
                            if (item.HeDaoTaoID != null)
                                worksheet.Cells[i, 34].Value = item.TenHeDaoTao;                            
                            if (item.ThoiGianTotNghiep != null)
                                worksheet.Cells[i, 35].Value = item.ThoiGianTotNghiep;
                            if (item.Qualification != null)
                                worksheet.Cells[i, 36].Value = item.Qualification;
                            if (item.TenTrinhDoAnhVan != null)
                                worksheet.Cells[i, 37].Value = item.TenTrinhDoAnhVan;
                            if (item.TenTrinhDoViTinh != null)
                                worksheet.Cells[i, 38].Value = item.TenTrinhDoViTinh;

                            if (item.Height != null)
                                worksheet.Cells[i, 39].Value = item.Height;
                            if (item.Weight != null)
                                worksheet.Cells[i, 40].Value = item.Weight;
                            if (item.BloodType != null)
                                worksheet.Cells[i, 41].Value = item.BloodType;
                        }  

                        worksheet.Row(i).Height = 32;
                        i++;
                    }

                    using (ExcelRange r = worksheet.Cells[5, 1, i - 1, 41])
                    {
                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }

                    worksheet.View.PageLayoutView = false;
                    //                    
                }



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

        public string XuatDanhSachBangCapHetHan(string[] listEmployeeID)
        {
            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
           // string server = Server.MapPath("~/App_Data");
            string server = ServerPath;

            FileInfo templateFile = new FileInfo(server + "//Mau_DSBC.xltx");
            FileInfo newFile = new FileInfo(outputDir.FullName + @"\DSBC.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\DSBC.xlsx");
            }



            ExcelPackage package;
            using (package = new ExcelPackage(newFile, templateFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int i = 14;
                //Duyệt qua danh sách để in ra
                int stt = 1;

                foreach (string item in listEmployeeID)
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

                        if (thuyenvien.BirthPlace != null)
                            worksheet.Cells[i, 3].Value = thuyenvien.BirthPlace;

                        //3. Bằng cấp
                        if (dre.DegreeName != null)
                            worksheet.Cells[i, 4].Value = dre.DegreeName;

                        //3. Số Bằng cấp
                        if (emdre.DegreeNo != null)
                            worksheet.Cells[i, 5].Value = emdre.DegreeNo;

                        //4. Ngày cấp
                        if (emdre.DegreeDate != null)
                            worksheet.Cells[i, 6].Value = Convert.ToDateTime(emdre.DegreeDate).ToString("dd/MM/yyyy");

                        //5. Ngày hết hạn
                        if (emdre.ExpirationDate != null)
                            worksheet.Cells[i, 7].Value = Convert.ToDateTime(emdre.ExpirationDate).ToString("dd/MM/yyyy");

                    }

                    i++;
                }

                using (ExcelRange r = worksheet.Cells[14, 1, i - 1, 7])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                using (ExcelRange r = worksheet.Cells[i + 1, 6, i + 1, 6])
                {
                    r.Style.Font.Italic = true;
                }
                worksheet.Cells[i + 1, 6].Value = "Ngày " + DateTime.Now.Day.ToString() + " tháng " + DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();
                worksheet.Cells[i + 2, 6].Value = "TRUNG TÂM THUYỀN VIÊN";
            }
            return "DSBC.xlsx";
        }

        public FileResult XuatDanhSachHocVienTheoKhoaDaoTao(int khoaDaoTaoID)
        {
            //khoaDaoTaoID = 1004;

            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = ServerPath;

            FileInfo templateFile = new FileInfo(server + "//Mau_KhoaDaoTao.xltx");
            FileInfo newFile = new FileInfo(outputDir.FullName + @"\Mau_KhoaDaoTao.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\Mau_KhoaDaoTao.xlsx");
            }
            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                //1. TIEUDE
                tbl_khoadaotao khoadaotao = (from p in db.tbl_khoadaotao
                                             where p.id_khoadaotao == khoaDaoTaoID
                                             select p).SingleOrDefault();

                if (khoadaotao.tenkhoadaotao != null)
                    worksheet.Cells[5, 3].Value = khoadaotao.tenkhoadaotao;
                if (khoadaotao.MonHoc != null)
                    worksheet.Cells[6, 3].Value = khoadaotao.MonHoc;
                if (khoadaotao.CapDo != null)
                    worksheet.Cells[7, 3].Value = khoadaotao.CapDo;
                if (khoadaotao.KhoaDaoTao != null)
                    worksheet.Cells[8, 3].Value = khoadaotao.KhoaDaoTao;
                if (khoadaotao.id_cosodaotao != null)
                    worksheet.Cells[9, 3].Value = khoadaotao.tbl_cosodaotao.tencoso;
                if (khoadaotao.ngaybatdau != null)
                    if (khoadaotao.NgayKetThuc != null)
                        worksheet.Cells[10, 3].Value = khoadaotao.ngaybatdau.Value.ToString("dd/MM/yyyy") + "-" + khoadaotao.NgayKetThuc.Value.ToString("dd/MM/yyyy");
                    else
                        worksheet.Cells[10, 3].Value = khoadaotao.ngaybatdau.Value.ToString("dd/MM/yyyy");

                if (khoadaotao.diadiem != null)
                    worksheet.Cells[11, 3].Value = khoadaotao.diadiem;
                if (khoadaotao.hocphi != null)
                    worksheet.Cells[12, 3].Value = khoadaotao.hocphi.Value;
                if (khoadaotao.TheoYeuCau != null)
                    worksheet.Cells[13, 3].Value = khoadaotao.TheoYeuCau;
                if (khoadaotao.GiayChungNhan != null)
                    worksheet.Cells[14, 3].Value = khoadaotao.GiayChungNhan;


                //****************************************
                //2. NOI DUNG


                var dsHocVien = from p in db.tbl_ctdaotao
                                join h in db.viewHRM_EMPLOYMENTHISTORY on p.EmployeeID equals h.EmployeeID
                                where p.id_khoadaotao == khoaDaoTaoID
                                orderby h.PositionID
                                select p;

                //var dsNhanVien = from p in db.viewHRM_EMPLOYMENTHISTORY
                //                 where p.EmployeeID in

                int i = 17;
                int stt = 1;

                foreach (var tv in dsHocVien)
                {
                    worksheet.Cells[i, 1].Value = stt;
                    stt++;


                    worksheet.Cells[i, 2].Value = tv.HRM_EMPLOYEE.FirstName + " " + tv.HRM_EMPLOYEE.LastName;

                    if (tv.ChucVu != null)
                        worksheet.Cells[i, 3].Value = tv.ChucVu;

                    if (tv.HRM_EMPLOYEE.BirthDay != null)// Năm sinh
                        worksheet.Cells[i, 4].Value = tv.HRM_EMPLOYEE.BirthDay.Value.ToString("dd/MM/yyyy");

                    if (tv.HRM_EMPLOYEE.BirthPlace != null) // Nơi sinh
                        worksheet.Cells[i, 5].Value = tv.HRM_EMPLOYEE.BirthPlace;

                    if (tv.ketqua != null) //Kết quả
                        worksheet.Cells[i, 6].Value = tv.ketqua;

                    if (tv.GhiChu != null)
                        worksheet.Cells[i, 7].Value = tv.GhiChu;

                    i++;
                }

                using (ExcelRange r = worksheet.Cells[17, 1, i - 1, 7])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                using (ExcelRange r = worksheet.Cells[i + 1, 6, i + 1, 6])
                {
                    r.Style.Font.Italic = true;
                }
                worksheet.Cells[i + 1, 6].Value = "Ngày " + DateTime.Now.Day.ToString() + " tháng " + DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();
                worksheet.Cells[i + 2, 6].Value = "TRUNG TÂM THUYỀN VIÊN";

                worksheet.View.PageLayoutView = false;
                // save our new workbook and we are done!
                package.Save();
            }
            string absolutePath = newFile.FullName;
            FileResult result = null;

            if (System.IO.File.Exists(absolutePath))
            {
                string fileName = System.IO.Path.GetFileName(absolutePath);
                byte[] fileBytes = System.IO.File.ReadAllBytes(absolutePath);
                result = File(fileBytes, "application/x-msdownload", fileName);
            }
            return result;
        }

        public FileResult Mau_BaoCaoChiPhiDaoTao(int employeeID)
        {
            //employeeID = 54;

            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = ServerPath;

            FileInfo templateFile = new FileInfo(server + "//Mau_BaoCaoChiPhiDaoTao.xltx");
            FileInfo newFile = new FileInfo(outputDir.FullName + @"\Mau_BaoCaoChiPhiDaoTao.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\Mau_BaoCaoChiPhiDaoTao.xlsx");
            }
            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                //****************************************
                //1. TIEUDE
                viewHRM_EMPLOYMENTHISTORY hrmEM = (from p in db.viewHRM_EMPLOYMENTHISTORY
                                                   where p.EmployeeID == employeeID
                                                   select p).SingleOrDefault();

                worksheet.Cells[6, 2].Value = hrmEM.FirstName + " " + hrmEM.LastName;
                if (hrmEM.ChucVu != null)
                    worksheet.Cells[6, 3].Value = hrmEM.ChucVu;

                //****************************************
                //2. NOI DUNG

                var dsKhoaDaoTao = from p in db.tbl_khoadaotao
                                   join k in db.tbl_ctdaotao on p.id_khoadaotao equals k.id_khoadaotao
                                   where k.EmployeeID == employeeID
                                   orderby p.ngaybatdau
                                   select new { p.tenkhoadaotao, p.tbl_cosodaotao.tencoso, p.TheoYeuCau, p.ngaybatdau, p.NgayKetThuc, p.hocphi };

                int i = 8;
                int stt = 1;
                double tongtien = 0;

                foreach (var khoaDaoTao in dsKhoaDaoTao)
                {
                    worksheet.Cells[i, 1].Value = stt;

                    worksheet.Cells[i, 2].Value = khoaDaoTao.tenkhoadaotao;

                    if (khoaDaoTao.tencoso != null)
                        worksheet.Cells[i, 3].Value = khoaDaoTao.tencoso;

                    if (khoaDaoTao.TheoYeuCau != null)
                        worksheet.Cells[i, 4].Value = khoaDaoTao.TheoYeuCau;

                    if (khoaDaoTao.ngaybatdau != null)
                        worksheet.Cells[i, 5].Value = khoaDaoTao.ngaybatdau.Value.ToString("dd/MM/yyyy");

                    if (khoaDaoTao.NgayKetThuc != null)
                        worksheet.Cells[i, 6].Value = khoaDaoTao.NgayKetThuc.Value.ToString("dd/MM/yyyy");

                    if (khoaDaoTao.hocphi != null)
                    {
                        worksheet.Cells[i, 7].Value = khoaDaoTao.hocphi;
                        tongtien += Convert.ToDouble(khoaDaoTao.hocphi);
                    }


                    i++;
                    stt++;
                }
                worksheet.Cells[i, 7].Value = tongtien;
                using (ExcelRange r = worksheet.Cells[i, 1, i, 6])
                {
                    r.Style.Font.Bold = true;
                    r.Merge = true;
                }
                worksheet.Cells[i, 1].Value = "TỔNG CỘNG";

                using (ExcelRange r = worksheet.Cells[i, 7, i, 7])
                {
                    r.Style.Font.Bold = true;
                }

                using (ExcelRange r = worksheet.Cells[8, 1, i, 7])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                using (ExcelRange r = worksheet.Cells[i + 1, 6, i + 1, 6])
                {
                    r.Style.Font.Italic = true;
                }
                worksheet.Cells[i + 1, 6].Value = "Ngày " + DateTime.Now.Day.ToString() + " tháng " + DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();
                worksheet.Cells[i + 2, 6].Value = "TRUNG TÂM THUYỀN VIÊN";

                worksheet.View.PageLayoutView = false;
                // save our new workbook and we are done!
                package.Save();
            }
            string absolutePath = newFile.FullName;
            FileResult result = null;

            if (System.IO.File.Exists(absolutePath))
            {
                string fileName = System.IO.Path.GetFileName(absolutePath);
                byte[] fileBytes = System.IO.File.ReadAllBytes(absolutePath);
                result = File(fileBytes, "application/x-msdownload", fileName);
            }
            return result;
        }

        public FileResult ThongKeQuaTrinhDaoTao(DateTime tungay, DateTime denngay, int loaibaocao)
        {
            //DateTime tungay = Convert.ToDateTime("2018-01-01");
            //DateTime denngay = Convert.ToDateTime("2018-12-12");
            //int loaibaocao = 2; // thống kê thuyền viên
            //loaibaocao = 2; // thống kê nhân viên

            int parentPB1;
            int parentPB2;

            if (loaibaocao == 1)
            {
                parentPB1 = 8;
                parentPB2 = 17;
            }
            else
            {
                parentPB1 = 1;
                parentPB2 = 1;
            }

            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = ServerPath;

            FileInfo templateFile = new FileInfo(server + "//Mau_ThongKeDaoTao.xltx");
            FileInfo newFile = new FileInfo(outputDir.FullName + @"\Mau_ThongKeDaoTao.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\Mau_ThongKeDaoTao.xlsx");
            }
            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                //****************************************
                //1. TIEUDE
                if (loaibaocao == 1)
                    worksheet.Cells[3, 1].Value = "THEO DÕI - THỐNG KÊ QUÁ TRÌNH ĐÀO TẠO THUYỀN VIÊN";
                else
                    worksheet.Cells[3, 1].Value = "THEO DÕI - THỐNG KÊ QUÁ TRÌNH ĐÀO TẠO NHÂN VIÊN";

                worksheet.Cells[4, 1].Value = "(Từ ngày " + tungay.ToString("dd/MM/yyyy") + " đến ngày " + denngay.ToString("dd/MM/yyyy") + ")";
                using (ExcelRange r = worksheet.Cells[4, 1, 4, 13])
                {
                    r.Style.Font.Italic = true;
                   
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                //****************************************
                //2. NOI DUNG

                var dsKhoaDaoTao = from p in db.tbl_khoadaotao
                                   join k in db.tbl_ctdaotao on p.id_khoadaotao equals k.id_khoadaotao
                                   join h in db.viewHRM_EMPLOYMENTHISTORY on k.EmployeeID equals h.EmployeeID
                                   where p.ngaybatdau >= tungay && p.ngaybatdau <= denngay && (h.ParentID == parentPB1 || h.ParentID == parentPB2)
                                   orderby p.ngaybatdau, p.id_khoadaotao, h.PositionID
                                   select new
                                   {
                                       p.tenkhoadaotao,
                                       p.tbl_cosodaotao.tencoso,
                                       p.GiayChungNhan,
                                       p.TheoYeuCau,
                                       p.ngaybatdau,
                                       p.NgayKetThuc,
                                       p.diadiem,
                                       p.hocphi,
                                       k.ketqua,
                                       h.FirstName,
                                       h.LastName,
                                       h.BirthDay,
                                       h.ParentID,
                                       h.DepartmentID,
                                       h.PositionID,
                                       h.ChucVu
                                   };


                int i = 6;
                int stt = 1;


                foreach (var khoaDaoTao in dsKhoaDaoTao)
                {
                    worksheet.Cells[i, 1].Value = stt;
                    worksheet.Cells[i, 2].Value = khoaDaoTao.FirstName + " " + khoaDaoTao.LastName;

                    if (khoaDaoTao.BirthDay != null)
                        worksheet.Cells[i, 3].Value = khoaDaoTao.BirthDay.Value.ToString("dd/MM/yyyy");

                    if (khoaDaoTao.ChucVu != null)
                        worksheet.Cells[i, 4].Value = khoaDaoTao.ChucVu;
                    if (khoaDaoTao.tenkhoadaotao != null)
                        worksheet.Cells[i, 5].Value = khoaDaoTao.tenkhoadaotao;

                    if (khoaDaoTao.tencoso != null)
                        worksheet.Cells[i, 6].Value = khoaDaoTao.tencoso;

                    if (khoaDaoTao.GiayChungNhan != null)
                        worksheet.Cells[i, 7].Value = khoaDaoTao.GiayChungNhan;

                    if (khoaDaoTao.ngaybatdau != null)
                        worksheet.Cells[i, 8].Value = khoaDaoTao.ngaybatdau.Value.ToString("dd/MM/yyyy");

                    if (khoaDaoTao.NgayKetThuc != null)
                        worksheet.Cells[i, 9].Value = khoaDaoTao.NgayKetThuc.Value.ToString("dd/MM/yyyy");

                    if (khoaDaoTao.diadiem != null)
                        worksheet.Cells[i, 10].Value = khoaDaoTao.diadiem;

                    if (khoaDaoTao.hocphi != null)
                        worksheet.Cells[i, 11].Value = khoaDaoTao.hocphi;

                    if (khoaDaoTao.TheoYeuCau != null)
                        worksheet.Cells[i, 12].Value = khoaDaoTao.TheoYeuCau;

                    if (khoaDaoTao.ketqua != null)
                        worksheet.Cells[i, 13].Value = khoaDaoTao.ketqua;

                    i++;
                    stt++;
                }

                try
                {
                    using (ExcelRange r = worksheet.Cells[6, 1, i - 1, 13])
                    {
                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                }
                catch { };


                using (ExcelRange r = worksheet.Cells[i, 10, i, 12])
                {
                    r.Style.Font.Italic = true;
                    r.Merge = true;
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                worksheet.Cells[i, 10].Value = "Ngày " + DateTime.Now.Day.ToString() + " tháng " + DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();

                using (ExcelRange r = worksheet.Cells[i + 1, 10, i + 1, 12])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                worksheet.Cells[i + 1, 10].Value = "TRUNG TÂM THUYỀN VIÊN";

                worksheet.View.PageLayoutView = false;
                // save our new workbook and we are done!
                package.Save();
            }
            string absolutePath = newFile.FullName;
            FileResult result = null;

            if (System.IO.File.Exists(absolutePath))
            {
                string fileName = System.IO.Path.GetFileName(absolutePath);
                byte[] fileBytes = System.IO.File.ReadAllBytes(absolutePath);
                result = File(fileBytes, "application/x-msdownload", fileName);
            }
            return result;
        }

        public FileResult XuatCrewMatrixSyQuanTau(int departmentID)
        {
            //int departmentID = 16;

            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = ServerPath;

            FileInfo templateFile = new FileInfo(server + "//Mau_CrewMatrixSyQuanTau.xltx");
            FileInfo newFile = new FileInfo(outputDir.FullName + @"\CrewMatrixSyQuanTau.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\CrewMatrixSyQuanTau.xlsx");
            }

            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                //****************************************
                //1. TIEUDE
                DIC_DEPARTMENT department = (from p in db.DIC_DEPARTMENT
                                             where p.DepartmentID == departmentID
                                             select p).SingleOrDefault();

                worksheet.Cells[3, 1].Value = "CREW MATRIX - " + department.DepartmentName;
                worksheet.Cells[4, 11].Value = "Ngày " + DateTime.Now.Date.ToString("dd/MM/yyyy");

                //****************************************
                //2. NOI DUNG

                var dsThuyenVien = from p in db.viewHRM_EMPLOYMENTHISTORY
                                   where p.DepartmentID == departmentID && (p.PositionID >= 20 && p.PositionID <= 27)
                                   orderby p.InternshipPosition, p.PositionID
                                   select p;

                int i = 6;
                int stt = 1;
                foreach (var tv in dsThuyenVien)
                {
                    worksheet.Cells[i, 1].Value = stt;
                    worksheet.Cells[i, 2].Value = tv.FirstName + " " + tv.LastName;

                    if (tv.BirthDay != null)
                        worksheet.Cells[i, 3].Value = tv.BirthDay.Value.ToString("dd/MM/yyyy");

                    if (tv.ChucVu != null)
                        worksheet.Cells[i, 4].Value = tv.ChucVu;

                    if (tv.Qualification != null)
                        worksheet.Cells[i, 5].Value = tv.Qualification;

                    if (tv.NgayXuongTau != null)
                        worksheet.Cells[i, 6].Value = tv.NgayXuongTau.Value.ToString("dd/MM/yyyy");

                    var crewmatrix = (from p in db.sp_LayThongTinMatrix(tv.PositionID, tv.EmployeeID)
                                      select p).FirstOrDefault();

                    worksheet.Cells[i, 7].Value = Convert.ToDouble(crewmatrix.WithOperator) / 365;
                    worksheet.Cells[i, 8].Value = Convert.ToDouble(crewmatrix.InRank) / 365;
                    worksheet.Cells[i, 9].Value = Convert.ToDouble(crewmatrix.ThisTypeOfTanker) / 365;                   
                    worksheet.Cells[i, 10].Value = Convert.ToDouble(crewmatrix.WatchKeeping) / 365;
                    worksheet.Cells[i, 11].Value = Convert.ToDouble(crewmatrix.AllTypeOfTanker) / 365;

                    i++;
                    stt++;
                }

                using (ExcelRange r = worksheet.Cells[6, 1, i - 1, 11])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
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

        public FileResult XuatDanhSachTuyenDung(DateTime tungay, DateTime denngay, int loaibaocao)
        {
            //DateTime tungay = Convert.ToDateTime("2017-01-01");
            //DateTime denngay = Convert.ToDateTime("2017-12-31");

            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = ServerPath;

            FileInfo templateFile = new FileInfo(server + "//Mau_DSTuyenDung.xltx");
            FileInfo newFile = new FileInfo(outputDir.FullName + @"\DSTuyenDung.xlsx");

            if (loaibaocao == 1)// Tuyen dung Thuyen vien
            {
                templateFile = new FileInfo(server + "//Mau_DSTuyenDung.xltx");

            }
            else //Tuyen dung Nhan vien
            {
                templateFile = new FileInfo(server + "//Mau_DSTuyenDungNhanVien.xltx");
            }


            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\DSTuyenDung.xlsx");
            }
            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xu?t d? li?u
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                worksheet.Cells[3, 1].Value = "( TỪ NGÀY " + tungay.ToString("dd/MM/yyyy") + " ĐẾN NGÀY " + denngay.ToString("dd/MM/yyyy") + " )";
                //****************************************
                //2. NOI DUNG
                int i = 6;
                int stt = 1;
                //int departmentID = 0;

                var listNV = from p in db.sp_LayDSTuyenDung(tungay, denngay, loaibaocao)
                             orderby p.PositionID, p.DecisionDate
                             select p;

                int SQ_Boong = 0;
                int SQ_May = 0;
                int TV_Boong = 0;
                int TV_May = 0;

                int TV_CD_DH = 0;
                int TV_SC_TC = 0;
                int tongtuoi = 0;
                int songuoi = 0;

                foreach (var tv in listNV)
                {
                    worksheet.Cells[i, 1].Value = stt;
                    worksheet.Cells[i, 2].Value = tv.FirstName + " " + tv.LastName;
                    if (tv.QuanHeID != null)
                        worksheet.Cells[i, 3].Value = tv.QuanHeID;

                    worksheet.Cells[i, 4].Value = tv.PositionName;

                    if (tv.BirthDay != null)
                        worksheet.Cells[i, 5].Value = tv.BirthDay.Value.ToString("dd/MM/yyyy");

                    if (tv.Qualification != null)
                        worksheet.Cells[i, 6].Value = tv.Qualification;

                    if (tv.EducationID != null)
                        worksheet.Cells[i, 7].Value = tv.EducationName;

                    if (tv.DecisionNo != null)
                        worksheet.Cells[i, 8].Value = tv.DecisionNo;

                    if (tv.Tuoi != null)
                    {
                        worksheet.Cells[i, 9].Value = tv.Tuoi;
                        tongtuoi += Convert.ToInt32(tv.Tuoi);
                        songuoi++;
                    }

                    worksheet.Cells[i, 10].Value = tv.DepartmentName;

                    if (tv.Note != null)
                        worksheet.Cells[i, 11].Value = tv.Note;

                    if (tv.DecisionDate != null)
                        worksheet.Cells[i, 12].Value = tv.DecisionDate.Value.ToString("dd/MM/yyyy");

                    if (tv.GroupPositionID == 1)
                        SQ_Boong++;
                    else if (tv.GroupPositionID == 2)
                        SQ_May++;

                    if (tv.GroupPositionID == 1 || tv.GroupPositionID == 5)
                        TV_Boong++;
                    else
                        TV_May++;


                    if (tv.EducationID == 1 || tv.EducationID == 2 || tv.EducationID == 3 || tv.EducationID == 4)
                        TV_CD_DH++;
                    else
                        TV_SC_TC++;

                    stt++;
                    i++;
                }

                using (ExcelRange r = worksheet.Cells[6, 1, i - 1, 12])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                if (loaibaocao == 1)
                {
                    worksheet.Cells[i + 1, 11, i + 1, 11].Style.WrapText = false;
                    worksheet.Cells[i + 1, 11].Value = "TRUNG TÂM THUYỀN VIÊN";

                    //Chức danh
                    if (SQ_Boong != 0)
                    {
                        worksheet.Cells[9, 16].Value = SQ_Boong;
                        worksheet.Cells[9, 17].Value = Convert.ToInt32(SQ_Boong * 100 / (SQ_Boong + SQ_May)).ToString() + "%";
                    }

                    if (SQ_May != 0)
                    {
                        worksheet.Cells[10, 16].Value = SQ_May;
                        worksheet.Cells[10, 17].Value = (100 - Convert.ToInt32(SQ_Boong * 100 / (SQ_Boong + SQ_May))).ToString() + "%";
                    }
                    worksheet.Cells[8, 16].Value = SQ_May + SQ_Boong;

                    if (TV_Boong != 0)
                    {
                        worksheet.Cells[6, 16].Value = TV_Boong;
                        worksheet.Cells[6, 17].Value = Convert.ToInt32(TV_Boong * 100 / (TV_Boong + TV_May)).ToString() + "%";
                    }

                    if (TV_May != 0)
                    {
                        worksheet.Cells[7, 16].Value = TV_May;
                        worksheet.Cells[7, 17].Value = (100 - Convert.ToInt32(TV_Boong * 100 / (TV_Boong + TV_May))).ToString() + "%";
                    }
                    worksheet.Cells[5, 16].Value = TV_May + TV_Boong;

                    if (TV_CD_DH != 0)
                    {
                        worksheet.Cells[11, 16].Value = TV_CD_DH;
                        worksheet.Cells[11, 17].Value = Convert.ToInt32(TV_CD_DH * 100 / (TV_CD_DH + TV_SC_TC)).ToString() + "%";
                    }

                    if (TV_SC_TC != 0)
                    {
                        worksheet.Cells[12, 16].Value = TV_SC_TC;
                        worksheet.Cells[12, 17].Value = (100 - Convert.ToInt32(TV_CD_DH * 100 / (TV_CD_DH + TV_SC_TC))).ToString() + "%";
                    }
                    worksheet.Cells[13, 16].Value = Convert.ToInt32(tongtuoi / songuoi);


                    var list_CD = from p in db.DIC_POSITION
                                  where p.GroupPositionID != 4
                                  select p;

                    int j = 16;
                    int tongcong = 0;
                    foreach (var CD in list_CD)
                    {
                        worksheet.Cells[j, 15].Value = CD.PositionName;

                        int SL_CD = (from p in db.sp_LayDSTuyenDung(tungay, denngay, loaibaocao)
                                     where p.PositionID == CD.PositionID
                                     select p).Count();

                        worksheet.Cells[j, 16].Value = SL_CD;
                        tongcong += SL_CD;
                        j++;
                    }
                    //TONGCONG
                    worksheet.Cells[j, 16].Value = tongcong;
                }



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

        public FileResult XuatDanhSachNghiViec(DateTime tungay, DateTime denngay, int loaibaocao)
        {
            //DateTime tungay = Convert.ToDateTime("2017-01-01");
            //DateTime denngay = Convert.ToDateTime("2017-12-31");

            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = ServerPath;

            FileInfo templateFile = new FileInfo(server + "//Mau_DSNghiViec.xltx");
            if (loaibaocao == 1)// Tuyen dung Thuyen vien
            {
                templateFile = new FileInfo(server + "//Mau_DSNghiViec.xltx");

            }
            else //Tuyen dung Nhan vien
            {
                templateFile = new FileInfo(server + "//Mau_DSNghiViecNhanVien.xltx");
            }


            FileInfo newFile = new FileInfo(outputDir.FullName + @"\DSNghiViec.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\DSNghiViec.xlsx");
            }
            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xu?t d? li?u
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                worksheet.Cells[3, 1].Value = "( TỪ NGÀY " + tungay.ToString("dd/MM/yyyy") + " ĐẾN NGÀY " + denngay.ToString("dd/MM/yyyy") + " )";
                //****************************************
                //2. NOI DUNG
                int i = 6;
                int stt = 1;
                //int departmentID = 0;

                var listNV = from p in db.sp_LayDSNghiViec(tungay, denngay, loaibaocao)
                             orderby p.PositionID, p.DecisionDate
                             select p;

                int SQ = 0;
                int CD_Khac = 0;
                int TV_Boong = 0;
                int TV_May = 0;
                int TV_Huu = 0;
                int TV_KyLuat = 0;
                int TV_NghiKhac = 0;
                int TV_CD_DH = 0;
                int TV_SC_TC = 0;

                foreach (var tv in listNV)
                {
                    worksheet.Cells[i, 1].Value = stt;
                    worksheet.Cells[i, 2].Value = tv.FirstName + " " + tv.LastName;

                    if (tv.QuanHeID != null)
                        worksheet.Cells[i, 3].Value = tv.QuanHeID;

                    if (tv.PositionName != null)
                        worksheet.Cells[i, 4].Value = tv.PositionName;

                    if (tv.BirthDay != null)
                        worksheet.Cells[i, 5].Value = tv.BirthDay.Value.ToString("dd/MM/yy");

                    if (tv.EducationName != null)
                        worksheet.Cells[i, 6].Value = tv.EducationName;

                    if (tv.QTDT != null)
                        worksheet.Cells[i, 7].Value = tv.QTDT;

                    if (tv.DecisionDate != null)
                        worksheet.Cells[i, 8].Value = tv.DecisionDate.Value.ToString("dd/MM/yy");

                    if (tv.TGNV != null)
                        worksheet.Cells[i, 9].Value = tv.TGNV;

                    if (tv.LyDoNghiViec_ID != null)
                        worksheet.Cells[i, 10].Value = tv.LyDoNghiViec_Name;

                    if (tv.Note != null)
                        worksheet.Cells[i, 11].Value = tv.Note;

                    if (tv.GroupPositionID == 1 || tv.GroupPositionID == 2)
                        SQ++;
                    else
                        CD_Khac++;

                    if (tv.GroupPositionID == 1 || tv.GroupPositionID == 5)
                        TV_Boong++;
                    else
                        TV_May++;

                    if (tv.LyDoNghiViec_ID == 1)
                        TV_Huu++;
                    else if (tv.LyDoNghiViec_ID == 3)
                        TV_KyLuat++;
                    else
                        TV_NghiKhac++;

                    if (tv.EducationID == 1 || tv.EducationID == 2 || tv.EducationID == 3 || tv.EducationID == 4)
                        TV_CD_DH++;
                    else
                        TV_SC_TC++;

                    stt++;
                    i++;
                }

                using (ExcelRange r = worksheet.Cells[5, 1, i - 1, 11])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                if (loaibaocao == 1)
                {
                    worksheet.Cells[i + 1, 9, i + 1, 9].Style.WrapText = false;
                    worksheet.Cells[i + 1, 9].Value = "TRUNG TÂM THUYỀN VIÊN";
                    //Chức danh
                    if (SQ != 0)
                    {
                        worksheet.Cells[6, 15].Value = SQ;
                        worksheet.Cells[6, 16].Value = Convert.ToInt32(SQ * 100 / (SQ + CD_Khac)).ToString() + "%";
                    }

                    if (CD_Khac != 0)
                    {
                        worksheet.Cells[7, 15].Value = CD_Khac;
                        worksheet.Cells[7, 16].Value = (100 - Convert.ToInt32(SQ * 100 / (SQ + CD_Khac))).ToString() + "%";
                    }

                    if (TV_Boong != 0)
                    {
                        worksheet.Cells[8, 15].Value = TV_Boong;
                        worksheet.Cells[8, 16].Value = Convert.ToInt32(TV_Boong * 100 / (TV_Boong + TV_May)).ToString() + "%";
                    }

                    if (TV_May != 0)
                    {
                        worksheet.Cells[9, 15].Value = TV_May;
                        worksheet.Cells[9, 16].Value = (100 - Convert.ToInt32(TV_Boong * 100 / (TV_Boong + TV_May))).ToString() + "%";
                    }

                    if (TV_KyLuat != 0)
                    {
                        worksheet.Cells[10, 15].Value = TV_KyLuat;
                        worksheet.Cells[10, 16].Value = Convert.ToInt32(TV_KyLuat * 100 / (TV_Huu + TV_KyLuat + TV_NghiKhac)).ToString() + "%";
                    }

                    if (TV_Huu != 0)
                    {
                        worksheet.Cells[11, 15].Value = TV_Huu;
                        worksheet.Cells[11, 16].Value = Convert.ToInt32(TV_Huu * 100 / (TV_Huu + TV_KyLuat + TV_NghiKhac)).ToString() + "%";
                    }

                    if (TV_NghiKhac != 0)
                    {
                        worksheet.Cells[12, 15].Value = TV_NghiKhac;
                        worksheet.Cells[12, 16].Value = (100 - Convert.ToInt32(TV_Huu * 100 / (TV_Huu + TV_KyLuat + TV_NghiKhac)) - Convert.ToInt32(TV_NghiKhac * 100 / (TV_KyLuat + TV_KyLuat + TV_NghiKhac))).ToString() + "%";
                    }

                    if (TV_CD_DH != 0)
                    {
                        worksheet.Cells[13, 15].Value = TV_CD_DH;
                        worksheet.Cells[13, 16].Value = Convert.ToInt32(TV_CD_DH * 100 / (TV_CD_DH + TV_SC_TC)).ToString() + "%";
                    }

                    if (TV_SC_TC != 0)
                    {
                        worksheet.Cells[14, 15].Value = TV_SC_TC;
                        worksheet.Cells[14, 16].Value = (100 - Convert.ToInt32(TV_CD_DH * 100 / (TV_CD_DH + TV_SC_TC))).ToString() + "%";
                    }



                    var list_CD = from p in db.DIC_POSITION
                                  where p.GroupPositionID != 4
                                  select p;

                    int j = 17;
                    int tongcong = 0;
                    foreach (var CD in list_CD)
                    {
                        worksheet.Cells[j, 14].Value = CD.PositionName;

                        int SL_CD = (from p in db.sp_LayDSNghiViec(tungay, denngay, 1)
                                     where p.PositionID == CD.PositionID
                                     select p).Count();

                        worksheet.Cells[j, 15].Value = SL_CD;
                        tongcong += SL_CD;
                        j++;
                    }
                    //TONGCONG
                    worksheet.Cells[j, 15].Value = tongcong;
                }



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

        public FileResult BaoCaoThongKeThuyenVien(DateTime ngaybaocao)
        {
            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = ServerPath;

            FileInfo templateFile = new FileInfo(server + "//Mau_ThongKeThuyenVien.xltx");
            FileInfo newFile = new FileInfo(outputDir.FullName + @"\TKTV.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\TKTV.xlsx");
            }
            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xu?t d? li?u
                ExcelWorksheet xlSheet = package.Workbook.Worksheets[1];

                xlSheet.Cells[5, 22].Value = "Tính đến ngày " + DateTime.Now.ToString("dd/MM/yyyy");


                //****************************************
                var listTau = from p in db.DIC_DEPARTMENT
                              where p.ParentID == 8 && p.DepartmentID != 24 && p.DepartmentID != 17 && p.DepartmentID != 11
                              select p;

                var listChucVu = from p in db.DIC_POSITION
                                 where p.GroupPositionID != 4
                                 select p;

                // Định biên, thực tập
                int hang, cot;
                cot = 13;
                foreach (DIC_DEPARTMENT tau in listTau)
                {
                    //xlSheet.Cells[11, cot].Value = tau.Description;
                    hang = 14;
                    int tong_theotau = 0;
                    int tong_dinhbien_tau = 0;
                    int tong_thuctap_tau = 0;

                    tong_dinhbien_tau = (from p in db.viewHRM_EMPLOYMENTHISTORY
                                         where p.DepartmentID == tau.DepartmentID && p.InternshipPosition == null && p.StatusID == 1
                                         select p).Count();

                    tong_thuctap_tau = (from p in db.viewHRM_EMPLOYMENTHISTORY
                                        where p.DepartmentID == tau.DepartmentID && p.InternshipPosition != null && p.StatusID == 1
                                        select p).Count();

                    if (tong_dinhbien_tau > 0)
                        xlSheet.Cells[hang - 3, cot].Value = tong_dinhbien_tau;
                    if (tong_thuctap_tau > 0)
                        xlSheet.Cells[hang - 2, cot].Value = tong_thuctap_tau;


                    foreach (DIC_POSITION cv in listChucVu)
                    {
                        int SL_Tau_CV = (from p in db.viewHRM_EMPLOYMENTHISTORY
                                         where p.DepartmentID == tau.DepartmentID && p.PositionID == cv.PositionID && p.StatusID == 1
                                         select p).Count();

                        tong_theotau += SL_Tau_CV;
                        if (SL_Tau_CV > 0)
                            xlSheet.Cells[hang, cot].Value = SL_Tau_CV;
                        xlSheet.Cells[hang, 3].Value = cv.PositionName;
                        hang++;
                    }

                    xlSheet.Cells[hang, cot].Value = tong_theotau;
                    cot++;
                }

                var listDuTru = from p in db.DIC_DEPARTMENT
                                where p.ParentID == 17 orderby p.ThuTuPhongBan
                                select p;
                cot = 22;

                //22 SSDD, 23 NKL, 24 NCL, 25CTV, 26TVM SSDD, 27 TVM ko SSDD
                foreach (DIC_DEPARTMENT tau in listDuTru)
                {
                    //xlSheet.Cells[11, cot].Value = tau.Description;
                    hang = 14;                    
                    int tong_theotau = 0;
                    int tong_tvm_ssdd = 0;
                    int tong_tvm_kossdd = 0;
                    int tong_ctv = 0;
                    int tong_nkl_kossdd = 0;                    
                    int tong_ssdd = 0;

                    foreach (DIC_POSITION cv in listChucVu)
                    {
                        int SL_Tau_CV = (from p in db.viewHRM_EMPLOYMENTHISTORY
                                         where p.DepartmentID == tau.DepartmentID && p.PositionID == cv.PositionID && p.StatusID == 1
                                         select p).Count();

                        //switch (tau.DepartmentID)
                        //{
                        //    case 18:
                        //        cot = 22;//SSDD
                        //        break;
                        //    case 19:
                        //        cot = 23;//NKL
                        //        break;
                        //    case 20:
                        //        cot = 25;//CTV
                        //        break;
                        //    case 21:
                        //        cot = 26;//TVM
                        //        break;
                        //    case 29:
                        //        cot = 24;//NCL
                        //        break;
                        //}




                        //if (tau.DepartmentID == 21)
                        //{
                        //    int SL_Tau_CV_SSDD = (from p in db.viewHRM_EMPLOYMENTHISTORY
                        //                          where p.DepartmentID == tau.DepartmentID && p.PositionID == cv.PositionID && p.StatusID == 1 && p.SSDD == true
                        //                          select p).Count();

                        //    int SL_Tau_CV_KoSSDD = (from p in db.viewHRM_EMPLOYMENTHISTORY
                        //                            where p.DepartmentID == tau.DepartmentID && p.PositionID == cv.PositionID && p.StatusID == 1 && (p.SSDD == false || p.SSDD == null)
                        //                            select p).Count();

                        //    if (SL_Tau_CV_SSDD > 0)
                        //    {
                        //        xlSheet.Cells[hang, 26].Value = SL_Tau_CV_SSDD;
                        //        tong_tvm_ssdd += SL_Tau_CV_SSDD;
                        //    }

                        //    if (SL_Tau_CV_KoSSDD > 0)
                        //    {
                        //        xlSheet.Cells[hang, 27].Value = SL_Tau_CV_KoSSDD;
                        //        tong_tvm_kossdd += SL_Tau_CV_KoSSDD;
                        //    }
                        //}
                        //else if (tau.DepartmentID == 19)
                        //{                           
                        //    int SL_NKL_CV_KoSSDD = (from p in db.viewHRM_EMPLOYMENTHISTORY
                        //                            where p.DepartmentID == tau.DepartmentID && p.PositionID == cv.PositionID && p.StatusID == 1 && (p.SSDD == false || p.SSDD == null)
                        //                            select p).Count();                           

                        //    if (SL_NKL_CV_KoSSDD > 0)
                        //    {
                        //        xlSheet.Cells[hang, 23].Value = SL_NKL_CV_KoSSDD;
                        //        tong_nkl_kossdd += SL_NKL_CV_KoSSDD;
                        //    }
                        //}
                        //else if (tau.DepartmentID == 18)
                        //{
                        //    int SL_NKL_CV_SSDD = (from p in db.viewHRM_EMPLOYMENTHISTORY
                        //                          where p.DepartmentID == 19 && p.PositionID == cv.PositionID && p.StatusID == 1 && p.SSDD == true
                        //                          select p).Count();

                        //    xlSheet.Cells[hang, cot].Value = SL_Tau_CV + SL_NKL_CV_SSDD;
                        //    tong_ssdd += SL_Tau_CV + SL_NKL_CV_SSDD;
                        //}
                        //else if (tau.DepartmentID == 18)
                        //{
                        //    int SL_NKL_CV_SSDD = (from p in db.viewHRM_EMPLOYMENTHISTORY
                        //                          where p.DepartmentID == 19 && p.PositionID == cv.PositionID && p.StatusID == 1 && p.SSDD == true
                        //                          select p).Count();

                        //    xlSheet.Cells[hang, cot].Value = SL_Tau_CV + SL_NKL_CV_SSDD;
                        //    tong_ssdd += SL_Tau_CV + SL_NKL_CV_SSDD;
                        //}
                        //else
                        //{
                        //    if (SL_Tau_CV > 0)
                        //        xlSheet.Cells[hang, cot].Value = SL_Tau_CV;
                        //}

                        if (SL_Tau_CV > 0)
                            xlSheet.Cells[hang, cot].Value = SL_Tau_CV;

                        tong_theotau += SL_Tau_CV;
                        hang++;
                    }
                   
                    
                    //if (tau.DepartmentID == 21)
                    //{
                    //    xlSheet.Cells[hang, 26].Value = tong_tvm_ssdd;
                    //    xlSheet.Cells[hang, 27].Value = tong_tvm_kossdd;
                    //}
                    //else if (tau.DepartmentID == 19)
                    //{
                    //    xlSheet.Cells[hang, 23].Value = tong_nkl_kossdd;                       
                    //}
                    //else if (tau.DepartmentID == 20)
                    //{
                    //    //xlSheet.Cells[hang, 25].Value = tong_
                    //}
                    //else if (tau.DepartmentID == 18)
                    //{
                    //    xlSheet.Cells[hang, 22].Value = tong_ssdd;
                    //}
                    //else
                    //    xlSheet.Cells[hang, cot].Value = tong_theotau;

                    xlSheet.Cells[hang, cot].Value = tong_theotau;
                    cot++;
                }


                hang = 14;
                int tong_tv_daunam = 0; // Tổng thuyền viên đầu năm - Thuyền viên mới 21
                int tong_tv = 0;        // Tổng thuyền viên - Thuyền viên mới chưa ký hợp đồng, chỉ tuyển dụng                
                int tong_tv_tau = 0;    // Tổng thuyền viên khối tàu
                int tong_tv_dutru = 0;  // Tổng thuyền viên khối dự trữ - Thuyền viên mới

                foreach (DIC_POSITION cv in listChucVu)
                {

                    // Thuyền viên đầu năm - Thuyền viên mới
                    int SL_CV_DauNam = (from p in db.sp_LayDSNhanVienTheoNgay(ngaybaocao)
                                        where p.PositionID == cv.PositionID && p.CategoryDecisionID != 3 && p.DepartmentID != 21
                                        select p).Count();
                    xlSheet.Cells[hang, 4].Value = SL_CV_DauNam;
                    tong_tv_daunam += SL_CV_DauNam;


                    // Thuyền viên khối tàu      

                    int SL_CV_Tau = (from p in db.viewHRM_EMPLOYMENTHISTORY
                                     where p.PositionID == cv.PositionID && p.ParentID != 17 && p.StatusID == 1
                                     select p).Count();

                    xlSheet.Cells[hang, 12].Value = SL_CV_Tau;
                    tong_tv_tau += SL_CV_Tau;

                    // Thuyền viên khối dự trữ - Thuyền viên mới
                    int SL_CV_DuTru = (from p in db.viewHRM_EMPLOYMENTHISTORY
                                       where p.PositionID == cv.PositionID && p.StatusID == 1 && p.ParentID == 17 && p.DepartmentID != 21
                                       select p).Count();
                    xlSheet.Cells[hang, 21].Value = SL_CV_DuTru;
                    tong_tv_dutru += SL_CV_DuTru;

                    // Tổng thuyền viên - Thuyền viên mới
                    xlSheet.Cells[hang, 11].Value = SL_CV_DuTru + SL_CV_Tau;

                    hang++;
                }
                xlSheet.Cells[hang, 11].Value = tong_tv_tau + tong_tv_dutru;
                xlSheet.Cells[hang, 12].Value = tong_tv_tau;
                if (tong_tv_dutru > 0)
                    xlSheet.Cells[hang, 21].Value = tong_tv_dutru;
                xlSheet.Cells[hang, 4].Value = tong_tv_daunam;

                // Tuyển dụng và nghỉ việc trong năm
                int nam = DateTime.Now.Year;
                int tong_tuyendung = 0;
                int tong_nghiviec_kyluat = 0;
                int tong_nghiviec_khac = 0;
                int tong_bonhiem = 0;
                int tong_kyluatgiamchucdanh = 0;

                hang = 14;
                foreach (DIC_POSITION cv in listChucVu)
                {
                    //Tuyển dụng
                    int SL_CV_TD = (from p in db.view_quatrinhcongtacFull
                                    where p.PositionID == cv.PositionID && p.XacNhan == true && p.DecisionDate.Value.Year == nam && p.CategoryDecisionID == 2
                                    select p).Count();

                    if (SL_CV_TD > 0)
                        xlSheet.Cells[hang, 5].Value = SL_CV_TD;
                    tong_tuyendung += SL_CV_TD;

                    //Bổ nhiệm
                    int SL_CV_BN = (from p in db.view_quatrinhcongtacFull
                                    where p.PositionID == cv.PositionID && p.XacNhan == true && p.DecisionDate.Value.Year == nam && p.CategoryDecisionID == 1
                                    select p.EmployeeID).Distinct().Count();

                    if (SL_CV_BN > 0)
                        xlSheet.Cells[hang, 9].Value = SL_CV_BN;
                    tong_bonhiem += SL_CV_BN;

                    //Kỷ luật giảm chức danh
                    int SL_CV_KLGCD = 0;

                    //int SL_CV_KLGCD = (from p in db.view_quatrinhcongtacFull
                    //                where p.PositionID == cv.PositionID && p.XacNhan == true && p.DecisionDate.Value.Year == nam && p.CategoryDecisionID == 5
                    //                select p).Count();



                    //if (SL_CV_KLGCD > 0)
                    //    xlSheet.Cells[hang, 10].Value = SL_CV_KLGCD;
                    //tong_kyluatgiamchucdanh += SL_CV_KLGCD;

                    var list_kyluat_hachucdanh = from p in db.view_quatrinhcongtacFull
                                                 where p.XacNhan == true && p.DecisionDate.Value.Year == nam && p.CategoryDecisionID == 5
                                                 select p;
                    foreach (view_quatrinhcongtacFull qtct_kyluat in list_kyluat_hachucdanh)
                    {
                        // Chọn QTCT trước khi hạ chức danh
                        var qtct_truoc_kyluat = (from p in db.view_quatrinhcongtacFull
                                                 where p.XacNhan == true && p.EmployeeID == qtct_kyluat.EmployeeID && p.DecisionDate < qtct_kyluat.DecisionDate
                                                 orderby p.DecisionDate descending, p.EmploymentHistoryID descending
                                                 select p).FirstOrDefault();
                        if (qtct_truoc_kyluat.PositionID == cv.PositionID)
                            SL_CV_KLGCD += 1;

                    }
                    if (SL_CV_KLGCD > 0)
                        xlSheet.Cells[hang, 10].Value = SL_CV_KLGCD;

                    //Nghỉ việc
                    int SL_CV_NV_KL = (from p in db.view_quatrinhcongtacFull
                                       where p.PositionID == cv.PositionID && p.XacNhan == true && p.DecisionDate.Value.Year == nam && p.CategoryDecisionID == 3 && p.LyDoNghiViec_ID == 3
                                       select p).Count();
                    if (SL_CV_NV_KL > 0)
                        xlSheet.Cells[hang, 7].Value = SL_CV_NV_KL;
                    tong_nghiviec_kyluat += SL_CV_NV_KL;

                    int SL_CV_NV_Khac = (from p in db.view_quatrinhcongtacFull
                                         where p.PositionID == cv.PositionID && p.XacNhan == true && p.DecisionDate.Value.Year == nam && p.CategoryDecisionID == 3 && (p.LyDoNghiViec_ID != 3 || p.LyDoNghiViec_ID == null)
                                         select p).Count();
                    if (SL_CV_NV_Khac > 0)
                        xlSheet.Cells[hang, 8].Value = SL_CV_NV_Khac;
                    tong_nghiviec_khac += SL_CV_NV_Khac;

                    if ((SL_CV_NV_Khac + SL_CV_NV_KL) > 0)
                        xlSheet.Cells[hang, 6].Value = SL_CV_NV_Khac + SL_CV_NV_KL;

                    hang++;
                }
                xlSheet.Cells[hang, 5].Value = tong_tuyendung;
                xlSheet.Cells[hang, 7].Value = tong_nghiviec_kyluat;
                xlSheet.Cells[hang, 8].Value = tong_nghiviec_khac;
                xlSheet.Cells[hang, 6].Value = tong_nghiviec_kyluat + tong_nghiviec_khac;
                xlSheet.Cells[hang, 9].Value = tong_bonhiem;
                xlSheet.Cells[hang, 10].Value = tong_kyluatgiamchucdanh;


                xlSheet.View.PageLayoutView = false;
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

        public FileResult XuatDanhSachBoNhiem(DateTime tungay, DateTime denngay, int loaibaocao)
        {
            List<sp_LayDSBoNhiem_Result> dsThuyenVien = (from p in db.sp_LayDSBoNhiem(tungay, denngay)
                                               where (p.ParentID ==8 || p.ParentID == 17)
                                               select p).ToList();


            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = ServerPath;
            FileInfo templateFile;
            FileInfo newFile;
            templateFile = new FileInfo(server + "//Mau_DSThuyenVienBoNhiem.xltx");
            newFile = new FileInfo(outputDir.FullName + @"\DSThuyenVienBoNhiem.xlsx");

            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\DSThuyenVienBoNhiem.xlsx");
            }

            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                worksheet.Cells[9, 1].Value = "TỪ NGÀY " + tungay.ToString("dd/MM/yyyy") + " ĐẾN NGÀY " + denngay.ToString("dd/MM/yyyy");

                //****************************************
                //2. NOI DUNG
                int i = 12;
                int stt = 1;                

                var listTV = dsThuyenVien;

                foreach (var tv in listTV)
                {
                    worksheet.Cells[i, 1].Value = stt;
                    stt++;

                    worksheet.Cells[i, 2].Value = tv.FirstName + ' ' + tv.LastName;

                    if (tv.QuanHeID != null) 
                        worksheet.Cells[i, 3].Value = tv.QuanHeID;                    

                    if (tv.ChucVu != null)
                        worksheet.Cells[i, 4].Value = tv.ChucVu;

                    if (tv.BirthDay != null)
                        worksheet.Cells[i, 5].Value = tv.BirthDay.Value.ToString("dd/MM/yy");

                    //6.GCNKNCM
                    if (tv.Qualification != null)
                        worksheet.Cells[i, 6].Value = tv.Qualification;

                    //7. TDCM
                    if (tv.EducationName != null)
                        worksheet.Cells[i, 7].Value = tv.EducationName;

                    ////8. NgayXuongTau
                    //if (tv.NgayXuongTau.HasValue)
                    //    worksheet.Cells[i, 8].Value = tv.NgayXuongTau.Value.ToString("dd/MM/yy");

                    //9. So ngay tai tau
                    //if (tv.TGDT != null)
                    //    worksheet.Cells[i, 9].Value = tv.TGDT;

                    //10. Tau da cong tac
                    if (tv.QTDT != null)
                        worksheet.Cells[i, 10].Value = tv.QTDT;

                    //11. Ghi chu
                    if (tv.Note != null)
                        worksheet.Cells[i, 14].Value = tv.Note;

                    //12. Ngay bo nhiem
                    if (tv.DecisionDate != null)
                        worksheet.Cells[i, 13].Value = tv.DecisionDate.Value.ToString("dd/MM/yy");
                    
                    i++;
                }

                using (ExcelRange r = worksheet.Cells[i + 1, 10, i + 1, 13])
                {                    
                    r.Merge = true;
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }               
                worksheet.Cells[i + 1, 10].Value = "TRUNG TÂM THUYỀN VIÊN";

                using (ExcelRange r = worksheet.Cells[12, 1, i - 1, 14])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

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

        public FileResult XuatDanhSachBangCapTheoThuyenVien(int EmployeeID)
        {

            //IQueryable<viewHRM_EMPLOYMENTHISTORY> ds
            //int departmentID = 10;
            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = ServerPath;

            FileInfo templateFile = new FileInfo(server + "//Mau_BCCC.xltx");
            FileInfo newFile = new FileInfo(outputDir.FullName + @"\Mau_BCCC.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\Mau_BCCC.xlsx");
            }
            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                //worksheet.Cells[2, 12].Value = "Cập nhật ngày " + DateTime.Now.ToString("dd/MM/yyyy");

                AuLacEntities dc = new AuLacEntities();

                viewHRM_EMPLOYMENTHISTORY thuyenvien = (from p in dc.viewHRM_EMPLOYMENTHISTORY
                                                        where p.EmployeeID == EmployeeID
                                                        select p).SingleOrDefault();

                // 1. Thông tin thuyền viên
                worksheet.Cells[6, 5].Value = thuyenvien.FirstName + " " + thuyenvien.LastName;

                if (thuyenvien.BirthDay != null)
                    worksheet.Cells[6, 9].Value = Convert.ToDateTime(thuyenvien.BirthDay).ToString("dd/MM/yyyy");

                worksheet.Cells[6, 13].Value = thuyenvien.PositionName;

                var listBC = from p in dc.DIC_DEGREE
                             where p.STT < 43
                             orderby p.STT
                             select p;
                int k = 0;
                foreach (DIC_DEGREE b in listBC)
                {
                    //--Basic Traning Certificate
                    var bC = (from p in dc.HRM_EMPLOYEE_DEGREE
                              where p.EmployeeID == EmployeeID && p.DegreeID == b.DegreeID
                              select p).SingleOrDefault();

                    if (bC != null)
                    {
                        if(bC.DegreeDate != null)
                            worksheet.Cells[8 + k, 2].Value = "X";

                        //worksheet.Cells[8 + k, 16].Value = bC.DIC_DEGREE.DegreeName;
                        if (bC.DegreeNo != null)
                            worksheet.Cells[8 + k, 9].Value = bC.DegreeNo;
                        if (bC.DegreeDate != null)
                            worksheet.Cells[8 + k, 11].Value = Convert.ToDateTime(bC.DegreeDate).ToString("dd/MM/yyyy");
                        if (bC.ExpirationDate != null)
                            worksheet.Cells[8 + k, 14].Value = Convert.ToDateTime(bC.ExpirationDate).ToString("dd/MM/yyyy");
                    }
                    k++;
                }

                var hopDong = (from p in db.viewHopDongGanNhats
                          where p.EmployeeID == EmployeeID
                          select p).FirstOrDefault();

                if (hopDong != null)
                {
                    worksheet.Cells[8 + k, 2].Value = "X";

                    //worksheet.Cells[8 + k, 16].Value = bC.DIC_DEGREE.DegreeName;
                    if (hopDong.ContractNo != null)
                        worksheet.Cells[8 + k, 9].Value = hopDong.ContractNo;
                    if (hopDong.ContractDate != null)
                        worksheet.Cells[8 + k, 11].Value = Convert.ToDateTime(hopDong.ContractDate).ToString("dd/MM/yyyy");
                    if (hopDong.ExpirationDate != null)
                        worksheet.Cells[8 + k, 14].Value = Convert.ToDateTime(hopDong.ExpirationDate).ToString("dd/MM/yyyy");
                }

                k++;

                ///////////////////////////////////////////////////////
                worksheet.Cells[57, 2].Value = thuyenvien.FirstName + " " + thuyenvien.LastName;
                worksheet.Cells[57, 10].Value = thuyenvien.DepartmentName;
                worksheet.Cells[55, 10].Value = DateTime.Now.ToString("dd/MM/yyyy");




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

        public FileResult XuatQuyetDinhTuyenDung(int ID_QTCT)
        {
            //int ID_QTCT = 2930;//2930


            AuLacEntities dc = new AuLacEntities();

            MemoryStream stream = new MemoryStream();
            string server = ServerPath;
            string fileNameTemplate = "";
            DocX doc = DocX.Create(stream);

            view_quatrinhcongtacFull vemhis = (from p in dc.view_quatrinhcongtacFull
                                               where p.EmploymentHistoryID == ID_QTCT
                                               select p).SingleOrDefault();

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
                doc.ReplaceText("%ChucVu%", vemhis.ChucVu.ToUpper());  
           
            doc.SaveAs(stream);
            return File(stream.ToArray(), "application/octet-stream", "QDTD.docx");
        }

        public FileResult XuatDanhSachBangCapTheoID(int DegreeID)
        {            
            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = ServerPath;
            FileInfo templateFile;
            FileInfo newFile;
            templateFile = new FileInfo(server + "//Mau_BangCapThuyenVien.xltx");
            newFile = new FileInfo(outputDir.FullName + @"\Mau_BangCapThuyenVien.xlsx");

            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\Mau_BangCapThuyenVien.xlsx");
            }

            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                String degreeName = (from p in db.DIC_DEGREE
                                     where p.DegreeID == DegreeID
                                     select p.DegreeName).SingleOrDefault();

                worksheet.Cells[2, 1].Value = degreeName.ToUpper();

                //****************************************
                //2. NOI DUNG
                int i = 5;
                int stt = 1;               

                var listTV = db.sp_LayDSBangCapTheoID(DegreeID).ToList();

                foreach (var tv in listTV)
                {
                    worksheet.Cells[i, 1].Value = stt;
                    stt++;

                    worksheet.Cells[i, 2].Value = tv.FirstName;
                    worksheet.Cells[i, 3].Value = tv.LastName;
                    if (tv.BirthDay != null)
                        worksheet.Cells[i, 4].Value = tv.BirthDay.Value.ToString("dd/MM/yy");
                    if (tv.DepartmentName != null)
                        worksheet.Cells[i, 5].Value = tv.DepartmentName;
                    if (tv.ChucVu != null)
                        worksheet.Cells[i, 6].Value = tv.ChucVu;                    
                    if (tv.DegreeNo != null)
                        worksheet.Cells[i, 7].Value = tv.DegreeNo;                    
                    if (tv.DegreeDate != null)
                        worksheet.Cells[i, 8].Value = tv.DegreeDate.Value.ToString("dd/MM/yy");                    
                    if (tv.ExpirationDate!=null)
                        worksheet.Cells[i, 9].Value = tv.ExpirationDate.Value.ToString("dd/MM/yy");

                    worksheet.Row(i).Height = 28;
                    i++;
                }

                using (ExcelRange r = worksheet.Cells[4, 1, i - 1, 9])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;                    
                }

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

        public FileResult XuatTatCaBangCapThuyenVien()
        {
            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = ServerPath;
            FileInfo templateFile;
            FileInfo newFile;
            templateFile = new FileInfo(server + "//Mau_TatCaBangCapThuyenVien.xltx");
            newFile = new FileInfo(outputDir.FullName + @"\Mau_TatCaBangCapThuyenVien.xlsx");

            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\Mau_TatCaBangCapThuyenVien.xlsx");
            }

            int DegreeID =1;
            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                String degreeName = (from p in db.DIC_DEGREE
                                     where p.DegreeID == DegreeID
                                     select p.DegreeName).SingleOrDefault();

                worksheet.Cells[2, 1].Value = degreeName.ToUpper();

                //****************************************
                //2. NOI DUNG
                int i = 3;
                int stt = 1;

                //var listTV = from p in db.viewNhanViens
                //             where (p.ParentID == 8 || p.ParentID == 17) && p.DepartmentID == 10
                //             orderby p.PositionID, p.DepartmentID
                //             select p;

                var listBC = from p in db.DIC_DEGREE
                             //where p.STT != null
                             orderby p.DegreeID
                             select p;

                var listCV_TV = from p in db.DIC_POSITION
                                where p.GroupPositionID != 4
                             orderby p.PositionID
                             select p;

                int vt_bc = 0;
                foreach (var objBC in listBC)
                {
                    worksheet.Cells[2, 10 + vt_bc * 3].Value = objBC.DegreeName;
                    worksheet.Cells[2, 10 + vt_bc * 3 + 1].Value = "Ngày cấp";
                    worksheet.Cells[2, 10 + vt_bc * 3 + 2].Value = "Ngày hết hạn";
                    vt_bc++;
                }

                int thutuChucVu = 0;
                foreach (var cvtv in listCV_TV)
                {                    
                    var listTV = from p in db.viewNhanViens
                                 where (p.ParentID == 8 || p.ParentID == 17) && p.PositionID == cvtv.PositionID
                                 orderby p.DepartmentID
                                 select p;

                    System.Drawing.Color cl;
                    //range.Style.Font.Color.SetColor(System.Drawing.Color.Red);
                    //range.Interior.ColorIndex = 0;

                    if (thutuChucVu % 2 == 0)                  
                        cl = System.Drawing.Color.AliceBlue;
                    else
                        cl = System.Drawing.Color.Yellow;

                    thutuChucVu++;

                    foreach (var tv in listTV)
                    {
                        int EmployeeID = tv.EmployeeID;

                        worksheet.Cells[i, 1].Value = stt;
                        stt++;

                        worksheet.Cells[i, 2].Value = tv.FirstName + " " + tv.LastName;
                        if (tv.ChucVu != null)
                            worksheet.Cells[i, 3].Value = tv.ChucVu;
                        if (tv.BirthDay != null)
                        {
                            worksheet.Cells[i, 4].Value = tv.BirthDay.Value.ToString("dd/MM/yy");
                            worksheet.Cells[i, 5].Value = tv.BirthDay.Value.Month;
                            worksheet.Cells[i, 7].Value = (DateTime.Now.Year - tv.BirthDay.Value.Year + 1).ToString();
                        }

                        if (tv.EmployeeCode != null)
                            worksheet.Cells[i, 6].Value = tv.EmployeeCode;

                        // Ngày hợp đồng và ngày tuyển dụng
                        //if (tv.DegreeDate != null) ///
                        //    worksheet.Cells[i, 8].Value = tv.DegreeDate.Value.ToString("dd/MM/yy");
                        //if (tv.ExpirationDate != null)
                        //    worksheet.Cells[i, 9].Value = tv.ExpirationDate.Value.ToString("dd/MM/yy");

                        var listBC_TV = (from p in db.HRM_EMPLOYEE_DEGREE
                                         where p.EmployeeID == EmployeeID
                                         orderby p.DegreeID
                                         select p).ToList();

                        int cot_dautien = 10;

                        int k = 0;
                        int tt = 0;
                        if (listBC_TV.Count() != 0)
                        {
                            foreach (var bc in listBC)
                            {
                                int bcID = bc.DegreeID;
                                if (bcID == listBC_TV.ElementAt(k).DegreeID)
                                {
                                    //in ra
                                    if (listBC_TV.ElementAt(k).DegreeNo != null)
                                        worksheet.Cells[i, cot_dautien + tt * 3].Value = listBC_TV.ElementAt(k).DegreeNo;
                                    if (listBC_TV.ElementAt(k).DegreeDate != null)
                                        worksheet.Cells[i, cot_dautien + tt * 3 + 1].Value = listBC_TV.ElementAt(k).DegreeDate.Value.ToString("dd/MM/yyyy");
                                    if (listBC_TV.ElementAt(k).ExpirationDate != null)
                                        worksheet.Cells[i, cot_dautien + tt * 3 + 2].Value = listBC_TV.ElementAt(k).ExpirationDate.Value.ToString("dd/MM/yyyy");

                                    k++;
                                    if (k >= listBC_TV.Count())
                                        break;

                                }
                                else
                                {
                                    // nhảy qua 3 ô
                                }

                                tt++;
                            }
                        }
                        

                        worksheet.Row(i).Height = 28;
                        ExcelRange r = worksheet.Cells[i, 1, i, 10 + listBC_TV.Count() * 3];
                        r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(cl);


                        i++;
                    }
                }

                

                //using (ExcelRange r = worksheet.Cells[3, 1, i - 1, 9])
                //{
                //    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                //    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                //    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                //}

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

        public FileResult XuatDSThuyenVienTheoLoaiChucDanh(int loaichucdanhID)
        {
            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = ServerPath;
            FileInfo templateFile;
            FileInfo newFile;
            templateFile = new FileInfo(server + "//Mau_DSTVTheoChucDanh.xltx");
            newFile = new FileInfo(outputDir.FullName + @"\Mau_DSTVTheoChucDanh.xlsx");

            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\Mau_DSTVTheoChucDanh.xlsx");
            }

            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                worksheet.Cells[2, 10].Value = "Cập nhật ngày " + DateTime.Now.ToString("dd/MM/yyyy");

                //****************************************
                //2. NOI DUNG
                int i = 4;
                int stt = 1;
                int departmentID = 0;

                var listTV = db.sp_LayDSThuyenVienTheoLoaiChucDanh(loaichucdanhID).ToList();

                foreach (var tv in listTV)
                {                    

                    worksheet.Cells[i, 1].Value = stt;
                    stt++;

                    worksheet.Cells[i, 2].Value = tv.FirstName + ' ' + tv.LastName;
                    worksheet.Cells[i, 3].Value = tv.DepartmentName;
                    if (tv.ChucVu != null)
                        worksheet.Cells[i, 4].Value = tv.ChucVu;
                    if (tv.BirthDay != null)
                        worksheet.Cells[i, 5].Value = tv.BirthDay.Value.ToString("dd/MM/yy");
                    //6.GCNKNCM
                    if (tv.Qualification != null)
                        worksheet.Cells[i, 6].Value = tv.Qualification;
                    //7. TDCM
                    if (tv.EducationName != null)
                        worksheet.Cells[i, 7].Value = tv.EducationName;
                    //8. NgayXuongTau
                    if (tv.NgayXuongTau.HasValue)
                        worksheet.Cells[i, 8].Value = tv.NgayXuongTau.Value.ToString("dd/MM/yy");
                    //9. So ngay tai tau
                    worksheet.Cells[i, 9].Value = tv.TGDT;
                    //10. Tau da cong tac
                    worksheet.Cells[i, 10].Value = tv.QTDT;

                    i++;
                }

                using (ExcelRange r = worksheet.Cells[4, 1, i - 1, 10])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

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

        public FileResult XuatDSSinhNhat(int thang, int loaiNhanVien)
        {
            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = ServerPath;
            FileInfo templateFile;
            FileInfo newFile;
            templateFile = new FileInfo(server + "//Mau_DSSinhNhat.xltx");
            newFile = new FileInfo(outputDir.FullName + @"\Mau_DSSinhNhat.xlsx");

            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\Mau_DSSinhNhat.xlsx");
            }

            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                worksheet.Cells[2, 10].Value = "Cập nhật ngày " + DateTime.Now.ToString("dd/MM/yyyy");

                //****************************************
                //2. NOI DUNG
                int i = 6;
                int stt = 1;
                int departmentID = 0;

                //INumeric
                IOrderedEnumerable<sp_T_LayDanhSachNhanVien_Result> listTV;

                if (loaiNhanVien == 1)                                   
                    listTV = db.sp_T_LayDanhSachNhanVien().Where(x => x.ParentDepartment == 1 && x.StatusID == 1 && x.BirthDay.HasValue && x.BirthDay.Value.Month == thang).OrderBy(x => x.BirthDay.Value.Day);                
                else if (loaiNhanVien == 2)
                    listTV = db.sp_T_LayDanhSachNhanVien().Where(x => x.StatusID == 1 && x.BirthDay.HasValue && x.BirthDay.Value.Month == thang && (x.ParentDepartment == 8 || x.ParentDepartment == 17)).OrderBy(x => x.BirthDay.Value.Day);
                else 
                    listTV = db.sp_T_LayDanhSachNhanVien().Where(x => x.StatusID == 1 && x.BirthDay.HasValue && x.BirthDay.Value.Month == thang).OrderBy(x => x.BirthDay.Value.Day);
                
                foreach (var tv in listTV)
                {
                    worksheet.Cells[i, 1].Value = stt;
                    stt++;

                    worksheet.Cells[i, 2].Value = tv.FirstName;
                    worksheet.Cells[i, 3].Value = tv.LastName;
                    if (tv.BirthDay != null)
                        worksheet.Cells[i, 4].Value = tv.BirthDay.Value.ToString("dd/MM/yy");
                    worksheet.Cells[i, 5].Value = tv.DepartmentName;
                    if (tv.ChucVu != null)
                        worksheet.Cells[i, 6].Value = tv.ChucVu;

                    HRM_CONTRACTHISTORY hopDong = db.HRM_CONTRACTHISTORY.Where(x => x.EmployeeID == tv.EmployeeID).FirstOrDefault();
                    if (hopDong != null)
                        worksheet.Cells[i, 7].Value = hopDong.ContractDate.Value.ToString("dd/MM/yy");

                    i++;
                }

                using (ExcelRange r = worksheet.Cells[6, 1, i - 1, 7])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

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

        public FileResult XuatBangTheoDoiGiayToTau(DateTime ngay)
        {
            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = ServerPath;
            FileInfo templateFile;
            FileInfo newFile;
            templateFile = new FileInfo(server + "//Mau_TheoDoiGiayToTau.xltx");
            newFile = new FileInfo(outputDir.FullName + @"\Mau_TheoDoiGiayToTau.xlsx");

            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\Mau_TheoDoiGiayToTau.xlsx");
            }

            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                //worksheet.Cells[2, 10].Value = "Cập nhật ngày " + DateTime.Now.ToString("dd/MM/yyyy");

                ExcelRange r1 = worksheet.Cells[4, 3, 4, 12 - ngay.Month + 3];
                r1.Merge = true;
                r1.Value = "NĂM " + ngay.Year.ToString();

                int k1 ;
                for (k1 = ngay.Month; k1 <= 12; k1++)
                    worksheet.Cells[5, 3 + k1 - ngay.Month].Value = k1;

                for (int k2 = 1; k2 < 15 - 12 + ngay.Month; k2++)
                    worksheet.Cells[5, 3 + k1 - ngay.Month + k2 - 1].Value = k2;

                ExcelRange r2 = worksheet.Cells[4, 12 - ngay.Month + 4, 4, 12 - ngay.Month + 12];
                r2.Merge = true;
                r2.Value = "NĂM " + (ngay.Year + 1).ToString();

                //****************************************
                //2. NOI DUNG
                int i = 6;
                int stt = 1;
                int departmentID = 0;

                var listTV = db.sp_BangTheoDoiGiayToTau(ngay).ToList();

                foreach (var tv in listTV)
                {

                    worksheet.Cells[i, 1].Value = stt;
                    stt++;

                    worksheet.Cells[i, 2].Value = tv.TenChungChi;
                    
                    if (tv.Thang1 != null)
                        worksheet.Cells[i, 3].Value = tv.Thang1;
                    if (tv.Thang2 != null)
                        worksheet.Cells[i, 4].Value = tv.Thang2;
                    if (tv.Thang3 != null)
                        worksheet.Cells[i, 5].Value = tv.Thang3;

                    if (tv.Thang4 != null)
                        worksheet.Cells[i, 6].Value = tv.Thang4;
                    if (tv.Thang5 != null)
                        worksheet.Cells[i, 7].Value = tv.Thang5;
                    if (tv.Thang6 != null)
                        worksheet.Cells[i, 8].Value = tv.Thang6;

                    if (tv.Thang7 != null)
                        worksheet.Cells[i, 9].Value = tv.Thang7;
                    if (tv.Thang8 != null)
                        worksheet.Cells[i, 10].Value = tv.Thang8;
                    if (tv.Thang9 != null)
                        worksheet.Cells[i, 11].Value = tv.Thang9;

                    if (tv.Thang10 != null)
                        worksheet.Cells[i, 12].Value = tv.Thang10;
                    if (tv.Thang11 != null)
                        worksheet.Cells[i, 13].Value = tv.Thang11;
                    if (tv.Thang12 != null)
                        worksheet.Cells[i, 14].Value = tv.Thang12;

                    if (tv.Thang13 != null)
                        worksheet.Cells[i, 15].Value = tv.Thang13;
                    if (tv.Thang14 != null)
                        worksheet.Cells[i, 16].Value = tv.Thang14;
                    if (tv.Thang15 != null)
                        worksheet.Cells[i, 17].Value = tv.Thang15;
                    
                    i++;
                }

                using (ExcelRange r = worksheet.Cells[6, 1, i - 1, 17])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

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
