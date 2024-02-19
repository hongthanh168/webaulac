using HRM.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAuLac.Models;


namespace WebAuLac.Controllers
{
    [Authorize(Roles = "Boss, HR, EduCenter, DaoTao")]
    public class ThongKeDaoTaoController : Controller
    {
        private AuLacEntities db = new AuLacEntities();
        //[Authorize(Roles = "Quan tri")]
        public ActionResult ThongKeTuNgayDenNgay()
        {
            return View();
        }
        //[Authorize(Roles = "Quan tri")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongKeTuNgayDenNgay([Bind(Include = "TuNgay,DenNgay")] TuNgayDenNgay obj)
        {
            if (ModelState.IsValid)
            {
                var result = db.sp_T_LayDanhSachHocTheoThoiGian(obj.TuNgay, obj.DenNgay).ToList();
                //lấy ra tổng chi phí và tổng người tham gia
                decimal tongChiPhi = result.Sum(x => x.hocphi.Value);
                int soNguoi = result.Select(x => x.EmployeeID).Distinct().Count();
                ViewBag.chiPhi = tongChiPhi;
                ViewBag.soNhanVien = soNguoi;
                ViewBag.BangKe = result;
                return View(obj);
            }
            return View();
        }
        //[Authorize(Roles = "Quan tri")]
        public ActionResult ThongKeTuNgayDenNgayNhanVien()
        {
            return View();
        }
        //[Authorize(Roles = "Quan tri")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongKeTuNgayDenNgayNhanVien([Bind(Include = "TuNgay,DenNgay")] TuNgayDenNgay obj, string answer)
        {
            if (ModelState.IsValid)
            {
                switch (answer)
                {
                    case "xemthongtin":
                        var result = db.sp_T_LayDanhSachHocTheoThoiGian(obj.TuNgay, obj.DenNgay).Where(x => x.ParentID != 8 && x.ParentID != 17).ToList();
                        //lấy ra tổng chi phí và tổng người tham gia
                        decimal tongChiPhi = result.Sum(x => x.hocphi.Value);
                        int soNguoi = result.Select(x => x.EmployeeID).Distinct().Count();
                        ViewBag.chiPhi = tongChiPhi;
                        ViewBag.soNhanVien = soNguoi;
                        ViewBag.BangKe = result;
                        return View(obj);
                    case "xuatexcel":
                        XuatWordExcelController ctrl = new XuatWordExcelController();
                        ctrl.ServerPath = Server.MapPath("~/App_Data");
                        ctrl.AppUser = User;
                        return ctrl.ThongKeQuaTrinhDaoTao(Convert.ToDateTime(obj.TuNgay), Convert.ToDateTime(obj.DenNgay), 2);



                    default:

                        break;
                }


            }

            return View();
        }
        //[Authorize(Roles = "Quan tri")]
        public ActionResult ThongKeTuNgayDenNgayThuyenVien()
        {
            return View();
        }
        //[Authorize(Roles = "Quan tri")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongKeTuNgayDenNgayThuyenVien([Bind(Include = "TuNgay,DenNgay")] TuNgayDenNgay obj, string answer)
        {
            if (ModelState.IsValid)
            {
                switch (answer)
                {
                    case "xemthongtin":
                        var result = db.sp_T_LayDanhSachHocTheoThoiGian(obj.TuNgay, obj.DenNgay).Where(x => x.ParentID == 8 || x.ParentID == 17).ToList();
                        //lấy ra tổng chi phí và tổng người tham gia
                        decimal tongChiPhi = result.Sum(x => x.hocphi.Value);
                        int soNguoi = result.Select(x => x.EmployeeID).Distinct().Count();
                        ViewBag.chiPhi = tongChiPhi;
                        ViewBag.soNhanVien = soNguoi;
                        ViewBag.BangKe = result;
                        return View(obj);
                        
                    case "xuatexcel":
                        XuatWordExcelController ctrl = new XuatWordExcelController();
                        ctrl.ServerPath = Server.MapPath("~/App_Data");
                        ctrl.AppUser = User;
                        return ctrl.ThongKeQuaTrinhDaoTao(Convert.ToDateTime(obj.TuNgay), Convert.ToDateTime(obj.DenNgay), 1);

                         
                    
                    default:
                        
                        break;
                }
               
               
            }
            return View();
        }
        //[Authorize(Roles = "Quan tri")]
       
