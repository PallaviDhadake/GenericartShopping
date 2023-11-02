<%@ WebHandler Language="C#" Class="UploadFile" %>

using System;
using System.Web;
using System.IO;
using System.Web.SessionState;

public class UploadFile : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");

        string filedata = string.Empty;
        if (context.Request.Files.Count > 0)
        {
            HttpFileCollection files = context.Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile file = files[i];
                if (Path.GetExtension(file.FileName).ToLower() != ".jpg" &&
                    Path.GetExtension(file.FileName).ToLower() != ".png" &&
                    Path.GetExtension(file.FileName).ToLower() != ".gif" &&
                    Path.GetExtension(file.FileName).ToLower() != ".jpeg" &&
                    Path.GetExtension(file.FileName).ToLower() != ".pdf"
                )
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Only jpg, png , gif, .jpeg, .pdf are allowed.!");
                    return;
                }
                decimal size = Math.Round(((decimal)file.ContentLength / (decimal)1024), 2);
                if (size > 2048)
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("File size should not exceed 2 MB.!");
                    return;
                }
                string fname;
                if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE" || HttpContext.Current.Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] {  
                        '\\'  
                    });
                    fname = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = file.FileName;
                }
                //here UploadFile is define my folder name , where files will be store.  
                string uploaddir = System.Configuration.ConfigurationManager.AppSettings["upload"];
                filedata = Guid.NewGuid() + fname;
                fname = Path.Combine(context.Server.MapPath("~/" + uploaddir + "/"), filedata);
                file.SaveAs(fname);
            }
        }
        context.Response.ContentType = "text/plain";
        context.Response.Write("File Uploaded Successfully:" + filedata + "!");
        //if you want to use file path in aspx.cs page , then assign it in to session  
        //context.Session["PathImage"] = filedata;
        HttpContext.Current.Session["PathImage"] = filedata;
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}