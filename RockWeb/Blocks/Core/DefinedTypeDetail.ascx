<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DefinedTypeDetail.ascx.cs" Inherits="RockWeb.Blocks.Core.DefinedTypeDetail" %>

<asp:UpdatePanel ID="upnlSettings" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hfDefinedTypeId" runat="server" />

        <asp:Panel ID="pnlDetails" runat="server" Visible="false">

            <div id="pnlEditDetails" runat="server">

                <div class="banner">
                    <h1><asp:Literal ID="lActionTitle" runat="server" /></h1>
                </div>

                <asp:ValidationSummary ID="vsDetails" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />

                <fieldset>
                    <div class="row">
                        <div class="col-md-6">
                            <Rock:DataTextBox ID="tbTypeName" runat="server" SourceTypeName="Rock.Model.DefinedType, Rock" PropertyName="Name" />
                        </div>
                    </div> 
                    
                    <div class="row">
                         <div class="col-md-12">
                             <Rock:DataTextBox ID="tbTypeDescription" runat="server" SourceTypeName="Rock.Model.DefinedType, Rock" PropertyName="Description" TextMode="MultiLine" Rows="3" />
                         </div>
                    </div>

                    <div class="row">
                         <div class="col-md-12">
                             <Rock:DataTextBox ID="tbHelpText" runat="server" SourceTypeName="Rock.Model.DefinedType, Rock" PropertyName="HelpText" TextMode="MultiLine" Rows="3" />
                         </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <Rock:DataTextBox ID="tbTypeCategory" runat="server" SourceTypeName="Rock.Model.DefinedType, Rock" PropertyName="Category" />
                        </div>
                        <div class="col-md-6">
                        </div>
                    </div>



                </fieldset>

                <div class="actions">
                    <asp:LinkButton ID="btnSaveType" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveType_Click" />
                    <asp:LinkButton ID="btnCancelType" runat="server" Text="Cancel" CssClass="btn btn-link" CausesValidation="false" OnClick="btnCancelType_Click" />
                </div>
            </div>

            <fieldset id="fieldsetViewDetails" runat="server">


                <div class="banner">
                    <h1>
                        <asp:Literal ID="lTitle" runat="server" />
                    </h1>
                </div>


                <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />

                <div class="row margin-b-md">
                    <div class="col-md-12">
                        <asp:Literal ID="lDescription" runat="server" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <Rock:RockControlWrapper ID="rcHelpText" runat="server" Label="Help Text">
                           <p class="form-control-static"><asp:Literal ID="lHelpText" runat="server" /></p>
                        </Rock:RockControlWrapper>
                    </div>
                </div>
                

                <div class="row">
                    <div class="col-md-6">
                        <asp:Literal ID="lblMainDetails" runat="server" />
                    </div>
                    <div class="col-md-6">
                        <asp:Panel ID="pnlAttributeTypes" runat="server">
                            <Rock:ModalAlert ID="mdGridWarningAttributes" runat="server" />
                            <Rock:Grid ID="gDefinedTypeAttributes" runat="server" AllowPaging="false" DisplayType="Light" RowItemText="Attribute">
                                <Columns>
                                    <Rock:ReorderField />
                                    <asp:BoundField DataField="Name" HeaderText="Attributes for Defined Type" />
                                    <Rock:EditField OnClick="gDefinedTypeAttributes_Edit" />
                                    <Rock:DeleteField OnClick="gDefinedTypeAttributes_Delete" />
                                </Columns>
                            </Rock:Grid>
                        </asp:Panel>
                    </div>
                </div>
                <div class="actions">
                    <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnEdit_Click" />
                    <Rock:ModalAlert ID="mdDeleteWarning" runat="server" />
                    <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-link" OnClick="btnDelete_Click" CausesValidation="false" />
                </div>

            </fieldset>
                        
        </asp:Panel>

        <asp:Panel ID="pnlDefinedTypeAttributes" runat="server" Visible="false">
            <Rock:AttributeEditor ID="edtDefinedTypeAttributes" runat="server" OnSaveClick="btnSaveDefinedTypeAttribute_Click" OnCancelClick="btnCancelDefinedTypeAttribute_Click" ValidationGroup="Attribute" />
        </asp:Panel>
        
    </ContentTemplate>
</asp:UpdatePanel>
