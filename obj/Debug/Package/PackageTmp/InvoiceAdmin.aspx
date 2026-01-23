<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="InvoiceAdmin.aspx.cs" Inherits="SaintPolycarp.BanhChung.InvoiceAdmin" %>
<%@ MasterType VirtualPath="~/Site.master" %> 
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="True">
        <Windows>
             <telerik:RadWindow ID="RadWindowPopupSize" runat="server" ShowContentDuringLoad="false" Width="900px"
                Height="600px" Title="Send SMS Text Reminder" Behaviors="Default" VisibleStatusbar="False" MaxHeight="800px" MaxWidth="1200px">
            </telerik:RadWindow>
             <telerik:RadWindow ID="RadWindowPopupPrintInvoice" runat="server" ShowContentDuringLoad="false" Width="1000px"
                Height="600px" Title="Print Invoice" Behaviors="Default" VisibleStatusbar="True" MaxHeight="1000px" MaxWidth="1400px">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <script language="javascript">
        function CancelledInvoiceConfirmation() {
            var invoice = document.getElementById('<%=TextBoxInvoiceNumber.ClientID%>').value;

            return confirm("Are you sure you want to cancel the invoice #" + invoice + "? It will not be reversible.");
        }
        function SetTarget() {

            document.forms[0].target = "_blank";

        }
        function printdiv(printcustomer, printpickupdate, printinvoicenumber, printpage2) {
            //Chau's note: take time to figure this out
            //before print all the content out from "document", need to remove the Button
            //The buttonID is "ButtonSaveInvoice". However, since it's inside of ConsoleContent of master page
            //the ID is "ConsoleContent_ButtonSaveInvoice". Take a while to realize this. Need to use F12 in Chrome and Element tab so view the element name 
            if (document.all.item('ConsoleContent_ButtonSaveInvoice')) { //if found the element then remove
                var myImage = document.getElementById('ConsoleContent_ButtonSaveInvoice');
                var linkElement = myImage.parentNode;
                linkElement.removeChild(myImage); //remove the element
            }
            if (document.all.item('ConsoleContent_ButtonPickupAll')) { //if found the element then remove
                var myImage = document.getElementById('ConsoleContent_ButtonPickupAll');
                var linkElement = myImage.parentNode;
                linkElement.removeChild(myImage); //remove the element
            }
            if (document.all.item('ConsoleContent_ButtonPaidAll')) { //if found the element then remove
                var myImage = document.getElementById('ConsoleContent_ButtonPaidAll');
                var linkElement = myImage.parentNode;
                linkElement.removeChild(myImage); //remove the element
            }
            if (document.all.item('ConsoleContent_ButtonCancelInvoice')) { //if found the element then remove
                var myImage = document.getElementById('ConsoleContent_ButtonCancelInvoice');
                var linkElement = myImage.parentNode;
                linkElement.removeChild(myImage); //remove the element
            }
            if (document.all.item('ConsoleContent_ButtonSaveInvoice')) { //if found the element then remove
                var myImage = document.getElementById('ConsoleContent_ButtonSaveInvoice');
                var linkElement = myImage.parentNode;
                linkElement.removeChild(myImage); //remove the element
            }
            if (document.all.item('ConsoleContent_ButtonSaveInvoice')) { //if found the element then remove
                var myImage = document.getElementById('ConsoleContent_ButtonSaveInvoice');
                var linkElement = myImage.parentNode;
                linkElement.removeChild(myImage); //remove the element
            }
            if (document.all.item('ConsoleContent_ButtonPrintInvoice')) { //if found the element then remove
                var myImage = document.getElementById('ConsoleContent_ButtonPrintInvoice');
                var linkElement = myImage.parentNode;
                linkElement.removeChild(myImage); //remove the element
            }


            //Javascript found here https://forums.asp.net/t/1261525.aspx?How+to+print+DIV+contents+only
            var headstr = "<html><head><title></title></head><body>";
            var footstr = "</body>";

            var headerLabel = "<table style='width: 100%;'><tr><td class='text-center' style='text-decoration: underline; font-size: x-large'><strong>CUSOMTER RECEIPT</strong></td></tr></table></br>"

            var customerSection = document.all.item(printcustomer).innerHTML;
            var pickupDateSection = document.all.item(printpickupdate).innerHTML;
            var printInvoiceNumber = document.all.item(printinvoicenumber).innerHTML;
            var topSection = " <table><tr><td>" + customerSection + "</td><td>" + pickupDateSection + "</td><td>" + printInvoiceNumber + "</td></tr></table></br>"
            var newstr2 = document.all.item(printpage2).innerHTML;
            //var newstr2 = "";
            var oldstr = document.body.innerHTML;
            document.body.innerHTML = headstr + headerLabel + topSection + newstr2 + footstr;
            //the ID is "ConsoleContent_ButtonSaveInvoice". Take a while to realize this. Need to use F12 in Chrome and Element tab so view the element name 

            window.print();
            document.body.innerHTML = oldstr;
            return false;
        }
        function openRadWin_PopupSendTextReminder(title, phoneNumber, invoiceNumber) {
            radopen("SendSmsTextMessage.aspx?title=" + title + "&phoneNumber=" + phoneNumber + "&invoiceNumber=" + invoiceNumber, "RadWindowPopupSize");
        }
        function openRadWin_PopupPrintInvoice(invoiceNumber) {
            radopen("InvoicePrint.aspx?InvoiceNumber=" + invoiceNumber, "RadWindowPopupPrintInvoice");
        }
