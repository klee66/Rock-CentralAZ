<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditFamily.ascx.cs" Inherits="RockWeb.Blocks.Crm.PersonDetail.EditFamily" %>

<asp:UpdatePanel ID="upEditFamily" runat="server">
    <ContentTemplate>

        <div class="banner">
            <h1><asp:Literal ID="lBanner" runat="server"></asp:Literal></h1>
        </div>

        <asp:ValidationSummary ID="valSummaryTop" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />

        <Rock:NotificationBox ID="nbNotice" runat="server" Visible="false" />

        <div class="row">
            <div class="col-md-4">
                <fieldset>
                    <Rock:RockTextBox ID="tbFamilyName" runat="server" Label="Family Name" Required="true" CssClass="input-meduim" AutoPostBack="true" OnTextChanged="tbFamilyName_TextChanged" />
                </fieldset>
            </div>
            <div class="col-md-4">
                <fieldset>
                    <Rock:CampusPicker ID="cpCampus" runat="server" Required="true" AutoPostBack="true" OnSelectedIndexChanged="cpCampus_SelectedIndexChanged" />
                </fieldset>
            </div>
            <div class="col-md-4">
                <fieldset>
                    <Rock:RockDropDownList ID="ddlRecordStatus" runat="server" Label="Record Status" AutoPostBack="true" OnSelectedIndexChanged="ddlRecordStatus_SelectedIndexChanged" /><br />
                    <Rock:RockDropDownList ID="ddlReason" runat="server" Label="Reason" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlReason_SelectedIndexChanged"></Rock:RockDropDownList>
                </fieldset>

            </div>
        </div>

        <div class="panel panel-widget editfamily-list">
            <div class="panel-heading clearfix">
                <h3 class="panel-title pull-left">Family Members</h3>
                <div class="pull-right">
                    <asp:LinkButton ID="lbAddPerson" runat="server" CssClass="btn btn-action btn-xs" OnClick="lbAddPerson_Click" CausesValidation="false"><i class="fa fa-user"></i> Add Person</asp:LinkButton>
                </div>
            </div>
            <div class="panel-body">
                <ul class="groupmembers">
                    <asp:ListView ID="lvMembers" runat="server">
                        <ItemTemplate>
                            <li class="member">
                                <div class="person-image" id="divPersonImage" runat="server"></div>
                                <h4><%# Eval("NickName") %> <%# Eval("LastName") %></h4>
                                <asp:RadioButtonList ID="rblRole" runat="server" DataValueField="Id" DataTextField="Name" />
                                <asp:LinkButton ID="lbNewFamily" runat="server" CssClass="btn btn-action btn-move btn-xs" CommandName="Move"><i class="fa fa-external-link"></i> Move to New Family</asp:LinkButton>
                                <asp:LinkButton ID="lbRemoveMember" runat="server" Visible="false" CssClass="btn btn-remove btn-xs" CommandName="Remove"><i class="fa fa-times"></i> Remove from Family</asp:LinkButton>
                            </li>
                        </ItemTemplate>
                    </asp:ListView>
                </ul>
            </div>
        </div>

        <div class="panel panel-widget">
            <div class="panel-heading clearfix">
                <h4 class="panel-title pull-left">Addresses</h4>
                <div class="pull-right">
                    <asp:LinkButton ID="lbMoved" runat="server" CssClass="btn btn-action btn-xs" OnClick="lbMoved_Click" CausesValidation="false"><i class="fa fa-truck fa-flip-horizontal"></i> Family Moved</asp:LinkButton>
                </div>
            </div>

            <div class="panel-body">
                <Rock:Grid ID="gLocations" runat="server" AllowSorting="true" AllowPaging="false" DisplayType="Light">
                    <Columns>
                        <asp:TemplateField HeaderText="Type">
                            <ItemTemplate>
                                <%# Eval("LocationTypeName") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <Rock:RockDropDownList ID="ddlLocType" runat="server" DataTextField="Name" DataValueField="Id" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Street">
                            <ItemTemplate>
                                <%# Eval("Street1") %><br />
                                <%# Eval("Street2") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <Rock:RockTextBox ID="tbStreet1" runat="server" Text='<%# Eval("Street1") %>' /><br />
                                <Rock:RockTextBox ID="tbStreet2" runat="server" Text='<%# Eval("Street2") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="City">
                            <ItemTemplate>
                                <%# Eval("City") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <Rock:RockTextBox ID="tbCity" runat="server" Text='<%# Eval("City") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="State">
                            <ItemTemplate>
                                <%# Eval("State") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <Rock:StateDropDownList ID="ddlState" runat="server" UseAbbreviation="true" CssClass="input-mini" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Zip">
                            <ItemTemplate>
                                <%# Eval("Zip") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <Rock:RockTextBox ID="tbZip" runat="server" Text='<%# Eval("Zip") %>' CssClass="input-small" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mailing" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# ((bool)Eval("IsMailing")) ? "<i class=\"fa fa-check\"></i>" : "" %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="cbMailing" runat="server" Checked='<%# Eval("IsMailing") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Map Location" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# ((bool)Eval("IsLocation")) ? "<i class=\"fa fa-check\"></i>" : "" %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="cbLocation" runat="server" Checked='<%# Eval("IsLocation") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="span1" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbEdit" runat="server" Text="Edit" CommandName="Edit" CssClass="btn btn-default btn-sm" CausesValidation="false"><i class="fa fa-pencil"></i></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lbSave" runat="server" Text="Save" CommandName="Update" CssClass="btn btn-sm btn-success"><i class="fa fa-check"></i></asp:LinkButton>
                                <asp:LinkButton ID="lbCancel" runat="server" Text="Cancel" CommandName="Cancel" CssClass="btn btn-sm btn-warning" CausesValidation="false"><i class="fa fa-minus"></i></asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <Rock:DeleteField OnClick="gLocation_RowDelete" />
                    </Columns>
                </Rock:Grid>
            </div>
        </div>

        
        <p>
            
        </p>

        

        <div class="actions">
            <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
            <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-link" CausesValidation="false" OnClick="btnCancel_Click" />
        </div>

        <Rock:ConfirmPageUnload ID="confirmExit" runat="server" ConfirmationMessage="Changes have been made to this family that have not yet been saved." Enabled="false" />

        <asp:HiddenField ID="hfActiveDialog" runat="server" />

        <Rock:ModalDialog ID="modalAddPerson" runat="server" Title="Add Person" Content-Height="380" ValidationGroup="AddPerson" >
            <Content>

                <asp:HiddenField ID="hfActiveTab" runat="server" />

                <ul class="nav nav-pills">
                    <li id="liExistingPerson" runat="server" class="active"><a href='#<%=divExistingPerson.ClientID%>' data-toggle="pill">Add Existing Person</a></li>
                    <li id="liNewPerson" runat="server"><a href='#<%=divNewPerson.ClientID%>' data-toggle="pill">Add New Person</a></li>
                </ul>

                <asp:ValidationSummary ID="valSummaryAddPerson" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" ValidationGroup="AddPerson"/>

                <div class="tab-content">

                    <div id="divExistingPerson" runat="server" class="tab-pane active">
                        <fieldset>
                            <Rock:PersonPicker ID="ppPerson" runat="server" Label="Person" Required="true" ValidationGroup="AddPerson" />
                            <Rock:RockCheckBox ID="cbRemoveOtherFamilies" runat="server" Checked="true" Text="Remove person from other families" ValidationGroup="AddPerson"/>
                        </fieldset>
                    </div>

                    <div id="divNewPerson" runat="server" class="tab-pane">
                        <div class="row">
                            <div class="col-md-4">
                                <fieldset>
                                    <Rock:RockTextBox ID="tbNewPersonFirstName" runat="server" Label="First Name" ValidationGroup="AddPerson" />
                                </fieldset>
                            </div>
                            <div class="col-md-4">
                                <fieldset>
                                    <Rock:RockTextBox ID="tbNewPersonLastName" runat="server" Label="Last Name" ValidationGroup="AddPerson" />
                                </fieldset>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <fieldset>
                                    <Rock:RockDropDownList ID="ddlNewPersonGender" runat="server" Label="Gender" ValidationGroup="AddPerson"/>
                                </fieldset>
                            </div>
                            <div class="col-md-4">
                                <fieldset>
                                    <Rock:DatePicker ID="dpNewPersonBirthDate" runat="server" Label="Birthdate" ValidationGroup="AddPerson"/>
                                </fieldset>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <fieldset>
                                    <Rock:RockRadioButtonList ID="rblNewPersonRole" runat="server" DataTextField="Name" DataValueField="Id" RepeatDirection="Horizontal" Label="Role" ValidationGroup="AddPerson"/>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                </div>

                <script>
                    Sys.Application.add_load(function () {
                        
                        $find('<%=modalAddPerson.ClientID%>').add_shown(function () {
                            enableRequiredField('<%=ppPerson.ClientID%>_rfv', true)
                            enableRequiredField('<%=tbNewPersonFirstName.ClientID%>_rfv', false);
                            enableRequiredField('<%=tbNewPersonLastName.ClientID%>_rfv', false);
                        });

                        $find('<%=modalAddPerson.ClientID%>').add_hiding(function () {
                            enableRequiredField('<%=ppPerson.ClientID%>_rfv', false)
                            enableRequiredField('<%=tbNewPersonFirstName.ClientID%>_rfv', false);
                            enableRequiredField('<%=tbNewPersonLastName.ClientID%>_rfv', false);
                        });

                        $('a[data-toggle="pill"]').on('shown.bs.tab', function (e) {
                            var tabHref = $(e.target).attr("href");
                            if (tabHref == '#<%=divExistingPerson.ClientID%>') {
                                $('#<%=hfActiveTab.ClientID%>').val('Existing');
                                enableRequiredField('<%=ppPerson.ClientID%>_rfv', true)
                                enableRequiredField('<%=tbNewPersonFirstName.ClientID%>_rfv', false);
                                enableRequiredField('<%=tbNewPersonLastName.ClientID%>_rfv', false);
                            } else {
                                $('#<%=hfActiveTab.ClientID%>').val('New');
                                enableRequiredField('<%=ppPerson.ClientID%>_rfv', false)
                                enableRequiredField('<%=tbNewPersonFirstName.ClientID%>_rfv', true);
                                enableRequiredField('<%=tbNewPersonLastName.ClientID%>_rfv', true);
                            }

                            $('#<%=valSummaryAddPerson.ClientID%>').hide();
                        });

                    })

                    function enableRequiredField(validatorId, enable) {

                        var jqObj = $('#' + validatorId);

                        if (jqObj != null) {
                            var domObj = jqObj.get(0);
                            if (domObj != null) {
                                ValidatorEnable(domObj, enable);
                            }
                        }

                    }

                </script>

            </Content>
        </Rock:ModalDialog>

    </ContentTemplate>
</asp:UpdatePanel>



