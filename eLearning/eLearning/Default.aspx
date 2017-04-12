<%@ Page Title="Home Page" Language="C#" MasterPageFile="Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="eLearning._Default" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Children&#39;s learning !</h2>
    <br />
        <asp:Label ID="Label1" runat="server" Text="Add Text here or upload a file"></asp:Label>
     <br />
       
        <asp:TextBox ID="txtStory" runat="server" TextMode="MultiLine" Height="50px" 
            Width="1204px"></asp:TextBox>            
       
      <div style="vertical-align:top"> 
       <asp:FileUpload ID="fileUpload" runat="server" />&nbsp;
       <asp:Button ID="btnGo" runat="server" Text="Go" onclick="btnGo_Click" 
         Width="60px" />
      </div>
        
         &nbsp; <asp:Label ID="lblKeywords" runat="server" Text=""></asp:Label>
         &nbsp;
            <asp:Label ID="lblSearchResult" runat="server" Text=""></asp:Label>
        <br />
   
        <asp:Panel ID="Panel1" runat="server" >
           <table class="imgVidTable">
           <tr style="background-color:Gray">
            <td> <asp:Label ID="lblImage" runat="server" Text="IMAGES"></asp:Label> </td>
            <td> <asp:Label ID="lblVideos" runat="server" Text="VIDOES"></asp:Label> </td>
           </tr>
            <tr>
                <td valign="top">
                 <asp:DataList ID="dlImageSearch" runat="server" RepeatColumns="4" CellPadding="5">
                    <ItemTemplate>
                        <asp:HyperLink ID="imgLink" runat="server" NavigateUrl='<%#Eval("Url") %>' Target="_blank"><img src='<%#Eval("Url") %>' width="130" height="130px"/></asp:HyperLink>
                        <br /><asp:HyperLink ID="titleLink" runat="server" Text='<%#Eval("Title") %>' NavigateUrl='<%#Eval("OriginalContextUrl") %>' Target="_blank"> </asp:HyperLink><br />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label Visible='<%#bool.Parse((dlImageSearch.Items.Count==0).ToString())%>' runat="server"
                            ID="lblNoRecord" Text="No Record Found!"></asp:Label>
                    </FooterTemplate>
                </asp:DataList>
                </td>
                <td valign="top">
                   <asp:DataList ID="dlVideoSearch" runat="server" RepeatColumns="4" CellPadding="5">
                    <ItemTemplate>
                        <a id="aVid1" href='<%#Eval("PlayUrl") %>' target="_blank"><img src='<%#Eval("Url") %>' width="130px" height="130px" /></a><br />
                        <a id="a1" href='<%#Eval("PlayUrl") %>' target="_blank"><%#Eval("Title")+"(" +Eval("Duration")+")"%></a>
                       <br />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label Visible='<%#bool.Parse((dlVideoSearch.Items.Count==0).ToString())%>' runat="server"
                            ID="lblNoRecord" Text="No Record Found!"></asp:Label>
                    </FooterTemplate>
                  </asp:DataList>
               </td>
            </tr>
           </table>
        </asp:Panel>
     
</asp:Content>
