<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendSmsTextMessage.aspx.cs" Inherits="SaintPolycarp.BanhChung.SendSmsTextMessage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
       <script language="javascript">

           function hideButton1() {
               document.getElementById('<%=ButtonSendConfirmation.ClientID %>').disabled = true;
               document.getElementById('<%=ButtonSendConfirmation.ClientID %>').style.background = "CadetBlue ";
            //document.getElementById('<%=ButtonSendConfirmation.ClientID %>').style.visibility = "hidden";
           }
           function hideButton2() {
               document.getElementById('<%=ButtonSendToday.ClientID %>').disabled = true;
               document.getElementById('<%=ButtonSendToday.ClientID %>').style.background = "CadetBlue ";
               //document.getElementById('<%=ButtonSendToday.ClientID %>').style.visibility = "hidden";
           }
           function hideButton3() {
               document.getElementById('<%=ButtonSendTomorrow.ClientID %>').disabled = true;
               document.getElementById('<%=ButtonSendTomorrow.ClientID %>').style.background = "CadetBlue ";
               //document.getElementById('<%=ButtonSendTomorrow.ClientID %>').style.visibility = "hidden";
           }
      
</script>
        <div>
             <asp:Label ID="LabelTitle" Font-Bold="true" Font-Size="Large" runat="server" Text=""></asp:Label>
            <br /><br />
            <table style="width: 100%;">
             
                <tr>
                    <td>  <asp:Label ID="Label1" runat="server" Text="Phone Number:"></asp:Label></td>
                    <td>  <asp:TextBox ID="TextBoxPhoneNumber" AutoPostBack="true" runat="server" Width="430px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td></td> <td></td>
                </tr>
                  <tr>
                    <td>  </td>
                    <td>
                         <table style="width: 100%;">
                             <tr>  
                                   <td  align="center">
                                     <asp:Button ID="ButtonSendConfirmation" Width="170px" AutoPostBack="true" BackColor="Orange" BorderStyle="Double" BorderColor="Blue" runat="server" Text="Send Confirmation" OnClick="ButtonSendConfirmation_Click" />
                                 </td>
                                 <td>
                                       <asp:TextBox ID="TextBoxMessageConfirmation" runat="server" Width="500px" Height="80px" TextMode="MultiLine"></asp:TextBox>
                                 </td>
                               
                             </tr>
                              <tr>  
                                   <td  align="center">
                                      <br />
                                 </td>
                                 <td>
                                     
                                 </td>
                               
                             </tr>
                             <tr>  
                                   <td  align="center">
                                     <asp:Button ID="ButtonSendToday" Width="170px" AutoPostBack="true" BackColor="Orange" BorderStyle="Double" BorderColor="Blue" runat="server" Text="Remind today pickup" OnClick="ButtonSendToday_Click"  />
                                 </td>
                                 <td>
                                       <asp:TextBox ID="TextBoxMessageToday" runat="server" Width="500px" Height="80px" TextMode="MultiLine"></asp:TextBox>
                                 </td>
                               
                             </tr>

                              <tr>  
                                   <td  align="center">
                                    <br />
                                 </td>
                                 <td>
                                     
                                 </td>
                               
                             </tr>
                             <tr>  
                                   <td  align="center">
                                     <asp:Button ID="ButtonSendTomorrow" Width="170px" BackColor="Orange" AutoPostBack="true" BorderStyle="Double" BorderColor="Blue"  runat="server" Text="Remind tomorrow pickup" OnClick="ButtonSendTomorrow_Click" />
                                 </td>
                                 <td>
                                       <asp:TextBox ID="TextBoxMessageTomorrow" runat="server" Width="500px" Height="80px" TextMode="MultiLine"></asp:TextBox>
                                 </td>
                               
                             </tr>

                            
                          </table>

                    </td>
                </tr>
                 </tr>
                <tr>
                    <td></td> 
                    <td>
                         
                    </td>
                </tr>
                 </tr>
                <tr>
                    <td>
                       
                    </td>
                    <td>
                          <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                               <ProgressTemplate>
                                                          <asp:Image ID="Image1" runat="server" ImageUrl="~/images/process2.gif" Visible="false" style="margin-left: 10px" Width="16px" />
                                               </ProgressTemplate>
                                         </asp:UpdateProgress> 
                         <asp:UpdatePanel runat="server" id="Panel">
                                <ContentTemplate>
                                    
                                </ContentTemplate>
                         </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
