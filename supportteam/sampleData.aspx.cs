using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_sampleData : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public class People
    {
        public int CustomrtID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
       
    }

    public class DataTableResponse
    {
        public int draw;
        public int recordsTotal;
        public int recordsFiltered;
        public List<People> data;
    }

    [WebMethod]
    public void GetDataForDataTable()
    {
        HttpContext context = HttpContext.Current;
        context.Response.ContentType = "text/plain";
        //List of Column shown in the Table (user for finding the name of column on Sorting)
        List<string> columns = new List<string>();
        columns.Add("CustomerName");
        columns.Add("CustomerMobile");
       

        //This is used by DataTables to ensure that the Ajax returns from server-side processing requests are drawn in sequence by DataTables
        Int32 ajaxDraw = Convert.ToInt32(context.Request.Form["draw"]);
        //OffsetValue
        Int32 OffsetValue = Convert.ToInt32(context.Request.Form["start"]);
        //No of Records shown per page
        Int32 PagingSize = Convert.ToInt32(context.Request.Form["length"]);
        //Getting value from the seatch TextBox
        string searchby = context.Request.Form["search[value]"];
        //Index of the Column on which Sorting needs to perform
        string sortColumn = context.Request.Form["order[0][column]"];
        //Finding the column name from the list based upon the column Index
        sortColumn = columns[Convert.ToInt32(sortColumn)];
        //Sorting Direction
        string sortDirection = context.Request.Form["order[0][dir]"];

        //Get the Data from the Database
        //DBLayer objDBLayer = new DBLayer();
        //DataTable dt = objDBLayer.GetData(sortColumn, sortDirection, OffsetValue, PagingSize, searchby);

        iClass c = new iClass();
        DataTable dt = c.GetData(sortColumn, sortDirection, OffsetValue, PagingSize, searchby);

        Int32 recordTotal = 0;

        List<People> peoples = new List<People>();

        //Binding the Data from datatable to the List
        if (dt != null)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                People people = new People();
                people.CustomrtID = Convert.IsDBNull(dt.Rows[i]["CustomrtID"]) ? default(int) : Convert.ToInt32(dt.Rows[i]["CustomrtID"]);
                people.CustomerName = Convert.IsDBNull(dt.Rows[i]["CustomerName"]) ? default(string) : Convert.ToString(dt.Rows[i]["CustomerName"]);
                people.CustomerMobile = Convert.IsDBNull(dt.Rows[i]["CustomerMobile"]) ? default(string) : Convert.ToString(dt.Rows[i]["CustomerMobile"]);
               
                peoples.Add(people);
            }
            recordTotal = dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["FilterTotalCount"]) : 0;
        }

        Int32 recordFiltered = recordTotal;

        DataTableResponse objDataTableResponse = new DataTableResponse()
        {
            draw = ajaxDraw,
            recordsFiltered = recordTotal,
            recordsTotal = recordTotal,
            data = peoples
        };
        //writing the response
        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(objDataTableResponse));

    }
}