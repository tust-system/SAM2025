<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TimePeriod.aspx.cs" Inherits="SAM2025.TimePeriod" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1 {
            font-size: x-large;
            color: #0000FF;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <p style="font-size: 24pt; color: #0066FF">
        班段查詢
    </p>
    <p class="">
        <asp:Label ID="lblDepartment" runat="server" />
    </p>
    <p>
        <asp:LinkButton ID="LinkButton1" runat="server">新增班段</asp:LinkButton>
        <asp:LinkButton ID="LinkButton2" runat="server">返回部門功能總覽</asp:LinkButton>
    </p>
    <div style="font-size: 9pt">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
            CellPadding="4" DataKeyNames="id,PID" DataSourceID="SqlDataSource1"
            ForeColor="#333333" GridLines="None" Width="800px" CellSpacing="1">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#CCFFFF" />
            <Columns>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False"
                    ReadOnly="True" SortExpression="id" />
                <asp:BoundField DataField="PID" HeaderText="班段" ReadOnly="True"
                    SortExpression="PID" />
                <asp:TemplateField HeaderText="說明" SortExpression="PNAME">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("PNAME") %>'
                            Width="50px"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("PNAME") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
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
            DeleteCommand="DELETE FROM [PCode] WHERE [id] = @id AND [PID] = @PID"
            InsertCommand="INSERT INTO [PCode] ([PID], [PNAME]) VALUES (@PID, @PNAME)"
            SelectCommand="SELECT [id], [PID], [PNAME] FROM [PCode] WHERE [PID] LIKE @DepID + '%'"
            UpdateCommand="UPDATE [PCode] SET [PNAME] = @PNAME WHERE [id] = @id AND [PID] = @PID">
            <DeleteParameters>
                <asp:Parameter Name="id" Type="Int32" />
                <asp:Parameter Name="PID" Type="String" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="PNAME" Type="String" />
                <asp:Parameter Name="id" Type="Int32" />
                <asp:Parameter Name="PID" Type="String" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="PID" Type="String" />
                <asp:Parameter Name="PNAME" Type="String" />
            </InsertParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="DepID" QueryStringField="DepID" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <br />
    </div>
</asp:Content>

