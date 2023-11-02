using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class admingenshopping_enquiry_details : System.Web.UI.Page
{
    iClass c = new iClass();
    public string customerId, errMsg, orderCount, shippingCharges, prescriptionStr, rdrUrl, mreq, deviceType;

    public string[] ordData = new string[20]; //15
    public string[] ordCustData = new string[10];

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                rdrUrl = "enquiry-report.aspx";
                GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));

                btnAssignOrder.Visible = false;

                int ordStatus = Convert.ToInt32(c.GetReqData("SavingCalc", "EnqStatus", "CalcID=" + Request.QueryString["id"]));
                if (ordStatus == 1 || ordStatus == 2 || ordStatus == 3 || ordStatus == 4 || ordStatus == 5 || ordStatus == 6 || ordStatus == 7 || ordStatus == 8 || ordStatus == 9) // accepted, denied, inprocess, shipped, delivered, rejected by 0001, Order amt low
                {
                    //btnSubmit.Visible = false;
                    //btnDeny.Visible = false;
                    btnAssignOrder.Visible = true;
                }

                if (ordStatus == 4)
                {
                    btnAssignOrder.Visible = false;
                }

                object mreFlag = c.GetReqData("SavingCalc", "MreqFlag", "CalcID=" + Request.QueryString["id"]);
                if (mreFlag != DBNull.Value && mreFlag != null && mreFlag.ToString() != "")
                {
                    if (mreFlag.ToString() == "1")
                    {
                        mreq = "<span class=\"medium clrProcessing bold_weight\">Customer Marked This Enquiry as Monthly Order</span><span class=\"space10\"></span>";
                    }
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

    public void GetOrdersData(int Idx)
    {
        try
        {
            using (DataTable dtOrder = c.GetDataTable("Select * From SavingCalc Where CalcID=" + Idx))
            {
                if (dtOrder.Rows.Count > 0)
                {
                    DataRow bRow = dtOrder.Rows[0];
                    ordData[0] = Idx.ToString();


                    ordData[1] = Convert.ToDateTime(bRow["CalcDate"]).ToString("dd/MM/yyyy");
                    ordData[11] = Convert.ToDateTime(bRow["CalcDate"]).ToString("hh:mm tt");
                    deviceType = bRow["DeviceType"] != DBNull.Value && bRow["DeviceType"] != null && bRow["DeviceType"].ToString() != "" ? bRow["DeviceType"].ToString() : "";

                    if (bRow["FK_AddressId"] != DBNull.Value && bRow["FK_AddressId"] != null && bRow["FK_AddressId"].ToString() != "")
                    {
                        using (DataTable dtCustAddr = c.GetDataTable("Select AddressID, AddressFKCustomerID, AddressName, AddressFull, AddressCity, AddressState, AddressPincode, AddressCountry From CustomersAddress Where AddressID=" + bRow["FK_AddressId"]))
                        {
                            if (dtCustAddr.Rows.Count > 0)
                            {
                                DataRow row = dtCustAddr.Rows[0];

                                ordData[4] = row["AddressFull"].ToString() + " <span class=\"bold_weight\" style=\"display:inline-block !important;\"> (" + row["AddressName"].ToString() + ")</span>";

                                ordData[6] = row["AddressCity"].ToString();
                                ordData[7] = row["AddressState"].ToString();
                                ordData[8] = row["AddressPincode"].ToString();
                                ordData[9] = row["AddressCountry"].ToString();
                            }
                        }
                    }

                    customerId = bRow["FK_CustId"].ToString();

                    if (bRow["EnqStatus"].ToString() == "10")
                    {
                        ordReturned.Visible = true;
                        ordData[18] = c.GetReqData("SavingEnqAssign", "ReturnReason", "FK_CalcID=" + Request.QueryString["id"] + " AND ReturnReason IS NOT NULL AND ReturnReason<>''").ToString();
                    }


                    //orderStatus.SelectedValue = bRow["OrderStatus"].ToString();

                    using (DataTable dtCust = c.GetDataTable("Select CustomerName, CustomerMobile, CustomerEmail, CustomerAddress, CustomerCity, CustomerState, CustomerPincode From CustomersData Where CustomrtID = " + customerId))
                    {
                        if (dtCust.Rows.Count > 0)
                        {
                            DataRow row = dtCust.Rows[0];

                            ordCustData[0] = row["CustomerName"].ToString();
                            ordCustData[1] = row["CustomerMobile"].ToString();
                            ordCustData[2] = row["CustomerEmail"].ToString();

                            
                            if (bRow["FK_AddressId"] != DBNull.Value && bRow["FK_AddressId"] != null && bRow["FK_AddressId"].ToString() != "")
                            {
                                using (DataTable dtCustAddr = c.GetDataTable("Select AddressID, AddressName, AddressFull, AddressCity, AddressState, AddressPincode, AddressCountry From CustomersAddress Where AddressFKCustomerID=" + customerId + " AND AddressID=" + bRow["FK_AddressId"]))
                                {
                                    if (dtCustAddr.Rows.Count > 0)
                                    {
                                        DataRow cRow = dtCustAddr.Rows[0];

                                        ordCustData[3] = cRow["AddressCity"].ToString();
                                        ordCustData[4] = cRow["AddressState"].ToString();
                                        ordCustData[5] = cRow["AddressPincode"].ToString();
                                        ordCustData[6] = cRow["AddressFull"].ToString() + "<span class=\"bold_weight\" style=\"display:inline-block !important;\"> (" + cRow["AddressName"].ToString() + ")</span>";
                                    }
                                }
                            }
                        }
                    }

                }
            }

            using (DataTable dtProduct = c.GetDataTable("Select a.BrandMedicine, 'Rs. ' + Convert(varchar(20), a.BrandPrice)  as BrandPrice, 'Rs. ' + Convert(varchar(20), a.GenericPrice) as GenericPrice , a.GenericMedicine, a.GenericCode, a.SavingAmount, Convert(varchar(20), a.SavingPercent) + '%' as SavingPercent from SavingCalcItems a where a.FK_CalcID =" + Idx))
            {

                gvOrderDetails.DataSource = dtProduct;
                gvOrderDetails.DataBind();
            }

            orderCount = c.returnAggregate("Select COUNT(a.CalcID) From SavingCalc a Join CustomersData b On b.CustomrtID = a.FK_CustId Where b.CustomrtID=" + customerId + " AND b.delMark = 0").ToString();

        }
        catch (Exception ex)
        {

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetOrdersData", ex.Message.ToString());
            return;

        }
    }
    protected void btnAssignOrder_Click(object sender, EventArgs e)
    {
        Response.Redirect("assign-enquiry.aspx?id=" + Request.QueryString["id"], false);
    }
}