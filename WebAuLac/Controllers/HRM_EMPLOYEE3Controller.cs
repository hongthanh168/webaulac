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

namespace WebAuLac.Controllers
{
    [Authorize]
    public class HRM_EMPLOYEE3Controller : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: HRM_EMPLOYEE1
        //public async Task<ActionResult> Index()
        //{
        //    //Trả lại dữ liệu có kèm cảnh báo bằng cấp
        //    var hRM_EMPLOYEE = db.fn_IsLimitDegree() as HRM_EMPLOYEE;
        //    //  var hRM_EMPLOYEE = db.HRM_EMPLOYEE.Include(h => h.DIC_DEPARTMENT_POSITION).Include(h => h.DIC_EDUCATION).Include(h => h.DIC_ETHNIC).Include(h => h.DIC_NATIONALITY).Include(h => h.DIC_RELIGION).Include(h => h.DIC_STATUS);
        //    return View(await hRM_EMPLOYEE.ToListAsync());
        //}

        public ActionResult Index()
        {
            //Trả lại dữ liệu có kèm cảnh báo bằng cấp
            //var hRM_EMPLOYEE = db.fn_IsLimitDegree().OrderByDescending(a =>a.ISLIMITDEGREE);
            var hRM_EMPLOYEE = db.fn_TestDegree().OrderBy(h=>h.EmployeeID);
            //  var hRM_EMPLOYEE = db.HRM_EMPLOYEE.Include(h => h.DIC_DEPARTMENT_POSITION).Include(h => h.DIC_EDUCATION).Include(h => h.DIC_ETHNIC).Include(h => h.DIC_NATIONALITY).Include(h => h.DIC_RELIGION).Include(h => h.DIC_STATUS);
            return View(hRM_EMPLOYEE.ToArray());

        }
        // GET: HRM_EMPLOYEE1/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE hRM_EMPLOYEE = await db.HRM_EMPLOYEE.FindAsync(id);
            if (hRM_EMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(hRM_EMPLOYEE);
        }

        // GET: HRM_EMPLOYEE1/Create
        public ActionResult Create()
        {
            ViewBag.Department_PositionID = new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Decription");
            ViewBag.EducationID = new SelectList(db.DIC_EDUCATION, "EducationID", "EducationName");
            ViewBag.EthnicID = new SelectList(db.DIC_ETHNIC, "EthnicID", "EthnicName");
            ViewBag.NationalityID = new SelectList(db.DIC_NATIONALITY, "NationalityID", "NationalityName");
            ViewBag.ReligionID = new SelectList(db.DIC_RELIGION, "ReligionID", "ReligionName");
            ViewBag.StatusID = new SelectList(db.DIC_STATUS, "StatusID", "StatusName");
            return View();
        }

