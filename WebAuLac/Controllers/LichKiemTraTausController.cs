using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAuLac.Controllers;
using WebAuLac.Models;

namespace WebAuLac
{
    public class LichKiemTraTausController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: LichKiemTraTaus
        public ActionResult Index()
        {
            DateTime ngay = DateTime.Now;
            var result2 = db.sp_T_LayLichTauTrongDieuDongBacThang(ngay).ToList();
            //thêm vào bảng tableDieuDong           
            List<tableDieuDongBacThang> result = new List<tableDieuDongBacThang>();
            ChuyenTuStoreLichTau(result2, result);
            ViewBag.BangKe = result;
            List<string> tieuDe = new List<string>();
            //tính ra tiêu đề của bảng
            for (int i =-10; i<11; i++)
            {
                DateTime dateMonthsAgo = ngay.AddMonths(i);
                tieuDe.Add("T" + dateMonthsAgo.ToString("MM/yy"));
            }
            ViewBag.TieuDe = tieuDe;
            return View();
        }
        public ActionResult locChucDanh()
        {
            return PartialView(db.DIC_POSITION.Where(x => x.PositionID >=20 && x.PositionID <=27));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult locChucDanh(int[] chkPos)
        {
            if (ModelState.IsValid)
            {
                DateTime ngay = DateTime.Now;
                //thêm vào bảng tableDieuDong           
                List<tableDieuDongBacThang> result = new List<tableDieuDongBacThang>();               
                foreach (int idPos in chkPos)
                {
                    string pos = "CAPT";
                    switch (idPos)
                    {
                        case 20:
                            pos = "CAPT";
                            break;
                        case 21:
                            pos = "C/O";
                            break;
                        case 22:
                            pos = "2/O";
                            break;
                        case 23:
                            pos = "3/O";
                            break;
                    }
                    if (idPos != 0)
                    {
                        var result2 = db.sp_T_LayDieuDongBacThangTheoChucDanh(idPos,ngay,pos).ToList();
                        ChuyenTuStoreDieuDongBacThang(result2, result);
                    }
                }               
                //sắp xếp theo tàu
                var result1 = db.sp_T_LayLichTauTrongDieuDongBacThang(ngay).ToList();
                ChuyenTuStoreLichTau(result1, result);
                result = result.OrderBy(x => x.DepartmentID).ThenBy(x => x.Description).ToList();                
                ViewBag.rowspan = 2;//mỗi tàu có mấy dòng
                List<string> tieuDe = new List<string>();
                //tính ra tiêu đề của bảng
                for (int i = -10; i < 11; i++)
                {
                    DateTime dateMonthsAgo = ngay.AddMonths(i);
                    tieuDe.Add("T" + dateMonthsAgo.ToString("MM/yy"));
                }
                ViewBag.TieuDe = tieuDe;
                return PartialView("DanhSachDieuDong", result);
            }
            return PartialView("DanhSachDieuDong", null);
        }
        public ActionResult DieuDongBacThang()
        {
            return View();
        }
        public ActionResult LayNguoiThayThe(int DieuDongID)
        {
            //lấy thông tin để gọi store
            DieuDongBacThang dd = db.DieuDongBacThangs.Find(DieuDongID);
            var result = db.sp_T_LayNguoiThayThe(dd.NguoiDangDiID, dd.PositionID).ToList();
            return PartialView("dsDuTru", result);
        }
        public ActionResult DanhSachDieuDong()
        {
            DateTime ngay = DateTime.Now;
            //thêm vào bảng tableDieuDong           
            List<tableDieuDongBacThang> result = new List<tableDieuDongBacThang>();
            //sắp xếp theo tàu
            var result1 = db.sp_T_LayLichTauTrongDieuDongBacThang(ngay).ToList();
            ChuyenTuStoreLichTau(result1, result);
            result = result.OrderBy(x => x.DepartmentID).ThenBy (x=> x.Description ).ToList();
            ViewBag.rowspan = 2;//mỗi tàu có mấy dòng
            List<string> tieuDe = new List<string>();
            //tính ra tiêu đề của bảng
            for (int i = -10; i < 11; i++)
            {
                DateTime dateMonthsAgo = ngay.AddMonths(i);
                tieuDe.Add("T" + dateMonthsAgo.ToString("MM/yy"));
            }
            ViewBag.TieuDe = tieuDe;
            return PartialView(result);
        }
        public void ChuyenTuStoreLichTau(List<sp_T_LayLichTauTrongDieuDongBacThang_Result> dlDauVao, List<tableDieuDongBacThang> dlRa)
        {
            dlDauVao = dlDauVao.OrderBy(x => x.DepartmentID).ThenBy(x => x.Description).ToList();
            int idDept = 0;
            tableDieuDongBacThang item = new tableDieuDongBacThang();
            foreach (sp_T_LayLichTauTrongDieuDongBacThang_Result l in dlDauVao)
            {
                if (l.DepartmentID !=idDept )
                {
                    item = new tableDieuDongBacThang();
                }   
                if (l.cot1 != "")
                {
                    item.cot1 = l.cot1;
                }
                if (l.cot10 != "")
                {
                    item.cot10 = l.cot10;
                }
                if (l.cot11 != "")
                {
                    item.cot11 = l.cot11;
                }
                if (l.cot12 != "")
                {
                    item.cot12 = l.cot12;
                }
                if (l.cot13 != "")
                {
                    item.cot13 = l.cot13;
                }
                if (l.cot14 != "")
                {
                    item.cot14 = l.cot14;
                }
                if (l.cot15 != "")
                {
                    item.cot15 = l.cot15;
                }
                if (l.cot16 != "")
                {
                    item.cot16 = l.cot16;
                }
                if (l.cot17 != "")
                {
                    item.cot17 = l.cot17;
                }
                if (l.cot18 != "")
                {
                    item.cot18 = l.cot18;
                }
                if (l.cot19 != "")
                {
                    item.cot19 = l.cot19;
                }
                if (l.cot20 != "")
                {
                    item.cot20 = l.cot20;
                }
                if (l.cot21 != "")
                {
                    item.cot21 = l.cot21;
                }
                if (l.cot3 != "")
                {
                    item.cot3 = l.cot3;
                }
                if (l.cot4 != "")
                {
                    item.cot14 = l.cot14;
                }
                if (l.cot5 != "")
                {
                    item.cot5 = l.cot5;
                }
                if (l.cot6 != "")
                {
                    item.cot6 = l.cot6;
                }
                if (l.cot7 != "")
                {
                    item.cot7 = l.cot7;
                }
                if (l.cot8 != "")
                {
                    item.cot8 = l.cot8;
                }
                if (l.cot9 != "")
                {
                    item.cot9 = l.cot9;
                }
                item.DepartmentID = l.DepartmentID.Value;
                item.Description = l.Description;
                item.Loai = l.Loai;
                if (idDept !=l.DepartmentID)
                {
                    dlRa.Add(item);
                    idDept = l.DepartmentID.Value;
                }                
            }
        }
        public void ChuyenTuStoreDieuDongBacThang(List<sp_T_LayDieuDongBacThangTheoChucDanh_Result> dlDauVao, List<tableDieuDongBacThang> dlRa)
        {
            foreach (sp_T_LayDieuDongBacThangTheoChucDanh_Result l in dlDauVao)
            {
                tableDieuDongBacThang item = new tableDieuDongBacThang();
                item.cot1 = l.cot1;
                item.cot10 = l.cot10;
                item.cot11 = l.cot11;
                item.cot12 = l.cot12;
                item.cot13 = l.cot13;
                item.cot14 = l.cot14;
                item.cot15 = l.cot15;
                item.cot16 = l.cot16;
                item.cot17 = l.cot17;
                item.cot18 = l.cot18;
                item.cot19 = l.cot19;
                item.cot2 = l.cot2;
                item.cot20 = l.cot20;
                item.cot21 = l.cot21;
                item.cot3 = l.cot3;
                item.cot4 = l.cot4;
                item.cot5 = l.cot5;
                item.cot6 = l.cot6;
                item.cot7 = l.cot7;
                item.cot8 = l.cot8;
                item.cot9 = l.cot9;
                item.DepartmentID = l.DepartmentID.Value;
                item.Description = l.Description;
                item.Loai = l.Loai;
                dlRa.Add(item);
            }
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
