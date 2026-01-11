using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Mitchell.ScmConsoles.SharedProperties;
using Telerik.Web.UI;

namespace Mitchell.ScmConsoles
{
    public partial class UnauthorizedHandleNoMaster: Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UnauthorizedMassage"] != null)
            {
                Literal_Message.Text = (string)Session["UnauthorizedMassage"];
            }
        }


    }

  
}