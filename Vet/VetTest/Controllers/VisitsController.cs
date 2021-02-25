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
    public class VisitsController : Controller
    {
        private VetContext db = new VetContext();

        [Authorize(Roles = "client")]
        public ActionResult Booking()
        {
            var loggedUser = db.MyUsers.Where(mu => mu.Email == User.Identity.Name).FirstOrDefault();
            var clientAnimals = db.Animals.Where(a => a.MyUserID == loggedUser.MyUserID);

            ViewBag.AnimalID = new SelectList(clientAnimals, "AnimalID", "Name");

            return View();
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public ActionResult Booking(Visit visit)
        {
            var loggedUser = db.MyUsers.Where(mu => mu.Email == User.Identity.Name).FirstOrDefault();

            var visitToPass = new Visit()
            {
                Day = visit.Day,
                Time = visit.Time,
                AnimalID = visit.AnimalID,
                MyUser = loggedUser
            };

            db.Visits.Add(visitToPass);
            db.SaveChanges();

            return RedirectToAction("Main", "Home");
        }

        // GET: Visits
        [Authorize(Roles = "admin,worker")]
        public ActionResult Index()
        {
            var visits = db.Visits.Include(v => v.Animal).Include(v => v.MyUser);
            var bookModel = new AddVisitModel()
            {
                Visits = visits
            };

            return View(bookModel);
        }

        // GET: Animals for logged Client
        [Authorize(Roles = "client")]
        public ActionResult IndexClient(String Email)
        {
            var loggedUser = db.MyUsers.Where(mu => mu.Email == Email).FirstOrDefault();
            var clientVisits = db.Visits.Where(v => v.MyUserID == loggedUser.MyUserID);

            var bookModel = new AddVisitModel()
            {
                Visits = clientVisits
            };

            return View(bookModel);
        }

        // GET: Visits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visit visit = db.Visits.Find(id);
            if (visit == null)
            {
                return HttpNotFound();
            }
            return View(visit);
        }

        // GET: Visits/Create
        public ActionResult Create()
        {
            ViewBag.AnimalID = new SelectList(db.Animals, "AnimalID", "Name");
            ViewBag.MyUserID = new SelectList(db.MyUsers, "MyUserID", "Name");
            return View();
        }

        // POST: Visits/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "client")]
        public ActionResult Create([Bind(Include = "VisitID,AnimalID,MyUserID,Day,Time")] Visit visit)
        {
            if (ModelState.IsValid)
            {
                db.Visits.Add(visit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AnimalID = new SelectList(db.Animals, "AnimalID", "Name", visit.AnimalID);
            ViewBag.MyUserID = new SelectList(db.MyUsers, "MyUserID", "Name", visit.MyUserID);
            return View(visit);
        }

        // GET: Visits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visit visit = db.Visits.Find(id);
            if (visit == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnimalID = new SelectList(db.Animals, "AnimalID", "Name", visit.AnimalID);
            ViewBag.MyUserID = new SelectList(db.MyUsers, "MyUserID", "Name", visit.MyUserID);
            return View(visit);
        }

        // POST: Visits/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles ="worker, client")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VisitID,AnimalID,MyUserID,Day,Time")] Visit visitModel, int ModelID)
        {

            var loggedUser = db.MyUsers.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
            Visit vis = db.Visits.Where(v => v.VisitID == ModelID).FirstOrDefault();

            if (ModelState.IsValid)
            {
                Visit visit = new Visit()
                {
                    Animal = vis.Animal,
                    VisitID = ModelID,
                    Day = visitModel.Day,
                    Time = visitModel.Time,
                    MyUser = vis.MyUser,
                    MyUserID = vis.MyUser.MyUserID,
                    AnimalID = vis.Animal.AnimalID
                };

                // vis Was causing problem
                db.Entry(vis).State = EntityState.Detached;

                db.Entry(visit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexClient", "Visits", new { Email = User.Identity.Name });
            }
            return View(visitModel);
        }

        // GET: Visits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visit visit = db.Visits.Find(id);
            if (visit == null)
            {
                return HttpNotFound();
            }
            return View(visit);
        }

        // POST: Visits/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Visit visit1)
        {
            Visit visit = db.Visits.Find(visit1.VisitID);
            db.Visits.Remove(visit);
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
