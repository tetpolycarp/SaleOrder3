using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mitchell.ScmConsoles
{
    public partial class ErrorHandle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Exception lastError = Server.GetLastError();
            if (lastError == null)
            {
                Literal_Message.Text = "An error occurred, but no error information is available";
            }
            else
            { 
                Literal_Message.Text = lastError.ToString();
            }
        }
    }
}