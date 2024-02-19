using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using WebAuLac.Models;

namespace WebAuLac.Controllers
{
    public class VideoFilesController : Controller
    {
        private AuLacEntities db = new AuLacEntities();

        // GET: VideoFiles
        public ActionResult Index()
        {
            return View(db.VideoFiles.ToList());
        }

        // GET: VideoFiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VideoFile videoFile = db.VideoFiles.Find(id);
            if (videoFile == null)
            {
                return HttpNotFound();
            }
            return PartialView(videoFile);
        }

        // GET: VideoFiles/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: VideoFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Stt,LichTau,Name,FileSize,FilePath")] VideoFile videoFile, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {

                }
                    db.VideoFiles.Add(videoFile);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            return PartialView(videoFile);
        }

        [WebMethod]        
        public JsonResult CreateWithUpload(VideoFile videoFile, HttpPostedFileBase upload)
        {
            // var img = Path.GetFileName(upload.FileName);
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(upload.FileName);
                    int fileSize = upload.ContentLength;
                    int Size = fileSize / 1000;
                    upload.SaveAs(Server.MapPath("~/VideoFileUpload/" + fileName));

                    //string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                    //using (SqlConnection con = new SqlConnection(CS))
                    //{
                    //    SqlCommand cmd = new SqlCommand("spAddNewVideoFile", con);
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    con.Open();
                    //    cmd.Parameters.AddWithValue("@Name", fileName);
                    //    cmd.Parameters.AddWithValue("@FileSize", Size);
                    //    cmd.Parameters.AddWithValue("FilePath", "~/VideoFileUpload/" + fileName);
                    //    cmd.ExecuteNonQuery();
                    //}

                    db.VideoFiles.Add(new VideoFile
                    {
                        Name = fileName,
                        FileSize = Size,
                        FilePath = "~/VideoFileUpload/" + fileName,
                        LichTau = videoFile.LichTau
                    });
                    db.SaveChanges();


                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        // GET: VideoFiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VideoFile videoFile = db.VideoFiles.Find(id);
            if (videoFile == null)
            {
                return HttpNotFound();
            }
            return View(videoFile);
        }

        // POST: VideoFiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Stt,LichTau,Name,FileSize,FilePath")] VideoFile videoFile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(videoFile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(videoFile);
        }

        // GET: VideoFiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VideoFile videoFile = db.VideoFiles.Find(id);
            if (videoFile == null)
            {
                return HttpNotFound();
            }
            return PartialView(videoFile);
        }

        // POST: VideoFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VideoFile videoFile = db.VideoFiles.Find(id);
            db.VideoFiles.Remove(videoFile);
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

        [HttpGet]
        public ActionResult UploadVideo()
        {
            List<VideoFile> videolist = new List<VideoFile>();            
            videolist = db.VideoFiles.ToList();
            //string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            //using (SqlConnection con = new SqlConnection(CS))
            //{
            //    SqlCommand cmd = new SqlCommand("spGetAllVideoFile", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    con.Open();
            //    SqlDataReader rdr = cmd.ExecuteReader();
            //    while (rdr.Read())
            //    {
            //        VideoFile video = new VideoFile();
            //        video.ID = Convert.ToInt32(rdr["ID"]);
            //        video.Name = rdr["Name"].ToString();
            //        video.FileSize = Convert.ToInt32(rdr["FileSize"]);
            //        video.FilePath = rdr["FilePath"].ToString();

            //        videolist.Add(video);
            //    }
            //}
            return View(videolist);
        }
        [HttpPost]
        public ActionResult UploadVideo(FormCollection form, HttpPostedFileBase fileupload)
        {
            string lichtau = form["LichTau"];
            if (fileupload != null)
            {
                string fileName = Path.GetFileName(fileupload.FileName);
                int fileSize = fileupload.ContentLength;
                int Size = fileSize / 1000;
                fileupload.SaveAs(Server.MapPath("~/VideoFileUpload/" + fileName));

                //string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                //using (SqlConnection con = new SqlConnection(CS))
                //{
                //    SqlCommand cmd = new SqlCommand("spAddNewVideoFile", con);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    con.Open();
                //    cmd.Parameters.AddWithValue("@Name", fileName);
                //    cmd.Parameters.AddWithValue("@FileSize", Size);
                //    cmd.Parameters.AddWithValue("FilePath", "~/VideoFileUpload/" + fileName);
                //    cmd.ExecuteNonQuery();
                //}
               
                db.VideoFiles.Add(new VideoFile
                {
                    Name = fileName,
                    FileSize = Size,
                    FilePath = "~/VideoFileUpload/" + fileName,
                    LichTau = lichtau
                });
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult viewVideo(int id)
        {
            var item = db.VideoFiles.Find(id);
            ViewBag.filePath = item.FilePath;
            return View();
        }


    }
}
