using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAuLac.Models;

namespace WebAuLac.Controllers
{
    [Authorize]
    public class HRM_EMPLOYEE_DEGREE2Controller : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: HRM_EMPLOYEE_DEGREE2
        public ActionResult Index()
        {
            var hRM_EMPLOYEE_DEGREE = db.HRM_EMPLOYEE_DEGREE.Include(h => h.DIC_DEGREE).Include(h => h.DIC_SCHOOL).Include(h => h.HRM_EMPLOYEE);
            return View(hRM_EMPLOYEE_DEGREE.ToList());
        }

        // GET: HRM_EMPLOYEE_DEGREE2/Details/5
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
            return View(hRM_EMPLOYEE_DEGREE);
        }

        // GET: HRM_EMPLOYEE_DEGREE2/Create
        public ActionResult Create()
        {
            ViewBag.DegreeID = new SelectList(db.DIC_DEGREE, "DegreeID", "DegreeName");
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName");
            ViewBag.SchoolID = new SelectList(db.DIC_SCHOOL, "SchoolID", "SchoolName");
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");
            return View();
        }

        // POST: HRM_EMPLOYEE_DEGREE2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeDegreeID,EmployeeID,DegreeID,SchoolID,PositionID,DegreeNo,DegreeDate,ExpirationDate,Qualification,GhiChu,IsBC")] HRM_EMPLOYEE_DEGREE hRM_EMPLOYEE_DEGREE)
        {
            if (ModelState.IsValid)
            {
                db.HRM_EMPLOYEE_DEGREE.Add(hRM_EMPLOYEE_DEGREE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DegreeID = new SelectList(db.DIC_DEGREE, "DegreeID", "DegreeName", hRM_EMPLOYEE_DEGREE.DegreeID);
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYEE_DEGREE.PositionID);
            ViewBag.SchoolID = new SelectList(db.DIC_SCHOOL, "SchoolID", "SchoolName", hRM_EMPLOYEE_DEGREE.SchoolID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_DEGREE.EmployeeID);
            return View(hRM_EMPLOYEE_DEGREE);
        }

        // GET: HRM_EMPLOYEE_DEGREE2/Edit/5
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
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYEE_DEGREE.PositionID);
            ViewBag.SchoolID = new SelectList(db.DIC_SCHOOL, "SchoolID", "SchoolName", hRM_EMPLOYEE_DEGREE.SchoolID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_DEGREE.EmployeeID);
            return View(hRM_EMPLOYEE_DEGREE);
        }

        // POST: HRM_EMPLOYEE_DEGREE2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeDegreeID,EmployeeID,DegreeID,SchoolID,PositionID,DegreeNo,DegreeDate,ExpirationDate,Qualification,GhiChu,IsBC")] HRM_EMPLOYEE_DEGREE hRM_EMPLOYEE_DEGREE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRM_EMPLOYEE_DEGREE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DegreeID = new SelectList(db.DIC_DEGREE, "DegreeID", "DegreeName", hRM_EMPLOYEE_DEGREE.DegreeID);
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYEE_DEGREE.PositionID);
            ViewBag.SchoolID = new SelectList(db.DIC_SCHOOL, "SchoolID", "SchoolName", hRM_EMPLOYEE_DEGREE.SchoolID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_DEGREE.EmployeeID);
            return View(hRM_EMPLOYEE_DEGREE);
        }

        // GET: HRM_EMPLOYEE_DEGREE2/Delete/5
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
            return View(hRM_EMPLOYEE_DEGREE);
        }

        // POST: HRM_EMPLOYEE_DEGREE2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HRM_EMPLOYEE_DEGREE hRM_EMPLOYEE_DEGREE = db.HRM_EMPLOYEE_DEGREE.Find(id);
            db.HRM_EMPLOYEE_DEGREE.Remove(hRM_EMPLOYEE_DEGREE);
            db.SaveChanges();
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
