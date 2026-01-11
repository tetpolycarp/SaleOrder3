using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace Mitchell.ScmConsoles
{
    public partial class DebugInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Trace.Write("I'm in Tracing");
            Debug.WriteLine("I'm in Debug");
            if(!IsPostBack)
            {
                Literal_logs.Text += string.Format("{0}: Tfs_ConfigurationConnection: {1} <br />", DateTime.Now.ToString(), ConfigurationManager.ConnectionStrings["Tfs_ConfigurationConnection"].ConnectionString);
                Literal_logs.Text += string.Format("{0}: Tfs_MitchellProjectsConnection: {1}<br />", DateTime.Now.ToString(), ConfigurationManager.ConnectionStrings["Tfs_MitchellProjectsConnection"].ConnectionString);
                Literal_logs.Text += string.Format("{0}: Deployment_CDBConnection: {1}<br />", DateTime.Now.ToString(), ConfigurationManager.ConnectionStrings["Deployment_CDBConnection"].ConnectionString);
                
                //Display Web Browser Info. Testing
                Literal_logs.Text += string.Format("{0}: Current Browser: {1}<br />", DateTime.Now.ToString(), WebBrowserInfo());

                string localfile = @"C:\temp\chau.ics";
                string remoteUri = "https://confluence.corp.int/rest/calendar-services/1.0/calendar/export/subcalendar/private/f4c43be7686a001fbd070578d48e784d5498c43b.ics";
                // Create a new WebClient instance.
                WebClient myWebClient = new WebClient();
                // Concatenate the domain with the Web resource filename.
                Console.WriteLine("Downloading File \"{0}\" from \"{1}\" .......\n\n", localfile, remoteUri);
                // Download the Web resource and save it into the current filesystem folder.
                myWebClient.DownloadFile(remoteUri, localfile);


                FileStream fs = new FileStream(localfile, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                string ical = sr.ReadToEnd();
                char[] delim = { '\n' };
                string[] lines = ical.Split(delim);
                delim[0] = ':';
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("BEGIN:VEVENT"))
                    {
                        string[] eventData = new string[9];
                        for (int j = 0; j < 9; j++)
                            eventData[j] = lines[i + j + 1].Split(delim)[1];
                        string strDate = eventData[0].ToString();
                        strDate = strDate.Replace("\r", "");

                        string format;
                        DateTime result;
                     }
                }
                sr.Close();
            }
        }
        private string WebBrowserInfo()
        {
            System.Web.HttpBrowserCapabilities browser = Request.Browser;
            string s = "Browser Capabilities\n"
                + "Type = " + browser.Type + "\n"
                + "Name = " + browser.Browser + "\n"
                + "Version = " + browser.Version + "\n"
                + "Major Version = " + browser.MajorVersion + "\n"
                + "Minor Version = " + browser.MinorVersion + "\n"
                + "Platform = " + browser.Platform + "\n"
                + "Is Beta = " + browser.Beta + "\n"
                + "Is Crawler = " + browser.Crawler + "\n"
                + "Is AOL = " + browser.AOL + "\n"
                + "Is Win16 = " + browser.Win16 + "\n"
                + "Is Win32 = " + browser.Win32 + "\n"
                + "Supports Frames = " + browser.Frames + "\n"
                + "Supports Tables = " + browser.Tables + "\n"
                + "Supports Cookies = " + browser.Cookies + "\n"
                + "Supports VBScript = " + browser.VBScript + "\n"
                + "Supports JavaScript = " +
                    browser.EcmaScriptVersion.ToString() + "\n"
                + "Supports Java Applets = " + browser.JavaApplets + "\n"
                + "Supports ActiveX Controls = " + browser.ActiveXControls
                      + "\n"
                + "Supports JavaScript Version = " +
                    browser["JavaScriptVersion"] + "\n";

            return s;
        }
    }
}