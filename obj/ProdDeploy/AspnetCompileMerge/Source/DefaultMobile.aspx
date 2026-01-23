<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" Inherits="SaintPolycarp.BanhChung.DefaultMobile" Codebehind="DefaultMobile.aspx.cs" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ConsoleContent" runat="server">
      <tr>
          <td>      <style type="text/css">
        .frontpagestyle {
  padding: 30px;
  margin-bottom: 30px;
  font-size: 14px;
  font-weight: 200;
  line-height: 2.1428571435;
  color: red;
 
  background-color: #eeeeee;
}

.frontpagestyle h3 {
  line-height: 1;
  color: black;
  }

.frontpagestyle p {
  line-height: 1.4;
}

.container .frontpagestyle {
  border-radius: 6px;
}

     
          .auto-style8 {
              color: #FB0202;
          }
 
        .auto-style9 {
        text-decoration: underline;
    }
   
        </style>
    <div class="frontpagestyle">
    
           <table style="width: 100%; background-color: white;">
               <tr>
                    <td align="center" style="width: 100%; vertical-align: central; align-content:center">
                            <h3 class="auto-style8">
                                <span class="auto-style9"><strong>Xuân Bính Ngọ 2026</strong></span><br /> 
                                Dự án Bánh Chưng <br />
                                BTV @ Cộng Đoàn St. Polycarp</h3>
                                             
                <br />
                   </td>
                    <td valign="top" align="center"> </td>
               </tr>
   
           
    </table>
        <br /> 
    </div>
              <asp:PlaceHolder runat="server" ID="LoginStatus" Visible="false">
            <p>
               <asp:Literal runat="server" ID="StatusText" />
            </p>
         </asp:PlaceHolder> 
         <asp:PlaceHolder runat="server" ID="LoginForm" Visible="false">
            <div style="margin-bottom: 10px">
               <asp:Label runat="server" AssociatedControlID="UserName">User name</asp:Label>
               <div>
                  <asp:TextBox runat="server" ID="UserName" />
               </div>
            </div>
            <div style="margin-bottom: 10px">
               <asp:Label runat="server" AssociatedControlID="Password">Password</asp:Label>
               <div>
                  <asp:TextBox runat="server" ID="Password" TextMode="Password" />
               </div>
            </div>
            <div style="margin-bottom: 10px">
               <div>
                  <asp:Button runat="server" OnClick="SignIn" Text="Log in" />
               </div>
            </div>
         </asp:PlaceHolder>
     
    <br /> 
    <br /> 

 
 
          </td>
      </tr>

 
 
</asp:Content>


