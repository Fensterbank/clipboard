<%@ Page Title="" Language="C#" MasterPageFile="MyMaster.Master" AutoEventWireup="true" CodeBehind="file.aspx.cs" Inherits="Zwischenablage.file" %>

<%--

    Clipboard - A simple file and image uploader and viewer
    Copyright (C) 2012 Frédéric Bolvin

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

 --%>

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
