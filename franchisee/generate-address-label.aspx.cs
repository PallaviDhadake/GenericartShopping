using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class franchisee_generate_address_label : System.Web.UI.Page
{
    iClass c = new iClass();
    public string rxStr, errMsg;
    public string[] custData = new string[10];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["frid"].ToString() != null && Request.QueryString["date"].ToString() != null)
        {
            GenerateMail();
        }
    }

    private void GenerateMail()
    {
        try
        {
            StringBuilder strMarkup = new StringBuilder();
            
            DateTime Date;
            Date = Convert.ToDateTime(Request.QueryString["date"].ToString());

            using (DataTable dtCust = c.GetDataTable(@"SELECT 
                            CD.[CustomrtID],
                            OD.[OrderID],
                            CD.[CustomerName] AS Name,
                            CD.[CustomerMobile] AS MobileNo,
                            CD.[CustomerEmail] As Email,
                            CD.[CustomerCity] AS CustomerCity,
                            CD.[CustomerState] AS CustomerState,
                            CD.[CustomerPincode] AS CustomerPincode,
                            CD.[CustomerAddress] AS CustomerAddress,
                            OD.[OrderAmount] AS OrderAmount,
                            OD.[OrderDate],
                            OD.[FK_AddressId],
                            (SELECT COUNT([OrdDetailID]) FROM[dbo].[OrdersDetails] WHERE[FK_DetailOrderID] = OD.[OrderID]) AS MedicineCount
                        FROM[dbo].[CustomersData] AS CD
                        INNER JOIN[dbo].[OrdersData] AS OD ON CD.[CustomrtID] = OD.[FK_OrderCustomerID]
                        INNER JOIN[dbo].[OrdersAssign] AS OA ON OD.[OrderID] = OA.[FK_OrderID]
                        INNER JOIN[dbo].[FranchiseeData] AS FD ON OA.[Fk_FranchID] = FD.[FranchID]
                        WHERE OD.[OrderStatus] = 6
                        AND OA.[Fk_FranchID] = " + Request.QueryString["frid"].ToString() + " AND CONVERT(VARCHAR(20),OA.[OrdAssignDate],112) = CONVERT(VARCHAR(20), CAST('" + Date + "' AS DATE), 112) Order By CD.[CustomrtID] ASC"))
            {               
                foreach (DataRow row in dtCust.Rows)
                {
                    custData[0] = row["CustomrtID"].ToString();
                    custData[1] = row["Name"].ToString();
                    custData[2] = row["MobileNo"].ToString();
                    custData[3] = row["Email"].ToString();

                    if (row["FK_AddressId"] != DBNull.Value && row["FK_AddressId"] != null && row["FK_AddressId"].ToString() != "")
                    {
                        using (DataTable dtCustAddr = c.GetDataTable("Select AddressID, AddressName, AddressFull, AddressCity, AddressState, AddressPinCode, AddressCountry From CustomersAddress Where AddressFKCustomerID = " + row["CustomrtID"].ToString() + " And AddressID = " + row["FK_AddressId"]))
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
                    strMarkup.Append("<span class=\"space15\"></span>");
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

                    string name = c.GetReqData("FranchiseeData", "FranchName", "FranchID = " + Request.QueryString["frid"].ToString()).ToString();
                    string mobile = c.GetReqData("FranchiseeData", "FranchMobile", "FranchID = " + Request.QueryString["frid"].ToString()).ToString();
                    string address = c.GetReqData("FranchiseeData", "FranchAddress", "FranchID = " + Request.QueryString["frid"].ToString()).ToString();

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
                    strMarkup.Append("<span class=\"space20\"></span>");
                    strMarkup.Append("<div class=\"rxBottomLine\"></div>");
                    rxStr = strMarkup.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            rxStr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}