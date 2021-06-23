using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using capstone_backend.Models;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
namespace capstone_backend.Controllers.Users
{
    [RoutePrefix("api/registration")]
    public class CSRFRegistrationController : ApiController
    {
        //connection
        private local_dbbmEntities core;
        class Response
        {
            public string response { get; set; }
        }
        user_data_management data = new user_data_management();
        Response resp = new Response();
        APISecurity secure = new APISecurity();
        [Route("registration-finalization"), HttpPost]
        public HttpResponseMessage storeuser()
        {

            try
            {
                var httprequest = HttpContext.Current.Request;
                using(core = new local_dbbmEntities())
                {
                    local_dbbmEntities coredb = new local_dbbmEntities();
                    //validate request here in backend
                    if(string.IsNullOrEmpty(httprequest.Form["firstname"]) || string.IsNullOrEmpty(httprequest.Form["lastname"]))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "empty1");
                    } //more backend validations here. assign to jastine or sherilyn
                    else
                    {
                        string emailfinal = httprequest.Form["email"];
                        var checker = core.users.Any(x => x.email == emailfinal);
                        var checker2 = core.users.Any(y => y.istype == "1");
                        if (checker)
                        {
                            //if email is already exist
                            return Request.CreateResponse(HttpStatusCode.OK, "exist");
                        }
                        else
                        {
                            if (checker2)
                            {
                                //customer registration insert
                                customer(
                                     httprequest.Form["firstname"], httprequest.Form["lastname"],
                                     httprequest.Form["municipality"], httprequest.Form["province"],
                                     httprequest.Form["address"], httprequest.Form["company_name"],
                                     httprequest["address_type"], httprequest.Form["email"],
                                     secure.Encrypt(httprequest.Form["password"]), Convert.ToInt32(httprequest.Form["mobileno"]),
                                     Convert.ToChar("0"), Convert.ToChar("1"),
                                     Convert.ToChar("1"), Convert.ToChar("1")
                                     );
                                user_google_allow allow = new user_google_allow();
                                allow.g_email = httprequest.Form["email"];
                                coredb.user_google_allow.Add(allow);
                                coredb.SaveChanges();
                                return Request.CreateResponse(HttpStatusCode.OK, "customer");
                            }
                            else
                            {
                                //admin insert
                                admin(
                                    httprequest.Form["firstname"], httprequest.Form["lastname"],
                                    httprequest.Form["municipality"], httprequest.Form["province"],
                                    httprequest.Form["address"], httprequest.Form["company_name"],
                                    httprequest["address_type"], httprequest.Form["email"],
                                    secure.Encrypt(httprequest.Form["password"]), Convert.ToInt32(httprequest.Form["mobileno"]),
                                    Convert.ToChar("1"), Convert.ToChar("1"),
                                    Convert.ToChar("1"), Convert.ToChar("1")
                                    );
                                coredb.update_sendAttempts(httprequest.Form["email"], null, "done_verified"); //done verified
                                user_google_allow allow = new user_google_allow();
                                allow.g_email = httprequest.Form["email"];
                                coredb.user_google_allow.Add(allow);
                                coredb.SaveChanges();
                                return Request.CreateResponse(HttpStatusCode.OK, "admin done");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        //admin
        public HttpResponseMessage admin(string firstname, string lastname, string municipality, string province,
            string address, string company_name, string address_type, string email, string password, int mobileno,
            char istype, char isverified, char isstatus, char is_google_verified)
        {
            try
            {
                using(core = new local_dbbmEntities())
                {
                    
                    core.stored_user_registration(data.firstname = firstname, data.lastname = lastname,
                        data.municipality = municipality, data.province = province,
                        data.address = address, data.company_name = company_name, data.address_type = address_type,
                        data.email = email, data.password = password, data.mobileno = mobileno,
                        Convert.ToString(istype), Convert.ToString(isverified), Convert.ToString(isstatus),
                         Convert.ToString(is_google_verified), "register");
                    resp.response = "success admin";
                    return Request.CreateResponse(HttpStatusCode.OK, resp);
                }
            }
            catch (Exception aa)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, aa.Message);
            }
        }
        //customer function
        public HttpResponseMessage customer(string firstname, string lastname, string municipality, string province,
           string address, string company_name, string address_type, string email, string password, int mobileno,
           char istype, char isverified, char isstatus, char is_google_verified)
        {
            try
            {
                using (core = new local_dbbmEntities())
                {
                    string Upassword;
                    core.stored_user_registration(data.firstname = firstname, data.lastname = lastname,
                        data.municipality = municipality, data.province = province,
                        data.address = address, data.company_name = company_name, data.address_type = address_type,
                        data.email = email, Upassword = password, data.mobileno = mobileno,
                        Convert.ToString(istype), Convert.ToString(isverified), Convert.ToString(isstatus),
                         Convert.ToString(is_google_verified), "register");
                    resp.response = "success admin";
                    return Request.CreateResponse(HttpStatusCode.OK, resp);
                }
            }
            catch (Exception aa)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, aa.Message);
            }
        }
        ///// check email if exist upon verification fill up
        [Route("check-verification"), HttpPost]
        public HttpResponseMessage verificationChecker(string email, string code)
        {
            try
            {
                var request_receiver = HttpContext.Current.Request;
                using(local_dbbmEntities coredb = new local_dbbmEntities())
                {
                    var checksend = coredb.code_verifications
                        .Any(y => y.isdone == "1" && y.g_email == email);
                    if (checksend)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "already verified");
                    }
                    else
                    {
                        if (coredb.code_verifications.Any(x => x.g_email == email))
                        {
                            //check the send attempts and if it is already verified
                            if (coredb.code_verifications.Any(z => z.sendattempts == "3" && z.g_email == email))
                            {
                                //exceed limit of 3 send
                                return Request.CreateResponse(HttpStatusCode.OK, "exceed");
                            }
                            else
                            {
                                //send another email
                                //update +1 the send attempts
                                coredb.update_sendAttempts(email, code, "update_sendattempts"); //updates the send attempts and verification code.
                                
                                _ = sendEmail(request_receiver.Form["firstname"], email, code);
                                return Request.CreateResponse(HttpStatusCode.OK, "sendsuccess1");
                            }
                        }
                        else
                        {
                            //send first email to customer email
                            _ = sendEmail(request_receiver.Form["firstname"], email, code);
                            addverification(email, code);
                            return Request.CreateResponse(HttpStatusCode.OK, "sendsuccess");
                        }
                    }
                }
            }
            catch (Exception xd)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, xd.Message);
            }
        }
        public HttpResponseMessage addverification(string email, string vcode)
        {
            try
            {
                using(core = new local_dbbmEntities())
                {
                    code_verifications coda = new code_verifications();
                    coda.g_email = email;
                    coda.vcode = vcode;
                    coda.isdone = "0";
                    coda.sendattempts = "1";
                    coda.validatedAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                    core.code_verifications.Add(coda);
                    core.SaveChanges();
                    resp.response = "success";
                    return Request.CreateResponse(HttpStatusCode.OK, resp.response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task sendEmail(string name, string email, string vcode)
        {
            try
            {
                var message = new MailMessage();
                message.To.Add(new MailAddress(name.ToString() + "<" + email.ToString() + ">"));
                message.From = new MailAddress("Account Verification <devopsbyte60@gmail.com>");
                message.Subject = "Verification Code";
                message.Body += "<center>";
                message.Body += "<img src='https://cdn.dribbble.com/users/458522/screenshots/14007167/media/214f6fa81fbd40f3b65b2cb747393226.png?compress=1&resize=1200x900' alt='No image' style='width: 50%; height: auto;' />'";
                message.Body += "<h1>This is your verification code : " + vcode.ToString() + "</h1>";
                message.Body += "</center>";
                message.IsBodyHtml = true;
                using(var smtp = new SmtpClient())
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
        [Route("store-user-001"), HttpPost]
        public HttpResponseMessage checkout(string email, string vcode)
        {
            try
            {
                using(core = new local_dbbmEntities())
                {
                    var check_code = core.code_verifications.Any(x => x.g_email == email && x.vcode == vcode);
                    if (check_code)
                    {
                        
                        return Request.CreateResponse(HttpStatusCode.OK, "valid code");
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
    }
}
