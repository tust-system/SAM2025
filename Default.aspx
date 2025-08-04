<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SAM2025._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1 {
            /*font-size: x-large;*/
            color: #0000FF;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <p style="font-size: 24pt; color: #0066FF">
        部門功能總覽
    </p>
    <p class="">
        <asp:Label ID="lblDepartment" runat="server" />
    </p>
    <p>
        <asp:LinkButton ID="LinkButton1" runat="server">班段查詢</asp:LinkButton>
        <asp:LinkButton ID="LinkButton3" runat="server">人員班段查詢</asp:LinkButton>
        <asp:LinkButton ID="LinkButton2" runat="server">部門打卡記錄</asp:LinkButton>
    </p>
</asp:Content>
