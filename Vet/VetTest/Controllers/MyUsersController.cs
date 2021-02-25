using Microsoft.AspNet.Identity.Owin;
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
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNet.Identity;

namespace VetTest.Controllers
{
    [Authorize(Roles ="admin")]
    public class MyUsersController : Controller
    {
        private VetContext db = new VetContext();
        private ApplicationDbContext adb = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public MyUsersController()
        {
        }

        public MyUsersController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Advance(int id)
        {
            var myUser = db.MyUsers.Find(id);
            var userId = adb.Users.Where(u => u.Email == myUser.Email).FirstOrDefault().Id;
            var user = adb.Users.Find(userId);

            UserManager.RemoveFromRole(userId, "client");
            UserManager.AddToRole(userId, "worker");

            adb.Entry(user).State = EntityState.Modified;
            adb.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: MyUsers
        public ActionResult Index(string search)
        {
            var appUsers = adb.Users;
            var usersModel = new MyUserModel()
            {
                MyUsers = db.MyUsers,
                ApplicationUsers = appUsers
            };

            if(search != null)
            {
                usersModel.MyUsers = db.MyUsers.Where(u => u.Name.StartsWith(search));
            }

            return View(usersModel);
        }

        // GET: MyUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyUser myUser = db.MyUsers.Find(id);
            if (myUser == null)
            {
                return HttpNotFound();
            }
            return View(myUser);
        }

        // GET: MyUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MyUsers/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MyUserID,Name,Email")] MyUser myUser)
        {
            if (ModelState.IsValid)
            {
                db.MyUsers.Add(myUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(myUser);
        }

        // GET: MyUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyUser myUser = db.MyUsers.Find(id);
            if (myUser == null)
            {
                return HttpNotFound();
            }
            return View(myUser);
        }

        // POST: MyUsers/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MyUserID,Name,Email")] MyUser myUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(myUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(myUser);
        }

        // GET: MyUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyUser myUser = db.MyUsers.Find(id);
            if (myUser == null)
            {
                return HttpNotFound();
            }
            return View(myUser);
        }

        // POST: MyUsers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(MyUserModel userModel, int MyUserID)
        {
            MyUser myUser = db.MyUsers.Find(MyUserID);
            ApplicationUser appuser = adb.Users.Where(u => u.Email == myUser.Email).FirstOrDefault();
            adb.Users.Remove(appuser);
            adb.SaveChanges();
            db.MyUsers.Remove(myUser);
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
