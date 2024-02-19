using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAuLac.Models;

namespace WebAuLac.Controllers
{
    public class HRM_ROLEController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: HRM_ROLE
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {


                if (!isAdminUser())
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


            var HRMRoles = db.HRM_ROLE.ToList();
            return View(HRMRoles);
        }

        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var s = UserManager.GetRoles(user.GetUserId());//.OrderBy();

                for (int i = 0; i < s.Count; i++)
                {
                    if (s[i].ToString() == "Admin")
                    {
                        return true;
                    }
                }
                return false;
            }
            return false;
        }
        /// <summary>
        /// Create  a New role
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    if (!isAdminUser())
            //    {
            //        return RedirectToAction("Index", "Home");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            ViewBag.RoleID = new SelectList(db.Roles, "Name", "Name");
            return View();
        }

        /// <summary>
        /// Create a New Role
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(HRM_ROLE Role)
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    if (!isAdminUser())
            //    {
            //        return RedirectToAction("Index", "Home");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            db.HRM_ROLE.Add(Role);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_ROLE role = db.HRM_ROLE.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }

            //ViewBag.RoleID = new SelectList(db.Roles, "Name", "Name");
            return View(role);

        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(HRM_ROLE role)
        //[Bind(Include ="RoleName,OriginalRoleName,Description")] EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {

  
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);  //model);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRM_ROLE role = db.HRM_ROLE.Find(id);
            //var role = db.Roles.First(r => r.Name == id);
            //var model = new RoleViewModel(role);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);  //model);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            HRM_ROLE role = db.HRM_ROLE.Find(id);
            db.HRM_ROLE.Remove(role);
            db.SaveChanges();
            //var role = db.Roles.First(r => r.Name == id);
            //var idManager = new IdentityManager();
            //idManager.DeleteRole(role.Id);
            return RedirectToAction("Index");
        }

    }
}