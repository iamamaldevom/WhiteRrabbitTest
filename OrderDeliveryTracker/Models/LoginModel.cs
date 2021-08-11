using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrderDeliveryTracker.Models
{
    public class LoginModel
    {
        [DisplayName("User Name")]
        [Required]
        public string UserName { get; set; }

        [DisplayName("Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}