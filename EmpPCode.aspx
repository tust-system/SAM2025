<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmpPCode.aspx.cs" Inherits="SAM2025.EmpPCode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p style="font-size: 24pt; color: #0066FF">
        人員班段清單<br />
    </p>
    <p class="">
        <asp:Label ID="lblDepartment" runat="server" />
    </p>
    <p>
        <asp:LinkButton ID="LinkButton3" runat="server">人員班段設定</asp:LinkButton>
        <asp:LinkButton ID="LinkButton1" runat="server">返回班段清單</asp:LinkButton>
        <asp:LinkButton ID="LinkButton2" runat="server">返回部門功能總覽</asp:LinkButton>
    </p>
    <div style="font-size: 9pt">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
            CellPadding="4" DataKeyNames="SQID" DataSourceID="SqlDataSource1"
            ForeColor="#333333" GridLines="None" Width="60%" CellSpacing="1">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#CCFFFF" />
            <Columns>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                <asp:BoundField DataField="SQID" HeaderText="SQID" InsertVisible="False"
                    ReadOnly="True" SortExpression="SQID" />
                <asp:BoundField DataField="EmpID" HeaderText="EmpID" />
                <asp:BoundField DataField="EmpName" HeaderText="EmpName" />
                <asp:BoundField DataField="PCodeID" HeaderText="PCodeID" />
            </Columns>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#FFCCFF" />
            <AlternatingRowStyle BackColor="#99CCFF" BorderColor="Black"
                BorderWidth="2px" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
            ConnectionString="<%$ ConnectionStrings:SAMConnectionString %>"
            SelectCommand="SELECT * FROM [EmpPCode] WHERE [DepID] = @DepID">
            <SelectParameters>
                <asp:QueryStringParameter Name="DepID" QueryStringField="DepID" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>
