<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EntityTypes.ascx.cs" Inherits="RockWeb.Blocks.Core.EntityTypes" %>

<script type="text/javascript">
    function clearActiveDialog() {
        $('#<%=hfActiveDialog.ClientID %>').val('');
    }
</script>

<asp:UpdatePanel ID="upMarketingCampaigns" runat="server">
    <ContentTemplate>

        <Rock:Grid ID="gEntityTypes" runat="server" AllowSorting="true">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Entity Type" SortExpression="Name" />
                <asp:BoundField DataField="FriendlyName" HeaderText="Friendly Name" SortExpression="FriendlyName" />
                <Rock:BoolField DataField="IsCommon" HeaderText="Common" SortExpression="IsCommon" />
                <Rock:TemplateFieldUnselected>
                    <HeaderStyle CssClass="span1" />
                    <ItemStyle HorizontalAlign="Center"/>
                    <ItemTemplate>
                        <a id="aSecure" runat="server" class="btn btn-security btn-sm" height="500px"><i class="fa fa-lock"></i></a>
                    </ItemTemplate>
                </Rock:TemplateFieldUnselected>
            </Columns>
        </Rock:Grid>

        <asp:HiddenField ID="hfActiveDialog" runat="server" />

        <Rock:ModalDialog ID="mdEdit" runat="server" Title="Entity" OnCancelScript="clearActiveDialog();">
            <Content>
                <asp:HiddenField ID="hfEntityTypeId" runat="server" />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />
                <Rock:DataTextBox ID="tbName" runat="server" SourceTypeName="Rock.Model.EntityType, Rock" PropertyName="Name" Label="Entity Type Name" />
                <Rock:DataTextBox ID="tbFriendlyName" runat="server" SourceTypeName="Rock.Model.EntityType, Rock" PropertyName="FriendlyName" Label="Friendly Name" />
                <Rock:RockCheckBox ID="cbCommon" runat="server" Label="Common" Text="Yes" Help="There are various places that a user is prompted for an entity type.  'Common' entities will be listed first for the user to easily find them" />
            </Content>
        </Rock:ModalDialog>

        <Rock:NotificationBox ID="nbWarning" runat="server" Title="Warning" NotificationBoxType="Warning" Visible="false" />

    </ContentTemplate>
</asp:UpdatePanel>