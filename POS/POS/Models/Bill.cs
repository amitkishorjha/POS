using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POS.Models
{
    public class Bill
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Please enter table name")]
        [Display(Name ="Table No")]
        public string TableNo { get; set; }

        public decimal BillTotal { get; set; } = 0;

        [StringLength(30)]
        [Required]
        public string CreatedBy { get; set; } = "System";

        [StringLength(30)]
        public string UpdatedBy { get; set; }

        [StringLength(30)]
        public string DeletedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedDate { get; set; }

    }
}