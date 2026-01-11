<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeBehind="TrackingChecks.aspx.cs" Inherits="SaintPolycarp.BanhChung.TrackingChecks" %>
<%@ MasterType VirtualPath="~/Site.master" %> 
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align:center;">
    <table style="width: 100%;">
        <tr>
            <td align="left">
                     <!--Place holder-->


            </td>
              <td align="left" style="width: 50%;">
                <asp:Label ID="LabelPageTitle" runat="server" Text="Tracking Checks" Font-Bold="true"  Font-Size="XX-Large" Font-Underline="true"></asp:Label> 
             </td>
            
        </tr>
        </table>
        </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ConsoleContent" runat="server">
  
    <style type="text/css">
        .datepicker
        {
            width: 80px !important;
            align-content:center;
            align-items: center;
        }
</style>
 
      <div style="text-align:center;">
          <br />
        <table style="width: 100%;">
           
            <tr>
                 <td align="center">
      <telerik:RadGrid ID="RadGridMoneyTransfers" ShowFooter="true" runat="server" ActiveItemStyle-BorderStyle="Solid" AllowPaging="false" AllowSorting="False" AllowFilteringByColumn="False" FilterType="CheckList" AutoGenerateColumns="False" OnNeedDataSource="RadGridMoneyTransfers_NeedDataSource" PageSize="200" ShowStatusBar="True" Width="100%"  Skin="Web20" GroupPanelPosition="Top"  >
     <ActiveItemStyle BorderStyle="Solid" />
    <PagerStyle Mode="NumericPages" AlwaysVisible="True" />
    <MasterTableView AllowMultiColumnSorting="True" DataKeyNames="InvoiceNumber" Name="InvoiceNumber" Width="100%">
      <Columns>
            <telerik:GridBoundColumn DataField="InvoiceNumber" Display="false" ItemStyle-BackColor="#666666" ItemStyle-ForeColor ="White" ItemStyle-Font-Size="X-Large" HeaderButtonType="TextButton" HeaderText="Invoice Number" AllowFiltering="true" FilterCheckListEnableLoadOnDemand="true"  SortExpression="InvoiceNumber" FilterControlAltText="Filter Invoice Number" UniqueName="InvoiceNumber" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="3%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
                   <FooterStyle Font-Bold="true" ForeColor="White" BackColor="#666666" Font-Size="Medium" HorizontalAlign="Right"/>
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="InvoiceNumberWithLink" ItemStyle-BackColor="#666666" ItemStyle-ForeColor ="White" ItemStyle-Font-Size="X-Large" HeaderButtonType="TextButton" HeaderText="Inv #" AllowFiltering="true" FilterCheckListEnableLoadOnDemand="true"  SortExpression="InvoiceNumberWithLink" FilterControlAltText="Filter Invoice Number" UniqueName="InvoiceNumberWithLink" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="3%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
                   <FooterStyle Font-Bold="true" ForeColor="White" BackColor="#666666" Font-Size="Medium" HorizontalAlign="Right"/>
        </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="CustomerName" ItemStyle-BackColor="#666666" ItemStyle-ForeColor="White" HeaderButtonType="TextButton" HeaderText="Customer Name" AllowFiltering="true" FilterCheckListEnableLoadOnDemand="true"  SortExpression="CustomerName" FilterControlAltText="Filter Customer Name" UniqueName="CustomerName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="3%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
                    <FooterStyle Font-Bold="true" ForeColor="White" BackColor="#666666" Font-Size="Medium" HorizontalAlign="Right"/>
        </telerik:GridBoundColumn>
           <telerik:GridBoundColumn DataField="CreatedBy" ItemStyle-BackColor="#666666" ItemStyle-ForeColor="White" HeaderButtonType="TextButton" HeaderText="Created By" AllowFiltering="true" FilterCheckListEnableLoadOnDemand="true"  SortExpression="CreatedBy" FilterControlAltText="Filter Created By" UniqueName="CreatedBy" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="3%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
                    <FooterStyle Font-Bold="true" ForeColor="White" BackColor="#666666" Font-Size="Medium" HorizontalAlign="Right"/>
        </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="PickupStatus" ItemStyle-BackColor="#666666" ItemStyle-ForeColor="White" HeaderButtonType="TextButton" HeaderText="Pickup Status" AllowFiltering="true" FilterCheckListEnableLoadOnDemand="true"  SortExpression="PickupStatus" FilterControlAltText="Filter Pickup Status" UniqueName="PickupStatus" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="3%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
                    <FooterStyle Font-Bold="true" ForeColor="White" BackColor="#666666" Font-Size="Medium" HorizontalAlign="Right"/>
        </telerik:GridBoundColumn>
         <telerik:GridBoundColumn DataField="PaidStatus" ItemStyle-BackColor="#666666" ItemStyle-ForeColor="White"  HeaderButtonType="TextButton" HeaderText="Paid Status" AllowFiltering="true" FilterCheckListEnableLoadOnDemand="true"  SortExpression="PaidStatus" FilterControlAltText="Filter Paid Status" UniqueName="PaidStatus" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="3%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
                 <FooterStyle Font-Bold="true" ForeColor="White" BackColor="#666666" Font-Size="Medium" HorizontalAlign="Right"/>
        </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="PaidByCash" ItemStyle-BackColor="YellowGreen" ItemStyle-ForeColor="White" HeaderButtonType="TextButton" HeaderText="Paid By Cash" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="PaidByCash" FilterControlAltText="Filter Paid By Cash" UniqueName="PaidByCash" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="3%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
                <ItemStyle HorizontalAlign="Right" />
                           <FooterStyle Font-Bold="true" ForeColor="White" BackColor="YellowGreen" Font-Size="Medium" HorizontalAlign="Right"/>
    
        </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="PaidByCheck" ItemStyle-BackColor="YellowGreen" ItemStyle-ForeColor="White" HeaderButtonType="TextButton" HeaderText="Paid By Check" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="PaidByCheck" FilterControlAltText="Filter Paid By Check" UniqueName="PaidByCheck" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="3%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
                <ItemStyle HorizontalAlign="Right" />
                           <FooterStyle Font-Bold="true" ForeColor="White" BackColor="YellowGreen" Font-Size="Medium" HorizontalAlign="Right"/>
    
        </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="TotalPaid" ItemStyle-BackColor="Green" ItemStyle-ForeColor="White" HeaderButtonType="TextButton" HeaderText="Total Paid" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="TotalPaid" FilterControlAltText="Filter Total Paid" UniqueName="TotalPaid" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="3%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
                <ItemStyle HorizontalAlign="Right" />
                           <FooterStyle Font-Bold="true" ForeColor="White" BackColor="Green" Font-Size="Medium" HorizontalAlign="Right"/>
    
        </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="InvoiceAmount" ItemStyle-BackColor="#666666" ItemStyle-ForeColor="White" HeaderButtonType="TextButton" HeaderText="Invoice Amount" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="InvoiceAmount" FilterControlAltText="Filter Invoice Amount" UniqueName="InvoiceAmount" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="3%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium"/>
             <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle Font-Bold="true" ForeColor="White" BackColor="#666666" Font-Size="Medium" HorizontalAlign="Right"/>
        </telerik:GridBoundColumn>
       
            <telerik:GridBoundColumn DataField="RemainBalance" ItemStyle-BackColor="Red" ItemStyle-ForeColor="White" HeaderButtonType="TextButton" HeaderText="Remain Balance" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="RemainBalance" FilterControlAltText="Filter Remain Balance" UniqueName="RemainBalance" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="3%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
                <ItemStyle HorizontalAlign="Right" />
                           <FooterStyle Font-Bold="true" ForeColor="White" BackColor="Red" Font-Size="Small" HorizontalAlign="Right"/>    
        </telerik:GridBoundColumn>
          <telerik:GridBoundColumn DataField="InvoiceType" HeaderButtonType="TextButton" HeaderText="Invoice Type" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="InvoiceType" FilterControlAltText="Filter Invoice Type" UniqueName="InvoiceType" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="3%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
                <ItemStyle HorizontalAlign="Center" />
        </telerik:GridBoundColumn>
          <telerik:GridBoundColumn DataField="CheckInfo" HeaderButtonType="TextButton" HeaderText="Check Info" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="CheckInfo" FilterControlAltText="Filter Check Info" UniqueName="CheckInfo" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="5%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
                <ItemStyle HorizontalAlign="Left" />                          
        </telerik:GridBoundColumn>
                   <telerik:GridBoundColumn DataField="Notes" HeaderButtonType="TextButton" HeaderText="Notes" AllowFiltering="false" FilterCheckListEnableLoadOnDemand="true"  SortExpression="Notes" FilterControlAltText="Filter Notes" UniqueName="Notes" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="12%">
          <HeaderStyle Font-Bold="True" Font-Size="Medium" />
                <ItemStyle HorizontalAlign="Left" />
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
          <asp:Literal ID="LiteralLogs" runat="server"></asp:Literal>
         
    </div>
</asp:Content>
