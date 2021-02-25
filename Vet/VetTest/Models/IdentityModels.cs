using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace VetTest.Models
{
    public class ApplicationUser : IdentityUser
    {

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Element authenticationType musi pasować do elementu zdefiniowanego w elemencie CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Dodaj tutaj niestandardowe oświadczenia użytkownika
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        
        public ApplicationDbContext()
            : base("UserContext", throwIfV1Schema: false)
        {
            Database.SetInitializer(new IdentityDBInizializer<ApplicationDbContext>());
        }



        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        public class IdentityDBInizializer<T> : CreateDatabaseIfNotExists<ApplicationDbContext>
        {
            protected override void Seed(ApplicationDbContext context)
            {


                context.Roles.Add(new IdentityRole { Name = "client" });
                context.Roles.Add(new IdentityRole { Name = "worker" });
                context.Roles.Add(new IdentityRole { Name = "admin" });
                context.SaveChanges();


                var store = new UserStore<ApplicationUser>(context);

                var manager = new UserManager<ApplicationUser>(store);

                var user = new ApplicationUser { UserName = "admin@admin.pl" };

                manager.Create(user, "!@#123QWEqwe");



                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                var role = context.Roles.SingleOrDefault(m => m.Name == "admin");

                ApplicationUser user2 = userManager.FindByName("admin@admin.pl");

                userManager.AddToRole(user2.Id, role.Name);

            }

        }
    }
}