using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class admingenshopping_route_orders : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnRoute.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnRoute, null) + ";");
    }

    //protected void btnRoute_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        using(DataTable dtOrders = c.GetDataTable("Select a.OrderID, a.FK_OrderCustomerId, a.OrderDate, a.OrderAmount, a.OrderStatus " +
    //                " From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID " +
    //                " Where b.CustomerFavShop IS NOT NULL AND a.OrderStatus IN (1, 3)"))
    //        {
    //            if (dtOrders.Rows.Count > 0)
    //            {
    //                foreach (DataRow row in dtOrders.Rows)
    //                {
    //                    int franchId = Convert.ToInt32(c.GetReqData("CustomersData", "CustomerFavShop", "CustomrtID=" + row["FK_OrderCustomerId"]));

    //                    if (row["OrderStatus"].ToString() == "1")
    //                    {
    //                        // route order direct to shop

    //                        // set order status to 3 -> as it is accepted by admin 
    //                        c.ExecuteQuery("Update OrdersData Set OrderStatus=3 Where OrderID=" + row["OrderID"]);

    //                        // insert entry in OrderAssign table, it is directly assigned to customers fav shop
    //                        c.ExecuteQuery("Update OrdersAssign Set OrdReAssign=1 Where FK_OrderID=" + row["OrderID"]);
    //                        int maxId = c.NextId("OrdersAssign", "OrdAssignID");
    //                        c.ExecuteQuery("Insert Into OrdersAssign (OrdAssignID, OrdAssignDate, FK_OrderID, Fk_FranchID, OrdAssignStatus, " +
    //                            " OrdReAssign) Values (" + maxId + ", '" + DateTime.Now + "', " + row["OrderID"] + ", " + franchId + ", 0, 0)");
    //                    }
    //                    else
    //                    {
    //                        if (!c.IsRecordExist("Select OrdAssignID From OrdersAssign Where FK_OrderID=" + row["OrderID"]))
    //                        {
    //                            // insert entry in OrderAssign table, it is directly assigned to customers fav shop
    //                            c.ExecuteQuery("Update OrdersAssign Set OrdReAssign=1 Where FK_OrderID=" + row["OrderID"]);
    //                            int maxId = c.NextId("OrdersAssign", "OrdAssignID");
    //                            c.ExecuteQuery("Insert Into OrdersAssign (OrdAssignID, OrdAssignDate, FK_OrderID, Fk_FranchID, OrdAssignStatus, " +
    //                                " OrdReAssign) Values (" + maxId + ", '" + DateTime.Now + "', " + row["OrderID"] + ", " + franchId + ", 0, 0)");
    //                        }
    //                    }
    //                }

    //                errMsg = c.ErrNotification(1, "Orders Routed Successfully..!!");
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errMsg = c.ErrNotification(3, ex.Message.ToString());
    //        return;
    //    }
    //}
}