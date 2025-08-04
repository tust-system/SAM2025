<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TimeExcelInput.aspx.cs" Inherits="SAM2025.TimeExcelInput" %>

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
            部門打卡記錄<br />
        </p>
        <p class="">
            <asp:Label ID="lblDepartment" runat="server" />
        </p>
        <p>
            <asp:LinkButton ID="LinkButton2" runat="server">返回打卡記錄查詢</asp:LinkButton>
            <asp:LinkButton ID="LinkButton1" runat="server">返回部門功能總覽</asp:LinkButton>
        </p>
        <br />
        請選檔案，再按上傳<br />
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <br />
        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="上傳" />
        <br />
        <br />

        <%--EXCEL檔工作表，請注意<span class="style2">考勤異常表</span>是否有資料<br />--%>
        EXCEL檔檔名請不要更改例如：cid37_62_2017-02-01_2017-02-07_917.xlsx，因為要判斷所匯入的日期<br />
        員工編號和日期：如假況不同，則後蓋前，SAM調整會清除<br />
        員工編號和日期：如假況相同，則判斷SAM是否有調整，有調則保留調過的<br />
        <br />
        <asp:HyperLink ID="HyperLink1" runat="server"
            NavigateUrl="~/file/cid37_62_2017-02-01_2017-02-21_1814.xlsx">卡鐘匯入範例</asp:HyperLink>
        <br />
        <br />
        <br />
        <br />
        <asp:Label ID="labError" runat="server"></asp:Label>
    </div>
</asp:Content>
