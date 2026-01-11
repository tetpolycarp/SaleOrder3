<%@ Page Title="Page Not Found Handle" Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" Inherits="Mitchell.ScmConsoles.PageNotFoundHandle" Codebehind="PageNotFoundHandle.aspx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>




   <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
         <table style="width: 100%;">
                   <tr>
                   
                            <td align="center" style="vertical-align:top; width: 300px;">
                             <asp:Image ID="Image2" runat="server" Height="115px" ImageUrl="~/images/PageNotFound.png" Width="142px" />
                                </td>
                           <td>
                           <table style="width: 100%;">
                               <tr>
                                   <td style="text-align:left; vertical-align:bottom; font-size: x-large;" class="auto-style1"><strong>Page Not Found.</strong></td>
                               </tr>
                               <tr>
                                   <asp:Literal ID="Literal_Message" runat="server"></asp:Literal>
                               </tr>
                           </table>
                         </td>
                        
                   </tr>
             </table>
  
  </asp:Content>
