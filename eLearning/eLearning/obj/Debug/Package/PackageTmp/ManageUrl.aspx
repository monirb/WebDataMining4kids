<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ManageUrl.aspx.cs" Inherits="eLearning.ManageUrl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <h2>   Subject Area and their URLs</h2>
    <br />
<table style="width:90%" border="0">
    <%--<tr style="background-color:Gray">
        <td colspan="2" align="center">Subject Area and their URLS</td>    
    </tr>--%>
    <tr>
        <td colspan="2"><asp:Label ID="lblErrorMsg" runat="server" ForeColor ="Red" Text=""></asp:Label></td>    
    </tr>
    <tr>
        <td style="width:30%; vertical-align:top">
            <asp:GridView ID="gvFields" runat="server" CellPadding="4" ForeColor="#333333" 
                GridLines="None" OnSelectedIndexChanged="gvFields_SelectedIndexChanged" AutoGenerateColumns="False"
                OnRowDataBound="gvFields_RowDataBound" Width="90%">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            <Columns>
                <asp:TemplateField HeaderText="Id" Visible = "false">
                    <ItemTemplate>
                        <asp:Label ID = "lblID" runat ="server" Text='<%# Bind("id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subject Area" >
                    <ItemTemplate>
                        <asp:Label ID = "lblFieldName" runat ="server" Text='<%# Bind("field_name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
             </Columns>
            </asp:GridView>
            
        </td>
        <td style="vertical-align:top">
            <table width="100%">
                <tr>
                    <td align="center" style=" background-color:#5D7B9D; color:white" >
                        <b>URL Lists</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnAddUrl" runat="server" Text="Add Url" Visible = "false"
                            onclick="btnAddUrl_Click"/> &nbsp;
                        <div style="display:inline-block"> 
                            <asp:Panel ID = "pnlAddUrl" runat="server" visible="false" >
                                <asp:TextBox ID="txtUrl" runat="server" ></asp:TextBox>
                                <asp:Button ID="btnSaveUrl" runat="server" Text="Save" onclick="btnSaveUrl_Click" />
                            </asp:Panel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvUrls" runat="server" Width="90%" 
                            AutoGenerateColumns="False" OnRowDataBound="gvUrls_RowDataBound" >
                            <Columns>
                                <asp:TemplateField HeaderText="Id" Visible = "false">
                                    <ItemTemplate>
                                        <asp:Label ID = "lblID" runat ="server" Text='<%# Bind("id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="FieldId" Visible = "false">
                                    <ItemTemplate>
                                        <asp:Label ID = "lblFieldID" runat ="server" Text='<%# Bind("field_id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Authentic Urls">
                                    <ItemTemplate>
                                        <asp:Label ID = "lblUrls" runat ="server" Text='<%# Bind("url") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:Button ID = "btnDelete" runat ="server" Text="Delete" OnClick="btnDelete_Click"></asp:Button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    
</table>

</asp:Content>
