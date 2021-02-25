using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VetTest.Models;

namespace VetTest.DAL
{
    public class VetDBInitializer<T> : CreateDatabaseIfNotExists<VetContext>
    {
        protected override void Seed(VetContext context)
        {
            IList<Animal> animals = new List<Animal>()
            {
                new Animal(){Name="Fufu", Age=2, Type="Pies"},
                new Animal(){Name="Mefi", Age=1, Type="Pies"}
            };
            context.Animals.AddRange(animals);

            MyUser testuser = new MyUser()
            {
                Name = "TEST",
                Email = "test@test.pl",
                Animals = animals
            };
            context.MyUsers.Add(testuser);

            context.SaveChanges();
           
        }
    }
}