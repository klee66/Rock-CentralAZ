<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SystemInfo.ascx.cs" Inherits="RockWeb.Blocks.Administration.SystemInfo" %>

<script type="text/javascript">

    function pageLoad() {

        $('#show-cache-objects').click(function () {
            $('#cache-objects').toggle('slow', function () {
                if ($('#modal-scroll-container').length) {
                    $('#modal-scroll-container').tinyscrollbar_update('relative');
                }
            });
        });

        $('a.show-pill').click(function () {
    	    $('ul.nav-pills > li').attr('class', '');
    	    $(this).parent().attr('class', 'active');
    	    $('div.tabContent > div').hide('slow');
    	    $('#' + $(this).attr('pill')).show('slow', function () {
    	        if ($('#modal-scroll-container').length) {
    	            $('#modal-scroll-container').tinyscrollbar_update('relative');
    	        }
    	    });
        });

        if ($('div.alert.alert-success').length > 0) {
    	        window.setTimeout("fadeAndClear()", 5000);
        }
    }

    function fadeAndClear() {
    	$('div.alert.alert-success').animate({ opacity: 0 }, 2000 );
    }


</script>

<ul class="nav nav-pills" >
    <li class='active'><a pill="version-info" class="show-pill" href="#">Version Info</a></li>
    <li><a pill="diagnostics-tab" class="show-pill" href="#">Diagnostics</a></li>
</ul>

<div class="tabContent" >

    <div id="version-info">

        <p><strong>Rock Version: </strong>
            <asp:Literal ID="lRockVersion" runat="server"></asp:Literal></p>

        <p><strong>Server Culture Setting: </strong>
            <asp:Literal ID="lServerCulture" runat="server"></asp:Literal></p>
        
        <Rock:NotificationBox ID="nbMessage" runat="server" NotificationBoxType="Success" Title="Success" Visible="false" Text=""></Rock:NotificationBox>

        <div class="actions margin-t-xl">
            <asp:Button runat="server" ID="btnFlushCache" CssClass="btn btn-primary btn-sm" Text="Clear Cache" OnClick="btnClearCache_Click" ToolTip="Flushes Pages, BlockTypes, Blocks and Attributes from the Rock web cache." />
            <asp:Button runat="server" ID="btnRestart" CssClass="btn btn-link btn-sm restart" Text="Restart Rock" OnClick="btnRestart_Click" ToolTip="Restarts the Application." />
        </div>
    </div>

    <div id="diagnostics-tab" style="display:none">
        
        <h4>Details</h4>
        <p>Database: <asp:Literal ID="lDatabase" runat="server"></asp:Literal></p>
        <p>System Date Time: <%= DateTime.Now.ToString("G") + " " + DateTime.Now.ToString("zzz") %></p>
        <p>Rock Time: <%= Rock.RockDateTime.Now.ToString("G") + " " + Rock.RockDateTime.OrgTimeZoneInfo.BaseUtcOffset %></p>

        <p>Executing Location: <asp:Literal ID="lExecLocation" runat="server"></asp:Literal></p>

        <h4>Cache</h4>
        <div id="cache-details">
            <asp:Literal ID="lCacheOverview" runat="server"></asp:Literal>
        </div>

<%-- This appears to have been disabled 9/19/2012:
    https://github.com/SparkDevNetwork/Rock/commit/f295069b2152d4b1ff93d44fa0d82fd2a2fb0d14#diff-357e7f0be3ea16b9658156b1ee1f8145L27    
    but this link was still here:         --%>
        <a id="show-cache-objects" href="#">Show Cache Objects</a>
        <div id="cache-objects" style="display:none">
            <asp:Literal ID="lCacheObjects" runat="server"></asp:Literal>
        </div>
        
        <h4>Routes</h4>
        <asp:Literal ID="lRoutes" runat="server"></asp:Literal>

        <asp:LinkButton runat="server" ID="btnDumpDiagnostics" CssClass="btn btn-action btn-sm" OnClick="btnDumpDiagnostics_Click" ToolTip="Generates a diagnostics file for sharing with others.">
            <i class="fa fa-download"></i> Download Diagnostics File
        </asp:LinkButton>

    </div>

    <script>
        $(".restart").on("click", function () {
            bootbox.alert("The Rock application will be restarted. You will need to reload this page to continue.")
        });
    </script>

</div>

