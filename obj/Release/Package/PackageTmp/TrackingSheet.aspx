<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeBehind="TrackingSheet.aspx.cs" Inherits="SaintPolycarp.BanhChung.TrackingSheet" %>
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
                <asp:Label ID="LabelPageTitle" runat="server" Text="Tracking Sheet" Font-Bold="true"  Font-Size="XX-Large" Font-Underline="true"></asp:Label> 
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
         .notes-style {
                 max-width: 100%;
         }
</style>
 
      <div style="text-align:center;">
           <table style="width: 100%; align-content:center; border:solid">
                        <tr">
                            <td  align="center">
                                Display Settings:
                            </td>
                             <td  align="left">
                                Giao 1 <asp:CheckBox ID="CheckBox1" Checked="true" AutoPostBack="true" runat="server" OnCheckedChanged="CheckBox_CheckedChanged" />
                            </td>
                            <td  align="left">
                                Giao 2 <asp:CheckBox ID="CheckBox2" Checked="true" AutoPostBack="true" runat="server" OnCheckedChanged="CheckBox_CheckedChanged" />
                            </td>
                             <td  align="left">
                                Giao 3 <asp:CheckBox ID="CheckBox3" Checked="true" AutoPostBack="true" runat="server" OnCheckedChanged="CheckBox_CheckedChanged" />
                            </td>
                             <td  align="left">
                                Giao 4 <asp:CheckBox ID="CheckBox4" Checked="true" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBox_CheckedChanged" />
                            </td>
                             <td  align="left">
                                Giao 5 <asp:CheckBox ID="CheckBox5"  Checked="true" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBox_CheckedChanged" />
                            </td>
                             <td  align="left">
                                Giao 6 <asp:CheckBox ID="CheckBox6" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBox_CheckedChanged" />
                            </td>
                             <td  align="left">
                                Giao 7 <asp:CheckBox ID="CheckBox7" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBox_CheckedChanged" />
                            </td>
                             <td  align="left">
                                Giao 8 <asp:CheckBox ID="CheckBox8" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBox_CheckedChanged" />
                            </td>
                             <td  align="left">
                                Giao 9 <asp:CheckBox ID="CheckBox9" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBox_CheckedChanged" />
                            </td>
                             <td  align="left">
                                Giao 10 <asp:CheckBox ID="CheckBox10" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBox_CheckedChanged" />
                            </td>
                              <td  align="center" style="align-content:center; border:solid">
                                Expand Width <asp:CheckBox ID="CheckBoxExpandWidth" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBox_CheckedChanged" />
                            </td>
                            <td align="center" style="align-content:center; ">
                                <asp:Button ID="ButtonSave" runat="server" Text="Save" Width="120px" BackColor="Orange" BorderStyle="Double" BorderColor="Blue" OnClick="ButtonSave_Click" /> 

                            </td>
                            <td align="center" style="align-content:center;"> 
                                <asp:Button ID="ButtonExportXls" runat="server" Text="Export Xls" Width="120px" BackColor="Orange" BorderStyle="Double" BorderColor="Blue" OnClick="ButtonExportXls_Click"/> 
                            </td>
                              <td align="center" style="align-content:center;"> 
                                <asp:Button ID="ButtonExportXlsx" runat="server" Text="Export Xlsx" Width="120px" BackColor="Orange" BorderStyle="Double" BorderColor="Blue" OnClick="ButtonExportXlsx_Click"/> 
                            </td>
                              
                             <td align="center" style="align-content:center; border:solid">
                                <asp:Label ID="LabelLastUpdate" runat="server" Text=""  Font-Size="Small"></asp:Label> 
                            </td>
                        </tr>
      </table>
          <br />
        <table style="width: 100%;">
           
            <tr>
                 <td align="center">
      <telerik:RadGrid ID="RadGridMoneyTransfers" ShowFooter="true" runat="server" ActiveItemStyle-BorderStyle="Solid" AllowPaging="false" AllowSorting="False" AllowFilteringByColumn="False" FilterType="CheckList" AutoGenerateColumns="False" OnNeedDataSource="RadGridMoneyTransfers_NeedDataSource" OnPreRender="RadGridMoneyTransfers_PreRender" PageSize="200" ShowStatusBar="True" Width="100%"  Skin="Web20" OnFilterCheckListItemsRequested="RadGridMoneyTransfers_FilterCheckListItemsRequested" OnItemCommand="RadGridMoneyTransfers_ItemCommand" OnItemDataBound="RadGridMoneyTransfers_ItemDataBound" GroupPanelPosition="Top" OnHTMLExporting="RadGrid_HTMLExporting"  >
       <ClientSettings>
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="2" ScrollHeight="700px"></Scrolling>
       </ClientSettings>
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
            <telerik:GridTemplateColumn DataField="Giao1" HeaderButtonType="TextButton" AllowFiltering="false" UniqueName="Giao1" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="Medium" />  
                                <HeaderTemplate>
                                      <asp:Label ID="LabeGiao1" Text="Giao #1" runat="server"></asp:Label><br />
                                     <telerik:RadDatePicker ID="RadDatePickerInvoicePickupDate1" CssClass="datepicker" Font-Bold="true" Font-Size="Small" runat="server" OnSelectedDateChanged="RadDatePickerInvoicePickupDate_SelectedDateChanged" Enabled="true" MinDate="2019-01-01">
                                          <DateInput DateFormat="MMM d  yyyy" DisplayDateFormat="MM/dd"> </DateInput>
                                     </telerik:RadDatePicker><br />
                                       <asp:CheckBox ID="Giao1" runat="server" Visible="true" OnCheckedChanged="CheckBoxGiao_CheckedChanged" AutoPostBack="true" />
                                </HeaderTemplate>
                                    <ItemTemplate> 
                                        <asp:TextBox ID="Giao1"  OnTextChanged="TextBoxGiao_TextChanged" AutoPostBack="true" Visible="true" runat="server" style="text-align: right" Width="80%">
                                        </asp:TextBox>
                                    </ItemTemplate> 
                                        <FooterStyle Font-Bold="true" ForeColor="Blue" Font-Size="Medium" HorizontalAlign="Right"/>  
                                        
                              
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
         </telerik:GridTemplateColumn> 
        <telerik:GridTemplateColumn DataField="Giao2" HeaderButtonType="TextButton" AllowFiltering="false" UniqueName="Giao2" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="Medium" />  
                                <HeaderTemplate>
                                      <asp:Label ID="LabeGiao2" Text="Giao #2" runat="server"></asp:Label><br />
                                        <telerik:RadDatePicker ID="RadDatePickerInvoicePickupDate2" CssClass="datepicker" Font-Bold="true" Font-Size="Small" runat="server" OnSelectedDateChanged="RadDatePickerInvoicePickupDate_SelectedDateChanged" Enabled="true" MinDate="2019-01-01">
                                             <DateInput DateFormat="MMM d  yyyy" DisplayDateFormat="MM/dd"> </DateInput>
                                     </telerik:RadDatePicker><br />
                                    <asp:CheckBox ID="Giao2" runat="server" Visible="true" OnCheckedChanged="CheckBoxGiao_CheckedChanged" AutoPostBack="true" />
                                </HeaderTemplate>
                                    <ItemTemplate> 
                                        <asp:TextBox ID="Giao2"  OnTextChanged="TextBoxGiao_TextChanged" AutoPostBack="true" Visible="true" runat="server" style="text-align: right" Width="80%">
                                        </asp:TextBox>
                                    </ItemTemplate> 
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                       <FooterStyle Font-Bold="true" ForeColor="Blue" Font-Size="Medium" HorizontalAlign="Right"/>
    
         </telerik:GridTemplateColumn> 
             <telerik:GridTemplateColumn DataField="Giao3" HeaderButtonType="TextButton" AllowFiltering="false" UniqueName="Giao3" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="Medium" />  
                                <HeaderTemplate>
                                      <asp:Label ID="LabeGiao3" Text="Giao #3" runat="server"></asp:Label><br />
                                       <telerik:RadDatePicker ID="RadDatePickerInvoicePickupDate3" CssClass="datepicker" Font-Bold="true" Font-Size="Small" runat="server" OnSelectedDateChanged="RadDatePickerInvoicePickupDate_SelectedDateChanged" Enabled="true" MinDate="2019-01-01">
                                            <DateInput DateFormat="MMM d  yyyy" DisplayDateFormat="MM/dd"> </DateInput>
                                       </telerik:RadDatePicker><br />
                                     <asp:CheckBox ID="Giao3" runat="server" Visible="true" OnCheckedChanged="CheckBoxGiao_CheckedChanged" AutoPostBack="true" />
                                </HeaderTemplate>
                                    <ItemTemplate> 
                                        <asp:TextBox ID="Giao3"  OnTextChanged="TextBoxGiao_TextChanged" AutoPostBack="true" Visible="true" runat="server" style="text-align: right" Width="80%">
                                        </asp:TextBox>
                                    </ItemTemplate> 
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <FooterStyle Font-Bold="true" ForeColor="Blue" Font-Size="Medium" HorizontalAlign="Right"/>
    
         </telerik:GridTemplateColumn> 
             <telerik:GridTemplateColumn DataField="Giao4" HeaderButtonType="TextButton" AllowFiltering="false" UniqueName="Giao4" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="Medium" />  
                                <HeaderTemplate>
                                      <asp:Label ID="LabeGiao4" Text="Giao #4" runat="server"></asp:Label><br />
                                        <telerik:RadDatePicker ID="RadDatePickerInvoicePickupDate4" CssClass="datepicker" Font-Bold="true" Font-Size="Small" runat="server" OnSelectedDateChanged="RadDatePickerInvoicePickupDate_SelectedDateChanged" Enabled="true" MinDate="2019-01-01">
                                             <DateInput DateFormat="MMM d  yyyy" DisplayDateFormat="MM/dd"> </DateInput>
                                        </telerik:RadDatePicker><br />
                                    <asp:CheckBox ID="Giao4" runat="server" Visible="true" OnCheckedChanged="CheckBoxGiao_CheckedChanged" AutoPostBack="true" />
                                </HeaderTemplate>
                                    <ItemTemplate> 
                                        <asp:TextBox ID="Giao4"  OnTextChanged="TextBoxGiao_TextChanged" AutoPostBack="true" Visible="true" runat="server" style="text-align: right" Width="80%">
                                        </asp:TextBox>
                                    </ItemTemplate> 
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <FooterStyle Font-Bold="true" ForeColor="Blue" Font-Size="Medium" HorizontalAlign="Right"/>
    
         </telerik:GridTemplateColumn> 
             <telerik:GridTemplateColumn DataField="Giao5" HeaderButtonType="TextButton" AllowFiltering="false" UniqueName="Giao5" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="Medium" />  
                                <HeaderTemplate>
                                      <asp:Label ID="LabeGiao5" Text="Giao #5" runat="server"></asp:Label><br />
                                        <telerik:RadDatePicker ID="RadDatePickerInvoicePickupDate5" CssClass="datepicker" Font-Bold="true" Font-Size="Small" runat="server" OnSelectedDateChanged="RadDatePickerInvoicePickupDate_SelectedDateChanged" Enabled="true" MinDate="2019-01-01">
                                             <DateInput DateFormat="MMM d  yyyy" DisplayDateFormat="MM/dd"> </DateInput>
                                        </telerik:RadDatePicker><br />
                                    <asp:CheckBox ID="Giao5" runat="server" Visible="true" OnCheckedChanged="CheckBoxGiao_CheckedChanged" AutoPostBack="true" />
                                </HeaderTemplate>
                                    <ItemTemplate> 
                                        <asp:TextBox ID="Giao5"  OnTextChanged="TextBoxGiao_TextChanged" AutoPostBack="true" Visible="true" runat="server" style="text-align: right" Width="80%">
                                        </asp:TextBox>
                                    </ItemTemplate> 
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <FooterStyle Font-Bold="true" ForeColor="Blue" Font-Size="Medium" HorizontalAlign="Right"/>
    
         </telerik:GridTemplateColumn> 
             <telerik:GridTemplateColumn DataField="Giao6" HeaderButtonType="TextButton" AllowFiltering="false" UniqueName="Giao6" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="Medium" />  
                                <HeaderTemplate>
                                      <asp:Label ID="LabeGiao6" Text="Giao #6" runat="server"></asp:Label><br />
                                        <telerik:RadDatePicker ID="RadDatePickerInvoicePickupDate6" CssClass="datepicker" Font-Bold="true" Font-Size="Small" runat="server" OnSelectedDateChanged="RadDatePickerInvoicePickupDate_SelectedDateChanged" Enabled="true" MinDate="2019-01-01">
                                             <DateInput DateFormat="MMM d  yyyy" DisplayDateFormat="MM/dd"> </DateInput>
                                        </telerik:RadDatePicker><br />
                                    <asp:CheckBox ID="Giao6" runat="server" Visible="true" OnCheckedChanged="CheckBoxGiao_CheckedChanged" AutoPostBack="true" />
                                </HeaderTemplate>
                                    <ItemTemplate> 
                                        <asp:TextBox ID="Giao6"  OnTextChanged="TextBoxGiao_TextChanged" AutoPostBack="true" Visible="true" runat="server" style="text-align: right" Width="80%">
                                        </asp:TextBox>
                                    </ItemTemplate> 
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <FooterStyle Font-Bold="true" ForeColor="Blue" Font-Size="Medium" HorizontalAlign="Right"/>
    
         </telerik:GridTemplateColumn> 
             <telerik:GridTemplateColumn DataField="Giao7" HeaderButtonType="TextButton" AllowFiltering="false" UniqueName="Giao7" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="Medium" />  
                                <HeaderTemplate>
                                      <asp:Label ID="LabeGiao7" Text="Giao #7" runat="server"></asp:Label><br />
                                        <telerik:RadDatePicker ID="RadDatePickerInvoicePickupDate7" CssClass="datepicker" Font-Bold="true" Font-Size="Small" runat="server" OnSelectedDateChanged="RadDatePickerInvoicePickupDate_SelectedDateChanged" Enabled="true" MinDate="2019-01-01">
                                             <DateInput DateFormat="MMM d  yyyy" DisplayDateFormat="MM/dd"> </DateInput>
                                        </telerik:RadDatePicker><br />
                                    <asp:CheckBox ID="Giao7" runat="server" Visible="true" OnCheckedChanged="CheckBoxGiao_CheckedChanged" AutoPostBack="true" />
                                </HeaderTemplate>
                                    <ItemTemplate> 
                                        <asp:TextBox ID="Giao7"  OnTextChanged="TextBoxGiao_TextChanged" AutoPostBack="true" Visible="true" runat="server" style="text-align: right" Width="80%">
                                        </asp:TextBox>
                                    </ItemTemplate> 
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <FooterStyle Font-Bold="true" ForeColor="Blue" Font-Size="Medium" HorizontalAlign="Right"/>
    
         </telerik:GridTemplateColumn> 
             <telerik:GridTemplateColumn DataField="Giao8" HeaderButtonType="TextButton" AllowFiltering="false" UniqueName="Giao8" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="Medium" />  
                                <HeaderTemplate>
                                      <asp:Label ID="LabeGiao8" Text="Giao #8" runat="server"></asp:Label><br />
                                        <telerik:RadDatePicker ID="RadDatePickerInvoicePickupDate8" CssClass="datepicker" Font-Bold="true" Font-Size="Small" runat="server" OnSelectedDateChanged="RadDatePickerInvoicePickupDate_SelectedDateChanged" Enabled="true" MinDate="2019-01-01">
                                             <DateInput DateFormat="MMM d  yyyy" DisplayDateFormat="MM/dd"> </DateInput>
                                        </telerik:RadDatePicker><br />
                                    <asp:CheckBox ID="Giao8" runat="server" Visible="true" OnCheckedChanged="CheckBoxGiao_CheckedChanged" AutoPostBack="true" />
                                </HeaderTemplate>
                                    <ItemTemplate> 
                                        <asp:TextBox ID="Giao8"  OnTextChanged="TextBoxGiao_TextChanged" AutoPostBack="true" Visible="true" runat="server" style="text-align: right" Width="80%">
                                        </asp:TextBox>
                                    </ItemTemplate> 
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <FooterStyle Font-Bold="true" ForeColor="Blue" Font-Size="Medium" HorizontalAlign="Right"/>
    
         </telerik:GridTemplateColumn> 
             <telerik:GridTemplateColumn DataField="Giao9" HeaderButtonType="TextButton" AllowFiltering="false" UniqueName="Giao9" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="Medium" />  
                                <HeaderTemplate>
                                      <asp:Label ID="LabeGiao9" Text="Giao #9" runat="server"></asp:Label><br />
                                        <telerik:RadDatePicker ID="RadDatePickerInvoicePickupDate9" CssClass="datepicker" Font-Bold="true" Font-Size="Small" runat="server" OnSelectedDateChanged="RadDatePickerInvoicePickupDate_SelectedDateChanged" Enabled="true" MinDate="2019-01-01">
                                             <DateInput DateFormat="MMM d  yyyy" DisplayDateFormat="MM/dd"> </DateInput>
                                        </telerik:RadDatePicker><br />
                                    <asp:CheckBox ID="Giao9" runat="server" Visible="true" OnCheckedChanged="CheckBoxGiao_CheckedChanged" AutoPostBack="true" />
                                </HeaderTemplate>
                                    <ItemTemplate> 
                                        <asp:TextBox ID="Giao9"  OnTextChanged="TextBoxGiao_TextChanged" AutoPostBack="true" Visible="true" runat="server" style="text-align: right" Width="80%">
                                        </asp:TextBox>
                                    </ItemTemplate> 
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <FooterStyle Font-Bold="true" ForeColor="Blue" Font-Size="Medium" HorizontalAlign="Right"/>
    
         </telerik:GridTemplateColumn> 
             <telerik:GridTemplateColumn DataField="Giao10" HeaderButtonType="TextButton" AllowFiltering="false" UniqueName="Giao10" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="Medium" />  
                                <HeaderTemplate>
                                      <asp:Label ID="LabeGiao10" Text="Giao #10" runat="server"></asp:Label><br />
                                        <telerik:RadDatePicker ID="RadDatePickerInvoicePickupDate10" CssClass="datepicker" Font-Bold="true" Font-Size="Small" runat="server" OnSelectedDateChanged="RadDatePickerInvoicePickupDate_SelectedDateChanged" Enabled="true" MinDate="2019-01-01">
                                             <DateInput DateFormat="MMM d  yyyy" DisplayDateFormat="MM/dd"> </DateInput>
                                        </telerik:RadDatePicker><br />
                                    <asp:CheckBox ID="Giao10" runat="server" Visible="true" OnCheckedChanged="CheckBoxGiao_CheckedChanged" AutoPostBack="true" />
                                </HeaderTemplate>
                                    <ItemTemplate> 
                                        <asp:TextBox ID="Giao10"  OnTextChanged="TextBoxGiao_TextChanged" AutoPostBack="true" Visible="true" runat="server" style="text-align: right" Width="80%">
                                        </asp:TextBox>
                                    </ItemTemplate> 
                 
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <FooterStyle Font-Bold="true" ForeColor="Blue" Font-Size="Medium" HorizontalAlign="Right"/>
    
         </telerik:GridTemplateColumn> 


           <telerik:GridTemplateColumn HeaderButtonType="TextButton" AllowFiltering="false" HeaderText="Giao Total" UniqueName="Total" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="Medium" />        
                                    <ItemTemplate> 
                                        <asp:TextBox ID="TextBoxTotal" Font-Bold="true" ForeColor="OrangeRed" Enabled="false" Visible="true"  runat="server" style="text-align: right" Width="90%">
                                        </asp:TextBox>
                                    </ItemTemplate> 
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                          <FooterStyle Font-Bold="true" ForeColor="OrangeRed" Font-Size="Medium" HorizontalAlign="Right"/>
    
         </telerik:GridTemplateColumn> 
            <telerik:GridTemplateColumn HeaderButtonType="TextButton" AllowFiltering="false" HeaderText="Validation" UniqueName="Validation" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="Medium" />        
                               
                              <ItemStyle HorizontalAlign="Center" Font-Size="X-Small"></ItemStyle>
                             <FooterStyle   Font-Size="X-Small" HorizontalAlign="Center"/>
                 
         </telerik:GridTemplateColumn> 
              <telerik:GridTemplateColumn HeaderButtonType="TextButton" AllowFiltering="false" HeaderText="Notes" UniqueName="Notes" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="Medium" />        
                                    <ItemTemplate> 
                                        <asp:TextBox ID="TextBoxNotes" Visible="true" TextMode="MultiLine"  runat="server" style="text-align: left" Width="95%">
                                        </asp:TextBox>
                                    </ItemTemplate> 
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                  <FooterStyle Font-Size="Small" HorizontalAlign="Center"/>
         </telerik:GridTemplateColumn> 
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
                <td>
                    <br /><br />
                           <table style="width: 80%;">
                      <tr>
                          <td>
                               <table style="width: 100%;">
                                   <tr>
                                       <td>
                                           <asp:Label ID="LabelNotes" Text="Notes:" runat="server"> </asp:Label> <br />
                                                <asp:TextBox ID="TextBoxNote" class="notes-style" runat="server" TextMode="MultiLine" Height="300px" Width="800px"></asp:TextBox>  
                                       </td>
                                   </tr>
                                   </table>
                          </td>
                           <td align="center" valign="middle">
                                 <table style="width: 100%;">
                                                 </table>
                </td>
            </tr>
        
            </table>
          <asp:Literal ID="LiteralLogs" runat="server"></asp:Literal>
         
    </div>
</asp:Content>
