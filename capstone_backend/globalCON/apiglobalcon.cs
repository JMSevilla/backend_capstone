using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using capstone_backend.Models;

namespace capstone_backend.globalCON
{
    public static class apiglobalcon
    {
        private static dbbmEntities _pribado;
        public static dbbmEntities publico
        {
            get
            {
                _pribado = new dbbmEntities();
                return _pribado;
            }
        }
        private static SqlConnection _privatecon;
        public static SqlConnection _publiccon
        {
            get
            {
                //string constring = "Server=dbburgermania.database.windows.net;Database=dbbm;User Id=bmadmin;Password=5418873Jmsevilla!;Trusted_Connection=False;Integrated Security=False;";
                string constring = "Server=localhost;database=dbbm;Integrated Security=False;Trusted_Connection=False;";
                _privatecon = new SqlConnection(constring);
                return _privatecon;
            }
        }
    }
}