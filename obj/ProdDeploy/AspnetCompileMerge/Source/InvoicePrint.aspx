<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="InvoicePrint.aspx.cs" Inherits="SaintPolycarp.BanhChung.InvoicePrint" %>
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
              function printpage() {
                  window.print();
              }
              function openRadWin_PopupSendTextReminder(title, phoneNumber, message) {
                  radopen("SendSmsTextMessage.aspx?title=" + title + "&phoneNumber=" + phoneNumber + "&message=" + message, "RadWindowPopupSize");
              }
              function openRadWin_PopupPrintInvoice(invoiceNumber) {
                  radopen("InvoicePrint.aspx?InvoiceNumber=" + invoiceNumber, "RadWindowPopupPrintInvoice");
              }
</script>
   
           <table style="width: 100%;">
           <tr>
                <td valign="top">
                           <table style="width: 100%;">
                            
                               <tr>
                                   <td>
                                   <table style="width: 100%;">
                                        <tr>
                                            <td valign="top" align="left">  
                                                    
                                                  <table style="width: 100%;">
                                                     <tr>
                                                <td valign="top" align="center" >  
                                                    <br /><strong>Xuân Bính Ngọ 2026</strong> 
                                                    <br />Liên Lạc:
                                                    <br />A. Thắng (714) 553-6065
                                                    <br />C. Ái Đơn (714) 260-4798
                                                    <br />C. Hòa (714) 702-3171
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
                  
                              </table>
               </td>
               <td valign="middle">
                      <div id="div_print_customer">
                     <table style="width: 100%;">
                         <tr>
                             <td  align="left" style= "border: 2px solid; " class="auto-style8">
                             <asp:Label ID="LabelCustomer" runat="server" Text="Khách hàng: " Font-Size="Medium"></asp:Label>

                             </td>
                           </tr>
                          <tr><td  align="left" style= "border: 2px solid; ">
                             <asp:Label ID="LabelPhoneNumber" runat="server" Text="Số phone: " Font-Size="Medium"></asp:Label>

                              </td>
                           </tr>
                         <tr>
                              <td  align="left" style= "border: 2px solid; ">
                             <asp:Label ID="LabelCreatedBy" runat="server" Text="Người nhận hàng: " Font-Size="Medium"></asp:Label>

                              </td>
                            
                          </tr>
                            
                         
                         </table>
                 
                       </div>
               </td>
                
                  <td valign="middle">
                        <div id="div_print_invoicenumber">
                         <table style="width: 100%; ">
                         <tr>
                              
                             <td  align="left" style= "border: 2px solid; ">
                             <asp:Label ID="LabelInvoiceNumber" runat="server" Text="Hóa đơn #" Font-Size="Medium"></asp:Label>

                             </td>
                            
                         </tr>
                          <tr>
                              <td  align="left" style= "border: 2px solid; ">
                             <asp:Label ID="LabelInvoiceDate" runat="server" Text="Ngày mở: " Font-Size="Medium"></asp:Label>

                              </td>
                            
                          </tr>                                                       
                              
                               <tr><td  align="left" style= "border: 2px solid; ">
                                   <asp:Label ID="LabelStatus" runat="server" Text="Thông số: " Font-Size="Medium"></asp:Label>
                                 </td>
                           
                             </tr>
                           
                      

                         </table>
                        </div>
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
            @media print {
             .noPrint { display:none; }
}
             </style>
             <table class="nav-justified">
             <tr><td><asp:Label ID="LabelPageTitle" runat="server" Text="HÓA ĐƠN" Font-Bold="true"  Font-Size="X-Large" Font-Underline="true"></asp:Label> 
                                    </td></tr>
              <tr><td class="auto-style8"><asp:Label ID="LabelServeError" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="#FF3300"></asp:Label></td></tr>
             </table>

            <br />

            <telerik:RadGrid ID="RadGridSaleItems" runat="server" ActiveItemStyle-BorderStyle="Solid" AutoGenerateColumns="False" AllowPaging="false" PageSize="10" OnNeedDataSource="RadGridSaleItems_NeedDataSource" ShowStatusBar="True" Width="100%"  Skin="Web20" OnItemDataBound="RadGridSaleItems_ItemDataBound" GroupPanelPosition="Top">
               <ActiveItemStyle BorderStyle="Solid" />
               <PagerStyle Mode="NumericPages" AlwaysVisible="False" />
             <MasterTableView AllowMultiColumnSorting="false" DataKeyNames="Index" Name="Index" Width="100%" Font-Size="Large">
             <Columns>
                          <telerik:GridBoundColumn DataField="Index" HeaderButtonType="TextButton" HeaderText="" SortExpression="Index" UniqueName="Index" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                      <HeaderStyle Font-Bold="True" Font-Size="" />

                       <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                    </telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="SaleItem" HeaderButtonType="TextButton" HeaderText="Tên Mặt Hàng" UniqueName="SaleItem" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="25%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="Medium" />        
                             
                            <ItemStyle HorizontalAlign="Left" Font-Size="Small"></ItemStyle>
                                </telerik:GridBoundColumn> 
                  
                     <telerik:GridBoundColumn DataField="PickupDate" HeaderButtonType="TextButton" HeaderText="Ngày Lẩy" SortExpression="PickupDate" UniqueName="PickupDate" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                      <HeaderStyle Font-Bold="True" Font-Size="Medium"  />

                       <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                    </telerik:GridBoundColumn>
                       <telerik:GridBoundColumn DataField="PickedUp" HeaderButtonType="TextButton" HeaderText="Lấy Hàng" UniqueName="PickedUp" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="Medium" />        
                            
                        <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                </telerik:GridBoundColumn> 
                            
                      <telerik:GridBoundColumn  DataField="OrderNumber" HeaderButtonType="TextButton" HeaderText="Order Number" UniqueName="OrderNumber" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" Visible="false" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                               
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridBoundColumn>
       
           
                    <telerik:GridBoundColumn DataField="Quantity" HeaderButtonType="TextButton" HeaderText="Số Lượng" UniqueName="Quantity" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="Medium" />        
                                
                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                </telerik:GridBoundColumn> 
                   <telerik:GridBoundColumn DataField="UnitPrice" HeaderButtonType="TextButton" HeaderText="Giá" UniqueName="UnitPrice" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="Medium" />        
                                 
                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                </telerik:GridBoundColumn> 
                 <telerik:GridBoundColumn DataField="DiscountPerUnit" HeaderButtonType="TextButton" HeaderText="Giảm Giá" UniqueName="DiscountPerUnit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="Medium" />        
                                  
                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                </telerik:GridBoundColumn> 
                   <telerik:GridBoundColumn DataField="SubTotal" HeaderButtonType="TextButton" HeaderText="Tổng Giá" UniqueName="SubTotal" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="Medium" />        
       
                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                </telerik:GridBoundColumn> 
  
                         
                     

                  </Columns>
                    <PagerStyle AlwaysVisible="True" />
                </MasterTableView>
          <ClientSettings Selecting-AllowRowSelect="false" EnableRowHoverStyle="false">
                   <Scrolling AllowScroll="false" UseStaticHeaders="true" />    
           </ClientSettings>
        <FilterMenu CssClass="RadFilterMenu_CheckList"></FilterMenu>
          </telerik:RadGrid>

        <br /><br />
        </td>
   
    </tr>
        <tr>
             <td align="center">
                        <telerik:RadGrid ID="RadGridOrders" runat="server" ActiveItemStyle-BorderStyle="Solid" AllowPaging="True"  AutoGenerateColumns="False" OnNeedDataSource="RadGridOrders_NeedDataSource" PageSize="10" ShowStatusBar="True" Width="95%"  Skin="Web20" OnItemDataBound="RadGridOrders_ItemDataBound" GroupPanelPosition="Top" Visible="false">
               <ActiveItemStyle BorderStyle="Solid" />
               <PagerStyle Mode="NumericPages" AlwaysVisible="True" />
               <HeaderStyle CssClass="CustomHeaderForOrdersTable" />
             <MasterTableView AllowMultiColumnSorting="false" DataKeyNames="OrderNumber" Name="OrderNumber" Width="100%" Font-Size="Large">
             <Columns>
                          <telerik:GridBoundColumn DataField="OrderNumber" HeaderButtonType="TextButton" HeaderText="Order Number" SortExpression="OrderNumber" UniqueName="OrderNumber" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                      <HeaderStyle Font-Bold="True" Font-Size="" />

                       <ItemStyle HorizontalAlign="Center" Font-Bold="true"></ItemStyle>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderButtonType="TextButton" HeaderText="Pickup Date" UniqueName="PickupDate" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                 
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn HeaderButtonType="TextButton" HeaderText="Total" UniqueName="Total" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                           
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridBoundColumn> 
                 <telerik:GridBoundColumn HeaderButtonType="TextButton" HeaderText="Already Paid By Cash" UniqueName="AlreadyPaid" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                          
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridBoundColumn> 
                     <telerik:GridBoundColumn HeaderButtonType="TextButton" HeaderText="Already Paid By Check" UniqueName="AlreadyPaidByCheck" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                                  
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridBoundColumn> 
                      <telerik:GridBoundColumn HeaderButtonType="TextButton" HeaderText="CheckNumber" UniqueName="CheckNumber" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                               
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridBoundColumn> 
                   <telerik:GridBoundColumn HeaderButtonType="TextButton" HeaderText="Remain Balance" UniqueName="RemainBalance" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >  
                              <HeaderStyle Font-Bold="True" Font-Size="" />        
                           
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </telerik:GridBoundColumn> 
  
                         
                     

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
                <td valign="top" align="left">
           
                <asp:Literal ID="LiteralNotes" runat="server"></asp:Literal>

                </td>
            <td align="right" style="width:400px">
                  <table>
                      <tr>
                       
                           <td align="right" valign="middle">
                          <table  style="width: 100%;">
                              <tr>
                                  <td>
                                                <table  style="width: 100%;">
                                                                <tr>
                                                                      <td align="right"><asp:Label ID="Label1" Text="Grand Total: " runat="server" Font-Bold="true" Font-Size="Large"></asp:Label> </td> 
                                                          <td align="right"><asp:Label ID="LabelGrandTotal" Text="" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label> </td> 
                                                             </tr>
                                                         <tr>
                                                              <td align="right"><asp:Label ID="Label2" Text="Total Paid By Cash: " runat="server" Font-Bold="true" Font-Size="Large"></asp:Label> </td>
                                                             <td align="right"><asp:Label ID="LabelTotalAlreadyPaid" Text="" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label> </td>
                                                     
                                                         </tr>
                                                       <tr>
                                                            <td align="right"><asp:Label ID="Label3" Text="Total Paid By Check: " runat="server" Font-Bold="true" Font-Size="Large"></asp:Label> </td>
                                                             <td align="right"><asp:Label ID="LabelTotalAlreadyPaidByCheck" Text="" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label> </td>
                                                     
                                                         </tr>
                                              
                                                              <tr>
                                                                   <td align="right"><asp:Label ID="Label4" Text="Remain Balance: " runat="server" Font-Bold="true" Font-Size="Large"></asp:Label> </td>
                                                             <td align="right"><asp:Label ID="LabelTotalRemainBalance" Text="" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label> </td>
                                                     
                                                         </tr>
                                                          </table>
                                  </td>
                                
                              </tr>
                           </table>
                      
                     </td>
                          </tr>
                      </table>
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

         </form>
</body>
</html>

