<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="OrderDetails.aspx.cs" Inherits="SaintPolycarp.BanhChung.OrderDetails" %>
<%@ MasterType VirtualPath="~/Site.master" %> 
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
       <script language="javascript">

           function SetTarget() {

               document.forms[0].target = "_blank";

           }


       </script>
    <div style="text-align:center;">
    <table style="width: 100%;">
        <tr>
            <td align="left">
    <telerik:RadHtmlChart runat="server" ID="RadHtmlChartPickupStatuses" Width="400" Height="200">
    <PlotArea>
        <Series>
            <telerik:ColumnSeries Name="Picked-up" GroupName="PickupStatus">
                <Appearance>
                    <FillStyle BackgroundColor="#FCD5B5" />
                </Appearance>
                <SeriesItems>
                </SeriesItems>
                <TooltipsAppearance ClientTemplate="#= series.name#: #= dataItem.value#" />
                <LabelsAppearance Visible="false"></LabelsAppearance>
            </telerik:ColumnSeries>
            <telerik:ColumnSeries Name="Not Pickup" GroupName="PickupStatus">
                <Appearance>
                    <FillStyle BackgroundColor="#E46C0A" />
                </Appearance>
                <SeriesItems>
                </SeriesItems>
                <TooltipsAppearance ClientTemplate="#= series.name#: #= dataItem.value#" Color="White" />
                <LabelsAppearance Visible="false"></LabelsAppearance>
            </telerik:ColumnSeries>
        </Series>
        <YAxis>
            <MajorGridLines Visible="false" />
            <MinorGridLines Visible="false" />
            <TitleAppearance Text="" />
        </YAxis>
        <XAxis>
            <MajorGridLines Visible="false" />
            <MinorGridLines Visible="false" />
            <LabelsAppearance DataFormatString="{0}" />
            <Items>
                <telerik:AxisItem LabelText="Bánh Chưng" />
                <telerik:AxisItem LabelText="Bánh Tét" />
            </Items>
        </XAxis>
    </PlotArea>
    <ChartTitle Text="">
    </ChartTitle>
    <Legend>
        <Appearance Position="Right" />
    </Legend>
