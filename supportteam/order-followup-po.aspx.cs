using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Razorpay.Api;
using System.Drawing;

public partial class supportteam_order_followup_po : System.Web.UI.Page
{
    iClass c = new iClass();
    public string pgTitle, err, followuplink;
    public string[] arrCust =new string[30];
    public string[] lastCall = new string[30];
    public string[] OrdDeatils = new string[30];
    public string[] OrdProduct = new string[30];

    protected void Page_Load(object sender, EventArgs e)
    {       
        if (!IsPostBack)
        {

            object OrderId = c.GetReqData("OrdersData", "TOP 1 OrderID", "FK_OrderCustomerID=" + Request.QueryString["custid"] + " Order By OrderDate Desc");

            using (DataTable dtOrdDetils = c.GetDataTable("Select FK_DetailProductID From [OrdersDetails] where FK_DetailOrderID=" + OrderId))
            {

                //int i = 0;

                if (dtOrdDetils.Rows.Count > 0)
                {

                    List<string> ordProductList = new List<string>(OrdProduct);

                    // Loop through the DataTable and add the FK_DetailProductID to the list
                    foreach (DataRow row in dtOrdDetils.Rows)
                    {
                        string productId = row["FK_DetailProductID"].ToString();
                        ordProductList.Add(productId);
                    }

                    //dtOrdDetils = OrdProduct.ToArray();
                    OrdProduct = ordProductList.ToArray();

                }
            }


            FillGrid();
            AddDefaultFirstRecord();
            FillQuantity();
            FillDDR();
            Bind_CustomerAddress();
        }

        ShowCustomersData();
        GetlastCallDeatils();
        GetLastOrder();

        //if (Request.QueryString["custId"] != null)
        //{
        //    if (c.IsRecordExist("SELECT [AddressID] FROM [dbo].[CustomersAddress] WHERE [AddressFKCustomerID] = " + Request.QueryString["custId"]))
        //    {
        //        c.FillComboBox("[AddressFull]", "[AddressID]", "[dbo].[CustomersAddress]", "[AddressFKCustomerID] = " + Request.QueryString["custId"] + "", "[AddressFull]", 0, ddrAddress);
        //    }
        //    else
        //    {
        //        existingAddr.Visible = false;
        //    }
        //}

        followuplink = "order-followup-po-history.aspx?&custId=" + Request.QueryString["custId"];
    }

    private void ShowCustomersData()
    {
        CustomersData custinfo = new CustomersData();

        int CustIdx = Convert.ToInt32(Request.QueryString["custId"]);

        custinfo.CustomresInfo(CustIdx);

        arrCust[0] = custinfo.CustomerName;
        arrCust[1] = custinfo.CustomerMobile;

        custinfo.FranchaiseeData(custinfo.CustomerFavShop);

       // object FavShopCity = c.GetReqData("CityData", "CityName", "CityID=" + custinfo.FK_FranchCityId);

        string FavShopCity = (custinfo.FK_FranchCityId != 0)
                             ? c.GetReqData("CityData", "CityName", "CityID=" + custinfo.FK_FranchCityId).ToString()
                             : "NA";
        string FranchName = !string.IsNullOrEmpty(custinfo.FranchName) ? custinfo.FranchName : "Not Found";

        string FranchShopCode = !string.IsNullOrEmpty(custinfo.FranchShopCode) ? custinfo.FranchShopCode : "Not Found";

        arrCust[2] = FranchName + " ( ShopCode - " + FranchShopCode + " ) , City - " + FavShopCity + "";

        custinfo.CustOrdersSatus(CustIdx);

        string TotalOrders = !string.IsNullOrEmpty(custinfo.Total_Orders) ? custinfo.Total_Orders : "NA";
        string Delivered = !string.IsNullOrEmpty(custinfo.Delivered) ? custinfo.Delivered : "NA";
        string InProcess = !string.IsNullOrEmpty(custinfo.InProcess) ? custinfo.InProcess : "NA";
        string Cancelled = !string.IsNullOrEmpty(custinfo.Cancelled) ? custinfo.Cancelled : "NA";

        arrCust[3] = TotalOrders;
        arrCust[4] = Delivered;
        arrCust[5] = InProcess;
        arrCust[6] = Cancelled;

        //OBP Deatils
        custinfo.OBPDataInfo(custinfo.FK_ObpID);

        if (custinfo.FK_ObpID == 0)
        {
            nogobp.Visible = true;
            arrCust[10] = "No GOBP Found";
            
        }
        else
        {
            mobno.Visible = true;
            string ObpUserId = custinfo.FK_ObpID == 0 ? "NA" : custinfo.OBP_UserID;
            arrCust[7] = custinfo.OBP_ApplicantName;
            arrCust[8] = custinfo.OBP_MobileNo;
            arrCust[9] = "( " + ObpUserId + " )";
        }        
    }