</script>
    <div style="text-align: center;">
         <table class="nav-justified">
             <tr><td><asp:Label ID="LabelPageTitle" runat="server" Text="" Font-Bold="true"  Font-Size="XX-Large" Font-Underline="true"></asp:Label> 
                                    </td></tr>
              <tr><td class="auto-style8"><asp:Label ID="LabelServeError" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="#FF3300"></asp:Label></td></tr>
             </table>
        <br />
         
    </div>
   
       
           <table style="width: 100%;">
           <tr>
               <td>
                      <div id="div_print_customer">
                     <table style="width: 100%;">
                         <tr>
                             <td  align="right">
                             <asp:Label ID="LabelCustomer" runat="server" Text="Customer Name"></asp:Label>

                             </td>
                             <td align="left">
                                 <telerik:RadTextBox ID="RadTextBoxCustomerName" runat="server"></telerik:RadTextBox>
                                  <asp:RequiredFieldValidator runat="server" id="reqCustomerName" controltovalidate="RadTextBoxCustomerName" errormessage="Please enter customer name" Font-Bold="true" ForeColor="Red"/>
               
                                 </td>
                         </tr>
                          <tr><td  align="right">
                             <asp:Label ID="LabelPhoneNumber" runat="server" Text="Phone Number"></asp:Label>

                              </td>
                             <td align="left" valign="bottom" >
                                  <telerik:RadMaskedTextBox RenderMode="Classic" runat="server" Mask="(###) ###-####"  Font-Size="Medium"  ID="RadMaskedTextBoxPhoneNumber" OnTextChanged="RadMaskedTextBoxPhoneNumber_TextChanged" AutoPostBack="true">
                                    </telerik:RadMaskedTextBox>
                                 <br />
                                
                                 <asp:Label ID="LabelSendTextReminder" runat="server" Font-Bold="true" Font-Underline="true" Text="Send Text Reminder"></asp:Label>
                              </td>
                          </tr>
                         <tr><td></td></tr>
                         </table>
                 
                       </div>
               </td>
                  <td>
                       <div id="div_print_pickupdate">
                         <table style="width: 100%;">      
                                       <tr>
                                 <td>
                                    <asp:Label ID="LabelInvoicePickupDate" runat="server" Text="Pickup Date "></asp:Label></td>
                                 <td align="left"><telerik:RadDatePicker ID="RadDatePickerInvoicePickupDate" AutoPostBack="false"  Font-Bold="true" Font-Size="X-Large" runat="server" OnSelectedDateChanged="RadDatePickerInvoicePickupDate_SelectedDateChanged" Enabled="true" MinDate="2019-01-01"></telerik:RadDatePicker>
                               
                                 </td>
                             </tr>
                               <tr><td></td><td></td></tr>
          
                             <tr>
                                 <td valign="top">
                                   <asp:Label ID="LabelInvoiceType" runat="server" Text="Invoice Type"></asp:Label>
                                 </td>
                             <td align="left">
                                 <asp:RadioButtonList ID="RadioButtonListInvoiceType" runat="server" OnSelectedIndexChanged="RadioButtonListInvoiceType_SelectedIndexChanged1" AutoPostBack="true">
                                     <asp:ListItem Selected="True">Simple</asp:ListItem>
                                     <asp:ListItem Value="Complex">Complex (Multiple orders/pickup dates)</asp:ListItem>
                                 </asp:RadioButtonList>
                                   </td>
                             </tr>
                   
                             <tr><td></td><td></td></tr>
                         </table>
                       </div>
               </td>
                  <td align="right" style= "border: solid #0040ff medium; ">
                        <div id="div_print_invoicenumber">
                         <table style="width: 100%; ">
                         <tr>
                             <td  align="right">
                             <asp:Label ID="LabelInvoiceNumber" runat="server" Text="Invoice #"></asp:Label>

                             </td>
                             <td align="left">
                             <asp:TextBox ID="TextBoxInvoiceNumber" Width="90%" runat="server" Enabled="false"></asp:TextBox>
                                 </td>
                         </tr>
                          <tr>
                              <td  align="right">
                             <asp:Label ID="LabelInvoiceDate" runat="server" Text="Invoice Date"></asp:Label>

                              </td>
                             <td align="left">
                             <asp:TextBox ID="TextBoxInvoiceDate" Width="90%" runat="server" Enabled="false"></asp:TextBox>
                              </td>
                          </tr>
                               <tr>
                              <td  align="right">
                             <asp:Label ID="LabelCreatedBy" runat="server" Text="Created By"></asp:Label>

                              </td>
                             <td align="left">
                             <asp:TextBox ID="TextBoxCreatedBy" Width="90%" runat="server" Enabled="false"></asp:TextBox>
                              </td>
                          </tr>
                              <tr>
                              <td  align="right">
                             <asp:Label ID="LabelLastUpdatedBy" runat="server" Text="Last Updated"></asp:Label>

                              </td>
                             <td align="left">
                             <asp:TextBox ID="TextBoxLastUpdatedBy" Width="90%" runat="server" Enabled="false"></asp:TextBox>
                              </td>
                          </tr>
                             <tr><td  align="right">
                                   <asp:Label ID="LabelStatus" runat="server" Text="Status"></asp:Label>
                                 </td>
                             <td align="left">
                                        <asp:TextBox ID="TextBoxInvoiceStatus" Width="90%" runat="server" Enabled="false"></asp:TextBox>
                                 </td>
                             </tr>
                             <tr><td></td><td></td></tr>

                         </table>
                        </div>
               </td>
             
           </tr>
           </table>
    
      
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ConsoleContent" runat="server">
        <br />
      <div id="div_print2">               
    <table style="width: 100%;">
   
    <tr>
        <td align="center">
         <style type="text/css">
            .CustomHeaderForSaleItemsTable
            {
                background-color:#0040ff !important;
                background-image:none !important;
            }
            .CustomHeaderForOrdersTable
            {
                background-color:#0040ff !important;
                background-image:none !important;
            }
            
             .auto-style8 {
                 height: 23px;
             }
              .notes-style {
                 max-width: 100%;
             }
            
             </style>  
            <telerik:RadGrid ID="RadGridSaleItems" runat="server" ActiveItemStyle-BorderStyle="Solid" AutoGenerateColumns="False" AllowPaging="False" PageSize="20" OnNeedDataSource="RadGridSaleItems_NeedDataSource" ShowStatusBar="True" Width="95%"  Skin="Web20" OnItemDataBound="RadGridSaleItems_ItemDataBound" GroupPanelPosition="Top">
               <ActiveItemStyle BorderStyle="Solid" />
               <PagerStyle Mode="NumericPages" AlwaysVisible="True" />
               <HeaderStyle CssClass="CustomHeaderForSaleItemsTable" />
             <MasterTableView AllowMultiColumnSorting="false" DataKeyNames="Index" Name="Index" Width="100%" Font-Size="Large">
             <Columns>
                          <telerik:GridBoundColumn DataField="Index" HeaderButtonType="TextButton" HeaderText="" SortExpression="Index" UniqueName="Index" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                      <HeaderStyle Font-Bold="True" Font-Size="" />

                       <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="PickupDate" HeaderButtonType="TextButton" HeaderText="Pickup Date" SortExpression="PickupDate" UniqueName="PickupDate" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                      <HeaderStyle Font-Bold="True" Font-Size="" />

                       <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </telerik:GridBoundColumn>
                       <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Picked Up" UniqueName="PickedUp" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >  
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
       
           
                      <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Sale Item" UniqueName="SaleItem" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="25%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:DropDownList ID="DropDownListSaleItem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListSaleItem_SelectedIndexChanged" Font-Bold="True" Font-Size="Large" style="text-align: left" Width="80%"></asp:DropDownList>
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
                    <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Quantity" UniqueName="Quantity" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:TextBox ID="TextBoxQuantity" runat="server" AutoPostBack="true" Enabled="true" OnTextChanged="TextBoxQuantity_TextChanged" Font-Bold="True" Font-Size="Large" style="text-align: right" Width="80%">
                                        </asp:TextBox>
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
                   <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Unit Price" UniqueName="UnitPrice" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate>  
                                        <asp:TextBox ID="TextBoxUnitPrice" runat="server" Enabled="false" Font-Bold="True" Font-Size="Large" style="text-align: right" Width="80%">
                                        </asp:TextBox>
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
                 <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Discount Per Unit" UniqueName="Discount" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:TextBox ID="TextBoxDiscount" runat="server" AutoPostBack="true" Enabled="true" OnTextChanged="TextBoxDiscount_TextChanged" Font-Bold="True" Font-Size="Large" style="text-align: right" Width="80%">
                                        </asp:TextBox>
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
                   <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Sub-Total" UniqueName="SubTotal" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:TextBox ID="TextBoxSubTotal" runat="server" Enabled="false" Font-Bold="True" Font-Size="Large" style="text-align: right" Width="80%">
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
                        <telerik:RadGrid ID="RadGridOrders" runat="server" ActiveItemStyle-BorderStyle="Solid" AllowPaging="False"  AutoGenerateColumns="False" OnNeedDataSource="RadGridOrders_NeedDataSource" PageSize="20" ShowStatusBar="True" Width="85%"  Skin="Web20" OnItemDataBound="RadGridOrders_ItemDataBound" GroupPanelPosition="Top" Visible="false">
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
                                           <telerik:RadDatePicker ID="RadDatePickerPickupDate" AutoPostBack="true"  Font-Bold="true" Font-Size="X-Large" runat="server" OnSelectedDateChanged="RadDatePickerPickupDate_SelectedDateChanged" Enabled="false"></telerik:RadDatePicker>
                                     
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
                 <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Already Paid By Cash" UniqueName="AlreadyPaid" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:TextBox ID="TextBoxAlreadyPaid" runat="server" AutoPostBack="true" OnTextChanged="TextBoxAlreadyPaid_TextChanged" Font-Bold="True" Font-Size="Large" style="text-align: right" Enabled="false">
                                        </asp:TextBox>
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
                     <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="Already Paid By Check" UniqueName="AlreadyPaidByCheck" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:TextBox ID="TextBoxAlreadyPaidByCheck" runat="server" AutoPostBack="true" OnTextChanged="TextBoxAlreadyPaidByCheck_TextChanged" Font-Bold="True" Font-Size="Large" style="text-align: right" Enabled="false">
                                        </asp:TextBox>
                                    </ItemTemplate> 

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridTemplateColumn> 
                      <telerik:GridTemplateColumn HeaderButtonType="TextButton" HeaderText="CheckNumber" UniqueName="CheckNumber" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                    <ItemTemplate> 
                                        <asp:TextBox ID="TextBoxCheckNumber" runat="server" AutoPostBack="false" Font-Bold="True" Font-Size="Large" style="text-align: right" Enabled="false">
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
       
        </table>
        <table style="width: 100%;">
        <tr>
            <td align="center">
                 <asp:Panel ID="PanelSummary" runat="server" Visible="true">
                  <table>
                      <tr>
                       
                           <td align="center" valign="middle">
                                 <table style="width: 100%;">
                                               <tr>
                                                   <td>    <asp:Button ID="ButtonRefreshPage" runat="server" Text="Update Page" Width="150px"  OnClick="ButtonRefreshPage_Click" BackColor="Orange" BorderStyle="Double" BorderColor="Blue" /> &emsp; </td>  
                                                <td>    <asp:Button ID="ButtonPickupAll" runat="server" Text="Pickup All" Width="150px" OnClick="ButtonPickupAll_Click" BackColor="Orange" BorderStyle="Double" BorderColor="Blue"/> &emsp; </td>
                                                 <td>    <asp:Button ID="ButtonPaidAll" runat="server" Text="Paid All" Width="150px" OnClick="ButtonPaidAll_Click" BackColor="Orange" BorderStyle="Double" BorderColor="Blue" /> &emsp; </td>
                                                    <td>    | &emsp; </td>
                                                 <td>    <asp:Button ID="ButtonSaveInvoice" runat="server" Text="Save Invoice" Width="150px"  OnClick="ButtonSaveInvoice_Click" ForeColor="White" BackColor="Green" BorderStyle="Double" BorderColor="Blue" /> &emsp; </td>
                                                <td>     <asp:Button ID="ButtonPrintInvoice" runat="server" Text="Print Invoice" Width="150px" ForeColor="White" BackColor="Green" BorderStyle="Double" BorderColor="Blue" OnClick="ButtonPrintInvoice_Click" OnClientClick = "SetTarget();"  /> &emsp; </td>
                                                  <td>    | &emsp; </td>
                                                <td>    <asp:Button ID="ButtonCancelInvoice" runat="server" Text="Cancel Invoice" Width="150px" ForeColor="White" BackColor="Red" BorderStyle="Double" BorderColor="Blue" OnClick="ButtonCancelInvoice_Click" OnClientClick="return CancelledInvoiceConfirmation();"/> &emsp; </td>    
                                        
                          </td>   
                                                
                                              </tr>
                                                 </table>
                  </asp:Panel>
              </td>
            <td align="right">
                  <asp:Panel ID="PanelSummaryBalance" runat="server" Visible="true">
                          <table  style="width: 100%;">
                              <tr>
                                  <td>
                                                <table  style="width: 100%;">
                                                                <tr>
                                                          <td align="right"><asp:Label ID="LabelGrandTotal" Text="Grand Total" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label> </td> 
                                                         <td><asp:TextBox ID="TextBoxGrandTotal" runat="server" style="text-align: right" Font-Bold="true" Font-Size="Large" Enabled="false" Width="100px"></asp:TextBox> </td> 
                                                             </tr>
                                                         <tr>
                                                             <td align="right"><asp:Label ID="LabelTotalAlreadyPaid" Text="Total Paid By Cash" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label> </td>
                                                             <td><asp:TextBox ID="TextBoxTotalAlreadyPaid" runat="server" style="text-align: right" Font-Bold="true" Font-Size="Large" Width="100px" Enabled="true" OnTextChanged="TextBoxTotalAlreadyPaid_TextChanged" AutoPostBack="true"></asp:TextBox> </td> 

                                                         </tr>
                                                       <tr>
                                                             <td align="right"><asp:Label ID="LabelTotalAlreadyPaidByCheck" Text="Total Paid By Check" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label> </td>
                                                             <td><asp:TextBox ID="TextBoxTotalAlreadyPaidByCheck" runat="server" style="text-align: right" Font-Bold="true" Font-Size="Large" Width="100px" Enabled="true" OnTextChanged="TextBoxTotalAlreadyPaidByCheck_TextChanged" AutoPostBack="true"></asp:TextBox> </td> 

                                                         </tr>
                                              
                                                              <tr>
                                                             <td align="right"><asp:Label ID="LabelTotalRemainBalance" Text="Remain Balance" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label> </td>
                                                             <td><asp:TextBox ID="TextBoxTotalRemainBalance" runat="server" style="text-align: right" Font-Bold="true" Font-Size="Large" Enabled="false" Width="100px"></asp:TextBox> </td> 

                                                         </tr>
                                                          </table>
                                  </td>
                                  <td>
                                             <table  style="width: 100%;">
                                                           <tr>
                                                              <td align="right"></td> 
                                                             <td> </td> 
                                                         </tr>
                                                     <tr>
                                                         <td align="right"></td>
                                                         <td></td> 

                                                     </tr>
                                                     <tr>
                                                             <td><asp:TextBox ID="TextBoxGrandCheckNumber" runat="server" Visible="false" style="text-align: right" Font-Bold="true" Font-Size="Large" Width="100px" Enabled="true" ></asp:TextBox> </td> 

                                                             <td align="left"><asp:Label ID="LabelGrandCheckNumber" Text="Check Number" runat="server" Visible="false" Font-Bold="true" Font-Size="Large"></asp:Label> </td>
                                                  
                                                        </tr>
                                     
                                                          <tr>
                                                         <td align="right"></td>
                                                         <td></td> 

                                                     </tr>
                                          </table>
                                  </td>
                              </tr>
                           </table>
                      
                      </asp:Panel>
             </td>
         </tr>
       <tr>

              <td align="center">
                               <table style="width: 100%;">
                                   <tr>
                                       <td align="center">
                                           <asp:Label ID="LabelNotes" Text="Notes:" runat="server"> </asp:Label> <br />
                                                <asp:TextBox ID="TextBoxNote" class="notes-style" runat="server" TextMode="MultiLine" Height="300px" Width="800px" ></asp:TextBox>  
                                       </td>
                                   </tr>
                                   </table>
                          </td>
           <td><asp:Label ID="temp" Text="" ></asp:Label></td>

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

