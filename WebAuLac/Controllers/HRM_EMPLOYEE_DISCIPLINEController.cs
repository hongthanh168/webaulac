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
    public class HRM_EMPLOYEE_DISCIPLINEController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: HRM_EMPLOYEE_DISCIPLINE
        public ActionResult Index()
        {
            var hRM_EMPLOYEE_DISCIPLINE = db.HRM_EMPLOYEE_DISCIPLINE.Include(h => h.DIC_TYPE_OF_DISCIPLINE).Include(h => h.HRM_EMPLOYEE);
            return View(hRM_EMPLOYEE_DISCIPLINE.ToList());
        }
        //set cache để nó load lại cái mới cập nhật
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult DisciplineOfOne(int EmployeeID)
        {
            ViewBag.EmployeeID = EmployeeID;        
            var hRM_EMPLOYEE_DISCIPLINE = db.HRM_EMPLOYEE_DISCIPLINE.Include(h => h.DIC_TYPE_OF_DISCIPLINE).Where(h=>h.EmployeeID==EmployeeID);
            return PartialView("_DisciplineOfOne", hRM_EMPLOYEE_DISCIPLINE.ToList());
        }
        // GET: HRM_EMPLOYEE_DISCIPLINE/Details/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE_DISCIPLINE hRM_EMPLOYEE_DISCIPLINE = db.HRM_EMPLOYEE_DISCIPLINE.Find(id);
            if (hRM_EMPLOYEE_DISCIPLINE == null)
            {
                return HttpNotFound();
            }
            return View(hRM_EMPLOYEE_DISCIPLINE);
        }

        // GET: HRM_EMPLOYEE_DISCIPLINE/Create
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            ViewBag.TypeOfDisciplineID = new SelectList(db.DIC_TYPE_OF_DISCIPLINE, "TypeOfDisciplineID", "TypeOfDisciplineName");
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");
            return View();
        }

        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult CreateOne(int EmployeeID)
        {
            HRM_EMPLOYEE_DISCIPLINE item = new HRM_EMPLOYEE_DISCIPLINE();
            item.EmployeeID = EmployeeID;
            ViewBag.TypeOfDisciplineID = new SelectList(db.DIC_TYPE_OF_DISCIPLINE, "TypeOfDisciplineID", "TypeOfDisciplineName");
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");
            return PartialView(item);
        }

        // POST: HRM_EMPLOYEE_DISCIPLINE/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Create([Bind(Include = "EmployeeDisciplineID,EmployeeID,TypeOfDisciplineID,DisciplineNo,DisciplineDate,Reason")] HRM_EMPLOYEE_DISCIPLINE hRM_EMPLOYEE_DISCIPLINE)
        {
            if (ModelState.IsValid)
            {
                db.HRM_EMPLOYEE_DISCIPLINE.Add(hRM_EMPLOYEE_DISCIPLINE);
                db.SaveChanges();
                return RedirectToAction("DisciplineOfOne", new { EmployeeID = hRM_EMPLOYEE_DISCIPLINE.EmployeeID });
            }

            ViewBag.TypeOfDisciplineID = new SelectList(db.DIC_TYPE_OF_DISCIPLINE, "TypeOfDisciplineID", "TypeOfDisciplineName", hRM_EMPLOYEE_DISCIPLINE.TypeOfDisciplineID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_DISCIPLINE.EmployeeID);
            return View(hRM_EMPLOYEE_DISCIPLINE);
        }

        // GET: HRM_EMPLOYEE_DISCIPLINE/Edit/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE_DISCIPLINE hRM_EMPLOYEE_DISCIPLINE = db.HRM_EMPLOYEE_DISCIPLINE.Find(id);
            if (hRM_EMPLOYEE_DISCIPLINE == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeOfDisciplineID = new SelectList(db.DIC_TYPE_OF_DISCIPLINE, "TypeOfDisciplineID", "TypeOfDisciplineName", hRM_EMPLOYEE_DISCIPLINE.TypeOfDisciplineID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_DISCIPLINE.EmployeeID);
            return PartialView(hRM_EMPLOYEE_DISCIPLINE);
        }

        // POST: HRM_EMPLOYEE_DISCIPLINE/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit([Bind(Include = "EmployeeDisciplineID,EmployeeID,TypeOfDisciplineID,DisciplineNo,DisciplineDate,Reason")] HRM_EMPLOYEE_DISCIPLINE hRM_EMPLOYEE_DISCIPLINE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRM_EMPLOYEE_DISCIPLINE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DisciplineOfOne", new { EmployeeID = hRM_EMPLOYEE_DISCIPLINE.EmployeeID });
            }
            ViewBag.TypeOfDisciplineID = new SelectList(db.DIC_TYPE_OF_DISCIPLINE, "TypeOfDisciplineID", "TypeOfDisciplineName", hRM_EMPLOYEE_DISCIPLINE.TypeOfDisciplineID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_DISCIPLINE.EmployeeID);
            return PartialView(hRM_EMPLOYEE_DISCIPLINE);
        }

        // GET: HRM_EMPLOYEE_DISCIPLINE/Delete/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE_DISCIPLINE hRM_EMPLOYEE_DISCIPLINE = db.HRM_EMPLOYEE_DISCIPLINE.Find(id);
            if (hRM_EMPLOYEE_DISCIPLINE == null)
            {
                return HttpNotFound();
            }
            return PartialView(hRM_EMPLOYEE_DISCIPLINE);
        }

        // POST: HRM_EMPLOYEE_DISCIPLINE/Delete/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HRM_EMPLOYEE_DISCIPLINE hRM_EMPLOYEE_DISCIPLINE = db.HRM_EMPLOYEE_DISCIPLINE.Find(id);
            int EmployeeID =(int) hRM_EMPLOYEE_DISCIPLINE.EmployeeID;
            db.HRM_EMPLOYEE_DISCIPLINE.Remove(hRM_EMPLOYEE_DISCIPLINE);
            db.SaveChanges();
            return RedirectToAction("DisciplineOfOne", new { EmployeeID = EmployeeID });
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
