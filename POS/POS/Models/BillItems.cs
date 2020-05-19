using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace POS.Models
{
    public class BillItems : BaseModel
    {
        [Required(ErrorMessage = "Please enter quantity")]
        public int Quantity { get; set; }

        [Required]
        public decimal Total { get; set; }

        public int BillNo { get; set; }

        [Required(ErrorMessage = "Please select product")]
        public Guid ProId { get; set; }

        [NotMapped]
        public string ProductName { get; set; }

        [NotMapped]
        public decimal price { get; set; }
    }
}