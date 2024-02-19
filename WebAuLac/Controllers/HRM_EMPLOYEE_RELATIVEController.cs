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
    public class HRM_EMPLOYEE_RELATIVEController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: HRM_EMPLOYEE_RELATIVE
        public ActionResult Index()
        {
            var hRM_EMPLOYEE_RELATIVE = db.HRM_EMPLOYEE_RELATIVE.Include(h => h.DIC_RELATIVE).Include(h => h.HRM_EMPLOYEE);
            return View(hRM_EMPLOYEE_RELATIVE.ToList());
        }
        //set cache để nó load lại cái mới cập nhật
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult RelativeOfOne(int EmployeeID)
        {
           
            var hRM_EMPLOYEE_RELATIVE = db.HRM_EMPLOYEE_RELATIVE.Include(h => h.DIC_RELATIVE).Where(h=>h.EmployeeID==EmployeeID);
            ViewBag.EmployeeID = EmployeeID;
            return PartialView("_RelativeOfOne", hRM_EMPLOYEE_RELATIVE.ToList());
        }

        // GET: HRM_EMPLOYEE_RELATIVE/Details/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE_RELATIVE hRM_EMPLOYEE_RELATIVE = db.HRM_EMPLOYEE_RELATIVE.Find(id);
            if (hRM_EMPLOYEE_RELATIVE == null)
            {
                return HttpNotFound();
            }
            return PartialView(hRM_EMPLOYEE_RELATIVE);
        }

        // GET: HRM_EMPLOYEE_RELATIVE/Create
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            ViewBag.RelativeID = new SelectList(db.DIC_RELATIVE, "RelativeID", "RelativeName");
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");
            return View();
        }
        // GET: HRM_EMPLOYEE_RELATIVE/Create
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult CreateOne(int EmployeeID)
        {
            HRM_EMPLOYEE_RELATIVE item = new HRM_EMPLOYEE_RELATIVE();
            item.EmployeeID = EmployeeID;
            ViewBag.RelativeID = new SelectList(db.DIC_RELATIVE, "RelativeID", "RelativeName");
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");
            return PartialView(item);
        }

        // POST: HRM_EMPLOYEE_RELATIVE/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Create([Bind(Include = "PersonID,EmployeeID,PersonName,RelativeID,Birthday,Email,Address,Phone,IncomeTaxCode,Job,CompanyAddress")] HRM_EMPLOYEE_RELATIVE hRM_EMPLOYEE_RELATIVE)
        {
            if (ModelState.IsValid)
            {
                db.HRM_EMPLOYEE_RELATIVE.Add(hRM_EMPLOYEE_RELATIVE);
                db.SaveChanges();
                return RedirectToAction("RelativeOfOne", new { EmployeeID = hRM_EMPLOYEE_RELATIVE.EmployeeID });
            }

            ViewBag.RelativeID = new SelectList(db.DIC_RELATIVE, "RelativeID", "RelativeName", hRM_EMPLOYEE_RELATIVE.RelativeID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_RELATIVE.EmployeeID);
            return View(hRM_EMPLOYEE_RELATIVE);
        }

        // GET: HRM_EMPLOYEE_RELATIVE/Edit/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE_RELATIVE hRM_EMPLOYEE_RELATIVE = db.HRM_EMPLOYEE_RELATIVE.Find(id);
            if (hRM_EMPLOYEE_RELATIVE == null)
            {
                return HttpNotFound();
            }
            ViewBag.RelativeID = new SelectList(db.DIC_RELATIVE, "RelativeID", "RelativeName", hRM_EMPLOYEE_RELATIVE.RelativeID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_RELATIVE.EmployeeID);
            return PartialView(hRM_EMPLOYEE_RELATIVE);
        }

        // POST: HRM_EMPLOYEE_RELATIVE/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit([Bind(Include = "PersonID,EmployeeID,PersonName,RelativeID,Birthday,Email,Address,Phone,IncomeTaxCode,Job,CompanyAddress")] HRM_EMPLOYEE_RELATIVE hRM_EMPLOYEE_RELATIVE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRM_EMPLOYEE_RELATIVE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RelativeOfOne", new { EmployeeID = hRM_EMPLOYEE_RELATIVE.EmployeeID });
            }
            ViewBag.RelativeID = new SelectList(db.DIC_RELATIVE, "RelativeID", "RelativeName", hRM_EMPLOYEE_RELATIVE.RelativeID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYEE_RELATIVE.EmployeeID);
            return PartialView(hRM_EMPLOYEE_RELATIVE);
        }

        // GET: HRM_EMPLOYEE_RELATIVE/Delete/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE_RELATIVE hRM_EMPLOYEE_RELATIVE = db.HRM_EMPLOYEE_RELATIVE.Find(id);
            if (hRM_EMPLOYEE_RELATIVE == null)
            {
                return HttpNotFound();
            }
            return PartialView(hRM_EMPLOYEE_RELATIVE);
        }

        // POST: HRM_EMPLOYEE_RELATIVE/Delete/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            HRM_EMPLOYEE_RELATIVE hRM_EMPLOYEE_RELATIVE = db.HRM_EMPLOYEE_RELATIVE.Find(id);
            int EmployeeID =(int) hRM_EMPLOYEE_RELATIVE.EmployeeID;
            db.HRM_EMPLOYEE_RELATIVE.Remove(hRM_EMPLOYEE_RELATIVE);
            db.SaveChanges();
            return RedirectToAction("RelativeOfOne", new { EmployeeID = EmployeeID });
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
