using HRM.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using WebAuLac.Models;

namespace WebAuLac.Controllers
{
    [Authorize(Roles = "Boss, HR, EduCenter, DaoTao, View, HoSoPhapLy")]   
    public class HoSoTauController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: HoSoTau
        public ActionResult Index()
        {
            var hoSoTaus = db.HoSoTaus.Include(h => h.ChungChiTau).Include(h => h.DIC_DEPARTMENT);
            return View(hoSoTaus.ToList());
        }

        public ActionResult ErrorNotExistsView()
        {
            return View("ErrorNotExistsView");
        }

        public ActionResult DSChungChiTau()
        {
            //lấy ra danh sách khối tàu
            var dp = db.DIC_DEPARTMENT.Where(x => x.ParentID == 8 && x.DepartmentID!=17);
            ViewBag.DepartmentID = new SelectList(dp, "DepartmentID", "DepartmentName");
            var hoSoTaus = db.sp_LayDSChungChiTau().OrderBy(x => x.STT);
            return View(hoSoTaus.ToList());
        }

        public ActionResult DSTau()
        {
            //lấy ra danh sách khối tàu
            var dp = db.DIC_DEPARTMENT.Where(x => x.ParentID == 8 && x.DepartmentID != 17);
            
            return View(dp.ToList());
        }

        public ActionResult TheoDoiGiayToTau()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TheoDoiGiayToTau([Bind(Include = "NgayBaoCao")] sp_BangTheoDoiGiayToTau_Result obj, string answer)
        {
            if (ModelState.IsValid)
            {
               

                switch (answer)
                {
                    case "xemthongtin":
                        var result = db.sp_BangTheoDoiGiayToTau(obj.NgayBaoCao).ToList();
                        //lấy ra tổng chi phí và tổng người tham gia
                        DateTime ngaybc = Convert.ToDateTime(obj.NgayBaoCao);
                        ViewBag.TuThang = ngaybc.Month;
                        ViewBag.Nam1 = ngaybc.Year.ToString().Substring(2, 2);
                        ViewBag.Nam2 = (ngaybc.Year + 1).ToString().Substring(2, 2);
                        if (ngaybc.Month == 11)//Hai tháng cuối năm sẽ qua thêm 1 năm mới
                        {
                            ViewBag.Nam3 = Convert.ToInt32(ViewBag.Nam2) + 1;
                            ViewBag.DenThang = 1;
                            ViewBag.DonCot3 = 1;
                            ViewBag.DonCot2 = 12;
                        }
                        else if (ngaybc.Month == 12)
                        {
                            ViewBag.Nam3 = Convert.ToInt32(ViewBag.Nam2) + 1;
                            ViewBag.DenThang = 2;
                            ViewBag.DonCot3 = 2;
                            ViewBag.DonCot2 = 12;
                        }
                        else
                        {
                            ViewBag.DenThang = ((ngaybc.Month + 15) % 13);
                            ViewBag.DonCot2 = 15 - ((12 - ngaybc.Month) + 1);
                        }
                        ViewBag.BangKe = result;
                        ViewBag.DonCot1 = (12 - ngaybc.Month) + 1;

                        return View(obj);
                    case "xuatexcel":
                        XuatWordExcelController ctrl = new XuatWordExcelController();
                        ctrl.ServerPath = Server.MapPath("~/App_Data");
                        ctrl.AppUser = User;
                        return ctrl.XuatBangTheoDoiGiayToTau(Convert.ToDateTime(obj.NgayBaoCao));
                    default:
                        break;
                }


                return View();


            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DSChungChiTau(int? DepartmentID, int? soThangHetHan)
        {
            //lấy ra danh sách khối tàu
            var hoSoTaus = db.sp_LayDSChungChiTau().ToList();
            if (DepartmentID.HasValue)
            {
                hoSoTaus = hoSoTaus.Where(x => x.DepartmentID == DepartmentID.Value).ToList();
            }
            if (soThangHetHan.HasValue)
            {
                hoSoTaus = hoSoTaus.Where(x => x.NgayHetHan.HasValue && ThanhUtilities.MonthDifference(DateTime.Now, x.NgayHetHan) <= soThangHetHan.Value).ToList();
            }
            var dp = db.DIC_DEPARTMENT.Where(x => x.ParentID == 8 && x.DepartmentID != 17);
            ViewBag.DepartmentID = new SelectList(dp, "DepartmentID", "DepartmentName", DepartmentID);
            return View(hoSoTaus.OrderBy(x => x.STT));
        }
        // GET: HoSoTau/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoSoTau hoSoTau = db.HoSoTaus.Find(id);
            if (hoSoTau == null)
            {
                return HttpNotFound();
            }
            return View(hoSoTau);
        }

        // GET: HoSoTau/Create
        [Authorize(Roles = "HoSoPhapLy")]
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            ViewBag.ChungChiID = new SelectList(db.ChungChiTaus, "ChungChiID", "TenChungChi");
            var dp = db.DIC_DEPARTMENT.Where(x => x.ParentID == 8 && x.DepartmentID != 17);
            ViewBag.DepartmentID = new SelectList(dp, "DepartmentID", "DepartmentName");
            return PartialView();
        }

