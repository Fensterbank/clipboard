<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Zwischenablage._default" %>
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
