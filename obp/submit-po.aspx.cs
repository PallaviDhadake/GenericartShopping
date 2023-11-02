using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class obp_submit_po : System.Web.UI.Page
{
    iClass c = new iClass();
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["custId"] != null)
                {
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

                c.ExecuteQuery("Insert Into OrdersData (OrderID, FK_OrderCustomerID, OrderDate, OrderStatus, OrderType, DeviceType, OrderPayStatus, GOBPId) Values (" + orderId +
                   ", " + customerId + ", '" + DateTime.Now + "', 0, 1, 'GOBP-Order', 0, " + Session["adminObp"] + ")");
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
            string finalOrderAmount = (Convert.ToDouble(orderAmount.ToString()) + shipCharges).ToString("0.00");

            c.ExecuteQuery("Update OrdersData Set OrderAmount=" + Convert.ToDouble(finalOrderAmount) + " Where OrderID=" + orderId);
            double gobpCom = c.GetOBPComission(orderId);
            c.ExecuteQuery("Update OrdersData Set OBPComTotal=" + gobpCom + " Where OrderID=" + orderId);

            GetOrdersData(orderId);

            txtMedName.Text = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnAdd_Click", ex.Message.ToString());
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

            //if (ddrAddress.SelectedIndex == 0 && txtAddress1.Text == "")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select or add address to continue');", true);
            //    return;
            //}
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

            c.ExecuteQuery("Update OrdersData Set OrderStatus=1, FK_AddressId=" + addressId + " Where OrderID=" + orderId);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Order Submitted Successfully !');", true);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('dashboard.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSubmitOrder_Click", ex.Message.ToString());
            return;
        }
    }
}