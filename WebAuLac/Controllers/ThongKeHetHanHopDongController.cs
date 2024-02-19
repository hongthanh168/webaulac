using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAuLac.Models;

namespace WebAuLac.Controllers
{
    public class ThongKeHetHanHopDongController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: QuyetDinhKyLuat
        public ActionResult Index()
        {
            //TuNgayDenNgay obj = new TuNgayDenNgay();
            //obj.TuNgay = new DateTime(DateTime.Now.Year, 1, 1);
            //obj.DenNgay = new DateTime(DateTime.Now.Year, 12, 31);
            var result = db.sp_LayDSKhongDatHopDong().ToList();
            ViewBag.BangKe = result;
            return View();
        }
        [Authorize(Roles = "HR, Boss")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Index()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = db.sp_LayDSKhongDatHopDong().ToList();
        //        ViewBag.BangKe = result;
        //        return View();
        //    }
        //    return View();
        //}

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
