<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmpPCodeAdd.aspx.cs" Inherits="SAM2025.EmpPCodeAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p style="font-size: 24pt; color: #0066FF">
        人員班段設定<br />
    </p>
    <p class="">
        <asp:Label ID="lblDepartment" runat="server" />
    </p>
    <p>
        <asp:LinkButton ID="LinkButton3" runat="server">返回人員班段清單</asp:LinkButton>
        <asp:LinkButton ID="LinkButton1" runat="server">返回班段清單</asp:LinkButton>
        <asp:LinkButton ID="LinkButton2" runat="server">返回部門功能總覽</asp:LinkButton>
    </p>
    <div style="font-size: 9pt">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
            CellPadding="4" ForeColor="#333333" GridLines="None" Width="40%" CellSpacing="1"
            OnRowDataBound="GridView1_RowDataBound">
            <Columns>
                <asp:BoundField DataField="EmpID" HeaderText="EmpID" />
                <asp:BoundField DataField="EmpChiName" HeaderText="EmpChiName" />
                <asp:TemplateField HeaderText="班段選擇">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlShift" runat="server" Width="150px" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#CCFFFF" />
            <AlternatingRowStyle BackColor="#99CCFF" BorderColor="Black" BorderWidth="2px" />
        </asp:GridView>
        <br />
        <asp:Button ID="btnSave" runat="server" Text="確定儲存" OnClick="btnSave_Click" s/>
    </div>
</asp:Content>
