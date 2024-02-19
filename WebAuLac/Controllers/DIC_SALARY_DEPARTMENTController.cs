using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using WebAuLac.Models;

namespace WebAuLac.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DIC_SALARY_DEPARTMENTController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: DIC_POSITION_DEGREE
        public ActionResult DSChucDanh()
        {
            return View(db.DIC_POSITION.Include(p => p.DIC_GROUPPOSITION).ToList());
        }

        public ActionResult Index()
        {
            //ViewBag.idPos = id;
            return View(db.DIC_SALARY_DEPARTMENT.ToList());
            //return View(db.DIC_SALARY_DEPARTMENT.Where(x =>x.PositionID==id).Include(x => x.DIC_POSITION).Include(x =>x.DIC_DEGREE).ToList());
        }

        // GET: DIC_POSITION_DEGREE/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_POSITION_DEGREE dIC_POSITION_DEGREE = db.DIC_POSITION_DEGREE.Find(id);
            if (dIC_POSITION_DEGREE == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_POSITION_DEGREE);
        }

        // GET: DIC_POSITION_DEGREE/Create
        public ActionResult Create()
        {
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName");
            ViewBag.DegreeID = new SelectList(db.DIC_DEGREE, "DegreeID", "DegreeName");
            return PartialView();
        }

        // POST: DIC_POSITION_DEGREE/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PositionDegreeID,PositionID,DegreeID")] DIC_POSITION_DEGREE dIC_POSITION_DEGREE)
        {
            if (ModelState.IsValid)
            {
                db.DIC_POSITION_DEGREE.Add(dIC_POSITION_DEGREE);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", dIC_POSITION_DEGREE.PositionID);
            ViewBag.DegreeID = new SelectList(db.DIC_DEGREE, "DegreeID", "DegreeName", dIC_POSITION_DEGREE.DegreeID);
            return PartialView(dIC_POSITION_DEGREE);
        }

        //thêm 1 lúc nhiều bằng cấp
        public ActionResult CreateMultiple(int idPos)
        {
            DIC_POSITION chucDanh = db.DIC_POSITION.Find(idPos);
            if (chucDanh == null)
            {
                return HttpNotFound();
            }
            ViewBag.chucDanh = chucDanh;
            //lấy danh sách chức danh đã có để loại trừ
            var idDaTonTai = db.DIC_POSITION_DEGREE.Where(ct => ct.PositionID == idPos).Select(x => x.DegreeID).ToList();
            var dsBangCap = db.DIC_DEGREE.Where(x => !idDaTonTai.Contains(x.DegreeID));
            return View(dsBangCap.ToList());
        }
        [WebMethod]
        public ActionResult ThemVaoDanhSach(string[] function_param, string idPos)
        {
            if (function_param.Length > 0)
            {
                for (int i = 0; i < function_param.Length; i++)
                {
                    DIC_POSITION_DEGREE obj = new DIC_POSITION_DEGREE();
                    obj.PositionID = int.Parse(idPos);
                    obj.DegreeID = int.Parse(function_param[i]);
                    //lấy ra chức vụ và add vào
                    db.DIC_POSITION_DEGREE.Add(obj);
                }
                db.SaveChanges();
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        // GET: DIC_POSITION_DEGREE/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_POSITION_DEGREE dIC_POSITION_DEGREE = db.DIC_POSITION_DEGREE.Find(id);
            if (dIC_POSITION_DEGREE == null)
            {
                return HttpNotFound();
            }
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", dIC_POSITION_DEGREE.PositionID);
            ViewBag.DegreeID = new SelectList(db.DIC_DEGREE, "DegreeID", "DegreeName", dIC_POSITION_DEGREE.DegreeID);
            return PartialView(dIC_POSITION_DEGREE);
        }

        // POST: DIC_POSITION_DEGREE/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PositionDegreeID,PositionID,DegreeID")] DIC_POSITION_DEGREE dIC_POSITION_DEGREE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dIC_POSITION_DEGREE).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", dIC_POSITION_DEGREE.PositionID);
            ViewBag.DegreeID = new SelectList(db.DIC_DEGREE, "DegreeID", "DegreeName", dIC_POSITION_DEGREE.DegreeID);
            return PartialView(dIC_POSITION_DEGREE);
        }

        // GET: DIC_POSITION_DEGREE/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIC_POSITION_DEGREE dIC_POSITION_DEGREE = db.DIC_POSITION_DEGREE.Find(id);
            if (dIC_POSITION_DEGREE == null)
            {
                return HttpNotFound();
            }
            return PartialView(dIC_POSITION_DEGREE);
        }

        // POST: DIC_POSITION_DEGREE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DIC_POSITION_DEGREE dIC_POSITION_DEGREE = db.DIC_POSITION_DEGREE.Find(id);
            db.DIC_POSITION_DEGREE.Remove(dIC_POSITION_DEGREE);
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
