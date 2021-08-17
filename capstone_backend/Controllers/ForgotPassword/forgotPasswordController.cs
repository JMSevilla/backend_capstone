using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;
using capstone_backend.Models;
namespace capstone_backend.Controllers.ForgotPassword
{
    [RoutePrefix("api/forgot-password")]
    public class forgotPasswordController : ApiController
    {
        APISecurity secure = new APISecurity();
        private local_dbbmEntities1 core;
        [Route("email-finder"), HttpPost]
        public HttpResponseMessage email_finder(string email)
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    if(core.users.Any(x => x.email == email))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "exist");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "not exist");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("verification-email-send"), HttpPost]
        public async Task sendEmail(string email, string vcode)
        {
            try
            {
                var message = new MailMessage();
                message.To.Add(new MailAddress("Burger Mania" + "<" + email.ToString() + ">"));
                message.From = new MailAddress("Account Verification <devopsbyte60@gmail.com>");
                message.Subject = "Verification Code";
                message.Body += "<center>";
                message.Body += "<img src='https://cdn.dribbble.com/users/879147/screenshots/3630290/forgot_password.jpg?compress=1&resize=800x600' alt='No image' style='width: 50%; height: auto;' />'";
                message.Body += "<h1>This is your verification code : " + vcode.ToString() + "</h1>";
                message.Body += "</center>";
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
        [Route("add-forgot-history"), HttpPost]
        public HttpResponseMessage addhistoryforgot(string vcode)
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    forgotpassword_identifier forgot = new forgotpassword_identifier();
                    forgot.forgotcode = vcode;
                    forgot.isdone = "0";
                    forgot.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                    core.forgotpassword_identifier.Add(forgot);
                    core.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "success forgot history");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("identify-code"), HttpPost]
        public HttpResponseMessage identify_code_forgot(string vcode)
        {
            try
            {
                using (core = new local_dbbmEntities1())
                {
                    if (core.forgotpassword_identifier.Any(x => x.forgotcode == vcode && x.isdone == "0"))
                    {
                        //update isdone to 1
                        core.sproc_update_isdone_forgotpassword(vcode, 1);
                        return Request.CreateResponse(HttpStatusCode.OK, "success isdone");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "invalid code");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("change-password"), HttpPost]
        public HttpResponseMessage change_password(string password, string email)
        {
            try
            {
                using(core = new local_dbbmEntities1())
                {
                    core.change_password_changer(email, secure.Encrypt(password), 1);
                    return Request.CreateResponse(HttpStatusCode.OK, "success change");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
