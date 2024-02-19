using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using WebAuLac.Models;

namespace WebAuLac.Controllers
{
    public static class ExpandoHelper
    {
        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            IDictionary<string, object> anonymousDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(anonymousObject);
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var item in anonymousDictionary)
                expando.Add(item);
            return (ExpandoObject)expando;
        }

    }

    [Authorize(Roles = "Boss, HR")]
    public class HRM_EMPLOYMENTHISTORY_GOCController : Controller
    {

        private AuLacEntities db = new AuLacEntities();


        // GET: HRM_EMPLOYMENTHISTORY_GOC
        public ActionResult Index()
        {
            var listID = db.HRM_QTCT.Select(h => h.EmploymentHistoryID).ToList();
            // var hRM_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Include(h => h.DIC_CATEGORYDECISION).Include(h => h.DIC_DEPARTMENT).Include(h => h.DIC_DEPARTMENT1).Include(h => h.DIC_INTERSHIP).Include(h => h.DIC_INTERSHIP1).Include(h => h.DIC_LOAITAU).Include(h => h.DIC_POSITION).Include(h => h.DIC_POSITION1).Include(h => h.DIC_POSITION2).Include(h => h.DIC_SALARY).Include(h => h.DIC_SALARY1).Include(h => h.HRM_EMPLOYEE).Include(h => h.tbl_LyDoNghiViec);
            var qtct = (from a in db.HRM_EMPLOYMENTHISTORY
                        where a.DecisionDate.Value.Year == DateTime.Now.Year && a.DecisionDate.Value.Month==DateTime.Now.Month && a.XacNhan==true
                        orderby a.DecisionDate descending
                        select new
                        {
                            EmploymentHistoryID = a.EmploymentHistoryID,
                            FirstName = a.HRM_EMPLOYEE.FirstName,
                            LastName = a.HRM_EMPLOYEE.LastName,
                            DecisionNo = a.DecisionNo,
                            DecisionDate = a.DecisionDate,
                            NgayXuongTau = a.NgayXuongTau,
                            DepartmentName = a.DIC_DEPARTMENT.DepartmentName,
                            PositionName = a.DIC_POSITION.PositionName,
                            PerPosition = a.PerPosition??0,
                            Salary = a.Salary??0,
                            AllowanceSalary = a.AllowanceSalary??0,
                            Bonus = a.Bonus??0,
                            AllowanceBonus = a.AllowanceBonus??0,
                            PerPlurality = a.PerPlurality??0,
                            SalaryPlurality = a.SalaryPlurality??0,
                            AllowanceSalaryPlurality = a.AllowanceSalaryPlurality??0,
                            BonusPlurality = a.BonusPlurality??0,
                            AllowanceBonusPlurality = a.AllowanceBonusPlurality??0,
                            TH = !(listID.Contains(a.EmploymentHistoryID))
                        }).AsEnumerable()
                       .Select(x => x.ToExpando());


            

            var years = Enumerable
    .Range(DateTime.Now.Year - 5, 15)
    .Select(year => new SelectListItem
    {
        Value = year.ToString(CultureInfo.InvariantCulture),
        Text = year.ToString(CultureInfo.InvariantCulture)
    });
            var months = DateTimeFormatInfo
  .InvariantInfo
  .MonthNames
  .TakeWhile(monthName => monthName != String.Empty)
  .Select((monthName, index) => new SelectListItem
  {
      Value = (index + 1).ToString(CultureInfo.InvariantCulture),
      Text = string.Format("({0}) {1}", index + 1, monthName)
  });
            ViewBag.Thang = months;
            ViewBag.Nam = years;
            return View(qtct);
        }
        //[Authorize(Roles = "Quan tri")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int Thang, int Nam, string answer)
        {
            if (ModelState.IsValid)
            {
                if (answer == "layluongcoban")
                {
                    int id = 0;
                    id = (from p in db.HRM_SALARY
                          where p.Years == Nam && p.Months == Thang
                          select p.HRMSalaryID).SingleOrDefault();

                    db.sp_LayTienLuong(id);
                }

                //var result = db.HRM_EMPLOYMENTHISTORY.Include(h => h.DIC_CATEGORYDECISION).Include(h => h.DIC_DEPARTMENT).Include(h => h.DIC_DEPARTMENT1).Include(h => h.DIC_INTERSHIP).Include(h => h.DIC_INTERSHIP1).Include(h => h.DIC_LOAITAU).Include(h => h.DIC_POSITION).Include(h => h.DIC_POSITION1).Include(h => h.DIC_POSITION2).Include(h => h.DIC_SALARY).Include(h => h.DIC_SALARY1).Include(h => h.HRM_EMPLOYEE).Include(h => h.tbl_LyDoNghiViec).Where(h=>h.DecisionDate.Value.Month == Thang&& h.DecisionDate.Value.Year==Nam).ToList();

                var listID = db.HRM_QTCT.Select(h => h.EmploymentHistoryID).ToList();
                // var hRM_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Include(h => h.DIC_CATEGORYDECISION).Include(h => h.DIC_DEPARTMENT).Include(h => h.DIC_DEPARTMENT1).Include(h => h.DIC_INTERSHIP).Include(h => h.DIC_INTERSHIP1).Include(h => h.DIC_LOAITAU).Include(h => h.DIC_POSITION).Include(h => h.DIC_POSITION1).Include(h => h.DIC_POSITION2).Include(h => h.DIC_SALARY).Include(h => h.DIC_SALARY1).Include(h => h.HRM_EMPLOYEE).Include(h => h.tbl_LyDoNghiViec);
                var qtct = (from a in db.HRM_EMPLOYMENTHISTORY
                            where a.DecisionDate.Value.Year == Nam && a.DecisionDate.Value.Month == Thang && a.XacNhan == true
                            orderby a.DecisionDate descending
                            select new
                            {
                                EmploymentHistoryID = a.EmploymentHistoryID,
                                FirstName = a.HRM_EMPLOYEE.FirstName,
                                LastName = a.HRM_EMPLOYEE.LastName,
                                DecisionNo = a.DecisionNo,
                                DecisionDate = a.DecisionDate,
                                NgayXuongTau = a.NgayXuongTau,
                                DepartmentName = a.DIC_DEPARTMENT.DepartmentName,
                                PositionName = a.DIC_POSITION.PositionName,
                                PerPosition = a.PerPosition??0,
                                Salary = a.Salary ?? 0,
                                AllowanceSalary = a.AllowanceSalary ?? 0,
                                Bonus = a.Bonus ?? 0,
                                AllowanceBonus = a.AllowanceBonus ?? 0,
                                PerPlurality = a.PerPlurality ?? 0,
                                SalaryPlurality = a.SalaryPlurality ?? 0,
                                AllowanceSalaryPlurality = a.AllowanceSalaryPlurality ?? 0,
                                BonusPlurality = a.BonusPlurality ?? 0,
                                AllowanceBonusPlurality = a.AllowanceBonusPlurality ?? 0,
                                TH = !(listID.Contains(a.EmploymentHistoryID))
                            }).AsEnumerable()
                           .Select(x => x.ToExpando());


                // ViewBag.BangKe = result;
                var years = Enumerable
.Range(DateTime.Now.Year - 5, 15)
.Select(year => new SelectListItem
{
    Selected = (year == DateTime.Now.Year),
    Value = year.ToString(CultureInfo.InvariantCulture),
Text = year.ToString(CultureInfo.InvariantCulture)
});
                var months = DateTimeFormatInfo
      .InvariantInfo
      .MonthNames
      .TakeWhile(monthName => monthName != String.Empty)
      .Select((monthName, index) => new SelectListItem
      {
          Selected = (index == DateTime.Now.Month),
          Value = (index + 1).ToString(CultureInfo.InvariantCulture),
          Text = string.Format("({0}) {1}", index + 1, monthName)
      });

                ViewBag.Thang = months;
                ViewBag.Nam = years;

                return View(qtct);
                    


            }
            return View();
        }

        public ActionResult LoadTienLuong(int Thang, int Nam)
        {
            //db.HRM_DETAILSALARY.RemoveRange(db.HRM_DETAILSALARY.Where(x => x.HRMSalaryID == id));
            //db.SaveChanges();

            //Update tiền lương
            int id = 0;
            id = (from p in db.HRM_SALARY
                  where p.Years == Nam && p.Months == Thang
                  select p.HRMSalaryID).SingleOrDefault();

            db.sp_LayTienLuong(id);
            return RedirectToAction("Index", new { Thang = Thang, Nam = Nam });
        }


        [WebMethod]
        public JsonResult LuuQTCT(string[] function_param)
        {
            //Lấy danh sách và luu lại thông tin 
            foreach (string item in function_param)
            {
                //Lấy thông tin nếu đã có lưu thì xóa đi lưu lại
                double id = Convert.ToDouble(item);
                try
                {
                    HRM_QTCT hRM_QTCT = db.HRM_QTCT.Where(h => h.EmploymentHistoryID == id).First(); //db.HRM_QTCT.Find(Convert.ToInt32(item));
                    if (hRM_QTCT != null)
                    {
                        db.HRM_QTCT.Remove(hRM_QTCT);
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {

                    
                }
             
               
              

                //Tạo cái mới
                var qtct_goc = db.HRM_EMPLOYMENTHISTORY.Find(Convert.ToInt32(item));
                HRM_QTCT qtct = new HRM_QTCT();
                qtct.DecisionDate = qtct_goc.DecisionDate;
                qtct.EffectiveDate = qtct_goc.EffectiveDate;

                qtct.ContentDecision= qtct_goc.ContentDecision;
                qtct.CategoryDecisionID = qtct_goc.CategoryDecisionID;
                qtct.EmployeeID= qtct_goc.EmployeeID;
                qtct.DepartmentID= qtct_goc.DepartmentID;
                qtct.PositionID=qtct_goc.PositionID;
                qtct.InternshipPosition =qtct_goc.InternshipPosition;
                qtct.PluralityID= qtct_goc.PluralityID;
                qtct.IntershipPlurality= qtct_goc.IntershipPlurality;
                qtct.Note = qtct_goc.Note;
                qtct.PerPosition = qtct_goc.PerPosition;
                qtct.PerPlurality = qtct_goc.PerPlurality;
                qtct.SalaryPositionID=  qtct_goc.SalaryPositionID;
                qtct.SalaryPluralityID = qtct_goc.SalaryPluralityID;
                qtct.DepartmentName = qtct_goc.DepartmentName;
                qtct.LoaiTauID = qtct_goc.LoaiTauID;
                qtct.Gross= qtct_goc.Gross;
                qtct.Power = qtct_goc.Power;
                qtct.NgayXuongTau = qtct_goc.NgayXuongTau;
                qtct.XacNhan = qtct_goc.XacNhan;
                qtct.DecisionNo = qtct_goc.DecisionNo;
                qtct.DepartmentPluralityID = qtct_goc.DepartmentPluralityID;
                qtct.LyDoNghiViec_ID = qtct_goc.LyDoNghiViec_ID;
                qtct.Salary = qtct_goc.Salary;
                qtct.AllowanceSalary= qtct_goc.AllowanceSalary;
                qtct.Bonus = qtct_goc.Bonus;
                qtct.AllowanceBonus =qtct_goc.AllowanceBonus;
                qtct.SalaryPlurality = qtct_goc.SalaryPlurality;
                qtct.AllowanceSalaryPlurality = qtct_goc.AllowanceSalaryPlurality;
                qtct.BonusPlurality = qtct_goc.BonusPlurality;
                qtct.AllowanceBonusPlurality= qtct_goc.AllowanceBonusPlurality;
                qtct.ThoiGianThucTap = qtct_goc.ThoiGianThucTap;
                qtct.EmploymentHistoryID = qtct_goc.EmploymentHistoryID;
                db.HRM_QTCT.Add(qtct);
                db.SaveChanges();

            }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);

            //return new JsonResult { Data = 1 };
        }
        [WebMethod]
        public JsonResult HuyQTCT(string[] function_param)
        {
            //Lấy danh sách và luu lại thông tin 
            foreach (string item in function_param)
            {
                try
                {
                    double id = Convert.ToDouble(item);
                    HRM_QTCT hRM_QTCT = db.HRM_QTCT.Where(h => h.EmploymentHistoryID == id).First(); //db.HRM_QTCT.Find(Convert.ToInt32(item));
                    db.HRM_QTCT.Remove(hRM_QTCT);
                    db.SaveChanges();
                }
                catch (Exception)
                {

                   
                }
               

            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

            //return new JsonResult { Data = 1 };
        }
        // GET: HRM_EMPLOYMENTHISTORY_GOC/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Find(id);
            if (hRM_EMPLOYMENTHISTORY == null)
            {
                return HttpNotFound();
            }
            return View(hRM_EMPLOYMENTHISTORY);
        }

        // GET: HRM_EMPLOYMENTHISTORY_GOC/Create
        public ActionResult Create()
        {
            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName");
            ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName");
            ViewBag.DepartmentPluralityID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName");
            ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");
            ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName");
            ViewBag.LoaiTauID = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau");
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName");
            ViewBag.PluralityID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName");
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName");
            ViewBag.SalaryPositionID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.SalaryPluralityID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID");
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode");
            ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name");
            return View();
        }

        // POST: HRM_EMPLOYMENTHISTORY_GOC/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmploymentHistoryID,DecisionDate,EffectiveDate,ContentDecision,CategoryDecisionID,EmployeeID,DepartmentID,PositionID,InternshipPosition,PluralityID,IntershipPlurality,Note,PerPosition,PerPlurality,SalaryPositionID,SalaryPluralityID,DepartmentName,LoaiTauID,Gross,Power,NgayXuongTau,XacNhan,DecisionNo,DepartmentPluralityID,LyDoNghiViec_ID,Salary,AllowanceSalary,Bonus,AllowanceBonus,SalaryPlurality,AllowanceSalaryPlurality,BonusPlurality,AllowanceBonusPlurality,ThoiGianThucTap")] HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY)
        {
            if (ModelState.IsValid)
            {
                db.HRM_EMPLOYMENTHISTORY.Add(hRM_EMPLOYMENTHISTORY);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", hRM_EMPLOYMENTHISTORY.CategoryDecisionID);
            ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentID);
            ViewBag.DepartmentPluralityID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentPluralityID);
            ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName", hRM_EMPLOYMENTHISTORY.InternshipPosition);
            ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName", hRM_EMPLOYMENTHISTORY.IntershipPlurality);
            ViewBag.LoaiTauID = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau", hRM_EMPLOYMENTHISTORY.LoaiTauID);
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            ViewBag.PluralityID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PluralityID);
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            ViewBag.SalaryPositionID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID", hRM_EMPLOYMENTHISTORY.SalaryPositionID);
            ViewBag.SalaryPluralityID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID", hRM_EMPLOYMENTHISTORY.SalaryPluralityID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYMENTHISTORY.EmployeeID);
            ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name", hRM_EMPLOYMENTHISTORY.LyDoNghiViec_ID);
            return View(hRM_EMPLOYMENTHISTORY);
        }

        // GET: HRM_EMPLOYMENTHISTORY_GOC/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Find(id);
            if (hRM_EMPLOYMENTHISTORY == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", hRM_EMPLOYMENTHISTORY.CategoryDecisionID);
            ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentID);
            ViewBag.DepartmentPluralityID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentPluralityID);
            ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName", hRM_EMPLOYMENTHISTORY.InternshipPosition);
            ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName", hRM_EMPLOYMENTHISTORY.IntershipPlurality);
            ViewBag.LoaiTauID = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau", hRM_EMPLOYMENTHISTORY.LoaiTauID);
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            ViewBag.PluralityID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PluralityID);
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            ViewBag.SalaryPositionID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID", hRM_EMPLOYMENTHISTORY.SalaryPositionID);
            ViewBag.SalaryPluralityID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID", hRM_EMPLOYMENTHISTORY.SalaryPluralityID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYMENTHISTORY.EmployeeID);
            ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name", hRM_EMPLOYMENTHISTORY.LyDoNghiViec_ID);
            return View(hRM_EMPLOYMENTHISTORY);
        }

        // POST: HRM_EMPLOYMENTHISTORY_GOC/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmploymentHistoryID,DecisionDate,EffectiveDate,ContentDecision,CategoryDecisionID,EmployeeID,DepartmentID,PositionID,InternshipPosition,PluralityID,IntershipPlurality,Note,PerPosition,PerPlurality,SalaryPositionID,SalaryPluralityID,DepartmentName,LoaiTauID,Gross,Power,NgayXuongTau,XacNhan,DecisionNo,DepartmentPluralityID,LyDoNghiViec_ID,Salary,AllowanceSalary,Bonus,AllowanceBonus,SalaryPlurality,AllowanceSalaryPlurality,BonusPlurality,AllowanceBonusPlurality,ThoiGianThucTap")] HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRM_EMPLOYMENTHISTORY).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryDecisionID = new SelectList(db.DIC_CATEGORYDECISION, "CategoryDecisionID", "CategoryDecisionName", hRM_EMPLOYMENTHISTORY.CategoryDecisionID);
            ViewBag.DepartmentID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentID);
            ViewBag.DepartmentPluralityID = new SelectList(db.DIC_DEPARTMENT, "DepartmentID", "DepartmentName", hRM_EMPLOYMENTHISTORY.DepartmentPluralityID);
            ViewBag.InternshipPosition = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName", hRM_EMPLOYMENTHISTORY.InternshipPosition);
            ViewBag.IntershipPlurality = new SelectList(db.DIC_INTERSHIP, "IntershipID", "IntershipName", hRM_EMPLOYMENTHISTORY.IntershipPlurality);
            ViewBag.LoaiTauID = new SelectList(db.DIC_LOAITAU, "LoaiTauID", "TenLoaiTau", hRM_EMPLOYMENTHISTORY.LoaiTauID);
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            ViewBag.PluralityID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PluralityID);
            ViewBag.PositionID = new SelectList(db.DIC_POSITION, "PositionID", "PositionName", hRM_EMPLOYMENTHISTORY.PositionID);
            ViewBag.SalaryPositionID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID", hRM_EMPLOYMENTHISTORY.SalaryPositionID);
            ViewBag.SalaryPluralityID = new SelectList(db.DIC_SALARY, "SalaryID", "SalaryID", hRM_EMPLOYMENTHISTORY.SalaryPluralityID);
            ViewBag.EmployeeID = new SelectList(db.HRM_EMPLOYEE, "EmployeeID", "EmployeeCode", hRM_EMPLOYMENTHISTORY.EmployeeID);
            ViewBag.LyDoNghiViec_ID = new SelectList(db.tbl_LyDoNghiViec, "LyDoNghiViec_ID", "LyDoNghiViec_Name", hRM_EMPLOYMENTHISTORY.LyDoNghiViec_ID);
            return View(hRM_EMPLOYMENTHISTORY);
        }

        // GET: HRM_EMPLOYMENTHISTORY_GOC/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Find(id);
            if (hRM_EMPLOYMENTHISTORY == null)
            {
                return HttpNotFound();
            }
            return View(hRM_EMPLOYMENTHISTORY);
        }

        // POST: HRM_EMPLOYMENTHISTORY_GOC/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HRM_EMPLOYMENTHISTORY hRM_EMPLOYMENTHISTORY = db.HRM_EMPLOYMENTHISTORY.Find(id);
            db.HRM_EMPLOYMENTHISTORY.Remove(hRM_EMPLOYMENTHISTORY);
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
