<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="logs.aspx.cs" Inherits="Mitchell.ScmConsoles.AdminPages.logs" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ConsoleContent" runat="server">
    <div>
        <table style="width: 100%;">
            <tr>
                <td style="width: 40%;">   <asp:Label ID="LabelTitle" runat="server" Font-Bold="True" Font-Overline="True" Font-Size="XX-Large" Font-Underline="True"></asp:Label>
                </td>
                <td align="left" style="width: 5%;">  <asp:Button ID="ButtonClearText" runat="server" Text="Clear Text" OnClick="ButtonClearText_Click"/></td>
                <td align="left" style="width: 5%;">  <asp:Button ID="ButtonRefresh" runat="server" Text="Refresh" OnClick="ButtonRefresh_Click"/></td>
                <td></td>
            </tr>
          
          
        </table>
       
      
        
        </div>
    
        <asp:Label ID="LabelContent" runat="server" Text=""></asp:Label>

  </asp:Content>
