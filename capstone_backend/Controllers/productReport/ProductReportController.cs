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
        private local_dbbmEntities core;
        [Route("product-report"), HttpPost]
        public HttpResponseMessage reportproblem(int id, string supplieremail, string productname, string problem1, string problem2, string problem3, string problem4, string remarks, string supplier)
        {
            try
            {
                using (core = new local_dbbmEntities())
                {
                    productreport report = new productreport();
                    report.problem1 = problem1;
                    report.problem2 = problem2;
                    report.problem3 = problem3;
                    report.problem4 = problem4;
                    report.remarks = remarks;
                    report.responsible = supplier;
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
        public async Task sendEmail(string problem1, string problem2, string problem3, string problem4, string remarks, string supplier, string supplieremail, string productname)
        {
            try
            {

                string FilePath = "C:\\Users\\devop\\source\\repos\\backend_capstone\\capstone_backend\\emailcontent\\emailtemplate.html";
                if(FilePath == null || FilePath == "" || FilePath == "undefined")
                {
                    FilePath = "Z:\\emailtemplate.html";
                }
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();
                if (problem1 == "undefined")
                {
                    MailText = MailText.Replace("[problem1]", "");
                    MailText = MailText.Replace("[problem2]", problem2);
                    MailText = MailText.Replace("[problem3]", problem3);
                    MailText = MailText.Replace("[problem4]", problem4);
                }
                else if (problem2 == "undefined") {
                    MailText = MailText.Replace("[problem1]", problem1);
                    MailText = MailText.Replace("[problem2]", "");
                    MailText = MailText.Replace("[problem3]", problem3);
                    MailText = MailText.Replace("[problem4]", problem4);
                }
                else if (problem3 == "undefined")
                {
                    MailText = MailText.Replace("[problem1]", problem1);
                    MailText = MailText.Replace("[problem2]", problem2);
                    MailText = MailText.Replace("[problem3]", "");
                    MailText = MailText.Replace("[problem4]", problem4);
                }
                else if (problem4 == "undefined")
                {
                    MailText = MailText.Replace("[problem1]", problem1);
                    MailText = MailText.Replace("[problem2]", problem2);
                    MailText = MailText.Replace("[problem3]", problem3);
                    MailText = MailText.Replace("[problem4]", "");
                }
                else
                {
                    MailText = MailText.Replace("[problem1]", problem1);
                    MailText = MailText.Replace("[problem2]", problem2);
                    MailText = MailText.Replace("[problem3]", problem3);
                    MailText = MailText.Replace("[problem4]", problem4);
                    MailText = MailText.Replace("[remarks]", remarks);
                }
                MailText = MailText.Replace("[ProductName]", productname);
                MailText = MailText.Replace("[DateCreated]", System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                var message = new MailMessage();
                message.To.Add(new MailAddress(supplier.ToString() + "<" + supplieremail.ToString() + ">"));
                message.From = new MailAddress("Product Return <devopsbyte60@gmail.com>");
                message.Subject = "Burger Mania Product Return";
                message.Body = MailText;
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
