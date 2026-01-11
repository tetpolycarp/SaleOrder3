<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" Inherits="_Default" Codebehind="Default.aspx.cs" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
      <style type="text/css">
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
  <br />
    <div class="frontpagestyle">
    
           <table style="width: 100%; background-color: white;">
               <tr>
                    <td align="center" style="width: 100%; vertical-align: central; align-content:center">
                            <h1 class="auto-style8">     
                                Dự án Bánh Chưng <br />
                                Ban Thường Vụ @ Cộng Đoàn St. Polycarp</h1>
                   </td>
               </tr>
        <tr>
           
            <td align="center"  style="width: 100%; vertical-align: central; align-content:center">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/images/home/banh-chung-tet.jpg" />
                <asp:Image ID="Image1" runat="server" Height="300px" ImageUrl="~/images/home/tet.png" Width="300px" />
            
            </td>
             
           
        </tr>
           
    </table>
         
    </div>
  
   <br />
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
  
</asp:Content>


