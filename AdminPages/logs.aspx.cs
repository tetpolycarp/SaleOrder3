using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Mitchell.ScmConsoles.AdminPages
{
    public partial class logs : System.Web.UI.Page
    {
        string LOG_FILE = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["servername"] == null || Request.QueryString["logname"] == null || Request.QueryString["logfilename"] == null)
            {
                return;
            }
            else
            {
                LabelTitle.Text = Request.QueryString["logname"];
                //@"\\papp270ntv\d$\Prod\LogFiles_ServiceApps\JiraWebHooks_CDBGatewayService_Log.txt";
                LOG_FILE = string.Format(@"\\{0}\d$\Prod\LogFiles_ServiceApps\{1}", Request.QueryString["servername"], Request.QueryString["logfilename"]);
                LabelContent.Text = "Read from log file at " + LOG_FILE;
                if (File.Exists(LOG_FILE))
                {
                    string text = File.ReadAllText(LOG_FILE);
                    //should be the same for all the logs. Inject the new line when seeing "*log*"
                    text = text.Replace("*log*", "<br/>*log*");

                    //Depend on each logs, we want to make some text to blue so it can indicate whenever there function get call
                    //for JIRA Deployment Schedule webhook service
                    text = text.Replace("====> Receive signal from Jira", "<font color=\"blue\"><strong>====> Receive signal from Jira</strong></font>");
                    text = text.Replace("Jira Issue Type:", "<font color=\"blue\">Jira Issue Type:</font>");

                    //For JIRA Deployment Scheduler
                    text = text.Replace("====> Start the application by calling Main()", "<font color=\"blue\"><strong>====> Start the application by calling Main()</strong></font>");
                    text = text.Replace("JiraId", "<font color=\"blue\">JiraId</font>");
                    text = text.Replace(">>>", "<font color=\"green\"><strong>>>></strong></font>");

                    //CDB service
                    text = text.Replace("Call GetServers()", "<font color=\"blue\"><strong>Call GetServers()</strong></font>");
                   


                    //should be the same for all the log, whenever there is another error, make it into RED
                   // text = text.Replace("WARNING", "<font color=\"brown\"><strong>WARNING</strong></font>");
                    text = text.Replace("INFO", "<font color=\"purple\"><strong>INFO</strong></font>");
                    text = text.Replace("ERROR", "<font color=\"red\"><strong>ERROR</strong></font>");
                    text = text.Replace("Error", "<font color=\"red\"><strong>Error</strong></font>");

                    LabelContent.Text += text;
                }
                else
                    LabelContent.Text = "Log file not found";
            }
        

        }

        protected void ButtonClearText_Click(object sender, EventArgs e)
        {
            File.WriteAllText(LOG_FILE, "");
        }

        protected void ButtonRefresh_Click(object sender, EventArgs e)
        {

        }
    }
}