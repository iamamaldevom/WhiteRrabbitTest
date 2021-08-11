using Newtonsoft.Json.Linq;
using OrderDeliveryTracker.Models;
using OrderDeliveryTracker.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderDeliveryTracker.Controllers
{
    public class HomeController : Controller
    {
        ServiceClass _service;

        public HomeController()
        {
            _service = new ServiceClass();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.Message = "Login";
            Session["UserName"] = string.Empty;
            Session["IsAdmin"] = false;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel _userData)
        {
            try
            {
                User _user = _service.Login(_userData);
                LoginOutput _loginOutput = _user.LoginOutput;
                switch (_loginOutput)
                {
                    case LoginOutput.UserDoesNotExist:
                        {
                            TempData["LoginResult"] = "User does not exist";
                            return View("Login");
                        }
                    case LoginOutput.InvalidPassword:
                        {
                            TempData["LoginResult"] = "Invalid password. Please try again";
                            return View("Login");
                        }
                    case LoginOutput.Success:
                        {
                            Session["UserName"] = _userData.UserName;
                            Session["IsAdmin"] = _user.IsAdmin;
                            return RedirectToAction("Orders");
                        }
                }
                return View("Error");
            }
            catch
            {
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            ViewBag.Message = "SignUp";

            return View();
        }

        [HttpPost]
        public ActionResult SignUp(CreateUser _userData)
        {
            try
            {
                CreateUserOutput createUserOutput = _service.SignUp(_userData);
                switch(createUserOutput)
                {
                    case CreateUserOutput.UserAlreadyExist:
                        {
                            TempData["LoginResult"] = "User already exist";
                            return View("SignUp");
                        }
                    case CreateUserOutput.Success:
                        {
                            TempData["LoginResult"] = "User added successfully. Please login";
                            return View("Login");
                        }
                }
                return View("Error");
            }
            catch
            {
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult Orders()
        {
            try
            {
                ViewBag.Message = "Orders";
                var orders = _service.GetOrders(Session["UserName"].ToString(), Convert.ToBoolean(Session["IsAdmin"]));
                return View(orders);
            }
            catch
            {
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult OrderDelivered(long OrderID)
        {
            try
            {
                var orders = _service.DeliveryUpdate(OrderID);
                return RedirectToAction("Orders");
            }
            catch
            {
                return View("Error");
            }
        }
        

    }
}