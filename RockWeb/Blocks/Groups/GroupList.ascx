<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GroupList.ascx.cs" Inherits="RockWeb.Blocks.Groups.GroupList" %>

<asp:UpdatePanel ID="upnlGroupList" runat="server">
    <ContentTemplate>
        <Rock:GridFilter ID="gfSettings" runat="server" OnDisplayFilterValue="rFilter_DisplayFilterValue">
            <Rock:GroupTypePicker ID="gtpGroupType" runat="server" Label="Group Type" />
            
            <Rock:RockDropDownList ID="ddlActiveFilter" runat="server" Label="Active Status">
                <asp:ListItem Text="[All]" Value="all"></asp:ListItem>
                <asp:ListItem Text="Active" Value="active"></asp:ListItem>
                <asp:ListItem Text="Inactive" Value="inactive"></asp:ListItem>
            </Rock:RockDropDownList>
        </Rock:GridFilter>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <Rock:Grid ID="gGroups" runat="server" RowItemText="Group" AllowSorting="true" OnRowSelected="gGroups_Edit">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="GroupTypeName" HeaderText="Group Type" SortExpression="GroupTypeName" />
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                <asp:BoundField DataField="GroupRole" HeaderText="Role" SortExpression="Role" />
                <asp:BoundField DataField="MemberCount" HeaderText="Members" SortExpression="MemberCount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                <Rock:DateTimeField DataField="DateAdded" HeaderText="Added" SortExpression="DateAdded" FormatAsElapsedTime="true" />
                <Rock:BoolField DataField="IsSystem" HeaderText="System" SortExpression="IsSystem" />
                <Rock:BoolField DataField="IsActive" HeaderText="Active" SortExpression="IsActive" />
                <Rock:DeleteField OnClick="gGroups_Delete" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>
