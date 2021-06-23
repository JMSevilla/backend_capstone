using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using capstone_backend.Models;
namespace capstone_backend.Interfaces
{
    public interface dataInterface
    {
        //store function
        void insert(string firstname, string lastname, string email);

    }
}
