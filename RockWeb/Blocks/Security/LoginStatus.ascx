<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LoginStatus.ascx.cs" Inherits="RockWeb.Blocks.Security.LoginStatus" %>
<ul class="nav navbar-nav loginstatus">
    <li class="dropdown">
        <a class="dropdown-toggle navbar-link" href="#" data-toggle="dropdown">
            <i class="fa fa-user"></i>
            <asp:PlaceHolder ID="phHello" runat="server"><asp:Literal ID="lHello" runat="server" /></asp:PlaceHolder>
            <b class="fa fa-caret-down"></b>
        </a>
        <ul class="dropdown-menu">
            <asp:PlaceHolder ID="phMyAccount" runat="server">
                <li>
                    <asp:HyperLink ID="hlMyAccount" runat="server" Text="My Account" />
                </li>
            </asp:PlaceHolder>
            <li class="divider"></li>
            <li><asp:LinkButton ID="lbLoginLogout" runat="server" OnClick="lbLoginLogout_Click" CausesValidation="false"></asp:LinkButton></li>
        </ul>
    </li>
</ul>
<asp:HiddenField ID="hfActionType" runat="server" />

