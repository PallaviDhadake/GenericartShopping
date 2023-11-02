using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Diagnostics;

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GetMACAddress();
        //GetMACAddress();
        lblMacId.Text = GetMACAddress();
    }

    public static string GetMACAddress()
    {
        string macAddress = string.Empty;

        try
        {
            // Get the IP address of the client's device
            string ipAddress = HttpContext.Current.Request.UserHostAddress;


            if (!string.IsNullOrEmpty(ipAddress))
            {
                // Retrieve the MAC address by pinging the client's IP address
                macAddress = GetMACAddressByPing(ipAddress);
                
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions as needed
            //Console.WriteLine($"Error retrieving MAC address: {ex.Message}");
        }

        return macAddress;
    }

    private static string GetMACAddressByPing(string ipAddress)
    {
        try
        {
            using (Process p = new Process())
            {
                ProcessStartInfo psi = new ProcessStartInfo("arp", "-a " + ipAddress)
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                p.StartInfo = psi;
                p.Start();

                string output = p.StandardOutput.ReadToEnd();
                string[] lines = output.Split('\n');

                if (lines.Length >= 4)
                {
                    // The MAC address is typically found in the 3rd line
                    string[] words = lines[3].Split(' ');

                    if (words.Length >= 3)
                    {
                        return words[1].Trim();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions as needed
            //Console.WriteLine($"Error retrieving MAC address by ping: {ex.Message}");
        }

        return string.Empty;
    }
}