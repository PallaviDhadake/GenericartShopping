using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text;
using System.IO;

/// <summary>
/// Summary description for ShoppingWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class ShoppingWebService : System.Web.Services.WebService {
    iClass c = new iClass();
    public ShoppingWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }

    [WebMethod(EnableSession = true)]
    public void AdminLoginUpdate()
    {
        try
        {
            //System.Diagnostics.Debugger.Break();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    [WebMethod(EnableSession = true)]
    public string GetCity(string cityName)
    {
        //Set the Cookie value.
        HttpContext.Current.Response.Cookies["cityId"].Value = cityName;
        HttpContext.Current.Response.Cookies["cityId"].Expires = DateTime.Now.AddDays(30);
        return cityName;
    }


    [WebMethod(EnableSession = true)]
    public int UpdateCartList(int prodIdX, int qty, int optionId)
    {
        try
        {
            int custId = 0;
            if (HttpContext.Current.Session["genericCust"] != null)
                custId = Convert.ToInt32(HttpContext.Current.Session["genericCust"]);
            else
                custId = 0;

            double prodAmount = Convert.ToDouble(c.GetReqData("ProductsData", "PriceSale", "ProductID=" + prodIdX));
            double ordAmount = 0, finalOrdAmount = 0, incrementPrice = 0;
            int prodOptionId = 0;

            if (optionId > 0)
            {
                prodOptionId = Convert.ToInt32(c.GetReqData("ProductOptions", "ProdOptionID", "FK_ProductID=" + prodIdX + " AND FK_OptionID=" + optionId));
                incrementPrice = Convert.ToDouble(c.GetReqData("ProductOptions", "PriceIncrement", "ProdOptionID=" + prodOptionId));
                ordAmount = prodAmount + incrementPrice;
                finalOrdAmount = ordAmount * qty;
            }
            else
            {
                ordAmount = prodAmount;
                finalOrdAmount = ordAmount * qty;
            }

            //double finalOrdAmount = ordAmount * qty;
            string prodCode = c.GetReqData("ProductsData", "ProductSKU", "ProductID=" + prodIdX).ToString();

            //int prodOptionId = 0;
            //if (optionId > 0)
            //{
            //    prodOptionId = Convert.ToInt32(c.GetReqData("ProductOptions", "ProdOptionID", "FK_ProductID=" + prodIdX + " AND FK_OptionID=" + optionId));
                
            //}

            DateTime cDate = DateTime.Now;
            string currentDate = cDate.ToString("yyyy-MM-dd HH:mm:ss.fff");

            // orderStatus=0 > added to cart, 1 > Order placed (New/pending), 2 > Cancelled By Customer , 3 > Accepted, 4 > Denied, 5 > Processing, 6 > Shipped, 7 > Delivered
            int orderId = 0;
            
            //int gobpid = Convert.ToInt32(c.GetReqData("[dbo].[CustomersData]", "[FK_ObpID]", "[CustomrtID] = " + custId));
            // CONFUSION??????????????????????????????????????????????????? 

            if (HttpContext.Current.Request.Cookies["ordId"] != null)
            {
                string[] arrOrdId = HttpContext.Current.Request.Cookies["ordId"].Value.Split('#');
                orderId = Convert.ToInt32(arrOrdId[0].ToString());
            }
            else
            {
                orderId = c.NextId("OrdersData", "OrderID");
                object objGOBP = c.GetReqData("[dbo].[CustomersData]", "[FK_ObpID]", "[CustomrtID] = " + custId);

                // Check is this "FlashAid's" Customer with referance of CustomersData table "Customer_WebKey" column (12-sept-23)
                object objWebKey = c.GetReqData("[dbo].[CustomersData]", "[Customer_WebKey]", "[CustomrtID] = " + custId);

                if (objWebKey != null && objWebKey != DBNull.Value && objWebKey.ToString() != "")
                {
                    //Assign fixed GOBP id of 325 i.e. OBP00325 (FlashAid)
                    objGOBP = 325;
                }

                if (objGOBP != null && objGOBP != DBNull.Value && objGOBP.ToString() != "")
                {
                    c.ExecuteQuery("Insert Into OrdersData (OrderID, FK_OrderCustomerID, OrderDate, OrderStatus, OrderType, DeviceType, GOBPId) Values (" + orderId +
                    ", " + custId + ", '" + currentDate + "', 0, 1, 'Web'," + Convert.ToInt32(objGOBP) + ")");
                }
                else
                {
                    c.ExecuteQuery("Insert Into OrdersData (OrderID, FK_OrderCustomerID, OrderDate, OrderStatus, OrderType, DeviceType) Values (" + orderId +
                    ", " + custId + ", '" + currentDate + "', 0, 1, 'Web')");
                }

                string ordId = orderId.ToString() + "#" + custId.ToString();

                HttpContext.Current.Response.Cookies["ordId"].Value = ordId;
                HttpContext.Current.Response.Cookies["ordId"].Expires = DateTime.Now.AddDays(30);
            }

            int detailsMaxId = 0;
            if (!c.IsRecordExist("Select OrdDetailID From OrdersDetails Where FK_DetailOrderID=" + orderId + " AND FK_DetailProductID=" + prodIdX))
            {
                detailsMaxId = c.NextId("OrdersDetails", "OrdDetailID");

                c.ExecuteQuery("Insert Into OrdersDetails (OrdDetailID, FK_DetailOrderID, FK_DetailProductID, OrdDetailQTY, OrdDetailPrice, " +
                    " OrdDetailAmount, OrdDetailSKU)" +
                    " Values (" + detailsMaxId + ", " + orderId + ", " + prodIdX + ", " + qty + ", " + ordAmount +
                    ", " + finalOrdAmount + ", '" + prodCode + "')");
            }

            if (c.IsRecordExist("SELECT [OrderID] FROM [dbo].[OrdersData] WHERE [GOBPId] IS NOT NULL OR [GOBPId] <> '' OR [GOBPId] <> 0 AND [OrderID] = " + orderId))
            {                
                c.GOBP_ProductWise_CommissionCalc(detailsMaxId, orderId, prodIdX, qty);

                // Update GOBO Commission total to main OrdersData table also.
                c.GOBP_CommissionTotal(orderId);
            }

            if (optionId > 0)
            {
                if (!c.IsRecordExist("Select OrderOptionID From OrderProductOptions Where FK_OrdDetailID=" + detailsMaxId + " AND FK_ProdOptionID=" + prodIdX))
                {
                    int maxOrdOptionId = c.NextId("OrderProductOptions", "OrderOptionID");
                    c.ExecuteQuery("Insert Into OrderProductOptions (OrderOptionID, FK_OrdDetailID, FK_ProdOptionID, PriceProduct, " +
                        " PriceIncrement) Values (" + maxOrdOptionId + ", " + detailsMaxId + ", " + prodOptionId + ", " + prodAmount + ", " + incrementPrice + ")");
                }
            }

            return 1;

        }
        catch (Exception)
        {
            return 0;
        }
    }


    [WebMethod(EnableSession = true)]
    public int CartListCount()
    {
        try
        {
            HttpCookie reqCookie = HttpContext.Current.Request.Cookies["ordId"];
            if (reqCookie != null)
            {
                string[] arrItems = reqCookie.Value.Split('#');
                int orderId = Convert.ToInt32(arrItems[0].ToString());
                int count = Convert.ToInt32(c.GetReqData("OrdersDetails", "Count(FK_DetailProductID)", "FK_DetailOrderID=" + orderId));
                return count;
            }
            else
            {
                return 0;
            }
        }
        catch (Exception)
        {
            return 0;
        }
    }

    [WebMethod(EnableSession = true)]
    public int UpdateAddress(int addrIdX, int orderIdX)
    {
        try
        {
            c.ExecuteQuery("Update OrdersData Set FK_AddressId=" + addrIdX + " Where OrderID=" + orderIdX);
            return 1;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    [WebMethod(EnableSession = true)]
    public int UpdateEnqAddress(int addrIdX, int calcIdX)
    {
        try
        {
            c.ExecuteQuery("Update SavingCalc Set FK_AddressId=" + addrIdX + " Where CalcID=" + calcIdX);
            return 1;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    [WebMethod(EnableSession = true)]
    public int UpdateFavShop(int frIdX, int custIdX)
    {
        try
        {
            c.ExecuteQuery("Update CustomersData Set CustomerFavShop=" + frIdX + " Where CustomrtID=" + custIdX);
            return 1;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    //[WebMethod(EnableSession = true)]

    //public int AssignHourlyOrders()
    //{
    //    try
    //    {
    //        string currentHours = DateTime.Now.ToString("hh");
    //        string ampm = DateTime.Now.ToString("tt");

    //        if (Convert.ToInt32(currentHours) >= 11 && ampm == "AM")
    //        {
    //            return AssignOrders();
    //        }
    //        else
    //        {
    //            if (Convert.ToInt32(currentHours) >= 12 || Convert.ToInt32(currentHours) <= 8 && ampm == "PM")
    //            {
    //                return AssignOrders();
    //            }
    //            else
    //            {
    //                return -1;
    //            }
    //        }
            
    //    }
    //    catch (Exception)
    //    {
    //        return -1;
    //    }
    //}

    //private int AssignOrders()
    //{
    //    try
    //    {
    //        int successRows = 0;
    //        int frId = Convert.ToInt32(c.GetReqData("DefaultShop", "FranchId", "DsID=1"));

    //        using (DataTable dtOrd = c.GetDataTable("Select OrdAssignID, OrdAssignDate, " +
    //        " SUBSTRING(CONVERT(VARCHAR, OrdAssignDate, 100), 13, 2) as hr, SUBSTRING(CONVERT(VARCHAR, OrdAssignDate, 100), 16, 2) as min, " +
    //        " SUBSTRING(CONVERT(VARCHAR, OrdAssignDate, 100), 18, 2) as ampm, FK_OrderID, FK_FranchID, OrdAssignStatus, " +
    //        " OrdReAssign From OrdersAssign Where OrdAssignStatus=0 AND OrdReAssign=0 AND Fk_FranchID<>" + frId + " Order By FK_OrderID"))
    //        {
    //            if (dtOrd.Rows.Count > 0)
    //            {
    //                DateTime curDate = Convert.ToDateTime(DateTime.Now);
    //                foreach (DataRow row in dtOrd.Rows)
    //                {
    //                    int custId = Convert.ToInt32(c.GetReqData("OrdersData", "FK_OrderCustomerID", "OrderID=" + row["FK_OrderID"]));
    //                    object custFavShop = c.GetReqData("CustomersData", "CustomerFavShop", "CustomrtID=" + custId);
    //                    if (custFavShop != DBNull.Value && custFavShop != null && custFavShop.ToString() != "" && custFavShop.ToString() != "0")
    //                    {
    //                        if (row["FK_FranchID"].ToString() == custFavShop.ToString())
    //                        {
    //                            // it is customers fav shop order, ignore it
    //                        }
    //                    }
    //                    else
    //                    {
    //                        TimeSpan timeSince = curDate.Subtract(Convert.ToDateTime(row["OrdAssignDate"]));

    //                        double hours = timeSince.TotalHours;
    //                        int hoursLimit = 4;
    //                        //int hoursLimit = 0;

    //                        //if (Convert.ToInt32(row["hr"].ToString()) >= 8 && row["ampm"].ToString() == "pm")
    //                        //{
    //                        //    hoursLimit = 15;
    //                        //}
    //                        //else if (Convert.ToInt32(row["hr"].ToString()) >= 12 && Convert.ToInt32(row["hr"].ToString()) <= 10 && row["ampm"].ToString() == "am")
    //                        //{
    //                        //    hoursLimit = 15;
    //                        //}
    //                        //else
    //                        //{
    //                        //    hoursLimit = 1;
    //                        //}

    //                        if (hours >= hoursLimit)
    //                        {
    //                            // insert entry in OrderAssign table, it is directly assigned to default shop


    //                            if (!c.IsRecordExist("Select OrdAssignID from OrdersAssign Where FK_OrderID = " + row["FK_OrderID"] + " AND Fk_FranchID=" + frId + " AND OrdAssignStatus=0"))
    //                            {
    //                                c.ExecuteQuery("Update OrdersAssign Set OrdReAssign=1 Where FK_OrderID=" + row["FK_OrderID"]);
    //                                int maxId = c.NextId("OrdersAssign", "OrdAssignID");

    //                                c.ExecuteQuery("Insert Into OrdersAssign (OrdAssignID, OrdAssignDate, FK_OrderID, Fk_FranchID, OrdAssignStatus, " +
    //                                    " OrdReAssign) Values (" + maxId + ", '" + DateTime.Now + "', " + row["FK_OrderID"] + ", " + frId + ", 0, 0)");


    //                                successRows++;
    //                                string shopname = c.GetReqData("FranchiseeData", "FranchName", "FranchID=" + row["FK_FranchID"]).ToString();
    //                                string shopcode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + row["FK_FranchID"]).ToString();
    //                                //write all assigned details in notepad
    //                                string filePath = HttpContext.Current.Server.MapPath("~/hourly_assign_log.txt");
    //                                if (File.Exists(filePath))
    //                                {
    //                                    StreamWriter writer = new StreamWriter(filePath, true);
    //                                    StringBuilder strError = new StringBuilder();

    //                                    strError.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
    //                                    strError.Append(Environment.NewLine);
    //                                    strError.Append("Order Id : " + row["FK_OrderID"]);
    //                                    strError.Append(Environment.NewLine);
    //                                    strError.Append("Hours passed : " + hours.ToString("0.00"));
    //                                    strError.Append(Environment.NewLine);
    //                                    strError.Append("Assigned From : " + shopname + ", " + shopcode);
    //                                    strError.Append(Environment.NewLine);
    //                                    strError.Append(Environment.NewLine);
    //                                    strError.Append(Environment.NewLine);
    //                                    writer.Write(strError.ToString());
    //                                    writer.Flush();
    //                                    writer.Close();
    //                                }
    //                            }

    //                        }
    //                    }
    //                }
    //                return successRows;
    //            }
    //            else
    //            {
    //                return -1;
    //            }
    //        }
    //    }
    //    catch (Exception)
    //    {
    //        return -1;
    //    }
    //}




    //[WebMethod(EnableSession = true)]

    //public int AssignHourlyEnquiries()
    //{
    //    try
    //    {
    //        string currentHours = DateTime.Now.ToString("hh");
    //        string ampm = DateTime.Now.ToString("tt");

    //        if (Convert.ToInt32(currentHours) >= 11 && ampm == "AM")
    //        {
    //            return AssignEnquries();
    //        }
    //        else
    //        {
    //            if (Convert.ToInt32(currentHours) >= 12 || Convert.ToInt32(currentHours) <= 8 && ampm == "PM")
    //            {
    //                return AssignEnquries();
    //            }
    //            else
    //            {
    //                return -1;
    //            }
    //        }

    //    }
    //    catch (Exception)
    //    {
    //        return -1;
    //    }
    //}

    //private int AssignEnquries()
    //{
    //    try
    //    {
    //        int successEnqRows = 0;
    //        int frId = Convert.ToInt32(c.GetReqData("DefaultShop", "FranchId", "DsID=1"));

    //        using (DataTable dtOrd = c.GetDataTable("Select EnqAssignID, EnqAssignDate, " +
    //        " SUBSTRING(CONVERT(VARCHAR, EnqAssignDate, 100), 13, 2) as hr, SUBSTRING(CONVERT(VARCHAR, EnqAssignDate, 100), 16, 2) as min, " +
    //        " SUBSTRING(CONVERT(VARCHAR, EnqAssignDate, 100), 18, 2) as ampm, FK_CalcID, Fk_FranchID, EnqAssignStatus, " +
    //        " EnqReAssign From SavingEnqAssign Where EnqAssignStatus=0 AND EnqReAssign=0 AND Fk_FranchID<>" + frId + " Order By FK_CalcID"))
    //        {
    //            if (dtOrd.Rows.Count > 0)
    //            {
    //                DateTime curDate = Convert.ToDateTime(DateTime.Now);
    //                foreach (DataRow row in dtOrd.Rows)
    //                {
    //                    TimeSpan timeSince = curDate.Subtract(Convert.ToDateTime(row["EnqAssignDate"]));

    //                    double hours = timeSince.TotalHours;
    //                    int hoursLimit = 4;

    //                    if (hours >= hoursLimit)
    //                    {
    //                        // insert entry in SavingEnqAssign table, it is directly assigned to default shop


    //                        if (!c.IsRecordExist("Select EnqAssignID from SavingEnqAssign Where FK_CalcID = " + row["FK_CalcID"] + " AND Fk_FranchID=" + frId + " AND EnqAssignStatus=0"))
    //                        {
    //                            c.ExecuteQuery("Update SavingEnqAssign Set EnqReAssign=1 Where FK_CalcID=" + row["FK_CalcID"]);

    //                            int maxId = c.NextId("SavingEnqAssign", "EnqAssignID");
    //                            c.ExecuteQuery("Insert Into SavingEnqAssign (EnqAssignID, EnqAssignDate, FK_CalcID, Fk_FranchID, EnqAssignStatus, " +
    //                                " EnqReAssign) Values (" + maxId + ", '" + DateTime.Now + "', " + row["FK_CalcID"] + ", " + frId + ", 0, 0)");

    //                            successEnqRows++;

    //                            string shopname = c.GetReqData("FranchiseeData", "FranchName", "FranchID=" + row["Fk_FranchID"]).ToString();
    //                            string shopcode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + row["Fk_FranchID"]).ToString();
                                
    //                            //write all assigned details in notepad
    //                            string filePath = HttpContext.Current.Server.MapPath("~/hourly_enq_assign_log.txt");
    //                            if (File.Exists(filePath))
    //                            {
    //                                StreamWriter writer = new StreamWriter(filePath, true);
    //                                StringBuilder strError = new StringBuilder();

    //                                strError.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
    //                                strError.Append(Environment.NewLine);
    //                                strError.Append("Enquiry Id : " + row["FK_CalcID"]);
    //                                strError.Append(Environment.NewLine);
    //                                strError.Append("Hours passed : " + hours.ToString("0.00"));
    //                                strError.Append(Environment.NewLine);
    //                                strError.Append("Assigned From : " + shopname + ", " + shopcode);
    //                                strError.Append(Environment.NewLine);
    //                                strError.Append(Environment.NewLine);
    //                                strError.Append(Environment.NewLine);
    //                                writer.Write(strError.ToString());
    //                                writer.Flush();
    //                                writer.Close();
    //                            }
    //                        }

    //                    }
    //                }
    //                return successEnqRows;
    //            }
    //            else
    //            {
    //                return -1;
    //            }
    //        }
    //    }
    //    catch (Exception)
    //    {
    //        return -1;
    //    }
    //}
}
