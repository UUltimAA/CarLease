using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarLease.Models;

namespace CarLease.Controllers
{
    public class AuthController : Controller
    {
        CarLeaseEntities context = new CarLeaseEntities();
        //
        // GET: /Auth/
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Signup(Customers customer, string returnUrl)
        {
            //sign up - create user if information is valid
            //if successful then login
            HttpContext.Session["user"] = new Customers() { FullName = "blah" };
            //return:
            return Redirect(HttpUtility.UrlDecode(returnUrl));
        }

        [HttpPost]
        public ActionResult Login(string email, string password, string returnUrl)
        {
            var a = context.Customers.FirstOrDefault(x => x.Email == email);
            if (a.Password == password)
            {
                HttpContext.Session["user"] = a;
                if (string.IsNullOrEmpty(returnUrl))
                    return Redirect("/");
                else
                    return Redirect(HttpUtility.UrlDecode(returnUrl));
            }
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.Session["user"] = null;
            return Redirect("/auth/login");
        }


	}
}