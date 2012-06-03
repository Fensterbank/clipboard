<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="file.aspx.cs" Inherits="Zwischenablage.file" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="span2">
        &nbsp;
        </div>
        <div class="span8">
            <form id="form1" runat="server" class="well form-horizontal">
                <h2 style="margin-bottom: 15px;"><asp:Literal runat="server" ID="litTitle" /></h2>                
                <asp:MultiView runat="server" ID="mvMain">
                    <asp:View runat="server" ID="viewShow">  
                        <asp:Panel runat="server" ID="divThumbnail" CssClass="thumbnail" >
                            <asp:HyperLink runat="server" ID="lnkImage" Width="100%" style="margin-right: auto; margin-left: auto; display:block; text-align:center;" /> 
                            <h5><asp:Literal runat="server" ID="litFilename" /></h5>
                            <p>Hochgeladen: <asp:Literal runat="server" ID="litInfoUploadedDate" /><br />Dateigröße: <asp:Literal runat="server" ID="litInfoFileSize" /></p>  
                        </asp:Panel>
                    </asp:View>                    
                    <asp:View runat="server" ID="viewError">
                        <asp:Image runat="server" ID="img404" ImageUrl="img/404.jpg" style="margin-right: auto; margin-left: auto; display:block;" />
                    </asp:View>
                </asp:MultiView>                
            </form> 
        </div>
        <div class="span2">
        &nbsp;
        </div>
    </div>
</asp:Content>
