using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderDeliveryTracker.Models
{
    public class User: LoginModel
    {
        public long UserID { get; set; }
        public bool IsAdmin { get; set; }
        public LoginOutput LoginOutput { get; set; }
    }
}