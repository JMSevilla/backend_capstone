using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
using System.Web;
namespace capstone_backend.Controllers.Settings
{
    [RoutePrefix("api/system-settings")]
    public class SystemSettingsController : ApiController
    {
<<<<<<< HEAD
=======
        //private local_dbbmEntities1 core;
>>>>>>> 9721cfa66296c4d6926767be1ac2f5f3bb89c400
        private local_dbbmEntities1 core;
        [Route("check-settings"), HttpPost]
        public HttpResponseMessage checksettings(string enableposettings)
        {
            try
            {
               using(core = new local_dbbmEntities1())
                {
                    
                    var httprequest = HttpContext.Current.Request;
                    var obj = core.tbposettings.Any(x => x.enablePO == enableposettings);
                    if (obj)
                    {
                        
                        //exist
                        core.update_settings(1, enableposettings, httprequest.Form["enablepoinventory"],
                           httprequest.Form["enablelistview"], httprequest.Form["viewentry"]);
                        return Request.CreateResponse(HttpStatusCode.OK, "update settings success");
                    }
                    else
                    {
                        //not exist

                        if(core.tbposettings.Any(y => y.enablePO == "1"))
                        {
                            core.update_settings(1, enableposettings, httprequest.Form["enablepoinventory"],
                           httprequest.Form["enablelistview"], httprequest.Form["viewentry"]);
                            return Request.CreateResponse(HttpStatusCode.OK, "update settings success");
                        }
                        else if(core.tbposettings.Any(x => x.enablePO == "0"))
                        {
                            core.update_settings(1, enableposettings, httprequest.Form["enablepoinventory"],
                          httprequest.Form["enablelistview"], httprequest.Form["viewentry"]);
                            return Request.CreateResponse(HttpStatusCode.OK, "update settings success");
                        }
                        else
                        {
                            tbposetting settings = new tbposetting();
                            settings.enablePO = enableposettings;
                            settings.enablePOInventory = httprequest.Form["enablepoinventory"];
                            settings.enableListview = httprequest.Form["enablelistview"];
                            settings.viewentry = httprequest.Form["viewentry"];
                            settings.updatedAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                            core.tbposettings.Add(settings);
                            core.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, "success settings");
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("update-inventory-settings"), HttpPost]
        public HttpResponseMessage updateinventsettings(string enableinvent, string enablesupplier, string enableexpiration)
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    if(core.tbinventsettings.Any(x => x.enableinventform == enableinvent))
                    {
                        core.update_invent_settings(1, enableinvent, enablesupplier, enableexpiration);
                        return Request.CreateResponse(HttpStatusCode.OK, "success update invent");
                    }
                    else
                    {
                        if(core.tbinventsettings.Any(x => x.enableinventform == "1"))
                        {
                            core.update_invent_settings(1, enableinvent, enablesupplier, enableexpiration);
                            return Request.CreateResponse(HttpStatusCode.OK, "success update invent");
                        }
                        else if(core.tbinventsettings.Any(x => x.enableinventform == "0"))
                        {
                            core.update_invent_settings(1, enableinvent, enablesupplier, enableexpiration);
                            return Request.CreateResponse(HttpStatusCode.OK, "success update invent");
                        }
                        else
                        {
                            tbinventsetting invent = new tbinventsetting();
                            invent.enableinventform = enableinvent;
                            invent.enablesupplier = enablesupplier;
                            invent.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                            invent.enableexpiration = enableexpiration;
                            core.tbinventsettings.Add(invent);
                            core.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, "success update invent");
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("get-inventory-settings"), HttpGet]
        public HttpResponseMessage getsettingsforinvent()
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    var obj = core.tbinventsettings.ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("get-po-settings"), HttpGet]
        public HttpResponseMessage getsettingsforpo()
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    var obj = core.tbposettings.ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
