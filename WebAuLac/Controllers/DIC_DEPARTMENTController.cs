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
    public class DIC_DEPARTMENTController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        public string getPath(int? parentID)
        {
            string path = "";
            DIC_DEPARTMENT pb = db.DIC_DEPARTMENT.Find(parentID);
            while (pb != null)
            {
                path = pb.DepartmentName + "\\" + path;
                pb = db.DIC_DEPARTMENT.Find(pb.ParentID);
            }
            return path;
        }
        public JsonResult IsExistsChild(string DepartmentName, int ParentID)
        {
            return Json(!db.DIC_DEPARTMENT.Any(x => x.ParentID == ParentID && x.DepartmentName.ToLower() == DepartmentName.ToLower()), JsonRequestBehavior.AllowGet);
        }
        // GET: DIC_DEPARTMENT
        public async Task<ActionResult> Index()
        {
            return View(await db.DIC_DEPARTMENT.ToListAsync());
        }

        // GET: DIC_DEPARTMENT/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_DEPARTMENT dIC_DEPARTMENT = await db.DIC_DEPARTMENT.FindAsync(id);
            if (dIC_DEPARTMENT == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Details",dIC_DEPARTMENT);
        }       
        
        public ActionResult Create(int? parentID)
        {
            DIC_DEPARTMENT pb = new DIC_DEPARTMENT();
            pb.ParentID = parentID;
            pb.IsLast = true;
            ViewBag.path = getPath(parentID);
            ViewBag.TypeOfVessel= new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau");
            return PartialView("_Create",pb);
        }

        // POST: DIC_DEPARTMENT/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DepartmentID,DepartmentName,Phone,Quantity,FactQuantity,Description,IsLast,ParentID, TypeOfVessel, Gross, Power, IMO, Length, Breadth, DeadWeight, Net, YearOfBuilding, PlaceOfBuiding, PortOfRegistry, ClassificationAgency, Draft")] DIC_DEPARTMENT dIC_DEPARTMENT)
        {
            if (ModelState.IsValid)
            {
                db.DIC_DEPARTMENT.Add(dIC_DEPARTMENT);
                await db.SaveChangesAsync();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);                
            }
            ViewBag.TypeOfVessel = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau");
            return View(dIC_DEPARTMENT);
        }

        // GET: DIC_DEPARTMENT/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_DEPARTMENT dIC_DEPARTMENT = await db.DIC_DEPARTMENT.FindAsync(id);
            if (dIC_DEPARTMENT == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeOfVessel = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau", dIC_DEPARTMENT.TypeOfVessel);
            return PartialView("_Edit",dIC_DEPARTMENT);
        }

        // POST: DIC_DEPARTMENT/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DepartmentID,DepartmentName,Phone,Quantity,FactQuantity,Description,IsLast,ParentID, TypeOfVessel, Gross, Power, IMO, Length, Breadth, DeadWeight, Net, YearOfBuilding, PlaceOfBuiding, PortOfRegistry, ClassificationAgency, Draft")] DIC_DEPARTMENT dIC_DEPARTMENT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dIC_DEPARTMENT).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.TypeOfVessel = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau", dIC_DEPARTMENT.TypeOfVessel);
            return PartialView("_Edit",dIC_DEPARTMENT);
        }

        // GET: DIC_DEPARTMENT/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_DEPARTMENT dIC_DEPARTMENT = await db.DIC_DEPARTMENT.FindAsync(id);
            if (dIC_DEPARTMENT == null)
            {
                return HttpNotFound();
            }
            //kiểm tra có phòng ban con hay không
            if (db.DIC_DEPARTMENT.Any(x => x.ParentID == id))
            {
                return PartialView("_DeleteParent", dIC_DEPARTMENT);
            }
            return PartialView("_Delete", dIC_DEPARTMENT);
        }

        // POST: DIC_DEPARTMENT/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DIC_DEPARTMENT dIC_DEPARTMENT = await db.DIC_DEPARTMENT.FindAsync(id);
            db.DIC_DEPARTMENT.Remove(dIC_DEPARTMENT);
            await db.SaveChangesAsync();
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
