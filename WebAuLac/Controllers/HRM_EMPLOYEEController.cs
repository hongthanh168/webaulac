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
using System.Drawing;
using OfficeOpenXml.Drawing;

namespace HRM.Controllers
{
   
    public class HRM_EMPLOYEEController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        public JsonResult LayHuyen(int tinhID)
        {
            List<SelectListItem> doiTuongs = new List<SelectListItem>();
            doiTuongs.Add(new SelectListItem() { Value = "", Text = "--Chọn--"});
            var huyens = db.DIC_QUANHUYEN.Where(x => x.TinhThanhPhoID==tinhID);
            foreach (var item in huyens)
            {
                SelectListItem objSel = new SelectListItem() { Value = item.QuanHuyenID.ToString(), Text = item.QuanHuyenName };
                doiTuongs.Add(objSel);
            }
            return Json(doiTuongs, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LayXa(int huyenID)
        {
            List<SelectListItem> doiTuongs = new List<SelectListItem>();
            doiTuongs.Add(new SelectListItem() { Value = "", Text = "--Chọn--" });
            var xas = db.DIC_PHUONGXA.Where(x => x.QuanHuyenID == huyenID);
            foreach (var item in xas)
            {
                SelectListItem objSel = new SelectListItem() { Value = item.PhuongXaID.ToString(), Text = item.PhuongXaName };
                doiTuongs.Add(objSel);
            }
            return Json(doiTuongs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ThongKeTVienNghiViec()
        {
            TuNgayDenNgay obj = new TuNgayDenNgay();
            obj.TuNgay = new DateTime(DateTime.Now.Year, 1, 1);
            obj.DenNgay = new DateTime(DateTime.Now.Year, 12, 31);
            var result = db.sp_LayDSNghiViec(obj.TuNgay, obj.DenNgay,1).OrderBy(x => x.PositionID).ToList();
            ViewBag.BangKe = result;
            return View(obj);
        }
        public ActionResult ThongKeNVienNghiViec()
        {
            TuNgayDenNgay obj = new TuNgayDenNgay();
            obj.TuNgay = new DateTime(DateTime.Now.Year, 1, 1);
            obj.DenNgay = new DateTime(DateTime.Now.Year, 12, 31);
            var result = db.sp_LayDSNghiViec(obj.TuNgay, obj.DenNgay,2).OrderBy(x => x.PositionID).ToList();
            ViewBag.BangKe = result;
            return View(obj);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongKeTVienNghiViec([Bind(Include = "TuNgay,DenNgay")] TuNgayDenNgay obj)
        {
            if (ModelState.IsValid)
            {
                var result = db.sp_LayDSNghiViec(obj.TuNgay, obj.DenNgay,1).ToList();                           
                ViewBag.BangKe = result;
                return View(obj);
            }
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongKeNVienNghiViec([Bind(Include = "TuNgay,DenNgay")] TuNgayDenNgay obj)
        {
            if (ModelState.IsValid)
            {
                var result = db.sp_LayDSNghiViec(obj.TuNgay, obj.DenNgay, 2).ToList();
                ViewBag.BangKe = result;
                return View(obj);
            }
            return View();
        }

        public ActionResult ThongKeThuyenVienHuyTuyenDung()
        {
            TuNgayDenNgay obj = new TuNgayDenNgay();
            obj.TuNgay = new DateTime(DateTime.Now.Year, 1, 1);
            obj.DenNgay = new DateTime(DateTime.Now.Year, 12, 31);
            var result = db.sp_LayDSThuyenVienHuyTuyenDung(obj.TuNgay, obj.DenNgay).Where(x => x.ParentID == 17 || x.ParentID == 8).OrderBy(x => x.PositionID).ToList();
            ViewBag.BangKe = result;
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongKeThuyenVienHuyTuyenDung([Bind(Include = "TuNgay,DenNgay")] TuNgayDenNgay obj)
        {
            if (ModelState.IsValid)
            {
                var result = db.sp_LayDSThuyenVienHuyTuyenDung(obj.TuNgay, obj.DenNgay).Where(x => x.ParentID == 17 || x.ParentID == 8).OrderBy(x => x.PositionID).ToList(); ;
                ViewBag.BangKe = result;
                return View(obj);
            }
            return View();
        }

        //Thống kê bổ nhiệm
        public ActionResult ThongKeBoNhiemTV()
        {
            TuNgayDenNgay obj = new TuNgayDenNgay();
            obj.TuNgay = new DateTime(DateTime.Now.Year, 1, 1);
            obj.DenNgay = new DateTime(DateTime.Now.Year, 12, 31);
            var result = db.sp_LayDSBoNhiem(obj.TuNgay, obj.DenNgay).Where(x=>x.ParentID==17||x.ParentID==8).OrderBy(x => x.PositionID).ToList();
            ViewBag.BangKe = result;
            return View(obj);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongKeBoNhiemTV([Bind(Include = "TuNgay,DenNgay")] TuNgayDenNgay obj)
        {
            if (ModelState.IsValid)
            {
                var result = db.sp_LayDSBoNhiem(obj.TuNgay, obj.DenNgay).Where(x => x.ParentID == 17 || x.ParentID == 8).OrderBy(x => x.PositionID).ToList(); ;
                ViewBag.BangKe = result;
                return View(obj);
            }
            return View();
        }
        public ActionResult ThongKeBoNhiemNV()
        {
            TuNgayDenNgay obj = new TuNgayDenNgay();
            obj.TuNgay = new DateTime(DateTime.Now.Year, 1, 1);
            obj.DenNgay = new DateTime(DateTime.Now.Year, 12, 31);
            var result = db.sp_LayDSBoNhiem(obj.TuNgay, obj.DenNgay).Where(x => x.ParentID == 1).OrderBy(x => x.PositionID).ToList();
            ViewBag.BangKe = result;
            return View(obj);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongKeBoNhiemNV([Bind(Include = "TuNgay,DenNgay")] TuNgayDenNgay obj)
        {
            if (ModelState.IsValid)
            {
                var result = db.sp_LayDSBoNhiem(obj.TuNgay, obj.DenNgay).Where(x => x.ParentID == 1).OrderBy(x => x.PositionID).ToList(); ;
                ViewBag.BangKe = result;
                return View(obj);
            }
            return View();
        }
        //Thống kê kỷ luật
        public ActionResult ThongKeKyLuatNV()
        {
            TuNgayDenNgay obj = new TuNgayDenNgay();
            obj.TuNgay = new DateTime(DateTime.Now.Year, 1, 1);
            obj.DenNgay = new DateTime(DateTime.Now.Year, 12, 31);
            var result = db.sp_LayDSKyLuat(obj.TuNgay, obj.DenNgay).Where(x => x.ParentID == 1 || x.ParentID == null).ToList();
            ViewBag.BangKe = result;
            return View(obj);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongKeKyLuatNV([Bind(Include = "TuNgay,DenNgay")] TuNgayDenNgay obj)
        {
            if (ModelState.IsValid)
            {
                var result = db.sp_LayDSKyLuat(obj.TuNgay, obj.DenNgay).Where(x => x.ParentID == 1 || x.ParentID == null).ToList(); ;
                ViewBag.BangKe = result;
                return View(obj);
            }
            return View();
        }
        public ActionResult ThongKeKyLuatTV()
        {
            TuNgayDenNgay obj = new TuNgayDenNgay();
            obj.TuNgay = new DateTime(DateTime.Now.Year, 1, 1);
            obj.DenNgay = new DateTime(DateTime.Now.Year, 12, 31);
            var result = db.sp_LayDSKyLuat(obj.TuNgay, obj.DenNgay).Where(x => x.ParentID == 1 || x.ParentID == null).ToList();
            ViewBag.BangKe = result;
            return View(obj);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongKeKyLuatTV([Bind(Include = "TuNgay,DenNgay")] TuNgayDenNgay obj)
        {
            if (ModelState.IsValid)
            {
                var result = db.sp_LayDSKyLuat(obj.TuNgay, obj.DenNgay).Where(x => x.ParentID == 1 || x.ParentID == null).ToList(); ;
                ViewBag.BangKe = result;
                return View(obj);
            }
            return View();
        }
        public FileResult XuatDanhSachNghiViec(DateTime tungay, DateTime denngay, int loaifile)
        {
            XuatWordExcelController ctrl = new XuatWordExcelController();
            ctrl.ServerPath = Server.MapPath("~/App_Data");
            ctrl.AppUser = User;
            return ctrl.XuatDanhSachNghiViec(tungay,denngay,loaifile);
        }

        public FileResult XuatDanhSachBoNhiem(DateTime tungay, DateTime denngay, int loaifile)
        {
            XuatWordExcelController ctrl = new XuatWordExcelController();
            ctrl.ServerPath = Server.MapPath("~/App_Data");
            ctrl.AppUser = User;
            return ctrl.XuatDanhSachBoNhiem(tungay, denngay, 1);
        }

        public ActionResult ThongKeNVienTuyenDung()
        {
            TuNgayDenNgay obj = new TuNgayDenNgay();
            obj.TuNgay = new DateTime(DateTime.Now.Year, 1, 1);
            obj.DenNgay = new DateTime(DateTime.Now.Year, 12, 31);
            var result = db.sp_LayDSTuyenDung(obj.TuNgay, obj.DenNgay,2).OrderBy(x=>x.PositionID).ToList();
            ViewBag.BangKe = result;
            return View(obj);
        }
        public ActionResult ThongKeTVienTuyenDung()
        {
            TuNgayDenNgay obj = new TuNgayDenNgay();
            obj.TuNgay = new DateTime(DateTime.Now.Year, 1, 1);
            obj.DenNgay = new DateTime(DateTime.Now.Year, 12, 31);
            var result = db.sp_LayDSTuyenDung(obj.TuNgay, obj.DenNgay,1).OrderBy(x => x.PositionID).ToList();
            ViewBag.BangKe = result;
            return View(obj);
        }
        public ActionResult ThongKeTuyenDung()
        {
            TuNgayDenNgay obj = new TuNgayDenNgay();
            obj.TuNgay = new DateTime(DateTime.Now.Year, 1, 1);
            obj.DenNgay = new DateTime(DateTime.Now.Year, 12, 31);
            var result = db.sp_LayDSTuyenDung(obj.TuNgay, obj.DenNgay,1).ToList();
            ViewBag.BangKe = result;
            return View(obj);
        }
        //[Authorize(Roles = "HR, Boss")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ThongKeTuyenDung([Bind(Include = "TuNgay,DenNgay")] TuNgayDenNgay obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = db.sp_LayDSTuyenDung(obj.TuNgay, obj.DenNgay).ToList();
        //        ViewBag.BangKe = result;
        //        return View(obj);
        //    }
        //    return View();
        //}
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongKeTVienTuyenDung([Bind(Include = "TuNgay,DenNgay")] TuNgayDenNgay obj)
        {
            if (ModelState.IsValid)
            {
                var result = db.sp_LayDSTuyenDung(obj.TuNgay, obj.DenNgay,1).ToList();
                ViewBag.BangKe = result;
                return View(obj);
            }
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongKeNVienTuyenDung([Bind(Include = "TuNgay,DenNgay")] TuNgayDenNgay obj)
        {
            if (ModelState.IsValid)
            {
                var result = db.sp_LayDSTuyenDung(obj.TuNgay, obj.DenNgay,2).ToList();
                ViewBag.BangKe = result;
                return View(obj);
            }
            return View();
        }
        public FileResult XuatDanhSachTuyenDung(DateTime tungay, DateTime denngay, int loaifile)
        {
            XuatWordExcelController ctrl = new XuatWordExcelController();
            ctrl.ServerPath = Server.MapPath("~/App_Data");
            ctrl.AppUser = User;
            return ctrl.XuatDanhSachTuyenDung(tungay, denngay, loaifile);
        }


        // GET: HRM_EMPLOYEE
        //Bỏ chức năng GiamDocIndex
        //Dùng chung trong Index và tùy vào quyền mà hiển thị khác nhau
        [Authorize(Roles ="Boss")]
        public ActionResult GiamDocIndex()
        {
            var hRM_EMPLOYEE = db.sp_T_LayDanhSachNhanVien().Where(x => x.StatusID == 1).OrderBy(x => x.ParentDepartment).ThenBy(x => x.DepartmentID).ThenBy(x => x.PositionID).ThenBy(x => x.LastName).ThenBy(x => x.FirstName);
            ViewBag.StatusID = new SelectList(db.DIC_STATUS, "StatusID", "StatusName");
            return View(hRM_EMPLOYEE.ToList());
        }
        public ActionResult Index()
        {
            var hRM_EMPLOYEE = db.sp_T_LayDanhSachNhanVien().Where(x => x.StatusID==1).OrderBy(x => x.ParentDepartment).ThenBy(x => x.DepartmentID).ThenBy(x => x.PositionID).ThenBy(x => x.LastName).ThenBy(x => x.FirstName);
            return View(hRM_EMPLOYEE.ToList());
        }
        public ActionResult DSUngVien()
        {
            var hRM_EMPLOYEE = db.sp_T_LayDanhSachNhanVien().Where(x => x.StatusID == 6).OrderBy(x => x.LastName).ThenBy(x => x.FirstName);
            return View(hRM_EMPLOYEE.ToList());
        }
        public ActionResult SinhNhat()
        {
            int thang = DateTime.Now.Month;
            var hRM_EMPLOYEE = db.sp_T_LayDanhSachNhanVien().Where(x => x.StatusID == 1 && x.BirthDay.HasValue && x.BirthDay.Value.Month==thang).OrderBy(x => x.BirthDay.Value.Day);
            ViewBag.bangKe = hRM_EMPLOYEE.ToList();
            ViewBag.thang = thang;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SinhNhat(int thang, int loaiNhanVien, string answer)
        {
            switch (answer)
            {
                case "xemthongtin":
                    if (loaiNhanVien == 1)
                    {
                        var hRM_EMPLOYEE = db.sp_T_LayDanhSachNhanVien().Where(x => x.ParentDepartment == 1 && x.StatusID == 1 && x.BirthDay.HasValue && x.BirthDay.Value.Month == thang).OrderBy(x => x.BirthDay.Value.Day);
                        ViewBag.bangKe = hRM_EMPLOYEE.ToList();
                    }
                    else if (loaiNhanVien == 2)
                    {
                        var hRM_EMPLOYEE = db.sp_T_LayDanhSachNhanVien().Where(x => x.StatusID == 1 && x.BirthDay.HasValue && x.BirthDay.Value.Month == thang && (x.ParentDepartment == 8 || x.ParentDepartment == 17)).OrderBy(x => x.BirthDay.Value.Day);
                        ViewBag.bangKe = hRM_EMPLOYEE.ToList();
                    }
                    else
                    {
                        var hRM_EMPLOYEE = db.sp_T_LayDanhSachNhanVien().Where(x => x.StatusID == 1 && x.BirthDay.HasValue && x.BirthDay.Value.Month == thang).OrderBy(x => x.BirthDay.Value.Day);
                        ViewBag.bangKe = hRM_EMPLOYEE.ToList();
                    }
                    ViewBag.thang = thang;
                    ViewBag.loaiNhanVien = loaiNhanVien;
                    return View();
                case "xuatexcel":
                    XuatWordExcelController ctrl = new XuatWordExcelController();
                    ctrl.ServerPath = Server.MapPath("~/App_Data");
                    ctrl.AppUser = User;
                    return ctrl.XuatDSSinhNhat(thang, loaiNhanVien);
                default:
                    break;
            }

            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int StatusID)
        {
            var hRM_EMPLOYEE = db.sp_T_LayDanhSachNhanVien().Where(x =>x.StatusID ==StatusID).OrderBy(x => x.ParentDepartment).ThenBy(x => x.DepartmentID).ThenBy(x => x.PositionID).ThenBy(x => x.LastName).ThenBy(x => x.FirstName);
            ViewBag.StatusID = new SelectList(db.DIC_STATUS, "StatusID", "StatusName", StatusID);
            return View(hRM_EMPLOYEE.ToList());
        }
        
        public ActionResult DSThuyenVien()
        {
            //lấy ra danh sách khối tàu
            List<SelectListItem> list = new List<SelectListItem>();
            //var dp = db.DIC_DEPARTMENT.Where(x => (x.ParentID == 8 || x.ParentID==17) && x.DepartmentID!=11 && x.DepartmentID!=24 && x.DepartmentID!=17).OrderBy(x=>x.ParentID);
            var query = (from x in db.DIC_DEPARTMENT
                         where (x.ParentID == 8 || x.ParentID == 17) && x.DepartmentID != 11 && x.DepartmentID != 24 && x.DepartmentID != 17
                         orderby x.ParentID
                         select new SelectListItem { Text = x.DepartmentName, Value = x.DepartmentID.ToString() }).Distinct();
            list.AddRange(query.ToList());
            //thêm giá trị dự trữ ssdieudong và sschua san sang            
            var newItem_ssdd= new SelectListItem { Text = "Dự trữ -SSĐĐ", Value = "-1" };
            list.Add(newItem_ssdd);
            var newItem_chuaSSDD = new SelectListItem { Text = "Dự trữ - chưa SSĐĐ", Value = "-2" };
            list.Add(newItem_chuaSSDD);
            ViewBag.DepartmentID = list;
            ViewBag.TauID = 0;
            var hRM_EMPLOYEE = db.sp_LayDSThuyenVien(0).OrderBy(x => x.ParentDepartment).ThenBy(x => x.DepartmentID).ThenBy(x => x.PositionID).ThenByDescending(x => x.SoNgay);
            return View(hRM_EMPLOYEE.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DSThuyenVien(int? DepartmentID)
        {
            //lấy ra danh sách khối tàu
            List<sp_LayDSThuyenVien_Result> hRM_EMPLOYEE=null;
            if (DepartmentID.HasValue)
            {
                if (DepartmentID.Value ==-1)//dự trữ ssdd
                {
                    hRM_EMPLOYEE = db.sp_LayDSThuyenVien(0).Where(x => x.ParentID == 17 && x.SSDD.HasValue && x.SSDD.Value).OrderBy(x => x.DepartmentID). ThenBy(x => x.PositionID).ThenByDescending(x => x.SoNgay).ToList();
                    ViewBag.TauID = DepartmentID.Value;
                }
                else if (DepartmentID.Value ==-2) //dự trữ chưa ssdd
                {
                    hRM_EMPLOYEE = db.sp_LayDSThuyenVien(0).Where(x => x.ParentID ==17 && ((x.SSDD.HasValue && !x.SSDD.Value) || !x.SSDD.HasValue)).OrderBy(x => x.DepartmentID).ThenBy(x => x.PositionID).ThenByDescending(x => x.SoNgay).ToList();
                    ViewBag.TauID = DepartmentID.Value;
                }
                else
                {
                    hRM_EMPLOYEE = db.sp_LayDSThuyenVien(0).Where(x => x.DepartmentID == DepartmentID.Value).OrderBy(x => x.PositionID).ThenByDescending(x => x.SoNgay).ToList();
                    ViewBag.TauID = DepartmentID.Value;
                }                
            }
            else
            {
                hRM_EMPLOYEE = db.sp_LayDSThuyenVien(0).OrderBy(x => x.ParentDepartment).ThenBy(x => x.DepartmentID).ThenBy(x => x.PositionID).ThenByDescending(x => x.SoNgay).ToList();
                ViewBag.TauID = 0;    
            }
            //lấy ra danh sách khối tàu
            List<SelectListItem> list = new List<SelectListItem>();
            //var dp = db.DIC_DEPARTMENT.Where(x => (x.ParentID == 8 || x.ParentID==17) && x.DepartmentID!=11 && x.DepartmentID!=24 && x.DepartmentID!=17).OrderBy(x=>x.ParentID);
            var query = (from x in db.DIC_DEPARTMENT
                         where (x.ParentID == 8 || x.ParentID == 17) && x.DepartmentID != 11 && x.DepartmentID != 24 && x.DepartmentID != 17
                         orderby x.ParentID
                         select new SelectListItem { Text = x.DepartmentName, Value = x.DepartmentID.ToString() }).Distinct();
            list.AddRange(query.ToList());
            //thêm giá trị dự trữ ssdieudong và sschua san sang            
            var newItem_ssdd = new SelectListItem { Text = "Dự trữ -SSĐĐ", Value = "-1" };
            list.Add(newItem_ssdd);
            var newItem_chuaSSDD = new SelectListItem { Text = "Dự trữ - chưa SSĐĐ", Value = "-2" };
            list.Add(newItem_chuaSSDD);
            ViewBag.DepartmentID = list;
            return View(hRM_EMPLOYEE);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////
        ////////1. IN CREWCARD
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private void AddImage(ExcelWorksheet oSheet, int rowIndex, int colIndex, Bitmap image)
        {
            //Bitmap image = new Bitmap(imagePath);
            ExcelPicture excelImage = null;
            if (image != null)
            {
                excelImage = oSheet.Drawings.AddPicture("Debopam Pal", image);
                excelImage.From.Column = colIndex;
                excelImage.From.Row = rowIndex;
                excelImage.SetSize(100, 130);
                // 2x2 px space for better alignment
                excelImage.From.ColumnOff = Pixel2MTU(2);
                excelImage.From.RowOff = Pixel2MTU(2);
            }
        }

        public int Pixel2MTU(int pixels)
        {
            int mtus = pixels * 9525;
            return mtus;
        }
        public FileResult InCrewCard(int NhanVienID)
        {
            //int NhanVienID = 152;

            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = Server.MapPath("~/App_Data");

            FileInfo templateFile = new FileInfo(server + "//Mau_CREWCARD.xltx");
            FileInfo newFile = new FileInfo(outputDir.FullName + @"\CrewCard.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\CrewCard.xlsx");
            }
            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet xlSheet = package.Workbook.Worksheets[1];


                //****************************************
                //TIEUDE   
                HRM_EMPLOYEE em = (from p in db.HRM_EMPLOYEE
                                   where p.EmployeeID == NhanVienID
                                   select p).SingleOrDefault();

                xlSheet.Cells[6, 2].Value = em.FirstName + " " + em.LastName;
                if (em.BirthDay != null)
                    xlSheet.Cells[6, 5].Value = em.BirthDay.Value.ToString("dd/MM/yyyy");

                if (em.BirthPlace != null)
                    xlSheet.Cells[8, 2].Value = em.BirthPlace;

                if (em.MainAddress != null)
                    xlSheet.Cells[12, 2].Value = em.MainAddress;


                //IDCard
                if (em.IDCard != null)
                    xlSheet.Cells[22, 2].Value = em.IDCard;
                if (em.IDCardDate != null)
                    xlSheet.Cells[22, 5].Value = em.IDCardDate.Value.ToString("dd/MM/yyyy");
                if (em.IDCardPlace != null)
                    xlSheet.Cells[22, 8].Value = em.IDCardPlace;

                //Chieu cao va Can nang
                if (em.Height != null)
                    xlSheet.Cells[10, 2].Value = em.Height;
                if (em.Weight != null)
                    xlSheet.Cells[10, 5].Value = em.Weight;
                // Dien thoai va Email
                if (em.CellPhone != null)
                    xlSheet.Cells[14, 2].Value = em.CellPhone;
                if (em.Email != null)
                    xlSheet.Cells[14, 5].Value = em.Email;
                //Xuất hình ảnh 3x4
                if (em.Photo!=null)
                {
                    System.Drawing.Bitmap x = (Bitmap)((new ImageConverter()).ConvertFrom(em.Photo));

                    AddImage(xlSheet, 5, 7, x);

                }



                //Seamam passport
                HRM_EMPLOYEE_DEGREE em_seamanpassport = (from p in db.HRM_EMPLOYEE_DEGREE
                                                         where p.EmployeeID == NhanVienID && p.DegreeID == 7
                                                         select p).SingleOrDefault();
                if (em_seamanpassport != null)
                {
                    xlSheet.Cells[16, 2].Value = em_seamanpassport.DegreeNo;
                    if (em_seamanpassport.DegreeDate != null)
                        xlSheet.Cells[16, 5].Value = em_seamanpassport.DegreeDate.Value.ToString("dd/MM/yyyy");
                    if (em_seamanpassport.ExpirationDate != null)
                        xlSheet.Cells[16, 8].Value = em_seamanpassport.ExpirationDate.Value.ToString("dd/MM/yyyy");
                }


                //Passport
                HRM_EMPLOYEE_DEGREE em_passport = (from p in db.HRM_EMPLOYEE_DEGREE
                                                   where p.EmployeeID == NhanVienID && p.DegreeID == 9
                                                   select p).SingleOrDefault();

                if (em_passport != null)
                {
                    xlSheet.Cells[18, 2].Value = em_passport.DegreeNo;
                    if (em_passport.DegreeDate != null)
                        xlSheet.Cells[18, 5].Value = em_passport.DegreeDate.Value.ToString("dd/MM/yyyy");
                    if (em_passport.ExpirationDate != null)
                        xlSheet.Cells[18, 8].Value = em_passport.ExpirationDate.Value.ToString("dd/MM/yyyy");
                }


                //Seaman book
                HRM_EMPLOYEE_DEGREE em_seamanbook = (from p in db.HRM_EMPLOYEE_DEGREE
                                                     where p.EmployeeID == NhanVienID && p.DegreeID == 8
                                                     select p).SingleOrDefault();

                if (em_seamanbook != null)
                {
                    xlSheet.Cells[20, 2].Value = em_seamanbook.DegreeNo;
                    if (em_seamanbook.DegreeDate != null)
                        xlSheet.Cells[20, 5].Value = em_seamanbook.DegreeDate.Value.ToString("dd/MM/yyyy");
                    if (em_seamanbook.ExpirationDate != null)
                        xlSheet.Cells[20, 8].Value = em_seamanbook.ExpirationDate.Value.ToString("dd/MM/yyyy");
                }


                // truong tot nghiep
                if (em.SchoolID != null)
                    xlSheet.Cells[24, 2].Value = em.DIC_SCHOOL.SchoolName;
                if (em.HeDaoTaoID != null)
                    xlSheet.Cells[26, 2].Value = em.HeDaoTao.TenHeDaoTao;
                if (em.ThoiGianTotNghiep != null)
                    xlSheet.Cells[26, 8].Value = em.ThoiGianTotNghiep;

                if(em.TrinhDoAnhVanID != null)
                    xlSheet.Cells[63, 3].Value = em.TrinhDoAnhVan.TenTrinhDoAnhVan;
                if (em.TrinhDoViTinhID != null)
                    xlSheet.Cells[64, 3].Value = em.TrinhDoViTinh.TenTrinhDoViTinh;

                //Competency
                HRM_EMPLOYEE_DEGREE em_competency = (from p in db.HRM_EMPLOYEE_DEGREE
                                                     where p.EmployeeID == NhanVienID && p.DegreeID == 2
                                                     select p).SingleOrDefault();
                if (em_competency != null)
                {
                    xlSheet.Cells[34, 3].Value = em_competency.DegreeNo;
                    if (em_competency.DegreeDate != null)
                        xlSheet.Cells[34, 4].Value = em_competency.DegreeDate.Value.ToString("dd/MM/yyyy");
                    if (em_competency.ExpirationDate != null)
                        xlSheet.Cells[34, 5].Value = em_competency.ExpirationDate.Value.ToString("dd/MM/yyyy");
                    if (em_competency.SchoolID != null)
                        xlSheet.Cells[34, 6].Value = em_competency.DIC_SCHOOL.SchoolName;
                }


                var listDegree = from p in db.HRM_EMPLOYEE_DEGREE
                                 where p.EmployeeID == NhanVienID
                                 orderby p.DegreeID
                                 select p;

                int hang = 38;

                foreach (HRM_EMPLOYEE_DEGREE degree in listDegree)
                {
                    if ((degree.DegreeID != 2) && (degree.DegreeID != 7) && (degree.DegreeID != 8) && (degree.DegreeID != 9) && (degree.DegreeID <= 30))
                    {
                        xlSheet.Cells[hang, 1].Value = degree.DIC_DEGREE.DegreeName;
                        if (degree.DegreeNo != null)
                            xlSheet.Cells[hang, 3].Value = degree.DegreeNo;
                        if (degree.DegreeDate != null)
                            xlSheet.Cells[hang, 4].Value = degree.DegreeDate.Value.ToString("dd/MM/yyyy");
                        if (degree.ExpirationDate != null)
                            xlSheet.Cells[hang, 5].Value = degree.ExpirationDate.Value.ToString("dd/MM/yyyy");
                        if (degree.SchoolID != null)
                            xlSheet.Cells[hang, 6].Value = degree.DIC_SCHOOL.SchoolName;
                        hang++;
                    }
                }

                hang = 69;
                var listQTCT = from p in db.view_quatrinhcongtacFull
                               where p.EmployeeID == NhanVienID && p.XacNhan == true
                               orderby p.DecisionDate
                               select p;


                Boolean flag = false;
                Boolean flag_tanker = false;
                Boolean dau = false;
                int stt = 0;
                DateTime tungay = DateTime.Now;
                DateTime denngay;

                DateTime tungay_tanker = DateTime.Now;
                DateTime denngay_tanker = DateTime.Now;

                int tongsongay = 0;
                int tongsongay_tanker = 0;

                foreach (view_quatrinhcongtacFull qtct in listQTCT)
                {
                    stt++;

                    if (qtct.ParentID != 17)
                    {
                        



                        xlSheet.Cells[hang, 1].Value = qtct.DepartmentName;
                        //Loai Tau
                        if (qtct.ParentID == 8)
                        {
                            var loaiTauID = (from p in db.DIC_DEPARTMENT
                                             where p.DepartmentID == qtct.DepartmentID
                                             select p.TypeOfVessel).SingleOrDefault();

                            if (loaiTauID != null)
                            {
                                String loaiTau = (from p in db.DIC_LOAITAU
                                                  where p.LoaiTauID == loaiTauID
                                                  select p.TenLoaiTau).SingleOrDefault();
                                xlSheet.Cells[hang, 2].Value = loaiTau;
                            }

                            xlSheet.Cells[hang, 4].Value = (from p in db.DIC_DEPARTMENT
                                                            where p.DepartmentID == qtct.DepartmentID
                                                            select p.Gross).SingleOrDefault();
                            xlSheet.Cells[hang, 5].Value = (from p in db.DIC_DEPARTMENT
                                                            where p.DepartmentID == qtct.DepartmentID
                                                            select p.Power).SingleOrDefault();
                        }
                        else
                        {
                            xlSheet.Cells[hang, 1].Value = qtct.DepartmentName;
                            if (qtct.Gross != null)
                                xlSheet.Cells[hang, 4].Value = qtct.Gross;

                            if (qtct.Power != null)
                                xlSheet.Cells[hang, 5].Value = qtct.Power;



                            int loaiTauID = Convert.ToInt32(qtct.LoaiTauID);
                            String loaiTau = (from p in db.DIC_LOAITAU
                                              where p.LoaiTauID == loaiTauID
                                              select p.TenLoaiTau).SingleOrDefault();


                            xlSheet.Cells[hang, 2].Value = loaiTau;
                            xlSheet.Cells[hang, 4].Value = qtct.Power;
                            xlSheet.Cells[hang, 5].Value = qtct.Gross; 
                        }

                        xlSheet.Cells[hang, 6].Value = qtct.ChucVu;
                        if (qtct.NgayXuongTau != null)
                        {
                            xlSheet.Cells[hang, 7].Value = qtct.NgayXuongTau.Value.ToString("dd/MM/yyyy");
                        }

                        if (stt == 1)
                        {
                            //hàng đầu tiên                            
                            xlSheet.Cells[28, 2].Value = qtct.NgayXuongTau.Value.ToString("dd/MM/yyyy");
                            tungay = qtct.NgayXuongTau.Value;

                            flag = true;
                            if (qtct.ParentID == 8)
                                flag_tanker = true;
                            if (qtct.LoaiTauID == 1 || qtct.LoaiTauID == 2)
                                flag_tanker = true;

                        }
                        else
                        {                            
                            //hàng cuối
                            if (stt == listQTCT.Count())
                            {
                                denngay = qtct.NgayXuongTau.Value;
                                if (flag == true)
                                {
                                    xlSheet.Cells[hang - 1, 8].Value = qtct.NgayXuongTau.Value.ToString("dd/MM/yyyy");
                                    xlSheet.Cells[hang - 1, 9].Value = from p in db.sp_GetYMD(denngay, tungay) select p;
                                    tongsongay += (denngay - tungay).Days;

                                    if (flag_tanker)
                                        tongsongay_tanker += (denngay - tungay).Days;
                                }

                                xlSheet.Cells[hang, 8].Value = DateTime.Now.Date.ToString("dd/MM/yyyy");
                                if (qtct.NgayXuongTau != null)
                                    tungay = qtct.NgayXuongTau.Value;
                                denngay = DateTime.Now.Date;

                                xlSheet.Cells[hang, 9].Value = from p in db.sp_GetYMD(denngay, tungay) select p;
                                tongsongay += (denngay - tungay).Days;

                                if (qtct.ParentID == 8)
                                    flag_tanker = true;
                                if (qtct.LoaiTauID == 1 || qtct.LoaiTauID == 2)
                                    flag_tanker = true;

                                if (flag_tanker)
                                    tongsongay_tanker += (denngay - tungay).Days;
                            }
                            else
                            {
                                if (flag == true)
                                {
                                    xlSheet.Cells[hang - 1, 8].Value = qtct.NgayXuongTau.Value.ToString("dd/MM/yyyy");
                                    denngay = qtct.NgayXuongTau.Value;
                                    xlSheet.Cells[hang - 1, 9].Value = from p in db.sp_GetYMD(denngay, tungay) select p;
                                    tongsongay += (denngay - tungay).Days;

                                    if (flag_tanker)
                                        tongsongay_tanker += (denngay - tungay).Days;
                                }
                                //else
                                //{
                                //    //xlSheet.Cells[hang - 1, 8].Value = qtct.DecisionDate.Value.ToString("dd/MM/yyyy");
                                //    //denngay = qtct.DecisionDate.Value;
                                //    //xlSheet.Cells[hang - 1, 9].Value = from p in db.sp_GetYMD(denngay, tungay) select p;
                                //}
                            }

                            //flag = true;

                            flag = true;
                            if (qtct.ParentID == 8)
                                flag_tanker = true;
                            if (qtct.LoaiTauID == 1 || qtct.LoaiTauID == 2)
                                flag_tanker = true;

                        }
                        tungay = qtct.NgayXuongTau.Value;
                        hang++;
                    }

                    if (flag && (qtct.ParentID == 17))
                    {
                        xlSheet.Cells[hang - 1, 8].Value = qtct.NgayXuongTau.Value.ToString("dd/MM/yyyy");
                        denngay = qtct.NgayXuongTau.Value;
                        xlSheet.Cells[hang - 1, 9].Value = from p in db.sp_GetYMD(denngay, tungay) select p;
                        tongsongay += (denngay - tungay).Days;
                       
                        if (flag_tanker)                       
                            tongsongay_tanker += (denngay - tungay).Days;

                        flag = false;
                        flag_tanker = false;
                    }
                }

                xlSheet.Cells[28, 8].Value = (tongsongay/30).ToString() + "M" + "\n" + (tongsongay_tanker/30).ToString() + "M Tanker";

                if (hang < 118)
                {
                    for (int i = 118; i > hang; i--)
                    {
                        xlSheet.DeleteRow(hang);
                    }

                }

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

        ////////////////////////////////////////////////////////////////////////////////////////////////
        //2. THỐNG KÊ THUYỀN VIÊN
        ////////////////////////////////////////////////////////////////////////////////////////////////

        public FileResult BaoCaoThongKeThuyenVien()
        {
            DateTime ngaybaocao = Convert.ToDateTime(DateTime.Now.Year.ToString() +"-01-01");
            XuatWordExcelController ctrl = new XuatWordExcelController();
            ctrl.ServerPath = Server.MapPath("~/App_Data");
            ctrl.AppUser = User;
            return ctrl.BaoCaoThongKeThuyenVien(ngaybaocao);
        }

        // GET: HRM_EMPLOYEE/Details/5
        [Authorize(Roles = "Boss, HR, EduCenter, BangCap, DaoTao, Luong")]        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE hRM_EMPLOYEE = db.HRM_EMPLOYEE.Find(id);
            if (hRM_EMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(hRM_EMPLOYEE);
        }
        //Xuat du lieu ra Excel
        public FileResult XuatDanhSachThuyenVien()
        {
            //IQueryable<viewHRM_EMPLOYMENTHISTORY> ds
            //int departmentID = 10;
            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = Server.MapPath("~/App_Data");

            FileInfo templateFile = new FileInfo(server + "//Mau_DSTV.xltx");
            FileInfo newFile = new FileInfo(outputDir.FullName + @"\DSTV.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\DSTV.xlsx");
            }
            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                //for (int j = 1; j <= 12; j++)
                //    worksheet.Column(j).Style.WrapText = false;

                //worksheet.DefaultRowHeight = 45;

                //****************************************
                //1. TIEU DE
                //DIC_DEPARTMENT depart = (from p in db.DIC_DEPARTMENT
                //                         where p.DepartmentID == departmentID
                //                         select p).SingleOrDefault();

                //worksheet.Cells[1, 1].Value = "DANH SÁCH THUYỀN VIÊN TÀU " + depart.DepartmentName.ToUpper();
                worksheet.Cells[2, 12].Value = "Cập nhật ngày " + DateTime.Now.ToString("dd/MM/yyyy");

                //****************************************
                //2. NOI DUNG
                int i = 4;
                int stt = 1;
                int departmentID = 0;

                var listTV = from p in db.sp_LayDSThuyenVien(0)
                             orderby p.DepartmentID, p.PositionID
                             select p;



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
                            worksheet.Row(i).Height = 45;
                        }

                        i++;

                    }

                    worksheet.Cells[i, 1].Value = stt;
                    stt++;

                    worksheet.Cells[i, 2].Value = tv.FirstName + ' ' + tv.LastName;
                    if (tv.QuanHeID != null)
                        worksheet.Cells[i, 3].Value = tv.QuanHeID;
                    if (tv.ChucVu != null)
                        worksheet.Cells[i, 4].Value = tv.ChucVu;
                    if (tv.BirthDay != null)
                        worksheet.Cells[i, 5].Value = tv.BirthDay.Value.ToString("dd/MM/yyyy");
                    //6.GCNKNCM
                    if (tv.Qualification!=null)
                        worksheet.Cells[i, 6].Value = tv.Qualification;
                    //7. TDCM
                    if (tv.EducationName!=null)
                        worksheet.Cells[i, 7].Value = tv.EducationName;
                    //8. NgayXuongTau
                    if (tv.NgayXuongTau.HasValue)
                        worksheet.Cells[i, 8].Value = tv.NgayXuongTau.Value.ToString("dd/MM/yyyy");
                    //9. So ngay tai tau
                    worksheet.Cells[i, 9].Value = tv.TGDT;
                    //10. Tau da cong tac
                    worksheet.Cells[i, 10].Value = tv.QTDT;
                    //11. Ghi chu
                    //worksheet.Cells[i, 11].Value = tv.EducationName;
                    //12. Thoi gian lam viec tai cong ty
                    //worksheet.Cells[i, 12].Value = tv.EducationName;
                    worksheet.Row(i).Height = 45;
                    i++;
                }

                using (ExcelRange r = worksheet.Cells[3, 1, i - 1, 12])
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
            string server = Server.MapPath("~/App_Data"); 

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
                            if (tv.MainAddress != null && (User.IsInRole("HR") || User.IsInRole("Boss")))
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

                        if (item.ContactAddress != null && (User.IsInRole("HR") || User.IsInRole("Boss")))
                            worksheet.Cells[i, 10].Value = item.ContactAddress;

                        if (item.ChucVu != null)
                            worksheet.Cells[i, 13].Value = item.ChucVu;

                        HRM_CONTRACTHISTORY hopDong = db.HRM_CONTRACTHISTORY.Where(x => x.EmployeeID == item.EmployeeID).FirstOrDefault();
                        if (hopDong != null)
                        {
                            worksheet.Cells[i, 14].Value = hopDong.ContractDate.Value.ToString("dd/MM/yyyy");
                        }

                        worksheet.Row(i).Height = 35;
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

        public FileResult InDanhSachRaExcel(string HoTen, string DiaChi, int? PositionID, int? DepartmentID, int? KhuVucID, int? StatusID,
            int? Origin_Tinh, int? Origin_Huyen, string NguyenQuan, int? ContactAddress_Tinh, int? ContactAddress_Huyen)
        {
            var kq = TimKiemNhanVien(HoTen, DiaChi, PositionID, DepartmentID, KhuVucID, StatusID, Origin_Tinh, Origin_Huyen, NguyenQuan, ContactAddress_Tinh, ContactAddress_Huyen);
            kq = kq.OrderBy(x => x.DepartmentID).ThenBy(x => x.PositionID).ThenBy(x => x.LastName).ThenBy(x => x.FirstName);
            XuatWordExcelController ctrl = new XuatWordExcelController();
            ctrl.ServerPath = Server.MapPath("~/App_Data");
            ctrl.AppUser = User;
            return ctrl.XuatDanhSachNhanVien(kq.ToList());                            
        }

        public FileResult InDanhSachTongHopRaExcel(string HoTen, string DiaChi, int? PositionID, int? DepartmentID, int? KhuVucID, int? StatusID,
            int? Origin_Tinh, int? Origin_Huyen, string NguyenQuan, int? ContactAddress_Tinh, int? ContactAddress_Huyen)
        {
            var kq = TimKiemNhanVien(HoTen, DiaChi, PositionID, DepartmentID, KhuVucID, StatusID, Origin_Tinh, Origin_Huyen, NguyenQuan, ContactAddress_Tinh, ContactAddress_Huyen);
            kq = kq.OrderBy(x => x.DepartmentID).ThenBy(x => x.PositionID).ThenBy(x => x.LastName).ThenBy(x => x.FirstName);
            XuatWordExcelController ctrl = new XuatWordExcelController();
            ctrl.ServerPath = Server.MapPath("~/App_Data");
            ctrl.AppUser = User;
            return ctrl.XuatDanhSachNhanVienTongHop(kq.ToList());
        }

        public FileResult XuatExcelDanhSachThuyenVien(int? TauID)
        {
            List<viewNhanVien> hRM_EMPLOYEE = null;
            if (TauID.HasValue && TauID>0)
            {
                hRM_EMPLOYEE = db.viewNhanViens.Where(x => x.DepartmentID == TauID.Value).OrderBy(x => x.PositionID).ThenByDescending(x => x.SoNgay).ToList();
                ViewBag.TauID = TauID.Value;                
            }
            else
            {
                hRM_EMPLOYEE = db.viewNhanViens.Where(x => x.ParentID == 8 || x.ParentID == 17).OrderBy(x => x.ParentID).ThenBy(x => x.DepartmentID).ThenBy(x => x.PositionID).ThenByDescending(x => x.SoNgay).ToList();
                //hRM_EMPLOYEE = db.viewNhanViens.OrderBy(x => x.ParentID).ThenBy(x => x.DepartmentID).ThenBy(x => x.PositionID).ThenBy(x => x.LastName).ThenBy(x => x.LastName).ToList();
                ViewBag.TauID = 0;
            }
            XuatWordExcelController ctrl = new XuatWordExcelController();
            ctrl.ServerPath = Server.MapPath("~/App_Data");
            ctrl.AppUser = User;
            return ctrl.XuatDanhSachThuyenVien(hRM_EMPLOYEE);
        }
        public FileResult XuatExcelCrewMatrixSyQuanTau(int? TauID)
        {
            List<viewNhanVien> hRM_EMPLOYEE = null;
            XuatWordExcelController ctrl = new XuatWordExcelController();
            //if (TauID.HasValue && TauID > 0)
            //{
                
                ctrl.ServerPath = Server.MapPath("~/App_Data");
                ctrl.AppUser = User;
                
            //}
            return ctrl.XuatCrewMatrixSyQuanTau((int)TauID);

        }
        //xuất crew matrix
        public ActionResult CrewMatrix(int EmployeeID, int PositionID)
        {
            return PartialView(db.sp_LayThongTinMatrix(PositionID, EmployeeID).First());
        }
        public ActionResult CrewMatrixTau(int TauID)
        {
            return PartialView(db.sp_T_CrewMatrixTau(TauID,0,0,0,1).First());
        }
        public ActionResult CrewMatrixSyQuanTau(int TauID)
        {
            DIC_DEPARTMENT tau = db.DIC_DEPARTMENT.Find(TauID);
            if (tau != null)
            {
                ViewBag.TenTau = tau.DepartmentName;
            }else
            {
                ViewBag.TenTau = "";
            }
            ViewBag.TauID = TauID;
            return View(db.sp_T_CrewMatrixTau(TauID, 0, 0, 0, 1));
        }
        public ActionResult CrewMatrixSyQuanTau_Full(int TauID)
        {
            DIC_DEPARTMENT tau = db.DIC_DEPARTMENT.Find(TauID);
            if (tau != null)
            {
                ViewBag.TenTau = tau.DepartmentName;
            }
            else
            {
                ViewBag.TenTau = "";
            }
            //lấy ra danh sách các ông có tính crew matrix
            var ds = db.viewHRM_EMPLOYMENTHISTORY.Where(x => x.DepartmentID == TauID && ( (x.PositionID >= 20 && x.PositionID <= 27 ) )).OrderBy(x => x.InternshipPosition).ThenBy(x =>x.PositionID) .ToList();
            return View(ds);
        }
        //TÌM KIẾM
        // GET: HRM_EMPLOYEE/Create
        public ActionResult TimKiem()
        {
            ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName");
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName");
            ViewBag.KhuVucID = new SelectList(db.DIC_KHUVUC, "KhuVucID", "TenKhuVuc");
            ViewBag.StatusID = new SelectList(db.DIC_STATUS, "StatusID", "StatusName");
            ViewBag.Origin_Tinh = new SelectList(db.DIC_TINHTHANHPHO, "TinhThanhPhoID", "TinhThanhPhoName");
            ViewBag.Origin_Huyen = new SelectList(db.DIC_QUANHUYEN.Take(0), "QuanHuyenID", "QuanHuyenName");
            ViewBag.ContactAddress_Tinh = new SelectList(db.DIC_TINHTHANHPHO, "TinhThanhPhoID", "TinhThanhPhoName");
            ViewBag.ContactAddress_Huyen = new SelectList(db.DIC_QUANHUYEN.Take(0), "QuanHuyenID", "QuanHuyenName");
            return View();
        }
        // POST: HRM_EMPLOYEE/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpGet]
        public ActionResult DanhSachTimKiem(string HoTen, string DiaChi, int? PositionID, int? DepartmentID, int? KhuVucID, int? StatusID, 
            int? Origin_Tinh, int? Origin_Huyen, string NguyenQuan, int? ContactAddress_Tinh, int? ContactAddress_Huyen )
        {
            if (ModelState.IsValid)
            {
                var kq = TimKiemNhanVien(HoTen, DiaChi, PositionID, DepartmentID, KhuVucID, StatusID, Origin_Tinh, Origin_Huyen, NguyenQuan, ContactAddress_Tinh, ContactAddress_Huyen);
                kq = kq.OrderBy(x => x.DepartmentID).ThenBy(x => x.PositionID).ThenBy(x => x.LastName).ThenBy(x => x.FirstName);

                //lưu vết lại việc tìm kiếm
                ViewBag.DepartmentID = DepartmentID;
                ViewBag.PositionID = PositionID;
                ViewBag.KhuVucID = KhuVucID;
                ViewBag.StatusID = StatusID;
                ViewBag.HoTen = HoTen;
                ViewBag.DiaChi = DiaChi;
                return View("Index", kq);
            }            
            return RedirectToAction("TimKiem");
        }

        public IEnumerable<WebAuLac.Models.sp_T_LayDanhSachNhanVien_Result> TimKiemNhanVien(string HoTen, string DiaChi, int? PositionID, int? DepartmentID, int? KhuVucID, int? StatusID, int? Origin_Tinh, int? Origin_Huyen, string NguyenQuan, int? ContactAddress_Tinh, int? ContactAddress_Huyen)
        {
            IEnumerable<WebAuLac.Models.sp_T_LayDanhSachNhanVien_Result> kq = db.sp_T_LayDanhSachNhanVien();
            //bat dau tim kiem
            if (HoTen!=null && HoTen != "")
            {
                kq = kq.Where(x => x.HoTen.ToLower().Contains(HoTen.ToLower()));
            }
            if (DiaChi!=null && DiaChi != "")
            {
                kq = kq.Where(x => !String.IsNullOrEmpty(x.ContactAddress) &&
                x.ContactAddress.ToLower().Contains(DiaChi.ToLower()));
            }
            if (ContactAddress_Tinh.HasValue && ContactAddress_Tinh > 0)
            {
                kq = kq.Where(x => x.eContacAddress_Tinh.HasValue && x.eContacAddress_Tinh == ContactAddress_Tinh.Value);
            }
            if (ContactAddress_Huyen.HasValue && ContactAddress_Huyen > 0)
            {
                kq = kq.Where(x => x.eContactAddress_Huyen.HasValue && x.eContactAddress_Huyen == ContactAddress_Tinh.Value);
            }
            if (NguyenQuan != null && NguyenQuan != "")
            {
                kq = kq.Where(x => !String.IsNullOrEmpty(x.eOrigin) &&
                x.eOrigin.ToLower().Contains(NguyenQuan.ToLower()));
            }
            if (Origin_Tinh.HasValue && Origin_Tinh > 0)
            {
                kq = kq.Where(x => x.eOrigin_Tinh.HasValue && x.eOrigin_Tinh == Origin_Tinh.Value);
            }
            if (Origin_Huyen.HasValue && Origin_Huyen > 0)
            {
                kq = kq.Where(x => x.eOrigin_Huyen.HasValue && x.eOrigin_Huyen == Origin_Tinh.Value);
            }
            if (PositionID.HasValue && PositionID>0)
            {
                kq = kq.Where(x => x.PositionID.HasValue && x.PositionID == PositionID.Value);
            }
            if (DepartmentID.HasValue && DepartmentID>0)
            {
                //lấy ra tất cả các phòng ban con nữa
                var dp = db.DIC_DEPARTMENT.Where(x => x.ParentID == DepartmentID.Value).Select(x => x.DepartmentID).ToList();
                dp.Add(DepartmentID.Value);             
                kq = kq.Where(x => x.DepartmentID.HasValue && dp.Contains(x.DepartmentID.Value));
            }
            if (KhuVucID.HasValue && KhuVucID>0)
            {                
                kq = kq.Where(x => x.KhuVucID.HasValue && x.KhuVucID == KhuVucID.Value);
            }
            if (StatusID.HasValue && StatusID>0)
            {
                kq = kq.Where(x => x.StatusID.HasValue && x.StatusID == StatusID.Value);
            }
            return kq;
        }
        
               
        // GET: HRM_EMPLOYEE/Create
        [Authorize(Roles ="HR, EduCenter")]
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            //lấy ra mã code nhân viên mới nhất
            HRM_EMPLOYEE newEmployee = new HRM_EMPLOYEE();
            List<string> maMois = db.sp_T_TaoMaNhanVien().ToList();
            newEmployee.EmployeeCode = maMois[0];
            ViewBag.Department_PositionID = new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Decription");
            ViewBag.EducationID = new SelectList(db.DIC_EDUCATION, "EducationID", "EducationName");
            ViewBag.EthnicID = new SelectList(db.DIC_ETHNIC, "EthnicID", "EthnicName");
            ViewBag.NationalityID = new SelectList(db.DIC_NATIONALITY, "NationalityID", "NationalityName");
            ViewBag.ReligionID = new SelectList(db.DIC_RELIGION, "ReligionID", "ReligionName");
            ViewBag.StatusID = new SelectList(db.DIC_STATUS, "StatusID", "StatusName");
            ViewBag.SchoolID = new SelectList(db.DIC_SCHOOL, "SchoolID", "SchoolName");
            ViewBag.KhuVucID = new SelectList(db.DIC_KHUVUC, "KhuVucID", "TenKhuVuc");
            ViewBag.TrinhDoAnhVanID = new SelectList(db.TrinhDoAnhVans, "TrinhDoAnhVanID", "TenTrinhDoAnhVan");
            ViewBag.TrinhDoViTinhID = new SelectList(db.TrinhDoViTinhs, "TrinhDoViTinhID", "TenTrinhDoViTinh");
            ViewBag.HeDaoTaoID = new SelectList(db.HeDaoTaos, "HeDaoTaoID", "TenHeDaoTao");
            //lấy ra quận huyện xã                                
            ViewBag.Origin_Tinh = new SelectList(db.DIC_TINHTHANHPHO, "TinhThanhPhoID", "TinhThanhPhoName");
            ViewBag.Origin_Huyen = new SelectList(db.DIC_QUANHUYEN.Take(0), "QuanHuyenID", "QuanHuyenName");
            ViewBag.Origin_Xa = new SelectList(db.DIC_PHUONGXA.Take(0), "PhuongXaID", "PhuongXaName");
            ViewBag.MainAddress_Tinh = new SelectList(db.DIC_TINHTHANHPHO, "TinhThanhPhoID", "TinhThanhPhoName");
            ViewBag.MainAddress_Huyen = new SelectList(db.DIC_QUANHUYEN.Take(0), "QuanHuyenID", "QuanHuyenName");
            ViewBag.MainAddress_Xa = new SelectList(db.DIC_PHUONGXA.Take(0), "PhuongXaID", "PhuongXaName");
            ViewBag.ContactAddress_Tinh = new SelectList(db.DIC_TINHTHANHPHO, "TinhThanhPhoID", "TinhThanhPhoName");
            ViewBag.ContactAddress_Huyen = new SelectList(db.DIC_QUANHUYEN.Take(0), "QuanHuyenID", "QuanHuyenName");
            ViewBag.ContactAddress_Xa = new SelectList(db.DIC_PHUONGXA.Take(0), "PhuongXaID", "PhuongXaName");
            return View(newEmployee);
        }

        // POST: HRM_EMPLOYEE/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "HR, EduCenter")]
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,EmployeeCode,CardNo,FirstName,LastName,Alias,Sex,MarriageStatus,BirthDay,BirthPlace,MainAddress,ContactAddress,CellPhone,HomePhone,Email,Skype,Yahoo,Facebook,IDCard,IDCardDate,IDCardPlace,TaxNo,BankCode,BankName,InsuranceCode,InsuranceDate,Photo,EducationID,DegreeID,EthnicID,ReligionID,NationalityID,StatusID, Origin, SchoolID,Qualification, Height, Weight, BloodType, KhuVucID,DiaChiTiengAnh, TrinhDoAnhVanID, TrinhDoViTinhID, HeDaoTaoID, ThoiGianTotNghiep, SDTNguoiThan, MainAddress_Xa, MainAddress_Huyen, MainAddress_Tinh, ContactAddress_Xa, ContactAddress_Huyen, ContactAddress_Tinh, Origin_Xa, Origin_Huyen, Origin_Tinh")] HRM_EMPLOYEE hRM_EMPLOYEE, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    byte[] imageBytes = null;
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        imageBytes = reader.ReadBytes(upload.ContentLength);
                    }
                    hRM_EMPLOYEE.Photo = imageBytes;
                }
                db.HRM_EMPLOYEE.Add(hRM_EMPLOYEE);
                db.SaveChanges();
                return RedirectToAction("Details", new { id=hRM_EMPLOYEE.EmployeeID });
            }
            ViewBag.Department_PositionID = new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Decription");
            ViewBag.EducationID = new SelectList(db.DIC_EDUCATION, "EducationID", "EducationName");
            ViewBag.EthnicID = new SelectList(db.DIC_ETHNIC, "EthnicID", "EthnicName");
            ViewBag.NationalityID = new SelectList(db.DIC_NATIONALITY, "NationalityID", "NationalityName");
            ViewBag.ReligionID = new SelectList(db.DIC_RELIGION, "ReligionID", "ReligionName");
            ViewBag.StatusID = new SelectList(db.DIC_STATUS, "StatusID", "StatusName");
            ViewBag.SchoolID = new SelectList(db.DIC_SCHOOL, "SchoolID", "SchoolName");
            ViewBag.KhuVucID = new SelectList(db.DIC_KHUVUC, "KhuVucID", "TenKhuVuc");
            ViewBag.TrinhDoAnhVanID = new SelectList(db.TrinhDoAnhVans, "TrinhDoAnhVanID", "TenTrinhDoAnhVan");
            ViewBag.TrinhDoViTinhID = new SelectList(db.TrinhDoViTinhs, "TrinhDoViTinhID", "TenTrinhDoViTinh");
            ViewBag.HeDaoTaoID = new SelectList(db.HeDaoTaos, "HeDaoTaoID", "TenHeDaoTao");
            //lấy ra quận huyện xã                                
            ViewBag.Origin_Tinh = new SelectList(db.DIC_TINHTHANHPHO, "TinhThanhPhoID", "TinhThanhPhoName");
            ViewBag.Origin_Huyen = new SelectList(db.DIC_QUANHUYEN.Take(0), "QuanHuyenID", "QuanHuyenName");
            ViewBag.Origin_Xa = new SelectList(db.DIC_PHUONGXA.Take(0), "PhuongXaID", "PhuongXaName");
            ViewBag.MainAddress_Tinh = new SelectList(db.DIC_TINHTHANHPHO, "TinhThanhPhoID", "TinhThanhPhoName");
            ViewBag.MainAddress_Huyen = new SelectList(db.DIC_QUANHUYEN.Take(0), "QuanHuyenID", "QuanHuyenName");
            ViewBag.MainAddress_Xa = new SelectList(db.DIC_PHUONGXA.Take(0), "PhuongXaID", "PhuongXaName");
            ViewBag.ContactAddress_Tinh = new SelectList(db.DIC_TINHTHANHPHO, "TinhThanhPhoID", "TinhThanhPhoName");
            ViewBag.ContactAddress_Huyen = new SelectList(db.DIC_QUANHUYEN.Take(0), "QuanHuyenID", "QuanHuyenName");
            ViewBag.ContactAddress_Xa = new SelectList(db.DIC_PHUONGXA.Take(0), "PhuongXaID", "PhuongXaName");
            return View(hRM_EMPLOYEE);
        }
        // GET: HRM_EMPLOYEE/Create
        [Authorize(Roles = "HR, EduCenter")]
        [Authorize(Roles = "Create")]
        public ActionResult CreateShort()
        {
            //lấy ra mã code nhân viên mới nhất
            HRM_EMPLOYEE newEmployee = new HRM_EMPLOYEE();
            //List<string> maMois = db.sp_T_TaoMaNhanVien().ToList();
            //newEmployee.EmployeeCode = maMois[0];
            ViewBag.EducationID = new SelectList(db.DIC_EDUCATION, "EducationID", "EducationName");
            ViewBag.QuanHeID = new SelectList(db.tbl_QuanHe, "QuanHeID", "HoTen");
            return PartialView(newEmployee);
        }

        // POST: HRM_EMPLOYEE/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "HR, EduCenter")]
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateShort([Bind(Include = "FirstName,LastName,Sex,BirthDay,BirthPlace,ContactAddress,CellPhone,Email,EducationID,Qualification, QuanHeID")] HRM_EMPLOYEE hRM_EMPLOYEE)
        {
            if (ModelState.IsValid)
            {
                hRM_EMPLOYEE.StatusID = 6; //ứng viên
                db.HRM_EMPLOYEE.Add(hRM_EMPLOYEE);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.EducationID = new SelectList(db.DIC_EDUCATION, "EducationID", "EducationName");
            ViewBag.QuanHeID = new SelectList(db.tbl_QuanHe, "QuanHeID", "HoTen", hRM_EMPLOYEE.QuanHeID);
            return PartialView(hRM_EMPLOYEE);
        }

        [Authorize(Roles = "HR, EduCenter")]
        [Authorize(Roles = "Create")]
        public ActionResult EditShort(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE hRM_EMPLOYEE = db.HRM_EMPLOYEE.Find(id);
            if (hRM_EMPLOYEE == null)
            {
                return HttpNotFound();
            }
            ViewBag.EducationID = new SelectList(db.DIC_EDUCATION, "EducationID", "EducationName", hRM_EMPLOYEE.EducationID);
            ViewBag.QuanHeID = new SelectList(db.tbl_QuanHe, "QuanHeID", "HoTen", hRM_EMPLOYEE.QuanHeID);
            return PartialView(hRM_EMPLOYEE);
        }

        // POST: HRM_EMPLOYEE/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "HR, EduCenter")]
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditShort([Bind(Include = "EmployeeID, FirstName,LastName,Sex,BirthDay,BirthPlace,ContactAddress,CellPhone,Email,EducationID,Qualification, QuanHeID")] HRM_EMPLOYEE hRM_EMPLOYEE)
        {
            if (ModelState.IsValid)
            {
                hRM_EMPLOYEE.StatusID = 6;
                db.Entry(hRM_EMPLOYEE).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.EducationID = new SelectList(db.DIC_EDUCATION, "EducationID", "EducationName");
            ViewBag.QuanHeID = new SelectList(db.tbl_QuanHe, "QuanHeID", "HoTen", hRM_EMPLOYEE.QuanHeID);
            return PartialView(hRM_EMPLOYEE);
        }
        // GET: HRM_EMPLOYEE/Edit/5
        [Authorize(Roles = "HR, EduCenter")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE hRM_EMPLOYEE = db.HRM_EMPLOYEE.Find(id);
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
            ViewBag.SchoolID = new SelectList(db.DIC_SCHOOL, "SchoolID", "SchoolName", hRM_EMPLOYEE.SchoolID);
            ViewBag.KhuVucID = new SelectList(db.DIC_KHUVUC, "KhuVucID", "TenKhuVuc", hRM_EMPLOYEE.KhuVucID);
            ViewBag.TrinhDoAnhVanID = new SelectList(db.TrinhDoAnhVans, "TrinhDoAnhVanID", "TenTrinhDoAnhVan", hRM_EMPLOYEE.TrinhDoAnhVanID);
            ViewBag.TrinhDoViTinhID = new SelectList(db.TrinhDoViTinhs, "TrinhDoViTinhID", "TenTrinhDoViTinh", hRM_EMPLOYEE.TrinhDoViTinhID);
            ViewBag.HeDaoTaoID = new SelectList(db.HeDaoTaos, "HeDaoTaoID", "TenHeDaoTao", hRM_EMPLOYEE.HeDaoTaoID);
            //lấy ra quận huyện xã                                
            ViewBag.Origin_Tinh = new SelectList(db.DIC_TINHTHANHPHO, "TinhThanhPhoID", "TinhThanhPhoName", hRM_EMPLOYEE.Origin_Tinh);
            ViewBag.Origin_Huyen = new SelectList(db.DIC_QUANHUYEN.Where(x => x.TinhThanhPhoID ==hRM_EMPLOYEE.Origin_Tinh), "QuanHuyenID", "QuanHuyenName", hRM_EMPLOYEE.Origin_Huyen);
            ViewBag.Origin_Xa = new SelectList(db.DIC_PHUONGXA.Where(x => x.QuanHuyenID == hRM_EMPLOYEE.Origin_Huyen), "PhuongXaID", "PhuongXaName", hRM_EMPLOYEE.Origin_Xa);
            ViewBag.MainAddress_Tinh = new SelectList(db.DIC_TINHTHANHPHO, "TinhThanhPhoID", "TinhThanhPhoName", hRM_EMPLOYEE.MainAddress_Tinh);
            ViewBag.MainAddress_Huyen = new SelectList(db.DIC_QUANHUYEN.Where(x => x.TinhThanhPhoID == hRM_EMPLOYEE.MainAddress_Tinh), "QuanHuyenID", "QuanHuyenName", hRM_EMPLOYEE.MainAddress_Huyen);
            ViewBag.MainAddress_Xa = new SelectList(db.DIC_PHUONGXA.Where(x => x.QuanHuyenID == hRM_EMPLOYEE.MainAddress_Huyen), "PhuongXaID", "PhuongXaName", hRM_EMPLOYEE.MainAddress_Xa);
            ViewBag.ContactAddress_Tinh = new SelectList(db.DIC_TINHTHANHPHO, "TinhThanhPhoID", "TinhThanhPhoName", hRM_EMPLOYEE.ContactAddress_Tinh);
            ViewBag.ContactAddress_Huyen = new SelectList(db.DIC_QUANHUYEN.Where(x => x.TinhThanhPhoID == hRM_EMPLOYEE.ContactAddress_Tinh), "QuanHuyenID", "QuanHuyenName", hRM_EMPLOYEE.ContactAddress_Huyen);
            ViewBag.ContactAddress_Xa = new SelectList(db.DIC_PHUONGXA.Where(x => x.QuanHuyenID == hRM_EMPLOYEE.ContactAddress_Huyen), "PhuongXaID", "PhuongXaName", hRM_EMPLOYEE.ContactAddress_Xa);
            return View(hRM_EMPLOYEE);
        }

        // POST: HRM_EMPLOYEE/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "HR, EduCenter")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,EmployeeCode,CardNo,FirstName,LastName,Alias,Sex,MarriageStatus,BirthDay,BirthPlace,MainAddress,ContactAddress,CellPhone,HomePhone,Email,Skype,Yahoo,Facebook,IDCard,IDCardDate,IDCardPlace,TaxNo,BankCode,BankName,InsuranceCode,InsuranceDate,EducationID,DegreeID,EthnicID,ReligionID,NationalityID,StatusID, Origin, SchoolID,Qualification, Height, Weight, BloodType, KhuVucID, DiaChiTiengAnh, TrinhDoAnhVanID, TrinhDoViTinhID, HeDaoTaoID, ThoiGianTotNghiep, SDTNguoiThan, MainAddress_Xa, MainAddress_Huyen, MainAddress_Tinh, ContactAddress_Xa, ContactAddress_Huyen, ContactAddress_Tinh, Origin_Xa, Origin_Huyen, Origin_Tinh")] HRM_EMPLOYEE hRM_EMPLOYEE, Boolean IsChangePhoto, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    byte[] imageBytes = null;
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        imageBytes = reader.ReadBytes(upload.ContentLength);
                    }
                    hRM_EMPLOYEE.Photo = imageBytes;
                }else
                {
                    if (!IsChangePhoto)
                    {
                        //tìm lại photo cũ
                        var q = from temp in db.HRM_EMPLOYEE where temp.EmployeeID == hRM_EMPLOYEE.EmployeeID select temp.Photo;
                        byte[] cover = q.First();
                        hRM_EMPLOYEE.Photo = cover;
                    }                    
                }
                db.Entry(hRM_EMPLOYEE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = hRM_EMPLOYEE.EmployeeID});
            }
            ViewBag.Department_PositionID = new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Decription",hRM_EMPLOYEE.Department_PositionID);
            ViewBag.EducationID = new SelectList(db.DIC_EDUCATION, "EducationID", "EducationName", hRM_EMPLOYEE.EducationID);
            ViewBag.EthnicID = new SelectList(db.DIC_ETHNIC, "EthnicID", "EthnicName", hRM_EMPLOYEE.EthnicID);
            ViewBag.NationalityID = new SelectList(db.DIC_NATIONALITY, "NationalityID", "NationalityName", hRM_EMPLOYEE.NationalityID);
            ViewBag.ReligionID = new SelectList(db.DIC_RELIGION, "ReligionID", "ReligionName", hRM_EMPLOYEE.ReligionID);
            ViewBag.StatusID = new SelectList(db.DIC_STATUS, "StatusID", "StatusName", hRM_EMPLOYEE.StatusID);
            ViewBag.KhuVucID = new SelectList(db.DIC_KHUVUC, "KhuVucID", "TenKhuVuc", hRM_EMPLOYEE.KhuVucID);
            ViewBag.TrinhDoAnhVanID = new SelectList(db.TrinhDoAnhVans, "TrinhDoAnhVanID", "TenTrinhDoAnhVan", hRM_EMPLOYEE.TrinhDoAnhVanID);
            ViewBag.TrinhDoViTinhID = new SelectList(db.TrinhDoViTinhs, "TrinhDoViTinhID", "TenTrinhDoViTinh", hRM_EMPLOYEE.TrinhDoViTinhID);
            ViewBag.HeDaoTaoID = new SelectList(db.HeDaoTaos, "HeDaoTaoID", "TenHeDaoTao", hRM_EMPLOYEE.HeDaoTaoID);
            //xã huyện tỉnh
            ViewBag.Origin_Tinh = new SelectList(db.DIC_TINHTHANHPHO, "TinhThanhPhoID", "TinhThanhPhoName", hRM_EMPLOYEE.Origin_Tinh);
            ViewBag.Origin_Huyen = new SelectList(db.DIC_QUANHUYEN.Where(x => x.QuanHuyenID == hRM_EMPLOYEE.Origin_Tinh), "QuanHuyenID", "QuanHuyenName", hRM_EMPLOYEE.Origin_Huyen);
            ViewBag.Origin_Xa = new SelectList(db.DIC_PHUONGXA.Where(x => x.PhuongXaID == hRM_EMPLOYEE.Origin_Huyen), "PhuongXaID", "PhuongXaName", hRM_EMPLOYEE.Origin_Xa);
            ViewBag.MainAddress_Tinh = new SelectList(db.DIC_TINHTHANHPHO, "TinhThanhPhoID", "TinhThanhPhoName", hRM_EMPLOYEE.MainAddress_Tinh);
            ViewBag.MainAddress_Huyen = new SelectList(db.DIC_QUANHUYEN.Where(x => x.QuanHuyenID == hRM_EMPLOYEE.MainAddress_Tinh), "QuanHuyenID", "QuanHuyenName", hRM_EMPLOYEE.MainAddress_Huyen);
            ViewBag.MainAddress_Xa = new SelectList(db.DIC_PHUONGXA.Where(x => x.QuanHuyenID == hRM_EMPLOYEE.MainAddress_Huyen), "PhuongXaID", "PhuongXaName", hRM_EMPLOYEE.MainAddress_Xa);
            ViewBag.ContactAddress_Tinh = new SelectList(db.DIC_TINHTHANHPHO, "TinhThanhPhoID", "TinhThanhPhoName", hRM_EMPLOYEE.ContactAddress_Tinh);
            ViewBag.ContactAddress_Huyen = new SelectList(db.DIC_QUANHUYEN.Where(x => x.QuanHuyenID == hRM_EMPLOYEE.ContactAddress_Tinh), "QuanHuyenID", "QuanHuyenName", hRM_EMPLOYEE.ContactAddress_Huyen);
            ViewBag.ContactAddress_Xa = new SelectList(db.DIC_PHUONGXA.Where(x => x.QuanHuyenID == hRM_EMPLOYEE.ContactAddress_Huyen), "PhuongXaID", "PhuongXaName", hRM_EMPLOYEE.ContactAddress_Xa);
            return View(hRM_EMPLOYEE);
        }

        [Authorize(Roles = "EduCenter")]
        [Authorize(Roles = "CreateTTTV")]
        public ActionResult EditNote(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE hRM_EMPLOYEE = db.HRM_EMPLOYEE.Find(id);
            if (hRM_EMPLOYEE == null)
            {
                return HttpNotFound();
            }
            ViewBag.QuanHeID = new SelectList(db.tbl_QuanHe, "QuanHeID", "HoTen", hRM_EMPLOYEE.QuanHeID);
            ViewBag.HoTen = hRM_EMPLOYEE.FirstName + " " + hRM_EMPLOYEE.LastName;
            return PartialView(hRM_EMPLOYEE);
        }

        // POST: HRM_EMPLOYEE/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "EduCenter")]
        [Authorize(Roles = "CreateTTTV")]
        [HttpPost, ActionName("EditNote")]
        [ValidateAntiForgeryToken]
        public ActionResult EditNote(int EmployeeID, string Note, int? QuanHeID, string LoaiQuanHe, Boolean? SSDD, string GhiChuSSDD, DateTime? NgaySSDD)
        {
            HRM_EMPLOYEE obj = db.HRM_EMPLOYEE.Find(EmployeeID);
            if (ModelState.IsValid)
            {
                obj.Note = Note;
                obj.QuanHeID = QuanHeID;
                obj.SSDD = SSDD;
                obj.LoaiQuanHe = LoaiQuanHe;
                obj.GhiChuSSDD = GhiChuSSDD;
                obj.NgaySSDD = NgaySSDD;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(obj);
        }
        // GET: HRM_EMPLOYEE/Delete/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE hRM_EMPLOYEE = db.HRM_EMPLOYEE.Find(id);
            if (hRM_EMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(hRM_EMPLOYEE);
        }

        // POST: HRM_EMPLOYEE/Delete/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HRM_EMPLOYEE hRM_EMPLOYEE = db.HRM_EMPLOYEE.Find(id);
            db.HRM_EMPLOYEE.Remove(hRM_EMPLOYEE);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult DeleteShort(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE hRM_EMPLOYEE = db.HRM_EMPLOYEE.Find(id);
            if (hRM_EMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return PartialView(hRM_EMPLOYEE);
        }

        // POST: HRM_EMPLOYEE/Delete/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        [HttpPost, ActionName("DeleteShort")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteShortConfirmed(int id)
        {
            HRM_EMPLOYEE hRM_EMPLOYEE = db.HRM_EMPLOYEE.Find(id);
            db.HRM_EMPLOYEE.Remove(hRM_EMPLOYEE);
            db.SaveChanges();
            return Json (new { success =true}, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPost]
        public JsonResult XuatBaoCaoTest(int[] function_param)
        {
            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = Server.MapPath("~/App_Data");

            var fileName = "Excel_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";

            //save the file to server temp folder
            string fullPath = Path.Combine(server, fileName);

            FileInfo templateFile = new FileInfo(server + "//Mau_DSTV_Ngang.xltx");
            //FileInfo newFile = new FileInfo(outputDir.FullName + @"\LLTN.xlsx");
            FileInfo newFile = new FileInfo(fullPath);//Tạo file tạm lưu ra
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\Mau_DSTV_Ngang.xlsx");
            }
            //IQueryable<viewHRM_EMPLOYMENTHISTORY> ds
            //int departmentID = 10;
            List<viewNhanVien> dsThuyenVien = (from p in db.viewNhanViens
                                               where function_param.Contains(p.EmployeeID)
                                               select p).ToList();
            ExcelPackage package;
            using (package = new ExcelPackage(newFile, templateFile))
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
                    //if (departmentID != tv.DepartmentID || departmentID == 0)
                    //{
                    //    stt = 1;
                    //    worksheet.Cells[i, 1].Value = tv.DepartmentName.ToUpper(); //"DANH SÁCH THUYỀN VIÊN "
                    //    departmentID = Convert.ToInt32(tv.DepartmentID);
                    //    using (var range = worksheet.Cells[i, 1, i, 12])
                    //    {
                    //        // Format text đỏ và đậm
                    //        range.Style.Font.Color.SetColor(System.Drawing.Color.Red);
                    //        range.Style.Font.Size = 18;
                    //        range.Style.Font.Bold = true;
                    //        range.Merge = true;
                    //        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //    }

                    //    i++;
                    //}

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
                    if (tv.Note != null)
                        worksheet.Cells[i, 13].Value = tv.Note;

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
