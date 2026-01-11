<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="Register.aspx.cs" Inherits="SaintPolycarp.BanhChung.Register" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <br />
        <h4 style="font-size: medium">Register a new user</h4>
        <hr />
        <p>
            <asp:Literal runat="server" ID="StatusMessage" />
        </p>  
          <table style="width: 100%;">
              <tr>
                  <td align="left" style="width: 39%;">
                           <div style="margin-bottom:10px">
                                    <asp:Label runat="server" AssociatedControlID="UserName">User name</asp:Label>
                                    <div>
                                        <asp:TextBox runat="server" ID="UserName" />                
                
                                    </div>
                                </div>
                                <div style="margin-bottom:10px">
                                    <asp:Label runat="server" AssociatedControlID="Password">Password</asp:Label>
                                    <div>
                                        <asp:TextBox runat="server" ID="Password" TextMode="Password" />                
                                    </div>
                                </div>
                                <div style="margin-bottom:10px">
                                    <asp:Label runat="server" AssociatedControlID="ConfirmPassword">Confirm password</asp:Label>
                                    <div>
                                        <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" />                
                                    </div>
                                </div>
                              <div style="margin-bottom:10px">
                                    <asp:Label runat="server" AssociatedControlID="PhoneNumber">Phone Number</asp:Label>
                                    <div>
                                        <asp:TextBox runat="server" ID="PhoneNumber" />   
                                        <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server"
                                          ControlToValidate="PhoneNumber"
                                          ErrorMessage="Phone number is a required field."
                                          ForeColor="Red">
                                        </asp:RequiredFieldValidator>
                
                                    </div>
                                </div>
                  </td>
                  <td  align="left" valign="center" style="width: 70%;">
                        <div style="margin-bottom:10px">
                            <div>
                                 <asp:CheckBox ID="CheckBoxUser" Text="User" Checked="true" runat="server" />     
                            </div>
                        </div>
                          <div style="margin-bottom:10px">
                            <div>
                                 <asp:CheckBox ID="CheckBoxInventoryAdmin" Text="Inventory Admin" Checked="false" runat="server" />     
                            </div>
                        </div>
                       <div style="margin-bottom:10px">
                            <div>
                                 <asp:CheckBox ID="CheckBoxFinancialAdmin" Text="Financial Admin" Checked="false" runat="server" />     
                            </div>
                        </div>
                            <div style="margin-bottom:10px">
                            <div>
                                 <asp:CheckBox ID="CheckBoxGlobalAdmin" Text="Global Admin" Checked="false" runat="server" />     
                            </div>
                        </div>
       
                  </td>
              </tr>
              </table>
     
     
        <div>
            <div>
                <asp:Button runat="server" OnClick="CreateUser_Click" Text="Register" /></br> 
                </br> 
                </br> 
            </div>
        </div>
    </div>
 </asp:Content>