<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TimePeriodAdd.aspx.cs" Inherits="SAM2025.TimePeriodAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p style="font-size: 24pt; color: #0066FF">
        新增班段<br />
    </p>
    <p class="">
        <asp:Label ID="lblDepartment" runat="server" />
    </p>
    <p>
        <asp:LinkButton ID="LinkButton1" runat="server">返回班段清單</asp:LinkButton>
        <asp:LinkButton ID="LinkButton2" runat="server">返回部門功能總覽</asp:LinkButton>
    </p>
    <p>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
            ConnectionString="<%$ ConnectionStrings:SAMConnectionString %>"
            OnInserting="SqlDataSource1_Inserting"
            DeleteCommand="DELETE FROM [PCode] WHERE [id] = @original_id AND [PID] = @original_PID"
            InsertCommand="INSERT INTO [PCode] ([DepID], [PID], [PNAME]) VALUES (@DepID, @PID, @PNAME)"
            OldValuesParameterFormatString="original_{0}"
            SelectCommand="SELECT [id], [PID], [PNAME] FROM [PCode]"
            UpdateCommand="UPDATE [PCode] SET [PNAME] = @PNAME WHERE [id] = @original_id AND [PID] = @original_PID">
            <DeleteParameters>
                <asp:Parameter Name="original_id" Type="Int32" />
                <asp:Parameter Name="original_PID" Type="String" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="PNAME" Type="String" />
                <asp:Parameter Name="original_id" Type="Int32" />
                <asp:Parameter Name="original_PID" Type="String" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="DepID" Type="String" />
                <asp:Parameter Name="PID" Type="String" />
                <asp:Parameter Name="PNAME" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>
        <br />
        請按下方新增，結束請按確定
    </p>
    <p>
        1：值機，0：不值機<br />
        班別代號不得重覆<asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False"
            BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="3px"
            CellPadding="5" DataKeyNames="id,PID" DataSourceID="SqlDataSource1"
            GridLines="None" Height="250px" Width="80px" CellSpacing="2"
            ForeColor="Black"
            OnItemCommand="DetailsView1_ItemCommand"
            OnItemInserting="DetailsView1_ItemInserting"
            OnItemUpdating="DetailsView1_ItemUpdating"
            OnItemDeleting="DetailsView1_ItemDeleting">
            <FooterStyle BackColor="Tan" />
            <PagerStyle BackColor="#9999FF" ForeColor="DarkSlateBlue"
                HorizontalAlign="Center" />
            <Fields>
                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False"
                    ReadOnly="True" SortExpression="id" />
                <asp:TemplateField HeaderText="PID" SortExpression="PID">
                    <EditItemTemplate>
                        <asp:Label ID="lblDep" runat="server" Text='<%# ViewState["DepID"] %>' />
                        _
                        <asp:TextBox ID="txbSuffix" runat="server"
                            Text='<%# GetPidSuffix(Eval("PID")) %>' />
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:TextBox ID="txbSuffix" runat="server" />
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <%# ShowFullPid(Eval("PID")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="PNAME" SortExpression="PNAME">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("PNAME") %>'
                            Width="50px"></asp:TextBox>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        
                    </InsertItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="True" ShowInsertButton="True" />
            </Fields>
            <HeaderStyle BackColor="Tan" Font-Bold="True" />
            <EditRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
            <AlternatingRowStyle BackColor="PaleGoldenrod" />
        </asp:DetailsView>
    </p>
</asp:Content>
