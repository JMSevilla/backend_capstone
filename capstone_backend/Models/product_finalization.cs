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
    
    public partial class product_finalization
    {
        public int id { get; set; }
        public string prodname { get; set; }
        public Nullable<int> prodquantity { get; set; }
        public Nullable<decimal> prodprice { get; set; }
        public string prodcategory { get; set; }
        public Nullable<decimal> prodtotal { get; set; }
        public string prodstatus { get; set; }
        public Nullable<System.DateTime> createdAt { get; set; }
        public string productCode { get; set; }
        public string prodimg { get; set; }
        public string integratedRaws { get; set; }
    }
}
