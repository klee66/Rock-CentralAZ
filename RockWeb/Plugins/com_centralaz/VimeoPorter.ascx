<%@ Control Language="C#" AutoEventWireup="true" CodeFile="VimeoPorter.ascx.cs" Inherits="RockWeb.Plugins.com_centralaz.VimeoPorter" %>


<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>

<asp:Panel ID="pnlView" runat="server" CssClass="panel panel-block">

    <div class="embed-responsive embed-responsive-16by9">
        <iframe class="embed-responsive-item" src="http://player.vimeo.com/video/<%=GetAttributeValue("VimeoVidID")%>"></iframe>
    </div>
    <div align="right"><a href="http://Media.Centralaz.com">View more Central content!</a></div>

</asp:Panel>





        <Rock:ModalDialog ID="mdDialog" runat="server" Title="Vimeo Porter Widget"
            OnSaveClick="mdDialog_SaveClick">
            <Content>
                </br>
                <div>
                    This block will take a video ID from Vimeo, all the numbers at the end of "vimeo.com/12345678" for example, and build a responsive box for the video to exist in. Simply enter the video ID, in the example it would be "12345678", and we will take care of the rest!
                </div>
                </br>
                <Rock:RockTextBox ID="tbVimeoVidID" Label="Vimeo ID Number:" runat="server" Required="true" RequiredErrorMessage="Please enter a video ID" Placeholder="Vimeo video number ID goes here.">

                </Rock:RockTextBox>

                <asp:ValidationSummary ID="valVimeoPorter" EnableClientScript="true" runat="server" HeaderText="Oops! something went wrong." CssClass="alert alert-danger" />

                <asp:RegularExpressionValidator ID="rev1" runat="server" ControlToValidate="tbVimeoVidID" ValidationExpression="[0-9]+" Display="Dynamic" ErrorMessage="Please only enter a number ID" CssClass="validation-error"></asp:RegularExpressionValidator>

            </Content>
        </Rock:ModalDialog>

    </ContentTemplate>
</asp:UpdatePanel>

