using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartAPIIntegradorFase1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }
        public ActionResult Register()
        {
            ViewBag.Title = "Register";
            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Title = "Login";
            return View();
        }

        public ActionResult Logout()
        {
            ViewBag.Title = "Logout";
            return View();
        }

        public ActionResult GetUserInfo()
        {
            ViewBag.Title = "Logout";
            return View();
        }
    }
}
