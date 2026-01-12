<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvoiceMobile.aspx.cs" Inherits="SaintPolycarp.BanhChung.InvoiceMobile" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
         <asp:ScriptManager runat="server">
            <Scripts>
                <%--To learn more about bundling scripts in ScriptManager see http://go.microsoft.com/fwlink/?LinkID=301884 --%>
                <%--Framework Scripts--%>
                <asp:ScriptReference Name="MsAjaxBundle" />
                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Name="bootstrap" />
                <asp:ScriptReference Name="respond" />
                <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                <asp:ScriptReference Name="WebFormsBundle" />
                <%--Site Scripts--%>
            </Scripts>
        </asp:ScriptManager>
         <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="True">
        <Windows>
             <telerik:RadWindow ID="RadWindowPopupSize" runat="server" ShowContentDuringLoad="false" Width="700px"
                Height="300px" Title="Send SMS Text Reminder" Behaviors="Default" VisibleStatusbar="False" MaxHeight="500px" MaxWidth="800px">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <script language="javascript">

        function openRadWin_PopupSendTextReminder(title, phoneNumber, message) {
            radopen("SendSmsTextMessage.aspx?title=" + title + "&phoneNumber=" + phoneNumber + "&message=" + message, "RadWindowPopupSize");
        }
        function NumberOnly() {
            var AsciiValue = event.keyCode
            if ((AsciiValue >= 48 && AsciiValue <= 57) || (AsciiValue == 8 || AsciiValue == 127))
                event.returnValue = true;
            else
                event.returnValue = false;
        }
