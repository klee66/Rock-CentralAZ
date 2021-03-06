<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MyAccount.ascx.cs" Inherits="RockWeb.Blocks.Security.MyAccount" %>

<script>
    $(function () {
        $(".photo a").fluidbox();
    });
</script>

<div class="row">

    <div class="col-sm-3">
        <div class="photo">
            <asp:PlaceHolder ID="phImage" runat="server" />
        </div>
    </div>

    <div class="col-sm-9">

        <h1 class="title name">
            <asp:Literal ID="lName" runat="server" /></h1>

        <div class="row">

            <div class="col-sm-6">
                <ul class="person-demographics list-unstyled">
                    <li><asp:Literal ID="lEmail" runat="server" /></li>
                    <li><asp:Literal ID="lGender" runat="server" /></li>
                    <li><asp:Literal ID="lAge" runat="server" /></li>
                </ul>
            </div>

            <div class="col-sm-6">

                <ul class="phone-list list-unstyled">
                <asp:Repeater ID="rptPhones" runat="server">
                    <ItemTemplate>
                        <li><%# (bool)Eval("IsUnlisted") ? "Unlisted" : FormatPhoneNumber( Eval("CountryCode"), Eval("Number") ) %> <small><%# Eval("NumberTypeValue.Name") %></small></li>
                    </ItemTemplate>
                </asp:Repeater>
                </ul>

                <asp:LinkButton ID="lbEditPerson" runat="server" CssClass="btn btn-primary btn-xs" OnClick="lbEditPerson_Click"><i class="fa fa-pencil"></i> Edit</asp:LinkButton>
        
            </div>

        </div>

    </div>

</div>



