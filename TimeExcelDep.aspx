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
        部門人員 : 
        <asp:DropDownList runat="server" ID="ddl_Emp" AutoPostBack="true" OnSelectedIndexChanged="ddl_Emp_SelectedIndexChanged"></asp:DropDownList>
        <br />
        <br />
        <div style="font-size: 9pt">
            <asp:GridView runat="server" ID="GridView1" AutoGenerateColumns="False"
                CellPadding="4"
                ForeColor="#333333" GridLines="None" Width="800px" CellSpacing="1">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#CCFFFF" />
                <Columns>
                    <asp:BoundField DataField="EmpID" HeaderText="員工編號" />
                    <asp:BoundField DataField="EmpName" HeaderText="姓名" />
                    <asp:BoundField DataField="Date" HeaderText="日期" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="PNAME" HeaderText="班別名稱" />
                    <asp:BoundField DataField="InStatus" HeaderText="上班狀態" />
                    <asp:BoundField DataField="OutStatus" HeaderText="下班狀態" />
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#FFCCFF" />
                <AlternatingRowStyle BackColor="#99CCFF" BorderColor="Black"
                    BorderWidth="2px" />
            </asp:GridView>
        </div>
    </div>
    <%--  --%>
</asp:Content>
