using System;
using System.ComponentModel.DataAnnotations;

namespace VetTest.Models
{
    public class Visit
    {
        [Key]
        public int VisitID { get; set; }
        public int AnimalID { get; set; }
        public int MyUserID { get; set; }
        public int Day { get; set; }
        public String Time { get; set; }
        public virtual Animal Animal { get; set; }
        public virtual MyUser MyUser { get; set; }
    }
}