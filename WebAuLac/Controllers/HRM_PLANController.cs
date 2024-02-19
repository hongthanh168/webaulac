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
    [Authorize(Roles = "Boss, HR, EduCenter")]
    public class HRM_PLANController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: HRM_PLAN        
        public ActionResult Index()
        {
            return View(db.view_HRM_PLAN.OrderByDescending(x =>x.PlanDate).ToList());
        }

        [Authorize(Roles = "Boss, HR, EduCenter")]
        public ActionResult IndexXacNhan()
        {
            return View(db.HRM_PLAN.OrderByDescending(x => x.PlanDate).ToList());
        }
        
        public ActionResult IndexQuyetDinh()
        {
            return View(db.HRM_PLAN.OrderByDescending(x => x.PlanDate).ToList());
        }

        [Authorize(Roles = "Boss")]
        public ActionResult IndexGiamDoc()
        {
            return View(db.HRM_PLAN.OrderByDescending(x => x.PlanDate).ToList());
        }
        // GET: HRM_PLAN/Details/5        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_PLAN hRM_PLAN = db.HRM_PLAN.Find(id);
            if (hRM_PLAN == null)
            {
                return HttpNotFound();
            }
            //lấy danh sách các tàu ra
            //ViewBag.TauID = new SelectList(db.DIC_DEPARTMENT.Where(x =>x.ParentID==8 && x.IsLast==true), "DepartmentID", "DepartmentName");
            ViewBag.TieuChuanID = new SelectList(db.TieuChuanCrewMatrices, "TieuChuanID", "TieuChuanName");
            return View(hRM_PLAN);
        }


        public ActionResult ChiTietKeHoach(int id)
        {
            HRM_PLAN objPlan = db.HRM_PLAN.Find(id);
            if (objPlan != null)
            {
                ViewBag.objPlan = objPlan;
            }
            return PartialView(db.sp_LayKeHoachDieuDong(id));
        }

        public ActionResult dsThuyenVien(int PlanID)
        {
            HRM_PLAN obj = db.HRM_PLAN.Find(PlanID);
            return PartialView(db.sp_LayDSThuyenVien(PlanID).Where(x => ThanhUtilities.MonthDifference(x.DecisionDate, obj.PlanDate) >= 8).OrderByDescending(x => x.SoNgay));
        }

        [Authorize(Roles = "EduCenter")]
        [Authorize(Roles = "Create")]
        public ActionResult InsertKH(int CrewOnID, int CrewOffID, int TauID, int PlanID)
        {
            db.sp_InsertChiTietKeHoachDieuDong(PlanID, TauID, CrewOffID, CrewOnID);
            db.SaveChanges();
            //lấy lại dữ liệu
            HRM_PLAN objPlan = db.HRM_PLAN.Find(PlanID);
            if (objPlan != null)
            {
                ViewBag.objPlan = objPlan;
            }
            return PartialView("ChiTietKeHoach", db.sp_LayKeHoachDieuDong(PlanID));
        }

        public ActionResult dsDuTru(int ThuyenVienID, int TauID, int PlanID, int tatCa)
        {
            //lấy ra chức vụ của thuyền viên đó
            viewHRM_EMPLOYMENTHISTORY tv = db.viewHRM_EMPLOYMENTHISTORY.Where(x => x.EmployeeID == ThuyenVienID).FirstOrDefault();
            if (tv == null)
            {
                if (tatCa==0)
                {
                    return PartialView(db.sp_LayDSDuTru(PlanID, TauID).Where(x => x.SSDD == true).OrderByDescending(x => x.SoNgay));
                }else
                {
                    return PartialView(db.sp_LayDSDuTru(PlanID, TauID).OrderByDescending(x => x.SoNgay));
                }
                
            }
            else
            {
                if (tatCa == 0)
                {
                    return PartialView(db.sp_LayDSDuTru(PlanID, TauID).Where(x => (x.PositionID == tv.PositionID ||(x.PluralityID.HasValue && x.PluralityID==tv.PositionID)) && x.SSDD == true).OrderByDescending(x => x.SoNgay));
                }
                else
                {
                    return PartialView(db.sp_LayDSDuTru(PlanID, TauID).Where(x => x.PositionID == tv.PositionID || (x.PluralityID.HasValue && x.PluralityID == tv.PositionID)).OrderByDescending(x => x.SoNgay));
                }                
            }
        }

        public ActionResult locDuTru(int ThuyenVienID, int TauID, int PlanID)
        {
            ViewBag.DepartmentID = TauID;
            ViewBag.PlanID = PlanID;
            ViewBag.LoaiDuTru = db.DIC_DEPARTMENT.Where(x => x.ParentID == 17);
            //lấy ra chức vụ của thuyền viên đó
            viewHRM_EMPLOYMENTHISTORY tv = db.viewHRM_EMPLOYMENTHISTORY.Where(x => x.EmployeeID == ThuyenVienID).FirstOrDefault();
            if (tv == null)
            {
                ViewBag.SelectedPos = -1;
            }
            else
            {
                ViewBag.SelectedPos = tv.PositionID;
            }
            return PartialView(db.DIC_POSITION.Where(x => x.GroupPositionID != 4 && x.GroupPositionID != 3));
        }

        public ActionResult KiemTraBangCap(int duTruID, int thuyenVienID, int planID)
        {
            //lấy ra vị trí của thuyền viên
            viewHRM_EMPLOYMENTHISTORY tv = db.viewHRM_EMPLOYMENTHISTORY.Where(x => x.EmployeeID == thuyenVienID).FirstOrDefault();
            int posID = tv.PositionID.Value;
            HRM_PLAN objPlan = db.HRM_PLAN.Find(planID);
            return PartialView(db.sp_LayDSBangCapTheoChucVu(duTruID, posID, objPlan.PlanDate).OrderBy(x => x.tinhtrang));
        }

        public ActionResult CheckCrewMatrix(int TauID, int PlanID, int TieuChuanID, int CrewOffID, int CrewOnID)
        {
            string kq = "";
            sp_T_CrewMatrixTau_Result tauMatrix = db.sp_T_CrewMatrixTau(TauID, PlanID, CrewOnID, CrewOffID, TieuChuanID).First();
            if (tauMatrix.KetQua.Value ==false) //không dạt
            {
                ViewBag.TieuChuanName = db.TieuChuanCrewMatrices.Find(TieuChuanID).TieuChuanName;
                return PartialView("CrewMatrixTau", tauMatrix);
            }else
            {
                kq = "Đạt";
                return Json(new { success = true, noiDung = kq }, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult locDuTru(int[] chkPos, int[] chkLoaiDuTru, int PlanID, int TauID, int soThangTu, int soThangDen)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<WebAuLac.Models.sp_LayDSDuTru_Result> result = db.sp_LayDSDuTru(PlanID, TauID);
                HRM_PLAN obj = db.HRM_PLAN.Find(PlanID);
                if (!chkPos.Contains(0))
                {
                    result = result.Where(x => (x.PositionID.HasValue && chkPos.Contains(x.PositionID.Value)) ||
                                               (x.PluralityID.HasValue && chkPos.Contains(x.PluralityID.Value))
                    ).ToList();
                }
                if (!chkLoaiDuTru.Contains(0))
                {
                    result = result.Where(x => x.DepartmentID.HasValue && chkLoaiDuTru.Contains(x.DepartmentID.Value)).ToList();
                }
                //lọc theo số ngày
                if (soThangTu > 0)
                {
                    result = result.Where(x => ThanhUtilities.MonthDifference(x.DecisionDate.Value, obj.PlanDate.Value) >= soThangTu);
                }
                if (soThangDen > 0)
                {
                    result = result.Where(x => ThanhUtilities.MonthDifference(x.DecisionDate.Value, obj.PlanDate.Value) <= soThangDen);
                }
                return PartialView("dsDuTru", result.OrderByDescending(x => x.SoNgay));
            }

            return PartialView("dsDuTru", db.sp_LayDSDuTru(PlanID, TauID));
        }

        public ActionResult locThuyenVien(int PlanID)
        {
            ViewBag.PlanID = PlanID;
            return PartialView(db.DIC_POSITION.Where(x => x.GroupPositionID!=4 && x.GroupPositionID!=3));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult locThuyenVien(int[] chkPos, int PlanID, int soThangTu, int soThangDen)
        {
            if (ModelState.IsValid)
            {
                HRM_PLAN obj = db.HRM_PLAN.Find(PlanID);
                IEnumerable<WebAuLac.Models.sp_LayDSThuyenVien_Result> result = db.sp_LayDSThuyenVien(PlanID);
                if (!chkPos.Contains(0))
                {
                    result = result.Where(x => (x.PositionID.HasValue && chkPos.Contains(x.PositionID.Value) ) 
                    || (x.PluralityID.HasValue && chkPos.Contains(x.PluralityID.Value))
                    ).ToList();
                }
                //lọc theo số ngày
                if (soThangTu > 0)
                {
                    result = result.Where(x => ThanhUtilities.MonthDifference(x.DecisionDate, obj.PlanDate) >= soThangTu);
                }
                if (soThangDen > 0)
                {
                    result = result.Where(x => ThanhUtilities.MonthDifference(x.DecisionDate, obj.PlanDate) <= soThangDen);
                }
                return PartialView("dsThuyenVien", result.OrderByDescending(x => x.SoNgay));
            }

            return PartialView("dsThuyenVien", db.sp_LayDSThuyenVien(PlanID));
        }      
          
        public ActionResult LocThuyenVien8thang(int PlanID, int TatCa)
        {
            if (ModelState.IsValid)
            {
                HRM_PLAN obj = db.HRM_PLAN.Find(PlanID);
                IEnumerable<WebAuLac.Models.sp_LayDSThuyenVien_Result> result = db.sp_LayDSThuyenVien(PlanID);
                if (TatCa == 0)
                {
                    result = result.Where(x => ThanhUtilities.MonthDifference(x.DecisionDate, obj.PlanDate) >= 8);
                }                
                return PartialView("dsThuyenVien", result.OrderByDescending(x => x.SoNgay));
            }
            return PartialView("dsThuyenVien", db.sp_LayDSThuyenVien(PlanID));
        }

        public ActionResult LocDuTruSSDD(int PlanID, int TauID, int ThuyenVienID, int TatCa)
        {
            if (ModelState.IsValid)
            {
                HRM_PLAN obj = db.HRM_PLAN.Find(PlanID);
                IEnumerable<WebAuLac.Models.sp_LayDSDuTru_Result> result = db.sp_LayDSDuTru(PlanID, TauID);
                if (ThuyenVienID > 0)
                {
                    viewHRM_EMPLOYMENTHISTORY tv = db.viewHRM_EMPLOYMENTHISTORY.Where(x => x.EmployeeID == ThuyenVienID).FirstOrDefault();
                    if (tv != null)
                    {
                        result = result.Where(x => x.PositionID == tv.PositionID);
                    }
                }
                if (TatCa == 0)
                {
                    result = result.Where(x => x.SSDD ==true);
                }
                return PartialView("dsDuTru", result.OrderByDescending(x => x.SoNgay));
            }
            return PartialView("dsDuTru", db.sp_LayDSDuTru(PlanID, TauID));
        }
        // GET: HRM_PLAN/Create
        [Authorize(Roles = "EduCenter")]
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: HRM_PLAN/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "EduCenter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlanID,PlanName,PlanDate, Note")] HRM_PLAN hRM_PLAN)
        {
            if (ModelState.IsValid)
            {
                db.HRM_PLAN.Add(hRM_PLAN);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView(hRM_PLAN);
        }

        // GET: HRM_PLAN/Edit/5
        [Authorize(Roles = "EduCenter")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_PLAN hRM_PLAN = db.HRM_PLAN.Find(id);
            if (hRM_PLAN == null)
            {
                return HttpNotFound();
            }
            return PartialView(hRM_PLAN);
        }

        // POST: HRM_PLAN/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "EduCenter")]
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlanID,PlanName,PlanDate,Note")] HRM_PLAN hRM_PLAN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRM_PLAN).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(hRM_PLAN);
        }

        [Authorize(Roles = "Boss")]
        public ActionResult GiamDocEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_PLAN hRM_PLAN = db.HRM_PLAN.Find(id);
            if (hRM_PLAN == null)
            {
                return HttpNotFound();
            }
            return PartialView(hRM_PLAN);
        }

        // POST: HRM_PLAN/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Boss")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GiamDocEdit(int PlanID, string Note)
        {
            HRM_PLAN hRM_PLAN = db.HRM_PLAN.Find(PlanID);
            if (ModelState.IsValid)
            {
                hRM_PLAN.Note = Note;
                db.Entry(hRM_PLAN).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(hRM_PLAN);
        }
        [Authorize(Roles = "Boss")]
        public ActionResult GiamDocDuyet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_PLAN hRM_PLAN = db.HRM_PLAN.Find(id);
            if (hRM_PLAN == null)
            {
                return HttpNotFound();
            }
            return PartialView(hRM_PLAN);
        }

        // POST: HRM_PLAN/Delete/5
        [Authorize(Roles = "Boss")]
        [HttpPost, ActionName("GiamDocDuyet")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanDuyet(int id)
        {
            HRM_PLAN hRM_PLAN = db.HRM_PLAN.Find(id);
            hRM_PLAN.IsLock = true;
            db.Entry(hRM_PLAN).State = EntityState.Modified;
            //check duyệt cho từng chi tiết kế hoạch
            var chiTiet = db.HRM_DETAILPLAN.Where(x => x.PlanID == id);
            foreach (HRM_DETAILPLAN item in chiTiet)
            {
                item.DaDuyet = true;
                db.Entry(item).State = EntityState.Modified;
            }
            db.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Boss")]
        public ActionResult GiamDocBoDuyet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_PLAN hRM_PLAN = db.HRM_PLAN.Find(id);
            if (hRM_PLAN == null)
            {
                return HttpNotFound();
            }
            return PartialView(hRM_PLAN);
        }

        // POST: HRM_PLAN/Delete/5
        [Authorize(Roles = "Boss")]
        [HttpPost, ActionName("GiamDocBoDuyet")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanBoDuyet(int id)
        {
            HRM_PLAN hRM_PLAN = db.HRM_PLAN.Find(id);
            hRM_PLAN.IsLock = false;
            db.Entry(hRM_PLAN).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        // GET: HRM_PLAN/Delete/5
        [Authorize(Roles = "EduCenter")]
        [Authorize(Roles = "Create")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_PLAN hRM_PLAN = db.HRM_PLAN.Find(id);
            if (hRM_PLAN == null)
            {
                return HttpNotFound();
            }
            return PartialView(hRM_PLAN);
        }

        // POST: HRM_PLAN/Delete/5
        [Authorize(Roles = "EduCenter")]
        [Authorize(Roles = "Create")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HRM_PLAN hRM_PLAN = db.HRM_PLAN.Find(id);
            db.HRM_PLAN.Remove(hRM_PLAN);
            db.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteMultipleDetails(int[] arrID)
        {
            foreach (int id in arrID)
            {
                HRM_DETAILPLAN obj = db.HRM_DETAILPLAN.Find(id);
                db.HRM_DETAILPLAN.Remove(obj);
            }
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