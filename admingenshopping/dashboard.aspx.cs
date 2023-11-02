using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;

public partial class admingenshopping_dashboard : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] arrCounts = new string[15]; //9
    public string nearestShopMarkup, nearestShopMarkupEnq;
    protected void Page_Load(object sender, EventArgs e)
    {
        GetCount();
        //GetNearestFranchisee();
        //AssignRemainingOrders();
    }

    protected void GetCount()
    {
        try
        {
            // Prepare From & To Date range as parameter (28-Apr-2023)
            string dateRange = c.GetFinancialYear();
            string[] arrDateRange = dateRange.ToString().Split('#');
            DateTime myFromDate = Convert.ToDateTime(arrDateRange[0]);
            DateTime myToDate = Convert.ToDateTime(arrDateRange[1]);

            arrCounts[0] = c.returnAggregate("Select Count(a.OrderID) From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID  Where a.OrderStatus=1 AND a.OrderType=1 AND (b.CustomerFavShop IS NULL OR b.CustomerFavShop=0)").ToString(); //new orders
            arrCounts[8] = c.returnAggregate("Select Count(a.OrderID) From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID  Where a.OrderStatus=1 AND a.OrderType=1 AND (b.CustomerFavShop IS NOT NULL OR b.CustomerFavShop<>0)").ToString(); // new fav shop orders

            //arrCounts[6] = c.returnAggregate("Select Count(a.OrderID) From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID Where a.OrderStatus<>0 AND a.OrderType IS NOT NULL AND a.OrderType<>0").ToString();
            arrCounts[6] = c.returnAggregate("Select Count(a.OrderID) From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID Where (Convert(varchar(20), a.OrderDate, 112) >= Convert(varchar(20), CAST('" + myFromDate + "' AS DATETIME), 112)) " +
                " AND (Convert(varchar(20), a.OrderDate, 112) <= Convert(varchar(20), CAST('" + myToDate + "' AS DATETIME), 112)) AND a.OrderStatus<>0 AND a.OrderType IS NOT NULL AND a.OrderType<>0").ToString();


            arrCounts[1] = c.returnAggregate("Select Count(ProductID) From ProductsData Where delMark=0 AND ProductActive=1").ToString();
            arrCounts[2] = c.returnAggregate("Select Count(CustomrtID) From CustomersData Where delMark=0 AND CustomerActive=1").ToString();
            arrCounts[3] = c.returnAggregate("Select Count(MfgId) From Manufacturers Where delMark=0").ToString();
            arrCounts[4] = c.returnAggregate("Select Count(a.OrderID) From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID Where a.OrderStatus=1 AND a.OrderType=2 AND a.FK_OrderCustomerID IS NOT NULL AND a.FK_OrderCustomerID<>0 AND (b.CustomerFavShop IS NULL OR b.CustomerFavShop=0)").ToString(); // new rx orders
            arrCounts[9] = c.returnAggregate("Select Count(a.OrderID) From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID Where a.OrderStatus=1 AND a.OrderType=2 AND a.FK_OrderCustomerID IS NOT NULL AND a.FK_OrderCustomerID<>0 AND (b.CustomerFavShop IS NOT NULL OR b.CustomerFavShop<>0)").ToString(); // new fav shop rx orders
            arrCounts[5] = c.returnAggregate("Select Count(a.CalcID) From SavingCalc a Inner Join CustomersData b On a.FK_CustId=b.CustomrtID Where a.EnqStatus=1").ToString();

            arrCounts[7] = c.returnAggregate("Select Count(GMitraID) From GenericMitra Where GMitraStatus=0").ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;

        }
    }

    //private void GetNearestFranchisee()
    //{
    //    int franchRow = 0, successRows = 0;
    //    try
    //    {
    //        if (c.IsRecordExist("Select a.OrderID from OrdersData a Inner Join CustomersAddress b On a.FK_OrderCustomerID=b.AddressFKCustomerID Where a.OrderStatus=1"))
    //        {
    //            //using (DataTable dtFranchisee = c.GetDataTable("Select Distinct a.OrderID, a.FK_OrderCustomerID, b.AddressLatitude, b.AddressLongitude, b.AddressPincode, b.AddressState from OrdersData a Inner Join CustomersAddress b On a.FK_AddressId=b.AddressID Where a.OrderStatus=1 And b.AddressLatitude Is Not Null And b.AddressLongitude Is Not Null And b.AddressLatitude<>'' And b.AddressLongitude<>'' Order By a.OrderID"))
    //            using (DataTable dtFranchisee = c.GetDataTable("Select Distinct a.OrderID, a.FK_OrderCustomerID, b.AddressLatitude, b.AddressLongitude, b.AddressPincode, b.AddressState from OrdersData a Inner Join CustomersAddress b On a.FK_AddressId=b.AddressID Where a.OrderStatus=1 Order By a.OrderID"))
    //            {
    //                foreach (DataRow frRow in dtFranchisee.Rows)
    //                {
    //                    franchRow++;

    //                    object custFavShop = c.GetReqData("CustomersData", "CustomerFavShop", "CustomrtID=" + frRow["FK_OrderCustomerID"]);
    //                    if (custFavShop != DBNull.Value && custFavShop != null && custFavShop.ToString() != "" && custFavShop.ToString() != "0")
    //                    {
    //                        // it is customers fav shop order, assign this order to its fav shop

    //                        int favShopId = Convert.ToInt32(custFavShop);
    //                        if (!c.IsRecordExist("Select OrdAssignID from OrdersAssign Where FK_OrderID = " + frRow["OrderID"].ToString() + " AND Fk_FranchID=" + favShopId + " AND OrdAssignStatus=0"))
    //                        {
    //                            // set order status to 3 -> as it is accepted by admin 
    //                            c.ExecuteQuery("Update OrdersData Set OrderStatus=3 Where OrderID=" + frRow["OrderID"].ToString());

    //                            //insert entry in OrderAssign table, it is directly assigned to nearest shop

    //                            c.ExecuteQuery("Update OrdersAssign Set OrdReAssign=1 Where FK_OrderID=" + frRow["OrderID"].ToString());
    //                            int maxId = c.NextId("OrdersAssign", "OrdAssignID");

    //                            c.ExecuteQuery("Insert Into OrdersAssign (OrdAssignID, OrdAssignDate, FK_OrderID, Fk_FranchID, OrdAssignStatus, " +
    //                                " OrdReAssign) Values (" + maxId + ", '" + DateTime.Now + "', " + frRow["OrderID"].ToString() + ", " + favShopId + ", 0, 0)");

    //                            string filePath = HttpContext.Current.Server.MapPath("~/assign_log.txt");
    //                            if (File.Exists(filePath))
    //                            {
    //                                StreamWriter writer = new StreamWriter(filePath, true);
    //                                StringBuilder strError = new StringBuilder();

    //                                strError.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
    //                                strError.Append(Environment.NewLine);
    //                                strError.Append("Order Id : " + frRow["OrderID"].ToString());
    //                                strError.Append(Environment.NewLine);
    //                                strError.Append("Assigned to fav shop : " + c.GetReqData("FranchiseeData", "FranchName", "FranchID=" + favShopId).ToString() + ", " + c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + favShopId).ToString());
    //                                strError.Append(Environment.NewLine);
    //                                strError.Append(Environment.NewLine);
    //                                strError.Append(Environment.NewLine);
    //                                writer.Write(strError.ToString());
    //                                writer.Flush();
    //                                writer.Close();
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        double frLattitude = frRow["AddressLatitude"] != DBNull.Value && frRow["AddressLatitude"] != null && frRow["AddressLatitude"].ToString() != "" ? Convert.ToDouble(frRow["AddressLatitude"]) : 0.0;
    //                        double frLongitude = frRow["AddressLongitude"] != DBNull.Value && frRow["AddressLongitude"] != null && frRow["AddressLongitude"].ToString() != "" ? Convert.ToDouble(frRow["AddressLongitude"]) : 0.0;
    //                        string pincode = frRow["AddressPincode"].ToString();
    //                        string orderId = frRow["OrderID"].ToString();


    //                        // check state where orders come from
    //                        if (frRow["AddressState"] != DBNull.Value && frRow["AddressState"] != null && frRow["AddressState"].ToString() != "")
    //                        {
    //                            // if it's maharashtra, madhyaparadesh, telengana, odisha, kerala then find nearest shop and assign it
    //                            if (frRow["AddressState"].ToString().ToLower().Contains("maha") || frRow["AddressState"].ToString().ToLower().Contains("madhya") || frRow["AddressState"].ToString().ToLower().Contains("telan") || frRow["AddressState"].ToString().ToLower().Contains("odi") || frRow["AddressState"].ToString().ToLower().Contains("kerala"))
    //                            {
    //                                if (frLattitude == 0.0 || frLongitude == 0.0)
    //                                {
    //                                    int frId = Convert.ToInt32(c.GetReqData("DefaultShop", "FranchId", "DsID=1"));
    //                                    //AssignOrderToDefaultShop(Convert.ToInt32(orderId), frId, successRows, "Reason : No any latitude & logitude found", "");
    //                                }
    //                                else
    //                                {
    //                                    using (DataTable dtNearFr = (DataTable)c.NearestShop(frLattitude, frLongitude, 10.00, pincode))
    //                                    //using (DataTable dtNearFr = (DataTable)c.NearestShop(16.82409800, 74.65248584, 20.00, "416410"))
    //                                    {
    //                                        if (dtNearFr.Rows.Count > 0)
    //                                        {
    //                                            DataRow row = dtNearFr.Rows[0];
    //                                            int frId = Convert.ToInt32(row["FranchID"]);
    //                                            if (frId > 0)
    //                                            {
    //                                                string frName = c.GetReqData("FranchiseeData", "FranchName", "FranchID=" + frId).ToString();
    //                                                string frShopCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + frId).ToString();
    //                                                double km = Convert.ToDouble(row["KM"]);

    //                                                //arrShowData[0] = frId.ToString();
    //                                                //arrShowData[1] = km.ToString();
    //                                                if (!c.IsRecordExist("Select OrdAssignID from OrdersAssign Where FK_OrderID = " + orderId + " AND Fk_FranchID=" + frId + " AND OrdAssignStatus=0"))
    //                                                {
    //                                                    // set order status to 3 -> as it is accepted by admin 
    //                                                    c.ExecuteQuery("Update OrdersData Set OrderStatus=3 Where OrderID=" + orderId);

    //                                                    // insert entry in OrderAssign table, it is directly assigned to nearest shop

    //                                                    c.ExecuteQuery("Update OrdersAssign Set OrdReAssign=1 Where FK_OrderID=" + orderId);
    //                                                    int maxId = c.NextId("OrdersAssign", "OrdAssignID");

    //                                                    c.ExecuteQuery("Insert Into OrdersAssign (OrdAssignID, OrdAssignDate, FK_OrderID, Fk_FranchID, OrdAssignStatus, " +
    //                                                        " OrdReAssign) Values (" + maxId + ", '" + DateTime.Now + "', " + orderId + ", " + frId + ", 0, 0)");

    //                                                    successRows++;

    //                                                    //send sms to shop

    //                                                    string frCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + frId).ToString();
    //                                                    string mobNo = c.GetReqData("FranchiseeData", "FranchMobile", "FranchID=" + frId).ToString();
    //                                                    string pendingOrdCount = c.returnAggregate("Select Count(OrdAssignID) From OrdersAssign Where OrdAssignStatus=0 AND OrdReAssign=0 AND Fk_FranchID=" + frId).ToString();
    //                                                    string msgData = "Dear " + frCode + ", You have received new order Order No " + orderId + " from Genericart Mobile App.Total Pending Order is/are " + pendingOrdCount + " Thank you Genericart Medicine Store Wahi Kaam, Sahi Daam";
    //                                                    c.SendSMS(msgData, mobNo);
    //                                                    //c.SendSMS(msgData, "8408027474");

    //                                                    //write all assigned details in notepad

    //                                                    string filePath = HttpContext.Current.Server.MapPath("~/assign_log.txt");
    //                                                    if (File.Exists(filePath))
    //                                                    {
    //                                                        StreamWriter writer = new StreamWriter(filePath, true);
    //                                                        StringBuilder strError = new StringBuilder();

    //                                                        strError.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
    //                                                        strError.Append(Environment.NewLine);
    //                                                        strError.Append("Order Id : " + orderId);
    //                                                        strError.Append(Environment.NewLine);
    //                                                        strError.Append("Shop : " + frName);
    //                                                        strError.Append(Environment.NewLine);
    //                                                        strError.Append("Shop Code : " + frShopCode + " (KM : " + km + ")");
    //                                                        strError.Append(Environment.NewLine);
    //                                                        strError.Append(Environment.NewLine);
    //                                                        strError.Append(Environment.NewLine);
    //                                                        writer.Write(strError.ToString());
    //                                                        writer.Flush();
    //                                                        writer.Close();
    //                                                    }
    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                nearestShopMarkup = "";
    //                                                int defaultfrId = Convert.ToInt32(c.GetReqData("DefaultShop", "FranchId", "DsID=1"));
    //                                                //AssignOrderToDefaultShop(Convert.ToInt32(orderId), defaultfrId, successRows, "Reason : No any shop found in given pincode", "");
    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            // not found any nearest shop in 10km radius, then assign it to GMMH00001 (Default Shop)

    //                                            int frId = Convert.ToInt32(c.GetReqData("DefaultShop", "FranchId", "DsID=1"));
    //                                            //AssignOrderToDefaultShop(Convert.ToInt32(orderId), frId, successRows, "Reason : No any nearest shop found in 10km radius", "");
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                            else
    //                            {
    //                                // if it's out of maharashtra then assign it to GMMH0001(Default Shop)
    //                                int frId = Convert.ToInt32(c.GetReqData("DefaultShop", "FranchId", "DsID=1"));

    //                                //AssignOrderToDefaultShop(Convert.ToInt32(orderId), frId, successRows, "", frRow["AddressState"].ToString());
                                    
    //                                //========================================================================================================================================//
    //                                //string frName = c.GetReqData("FranchiseeData", "FranchName", "FranchID=" + frId).ToString();
    //                                //string frShopCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + frId).ToString();

    //                                //if (!c.IsRecordExist("Select OrdAssignID from OrdersAssign Where FK_OrderID = " + orderId + " AND Fk_FranchID=" + frId + " AND OrdAssignStatus=0"))
    //                                //{
    //                                //    // set order status to 3 -> as it is accepted by admin 
    //                                //    c.ExecuteQuery("Update OrdersData Set OrderStatus=3 Where OrderID=" + orderId);

    //                                //    // insert entry in OrderAssign table, it is directly assigned to nearest shop

    //                                //    c.ExecuteQuery("Update OrdersAssign Set OrdReAssign=1 Where FK_OrderID=" + orderId);
    //                                //    int maxId = c.NextId("OrdersAssign", "OrdAssignID");

    //                                //    c.ExecuteQuery("Insert Into OrdersAssign (OrdAssignID, OrdAssignDate, FK_OrderID, Fk_FranchID, OrdAssignStatus, " +
    //                                //        " OrdReAssign) Values (" + maxId + ", '" + DateTime.Now + "', " + orderId + ", " + frId + ", 0, 0)");

    //                                //    successRows++;

    //                                //    //write all assigned details in notepad

    //                                //    string filePath = HttpContext.Current.Server.MapPath("~/assign_log.txt");
    //                                //    if (File.Exists(filePath))
    //                                //    {
    //                                //        StreamWriter writer = new StreamWriter(filePath, true);
    //                                //        StringBuilder strError = new StringBuilder();

    //                                //        strError.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
    //                                //        strError.Append(Environment.NewLine);
    //                                //        strError.Append("Order Id : " + orderId);
    //                                //        strError.Append(Environment.NewLine);
    //                                //        strError.Append("Shop : " + frName);
    //                                //        strError.Append(Environment.NewLine);
    //                                //        strError.Append("Shop Code : " + frShopCode);
    //                                //        strError.Append(Environment.NewLine);
    //                                //        strError.Append("State : " + frRow["AddressState"].ToString());
    //                                //        strError.Append(Environment.NewLine);
    //                                //        strError.Append(Environment.NewLine);
    //                                //        strError.Append(Environment.NewLine);
    //                                //        writer.Write(strError.ToString());
    //                                //        writer.Flush();
    //                                //        writer.Close();
    //                                //    }
    //                                //}

    //                            }
    //                        }
    //                    }
    //                }
    //                if (successRows > 0)
    //                {
    //                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', '" + successRows + " Orders Routed Successfully..!! Just Now..!! <a href=\"" + Master.rootPath + "assign_log.txt\" target=\"_blank\">View Details</a>');", true);
    //                }

    //            }

    //        }

    //        // Auto Route Enquiries also
    //        int successEnqRow = 0;
    //        if (c.IsRecordExist("Select a.CalcID from SavingCalc a Inner Join CustomersAddress b On a.FK_CustId=b.AddressFKCustomerID Where a.EnqStatus=1 And b.AddressLatitude Is Not Null And b.AddressLongitude Is Not Null And b.AddressLatitude<>'' And b.AddressLongitude <>''"))
    //        {
    //            //using (DataTable dtEnqs = c.GetDataTable("Select Distinct a.CalcID, a.FK_CustId, b.AddressLatitude, b.AddressLongitude, b.AddressPincode, b.AddressState from SavingCalc a Inner Join CustomersAddress b On a.FK_AddressID=b.AddressID Where a.EnqStatus=1 And b.AddressLatitude Is Not Null And b.AddressLongitude Is Not Null And b.AddressLatitude<>'' And b.AddressLongitude<>'' Order By a.CalcID"))
    //            using (DataTable dtEnqs = c.GetDataTable("Select Distinct a.CalcID, a.FK_CustId, b.AddressLatitude, b.AddressLongitude, b.AddressPincode, b.AddressState from SavingCalc a Inner Join CustomersAddress b On a.FK_AddressID=b.AddressID Where a.EnqStatus=1 Order By a.CalcID"))
    //            {
    //                foreach (DataRow enqRow in dtEnqs.Rows)
    //                {
    //                    double frLattitude = enqRow["AddressLatitude"] != DBNull.Value && enqRow["AddressLatitude"] != null && enqRow["AddressLatitude"].ToString() != "" ? Convert.ToDouble(enqRow["AddressLatitude"]) : 0.0;
    //                    double frLongitude = enqRow["AddressLongitude"] != DBNull.Value && enqRow["AddressLongitude"] != null && enqRow["AddressLongitude"].ToString() != "" ? Convert.ToDouble(enqRow["AddressLongitude"]) : 0.0;
                        
    //                    string pincode = enqRow["AddressPincode"].ToString();
    //                    string enqId = enqRow["CalcID"].ToString();

    //                    // check state where enq come from
    //                    if (enqRow["AddressState"] != DBNull.Value && enqRow["AddressState"] != null && enqRow["AddressState"].ToString() != "")
    //                    {
    //                        // if it's maharashtra, madhyaparadesh, telengana, odisha, kerala then find nearest shop and assign it
    //                        if (enqRow["AddressState"].ToString().ToLower().Contains("maha") || enqRow["AddressState"].ToString().ToLower().Contains("madhya") || enqRow["AddressState"].ToString().ToLower().Contains("telan") || enqRow["AddressState"].ToString().ToLower().Contains("odi") || enqRow["AddressState"].ToString().ToLower().Contains("kerala"))
    //                        {
    //                            if (frLattitude == 0.0 || frLongitude == 0.0)
    //                            {
    //                                int frId = Convert.ToInt32(c.GetReqData("DefaultShop", "FranchId", "DsID=1"));
    //                                AssignEnquiryToDefaultShop(Convert.ToInt32(enqId), frId, successRows, "Reason : No any latitude & logitude found", "");
    //                            }
    //                            else
    //                            {
    //                                using (DataTable dtNearFr = (DataTable)c.NearestShop(frLattitude, frLongitude, 10.00, pincode))
    //                                {
    //                                    if (dtNearFr.Rows.Count > 0)
    //                                    {
    //                                        DataRow row = dtNearFr.Rows[0];
    //                                        int frId = Convert.ToInt32(row["FranchID"]);
    //                                        if (frId > 0)
    //                                        {
    //                                            string frName = c.GetReqData("FranchiseeData", "FranchName", "FranchID=" + frId).ToString();
    //                                            string frShopCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + frId).ToString();
    //                                            double km = Convert.ToDouble(row["KM"]);

    //                                            if (!c.IsRecordExist("Select EnqAssignID from SavingEnqAssign Where FK_CalcID = " + enqId + " AND Fk_FranchID=" + frId + " AND EnqAssignStatus=0"))
    //                                            {
    //                                                // set enq status to 2 -> as it is converted
    //                                                c.ExecuteQuery("Update SavingCalc Set EnqStatus=2 Where CalcID=" + enqId);
    //                                                // insert entry in EnqAssign table, it is directly assigned to nearest shop
    //                                                c.ExecuteQuery("Update SavingEnqAssign Set EnqReAssign=1 Where FK_CalcID=" + enqId);
    //                                                int maxId = c.NextId("SavingEnqAssign", "EnqAssignID");
    //                                                c.ExecuteQuery("Insert Into SavingEnqAssign (EnqAssignID, EnqAssignDate, FK_CalcID, Fk_FranchID, EnqAssignStatus, " +
    //                                                    " EnqReAssign) Values (" + maxId + ", '" + DateTime.Now + "', " + enqId + ", " + frId + ", 0, 0)");

    //                                                successEnqRow++;

    //                                                //write all assigned details in notepad

    //                                                string filePath = HttpContext.Current.Server.MapPath("~/enq_assign_log.txt");
    //                                                if (File.Exists(filePath))
    //                                                {
    //                                                    StreamWriter writer = new StreamWriter(filePath, true);
    //                                                    StringBuilder strError = new StringBuilder();

    //                                                    strError.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
    //                                                    strError.Append(Environment.NewLine);
    //                                                    strError.Append("Enquiry Id : " + enqId);
    //                                                    strError.Append(Environment.NewLine);
    //                                                    strError.Append("Shop : " + frName);
    //                                                    strError.Append(Environment.NewLine);
    //                                                    strError.Append("Shop Code : " + frShopCode + " (KM : " + km + ")");
    //                                                    strError.Append(Environment.NewLine);
    //                                                    strError.Append(Environment.NewLine);
    //                                                    strError.Append(Environment.NewLine);
    //                                                    writer.Write(strError.ToString());
    //                                                    writer.Flush();
    //                                                    writer.Close();
    //                                                }
    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            nearestShopMarkup = "";
    //                                            int defaultfrId = Convert.ToInt32(c.GetReqData("DefaultShop", "FranchId", "DsID=1"));
    //                                            AssignEnquiryToDefaultShop(Convert.ToInt32(enqId), defaultfrId, successRows, "Reason : No any shop found in given pincode", "");
    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        // not found any nearest shop in 10km radius, then assign it to GMMH00001 (Default Shop)

    //                                        int frId = Convert.ToInt32(c.GetReqData("DefaultShop", "FranchId", "DsID=1"));

    //                                        AssignEnquiryToDefaultShop(Convert.ToInt32(enqId), frId, successRows, "Reason : No any nearest shop found in 10km radius", "");
    //                                    }
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            // if it's out of maharashtra then assign it to GMMH0001(Default Shop)
    //                            int frId = Convert.ToInt32(c.GetReqData("DefaultShop", "FranchId", "DsID=1"));

    //                            AssignEnquiryToDefaultShop(Convert.ToInt32(enqId), frId, successRows, "", enqRow["AddressState"].ToString());
    //                        }
    //                    }
    //                }
    //                if (successEnqRow > 0)
    //                {
    //                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', '" + successEnqRow + " Enquiries Routed Successfully..!! Just Now..!! <a href=\"" + Master.rootPath + "enq_assign_log.txt\" target=\"_blank\">View Details</a>');", true);
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing " + franchRow + "');", true);
    //        c.ErrorLogHandler(this.ToString(), "GetNearestFranchisee", ex.Message.ToString());
    //        return;
    //    }
    //}


    //private void AssignOrderToDefaultShop(int ordIdX, int frIdX, int successRowsX, string reasonX, string stateX)
    //{
    //    try
    //    {
    //        string frName = c.GetReqData("FranchiseeData", "FranchName", "FranchID=" + frIdX).ToString();
    //        string frShopCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + frIdX).ToString();

    //        if (!c.IsRecordExist("Select OrdAssignID from OrdersAssign Where FK_OrderID = " + ordIdX + " AND Fk_FranchID=" + frIdX + " AND OrdAssignStatus=0"))
    //        {
    //            //// set order status to 3 -> as it is accepted by admin 
    //            //c.ExecuteQuery("Update OrdersData Set OrderStatus=3 Where OrderID=" + ordIdX);

    //            //// insert entry in OrderAssign table, it is directly assigned to nearest shop

    //            //c.ExecuteQuery("Update OrdersAssign Set OrdReAssign=1 Where FK_OrderID=" + ordIdX);
    //            //int maxId = c.NextId("OrdersAssign", "OrdAssignID");

    //            //c.ExecuteQuery("Insert Into OrdersAssign (OrdAssignID, OrdAssignDate, FK_OrderID, Fk_FranchID, OrdAssignStatus, " +
    //            //    " OrdReAssign) Values (" + maxId + ", '" + DateTime.Now + "', " + ordIdX + ", " + frIdX + ", 0, 0)");

    //            successRowsX++;

    //            //write all assigned details in notepad

    //            string filePath = HttpContext.Current.Server.MapPath("~/assign_log.txt");
    //            if (File.Exists(filePath))
    //            {
    //                StreamWriter writer = new StreamWriter(filePath, true);
    //                StringBuilder strError = new StringBuilder();

    //                strError.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
    //                strError.Append(Environment.NewLine);
    //                strError.Append("Order Id : " + ordIdX);
    //                strError.Append(Environment.NewLine);
    //                strError.Append("Shop : " + frName);
    //                strError.Append(Environment.NewLine);
    //                strError.Append("Shop Code : " + frShopCode);
    //                strError.Append(Environment.NewLine);
    //                if (reasonX.ToString() != "")
    //                {
    //                    strError.Append(reasonX);
    //                }
    //                if (stateX.ToString() != "")
    //                {
    //                    strError.Append("State : " + stateX.ToString());
    //                }
    //                strError.Append(Environment.NewLine);
    //                strError.Append(Environment.NewLine);
    //                strError.Append(Environment.NewLine);
    //                writer.Write(strError.ToString());
    //                writer.Flush();
    //                writer.Close();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
    //        c.ErrorLogHandler(this.ToString(), "AssignOrderToDefaultShop", ex.Message.ToString());
    //        return;
    //    }
    //}

    //private void AssignEnquiryToDefaultShop(int enqIdX, int frIdX, int successRowsX, string reasonX, string stateX)
    //{
    //    try
    //    {
    //        string frName = c.GetReqData("FranchiseeData", "FranchName", "FranchID=" + frIdX).ToString();
    //        string frShopCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + frIdX).ToString();

    //        if (!c.IsRecordExist("Select EnqAssignID from SavingEnqAssign Where FK_CalcID = " + enqIdX + " AND Fk_FranchID=" + frIdX + " AND EnqAssignStatus=0"))
    //        {
    //            // set enq status to 2 -> as it is converted
    //            c.ExecuteQuery("Update SavingCalc Set EnqStatus=2 Where CalcID=" + enqIdX);
    //            // insert entry in EnqAssign table, it is directly assigned to nearest shop
    //            c.ExecuteQuery("Update SavingEnqAssign Set EnqReAssign=1 Where FK_CalcID=" + enqIdX);
    //            int maxId = c.NextId("SavingEnqAssign", "EnqAssignID");
    //            c.ExecuteQuery("Insert Into SavingEnqAssign (EnqAssignID, EnqAssignDate, FK_CalcID, Fk_FranchID, EnqAssignStatus, " +
    //                " EnqReAssign) Values (" + maxId + ", '" + DateTime.Now + "', " + enqIdX + ", " + frIdX + ", 0, 0)");

    //            successRowsX++;

    //            //write all assigned details in notepad

    //            string filePath = HttpContext.Current.Server.MapPath("~/enq_assign_log.txt");
    //            if (File.Exists(filePath))
    //            {
    //                StreamWriter writer = new StreamWriter(filePath, true);
    //                StringBuilder strError = new StringBuilder();

    //                strError.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
    //                strError.Append(Environment.NewLine);
    //                strError.Append("Enquiry Id : " + enqIdX);
    //                strError.Append(Environment.NewLine);
    //                strError.Append("Shop : " + frName);
    //                strError.Append(Environment.NewLine);
    //                strError.Append("Shop Code : " + frShopCode);
    //                strError.Append(Environment.NewLine);
    //                if (reasonX.ToString() != "")
    //                {
    //                    strError.Append(reasonX);
    //                }
    //                if (stateX.ToString() != "")
    //                {
    //                    strError.Append("State : " + stateX.ToString());
    //                }
    //                strError.Append(Environment.NewLine);
    //                strError.Append(Environment.NewLine);
    //                strError.Append(Environment.NewLine);
    //                writer.Write(strError.ToString());
    //                writer.Flush();
    //                writer.Close();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
    //        c.ErrorLogHandler(this.ToString(), "AssignEnquiryToDefaultShop", ex.Message.ToString());
    //        return;
    //    }
    //}


    //private void AssignRemainingOrders()
    //{
    //    try
    //    {
    //        if (c.IsRecordExist("Select a.OrderID from OrdersData a Inner Join CustomersAddress b On a.FK_OrderCustomerID=b.AddressFKCustomerID Where a.OrderStatus=1"))
    //        {
    //            using (DataTable dtFranchisee = c.GetDataTable("Select Distinct a.OrderID, a.FK_OrderCustomerID, b.AddressLatitude, b.AddressLongitude, b.AddressPincode, b.AddressState from OrdersData a Inner Join CustomersAddress b On a.FK_AddressId=b.AddressID Where a.OrderStatus=1 Order By a.OrderID"))
    //            {
    //                if (dtFranchisee.Rows.Count > 0)
    //                {
    //                    //1. get shops from compnayownshops table where impOutlet=1 & not GMMH0001 (only mumbai shops)
    //                    int mumShopIndex = 0;
    //                    string mumShopIds = "";
    //                    using (DataTable dtMumbaiShops = c.GetDataTable("Select CSID, FK_FranchID From CompanyOwnShops Where ImpOutlet=1 AND DelMark=0 AND FK_FranchID<>24 Order By CSID ASC"))
    //                    {
    //                        if (dtMumbaiShops.Rows.Count > 0)
    //                        {
    //                            foreach (DataRow shRow in dtMumbaiShops.Rows)
    //                            {
    //                                if (mumShopIds == "")
    //                                    mumShopIds = shRow["FK_FranchID"].ToString();
    //                                else
    //                                    mumShopIds = mumShopIds + "," + shRow["FK_FranchID"].ToString();
    //                            }
    //                        }
    //                    }

    //                    //=================================================================================================================================================================//

    //                    //2. get all shops from compnayownshops table
    //                    int shopIndex = 0;
    //                    string shopIds = "";
    //                    using (DataTable dtCompShops = c.GetDataTable("Select CSID, FK_FranchID From CompanyOwnShops Where DelMark=0 Order By CSID ASC"))
    //                    {
    //                        if (dtCompShops.Rows.Count > 0)
    //                        {
    //                            foreach (DataRow coRow in dtCompShops.Rows)
    //                            {
    //                                if (shopIds == "")
    //                                    shopIds = coRow["FK_FranchID"].ToString();
    //                                else
    //                                    shopIds = shopIds + "," + coRow["FK_FranchID"].ToString();
    //                            }
    //                        }
    //                    }


    //                    string[] arrMumShops = mumShopIds.ToString().Split(',');
    //                    string[] arrCompShops = shopIds.ToString().Split(',');
    //                    int successRows = 0;
    //                    foreach (DataRow row in dtFranchisee.Rows)
    //                    {
    //                        // ignore fav shop orders
    //                        object custFavShop = c.GetReqData("CustomersData", "CustomerFavShop", "CustomrtID=" + row["FK_OrderCustomerID"]);
    //                        if (custFavShop != DBNull.Value && custFavShop != null && custFavShop.ToString() != "" && custFavShop.ToString() != "0")
    //                        {
    //                            // fav shop order, ignore it
    //                        }
    //                        else
    //                        {
    //                            // 1. Check if order pincode is of mumbai region, then assign it to mumbai shops
    //                            // 2. If order not from mumbai region, then assign it to random company own shops

    //                            if (Convert.ToInt32(row["AddressPincode"]) >= 400001 && Convert.ToInt32(row["AddressPincode"]) <= 401703)
    //                            {
    //                                // it is mumbai region order
    //                                if (mumShopIndex == arrMumShops.Length)
    //                                {
    //                                    mumShopIndex = 0;
    //                                }
    //                                else
    //                                {
    //                                    AssignOrderToDefaultShop(Convert.ToInt32(row["OrderID"]), Convert.ToInt32(arrMumShops[mumShopIndex]), successRows, "Mumbai Region Order", "");
    //                                    mumShopIndex++;
    //                                }
    //                            }
    //                            else
    //                            {
    //                                // other than mumbai order ex. 1. pincode not found order, 2. latlongs not found or latlongs 0, 
    //                                // 3. out of 10km radius order or 4. out of maharashtra state order
    //                                if (shopIndex == arrCompShops.Length)
    //                                {
    //                                    shopIndex = 0;
    //                                }
    //                                else
    //                                {
    //                                    string routeReason = "", state = "";
    //                                    double frLattitude = row["AddressLatitude"] != DBNull.Value && row["AddressLatitude"] != null && row["AddressLatitude"].ToString() != "" ? Convert.ToDouble(row["AddressLatitude"]) : 0.0;
    //                                    double frLongitude = row["AddressLongitude"] != DBNull.Value && row["AddressLongitude"] != null && row["AddressLongitude"].ToString() != "" ? Convert.ToDouble(row["AddressLongitude"]) : 0.0;

    //                                    if (row["AddressState"].ToString().ToLower().Contains("maha") || row["AddressState"].ToString().ToLower().Contains("madhya") || row["AddressState"].ToString().ToLower().Contains("telan") || row["AddressState"].ToString().ToLower().Contains("odi") || row["AddressState"].ToString().ToLower().Contains("kerala"))
    //                                    {
    //                                        if (frLattitude == 0.0 || frLongitude == 0.0)
    //                                        {
    //                                            routeReason = "Reason : No any latitude & logitude found";
    //                                        }
    //                                        else
    //                                        {
    //                                            using (DataTable dtNearFr = (DataTable)c.NearestShop(frLattitude, frLongitude, 10.00, row["AddressPincode"].ToString()))
    //                                            {
    //                                                if (dtNearFr.Rows.Count > 0)
    //                                                {
    //                                                    DataRow nrow = dtNearFr.Rows[0];
    //                                                    int frId = Convert.ToInt32(nrow["FranchID"]);
    //                                                    if (frId > 0)
    //                                                    {

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        routeReason = "Reason : No any shop found in given pincode";
    //                                                    }
    //                                                }
    //                                                else
    //                                                {
    //                                                    routeReason = "Reason : No any nearest shop found in 10km radius";
    //                                                }
    //                                            }
    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        routeReason = "Reason : Out of Maharashtra State Order";
    //                                        state = row["AddressState"].ToString();
    //                                    }
    //                                    AssignOrderToDefaultShop(Convert.ToInt32(row["OrderID"]), Convert.ToInt32(arrCompShops[shopIndex]), successRows, routeReason, state);
    //                                    shopIndex++;
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
    //        c.ErrorLogHandler(this.ToString(), "AssignRemainingOrders", ex.Message.ToString());
    //        return;
    //    }
    //}
}