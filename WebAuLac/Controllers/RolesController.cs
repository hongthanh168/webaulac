using System;
using System.Linq;
using System.Web.Mvc;
using WebAuLac.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Data.Entity;

namespace WebAuLac.Controllers
{
    [Authorize]
	public class RolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
  //      ApplicationDbContext context;

		//public RoleController()
		//{
		//	context = new ApplicationDbContext();
		//}

		/// <summary>
		/// Get All Roles
		/// </summary>
		/// <returns></returns>
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

			var Roles = db.Roles.ToList();
			return View(Roles);

		}
		public Boolean isAdminUser()
		{
			if (User.Identity.IsAuthenticated)
			{
				var user = User.Identity;
				var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var s = UserManager.GetRoles(user.GetUserId());//.OrderBy();

                for(int i=0; i < s.Count; i++)
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

			var Role = new IdentityRole();
			return View(Role);
		}

		/// <summary>
		/// Create a New Role
		/// </summary>
		/// <param name="Role"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Create(IdentityRole Role)
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

            db.Roles.Add(Role);
            db.SaveChanges();
			return RedirectToAction("Index");
		}

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id) )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdentityRole role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);

            //// It's actually the Role.Name tucked into the id param:
            //var role = db.Roles.First(r => r.Name == id);
            //var roleModel = new EditRoleViewModel(role);
            //return View(roleModel);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(IdentityRole role)
            //[Bind(Include ="RoleName,OriginalRoleName,Description")] EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(role).State = EntityState.Modified;
                //var role = db.Roles.First(r => r.Name == model.OriginalRoleName);
                //role.Name = model.RoleName;
                //role.Description = model.Description;
                //db.Entry(role).State = EntityState.Modified;
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
            IdentityRole role = db.Roles.Find(id);
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
            IdentityRole role = db.Roles.Find(id);
            db.Roles.Remove(role);
            db.SaveChanges();
            //var role = db.Roles.First(r => r.Name == id);
            //var idManager = new IdentityManager();
            //idManager.DeleteRole(role.Id);
            return RedirectToAction("Index");
        }

    }
}