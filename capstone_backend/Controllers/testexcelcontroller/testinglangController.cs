using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ExcelDataReader;
using capstone_backend.Models;
namespace capstone_backend.Controllers.testexcelcontroller
{
    [RoutePrefix("api/excel-test-lang")]
    public class testinglangController : ApiController
    {
        private local_dbbmEntities core;
        [Route("filepost"), HttpPost]
        public HttpResponseMessage postfile()
        {
            try
            {
                string message = "";
                var httpreq = HttpContext.Current.Request;
                DataSet dsexcel = new DataSet();
                IExcelDataReader reader = null;
                HttpPostedFile inputfile = null;
                Stream FileStream = null;

                using(core = new local_dbbmEntities())
                {
                    if(httpreq.Files.Count > 0)
                    {
                        inputfile = httpreq.Files[0];
                        FileStream = inputfile.InputStream;

                        if(inputfile != null && FileStream != null)
                        {
                            if (inputfile.FileName.EndsWith(".xls"))
                            {
                                reader = ExcelReaderFactory.CreateBinaryReader(FileStream);
                            }
                            else if(inputfile.FileName.EndsWith(".xlsx"))
                            {
                                reader = ExcelReaderFactory.CreateOpenXmlReader(FileStream);
                            }
                            else
                            {
                                message = "The file format is not supported";
                                return Request.CreateResponse(HttpStatusCode.OK, message);
                            }
                            dsexcel = reader.AsDataSet();
                            reader.Close();

                            if(dsexcel != null && dsexcel.Tables.Count > 0)
                            {
                                DataTable tbtesttable = dsexcel.Tables[0];
                                for(int i = 0; i < tbtesttable.Rows.Count; i++)
                                {
                                    tbexceltest tbtest = new tbexceltest();
                                    tbtest.fname = Convert.ToString(tbtesttable.Rows[i][0]);
                                    tbtest.lname = Convert.ToString(tbtesttable.Rows[i][1]);
                                    core.tbexceltests.Add(tbtest);
                                }
                                int output = core.SaveChanges();
                                if(output > 0)
                                {
                                    message = "Successfully uploaded";
                                    return Request.CreateResponse(HttpStatusCode.OK, message);
                                }
                                else
                                {
                                    message = "Something went wrong";
                                    return Request.CreateResponse(HttpStatusCode.OK, message);
                                }
                            }
                            else
                            {
                                message = "Selected file is empty";
                                return Request.CreateResponse(HttpStatusCode.OK, message);
                            }
                        }
                        else
                        {
                            message = "Invalid file";
                            return Request.CreateResponse(HttpStatusCode.OK, message);
                        }
                    }
                    else
                    {
                        message = "BadRequest boi";
                        return Request.CreateResponse(HttpStatusCode.OK, message);
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
