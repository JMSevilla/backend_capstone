using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace capstone_backend.Models
{
    public class user_data_management
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string municipality { get; set; }
        public string province { get; set; }
        public string address { get; set; }
        public string company_name { get; set; }
        public string address_type { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int mobileno { get; set; }
        public char istype { get; set; }
        public char isverified { get; set; }
        public char isstatus { get; set; }
        public char is_google_verified { get; set; }
    }
}