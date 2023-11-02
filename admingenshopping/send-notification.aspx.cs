using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class admingenshopping_send_notification : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, images;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSubmit.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnSubmit, null) + ";");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            txtNotifTitle.Text = txtNotifTitle.Text.Trim().Replace("'", "");
            txtNotifMsg.Text = txtNotifMsg.Text.Trim().Replace("'", "");

            if (txtNotifTitle.Text == "" || txtNotifMsg.Text == "")
            {
                errMsg = c.ErrNotification(2, "All * Marked Fields are compulsory");
                return;
            }

            if (txtNotifMsg.Text.ToString().Length > 500)
            {
                errMsg = c.ErrNotification(2, "Notification message must be less than 500 characters");
                return;
            }

            string fileName = "";
            if (fuFile.HasFile)
            {
                string fExt = Path.GetExtension(fuFile.FileName).ToString().ToLower();
                if (fExt == ".jpg" || fExt == ".jpeg" || fExt == ".png")
                {
                    fileName = "notif-" + DateTime.Now.ToString("ddMMyyyyhhmmss") + fExt;
                    fuFile.SaveAs(Server.MapPath("~/upload/notifImg/") + fileName);
                }
                else
                {
                    errMsg = c.ErrNotification(2, "Only .jpg, .jpeg or .png files are allowed");
                    return;
                }
            }

            string custIds = "";
            using (DataTable dtCust = c.GetDataTable("Select CustomrtID, CustomerToken From CustomersData Where delMark=0 AND CustomerActive=1 AND CustomerToken IS NOT NULL AND CustomerToken<>''"))
            {
                if (dtCust.Rows.Count > 0)
                {
                    foreach (DataRow row in dtCust.Rows)
                    {
                        if (row["CustomerToken"].ToString().Length > 100)
                        {
                            if (custIds == "")
                            {
                                custIds = row["CustomrtID"].ToString();
                            }
                            else
                            {
                                custIds = custIds + "," + row["CustomrtID"].ToString();
                            }
                        }
                    }
                }
            }

            string[] arrcustomerId;
            if (custIds.Contains(','))
            {
                arrcustomerId = custIds.Split(',');
            }
            else
            {
                arrcustomerId = new string[] { custIds };
            }
            string notifImg = "";
            if (fuFile.HasFile)
            {
                notifImg = Master.rootPath + "upload/notifImg" + fileName;
            }
            else if (txtUrl.Text != "") 
            {
                notifImg = txtUrl.Text;
            }
            else
            {

            }

            int saveindb = chkDb.Checked == true ? 1 : 0;

            //c.SendPushNotification(custIds, txtNotifTitle.Text, txtNotifMsg.Text, notifImg);
            iThread it = new iThread();
            it.NotificationTrigger(txtNotifTitle.Text, txtNotifMsg.Text, notifImg, arrcustomerId, saveindb);

            txtNotifTitle.Text = txtNotifMsg.Text = "";

            errMsg = c.ErrNotification(1, "Notification Sent");
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}

