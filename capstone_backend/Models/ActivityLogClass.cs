using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace capstone_backend.Models
{
    public class ActivityLogClass
    {
        public string activitymessage { get; set; }
        public string activitystatus { get; set; }
        public string activitycode { get; set; }
        public DateTime createdat { get; set; }
    }
}