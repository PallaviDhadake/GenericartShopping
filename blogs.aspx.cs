using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

public partial class blogs : System.Web.UI.Page
{
    iClass c = new iClass();
    public string newsStr;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(Page.RouteData.Values["blogId"].ToString()))
                {
                    GetAllNews();			
                }
                else
                {
                    string[] arrLinks = Page.RouteData.Values["blogId"].ToString().Split('-');
                    GetAllNewsDetails(Convert.ToInt16(arrLinks[arrLinks.Length - 1]));
                }
            }
        }
        catch (Exception ex)
        {
            newsStr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void GetAllNews()
    {
        try
        {
            using (DataTable dtNews = c.GetDataTable("Select NewsId, NewsDate, NewsTitle, LEFT(NewsDesc, 150) as newsDesc, NewsImage From NewsData ORDER BY NewsDate DESC"))
            {
                if (dtNews.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();
                    int nCount = 1;

                    foreach (DataRow row in dtNews.Rows)
                    {
                        if (row["NewsImage"] != DBNull.Value && row["NewsImage"].ToString() != "" && row["NewsImage"].ToString() != "no-photo.png")
                        {
                            strMarkup.Append("<div class=\"news-img\">");
                            strMarkup.Append("<img src=\"" + Master.rootPath + "upload/news/thumb/" + row["NewsImage"].ToString() + "\" alt=\"" + row["NewsTitle"].ToString() + "\" class=\"width100\" />");
                            strMarkup.Append("</div>");
                            strMarkup.Append("<div class=\"news-info\">");
                        }

                        string nUrl = Master.rootPath + "blogs/" + c.UrlGenerator(row["NewsTitle"].ToString().ToLower() + '-' + row["NewsId"].ToString());
                        strMarkup.Append("<a href=\"" + nUrl + "\" class=\"news-Tag mrg_B_10 fontRegular\">" + row["NewsTitle"].ToString() + "</a>");

                        DateTime nDate = Convert.ToDateTime(row["NewsDate"]);

                        strMarkup.Append("<span class=\"newspost\">Admin | " + nDate.ToString("dd MMM yyyy") + "</span>");
                        strMarkup.Append("<p class=\"paraTxt mrg_B_15\">" + row["newsDesc"].ToString() + "...</p>");
                        strMarkup.Append("<a href=\"" + nUrl + "\" class=\"Readmore fontRegular\">Read More</a>");

                        if (row["NewsImage"] != DBNull.Value && row["NewsImage"].ToString() != "" && row["NewsImage"].ToString() != "no-photo.png")
                            strMarkup.Append("</div>");
                        strMarkup.Append("<div class=\"float_clear\"></div>");
                        if (nCount < dtNews.Rows.Count)
                            strMarkup.Append("<span class=\"greyLine\"></span>");
                        nCount++;
                    }
                    newsStr = strMarkup.ToString();
                }
                else
                {
                    newsStr = "<span class=\"infoClr\">Nothing to display, Come back later...</span>";
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            newsStr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void GetAllNewsDetails(int nwsIdX)
    {
        try
        {
            using (DataTable dtNws = c.GetDataTable("Select * From NewsData Where NewsId=" + nwsIdX))
            {
                if (dtNws.Rows.Count > 0)
                {
                    DataRow row = dtNws.Rows[0];
                    StringBuilder strMarkup = new StringBuilder();

                    this.Title = row["NewsTitle"].ToString() + "| Latest News, Events of Genericart Medicine.";

                    strMarkup.Append("<h2 class=\"pageH2 themeClrPrime mrg_B_5\">" + row["NewsTitle"].ToString() + "</h2>");
                    DateTime nDate = Convert.ToDateTime(row["NewsDate"]);
                    strMarkup.Append("<span class=\"newspost\">Admin | " + nDate.ToString("dd MMM yyyy") + "</span>");

                    strMarkup.Append("<span class=\"space15\"></span>");

                    //Sharing Options
                    strMarkup.Append("<div class=\"a2a_kit a2a_kit_size_24 a2a_default_style\" >");
                    strMarkup.Append("<a class=\"a2a_dd\" href=\"https://www.addtoany.com/share\"></a>");
                    strMarkup.Append("<a class=\"a2a_button_facebook\"></a>");
                    strMarkup.Append("<a class=\"a2a_button_twitter\"></a>");
                    strMarkup.Append("<a class=\"a2a_button_pinterest\"></a>");
                    strMarkup.Append("<a class=\"a2a_button_email\"></a>");
                    strMarkup.Append("<a class=\"a2a_button_linkedin\"></a>");
                    strMarkup.Append("<a class=\"a2a_button_reddit\"></a>");
                    strMarkup.Append("<a class=\"a2a_button_whatsapp\"></a>");
                    strMarkup.Append("<a class=\"a2a_button_blogger_post\"></a>");
                    strMarkup.Append("</div>");
                    strMarkup.Append("<script async src=\"https://static.addtoany.com/menu/page.js\"></script>");

                    //Add Page sharing links code here
                    strMarkup.Append("<div class=\"float_clear\"></div>");
                    strMarkup.Append("<div class=\"space20\"></div>");

                    if (row["NewsImage"] != DBNull.Value && row["NewsImage"].ToString() != "" && row["NewsImage"].ToString() != "no-photo.png")
                        strMarkup.Append("<img src=\"" + Master.rootPath + "upload/news/" + row["NewsImage"].ToString() + "\" alt=\"" + row["NewsTitle"].ToString() + "\" class=\"width100\" />");
                    strMarkup.Append("<span class=\"space20\"></span>");
                    strMarkup.Append("<p class=\"paraTxt\">" + Regex.Replace(row["NewsDesc"].ToString(), @"\r\n?|\n", "<br />") + "</p>");

                    newsStr = strMarkup.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            newsStr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}