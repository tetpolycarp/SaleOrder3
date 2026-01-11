<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="PickupForecast.aspx.cs" Inherits="SaintPolycarp.BanhChung.PickupForecast" %>
<%@ MasterType VirtualPath="~/Site.master" %> 
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align:center;">
    <table style="width: 100%;">
        <tr>
            <td align="right">
                     <asp:Label ID="LabelPageTitle" runat="server" Text="Pickup Forecast" Font-Bold="true"  Font-Size="XX-Large" Font-Underline="true"></asp:Label> 
            </td>
              <td align="left" style="width: 50%;">
               
            </td>
        </tr>
        <tr>
            <td align="left">
                  <asp:Label ID="LabelForecastMessage" runat="server" Text="Pickup in future" Font-Size="Large"></asp:Label> 
                <br />
                    <asp:Label ID="LabelBanhChung" runat="server" Text="Bánh Chưng - 0" ForeColor="Red"  Font-Size="Medium"></asp:Label> 
                <br />
                      <asp:Label ID="LabelBanhTet" runat="server" Text=" Bánh Tét - 0"  ForeColor="Red"  Font-Size="Medium"></asp:Label> 
            </td>
            <td align="left">
               
            </td>
        </tr>
       
        </table>
        </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ConsoleContent" runat="server">
       <style type="text/css">
            .CustomHeaderForRadGridForecastBanhChung
            {
                background-color:#32a895 !important;
                background-image:none !important;
            }
            .CustomHeaderForRadGridForecastBanhTet
            {
                background-color:#325fa8 !important;
                background-image:none !important;
            }
            
             .auto-style8 {
                 height: 23px;
             }
            
             </style>  

      <div style="text-align:center;">
        <table style="width: 100%;">
           
            <tr>
                 <td align="center">
      <telerik:RadGrid ID="RadGridForecastBanhChung" runat="server" ActiveItemStyle-BorderStyle="Solid" AllowPaging="False" AllowSorting="True" AllowFilteringByColumn="false" FilterType="CheckList" AutoGenerateColumns="False" OnNeedDataSource="RadGridForecastBanhChung_NeedDataSource"  PageSize="1000" ShowStatusBar="True" Width="98%"  Skin="Sunset" GroupPanelPosition="Top" >
    <ActiveItemStyle BorderStyle="Solid" />
    <PagerStyle Mode="NumericPages" AlwaysVisible="True" />
    <HeaderStyle CssClass="CustomHeaderForRadGridForecastBanhChung" />
    <MasterTableView AllowMultiColumnSorting="True" DataKeyNames="InvoiceNumber" Name="InvoiceNumber" Width="100%">
      <Columns>
           <telerik:GridBoundColumn DataField="Link" Visible="true" HeaderButtonType="TextButton" HeaderText="" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="false"  SortExpression="Link" FilterControlAltText="Filter Link" UniqueName="Link" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="5%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="InvoiceNumber" HeaderButtonType="TextButton" HeaderText="Invoice Number" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="InvoiceNumber" FilterControlAltText="Filter Invoice Number" UniqueName="InvoiceNumber" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>
          <telerik:GridBoundColumn DataField="PackedForPickup" HeaderButtonType="TextButton" HeaderText="Packed For Pickup" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="PackedForPickup" FilterControlAltText="Filter Packed For Pickuped" UniqueName="PackedForPickup" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
            <HeaderStyle Font-Bold="True" Font-Size="Medium" />
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="CreatedBy" HeaderButtonType="TextButton" HeaderText="Created By" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="CreatedBy" FilterControlAltText="Filter Created By" UniqueName="CreatedBy" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="CustomerName" HeaderButtonType="TextButton" HeaderText="Customer Name" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="CustomerName" FilterControlAltText="Filter Customer Name" UniqueName="CustomerName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="CustomerPhone" Visible="false" HeaderButtonType="TextButton" HeaderText="Customer Phone" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="CustomerPhone" FilterControlAltText="Filter Customer Phone" UniqueName="CustomerPhone" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>
         <telerik:GridBoundColumn DataField="InvoiceType" Visible="false" HeaderButtonType="TextButton" HeaderText="Invoice Type" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="InvoiceType" FilterControlAltText="Filter Invoice Type" UniqueName="InvoiceType" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="5%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>
         <telerik:GridBoundColumn DataField="PickupDate" HeaderButtonType="TextButton" HeaderText="Pickup Date" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="PickupDate" FilterControlAltText="Filter Pickup Date" UniqueName="PickupDate" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="PickupStatus" HeaderButtonType="TextButton" HeaderText="Pickup Status" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="PickupStatus" FilterControlAltText="Filter Pickup Status" UniqueName="PickupStatus" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="OrderNumber" Visible="false" HeaderButtonType="TextButton" HeaderText="Order Number" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="OrderNumber" FilterControlAltText="Filter Order Number" UniqueName="OrderNumber" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="OrderItem" HeaderButtonType="TextButton" HeaderText="Order Item" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="OrderItem" FilterControlAltText="Filter Order Item" UniqueName="OrderItem" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>
                  <telerik:GridBoundColumn DataField="Quantity" HeaderButtonType="TextButton" HeaderText="Quantity" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="false"  SortExpression="Quantity" FilterControlAltText="Filter Quantity" UniqueName="Quantity" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>

              

      </Columns>
        <PagerStyle AlwaysVisible="True" />
    </MasterTableView>
  <ClientSettings>
           <Selecting AllowRowSelect="true"></Selecting>
                       
   </ClientSettings>
