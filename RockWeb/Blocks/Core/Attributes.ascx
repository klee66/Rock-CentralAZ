<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Attributes.ascx.cs" Inherits="RockWeb.Blocks.Core.Attributes" %>

<script type="text/javascript">
    function clearActiveDialog() {
        $('#<%=hfActiveDialog.ClientID %>').val('');
    }

    Sys.Application.add_load(function () {
        $('.js-grid-scroll').tinyscrollbar({ size: 150, sizethumb: 20 });
        $find('<%=mdAttribute.BehaviorID %>').add_shown(function () {
            $('.js-grid-scroll').tinyscrollbar_update('relative');
        });
    });

</script>

<asp:UpdatePanel ID="upPanel" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlList" runat="server">

            <Rock:GridFilter ID="rFilter" runat="server" OnDisplayFilterValue="rFilter_DisplayFilterValue">
                <Rock:EntityTypePicker ID="ddlEntityType" runat="server" Label="Entity Type" IncludeGlobalOption="true" AutoPostBack="true" OnSelectedIndexChanged="ddlEntityType_SelectedIndexChanged" />
                <Rock:CategoryPicker ID="cpCategoriesFilter" runat="server" Label="Categories" AllowMultiSelect="true" />
            </Rock:GridFilter>
            <Rock:Grid ID="rGrid" runat="server" AllowSorting="true" RowItemText="setting" TooltipField="Description" OnRowSelected="rGrid_RowSelected">
                <Columns>
                    <asp:BoundField 
                        DataField="Id" 
                        HeaderText="Id" 
                        SortExpression="EntityType.FriendlyName" 
                        ItemStyle-Wrap="false"
                        ItemStyle-HorizontalAlign="Right"
                        HeaderStyle-HorizontalAlign="Right" />
                    <asp:TemplateField ItemStyle-Wrap="false">
                        <HeaderTemplate>Qualifier</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="lEntityQualifier" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-Wrap="false" />
                    <asp:TemplateField ItemStyle-Wrap="false">
                        <HeaderTemplate>Categories</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="lCategories" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>Default Value</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="lDefaultValue" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>Value</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="lValue" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <Rock:EditField OnClick="rGrid_Edit" />
                    <Rock:SecurityField TitleField="Name" />
                    <Rock:DeleteField OnClick="rGrid_Delete" />
                </Columns>
            </Rock:Grid>

        </asp:Panel>

        <asp:HiddenField ID="hfActiveDialog" runat="server" />

        <Rock:ModalDialog ID="mdAttribute" runat="server" Title="Attribute" OnCancelScript="clearActiveDialog();" ValidationGroup="Attribute" >
            <Content>

    
                <asp:ValidationSummary ID="valSummaryTop" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />

                <div class="scroll-container js-grid-scroll scroll-container-vertical" style="width: 720px">
                    <div class="scrollbar">
                        <div class="track">
                            <div class="thumb">
                                <div class="end"></div>
                            </div>
                        </div>
                    </div>
                    <div class="viewport" style="width: 690px">
                        <div class="overview">
                            <Rock:EntityTypePicker ID="ddlAttrEntityType" runat="server" Label="Entity Type" IncludeGlobalOption="true" Required="true" />
                            <Rock:RockTextBox ID="tbAttrQualifierField" runat="server" Label="Qualifier Field" />
                            <Rock:RockTextBox ID="tbAttrQualifierValue" runat="server" Label="Qualifier Value" />
                            <Rock:AttributeEditor ID="edtAttribute" runat="server" ShowActions="false" ValidationGroup="Attribute" />
                        </div>
                    </div>
                </div>
            </Content>
        </Rock:ModalDialog>

        <Rock:ModalDialog ID="mdAttributeValue" runat="server" Title="Attribute Value" OnCancelScript="clearActiveDialog();" ValidationGroup="AttributeValue" >
            <Content>
                <asp:HiddenField ID="hfIdValues" runat="server" />
                <asp:ValidationSummary ID="ValidationSummaryValue" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" ValidationGroup="AttributeValue"  />
                <fieldset id="fsEditControl" runat="server"/>
            </Content>
        </Rock:ModalDialog>

        <Rock:NotificationBox ID="nbMessage" runat="server" Title="Error" NotificationBoxType="Danger" Visible="false" />

    </ContentTemplate>
</asp:UpdatePanel>