        public FileResult InDanhSachRaExcel(int loaibaocao)
        {
            string tungay = String.Format("{0}", Request.Form["TuNgay"]);
            string denngay = String.Format("{0}", Request.Form["DenNgay"]);
            XuatWordExcelController ctrl = new XuatWordExcelController();
            ctrl.ServerPath = Server.MapPath("~/App_Data");
            ctrl.AppUser = User;
            return ctrl.ThongKeQuaTrinhDaoTao(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay),loaibaocao);
        }
        public JsonResult LocDoiTuong(int loaiDoiTuong)
        {
            List<SelectListItem> doiTuongs = new List<SelectListItem>();
            IQueryable<viewHRM_EMPLOYMENTHISTORY> dsDoiTuong = db.viewHRM_EMPLOYMENTHISTORY;
            if (loaiDoiTuong == 1)// thuyen vien
            {
                dsDoiTuong = dsDoiTuong.Where(x => x.ParentID == 8 || x.ParentID == 17);
            }
            else if (loaiDoiTuong == 2)
            {
                dsDoiTuong = dsDoiTuong.Where(x => x.ParentID.Value != 8 && x.ParentID.Value != 17);
            }
            foreach (var item in dsDoiTuong)
            {
                SelectListItem objSel = new SelectListItem() { Value = item.EmployeeID.ToString(), Text = item.HoTen };
                doiTuongs.Add(objSel);
            }
            return Json(doiTuongs, JsonRequestBehavior.AllowGet);
        }       
       
        public ActionResult ThongKeCaNhan()
        {
            //mặc định là lấy ra danh sách thuyền viên
            IQueryable<viewHRM_EMPLOYMENTHISTORY> danhSach = db.viewHRM_EMPLOYMENTHISTORY;
            ViewBag.EmployeeID = new SelectList(danhSach, "EmployeeID", "HoTen");
            return View();
        }
        //[Authorize(Roles = "Quan tri")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongKeCaNhan(int EmployeeID, string answer)
        {
            if (ModelState.IsValid)
            {
                switch (answer)
                {
                    case "xemthongtin":
                        var result = db.sp_T_ThongKeDaoTaoCaNhan(EmployeeID).ToList();
                ViewBag.BangKe = result;
                //lấy ra tổng chi phí và chức danh
                decimal tongChiPhi = result.Sum(x => x.hocphi.Value);
                string chucDanh = "";
                if (result.Count > 0)
                {
                    chucDanh = result[0].ChucDanh;
                }
                ViewBag.tongChiPhi = tongChiPhi;
                ViewBag.chucDanh = chucDanh;
                IQueryable<viewHRM_EMPLOYMENTHISTORY> danhSach = db.viewHRM_EMPLOYMENTHISTORY;
                //if (loaiDoiTuong==1)// thuyen vien
                //{
                //    doiTuong = doiTuong.Where(x => x.ParentID == 8 || x.ParentID == 17);
                //}else if(loaiDoiTuong==2)
                //{
                //    doiTuong = doiTuong.Where(x => x.ParentID.Value != 8 && x.ParentID.Value != 17);
                //}
                ViewBag.EmployeeID = new SelectList(danhSach, "EmployeeID", "HoTen", EmployeeID);
                return View();
                    case "xuatexcel":
                        XuatWordExcelController ctrl = new XuatWordExcelController();
                        ctrl.ServerPath = Server.MapPath("~/App_Data");
                        ctrl.AppUser = User;
                        return ctrl.Mau_BaoCaoChiPhiDaoTao(EmployeeID);



                    default:

                        break;
                }


            }
            return View();
        }


