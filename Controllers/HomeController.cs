using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CStructorSite.Models;

namespace CStructorSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            if (UserLoggedIn())
            {
                return View("Index");
            }
            else
            {
                return View();
            }
        }
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult ClassList()
        {
            using (var context = new minicstructorContext())
            {
                var classes = context.Classes.ToList();
                return View(classes);
            }
        }

        public IActionResult StudentClasses()
        {
            var userEmail = Request.Cookies["user"];
            if (userEmail == null)
            {
                return RedirectToAction("Login");
            }

            using (var context = new minicstructorContext())
            {
                var user = context.Users.First(u => u.UserEmail == userEmail);
                var userClasses = context.UserClasses.Where(u => u.UserId == user.UserId).Select(id => id.ClassId).ToList();

                var classes = context.Classes.Where(c => userClasses.Contains(c.ClassId)).ToList();

                if (user != null && classes != null)
                {
                    return View(classes.ToList());
                }
            }
            return View();
        }

        public IActionResult Enroll()
        {
            if (UserLoggedIn())
            {
                using (var context = new minicstructorContext())
                {
                    var user = context.Users.FirstOrDefault(u => u.UserEmail.ToLower() == Request.Cookies["user"].ToLower());
                    if (user != null)
                    {
                        var userClasses = context.UserClasses.Where(u => u.UserId == user.UserId).Select(id => id.ClassId).ToList();
                        var classes = context.Classes.Where(c => !userClasses.Contains(c.ClassId)).ToList();
                        return View(classes);
                    }
                }
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Enroll(Class cls)
        {
            using (var context = new minicstructorContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserEmail == Request.Cookies["user"].ToLower());

                if (user != null)
                {
                    context.UserClasses.Add(new UserClass()
                    {
                        UserId = user.UserId,
                        ClassId = cls.ClassId
                    });

                    context.SaveChanges();
                    return RedirectToAction("StudentClasses");
                }
            }
            ViewBag.ErrorMessage = "Error enrolling for class please contact administrator";
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            using (var context = new minicstructorContext())
            {
                var loggedUser = context.Users.FirstOrDefault(u => u.UserEmail == user.UserEmail && u.UserPassword == user.UserPassword);
                if (loggedUser != null)
                {
                    Response.Cookies.Append("user", user.UserEmail);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid username or password";
                    return View();
                }
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("user");
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult Register(User user)
        {
            using (var context = new minicstructorContext())
            {
                var input = context.Users.FirstOrDefault(u => u.UserEmail.ToLower() == user.UserEmail.ToLower());
                if (input == null)
                {
                    context.Users.Add(new Models.User()
                    {
                        UserEmail = user.UserEmail.ToLower(),
                        UserPassword = user.UserPassword
                    });

                    context.SaveChanges();
                    Response.Cookies.Append("user", user.UserEmail.ToLower());
                    return RedirectToAction("Index");
                }

                else
                {
                    ViewBag.ErrorMessage = "Username already exists";
                    return View();
                };
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [NonAction]
        public bool UserLoggedIn()
        {
            return Request.Cookies["user"] != null;
        }
    }
}
