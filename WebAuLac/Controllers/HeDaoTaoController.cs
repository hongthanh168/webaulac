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
    public class HeDaoTaoController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: DIC_SCHOOL
        public ActionResult Index()
        {
            return View(db.HeDaoTaos.ToList());
        }

        // GET: DIC_SCHOOL/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HeDaoTao heDaoTao = db.HeDaoTaos.Find(id);
            if (heDaoTao == null)
            {
                return HttpNotFound();
            }
            return PartialView(heDaoTao);
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
        public ActionResult Create([Bind(Include = "HeDaoTaoID,TenHeDaoTao")]  HeDaoTao heDaoTao)
        {
            if (ModelState.IsValid)
            {
                db.HeDaoTaos.Add(heDaoTao);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView(heDaoTao);
        }

        // GET: DIC_SCHOOL/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HeDaoTao heDaoTao = db.HeDaoTaos.Find(id);
            if (heDaoTao == null)
            {
                return HttpNotFound();
            }
            return PartialView(heDaoTao);
        }

        // POST: DIC_SCHOOL/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HeDaoTaoID,TenHeDaoTao")]  HeDaoTao heDaoTao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(heDaoTao).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(heDaoTao);
        }

        // GET: DIC_SCHOOL/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HeDaoTao heDaoTao = db.HeDaoTaos.Find(id);
            if (heDaoTao == null)
            {
                return HttpNotFound();
            }
            return PartialView(heDaoTao);
        }

        // POST: DIC_SCHOOL/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HeDaoTao heDaoTao = db.HeDaoTaos.Find(id);
            db.HeDaoTaos.Remove(heDaoTao);
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
