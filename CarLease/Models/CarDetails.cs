using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarLease.Models
{
    public class CarDetails
    {
        public Cars Car { get; set; }
        public CarTypes CarType { get; set; }
        public Orders Order { get; set; }

        public CarDetails()
        {
        }

        public CarDetails(Cars car, CarTypes type, Orders order)
        {
            Car = car;
            CarType = type;
            Order = order;
        }
    }
}