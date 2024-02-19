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
    [Authorize(Roles = "Boss, HR, EduCenter, DaoTao")]
    public class tbl_khoadaotaoController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: tbl_khoadaotao
        public ActionResult Index()
        {
            var tbl_khoadaotao = db.tbl_khoadaotao.Include(t => t.tbl_cosodaotao);
            return View(tbl_khoadaotao.ToList());
        }

        // GET: tbl_khoadaotao/Details/5       
        public ActionResult Details(int? id)
        {
            return RedirectToAction("Index", "tbl_ctdaotao",new {idKhoaDaoTao =id.Value});
        }

        // GET: tbl_khoadaotao/Create
        [Authorize(Roles = "DaoTao")]
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            ViewBag.id_cosodaotao = new SelectList(db.tbl_cosodaotao, "id_cosodaotao", "tencoso");
            return PartialView();
        }

        // POST: tbl_khoadaotao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "DaoTao")]
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_khoadaotao,tenkhoadaotao, id_cosodaotao,ngaybatdau,hocphi,diadiem,MonHoc,CapDo,TheoYeuCau,GiayChungNhan,KhoaDaoTao,NgayKetThuc")] tbl_khoadaotao tbl_khoadaotao)
        {
            if (ModelState.IsValid)
            {
                db.tbl_khoadaotao.Add(tbl_khoadaotao);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet); ;
            }

            ViewBag.id_cosodaotao = new SelectList(db.tbl_cosodaotao, "id_cosodaotao", "tencoso", tbl_khoadaotao.id_cosodaotao);
            return View(tbl_khoadaotao);
        }

        // GET: tbl_khoadaotao/Edit/5
        [Authorize(Roles = "DaoTao")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_khoadaotao tbl_khoadaotao = db.tbl_khoadaotao.Find(id);
            if (tbl_khoadaotao == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cosodaotao = new SelectList(db.tbl_cosodaotao, "id_cosodaotao", "tencoso", tbl_khoadaotao.id_cosodaotao);
            return PartialView(tbl_khoadaotao);
        }

        // POST: tbl_khoadaotao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "DaoTao")]
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_khoadaotao,tenkhoadaotao,id_cosodaotao,ngaybatdau,hocphi,diadiem,MonHoc,CapDo,TheoYeuCau,GiayChungNhan,KhoaDaoTao,NgayKetThuc")] tbl_khoadaotao tbl_khoadaotao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_khoadaotao).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.id_cosodaotao = new SelectList(db.tbl_cosodaotao, "id_cosodaotao", "tencoso", tbl_khoadaotao.id_cosodaotao);
            return View(tbl_khoadaotao);
        }

        // GET: tbl_khoadaotao/Delete/5
        [Authorize(Roles = "DaoTao")]
        [Authorize(Roles = "Create")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_khoadaotao tbl_khoadaotao = db.tbl_khoadaotao.Find(id);
            if (tbl_khoadaotao == null)
            {
                return HttpNotFound();
            }
            return PartialView(tbl_khoadaotao);
        }

        // POST: tbl_khoadaotao/Delete/5
        [Authorize(Roles = "DaoTao")]
        [Authorize(Roles = "Create")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_khoadaotao tbl_khoadaotao = db.tbl_khoadaotao.Find(id);
            db.tbl_khoadaotao.Remove(tbl_khoadaotao);
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