<FilterMenu CssClass="RadFilterMenu_CheckList"></FilterMenu>
          <FilterItemStyle HorizontalAlign="Center"  />
  </telerik:RadGrid>
                </td>
            </tr>
         <tr>

               <td align="center">
      <telerik:RadGrid ID="RadGridForecastBanhTet" runat="server" ActiveItemStyle-BorderStyle="Solid" AllowPaging="False" AllowSorting="True" AllowFilteringByColumn="false" FilterType="CheckList" AutoGenerateColumns="False" OnNeedDataSource="RadGridForecastBanhTet_NeedDataSource" PageSize="1000" ShowStatusBar="True" Width="98%"  Skin="Sunset"  GroupPanelPosition="Top"  >
    <ActiveItemStyle BorderStyle="Solid" />
    <PagerStyle Mode="NumericPages" AlwaysVisible="True" />
          <HeaderStyle CssClass="CustomHeaderForRadGridForecastBanhTet" />
    <MasterTableView AllowMultiColumnSorting="True" DataKeyNames="InvoiceNumber" Name="InvoiceNumber" Width="100%">
      <Columns>
           <telerik:GridBoundColumn DataField="Link" Visible="true" HeaderButtonType="TextButton" HeaderText="" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="false"  SortExpression="Link" FilterControlAltText="Filter Link" UniqueName="Link" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="5%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="InvoiceNumber" HeaderButtonType="TextButton" HeaderText="Invoice Number" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="InvoiceNumber" FilterControlAltText="Filter Invoice Number" UniqueName="InvoiceNumber" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>
           <telerik:GridBoundColumn DataField="PackedForPickup" HeaderButtonType="TextButton" HeaderText="Packed For Pickup" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="PackedForPickup" FilterControlAltText="Filter Packed For Pickuped" UniqueName="PackedForPickup" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
   <HeaderStyle Font-Bold="True" Font-Size="Medium" />
   </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="CreatedBy" HeaderButtonType="TextButton" HeaderText="Created By" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="CreatedBy" FilterControlAltText="Filter Created By" UniqueName="CreatedBy" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="CustomerName" HeaderButtonType="TextButton" HeaderText="Customer Name" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="CustomerName" FilterControlAltText="Filter Customer Name" UniqueName="CustomerName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="CustomerPhone" Visible="false" HeaderButtonType="TextButton" HeaderText="Customer Phone" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="CustomerPhone" FilterControlAltText="Filter Customer Phone" UniqueName="CustomerPhone" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>
         <telerik:GridBoundColumn DataField="InvoiceType" Visible="false" HeaderButtonType="TextButton" HeaderText="Invoice Type" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="InvoiceType" FilterControlAltText="Filter Invoice Type" UniqueName="InvoiceType" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="5%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>
         <telerik:GridBoundColumn DataField="PickupDate" HeaderButtonType="TextButton" HeaderText="Pickup Date" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="PickupDate" FilterControlAltText="Filter Pickup Date" UniqueName="PickupDate" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="PickupStatus" HeaderButtonType="TextButton" HeaderText="Pickup Status" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="PickupStatus" FilterControlAltText="Filter Pickup Status" UniqueName="PickupStatus" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="OrderNumber" Visible="false" HeaderButtonType="TextButton" HeaderText="Order Number" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="OrderNumber" FilterControlAltText="Filter Order Number" UniqueName="OrderNumber" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="OrderItem" HeaderButtonType="TextButton" HeaderText="Order Item" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="OrderItem" FilterControlAltText="Filter Order Item" UniqueName="OrderItem" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>
                  <telerik:GridBoundColumn DataField="Quantity" HeaderButtonType="TextButton" HeaderText="Quantity" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="false"  SortExpression="Quantity" FilterControlAltText="Filter Quantity" UniqueName="Quantity" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
        </telerik:GridBoundColumn>

              

      </Columns>
        <PagerStyle AlwaysVisible="True" />
    </MasterTableView>
  <ClientSettings>
           <Selecting AllowRowSelect="true"></Selecting>
                       
   </ClientSettings>
<FilterMenu CssClass="RadFilterMenu_CheckList"></FilterMenu>
          <FilterItemStyle HorizontalAlign="Center"  />
  </telerik:RadGrid>
                </td>
            </tr>
            </table>
    </div>
</asp:Content>
