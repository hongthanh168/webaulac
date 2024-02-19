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
    [Authorize(Roles = "Admin")]
    public class DIC_TYPE_OF_DISCIPLINEController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        public JsonResult IsDIC_STATUSExists(string StatusName)
        {
            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
            return Json(!db.DIC_TYPE_OF_DISCIPLINE.Any(x => x.TypeOfDisciplineName.ToLower() == StatusName.ToLower()), JsonRequestBehavior.AllowGet);
        }

        // GET: DIC_TYPE_OF_DISCIPLINE
        public async Task<ActionResult> Index()
        {
            return View(await db.DIC_TYPE_OF_DISCIPLINE.ToListAsync());
        }

        // GET: DIC_TYPE_OF_DISCIPLINE/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_TYPE_OF_DISCIPLINE DIC_TYPE_OF_DISCIPLINE = await db.DIC_TYPE_OF_DISCIPLINE.FindAsync(id);
            if (DIC_TYPE_OF_DISCIPLINE == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Details", DIC_TYPE_OF_DISCIPLINE);
        }

        // GET: DIC_TYPE_OF_DISCIPLINE/Create
        public ActionResult Create()
        {
            return PartialView("_Create");
        }

        // POST: DIC_TYPE_OF_DISCIPLINE/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TypeOfDisciplineID,TypeOfDisciplineName")] DIC_TYPE_OF_DISCIPLINE dIC_TYPE_OF_DISCIPLINE)
        {
            if (ModelState.IsValid)
            {
                db.DIC_TYPE_OF_DISCIPLINE.Add(dIC_TYPE_OF_DISCIPLINE);
                await db.SaveChangesAsync();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView("_Create", dIC_TYPE_OF_DISCIPLINE);
        }

        // GET: DIC_TYPE_OF_DISCIPLINE/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_TYPE_OF_DISCIPLINE dIC_TYPE_OF_DISCIPLINE = await db.DIC_TYPE_OF_DISCIPLINE.FindAsync(id);
            if (dIC_TYPE_OF_DISCIPLINE == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Edit", dIC_TYPE_OF_DISCIPLINE);
        }

        // POST: DIC_TYPE_OF_DISCIPLINE/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TypeOfDisciplineID,TypeOfDisciplineName")] DIC_TYPE_OF_DISCIPLINE dIC_TYPE_OF_DISCIPLINE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dIC_TYPE_OF_DISCIPLINE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView("_Edit", dIC_TYPE_OF_DISCIPLINE);
        }

        // GET: DIC_TYPE_OF_DISCIPLINE/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_TYPE_OF_DISCIPLINE dIC_TYPE_OF_DISCIPLINE = await db.DIC_TYPE_OF_DISCIPLINE.FindAsync(id);
            if (dIC_TYPE_OF_DISCIPLINE == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete", dIC_TYPE_OF_DISCIPLINE);
        }

        // POST: DIC_TYPE_OF_DISCIPLINE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DIC_TYPE_OF_DISCIPLINE dIC_TYPE_OF_DISCIPLINE = await db.DIC_TYPE_OF_DISCIPLINE.FindAsync(id);
            db.DIC_TYPE_OF_DISCIPLINE.Remove(dIC_TYPE_OF_DISCIPLINE);
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
