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
    [Authorize(Roles ="Admin")]
    public class DIC_CONTRACTTYPEController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: DIC_CONTRACTTYPE
        public ActionResult Index()
        {
            return View(db.DIC_CONTRACTTYPE.ToList());
        }

        // GET: DIC_CONTRACTTYPE/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_CONTRACTTYPE dIC_CONTRACTTYPE = db.DIC_CONTRACTTYPE.Find(id);
            if (dIC_CONTRACTTYPE == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_CONTRACTTYPE);
        }

        // GET: DIC_CONTRACTTYPE/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: DIC_CONTRACTTYPE/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContractTypeID,ContractTypeName")] DIC_CONTRACTTYPE dIC_CONTRACTTYPE)
        {
            if (ModelState.IsValid)
            {
                db.DIC_CONTRACTTYPE.Add(dIC_CONTRACTTYPE);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView(dIC_CONTRACTTYPE);
        }

        // GET: DIC_CONTRACTTYPE/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_CONTRACTTYPE dIC_CONTRACTTYPE = db.DIC_CONTRACTTYPE.Find(id);
            if (dIC_CONTRACTTYPE == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_CONTRACTTYPE);
        }

        // POST: DIC_CONTRACTTYPE/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContractTypeID,ContractTypeName")] DIC_CONTRACTTYPE dIC_CONTRACTTYPE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dIC_CONTRACTTYPE).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(dIC_CONTRACTTYPE);
        }

        // GET: DIC_CONTRACTTYPE/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_CONTRACTTYPE dIC_CONTRACTTYPE = db.DIC_CONTRACTTYPE.Find(id);
            if (dIC_CONTRACTTYPE == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_CONTRACTTYPE);
        }

        // POST: DIC_CONTRACTTYPE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DIC_CONTRACTTYPE dIC_CONTRACTTYPE = db.DIC_CONTRACTTYPE.Find(id);
            db.DIC_CONTRACTTYPE.Remove(dIC_CONTRACTTYPE);
            db.SaveChanges();
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
