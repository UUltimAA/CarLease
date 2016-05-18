using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarLease.Models;
using CarLease.App_Start.Filters;


namespace CarLease.Controllers
{
    //[IsAdmin]
    [IsAuthenticated(true)]
    public class ManagementController : Controller
    {
        // GET: Managment
        CarLeaseEntities context = new CarLeaseEntities();

        public ActionResult Management()
        {
            return View();
        }
        public ActionResult Customers()
        {
            return View(context.Customers.ToList());
        }

        public ActionResult GetCustomers()
        {
            return Json(context.Customers.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateCustomer(Customers customer)
        {
            context.Customers.Attach(customer);
            context.Entry(customer).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return Json(context.Customers.Where(c => c.Id == customer.Id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult AddCustomer(Customers customer)
        {
            context.Customers.Attach(customer);
            context.Entry(customer).State = System.Data.Entity.EntityState.Added;
            context.SaveChanges();

            return Json(context.Customers.Where(c => c.Id == customer.Id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult DeleteCustomer(Customers customer)
        {
            context.Customers.Remove(customer);
            context.Entry(customer).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();

            return Json(context.Customers.Where(c => c.Id == customer.Id).FirstOrDefault());
        }

        //Orders Management/
//public ActionResult GetOrders()
        //{
        //    return Json(context.Orders.ToArray(), JsonRequestBehavior.AllowGet);
        //}
//[HttpPost]
        //public ActionResult UpdateOrder(Orders order)
        //{
        //    context.Orders.Attach(order);
        //    context.Entry(order).State = System.Data.Entity.EntityState.Modified;
        //    context.SaveChanges();

        //    return Json(context.Orders.Where(o => o.Id == order.Id).FirstOrDefault());
        //}

        public ActionResult Orders()
        {
            var results = (from order in context.Orders
                           join customer in context.Customers on order.CustomerId equals customer.Id
                           join car in context.Cars on order.CarId equals car.Id
                           join carType in context.CarTypes on car.CarTypeId equals carType.Id
                           select new OrderDetails()
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

                           }).ToList();

            //    context.Orders.Join().Select(o => new OrderDetails()
            //{
            //    CarId = o.CarId,
            //    LeaseEndDate = o.LeaseEndDate,
            //    LeaseStartDate = o.LeaseStartDate
            //});

            return View(results);
        }

        [HttpGet]
        public ActionResult OrderEdit(int? orderId)
        {
            var allCustomers = context.Customers.ToDictionary(c=>c.Id,c=>c.FullName);
            ViewBag.Customers = allCustomers;

            var allCars = context.Cars;
            ViewBag.Cars = allCars;

            ViewBag.CarsDictionary = allCars.ToDictionary(c => c.Id, c => c.LicenseNumber);

            if (orderId.HasValue)
            {
                Orders order = context.Orders.FirstOrDefault(x => x.Id == orderId);
                return View(order);
            }
            else
            { 
                Orders order = new Orders();
                return View(order);
            }
        }

        [HttpPost]
        public ActionResult OrderEdit(Orders order)
        {
            

            var query = context.Orders.FirstOrDefault(x => x.Id == order.Id);
            //if has id then update
            if (query != null)
            {
                //  -  attach/mark as updated
                //context.Orders.Attach(order);
                context.Entry(query).CurrentValues.SetValues(order);
                //context.Entry(order).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            else
            {
                //else create new
                //  -  mark as new

                context.Orders.Add(order);
                context.Entry(order).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
            //  -  save
            //nullable exception

            return Redirect("/management/orders");

        }

        [HttpGet]
        public ActionResult DeleteOrder(int orderId)
        {
            var query = context.Orders.FirstOrDefault(x => x.Id == orderId);
            context.Orders.Remove(context.Orders.FirstOrDefault(x => x.Id == orderId));
            context.Entry(query).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();

            //return Json(context.Orders.Where(o => o.Id == orderId).FirstOrDefault(), JsonRequestBehavior.AllowGet);
            return Redirect("/management/orders");
        }

        //Customer Management
        [HttpGet]
        public ActionResult CustomerEdit(int? customerId)
        {
            var allCustomers = context.Customers.ToDictionary(c => c.Id, c => c.FullName);
            ViewBag.Customers = allCustomers;

            if (customerId.HasValue)
            {
                Customers customer = context.Customers.FirstOrDefault(c => c.Id == customerId);
                return View(customer);
            }
            else
            {
                Customers customer = new Customers();
                return View(customer);
            }
        }

        [HttpPost]
        public ActionResult CustomerEdit(Customers customer)
        {
            var query = context.Customers.FirstOrDefault(c => c.Id == customer.Id);

            if (query != null)
            {
                context.Entry(query).CurrentValues.SetValues(customer);
                context.SaveChanges();
            }
            else
            {
                context.Customers.Add(customer);
                context.Entry(customer).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
            return Redirect("/management/customers");
            //return View(customer);
        }

        [HttpGet]
        public ActionResult DeleteCustomer(int customerId)
        {
            var query = context.Customers.FirstOrDefault(x => x.Id == customerId);
            context.Customers.Remove(context.Customers.FirstOrDefault(x => x.Id == customerId));
            context.Entry(query).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();

            return Redirect("/management/customers");
        }
        //Cars Management

        public ActionResult Cars()
        {
            return View(context.Cars.ToList());
        }

        [HttpGet]
        public ActionResult CarEdit(int? carId)
        {
            var allCars = context.Cars.ToDictionary(c => c.Id, c => c.CarTypeId);
            ViewBag.Cars = allCars;

            var allBranches = context.Branches.ToDictionary(b => b.Id, b => b.Name);
            ViewBag.Branches = allBranches;

            if (carId.HasValue)
            {
                Cars car = context.Cars.FirstOrDefault(c => c.Id == carId);
                return View(car);
            }
            else
            {
                Cars car = new Cars();
                return View(car);
            }
        }

        [HttpPost]
        public ActionResult CarEdit(Cars car)
        {
              var query = context.Cars.FirstOrDefault(c => c.Id == car.Id);
              var allBranches = context.Branches.ToDictionary(b => b.Id, b => b.Name);
              ViewBag.Branches = allBranches;

            if (query != null)
            {
                context.Entry(query).CurrentValues.SetValues(car);
                context.SaveChanges();
            }
            else
            {
                try
                {
                context.Cars.Add(car);
                context.Entry(car).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
                }
                catch (Exception ex)
                {
                    
                }
            }
            return Redirect("/management/cars");
            //return View(car);
        }
        
        [HttpGet]
        public ActionResult DeleteCar (int carId)
        {
            var query = context.Cars.FirstOrDefault(c => c.Id == carId);
            context.Cars.Remove(context.Cars.FirstOrDefault(x => x.Id == carId));
            context.Entry(query).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();

            return Redirect("/management/cars");
        }

        //CarTypes Management/

        //public ActionResult GetCarTypes()
        //{
        //    return Json(context.CarTypes.ToArray(), JsonRequestBehavior.AllowGet);
        //}
        public ActionResult CarTypes()
        {
            return View(context.CarTypes.ToList());
        }

        [HttpGet]
        public ActionResult CarTypeEdit(int? carTypeId)
        {
            var allCarTypes = context.CarTypes.ToDictionary(c => c.Id, c => c.Model);
            ViewBag.CarTypes = allCarTypes;

            if (carTypeId.HasValue)
            {
                CarTypes carType = context.CarTypes.FirstOrDefault(c => c.Id == carTypeId);
                return View(carType);
            }
            else
            {
                CarTypes carType = new CarTypes();
                return View(carType);
            }
        }

        [HttpPost]
        public ActionResult CarTypeEdit(CarTypes carType)
        {
              var query = context.CarTypes.FirstOrDefault(c => c.Id == carType.Id);
            if (query != null)
            {
                context.Entry(query).CurrentValues.SetValues(carType);
                context.SaveChanges();
            }
            else
            {
                context.CarTypes.Add(carType);
                context.Entry(carType).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
            return Redirect("/management/cartypes");
        
        }

        [HttpGet]
        public ActionResult DeleteCarType(int carTypeId)
        {
            var query = context.CarTypes.FirstOrDefault(c => c.Id == carTypeId);
            context.CarTypes.Remove(context.CarTypes.FirstOrDefault(x => x.Id == carTypeId));
            context.Entry(query).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();

            return Redirect("/management/cartypes");
        }

    }
}