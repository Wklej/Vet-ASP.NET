using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VetTest.Models
{
    public class MyUser
    {
        [Key]
        public int MyUserID { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public virtual ICollection<Animal> Animals { get; set; }
    }
}