using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAuLac.Models;

namespace WebAuLac.Controllers
{
    [Authorize(Roles = "Boss, HR, EduCenter")]
    public class HRM_DETAILPLANController : Controller
    {
        private AuLacEntities db = new AuLacEntities();
        public FileResult InKeHoachDieuDong(int planID)
        {
            //InKeHoachDieuDong
            //int planID = 2;

            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = Server.MapPath("~/App_Data");

            FileInfo templateFile = new FileInfo(server + "//Mau_KHDD.xltx");
            FileInfo newFile = new FileInfo(outputDir.FullName + @"\KHDD.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\KHDD.xlsx");
            }
            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];


                //****************************************
                //1. TIEU DE
                HRM_PLAN plan = (from p in db.HRM_PLAN
                                 where p.PlanID == planID
                                 select p).SingleOrDefault();

                worksheet.Cells[8, 1].Value = plan.PlanName;

                //****************************************
                //2. NOI DUNG
                int row0 = 14;
                int i = 14;
                int icu = 14;
                var detailPlan = from p in db.HRM_DETAILPLAN
                                 where p.PlanID == planID
                                 orderby p.CrewOffDepartmentID
                                 select p;
                String tenTau = "";

                foreach (HRM_DETAILPLAN dp in detailPlan)
                {
                    HRM_EMPLOYEE thuyenvien = (from p in db.HRM_EMPLOYEE
                                               where p.EmployeeID == dp.CrewOffID
                                               select p).SingleOrDefault();

                    if (tenTau != dp.DIC_DEPARTMENT.Description)
                    {
                        tenTau = dp.DIC_DEPARTMENT.Description;
                        worksheet.Cells[i, 1].Value = dp.DIC_DEPARTMENT.Description;
                        if (i != 14)
                        {
                            using (var range = worksheet.Cells[icu, 1, i - 1, 1])
                            {
                                // Format text đỏ và đậm
                                //range.Style.Font.Color.SetColor(Color.Red);
                                range.Style.Font.Bold = true;
                                range.Merge = true;
                                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            }
                            icu = i;
                        }
                    }

                    if (i == row0 + detailPlan.Count() - 1)
                    {
                        using (var range = worksheet.Cells[icu, 1, row0 + detailPlan.Count() - 1, 1])
                        {
                            // Format text đỏ và đậm
                            //range.Style.Font.Color.SetColor(Color.Red);
                            range.Style.Font.Bold = true;
                            range.Merge = true;
                            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }
                    }

                    if (thuyenvien != null)
                    {
                        //2. Họ và tên thuyền viên
                        worksheet.Cells[i, 2].Value = thuyenvien.FirstName + " " + thuyenvien.LastName;
                        //3. Chức danh trên tàu
                        worksheet.Cells[i, 3].Value = dp.CrewOffPosition;
                        //4. Năm sinh
                        if (thuyenvien.BirthDay != null)
                        {
                            worksheet.Cells[i, 4].Value = thuyenvien.BirthDay.Value.Year.ToString();
                        }
                        //5. Ngày xuống tàu
                        worksheet.Cells[i, 5].Value = Convert.ToDateTime(dp.DateOff).ToString("dd/MM/yyyy");
                        //6. Thời gian đi tàu
                        worksheet.Cells[i, 6].Value = dp.TimeOff;
                    }

                    HRM_EMPLOYEE dutru = (from p in db.HRM_EMPLOYEE
                                          where p.EmployeeID == dp.CrewOnID
                                          select p).SingleOrDefault();

                    if (dutru != null)
                    {
                        //7. Họ tên dự trữ
                        worksheet.Cells[i, 7].Value = dutru.FirstName + " " + dutru.LastName;
                        //8. Năm sinh
                        if (dutru.BirthDay != null)
                        {
                            worksheet.Cells[i, 8].Value = dutru.BirthDay.Value.Year.ToString();
                        }
                        //9. Trình độ
                        if (dutru.DIC_EDUCATION != null)
                            worksheet.Cells[i, 9].Value = dutru.DIC_EDUCATION.EducationName;
                        //10.QT Đi tàu
                        worksheet.Cells[i, 10].Value = dp.CrewOnHistory;
                        //11. Thời gian ở dự trữ
                        worksheet.Cells[i, 11].Value = dp.TimeOn;
                        //12. Note
                        if (dp.Note != null)
                            worksheet.Cells[i, 12].Value = dp.Note;
                    }

                    i++;
                }
                using (ExcelRange r = worksheet.Cells[row0, 1, i - 1, 12])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                }

                worksheet.View.PageLayoutView = false;
                // save our new workbook and we are done!
                package.Save();
            }
            string absolutePath = newFile.FullName;// = _environment.WebRootPath + @"\" + relativePath;
            FileResult result = null;

            if (System.IO.File.Exists(absolutePath))
            {
                string fileName = System.IO.Path.GetFileName(absolutePath);
                byte[] fileBytes = System.IO.File.ReadAllBytes(absolutePath);
                result = File(fileBytes, "application/x-msdownload", fileName);
            }
            return result;

        }

        public FileResult InKeHoachDieuDongMauMoi(int planID)
        {
            //InKeHoachDieuDong
            //int planID = 2;

            DirectoryInfo outputDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            string server = Server.MapPath("~/App_Data");

            FileInfo templateFile = new FileInfo(server + "//Mau_KHDDMoi.xltx");
            FileInfo newFile = new FileInfo(outputDir.FullName + @"\KHDDMoi.xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\KHDDMoi.xlsx");
            }
            using (ExcelPackage package = new ExcelPackage(newFile, templateFile))
            {
                //Sheet xuất dữ liệu
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];


                //****************************************
                //1. TIEU DE
                //HRM_PLAN plan = (from p in db.HRM_PLAN
                //                 where p.PlanID == planID
                //                 select p).SingleOrDefault();

                worksheet.Cells[7, 13].Value = "TP. Hồ Chí Minh, ngày " + DateTime.Now.Day.ToString() + " tháng " + DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();

                //****************************************
                //2. NOI DUNG
                int row0 = 20;
                int i = 20;
                int icu = 20;
                int stt = 1;
                var detailPlan = from p in db.HRM_DETAILPLAN
                                 where p.PlanID == planID
                                 orderby p.CrewOffDepartmentID
                                 select p;
                String tenTau = "";

                foreach (HRM_DETAILPLAN dp in detailPlan)
                {
                    HRM_EMPLOYEE thuyenvien = (from p in db.HRM_EMPLOYEE
                                               where p.EmployeeID == dp.CrewOffID
                                               select p).SingleOrDefault();


                    if (tenTau != dp.DIC_DEPARTMENT.Description)
                    {
                        using (var range = worksheet.Cells[i, 1, i, 20])
                        {
                            // Format text đỏ và đậm
                            //range.Style.Font.Color.SetColor(Color.Red);
                            range.Style.Font.Bold = true;
                            range.Merge = true;
                            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }

                        tenTau = dp.DIC_DEPARTMENT.Description;
                        worksheet.Cells[i, 1].Value = dp.DIC_DEPARTMENT.DepartmentName;
                        i++;
                        //if (i != 20)
                        //{
                        //    using (var range = worksheet.Cells[icu, 1, i - 1, 1])
                        //    {
                        //        // Format text đỏ và đậm
                        //        //range.Style.Font.Color.SetColor(Color.Red);
                        //        range.Style.Font.Bold = true;
                        //        range.Merge = true;
                        //        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //    }
                        //    icu = i;
                        //}
                    }

                    worksheet.Cells[i, 1].Value = stt;
                    stt++;
                    //if (i == row0 + detailPlan.Count() - 1)
                    //{
                    //    using (var range = worksheet.Cells[icu, 1, row0 + detailPlan.Count() - 1, 1])
                    //    {
                    //        // Format text đỏ và đậm
                    //        //range.Style.Font.Color.SetColor(Color.Red);
                    //        range.Style.Font.Bold = true;
                    //        range.Merge = true;
                    //        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //    }
                    //}

                    if (thuyenvien != null)
                    {
                        //2. Họ và tên thuyền viên
                        worksheet.Cells[i, 2].Value = thuyenvien.FirstName + " " + thuyenvien.LastName;
                        //3. Chức danh trên tàu
                        worksheet.Cells[i, 3].Value = dp.CrewOffPosition;
                        //4. Năm sinh
                        if (thuyenvien.BirthDay != null)
                        {
                            worksheet.Cells[i, 4].Value = thuyenvien.BirthDay.Value.Year.ToString();
                        }
                        //5. Ngày xuống tàu
                        worksheet.Cells[i, 5].Value = Convert.ToDateTime(dp.DateOff).ToString("dd/MM/yyyy");
                        //6. Thời gian đi tàu
                        worksheet.Cells[i, 6].Value = dp.TimeOff;
                    }

                    HRM_EMPLOYEE dutru = (from p in db.HRM_EMPLOYEE
                                          where p.EmployeeID == dp.CrewOnID
                                          select p).SingleOrDefault();

                    if (dutru != null)
                    {
                        //7. Họ tên dự trữ
                        worksheet.Cells[i, 11].Value = dutru.FirstName + " " + dutru.LastName;
                        //8. Năm sinh
                        if (dutru.BirthDay != null)
                        {
                            worksheet.Cells[i, 12].Value = dutru.BirthDay.Value.Year.ToString();
                        }
                        ////9. Trình độ
                        //if (dutru.DIC_EDUCATION != null)
                        //    worksheet.Cells[i, 13].Value = dutru.DIC_EDUCATION.EducationName;
                        //13.QT Đi tàu
                        worksheet.Cells[i, 13].Value = dp.CrewOnHistory;
                        //11. Thời gian ở dự trữ
                        worksheet.Cells[i, 14].Value = Convert.ToDateTime(dp.DateOn).ToString("dd/MM/yyyy");
                        worksheet.Cells[i, 15].Value = dp.TimeOn;
                        //12. Note
                        if (dp.Note != null)
                            worksheet.Cells[i, 20].Value = dp.Note;
                    }

                    i++;
                }
                using (ExcelRange r = worksheet.Cells[row0, 1, i - 1, 20])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
                ////////////////////////////////// DUOI EXCEL

                using (var range = worksheet.Cells[i + 1, 2, i + 1, 20])
                {
                    // Format text đỏ và đậm
                    //range.Style.Font.Color.SetColor(Color.Red);

                    range.Style.Font.Size = 11;
                    range.Merge = true;
                    //range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                worksheet.Cells[i + 1, 2].Value = "Trung tâm thuyền viên kính mong Tổng Giám Đốc phê duyệt để triển khai thực hiện";

                using (var range = worksheet.Cells[i + 3, 2, i + 3, 5])
                {
                    // Format text đỏ và đậm
                    //range.Style.Font.Color.SetColor(Color.Red);

                    range.Style.Font.Size = 11;
                    range.Merge = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                worksheet.Cells[i + 3, 2].Value = "TỔNG GIÁM ĐỐC";


                using (var range = worksheet.Cells[i + 3, 15, i + 3, 20])
                {
                    // Format text đỏ và đậm
                    //range.Style.Font.Color.SetColor(Color.Red);

                    range.Style.Font.Size = 11;
                    range.Merge = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                worksheet.Cells[i + 3, 15].Value = "Trung tâm thuyền viên";

                worksheet.View.PageLayoutView = false;
                // save our new workbook and we are done!
                package.Save();
            }
            string absolutePath = newFile.FullName;// = _environment.WebRootPath + @"\" + relativePath;
            FileResult result = null;

            if (System.IO.File.Exists(absolutePath))
            {
                string fileName = System.IO.Path.GetFileName(absolutePath);
                byte[] fileBytes = System.IO.File.ReadAllBytes(absolutePath);
                result = File(fileBytes, "application/x-msdownload", fileName);
            }
            return result;

        }

        public ActionResult XuatDSDieuDongExcel(int PlanID)
        {
            CModuleSy cUtitlies = new CModuleSy();
            cUtitlies.BaoCaoKeHoachDieuDong(Server.MapPath("~/App_Data"), PlanID);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("DetailsOfOnePlan", new {PlanID = PlanID });
        }
        public ActionResult dsThuyenVienDieuDong(int PlanID)
        {
            //lấy danh sách thuyền viên của kế hoạch trên
            HRM_PLAN obj = db.HRM_PLAN.Find(PlanID);
            return PartialView(db.sp_LayDSThuyenVienDieuDong(PlanID).Where(x => ThanhUtilities.MonthDifference(x.DecisionDate, obj.PlanDate) >= 8).OrderByDescending(x => x.SoNgay));
        }
        public ActionResult GDLocThuyenVien8thang(int PlanID, int TatCa)
        {
            if (ModelState.IsValid)
            {
                HRM_PLAN obj = db.HRM_PLAN.Find(PlanID);
                IEnumerable<WebAuLac.Models.sp_LayDSThuyenVienDieuDong_Result> result = db.sp_LayDSThuyenVienDieuDong(PlanID);
                if (TatCa == 0)
                {
                    result = result.Where(x => ThanhUtilities.MonthDifference(x.DecisionDate, obj.PlanDate) >= 8);
                }
                return PartialView("dsThuyenVienDieuDong", result.OrderByDescending(x => x.SoNgay));
            }
            return PartialView("dsThuyenVienDieuDong", db.sp_LayDSThuyenVien(PlanID));
        }
        public ActionResult dsDuTru(int ThuyenVienID, int TauID, int PlanID)
        {
            //lấy ra chức vụ của thuyền viên đó
            viewHRM_EMPLOYMENTHISTORY tv = db.viewHRM_EMPLOYMENTHISTORY.Where(x => x.EmployeeID == ThuyenVienID).FirstOrDefault();
            if (tv == null)
            {
                return PartialView(db.sp_LayDSDuTruTheoKeHoach(PlanID, TauID,ThuyenVienID).OrderBy(x => x.ThongTin).OrderByDescending(x => x.SoNgay));
            }
            else
            {
                return PartialView(db.sp_LayDSDuTruTheoKeHoach(PlanID, TauID, ThuyenVienID).Where(x => x.PositionID == tv.PositionID).OrderBy(x => x.ThongTin).OrderByDescending(x => x.SoNgay));
            }
        }
        // GET: HRM_DETAILPLAN
        public ActionResult DetailsOfOnePlan(int PlanID)
        {
            HRM_PLAN obj = db.HRM_PLAN.Find(PlanID);
            if (obj == null)
            {
                return HttpNotFound();
            }
            ViewBag.Plan = obj;
            var list = db.sp_LayKeHoachDieuDong(PlanID);
            return View(list);
        }
        [Authorize(Roles = "Boss")]
        public ActionResult PlanViewGD(int PlanID)
        {
            HRM_PLAN obj = db.HRM_PLAN.Find(PlanID);
            if (obj == null)
            {
                return HttpNotFound();
            }
            ViewBag.Plan = obj;
            var list = db.sp_LayKeHoachDieuDong(PlanID);
            return View(list);
        }
        // GET: HRM_DETAILPLAN/Details/5
        [Authorize(Roles = "EduCenter")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_DETAILPLAN hRM_DETAILPLAN = db.HRM_DETAILPLAN.Find(id);
            if (hRM_DETAILPLAN == null)
            {
                return HttpNotFound();
            }
            return View(hRM_DETAILPLAN);
        }

        // GET: HRM_DETAILPLAN/Create
        [Authorize(Roles = "EduCenter")]
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            ViewBag.CrewOffDepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName");
            ViewBag.EducationID = new SelectList(db.DIC_EDUCATION, "EducationID", "EducationName");
            ViewBag.CrewOnID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");
            ViewBag.CrewOnID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");
            ViewBag.CrewOffHistoryID = new SelectList(db.HRM_EMPLOYMENTHISTORY, "EmploymentHistoryID", "DecisionNo");
            ViewBag.CrewOnID = new SelectList(db.HRM_EMPLOYMENTHISTORY, "EmploymentHistoryID", "DecisionNo");
            ViewBag.PlanID = new SelectList(db.HRM_PLAN, "PlanID", "PlanName");
            ViewBag.OffPositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName");
            ViewBag.OnPositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName");
            ViewBag.OffPluralityID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName");
            ViewBag.OnPluralityID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName");
            return View();
        }

        // POST: HRM_DETAILPLAN/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "EduCenter")]
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DetailPlanID,PlanID,CrewOffID,CrewOffDepartmentID,CrewOffPosition,DateOff,TimeOff,CrewOffHistoryID,CrewOnID,CrewOnPosition,DateOn,TimeOn,CrewOnHistoryID,EducationID,CrewOnHistory,Note,OffPositionID,OffInternshipPosition,OffPluralityID,OffInternshipPlurality,OnPositionID,OnInternshipPosition,OnPluralityID,OnInternshipPlurality")] HRM_DETAILPLAN hRM_DETAILPLAN)
        {
            if (ModelState.IsValid)
            {
                db.HRM_DETAILPLAN.Add(hRM_DETAILPLAN);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CrewOffDepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", hRM_DETAILPLAN.CrewOffDepartmentID);
            ViewBag.EducationID = new SelectList(db.DIC_EDUCATION, "EducationID", "EducationName", hRM_DETAILPLAN.EducationID);
            ViewBag.CrewOnID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_DETAILPLAN.CrewOnID);
            ViewBag.CrewOnID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_DETAILPLAN.CrewOnID);
            ViewBag.CrewOffHistoryID = new SelectList(db.HRM_EMPLOYMENTHISTORY, "EmploymentHistoryID", "DecisionNo", hRM_DETAILPLAN.CrewOffHistoryID);
            ViewBag.CrewOnID = new SelectList(db.HRM_EMPLOYMENTHISTORY, "EmploymentHistoryID", "DecisionNo", hRM_DETAILPLAN.CrewOnID);
            ViewBag.PlanID = new SelectList(db.HRM_PLAN, "PlanID", "PlanName", hRM_DETAILPLAN.PlanID);
            ViewBag.OffPositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_DETAILPLAN.OffPositionID);
            ViewBag.OnPositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_DETAILPLAN.OnPositionID);
            ViewBag.OffPluralityID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_DETAILPLAN.OffPluralityID);
            ViewBag.OnPluralityID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_DETAILPLAN.OnPluralityID);
            return View(hRM_DETAILPLAN);
        }
        [Authorize(Roles = "Boss")]
        public ActionResult GiamDocDuyet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_DETAILPLAN hRM_DETAILPLAN = db.HRM_DETAILPLAN.Find(id);
            if (hRM_DETAILPLAN == null)
            {
                return HttpNotFound();
            }
            if (hRM_DETAILPLAN.CrewOffID != 0)
            {
                ViewBag.thuyenVien = db.HRM_EMPLOYEE.Find(hRM_DETAILPLAN.CrewOffID);
            }
            else
            {
                ViewBag.thuyenVien = null;
            }
            if (hRM_DETAILPLAN.CrewOnID != 0)
            {
                ViewBag.duTru = db.HRM_EMPLOYEE.Find(hRM_DETAILPLAN.CrewOnID);
            }
            else
            {
                ViewBag.duTru = null;
            }
            return PartialView(hRM_DETAILPLAN);
        }

        // POST: HRM_PLAN/Delete/5
        [Authorize(Roles = "Boss")]
        [HttpPost, ActionName("GiamDocDuyet")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanDuyet(int DetailPlanID, string Note)
        {
            HRM_DETAILPLAN item = db.HRM_DETAILPLAN.Find(DetailPlanID);
            item.DaDuyet = true;
            item.Note = Note;
            db.Entry(item).State = EntityState.Modified;            
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
            HRM_DETAILPLAN item = db.HRM_DETAILPLAN.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            item.DaDuyet = false;
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("PlanViewGD", new { PlanID =item.PlanID });
        }
        // GET: HRM_DETAILPLAN/Edit/5
        [Authorize(Roles = "EduCenter, Boss")]
        [Authorize(Roles = "Create")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_DETAILPLAN hRM_DETAILPLAN = db.HRM_DETAILPLAN.Find(id);
            if (hRM_DETAILPLAN == null)
            {
                return HttpNotFound();
            }
            if (hRM_DETAILPLAN.CrewOffID != 0)
            {
                ViewBag.thuyenVien = db.HRM_EMPLOYEE.Find(hRM_DETAILPLAN.CrewOffID);
            }
            else
            {
                ViewBag.thuyenVien = null;
            }
            if (hRM_DETAILPLAN.CrewOnID != 0)
            {
                ViewBag.duTru = db.HRM_EMPLOYEE.Find(hRM_DETAILPLAN.CrewOnID);
            }
            else
            {
                ViewBag.duTru = null;
            }
            return PartialView(hRM_DETAILPLAN);
        }

        // POST: HRM_DETAILPLAN/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "EduCenter, Boss")]
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int DetailPlanID, string Note)
        {
            HRM_DETAILPLAN hRM_DETAILPLAN = db.HRM_DETAILPLAN.Find(DetailPlanID);
            if (ModelState.IsValid)            {
                
                db.Entry(hRM_DETAILPLAN).State = EntityState.Modified;
                hRM_DETAILPLAN.Note = Note;
                db.SaveChanges();
                return Json(new { success = true},JsonRequestBehavior.AllowGet);
            }
            if (hRM_DETAILPLAN.CrewOffID != 0)
            {
                ViewBag.thuyenVien = db.HRM_EMPLOYEE.Find(hRM_DETAILPLAN.CrewOffID);
            }
            else
            {
                ViewBag.thuyenVien = null;
            }
            if (hRM_DETAILPLAN.CrewOnID != 0)
            {
                ViewBag.duTru = db.HRM_EMPLOYEE.Find(hRM_DETAILPLAN.CrewOnID);
            }
            else
            {
                ViewBag.duTru = null;
            }
            return PartialView(hRM_DETAILPLAN);
        }

        // GET: HRM_DETAILPLAN/Delete/5
        [Authorize(Roles = "EduCenter")]
        [Authorize(Roles = "Create")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_DETAILPLAN hRM_DETAILPLAN = db.HRM_DETAILPLAN.Find(id);
            if (hRM_DETAILPLAN == null)
            {
                return HttpNotFound();
            }
            if (hRM_DETAILPLAN.CrewOffID != 0)
            {
                ViewBag.thuyenVien = db.HRM_EMPLOYEE.Find(hRM_DETAILPLAN.CrewOffID);
            }
            else
            {
                ViewBag.thuyenVien = null;
            }
            if (hRM_DETAILPLAN.CrewOnID != 0)
            {
                ViewBag.duTru = db.HRM_EMPLOYEE.Find(hRM_DETAILPLAN.CrewOnID);
            }
            else
            {
                ViewBag.duTru = null;
            }
            return PartialView(hRM_DETAILPLAN);
        }

        // POST: HRM_DETAILPLAN/Delete/5
        [Authorize(Roles = "EduCenter")]
        [Authorize(Roles = "Create")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HRM_DETAILPLAN hRM_DETAILPLAN = db.HRM_DETAILPLAN.Find(id);
            int idPlan = hRM_DETAILPLAN.PlanID.Value;
            db.HRM_DETAILPLAN.Remove(hRM_DETAILPLAN);
            db.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("ChiTietKeHoach", "HRM_PLAN" ,new { id = idPlan });
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
