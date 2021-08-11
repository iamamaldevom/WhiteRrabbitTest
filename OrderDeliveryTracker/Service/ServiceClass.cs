using OrderDeliveryTracker.Data;
using OrderDeliveryTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderDeliveryTracker.Service
{
    public class ServiceClass
    {
        DataProvider _dataProvider;

        public ServiceClass()
        {
            _dataProvider = new DataProvider();
        }

        public User Login(LoginModel _loginUser)
        {
            return _dataProvider.Login(_loginUser);
        }

        public CreateUserOutput SignUp(CreateUser _user)
        {
            return _dataProvider.SignUp(_user);
        }

        public List<Order> GetOrders(string userName, bool isAdmin)
        {
            return _dataProvider.GetOrders(userName, isAdmin);
        }

        public bool DeliveryUpdate(long orderId)
        {
            return _dataProvider.DeliveryUpdate(orderId);
        }

    }
}