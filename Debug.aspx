<%@ Page Title="Debug Info" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Debug.aspx.cs" Inherits="Mitchell.ScmConsoles.DebugInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        Debug information:<br />
    </p>
    <p>
        <asp:Literal ID="Literal_logs" runat="server"></asp:Literal>
    </p>
    <p>
        &nbsp;</p>
</asp:Content>
