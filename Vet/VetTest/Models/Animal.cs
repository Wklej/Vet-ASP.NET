using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VetTest.Models
{
    public class Animal
    {
        [Key]
        public int AnimalID { get; set; }
        public String Name { get; set; }
        public int Age { get; set; }
        public String Type { get; set; }
        public int MyUserID { get; set; }
        public virtual MyUser MyUser { get; set; }
    }
}