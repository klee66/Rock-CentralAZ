<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BlockTypeList.ascx.cs" Inherits="RockWeb.Blocks.Core.BlockTypeList" %>

<asp:UpdatePanel ID="upPanel" runat="server">
    <ContentTemplate>
        <Rock:GridFilter ID="gfSettings" runat="server">
            <Rock:RockTextBox ID="tbNameFilter" runat="server" Label="Name" />
            <Rock:RockTextBox ID="tbPathFilter" runat="server" Label="Path" />
            <Rock:RockDropDownList ID="ddlCategoryFilter" runat="server" Label="Category" />
            <Rock:RockCheckBox ID="cbExcludeSystem" runat="server" Label="Exclude 'System' types?" Text="Yes" />
        </Rock:GridFilter>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <Rock:Grid ID="gBlockTypes" runat="server" AllowSorting="true" OnRowDataBound="gBlockTypes_RowDataBound" OnRowSelected="gBlockTypes_Edit" TooltipField="Description">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField HeaderText="Category" DataField="Category" SortExpression="Category" />
                <asp:BoundField HeaderText="Path" DataField="Path" SortExpression="Path" />
                <Rock:BadgeField HeaderText="Usage" DataField="BlocksCount" SortExpression="BlocksCount"
                    ImportantMin="0" ImportantMax="0" InfoMin="1" InfoMax="1" SuccessMin="2" />
                <asp:BoundField HeaderText="Status" SortExpression="Status" />
                <Rock:BoolField DataField="IsSystem" HeaderText="System" SortExpression="IsSystem" />
                <Rock:DeleteField OnClick="gBlockTypes_Delete" />
            </Columns>
        </Rock:Grid>
        <div class="actions">
            <asp:LinkButton id="btnRefreshAll" runat="server" Text="Reload All Block Type Attributes" CssClass="btn btn-link" CausesValidation="false" OnClick="btnRefreshAll_Click" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

