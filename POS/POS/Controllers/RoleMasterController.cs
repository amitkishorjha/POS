﻿using POS.Common;
using POS.Models;
using POS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POS.Controllers
{
    [SimpleAuthorizeAttribute]
    [SessionExpire]

    public class RoleMasterController : Controller
    {
        POSContext context = new POSContext();

        // GET: RoleMaster
        public ActionResult Index()
        {
            var roleList = context.RoleMaster.ToList();
            return View(roleList);
        }

        // GET: ActionMaster/Details/5
        public ActionResult Details(Guid UniqueId)
        {
            var rolemaster = context.RoleMaster.Where(x => x.UniqueId == UniqueId).FirstOrDefault();
            return View(rolemaster);
        }

        // GET: ActionMaster/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ActionMaster/Create
        [HttpPost]
        public ActionResult Create(RoleMaster roleMaster)
        {
            try
            {
                // TODO: Add insert logic here
                roleMaster.CreatedBy = User.Identity.Name;
                roleMaster.CreatedDate = DateTime.Now;
                roleMaster.IsActive = true;
                context.RoleMaster.Add(roleMaster);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(roleMaster);
            }
        }

        // GET: ActionMaster/Edit/5
        public ActionResult Edit(Guid UniqueId)
        {
            var rolemaster = context.RoleMaster.Where(x => x.UniqueId == UniqueId).FirstOrDefault();
            return View(rolemaster);
        }

        // POST: ActionMaster/Edit/5
        [HttpPost]
        public ActionResult Edit(RoleMaster roleMaster)
        {
            try
            {
                // TODO: Add update logic here
                roleMaster.UpdatedBy = User.Identity.Name;
                roleMaster.UpdatedDate = DateTime.Now;
                context.RoleMaster.Attach(roleMaster);
                context.Entry(roleMaster).State = System.Data.Entity.EntityState.Modified;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(roleMaster);
            }
        }

        // GET: ActionMaster/Delete/5
        public ActionResult Delete(Guid UniqueId)
        {
            var rolemaster = context.RoleMaster.Where(x => x.UniqueId == UniqueId).FirstOrDefault();
            return View(rolemaster);
        }

        // POST: ActionMaster/Delete/5
        [HttpPost]
        public ActionResult Delete(Guid UniqueId, RoleMaster roleMaster)
        {
            var rolemaster = context.RoleMaster.Where(x => x.UniqueId == UniqueId).FirstOrDefault();

            try
            {

                rolemaster.DeletedBy = User.Identity.Name;
                rolemaster.DeletedDate = DateTime.Now;
                context.RoleMaster.Remove(rolemaster);
                context.SaveChanges();
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View(rolemaster);
            }
        }

        [Authorize]
        [SessionExpire]
        public ActionResult RoleActionMapping(Guid UniqueId)
        {
            RoleActionMappings roleActionMapping = new RoleActionMappings();

            RoleMaster roleMaster = context.RoleMaster.Where(x => x.UniqueId == UniqueId).FirstOrDefault();

            roleActionMapping.RoleId = roleMaster.UniqueId;
            roleActionMapping.RoleName = roleMaster.Name;

            roleActionMapping.Action = context.ActionMaster.ToList();

            roleActionMapping.RoleActionMapping = context.RoleActionMapping.Where(x => x.RoleMasterId == UniqueId && x.IsActive == true).ToList();

            return View(roleActionMapping);

        }

        [Authorize]
        [SessionExpire]
        [HttpPost]
        public ActionResult RoleActionMapping(Guid[] ActionId, Guid RoleId, Guid[] UnMap)
        {
            RoleActionMappings userRoleMappings = new RoleActionMappings();
            try
            {
                var roleActionMap = context.RoleActionMapping.Where(x => x.RoleMasterId == RoleId && x.IsActive == true).ToList();

                if (ActionId != null)
                {
                    foreach (var item in ActionId)
                    {
                        if (roleActionMap.Select(x => x.ActionMasterId).Contains(item) == false)
                        {
                            RoleActionMapping roleActionMapping = new RoleActionMapping();
                            roleActionMapping.RoleMasterId = RoleId;
                            roleActionMapping.ActionMasterId = item;
                            roleActionMapping.CreatedBy = User.Identity.Name;
                            roleActionMapping.CreatedDate = DateTime.Now;

                            context.RoleActionMapping.Add(roleActionMapping);
                            context.SaveChanges();
                        }
                    }
                }

                if (UnMap != null)
                {
                    foreach (var item in UnMap)
                    {
                        if (roleActionMap.Select(x => x.ActionMasterId).Contains(item))
                        {
                            RoleActionMapping roleActionMapping = roleActionMap.Where(x => x.ActionMasterId == item && x.RoleMasterId == RoleId).FirstOrDefault();
                            roleActionMapping.IsActive = false;
                            roleActionMapping.DeletedBy = User.Identity.Name;
                            roleActionMapping.DeletedDate = DateTime.Now;

                            context.RoleActionMapping.Remove(roleActionMapping);
                            context.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                RoleMaster roleMaster = context.RoleMaster.Where(x => x.UniqueId == RoleId).FirstOrDefault();

                userRoleMappings.RoleId = roleMaster.UniqueId;
                userRoleMappings.RoleName = roleMaster.Name;

                userRoleMappings.Action = context.ActionMaster.ToList();

                userRoleMappings.RoleActionMapping = context.RoleActionMapping.Where(x => x.RoleMasterId == RoleId).ToList();
            }

            return View(userRoleMappings);
        }

        public ActionResult Poster()
        {
            return View();
        }
    }
}