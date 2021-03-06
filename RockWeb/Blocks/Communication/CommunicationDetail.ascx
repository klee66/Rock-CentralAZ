<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CommunicationDetail.ascx.cs" Inherits="RockWeb.Blocks.Communication.CommunicationDetail" %>

<asp:UpdatePanel ID="upPanel" runat="server">
    <ContentTemplate>

        <div class="banner">
            <h1>
                <asp:Literal ID="lTitle" runat="server"></asp:Literal></h1>
            <Rock:HighlightLabel ID="hlStatus" runat="server" />
        </div>

        <asp:Panel ID="pnlDetails" runat="server">

            <div class="form-horizontal">

                <asp:Literal ID="lFutureSend" runat="server"></asp:Literal>

                <div class="row margin-b-lg">
                    <div class="col-md-6">
                        <asp:Literal ID="lCreatedBy" runat="server"></asp:Literal>
                    </div>
                    <div class="col-md-6 text-right">
                        <asp:Literal ID="lApprovedBy" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>

            <div class="recipient-status row">
                <asp:Panel id="pnlPending" runat="server">
                    <a id="aPending" runat="server" class="btn btn-lg btn-block btn-default">
                        <asp:Literal ID="lPending" runat="server"></asp:Literal><br />
                        <small>Pending</small></a>
                </asp:Panel>
                <asp:Panel id="pnlDelivered" runat="server">
                    <a id="aDelivered" runat="server" class="btn btn-lg btn-block btn-info">
                        <asp:Literal ID="lDelivered" runat="server"></asp:Literal><br />
                        <small>Delivered</small></a>
                </asp:Panel>
                <asp:Panel id="pnlOpened" runat="server">
                    <a id="aOpened" runat="server" class="btn btn-lg btn-block btn-success" disabled="disabled">
                        <asp:Literal ID="lOpened" runat="server"></asp:Literal><br />
                        <small>Opened</small></a>
                </asp:Panel>
                <asp:Panel id="pnlFailed" runat="server">
                    <a id="aFailed" runat="server" class="btn btn-lg btn-block btn-danger">
                        <asp:Literal ID="lFailed" runat="server"></asp:Literal><br />
                        <small>Failed</small></a>
                </asp:Panel>
                <asp:Panel id="pnlCancelled" runat="server">
                    <a id="aCancelled" runat="server" class="btn btn-lg btn-block btn-warning">
                        <asp:Literal ID="lCancelled" runat="server"></asp:Literal><br />
                        <small>Cancelled</small></a>
                </asp:Panel>
            </div>

            <asp:HiddenField ID="hfActiveRecipient" runat="server" />

            <section id="sPending" runat="server" class="js-communication-recipients panel panel-widget">
                <header class="panel-heading clearfix">Pending Recipients</header>
                <div class="panel-body">
                    <Rock:Grid ID="gPending" runat="server" AllowSorting="true">
                        <Columns>
                            <Rock:PersonField HeaderText="Name" DataField="Person" SortExpression="Person.LastName,Person.NickName" />
                            <asp:BoundField HeaderText="Note" DataField="StatusNote" SortExpression="StatusNote" />
                        </Columns>
                    </Rock:Grid>
                </div>
            </section>

            <section id="sDelivered" runat="server" class="js-communication-recipients panel panel-widget">
                <header class="panel-heading clearfix">Delivered Recipients</header>
                <div class="panel-body">
                    <Rock:Grid ID="gDelivered" runat="server" AllowSorting="true" ShowActionRow="false">
                        <Columns>
                            <Rock:PersonField HeaderText="Name" DataField="Person" SortExpression="Person.LastName,Person.NickName" />
                            <Rock:EnumField HeaderText="Status" DataField="Status" SortExpression="Status" />
                            <asp:BoundField HeaderText="Note" DataField="StatusNote" SortExpression="StatusNote" />
                            <Rock:DateTimeField HeaderText="Opened" DataField="OpenedDateTime" SortExpression="OpenedDateTime" />
                            <asp:BoundField HeaderText="Client" DataField="OpenedClient" SortExpression="OpenedClient" />
                            <asp:BoundField HeaderText="Message ID" DataField="UniqueMessageId" SortExpression="UniqueMessageId" />
                            <asp:BoundField HeaderText="Activity" ItemStyle-CssClass="wrap-contents" DataField="ActivityList" HtmlEncode="false" />
                        </Columns>
                    </Rock:Grid>
                </div>
            </section>

            <section id="sFailed" runat="server" class="js-communication-recipients panel panel-widget">
                <header class="panel-heading clearfix">Failed Recipients</header>
                <div class="panel-body">
                    <Rock:Grid ID="gFailed" runat="server" AllowSorting="true">
                        <Columns>
                            <Rock:PersonField HeaderText="Name" DataField="Person" SortExpression="Person.LastName,Person.NickName" />
                            <asp:BoundField HeaderText="Note" DataField="StatusNote" SortExpression="StatusNote" />
                        </Columns>
                    </Rock:Grid>
                </div>
            </section>

            <section id="sCancelled" runat="server" class="js-communication-recipients panel panel-widget">
                <header class="panel-heading clearfix">Cancelled Recipients</header>
                <div class="panel-body">
                    <Rock:Grid ID="gCancelled" runat="server" AllowSorting="true">
                        <Columns>
                            <Rock:PersonField HeaderText="Name" DataField="Person" SortExpression="Person.LastName,Person.NickName" />
                            <asp:BoundField HeaderText="Note" DataField="StatusNote" SortExpression="StatusNote" />
                        </Columns>
                    </Rock:Grid>
                </div>
            </section>

            <section id="sOpened" runat="server" class="js-communication-recipients panel panel-widget">
                <header class="panel-heading clearfix">Opened Recipients</header>
                <div class="panel-body">
                    <Rock:Grid ID="gOpened" runat="server" AllowSorting="true">
                        <Columns>
                            <Rock:PersonField HeaderText="Name" DataField="Person" SortExpression="Person.LastName,Person.NickName" />
                            <asp:BoundField HeaderText="Note" DataField="StatusNote" SortExpression="StatusNote" />
                            <Rock:DateTimeField HeaderText="Opened" DataField="OpenedDateTime" SortExpression="OpenedDateTime" />
                            <asp:BoundField HeaderText="Client" DataField="OpenedClient" SortExpression="OpenedClient" />
                            <asp:BoundField HeaderText="Message ID" DataField="UniqueMessageId" SortExpression="UniqueMessageId" />
                            <asp:BoundField HeaderText="Activity" DataField="ActivityList" HtmlEncode="false" />
                        </Columns>
                    </Rock:Grid>
                </div>
            </section>

            <Rock:PanelWidget ID="wpMessageDetails" runat="server" Title="Message Details">
                <asp:Literal ID="lDetails" runat="server" />
            </Rock:PanelWidget>

            <Rock:PanelWidget ID="wpEvents" runat="server" Title="Activity" Expanded="false">
                <Rock:Grid ID="gActivity" runat="server" AllowSorting="true" RowItemText="Activity">
                    <Columns>
                        <Rock:DateTimeField HeaderText="Date" DataField="ActivityDateTime" SortExpression="ActivityDateTime" />
                        <Rock:PersonField HeaderText="Person" DataField="CommunicationRecipient.Person" SortExpression="CommunicationRecipient.Person.LastName,CommunicationRecipient.Person.NickName" />
                        <asp:BoundField HeaderText="Activity" DataField="ActivityType" SortExpression="ActivityType" />
                        <Rock:EnumField HeaderText="Details" ItemStyle-CssClass="wrap-contents" DataField="ActivityDetail" SortExpression="ActivityDetail" />
                    </Columns>
                </Rock:Grid>
            </Rock:PanelWidget>

            <div class="actions">
                <asp:LinkButton ID="btnApprove" runat="server" Text="Approve" CssClass="btn btn-primary" OnClick="btnApprove_Click" />
                <asp:LinkButton ID="btnDeny" runat="server" Text="Deny" CssClass="btn btn-link" OnClick="btnDeny_Click" />
                <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel Send" CssClass="btn btn-link" OnClick="btnCancel_Click" />
                <asp:LinkButton ID="btnCopy" runat="server" Text="Copy Communication" CssClass="btn btn-link" OnClick="btnCopy_Click" />
            </div>

        </asp:Panel>

        <asp:Panel ID="pnlResult" runat="server" Visible="false">
            <Rock:NotificationBox ID="nbResult" runat="server" NotificationBoxType="Success" />
            <br />
            <asp:HyperLink ID="hlViewCommunication" runat="server" Text="View Communication" />
        </asp:Panel>

        <script>
            $('.js-date-rollover').tooltip();
        </script>

    </ContentTemplate>
</asp:UpdatePanel>


