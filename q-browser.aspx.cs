using System;

public partial class q_browser : System.Web.UI.Page
{
    public MasterClass c = new MasterClass();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void cmdSubmit_Click(object sender, EventArgs e)
    {
        if (txtQuery.Text == "" || txtPassword.Text == "")
        {
            Response.Write("Enter Query & Password");
            return;
        }
        if (txtPassword.Text.Trim() != "genShopping@2020")
        {
            Response.Write("Invalid Password");
            return;
        }
        try
        {
            c.ExecuteQuery(txtQuery.Text);
            Response.Write("Query successfully executed");
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }


    }
}
