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
using System.Collections;

namespace WebAuLac.Controllers
{
    [Authorize]
    public class HRM_EMPLOYEE_DEGREE1Controller : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: HRM_EMPLOYEE_DEGREE1
        public async Task<ActionResult> Index()
        {
            var hRM_EMPLOYEE_DEGREE = db.HRM_EMPLOYEE_DEGREE.Include(h => h.DIC_DEGREE).Include(h => h.DIC_SCHOOL).Include(h => h.HRM_EMPLOYEE).Where(h=>h.IsBC==true);
            return View(await hRM_EMPLOYEE_DEGREE.ToListAsync());
        }
        public ActionResult LimitDegree(int EmployeeID)
        {
            //    var hRM_EMPLOYEE_DEGREE = db.HRM_EMPLOYEE_DEGREE.Include(h => h.DIC_DEGREE).Include(h => h.DIC_POSITION).Include(h => h.DIC_SCHOOL).Where(h => h.HRM_EMPLOYEE.EmployeeID==EmployeeID);
            var hRM_EMPLOYEE_DEGREE = db.HRM_EMPLOYEE_DEGREE.Where(h => (System.Data.Entity.Core.Objects.EntityFunctions.AddDays(DateTime.Now, (-1) * h.DIC_DEGREE.WarningBefore)) >= h.ExpirationDate || (System.Data.Entity.Core.Objects.EntityFunctions.AddDays(DateTime.Now, h.DIC_DEGREE.WarningBefore)) >= h.ExpirationDate).Include(h => h.DIC_SCHOOL).Where(h => h.HRM_EMPLOYEE.EmployeeID == EmployeeID);

            ViewBag.idNhanVien = EmployeeID;
            ViewBag.TenThuyenVien = db.HRM_EMPLOYEE.Where(h => h.EmployeeID == EmployeeID).Select(h => h.FirstName + " " + h.LastName).ToList().First();
            return View("LimitDegree", hRM_EMPLOYEE_DEGREE.ToList());
            //return View(await hRM_EMPLOYEE_ACCIDENT.ToListAsync());
        }
        public ActionResult Employees(int EmployeeID)
        {
            var hRM_EMPLOYEE_DEGREE = db.HRM_EMPLOYEE_DEGREE.Include(h => h.DIC_DEGREE).Where(h => h.HRM_EMPLOYEE.EmployeeID == EmployeeID);
            ViewBag.idThuyenVien = EmployeeID;
            var query = (from a in db.HRM_EMPLOYEE
                         where a.EmployeeID == EmployeeID
                         select a).First();
            ViewBag.tenThuyenVien = query.FirstName + " " + query.LastName;
            //return PartialView("_Employees", hRM_EMPLOYEE_DEGREE.ToList());
            return View("_Employees", hRM_EMPLOYEE_DEGREE.ToList());
            //return View(await hRM_EMPLOYEE_ACCIDENT.ToListAsync());
        }
        // GET: HRM_EMPLOYEE_DEGREE1/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE_DEGREE hRM_EMPLOYEE_DEGREE = await db.HRM_EMPLOYEE_DEGREE.FindAsync(id);
            if (hRM_EMPLOYEE_DEGREE == null)
            {
                return HttpNotFound();
            }
            return View(hRM_EMPLOYEE_DEGREE);
        }

        // GET: HRM_EMPLOYEE_DEGREE1/Create
        public ActionResult Create()
        {
            ViewBag.DegreeID = new SelectList(db.DIC_DEGREE, "DegreeID", "DegreeName");
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName");
            ViewBag.SchoolID = new SelectList(db.DIC_SCHOOL, "SchoolID", "SchoolName");

            return View();
        }
        //create one
        public async Task<ActionResult> CreateOne(int EmployeeID)
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
            return PartialView("_CreateOne", item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOne([Bind(Include = "EmployeeDegreeID,EmployeeID,DegreeID,SchoolID,PositionID,DegreeNo,DegreeDate,ExpirationDate,Qualification,GhiChu,IsBC")] HRM_EMPLOYEE_DEGREE hRM_EMPLOYEE_DEGREE)
        {

            if (ModelState.IsValid)
            {
                //Kiểm tra bằng cấp đó có tồn tại chưa để thêm mới
                var q = from a in db.HRM_EMPLOYEE_DEGREE
                        where a.EmployeeID == hRM_EMPLOYEE_DEGREE.EmployeeID
                        && a.DegreeID == hRM_EMPLOYEE_DEGREE.DegreeID
                        select a;
                if (q.Count() >0)
                {
                    ViewBag.ThongBao = "Bằng cấp đã tồn tại - Sửa thông tin";
                    //Có thông tin

                    ViewBag.EmployeeID = hRM_EMPLOYEE_DEGREE.EmployeeID;
                    
                    ViewBag.idThuyenVien = hRM_EMPLOYEE_DEGREE.EmployeeID;
                    //Hiển thị ngày tháng năm sinh và nơi sinh của thuyền viên để kiểm tra bằng mắt
                    var thuyenvien = (from a in db.HRM_EMPLOYEE
                                      where a.EmployeeID == hRM_EMPLOYEE_DEGREE.EmployeeID
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
                 
                    return PartialView("_CreateOne", hRM_EMPLOYEE_DEGREE);
                    //return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }


                db.HRM_EMPLOYEE_DEGREE.Add(hRM_EMPLOYEE_DEGREE);
                await db.SaveChangesAsync();
                //return RedirectToAction("Index");
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            ViewBag.DegreeID = new SelectList(db.DIC_DEGREE, "DegreeID", "DegreeName", hRM_EMPLOYEE_DEGREE.DegreeID);
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYEE_DEGREE.PositionID);
            ViewBag.SchoolID = new SelectList(db.DIC_SCHOOL, "SchoolID", "SchoolName", hRM_EMPLOYEE_DEGREE.SchoolID);
            // ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_DEGREE.EmployeeID);
            // return View(hRM_EMPLOYEE_DEGREE);
            return PartialView("_CreateOne", hRM_EMPLOYEE_DEGREE);
        }
        // POST: HRM_EMPLOYEE_DEGREE1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmployeeDegreeID,EmployeeID,DegreeID,SchoolID,PositionID,DegreeNo,DegreeDate,ExpirationDate,Qualification")] HRM_EMPLOYEE_DEGREE hRM_EMPLOYEE_DEGREE)
        {
            if (ModelState.IsValid)
            {
                db.HRM_EMPLOYEE_DEGREE.Add(hRM_EMPLOYEE_DEGREE);
                await db.SaveChangesAsync();
                //return RedirectToAction("Index");
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            ViewBag.DegreeID = new SelectList(db.DIC_DEGREE, "DegreeID", "DegreeName", hRM_EMPLOYEE_DEGREE.DegreeID);
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYEE_DEGREE.PositionID);
            ViewBag.SchoolID = new SelectList(db.DIC_SCHOOL, "SchoolID", "SchoolName", hRM_EMPLOYEE_DEGREE.SchoolID);
            // ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_DEGREE.EmployeeID);
            // return View(hRM_EMPLOYEE_DEGREE);
            return PartialView("_CreateOne", hRM_EMPLOYEE_DEGREE);
        }
        //HAM LAY THONG TIN TIEN LUONG THAM KHAO
        [WebMethod]
        public JsonResult TestRules(int DegreeID, string rule)
        {
            ArrayList arl = new ArrayList();


            var ret = db.DIC_DEGREE.Where(a => a.DegreeID == DegreeID);

            foreach (DIC_DEGREE rg in ret)
            {

                arl.Add(new { Unclos = rg.Unclos });


            }

            return new JsonResult { Data = arl };
        }

        // GET: HRM_EMPLOYEE_DEGREE1/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE_DEGREE hRM_EMPLOYEE_DEGREE = await db.HRM_EMPLOYEE_DEGREE.FindAsync(id);
            if (hRM_EMPLOYEE_DEGREE == null)
            {
                return HttpNotFound();
            }
            ViewBag.DegreeID = new SelectList(db.DIC_DEGREE, "DegreeID", "DegreeName", hRM_EMPLOYEE_DEGREE.DegreeID);
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYEE_DEGREE.PositionID);
            ViewBag.SchoolID = new SelectList(db.DIC_SCHOOL, "SchoolID", "SchoolName", hRM_EMPLOYEE_DEGREE.SchoolID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_DEGREE.EmployeeID);
            return PartialView("_Edit", hRM_EMPLOYEE_DEGREE);
        }

        // POST: HRM_EMPLOYEE_DEGREE1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmployeeDegreeID,EmployeeID,DegreeID,SchoolID,PositionID,DegreeNo,DegreeDate,ExpirationDate,Qualification")] HRM_EMPLOYEE_DEGREE hRM_EMPLOYEE_DEGREE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRM_EMPLOYEE_DEGREE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.DegreeID = new SelectList(db.DIC_DEGREE, "DegreeID", "DegreeName", hRM_EMPLOYEE_DEGREE.DegreeID);
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYEE_DEGREE.PositionID);
            ViewBag.SchoolID = new SelectList(db.DIC_SCHOOL, "SchoolID", "SchoolName", hRM_EMPLOYEE_DEGREE.SchoolID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_DEGREE.EmployeeID);
            return View(hRM_EMPLOYEE_DEGREE);
        }

        // GET: HRM_EMPLOYEE_DEGREE1/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE_DEGREE hRM_EMPLOYEE_DEGREE = await db.HRM_EMPLOYEE_DEGREE.FindAsync(id);
            if (hRM_EMPLOYEE_DEGREE == null)
            {
                return HttpNotFound();
            }
            return View(hRM_EMPLOYEE_DEGREE);
        }

        // POST: HRM_EMPLOYEE_DEGREE1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HRM_EMPLOYEE_DEGREE hRM_EMPLOYEE_DEGREE = await db.HRM_EMPLOYEE_DEGREE.FindAsync(id);
            db.HRM_EMPLOYEE_DEGREE.Remove(hRM_EMPLOYEE_DEGREE);
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