using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarLease.Models
{
    public class OrderDetails
    {
        public DateTime LeaseStartDate { get; set; }
        public DateTime LeaseEndDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public int CustomerId { get; set; }
        public int OrderId { get; set; }
        public int CarId { get; set; }
        public string UserName { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal? Dailycost { get; set; }
        public decimal? DailyLateCost { get; set; }
        public string Gear { get; set; }
        public int? LicenseNumber { get; set; }

        public OrderDetails()
        {

        }
        public OrderDetails(int carId, int customerId, int orderId, DateTime leaseStartDate, DateTime leaseEndDate,
            decimal dailyCost, DateTime? actualReturnDate, string model, decimal dailyLateCost, string gear, string brand, int licenseNumber)
        {
            CarId = carId;
            CustomerId = customerId;
            OrderId = orderId;
            LeaseStartDate = leaseStartDate;
            LeaseEndDate = leaseEndDate;
            Dailycost = dailyCost;
            ActualReturnDate = actualReturnDate;
            Brand = brand;
            DailyLateCost = dailyLateCost;
            Gear = gear;
            Model = model;
            LicenseNumber = LicenseNumber;
        }
    }
    }