        // POST: HRM_EMPLOYEE1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmployeeID,EmployeeCode,CardNo,FirstName,LastName,Alias,Sex,Marriage,BirthDay,BirthPlace,MainAddress,ContactAddress,CellPhone,HomePhone,Email,Skype,Yahoo,Facebook,IDCard,IDCardDate,IDCardPlace,TaxNo,BankCode,BankName,InsuranceCode,InsuranceDate,Photo,EducationID,DegreeID,EthnicID,ReligionID,NationalityID,Department_PositionID,StatusID,IsDaiDuong")] HRM_EMPLOYEE hRM_EMPLOYEE)
        {
            if (ModelState.IsValid)
            {
                db.HRM_EMPLOYEE.Add(hRM_EMPLOYEE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Department_PositionID = new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Decription", hRM_EMPLOYEE.Department_PositionID);
            ViewBag.EducationID = new SelectList(db.DIC_EDUCATION, "EducationID", "EducationName", hRM_EMPLOYEE.EducationID);
            ViewBag.EthnicID = new SelectList(db.DIC_ETHNIC, "EthnicID", "EthnicName", hRM_EMPLOYEE.EthnicID);
            ViewBag.NationalityID = new SelectList(db.DIC_NATIONALITY, "NationalityID", "NationalityName", hRM_EMPLOYEE.NationalityID);
            ViewBag.ReligionID = new SelectList(db.DIC_RELIGION, "ReligionID", "ReligionName", hRM_EMPLOYEE.ReligionID);
            ViewBag.StatusID = new SelectList(db.DIC_STATUS, "StatusID", "StatusName", hRM_EMPLOYEE.StatusID);
            return View(hRM_EMPLOYEE);
        }

        // GET: HRM_EMPLOYEE1/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE hRM_EMPLOYEE = await db.HRM_EMPLOYEE.FindAsync(id);
            if (hRM_EMPLOYEE == null)
            {
                return HttpNotFound();
            }
            ViewBag.Department_PositionID = new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Decription", hRM_EMPLOYEE.Department_PositionID);
            ViewBag.EducationID = new SelectList(db.DIC_EDUCATION, "EducationID", "EducationName", hRM_EMPLOYEE.EducationID);
            ViewBag.EthnicID = new SelectList(db.DIC_ETHNIC, "EthnicID", "EthnicName", hRM_EMPLOYEE.EthnicID);
            ViewBag.NationalityID = new SelectList(db.DIC_NATIONALITY, "NationalityID", "NationalityName", hRM_EMPLOYEE.NationalityID);
            ViewBag.ReligionID = new SelectList(db.DIC_RELIGION, "ReligionID", "ReligionName", hRM_EMPLOYEE.ReligionID);
            ViewBag.StatusID = new SelectList(db.DIC_STATUS, "StatusID", "StatusName", hRM_EMPLOYEE.StatusID);
            return View(hRM_EMPLOYEE);
        }

        // POST: HRM_EMPLOYEE1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmployeeID,EmployeeCode,CardNo,FirstName,LastName,Alias,Sex,Marriage,BirthDay,BirthPlace,MainAddress,ContactAddress,CellPhone,HomePhone,Email,Skype,Yahoo,Facebook,IDCard,IDCardDate,IDCardPlace,TaxNo,BankCode,BankName,InsuranceCode,InsuranceDate,Photo,EducationID,DegreeID,EthnicID,ReligionID,NationalityID,Department_PositionID,StatusID,IsDaiDuong")] HRM_EMPLOYEE hRM_EMPLOYEE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRM_EMPLOYEE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Department_PositionID = new SelectList(db.DIC_DEPARTMENT_POSITION, "Department_PositionID", "Decription", hRM_EMPLOYEE.Department_PositionID);
            ViewBag.EducationID = new SelectList(db.DIC_EDUCATION, "EducationID", "EducationName", hRM_EMPLOYEE.EducationID);
            ViewBag.EthnicID = new SelectList(db.DIC_ETHNIC, "EthnicID", "EthnicName", hRM_EMPLOYEE.EthnicID);
            ViewBag.NationalityID = new SelectList(db.DIC_NATIONALITY, "NationalityID", "NationalityName", hRM_EMPLOYEE.NationalityID);
            ViewBag.ReligionID = new SelectList(db.DIC_RELIGION, "ReligionID", "ReligionName", hRM_EMPLOYEE.ReligionID);
            ViewBag.StatusID = new SelectList(db.DIC_STATUS, "StatusID", "StatusName", hRM_EMPLOYEE.StatusID);
            return View(hRM_EMPLOYEE);
        }

        // GET: HRM_EMPLOYEE1/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYEE hRM_EMPLOYEE = await db.HRM_EMPLOYEE.FindAsync(id);
            if (hRM_EMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(hRM_EMPLOYEE);
        }

        // POST: HRM_EMPLOYEE1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HRM_EMPLOYEE hRM_EMPLOYEE = await db.HRM_EMPLOYEE.FindAsync(id);
            db.HRM_EMPLOYEE.Remove(hRM_EMPLOYEE);
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
