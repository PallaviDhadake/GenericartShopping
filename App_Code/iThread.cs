using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Data;
/// <summary>
/// Summary description for iThread
/// </summary>
public class iThread
{

    public iThread()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public void NotificationTrigger(string notifTitle, string notifMsg, string notifImage, string[] custIds, int saveDb)
    {
        Thread t1 = new Thread(new ThreadStart(() => SendNotifications(notifTitle, notifMsg, notifImage, custIds, saveDb)));
        t1.Start();
    }
    private void SendNotifications(string nTitle, string nMsg, string nImg, string[] arrCustIds, int saveDbFlag)
    {
        iClass c = new iClass();

        foreach (string custId in arrCustIds)
        {
            if (saveDbFlag == 1)
            {
                int maxID = c.NextId("Notification", "Notifi_id");

                c.ExecuteQuery("Insert Into Notification (Notifi_id, Notifi_CustomrtID, Notifi_heading, Notifi_msg, Notifi_status, Notifi_img) " +
                    " Values (" + maxID + ", " + custId + ", '" + nTitle + "', '" + nMsg + "', 0, '" + nImg + "')");
            }

            c.SendPushNotification(custId, nTitle, nMsg, nImg);
            Thread.Sleep(1000);
        }
    }

    
}