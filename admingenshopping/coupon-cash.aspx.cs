using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Services;
using System.IO;

public partial class admingenshopping_coupon_cash : System.Web.UI.Page
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
                        //btnSave.Text = "Save Info";
                        pgTitle = "Add Coupon";
                        //btnDelete.Visible = false;

                    }
                    else
                    {
                        pgTitle = "Edit Coupon";
                        //btnSave.Text = "Modify Info";
                        //btnDelete.Visible = true;
                        GetCouponData(Convert.ToInt32(Request.QueryString["id"]));
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

    private void GetCouponData(int nIdX)
    {
        try
        {
            using (DataTable dtNws = c.GetDataTable("Select * From coupon Where coupon_id=" + nIdX))
            {
                if (dtNws.Rows.Count > 0)
                {
                    lblId.Text = nIdX.ToString();
                    DataRow row = dtNws.Rows[0];
                    txtCpnCode.Text = row["coupon_code"].ToString();
                    txtStartDate.Text = Convert.ToDateTime(row["coupon_effective_date"]).ToString("dd/MM/yyyy");
                    txtEndDate.Text = Convert.ToDateTime(row["coupon_expiry_date"]).ToString("dd/MM/yyyy");
                    txtCpnTitle.Text = row["coupon_head"].ToString();
                    txtCpnDesc.Text = row["coupon_body"].ToString();
                    txtCpnTC.Text = row["coupon_TC"].ToString();

                    switch(row["coupon_ref_type"].ToString())
                        {
                            case "All":
                                rdBtnAll.Checked = true;
                                break;
                            case "Product":
                                rdBtnProduct.Checked = true;
                                txtProduct.Text = c.GetReqData("ProductsData", "ProductName", "ProductID=" + Convert.ToInt32(row["coupon_ref_id"].ToString())).ToString();
                                break;
                            case "Category":
                                rdBtnCategory.Checked = true;
                                txtProduct.Text = c.GetReqData("ProductCategory", "ProductCatName", "ProductCatID=" + Convert.ToInt32(row["coupon_ref_id"].ToString())).ToString();
                                break;
                            default: 
                                
                                break;
                        }

                    txtPercentage.Text = row["coupon_per"].ToString();
                    txtMinPurchase.Text = row["coupon_min_amt"].ToString();
                    txtMaxDiscount.Text = row["coupon_max_amt"].ToString();


                   

                    //if (row["NewsImage"] != DBNull.Value && row["NewsImage"] != null && row["NewsImage"].ToString() != "")
                    //{
                    //    blogImg = "<img src=\"" + Master.rootPath + "upload/news/thumb/" + row["NewsImage"].ToString() + "\" width=\"200\" />";
                    //}
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

    private void FillGrid()
    {
        try
        {
            using (DataTable dtBlog = c.GetDataTable("Select coupon_id, Convert(varchar(20), coupon_effective_date, 103) as cpnDate, coupon_code, coupon_head From coupon Order By coupon_id DESC"))
            {
                gvCoupon.DataSource = dtBlog;
                gvCoupon.DataBind();

                if (dtBlog.Rows.Count > 0)
                {
                    gvCoupon.UseAccessibleHeader = true;
                    gvCoupon.HeaderRow.TableSection = TableRowSection.TableHeader;
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
    protected void gvCoupon_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"coupon-cash.aspx?action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\" title=\"View/Edit\"></a>";
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvBlogs_RowDataBound", ex.Message.ToString());
            return;
        }
    }
    protected void gvCoupon_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCoupon.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("coupon-cash.aspx", false);
    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    public static List<ListItem> GetProducts()
    {
        string query = "SELECT CustomerId, Name FROM Customers";
        string constr = ConfigurationManager.ConnectionStrings["GenCartDATA"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                List<ListItem> customers = new List<ListItem>();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                con.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        customers.Add(new ListItem
                        {
                            Value = sdr["CustomerId"].ToString(),
                            Text = sdr["Name"].ToString()
                        });
                    }
                }
                con.Close();
                return customers;
            }
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static List<ListItem> FillDropdownList(string sqlQuery, string displayField, string valueField)
    {
        string query = sqlQuery;
        string constr = ConfigurationManager.ConnectionStrings["GenCartDATA"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                List<ListItem> customers = new List<ListItem>();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                con.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        customers.Add(new ListItem
                        {
                            Value = sdr[valueField].ToString(),
                            Text = sdr[displayField].ToString()
                        });
                    }
                }
                con.Close();
                return customers;
            }
        }
    }


    [System.Web.Services.WebMethod()]
    public static List<string> GetSearchControl(string srchType, string prefix)
    {
        iClass c = new iClass();
        List<string> customers = new List<string>();
        string sqlQuery = "";

        if (srchType == "Product")
        {
            sqlQuery = "SELECT ProductID, ProductName as pName FROM ProductsData WHERE ProductName like '" + prefix + "%' and delMark=0 AND isnull(ProductActive, 0) = 1";
        }
        else if (srchType == "Category")
        {
            sqlQuery = "Select ProductCatID, ProductCatName as pName From ProductCategory Where ProductCatName like '" + prefix + "%' AND ParentCatID<>0 AND ChildCatFlag<>1 AND DelMark=0 Order By ProductCatName";
        }


        using (DataTable dtProd = c.GetDataTable(sqlQuery))
        {
            if (dtProd.Rows.Count > 0)
            {
                foreach (DataRow row in dtProd.Rows)
                {
                    customers.Add(string.Format("{0}", row["pName"]));
                }
            }
            else
            {
                customers.Add("Match Not Found");
            }
        }
        return customers;
    }

    //[System.Web.Services.WebMethod(EnableSession = true)]
    //public static Boolean FillDropdownList(DropDownList ddrName)
    //{
    //    iClass c = new iClass();
    //    //Page page = (Page)HttpContext.Current.Handler;
    //    //DropDownList ddrProduct = (DropDownList)page.FindControl(ddrName);

    //    c.FillComboBox("imageName", "bannerId", "BannerData", "", "imageName", 0, ddrName);

    //    return true;
        
    //}

    [System.Web.Services.WebMethod()]
    public static string intellectsys(string myPara)
    {
        return myPara;
    }


    [WebMethod]
    public static string SaveCouponData(coupon cinfo)
    {
        iClass c = new iClass();
        
        // Return Error Code types
        // 1: Successful record entry
        // 2: Duplicate Coupon code
        // 3: Invalid Product / Category name entered

        // =========== Validations ===========
        // Coupon Code duplication
        if (cinfo.CouponID == "[New]")
        {
            if (c.IsRecordExist("Select coupon_id from coupon Where coupon_code='" + cinfo.CouponCode + "'") == true)
            {
                return "2";
            }
        }
        else
        {
            if (c.IsRecordExist("Select coupon_id from coupon Where coupon_code='" + cinfo.CouponCode + "' And coupon_id <>" + Convert.ToInt32(cinfo.CouponID)) == true)
            {
                return "2";
            }
        }
        

        // Invalid Product / Category valdation
        object refIdInfo;
        if (cinfo.CouponRefType == "Category")
        {
            refIdInfo = c.GetReqData("ProductCategory", "ProductCatID", "ProductCatName='" + cinfo.CouponProductName + "'");

            if(refIdInfo!= DBNull.Value && refIdInfo!= null && refIdInfo!= "")
            {
                cinfo.CouponRefId = Convert.ToInt32(refIdInfo);
            }
            else
            {
                return "3";
            }
        }
        if (cinfo.CouponRefType == "Product")
        {
            refIdInfo = c.GetReqData("ProductsData", "ProductID", "ProductName='" + cinfo.CouponProductName + "'");

            if(refIdInfo!= DBNull.Value && refIdInfo!= null && refIdInfo!= "")
            {
                cinfo.CouponRefId = Convert.ToInt32(refIdInfo);
            }
            else
            {
                return "3";
            }
        }

        //var result = x > y ? "x is greater than y" : "x is less than y";        
        int maxId = cinfo.CouponID == "[New]" ? c.NextId("coupon", "coupon_id") : Convert.ToInt32(cinfo.CouponID);

        string[] arrStartDate = cinfo.CouponStartDate.Split('/');
        DateTime StartDate = Convert.ToDateTime(arrStartDate[1] + "/" + arrStartDate[0] + "/" + arrStartDate[2]);

        string[] arrEndDate = cinfo.CouponEndDate.Split('/');
        DateTime EndDate = Convert.ToDateTime(arrEndDate[1] + "/" + arrEndDate[0] + "/" + arrEndDate[2]);


        //txtNDate.Text = txtNDate.Text.Trim().Replace("'", "");
        cinfo.CouponImg = cinfo.CouponImg.Trim().Replace("'", "");
        cinfo.CouponInfo = cinfo.CouponInfo.Trim().Replace("'", "");
        cinfo.CouponTerms = cinfo.CouponTerms.Trim().Replace("'", "");

        string constr = ConfigurationManager.ConnectionStrings["GenCartDATA"].ConnectionString;


        using (SqlConnection con = new SqlConnection(constr))
        {

            if (cinfo.CouponID == "[New]")
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO coupon (coupon_id, coupon_image, coupon_head, coupon_body, coupon_TC, coupon_code, coupon_type, coupon_ref_type, coupon_ref_id, coupon_product_id, coupon_product_qty, coupon_per, coupon_cmp_value, coupon_min_amt, coupon_max_amt, coupon_max_allow, coupon_used_amount, coupon_effective_date, coupon_expiry_date, coupon_display) VALUES(@coupon_id, @coupon_image, @coupon_head, @coupon_body, @coupon_TC, @coupon_code, @coupon_type, @coupon_ref_type, @coupon_ref_id, @coupon_product_id, @coupon_product_qty, @coupon_per, @coupon_cmp_value, @coupon_min_amt, @coupon_max_amt, @coupon_max_allow, @coupon_used_amount, @coupon_effective_date, @coupon_expiry_date, @coupon_display)"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@coupon_id", maxId);
                    cmd.Parameters.AddWithValue("@coupon_image", cinfo.CouponImg);
                    cmd.Parameters.AddWithValue("@coupon_head", cinfo.CouponTitle);
                    cmd.Parameters.AddWithValue("@coupon_body", cinfo.CouponInfo);
                    cmd.Parameters.AddWithValue("@coupon_TC", cinfo.CouponTerms);
                    cmd.Parameters.AddWithValue("@coupon_code", cinfo.CouponCode);
                    cmd.Parameters.AddWithValue("@coupon_type", cinfo.CouponType);
                    cmd.Parameters.AddWithValue("@coupon_ref_type", cinfo.CouponRefType);
                    cmd.Parameters.AddWithValue("@coupon_ref_id", cinfo.CouponRefId);
                    cmd.Parameters.AddWithValue("@coupon_product_id", cinfo.CouponProductId);
                    cmd.Parameters.AddWithValue("@coupon_product_qty", cinfo.CouponProductQty);
                    cmd.Parameters.AddWithValue("@coupon_per", cinfo.CouponPercentage);
                    cmd.Parameters.AddWithValue("@coupon_cmp_value", cinfo.CouponCompareVal);
                    cmd.Parameters.AddWithValue("@coupon_min_amt", cinfo.CouponMinAmount);
                    cmd.Parameters.AddWithValue("@coupon_max_amt", cinfo.CouponMaxAmount);
                    cmd.Parameters.AddWithValue("@coupon_max_allow", cinfo.CouponMaxAllow);
                    cmd.Parameters.AddWithValue("@coupon_used_amount", cinfo.CouponUsedAmount);
                    cmd.Parameters.AddWithValue("@coupon_effective_date", StartDate);
                    cmd.Parameters.AddWithValue("@coupon_expiry_date", EndDate);
                    cmd.Parameters.AddWithValue("@coupon_display", cinfo.CouponDisplay);

                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

            }
            else
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE coupon SET coupon_head = @coupon_head, coupon_body = @coupon_body, coupon_TC = @coupon_TC, coupon_code = @coupon_code, coupon_type = @coupon_type, coupon_ref_type = @coupon_ref_type, coupon_ref_id = @coupon_ref_id, coupon_product_id = @coupon_product_id, coupon_product_qty = @coupon_product_qty, coupon_per = @coupon_per, coupon_cmp_value = @coupon_cmp_value, coupon_min_amt = @coupon_min_amt, coupon_max_amt = @coupon_max_amt, coupon_max_allow = @coupon_max_allow, coupon_used_amount = @coupon_used_amount, coupon_effective_date = @coupon_effective_date, coupon_expiry_date = @coupon_expiry_date, coupon_display = @coupon_display  WHERE coupon_id = @coupon_id", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@coupon_id", maxId);
                    //cmd.Parameters.AddWithValue("@coupon_image", cinfo.CouponImg);
                    cmd.Parameters.AddWithValue("@coupon_head", cinfo.CouponTitle);
                    cmd.Parameters.AddWithValue("@coupon_body", cinfo.CouponInfo);
                    cmd.Parameters.AddWithValue("@coupon_TC", cinfo.CouponTerms);
                    cmd.Parameters.AddWithValue("@coupon_code", cinfo.CouponCode);
                    cmd.Parameters.AddWithValue("@coupon_type", cinfo.CouponType);
                    cmd.Parameters.AddWithValue("@coupon_ref_type", cinfo.CouponRefType);
                    cmd.Parameters.AddWithValue("@coupon_ref_id", cinfo.CouponRefId);
                    cmd.Parameters.AddWithValue("@coupon_product_id", cinfo.CouponProductId);
                    cmd.Parameters.AddWithValue("@coupon_product_qty", cinfo.CouponProductQty);
                    cmd.Parameters.AddWithValue("@coupon_per", cinfo.CouponPercentage);
                    cmd.Parameters.AddWithValue("@coupon_cmp_value", cinfo.CouponCompareVal);
                    cmd.Parameters.AddWithValue("@coupon_min_amt", cinfo.CouponMinAmount);
                    cmd.Parameters.AddWithValue("@coupon_max_amt", cinfo.CouponMaxAmount);
                    cmd.Parameters.AddWithValue("@coupon_max_allow", cinfo.CouponMaxAllow);
                    cmd.Parameters.AddWithValue("@coupon_used_amount", cinfo.CouponUsedAmount);
                    cmd.Parameters.AddWithValue("@coupon_effective_date", StartDate);
                    cmd.Parameters.AddWithValue("@coupon_expiry_date", EndDate);
                    cmd.Parameters.AddWithValue("@coupon_display", cinfo.CouponDisplay);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();
                }

                if (cinfo.CouponImg != "noimage.jpg")
                using (SqlCommand cmd = new SqlCommand("UPDATE coupon SET coupon_image = @coupon_image WHERE coupon_id = @coupon_id", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@coupon_id", maxId);
                    cmd.Parameters.AddWithValue("@coupon_image", cinfo.CouponImg);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();
                }
                


            }
           
        }



        return "1"; ;
        
    }


    [WebMethod]
    public static string SaveAsCouponImage(string couponNameX)
    {
        HttpContext context = HttpContext.Current;
        iClass c = new iClass();
        string imgName = context.Session["PathImage"].ToString();


        string origImgPath = System.Web.HttpContext.Current.Server.MapPath("~/upload/" + imgName);
        string normalImgPath = System.Web.HttpContext.Current.Server.MapPath("~/upload/coupon/" + imgName);


        System.IO.File.Copy(origImgPath, normalImgPath, true);
        
        // Update Coupon name to table
        c.ExecuteQuery("Update coupon set coupon_image='" + imgName + "' Where coupon_code='" + couponNameX + "'");
        
        
        //Delete rew image from server
//            File.Delete(context.Server.MapPath(origImgPath));

        //Insert data into Database
        //int maxId = c.NextId("ProductPhotos", "ProductPhotoID");
        //c.ExecuteQuery("Insert into ProductPhotos(ProductPhotoID, FK_ProductID, PhotoName, DefaultFlag) Values(" + maxId + ", " + productIDX +
        //", '" + imgName + "', 0) ");

        //GetAlbumPhotos(productIDX);
        return "Image uploaded!";

    }


    [System.Web.Services.WebMethod()]
    public static string CouponDemo(coupon cinfo)
    {
        iClass c = new iClass();
        int maxId = c.NextId("coupon", "coupon_id");

        return "Hello India";
        
    }
}