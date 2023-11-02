using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Razorpay.Api;
using System.IO;
using System.Text;

public partial class supportteam_edit_new_order : System.Web.UI.Page
{
    iClass c = new iClass();
    public string pgTitle, err, followuplink, ordAmmount, paymentslip;
    public string[] arrCust = new string[30];
    public string[] lastCall = new string[30];
    public string[] OrdDeatils = new string[30];
    public string[] OrdProduct = new string[30];

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            object OrderId = c.GetReqData("OrdersData", "TOP 1 OrderID", "FK_OrderCustomerID=" + Request.QueryString["custid"] + " Order By OrderDate Desc");

            using (DataTable dtOrdDetils = c.GetDataTable("Select FK_DetailProductID From [OrdersDetails] where FK_DetailOrderID=" + OrderId))
            {                
                if (dtOrdDetils.Rows.Count > 0)
                {

                    List<string> ordProductList = new List<string>(OrdProduct);

                    // Loop through the DataTable and add the FK_DetailProductID to the list
                    foreach (DataRow row in dtOrdDetils.Rows)
                    {
                        string productId = row["FK_DetailProductID"].ToString();
                        ordProductList.Add(productId);
                    }
                    
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
    }

    private void ShowCustomersData()
    {
        CustomersData custinfo = new CustomersData();

        int CustIdx = Convert.ToInt32(Request.QueryString["custId"]);

        custinfo.CustomresInfo(CustIdx);

        arrCust[0] = custinfo.CustomerName;
        arrCust[1] = custinfo.CustomerMobile;

        custinfo.FranchaiseeData(custinfo.CustomerFavShop);        

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
                    lastCall[0] = "NA";
                    lastCall[1] = "NA";
                    lastCall[2] = "NA";
                    lastCall[3] = "NA";
                    lastCall[4] = "NA";
                    lastCall[5] = "NA";
                    lastCall[6] = "NA";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetlastCallDeatils", ex.Message.ToString());
            return;
        }
    }

