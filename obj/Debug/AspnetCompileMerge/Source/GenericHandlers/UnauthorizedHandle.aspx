<%@ Page Title="Unauthorized Handle" Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" Inherits="Mitchell.ScmConsoles.UnauthorizedHandle" Codebehind="UnauthorizedHandle.aspx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>




   <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
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
                                   <td>You are not member of <strong>Admin</strong>.</td>
                                   <asp:Literal ID="Literal_Message" runat="server"></asp:Literal>
                               </tr>
                           </table>
                         </td>
                        
                   </tr>
             </table>
  
  </asp:Content>
