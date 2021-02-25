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
    public class AnimalsController : Controller
    {
        private VetContext db = new VetContext();

        [Authorize(Roles ="admin,worker")]
        // GET: Animals
        public ActionResult Index()
        {
            var animals = db.Animals.Include(a => a.MyUser);
            return View(animals.ToList());
        }

        [Authorize(Roles = "client")]
        // GET: Animals for logged Client
        public ActionResult IndexClient(String Email)
        {
            var loggedUser = db.MyUsers.Where(mu => mu.Email == Email).FirstOrDefault();
            var clientAnimals = db.Animals.Where(a => a.MyUserID == loggedUser.MyUserID);

            var addModel = new AddAnimalModel()
            {
                Animals = clientAnimals
            };

            return View(addModel);
        }


        // GET: Animals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = db.Animals.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }

        // GET: Animals/Create
        [Authorize(Roles = "client")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Animals/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="client")]
        public ActionResult Create([Bind(Include = "AnimalID,Name,Age,Type")] AddAnimalModel addAnimal)
        {
            var loggedUser = db.MyUsers.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
            var animalToPass = new Animal()
            {
                Name = addAnimal.Name,
                Age = addAnimal.Age,
                Type = addAnimal.Type,
                MyUser = loggedUser
            };

            if (ModelState.IsValid)
            {
                animalToPass.MyUser = loggedUser;

                db.Animals.Add(animalToPass);
                db.SaveChanges();
                return RedirectToAction("IndexClient", "Animals", new { Email = loggedUser.Email });
            }
            return View();
        }

        // GET: Animals/Edit/5
        [Authorize(Roles = "client")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = db.Animals.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }

        // POST: Animals/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="client")]
        public ActionResult Edit([Bind(Include = "AnimalID,Name,Age,Type,MyUserID")] Animal animal, int ModelID)
        {
            var loggedUser = db.MyUsers.Where(u => u.Email == User.Identity.Name).FirstOrDefault();

            if (ModelState.IsValid)
            {
                Animal animalToUpdate = new Animal()
                {
                    MyUser = loggedUser,
                    Name = animal.Name,
                    Age = animal.Age,
                    Type = animal.Type,
                    AnimalID = ModelID,
                    MyUserID = loggedUser.MyUserID
                };

                db.Entry(animalToUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexClient", "Animals", new { Email = User.Identity.Name });
            }
            return View(animal);
        }

        // GET: Animals/Delete/5
        [Authorize(Roles ="client")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = db.Animals.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }

        // POST: Animals/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "client")]
        public ActionResult DeleteConfirmed(AddAnimalModel animalModel, int AnimalID)
        {
            Animal animal = db.Animals.Find(AnimalID);
            db.Animals.Remove(animal);
            db.SaveChanges();
            return RedirectToAction("IndexClient", "Animals", new { Email = User.Identity.Name });
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
