using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POS.Models
{
    public class BaseModel
    {
        public BaseModel()
        {
            UniqueId = Guid.NewGuid();
            IsActive = true;
            CreatedDate = DateTime.Now;
            CreatedBy = "System";
        }

        [Key]
        public Guid UniqueId { get; set; }

        public Guid Id { get; set; }

        public bool IsActive { get; set; }

        [StringLength(30)]
        [Required]
        public string CreatedBy { get; set; }

        [StringLength(30)]
        public string UpdatedBy { get; set; }

        [StringLength(30)]
        public string DeletedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedDate { get; set; }

        [DisplayName("Access Code")]
        public string AccessCode { get; set; }

    }
}