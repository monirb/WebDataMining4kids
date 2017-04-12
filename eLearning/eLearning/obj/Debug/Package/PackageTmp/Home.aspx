<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="eLearning.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="90%">
    <tr>
        <td style="text-align:center">
            <asp:ImageButton ID="imgChildrenLearning" runat="server" 
                onclick="ImageButton1_Click" ImageUrl="~/images/children-Education.jpg" BorderWidth="1" Height="200" Width="200" />
               <div> Children's Learning </div>
        </td>
        <td style="text-align:center">
            <asp:ImageButton ID="imgHigherEducation" runat="server" Height="200px" 
                ImageUrl="~/images/GraduationCap.jpg" BorderWidth="1" Width="200px" />
               <div> Higher Education Learning</div>
        </td>
    </tr>
    <tr>
        <td style="text-align:center">
                <asp:ImageButton ID="imgFarming" runat="server" Height="200px" 
                ImageUrl="~/images/farmer.gif" BorderWidth="1" Width="200px" />
               <div> Farmer's Helper</div>
        </td>
        <td style="text-align:center">
            <asp:ImageButton ID="imgLanguages" runat="server" Height="200px" 
                ImageUrl="~/images/languages.jpg" BorderWidth="1" Width="200px" />
               <div> Learn Languages</div>
        </td>
    </tr>
</table>

</asp:Content>
