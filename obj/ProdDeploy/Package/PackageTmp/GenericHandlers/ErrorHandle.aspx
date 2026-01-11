<%@ Page Title="Error Handle" Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" Inherits="Mitchell.ScmConsoles.ErrorHandle" Codebehind="ErrorHandle.aspx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>




   <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
         <table style="width: 100%;">
                   <tr>
                   
                            <td align="center" style="vertical-align:top; width: 322px;">
                             <asp:Image ID="Image2" runat="server" Height="91px" ImageUrl="~/images/error.png" Width="94px" />
                                </td>
                           <td>
                           <table class="nav-justified">
                               
                               <tr>
                                   <td style="text-align:left; vertical-align:bottom; font-size: x-large;" class="auto-style1"><strong>Error Found.</strong></td>
                               </tr>
                               <tr>
                                   <td><asp:Literal ID="Literal_Message" runat="server"></asp:Literal></td>
                               </tr>
                           </table>
                         </td>
                        
                   </tr>
             </table>
  
  </asp:Content>