    private void FillGrid()
    {
        try
        {
            using (DataTable dtProduct = c.GetDataTable("Select a.OrdDetailID, a.FK_DetailProductID, a.OrdDetailQTY, 'Rs. ' + Convert(varchar(20), a.OrdDetailPrice)  as OrigPrice, 'Rs. ' + Convert(varchar(20), a.OrdDetailAmount) as OrdAmount , a.OrdDetailSKU, b.ProductName from OrdersDetails a Inner Join ProductsData b on a.FK_DetailProductID = b.ProductID where a.FK_DetailOrderID =" + Request.QueryString["ordId"].ToString()))
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
        dt.Columns.Add(new DataColumn("FK_DetailProductID", typeof(int)));
        dt.Columns.Add(new DataColumn("ProductName", typeof(string)));
        dt.Columns.Add(new DataColumn("OrdDetailQTY", typeof(int)));
        dt.Columns.Add(new DataColumn("OrdDetailPrice", typeof(int)));
        dt.Columns.Add(new DataColumn("OrdDetailSKU", typeof(string)));
        dt.Columns.Add(new DataColumn("OrdDetailAmount", typeof(int)));
        dt.Columns.Add(new DataColumn("FK_DetailOrderID", typeof(int)));
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
                        int Price = Convert.ToInt32(pRow["PriceSale"]);
                        int FinalAmount = OrdQty * Price;
                        
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
            string orderAmount = c.returnAggregate("Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID=" + OrderIdX).ToString("0.00");
            string finalOrderAmount = "";
            int shipCharges = 0;

            if (Convert.ToDouble(orderAmount) >= 500)
            {
                shipCharges = 0;
                finalOrderAmount = (Convert.ToDouble(orderAmount.ToString()) + shipCharges).ToString("0.00");
            }
            else
            {
                shipCharges = 30;
                finalOrderAmount = (Convert.ToDouble(orderAmount.ToString()) + shipCharges).ToString("0.00");
            }

            c.ExecuteQuery("Update OrdersData Set OrderAmount=" + Convert.ToDouble(finalOrderAmount) + ", OrderShippingAmount = " + shipCharges + " Where OrderID=" + OrderIdX);

            ordAmmount = "Total Amount : " + finalOrderAmount;
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
            string deviceType = "FL-Edit Order";

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

            string imgName = "";
            int payslipId = c.NextId("OrdersPaymentSlip", "OrdPayId");

            if (fuPaymentSlip.HasFile)
            {
                string extension = System.IO.Path.GetExtension(fuPaymentSlip.FileName);
                string fExt = Path.GetExtension(fuPaymentSlip.FileName).ToLower();

                if (fExt == ".jpg" || fExt == ".jpeg" || fExt == ".png")
                {
                    imgName = "payment-slip-" + maxId + fExt;
                    // Save the uploaded file to a directory on the server
                    string uploadFolderPath = Server.MapPath("~/upload/paymentslip/"); // Specify your desired folder
                    string filePath = Path.Combine(uploadFolderPath, imgName);
                    fuPaymentSlip.SaveAs(filePath);
                    ImageUploadProcess(imgName);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg or .png files are allowed');", true);
                }

                c.ExecuteQuery("Insert Into OrdersPaymentSlip (OrdPayId, OrdPayDate, OrdPaySlip, FK_OrderId) " +
                    " Values (" + payslipId + ", '" + DateTime.Now + "', '" + imgName + "', " + maxId + ")");

                GetUploadedPrescription(maxId);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select image to upload prescription');", true);
            }

            //string imgName = "";
            //int payslipId = c.NextId("OrdersPaymentSlip", "OrdPayId");
            //if (fuPaymentSlip.HasFile)
            //{
            //    string fExt = Path.GetExtension(fuPaymentSlip.FileName).ToString().ToLower();
            //    if (fExt == ".jpg" || fExt == ".jpeg" || fExt == ".png")
            //    {
            //        imgName = "payment-slip-" + maxId + fExt;
            //        ImageUploadProcess(imgName);
            //    }
            //    else
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg or .png files are allowed');", true);
            //        //return;
            //    }

            //    c.ExecuteQuery("Insert Into OrdersPaymentSlip (OrdPayId, OrdPayDate, OrdPaySlip, FK_OrderId) " +
            //        " Values (" + payslipId + ", '" + DateTime.Now + "', '" + imgName + "', " + maxId + ")");

            //    GetUploadedPrescription(maxId);
            //}
            //else
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select image to upload prescription');", true);
            //    //return;
            //}
           
            deviceType = "FL-Edit Order";

            int maxFlId = c.NextId("FollowupOrders", "FlupID");
            if (Request.QueryString["ordId"] != null)
            {
                c.ExecuteQuery("Update FollowupOrders Set FlupStatus='Closed' Where FK_OrderId=" + Request.QueryString["ordId"] +
                    " AND FK_CustomerId=" + Request.QueryString["custId"]);
            }

            c.ExecuteQuery("Insert Into FollowupOrders (FlupID, FlupDate, FK_CustomerId, FK_OrderId, FK_TeamMemberId, " +
                " FlupRemarkStatusID, FlupRemarkStatus, FlupRemark, FlupNextDate, FlupNextTime, FlupRptOrderId, " +
                " FlupStatus) Values (" + maxFlId + ", '" + DateTime.Now + "', " + Request.QueryString["custId"] +
                ", " + maxId + ", " + Session["adminSupport"] + ", 8, 'Connected: Edited New Order', " +
                " 'Edited New Order Placed', '" + nextFlDate + "', '11:00 AM', 0, 'Open')");

            c.ExecuteQuery("Update CustomersData Set CallBusyFlag=0, CallBusyBy=NULL, RBNO_NextFollowup='" + nextFlDate + "' Where CustomrtID=" + Request.QueryString["custId"]);

            if (Request.QueryString["ordId"] != null)
            {
                //update prev order followup status as inactive
                c.ExecuteQuery("Update OrdersData Set FollowupStatus='Inactive' Where OrderID=" + Request.QueryString["ordId"]);
            }

            //Check If this Order referance is from Enquiry i.e. SavingCalc then update SavingCalc => FollowupStatus ='Close' (14-Apr-23 by Vinayak)
            if (Request.QueryString["enqref"] != null)
            {
                c.ExecuteQuery("Update SavingCalc Set FollowupStatus='Inactive' Where CalcID=" + Request.QueryString["enqref"]);
            }
                          
            int codFlag = chkCODType.Checked == true ? 1 : 2;

            if (chkSelfPickup.Checked == true)
            {
                codFlag = 3;
            }

            if (codFlag == 3)
            {
                int custId = Convert.ToInt32(Request.QueryString["custId"]);
                int shipCharges = 0;
                int ordId = Convert.ToInt32(c.GetReqData("[dbo].[OrdersData]", "TOP 1 [OrderID]", "[FK_OrderCustomerID]=" + custId + "ORDER BY [OrderDate] DESC").ToString());
                string orderAmount = c.returnAggregate("Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID=" + ordId).ToString("0.00");
                string finalOrderAmount = "";

                shipCharges = 0;
                finalOrderAmount = (Convert.ToDouble(orderAmount.ToString()) + shipCharges).ToString("0.00");

                c.ExecuteQuery("Update OrdersData Set OrderAmount=" + Convert.ToDouble(finalOrderAmount) + ", [OrderShippingAmount] = 0 Where OrderID=" + ordId);
            }

            if (ddrCourier.SelectedIndex == 1 || ddrCourier.SelectedIndex == 2 || ddrCourier.SelectedIndex == 3 || ddrCourier.SelectedIndex == 4)
            {
                c.ExecuteQuery("Update OrdersData Set OrderStatus=3, FK_AddressId=" + addressId + ", MreqFlag=" + mreq +
                                       ", DeviceType='" + deviceType + "', FollowupLastDate='" + DateTime.Now + "', FollowupNextDate='" + nextFlDate +
                                       "', FollowupNextTime='11:00 AM', FollowupStatus='Active', OrderPayMode = " + codFlag + ", UPIID = '" + txtUPIID.Text + "', CourierOptions = '" + ddrCourier.SelectedItem.Text + "' Where OrderID=" + maxId);
            }
            else if (ddrCourier.SelectedIndex == 5)
            {
                c.ExecuteQuery("Update OrdersData Set OrderStatus=3, FK_AddressId=" + addressId + ", MreqFlag=" + mreq +
                                       ", DeviceType='" + deviceType + "', FollowupLastDate='" + DateTime.Now + "', FollowupNextDate='" + nextFlDate +
                                       "', FollowupNextTime='11:00 AM', FollowupStatus='Active', OrderPayMode = " + codFlag + ", UPIID = '" + txtUPIID.Text + "', CourierOptions = '" + txtCourier.Text + "' Where OrderID=" + maxId);
            }
            else if (ddrCourier.SelectedIndex == 0)
            {
                c.ExecuteQuery("Update OrdersData Set OrderStatus=3, FK_AddressId=" + addressId + ", MreqFlag=" + mreq +
                                       ", DeviceType='" + deviceType + "', FollowupLastDate='" + DateTime.Now + "', FollowupNextDate='" + nextFlDate +
                                       "', FollowupNextTime='11:00 AM', FollowupStatus='Active', OrderPayMode = " + codFlag + ", UPIID = '" + txtUPIID.Text + "', CourierOptions = '-' Where OrderID=" + maxId);
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

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Order placed successfully');", true);

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('dashboard.aspx', 2000);", true);

            //
            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Edited Order is Placed', type: 'success'});", true);       

            //Response.Redirect("dashboard.aspx", false);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }

    private void ImageUploadProcess(string imgName)
    {
        try
        {
            string origImgPath = "~/upload/paymentslip/original/";
            string normalImgPath = "~/upload/paymentslip/";

            fuPaymentSlip.SaveAs(Server.MapPath(origImgPath) + imgName);

            c.ImageOptimizer(imgName, origImgPath, normalImgPath, 800, true);

            //Delete rew image from server
            File.Delete(Server.MapPath(origImgPath) + imgName);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ImageUploadProcess", ex.Message.ToString());
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
                int textVlaue = Convert.ToInt32(txtQty.Text);

                int ProductId = Convert.ToInt32(gRow.Cells[0].Text);

                AddNewRecordRowToGrid(textVlaue, ProductId);

                txtQty.Text = "";
            }
        }
        catch (Exception ex)
        {
            err = ex.Message.ToString();

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: '" + err + "', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "gvProducts_RowCommand", ex.Message.ToString());
            return;
        }
    }

    protected void gvOrdDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            ordAmmount = c.GetReqData("[dbo].[OrdersData]", "[OrderAmount]", "[OrderID]=" + Request.QueryString["ordId"].ToString()).ToString();

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
                            int Price = Convert.ToInt32(pRow["PriceMRP"]);
                            int FinalAmount = Convert.ToInt32(ddrQty.SelectedValue) * Price;
                            
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
        catch (Exception ex)
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
            int ProductId = Convert.ToInt32(c.GetReqData("ProductsData", "ProductID", "ProductName='" + txtMedName.Text + "'"));

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
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: '" + err + "', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "AddOtherMedToGrid", ex.Message.ToString());
            return;
        }
    }

    private void GetUploadedPrescription(int ordIdX)
    {
        try
        {
            using (DataTable dtRx = c.GetDataTable("Select OrdPayId, OrdPaySlip From OrdersPaymentSlip Where FK_OrderId = " + ordIdX))
            {
                if (dtRx.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();
                    int bCount = 0;
                    foreach (DataRow row in dtRx.Rows)
                    {
                        strMarkup.Append("<div class=\"imgBox\">");
                        strMarkup.Append("<div class=\"pad-5\">");
                        strMarkup.Append("<div class=\"border1\">");
                        strMarkup.Append("<div class=\"pad-5\">");
                        strMarkup.Append("<div class=\"imgContainer\">");
                        strMarkup.Append("<a href=\"" + Master.rootPath + "upload/paymentslip/" + row["OrdPaySlip"] + "\" data-fancybox=\"rxGroup\"><img src=\"" + Master.rootPath + "upload/paymentslip/" + row["OrdPaySlip"] + "\" class=\"width100\" /></a>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");                        
                        strMarkup.Append("</div>");

                        bCount++;

                        if ((bCount % 4) == 0)
                        {
                            strMarkup.Append("<div class=\"float_clear\"></div>");
                        }
                    }
                    strMarkup.Append("<div class=\"float_clear\"></div>");
                    paymentslip = strMarkup.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetUploadedPrescription", ex.Message.ToString());
            return;
        }
    }
}