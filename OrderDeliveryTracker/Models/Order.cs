using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderDeliveryTracker.Models
{
    public class Order
    {
        public long OrderID { get; set; }
        public string OrderName { get; set; }
        public string AssignTo { get; set; }
        public string Status { get; set; }
    }

    public class AllOrders
    {
        public List<Order> Orders { get; set; }
        public AllOrders()
        {
            Orders = new List<Order>();
        }
    }
}