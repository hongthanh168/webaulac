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
    public class ThongKeDSThanNhanController : Controller
    {
        private AuLacEntities db = new AuLacEntities();
       
        public ActionResult Index()
        {            
            var result = db.sp_LayDSThanNhan().ToList();
            ViewBag.viewDSThanNhan = result;
            return View();
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
