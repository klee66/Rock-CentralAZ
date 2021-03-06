<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GroupSearch.ascx.cs" Inherits="RockWeb.Blocks.Groups.GroupSearch" %>

<Rock:Grid ID="gGroups" runat="server" EmptyDataText="No Groups Found">
    <Columns>
        <asp:BoundField
            HeaderText="Group"
            DataField="Structure"
            SortExpression="Structure" HtmlEncode="false" />
        <asp:BoundField 
            HeaderText="Type"
            DataField="GroupType" 
            SortExpression="GroupType" />
        <asp:BoundField 
            HeaderText="Member Count"
            ItemStyle-HorizontalAlign="Right"
            DataField="MemberCount" 
            SortExpression="MemberCount"
            DataFormatString="{0:N0}" />
    </Columns>
</Rock:Grid>


