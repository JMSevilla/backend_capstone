using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using capstone_backend.Models;
using ExcelDataReader;
using System.Web;
using System.Data;
using System.IO;

namespace capstone_backend.Controllers.InventoryAI
{
    [RoutePrefix("api/inventory-ai")]
    public class InventoryAIController : ApiController
    {
        private local_dbbmEntities core;
        //private dbbmEntities core;
        [Route("artificial-intel-auto-compute"), HttpPost]
        public HttpResponseMessage aicompute(bool valbool)
        {
            try
            {
                var httprequest = HttpContext.Current.Request;
                DataSet dsexcel = new DataSet();
                IExcelDataReader reader = null;
                HttpPostedFile inputFile = null;
                Stream FileStream = null;
                using(core = new local_dbbmEntities())
                {
                   if(valbool == true)
                    {
                        if (httprequest.Files.Count > 0)
                        {
                            inputFile = httprequest.Files[0];
                            FileStream = inputFile.InputStream;
                            if (inputFile != null && FileStream != null)
                            {
                                if (inputFile.FileName.EndsWith(".xls"))
                                {
                                    reader = ExcelReaderFactory.CreateBinaryReader(FileStream);
                                }
                                else if (inputFile.FileName.EndsWith(".xlsx"))
                                {
                                    reader = ExcelReaderFactory.CreateOpenXmlReader(FileStream);
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, "not supported");
                                }
                                dsexcel = reader.AsDataSet();
                                reader.Close();
                                if (dsexcel != null && dsexcel.Tables.Count > 0)
                                {
                                    DataTable inventorytable = dsexcel.Tables[0];
                                    for (int i = 0; i < inventorytable.Rows.Count; i++)
                                    {
                                        stock_on_hand aistocks = new stock_on_hand();
                                        aistocks.stockNumber = Convert.ToString(inventorytable.Rows[i][0]);
                                        aistocks.productname = Convert.ToString(inventorytable.Rows[i][1]);
                                        aistocks.productquantity = Convert.ToInt32(inventorytable.Rows[i][2]);
                                        aistocks.productprice = Convert.ToDecimal(inventorytable.Rows[i][3]);
                                        aistocks.product_total = Convert.ToInt32(inventorytable.Rows[i][3]) * Convert.ToInt32(inventorytable.Rows[i][2]);
                                        aistocks.productstatus = "0";
                                        aistocks.productcreator = "1";
                                        aistocks.productsupplier = Convert.ToString(inventorytable.Rows[i][4]);
                                        aistocks.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                                        aistocks.productcategory = Convert.ToString(inventorytable.Rows[i][5]);
                                        aistocks.productimgurl = "https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/No-Image-Placeholder.svg/1200px-No-Image-Placeholder.svg.png";
                                        core.stock_on_hand.Add(aistocks);
                                    }
                                    int output = core.SaveChanges();
                                    if (output > 0)
                                    {
                                        return Request.CreateResponse(HttpStatusCode.OK, "success import");
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.OK, "something went wrong");
                                    }
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, "selected file is empty");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, "invalid file");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "bad request");
                        }
                    }
                    else
                    {
                        if (httprequest.Files.Count > 0)
                        {
                            inputFile = httprequest.Files[0];
                            FileStream = inputFile.InputStream;
                            if (inputFile != null && FileStream != null)
                            {
                                if (inputFile.FileName.EndsWith(".xls"))
                                {
                                    reader = ExcelReaderFactory.CreateBinaryReader(FileStream);
                                }
                                else if (inputFile.FileName.EndsWith(".xlsx"))
                                {
                                    reader = ExcelReaderFactory.CreateOpenXmlReader(FileStream);
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, "not supported");
                                }
                                dsexcel = reader.AsDataSet();
                                reader.Close();
                                if (dsexcel != null && dsexcel.Tables.Count > 0)
                                {
                                    DataTable inventorytable = dsexcel.Tables[0];
                                    for (int i = 0; i < inventorytable.Rows.Count; i++)
                                    {
                                        product_inventory ainventory = new product_inventory();
                                        ainventory.productCode = Convert.ToString(inventorytable.Rows[i][0]);
                                        ainventory.productName = Convert.ToString(inventorytable.Rows[i][1]);
                                        ainventory.product_quantity = Convert.ToInt32(inventorytable.Rows[i][2]);
                                        ainventory.product_price = Convert.ToDecimal(inventorytable.Rows[i][3]);
                                        ainventory.product_total = Convert.ToInt32(inventorytable.Rows[i][3]) * Convert.ToInt32(inventorytable.Rows[i][2]);
                                        ainventory.product_status = "0";
                                        ainventory.product_creator = "1";
                                        ainventory.product_supplier = Convert.ToString(inventorytable.Rows[i][4]);
                                        ainventory.createdAt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy/MM/dd h:m:s"));
                                        ainventory.product_category = Convert.ToString(inventorytable.Rows[i][5]);
                                        ainventory.productimgurl = "https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/No-Image-Placeholder.svg/1200px-No-Image-Placeholder.svg.png";
                                        core.product_inventory.Add(ainventory);
                                    }
                                    int output = core.SaveChanges();
                                    if (output > 0)
                                    {
                                        return Request.CreateResponse(HttpStatusCode.OK, "success import");
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.OK, "something went wrong");
                                    }
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, "selected file is empty");
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, "invalid file");
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "bad request");
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("get-list-excel-save"), HttpGet]
        public HttpResponseMessage getexcellist()
        {
            try
            {
                using(core = new local_dbbmEntities())
                {
                    var obj = core.excelStorages.ToList();
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
