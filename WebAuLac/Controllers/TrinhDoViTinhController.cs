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
    public class TrinhDoViTinhController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: DIC_SCHOOL
        public ActionResult Index()
        {
            return View(db.TrinhDoViTinhs.ToList());
        }

        // GET: DIC_SCHOOL/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrinhDoViTinh trinhDoViTinh = db.TrinhDoViTinhs.Find(id);
            if (trinhDoViTinh == null)
            {
                return HttpNotFound();
            }
            return PartialView(trinhDoViTinh);
        }

        // GET: DIC_SCHOOL/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: DIC_SCHOOL/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TrinhDoViTinhID,TenTrinhDoViTinh")]  TrinhDoViTinh trinhDoViTinh)
        {
            if (ModelState.IsValid)
            {
                db.TrinhDoViTinhs.Add(trinhDoViTinh);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView(trinhDoViTinh);
        }

        // GET: DIC_SCHOOL/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrinhDoViTinh trinhDoViTinh = db.TrinhDoViTinhs.Find(id);
            if (trinhDoViTinh == null)
            {
                return HttpNotFound();
            }
            return PartialView(trinhDoViTinh);
        }

        // POST: DIC_SCHOOL/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrinhDoViTinhID,TenTrinhDoViTinh")]  TrinhDoViTinh trinhDoViTinh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trinhDoViTinh).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(trinhDoViTinh);
        }

        // GET: DIC_SCHOOL/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrinhDoViTinh trinhDoViTinh = db.TrinhDoViTinhs.Find(id);
            if (trinhDoViTinh == null)
            {
                return HttpNotFound();
            }
            return PartialView(trinhDoViTinh);
        }

        // POST: DIC_SCHOOL/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrinhDoViTinh trinhDoViTinh = db.TrinhDoViTinhs.Find(id);
            db.TrinhDoViTinhs.Remove(trinhDoViTinh);
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
