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
        private static local_dbbmEntities2 _pribado;
        public static local_dbbmEntities2 publico
        {
            get
            {
                _pribado = new local_dbbmEntities2();
                return _pribado;
            }
        }
        private static SqlConnection _privatecon;
        public static SqlConnection _publiccon
        {
            get
            {
                string constring = "Server=localhost;Database=local_dbbm;User Id=root;Password=;Trusted_Connection=True;Integrated Security=true;";
                _privatecon = new SqlConnection(constring);
                return _privatecon;
            }
        }
    }
}