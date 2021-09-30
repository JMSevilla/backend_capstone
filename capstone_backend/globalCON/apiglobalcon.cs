using System;
using System.Collections.Generic;
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
    }
}