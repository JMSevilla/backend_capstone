using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace capstone_backend.Models
{
    public class archiveUsersClass
    {
        public string archiveID { get; set; }
        public int clientID { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public char usertype { get; set; }
        public string archiveusermessage { get; set; }
        public DateTime archivecreated { get; set; }
    }
}