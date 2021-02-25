using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VetTest.DAL;
using VetTest.Models;

namespace VetTest.Controllers
{
    [Authorize]
    public class SuppliesController : Controller
    {
        private VetContext db = new VetContext();

        // GET: Supplies
        [Authorize(Roles = "admin,worker")]
        public ActionResult Index()
        {
            return View(db.Supplies.ToList());
        }

        // GET: Supplies for Worker
        [Authorize(Roles = "worker")]
        public ActionResult IndexWorkers()
        {
            return View(db.Supplies.ToList());
        }


        // GET: Supplies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supply supply = db.Supplies.Find(id);
            if (supply == null)
            {
                return HttpNotFound();
            }
            return View(supply);
        }

        // GET: Supplies/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Supplies/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SupplyID,Name,Amount")] Supply supply)
        {
            if (ModelState.IsValid)
            {
                db.Supplies.Add(supply);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supply);
        }

        // GET: Supplies/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supply supply = db.Supplies.Find(id);
            if (supply == null)
            {
                return HttpNotFound();
            }
            return View(supply);
        }

        // POST: Supplies/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "SupplyID,Name,Amount")] Supply supply)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supply).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supply);
        }

        // GET: Supplies/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supply supply = db.Supplies.Find(id);
            if (supply == null)
            {
                return HttpNotFound();
            }
            return View(supply);
        }

        // POST: Supplies/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(Supply supply)
        {
            Supply supplyToDelete = db.Supplies.Find(supply.SupplyID);
            db.Supplies.Remove(supplyToDelete);
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
