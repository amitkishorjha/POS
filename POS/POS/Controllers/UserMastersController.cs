using POS.Common;
using POS.Models;
using POS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace POS.Controllers
{
    public class UserMastersController : Controller
    {
        POSContext context = new POSContext();

        [SimpleAuthorizeAttribute]
        [SessionExpire]
        // GET: UserMasters
        public ActionResult Index()
        {
            return View(context.UserMaster.ToList());
        }

        [SimpleAuthorizeAttribute]
        [SessionExpire]
        // GET: UserMasters/Details/5
        public ActionResult Details(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMaster userMaster = context.UserMaster.Where(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (userMaster == null)
            {
                return HttpNotFound();
            }
            return View(userMaster);
        }

        [SimpleAuthorizeAttribute]
        [SessionExpire]
        // GET: UserMasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [SimpleAuthorizeAttribute]
        [SessionExpire]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserMaster userMaster)
        {
            try
            {
                userMaster.CreatedBy = User.Identity.Name;
                userMaster.CreatedDate = DateTime.Now;
                userMaster.Username = userMaster.FirstName + userMaster.MiddleName + userMaster.LastName;
                context.UserMaster.Add(userMaster);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(userMaster);
            }
        }

        [SimpleAuthorizeAttribute]
        [SessionExpire]
        // GET: UserMasters/Edit/5
        public ActionResult Edit(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMaster userMaster = context.UserMaster.Where(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (userMaster == null)
            {
                return HttpNotFound();
            }
            return View(userMaster);
        }

        // POST: UserMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [SimpleAuthorizeAttribute]
        [SessionExpire]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserMaster userMaster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    userMaster.UpdatedBy = User.Identity.Name;
                    userMaster.UpdatedDate = DateTime.Now;
                    userMaster.Username = userMaster.FirstName + userMaster.MiddleName + userMaster.LastName;
                    context.UserMaster.Attach(userMaster);
                    context.Entry(userMaster).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

            }
            return View(userMaster);
        }

        // GET: UserMasters/Delete/5
        [SimpleAuthorizeAttribute]
        [SessionExpire]
        public ActionResult Delete(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMaster userMaster = context.UserMaster.Where(x => x.UniqueId == UniqueId).FirstOrDefault();
            if (userMaster == null)
            {
                return HttpNotFound();
            }
            return View(userMaster);
        }

        // POST: UserMasters/Delete/5
        [SimpleAuthorizeAttribute]
        [SessionExpire]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid UniqueId)
        {
            UserMaster userMaster = context.UserMaster.Where(x => x.UniqueId == UniqueId).FirstOrDefault();
            userMaster.DeletedBy = User.Identity.Name;
            userMaster.DeletedDate = DateTime.Now;
            context.UserMaster.Remove(userMaster);
            context.SaveChanges();
            return RedirectToAction("Index");
        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(UserMaster _user, string returnUrl)
        {
            UserMaster user = context.UserMaster.Where(us => us.Username == _user.Username && us.IsActive == true).FirstOrDefault();

            if (user != null)
            {
                if (user.Password == _user.Password)
                {
                    FormsAuthentication.SetAuthCookie(_user.Username, true);
                    Session["user"] = user;

                    user.LastLoginDate = DateTime.Now.ToString("dd-MM-yyyy hh:mm");
                    user.UpdatedBy = _user.Username;
                    user.UpdatedDate = DateTime.Now;
                    context.UserMaster.Attach(user);
                    context.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("GetUserRoles", "UserMasters");
                    }

                }
                else
                    ModelState.AddModelError(string.Empty, "please enter correct details");
            }

            ModelState.AddModelError(string.Empty, "User not exist");
            return View();
        }

        [HttpGet]
        public ActionResult logOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");

        }

        [SimpleAuthorizeAttribute]
        [Authorize]
        [SessionExpire]
        public ActionResult UserRoleMapping(Guid UniqueId)
        {
            UserRoleMappings userRoleMapping = new UserRoleMappings();

            UserMaster userMaster = context.UserMaster.Where(x => x.UniqueId == UniqueId).FirstOrDefault();

            userRoleMapping.UserId = userMaster.UniqueId;
            userRoleMapping.UserName = userMaster.Username;

            userRoleMapping.Role = context.RoleMaster.ToList();

            userRoleMapping.UserRoleMapping = context.UserRoleMapping.Where(x => x.UserMasterId == UniqueId && x.IsActive == true).ToList();

            return View(userRoleMapping);

        }

        [SimpleAuthorizeAttribute]
        [SessionExpire]
        [HttpPost]
        public ActionResult UserRoleMapping(Guid[] RoleId, Guid UserId, Guid[] UnMap)
        {
            UserRoleMappings userRoleMappings = new UserRoleMappings();
            try
            {
                var userRoleMapp = context.UserRoleMapping.Where(x => x.UserMasterId == UserId && x.IsActive == true).ToList();

                if (RoleId != null)
                {
                    foreach (var item in RoleId)
                    {
                        if (userRoleMapp.Select(x => x.RoleMasterId).Contains(item) == false)
                        {
                            UserRoleMapping userRoleMapping = new UserRoleMapping();
                            userRoleMapping.UserMasterId = UserId;
                            userRoleMapping.RoleMasterId = item;
                            userRoleMapping.CreatedBy = User.Identity.Name;
                            userRoleMapping.CreatedDate = DateTime.Now;

                            context.UserRoleMapping.Add(userRoleMapping);
                            context.SaveChanges();
                        }
                    }
                }

                if (UnMap != null)
                {
                    foreach (var item in UnMap)
                    {
                        if (userRoleMapp.Select(x => x.RoleMasterId).Contains(item))
                        {
                            UserRoleMapping userRoleMapping = userRoleMapp.Where(x => x.RoleMasterId == item && x.UserMasterId == UserId).FirstOrDefault();
                            userRoleMapping.IsActive = false;
                            userRoleMapping.DeletedBy = User.Identity.Name;
                            userRoleMapping.DeletedDate = DateTime.Now;

                            context.UserRoleMapping.Remove(userRoleMapping);
                            context.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                UserMaster userMaster = context.UserMaster.Where(x => x.UniqueId == UserId).FirstOrDefault();

                userRoleMappings.UserId = userMaster.UniqueId;
                userRoleMappings.UserName = userMaster.Username;

                userRoleMappings.Role = context.RoleMaster.ToList();
            }

            return View(userRoleMappings);
        }

        [HttpGet]
        public ActionResult GetUserRoles()
        {
            var user = context.UserMaster.Where(x => x.Username == User.Identity.Name
                        && x.IsActive == true).FirstOrDefault();

            ViewBag.Roles = (from ur in context.UserRoleMapping.Where(x => x.UserMasterId == user.UniqueId)
                             join ro in context.RoleMaster on ur.RoleMasterId equals ro.UniqueId
                             select ro).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult GetUserRoles(Guid RoleId)
        {
            Session["RoleId"] = RoleId;
            return RedirectToAction("FirstPage");
        }


        [SessionExpire]
        public ActionResult FirstPage()
        {
            return View();
        }

        public ActionResult Unauthorized()
        {
            return View();
        }

    }
}