</script>
    <div style="text-align: center;">
         <table class="nav-justified">
             <tr><td><asp:Label ID="LabelPageTitle" runat="server" Text="" Font-Bold="true"  Font-Size="XX-Large" Font-Underline="true"></asp:Label> 
                                    </td></tr>
              <tr><td class="auto-style8"><asp:Label ID="LabelServeError" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="#FF3300"></asp:Label></td></tr>
        <br />
         
    </div>
        <div>
                <table style="width: 100%;">
                  <tr><td><asp:Label ID="Label1" runat="server" Text="RECEIPT" Font-Bold="true"  Font-Size="X-Large" Font-Underline="true"></asp:Label> 
                 </td></tr>
                  <tr>
                 <td>
                     <table style="width: 100%;">
                         <tr>
                             <td valign="top" align="left">  
                        <table style="width: 100%;">
                         <tr>
                            
                             <td align="left">
                                  <asp:Label ID="LabelCustomer" runat="server" Text="Customer Name" Enabled="false"></asp:Label>
                                 <br />
                                 <telerik:RadTextBox ID="RadTextBoxCustomerName" runat="server" Font-Size="Medium"></telerik:RadTextBox>
                                  <asp:RequiredFieldValidator runat="server" id="reqCustomerName" controltovalidate="RadTextBoxCustomerName" errormessage="Please enter customer name" Font-Bold="true" ForeColor="Red"/>
               
                                 </td>
                         </tr>
                          <tr>
                             <td align="left" valign="bottom" >
                                 <asp:Label ID="LabelPhoneNumber" runat="server" Text="Phone Number" Enabled="false"></asp:Label>
                                 <br />
                                  <telerik:RadMaskedTextBox RenderMode="Classic" runat="server" Mask="(###) ###-####" RequireCompleteText="true"  Font-Size="Medium"  ID="RadMaskedTextBoxPhoneNumber"  AutoPostBack="false">
                                    </telerik:RadMaskedTextBox>
                                 <br />
                              
                              </td>
                          </tr>
                          <tr>
                               <td align="left" valign="bottom" >
                                   <asp:Label ID="LabelInvoicePickupDate" runat="server" Text="Pickup Date" Enabled="false"></asp:Label>
                                   <br />
                                   <telerik:RadDatePicker ID="RadDatePickerInvoicePickupDate" AutoPostBack="false"  Font-Bold="true" Font-Size="X-Large" runat="server" OnSelectedDateChanged="RadDatePickerInvoicePickupDate_SelectedDateChanged" Enabled="true" MinDate="2019-01-01" DateInput-DisabledStyle-ForeColor="Red"></telerik:RadDatePicker>
                                
                                   </td>
                          </tr>
                         </table>          
                             </td>
                             <td valign="top" align="left">  
                                   <table style="width: 100%;">
                                                     <tr>
                                                <td valign="top" align="center" >  
                                                    <br /><strong>Xuân Bính Ngọ 2026</strong> 
                                                    <br />Contacts:
                                                    <br />A. Thắng (714) 553-6065
                                                    <br />C. Ái Đơn (714) 260-4798
                                                    <br />C. Hằng (714) 622-8559
                                                    <br />C. Thảo (714) 452-4684
                                                   
                                                </td>
                                                     </tr>
                              
                                            </table> 
                             </td>
                         </tr>
                         </table>
                                   
               </td>
                  </tr>
              
                    <tr>   
                   <td align="right" style= "border: solid #ff3300 medium; ">    
                        <asp:Panel ID="PanelInvoiceInfo" runat="server" Visible="false">
                              <table style="width: 100%; ">
                                  <tr>
                                      <td>
                                        <table style="width: 100%; ">
                                                         <tr>
                                                             <td align="left">
                                                             <asp:Label ID="LabelInvNum" runat="server" Text="Invoice number: "></asp:Label> 
                                                              <asp:Label ID="LabelInvoiceNumber" runat="server" Text="" Font-Bold="true" Font-Size="X-Large" ForeColor="#ff3300"></asp:Label>
                                                                 <br />
                                                                 </td>
                                                         </tr>
                                                          <tr>
                                                             <td align="left">
                                                              <asp:Label ID="LabelInvDate" runat="server" Text="Invoice Date: "></asp:Label>
                                                                 <asp:Label ID="LabelInvoiceDate" runat="server" Text=""></asp:Label>
                                                                 <br />
                                                              </td>
                                                          </tr>
                                                             <tr>
                                                             <td align="left">
                                                                  <asp:Label ID="LabelStatus" runat="server" Text="Status: "></asp:Label>  
                                                                  <asp:Label ID="LabelInvoiceStatus" runat="server" Text=""></asp:Label>  
                                                                 <br />
                                                                 </td>                          
                                                             </tr>
                                                             <tr> 
                                                                 <td align="left" class="auto-style2">
                                                                       <asp:Label ID="LabelCreatedBy" runat="server" Text="Create By: "></asp:Label>
                                                                       <asp:Label ID="LabelInvoiceCreatedBy" runat="server" Text=""></asp:Label>
                                                                       <br />
                                                                   </td>
                                                                 </tr>

                                                             <tr>
                                                                 <td align="left">
                                                                     <asp:Label ID="LabelInvoiceType" runat="server" Text="Invoice Type"></asp:Label>
                                                                     <br />
                                                                     <asp:RadioButtonList ID="RadioButtonListInvoiceType" runat="server" AutoPostBack="false" OnSelectedIndexChanged="RadioButtonListInvoiceType_SelectedIndexChanged1" Visible="false">
                                                                         <asp:ListItem Selected="True">Simple</asp:ListItem>
                                                                         <asp:ListItem Enabled="false" Value="Complex"> Complex (Multiple orders/pickup dates)</asp:ListItem>
                                                                     </asp:RadioButtonList>
                                                                 </td>
                                                             </tr>
             
                             </table>
                                      </td>
                                      <td>
                                               <asp:Image ID="ImageViewOnly" runat="server" ImageUrl="~/images/ViewOnly.png"  Height="80px" Width="97px" Visible="false" />
                                      </td>
                                  </tr>
                            
                        </asp:Panel>
                   </td>            
                </tr>
            
           </table>

        <br />
      <div id="div_print2">               
    <table style="width: 100%;">
   
    <tr>
        <td align="center">
         <style type="text/css">
            .CustomHeaderForSaleItemsTable
            {
                background-color:#ff3300 !important;
                background-image:none !important;
            }
            .CustomHeaderForOrdersTable
            {
                background-color:darkorange !important;
                background-image:none !important;
            }
            
             .auto-style8 {
                 height: 23px;
             }
            
             </style>  
            <telerik:RadGrid ID="RadGridSaleItems" runat="server" ActiveItemStyle-BorderStyle="Solid" AutoGenerateColumns="False" OnNeedDataSource="RadGridSaleItems_NeedDataSource" AllowPaging="True" PageSize="10" ShowStatusBar="True" Width="100%"  Skin="Web20" OnItemDataBound="RadGridSaleItems_ItemDataBound" GroupPanelPosition="Top">
               <ActiveItemStyle BorderStyle="Solid" />
               <PagerStyle Mode="NumericPages" AlwaysVisible="True" />
               <HeaderStyle CssClass="CustomHeaderForSaleItemsTable" />
             <MasterTableView AllowMultiColumnSorting="false" DataKeyNames="Index" Name="Index" Width="100%" Font-Size="Large">
             <Columns>
                          <telerik:GridBoundColumn DataField="Index" HeaderButtonType="TextButton" HeaderText="" SortExpression="Index" UniqueName="Index" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                      <HeaderStyle Font-Bold="True" Font-Size="" />

                       <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="PickupDate" HeaderButtonType="TextButton" Display="false" HeaderText="Pickup Date" SortExpression="PickupDate" UniqueName="PickupDate" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                      <HeaderStyle Font-Bold="True" Font-Size="" />

                       <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </telerik:GridBoundColumn>
                       <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Picked Up" Display="false" UniqueName="PickedUp" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:ImageButton ID="ImageButtonPickedUp" runat="server" ImageUrl="~/images/pickedup_yes.png" Width="22px" Height="22px" OnClick="ImageButtonPickedUp_Click" Enabled="false" Visible="false" />
                                        <asp:ImageButton ID="ImageButtonNotPickedUp" runat="server" ImageUrl="~/images/pickedup_no.png" Width="20px" Height="20px" OnClick="ImageButtonNotPickedUp_Click" Enabled="false" Visible="false"   />
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
                            
                      <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Order Number" UniqueName="OrderNumber" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" Visible="false" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:DropDownList ID="DropDownListOrderNumber" runat="server" AutoPostBack="false" OnSelectedIndexChanged="DropDownListOrderNumber_SelectedIndexChanged" Enabled="false" Font-Bold="True" Font-Size="Large" style="text-align: left" Width="50%"></asp:DropDownList>
                                    </ItemTemplate> 

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn>
       
           
                      <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Sale Item" UniqueName="SaleItem" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="30%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:DropDownList ID="DropDownListSaleItem" runat="server" Visible="false" AutoPostBack="false" OnSelectedIndexChanged="DropDownListSaleItem_SelectedIndexChanged" Font-Bold="True" Font-Size="Large" style="text-align: left" Width="80%"></asp:DropDownList>
                                           <telerik:RadDropDownList ID="RadDropDownListSaleItem" runat="server" Font-Bold="true" Font-Size="Small" Width="80%">
                                                
                                           </telerik:RadDropDownList>
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
                    <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Quantity" UniqueName="Quantity" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:TextBox ID="TextBoxQuantity" runat="server" AutoPostBack="false" Enabled="true" onkeypress="return NumberOnly()" OnTextChanged="TextBoxQuantity_TextChanged" Font-Bold="True" Font-Size="Large" style="text-align: right" Width="80%">
                                        </asp:TextBox>
                           </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
                   <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Unit Price" UniqueName="UnitPrice" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate>  
                                        <asp:TextBox ID="TextBoxUnitPrice" runat="server" BackColor="#c0c0c0" Enabled="false" Font-Bold="True" Font-Size="Medium" style="text-align: right" Width="80%">
                                        </asp:TextBox>
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
                 <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Discount Per Unit" Display="false" UniqueName="Discount" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:TextBox ID="TextBoxDiscount" runat="server" AutoPostBack="false" Enabled="false" OnTextChanged="TextBoxDiscount_TextChanged" Font-Bold="True" Font-Size="Large" style="text-align: right" Width="80%">
                                        </asp:TextBox>
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
                   <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Sub-Total" UniqueName="SubTotal" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:TextBox ID="TextBoxSubTotal" runat="server" BackColor="#c0c0c0" Enabled="false" Font-Bold="True" Font-Size="Medium" style="text-align: right" Width="80%">
                                        </asp:TextBox>
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
  
                         
                     

                  </Columns>
                    <PagerStyle AlwaysVisible="True" />
                </MasterTableView>
          <ClientSettings Selecting-AllowRowSelect="true" EnableRowHoverStyle="true">
                   <Scrolling AllowScroll="false" UseStaticHeaders="true" />    
           </ClientSettings>
        <FilterMenu CssClass="RadFilterMenu_CheckList"></FilterMenu>
          </telerik:RadGrid>

        <br /><br />
        </td>
   
    </tr>
        <tr>
             <td align="center">
                        <telerik:RadGrid ID="RadGridOrders" runat="server" ActiveItemStyle-BorderStyle="Solid" AllowPaging="True"  AutoGenerateColumns="False" OnNeedDataSource="RadGridOrders_NeedDataSource" PageSize="10" ShowStatusBar="True" Width="70%"  Skin="Web20" OnItemDataBound="RadGridOrders_ItemDataBound" GroupPanelPosition="Top" Visible="false">
               <ActiveItemStyle BorderStyle="Solid" />
               <PagerStyle Mode="NumericPages" AlwaysVisible="True" />
               <HeaderStyle CssClass="CustomHeaderForOrdersTable" />
             <MasterTableView AllowMultiColumnSorting="false" DataKeyNames="OrderNumber" Name="OrderNumber" Width="100%" Font-Size="Large">
             <Columns>
                          <telerik:GridBoundColumn DataField="OrderNumber" HeaderButtonType="TextButton" HeaderText="Order Number" SortExpression="OrderNumber" UniqueName="OrderNumber" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                      <HeaderStyle Font-Bold="True" Font-Size="" />

                       <ItemStyle HorizontalAlign="Center" Font-Bold="true"></ItemStyle>
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Pickup Date" UniqueName="PickupDate" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                           <telerik:RadDatePicker ID="RadDatePickerPickupDate" AutoPostBack="false"  Font-Bold="true" Font-Size="X-Large" runat="server" OnSelectedDateChanged="RadDatePickerPickupDate_SelectedDateChanged" Enabled="false"></telerik:RadDatePicker>
                                     
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
                                        <asp:TextBox ID="TextBoxAlreadyPaid" runat="server" AutoPostBack="false" OnTextChanged="TextBoxAlreadyPaid_TextChanged" Font-Bold="True" Font-Size="Large" style="text-align: right" Enabled="false">
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
          <ClientSettings  EnableRowHoverStyle="true">
                   <Selecting AllowRowSelect="true"></Selecting>
                       
           </ClientSettings>
        <FilterMenu CssClass="RadFilterMenu_CheckList"></FilterMenu>
          </telerik:RadGrid>
            </td>
        </tr>
        <tr>
            <td align="center">
                 <asp:Panel ID="PanelSummary" runat="server" Visible="true">
                  <table style="width: 80%;">
                      <tr>
                          <td>
                               <table style="width: 100%;">
                                   <tr>
                                       <td>
                                              <asp:Label ID="LabelNotes" Text="Notes:" runat="server"> </asp:Label> <br />
                                                <asp:TextBox ID="TextBoxNote" runat="server" TextMode="MultiLine" Height="80px" Width="200px"></asp:TextBox>  
                                       </td>
                                  
                                    <td>
                              <asp:Panel ID="PanelSummaryBalance" runat="server" Visible="true">
                                            <table  style="width: 100%;">
                                                                    <tr>
                                                              <td align="right">
                                                                  <asp:Label ID="LabelGrandTotal" Text="Grand Total:" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                                                              </td> 
                                                             <td><asp:TextBox ID="TextBoxGrandTotal" runat="server" style="text-align: right" Font-Bold="true" ForeColor="#ff3300" Font-Size="Large" BackColor="#c0c0c0" Enabled="false" Width="100px"></asp:TextBox> </td> 
                                                         </tr>
                                                     <tr>
                                                         <td align="right"><asp:Label ID="LabelTotalAlreadyPaid" Text="Already Paid" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label> </td>
                                                         <td><asp:TextBox ID="TextBoxTotalAlreadyPaid" runat="server" style="text-align: right" Font-Bold="true" ForeColor="#ff3300" Font-Size="Large" BackColor="#c0c0c0" Width="100px"  Enabled="false" OnTextChanged="TextBoxTotalAlreadyPaid_TextChanged" ></asp:TextBox> </td> 

                                                     </tr>
                                                          <tr>
                                                         <td align="right"><asp:Label ID="LabelTotalRemainBalance" Text="Remain Balance" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label> </td>
                                                         <td><asp:TextBox ID="TextBoxTotalRemainBalance" runat="server" style="text-align: right" Font-Bold="true" ForeColor="#ff3300" Font-Size="Large" BackColor="#c0c0c0" Enabled="false" Width="100px"></asp:TextBox> </td> 

                                                     </tr>
                                                      </table>
                                  </asp:Panel>
                            </td>
                      </tr>
                                   </table>
                          </td>
                       </tr>
                  
                       <tr>
                           <td align="center" valign="middle">
                                 <table style="width: 100%;">
                                               <tr>
                                                  
                                                <td>    <asp:Button ID="ButtonSave" runat="server" Text="Save" Visible="false" Width="100px"  OnClick="ButtonSave_Click" BackColor="Orange" BorderStyle="Double" BorderColor="Blue" /> &emsp; </td>
                                                
                           </tr>
               
                                                 </table>
                  </asp:Panel>
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
        </div>
    </form>
</body>
</html>
