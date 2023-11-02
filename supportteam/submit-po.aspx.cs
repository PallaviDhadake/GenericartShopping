using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.ComponentModel;
using Razorpay.Api;

public partial class supportteam_submit_po : System.Web.UI.Page
{
    iClass c = new iClass();
    public string pageTitle, ordAmmount, paymentslip;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["enqref"] != null)
                {
                    EnqMedicines.Visible = true;
                    Enquiry_Medicines(Convert.ToInt32(Request.QueryString["enqref"].ToString()));
                }
                else
                {
                    EnqMedicines.Visible = false;
                }

                if (Request.QueryString["custId"] != null)
                {
                    FillDDR();
                    FillQuantity();
                    pageTitle = c.GetReqData("CustomersData", "CustomerName", "CustomrtID=" + Request.QueryString["custId"]).ToString() + " - PO";
                    btnSubmitOrder.Visible = false;

                    if (c.IsRecordExist("Select OrderID From OrdersData Where FK_OrderCustomerID=" + Request.QueryString["custId"] +
                        " AND (Convert(varchar(20), OrderDate, 112) = Convert(varchar(20), CAST('" + DateTime.Now + "' as datetime), 112)) " +
                        " AND OrderStatus=0"))
                    {
                        int orderId = Convert.ToInt32(c.GetReqData("OrdersData", "OrderID", "FK_OrderCustomerID=" + Request.QueryString["custId"] +
                            " AND (Convert(varchar(20), OrderDate, 112) = Convert(varchar(20), CAST('" + DateTime.Now + "' as datetime), 112)) AND OrderStatus=0"));
                        GetOrdersData(orderId);
                    }


                    if (c.IsRecordExist("Select AddressID From CustomersAddress Where AddressFKCustomerID=" + Request.QueryString["custId"]))
                    {
                        c.FillComboBox("AddressFull", "AddressID", "CustomersAddress", "AddressFKCustomerID=" + Request.QueryString["custId"] + "", "AddressFull", 0, ddrAddress);
                    }
                    else
                    {
                        existingAddr.Visible = false;
                    }
                }
                else
                {
                    Response.Redirect("dashboard.aspx", false);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "Page_Load", ex.Message.ToString());
            return;
        }
    }

    private void Enquiry_Medicines(int enqid)
    {
        try
        {
            using (DataTable dtMedicine = c.GetDataTable(@"Select 
                                            	a.BrandMedicine, 
                                            	'Rs. ' + Convert(varchar(20), a.BrandPrice)  as BrandPrice, 
                                            	'Rs. ' + Convert(varchar(20), a.GenericPrice) as GenericPrice , 
                                            	a.GenericMedicine, 
                                            	a.GenericCode, 
                                            	a.SavingAmount, 
                                            	Convert(varchar(20), a.SavingPercent) + '%' as SavingPercent 
                                            from SavingCalcItems a 
                                            where a.FK_CalcID = " + Request.QueryString["enqref"].ToString()))
            {
                gvEnqMedicine.DataSource = dtMedicine;
                gvEnqMedicine.DataBind();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error Occoured while processing', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "Enquiry_Medicines", ex.Message.ToString());
            return;
        }
    }

    private void FillDDR()
    {
        try
        {
            SqlConnection con = new SqlConnection(c.OpenConnection());

            SqlDataAdapter da = new SqlDataAdapter("Select a.FranchShopCode+' - '+b.FranchName as frName, a.FK_FranchID From CompanyOwnShops a Inner Join FranchiseeData b On a.FranchShopCode=b.FranchShopCode Where a.DelMark=0 Order By frName", con);
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

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            txtMedName.Text = txtMedName.Text.Trim().Replace("'", "");
            int customerId = Convert.ToInt32(Request.QueryString["custId"]);

            if (txtMedName.Text == "" || ddrQty.SelectedIndex == 0)
            {
                pageTitle = c.GetReqData("CustomersData", "CustomerName", "CustomrtID=" + Request.QueryString["custId"]).ToString() + " - PO";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * fields are compulsory');", true);
                return;
            }
            pageTitle = c.GetReqData("CustomersData", "CustomerName", "CustomrtID=" + Request.QueryString["custId"]).ToString() + " - PO";
            int prodIdX = Convert.ToInt32(c.GetReqData("ProductsData", "ProductID", "ProductName = '" + txtMedName.Text + "' and delMark=0 AND isnull(ProductActive, 0) = 1"));
            double prodAmount = Convert.ToDouble(c.GetReqData("ProductsData", "PriceSale", "ProductID=" + prodIdX));
            double ordAmount = 0, finalOrdAmount = 0, incrementPrice = 0;
            int prodOptionId = 0;
            int optionId = 0;
            int qty = Convert.ToInt32(ddrQty.SelectedValue);

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

            string prodCode = c.GetReqData("ProductsData", "ProductSKU", "ProductID=" + prodIdX).ToString();
            
            int orderId = 0;
            if (c.IsRecordExist("Select OrderID From OrdersData Where FK_OrderCustomerID=" + customerId +
                " AND (Convert(varchar(20), OrderDate, 112) = Convert(varchar(20), CAST('" + DateTime.Now + "' as datetime), 112)) " +
                " AND OrderStatus=0"))
            {
                orderId = Convert.ToInt32(c.GetReqData("OrdersData", "OrderID", "FK_OrderCustomerID=" + customerId + 
                    " AND (Convert(varchar(20), OrderDate, 112) = Convert(varchar(20), CAST('" + DateTime.Now + "' as datetime), 112)) AND OrderStatus=0"));
            }
            else
            {
                orderId = c.NextId("OrdersData", "OrderID");
                                
                object objGOBP = c.GetReqData("[dbo].[CustomersData]", "[FK_ObpID]", "[CustomrtID] = " + customerId);

                if (objGOBP != null && objGOBP != DBNull.Value && objGOBP != "")
                {
                    c.ExecuteQuery("Insert Into OrdersData (OrderID, FK_OrderCustomerID, OrderDate, OrderStatus, OrderType, DeviceType, OrderPayStatus, GOBPId) Values (" + orderId +
                   ", " + customerId + ", '" + DateTime.Now + "', 0, 1, 'CS-Order', 0, " + Convert.ToInt32(objGOBP) + ")");
                }
                else
                {
                    c.ExecuteQuery("Insert Into OrdersData (OrderID, FK_OrderCustomerID, OrderDate, OrderStatus, OrderType, DeviceType, OrderPayStatus) Values (" + orderId +
                   ", " + customerId + ", '" + DateTime.Now + "', 0, 1, 'CS-Order', 0)");
                }
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

            object deliveryType = c.GetReqData("OrdersData", "DeliveryType", "OrderID=" + orderId);
            int shipCharges = 0;
            if (deliveryType != DBNull.Value && deliveryType != null && deliveryType.ToString() != "")
            {
                if (deliveryType.ToString() == "2")
                {
                    shipCharges = 0;
                }
                else if (deliveryType.ToString() == "1")
                {
                    shipCharges = 30;
                }
                else
                {
                    shipCharges = 0;
                }
            }
            else
            {
                shipCharges = 0;
            }

            string orderAmount = c.returnAggregate("Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID=" + orderId).ToString("0.00");
            string finalOrderAmount = "";

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

            c.ExecuteQuery("Update OrdersData Set OrderAmount=" + Convert.ToDouble(finalOrderAmount) + ", OrderShippingAmount = " + shipCharges + " Where OrderID=" + orderId);

            GetOrdersData(orderId);
            ordAmmount = "Total Amount : " + finalOrderAmount;
            txtMedName.Text = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnAdd_Click", ex.Message.ToString());
            return;
        }
    }

    private void calcOrderTotalAmount()
    {
        try
        {
            int orderId = 0;
            int customerId = Convert.ToInt32(Request.QueryString["custId"]);
            if (c.IsRecordExist("Select OrderID From OrdersData Where FK_OrderCustomerID=" + customerId +
                " AND (Convert(varchar(20), OrderDate, 112) = Convert(varchar(20), CAST('" + DateTime.Now + "' as datetime), 112)) " +
                " AND OrderStatus=0"))
            {
                orderId = Convert.ToInt32(c.GetReqData("OrdersData", "OrderID", "FK_OrderCustomerID=" + customerId +
                    " AND (Convert(varchar(20), OrderDate, 112) = Convert(varchar(20), CAST('" + DateTime.Now + "' as datetime), 112)) AND OrderStatus=0"));
            }
            else
            {
                orderId = c.NextId("OrdersData", "OrderID");
            }

            GetOrdersData(orderId);
        }

        catch(Exception ex)
        {
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
                        //strMarkup.Append("<a href=\"#\" title=\"Delete Payment Slip\"  class=\"closeAnch\"></a>");
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

    private void GetOrdersData(int ordIdX)
    {
        try
        {
            using (DataTable dtProduct = c.GetDataTable("Select a.OrdDetailID, a.FK_DetailOrderID, a.FK_DetailProductID, a.OrdDetailQTY, 'Rs. ' + Convert(varchar(20), a.OrdDetailPrice)  as OrigPrice, 'Rs. ' + Convert(varchar(20), a.OrdDetailAmount) as OrdAmount , a.OrdDetailSKU, b.ProductName from OrdersDetails a Inner Join ProductsData b on a.FK_DetailProductID = b.ProductID where a.FK_DetailOrderID =" + ordIdX))
            {
                gvOrderDetails.DataSource = dtProduct;
                gvOrderDetails.DataBind();

                if (gvOrderDetails.Rows.Count > 0)
                {
                    gvOrderDetails.UseAccessibleHeader = true;
                    gvOrderDetails.HeaderRow.TableSection = TableRowSection.TableHeader;

                    btnSubmitOrder.Visible = true;
                    AddressBox.Visible = true;                    
                }
                else
                {
                    AddressBox.Visible = false;
                    btnSubmitOrder.Visible = false;
                }
                pageTitle = c.GetReqData("CustomersData", "CustomerName", "CustomrtID=" + Request.QueryString["custId"]).ToString() + " - PO";

                //string orderAmount = c.returnAggregate("Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID=" + ordIdX).ToString("0.00");
                string orderAmount = c.GetReqData("OrdersData", "OrderAmount", "OrderID=" + ordIdX).ToString();
                string finalOrderAmount = (Convert.ToDouble(orderAmount.ToString())).ToString("0.00");
                ordAmmount = "Total Amount : " + finalOrderAmount;
                //calcOrderTotalAmount();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetOrdersData", ex.Message.ToString());
            return;
        }
    }

    protected void gvOrderDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (c.IsRecordExist("Select ProdOptionID From ProductOptions Where FK_ProductID=" + e.Row.Cells[1].Text))
                {
                    int ProdOptId = Convert.ToInt32(c.GetReqData("OrderProductOptions", "FK_ProdOptionID", "FK_OrdDetailID=" + e.Row.Cells[0].Text));
                    int optGroupId = Convert.ToInt32(c.GetReqData("ProductOptions", "FK_OptionGroupID", "ProdOptionID=" + ProdOptId));
                    int optId = Convert.ToInt32(c.GetReqData("ProductOptions", "FK_OptionID", "ProdOptionID=" + ProdOptId));
                    string groupName = c.GetReqData("OptionGroups", "OptionGroupName", "OptionGroupID=" + optGroupId).ToString();
                    string optName = c.GetReqData("OptionsData", "OptionName", "OptionID=" + optId + " AND FK_OptionGroupID=" + optGroupId).ToString();
                    e.Row.Cells[2].Text += "<span class=\"space10\"></span> <span class=\"space1\"></span><span class=\"text-bold text-primary\">" + groupName + " : " + optName + "</span>";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvOrderDetails_RowDataBound", ex.Message.ToString());
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
    protected void gvOrderDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridViewRow gRow = (GridViewRow)((Button)e.CommandSource).NamingContainer;
            if (e.CommandName == "gvDel")
            {
                c.ExecuteQuery("Delete From OrdersDetails Where OrdDetailID=" + gRow.Cells[0].Text);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Medicine Deleted');", true);
            }

            object deliveryType = c.GetReqData("OrdersData", "DeliveryType", "OrderID=" + gRow.Cells[8].Text);
            int shipCharges = 0;
            if (deliveryType != DBNull.Value && deliveryType != null && deliveryType.ToString() != "")
            {
                if (deliveryType.ToString() == "2")
                {
                    shipCharges = 0;
                }
                else if (deliveryType.ToString() == "1")
                {
                    shipCharges = 30;
                }
                else
                {
                    shipCharges = 0;
                }
            }
            else
            {
                shipCharges = 0;
            }

            string orderAmount = c.returnAggregate("Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID=" + gRow.Cells[8].Text).ToString("0.00");
            string finalOrderAmount = (Convert.ToDouble(orderAmount.ToString()) + shipCharges).ToString("0.00");

            c.ExecuteQuery("Update OrdersData Set OrderAmount=" + Convert.ToDouble(finalOrderAmount) + " Where OrderID=" + gRow.Cells[8].Text);

            if (Convert.ToInt32(c.returnAggregate("Select Count(FK_DetailProductID) From OrdersDetails Where FK_DetailOrderID=" + gRow.Cells[8].Text)) <= 0)
            {
                c.ExecuteQuery("Delete From OrdersData Where OrderID=" + gRow.Cells[8].Text);
            }

            GetOrdersData(Convert.ToInt32(gRow.Cells[8].Text));
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvOrderDetails_RowCommand", ex.Message.ToString());
            return;
        }
    }

    protected void ddrCourier_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //courieroption.Visible = false;
            //if (ddrCourier.SelectedIndex == 5)
            //{
            //    courieroption.Visible = true;
            //}
            calcOrderTotalAmount();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ddlRelation_TextChanged", ex.Message.ToString());
            return;
        }
    }

    protected void btnSubmitOrder_Click(object sender, EventArgs e)
    {
        try
        {
            GridViewRow row = gvOrderDetails.Rows[0];

            int orderId = Convert.ToInt32(row.Cells[8].Text);

            txtCountry.Text = txtCountry.Text.Trim().Replace("'", "");
            txtState.Text = txtState.Text.Trim().Replace("'", "");
            txtCity.Text = txtCity.Text.Trim().Replace("'", "");
            txtPinCode.Text = txtPinCode.Text.Trim().Replace("'", "");
            txtAddress1.Text = txtAddress1.Text.Trim().Replace("'", "");
            txtCalendar.Text = txtCalendar.Text.Trim().Replace("'", "");

            //if (ddrAddress.SelectedIndex == 0 && txtAddress1.Text == "")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select or add address to continue');", true);
            //    return;
            //}

            //Assign Order
            int shopToAssign = 0;
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
                    if (!c.IsRecordExist("Select FranchID From FranchiseeData Where FranchShopCode='" + txtShopCode.Text + "' AND FranchActive=1"))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Shop with above shop code is not exists');", true);
                        return;
                    }
                    else
                    {
                        shopToAssign = Convert.ToInt32(c.GetReqData("FranchiseeData", "FranchID", "FranchShopCode='" + txtShopCode.Text + "' AND FranchActive=1"));
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

                int maxAddrId = c.NextId("CustomersAddress", "AddressID");
                c.ExecuteQuery("Insert Into CustomersAddress (AddressID, AddressFKCustomerID, AddressFull, AddressCity, AddressState, " +
                    " AddressPincode, AddressCountry, AddressStatus, AddressName) Values (" + maxAddrId + ", " + Request.QueryString["custId"] + ", '" + txtAddress1.Text +
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

            if (Request.QueryString["type"] != "")
            {
                if (Request.QueryString["type"] == "newOrd")
                {
                    deviceType = "FL-New Order";

                    int maxFlId = c.NextId("FollowupOrders", "FlupID");
                    if (Request.QueryString["ordId"] != null)
                    {
                        c.ExecuteQuery("Update FollowupOrders Set FlupStatus='Closed' Where FK_OrderId=" + Request.QueryString["ordId"] +
                            " AND FK_CustomerId=" + Request.QueryString["custId"]);
                    }

                    // Check is this Enquiry Referance Submit-PO, If YES then add "FlupEnquiryRef" column with enquiryref

                    c.ExecuteQuery("Insert Into FollowupOrders (FlupID, FlupDate, FK_CustomerId, FK_OrderId, FK_TeamMemberId, " +
                        " FlupRemarkStatusID, FlupRemarkStatus, FlupRemark, FlupNextDate, FlupNextTime, FlupRptOrderId, " +
                        " FlupStatus) Values (" + maxFlId + ", '" + DateTime.Now + "', " + Request.QueryString["custId"] +
                        ", " + orderId + ", " + Session["adminSupport"] + ", 7, 'Connected: New Order', " +
                        " 'New Order Placed', '" + nextFlDate + "', '11:00 AM', 0, 'Open')");

                    c.ExecuteQuery("Update CustomersData Set CallBusyFlag=0, CallBusyBy=NULL, RBNO_NextFollowup='" + nextFlDate + "' Where CustomrtID=" + Request.QueryString["custId"]);

                    if (Request.QueryString["ordId"] != null)
                    {
                        //update prev order followup status as inactive
                        c.ExecuteQuery("Update OrdersData Set FollowupStatus='Inactive' Where OrderID=" + Request.QueryString["ordId"]);
                    }

                    //Check If this Order referance is from Enquiry i.e. SavingCalc then update SavingCalc => FollowupStatus ='Close' (14-Apr-23 by Vinayak)
                    // Check is this Enquiry Referance Submit-PO, If YES then add "FlupEnquiryRef" column with enquiryref into FollowupOrders table.
                    if (Request.QueryString["enqref"] != null)
                    {
                        c.ExecuteQuery("Update SavingCalc Set FollowupStatus='Inactive' Where CalcID=" + Request.QueryString["enqref"]);
                        c.ExecuteQuery("Update FollowupOrders set FlupEnquiryRef=" + Request.QueryString["enqref"] + " Where FlupID=" + maxFlId);
                    }
                }
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
                                       "', FollowupNextTime='11:00 AM', FollowupStatus='Active', OrderPayMode = " + codFlag + ", UPIID = '" + txtUPIID.Text + "', CourierOptions = '" + ddrCourier.SelectedItem.Text + "' Where OrderID=" + orderId);
            }
            else if (ddrCourier.SelectedIndex == 5)
            {
                c.ExecuteQuery("Update OrdersData Set OrderStatus=3, FK_AddressId=" + addressId + ", MreqFlag=" + mreq +
                                       ", DeviceType='" + deviceType + "', FollowupLastDate='" + DateTime.Now + "', FollowupNextDate='" + nextFlDate +
                                       "', FollowupNextTime='11:00 AM', FollowupStatus='Active', OrderPayMode = " + codFlag + ", UPIID = '" + txtUPIID.Text + "', CourierOptions = '" + txtCourier.Text + "' Where OrderID=" + orderId);
            }
            else if (ddrCourier.SelectedIndex == 0)
            {
                c.ExecuteQuery("Update OrdersData Set OrderStatus=3, FK_AddressId=" + addressId + ", MreqFlag=" + mreq +
                                       ", DeviceType='" + deviceType + "', FollowupLastDate='" + DateTime.Now + "', FollowupNextDate='" + nextFlDate +
                                       "', FollowupNextTime='11:00 AM', FollowupStatus='Active', OrderPayMode = " + codFlag + ", UPIID = '" + txtUPIID.Text + "', CourierOptions = '-' Where OrderID=" + orderId);
            }            

            // insert into OrdersAssign table
            int maxAssignId = c.NextId("OrdersAssign", "OrdAssignID");
            c.ExecuteQuery("Insert Into OrdersAssign (OrdAssignID, OrdAssignDate, FK_OrderID, Fk_FranchID, OrdAssignStatus, " +
                " OrdReAssign, AssignedFrom) Values (" + maxAssignId + ", '" + DateTime.Now + "', " + orderId + ", " + shopToAssign +
                ", 0, 0, '" + deviceType + "')");

            // Check is this GOBP is of isMLM type i.e. Members of Lakshman Ambi, then go for level wise sales commission calculation.
            // 8-Sept-2023 (Vinayak)
            object GOBP_MLM = c.GetReqData("OBPData", "OBP_ID", "OBP_ID=(Select GOBPId From OrdersData where OrderID =" + orderId + ")");

            if (GOBP_MLM != null && Convert.ToInt32(GOBP_MLM) != 0)
            {
                if (c.IsRecordExist("Select OBP_ID From OBPData Where IsMLM=1 AND OBP_ID=" + Convert.ToInt32(GOBP_MLM)) == true)
                {
                    c.GOBP_Sales_CommissionChain(Convert.ToInt32(GOBP_MLM), orderId);
                }
            }

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Order Submitted Successfully !');", true);
            if (Request.QueryString["type"] != "")
            {
                if (Request.QueryString["type"] == "newOrd")
                {
                    //followup-order-report
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('followup-order-report.aspx', 2000);", true);
                    Response.Redirect("dashboard.aspx", false);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('staff-followup-all-orders.aspx', 2000);", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('staff-followup-all-orders.aspx', 2000);", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSubmitOrder_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnRmPaymentSlip_Click(object sender, EventArgs e)
    {
        try
        {
            GridViewRow row = gvOrderDetails.Rows[0];

            int orderId = Convert.ToInt32(row.Cells[8].Text);

            int payslipid = Convert.ToInt32(c.GetReqData("[dbo].[OrdersPaymentSlip]", "TOP 1 [OrdPayId]", "[FK_OrderId] = " + orderId + " ORDER BY [OrdPayId] DESC").ToString());

            c.ExecuteQuery("DELETE [dbo].[OrdersPaymentSlip] WHERE [OrdPayId] = " + payslipid + " AND [FK_OrderId] = " + orderId);

            GetUploadedPrescription(orderId);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnRmPaymentSlip_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnPaymentSlip_Click(object sender, EventArgs e)
    {
        try
        {
            GridViewRow row = gvOrderDetails.Rows[0];

            int orderId = Convert.ToInt32(row.Cells[8].Text);

            string imgName = "";
            int payslipId = c.NextId("OrdersPaymentSlip", "OrdPayId");
            if (fuPaymentSlip.HasFile)
            {
                string fExt = Path.GetExtension(fuPaymentSlip.FileName).ToString().ToLower();
                if (fExt == ".jpg" || fExt == ".jpeg" || fExt == ".png")
                {
                    imgName = "payment-slip-" + orderId + fExt;
                    ImageUploadProcess(imgName);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg or .png files are allowed');", true);
                    //return;
                }

                c.ExecuteQuery("Insert Into OrdersPaymentSlip (OrdPayId, OrdPayDate, OrdPaySlip, FK_OrderId) " +
                    " Values (" + payslipId + ", '" + DateTime.Now + "', '" + imgName + "', " + orderId + ")");

                GetUploadedPrescription(orderId);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select image to upload prescription');", true);
                //return;
            }
            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + Master.rootPath + "checkout', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnPaymentSlip_Click", ex.Message.ToString());
            return;
        }
    }
}