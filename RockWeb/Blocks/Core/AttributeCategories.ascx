<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AttributeCategories.ascx.cs" Inherits="RockWeb.Blocks.Core.AttributeCategories" %>

<asp:UpdatePanel ID="upPanel" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlList" runat="server">

            <Rock:GridFilter ID="rFilter" runat="server" OnDisplayFilterValue="rFilter_DisplayFilterValue">
                <Rock:EntityTypePicker ID="entityTypeFilter" runat="server" Required="false" Label="Entity Type" IncludeGlobalOption="true" />
            </Rock:GridFilter>
            <Rock:Grid ID="rGrid" runat="server" RowItemText="Category" OnRowSelected="rGrid_Edit" TooltipField="Description">
                <Columns>
                    <Rock:ReorderField />
                    <asp:BoundField DataField="Name" HeaderText="Category" />
                    <asp:BoundField DataField="IconCssClass" HeaderText="Icon Class" />
                    <asp:TemplateField>
                        <HeaderTemplate>Entity Type</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="lEntityType" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <Rock:DeleteField OnClick="rGrid_Delete" />
                </Columns>
            </Rock:Grid>

        </asp:Panel>

        <Rock:ModalDialog ID="modalDetails" runat="server" Title="Category" ValidationGroup="EntityTypeName">
            <Content>

                <asp:HiddenField ID="hfIdValue" runat="server" />

                <div class="row">
                    <div class="col-md-6">
                        <Rock:DataTextBox ID="tbName" runat="server" SourceTypeName="Rock.Model.Category, Rock" PropertyName="Name" Required="true" />
                    </div>
                    <div class="col-md-6">
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <Rock:DataTextBox ID="tbDescription" runat="server" SourceTypeName="Rock.Model.Category, Rock" PropertyName="Description" TextMode="MultiLine" Rows="3" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <Rock:EntityTypePicker ID="entityTypePicker" runat="server" Required="true" Label="Entity Type" IncludeGlobalOption="true" />
                    </div>
                    <div class="col-md-6">
                        <Rock:DataTextBox ID="tbIconCssClass" runat="server" SourceTypeName="Rock.Model.Category, Rock" PropertyName="IconCssClass" />
                    </div>

                </div>

            </Content>
        </Rock:ModalDialog>

        <Rock:NotificationBox ID="nbMessage" runat="server" Title="Error" NotificationBoxType="Danger" Visible="false" />

    </ContentTemplate>
</asp:UpdatePanel>
