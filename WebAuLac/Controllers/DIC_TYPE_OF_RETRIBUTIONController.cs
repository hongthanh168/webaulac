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
    public class DIC_TYPE_OF_RETRIBUTIONController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        public JsonResult IsDIC_STATUSExists(string StatusName)
        {
            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
            return Json(!db.DIC_TYPE_OF_RETRIBUTION.Any(x => x.TypeOfRetributionName.ToLower() == StatusName.ToLower()), JsonRequestBehavior.AllowGet);
        }

        // GET: DIC_TYPE_OF_RETRIBUTION
        public async Task<ActionResult> Index()
        {
            return View(await db.DIC_TYPE_OF_RETRIBUTION.ToListAsync());
        }

        // GET: DIC_TYPE_OF_RETRIBUTION/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_TYPE_OF_RETRIBUTION dIC_TYPE_OF_RETRIBUTION = await db.DIC_TYPE_OF_RETRIBUTION.FindAsync(id);
            if (dIC_TYPE_OF_RETRIBUTION == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Details", dIC_TYPE_OF_RETRIBUTION);
        }

        // GET: DIC_TYPE_OF_RETRIBUTION/Create
        public ActionResult Create()
        {
            return PartialView("_Create");
        }

        // POST: DIC_TYPE_OF_RETRIBUTION/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TypeOfRetributionID,TypeOfRetributionName")] DIC_TYPE_OF_RETRIBUTION dIC_TYPE_OF_RETRIBUTION)
        {
            if (ModelState.IsValid)
            {
                db.DIC_TYPE_OF_RETRIBUTION.Add(dIC_TYPE_OF_RETRIBUTION);
                await db.SaveChangesAsync();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView("_Create", dIC_TYPE_OF_RETRIBUTION);
        }

        // GET: DIC_TYPE_OF_RETRIBUTION/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_TYPE_OF_RETRIBUTION dIC_TYPE_OF_RETRIBUTION = await db.DIC_TYPE_OF_RETRIBUTION.FindAsync(id);
            if (dIC_TYPE_OF_RETRIBUTION == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Edit", dIC_TYPE_OF_RETRIBUTION);
        }

        // POST: DIC_TYPE_OF_RETRIBUTION/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TypeOfRetributionID,TypeOfRetributionName")] DIC_TYPE_OF_RETRIBUTION dIC_TYPE_OF_RETRIBUTION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dIC_TYPE_OF_RETRIBUTION).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView("_Edit", dIC_TYPE_OF_RETRIBUTION);
        }

        // GET: DIC_TYPE_OF_RETRIBUTION/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_TYPE_OF_RETRIBUTION dIC_TYPE_OF_RETRIBUTION = await db.DIC_TYPE_OF_RETRIBUTION.FindAsync(id);
            if (dIC_TYPE_OF_RETRIBUTION == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete", dIC_TYPE_OF_RETRIBUTION);
        }

        // POST: DIC_TYPE_OF_RETRIBUTION/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DIC_TYPE_OF_RETRIBUTION dIC_TYPE_OF_RETRIBUTION = await db.DIC_TYPE_OF_RETRIBUTION.FindAsync(id);
            db.DIC_TYPE_OF_RETRIBUTION.Remove(dIC_TYPE_OF_RETRIBUTION);
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
