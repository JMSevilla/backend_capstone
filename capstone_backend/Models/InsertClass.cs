using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using capstone_backend.Interfaces;
namespace capstone_backend.Models 
{
    public class InsertClass : dataInterface
    {
        private dbtrainingEntities core; //connection database
        public void insert(string firstname, string lastname, string email)
        {
            try
            {
                using(core = new dbtrainingEntities())
                {
                    tbinformation data = new tbinformation();
                    data.firstname = firstname;
                    data.lastname = lastname;
                    data.email = email;
                    data.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                    core.tbinformations.Add(data);
                    core.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}