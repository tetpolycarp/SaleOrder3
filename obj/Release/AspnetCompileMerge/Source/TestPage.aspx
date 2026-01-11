<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Invoice.aspx.cs" Inherits="SaintPolycarp.BanhChung.Invoice" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="LeftContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <input name="b_print" type="button" class="ipt"   onClick="printdiv('div_print', 'div_print2');" value=" Print ">
<script language="javascript">
function printdiv(printpage, printpage2)
{
var headstr = "<html><head><title></title></head><body>";
var footstr = "</body>";
var newstr = document.all.item(printpage).innerHTML;
var newstr2 = document.all.item(printpage2).innerHTML;
var oldstr = document.body.innerHTML;
document.body.innerHTML = headstr+newstr+newstr2+footstr;
window.print();
document.body.innerHTML = oldstr;
return false;
}
</script>
    <div style="text-align: center;">
         <span style="font-size: xx-large;" class="auto-style1"><strong>DEPLOYMENT SCHEDULES</strong></span>
        <br />
       
         <asp:Label ID="LabelScheduleDate" runat="server" Text="" Font-Bold="True" Font-Size="X-Large" ></asp:Label>
         <br /><asp:Label ID="LabelUserName" runat="server" Text="" Font-Size="Smaller"></asp:Label>
         <br /><asp:Label ID="LabelServeError" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="#FF3300"></asp:Label>
         <br />
            <br />
            <br />
     
    </div>
      <div id="div_print" style="text-align: left; width: 113%;">
           <table style="width: 100%;">
               <tr>
                   <td >
                             <table style="border: solid #0040ff thick; width: 100%;">
                                      <tr >
                                         <td align="left" class="auto-style2">    <br /> </td>
                                      <td align="center" class="auto-style2"> 
                                      <td align="center" class="auto-style2">  <br /> </td>
                                       <td align="right" class="auto-style2">  <br /> </td>
                           </tr>
                             
                            <tr >
                     <td align="right" style="width: 50%;">  <asp:Label ID="LabelEnterNewCustomer" runat="server" Text="Enter New Customer" Font-Bold="True" Font-Size="Large" ></asp:Label>   <br /> </td>
                  <td align="center" style="width: 50%;">  <asp:TextBox ID="TextBoxEnterNewCustomer" runat="server" Font-Bold="True" Font-Size="Large" style="text-align: center"></asp:TextBox>
                         <td align="right" style="width: 50%;">  <asp:Label ID="LabelTakeOrderBy" runat="server" Text="Take Order By" Font-Bold="True" Font-Size="Large" ></asp:Label>   <br /> </td>
                  <td align="center" style="width: 50%;">  <asp:DropDownList ID="DropDownListTakeOrderBy" runat="server" Font-Bold="True" Font-Size="Large" style="text-align: center" OnSelectedIndexChanged="DropDownListTakeOrderBy_SelectedIndexChanged">
                      <asp:ListItem>1</asp:ListItem>
                      <asp:ListItem>2</asp:ListItem>
                      </asp:DropDownList>
                      <br /> </td>
                   <td align="right" style="width: 50%;">  
                 
                       <br /> </td>
                           </tr>
                           <tr>
                  <td align="right" style="width: 50%;">  <asp:Label ID="LabelUseExistingCustomer" runat="server" Text="Secondary On-Call" Font-Bold="True" Font-Size="Large" ></asp:Label>   </td>
                  <td align="center" style="width: 50%;">   <asp:DropDownList ID="DropDownListUseExistingCustomer" runat="server" Font-Bold="True" Font-Size="Large" style="text-align: center"></asp:DropDownList></td>
                                  <td align="right" style="width: 50%;">  <asp:Label ID="LabelExpectedPickupDate" runat="server" Text="Expected Pickup Date" Font-Bold="True" Font-Size="Large" ></asp:Label>   <br /> </td>
                  <td align="center" style="width: 50%;">   <telerik:RadDatePicker ID="RadDatePickerPickupDate" Font-Bold="true" Font-Size="X-Large" runat="server" AutoPostBack="true"></telerik:RadDatePicker></td>
                     <td align="right" style="width: 50%;">    <br /> 
                                      
                    </td>
              </tr>
                                   <tr>
                  <td align="right" style="width: 50%;">  <asp:Label ID="LabelPhoneNumber" runat="server" Text="Phone Number" Font-Bold="True" Font-Size="Large" ></asp:Label>   </td>
                  <td align="center" style="width: 50%;">  <asp:TextBox ID="TextBoxPhoneNumber" runat="server" Font-Bold="True" Font-Size="Large" style="text-align: center" OnTextChanged="TextBoxPhoneNumber_TextChanged"></asp:TextBox></td>
                                  <td align="right" style="width: 50%;">  <asp:Label ID="LabelPaidCheckNumber" runat="server" Text="Paid Check #" Font-Bold="True" Font-Size="Large" ></asp:Label>   <br />     <asp:Button ID="ButtonTextDepositReminder" runat="server" Text="Text Deposit Reminder"  /> &emsp;</td>
                  <td align="center" style="width: 50%;">  <asp:TextBox ID="TextBoxPaidCheckNumber" runat="server" Font-Bold="True" Font-Size="Large" style="text-align: center" ></asp:TextBox>
                      <br />
                      <br />
                                       <asp:Button ID="ButtonRefreshOrder" runat="server" Text="RefreshOrder" OnClick="ButtonRefreshOrder_Click"  /> 
                                       </td>
                     <td align="right" style="width: 50%;">    <br /> 
                                      
                    </td>
              </tr>
                                         <tr >
                                         <td align="left">    <br /> </td>
                                      <td align="center"> 
                                      <td align="center">  <br /> </td>
                                       <td align="right">  <br /> </td>
                           </tr>
                               
          </table>
                   </td>
                   <td style="width: 5%;"></td>
                   <td >
                <table style="border: double #ff8000; width: 100%;">
                    <tr>
                      
                      <td>   <strong><span style="font-size: large"><span style="text-decoration: underline">Options:</span> </span></strong> </td>
                      <td>&nbsp;
                      </td>
                  </tr>
                   <tr>

                       <td align="left" valign="top">
                                          &nbsp;&nbsp;&nbsp;<asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="ViewDeploymentsInQueue">View Deployments In Queue</asp:ListItem>
                                            <asp:ListItem Value="ViewAllDeployments" >View All Deployments</asp:ListItem>
                                                 </asp:RadioButtonList> <br />
                                      
    
                       </td>
                       <td >
                           <table>
                              <tr>
                                          <td >
                                      <table>
                                          <tr>
                                              <td align="left">
                                                  Select Date:
                                              </td>
                                          </tr>
                                          <tr>
                                              <td align="center">
                                                         <telerik:RadDatePicker ID="RadDatePickerDeploymentDate" Font-Bold="true" Font-Size="X-Large" runat="server" AutoPostBack="true" OnSelectedDateChanged="RadDatePickerDeploymentDate_SelectedDateChanged"></telerik:RadDatePicker>
                                        <br />
                                              </td>
                                          </tr>
                                      </table>
                                
                                  </td>
                              </tr>
                                <tr>
                                  <td align="right">
                                       <asp:Button ID="ButtonSaveSchedule" runat="server" Text="Save" OnClick="ButtonSaveSchedule_Click"  /> &emsp;
                                       <asp:Button ID="ButtonEmailSchedule" runat="server" Text="Email" OnClick="ButtonEmailSchedule_Click" />&emsp;<br />
                                  </td>
                                  </td>
                              </tr>
                           </table>
                                      
                        
                       </td>
                   </tr>
                 
                    
                  
              </table>
                   </td>
               </tr>
           </table>
     
        
          </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ConsoleContent" runat="server">
        <br />
      <div id="div_print2">               
    <table style="width: 100%;">
   
    <tr>
        <td align="center">
         <style type="text/css">
            .CustomHeaderView
            {
                background-color:#0040ff !important;
                background-image:none !important;
            }
            .CustomHeaderViewAllDeploymentsStyle
            {
                background-color:#ff8000 !important;
                background-image:none !important;
            }
            
        </style>  
            <telerik:RadGrid ID="RadGridSaleItems" runat="server" ActiveItemStyle-BorderStyle="Solid" AllowPaging="True"  AutoGenerateColumns="False" OnNeedDataSource="RadGridSaleItems_NeedDataSource" PageSize="10" ShowStatusBar="True" Width="80%"  Skin="Web20" OnItemDataBound="RadGridSaleItems_ItemDataBound" GroupPanelPosition="Top">
               <ActiveItemStyle BorderStyle="Solid" />
               <PagerStyle Mode="NumericPages" AlwaysVisible="True" />
               <HeaderStyle CssClass="CustomHeaderView" />
             <MasterTableView AllowMultiColumnSorting="false" DataKeyNames="Index" Name="Index" Width="100%" Font-Size="Large">
             <Columns>
                          <telerik:GridBoundColumn DataField="Index" HeaderButtonType="TextButton" HeaderText="Index" SortExpression="Index" UniqueName="Index" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                      <HeaderStyle Font-Bold="True" Font-Size="" />

                       <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </telerik:GridBoundColumn>
                       <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Picked Up" UniqueName="PickedUp" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:ImageButton ID="ImageButtonPickedUp" runat="server" ImageUrl="~/images/pickedup.gif" Width="40px" Height="40px" OnClick="ImageButtonPickedUp_Click" Enabled="false" Visible="false" />
                                        <asp:ImageButton ID="ImageButtonNotPickedUp" runat="server" ImageUrl="~/images/pickedup_no.png" Width="30px" Height="30px" OnClick="ImageButtonNotPickedUp_Click" Enabled="false" Visible="false"   />
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
                            
                      <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Order Number" UniqueName="OrderNumber" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:DropDownList ID="DropDownListOrderNumber" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListOrderNumber_SelectedIndexChanged" Enabled="false" Font-Bold="True" Font-Size="Large" style="text-align: left"></asp:DropDownList>
                                    </ItemTemplate> 

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn>
       
           
                      <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Sale Item" UniqueName="SaleItem" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:DropDownList ID="DropDownListSaleItem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListSaleItem_SelectedIndexChanged" Font-Bold="True" Font-Size="Large" style="text-align: left"></asp:DropDownList>
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
                    <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Quantity" UniqueName="Quantity" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:TextBox ID="TextBoxQuantity" runat="server" AutoPostBack="true" Enabled="false" OnTextChanged="TextBoxQuantity_TextChanged" Font-Bold="True" Font-Size="Large" style="text-align: right">
                                        </asp:TextBox>
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
                   <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Unit Price" UniqueName="UnitPrice" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:TextBox ID="TextBoxUnitPrice" runat="server" Enabled="false" Font-Bold="True" Font-Size="Large" style="text-align: right">
                                        </asp:TextBox>
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
                 <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Discount Per Unit" UniqueName="Discount" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:TextBox ID="TextBoxDiscount" runat="server" AutoPostBack="true" Enabled="false" OnTextChanged="TextBoxDiscount_TextChanged" Font-Bold="True" Font-Size="Large" style="text-align: right">
                                        </asp:TextBox>
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
                   <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Sub-Total" UniqueName="SubTotal" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:TextBox ID="TextBoxSubTotal" runat="server" Enabled="false" Font-Bold="True" Font-Size="Large" style="text-align: right">
                                        </asp:TextBox>
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
  
                         
                     

                  </Columns>
                    <PagerStyle AlwaysVisible="True" />
                </MasterTableView>
          <ClientSettings>
                   <Selecting AllowRowSelect="true"></Selecting>
                       
           </ClientSettings>
        <FilterMenu CssClass="RadFilterMenu_CheckList"></FilterMenu>
          </telerik:RadGrid>

        <br /><br />
        </td>
   
    </tr>
        <tr>
             <td align="center">
                        <telerik:RadGrid ID="RadGridOrders" runat="server" ActiveItemStyle-BorderStyle="Solid" AllowPaging="True"  AutoGenerateColumns="False" OnNeedDataSource="RadGridOrders_NeedDataSource" PageSize="10" ShowStatusBar="True" Width="70%"  Skin="Web20" OnItemDataBound="RadGridOrders_ItemDataBound" GroupPanelPosition="Top">
               <ActiveItemStyle BorderStyle="Solid" />
               <PagerStyle Mode="NumericPages" AlwaysVisible="True" />
               <HeaderStyle CssClass="CustomHeaderView" />
             <MasterTableView AllowMultiColumnSorting="false" DataKeyNames="OrderNumber" Name="OrderNumber" Width="100%" Font-Size="Large">
             <Columns>
                          <telerik:GridBoundColumn DataField="OrderNumber" HeaderButtonType="TextButton" HeaderText="Order Number" SortExpression="OrderNumber" UniqueName="OrderNumber" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                      <HeaderStyle Font-Bold="True" Font-Size="" />

                       <ItemStyle HorizontalAlign="Center" Font-Bold="true"></ItemStyle>
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Expected Pickup Date" UniqueName="ExpectedPickupDate" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                           <telerik:RadDatePicker ID="RadDatePickerPickupDate" Font-Bold="true" Font-Size="X-Large" runat="server" OnSelectedDateChanged="RadDatePickerPickupDate_SelectedDateChanged" Enabled="false"></telerik:RadDatePicker>
                                     
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
                    <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Total" UniqueName="Total" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:TextBox ID="TextBoxTotal" runat="server" Enabled="false" Font-Bold="True" Font-Size="Large" style="text-align: right" OnTextChanged="TextBoxTotal_TextChanged">
                                        </asp:TextBox>
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
                 <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Already Paid" UniqueName="AlreadyPaid" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:TextBox ID="TextBoxAlreadyPaid" runat="server" AutoPostBack="true" OnTextChanged="TextBoxAlreadyPaid_TextChanged" Font-Bold="True" Font-Size="Large" style="text-align: right" Enabled="false">
                                        </asp:TextBox>
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
                   <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Remain Balance" UniqueName="RemainBalance" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:TextBox ID="TextBoxRemainBalance" runat="server" Enabled="false" Font-Bold="True" Font-Size="Large" style="text-align: right">
                                        </asp:TextBox>
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
  
                         
                     

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
        <td >&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
            <asp:Label ID="LabelChangeHistory" runat="server" Text="Change History:" Font-Bold="True" Font-Italic="False" Font-Size="Medium" Font-Underline="True" Visible="false"></asp:Label>
        </td>
     
    </tr>
    <tr>
        <td><asp:Literal ID="LiteralChangeHistory" runat="server"></asp:Literal></td>
     
    </tr>
</table>
          </div>     
    </asp:Content>

