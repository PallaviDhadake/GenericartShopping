using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

using System.Net;
using System.Net.Mail;
using System.Web.Services;
using System.Globalization;
using System.Threading;

using System.Device.Location;
using System.Configuration; 
/// <summary>
/// Summary description for iClass
/// </summary>
public class iClass : IDisposable
{
    public iClass()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    void IDisposable.Dispose()
    {

    }
    /// <summary>
    /// Opens connection to database
    /// </summary>
    /// <returns>connection string</returns>
    public string OpenConnection()
    {
        return System.Web.Configuration.WebConfigurationManager.ConnectionStrings["GenCartDATA"].ConnectionString;
    }

    /// <summary>
    /// Executes a SQL Query
    /// </summary>
    /// <param name="strQuery">SQL Query as String</param>
    public void ExecuteQuery(string strQuery)
    {
        try
        {
            SqlConnection con = new SqlConnection(OpenConnection());
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strQuery;
            cmd.CommandTimeout = 3000000; // seconds
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
            con = null;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    /// <summary>
    /// Used to check if record exists or not
    /// </summary>
    /// <param name="strQuery">SQL Query as string</param>
    /// <returns>True/False</returns>
    public bool IsRecordExist(string strQuery)
    {
        try
        {

            bool rValue = false;
            SqlConnection con = new SqlConnection(OpenConnection());
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = default(SqlDataReader);

            cmd.CommandText = strQuery;
            dr = cmd.ExecuteReader();

            rValue = dr.HasRows;
            dr.Close();
            cmd.Dispose();
            con.Close();
            con = null;

            return rValue;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    /// <summary>
    /// Gets next Id in column(Integer)
    /// </summary>
    /// <param name="tableName">Name of Table in Databse</param>
    /// <param name="fieldName">Name of Column</param>
    /// <returns>Next value in column</returns>
    public int NextId(string tableName, string fieldName)
    {
        try
        {
            int retValue = 1;
            SqlConnection con = new SqlConnection(OpenConnection());
            con.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr = default(SqlDataReader);
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select MAX(" + fieldName + ") as maxNo From " + tableName;
            dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if ((dr["maxNo"]) != System.DBNull.Value)
                    {
                        retValue = Convert.ToInt32(dr["maxNo"]) + 1;
                    }
                    else
                    {
                        retValue = 1;
                    }
                }
            }
            else
            {
                retValue = 1;
            }
            dr.Close();
            cmd.Dispose();
            con.Close();
            con = null;
            return retValue;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Used to get single value from databse table
    /// </summary>
    /// <param name="tableName">Name of Database Table</param>
    /// <param name="fieldName">Naem of Column</param>
    /// <param name="whereCon">Where Condition</param>
    /// <returns>Value as object</returns>
    public object GetReqData(string tableName, string fieldName, string whereCon)
    {
        try
        {
            object retValue = null;
            SqlConnection con = new SqlConnection(OpenConnection());
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = default(SqlDataReader);
            cmd.CommandText = whereCon == "" ? "Select " + fieldName + " as colName From " + tableName : "Select " + fieldName + " as colName From " + tableName + " Where " + whereCon;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr["colName"] == DBNull.Value)
                {
                    retValue = null;
                }
                else
                {
                    retValue = dr["colName"];
                }

            }
            dr.Close();
            cmd.Dispose();
            con.Close();
            con = null;
            return retValue;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Used to display error Messagge on Client Side
    /// </summary>
    /// <param name="errType">Error Type as byte 1 for Succes/2 for Warning/3 for Error</param>
    /// <param name="errMsgStr">Message to display</param>
    /// <returns>Markup as string</returns>
    public string ErrNotification(byte errType, string errMsgStr)
    {
        try
        {
            string rValue = "";

            switch (errType)
            {
                case 1:
                    rValue = "<span class='success'><b>Success:</b> " + errMsgStr + "</span>";
                    break;
                case 2:
                    rValue = "<span class='warning'><b>Warning:</b> " + errMsgStr + "</span>";
                    break;
                case 3:
                    rValue = "<span class='error'><b>Error:</b> " + errMsgStr + "</span>";
                    break;
            }

            return rValue;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Checks if input Email Id is in standard email format
    /// </summary>
    /// <param name="emailAddress">input Email</param>
    /// <returns>Status(True/False)</returns>
    public bool EmailAddressCheck(string emailAddress)
    {
        try
        {
            bool rValue = false;
            string pattern = "^[a-zA-Z][\\w\\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\\w\\.-]*[a-zA-Z0-9]\\.[a-zA-Z][a-zA-Z\\.]*[a-zA-Z]$";
            Match emailAddressMatch = Regex.Match(emailAddress, pattern);


            if (emailAddressMatch.Success)
            {
                rValue = true;
            }
            else
            {
                rValue = false;
            }

            return rValue;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



    /// <summary>
    /// Returns Datatable using String query
    /// </summary>
    /// <param name="strQuery">Query</param>
    /// <returns>Datatable</returns>
    public DataTable GetDataTable(string strQuery)
    {
        try
        {
            SqlConnection con = new SqlConnection(OpenConnection());

            SqlDataAdapter da = new SqlDataAdapter(strQuery, con);
            DataTable dt = new DataTable();
            da.SelectCommand.CommandTimeout = 3000000;  // seconds
            da.Fill(dt);

            con.Close();
            con = null;

            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    /// <summary>
    /// Route Path of Project
    /// </summary>
    /// <param name="reqType">Type(0-Routepath/1-CSS/2-Javascript)</param>
    /// <returns>Domain Path(LocalHost/Online)</returns>
    public string ReturnHttp()
    {
        try
        {
            string rValue = null;

           string domain = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
           //string domain = "https://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
            string localFolder;

            if (domain.Contains("localhost") == true)
            {
                localFolder = "/";
                //localFolder = "/GenericartShopping/";
            }
            else
            {
                localFolder = "/";
            }

            rValue = domain + localFolder;

            return rValue;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }




    /// <summary>
    /// Creates SEO friendly url (Removes special characters from url)
    /// </summary>
    /// <param name="oldUrl">Current Url</param>
    /// <returns>Modified Url</returns>
    public string UrlGenerator(string oldUrl)
    {
        try
        {
            if (oldUrl.Contains("$") == true)
            {
                oldUrl = oldUrl.Replace("$", "");
            }
            if (oldUrl.Contains(" ") == true)
            {
                oldUrl = oldUrl.Replace(" ", "-");
            }
            if (oldUrl.Contains("+") == true)
            {
                oldUrl = oldUrl.Replace("+", "");
            }
            if (oldUrl.Contains(";") == true)
            {
                oldUrl = oldUrl.Replace(";", "");
            }
            if (oldUrl.Contains("=") == true)
            {
                oldUrl = oldUrl.Replace("=", "");
            }
            if (oldUrl.Contains("?") == true)
            {
                oldUrl = oldUrl.Replace("?", "");
            }
            if (oldUrl.Contains("@") == true)
            {
                oldUrl = oldUrl.Replace("@", "");
            }
            if (oldUrl.Contains("<") == true)
            {
                oldUrl = oldUrl.Replace("<", "");
            }
            if (oldUrl.Contains('"') == true)
            {
                oldUrl = oldUrl.Replace('"', '-');
            }
            if (oldUrl.Contains("'") == true)
            {
                oldUrl = oldUrl.Replace("'", "-");
            }
            if (oldUrl.Contains(">") == true)
            {
                oldUrl = oldUrl.Replace(">", "");
            }
            if (oldUrl.Contains("#") == true)
            {
                oldUrl = oldUrl.Replace("#", "");
            }
            if (oldUrl.Contains("{") == true)
            {
                oldUrl = oldUrl.Replace("{", "");
            }
            if (oldUrl.Contains("}") == true)
            {
                oldUrl = oldUrl.Replace("}", "");
            }
            if (oldUrl.Contains("|") == true)
            {
                oldUrl = oldUrl.Replace("|", "");
            }
            if (oldUrl.Contains("\\") == true)
            {
                oldUrl = oldUrl.Replace("\\", "");
            }
            if (oldUrl.Contains("^") == true)
            {
                oldUrl = oldUrl.Replace("^", "");
            }
            if (oldUrl.Contains("~") == true)
            {
                oldUrl = oldUrl.Replace("~", "");
            }
            if (oldUrl.Contains("[") == true)
            {
                oldUrl = oldUrl.Replace("[", "");
            }
            if (oldUrl.Contains("]") == true)
            {
                oldUrl = oldUrl.Replace("]", "");
            }
            if (oldUrl.Contains("`") == true)
            {
                oldUrl = oldUrl.Replace("`", "");
            }
            if (oldUrl.Contains("%") == true)
            {
                oldUrl = oldUrl.Replace("%", "percent");
            }
            if (oldUrl.Contains("&") == true)
            {
                oldUrl = oldUrl.Replace("&", "and");
            }
            if (oldUrl.Contains(":") == true)
            {
                oldUrl = oldUrl.Replace(":", "");
            }
            if (oldUrl.Contains(".") == true)
            {
                oldUrl = oldUrl.Replace(".", "-");
            }
            if (oldUrl.Contains(",") == true)
            {
                oldUrl = oldUrl.Replace(",", "-");
            }
            if (oldUrl.Contains("/") == true)
            {
                oldUrl = oldUrl.Replace("/", "");
            }

            if (oldUrl.Contains("(") == true)
            {
                oldUrl = oldUrl.Replace("(", "");
            }
            if (oldUrl.Contains(")") == true)
            {
                oldUrl = oldUrl.Replace(")", "");
            }
            if (oldUrl.Contains("--") == true)
            {
                oldUrl = oldUrl.Replace("--", "-");
            }
            if (oldUrl.EndsWith("-") == true)
            {
                oldUrl = oldUrl.Substring(0, oldUrl.Length - 1);
            }

            return oldUrl.ToLower();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    /// <summary>
    /// date Validation
    /// </summary>
    /// <param name="date">Date</param>
    /// <returns>True / False</returns>
    public bool IsDate(string date)
    {
        try
        {
            DateTime dt = DateTime.Parse(date);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Function validates Mobile number
    /// </summary>
    /// <param name="mobileNo">mobile number</param>
    /// <returns>true/false</returns>
    public bool ValidateMobile(string mobileNo)
    {
        if (!IsNumeric(mobileNo))
        {
            return false;
        }

        if (mobileNo.Length != 10)
        {
            return false;
        }
        char[] mobDigits = mobileNo.ToCharArray();

        if (mobDigits[0] == '0')
        {
            return false;
        }
        if (mobDigits[0] != '9' && mobDigits[0] != '8' && mobDigits[0] != '7' && mobDigits[0] != '6')
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Used to fill DropDownList
    /// </summary>
    /// <param name="fieldStr">Display Contents(Column Name)</param>
    /// <param name="fieldVal">Value Content(Column Name)</param>
    /// <param name="tableName">Database Table Name</param>
    /// <param name="whereCond">Specific Condition (SQL Where Condition)</param>
    /// <param name="myComboBox">Name of DropDownList</param>
    public void FillComboBox(string fieldStr, string fieldVal, string tableName, string whereCond, string sortColumn, int ordType, DropDownList myComboBox)
    {
        try
        {
            SqlConnection con = new SqlConnection(OpenConnection());
            string strSql = "";
            if (whereCond == "")
            {
                if (sortColumn == "")
                {
                    strSql = "SELECT " + fieldStr + ", " + fieldVal + " From " + tableName;
                }
                else
                {
                    if (ordType == 0)
                    {
                        strSql = "SELECT " + fieldStr + ", " + fieldVal + " From " + tableName + " Order By " + sortColumn;
                    }
                    else
                    {
                        strSql = "SELECT " + fieldStr + ", " + fieldVal + " From " + tableName + " Order By " + sortColumn + " Desc";
                    }
                }
            }
            else
            {
                if (sortColumn == "")
                {
                    strSql = "SELECT " + fieldStr + ", " + fieldVal + " From " + tableName + " Where " + whereCond;
                }
                else
                {
                    if (ordType == 0)
                    {
                        strSql = "SELECT " + fieldStr + ", " + fieldVal + " From " + tableName + " Where " + whereCond + " Order By " + sortColumn;
                    }
                    else
                    {
                        strSql = "SELECT " + fieldStr + ", " + fieldVal + " From " + tableName + " Where " + whereCond + " Order By " + sortColumn + " Desc";
                    }
                }

            }
            SqlDataAdapter da = new SqlDataAdapter(strSql, con);
            DataSet ds = new DataSet();
            da.Fill(ds, "myCombo");
            myComboBox.DataSource = ds.Tables[0];
            myComboBox.DataTextField = ds.Tables[0].Columns[fieldStr].ColumnName.ToString();
            myComboBox.DataValueField = ds.Tables[0].Columns[fieldVal].ColumnName.ToString();
            myComboBox.DataBind();

            myComboBox.Items.Insert(0, "<-Select->");
            myComboBox.Items[0].Value = "0";
            //myComboBox.BackColor = Color.Violet;

            con.Close();
            con = null;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Optimizes the uploaded image
    /// </summary>
    /// <param name="imgName">Image Name</param>
    /// <param name="srcPath">Path of source file</param>
    /// <param name="destPath">Path where u want to save the optimized file</param>
    /// <param name="widthLimit">Target width limit</param>
    /// <param name="imageProportion">Either to maintain proportion of Width and height</param>
    public void ImageOptimizer(string imgName, string srcPath, string destPath, float widthLimit, bool imageProportion)
    {
        try
        {
            string src = HttpContext.Current.Server.MapPath(srcPath + imgName);
            FileStream fs = new FileStream(src, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            System.Drawing.Image image = System.Drawing.Image.FromStream(fs, true, false);

            float srcWidth = image.Width;
            float srcHeight = image.Height;
            image.Dispose();
            fs.Close();

            float percentWidth = 0;
            float percentHeight = 0;
            float thumbWidth = widthLimit;
            float thumbHeight;

            if (imageProportion == true)
            {
                if (srcWidth >= srcHeight)
                {
                    thumbHeight = 0;
                }
                else
                {
                    thumbWidth = 0;
                    thumbHeight = widthLimit;
                    goto heightProcess;
                }
            }


            if (srcWidth > widthLimit)
            {
                //float tvar1 = (100 * widthLimit) / srcWidth;
                //percentWidth = Convert.ToInt32( Math.Round(tvar1));
                ////percentWidth = Convert.ToInt32( Math.Round(percentWidth));
                //thumbWidth = (srcWidth * percentWidth) / 100;

                //New
                percentWidth = (100 * widthLimit) / srcWidth;
                thumbWidth = (srcWidth * percentWidth) / 100;



                //float tvar2 = (thumbWidth * srcHeight) / srcWidth;
                //thumbHeight = Convert.ToInt32(Math.Round(tvar2));
                thumbHeight = (thumbWidth * srcHeight) / srcWidth;

                //thumbprocess
                ThumbnailProcessor(imgName, srcPath, destPath, Convert.ToInt32(thumbWidth), Convert.ToInt32(thumbHeight));
            }
            else
            {

                thumbWidth = srcWidth;
                thumbHeight = srcHeight;
                //thumbprocess
                ThumbnailProcessor(imgName, srcPath, destPath, Convert.ToInt32(thumbWidth), Convert.ToInt32(thumbHeight));
            }
            return;



        heightProcess:
            if (srcHeight > widthLimit)
            {
                //float tvar3 = (100 * widthLimit) / srcHeight;
                //percentHeight = Convert.ToInt32( Math.Round( tvar3 ));
                percentHeight = (100 * widthLimit) / srcHeight;

                thumbHeight = (srcHeight * percentHeight) / 100;

                //float tvar4 = (thumbHeight * srcWidth) / srcHeight;
                //thumbWidth = Convert.ToInt32( Math.Round(tvar4) );

                thumbWidth = (thumbHeight * srcWidth) / srcHeight;

                //thumbnailprocessor
                ThumbnailProcessor(imgName, srcPath, destPath, Convert.ToInt32(thumbWidth), Convert.ToInt32(thumbHeight));
            }
            else
            {
                thumbWidth = srcWidth;
                thumbHeight = srcHeight;

                //thumbnailprocessor
                ThumbnailProcessor(imgName, srcPath, destPath, Convert.ToInt32(thumbWidth), Convert.ToInt32(thumbHeight));
            }
            return;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void ThumbnailProcessor(string imgName, string testPath, string deployImgPath, int thumbWidth, int thumbHeight)
    {
        try
        {
            //string sImageName = imgName;

            // ORIGINAL WIDTH AND HEIGHT.
            //Bitmap bitmap = new Bitmap(Server.MapPath("~/" + Path.GetFileName(hpf.FileName)));
            Bitmap bitmap = new Bitmap(HttpContext.Current.Server.MapPath(testPath + imgName));
            string extn = Path.GetExtension(testPath + imgName);

            int iwidth = thumbWidth;
            int iheight = thumbHeight;

            bitmap.Dispose();

            // CREATE AN IMAGE OBJECT USING ORIGINAL WIDTH AND HEIGHT.
            // ALSO DEFINE A PIXEL FORMAT (FOR RICH RGB COLOR).

            //Format16bppRgb555 - previous format
            System.Drawing.Image objOptImage = new System.Drawing.Bitmap(iwidth, iheight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // code for transparent backround image (12/12/2018)
            using (var g = Graphics.FromImage(objOptImage))
            {
                g.Clear(Color.Transparent);
                //g.DrawLine(Pens.Red, 0, 0, 135, 135);
            }

            // GET THE ORIGINAL IMAGE.
            using (System.Drawing.Image objImg = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(testPath + imgName)))
            {

                // RE-DRAW THE IMAGE USING THE NEWLY OBTAINED PIXEL FORMAT.
                using (System.Drawing.Graphics oGraphic = System.Drawing.Graphics.FromImage(objOptImage))
                {
                    var _1 = oGraphic;
                    System.Drawing.Rectangle oRectangle = new System.Drawing.Rectangle(0, 0, iwidth, iheight);

                    _1.DrawImage(objImg, oRectangle);
                }

                // SAVE THE OPTIMIZED IMAGE.
                switch (extn.ToLower())
                {
                    case ".jpg":

                        objOptImage.Save(HttpContext.Current.Server.MapPath(deployImgPath + imgName), System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case ".jpeg":
                        objOptImage.Save(HttpContext.Current.Server.MapPath(deployImgPath + imgName), System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case ".png":
                        objOptImage.Save(HttpContext.Current.Server.MapPath(deployImgPath + imgName), System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case ".gif":
                        objOptImage.Save(HttpContext.Current.Server.MapPath(deployImgPath + imgName), System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }

                objImg.Dispose();

            }

            objOptImage.Dispose();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Crops the image & maintains the center
    /// </summary>
    /// <param name="imageName">Name of Image</param>
    /// <param name="srcPath">Source path of target image</param>
    /// <param name="destPath">Path where you want to save the cropeed image</param>
    /// <param name="targetW">Target width</param>
    /// <param name="targetH">Target height</param>
    public void CenterCropImage(string imageName, string srcPath, string destPath, int targetW, int targetH)
    {
        try
        {
            string src = HttpContext.Current.Server.MapPath(srcPath + imageName);
            string dest = HttpContext.Current.Server.MapPath(destPath + imageName);

            FileStream fsCrop = new FileStream(src, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            System.Drawing.Image imgPhoto = System.Drawing.Image.FromStream(fsCrop, true, false);

            int targetX = (imgPhoto.Width - targetW) / 2;
            int targetY = (imgPhoto.Height - targetH) / 2;

            System.Drawing.Bitmap bmPhoto = new Bitmap(targetW, targetH, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(72, 72);

            System.Drawing.Graphics grPhoto = System.Drawing.Graphics.FromImage(bmPhoto);

            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

            grPhoto.Clear(System.Drawing.Color.FromArgb(255, 255, 255, 255));
            grPhoto.DrawImage(imgPhoto, new System.Drawing.Rectangle(0, 0, targetW, targetH), targetX, targetY, targetW, targetH, System.Drawing.GraphicsUnit.Pixel);

            EncoderParameters ep = new EncoderParameters(1);
            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Convert.ToInt64(100));

            imgPhoto.Dispose();
            grPhoto.Dispose();
            grPhoto = null;
            bmPhoto.Save(dest);
            bmPhoto.Dispose();
            bmPhoto = null;

            fsCrop.Dispose();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public string EncryptData(string originalStr)
    {
        try
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[originalStr.Length];
            encode = Encoding.UTF8.GetBytes(originalStr);
            strmsg = Convert.ToBase64String(encode);
            strmsg = strmsg.Replace("=", "");
            return strmsg;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Send SMS
    /// </summary>
    /// <param name="msgData">SMS Data</param>
    /// <param name="recNumber">Receipant Number</param>
    public void SendSMS(string msgData, string recNumber)
    {
        try
        {
            WebClient wbClient = new WebClient();
            
            //string baseUrl = "http://msg.msgclub.net/rest/services/sendSMS/sendGroupSms?AUTH_KEY=f998309b66f2cbd5a7dad69fdbf1a477&message=" + msgData + "&senderId=INTSYS&routeId=1&mobileNos=" + recNumber + "&smsContentType=english";
            //string baseUrl = "https://www.smsidea.co.in/smsstatuswithid.aspx?mobile=8275583387&pass=GMPL@app&senderid=GENERI&to=" + recNumber + "&msg=" + msgData + "";
            //GENRIC
            string baseUrl = "https://www.smsidea.co.in/smsstatuswithid.aspx?mobile=8275583387&pass=GMPL@app&senderid=GENRIC&to=" + recNumber + "&msg=" + msgData + "";
            Stream dStream = wbClient.OpenRead(baseUrl);
            StreamReader strReader = new StreamReader(dStream);
            string s = strReader.ReadToEnd();

            dStream.Close();
            strReader.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Sends an Email
    /// </summary>
    /// <param name="fromEmail">Sender Email Id</param>
    /// <param name="toEmail">Receipant Email Id</param>
    /// <param name="msgData">Message Details</param>
    /// <param name="mailSubject">Subject Line</param>
    /// <param name="bccEmail">BCC Email Id</param>
    /// <param name="isHtmlEmail">True/False</param>
    /// <param name="attachStr">path of attachment file</param>
    /// <param name="documentName">Name of document</param>
    public void SendMail(string fromEmail, string MailerName, string toEmail, string msgData, string mailSubject, string bccEmail, bool isHtmlEmail, string attachStr, string documentName)
    {
        try
        {
            MailMessage msg = new MailMessage();
            string totalMessage;
            msg.From = new MailAddress(fromEmail, MailerName);
            msg.To.Add(toEmail);
            msg.Subject = mailSubject;

            totalMessage = msgData + Environment.NewLine + Environment.NewLine;

            if (bccEmail != "")
            {
                msg.Bcc.Add(bccEmail);
            }

            msg.Body = (totalMessage.ToString()).Trim();
            msg.IsBodyHtml = isHtmlEmail;

            if (attachStr != "")
            {
                FileStream fs = new FileStream(attachStr, FileMode.Open, FileAccess.Read);
                Attachment file = new Attachment(fs, documentName);
                msg.Attachments.Add(file);
            }
            SmtpClient smtp = new SmtpClient("smtp.zoho.in", 587);

            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("info@genericart.in", "itDept@zoho20");

            smtp.Send(msg);
            msg = null;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    /// <summary>
    /// Creates Email Markup //here used for JobNiti.com
    /// </summary>
    /// <param name="fromEmail">Sender Email Id</param>
    /// <param name="toEmail">Receipant Email Id</param>
    /// <param name="subjectLine">Subject Line</param>
    /// <param name="subjectBrief">Short Details</param>
    /// <param name="msgData">Detail Message</param>
    /// <param name="bccEMail">BCC Email Id</param>
    public void EmailHttpMarkup(string fromEmail, string mailerName, string toEmail, string subjectLine, string subjectBrief, string msgData, string bccEMail, string attchStr, string docName)
    {
        try
        {
            StringBuilder strMail = new StringBuilder();

            strMail.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            strMail.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">");

            //Head tag starts
            strMail.Append("<head>");
            strMail.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            strMail.Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"/>");
            strMail.Append("</head>");
            //Head tag Ends

            //Body tag starts
            strMail.Append("<body style=\"margin:0;padding:0;\">");

            //Main table wrapper starts
            strMail.Append("<table style=\"background-color:#f4f4f4 \" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
            strMail.Append("<tr>");
            strMail.Append("<td style=\"padding:10px 0 30px 0px\">");

            //Container Table wrapper starts
            strMail.Append("<table border=\"0\" width=\"600\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" style=\"border-collapse:collapse\">");

            //Header starts
            strMail.Append("<tr>");
            strMail.Append("<td style=\"background:#73BF45;padding:20px 0px 20px 20px;\">");

            strMail.Append("<table style=\"width:100%\">");
            strMail.Append("<tr>");
            strMail.Append("<td align=\"left\" style=\"width:50%\">");
            string rootPath = ReturnHttp();
            //strMail.Append("<img src=\"" + rootPath + "images/natural-wood-logo.png\" alt=\"Naturalwoods\" width=\"160\" style=\"display:block;\" />");
            strMail.Append("<span style=\"display:block; color:#fff; font-size:1.8em; font-weight:600; letter-spacing:2px; text-transform:uppercase;\">Naturalwoods</span>");
            //strMail.Append("<img src=\"http://demo.intellect-systems.com/images/Nandadeep-logo.png\" alt=\"Nandadeep Eye Hospital\" width=\"150\" style=\"display:block;\" />");
            strMail.Append("</td>");
            strMail.Append("<td align=\"right\" style=\"width:50%;\">");

            strMail.Append("<a href=\"https://www.facebook.com/profile.php?id=100009522314111\" style=\"margin-right:10px;\" ><img src=\"http://www.naturalwoods.co.in/images/icons/topFb.png\" title=\"Naturalwoods on Facebook\" /></a>");
            strMail.Append("<a href=\"#\" style=\"margin-right:10px;\" ><img src=\"http://www.naturalwoods.co.in/images/icons/topInsta.png\" title=\"Naturalwoods on Instagram\" /></a>");
            
            //strMail.Append("<a href=\"https://www.facebook.com/nandadeepeyehospital/\" style=\"margin-right:10px;\" ><img src=\"http://demo.intellect-systems.com/images/icons/fb_mail.png\" title=\"Nandadeep Hospital on Facebook\" /></a>");
            //strMail.Append("<a href=\"https://www.youtube.com/user/sourabh9000\" style=\"margin-right:10px;\" ><img src=\"http://demo.intellect-systems.com/images/icons/yt_mail.png\" title=\"Nandadeep Hospital on Youtube\" /></a>");

            strMail.Append("</td>");
            strMail.Append("</tr>");
            strMail.Append("</table>");

            strMail.Append("</td>");
            strMail.Append("</tr>");
            //Header ends

            //Email message data starts
            strMail.Append("<tr>");
            strMail.Append("<td bgcolor=\"#ffffff\" style=\"padding:40px 30px 40px 30px; background-color:#ffffff\">");

            //4 rows table starts
            strMail.Append("<table border=\"0\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">");

            //Subject Title starts 
            strMail.Append("<tr>");
            strMail.Append("<td style=\"color:#222;font-family:Arial,Sans-Serif;font-size:20px;padding-bottom:5px\">");

            strMail.Append(subjectLine);

            strMail.Append("</td>");
            strMail.Append("</tr>");
            //Subject Title ends

            //Subject subtitle starts
            strMail.Append("<tr>");
            strMail.Append("<td style=\"color:#555;font-size:14px;font-family:Arial,Sans-Serif;\">");

            strMail.Append(subjectBrief);

            strMail.Append("</td>");
            strMail.Append("</tr>");
            //Subject subtitle ends

            //Separator line
            strMail.Append("<tr><td style=\"height:15px\"></td></tr>");
            strMail.Append("<tr><td style=\"background-color:#ececec;height:1px\"></td></tr>");
            strMail.Append("<tr><td style=\"height:15px\"></td></tr>");

            //Actual Template Message from Database

            strMail.Append("<tr>");
            strMail.Append("<td style=\"color:#555;font-size:14px; line-height:1.5; font-family:Arial,Sans-Serif;\">");

            strMail.Append(msgData);

            strMail.Append("</td>");
            strMail.Append("</tr>");
            strMail.Append("</table>");
            //4 rows table ends

            strMail.Append("</td>");
            strMail.Append("</tr>");
            //Email Message data ends

            //Footer starts
            strMail.Append("<tr>");
            strMail.Append("<td bgcolor=\"#181818\" style=\"text-align:center;padding:20px 20px 20px 20px\">");
            strMail.Append("<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");

            strMail.Append("<tr>");
            strMail.Append("<td style=\"padding:0px 0px 10px 0px;font-family:Arial,Sans-Serif;font-size:13px;color:#a1a0a0\">For any queries, complains or suggestions, please feel free to contact us at <a href=\"mailto:enquiry@naturalwoods.co.in\" style=\"font-family:Arial,Sans-Serif;font-size:13px;color:#73BF45\">enquiry@naturalwoods.co.in</a></td>");
            strMail.Append("</tr>");
            strMail.Append("<tr>");
            strMail.Append("<td style=\"font-family:Arial,Sans-Serif;font-size:14px;color:#a1a0a0;padding-bottom:10px\">Naturalwoods, Sangli, Maharashtra.</td>");
            strMail.Append("</tr>");
            strMail.Append("</table>");
            strMail.Append("</td>");
            strMail.Append("</tr>");
            //Footer ends

            strMail.Append("</table>");
            //Container Table wrapper ends

            strMail.Append("</td>");
            strMail.Append("</tr>");
            strMail.Append("</table>");
            //Main Table wrapper ends

            strMail.Append("</body>");
            //Body tag ends

            strMail.Append("</html>");


            string msgData1 = strMail.ToString();

            SendMail(fromEmail, mailerName, toEmail, msgData1, subjectLine, bccEMail, true, attchStr, docName);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Timespan between specific date and current date
    /// </summary>
    /// <param name="startDate">Start Date</param>
    /// <returns>Timespan (String)</returns>
    public string GetTimeSpan(DateTime startDate)
    {
        try
        {

            startDate = Convert.ToDateTime(startDate);

            DateTime curDate = Convert.ToDateTime(DateTime.Now);

            //TimeSpan timeSince = new TimeSpan();
            TimeSpan timeSince = curDate.Subtract(startDate);

            if (timeSince.TotalMinutes < 1)
            {
                return "Just now";
            }

            if (timeSince.TotalMinutes < 2)
            {
                return "1 minute ago";
            }
            if (timeSince.TotalMinutes < 60)
            {
                return string.Format("{0} minutes ago", timeSince.Minutes);
            }
            if (timeSince.TotalMinutes < 120)
            {
                return "1 hour ago";
            }
            if (timeSince.TotalHours < 24)
            {
                return string.Format("{0} hours ago", timeSince.Hours);
            }
            //if (timeSince.TotalDays == 1)
            //{
            //    return "Yesterday";
            //}
            if (timeSince.TotalDays < 7)
            {
                return string.Format("{0} days ago", timeSince.Days);
            }
            if (timeSince.TotalDays < 14)
            {
                return "1 week ago";
            }
            if (timeSince.TotalDays < 21)
            {
                return "2 weeks ago";
            }
            if (timeSince.TotalDays < 28)
            {
                return "3 weeks ago";
            }
            if (timeSince.TotalDays < 60)
            {
                return "1 month ago";
            }
            if (timeSince.TotalDays < 365)
            {
                return String.Format("{0} months ago", Math.Round(timeSince.TotalDays / 30));
            }
            if (timeSince.TotalDays < 730)
            {
                return "1 year ago";
            }
            return string.Format("{0} years ago", Math.Round(timeSince.TotalDays / 365));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //private void IsNumeric(
    //public static bool IsNumeric(this string s)
    //{
    //    float output;
    //    return float.TryParse(s, out output);
    //}

    public bool IsNumeric(String num)
    {
        try
        {

            bool rValue;
            double result;
            bool isNum = double.TryParse(num, out result);
            if (isNum)
            {
                rValue = true;
            }
            else
            {
                rValue = false;
            }
            return rValue;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }




   









    ///// <summary>
    ///// Used to check if record exists or not
    ///// </summary>
    ///// <param name="strQuery">SQL Query as string</param>
    ///// <returns>True/False</returns>
    //public bool IsRecordExist(string strQuery)
    //{
    //    try
    //    {

    //        bool rValue = false;
    //        SqlConnection con = new SqlConnection(OpenConnection());
    //        con.Open();

    //        SqlCommand cmd = new SqlCommand();
    //        cmd.Connection = con;
    //        cmd.CommandType = CommandType.Text;
    //        SqlDataReader dr = default(SqlDataReader);

    //        cmd.CommandText = strQuery;
    //        dr = cmd.ExecuteReader();

    //        rValue = dr.HasRows;
    //        dr.Close();
    //        cmd.Dispose();
    //        con.Close();
    //        con = null;

    //        return rValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }

    //}
    /// <summary>
    /// Used to get aggregate value of single column
    /// </summary>
    /// <param name="strQuery">SQL Query as string</param>
    /// <returns>value</returns>
    public long returnAggregate(string strQuery)
    {
        try
        {
            long rValue = 0;
            SqlConnection con = new SqlConnection(OpenConnection());
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strQuery;

            object result = cmd.ExecuteScalar();

            if (result.GetType() != typeof(DBNull))
            {
                rValue = Convert.ToInt32(result);
            }
            else
            {
                rValue = 0;

            }

            con.Close();
            con = null;
            cmd.Dispose();
            return rValue;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public double returnAggregateNew(string strQuery)
    {
        try
        {
            double rValue = 0;
            SqlConnection con = new SqlConnection(OpenConnection());
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strQuery;

            object result = cmd.ExecuteScalar();

            if (result.GetType() != typeof(DBNull))
            {
                rValue = Convert.ToDouble(result);
            }
            else
            {
                rValue = 0;

            }

            con.Close();
            con = null;
            cmd.Dispose();
            return rValue;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void ErrorLogHandler(string pageName, string funcName, string errMsg)
    {
        string rootPath = ReturnHttp();

        string filePath = HttpContext.Current.Server.MapPath("~/error_log.txt");
        if (File.Exists(filePath))
        {
            StreamWriter writer = new StreamWriter(filePath, true);
            StringBuilder strError = new StringBuilder();

            strError.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
            strError.Append(Environment.NewLine);
            strError.Append(pageName);
            strError.Append(Environment.NewLine);
            strError.Append(funcName);
            strError.Append(Environment.NewLine);
            strError.Append(errMsg);
            strError.Append(Environment.NewLine);
            strError.Append(Environment.NewLine);
            strError.Append(Environment.NewLine);
            writer.Write(strError.ToString());
            writer.Flush();
            writer.Close();
        }

    }

    // Encrypt string
    public string EnryptString(string strEncrypted)
    {
        byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
        string encrypted = Convert.ToBase64String(b);
        return encrypted;
    }

    // Decrypt string
    public string DecryptString(string encrString)
    {
        byte[] b;
        string decrypted;
        try
        {
            b = Convert.FromBase64String(encrString);
            decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
        }
        catch (FormatException fe)
        {
            decrypted = "";
        }
        return decrypted;
    }

    /// <summary>
    /// Send Notification
    ///  <param name="customerId">Customer Id</param>
    ///  <param name="msgTitle">Message Title</param>
    ///  <param name="msgBody">Message Body</param>
    ///  <param name="msgImage">message Image (default value is empty)</param>
    /// </summary>
    public void SendPushNotification(string customerId, string msgTitle, string msgBody, string msgImage = "")
    {
        try
        {
            WebClient wbClient = new WebClient();
            string baseUrl = "https://genericartmedicine.com/api_ecom/send_notification?customer_id=" + customerId + "&title=" + msgTitle + "&body=" + msgBody + "&image=" + msgImage;
            Stream dStream = wbClient.OpenRead(baseUrl);
            StreamReader strReader = new StreamReader(dStream);
            string s = strReader.ReadToEnd();

            dStream.Close();
            strReader.Close();
        }
        catch (Exception)
        {

            throw;
        }
    }

    /// <summary>
    ///Used to get nearest shop from user location
    /// </summary>
    /// <param name="latitudeX">users address lattitude</param>
    /// <param name="longitudeY">users address longitude</param>
    /// <param name="kmRange">kilometers in range </param>
    /// <param name="pincode">user pincode </param>
    /// <returns>value</returns>
    public Object NearestShop(double latitudeX, double longitudeY, double kmRange, string pincode)
    {
        try
        {
            string strQuery = "";
            if (pincode != "")
            {
                strQuery = "Select FranchID, FranchLatLong From FranchiseeData Where FranchPinCode='" + pincode + "' AND FranchLatLong Is Not Null And FranchLatLong<>'' AND FranchLatLong like '_%,%_'";
                //strQuery = "Select FranchID, FranchLatLong From FranchiseeData Where FranchLatLong Is Not Null And FranchLatLong<>''And FranchLatLong like '_%,%_'";
            }
            else
            {
                strQuery = "Select FranchID, FranchLatLong From FranchiseeData Where FranchLatLong Is Not Null And FranchLatLong<>''And FranchLatLong like '_%,%_'";
            }

            using (DataTable dt = GetDataTable(strQuery))
            {
                DataTable franchisee = new DataTable();
                franchisee.Columns.Add("FranchID", typeof(int));
                franchisee.Columns.Add("KM", typeof(double));

                if (dt.Rows.Count > 0)
                {
                    double frLattitude, frLongitude;
                    
                    DataTable franchiseeSorted = new DataTable();
                   
                    foreach (DataRow row in dt.Rows)
                    {
                        string latlong = row["FranchLatLong"].ToString();
                        if (latlong.Length > 11)
                        {
                            var location = latlong.Split(',');
                            frLattitude = Convert.ToDouble(location[0]);
                            frLongitude = Convert.ToDouble(location[1]);

                            var sCoord = new GeoCoordinate(latitudeX, longitudeY);
                            var eCoord = new GeoCoordinate(frLattitude, frLongitude);
                            double meters = sCoord.GetDistanceTo(eCoord);
                            double tempkm = meters / 1000;
                            double km = Math.Round((Double)tempkm, 2);
                            if (km <= kmRange)
                            {
                                franchisee.Rows.Add(Convert.ToInt32(row["FranchID"]), km);
                            }
                        }
                    }
                    franchisee.DefaultView.Sort = "KM ASC";
                    franchiseeSorted = franchisee.DefaultView.ToTable();
                    return franchiseeSorted;
                }
                else
                {
                    franchisee.Rows.Add(0, 0);
                    return franchisee.DefaultView.ToTable(); 
                }
            }
        }
        catch (Exception)
        {

            throw;
        }

    }


    public DataTable GetData(string sortColumn, string sortDirection, int OffsetValue, int PagingSize, string searchby)
    {
        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GenCartDATA"].ConnectionString))
        {
            conn.Open();
            SqlCommand com = new SqlCommand("spDataInDataTable", conn);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@sortColumn", sortColumn);
            com.Parameters.AddWithValue("@sortOrder", sortDirection);
            com.Parameters.AddWithValue("@OffsetValue", OffsetValue);
            com.Parameters.AddWithValue("@PagingSize", PagingSize);
            com.Parameters.AddWithValue("@SearchText", searchby);
            SqlDataAdapter da = new SqlDataAdapter(com);
            da.Fill(dt);
            da.Dispose();
            conn.Close();
        }
        return dt;

    }

    public string GetRazorPayTransStatus(string txnId)
    {
        try
        {
            string apiUrl = String.Format("https://www.genericartmedicine.com/api_ecom/Razorpay_Transaction_Status");
            WebRequest request = WebRequest.Create(apiUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            request.Headers["20"] = "*/*";
            request.Headers["22"] = "*";
            request.Headers["23"] = "en-US,en;q=0.8,hi;q=0.6";
            request.Headers["0"] = "no-cache";

            string postData = "";
            postData = "id=" + txnId;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return result;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string GetTransactionStatus(int ordId, int custId, int type)
    {
        try
        {
            string apiUrl = String.Format("https://www.genericartmedicine.com/api_ecom/transaction_new_status");
            WebRequest request = WebRequest.Create(apiUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            request.Headers["20"] = "*/*";
            request.Headers["22"] = "*";
            request.Headers["23"] = "en-US,en;q=0.8,hi;q=0.6";
            request.Headers["0"] = "no-cache";

            string postData = "";
            postData = "order_id=" + ordId + "&CustomrtID=" + custId + "&type=" + type;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return result;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string GetOrderTxnId(int ordId, int custId, double ordAmount, int type, string deviceType)
    {
        try
        {
            string txnUrl = String.Format("https://www.genericartmedicine.com/api_ecom/get_new_token");

            WebRequest request = WebRequest.Create(txnUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            request.Headers["20"] = "*/*";
            request.Headers["22"] = "*";
            request.Headers["23"] = "en-US,en;q=0.8,hi;q=0.6";
            request.Headers["0"] = "no-cache";

            string postData = "";
            postData = "order_id=" + ordId + "&final_amount=" + ordAmount + "&CustomrtID=" + custId + "&device_type=" + deviceType + "&type=" + type;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return result;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string GetSettlementStatus()
    {
        try
        {
            string apiUrl = String.Format("https://www.genericartmedicine.com/api_ecom/Razorpay_settlement");
            WebRequest request = WebRequest.Create(apiUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            request.Headers["20"] = "*/*";
            request.Headers["22"] = "*";
            request.Headers["23"] = "en-US,en;q=0.8,hi;q=0.6";
            request.Headers["0"] = "no-cache";

            string postData = "";
            //postData = "order_id=" + ordId + "&CustomrtID=" + custId + "&type=" + type;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return result;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    // Servetel Click_To_Call
    /// <summary>
    /// 
    /// </summary>
    /// <param name="AgentNumber"></param>
    /// <param name="DestinNumber"></param>
    /// <returns></returns>
    public string Servetel_ClickToCall(string AgentNumber, string DestinNumber)
    {
        try
        {
            string apiUrl = String.Format("https://api.servetel.in/v1/click_to_call"); //testing url
            WebRequest request = WebRequest.Create(apiUrl);
            request.Method = "POST";
            request.ContentType = "application/json";

            //request.Headers.Add("Authorization", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOjEwNTgyLCJpc3MiOiJodHRwczpcL1wvY3VzdG9tZXIuc2VydmV0ZWwuaW5cL3Rva2VuXC9nZW5lcmF0ZSIsImlhdCI6MTY0NjI5OTI2NCwiZXhwIjoxOTQ2Mjk5MjY0LCJuYmYiOjE2NDYyOTkyNjQsImp0aSI6IjZYenk3SlNwUGdhUWpvTzcifQ.crtGwSdWo7F6vcrXLM_0Xa-T_sKcdhXKEKVJNBMXlLk");
            request.Headers.Add("Authorization", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOjEwNTgyLCJpc3MiOiJodHRwczpcL1wvY3VzdG9tZXIuc2VydmV0ZWwuaW5cL3Rva2VuXC9nZW5lcmF0ZSIsImlhdCI6MTY1ODQwMTQ4OCwiZXhwIjoxOTU4NDAxNDg4LCJuYmYiOjE2NTg0MDE0ODgsImp0aSI6IjZvb09SdHlMelFJOFh0VXEifQ.trltm3lpp_ED1CDUGE8mLbeY5cqpJLAS0dlHzoy2v6U");

            string destinationNo = DestinNumber;

            //string postData = "{\"destination_number\":\"" + destinationNo + "\", \"agent_number\":\"8446247016\"}";
            string postData = "{\"destination_number\":\"" + destinationNo + "\", \"agent_number\":\"" + AgentNumber + "\", \"caller_id\":\"8068493740\"}";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    //apiResponse = ErrNotification(1, "Response : " + result.ToString());

                    return result.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            //return ex.Message.ToString();
            return "Call Connection Failure";
        }
    }

    public CustomerLookup GetCustLookupDetails(int custId)
    {
        try
        {
            CustomerLookup cl = new CustomerLookup();
            using (DataTable dtCust = GetDataTable("Select CustomrtID, CustomerJoinDate, CustomerName, CustomerMobile, CustomerEmail, CustomerDOB, CustomerFavShop From CustomersData Where CustomrtID=" + custId))
            {
                if (dtCust.Rows.Count > 0)
                {
                    DataRow row = dtCust.Rows[0];

                    cl.CustomerId = Convert.ToInt32(row["CustomrtID"]);
                    cl.CustomerName = row["CustomerName"] != DBNull.Value && row["CustomerName"] != null && row["CustomerName"].ToString() != "" ? row["CustomerName"].ToString() : "NA";
                    string daysPassed = "NA", joinDate = "NA";
                    if (row["CustomerJoinDate"] != DBNull.Value && row["CustomerJoinDate"] != null && row["CustomerJoinDate"].ToString() != "")
                    {
                        joinDate = Convert.ToDateTime(row["CustomerJoinDate"]).ToString("dd/MM/yyyy");
                        daysPassed = GetTimeSpan(Convert.ToDateTime(row["CustomerJoinDate"]));
                    }
                    cl.CustomerJoinDate = joinDate + ", (" + daysPassed + ")";
                    //cl.CustomerDOB = row["CustomerDOB"] != DBNull.Value && row["CustomerDOB"] != null && row["CustomerDOB"].ToString() != "" ? Convert.ToDateTime(row["CustomerDOB"]).ToString("dd/MM/yyyy") : "NA";
                    if (row["CustomerDOB"] != DBNull.Value && row["CustomerDOB"] != null && row["CustomerDOB"].ToString() != "")
                    {
                        string dob = Convert.ToDateTime(row["CustomerDOB"]).ToString("yyyy");
                        if (dob.ToString().Contains("1900"))
                        {
                            cl.CustomerDOB = "NA";
                        }
                        else
                        {
                            cl.CustomerDOB = Convert.ToDateTime(row["CustomerDOB"]).ToString("dd/MM/yyyy");
                        }
                    }
                    else
                    {
                        cl.CustomerDOB = "NA";
                    }
                    cl.CustomerEmail = row["CustomerEmail"] != DBNull.Value && row["CustomerEmail"] != null && row["CustomerEmail"].ToString() != "" ? row["CustomerEmail"].ToString() : "NA";
                    cl.CustomerMob = row["CustomerMobile"] != DBNull.Value && row["CustomerMobile"] != null && row["CustomerMobile"].ToString() != "" ? row["CustomerMobile"].ToString() : "NA";
                    string favShop = "NA";
                    if (row["CustomerFavShop"] != DBNull.Value && row["CustomerFavShop"] != null && row["CustomerFavShop"].ToString() != "")
                    {
                        favShop = GetReqData("FranchiseeData", "FranchName", "FranchID=" + row["CustomerFavShop"]).ToString();
                    }
                    cl.CustomerFavShop = favShop;
                }
            }

            // Orders Details
            cl.TotalOrders = returnAggregate("Select Count(OrderID) From OrdersData Where FK_OrderCustomerID=" + custId + " AND OrderStatus IN (6, 7)").ToString();

            string ordDate = GetReqData("OrdersData", "TOP 1 OrderDate", "FK_OrderCustomerID=" + custId + " Order By OrderDate DESC").ToString();
            string daysPassedFromOrder = "-";
            daysPassedFromOrder = GetTimeSpan(Convert.ToDateTime(ordDate));
            cl.LastOrderDate = Convert.ToDateTime(ordDate).ToString("dd/MM/yyyy hh:mm tt") + "(" + daysPassedFromOrder + ")";

            cl.ProductsPurchased = "<a href=\"products-purchased-by-cust.aspx?custId=" + custId + "\" >Click Here</a>";


            cl.TotalOrderAmount = returnAggregateNew("Select SUM(OrderAmount) From OrdersData Where FK_OrderCustomerID=" + custId + " AND OrderStatus IN (6, 7)").ToString();

            double avgAmt = Convert.ToDouble(cl.TotalOrderAmount) / Convert.ToInt32(cl.TotalOrders);
            cl.AvgOrderAmount = avgAmt.ToString();

            //yearly Order Summary
            //string endDate = DateTime.Now.AddMonths(-1).ToString("yyyyMMdd");
            string endDate = DateTime.Now.ToString("yyyy") + DateTime.Now.AddMonths(-1).ToString("MM") + "01";
            //string startDate = DateTime.Now.AddMonths(-2).AddYears(-1).ToString("yyyyMMdd");
            string startDate = DateTime.Now.AddYears(-1).ToString("yyyy") + DateTime.Now.ToString("MM") + "01";

            using (DataTable dtYears = GetDataTable("DECLARE @start DATE = '" + startDate + "'  , @end DATE = '" + endDate + "'  ;WITH Numbers (Number) AS   " +
                        " (SELECT ROW_NUMBER() OVER (ORDER BY OBJECT_ID) FROM sys.all_objects)  " +
                        " SELECT DATENAME(MONTH,DATEADD(MONTH, Number - 1, @start)) Name,MONTH(DATEADD(MONTH, Number - 1, @start)) MonthId  ," +
                        " DATENAME(YEAR,DATEADD(MONTH, Number - 1, @start)) Year FROM Numbers  a WHERE Number - 1 <= DATEDIFF(MONTH, @start, @end)  "))
            {
                if (dtYears.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();
                    strMarkup.Append("<div class=\"table-responsive-lg\">");
                    strMarkup.Append("<table class=\"table\">");
                    strMarkup.Append("<thead class=\"thead-dark\">");
                    strMarkup.Append("<tr>");
                    strMarkup.Append("<th scope=\"col\">Year</th>");
                    strMarkup.Append("<th scope=\"col\">Total Orders</th>");
                    strMarkup.Append("<th scope=\"col\">Total Amount</th>");
                    strMarkup.Append("</tr>");
                    strMarkup.Append("</thead>");
                    strMarkup.Append("<tbody>");
                    int ordCount = 0;
                    double ordAmt = 0.0;
                    foreach (DataRow yRow in dtYears.Rows)
                    {
                        strMarkup.Append("<tr>");
                        strMarkup.Append("<th scope=\"row\">" + yRow["Name"] + " " + yRow["Year"] + "</th>");
                        int yOrderCount = Convert.ToInt32(returnAggregate("Select Count(OrderID) From OrdersData Where FK_OrderCustomerID=" + custId + " AND OrderStatus IN (6, 7) AND (YEAR(OrderDate) = " + yRow["Year"] + " And MONTH(OrderDate)=" + yRow["MonthId"] + ")"));
                        strMarkup.Append("<td>" + yOrderCount + "</td>");
                        ordCount = ordCount + yOrderCount;
                        double yOrdAmount = Convert.ToDouble(returnAggregateNew("Select SUM(OrderAmount) From OrdersData Where FK_OrderCustomerID=" + custId + " AND OrderStatus IN (6, 7) AND (YEAR(OrderDate) = " + yRow["Year"] + " And MONTH(OrderDate)=" + yRow["MonthId"] + ")"));
                        strMarkup.Append("<td>" + yOrdAmount + "</td>");
                        ordAmt = ordAmt + yOrdAmount;
                        strMarkup.Append("</tr>");
                    }
                    strMarkup.Append("<tr class=\"table-dark\">");
                    strMarkup.Append("<th scope=\"row\">Total</th>");
                    strMarkup.Append("<td>" + ordCount + "</td>");
                    strMarkup.Append("<td>" + ordAmt + "</td>");
                    strMarkup.Append("</tr>");
                    strMarkup.Append("</tbody>");
                    strMarkup.Append("</table>");
                    strMarkup.Append("</div>");

                    cl.YearlyOrderSummary = strMarkup.ToString();
                }
            }

            return cl;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public double GetOBPComission(int OrderIdX)
    {
        try
        {
            if (IsRecordExist("Select OrderID From OrdersData Where GOBPId IS NOT NULL OR GOBPId<>'' OR GOBPId<>0 AND OrderID=" + OrderIdX))
            {
                int gobpId = Convert.ToInt32(GetReqData("OrdersData", "GOBPId", "OrderID=" + OrderIdX));
                if (IsRecordExist("Select OrdDetailID From OrdersDetails Where FK_DetailOrderID=" + OrderIdX))
                {
                    //Generic Products
                    string GenericProdAmount = returnAggregate("Select SUM(a.OrdDetailAmount) From OrdersDetails a Inner Join ProductsData b " +
                        " On a.FK_DetailProductID=b.ProductID Inner Join OrdersData c On a.FK_DetailOrderID=c.OrderID Where " +
                        " c.OrderID=" + OrderIdX + " AND c.GOBPId=" + gobpId + " AND b.FK_CategoryID=2").ToString();

                    //Surgical Products
                    string SurgicalProdAmount = returnAggregate("Select SUM(a.OrdDetailAmount) From OrdersDetails a Inner Join ProductsData b " +
                        " On a.FK_DetailProductID=b.ProductID Inner Join OrdersData c On a.FK_DetailOrderID=c.OrderID Where " +
                        " c.OrderID=" + OrderIdX + " AND c.GOBPId=" + gobpId + " AND b.FK_CategoryID=12").ToString();

                    //Ayurvedic Products
                    string AyurvedicProdAmount = returnAggregate("Select SUM(a.OrdDetailAmount) From OrdersDetails a Inner Join ProductsData b " +
                        " On a.FK_DetailProductID=b.ProductID Inner Join OrdersData c On a.FK_DetailOrderID=c.OrderID Where " +
                        " c.OrderID=" + OrderIdX + " AND c.GOBPId=" + gobpId + " AND b.FK_CategoryID=10").ToString();

                    //Cosmetic Products
                    string CosmeticProdAmount = returnAggregate("Select SUM(a.OrdDetailAmount) From OrdersDetails a Inner Join ProductsData b " +
                        " On a.FK_DetailProductID=b.ProductID Inner Join OrdersData c On a.FK_DetailOrderID=c.OrderID Where " +
                        " c.OrderID=" + OrderIdX + " AND c.GOBPId=" + gobpId + " AND b.FK_CategoryID=3").ToString();

                    //obp type = 1>30K, 2>60K, 3>1Lac
                    int obpType = Convert.ToInt32(GetReqData("OBPData", "OBP_FKTypeID", "OBP_ID=" + gobpId));
                    int genProdPercentage = 0, surgProdPercentage = 0, ayurProdPercentage = 0, cosProdPercentage = 0;

                    switch (obpType)
                    {
                        case 1: 
                            //genProdPercentage = 20; 
                            genProdPercentage = Convert.ToInt32(GetReqData("ProductCategory", "GOBP30K", "ProductCatID=2"));
                            surgProdPercentage = Convert.ToInt32(GetReqData("ProductCategory", "GOBP30K", "ProductCatID=12"));
                            ayurProdPercentage = Convert.ToInt32(GetReqData("ProductCategory", "GOBP30K", "ProductCatID=10"));
                            cosProdPercentage = Convert.ToInt32(GetReqData("ProductCategory", "GOBP30K", "ProductCatID=3"));
                            break;
                        case 2:
                            genProdPercentage = Convert.ToInt32(GetReqData("ProductCategory", "GOBP60K", "ProductCatID=2"));
                            surgProdPercentage = Convert.ToInt32(GetReqData("ProductCategory", "GOBP60K", "ProductCatID=12"));
                            ayurProdPercentage = Convert.ToInt32(GetReqData("ProductCategory", "GOBP60K", "ProductCatID=10"));
                            cosProdPercentage = Convert.ToInt32(GetReqData("ProductCategory", "GOBP60K", "ProductCatID=3"));
                            break;
                        case 3:
                            genProdPercentage = Convert.ToInt32(GetReqData("ProductCategory", "GOBP1Lac", "ProductCatID=2"));
                            surgProdPercentage = Convert.ToInt32(GetReqData("ProductCategory", "GOBP1Lac", "ProductCatID=12"));
                            ayurProdPercentage = Convert.ToInt32(GetReqData("ProductCategory", "GOBP1Lac", "ProductCatID=10"));
                            cosProdPercentage = Convert.ToInt32(GetReqData("ProductCategory", "GOBP1Lac", "ProductCatID=3"));
                            break;
                    }

                    //int surgicalPercent = GetReqData("ProductCategory","")

                    cosProdPercentage = 10;
                    double finalGenProdCommission = (Convert.ToDouble(GenericProdAmount) * genProdPercentage) / 100;
                    double finalSurgicalProdCommission = (Convert.ToDouble(SurgicalProdAmount) * surgProdPercentage) / 100;
                    double finalAyurvedicProdCommission = (Convert.ToDouble(AyurvedicProdAmount) * ayurProdPercentage) / 100;
                    double finalCosmeticProdCommission = (Convert.ToDouble(CosmeticProdAmount) * cosProdPercentage) / 100;

                    double comAmount = (finalGenProdCommission + finalSurgicalProdCommission + finalAyurvedicProdCommission + finalCosmeticProdCommission);
                    return comAmount;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        catch (Exception)
        {
            return -1;
        }
    }

    public double CalCulateGMitraComission(int OrderIdX)
    {
        try
        {
            if (IsRecordExist("Select OrderID From OrdersData Where GMitraId IS NOT NULL OR GMitraId<>'' OR GMitraId<>0 AND OrderID=" + OrderIdX))
            {
                int gMitraId = Convert.ToInt32(GetReqData("OrdersData", "GMitraId", "OrderID=" + OrderIdX));
                if (IsRecordExist("Select OrdDetailID From OrdersDetails Where FK_DetailOrderID=" + OrderIdX))
                {
                    //Generic Products
                    string GenericProdAmount = returnAggregate("Select SUM(a.OrdDetailAmount) From OrdersDetails a Inner Join ProductsData b " +
                        " On a.FK_DetailProductID=b.ProductID Inner Join OrdersData c On a.FK_DetailOrderID=c.OrderID Where " +
                        " c.OrderID=" + OrderIdX + " AND c.GMitraId=" + gMitraId + " AND b.FK_CategoryID=2").ToString();

                    //Surgical Products
                    string SurgicalProdAmount = returnAggregate("Select SUM(a.OrdDetailAmount) From OrdersDetails a Inner Join ProductsData b " +
                        " On a.FK_DetailProductID=b.ProductID Inner Join OrdersData c On a.FK_DetailOrderID=c.OrderID Where " +
                        " c.OrderID=" + OrderIdX + " AND c.GMitraId=" + gMitraId + " AND b.FK_CategoryID=12").ToString();

                    //Ayurvedic Products
                    string AyurvedicProdAmount = returnAggregate("Select SUM(a.OrdDetailAmount) From OrdersDetails a Inner Join ProductsData b " +
                        " On a.FK_DetailProductID=b.ProductID Inner Join OrdersData c On a.FK_DetailOrderID=c.OrderID Where " +
                        " c.OrderID=" + OrderIdX + " AND c.GMitraId=" + gMitraId + " AND b.FK_CategoryID=10").ToString();

                    //Cosmetic Products
                    string CosmeticProdAmount = returnAggregate("Select SUM(a.OrdDetailAmount) From OrdersDetails a Inner Join ProductsData b " +
                        " On a.FK_DetailProductID=b.ProductID Inner Join OrdersData c On a.FK_DetailOrderID=c.OrderID Where " +
                        " c.OrderID=" + OrderIdX + " AND c.GMitraId=" + gMitraId + " AND b.FK_CategoryID=3").ToString();

                    //obp type = 1>30K, 2>60K, 3>1Lac
                    int obpType = 1;
                    int genProdPercentage = 0, surgProdPercentage = 0, ayurProdPercentage = 0, cosProdPercentage = 0;

                    genProdPercentage = Convert.ToInt32(GetReqData("ProductCategory", "GOBP30K", "ProductCatID=2"));
                    surgProdPercentage = Convert.ToInt32(GetReqData("ProductCategory", "GOBP30K", "ProductCatID=12"));
                    ayurProdPercentage = Convert.ToInt32(GetReqData("ProductCategory", "GOBP30K", "ProductCatID=10"));
                    cosProdPercentage = Convert.ToInt32(GetReqData("ProductCategory", "GOBP30K", "ProductCatID=3"));
                    //int surgicalPercent = GetReqData("ProductCategory","")

                    cosProdPercentage = 10;
                    double finalGenProdCommission = (Convert.ToDouble(GenericProdAmount) * genProdPercentage) / 100;
                    double finalSurgicalProdCommission = (Convert.ToDouble(SurgicalProdAmount) * surgProdPercentage) / 100;
                    double finalAyurvedicProdCommission = (Convert.ToDouble(AyurvedicProdAmount) * ayurProdPercentage) / 100;
                    double finalCosmeticProdCommission = (Convert.ToDouble(CosmeticProdAmount) * cosProdPercentage) / 100;

                    double comAmount = (finalGenProdCommission + finalSurgicalProdCommission + finalAyurvedicProdCommission + finalCosmeticProdCommission);
                    return comAmount;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        catch (Exception)
        {
            return -1;
        }
    }

    public string GetFinancialYear()
    {
        try
        {
            DateTime now = DateTime.Now;
            DateTime startDate, endDate;
            if (Convert.ToInt32(now.ToString("MM")) >= 4)
            {
                // Temp make it from 1-JAN
                startDate = Convert.ToDateTime("4/1/" + now.ToString("yyyy"));
                endDate = Convert.ToDateTime("3/31/" + now.AddYears(1).ToString("yyyy"));
            }
            else
            {
                startDate = Convert.ToDateTime("4/1/" + now.AddYears(-1).ToString("yyyy"));
                endDate = Convert.ToDateTime("3/31/" + now.ToString("yyyy"));
            }
            return startDate.ToString("MM/dd/yyyy") + "#" + endDate.ToString("MM/dd/yyyy");
        }
        catch (Exception)
        {
            return "";
        }
    }

    public GOBPLookup GetGOBPLookupDetails(int obpid)
    {
        try
        {
            GOBPLookup cl = new GOBPLookup();
            using (DataTable dtCust = GetDataTable("SELECT [OBP_ID], [OBP_JoinDate], [OBP_ApplicantName], [OBP_MobileNo], [OBP_EmailId], [OBP_UserID], [OBP_UserPWD] FROM [dbo].[OBPData] WHERE [OBP_ID] =" + obpid))
            {
                if (dtCust.Rows.Count > 0)
                {
                    DataRow row = dtCust.Rows[0];

                    cl.OBP_ID = Convert.ToInt32(row["OBP_ID"]).ToString();
                    cl.OBP_ApplicantName = row["OBP_ApplicantName"] != DBNull.Value && row["OBP_ApplicantName"] != null && row["OBP_ApplicantName"].ToString() != "" ? row["OBP_ApplicantName"].ToString() : "NA";
                    string daysPassed = "NA", joinDate = "NA";
                    if (row["OBP_JoinDate"] != DBNull.Value && row["OBP_JoinDate"] != null && row["OBP_JoinDate"].ToString() != "")
                    {
                        joinDate = Convert.ToDateTime(row["OBP_JoinDate"]).ToString("dd/MM/yyyy");
                        daysPassed = GetTimeSpan(Convert.ToDateTime(row["OBP_JoinDate"]));
                    }
                    cl.OBP_JoinDate = joinDate + ", (" + daysPassed + ")";
                    //cl.CustomerDOB = row["CustomerDOB"] != DBNull.Value && row["CustomerDOB"] != null && row["CustomerDOB"].ToString() != "" ? Convert.ToDateTime(row["CustomerDOB"]).ToString("dd/MM/yyyy") : "NA";

                    cl.OBP_EmailId = row["OBP_EmailId"] != DBNull.Value && row["OBP_EmailId"] != null && row["OBP_EmailId"].ToString() != "" ? row["OBP_EmailId"].ToString() : "NA";
                    cl.OBP_MobileNo = row["OBP_MobileNo"] != DBNull.Value && row["OBP_MobileNo"] != null && row["OBP_MobileNo"].ToString() != "" ? row["OBP_MobileNo"].ToString() : "NA";
                    cl.OBP_UserID = row["OBP_UserID"] != DBNull.Value && row["OBP_UserID"] != null && row["OBP_UserID"].ToString() != "" ? row["OBP_UserID"].ToString() : "NA";
                    cl.OBP_UserPWD = row["OBP_UserPWD"] != DBNull.Value && row["OBP_UserPWD"] != null && row["OBP_UserPWD"].ToString() != "" ? row["OBP_UserPWD"].ToString() : "NA";
                }
            }

            //yearly Order Summary
            //string endDate = DateTime.Now.AddMonths(-1).ToString("yyyyMMdd");
            string endDate = DateTime.Now.ToString("yyyy") + DateTime.Now.AddMonths(-1).ToString("MM") + "01";
            //string startDate = DateTime.Now.AddMonths(-2).AddYears(-1).ToString("yyyyMMdd");
            string startDate = DateTime.Now.AddYears(-1).ToString("yyyy") + DateTime.Now.ToString("MM") + "01";

            using (DataTable dtYears = GetDataTable("DECLARE @start DATE = '" + startDate + "'  , @end DATE = '" + endDate + "'  ;WITH Numbers (Number) AS   " +
                        " (SELECT ROW_NUMBER() OVER (ORDER BY OBJECT_ID) FROM sys.all_objects)  " +
                        " SELECT DATENAME(MONTH,DATEADD(MONTH, Number - 1, @start)) Name,MONTH(DATEADD(MONTH, Number - 1, @start)) MonthId  ," +
                        " DATENAME(YEAR,DATEADD(MONTH, Number - 1, @start)) Year FROM Numbers  a WHERE Number - 1 <= DATEDIFF(MONTH, @start, @end)"))
            {
                if (dtYears.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();
                    strMarkup.Append("<div class=\"table-responsive-lg\">");
                    strMarkup.Append("<table class=\"table\">");
                    strMarkup.Append("<thead class=\"thead-dark\">");
                    strMarkup.Append("<tr>");
                    strMarkup.Append("<th scope=\"col\">Year</th>");
                    strMarkup.Append("<th scope=\"col\">Total Customers</th>");
                    strMarkup.Append("<th scope=\"col\">Total Orders</th>");
                    strMarkup.Append("<th scope=\"col\">Total Incentive</th>");
                    strMarkup.Append("</tr>");
                    strMarkup.Append("</thead>");
                    strMarkup.Append("<tbody>");
                    int ordCust = 0;
                    int ordCount = 0;
                    double ordAmt = 0.0;
                    foreach (DataRow yRow in dtYears.Rows)
                    {
                        strMarkup.Append("<tr>");
                        strMarkup.Append("<th scope=\"row\">" + yRow["Name"] + " " + yRow["Year"] + "</th>");
                        int yOrderCustomer = Convert.ToInt32(returnAggregate("SELECT ISNULL(COUNT([CustomrtID]),0) FROM [dbo].[CustomersData] WHERE [delMark] = 0 AND [CustomerActive] = 1 AND [FK_ObpID] = " + obpid + " AND (YEAR([CustomerJoinDate]) = " + yRow["Year"] + " AND MONTH([CustomerJoinDate]) = " + yRow["MonthId"] + ")"));
                        strMarkup.Append("<td>" + yOrderCustomer + "</td>");
                        ordCust = ordCust + yOrderCustomer;

                        int yOrderCount = Convert.ToInt32(returnAggregate(@"SELECT ISNULL(COUNT(DISTINCT a.[OrderID]),0) FROM [dbo].[OrdersData] AS a
                                                                            LEFT JOIN[dbo].[OBPData] AS b ON a.[GOBPId] = b.[OBP_ID] WHERE b.[OBP_ID] = " + obpid + " AND (YEAR(a.[OrderDate]) = " + yRow["Year"] + " AND MONTH(a.[OrderDate]) = " + yRow["MonthId"] + ")"));
                        strMarkup.Append("<td>" + yOrderCount + "</td>");
                        ordCount = ordCount + yOrderCount;

                        double yOrdAmount = Convert.ToDouble(returnAggregate(@"SELECT SUM(a.[OBPComTotal]) FROM [dbo].[OrdersData] AS a
                                                   LEFT JOIN [dbo].[OBPData] AS b ON a.[GOBPId] = b.[OBP_ID] WHERE b.[OBP_ID] = " + obpid + " AND (YEAR(a.[OrderDate]) = " + yRow["Year"] + " AND MONTH(a.[OrderDate]) = " + yRow["MonthId"] + ")"));
                        strMarkup.Append("<td>" + yOrdAmount + "</td>");
                        ordAmt = ordAmt + yOrdAmount;
                        strMarkup.Append("</tr>");
                    }
                    strMarkup.Append("<tr class=\"table-dark\">");
                    strMarkup.Append("<th scope=\"row\" style=\"color: black\">Total</th>");
                    strMarkup.Append("<td style=\"color: black\">" + ordCust + "</td>");
                    strMarkup.Append("<td style=\"color: black\">" + ordCount + "</td>");
                    strMarkup.Append("<td style=\"color: black\">" + ordAmt + "</td>");
                    strMarkup.Append("</tr>");
                    strMarkup.Append("</tbody>");
                    strMarkup.Append("</table>");
                    strMarkup.Append("</div>");

                    cl.YearlyGOBPSummary = strMarkup.ToString();
                }
            }

            return cl;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    // Calculate Product wise Commission total & Update it to main OrdersData table (5-July-2023)
    public void GOBP_CommissionTotal(int OrderIdX)
    {
        try
        {
            ExecuteQuery("Update OrdersData set OBPComTotal=(Select SUM(OBPComAmt) From OrdersDetails Where FK_DetailOrderID=" + OrderIdX + ") Where OrderId=" + OrderIdX);
        }
        catch(Exception ex)
        {
            throw ex;
        }

    }
    public void GOBP_ProductWise_CommissionCalc(int OrdDetailIDX, int OrderIdX, int ProductID, int ProductQty)
    {
        try
        {
            // Get GOBP Id From OrdersData Table, & check his / her OBP-Type 1/2/3
            int myOBP = Convert.ToInt32(GetReqData("OrdersData", "GobpId", "OrderId = " + OrderIdX));
            int obpType = Convert.ToInt32(GetReqData("OBPData", "OBP_FKTypeID", "OBP_ID = " + myOBP));

            //Get Products Category
            int proCategory = Convert.ToInt32(GetReqData("ProductsData", "FK_CategoryID", "ProductID = " + ProductID));

            //Get Products Taxless Amount
            int taxlessPrice = Convert.ToInt32(GetReqData("ProductsData", "TaxLessAmount", "ProductID = " + ProductID));

            //Get GOBP Percent Value according to GOBPType & Product Category
            int incentPercent = 0;

            switch (obpType)
            {
                case 1:
                    incentPercent = Convert.ToInt32(GetReqData("ProductCategory", "GOBP30K", "ProductCatID=" + proCategory));
                    break;
                case 2:
                    incentPercent = Convert.ToInt32(GetReqData("ProductCategory", "GOBP60K", "ProductCatID=" + proCategory));
                    break;
                case 3:
                    incentPercent = Convert.ToInt32(GetReqData("ProductCategory", "GOBP1Lac", "ProductCatID=" + proCategory));
                    break;
                case 4:
                    //Corporate GOBP type
                    // If Product Category is MEDICNE type i.e. "2", then get commission percentage defined in OBPData table for this GOBP
                    // For other than "2" category get percent of commission like GOBP30K type.
                    if (proCategory == 2)
                    {
                        incentPercent = Convert.ToInt32(GetReqData("OBPData", "OBP_CorpCommission", "OBP_ID =" + myOBP));
                    }
                    else
                    {
                        incentPercent = Convert.ToInt32(GetReqData("ProductCategory", "GOBP30K", "ProductCatID=" + proCategory));
                    }
                    break;
            }

            // Calculate Commission Amount
            //double calcComm = (ProductQty * taxlessPrice) * (incentPercent / 100);
            double calcComm = ((double)incentPercent / 100.0) * (taxlessPrice * ProductQty); ;
            double commAmount = Math.Round(calcComm, 2);

            ExecuteQuery("Update OrdersDetails set OBPComPercent=" + incentPercent + ", OBPComAmt=" + commAmount + " Where  OrdDetailID=" + OrdDetailIDX);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    

    public void GOBP_Recruit_CommissionChain(string GOBP_Operator_UserId, string GOBP_referral_UserId)
    {
        try
        {
            // ======= Level-1 Own Commission =======

            int maxCommId = NextId("OBPCommission", "ObpComId");
            int myObpId = Convert.ToInt32(GetReqData("OBPData", "OBP_ID", "OBP_UserID='" + GOBP_Operator_UserId + "'"));
            string myObpUserID = GOBP_Operator_UserId;

            int myParentOBP_ID = 0;
            int myGrandParentOBP_ID = 0;

            // ======= Level-1 Commission =======
            string queryInsert = @"Insert into OBPCommission(ObpComId, ObpComDate, ObpComType, ObpUserId, FK_Obp_ID, ObpRefUserId, ObpComLevel, ObpComPercent, ObpComAmount)
                Values(" + maxCommId + ", '" + DateTime.Now + "', 'Recruit-Direct', '" + myObpUserID + "', " + myObpId + ", '" + GOBP_referral_UserId + "', 1, 20, 6000)";

            ExecuteQuery(queryInsert);

            // ======= Level-2 Parent's Commission =======
            // Check if any PARENT OBP present for this Session-OBP
            object parentOBP = GetReqData("OBPData", "OBP_Ref_UserId", "OBP_ID=" + myObpId);

            if (parentOBP != null)
            {
                myParentOBP_ID = Convert.ToInt32(GetReqData("OBPData", "OBP_ID", "OBP_UserID='" + parentOBP.ToString() + "'"));

                maxCommId = NextId("OBPCommission", "ObpComId");
                queryInsert = @"Insert into OBPCommission(ObpComId, ObpComDate, ObpComType, ObpUserId, FK_Obp_ID, ObpRefUserId, ObpComLevel, ObpComPercent, ObpComAmount)
                Values(" + maxCommId + ", '" + DateTime.Now + "', 'Recruit-Ref', '" + parentOBP.ToString() + "', " + myParentOBP_ID + ", '" + GOBP_referral_UserId + "', 2, 10, 3000)";

                ExecuteQuery(queryInsert);
            }

            // ======= Level-3 Grand-Parent's Commission =======
            if (parentOBP != null)
            {
                // Check if any GRAND-PARENT OBP present for my Parent-OBP
                object GrandParentOBP = GetReqData("OBPData", "OBP_Ref_UserId", "OBP_ID=" + myParentOBP_ID);

                if (GrandParentOBP != null)
                {
                    myGrandParentOBP_ID = Convert.ToInt32(GetReqData("OBPData", "OBP_ID", "OBP_UserID='" + GrandParentOBP.ToString() + "'"));

                    maxCommId = NextId("OBPCommission", "ObpComId");
                    queryInsert = @"Insert into OBPCommission(ObpComId, ObpComDate, ObpComType, ObpUserId, FK_Obp_ID, ObpRefUserId, ObpComLevel, ObpComPercent, ObpComAmount)
                    Values(" + maxCommId + ", '" + DateTime.Now + "', 'Recruit-Ref', '" + GrandParentOBP.ToString() + "', " + myGrandParentOBP_ID + ", '" + GOBP_referral_UserId + "', 3, 5, 1500)";

                    ExecuteQuery(queryInsert);
                }

            }

        }
        catch (Exception ex)
        {
            //errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    public void GOBP_Sales_CommissionChain(int GOBP_IdX, int Order_IdX)
    {
        try
        {
            // Check If already Commission released for this order, if YES then exit.
            if(IsRecordExist("Select ObpComId From OBPCommission Where FK_OrderId=" + Order_IdX) == true)
            {
                return;
            }

            // ======= Level-1 Own Commission =======
            int myObpId = GOBP_IdX;
            string myObpUserId = GetReqData("OBPData", "OBP_UserId", "OBP_ID=" + myObpId).ToString();

            // Get Total Sales amount total of order as per TaxLess Price and Qty
            double salesTotal = returnAggregate("Select SUM(OD.OrdDetailQTY * PD.TaxLessAmount) as SalesTotal From OrdersDetails OD Inner Join ProductsData PD on OD.FK_DetailProductID=PD.ProductID Where OD.FK_DetailOrderID=" + Order_IdX);
            double commLevelOne = salesTotal * 0.25; // 25%

            int maxCommId = NextId("OBPCommission", "ObpComId");

            // ======= Level-1 Direct Sale Commission 25% =======
            string queryInsert = @"Insert into OBPCommission(ObpComId, ObpComDate, ObpComType, ObpUserId, FK_Obp_ID, ObpRefUserId, FK_OrderId, ObpComLevel, ObpComPercent, ObpComAmount)
                Values(" + maxCommId + ", '" + DateTime.Now + "', 'Sales-Direct', '" + myObpUserId + "', " + myObpId + ", NULL, " + Order_IdX + ", 1, 25, " + commLevelOne + ")";
            ExecuteQuery(queryInsert);

            int myParentOBP_ID = 0;
            //string myParentOBP_UserID = "";

            // ======= Level-2 Parent's Commission =======
            // Check if any PARENT OBP present for this Session-OBP
            object parentOBP_L1 = GetReqData("OBPData", "OBP_Ref_UserId", "OBP_ID=" + myObpId);
            object parentOBP_L2 = null;
            object parentOBP_L3 = null;
            object parentOBP_L4 = null;

            if (parentOBP_L1 != null)
            {
                myParentOBP_ID = Convert.ToInt32(GetReqData("OBPData", "OBP_ID", "OBP_UserID='" + parentOBP_L1.ToString() + "'"));
                maxCommId = NextId("OBPCommission", "ObpComId");
                double commLevelTwo = salesTotal * 0.06; // 6%

                queryInsert = @"Insert into OBPCommission(ObpComId, ObpComDate, ObpComType, ObpUserId, FK_Obp_ID, ObpRefUserId, FK_OrderId, ObpComLevel, ObpComPercent, ObpComAmount)
                Values(" + maxCommId + ", '" + DateTime.Now + "', 'Sales-Ref', '" + parentOBP_L1.ToString() + "', " + myParentOBP_ID + ",'" + myObpUserId  + "', " + Order_IdX + ", 2, 6, " + commLevelTwo + ")";

                ExecuteQuery(queryInsert);
            }



            // ======= Level-3 Grand-Parent's Commission =======
            if (parentOBP_L1 != null)
            {
                // Check if any GRAND-PARENT OBP present for my Parent-OBP
                parentOBP_L2 = GetReqData("OBPData", "OBP_Ref_UserId", "OBP_UserID='" + parentOBP_L1.ToString() + "'");

                if (parentOBP_L2 != null)
                {
                   int  parentOBP_ID_L2 = Convert.ToInt32(GetReqData("OBPData", "OBP_ID", "OBP_UserID='" + parentOBP_L2.ToString() + "'"));

                    maxCommId = NextId("OBPCommission", "ObpComId");
                    double commLevelTree = salesTotal * 0.02; // 2%

                    queryInsert = @"Insert into OBPCommission(ObpComId, ObpComDate, ObpComType, ObpUserId, FK_Obp_ID, ObpRefUserId, FK_OrderId, ObpComLevel, ObpComPercent, ObpComAmount)
                Values(" + maxCommId + ", '" + DateTime.Now + "', 'Sales-Ref', '" + parentOBP_L2.ToString() + "', " + parentOBP_ID_L2 + ",'" + myObpUserId + "', " + Order_IdX + ", 3, 2, " + commLevelTree + ")";

                    ExecuteQuery(queryInsert);
                }

            }


            // ======= Level-4 Grand-Parent's Commission =======
            if (parentOBP_L2 != null)
            {
                // Check if any GRAND-PARENT OBP present for my Parent-OBP
                parentOBP_L3 = GetReqData("OBPData", "OBP_Ref_UserId", "OBP_UserID='" + parentOBP_L2.ToString() + "'");

                if (parentOBP_L3 != null)
                {
                    int parentOBP_ID_L3 = Convert.ToInt32(GetReqData("OBPData", "OBP_ID", "OBP_UserID='" + parentOBP_L3.ToString() + "'"));

                    maxCommId = NextId("OBPCommission", "ObpComId");
                    double commLevelFour = salesTotal * 0.02; // 2%

                    queryInsert = @"Insert into OBPCommission(ObpComId, ObpComDate, ObpComType, ObpUserId, FK_Obp_ID, ObpRefUserId, FK_OrderId, ObpComLevel, ObpComPercent, ObpComAmount)
                Values(" + maxCommId + ", '" + DateTime.Now + "', 'Sales-Ref', '" + parentOBP_L3.ToString() + "', " + parentOBP_ID_L3 + ",'" + myObpUserId + "', " + Order_IdX + ", 4, 2, " + commLevelFour + ")";

                    ExecuteQuery(queryInsert);
                }

            }



            // ======= Level-5 Grand-Parent's Commission =======
            if (parentOBP_L3 != null)
            {
                // Check if any GRAND-PARENT OBP present for my Parent-OBP
                parentOBP_L4 = GetReqData("OBPData", "OBP_Ref_UserId", "OBP_UserID='" + parentOBP_L2.ToString() + "'");

                if (parentOBP_L4 != null)
                {
                    int parentOBP_ID_L4 = Convert.ToInt32(GetReqData("OBPData", "OBP_ID", "OBP_UserID='" + parentOBP_L4.ToString() + "'"));

                    maxCommId = NextId("OBPCommission", "ObpComId");
                    double commLevelFive = salesTotal * 0.02; // 2%

                    queryInsert = @"Insert into OBPCommission(ObpComId, ObpComDate, ObpComType, ObpUserId, FK_Obp_ID, ObpRefUserId, FK_OrderId, ObpComLevel, ObpComPercent, ObpComAmount)
                Values(" + maxCommId + ", '" + DateTime.Now + "', 'Sales-Ref', '" + parentOBP_L4.ToString() + "', " + parentOBP_ID_L4 + ",'" + myObpUserId + "', " + Order_IdX + ", 5, 2, " + commLevelFive + ")";

                    ExecuteQuery(queryInsert);
                }

            }


        }
        catch (Exception ex)
        {
            //errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}