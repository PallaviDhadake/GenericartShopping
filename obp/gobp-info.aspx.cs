using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class obp_gobp_info : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] obpData = new string[30];
    public string MyEarnings, totalCust, totalOrders, thismonearning, thismontcust, thismontorder; 
    protected void Page_Load(object sender, EventArgs e)
    {
        ShowOBPData();
    }


    private void ShowOBPData()
    {
       
        GobpInfo obpinfo = new GobpInfo();
        int ObpId = Convert.ToInt32(Request.QueryString["id"]);

        obpinfo.OBPData(ObpId);

        obpData[9] = obpinfo.OBPUserID;
        obpData[0] = obpinfo.ApplicantName;
        obpData[1] = obpinfo.MobileNo;
        obpData[2] = obpinfo.EmailId;
        obpData[3] = obpinfo.City;
        obpData[4] = obpinfo.OBPReferral.ToString();
        string DistrictHead = !string.IsNullOrEmpty(obpinfo.DistrictHeadName) ? obpinfo.DistrictHeadName : "NA";
        obpData[5] = DistrictHead.ToString();

        if (obpinfo.IsMLM == 1)
        {
            obpData[6] = "Yes";
        }
        else
        {
            obpData[6] = "No";
        }

        //obpData[6] = obpinfo.IsMLM.ToString();
        obpData[7] = obpinfo.JoinLevel.ToString();
        obpData[8] = obpinfo.OBPRefUserId;


        if (c.IsRecordExist("Select GOBPId From OrdersData where GOBPId="+ ObpId))
        {

            MyEarnings= c.returnAggregate("Select Sum(OBPComTotal) from OrdersData Where OrderStatus In(6,7) AND GOBPId=" + ObpId).ToString();
        }
        else
        {
            MyEarnings = "0";
        }

        if (c.IsRecordExist("Select GOBPId From OrdersData where GOBPId=" + ObpId))
        {

            thismonearning = c.returnAggregate("Select Sum(OBPComTotal) from OrdersData Where OrderStatus In(6,7) AND  MONTH(OrderDate) = MONTH(GETDATE()) AND YEAR(OrderDate) = YEAR(GETDATE()) AND GOBPId=" + ObpId).ToString();
        }
        else
        {
            thismonearning = "0";
        }


        if (c.IsRecordExist("Select FK_ObpID From CustomersData where FK_ObpID=" + ObpId))
        {

            totalCust = c.returnAggregate("Select count(CustomrtID) from CustomersData where FK_ObpID=" + ObpId).ToString();
        }
        else
        {
            totalCust = "0";
        }

        if (c.IsRecordExist("Select FK_ObpID From CustomersData where FK_ObpID=" + ObpId))
        {

            thismontcust = c.returnAggregate("Select Count(CustomrtID) from CustomersData where MONTH(CustomerJoinDate) = MONTH(GETDATE()) AND YEAR(CustomerJoinDate) = YEAR(GETDATE()) AND FK_ObpID=" + ObpId).ToString();
        }
        else
        {
            thismontcust = "0";
        }

        if (c.IsRecordExist("Select GOBPId From OrdersData where GOBPId=" + ObpId))
        {
            totalOrders = c.returnAggregate("Select Count(OrderID) From OrdersData Where OrderStatus In(6,7) AND GOBPId=" + ObpId).ToString();

        }
        else
        {
            totalOrders = "0";
        }
       

        if (c.IsRecordExist("Select GOBPId From OrdersData where GOBPId=" + ObpId))
        {

            thismontorder = c.returnAggregate("SELECT COUNT(OrderID) FROM OrdersData WHERE MONTH(OrderDate) = MONTH(GETDATE()) AND YEAR(OrderDate) = YEAR(GETDATE()) AND OrderStatus In (6,7) AND GOBPId=" + ObpId).ToString();
        }
        else
        {
            thismontorder = "0";
        }

        //thismonearning = c.returnAggregate("Select OBPComTotal from OrdersData Where OrderStatus In(6,7) AND MONTH(OrderDate) = MONTH(GETDATE()) AND YEAR(OrderDate) = YEAR(GETDATE()) AND GOBPId=" + ObpId).ToString();

        //object obpComTotalObj = c.GetReqData("OrdersData", "OBPComTotal", "OrderStatus In(6,7) AND MONTH(OrderDate) = MONTH(GETDATE()) AND YEAR(OrderDate) = YEAR(GETDATE()) AND GOBPId=" + ObpId);

        //if (obpComTotalObj != null)
        //{
        //     thismonearning = obpComTotalObj.ToString();
        //}
        //else
        //{
        //    thismonearning = "0";
        //}

        //object earning = c.GetReqData("OrdersData", "OBPComTotal", "OrderStatus In(6,7) AND GOBPId=" + ObpId).ToString();

        //if (earning != null)
        //{
        //    MyEarnings = earning.ToString();

        //}
        //else
        //{
        //    MyEarnings = "0";

        //}

        //MyEarnings = c.GetReqData("OrdersData", "OBPComTotal", "OrderStatus In(6,7) AND GOBPId=" + ObpId).ToString();

    }

}