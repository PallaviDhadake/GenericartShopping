using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web.Services;

public partial class cart : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnUpdateQty.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnUpdateQty, null) + ";");

        if (!IsPostBack)
        {
            HttpCookie cartItemCookie = Request.Cookies["ordId"];
            if (Request.QueryString["action"] != null && Request.QueryString["id"] != null)
            {
                string prodId = Request.QueryString["id"];
                if (cartItemCookie != null)
                {
                    string[] arrOrd = cartItemCookie.Value.Split('#');
                    int orderId = Convert.ToInt32(arrOrd[0].ToString());
                    int ordDetailID = Convert.ToInt32(c.GetReqData("OrdersDetails", "OrdDetailID", "FK_DetailOrderID=" + orderId + " AND FK_DetailProductID=" + prodId));
                    c.ExecuteQuery("Delete From OrderProductOptions Where FK_OrdDetailID=" + ordDetailID);
                    c.ExecuteQuery("Delete From OrdersDetails Where FK_DetailOrderID=" + orderId + " AND FK_DetailProductID=" + prodId);

                    if (Convert.ToInt32(c.returnAggregate("Select Count(OrdDetailID) From OrdersDetails Where FK_DetailOrderID=" + orderId)) <= 0)
                    {
                        c.ExecuteQuery("Delete From OrdersData Where OrderID=" + orderId);
                        // delete all cookies
                        Response.Cookies["ordId"].Expires = DateTime.Now.AddDays(-1);

                        Response.Redirect(Master.rootPath + "cart", false);
                    }

                    FillCartGrid();
                    Response.Redirect(Master.rootPath + "cart", false);
                }
            }

            if (cartItemCookie != null)
            {
                //string[] arrOrd = cartItemCookie.Value.Split('#');
                //int orderId = Convert.ToInt32(arrOrd[0].ToString());
                //object ordNote = c.GetReqData("OrdersData", "OrderNote", "OrderID=" + orderId);
                //if (ordNote != DBNull.Value && ordNote != null && ordNote.ToString() != "")
                //    txtNote.Text = ordNote.ToString();
                //else
                //    txtNote.Text = "";

                FillCartGrid();
            }
            else
            {
                cartMarkup.Visible = false;
                errMsg = "<div class=\"txtCenter\"><span class=\"large semiBold\" style=\"color:#1db1b0;\">Oops ! No Pending Medicine Request</span><span class=\"space5\"></span><span class=\"medium clrBlack fontRegular\">Looks like you haven't made your choice yet</span> <span class=\"space15\"></span> <a href=\"" + Master.rootPath + "\" class=\"readAnch semiBold upperCase letter-sp-2\">Add More Products</a> <span class=\"space80\"></span>	<span class=\"space30\"></span></div>";
            }
        }
    }

    private void FillCartGrid()
    {
        try
        {
            HttpCookie ordCookie = Request.Cookies["ordId"];     // Get Cookies Value
            if (ordCookie != null) // Check whether cookies are not null
            {
                string[] arrOrd = ordCookie.Value.Split('#'); // if cookies are not null, split its value by '#' and get its orderId
                int orderId = Convert.ToInt32(arrOrd[0].ToString());
                string cartItems = "";
                using (DataTable dtProducts = c.GetDataTable("Select FK_DetailProductID From OrdersDetails Where FK_DetailOrderID=" + orderId))
                {
                    if (dtProducts.Rows.Count > 0)
                    {
                        foreach (DataRow prRow in dtProducts.Rows)
                        {
                            if (cartItems == "")
                            {
                                cartItems = prRow["FK_DetailProductID"].ToString();
                            }
                            else
                            {
                                cartItems = cartItems + ", " + prRow["FK_DetailProductID"].ToString();
                            }
                        }
                    }
                }

                //using (DataTable dtCartProducts = c.GetDataTable("Select ProductID, ProductName, PriceFinal, FK_SubCategoryID, ProductPhoto From ProductsData Where ProductID IN (" + cartItems + ") Order By ProductID DESC"))
                using (DataTable dtCartProducts = c.GetDataTable("Select Distinct a.ProductID, a.ProductName, a.FK_SubCategoryID, " +
                    " a.ProductPhoto, b.OrdDetailPrice as PriceFinal From ProductsData a Inner Join OrdersDetails b " +
                    " On a.ProductID=b.FK_DetailProductID Where a.ProductID IN (" + cartItems + ") AND b.FK_DetailOrderID=" + orderId + " Order By a.ProductID DESC"))
                {
                    gvCartItems.DataSource = dtCartProducts;
                    gvCartItems.DataBind();

                    double subTotal = 0.00;
                    if (dtCartProducts.Rows.Count > 0)
                    {
                        foreach (DataRow cpRow in dtCartProducts.Rows)
                        {
                            int qty = Convert.ToInt32(c.GetReqData("OrdersDetails", "OrdDetailQTY", "FK_DetailOrderID=" + orderId + " AND FK_DetailProductID=" + cpRow["ProductID"]));

                            //double prodPrice = Convert.ToDouble(c.GetReqData("OrdersDetails", "OrdDetailPrice", "FK_DetailOrderID=" + orderId + " AND FK_DetailProductID=" + cpRow["ProductID"]));
                            subTotal = (subTotal + (Convert.ToDouble(cpRow["PriceFinal"].ToString()) * qty));
                            //subTotal = (subTotal + (prodPrice * qty));
                        }
                    }

                    lblSubTotal.Text = subTotal.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void gvCartItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litProdPhoto = (Literal)e.Row.FindControl("litProdPhoto");
                litProdPhoto.Text = "<img src=\"" + Master.rootPath + "upload/products/thumb/" + e.Row.Cells[1].Text + "\" alt=\"" + e.Row.Cells[4].Text + " - Genericart Generic Medicine\" class=\"width100 prodImg\" />";

                Literal litCategory = (Literal)e.Row.FindControl("litCategory");
                string catName = c.GetReqData("ProductCategory", "ProductCatName", "ProductCatID=" + e.Row.Cells[2].Text).ToString();
                litCategory.Text = catName.ToString();

                TextBox txtQty = (TextBox)e.Row.FindControl("txtQuantity");

                Label lblTotal = (Label)e.Row.FindControl("lblTotal");
                double prodPrice = Convert.ToDouble(e.Row.Cells[6].Text);

                int orderId = 0;
                HttpCookie ordCookie = Request.Cookies["ordId"];
                if (ordCookie != null)
                {
                    string[] arrOrd = ordCookie.Value.Split('#');
                    orderId = Convert.ToInt32(arrOrd[0].ToString());

                    int qty = Convert.ToInt32(c.GetReqData("OrdersDetails", "OrdDetailQTY", "FK_DetailOrderID=" + orderId + " AND FK_DetailProductID=" + e.Row.Cells[0].Text));

                    txtQty.Text = qty.ToString();
                    lblTotal.Text = ((prodPrice) * Convert.ToInt32(txtQty.Text)).ToString();
                }

                Literal litRemove = (Literal)e.Row.FindControl("litRemove");
                litRemove.Text = "<a href=\"" + Master.rootPath + "cart?action=remove&id=" + e.Row.Cells[0].Text + "\" class=\"deleteProd\"></a>";

                //e.Row.Cells[4].Text += "<span class=\"space10\"></span><span class=\"semiBold\">Unit Price : &#8377; " + e.Row.Cells[6].Text + "</span>";
                

                CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                int remindFlag = Convert.ToInt32(c.GetReqData("ProductsData", "RemindFlag", "ProductID=" + e.Row.Cells[0].Text));
                if (remindFlag == 0)
                {
                    chkSelect.Visible = false;
                }

                if (c.IsRecordExist("Select ProdOptionID From ProductOptions Where FK_ProductID=" + e.Row.Cells[0].Text))
                {
                    int ordDetailId = Convert.ToInt32(c.GetReqData("OrdersDetails", "OrdDetailID", "FK_DetailOrderID=" + orderId + " AND FK_DetailProductID=" + e.Row.Cells[0].Text));
                    int ProdOptId = Convert.ToInt32(c.GetReqData("OrderProductOptions", "FK_ProdOptionID", "FK_OrdDetailID=" + ordDetailId));
                    int optGroupId = Convert.ToInt32(c.GetReqData("ProductOptions", "FK_OptionGroupID", "ProdOptionID=" + ProdOptId));
                    int optId = Convert.ToInt32(c.GetReqData("ProductOptions", "FK_OptionID", "ProdOptionID=" + ProdOptId));
                    string groupName = c.GetReqData("OptionGroups", "OptionGroupName", "OptionGroupID=" + optGroupId).ToString();
                    string optName = c.GetReqData("OptionsData", "OptionName", "OptionID=" + optId + " AND FK_OptionGroupID=" + optGroupId).ToString();
                    e.Row.Cells[4].Text += "<span class=\"space10\"></span><span class=\"semiBold small\">Unit Price : &#8377; " + e.Row.Cells[6].Text + "</span> <span class=\"space1\"></span><span class=\"semiBold small\">" + groupName + " : " + optName + "</span>";
                }
                else
                {
                    e.Row.Cells[4].Text += "<span class=\"space10\"></span><span class=\"semiBold small\">Unit Price : &#8377; " + e.Row.Cells[6].Text + "</span>";
                }


                int prescriptionFlag = Convert.ToInt32(c.GetReqData("ProductsData", "PrescriptionFlag", "ProductID=" + e.Row.Cells[0].Text));
                if (prescriptionFlag == 1)
                {
                    e.Row.Cells[4].Text += "<span class=\"space10\"></span><span class=\"colrPink semiBold small\"><span class=\"reqPrescription\">Rx</span> Prescription Required</span>";
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void gvCartItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCartItems.PageIndex = e.NewPageIndex;
        FillCartGrid();
    }

    protected void btnUpdateQty_Click(object sender, EventArgs e)
    {
        try
        {
            HttpCookie ordCookie = Request.Cookies["ordId"];
            int orderId = 0;
            if (ordCookie != null)
            {
                string[] arrOrd = ordCookie.Value.Split('#');
                orderId = Convert.ToInt32(arrOrd[0].ToString());
            }

            if (orderId != 0)
            {
                //if (txtNote.Text != "")
                //{
                //    if (txtNote.Text.Length > 200)
                //    {
                //        errMsg = c.ErrNotification(2, "Note must be less than 200 characters");
                //        return;
                //    }
                //}

                //c.ExecuteQuery("Update OrdersData Set OrderNote='" + txtNote.Text + "' Where OrderID=" + orderId);

                foreach (GridViewRow row in gvCartItems.Rows)
                {
                    TextBox txtQty = (TextBox)row.FindControl("txtQuantity");
                    double prodPrice = Convert.ToDouble(row.Cells[6].Text);
                    double subTotal = prodPrice * (Convert.ToInt32(txtQty.Text));
                    //double subTotal = Convert.ToDouble(row.Cells[8].Text);
                    //c.ExecuteQuery("Update OrdersDetails Set OrdDetailQTY=" + Convert.ToInt32(txtQty.Text) + ", OrdDetailPrice=" + subTotal +
                    //    " Where FK_DetailOrderID=" + orderId + " AND FK_DetailProductID=" + row.Cells[0].Text);
                    c.ExecuteQuery("Update OrdersDetails Set OrdDetailQTY=" + Convert.ToInt32(txtQty.Text) + ", OrdDetailAmount=" + subTotal +
                        " Where FK_DetailOrderID=" + orderId + " AND FK_DetailProductID=" + row.Cells[0].Text);
                }

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Cart Updated');", true);
                FillCartGrid();
                Response.Redirect(Master.rootPath + "cart", false);
            }
            else
            {
                errMsg = c.ErrNotification(3, "Something Went Wrong..!!");
                return;
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void btnCheckout_Click(object sender, EventArgs e)
    {
        try
        {
            HttpCookie ordCookie = Request.Cookies["ordId"];
            int orderId = 0;
            if (ordCookie != null)
            {
                string[] arrOrd = ordCookie.Value.Split('#');
                orderId = Convert.ToInt32(arrOrd[0].ToString());
            }
            if (orderId != 0)
            {
                int monthlyFlag = chkMreq.Checked == true ? 1 : 0;
                c.ExecuteQuery("Update OrdersData Set MreqFlag=" + monthlyFlag + " Where OrderID=" + orderId);
            }

            int custId = Session["genericCust"] != null ? Convert.ToInt32(Session["genericCust"]) : 0;

            int remindCount = 0;
            string remindId = "";
            double subTotal = 0.0;
            foreach (GridViewRow gRow in gvCartItems.Rows)
            {
                CheckBox chk = (CheckBox)gRow.FindControl("chkSelect");
                if (chk.Checked)
                {
                    remindCount++;
                    int pillMaxID = c.NextId("PillReminder", "RemindID");
                    string[] arrMedName = gRow.Cells[4].Text.Split('<');
                    c.ExecuteQuery("Insert Into PillReminder (RemindID, FK_CustomerID, RemindMedicine, RemindStatus) Values (" + pillMaxID +
                        ", " + custId + ", '" + arrMedName[0].ToString() + "', 1)");

                    if (remindId == "")
                    {
                        remindId = pillMaxID.ToString();
                    }
                    else
                    {
                        remindId = remindId + "-" + pillMaxID.ToString();
                    }
                }

                //subTotal = subTotal + Convert.ToDouble(gRow.Cells[8].Text);
                //TextBox txtQty = (TextBox)gRow.FindControl("txtQuantity");
                //double prodPrice = Convert.ToDouble(gRow.Cells[6].Text);
                //double sTotal = prodPrice * (Convert.ToInt32(txtQty.Text));

                Label lblFinalAmt = (Label)gRow.FindControl("lblTotal");
                subTotal = subTotal + Convert.ToDouble(lblFinalAmt.Text);
            }

            if (subTotal < 100)
            {
                //errMsg = c.ErrNotification(2, "Minimum cart value must be 100 Rs.");
                errMsg = "<span style=\"font-size:1.8em; color:#ff0000; font-weight:600; display:block;\">Note : Minimum cart value must be Rs. 100</span>";
                return;
            }

            if (Session["genericCust"] != null)
            {
                Response.Redirect(Master.rootPath + "checkout", false);
            }
            else
            {
                if (remindCount > 0)
                {
                    //Response.Redirect(Master.rootPath + "login?ref=cart&remind=" + remindId, false);  old
                    Response.Redirect(Master.rootPath + "registration?ref=cart&remind=" + remindId, false); //new on req from sujata mam
                }
                else 
                {
                    //Response.Redirect(Master.rootPath + "login?ref=cart", false); old
                    Response.Redirect(Master.rootPath + "registration?ref=cart", false); //new on req from sujata mam
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}