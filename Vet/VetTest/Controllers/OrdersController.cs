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
    public class OrdersController : Controller
    {
        private VetContext db = new VetContext();

        // GET: Orders
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        // GET: ORders for logged Worker
        [Authorize(Roles = "worker")]
        public ActionResult IndexWorkers()
        {

            var addModel = new AddOrderModel()
            {
                Orders = db.Orders.ToList()
            };

            return View(addModel);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles = "worker")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "worker")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,Name,Amount")] AddOrderModel orderModel)
        {
            if (ModelState.IsValid)
            {
                var order = new Order()
                {
                    Name = orderModel.Name,
                    Amount = orderModel.Amount
                };

                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("IndexWorkers");
            }

            return View(orderModel);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Accept(int? id)
        {
            Order order = db.Orders.Find(id);
            Supply supplyToModify = db.Supplies.Where(s => s.Name == order.Name).FirstOrDefault();

            if (supplyToModify == null)
            {
                Supply newSupply = new Supply()
                {
                    Amount = order.Amount,
                    Name = order.Name
                };

                db.Supplies.Add(newSupply);
                db.Orders.Remove(order);
                db.SaveChanges();
                return RedirectToAction("Index", "Orders");
            }
            else
            {
                supplyToModify.Amount += order.Amount;

                db.Entry(supplyToModify).State = EntityState.Modified;
                db.Orders.Remove(order);
                db.SaveChanges();
                return RedirectToAction("Index", "Orders");
            }
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,Name,Amount")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        /*[HttpPost, ActionName("Delete")]*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(Order order)
        {
            Order orderToDelete = db.Orders.Find(order.OrderID);
            db.Orders.Remove(orderToDelete);
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
