<%@ Page Title="Unauthorized Handle" Language="C#"  AutoEventWireup="true" Inherits="Mitchell.ScmConsoles.UnauthorizedHandleNoMaster" Codebehind="UnauthorizedHandleNoMaster.aspx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>



<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
                   <table style="width: 100%;">
                   <tr>
                   
                            <td align="center" style="vertical-align:top; width: 300px;">
                             <asp:Image ID="Image2" runat="server" Height="100px" ImageUrl="~/images/AccessDenied.jpg" Width="173px" />
                                </td>
                           <td>
                           <table style="width: 100%;">
                               <tr>
                                   <td style="text-align:left; vertical-align:bottom; font-size: x-large;" class="auto-style1"><strong>Unauthorized Permission.</strong></td>
                               </tr>
                               <tr>
                                   <td>You are not member of <strong>Software Configuration Management</strong>.</td>
                                   <asp:Literal ID="Literal_Message" runat="server"></asp:Literal>
                               </tr>
                           </table>
                         </td>
                        
                   </tr>
             </table>
    </div>
    </form>
</body>
</html>



