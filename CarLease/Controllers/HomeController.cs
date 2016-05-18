using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarLease.Models;

namespace CarLease.Controllers
{
    public class HomeController : Controller
    {
        CarLeaseEntities context = new CarLeaseEntities();

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

        public ActionResult RentalCost()
        {
            ViewBag.Message = "Rental Cost";

            return View();
        }

        public ActionResult GetCustomers()
        {
            return Json(context.Customers.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}