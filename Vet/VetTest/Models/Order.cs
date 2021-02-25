using System;
using System.ComponentModel.DataAnnotations;

namespace VetTest.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public String Name { get; set; }
        public int Amount { get; set; }
    }
}