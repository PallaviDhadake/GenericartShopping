using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Http;
using System.Xml;
using System.IO;
using System.Text;
using ServiceFinderQueryRef;
//using PickupRegistrationServiceRef;

public partial class franchisee_consume_api : System.Web.UI.Page
{
    public string errMsg, custaddr1, custaddr2, custaddr3;
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnAddress_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtAddress.Text.Length <= 30)
            {
                custaddr1 = txtAddress.Text.ToString();
            }
            else if (txtAddress.Text.Length > 30 && txtAddress.Text.Length <= 60)
            {
                custaddr1 = txtAddress.Text.ToString().Substring(0, 30);
                custaddr2 = txtAddress.Text.ToString().Substring(31);
            }
            if (txtAddress.Text.Length > 60 && txtAddress.Text.Length <= 90)
            {
                custaddr1 = txtAddress.Text.ToString().Substring(0, 30);
                custaddr2 = txtAddress.Text.ToString().Substring(30, Math.Min(txtAddress.Text.Length, 30));
                custaddr3 = txtAddress.Text.ToString().Substring(Math.Min(txtAddress.Text.Length, 60));
            }
            else
            {
                //custaddr1 = custaddr2 = custaddr3 = "addr";
                custaddr1 = txtAddress.Text.ToString().Substring(0, 30);
                custaddr2 = txtAddress.Text.ToString().Substring(30, Math.Min(txtAddress.Text.Length, 30));
                custaddr3 = txtAddress.Text.ToString().Substring(Math.Min(txtAddress.Text.Length, 60), Math.Min(txtAddress.Text.Length, 29));
            }

            errMsg = custaddr1.ToString() + "," + custaddr2.ToString() + "," + custaddr3.ToString();
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        ServiceFinderQueryClient service = new ServiceFinderQueryClient();
        ServiceCenterDetailsReference pincodeDetails = new ServiceCenterDetailsReference();

        UserProfile user = new UserProfile();
        user.LoginID = "SAG60897";
        user.LicenceKey = "hzfukpgpahfoqkplnehojpu6rjkmekfh";

        pincodeDetails = service.GetServicesforPincode("416416", user);

        string DP_IB_Service = pincodeDetails.DomesticPriorityInbound;
        string DP_OB_Service = pincodeDetails.DomesticPriorityOutbound;
        double DP_value_limit = pincodeDetails.DPDutsValueLimit;
        bool isError = pincodeDetails.IsError;

        //==================================================================================================//
        //Pickup Registration

        //PickupRegistrationClient client = new PickupRegistrationClient();
        //PickupRegistrationRequest regRequest = new PickupRegistrationRequest();
        //regRequest.AreaCode = "SAG";
        //regRequest.CISDDN = false;
        //regRequest.ContactPersonName = "Vinayak Butale";
        ////regRequest.CustomerAddress1 = "Sangli";
        //regRequest.CustomerCode = "060664";
        //regRequest.CustomerName = "Vinayak Butale";
        //regRequest.CustomerPincode = "416416";
        //regRequest.CustomerTelephoneNumber = "9922335716";
        //regRequest.DoxNDox = "1";
        //regRequest.EmailID = "butale_vinayak@hotmail.com";
        //regRequest.IsForcePickup = true;
        //regRequest.IsReversePickup = false;
        //regRequest.MobileTelNo = "9922335716";
        //regRequest.NumberofPieces = 1;
        //regRequest.OfficeCloseTime = "16:00";
        //regRequest.ProductCode = "A";
        //regRequest.RouteCode = "99";
        //DateTime pickupDate = new DateTime(2021, 08, 27);
        //regRequest.ShipmentPickupDate = pickupDate;
        //regRequest.ShipmentPickupTime = "15:00";
        //string[] arrSubProd = new string[1] { "E-Tailing" };
        //regRequest.SubProducts = arrSubProd;
        //regRequest.VolumeWeight = 1.2;
        //regRequest.WeightofShipment = 1.2;
        //regRequest.isToPayShipper = false;

        
        

        //UserProfile user = new UserProfile();
        //user.LoginID = "SAG60897";
        //user.LicenceKey = "rr5sesemke7nijinfusporovnnxgsjlq"; //production
        //user.Api_type = "S";
        //user.Area = "SAG";
        //user.Customercode = "060664";
        //user.Version = "1.9";

        //PickupRegistrationResponse pickupResponse = client.RegisterPickup(regRequest, user);

        //bool isError = pickupResponse.IsError;

        if (isError)
        {
            errMsg = "Error Occoured while submitting request";
            return;
        }
        else
        {
            errMsg = "Request Submitted Successfully";
        }

    }
}