    void Bind_CustomerAddress()
    {
        try
        {
            SqlConnection con = new SqlConnection(c.OpenConnection());
            SqlCommand cmd = new SqlCommand("SELECT [AddressID],[AddressFull] FROM [dbo].[CustomersAddress] WHERE [AddressFKCustomerID] = @CustId", con);
            cmd.Parameters.AddWithValue("@CustId", Request.QueryString["custId"]);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt1 = new DataTable();
            sda.Fill(dt1);
            ddrAddress.Items.Clear();
            ddrAddress.Items.Add(new ListItem("--Select Address --", "0"));
            ddrAddress.DataSource = dt1;
            ddrAddress.DataValueField = "AddressID"; ddrAddress.DataTextField = "AddressFull";
            ddrAddress.DataBind();

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "Bind_DistrictDropDown", ex.Message.ToString());
            return;
        }
    }

    private void GetlastCallDeatils()
    {
        try
        {
            int custIdX = Convert.ToInt32(Request.QueryString["custId"]);
            using (DataTable dtFollowupOrd = c.GetDataTable("Select Top 1 FlupID, FlupDate, FK_TeamMemberId, FlupRemarkStatusID, FlupRemarkStatus, Convert(Varchar(20), FlupNextDate,103) as FlupNextDate, FlupNextTime From FollowupOrders where FK_CustomerId=" + custIdX + " Order By FlupDate Desc"))
            {
                if (dtFollowupOrd.Rows.Count > 0)
                {
                    DataRow row = dtFollowupOrd.Rows[0];

                    DateTime date = Convert.ToDateTime(row["FlupDate"].ToString());
                    string timeString = date.ToString("h:mm:ss tt");

                    DateTime FlupDate = Convert.ToDateTime(row["FlupDate"].ToString());
                    string dateOnly = FlupDate.ToString("dd/MM/yyyy");

                    string CallByPersonName = (row["FK_TeamMemberId"] != DBNull.Value && row["FK_TeamMemberId"] != null && row["FK_TeamMemberId"].ToString() != "")
                                              ? c.GetReqData("SupportTeam", "TeamPersonName", "TeamID=" + row["FK_TeamMemberId"]).ToString()
                                              : "NA";
                    string Remark = (row["FlupRemarkStatusID"] != DBNull.Value && row["FlupRemarkStatusID"] != null && row["FlupRemarkStatusID"].ToString() != "")
                                    ? c.GetReqData("FollowupOrdersStatus", "FL_Status", "FL_StatusID=" + row["FlupRemarkStatusID"]).ToString()
                                    : "NA";
                    lastCall[0] = dateOnly;
                    lastCall[1] = timeString;
                    lastCall[2] = CallByPersonName.ToString();
                    lastCall[3] = row["FlupRemarkStatus"] != DBNull.Value && row["FlupRemarkStatus"] != null && row["FlupRemarkStatus"].ToString() != "" ? row["FlupRemarkStatus"].ToString() : "NA";
                    lastCall[4] = Remark.ToString();
                    lastCall[5] = row["FlupNextDate"] != DBNull.Value && row["FlupNextDate"] != null && row["FlupNextDate"].ToString() != "" ? row["FlupNextDate"].ToString() : "NA";
                    lastCall[6] = row["FlupNextTime"] != DBNull.Value && row["FlupNextTime"] != null && row["FlupNextTime"].ToString() != "" ? row["FlupNextTime"].ToString() : "NA";
                }
                else
                {
                    lastCall[0]="NA";
                    lastCall[1]="NA";
                    lastCall[2]="NA";
                    lastCall[3]="NA";
                    lastCall[4]="NA";
                    lastCall[5]="NA";
                    lastCall[6]="NA";
                }
            }
        }
        catch(Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetlastCallDeatils", ex.Message.ToString());
            return;
        }
    }

    private void GetLastOrder()
    {
        try
        {
            int custIdX = Convert.ToInt32(Request.QueryString["custId"]);

            string query = "SELECT TOP 1 " +
                                "OrderID, " +
                                "Convert(Varchar(20), OrderDate,103) as OrderDate, " +
                                "OrderStatus, " +
                                "(SELECT COUNT(OrderID) FROM [OrdersData] WHERE FK_OrderCustomerID="+ custIdX + ") as NumOrders, " +
                                "(SELECT SUM(OrderAmount) FROM [OrdersData]  WHERE FK_OrderCustomerID=" + custIdX + ") as TotalOrderAmount, " +
                                "(SELECT CustomerFavShop FROM CustomersData  WHERE CustomrtID=" + custIdX + ") as CustomerFavShop " +
                            "FROM [OrdersData] " +
                            "WHERE FK_OrderCustomerID="+ custIdX + " Order By OrderDate Desc";

            using (DataTable dtFollowupOrd = c.GetDataTable(query))
            {
                if (dtFollowupOrd.Rows.Count > 0)
                {
                    DataRow row = dtFollowupOrd.Rows[0];

                    string ShopAssinged = "";
                    
                    if (row["CustomerFavShop"]!=DBNull.Value && row["CustomerFavShop"]!=null && row["CustomerFavShop"].ToString() != "")
                    {
                         ShopAssinged = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + row["CustomerFavShop"]).ToString();
                    }
                    else
                    {
                        ShopAssinged = "Not Found";
                    }

                    string OrderStatus = "";

                    if (row["OrderStatus"] != DBNull.Value && row["OrderStatus"] != null && row["OrderStatus"].ToString() != "")
                    {
                        OrderStatus = c.GetReqData("OrderStatusMeanings", "OrderStatusTitle", "OrderStatusFlag=" + row["OrderStatus"]).ToString();
                    }
                    else
                    {
                        OrderStatus = "Not Found";
                    }

                    OrdDeatils[0] = row["OrderID"] != DBNull.Value && row["OrderID"] != null && row["OrderID"].ToString() != "" ? row["OrderID"].ToString() : "0";
                    OrdDeatils[1] = row["OrderDate"] != DBNull.Value && row["OrderDate"] != null && row["OrderDate"].ToString() != "" ? row["OrderDate"].ToString() : "NA";
                    OrdDeatils[2] = row["TotalOrderAmount"] != DBNull.Value && row["TotalOrderAmount"] != null && row["TotalOrderAmount"].ToString() != "" ? row["TotalOrderAmount"].ToString() : "NA";
                    OrdDeatils[3] = row["NumOrders"] != DBNull.Value && row["TotalOrderAmount"] != null && row["NumOrders"].ToString() != "" ? row["NumOrders"].ToString() : "NA";
                    OrdDeatils[4] = ShopAssinged;
                    OrdDeatils[5] = OrderStatus;                    
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetLastOrder", ex.Message.ToString());
            return;
        }
    }

    private void FillGrid()
    {
        try
        {
            using (DataTable dtProduct = c.GetDataTable("SELECT DISTINCT b.ProductName, MAX(a.FK_DetailProductID) AS FK_DetailProductID, (SELECT TOP 1[OrdDetailQTY] FROM[dbo].[OrdersDetails] WHERE[FK_DetailOrderID] = MAX(c.[OrderID]) ORDER BY[FK_DetailOrderID] DESC) AS OrdDetailQTY,'Rs. ' + CONVERT(varchar(20), MAX(a.OrdDetailPrice)) AS OrigPrice, MAX(a.OrdDetailSKU) AS OrdDetailSKU, MAX(c.OrderDate) AS OrderDate FROM OrdersDetails a INNER JOIN ProductsData b ON a.FK_DetailProductID = b.ProductID INNER JOIN OrdersData c ON a.FK_DetailOrderID = c.OrderID WHERE c.FK_OrderCustomerID = " + Request.QueryString["custId"] +" Group BY b.ProductName ORDER BY OrderDate DESC"))

            {
                gvProducts.DataSource = dtProduct;
                gvProducts.DataBind();

                if (gvProducts.Rows.Count > 0)
                {
                    gvProducts.UseAccessibleHeader = true;
                    gvProducts.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;
        }
    }

    private void AddDefaultFirstRecord()
    {        
            //creating DataTable  
            DataTable dt = new DataTable();
            DataRow dr;
            dt.TableName = "OrdersDetails";
            //creating columns for DataTable  
            //dt.Columns.Add(new DataColumn("OrdDetailID", typeof(int)));
            dt.Columns.Add(new DataColumn("FK_DetailProductID", typeof(int)));
            dt.Columns.Add(new DataColumn("ProductName", typeof(string)));
            dt.Columns.Add(new DataColumn("OrdDetailQTY", typeof(int)));
            dt.Columns.Add(new DataColumn("OrdDetailPrice", typeof(int)));
            dt.Columns.Add(new DataColumn("OrdDetailSKU", typeof(string)));
            dt.Columns.Add(new DataColumn("OrdDetailAmount", typeof(int)));
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            ViewState["FollowupPo"] = dt;
            gvOrdDetails.DataSource = dt;
            gvOrdDetails.DataBind();
    }

    private void AddNewRecordRowToGrid(int OrdQty, int ProductIdx)
    {
        bool duplicateFound = false;
        if (ViewState["FollowupPo"] != null)
        {
            // Get ViewState Data to DataTable
            DataTable dtCurrentTable = (DataTable)ViewState["FollowupPo"];

            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                {
                    DataRow row = dtCurrentTable.Rows[i];

                    if (!row.IsNull("FK_DetailProductID") && row.Field<int>("FK_DetailProductID") == ProductIdx)
                    {
                        if (!row.IsNull("OrdDetailPrice"))
                        {
                            int OldOty = Convert.ToInt32(row["OrdDetailQTY"]);
                            int FinalQty = OldOty + OrdQty;

                            row["OrdDetailQTY"] = FinalQty;

                            row["OrdDetailAmount"] = FinalQty * Convert.ToInt32(row["OrdDetailPrice"]);

                            duplicateFound = true;
                        }
                    }
                }
            }
            
            if (duplicateFound == false)
            {

                DataTable dtOrderDetails = c.GetDataTable("SELECT ProductID, ProductName, ProductSKU, PriceSale FROM  ProductsData  WHERE ProductID=" + ProductIdx);

                if (dtOrderDetails.Rows.Count > 0)
                {
                    foreach (DataRow pRow in dtOrderDetails.Rows)
                    {
                        DataRow drCurrentRow = dtCurrentTable.NewRow();
                       // DataRow pRow = dtOrderDetails.Rows[i];
                        int Price = Convert.ToInt32(pRow["PriceSale"]);
                        int FinalAmount = OrdQty * Price;

                       // drCurrentRow["OrdDetailID"] = pRow["OrdDetailID"];
                        drCurrentRow["FK_DetailProductID"] = pRow["ProductID"];
                        drCurrentRow["ProductName"] = pRow["ProductName"];
                        drCurrentRow["OrdDetailSKU"] = pRow["ProductSKU"];
                        drCurrentRow["OrdDetailPrice"] = pRow["PriceSale"];
                        drCurrentRow["OrdDetailQTY"] = OrdQty;
                        drCurrentRow["OrdDetailAmount"] = FinalAmount;

                        // Added New Record to the DataTable  
                        dtCurrentTable.Rows.Add(drCurrentRow);
                    }
                }
            }

            // Removing initial blank row  
            if (dtCurrentTable.Rows[0][0].ToString() == "")
            {
                dtCurrentTable.Rows[0].Delete();
                dtCurrentTable.AcceptChanges();
            }

            // Storing DataTable to ViewState  
            ViewState["FollowupPo"] = dtCurrentTable;

            // Binding Gridview with updated or new row  
            gvOrdDetails.DataSource = dtCurrentTable;
            gvOrdDetails.DataBind();
        }
    }

    private void GetOrderTotal(int OrderIdX)
    {
        try
        {
            c.ExecuteQuery("Update OrdersData set OrderAmount=(Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID=" + OrderIdX + ") Where OrderId=" + OrderIdX);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FillDDR()
    {
        try
        {
            SqlConnection con = new SqlConnection(c.OpenConnection());

            SqlDataAdapter da = new SqlDataAdapter("SELECT a.[FranchShopCode]+' - '+b.[FranchName] as frName, a.[FK_FranchID] FROM [dbo].[CompanyOwnShops] a INNER JOIN [dbo].[FranchiseeData] b ON a.[FranchShopCode] = b.[FranchShopCode] WHERE a.[DelMark] = 0 ORDER BY [frName]", con);
            DataSet ds = new DataSet();
            da.Fill(ds, "myCombo");

            ddrShops.DataSource = ds.Tables[0];
            ddrShops.DataTextField = ds.Tables[0].Columns["frName"].ColumnName.ToString();
            ddrShops.DataValueField = ds.Tables[0].Columns["FK_FranchID"].ColumnName.ToString();
            ddrShops.DataBind();

            ddrShops.Items.Insert(0, "<-Select->");
            ddrShops.Items[0].Value = "0";

            con.Close();
            con = null;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error Occoured while processing', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "FillDDR", ex.Message.ToString());
            return;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        try
        {
            DataTable dt = (DataTable)ViewState["FollowupPo"];

            if (dt.Rows[0][0].ToString() == "") //gvOrdDetails.Rows.Count>0 Check Weater first row contains data or not
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Enter Item Details', type: 'warning'});", true);
                return;
            }

            int maxId = c.NextId("OrdersData", "OrderID");            

            object objGOBP = c.GetReqData("[dbo].[CustomersData]", "[FK_ObpID]", "[CustomrtID] = " + Request.QueryString["custId"]);
                        
            txtCountry.Text = txtCountry.Text.Trim().Replace("'", "");
            txtState.Text = txtState.Text.Trim().Replace("'", "");
            txtCity.Text = txtCity.Text.Trim().Replace("'", "");
            txtPinCode.Text = txtPinCode.Text.Trim().Replace("'", "");
            txtAddress1.Text = txtAddress1.Text.Trim().Replace("'", "");
            txtCalendar.Text = txtCalendar.Text.Trim().Replace("'", "");

            int shopToAssign = 0;
            if (chkConfirm.Checked == true)
            {
                //assign order to prev shop
                int prevShopId = Convert.ToInt32(c.GetReqData("[dbo].[OrdersAssign]", "TOP 1 [Fk_FranchID]", "[FK_OrderID] = " + maxId + " AND [OrdReAssign] = 0 ORDER BY [OrdAssignDate] DESC"));
                shopToAssign = prevShopId;
            }
            else
            {
                if (ddrShops.SelectedIndex == 0 && txtShopCode.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter shop code or select shop to assign order');", true);
                    return;
                }

                if (ddrShops.SelectedIndex > 0)
                {
                    shopToAssign = Convert.ToInt32(ddrShops.SelectedValue);
                }
                else
                {
                    if (txtShopCode.Text != "")
                    {
                        if (!c.IsRecordExist("SELECT [FranchID] FROM [dbo].[FranchiseeData] WHERE [FranchShopCode] = '" + txtShopCode.Text + "' AND [FranchActive] = 1"))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Shop with above shop code is not exists');", true);
                            return;
                        }
                        else
                        {
                            shopToAssign = Convert.ToInt32(c.GetReqData("[dbo].[FranchiseeData]", "[FranchID]", "[FranchShopCode] = '" + txtShopCode.Text + "' AND [FranchActive] = 1"));
                        }
                    }
                }
            }

            int addressId = 0;
            if (ddrAddress.SelectedIndex > 0)
            {
                addressId = Convert.ToInt32(ddrAddress.SelectedValue);
            }
            else
            {
                if (txtAddress1.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select or add address to continue');", true);
                    return;
                }
                if (txtCountry.Text == "" ||
                    txtState.Text == "" || txtCity.Text == "" || txtPinCode.Text == "" || txtAddress1.Text == "" || ddrAddrName.SelectedIndex == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * Marked fields are Mandatory');", true);
                    return;
                }
                if (!c.IsNumeric(txtPinCode.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Pincode must be numeric value');", true);
                    return;
                }

                int maxAddrId = c.NextId("[dbo].[CustomersAddress]", "[AddressID]");
                c.ExecuteQuery("INSERT INTO [dbo].[CustomersAddress] ([AddressID], [AddressFKCustomerID], [AddressFull], [AddressCity], [AddressState], " +
                    " [AddressPincode], [AddressCountry], [AddressStatus], [AddressName]) VALUES (" + maxAddrId + ", " + Request.QueryString["custId"] + ", '" + txtAddress1.Text +
                    "', '" + txtCity.Text + "', '" + txtState.Text + "', '" + txtPinCode.Text + "', '" + txtCountry.Text + "', 1, '" + ddrAddrName.SelectedItem.Text + "')");
                addressId = maxAddrId;
            }

            int mreq = chkMonthly.Checked == true ? 1 : 0;
            string deviceType = "CS-Order";

            DateTime nextFlDate;
            if (txtCalendar.Text != null && !string.IsNullOrEmpty(txtCalendar.Text))
            {
                string[] arrTDate = txtCalendar.Text.Split('/');
                if (!DateTime.TryParse(arrTDate[1] + "/" + arrTDate[0] + "/" + arrTDate[2], out nextFlDate))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Date');", true);
                    return;
                }
            }
            else
            {
                nextFlDate = DateTime.Now.AddDays(26);
            }

            if (objGOBP != null && objGOBP != DBNull.Value && objGOBP.ToString() != "")
            {
                c.ExecuteQuery("Insert Into OrdersData (OrderID, FK_OrderCustomerID, OrderDate, OrderStatus, OrderType, DeviceType, OrderPayStatus, GOBPId, FollowupLastDate, FollowupNextDate, FollowupNextTime, FollowupStatus) Values (" + maxId +
               ", " + Request.QueryString["custId"] + ", '" + DateTime.Now + "', 0, 1, 'CS-Order', 0, " + Convert.ToInt32(objGOBP) + ", '" + DateTime.Now + "', '" + nextFlDate + "', '11:00 AM', 'Active')");
            }
            else
            {
                c.ExecuteQuery("Insert Into OrdersData (OrderID, FK_OrderCustomerID, OrderDate, OrderStatus, OrderType, DeviceType, OrderPayStatus, FollowupLastDate, FollowupNextDate, FollowupNextTime, FollowupStatus) Values (" + maxId +
               ", " + Request.QueryString["custId"] + ", '" + DateTime.Now + "', 0, 1, 'CS-Order', 0, '" + DateTime.Now + "', '" + nextFlDate + "', '11:00 AM', 'Active')");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];

                int OrdDetailMaxId = c.NextId("OrdersDetails", "OrdDetailID");
                int ProductPrice = Convert.ToInt32(row["OrdDetailPrice"]);
                int ProductAmmount = Convert.ToInt32(row["OrdDetailAmount"]);
                int ProductQty = Convert.ToInt32(row["OrdDetailQTY"]);
                string ProductCode = row["OrdDetailSKU"].ToString();
                int FkDetailProductId = Convert.ToInt32(row["FK_DetailProductID"]);
                int FKDetailOrdId = maxId;


                c.ExecuteQuery("Insert Into OrdersDetails (OrdDetailID, FK_DetailOrderID, FK_DetailProductID, OrdDetailQTY, OrdDetailPrice, OrdDetailAmount, OrdDetailSKU) Values (" +
                OrdDetailMaxId + ", " + FKDetailOrdId + ", " + FkDetailProductId + ", " + ProductQty + ", " + ProductPrice + ",  " + ProductAmmount + ", '" + ProductCode + "')");

                if (c.IsRecordExist("SELECT [OrderID] FROM [dbo].[OrdersData] WHERE [GOBPId] IS NOT NULL OR [GOBPId] <> '' OR [GOBPId] <> 0 AND [OrderID] = " + maxId))
                {
                    c.GOBP_ProductWise_CommissionCalc(OrdDetailMaxId, maxId, FkDetailProductId, ProductQty);

                    //Update Total Order Amount to main OrdersData table 
                    GetOrderTotal(maxId);

                    // Update GOBO Commission total to main OrdersData table also.
                    c.GOBP_CommissionTotal(maxId);
                }
            }

            int maxFlId = c.NextId("FollowupOrders", "FlupID");

            c.ExecuteQuery("INSERT INTO [dbo].[FollowupOrders] ([FlupID], [FlupDate], [FK_CustomerId], [FK_OrderId], [FK_TeamMemberId], " +
                        " [FlupRemarkStatusID], [FlupRemarkStatus], [FlupRemark], [FlupNextDate], [FlupNextTime], [FlupRptOrderId], " +
                        " [FlupStatus]) Values (" + maxFlId + ", '" + DateTime.Now + "', " + Request.QueryString["custId"] +
                        ", " + maxId + ", " + Session["adminSupport"] + ", 7, 'Connected: New Order', " +
                        " 'New Order Placed', '" + nextFlDate + "', '11:00 AM', 0, 'Open')");

            c.ExecuteQuery("UPDATE [dbo].[CustomersData] SET [CallBusyFlag] = 0, [CallBusyBy] = NULL WHERE [CustomrtID] = " + Request.QueryString["custId"]);

            // insert into OrdersAssign table
            int maxAssignId = c.NextId("[dbo].[OrdersAssign]", "[OrdAssignID]");
            c.ExecuteQuery("INSERT INTO [dbo].[OrdersAssign] ([OrdAssignID], [OrdAssignDate], [FK_OrderID], [Fk_FranchID], [OrdAssignStatus], " +
                " [OrdReAssign], [AssignedFrom]) VALUES (" + maxAssignId + ", '" + DateTime.Now + "', " + maxId + ", " + shopToAssign +
                ", 0, 0, 'New Order')");

            if (dt.Rows[0][0].ToString() == "") //gvOrdDetails.Rows.Count>0 Check Weater first row contains data or not
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Enter Item Details', type: 'warning'});", true);
                return;
            }
            else
            {
                // BulkInsertToDataBase();
            }
            // txtTitle.Text = txtInfo.Text = txtName.Text = txtInward.Text = "";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'New Order Added', type: 'success'});", true);
        }
        catch(Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }
    
    protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "add")
            {

                GridViewRow gRow = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                TextBox txtQty = (TextBox)gRow.FindControl("txtQty");

                if (txtQty.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Enter Item Quantity', type: 'warning'});", true);
                    return;

                }

                if (Convert.ToInt32(txtQty.Text) <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Quantity Must be Greater than 0', type: 'warning'});", true);
                    return;
                }

                OrderItem.Visible = true;
                //int OrdId =Convert.ToInt32(gRow.Cells[0].Text);
                int textVlaue = Convert.ToInt32(txtQty.Text);
                
                int ProductId = Convert.ToInt32(gRow.Cells[0].Text);

                AddNewRecordRowToGrid(textVlaue, ProductId);

                txtQty.Text = "";                
            }
        }
        catch(Exception ex)
        {
            err = ex.Message.ToString();
            
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: '"+ err + "', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "gvProducts_RowCommand", ex.Message.ToString());
            return;
        }
    }


    protected void gvOrdDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "del")
            {
                GridViewRow gRow = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                
                int rowIndex = gRow.RowIndex;
                DataTable dt = ViewState["FollowupPo"] as DataTable;
                dt.Rows[rowIndex].Delete();
                dt.AcceptChanges();

                if (dt.Rows.Count > 0)
                {
                    ViewState["FollowupPo"] = dt;
                    gvOrdDetails.DataSource = dt;
                    gvOrdDetails.DataBind();
                }
                else
                {
                    AddDefaultFirstRecord();
                    OrderItem.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: '" + err + "', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "gvOrdDetails_RowCommand", ex.Message.ToString());
            return;
        }
    }

    private void FillQuantity()
    {
        for (int i = 1; i < 31; i++)
        {
            ddrQty.Items.Add(i.ToString());
            ddrQty.SelectedValue = "1";
        }
    }

    private void AddOtherMedToGrid(int ProductIdx)
    {
        try
        {
            bool duplicateFound = false;
            if (ViewState["FollowupPo"] != null)
            {
                // Get ViewState Data to DataTable
                DataTable dtCurrentTable = (DataTable)ViewState["FollowupPo"];

                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {
                        DataRow row = dtCurrentTable.Rows[i];

                        if (!row.IsNull("FK_DetailProductID") && row.Field<int>("FK_DetailProductID") == ProductIdx)
                        {
                            if (!row.IsNull("OrdDetailPrice"))
                            {

                                int OldOty = Convert.ToInt32(row["OrdDetailQTY"]);
                                int FinalQty = OldOty + Convert.ToInt32(ddrQty.SelectedValue);

                                row["OrdDetailQTY"] = FinalQty;

                                row["OrdDetailAmount"] = FinalQty * Convert.ToInt32(row["OrdDetailPrice"]);

                                duplicateFound = true;
                            }
                        }
                    }
                }

                if (duplicateFound == false)
                {
                    DataTable dtOrderDetails = c.GetDataTable("SELECT ProductID, ProductName, ProductSKU, PriceMRP FROM  ProductsData  WHERE ProductID=" + ProductIdx);

                    if (dtOrderDetails.Rows.Count > 0)
                    {
                        foreach (DataRow pRow in dtOrderDetails.Rows)
                        {

                            DataRow drCurrentRow = dtCurrentTable.NewRow();
                            // DataRow pRow = dtOrderDetails.Rows[i];
                            int Price = Convert.ToInt32(pRow["PriceMRP"]);
                            int FinalAmount = Convert.ToInt32(ddrQty.SelectedValue) * Price;

                            //drCurrentRow["OrdDetailID"] = pRow["OrdDetailID"];
                            drCurrentRow["FK_DetailProductID"] = pRow["ProductID"];
                            drCurrentRow["ProductName"] = txtMedName.Text;
                            drCurrentRow["OrdDetailSKU"] = pRow["ProductSKU"];
                            drCurrentRow["OrdDetailPrice"] = pRow["PriceMRP"];
                            drCurrentRow["OrdDetailQTY"] = ddrQty.SelectedValue;
                            drCurrentRow["OrdDetailAmount"] = FinalAmount;

                            // Added New Record to the DataTable  
                            dtCurrentTable.Rows.Add(drCurrentRow);
                        }
                    }
                }

                // Removing initial blank row  
                if (dtCurrentTable.Rows[0][0].ToString() == "")
                {
                    dtCurrentTable.Rows[0].Delete();
                    dtCurrentTable.AcceptChanges();
                }

                // Storing DataTable to ViewState  
                ViewState["FollowupPo"] = dtCurrentTable;

                // Binding Gridview with updated or new row  
                gvOrdDetails.DataSource = dtCurrentTable;
                gvOrdDetails.DataBind();
            }
        }
        catch(Exception ex)
        {
            err = ex.Message.ToString();
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: '" + err + "', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "AddOtherMedToGrid", ex.Message.ToString());
            return;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {           
            int ProductId =Convert.ToInt32(c.GetReqData("ProductsData", "ProductID", "ProductName='"+txtMedName.Text+"'"));
          
            if (txtMedName.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Enter Item Name', type: 'warning'});", true);
                return;
            }
            if (Convert.ToInt32(ddrQty.SelectedValue) <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Quantity Must be Greater than 0', type: 'warning'});", true);
                return;
            }

            OrderItem.Visible = true;

            AddOtherMedToGrid(ProductId);

            txtMedName.Text = "";
            ddrQty.SelectedValue = "1";
        }
        catch(Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: '" + err + "', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "AddOtherMedToGrid", ex.Message.ToString());
            return;
        }
    }

    protected void gvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Assuming the product ID is in the first cell (index 0) of the row.
                string productId = e.Row.Cells[0].Text;

                // Check if the product ID exists in the OrdProduct array.
                if (OrdProduct.Contains(productId))
                {
                    // Apply the desired formatting or action when there is a match.
                    e.Row.Style["background-color"] = "LightBlue"; 
                }
            }

        }
        catch (Exception ex)
        {

        }
    }
}