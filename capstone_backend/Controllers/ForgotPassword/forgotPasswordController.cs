using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;
using capstone_backend.globalCON;
using capstone_backend.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace capstone_backend.Controllers.ForgotPassword
{
    [RoutePrefix("api/forgot-password")]
    public class forgotPasswordController : ApiController
    {
        APISecurity secure = new APISecurity();
        //private local_dbbmEntities core;
        private dbbmEntities core;
        [Route("email-finder"), HttpPost]
        public HttpResponseMessage email_finder(string email)
        {
            try
            {
                using(core = apiglobalcon.publico)
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
        [Route("code-verification-entry"), HttpPut]
        public IHttpActionResult entryVerification(string email, string apicode)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var check = core.code_verifications.Where(x => x.g_email == email && x.isdone == "0").FirstOrDefault();
                    if(check != null)
                    {
                        if(check.sendattempts != 3)
                        {
                            check.sendattempts = check.sendattempts + 1;
                            check.vcode = apicode;
                            core.SaveChanges();
                            return Ok("success send");
                        }
                        else
                        {
                            return Ok("maximum 3");
                        }
                    }
                    else
                    {
                        
                        return Ok("post send");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("post-send-email"), HttpPost]
        public IHttpActionResult postSend(string email, string apicode)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    code_verifications code = new code_verifications();
                    code.vcode = apicode;
                    code.isdone = "0";
                    code.sendattempts = 1;
                    code.g_email = email;
                    code.validatedAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd"));
                    core.code_verifications.Add(code);
                    core.SaveChanges();
                    return Ok("success post");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("send-email"), HttpPost]
        public async Task sendEmail( string email, string vcode)
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
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("devopsbyte60@gmail.com", "09663147803miguel");
                    await smtp.SendMailAsync(message);
                    await Task.FromResult(0);
                }
                //var htmlContent = "";
                //var apikey = "SG.pku0MTEQSmu3RNponNoWeQ.uEEDzuT5AmTkIm0YfmoDWYRQENgMVLin3V02p4-c4tY";
                //var client = new SendGridClient(apikey);
                //var from = new EmailAddress("devopsbyte60@gmail.com", "Account Verification");
                //var to = new EmailAddress(email.ToString(), "Burger Mania");
                //var subject = "Verification Code";
                //var plainTextContent = "easy shit";
                //htmlContent += "<center>";
                //htmlContent += "<img src='https://cdn.dribbble.com/users/879147/screenshots/3630290/forgot_password.jpg?compress=1&resize=800x600' alt='No image' style='width: 50%; height: auto;' />'";
                //htmlContent += "<h1>This is your verification code : " + vcode.ToString() + "</h1>";
                //htmlContent += "</center>";
                //var msg = MailHelper.CreateSingleEmail(
                //    from,
                //    to,
                //    subject,
                //    plainTextContent,
                //    htmlContent
                //    );
                //await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        [Route("verify-code"), HttpGet]
        public IHttpActionResult getCode(string code , string email)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var check = core.code_verifications.Where(x => x.vcode == code && x.g_email == email && x.isdone == "0").FirstOrDefault();
                    if(check != null)
                    {
                        check.isdone = "1";
                        core.SaveChanges();
                        return Ok("success verification");
                    }
                    else
                    {
                        return Ok("invalid verification");
                    }
                    
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
                using(core = apiglobalcon.publico)
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
                using (core = apiglobalcon.publico)
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
        [Route("user-checker"), HttpGet]
        public IHttpActionResult userChecker(string email)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var checkuser = core.users.Where(x => x.email == email).FirstOrDefault();
                    if(checkuser != null)
                    {
                        return Ok("exist");
                    }
                    else
                    {
                        return Ok("not exist");
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
                using(core = apiglobalcon.publico)
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
        [Route("unlock-account"), HttpPut]
        public IHttpActionResult UnlockAccount(string email)
        {
            try
            {
                using(core = apiglobalcon.publico)
                {
                    var chk = core.users.Where(x => x.email == email).FirstOrDefault();
                    if(chk != null)
                    {
                        chk.isattemptCounter = 0;
                        chk.isattemptStatus = "0";
                        core.SaveChanges();
                    }
                    return Ok("success unlock");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
