using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using Telerik.Web.UI;
using SaintPolycarp.BanhChung.SharedMethods;
using SaintPolycarp.BanhChung.SharedConstansts;

namespace SaintPolycarp.BanhChung
{
    public partial class Logs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Trace.Write("I'm in Tracing");
            Debug.WriteLine("I'm in Debug");
            if(!IsPostBack)
            {
                //if user hasn't login, then need to ask user to do so
                if (!User.Identity.IsAuthenticated)
                {
                    string rootURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/");
                    string OriginalUrl = HttpContext.Current.Request.RawUrl;
                    string LoginPageUrl = rootURL + "Account/Login.aspx";
                    HttpContext.Current.Response.Redirect(String.Format("{0}?ReturnUrl={1}", LoginPageUrl, OriginalUrl));
                }
                else //check again, if user login but not financial admin, then reject as well.
                {
                    //if the current login user is not admin
                    if (!SharedIdentityMethod.UserBelongToRole(User.Identity.Name, "global admin"))
                    {
                        string rootURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/");
                        string OriginalUrl = HttpContext.Current.Request.RawUrl;
                        string UnthorizedPageUrl = rootURL + "GenericHandlers/UnauthorizedHandle.aspx";
                        HttpContext.Current.Response.Redirect(String.Format("{0}", UnthorizedPageUrl));
                    }
                }

                try
                {
                    //  var logs = File.ReadAllText(@"C:\temp\SaleOrderLogs.txt");
                    using (StreamReader sr = new StreamReader(SharedConstants.ACTIVITY_LOGS_FILE))
                    {
                        string logs = sr.ReadToEnd();
                        Literal1.Text = logs;
                    }
                }
                catch(Exception ex)
                {
                    Literal1.Text = ex.Message;
                }
            }
        }
     

    }
}