//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarLease.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cars
    {
        public int Id { get; set; }
        public int CarTypeId { get; set; }
        public string CurrentKM { get; set; }
        public byte[] Picture { get; set; }
        public string GoodForRental { get; set; }
        public int LicenseNumber { get; set; }
        public string BranchId { get; set; }
        public int Order_Id { get; set; }
    }
}