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
    
    public partial class login_history
    {
        public int id { get; set; }
        public string email { get; set; }
        public string loggedinstatus { get; set; }
        public string message { get; set; }
        public Nullable<System.DateTime> logindate { get; set; }
        public string loggedoutstatus { get; set; }
        public Nullable<System.DateTime> logoutdate { get; set; }
    }
}
