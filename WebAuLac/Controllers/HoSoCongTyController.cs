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
    [Authorize(Roles = "Boss, HR, EduCenter, DaoTao, View, HoSoPhapLy, BangCap")]
    
    public class HoSoCongTyController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: HoSoCongTy
        public ActionResult Index()
        {
            return View(db.HoSoCongTies.OrderBy(x => x.STT).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int? soThangHetHan)
        {
            var hoSos = db.HoSoCongTies.ToList();
            if (soThangHetHan.HasValue)
            {
                hoSos = hoSos.Where(x => x.NgayHetHan.HasValue && ThanhUtilities.MonthDifference(DateTime.Now, x.NgayHetHan) <= soThangHetHan.Value).ToList();
                return View(hoSos.OrderBy(x => x.STT));
            }else
            {
                return View(hoSos.OrderBy(x => x.STT));
            }            
        }
        
        [Authorize(Roles = "HoSoPhapLy")]
        [Authorize(Roles = "Create")]
        public ActionResult SapXep(int id, int lenXuong)
        {
            db.sp_T_UpdateSTTLenXuong(id, lenXuong);
            return RedirectToAction("Index");
        }

        public ActionResult ErrorNotExistsView()
        {
            return View("ErrorNotExistsView");
        }

        // GET: HoSoCongTy/Details/5
        [Authorize(Roles = "HoSoPhapLy")]
        [Authorize(Roles = "Create")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoSoCongTy hoSoCongTy = db.HoSoCongTies.Find(id);
            if (hoSoCongTy == null)
            {
                return HttpNotFound();
            }
            return View(hoSoCongTy);
        }

        // GET: HoSoCongTy/Create
        [Authorize(Roles = "HoSoPhapLy")]
        [Authorize(Roles = "Create")]        
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: HoSoCongTy/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
           
        
        [Authorize(Roles = "HoSoPhapLy")]
        [Authorize(Roles = "Create")]
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HoSoCongTyID,TenHoSo,SoHoSo,NoiCap,NgayHoSo,NgayHetHan,FileDinhKem,SoNgayCanhBao")] HoSoCongTy hoSoCongTy, HttpPostedFileBase upload)
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
					//Copy file bỏ vào thư mục công ty trong App_Data
                    hoSoCongTy.FileDinhKem = upload.FileName;
                    //hoSoCongTy.fileHoSo = imageBytes;
                }
                //tìm ra order
                int maxSTT = 0;
                try
                {
                    maxSTT = db.HoSoCongTies.Max(x => x.STT.Value);
                }
                catch { }
                hoSoCongTy.STT = maxSTT + 1;
                db.HoSoCongTies.Add(hoSoCongTy);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView(hoSoCongTy);
        }

        [WebMethod]
        [Authorize(Roles = "HoSoPhapLy")]
        [Authorize(Roles = "Create")]
        public JsonResult CreateWithUpload(HoSoCongTy hoSoCongTy, HttpPostedFileBase upload)

        {
           // var img = Path.GetFileName(upload.FileName);
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var img = Path.GetFileName(upload.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/HoSoCongTy/"),
                                        System.IO.Path.GetFileName(upload.FileName));
                    upload.SaveAs(path);
                    //Copy file bỏ vào thư mục công ty trong App_Data
                    hoSoCongTy.FileDinhKem = img;

                    //thêm upload hình ảnh vào đây
                    //byte[] imageBytes = null;
                    //using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    //{
                    //    imageBytes = reader.ReadBytes(upload.ContentLength);
                    //}
                    //hoSoCongTy.fileHoSo = imageBytes;
                }
                //tìm ra order
                int maxSTT = 0;
                try
                {
                    maxSTT = db.HoSoCongTies.Max(x => x.STT.Value);
                }
                catch { }
                hoSoCongTy.STT = maxSTT + 1;
                db.HoSoCongTies.Add(hoSoCongTy);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        [WebMethod]
        [Authorize(Roles = "HoSoPhapLy")]
        [Authorize(Roles = "Create")]
        public JsonResult EditWithUpload( HoSoCongTy hoSoCongTy, HttpPostedFileBase upload)
        {            
          
            if (ModelState.IsValid)
            {
                if (upload == null)
                {
                    //Không sửa file thì lấy lại tên file cũ
                    var query = db.HoSoCongTies.Where(h => h.HoSoCongTyID == hoSoCongTy.HoSoCongTyID).Select(h => h.FileDinhKem).First();
                    hoSoCongTy.FileDinhKem = query;

                    //tìm lại file cũ
                    //var q = from temp in db.HoSoCongTies where temp.HoSoCongTyID == hoSoCongTy.HoSoCongTyID select temp.FileDinhKem;
                    //string cover = q.First();
                    //hoSoCongTy.FileDinhKem = cover;
                }
                if (upload != null && upload.ContentLength > 0)
                {
                    var img = Path.GetFileName(upload.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/HoSoCongTy/"),
                                        System.IO.Path.GetFileName(upload.FileName));
                    upload.SaveAs(path);
                    //Copy file bỏ vào thư mục công ty trong App_Data
                    hoSoCongTy.FileDinhKem = img;

                    //thêm upload hình ảnh vào đây
                    //byte[] imageBytes = null;
                    //using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    //{
                    //    imageBytes = reader.ReadBytes(upload.ContentLength);
                    //}
                    //hoSoCongTy.fileHoSo = imageBytes;
                }
                db.Entry(hoSoCongTy).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
             
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        // GET: HoSoCongTy/Edit/5
        [Authorize(Roles = "HoSoPhapLy")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoSoCongTy hoSoCongTy = db.HoSoCongTies.Find(id);
            if (hoSoCongTy == null)
            {
                return HttpNotFound();
            }
            return PartialView(hoSoCongTy);
        }

        // POST: HoSoCongTy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HoSoPhapLy")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit([Bind(Include = "HoSoCongTyID,TenHoSo,SoHoSo,NoiCap,NgayHoSo,NgayHetHan,FileDinhKem,SoNgayCanhBao")] HoSoCongTy hoSoCongTy)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(hoSoCongTy).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }
            HoSoCongTy hoSoCongTy1 = db.HoSoCongTies.Find(hoSoCongTy.HoSoCongTyID);
            return PartialView(hoSoCongTy1);
        }

        // GET: HoSoCongTy/Delete/5
        [Authorize(Roles = "HoSoPhapLy")]
        [Authorize(Roles = "Create")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoSoCongTy hoSoCongTy = db.HoSoCongTies.Find(id);
            if (hoSoCongTy == null)
            {
                return HttpNotFound();
            }
            return PartialView(hoSoCongTy);
        }

        // POST: HoSoCongTy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HoSoPhapLy")]
        [Authorize(Roles = "Create")]
        public ActionResult DeleteConfirmed(int id)
        {
            HoSoCongTy hoSoCongTy = db.HoSoCongTies.Find(id);
            db.HoSoCongTies.Remove(hoSoCongTy);
            db.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPdf(string fileName)
        {
            try
            {
                var fileStream = new FileStream(Server.MapPath("~/App_Data/HoSoCongTy/") + fileName,
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
            HoSoCongTy hoSo = db.HoSoCongTies.Find(id);
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
        public JsonResult XoaFile(int? HoSoCongTyID)
        {
            if (HoSoCongTyID.HasValue)
            {
                HoSoCongTy hoSoCongTy = db.HoSoCongTies.Find(HoSoCongTyID);
                db.Entry(hoSoCongTy).State = EntityState.Modified;
                hoSoCongTy.FileDinhKem = null;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
              
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
