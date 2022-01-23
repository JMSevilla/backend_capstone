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
        private local_dbbmEntities2 core;

        [Route("summary"), HttpGet]
        public HttpResponseMessage getDashboardSummary()
        {
            try
            {
                using (core = apiglobalcon.publico)
                {
                    DashboardSummaryDto dashboardSummary = new DashboardSummaryDto();

                    dashboardSummary.TotalProducts = core.product_inventory.Select(x => x.productID)
                                                                           .Count();
                    dashboardSummary.SystemUsers = core.users.Select(x => x.id)
                                                            .Count();
                    dashboardSummary.SalesToday = core.product_inventory.Select(x => x.productID)
                                                                        .Count();
                    dashboardSummary.WarningProduct = core.stock_on_hand.Where(x => x.productquantity > 0 &&
                                                                                    x.productquantity < 10)
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