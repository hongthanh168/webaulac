using HRM.Controllers;
using OfficeOpenXml;
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
    [Authorize]
    public class HRM_EMPLOYEE_DEGREEController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        //Set cache để nó load lại cái mới cập nhật
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]        
        public ActionResult DegreeOfOne(int id)
        {
            var hRM_EMPLOYEE_DEGREE = db.HRM_EMPLOYEE_DEGREE.Include(h => h.DIC_DEGREE).Include(h => h.DIC_SCHOOL).Where(h=> h.EmployeeID==id).OrderBy(h=>h.DIC_DEGREE.STT);
            ViewBag.EmployeeID = id;
            return PartialView("_DegreeOfOne",hRM_EMPLOYEE_DEGREE.ToList());
        }
        // GET: HRM_EMPLOYEE_DEGREE/Details/5
        [Authorize(Roles = "BangCap")]
        [Authorize(Roles = "CreateTTTV")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE_DEGREE hRM_EMPLOYEE_DEGREE = db.HRM_EMPLOYEE_DEGREE.Find(id);
            if (hRM_EMPLOYEE_DEGREE == null)
            {
                return HttpNotFound();
            }
            return PartialView(hRM_EMPLOYEE_DEGREE);
        }
               

        // POST: HRM_EMPLOYEE_DEGREE/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "BangCap")]
        [Authorize(Roles = "CreateTTTV")]
        public ActionResult Create([Bind(Include = "EmployeeDegreeID,EmployeeID,DegreeID,SchoolID,DegreeNo,DegreeDate,ExpirationDate")] HRM_EMPLOYEE_DEGREE hRM_EMPLOYEE_DEGREE)
        {
            if (ModelState.IsValid)
            {
                //Kiểm tra bằng cấp đó có tồn tại chưa để thêm mới
                var q = from a in db.HRM_EMPLOYEE_DEGREE
                        where a.EmployeeID == hRM_EMPLOYEE_DEGREE.EmployeeID
                        && a.DegreeID == hRM_EMPLOYEE_DEGREE.DegreeID
                        select a;
                if (q.Count() > 0) //bằng cấp đã tồn tại thì xóa đi, tạo mới
                {
                    HRM_EMPLOYEE_DEGREE tontaibangcap = db.HRM_EMPLOYEE_DEGREE.Find(q.First().EmployeeDegreeID);
                    db.HRM_EMPLOYEE_DEGREE.Remove(tontaibangcap);
                    db.SaveChanges();//xóa xong thì tiếp tục thêm mới
                }
                db.HRM_EMPLOYEE_DEGREE.Add(hRM_EMPLOYEE_DEGREE);
                db.SaveChanges();
                return RedirectToAction("DegreeOfOne", new { id=hRM_EMPLOYEE_DEGREE.EmployeeID });
            }

            ViewBag.DegreeID = new SelectList(db.DIC_DEGREE, "DegreeID", "DegreeName", hRM_EMPLOYEE_DEGREE.DegreeID);
            ViewBag.SchoolID = new SelectList(db.DIC_SCHOOL, "SchoolID", "SchoolName", hRM_EMPLOYEE_DEGREE.SchoolID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_DEGREE.EmployeeID);
            return View(hRM_EMPLOYEE_DEGREE);
        }
        // POST: HRM_EMPLOYEE_DEGREE/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "BangCap")]
        [Authorize(Roles = "CreateTTTV")]
        public ActionResult CreateAll(FormCollection form)
        {
            string name = Request.Form["chon"];
            int EmployeeID = Convert.ToInt32(Request.Form["EmployeeID"]);


            //Kiểm tra form để xem thêm mới theo cách nào
            //1 là thêm theo chức danh
            //Lấy thông tin chức danh của nhân viên theo hiện tại
            if (name =="1")
            {
                var q = from a in db.HRM_EMPLOYMENTHISTORY
                        where a.EmployeeID == EmployeeID
                        orderby a.EmploymentHistoryID descending
                        select a;
                if (q != null)
                {
                    int chucdanhid = (int)q.First().PositionID;
                    //Thêm mới hàng loạt bằng cấp có trong bảng DIC_POSITION_DEGREE
                    //Lây danh sách bằng cấp theo chức danh
                    var dsbangcap = (from a in db.DIC_POSITION_DEGREE
                                    where a.PositionID == chucdanhid
                                    select a).ToList();
                    foreach (DIC_POSITION_DEGREE item in dsbangcap)
                    {
                        //Thêm mới thông tin vào bảng bằng cấp của nhân viên
                        HRM_EMPLOYEE_DEGREE bcnv = new HRM_EMPLOYEE_DEGREE();
                        bcnv.DegreeID = item.DegreeID;
                        bcnv.EmployeeID = EmployeeID;
                        db.HRM_EMPLOYEE_DEGREE.Add(bcnv);
                        db.SaveChanges();
                    }
                }
            }
            else
            {
                //2 là thêm theo danh sách bằng cấp hiện có trong danh mục
                var dsbangcap =( from a in db.DIC_DEGREE
                                select a).ToList();
                foreach (DIC_DEGREE item in dsbangcap)
                {
                    //Thêm mới thông tin vào bảng bằng cấp của nhân viên
                    HRM_EMPLOYEE_DEGREE bcnv = new HRM_EMPLOYEE_DEGREE();
                    bcnv.DegreeID = item.DegreeID;
                    bcnv.EmployeeID = EmployeeID;
                    db.HRM_EMPLOYEE_DEGREE.Add(bcnv);
                    db.SaveChanges();
                }

            }


            return RedirectToAction("DegreeOfOne", new { id =EmployeeID });
        }
        //create one
        [Authorize(Roles = "BangCap")]
        [Authorize(Roles = "CreateTTTV")]
        public ActionResult CreateOneCheck(int EmployeeID)
        {
            HRM_EMPLOYEE_DEGREE item = new HRM_EMPLOYEE_DEGREE();
            ViewBag.EmployeeID = EmployeeID;
            item.EmployeeID = EmployeeID;
            ViewBag.idThuyenVien = EmployeeID;
            //Hiển thị ngày tháng năm sinh và nơi sinh của thuyền viên để kiểm tra bằng mắt
            var thuyenvien = (from a in db.HRM_EMPLOYEE
                              where a.EmployeeID == EmployeeID
                              select a).First();
            ViewBag.HoTen = thuyenvien.FirstName + " " + thuyenvien.LastName;
            ViewBag.NgaySinh = thuyenvien.BirthDay.Value.ToShortDateString();
            ViewBag.NoiSinh = thuyenvien.BirthPlace;
            ViewBag.Photo = thuyenvien.Photo;
            var positionID = from a in db.DIC_POSITION
                             where a.GroupPositionID != 4
                             select a;

            ViewBag.DegreeID = new SelectList(db.DIC_DEGREE, "DegreeID", "DegreeName");
            ViewBag.PositionID = new SelectList(positionID, "PositionID", "PositionName");
            ViewBag.SchoolID = new SelectList(db.DIC_SCHOOL, "SchoolID", "SchoolName");
            return PartialView("CreateOneCheck", item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "BangCap")]
        [Authorize(Roles = "CreateTTTV")]
        public ActionResult CreateOneCheck([Bind(Include = "EmployeeDegreeID,EmployeeID,DegreeID,SchoolID,PositionID,DegreeNo,DegreeDate,ExpirationDate,Qualification,GhiChu,IsBC")] HRM_EMPLOYEE_DEGREE hRM_EMPLOYEE_DEGREE)
        {

            if (ModelState.IsValid)
            {
                //Kiểm tra bằng cấp đó có tồn tại chưa để thêm mới
                var q = from a in db.HRM_EMPLOYEE_DEGREE
                        where a.EmployeeID == hRM_EMPLOYEE_DEGREE.EmployeeID
                        && a.DegreeID == hRM_EMPLOYEE_DEGREE.DegreeID
                        select a;
                if (q.Count() > 0)
                {
                     
                        HRM_EMPLOYEE_DEGREE tontaibangcap = db.HRM_EMPLOYEE_DEGREE.Find(q.First().EmployeeDegreeID);
                        db.HRM_EMPLOYEE_DEGREE.Remove(tontaibangcap);
                        db.SaveChanges();//xóa xong thì tiếp tục thêm mới
                    
                }
                db.HRM_EMPLOYEE_DEGREE.Add(hRM_EMPLOYEE_DEGREE);
                db.SaveChanges();
                //return RedirectToAction("Index");
                //return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                return RedirectToAction("DegreeOfOne", new { id = hRM_EMPLOYEE_DEGREE.EmployeeID });
            }

            ViewBag.DegreeID = new SelectList(db.DIC_DEGREE, "DegreeID", "DegreeName", hRM_EMPLOYEE_DEGREE.DegreeID);
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYEE_DEGREE.PositionID);
            ViewBag.SchoolID = new SelectList(db.DIC_SCHOOL, "SchoolID", "SchoolName", hRM_EMPLOYEE_DEGREE.SchoolID);
            // ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_DEGREE.EmployeeID);
            // return View(hRM_EMPLOYEE_DEGREE);
            return PartialView("CreateOneCheck", hRM_EMPLOYEE_DEGREE);
           // return View(hRM_EMPLOYEE_DEGREE);
        }

        [Authorize(Roles = "BangCap")]
        [Authorize(Roles = "CreateTTTV")]
        public ActionResult CreateOne(int id)
        {
            HRM_EMPLOYEE_DEGREE item = new HRM_EMPLOYEE_DEGREE();
            item.EmployeeID = id;
            ViewBag.DegreeID = new SelectList(db.DIC_DEGREE, "DegreeID", "DegreeName");
            ViewBag.SchoolID = new SelectList(db.DIC_SCHOOL, "SchoolID", "SchoolName");
            return PartialView(item);
        }
        //THêm mới tất cả bằng cấp của nhân viên theo chức danh hiện tại
        [Authorize(Roles = "BangCap")]
        [Authorize(Roles = "CreateTTTV")]
        public ActionResult CreateAll(int id)
        {
            HRM_EMPLOYEE_DEGREE item = new HRM_EMPLOYEE_DEGREE();
            item.EmployeeID = id;
            ViewBag.DegreeID = new SelectList(db.DIC_DEGREE, "DegreeID", "DegreeName");
            ViewBag.SchoolID = new SelectList(db.DIC_SCHOOL, "SchoolID", "SchoolName");
            return PartialView(item);
        }
        // GET: HRM_EMPLOYEE_DEGREE/Edit/5
        [Authorize(Roles = "BangCap")]
        [Authorize(Roles = "CreateTTTV")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE_DEGREE hRM_EMPLOYEE_DEGREE = db.HRM_EMPLOYEE_DEGREE.Find(id);
            if (hRM_EMPLOYEE_DEGREE == null)
            {
                return HttpNotFound();
            }
            ViewBag.DegreeID = new SelectList(db.DIC_DEGREE, "DegreeID", "DegreeName", hRM_EMPLOYEE_DEGREE.DegreeID);
            ViewBag.SchoolID = new SelectList(db.DIC_SCHOOL, "SchoolID", "SchoolName", hRM_EMPLOYEE_DEGREE.SchoolID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_DEGREE.EmployeeID);
            return PartialView(hRM_EMPLOYEE_DEGREE);
        }

        // POST: HRM_EMPLOYEE_DEGREE/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "BangCap")]
        [Authorize(Roles = "CreateTTTV")]
        public ActionResult Edit([Bind(Include = "EmployeeDegreeID,EmployeeID,DegreeID,SchoolID,DegreeNo,DegreeDate,ExpirationDate")] HRM_EMPLOYEE_DEGREE hRM_EMPLOYEE_DEGREE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRM_EMPLOYEE_DEGREE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DegreeOfOne", new { id = hRM_EMPLOYEE_DEGREE.EmployeeID });
            }
            ViewBag.DegreeID = new SelectList(db.DIC_DEGREE, "DegreeID", "DegreeName", hRM_EMPLOYEE_DEGREE.DegreeID);
            ViewBag.SchoolID = new SelectList(db.DIC_SCHOOL, "SchoolID", "SchoolName", hRM_EMPLOYEE_DEGREE.SchoolID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_DEGREE.EmployeeID);
            return PartialView(hRM_EMPLOYEE_DEGREE);
        }

        // GET: HRM_EMPLOYEE_DEGREE/Delete/5
        [Authorize(Roles = "BangCap")]
        [Authorize(Roles = "CreateTTTV")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE_DEGREE hRM_EMPLOYEE_DEGREE = db.HRM_EMPLOYEE_DEGREE.Find(id);
            if (hRM_EMPLOYEE_DEGREE == null)
            {
                return HttpNotFound();
            }
            return PartialView(hRM_EMPLOYEE_DEGREE);
        }

        //THêm mới tất cả bằng cấp của nhân viên theo chức danh hiện tại
        [Authorize(Roles = "BangCap")]
        [Authorize(Roles = "CreateTTTV")]
        public ActionResult DeleteAll(int id)
        {
            HRM_EMPLOYEE_DEGREE hRM_EMPLOYEE_DEGREE = db.HRM_EMPLOYEE_DEGREE.Find(id);
            return PartialView(hRM_EMPLOYEE_DEGREE);
        }

        // POST: HRM_EMPLOYEE_DEGREE/Delete/5
        [Authorize(Roles = "BangCap")]
        [Authorize(Roles = "CreateTTTV")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HRM_EMPLOYEE_DEGREE hRM_EMPLOYEE_DEGREE = db.HRM_EMPLOYEE_DEGREE.Find(id);
            hRM_EMPLOYEE_DEGREE.DegreeNo = null;
            hRM_EMPLOYEE_DEGREE.DegreeDate = null;
            hRM_EMPLOYEE_DEGREE.SchoolID = null;
            //db.HRM_EMPLOYEE_DEGREE.Remove(hRM_EMPLOYEE_DEGREE);
            db.Entry(hRM_EMPLOYEE_DEGREE).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DegreeOfOne", new { id = hRM_EMPLOYEE_DEGREE.EmployeeID });
        }
        [Authorize(Roles = "BangCap")]
        [Authorize(Roles = "CreateTTTV")]
        [HttpPost, ActionName("DeleteAll")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAll(FormCollection form)
        {
            string name = Request.Form["chon"];
            int EmployeeID = Convert.ToInt32(Request.Form["EmployeeID"]);
            int EmployeeDegreeID = Convert.ToInt32(Request.Form["EmployeeDegreeID"]);

            //Kiểm tra form để xem xóa theo cách nào
            //1 là xóa hoàn toàn
            //Lấy thông tin chức danh của nhân viên theo hiện tại
            if (name == "1")
            {
                HRM_EMPLOYEE_DEGREE hRM_EMPLOYEE_DEGREE = db.HRM_EMPLOYEE_DEGREE.Find(EmployeeDegreeID);
                 db.HRM_EMPLOYEE_DEGREE.Remove(hRM_EMPLOYEE_DEGREE);
                 db.SaveChanges();
            }
            else
            {
                //2 là Xóa giữ thông tin
                HRM_EMPLOYEE_DEGREE hRM_EMPLOYEE_DEGREE = db.HRM_EMPLOYEE_DEGREE.Find(EmployeeDegreeID);
                hRM_EMPLOYEE_DEGREE.DegreeNo = null;
                hRM_EMPLOYEE_DEGREE.DegreeDate = null;
                hRM_EMPLOYEE_DEGREE.SchoolID = null;
                hRM_EMPLOYEE_DEGREE.ExpirationDate = null;
                //db.HRM_EMPLOYEE_DEGREE.Remove(hRM_EMPLOYEE_DEGREE);
                db.Entry(hRM_EMPLOYEE_DEGREE).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("DegreeOfOne", new { id = EmployeeID });
        }
        //in danh sách bằng cấp của 1 người
        public FileResult XuatDanhSachBangCapTheoThuyenVien(int EmployeeID)
        {
            XuatWordExcelController ctrl = new XuatWordExcelController();
            ctrl.ServerPath = Server.MapPath("~/App_Data");
            ctrl.AppUser = User;
            return ctrl.XuatDanhSachBangCapTheoThuyenVien(EmployeeID);

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
