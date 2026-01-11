<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Inherits="SaintPolycarp.BanhChung.Inventory" Codebehind="Inventory.aspx.cs" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ConsoleContent" runat="server">
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

    <div class="frontpagestyle">
    
           <table style="width: 100%; background-color: white;">
             
               <tr>
                    <td align="center" style="width: 100%; vertical-align: central; align-content:center">
                         <asp:PlaceHolder runat="server" ID="DesktopView" Visible="false">
                            <iframe frameborder="1" src="https://docs.google.com/spreadsheets/d/1i0WCKcebnyzmJn6C5hYO0n8ctIUsMypS3UoUCVw84vQ/edit?usp=sharing" width="100%" height="1000px"></iframe>
                 <br />
                          </asp:PlaceHolder>
                        </td>
               </tr>
                 <tr>
                     <td align="center" style="width: 100%; vertical-align: central; align-content:center">
                        <asp:PlaceHolder runat="server" ID="MobileButtonView" Visible="false">
                       <asp:Button ID="ButtonEdit" runat="server" Text="Edit Inventory" OnClick="ButtonEdit_Click"/>
                       <br />
                        </asp:PlaceHolder>
                   </td>
               </tr>
                 <tr>
                    <td align="center" style="width: 100%; vertical-align: central; align-content:center">
                         <asp:PlaceHolder runat="server" ID="MobileView" Visible="false">
                            <iframe frameborder="1" src="https://docs.google.com/spreadsheets/d/e/2PACX-1vSQhUYEMWJQGw0ZIF1C3j2_R71__Oxso4MCkZ89CvJmjgHjhS7PIYVdbxlvNrWTMRcS2I-fvObl0Xd_/pubhtml" width="100%" height="1000px"></iframe>
                       <br />   
                          </asp:PlaceHolder>
                    </td>
               </tr>
              
    
           
    </table>
         
    </div>
  
   <br />
       
 
</asp:Content>


