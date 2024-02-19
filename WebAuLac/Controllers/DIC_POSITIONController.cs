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
    [Authorize(Roles = "Admin")]
    public class DIC_POSITIONController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: DIC_POSITION
        public ActionResult Index()
        {
            return View(db.DIC_POSITION.Include(p => p.DIC_GROUPPOSITION).ToList());
        }

        // GET: DIC_POSITION/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_POSITION dIC_POSITION = db.DIC_POSITION.Find(id);
            if (dIC_POSITION == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_POSITION);
        }

        // GET: DIC_POSITION/Create
        public ActionResult Create()
        {
            ViewBag.GroupPositionID = new SelectList(db.DIC_GROUPPOSITION, "GroupPositionID", "GroupPositionName");
            return PartialView();
        }

        // POST: DIC_POSITION/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PositionID,PositionName,Description,GroupPositionID")] DIC_POSITION dIC_POSITION)
        {
            if (ModelState.IsValid)
            {
                //tìm ra id mới
                int maxPositionID = 0;
                try
                {
                    maxPositionID = db.DIC_POSITION.Max(x => x.PositionID);
                }
                catch { }
                dIC_POSITION.PositionID = maxPositionID + 1;
                db.DIC_POSITION.Add(dIC_POSITION);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.GroupPositionID = new SelectList(db.DIC_GROUPPOSITION, "GroupPositionID", "GroupPositionName");
            return PartialView(dIC_POSITION);
        }

        // GET: DIC_POSITION/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_POSITION dIC_POSITION = db.DIC_POSITION.Find(id);
            if (dIC_POSITION == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupPositionID = new SelectList(db.DIC_GROUPPOSITION, "GroupPositionID", "GroupPositionName", dIC_POSITION.GroupPositionID);
            return PartialView(dIC_POSITION);
        }

        // POST: DIC_POSITION/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PositionID,PositionName,Description,GroupPositionID")] DIC_POSITION dIC_POSITION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dIC_POSITION).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.GroupPositionID = new SelectList(db.DIC_GROUPPOSITION, "GroupPositionID", "GroupPositionName", dIC_POSITION.GroupPositionID);
            return PartialView(dIC_POSITION);
        }

        // GET: DIC_POSITION/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_POSITION dIC_POSITION = db.DIC_POSITION.Find(id);
            if (dIC_POSITION == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_POSITION);
        }

        // POST: DIC_POSITION/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DIC_POSITION dIC_POSITION = db.DIC_POSITION.Find(id);
            db.DIC_POSITION.Remove(dIC_POSITION);
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
