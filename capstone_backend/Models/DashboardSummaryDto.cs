using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace capstone_backend.Models
{
    public class DashboardSummaryDto
    {
        public int TotalProducts { get; set; }
        public int SystemUsers { get; set; }
        public int SalesToday { get; set; }
        public int WarningProduct { get; set; }
    }
}