        public JsonResult LocDoiTuong1(int loaiDoiTuong)
        {
            List<SelectListItem> doiTuongs = new List<SelectListItem>();
            IQueryable<viewHRM_EMPLOYMENTHISTORY> dsDoiTuong = db.viewHRM_EMPLOYMENTHISTORY;
            if (loaiDoiTuong == 1)// thuyen vien
            {
                dsDoiTuong = dsDoiTuong.Where(x => x.ParentID == 8 || x.ParentID == 17);
            }
            else if (loaiDoiTuong == 2)
            {
                dsDoiTuong = dsDoiTuong.Where(x => x.ParentID.Value != 8 && x.ParentID.Value != 17);
            }
            foreach (var item in dsDoiTuong)
            {
                SelectListItem objSel = new SelectListItem() { Value = item.EmployeeID.ToString(), Text = item.HoTen };
                doiTuongs.Add(objSel);
            }
            return Json(doiTuongs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ThongKeBangCap()
        {
            //mặc định là lấy ra danh sách bằng cấp
            IQueryable<DIC_DEGREE> danhSach = db.DIC_DEGREE;
            ViewBag.DegreeID = new SelectList(danhSach, "DegreeID", "DegreeName");
            return View();
        }
        //[Authorize(Roles = "Quan tri")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongKeBangCap(int DegreeID, string answer)
        {
            if (ModelState.IsValid)
            {
                XuatWordExcelController ctrl = new XuatWordExcelController();
                switch (answer)
                {
                   
                    case "xemthongtin":
                        var result = db.sp_LayDSBangCapTheoID(DegreeID).ToList();
                        ViewBag.BangKe = result;                       
                        IQueryable<DIC_DEGREE> danhSach = db.DIC_DEGREE;
                        ViewBag.DegreeID = new SelectList(danhSach, "DegreeID", "DegreeName", DegreeID);
                        return View();
                    case "xuatexcel":                        
                        ctrl.ServerPath = Server.MapPath("~/App_Data");
                        ctrl.AppUser = User;
                        return ctrl.XuatDanhSachBangCapTheoID(DegreeID);
                    case "xuattatca":                        
                        ctrl.ServerPath = Server.MapPath("~/App_Data");
                        ctrl.AppUser = User;
                        return ctrl.XuatTatCaBangCapThuyenVien();
                    default:
                        break;
                }
            }
            return View();
        }

        public ActionResult ThongKeTheoLoaiChucDanh()
        {
            //mặc định là lấy ra danh sách bằng cấp
            IQueryable<DIC_INTERSHIP> danhSach = db.DIC_INTERSHIP;
            ViewBag.IntershipID = new SelectList(danhSach, "IntershipID", "IntershipName");
            return View();
        }
        //[Authorize(Roles = "Quan tri")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongKeTheoLoaiChucDanh(int IntershipID, string answer)
        {
            if (ModelState.IsValid)
            {
                switch (answer)
                {
                    case "xemthongtin":
                        var result = db.sp_LayDSThuyenVienTheoLoaiChucDanh(IntershipID).ToList();
                        ViewBag.BangKe = result;
                        IQueryable<DIC_INTERSHIP> danhSach = db.DIC_INTERSHIP;
                        ViewBag.IntershipID = new SelectList(danhSach, "IntershipID", "IntershipName", IntershipID);
                        return View();
                    case "xuatexcel":
                        XuatWordExcelController ctrl = new XuatWordExcelController();
                        ctrl.ServerPath = Server.MapPath("~/App_Data");
                        ctrl.AppUser = User;
                        return ctrl.XuatDSThuyenVienTheoLoaiChucDanh(IntershipID);
                    default:
                        break;
                }
            }
            return View();
        }

    }
}