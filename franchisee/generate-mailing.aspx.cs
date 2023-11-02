using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class franchisee_generate_mailing : System.Web.UI.Page
{
    iClass c = new iClass();
    public string rxStr, errMsg;
    public string[] custData = new string[10];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["id"].ToString() != null)
        {
            GenerateMail();
        }
    }

    private void GenerateMail()
    {
        try
        {
            StringBuilder strMarkup = new StringBuilder();
            int custid = Convert.ToInt32(c.GetReqData("OrdersData", "FK_OrderCustomerID", "OrderID = " + Request.QueryString["id"].ToString()).ToString());
            using (DataTable dtCust = c.GetDataTable("Select CustomerName, CustomerMobile, CustomerEmail, CustomerAddress, CustomerCity, CustomerState, CustomerPincode From CustomersData Where CustomrtID = " + custid))
            {
                if (dtCust.Rows.Count > 0)
                {
                    DataRow row = dtCust.Rows[0];
                    // cust name
                    custData[0] = Request.QueryString["id"].ToString();
                    custData[1] = row["CustomerName"].ToString();
                    custData[2] = row["CustomerMobile"].ToString();
                    custData[3] = row["CustomerEmail"].ToString();

                    string add = c.GetReqData("OrdersData", "FK_AddressId", "OrderID = " + Request.QueryString["id"].ToString()).ToString();
                    //if (c.IsRecordExist("Select AddressID From CustomersAddress Where AddressFKCustomerID=" + customerId + " AND AddressID=" + bRow["FK_AddressId"]))
                    if (add != null && add.ToString() != "")
                    {
                        using (DataTable dtCustAddr = c.GetDataTable("Select AddressID, AddressName, AddressFull, AddressCity, AddressState, AddressPincode, AddressCountry From CustomersAddress Where AddressFKCustomerID=" + custid + " And AddressID = " + add))
                        {
                            if (dtCustAddr.Rows.Count > 0)
                            {
                                DataRow cRow = dtCustAddr.Rows[0];

                                custData[4] = cRow["AddressCity"].ToString();
                                custData[5] = cRow["AddressState"].ToString();
                                custData[6] = cRow["AddressPincode"].ToString();
                                custData[7] = cRow["AddressFull"].ToString() + "<span class=\"bold_weight\" style=\"display:inline-block !important;\"> (" + cRow["AddressName"].ToString() + ")</span>";
                            }
                        }
                    }
                    else
                    {
                        custData[4] = row["CustomerCity"].ToString();
                        custData[5] = row["CustomerState"].ToString();
                        custData[6] = row["CustomerPincode"].ToString();
                        custData[7] = row["CustomerAddress"].ToString();
                    }

                    strMarkup.Append("<div class=\"float_left\">");
                    strMarkup.Append("<div class=\"rxMail\">");
                    strMarkup.Append("<h4 class=\"clrBlack mrg_B_5 small\"> To, </h4>");
                    strMarkup.Append("<h4 class=\"clrBlack mrg_B_5 small\">" + custData[1] + "</h4>");
                    strMarkup.Append("<h4 class=\"clrBlack mrg_B_5 small\">" + custData[3] + "</h4>");                   
                    strMarkup.Append("<h4 class=\"clrBlack mrg_B_5 small\">" + custData[7] + " , " + custData[5] + "</h4>");
                    strMarkup.Append("<h4 class=\"clrBlack mrg_B_5 small\">" + custData[4] + " : " + custData[6] + "</h4>");
                    strMarkup.Append("<span class=\"space5\"></span>");
                    strMarkup.Append("<h4 class=\"clrBlack mrg_B_5 small\">Contact : " + custData[2] + "</h4>");
                    strMarkup.Append("<span class=\"space15\"></span>");
                    strMarkup.Append("</div>");
                    strMarkup.Append("</div>");
                    strMarkup.Append("<div class=\"float_clear\"></div>");
                    strMarkup.Append("<div class=\"rxLine\"></div>");
                    strMarkup.Append("<span class=\"space15\"></span>");

                    strMarkup.Append("<div class=\"float_left\">");

                    //int ordid = Convert.ToInt32(c.GetReqData("OrdersData", "OrderID", "FK_OrderCustomerID = " + Request.QueryString["id"].ToString()).ToString());
                    int Franchiseeid = Convert.ToInt32(c.GetReqData("OrdersAssign", "Fk_FranchID", "FK_OrderID = " + Request.QueryString["id"].ToString()).ToString());
                    string name = c.GetReqData("FranchiseeData", "FranchName", "FranchID = " + Franchiseeid).ToString();
                    string mobile = c.GetReqData("FranchiseeData", "FranchMobile", "FranchID = " + Franchiseeid).ToString();
                    string address = c.GetReqData("FranchiseeData", "FranchAddress", "FranchID = " + Franchiseeid).ToString();

                    strMarkup.Append("<h4 class=\"clrBlack mrg_B_5 small\">From, </h4>");
                    strMarkup.Append("<h4 class=\"clrBlack mrg_B_5 small\">Shop Name : " + name + "</h4>");
                    strMarkup.Append("<h4 class=\"clrBlack mrg_B_5 small\">Mobile No. : " + mobile + "</h4>");
                    strMarkup.Append("<h4 class=\"clrBlack mrg_B_5 small\">Address : " + address + "</h4>");
                    strMarkup.Append("</div>");
                    strMarkup.Append("<div class=\"float-clear\">");

                    strMarkup.Append("<span class=\"space80\"></span>");
                    strMarkup.Append("<span class=\"space50\"></span>");
                    strMarkup.Append("<div class=\"rxBottomLine\"></div>");
                    strMarkup.Append("<span class=\"space20\"></span>");
                    strMarkup.Append("<h4 class=\"clrDarkGrey mrg_B_5 small\" style=\"text-align: center\">www.genericartmedicine.com</h4>");
                    rxStr = strMarkup.ToString();
                }
            }
        }
        catch(Exception ex)
        {
            rxStr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}