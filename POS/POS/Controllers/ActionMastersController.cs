using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using POS.Common;
using POS.Models;

namespace POS.Controllers
{
    [SimpleAuthorizeAttribute]
    [SessionExpire]
    public class ActionMastersController : Controller
    {
        private POSContext db = new POSContext();

        // GET: ActionMasters
        public ActionResult Index()
        {
            return View(db.ActionMaster.ToList());
        }

        // GET: ActionMasters/Details/5
        public ActionResult Details(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionMaster actionMaster = db.ActionMaster.Find(UniqueId);
            if (actionMaster == null)
            {
                return HttpNotFound();
            }
            return View(actionMaster);
        }

        // GET: ActionMasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ActionMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ActionMaster actionMaster)
        {
            actionMaster.CreatedBy = User.Identity.Name;
            if (ModelState.IsValid)
            {
                // TODO: Add insert logic here
                actionMaster.CreatedDate = DateTime.Now;
                actionMaster.IsActive = true;
                db.ActionMaster.Add(actionMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(actionMaster);
        }

        // GET: ActionMasters/Edit/5
        public ActionResult Edit(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionMaster actionMaster = db.ActionMaster.Find(UniqueId);
            if (actionMaster == null)
            {
                return HttpNotFound();
            }
            return View(actionMaster);
        }

        // POST: ActionMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ActionMaster actionMaster)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add update logic here
                actionMaster.UpdatedBy = User.Identity.Name;
                actionMaster.UpdatedDate = DateTime.Now;
                db.Entry(actionMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actionMaster);
        }

        // GET: ActionMasters/Delete/5
        public ActionResult Delete(Guid? UniqueId)
        {
            if (UniqueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionMaster actionMaster = db.ActionMaster.Find(UniqueId);
            if (actionMaster == null)
            {
                return HttpNotFound();
            }
            return View(actionMaster);
        }

        // POST: ActionMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid UniqueId)
        {
            ActionMaster actionMaster = db.ActionMaster.Find(UniqueId);
            actionMaster.DeletedBy = User.Identity.Name;
            actionMaster.DeletedDate = DateTime.Now;
            db.ActionMaster.Remove(actionMaster);
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
