using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using capstone_backend.Models;
namespace capstone_backend.Models
{
    public class DatabaseManager
    {
        private static local_dbbmEntities _instance;
        private DatabaseManager()
        {

        }
        static DatabaseManager()
        {
            _instance = new local_dbbmEntities();
        }

    }
}