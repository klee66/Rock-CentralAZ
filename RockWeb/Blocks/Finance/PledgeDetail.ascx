<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PledgeDetail.ascx.cs" Inherits="RockWeb.Blocks.Finance.PledgeDetail" %>

<asp:UpdatePanel ID="upPledgeDetails" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDetails" runat="server" Visible="False">
            <asp:HiddenField ID="hfPledgeId" runat="server" />

            <div class="banner">
                 <h1><asp:Literal ID="lActionTitle" runat="server"/></h1>
            </div>
            
            <asp:ValidationSummary ID="valSummaryTop" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />

            <Rock:NotificationBox ID="nbWarningMessage" runat="server" NotificationBoxType="Warning" />

            <fieldset>
                <div class="row">
                    <div class="col-md-6">
                        <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />
            
                        <Rock:PersonPicker ID="ppPerson" runat="server" Label="Person" Required="True"/>
                        <Rock:AccountPicker ID="fpFund" runat="server" Label="Fund" Required="True"/>
            
                        <Rock:CurrencyBox ID="tbAmount" runat="server" Label="Total Amount" Required="True" />
                    </div>

                    <div class="col-md-6">
                        <Rock:DateRangePicker ID="dpDateRange" runat="server" Label="Date Range" Required="True"/>
                        <Rock:DataDropDownList ID="ddlFrequencyType" runat="server" SourceTypeName="Rock.Model.FinancialPledge, Rock" PropertyName="PledgeFrequencyValue" Label="Payment Schedule" Required="True"/>
                    </div>
                </div>

                <div class="actions">
                    <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-link" CausesValidation="false" OnClick="btnCancel_Click" />
                </div>

            </fieldset>

            
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>