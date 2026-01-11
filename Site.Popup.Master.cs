using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SaintPolycarp.BanhChung.SharedMethods;
using SaintPolycarp.BanhChung.SharedConstansts;

namespace Mitchell.ScmConsoles
{
    public partial class Popup : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(SharedMethod.GetUser()))
                {
                    LabelUsername.Text = "Logon as " + SharedMethod.GetUser();
                }
            }
            //write to log
            string currentPage = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                SharedMethod.WriteLog("Anonymous loads this page: " + currentPage, "Mobile Master", SharedConstants.ACTIVITY_LOGS_FILE);
            }
            else
            {
                SharedMethod.WriteLog(HttpContext.Current.User.Identity.Name + " loads this page: " + currentPage, "Mobile Master", SharedConstants.ACTIVITY_LOGS_FILE);
            }
        }
    
    }
}