<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TimeExcelDep.aspx.cs" Inherits="SAM2025.TimeExcel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2 {
            color: #FF0000;
            height: 673px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 673px">
        <p style="font-size: 24pt; color: #0066FF">
            部門打卡記錄查詢<br />
        </p>
        <p class="">
            <asp:Label ID="lblDepartment" runat="server" />
        </p>
        <p>
            <asp:LinkButton ID="LinkButton2" runat="server">部門打卡記錄上傳</asp:LinkButton>
            <asp:LinkButton ID="LinkButton1" runat="server">返回部門功能總覽</asp:LinkButton>
        </p>
        <br />
        開始日期 :
        <asp:TextBox runat="server" ID="tbxStartDate" TextMode="Date"></asp:TextBox>
        結束日期 :
        <asp:TextBox runat="server" ID="tbxEndDate" TextMode="Date"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="查詢" />
        <br />
        <asp:GridView runat="server" ID="DridView1">
        </asp:GridView>
    </div>
    <%--  --%>
</asp:Content>
