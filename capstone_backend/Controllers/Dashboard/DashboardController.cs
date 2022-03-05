using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using capstone_backend.Models;
using capstone_backend.globalCON;

namespace capstone_backend.Controllers.Dashboard
{
    [RoutePrefix("api/dashboard")]
    public class DashboardController : ApiController
    {
        private dbbmEntities core;

        [Route("summary"), HttpGet]
        public HttpResponseMessage getDashboardSummary()
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    DateTime today = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd"));
                    DashboardSummaryDto dashboardSummary = new DashboardSummaryDto();

                    dashboardSummary.TotalProducts = core.stock_on_hand.Select(x => x.stockID)

                                                                           .Count();
                    dashboardSummary.SystemUsers = core.users.Select(x => x.isarchive == "0")

                                                            .Count();
                    
                    dashboardSummary.SalesToday = core.product_sales.Where(x => x.createdAt == today)
                                                                        .Count();

                    dashboardSummary.WarningProduct = core.stock_on_hand.Where(x => x.productquantity <= 50 ||
                                                                                    x.productquantity <= 30)

                                                                        .Count();

                    return Request.CreateResponse(HttpStatusCode.OK, dashboardSummary);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}