        // POST: HoSoTau/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HoSoPhapLy")]
        [Authorize(Roles = "Create")]
        public ActionResult Create([Bind(Include = "HoSoTauID,DepartmentID,ChungChiID,SoHoSo,NgayCap,NoiCap,NgayHetHan,FileDinhKem")] HoSoTau hoSoTau)
        {
            if (ModelState.IsValid)
            {
                db.HoSoTaus.Add(hoSoTau);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ChungChiID = new SelectList(db.ChungChiTaus, "ChungChiID", "TenChungChi", hoSoTau.ChungChiID);
            var dp = db.DIC_DEPARTMENT.Where(x => x.ParentID == 8 && x.DepartmentID != 17);
            ViewBag.DepartmentID = new SelectList(dp, "DepartmentID", "DepartmentName", hoSoTau.DepartmentID);            
            return View(hoSoTau);
        }

        // GET: HoSoTau/Edit/5
        [Authorize(Roles = "HoSoPhapLy")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoSoTau hoSoTau = db.HoSoTaus.Find(id);
            if (hoSoTau == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChungChiID = new SelectList(db.ChungChiTaus, "ChungChiID", "TenChungChi", hoSoTau.ChungChiID);
            var dp = db.DIC_DEPARTMENT.Where(x => x.ParentID == 8 && x.DepartmentID != 17);
            ViewBag.DepartmentID = new SelectList(dp, "DepartmentID", "DepartmentName", hoSoTau.DepartmentID);
            return PartialView(hoSoTau);
        }

        // POST: HoSoTau/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HoSoPhapLy")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit([Bind(Include = "HoSoTauID,DepartmentID,ChungChiID,SoHoSo,NgayCap,NoiCap,NgayHetHan,FileDinhKem")] HoSoTau hoSoTau)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(hoSoTau).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }
            ViewBag.ChungChiID = new SelectList(db.ChungChiTaus, "ChungChiID", "TenChungChi", hoSoTau.ChungChiID);
            var dp = db.DIC_DEPARTMENT.Where(x => x.ParentID == 8 && x.DepartmentID != 17);
            ViewBag.DepartmentID = new SelectList(dp, "DepartmentID", "DepartmentName", hoSoTau.DepartmentID);
            HoSoTau hoSoTau1 = db.HoSoTaus.Find(hoSoTau.HoSoTauID);
            return PartialView(hoSoTau1);
        }
        [WebMethod]
        [Authorize(Roles = "HoSoPhapLy")]
        [Authorize(Roles = "Create")]
        public JsonResult CreateWithUpload(HoSoTau hoSoTau, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string sFileDinhKem = "";
                if (upload != null && upload.ContentLength > 0)
                {
                    ////Is the file too big to upload?
                    //int fileSize = upload.ContentLength;
                    //if (fileSize > (20000000))  //quá 20M
                    //{

                    //    return Json(new { success = false, message ="Dung lượng file vượt quá 20M" }, JsonRequestBehavior.AllowGet);
                    //}
                    sFileDinhKem = Path.GetFileName(upload.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/HoSoTau/"),
                                        System.IO.Path.GetFileName(upload.FileName));
                    upload.SaveAs(path);
                    //Copy file bỏ vào thư mục công ty trong App_Data
                    hoSoTau.FileDinhKem = sFileDinhKem;

                    //thêm upload hình ảnh vào đây
                    //byte[] imageBytes = null;
                    //using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    //{
                     //   imageBytes = reader.ReadBytes(upload.ContentLength);
                    //}
                   // hoSoTau.fileHoSo = imageBytes;
                }
                db.HoSoTaus.Add(hoSoTau);
                db.SaveChanges();
                //lưu trước upload sau
                //if (upload != null && upload.ContentLength > 0)
                //{
                //    var path = Path.Combine(Server.MapPath("~/App_Data/HoSoTau/"),
                //                        System.IO.Path.GetFileName(upload.FileName));
                //    upload.SaveAs(path);                    
                //}
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        [WebMethod]
        [Authorize(Roles = "HoSoPhapLy")]
        [Authorize(Roles = "Create")]
        public JsonResult EditWithUpload(HoSoTau hoSoTau, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string sFileDinhKem = "";
                if (upload == null)
                {
                    //Không sửa file thì lấy lại tên file cũ
                    sFileDinhKem = db.HoSoTaus.Where(h => h.HoSoTauID == hoSoTau.HoSoTauID).Select(h => h.FileDinhKem).First();
                    hoSoTau.FileDinhKem = sFileDinhKem;

                    //thanh thêm ở đây
                    //tìm lại file cũ
                    //var q = from temp in db.HoSoTaus where temp.HoSoTauID == hoSoTau.HoSoTauID select temp.fileHoSo;
                    //byte[] cover = q.First();
                    //hoSoTau.fileHoSo = cover;
                }
                if (upload != null && upload.ContentLength > 0)
                {
                    var img = Path.GetFileName(upload.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/HoSoTau/"),
                                        System.IO.Path.GetFileName(upload.FileName));
                    upload.SaveAs(path);
                    //Copy file bỏ vào thư mục công ty trong App_Data
                    hoSoTau.FileDinhKem = img;

                    //sFileDinhKem = Path.GetFileName(upload.FileName);                    
                    ////Copy file bỏ vào thư mục hồ sơ tàu trong App_Data
                    //hoSoTau.FileDinhKem = sFileDinhKem;

                    //thêm upload hình ảnh vào đây
                    //byte[] imageBytes = null;
                    //using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    //{
                    //    imageBytes = reader.ReadBytes(upload.ContentLength);
                    //}
                    //hoSoTau.fileHoSo = imageBytes;

                }
                db.Entry(hoSoTau).State = EntityState.Modified;
                db.SaveChanges();
                ////lưu trước upload sau
                //if (upload != null && upload.ContentLength > 0)
                //{
                //    //Copy file bỏ vào thư mục hồ sơ tàu trong App_Data                    
                //    var path = Path.Combine(Server.MapPath("~/App_Data/HoSoTau/"),
                //                        System.IO.Path.GetFileName(upload.FileName));
                //    upload.SaveAs(path);
                //}
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPdf(string fileName)
        {
            try
            {
                var fileStream = new FileStream(Server.MapPath("~/App_Data/HoSoTau/") + fileName,
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
        public ActionResult GetPdf2(int id)
        {
            HoSoTau hoSo = db.HoSoTaus.Find(id);
            try
            {
                if (hoSo.fileHoSo.Length > 0)
                {
                    return File(hoSo.fileHoSo, "application/pdf");
                }
                else
                {
                    return View("NotFound");
                }
            }
            catch (Exception)
            {

                //throw; 

            }
            return View("ErrorNotExistsView");
        }
        //HAM XOA FILE DINH KEM
        [WebMethod]
        [Authorize(Roles = "HoSoPhapLy")]
        [Authorize(Roles = "Create")]
        public JsonResult XoaFile(int? HoSoTauID)
        {
            if (HoSoTauID.HasValue)
            {
                HoSoTau hoSoTau = db.HoSoTaus.Find(HoSoTauID);
                db.Entry(hoSoTau).State = EntityState.Modified;
                hoSoTau.FileDinhKem = null;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);

            }
            else return Json(null);

        }
        // GET: HoSoTau/Delete/5
        [Authorize(Roles = "HoSoPhapLy")]
        [Authorize(Roles = "Create")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoSoTau hoSoTau = db.HoSoTaus.Find(id);
            if (hoSoTau == null)
            {
                return HttpNotFound();
            }
            return PartialView(hoSoTau);
        }

        // POST: HoSoTau/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HoSoPhapLy")]
        [Authorize(Roles = "Create")]
        public ActionResult DeleteConfirmed(int id)
        {
            HoSoTau hoSoTau = db.HoSoTaus.Find(id);
            db.HoSoTaus.Remove(hoSoTau);
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
    }
}
