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
    [Authorize(Roles = "DaoTao")]
    [Authorize(Roles = "Create")]
    public class tbl_cosodaotaoController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: tbl_cosodaotao
        public ActionResult Index()
        {
            return View(db.tbl_cosodaotao.ToList());
        }

        // GET: tbl_cosodaotao/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_cosodaotao tbl_cosodaotao = db.tbl_cosodaotao.Find(id);
            if (tbl_cosodaotao == null)
            {
                return HttpNotFound();
            }
            return PartialView(tbl_cosodaotao);
        }

        // GET: tbl_cosodaotao/Create
       
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: tbl_cosodaotao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_cosodaotao,tencoso")] tbl_cosodaotao tbl_cosodaotao)
        {
            if (ModelState.IsValid)
            {
                db.tbl_cosodaotao.Add(tbl_cosodaotao);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return View(tbl_cosodaotao);
        }

        // GET: tbl_cosodaotao/Edit/5
       
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_cosodaotao tbl_cosodaotao = db.tbl_cosodaotao.Find(id);
            if (tbl_cosodaotao == null)
            {
                return HttpNotFound();
            }
            return PartialView(tbl_cosodaotao);
        }

        // POST: tbl_cosodaotao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_cosodaotao,tencoso")] tbl_cosodaotao tbl_cosodaotao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_cosodaotao).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(tbl_cosodaotao);
        }

        // GET: tbl_cosodaotao/Delete/5
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_cosodaotao tbl_cosodaotao = db.tbl_cosodaotao.Find(id);
            if (tbl_cosodaotao == null)
            {
                return HttpNotFound();
            }
            return PartialView(tbl_cosodaotao);
        }

        // POST: tbl_cosodaotao/Delete/5
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_cosodaotao tbl_cosodaotao = db.tbl_cosodaotao.Find(id);
            db.tbl_cosodaotao.Remove(tbl_cosodaotao);
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
