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
    [Authorize(Roles = "Boss, HR, EduCenter, DaoTao, View")]
    public class LichKiemTrasController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: LichKiemTras
        public ActionResult Index()
        {
            int year = DateTime.Now.Year;           
            var lichKiemTras = db.LichKiemTras.Where(x => x.Nam ==year).Include(l => l.DIC_DEPARTMENT).Include(l => l.LoaiKiemTra);
            return View(lichKiemTras.ToList());
        }
        public ActionResult LichTheoPhongBan()
        {
            //lấy danh sách các loại kiểm tra
            List<string> listLoaiKT = new List<string>();
            var loaiKiemTra = db.LoaiKiemTras.ToList();
            foreach (var item in loaiKiemTra)
            {
                listLoaiKT.Add(item.VietTat);
            }
            //lấy ra danh sách kiểm tra có trong năm hiện tại
            int year = DateTime.Now.Year;
            var lichKiemTras = db.LichKiemTras.Where(x => x.Nam == year).Include(l => l.DIC_DEPARTMENT).Include(l => l.LoaiKiemTra);

            //tạo bảng chứa dữ liệu
            DataTable dataTable = new DataTable();
            //tạo cột cho bảng từ danh sách listLoaiKT
            dataTable.Columns.Add("PhongBan", typeof(string));
            foreach (var item in listLoaiKT)
            {
                dataTable.Columns.Add(item, typeof(string));
            }
            //tạo dòng cho bảng
            DataRow dataRow;
            //lấy ra danh sách phòng ban from lichKiemTras
            var phongBan = lichKiemTras.Select(x => x.DIC_DEPARTMENT).Distinct().ToList();            
            foreach (var item in phongBan)
            {
                dataRow = dataTable.NewRow();
                dataRow["PhongBan"] = item.DepartmentName;
                foreach (var item2 in listLoaiKT)
                {
                    var kiemTra = lichKiemTras.Where(x => x.DepartmentID == item.DepartmentID && x.LoaiKiemTra.VietTat == item2).FirstOrDefault();
                    if (kiemTra != null)
                    {
                        dataRow[item2] = kiemTra.Ngay + "/" + kiemTra.Thang;
                        //kiểm tra xem ngày kiểm tra có nằm trong khoảng 30 ngaày sau ngày hiện tại không
                        DateTime ngayKiemTra = new DateTime(year, kiemTra.Thang.Value, kiemTra.Ngay.Value);
                        if (ngayKiemTra.AddDays (-30) <= DateTime.Now && DateTime.Now <= ngayKiemTra  )
                        {
                            dataRow[item2] = "<span style='color:red'>" + kiemTra.Ngay + "/" + kiemTra.Thang + "</span>";
                        }
                    }
                    else
                    {
                        dataRow[item2] = "";
                    }
                }
                dataTable.Rows.Add(dataRow);
            }
            //order dt by phong ban
            dataTable.DefaultView.Sort = "PhongBan ASC";
            ViewBag.listLoaiKT = listLoaiKT;
            return View(dataTable);
        }
        // GET: LichKiemTras/Details/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichKiemTra lichKiemTra = db.LichKiemTras.Find(id);
            if (lichKiemTra == null)
            {
                return HttpNotFound();
            }
            return View(lichKiemTra);
        }

        // GET: LichKiemTras/Create
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName");
            ViewBag.LoaiKiemTraID = new SelectList(db.LoaiKiemTras, "LoaiKiemTraID", "TenLoaiKiemTra");
            return PartialView();
        }

        // POST: LichKiemTras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Create([Bind(Include = "LichKiemTraID,DepartmentID,LoaiKiemTraID,Thang,Nam,Ngay")] LichKiemTra lichKiemTra)
        {
            if (ModelState.IsValid)
            {
                db.LichKiemTras.Add(lichKiemTra);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", lichKiemTra.DepartmentID);
            ViewBag.LoaiKiemTraID = new SelectList(db.LoaiKiemTras, "LoaiKiemTraID", "TenLoaiKiemTra", lichKiemTra.LoaiKiemTraID);
            return PartialView(lichKiemTra);
        }

        // GET: LichKiemTras/Edit/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichKiemTra lichKiemTra = db.LichKiemTras.Find(id);
            if (lichKiemTra == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", lichKiemTra.DepartmentID);
            ViewBag.LoaiKiemTraID = new SelectList(db.LoaiKiemTras, "LoaiKiemTraID", "TenLoaiKiemTra", lichKiemTra.LoaiKiemTraID);
            return View(lichKiemTra);
        }

        // POST: LichKiemTras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit([Bind(Include = "LichKiemTraID,DepartmentID,LoaiKiemTraID,Thang,Nam,Ngay")] LichKiemTra lichKiemTra)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lichKiemTra).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", lichKiemTra.DepartmentID);
            ViewBag.LoaiKiemTraID = new SelectList(db.LoaiKiemTras, "LoaiKiemTraID", "TenLoaiKiemTra", lichKiemTra.LoaiKiemTraID);
            return View(lichKiemTra);
        }

        // GET: LichKiemTras/Delete/5
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichKiemTra lichKiemTra = db.LichKiemTras.Find(id);
            if (lichKiemTra == null)
            {
                return HttpNotFound();
            }
            return View(lichKiemTra);
        }

        // POST: LichKiemTras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        [Authorize(Roles = "Create")]
        public ActionResult DeleteConfirmed(int id)
        {
            LichKiemTra lichKiemTra = db.LichKiemTras.Find(id);
            db.LichKiemTras.Remove(lichKiemTra);
            db.SaveChanges();
            return RedirectToAction("Index");
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
