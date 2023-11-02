using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_add_team_master : System.Web.UI.Page
{
    iClass c = new iClass();
    public string pgTitle, pageHeadName;
    protected void Page_Load(object sender, EventArgs e)
    {
        pgTitle = Request.QueryString["action"] == "new" ? "Add New Member" : "Edit Member";
        if (!IsPostBack)
        {
            //pgTitle = Request.QueryString["action"] == "new" ? "Add New Member" : "Edit Member";

            if (Request.QueryString["action"] != null)
            {
                editTeam.Visible = true;
                viewTeam.Visible = false;
                if (Request.QueryString["action"] == "new")
                {
                    btnSave.Text = "Save Info";
                    btnDelete.Visible = false;
                    btnActive.Visible = false;
                    btnBlock.Visible = false;
                    GetUserName();
                }
                else
                {
                    btnSave.Text = "Modify Info";
                    btnDelete.Visible = true;
                    GetTeamData(Convert.ToInt32(Request.QueryString["id"]));
                    ButtonsVisibility();
                }
            }
            else
            {
                editTeam.Visible = false;
                viewTeam.Visible = true;
                FillGrid();
            }
        }
    }
    private void GetUserName()
    {
        try
        {
            int id = c.NextId("SupportTeam", "TeamID");
            string username = "";
            if (id >= 100)
                username = "GMPLCS" + id;
            else if (id >= 10)
                username = "GMPLCS0" + id;
            else
                username = "GMPLCS00" + id;

            txtUserName.Text = username;
            
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetUserName", ex.Message.ToString());
            return;
        }
    }
    private void ButtonsVisibility()
    {
        try
        {
            int userStatus = Convert.ToInt32(c.GetReqData("SupportTeam", "TeamUserStatus", "TeamID=" + Convert.ToInt32(Request.QueryString["id"]) + ""));
            if (userStatus == 0)
            {
                btnActive.Visible = false;
                btnBlock.Visible = true;
            }
            else
            {
                btnActive.Visible = true;
                btnBlock.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ButtonsVisibility", ex.Message.ToString());
            return;
        }
    }
    private void FillGrid()
    {
        try
        {
            string strQuery = "";
            if (Request.QueryString["type"] != null)
            {
                switch (Request.QueryString["type"])
                {
                    case "all":
                        strQuery = "Select TeamID, TeamUserID, isnull(TeamTaskID, 0) as TeamTaskID, convert(varchar(20), TeamRegDate, 103) as TeamRegDate, TeamPersonName, TeamUserStatus From SupportTeam Where TeamAuthority=2 And TeamUserStatus<>2";
                        pageHeadName = "All Staff";
                        break;
                    case "active":
                        strQuery = "Select TeamID, TeamUserID, isnull(TeamTaskID, 0) as TeamTaskID, convert(varchar(20), TeamRegDate, 103) as TeamRegDate, TeamPersonName, TeamUserStatus From SupportTeam Where TeamAuthority=2 And TeamUserStatus=0";
                        pageHeadName = "Active Staff";
                        break;
                    case "blocked":
                        strQuery = "Select TeamID, TeamUserID, isnull(TeamTaskID, 0) as TeamTaskID, convert(varchar(20), TeamRegDate, 103) as TeamRegDate, TeamPersonName, TeamUserStatus From SupportTeam Where TeamAuthority=2 And TeamUserStatus=1";
                        pageHeadName = "Blocked Staff";
                        break;
                    
                }
            }

            using (DataTable dtPackege = c.GetDataTable(strQuery))
            {

                gvTeam.DataSource = dtPackege;
                gvTeam.DataBind();
                if (gvTeam.Rows.Count > 0)
                {
                    gvTeam.UseAccessibleHeader = true;
                    gvTeam.HeaderRow.TableSection = TableRowSection.TableHeader;
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
    protected void gvTeam_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"add-team-master.aspx?action=edit&id=" + e.Row.Cells[0].Text + "&type=" + Request.QueryString["type"] + "\" class=\"gAnch\" title=\"View / Edit\" target=\"_blank\"></a>";

                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                switch (e.Row.Cells[1].Text)
                {
                    case "0":
                        litStatus.Text = "<div class=\"stsActive\">Active</div>";
                        break;
                    case "1":
                        litStatus.Text = "<div class=\"stsBlocked\">Blocked</div>";
                        break;
                }

                Literal litTask = (Literal)e.Row.FindControl("litTask");
                switch (e.Row.Cells[2].Text)
                {
                    case "0":
                        litTask.Text = "<div>NA</div>";
                        break;
                    case "1":
                        litTask.Text = "<div>Registered customer list</div>";
                        break;
                    case "2":
                        litTask.Text = "<div>Delivered order list</div>";
                        break;
                    case "3":
                        litTask.Text = "<div>All order list</div>";
                        break;
                    case "4":
                        litTask.Text = "<div>Lab appointment list</div>";
                        break;
                    case "5":
                        litTask.Text = "<div>Doctor appointment list</div>";
                        break;
                    case "6":
                        litTask.Text = "<div>Prescription request list</div>";
                        break;
                    case "7":
                        litTask.Text = "<div>Purchase Department</div>";
                        break;
                    case "8":
                        litTask.Text = "<div>Company Owned Shop Orders</div>";
                        break;
                    case "9":
                        litTask.Text = "<div>Trainee</div>";
                        break;
                    case "10":
                        litTask.Text = "<div>Online Payment Settlement</div>";
                        break;

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvTeam_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtName.Text == "" || txtMobile.Text == "" || ddrTask.SelectedIndex == 0 )
            {
                pgTitle = Request.QueryString["action"] == "new" ? "Add New Member" : "Edit Member";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * fields are mandetory');", true);
                return;
            }
            // Insert / Update data into database 
            int maxId = lblId.Text == "[New]" ? c.NextId("SupportTeam", "TeamID") : Convert.ToInt16(lblId.Text);

            DateTime cDate = DateTime.Now;
            string currentDate = cDate.ToString("yyyy-MM-dd HH:mm:ss.fff");

            if (lblId.Text == "[New]")
            {
                string username, password;
                username = "GMPLCS00" + maxId;
                password = "123456";
                
                c.ExecuteQuery("Insert Into SupportTeam(TeamID, TeamRegDate, TeamUserID, TeamPassword, TeamPersonName, TeamMobile, TeamAuthority, TeamTaskID, TeamUserStatus)Values("+ maxId +", '"+ currentDate + "', '"+ username +"', '"+ password +"', '"+ txtName.Text +"', '"+ txtMobile.Text +"', 2, "+ ddrTask.SelectedValue +", 0)");
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Team Member Info Added');", true);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('add-team-master.aspx?action=new', 2000);", true);
                txtName.Focus();
            }
            else
            {
                c.ExecuteQuery("Update SupportTeam Set TeamPersonName='" + txtName.Text + "', TeamMobile='" + txtMobile.Text + "', TeamTaskID=" + ddrTask.SelectedValue + " Where TeamID=" + maxId);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Team Member Info Updated');", true);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('add-team-master.aspx?type=all', 2000);", true);
            }
           
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }


    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            
            c.ExecuteQuery("Update SupportTeam Set TeamUserStatus=2 Where TeamID=" + Request.QueryString["id"]);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Team Member Deleted successfully.');", true);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('add-team-master.aspx?type=all', 2000);", true);
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
        if (Request.QueryString["type"] != null)
        {
            Response.Redirect("add-team-master.aspx?type=" + Request.QueryString["type"], false);
        }
        else
        {
            Response.Redirect("dashboard.aspx", false);
        }
    }

    private void GetTeamData(int teamIdx)
    {
        try
        {
            using (DataTable dtTeam = c.GetDataTable("Select TeamPersonName, TeamMobile, TeamTaskID, TeamUserID, TeamPassword From SupportTeam Where TeamID=" + teamIdx))
            {
                if (dtTeam.Rows.Count>0)
                {
                    DataRow bRow = dtTeam.Rows[0];
                    lblId.Text = teamIdx.ToString();

                    txtName.Text = bRow["TeamPersonName"].ToString();
                    txtMobile.Text = bRow["TeamMobile"].ToString();
                    txtUserName.Text = bRow["TeamUserID"].ToString();
                    txtPassword.Text = bRow["TeamPassword"].ToString();
                    ddrTask.SelectedValue = bRow["TeamTaskID"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetTeamData", ex.Message.ToString());
            return;
        }
    }

    protected void btnBlock_Click(object sender, EventArgs e)
    {
        try
        {
            c.ExecuteQuery("Update SupportTeam Set TeamUserStatus=1 Where TeamID=" + Request.QueryString["id"]);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Team Member Blocked successfully.');", true);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('add-team-master.aspx?type=active', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnBlock_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnActive_Click(object sender, EventArgs e)
    {
        try
        {
            c.ExecuteQuery("Update SupportTeam Set TeamUserStatus=0 Where TeamID=" + Request.QueryString["id"]);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Team Member Activated successfully.');", true);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('add-team-master.aspx?type=blocked', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnActive_Click", ex.Message.ToString());
            return;
        }
    }
}