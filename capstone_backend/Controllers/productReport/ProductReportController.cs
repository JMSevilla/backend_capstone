using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;
using capstone_backend.Models;
namespace capstone_backend.Controllers.productReport
{
    [RoutePrefix("api/report-problem")]
    public class ProductReportController : ApiController
    {
<<<<<<< HEAD
=======
        //private burgerdbEntities core;
>>>>>>> 9721cfa66296c4d6926767be1ac2f5f3bb89c400
        private local_dbbmEntities1 core;
        [Route("product-report"), HttpPost]
        public HttpResponseMessage reportproblem(int id, string supplieremail, string productname, string problem1, string problem2, string problem3, string problem4, string remarks, string supplier, string ponumber)
        {
            try
            {
                using (core = new local_dbbmEntities1())
                {
                    productreport report = new productreport();
                    report.problem1 = problem1;
                    report.problem2 = problem2;
                    report.problem3 = problem3;
                    report.problem4 = problem4;
                    report.remarks = remarks;
                    report.responsible = supplier;
                    report.ponumber = ponumber;
                    report.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                    core.productreports.Add(report);
                    core.SaveChanges();

                    core.stored_update_purchase_status(id, 2);
                    _ = sendEmail(problem1, problem2, problem3, problem4, remarks, supplier, supplieremail, productname);
                    return Request.CreateResponse(HttpStatusCode.OK, "success report");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("get-all-returns-order"), HttpGet]
        public HttpResponseMessage getreturnsorder()
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    var obj = core.productreports.ToList();
                    return Request.CreateResponse
                        (HttpStatusCode.OK, obj);
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse
                    (HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Route("remove-returns-order"), HttpPost]
        public HttpResponseMessage removereturns(int id)
        {
            if(id <= 0)
            {
                return Request.CreateResponse
                    (HttpStatusCode.OK, "invalid id");
            }
            else
            {
                using(core = new local_dbbmEntities1())
                {
                    var remover = core.productreports
                        .Where(x => x.id == id).FirstOrDefault();
                    core.Entry(remover).State
                        = System.Data.Entity.EntityState.Deleted;
                    core.SaveChanges();
                    return Request.CreateResponse
                        (HttpStatusCode.OK, "success");
                }
            }
        }
        [Route("get-product-by-ponumber"), HttpGet]
        public HttpResponseMessage getprodByPO(string ponumber)
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    var obj = core.puchase_orders
                        .Where(x => x.ponumber == ponumber).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, obj);
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse
                    (HttpStatusCode.BadRequest, ex.Message);
            }
        }
        public async Task sendEmail(string problem1, string problem2, string problem3, string problem4, string remarks, string supplier, string supplieremail, string productname)
        {
            try
            {

                if (problem1 == "undefined")
                {
                    problem1 = "";
                }
                 if (problem2 == "undefined") {
                    problem2 = "";
                }
                 if (problem3 == "undefined")
                {
                    problem3 = "";
                }
                 if (problem4 == "undefined")
                {
                    problem4 = "";

                }
                var message = new MailMessage();
                message.To.Add(new MailAddress("Burger Mania" + "<" + supplieremail.ToString() + ">"));
                message.From = new MailAddress("Product Return <devopsbyte60@gmail.com>");
                message.Subject = "Burger Mania Product Return";
                message.Body += "<!DOCTYPE html>" +
                    "<html>" +
                    "<head>" +
                    "<style>" +
                    "table {" +
                    "font-family: arial, sans-serif;" +
                    "border-collapse: collapse;" +
                    "width: 100%;" +
                    "}" +
                    "td, th {" +
                    "border: 1px solid #dddddd;" +
                    "text-align: left;" +
                    "padding: 8px;}" +
                    "tr:nth-child(even) {background-color: #dddddd;}" +
                    ".button {border: none;color: white;padding: 15px 32px;text-align: center;text-decoration: none;display: inline-block;font-size: 16px;margin: 4px 2px;cursor: pointer;}" +
                    ".button2 {background-color: #008CBA;}" +
                    "</style>" +
                    "</head>";
                message.Body += "<body>";
                message.Body += "<center>";
                message.Body += "<img src='https://cdn.dribbble.com/users/879147/screenshots/3630290/forgot_password.jpg?compress=1&resize=800x600' alt='No image' style='width: 50%; height: auto;' />'";
                message.Body += "<h1>Product Returns</h1>";
                message.Body += "<table>" +
                    "<tr>" +
                    "<th>Product Name</th>" +
                    "<th>Issue 1</th>" +
                    "<th>Issue 2</th>" +
                    "<th>Issue 3</th>" +
                    "<th>Issue 4</th>" +
                    "<th>Remarks</th>" +
                    "<th>Responsible</th>" +
                    "<th>Created</th>" +
                    "</tr>" +
                    "<tr>" +
                    "<td>" + productname.ToString() + "</td>" +
                    "<td>" + problem1.ToString() + "</td>" +
                    "<td>" + problem2.ToString() + "</td>" +
                    "<td>" + problem3.ToString() + "</td>" +
                    "<td>" + problem4.ToString() + "</td>" +
                    "<td>" + remarks.ToString() + "</td>" +
                    "<td>" + supplier.ToString() + "</td>" +
                    "<td>" + System.DateTime.Now.ToString("yyyy/MM/dd h:m:s") + "</td>" +
                    "</table>";
                message.Body += "</center>";
                message.Body += "</body>";
                message.Body += "</html>";
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message);
                    await Task.FromResult(0);
                }


            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
