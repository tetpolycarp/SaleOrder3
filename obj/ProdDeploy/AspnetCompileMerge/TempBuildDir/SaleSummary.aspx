<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="SaleSummary.aspx.cs" Inherits="SaintPolycarp.BanhChung.SaleSummary" %>
<%@ MasterType VirtualPath="~/Site.master" %> 
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align:center;">
    <table style="width: 100%;">
        <tr>
            <td>
                <asp:Label ID="LabelPageTitle" runat="server" Text="Sales Summary" Font-Bold="true"  Font-Size="XX-Large" Font-Underline="true"></asp:Label> 
            </td>
        </tr>
        </table>
        </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ConsoleContent" runat="server">
      <div style="text-align:center;">
               <table style="width: 20%; align-content:center; border:solid">
                        <tr">
               
                            <td align="center" style="align-content:center;"> 
                                <asp:Button ID="Button1" runat="server" Text="Export" Width="120px" BackColor="Orange" BorderStyle="Double" BorderColor="Blue" OnClick="ButtonExport_Click"/> 

                            </td>      
                           
                        </tr>
      </table>
          <br />
        <table style="width: 100%;">
            <tr>
                 <td align="center">
                     <telerik:RadGrid ID="RadGridInventoryForecast" runat="server" ActiveItemStyle-BorderStyle="Solid" AllowPaging="false" AllowSorting="False" AllowFilteringByColumn="False" FilterType="CheckList" AutoGenerateColumns="False" OnNeedDataSource="RadGridInventoryForecast_NeedDataSource" OnPreRender="RadGridInventoryForecast_PreRender" PageSize="1000" ShowStatusBar="True" Width="98%"  Skin="Web20" OnFilterCheckListItemsRequested="RadGridInventoryForecast_FilterCheckListItemsRequested" OnItemDataBound="RadGridInventoryForecast_ItemDataBound" GroupPanelPosition="Top" OnHTMLExporting="RadGrid_HTMLExporting"  >
    <ActiveItemStyle BorderStyle="Solid" />
    <PagerStyle Mode="NumericPages" AlwaysVisible="True" />
    <MasterTableView AllowMultiColumnSorting="True" DataKeyNames="InvoiceNumber" Name="InvoiceNumber" Width="100%">
      <Columns>
        <telerik:GridBoundColumn DataField="InvoiceNumber" HeaderButtonType="TextButton" HeaderText="Inv" AllowFiltering="false" SortExpression="InvoiceNumber" FilterControlAltText="Filter Invoice Number" UniqueName="InvoiceNumber" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="5%">
          <HeaderStyle Font-Bold="True" Font-Size="Large" />
        </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="CustomerName" HeaderButtonType="TextButton" HeaderText="Customer Name" AllowFiltering="false" SortExpression="CustomerName" FilterControlAltText="Filter Customer Name" UniqueName="CustomerName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Large" />
        </telerik:GridBoundColumn>
                 <telerik:GridBoundColumn DataField="QuantityType" HeaderButtonType="TextButton" HeaderText="Quantity Type" AllowFiltering="false" SortExpression="QuantityType" FilterControlAltText="Filter Quantity Type" UniqueName="QuantityType" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Large" />
        </telerik:GridBoundColumn>
       

      </Columns>
        <PagerStyle AlwaysVisible="True" />
    </MasterTableView>
  <ClientSettings>
           <Selecting AllowRowSelect="true"></Selecting>
         
   </ClientSettings>
<FilterMenu CssClass="RadFilterMenu_CheckList"></FilterMenu>
  </telerik:RadGrid>
                </td>
            </tr>
        <tr>
              <table style="width: 100%;">
                                               <tr>
                                                  
                                           <td>    <asp:Button ID="ButtonExport" runat="server" Text="Export" Width="120px" BackColor="Orange" BorderStyle="Double" BorderColor="Blue" OnClick="ButtonExport_Click"/> &emsp; </td>
                                           
                          </td>   
                                                
                                              </tr>
                                                 </table>
        </tr>
            </table>
    </div>
</asp:Content>
