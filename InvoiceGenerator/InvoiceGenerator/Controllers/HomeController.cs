//using InvoiceGenerator.Models;
//using Microsoft.AspNetCore.Mvc;
//using System.Diagnostics;

//namespace InvoiceGenerator.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;

//        public HomeController(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}

using InvoiceGenerator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace InvoiceGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;
        public static Users currUser;

        public static bool isLogout = false;
        public static bool ispublic = false;
        public HomeController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.User = HomeController.currUser;
            ViewBag.isPublic = ispublic;
            ViewBag.isLogout = isLogout;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {

            isLogout = false;
            ispublic = false;
            return View();
        }
        [HttpPost]
        public IActionResult Login(Users user)
        {
            if (!isLogout)
            {
                if (ModelState.GetFieldValidationState("Email") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
                {
                    if (user.Email != null && user.Password != null)
                    {
                        Users validateuser = _userRepository.GetUserEmailPassword(user.Email, user.Password);
                        if (validateuser != null)
                        {
                            ViewBag.User = validateuser;
                            currUser = validateuser;
                            
                            return RedirectToAction("Index","Product");
                        }
                        else
                        {
                            ViewData["InvalidUser"] = "Invalid Email Address or Password";

                        }
                    }
                }
                return View();
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult UpdateProfile()
        {
            if (!isLogout)
            {
                Users newuser = _userRepository.GetUserFromId(currUser.Id);
                ViewBag.User = currUser;
                return View(newuser);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult UpdateProfile(Users user)
        {
            if (!isLogout)
            {
                if (ModelState.GetFieldValidationState("Username") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid
                    && ModelState.GetFieldValidationState("Email") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid
                    && ModelState.GetFieldValidationState("Contact") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
                {
                    Users newuser = _userRepository.GetUserFromId(currUser.Id);

                    newuser.Email = user.Email;

                    Users updateduser = _userRepository.Update(newuser);

                    return RedirectToAction("Home");
                }
                return View();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Signup()
        {
            isLogout = false;
            return View();
        }
        [HttpPost]
        public IActionResult Signup(Users user)
        {
            if (ModelState.IsValid)
            {
                Users newuser = _userRepository.Add(user);
                currUser = newuser;
                return RedirectToAction("Index","Product");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Home()
        {
            if (!isLogout)
            {
                Users newuser = _userRepository.GetUserFromId(currUser.Id);
                ViewBag.User = newuser;
                return View();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Logout()
        {
            isLogout = true;
            currUser = null;
            return RedirectToAction("Login");
        }
    }
}
