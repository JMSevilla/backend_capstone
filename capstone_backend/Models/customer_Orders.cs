//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace capstone_backend.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class customer_Orders
    {
        public int orderID { get; set; }
        public string orderCode { get; set; }
        public string orderName { get; set; }
        public string orderBarcode { get; set; }
        public Nullable<decimal> orderPrice { get; set; }
        public Nullable<int> orderQuantity { get; set; }
        public string orderCategory { get; set; }
        public Nullable<decimal> orderTotalPrice { get; set; }
        public string orderImage { get; set; }
        public Nullable<System.DateTime> createdAt { get; set; }
        public string orderStatus { get; set; }
    }
}
