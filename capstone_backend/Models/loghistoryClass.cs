using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace capstone_backend.Models
{
    public class loghistoryClass
    {
        public string email { get; set; }
        public char loggedinstatus { get; set; }
        public string message { get; set; }
        public DateTime logindate { get; set; }
    }
}