using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using VetTest.Models;

namespace VetTest.DAL
{
    public class VetContext: DbContext
    {
        public VetContext() : base("name=VetTest")
        {
            Database.SetInitializer(new VetDBInitializer<VetContext>());

        }

        public DbSet<Animal> Animals { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<MyUser> MyUsers { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<Order> Orders { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

        }

    }

    
}