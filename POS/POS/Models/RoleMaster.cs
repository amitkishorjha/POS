using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POS.Models
{
    public class RoleMaster : BaseModel
    {
        [DisplayName("Name")]
        [Required(ErrorMessage = "Please enter role name")]
        public string Name { get; set; }
    }
}