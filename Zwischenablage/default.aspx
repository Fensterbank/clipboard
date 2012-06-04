<%@ Page Title="" Language="C#" MasterPageFile="MyMaster.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Zwischenablage._default" %>

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
        <div class="span3">
        &nbsp;
        </div>
        <div class="span6">
            <form id="form1" runat="server" class="well form-horizontal">
                <h2 style="margin-bottom: 15px;">Datei ablegen</h2>
                <asp:MultiView runat="server" ID="mvMain">
                    <asp:View runat="server" ID="viewUpload">  
                        <fieldset>
                            <div class="control-group">
                                <label class="control-label" for="<%=upFile.ClientID%>">Datei wählen:</label>                        
                                <div class="controls">
                                    <asp:FileUpload runat="server" ID="upFile" CssClass="input-file" style="width: 100%;" />  
                                    <p class="help-block">Die maximale Dateigröße beträgt 64 MB.</p>   
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label" for="<%=ddlDeleteAfter.ClientID%>">Löschoptionen:</label>    
                                <div class="controls">
                                    <asp:DropDownList runat="server" ID="ddlDeleteAfter">
                                        <asp:ListItem Text="Löschen nach 30 Minuten" Value="1" />
                                        <asp:ListItem Text="Löschen nach 6 Stunden" Value="2" />
                                        <asp:ListItem Text="Löschen nach 1 Tag" Selected="True" Value="3" />
                                        <asp:ListItem Text="Löschen nach 1 Woche" Value="4" />
                                        <asp:ListItem Text="Löschen nach 1 Monat" Value="5" />
                                        <asp:ListItem Text="Niemals löschen" Value="6" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <asp:PlaceHolder runat="server" ID="phUploadPassword">
                                <div class="control-group">
                                    <label class="control-label" for="<%=tbUploadPassword.ClientID%>">Uploadpasswort:</label>                        
                                    <div class="controls">
                                        <asp:TextBox runat="server" TextMode="Password" ID="tbUploadPassword" Width="100%" />
                                        <p class="help-block">Die Dateiablage ist nur unter Angabe des korrekten Passworts möglich.</p>   
                                    </div>
                                </div>
                            </asp:PlaceHolder>
                            <div class="form-actions" style="margin-bottom:0px; padding-bottom:6px;">
                                <asp:Button runat="server" ID="btnUpload" CssClass="btn btn-primary" Text="Datei ablegen" OnClick="btnUpload_Click" />
                            </div>                
                        </fieldset>                      
                    </asp:View>
                    <asp:View runat="server" ID="viewResult">
                        <fieldset>
                            <div class="control-group">
                                <label class="control-label" for="<%=tbPageLink.ClientID%>">Link:</label>
                                <div class="controls">
                                    <asp:TextBox runat="server" ID="tbPageLink" CssClass="input-xlarge" ReadOnly="true" onClick="this.select()" style="cursor: text;" width="100%" />                                
                                    <p class="help-block">Verwende diesen Link, um auf eine Download- bzw. Betrachterseite zu gelangen.</p>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label" for="<%=tbDirectLink.ClientID%>">Direkter Link:</label>
                                <div class="controls">
                                    <asp:TextBox runat="server" ID="tbDirectLink" CssClass="input-xlarge" ReadOnly="true" onClick="this.select()" style="cursor: text;" width="100%" />                                
                                    <p class="help-block">Verwende diesen Link, um direkt auf die Datei zuzugreifen.</p>
                                </div>
                            </div>
                        </fieldset>
                    </asp:View>
                </asp:MultiView>                
            </form> 
        </div>
        <div class="span3">
        &nbsp;
        </div>
    </div>
</asp:Content>