</telerik:RadHtmlChart>
            </td>
              <td align="center" style="width: 50%;">
                    <table style="width: 20%; align-content:center; >
                        <tr>
                            <td>
                        <asp:Label ID="LabelPageTitle" runat="server" Text="Order Details" Font-Bold="true"  Font-Size="XX-Large" Font-Underline="true"></asp:Label> 
                             </td>
                        </tr>
                         <tr>
                             <td>
                                <table style="width: 20%; align-content:center; border:solid">
                                    <tr">
               
                          
                                      <td>    <asp:Button ID="ButtonExportOrderDetails" runat="server" Text="Export Order Details" Width="200px" BackColor="Orange" BorderStyle="Double" BorderColor="Blue" OnClick="ButtonExportOrderDetails_Click"/> &emsp; </td>
                                           
                                                         </td>   
                                                        <td>    <asp:Button ID="ButtonExportOverall" runat="server" Text="Export Overall Quantity" Width="200px" BackColor="Orange" BorderStyle="Double" BorderColor="Blue" OnClick="ButtonExportOverall_Click"/> &emsp; </td>
                                           
                                                        </td>  
                                                        <td>    <asp:Button ID="ButtonPrintSelected" runat="server" Text="Print Selected" Width="200px" BackColor="Orange" BorderStyle="Double" BorderColor="Blue" OnClick="ButtonPrintSelected_Click" OnClientClick = "SetTarget();" /> &emsp; </td>
                                           
                                                        </td>
                           
                                    </tr>
                                </table>
                             </td>
                            </tr>
                     </table>
                 <br />
             </td>

        </tr>
        </table>
        </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ConsoleContent" runat="server">
      <div style="text-align:center;">
        <table style="width: 100%;">
             <tr>
                 <td align="center">
     <telerik:RadGrid ID="RadGridSummaryOrderDetails" visible="false" runat="server" ActiveItemStyle-BorderStyle="Solid" AllowPaging="False" AllowSorting="False" AllowFilteringByColumn="False" FilterType="CheckList" AutoGenerateColumns="False" PageSize="100" ShowStatusBar="True" Width="80%"  Skin="Web20"  GroupPanelPosition="Top" OnItemDataBound="RadGridSummaryOrderDetails_ItemDataBound" OnNeedDataSource="RadGridSummaryOrderDetails_NeedDataSource" OnHTMLExporting="RadGrid_HTMLExporting" >
    <ActiveItemStyle BorderStyle="Solid" />
    <PagerStyle Mode="NumericPages" AlwaysVisible="False" />
    <MasterTableView AllowMultiColumnSorting="False" DataKeyNames="QuantityType" Name="QuantityType" Width="100%">
      <Columns>
         <telerik:GridBoundColumn DataField="QuantityType" HeaderButtonType="TextButton" HeaderText="" AllowFiltering="false" SortExpression="QuantityType" FilterControlAltText="Filter Quantity Type" UniqueName="QuantityType" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
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
                 <td align="center">
      <telerik:RadGrid ID="RadGridOrderDetails" runat="server" ActiveItemStyle-BorderStyle="Solid" AllowPaging="True" AllowSorting="True" AllowFilteringByColumn="True" FilterType="CheckList" AutoGenerateColumns="False" OnNeedDataSource="RadGridOrderDetails_NeedDataSource" OnPreRender="RadGridOrderDetails_PreRender" PageSize="1000" ShowStatusBar="True" Width="98%"  Skin="Sunset" OnFilterCheckListItemsRequested="RadGridOrderDetails_FilterCheckListItemsRequested" OnItemCommand="RadGridOrderDetails_ItemCommand" OnItemDataBound="RadGridOrderDetails_ItemDataBound" GroupPanelPosition="Top" OnHTMLExporting="RadGrid_HTMLExporting" >
    <ActiveItemStyle BorderStyle="Solid" />
    <PagerStyle Mode="NumericPages" AlwaysVisible="True" />
    <MasterTableView AllowMultiColumnSorting="True" DataKeyNames="InvoiceNumber" Name="InvoiceNumber" Width="100%">
      <Columns>
           <telerik:GridTemplateColumn DataField="ResetButton" HeaderButtonType="TextButton" HeaderText="" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="false"  SortExpression="ResetButton" FilterControlAltText="Filter ResetButton" UniqueName="ResetButton" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="5%">
          <HeaderStyle Font-Bold="True" Font-Size="Large" />
                  <HeaderTemplate>
                       <asp:Button ID="ButtonResetFilter" runat="server" Text="Reset Filter" Width="100px" BackColor="Orange" BorderStyle="Double" BorderColor="Blue" OnClick="ButtonResetFilter_Click"/>
                  </HeaderTemplate>
        </telerik:GridTemplateColumn>
           <telerik:GridBoundColumn DataField="Link" HeaderButtonType="TextButton" HeaderText="" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="false"  SortExpression="Link" FilterControlAltText="Filter Link" UniqueName="Link" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="5%">
          <HeaderStyle Font-Bold="True" Font-Size="Large" />
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="InvoiceNumber" HeaderButtonType="TextButton" HeaderText="Invoice Number" AllowFiltering="true" FilterCheckListEnableLoadOnDemand="true"  SortExpression="InvoiceNumber" FilterControlAltText="Filter Invoice Number" UniqueName="InvoiceNumber" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Large" />
        </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="CreatedBy" HeaderButtonType="TextButton" HeaderText="Created By" AllowFiltering="true" FilterCheckListEnableLoadOnDemand="true"  SortExpression="CreatedBy" FilterControlAltText="Filter Created By" UniqueName="CreatedBy" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Large" />
        </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="CustomerName" HeaderButtonType="TextButton" HeaderText="Customer Name" AllowFiltering="true" FilterCheckListEnableLoadOnDemand="true"  SortExpression="CustomerName" FilterControlAltText="Filter Customer Name" UniqueName="CustomerName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Large" />
        </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="CustomerPhone" HeaderButtonType="TextButton" HeaderText="Customer Phone" AllowFiltering="true" FilterCheckListEnableLoadOnDemand="true"  SortExpression="CustomerPhone" FilterControlAltText="Filter Customer Phone" UniqueName="CustomerPhone" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Large" />
        </telerik:GridBoundColumn>
         <telerik:GridBoundColumn DataField="InvoiceType" HeaderButtonType="TextButton" HeaderText="Invoice Type" AllowFiltering="true" FilterCheckListEnableLoadOnDemand="true"  SortExpression="InvoiceType" FilterControlAltText="Filter Invoice Type" UniqueName="InvoiceType" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="5%">
          <HeaderStyle Font-Bold="True" Font-Size="Large" />
        </telerik:GridBoundColumn>
         <telerik:GridBoundColumn DataField="PickupDate" HeaderButtonType="TextButton" HeaderText="Pickup Date" AllowFiltering="true" FilterCheckListEnableLoadOnDemand="true"  SortExpression="PickupDate" FilterControlAltText="Filter Pickup Date" UniqueName="PickupDate" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Large" />
        </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="PickupStatus" HeaderButtonType="TextButton" HeaderText="Pickup Status" AllowFiltering="true" FilterCheckListEnableLoadOnDemand="true"  SortExpression="PickupStatus" FilterControlAltText="Filter Pickup Status" UniqueName="PickupStatus" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Large" />
        </telerik:GridBoundColumn>
              <telerik:GridBoundColumn DataField="PackStatus" HeaderButtonType="TextButton" HeaderText="Pack Status" AllowFiltering="true" FilterCheckListEnableLoadOnDemand="true"  SortExpression="PickupStatus" FilterControlAltText="Filter Pack Status" UniqueName="PackStatus" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
            <HeaderStyle Font-Bold="True" Font-Size="Large" />
        </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="OrderNumber" HeaderButtonType="TextButton" HeaderText="Order Number" AllowFiltering="true" FilterCheckListEnableLoadOnDemand="true"  SortExpression="OrderNumber" FilterControlAltText="Filter Order Number" UniqueName="OrderNumber" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Large" />
        </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="OrderItem" HeaderButtonType="TextButton" HeaderText="Order Item" AllowFiltering="true" FilterCheckListEnableLoadOnDemand="true"  SortExpression="OrderItem" FilterControlAltText="Filter Order Item" UniqueName="OrderItem" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Large" />
        </telerik:GridBoundColumn>
                  <telerik:GridBoundColumn DataField="Quantity" HeaderButtonType="TextButton" HeaderText="Quantity" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="false"  SortExpression="Quantity" FilterControlAltText="Filter Quantity" UniqueName="Quantity" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
          <HeaderStyle Font-Bold="True" Font-Size="Large" />
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
                <table style="width: 100%;">
                                               <tr>
                                                  
                                         
                                              </tr>
                                                 </table>
           </tr>
            </table>
    </div>
</asp:Content>
