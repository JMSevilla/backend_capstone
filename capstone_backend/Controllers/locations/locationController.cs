﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
namespace capstone_backend.Controllers.locations
{
    [RoutePrefix("api/location-getter")]
    public class locationController : ApiController
    {
        //connection
<<<<<<< HEAD
=======
        //private local_dbbmEntities1 core;
>>>>>>> 9721cfa66296c4d6926767be1ac2f5f3bb89c400
        private local_dbbmEntities1 core;
        //get all municipalities
        [Route("municipalities"), HttpGet]
        public HttpResponseMessage getmunicipality()
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    var obj = core.locations.Select(x => new
                    {
                        x.id,
                        x.municipality
                    }).ToList().Distinct();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadGateway, ex.Message);
            }
        }
        //get province by municipality
        [Route("get-province-by-municipality"), HttpGet]
        public HttpResponseMessage getprovince(string municipality)
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    var obj = core.locations.Where(x => x.municipality == municipality)
                        .Select(y => new
                        {
                            y.id,
                            y.province
                        }).ToList().Distinct();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception ee)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadGateway, ee.Message);
            }
        }
    }
}
