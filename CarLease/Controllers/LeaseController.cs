using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarLease.Models;
using CarLease.App_Start.Filters;

namespace CarLease.Controllers
{
    public class LeaseController : Controller
    {
        CarLeaseEntities context = new CarLeaseEntities();
        // GET: Lease
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string gear, string brand, string model, string freeText, string date)
        {
            var typeQuery = context.CarTypes.AsQueryable();

            if (!string.IsNullOrEmpty(gear))
                typeQuery = typeQuery.Where(x => x.Gear == gear);
            if (!string.IsNullOrEmpty(brand))
            {
                typeQuery = typeQuery.Where(x => x.Brand == brand);
            }
            if (!string.IsNullOrEmpty(model))
            {
                typeQuery = typeQuery.Where(x => x.Model == model);
            }
            if (!string.IsNullOrEmpty(freeText))
            {
                typeQuery = typeQuery.Where(x => x.Brand.ToLower().Contains(freeText.ToLower()));
            }

            var validCarTypes = typeQuery.Select(t => t.Id);
            var query = context.Cars.Where(c => validCarTypes.Contains(c.CarTypeId));
            var retArr = query.Select(c => new CarDetails()
            {
                Car = c,
                CarType = context.CarTypes.FirstOrDefault(t => t.Id == c.CarTypeId),
                // Order = new Orders()
            }
            );
            return Json(retArr);

            //var query = context.CarTypes.AsQueryable();
            //if (!string.IsNullOrEmpty(gear))
            //    query = query.Where(x => x.Gear == gear);
            //if (!string.IsNullOrEmpty(brand))
            //{
            //    query = query.Where(x => x.Brand == brand);
            //}
            //if (!string.IsNullOrEmpty(model))
            //{
            //    query = query.Where(x => x.Model == model);
            //}
            //if (!string.IsNullOrEmpty(freeText))
            //{
            //    query = query.Where(x => x.Brand.ToLower().Contains(freeText.ToLower()));
            //}

            //return Json(query.ToArray());
        }

        //public ActionResult Order()
        //{
        //    //need continuity too? display the whole order before pushing it into database
        //    return View();
        //}

        [IsAuthenticated]
        [HttpGet]
        public ActionResult Order(int carId, DateTime dateStart, int leaseDays)
        {
            CarDetails details = new CarDetails();
            details.Car = context.Cars.FirstOrDefault(x => x.Id == carId);
            details.CarType = context.CarTypes.FirstOrDefault(x => x.Id == details.Car.CarTypeId);
            if (details != null)
            {
                if (details.Order == null)
                {
                    details.Order = new Orders()
                    {
                        CarId = carId,
                        LeaseStartDate = dateStart,
                        LeaseEndDate = dateStart.AddDays(leaseDays)

                    };
                }
                // var a = details.Order.LeaseEndDate.Subtract(details.Order.LeaseStartDate);

                return View(details);
            }
            else
                return Redirect("/");

        }

        [HttpPost]
        public ActionResult Order(Orders order)
        {
            Customers curUser = (Customers)HttpContext.Session["user"];
            order.CustomerId = curUser.Id;
            context.Orders.Attach(order);
            context.Entry(order).State = System.Data.Entity.EntityState.Added;
            context.SaveChanges();

            return View();
        }

        public ActionResult Return(int carNumber)
        {
            var a = context.Cars.Where(x => x.LicenseNumber == carNumber);
            //int carId = context.Cars.Where(x => x. == a.Id);
            if (a != null)
            {
                //change ActualReturnDate in database
            }

            return null;
        }

        public ActionResult Calculator(int carId) //DateTime dateStart, DateTime dateEnd)
        {
            // decimal a = context.CarTypes.Where(x => x.DailyCost); // need continuity from the browse\search page (which car "dailyCost"?)

            // 1. get car details (car && cartype)
            // 2. return the full model to the view for presentation

            CarDetails details = new CarDetails();
            details.Car = context.Cars.FirstOrDefault(x => x.Id == carId);
            if (details != null)
            {
                try
                {
                    var typeid = details.Car.CarTypeId;
                    var carType = context.CarTypes.FirstOrDefault(x => x.Id == typeid);
                    details.CarType = carType;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            return View(details);
        }

        [HttpPost]
        public ActionResult Calculator(DateTime dateStart, int numberOfDays, string carId)
        {
            //context.Cars.Where(x => context.CarTypes.Where( x.CarTypeId);
            return null;
        }

        [IsAuthenticated]
        public ActionResult MyOrders()
        {
            int customerId = ((Customers)HttpContext.Session["user"]).Id;
            var results = (from order in context.Orders
                           join customer in context.Customers on order.CustomerId equals customer.Id
                           join car in context.Cars on order.CarId equals car.Id
                           join carType in context.CarTypes on car.CarTypeId equals carType.Id
                           where customer.Id == customerId //((Customers)HttpContext.Session["user"]).Id
                           select new OrderDetails()
                               
                               //(
                           //   car.Id, customer.Id, order.Id, order.LeaseStartDate, order.LeaseEndDate,carType.DailyCost,order.ActualReturnDate,carType.Brand,carType.DialyLateCost,carType.Gear,carType.Model,car.LicenseNumber)
                           {
                           
                               CarId = car.Id, 
                               CustomerId = customer.Id,
                               OrderId = order.Id,
                               LeaseStartDate = order.LeaseStartDate,
                               LeaseEndDate = order.LeaseEndDate,
                               Dailycost = carType.DailyCost,
                               ActualReturnDate = order.ActualReturnDate,
                               Brand = carType.Brand,
                               DailyLateCost = carType.DailyLateCost,
                               Gear = carType.Gear,
                               Model = carType.Model,
                               LicenseNumber = car.LicenseNumber

                           }
        ).ToList();

            return View(results);
        }
    }


}
