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
    public class HRM_CONTRACTHISTORYController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: HRM_CONTRACTHISTORY
        public ActionResult Index()
        {
            var hRM_CONTRACTHISTORY = db.HRM_CONTRACTHISTORY.Include(h => h.DIC_CONTRACTTYPE).Include(h => h.HRM_EMPLOYEE);
           return View(hRM_CONTRACTHISTORY.ToList());
        }
        //set cache để nó load lại cái mới cập nhật
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ContractOfOne(int EmployeeID)
        {
          
          
            var hRM_CONTRACTHISTORY = db.HRM_CONTRACTHISTORY.Include(h => h.DIC_CONTRACTTYPE).Where(h=>h.EmployeeID == EmployeeID);
            ViewBag.EmployeeID = EmployeeID;
            return PartialView("_ContractOfOne", hRM_CONTRACTHISTORY.ToList());
        }

        // GET: HRM_CONTRACTHISTORY/Details/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_CONTRACTHISTORY hRM_CONTRACTHISTORY = db.HRM_CONTRACTHISTORY.Find(id);
            if (hRM_CONTRACTHISTORY == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Details", hRM_CONTRACTHISTORY);
        }

        // GET: HRM_CONTRACTHISTORY/Create
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            ViewBag.ContractTypeID = new SelectList(db.DIC_CONTRACTTYPE, "ContractTypeID", "ContractTypeName");
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");
            return PartialView("_Create");
        }
        //Tạo ra hợp đồng mới dùng ID có sẵn
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult CreateOne(int EmployeeID)
        {
            HRM_CONTRACTHISTORY item = new HRM_CONTRACTHISTORY();
            item.ContractHistoryID = EmployeeID;
            ViewBag.ContractTypeID = new SelectList(db.DIC_CONTRACTTYPE, "ContractTypeID", "ContractTypeName");
            
            return PartialView(item);
        }
        // POST: HRM_CONTRACTHISTORY/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult CreateWithEmployee(int EmployeeID, [Bind(Include = "ContractHistoryID,ContractTypeID,ContractNo,ContractDate,EffctiveDate,ExpirationDate,EmployeeID")] HRM_CONTRACTHISTORY hRM_CONTRACTHISTORY)
        {
            if (ModelState.IsValid)
            {
                db.HRM_CONTRACTHISTORY.Add(hRM_CONTRACTHISTORY);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContractTypeID = new SelectList(db.DIC_CONTRACTTYPE, "ContractTypeID", "ContractTypeName", hRM_CONTRACTHISTORY.ContractTypeID);
            ViewBag.EmployeeID = EmployeeID;// new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_CONTRACTHISTORY.EmployeeID);
            return PartialView("_CreateWithEmployee", hRM_CONTRACTHISTORY);
        }

       // POST: HRM_CONTRACTHISTORY/Create
       // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
       // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
       [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Create([Bind(Include = "ContractHistoryID,ContractTypeID,ContractNo,ContractDate,EffctiveDate,ExpirationDate,EmployeeID")] HRM_CONTRACTHISTORY hRM_CONTRACTHISTORY)
        {
            if (ModelState.IsValid)
            {
                db.HRM_CONTRACTHISTORY.Add(hRM_CONTRACTHISTORY);
                db.SaveChanges();
                return RedirectToAction("ContractOfOne", new { EmployeeID = hRM_CONTRACTHISTORY.EmployeeID });
            }

            ViewBag.ContractTypeID = new SelectList(db.DIC_CONTRACTTYPE, "ContractTypeID", "ContractTypeName", hRM_CONTRACTHISTORY.ContractTypeID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_CONTRACTHISTORY.EmployeeID);
            return View(hRM_CONTRACTHISTORY);
        }

        // GET: HRM_CONTRACTHISTORY/Edit/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_CONTRACTHISTORY hRM_CONTRACTHISTORY = db.HRM_CONTRACTHISTORY.Find(id);
            if (hRM_CONTRACTHISTORY == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContractTypeID = new SelectList(db.DIC_CONTRACTTYPE, "ContractTypeID", "ContractTypeName", hRM_CONTRACTHISTORY.ContractTypeID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_CONTRACTHISTORY.EmployeeID);
            return PartialView(hRM_CONTRACTHISTORY);
        }

        // POST: HRM_CONTRACTHISTORY/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit([Bind(Include = "ContractHistoryID,ContractTypeID,ContractNo,ContractDate,EffctiveDate,ExpirationDate,EmployeeID")] HRM_CONTRACTHISTORY hRM_CONTRACTHISTORY)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRM_CONTRACTHISTORY).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ContractOfOne", new { EmployeeID = hRM_CONTRACTHISTORY.EmployeeID });
            }
            ViewBag.ContractTypeID = new SelectList(db.DIC_CONTRACTTYPE, "ContractTypeID", "ContractTypeName", hRM_CONTRACTHISTORY.ContractTypeID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_CONTRACTHISTORY.EmployeeID);
            return PartialView(hRM_CONTRACTHISTORY);
        }

        // GET: HRM_CONTRACTHISTORY/Delete/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_CONTRACTHISTORY hRM_CONTRACTHISTORY = db.HRM_CONTRACTHISTORY.Find(id);
            if (hRM_CONTRACTHISTORY == null)
            {
                return HttpNotFound();
            }
            return PartialView(hRM_CONTRACTHISTORY);
        }

        // POST: HRM_CONTRACTHISTORY/Delete/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HRM_CONTRACTHISTORY hRM_CONTRACTHISTORY = db.HRM_CONTRACTHISTORY.Find(id);
            db.HRM_CONTRACTHISTORY.Remove(hRM_CONTRACTHISTORY);
            db.SaveChanges();
            return RedirectToAction("ContractOfOne", new { EmployeeID = hRM_CONTRACTHISTORY.EmployeeID });
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
