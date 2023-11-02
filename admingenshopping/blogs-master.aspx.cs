using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class admingenshopping_blogs_master : System.Web.UI.Page
{
    iClass c = new iClass();
    public string pgTitle, blogImg;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["action"] != null)
                {
                    editBlog.Visible = true;
                    viewBlog.Visible = false;

                    if (Request.QueryString["action"] == "new")
                    {
                        btnSave.Text = "Save Info";
                        pgTitle = "Add Blogs";
                        btnDelete.Visible = false;

                    }
                    else
                    {
                        pgTitle = "Edit Blogs";
                        btnSave.Text = "Modify Info";
                        btnDelete.Visible = true;
                        GetBlogsData(Convert.ToInt32(Request.QueryString["id"]));
                    }
                }
                else
                {
                    viewBlog.Visible = true;
                    editBlog.Visible = false;
                    FillGrid();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "Page_Load", ex.Message.ToString());
            return;
        }
    }

    private void FillGrid()
    {
        try
        {
            using (DataTable dtBlog = c.GetDataTable("Select NewsId, Convert(varchar(20), NewsDate, 103) as nDate, NewsTitle From NewsData Order By NewsId DESC"))
            {
                gvBlogs.DataSource = dtBlog;
                gvBlogs.DataBind();

                if (dtBlog.Rows.Count > 0)
                {
                    gvBlogs.UseAccessibleHeader = true;
                    gvBlogs.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;
        }
    }
    protected void gvBlogs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"blogs-master.aspx?action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\" title=\"View/Edit\"></a>";
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvBlogs_RowDataBound", ex.Message.ToString());
            return;
        }
    }
    protected void gvBlogs_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBlogs.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            txtNDate.Text = txtNDate.Text.Trim().Replace("'", "");
            txtTitle.Text = txtTitle.Text.Trim().Replace("'", "");
            txtDesc.Text = txtDesc.Text.Trim().Replace("'", "");

            if (txtTitle.Text == "" || txtDesc.Text == "" || txtNDate.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * marked fields are mandatary');", true);
                return;
            }
            string[] arrDate = txtNDate.Text.Split('/');
            if (c.IsDate(arrDate[1] + "/" + arrDate[0] + "/" + arrDate[2]) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter valid date');", true);
                return;
            }

            DateTime nDate = Convert.ToDateTime(arrDate[1] + "/" + arrDate[0] + "/" + arrDate[2]);

            int maxId = Request.QueryString["action"] == "new" ? c.NextId("NewsData", "NewsId") : Convert.ToInt32(lblId.Text);

            string fileName = "";

            if (fuImg.HasFile)
            {
                string fExt = Path.GetExtension(fuImg.FileName).ToString().ToLower();
                if (fExt == ".png" || fExt == ".jpg" || fExt == ".jpeg")
                {
                    fileName = "blog-image-" + maxId + fExt;
                    ImageUploadProcess(fileName);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .png, .jpg, .jpeg files are allowed');", true);
                    return;
                }
            }

            if (lblId.Text == "[New]")
            {
                c.ExecuteQuery("Insert Into NewsData (NewsId, NewsDate, NewsTitle, NewsDesc, ReadCount) Values (" + maxId + ", '" + nDate +
                    "', '" + txtTitle.Text + "', '" + txtDesc.Text + "', 0)");

                if (fuImg.HasFile)
                {
                    c.ExecuteQuery("Update NewsData Set NewsImage='" + fileName + "' Where NewsId=" + maxId);
                }
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Blog added successfully...!!');", true);
            }
            else
            {
                c.ExecuteQuery("Update NewsData Set NewsDate='" + nDate + "', NewsTitle='" + txtTitle.Text + 
                    "', NewsDesc='" + txtDesc.Text + "' Where NewsId=" + maxId);

                if (fuImg.HasFile)
                {
                    c.ExecuteQuery("Update NewsData Set NewsImage='" + fileName + "' Where NewsId=" + maxId);
                }
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Blog updated successfully...!!');", true);
            }
            txtNDate.Text = txtTitle.Text = txtDesc.Text = "";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('blogs-master.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }

    private void ImageUploadProcess(string imgName)
    {
        try
        {
            string origImgPath = "~/upload/news/original/";
            string thumbImgPath = "~/upload/news/thumb/";
            string normalImgPath = "~/upload/news/";

            fuImg.SaveAs(Server.MapPath(origImgPath) + imgName);
            c.ImageOptimizer(imgName, origImgPath, normalImgPath, 860, true);
            c.ImageOptimizer(imgName, normalImgPath, thumbImgPath, 280, true);
            File.Delete(Server.MapPath(origImgPath) + imgName);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ImageUploadProcess", ex.Message.ToString());
            return;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            c.ExecuteQuery("Delete From NewsData Where NewsId=" + Request.QueryString["id"]);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Blog Deleted');", true);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('blogs-master.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnDelete_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("blogs-master.aspx", false);
    }

    private void GetBlogsData(int nIdX)
    {
        try
        {
            using (DataTable dtNws = c.GetDataTable("Select * From NewsData Where NewsId=" + nIdX))
            {
                if (dtNws.Rows.Count > 0)
                {
                    lblId.Text = nIdX.ToString();
                    DataRow row = dtNws.Rows[0];

                    txtNDate.Text = Convert.ToDateTime(row["NewsDate"]).ToString("dd/MM/yyyy");
                    txtTitle.Text = row["NewsTitle"].ToString();
                    txtDesc.Text = row["NewsDesc"].ToString();

                    if (row["NewsImage"] != DBNull.Value && row["NewsImage"] != null && row["NewsImage"].ToString() != "")
                    {
                        blogImg = "<img src=\"" + Master.rootPath + "upload/news/thumb/" + row["NewsImage"].ToString() + "\" width=\"200\" />";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetBlogsData", ex.Message.ToString());
            return;
        }
    }
}