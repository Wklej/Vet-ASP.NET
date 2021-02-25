using System;
using System.ComponentModel.DataAnnotations;

namespace VetTest.Models
{
    public class Supply
    {
        [Key]
        public int SupplyID { get; set; }
        public String Name { get; set; }
        public int Amount { get; set; }
    }
}