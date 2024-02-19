using HRM.Controllers;
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
    [Authorize(Roles = "Boss, HR, EduCenter, DaoTao")]
    public class tbl_ctdaotaoController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: tbl_ctdaotao
        public ActionResult Index(int idKhoaDaotao)
        {
            //lấy thông tin khóa đào tạo
            tbl_khoadaotao obj = db.tbl_khoadaotao.Find(idKhoaDaotao);
            if (obj == null)
            {
                return HttpNotFound();
            }
            ViewBag.khoaDaoTao = obj;
            var tbl_ctdaotao = db.tbl_ctdaotao.Where(x => x.id_khoadaotao==idKhoaDaotao).Include(t => t.HRM_EMPLOYEE).Include(t => t.tbl_khoadaotao);
            return View(tbl_ctdaotao.ToList());
        }

        // GET: tbl_ctdaotao/Details/5
        [Authorize(Roles = "DaoTao")]
        [Authorize(Roles = "Create")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_ctdaotao tbl_ctdaotao = db.tbl_ctdaotao.Find(id);
            if (tbl_ctdaotao == null)
            {
                return HttpNotFound();
            }
            return PartialView(tbl_ctdaotao);
        }

        [Authorize(Roles = "DaoTao")]
        [Authorize(Roles = "Create")]
        public FileResult InDanhSachRaExcel(int khoaDaoTaoID)
        {
            XuatWordExcelController ctrl = new XuatWordExcelController();
            ctrl.ServerPath = Server.MapPath("~/App_Data");
            ctrl.AppUser = User;
            return ctrl.XuatDanhSachHocVienTheoKhoaDaoTao(khoaDaoTaoID);
        }
        // GET: tbl_ctdaotao/Create
        [Authorize(Roles = "DaoTao")]
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "HoTen");
            ViewBag.id_khoadaotao = new SelectList(db.tbl_khoadaotao, "id_khoadaotao", "tenkhoadaotao");
            return PartialView();
        }

        [Authorize(Roles = "DaoTao")]
        [Authorize(Roles = "Create")]
        public ActionResult CreateWithID(int idKhoaDaoTao)
        {
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "HoTen");
            tbl_khoadaotao khoa = db.tbl_khoadaotao.Find(idKhoaDaoTao);
            if (khoa == null)
            {
                return HttpNotFound();
            }
            ViewBag.khoaDaoTao = khoa;
            //ViewBag.id_khoadaotao = new SelectList(db.tbl_khoadaotao, "id_khoadaotao", "tenkhoadaotao", idKhoaDaoTao);
            tbl_ctdaotao obj = new tbl_ctdaotao();
            obj.id_khoadaotao = idKhoaDaoTao;
            return PartialView(obj);
        }

        [Authorize(Roles = "DaoTao")]
        [Authorize(Roles = "Create")]
        public ActionResult CreateMultiple(int idKhoaDaoTao)
        {
            tbl_khoadaotao khoa = db.tbl_khoadaotao.Find(idKhoaDaoTao);
            if (khoa == null)
            {
                return HttpNotFound();
            }
            ViewBag.khoaDaoTao = khoa;
            //lấy danh sách nhân viên đã có trong lớp để loại trừ
            var idDaTonTai = db.tbl_ctdaotao.Where(ct => ct.id_khoadaotao==idKhoaDaoTao).Select(x => x.EmployeeID).ToList();
            var hRM_EMPLOYEE = db.sp_T_LayDanhSachNhanVien().Where(x => !idDaTonTai.Contains(x.EmployeeID));
            hRM_EMPLOYEE = hRM_EMPLOYEE.OrderBy(x => x.DepartmentID).ThenBy(x => x.PositionID).ThenBy(x => x.LastName).ThenBy(x => x.FirstName);
            return View(hRM_EMPLOYEE.ToList());
        }
        [WebMethod]
        [Authorize(Roles = "DaoTao")]
        [Authorize(Roles = "Create")]
        public ActionResult ThemVaoLop(string[] function_param, string idKhoa)
        {
            if (function_param.Length > 0)
            {
                for(int i=0; i < function_param.Length; i++)
                {
                    tbl_ctdaotao obj = new tbl_ctdaotao();
                    obj.id_khoadaotao = int.Parse(idKhoa);
                    obj.EmployeeID = int.Parse(function_param[i]);
                    //lấy ra chức vụ và add vào
                    viewHRM_EMPLOYMENTHISTORY objThongTin = db.viewHRM_EMPLOYMENTHISTORY.Where(x => x.EmployeeID == obj.EmployeeID.Value).FirstOrDefault();
                    if (objThongTin != null)
                    {
                        obj.ChucVu = objThongTin.ChucVu;
                    }
                    db.tbl_ctdaotao.Add(obj);                    
                }
                db.SaveChanges();
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        // POST: tbl_ctdaotao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "DaoTao")]
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_ctdaotao,id_khoadaotao,EmployeeID,daotaotheoyeucau,ketqua")] tbl_ctdaotao tbl_ctdaotao)
        {
            if (ModelState.IsValid)
            {
                db.tbl_ctdaotao.Add(tbl_ctdaotao);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "HoTen", tbl_ctdaotao.EmployeeID);
            ViewBag.id_khoadaotao = new SelectList(db.tbl_khoadaotao, "id_khoadaotao", "tenkhoadaotao", tbl_ctdaotao.id_khoadaotao);
            return PartialView(tbl_ctdaotao);
        }

        // GET: tbl_ctdaotao/Edit/5
        [Authorize(Roles = "DaoTao")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_ctdaotao tbl_ctdaotao = db.tbl_ctdaotao.Find(id);
            if (tbl_ctdaotao == null)
            {
                return HttpNotFound();
            }
            ViewBag.HoTen = tbl_ctdaotao.HRM_EMPLOYEE.HoTen;
            ViewBag.TenLop = tbl_ctdaotao.tbl_khoadaotao.tenkhoadaotao;
            return PartialView(tbl_ctdaotao);
        }

        // POST: tbl_ctdaotao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "DaoTao")]
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id_ctdaotao, string ketqua, string GhiChu, string ChucVu)
        {
            tbl_ctdaotao obj = db.tbl_ctdaotao.Find(id_ctdaotao);
            if (ModelState.IsValid)
            {
                obj.ketqua = ketqua;
                obj.GhiChu = GhiChu;
                obj.ChucVu = ChucVu;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.HoTen = obj.HRM_EMPLOYEE.HoTen;
            ViewBag.TenLop = obj.tbl_khoadaotao.tenkhoadaotao;
            return PartialView(obj);
        }

        // GET: tbl_ctdaotao/Delete/5
        [Authorize(Roles = "DaoTao")]
        [Authorize(Roles = "Create")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_ctdaotao tbl_ctdaotao = db.tbl_ctdaotao.Find(id);
            if (tbl_ctdaotao == null)
            {
                return HttpNotFound();
            }
            return PartialView(tbl_ctdaotao);
        }

        // POST: tbl_ctdaotao/Delete/5
        [Authorize(Roles = "DaoTao")]
        [Authorize(Roles = "Create")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_ctdaotao tbl_ctdaotao = db.tbl_ctdaotao.Find(id);
            db.tbl_ctdaotao.Remove(tbl_ctdaotao);
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
