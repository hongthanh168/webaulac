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
    public class HRM_EMPLOYEE_RETRIBUTIONController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: HRM_EMPLOYEE_RETRIBUTION
        public ActionResult Index()
        {
            var hRM_EMPLOYEE_RETRIBUTION = db.HRM_EMPLOYEE_RETRIBUTION.Include(h => h.DIC_TYPE_OF_RETRIBUTION).Include(h => h.HRM_EMPLOYEE);
            return View(hRM_EMPLOYEE_RETRIBUTION.ToList());
        }
        //set cache để nó load lại cái mới cập nhật
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult RetributionOfOne(int EmployeeID)
        {
            ViewBag.EmployeeID = EmployeeID;
            var hRM_EMPLOYEE_RETRIBUTION = db.HRM_EMPLOYEE_RETRIBUTION.Include(h => h.DIC_TYPE_OF_RETRIBUTION).Where(h=>h.EmployeeID==EmployeeID);
            return PartialView("_RetributionOfOne",hRM_EMPLOYEE_RETRIBUTION.ToList());
        }

        // GET: HRM_EMPLOYEE_RETRIBUTION/Details/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE_RETRIBUTION hRM_EMPLOYEE_RETRIBUTION = db.HRM_EMPLOYEE_RETRIBUTION.Find(id);
            if (hRM_EMPLOYEE_RETRIBUTION == null)
            {
                return HttpNotFound();
            }
            return View(hRM_EMPLOYEE_RETRIBUTION);
        }

        // GET: HRM_EMPLOYEE_RETRIBUTION/Create
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            ViewBag.TypeOfRetributionID = new SelectList(db.DIC_TYPE_OF_RETRIBUTION, "TypeOfRetributionID", "TypeOfRetributionName");
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");
            return View();
        }
        public ActionResult CreateOne(int EmployeeID)
        {
            HRM_EMPLOYEE_RETRIBUTION item = new HRM_EMPLOYEE_RETRIBUTION();
            item.EmployeeID = EmployeeID;
            ViewBag.TypeOfRetributionID = new SelectList(db.DIC_TYPE_OF_RETRIBUTION, "TypeOfRetributionID", "TypeOfRetributionName");
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");
            return PartialView(item);
        }

        // POST: HRM_EMPLOYEE_RETRIBUTION/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Create([Bind(Include = "EmployeeRetributionID,EmployeeID,TypeOfRetributionID,RetributionNo,RetributionDate,Reason")] HRM_EMPLOYEE_RETRIBUTION hRM_EMPLOYEE_RETRIBUTION)
        {
            if (ModelState.IsValid)
            {
                db.HRM_EMPLOYEE_RETRIBUTION.Add(hRM_EMPLOYEE_RETRIBUTION);
                db.SaveChanges();
                return RedirectToAction("RetributionOfOne", new { EmployeeID = hRM_EMPLOYEE_RETRIBUTION.EmployeeID });
            }

            ViewBag.TypeOfRetributionID = new SelectList(db.DIC_TYPE_OF_RETRIBUTION, "TypeOfRetributionID", "TypeOfRetributionName", hRM_EMPLOYEE_RETRIBUTION.TypeOfRetributionID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_RETRIBUTION.EmployeeID);
            return PartialView(hRM_EMPLOYEE_RETRIBUTION);
        }

        // GET: HRM_EMPLOYEE_RETRIBUTION/Edit/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE_RETRIBUTION hRM_EMPLOYEE_RETRIBUTION = db.HRM_EMPLOYEE_RETRIBUTION.Find(id);
            if (hRM_EMPLOYEE_RETRIBUTION == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeOfRetributionID = new SelectList(db.DIC_TYPE_OF_RETRIBUTION, "TypeOfRetributionID", "TypeOfRetributionName", hRM_EMPLOYEE_RETRIBUTION.TypeOfRetributionID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_RETRIBUTION.EmployeeID);
            return PartialView(hRM_EMPLOYEE_RETRIBUTION);
        }

        // POST: HRM_EMPLOYEE_RETRIBUTION/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit([Bind(Include = "EmployeeRetributionID,EmployeeID,TypeOfRetributionID,RetributionNo,RetributionDate,Reason")] HRM_EMPLOYEE_RETRIBUTION hRM_EMPLOYEE_RETRIBUTION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRM_EMPLOYEE_RETRIBUTION).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RetributionOfOne", new { EmployeeID = hRM_EMPLOYEE_RETRIBUTION.EmployeeID });
            }
            ViewBag.TypeOfRetributionID = new SelectList(db.DIC_TYPE_OF_RETRIBUTION, "TypeOfRetributionID", "TypeOfRetributionName", hRM_EMPLOYEE_RETRIBUTION.TypeOfRetributionID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_RETRIBUTION.EmployeeID);
            return PartialView(hRM_EMPLOYEE_RETRIBUTION);
        }

        // GET: HRM_EMPLOYEE_RETRIBUTION/Delete/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE_RETRIBUTION hRM_EMPLOYEE_RETRIBUTION = db.HRM_EMPLOYEE_RETRIBUTION.Find(id);
            if (hRM_EMPLOYEE_RETRIBUTION == null)
            {
                return HttpNotFound();
            }
            return PartialView(hRM_EMPLOYEE_RETRIBUTION);
        }

        // POST: HRM_EMPLOYEE_RETRIBUTION/Delete/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HRM_EMPLOYEE_RETRIBUTION hRM_EMPLOYEE_RETRIBUTION = db.HRM_EMPLOYEE_RETRIBUTION.Find(id);
            int EmployeeID =(int) hRM_EMPLOYEE_RETRIBUTION.EmployeeID;
            db.HRM_EMPLOYEE_RETRIBUTION.Remove(hRM_EMPLOYEE_RETRIBUTION);
            db.SaveChanges();
            return RedirectToAction("RetributionOfOne", new { EmployeeID = EmployeeID });
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
