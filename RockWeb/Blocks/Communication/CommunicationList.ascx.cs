// <copyright>
// Copyright 2013 by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.Attribute;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Communication
{
    [DisplayName( "Communication List" )]
    [Category( "Communication" )]
    [Description( "Lists the status of all previously created communications." )]

    [SecurityAction( Authorization.APPROVE, "The roles and/or users that have access to approve new communications." )]

    [LinkedPage( "Detail Page" )]
    public partial class CommunicationList : Rock.Web.UI.RockBlock
    {
        private bool canApprove = false;

        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            BindFilter();
            rFilter.ApplyFilterClick += rFilter_ApplyFilterClick;
            rFilter.DisplayFilterValue += rFilter_DisplayFilterValue;

            gCommunication.DataKeyNames = new string[] { "Id" };
            gCommunication.Actions.ShowAdd = false;
            gCommunication.GridRebind += gCommunication_GridRebind;

            // The created by column/filter should only be displayed if user is allowed to approve
            canApprove = this.IsUserAuthorized( "Approve" );
            ppSender.Visible = canApprove;
            gCommunication.Columns[2].Visible = canApprove;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            if ( !Page.IsPostBack )
            {
                BindGrid();
            }

            base.OnLoad( e );
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the ApplyFilterClick event of the rFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void rFilter_ApplyFilterClick( object sender, EventArgs e )
        {
            rFilter.SaveUserPreference( "Subject", tbSubject.Text );
            rFilter.SaveUserPreference( "Channel", cpChannel.SelectedValue );
            rFilter.SaveUserPreference( "Status", ddlStatus.SelectedValue );
            if ( canApprove )
            {
                rFilter.SaveUserPreference( "Created By", ppSender.PersonId.ToString() );
            }
            rFilter.SaveUserPreference( "Content", tbContent.Text );

            BindGrid();
        }

        /// <summary>
        /// Handles the filter display for each saved user value
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected void rFilter_DisplayFilterValue( object sender, Rock.Web.UI.Controls.GridFilter.DisplayFilterValueArgs e )
        {
            switch ( e.Key )
            {
                case "Channel":
                    {
                        var entity = EntityTypeCache.Read( e.Value.AsGuid() );
                        if ( entity != null )
                        {
                            e.Value = entity.FriendlyName;
                        }

                        break;
                    }
                case "Status":
                    {
                        if ( !string.IsNullOrWhiteSpace( e.Value ) )
                        {
                            e.Value = ( (CommunicationStatus)System.Enum.Parse( typeof( CommunicationStatus ), e.Value ) ).ConvertToString();
                        }

                        break;
                    }
                case "Created By":
                    {
                        int personId = 0;
                        if ( int.TryParse( e.Value, out personId ) && personId != 0 )
                        {
                            var personService = new PersonService( new RockContext() );
                            var person = personService.Get( personId );
                            if ( person != null )
                            {
                                e.Value = person.FullName;
                            }
                        }

                        break;
                    }
            }
        }

        /// <summary>
        /// Handles the RowDataBound event of the gCommunication control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs" /> instance containing the event data.</param>
        protected void gCommunication_RowDataBound( object sender, GridViewRowEventArgs e )
        {
            if ( e.Row.RowType == DataControlRowType.DataRow )
            {
                var communicationItem = e.Row.DataItem as CommunicationItem;
                if ( communicationItem != null )
                {
                    var bPending = e.Row.FindControl( "bPending" ) as Badge;
                    bPending.Visible = communicationItem.PendingRecipients > 0;
                    bPending.Text = communicationItem.PendingRecipients.ToString( "N0" );

                    var bCancelled = e.Row.FindControl( "bCancelled" ) as Badge;
                    bCancelled.Visible = communicationItem.CancelledRecipients > 0;
                    bCancelled.Text = communicationItem.CancelledRecipients.ToString( "N0" );

                    var bFailed = e.Row.FindControl( "bFailed" ) as Badge;
                    bFailed.Visible = communicationItem.FailedRecipients > 0;
                    bFailed.Text = communicationItem.FailedRecipients.ToString( "N0" );

                    var bDelivered = e.Row.FindControl( "bDelivered" ) as Badge;
                    bDelivered.Visible = communicationItem.DeliveredRecipients > 0;
                    bDelivered.Text = communicationItem.DeliveredRecipients.ToString( "N0" );

                    var bOpened = e.Row.FindControl( "bOpened" ) as Badge;
                    bOpened.Visible = communicationItem.OpenedRecipients > 0;
                    bOpened.Text = communicationItem.OpenedRecipients.ToString( "N0" );

                    // Hide delete button if there are any successful recipients
                    e.Row.Cells[7].Controls[0].Visible = communicationItem.DeliveredRecipients <= 0;
                }
            }
        }

        /// <summary>
        /// Handles the RowSelected event of the gCommunication control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Rock.Web.UI.Controls.RowEventArgs" /> instance containing the event data.</param>
        protected void gCommunication_RowSelected( object sender, Rock.Web.UI.Controls.RowEventArgs e )
        {
            NavigateToLinkedPage( "DetailPage", "CommunicationId", (int)e.RowKeyValue );
        }

        /// <summary>
        /// Handles the Delete event of the gCommunication control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Rock.Web.UI.Controls.RowEventArgs"/> instance containing the event data.</param>
        protected void gCommunication_Delete( object sender, Rock.Web.UI.Controls.RowEventArgs e )
        {
            var rockContext = new RockContext();
            var communicationService = new CommunicationService( rockContext );
            var communication = communicationService.Get( e.RowKeyId );
            if ( communication != null )
            {
                string errorMessage;
                if ( !communicationService.CanDelete( communication, out errorMessage ) )
                {
                    mdGridWarning.Show( errorMessage, ModalAlertType.Information );
                    return;
                }

                communicationService.Delete( communication );

                rockContext.SaveChanges();
            }

            BindGrid();
        }

        /// <summary>
        /// Handles the GridRebind event of the gCommunication control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void gCommunication_GridRebind( object sender, EventArgs e )
        {
            BindGrid();
        }

        #endregion

        #region Internal Methods

        private void BindFilter()
        {
            if ( cpChannel.Items[0].Value != string.Empty )
            {
                cpChannel.Items.Insert( 0, new ListItem( string.Empty, string.Empty ) );
            }

            ddlStatus.BindToEnum( typeof( CommunicationStatus ) );
            // Replace the Transient status with an emtyp value (need an empty one, and don't need transient value)
            ddlStatus.Items[0].Text = string.Empty;
            ddlStatus.Items[0].Value = string.Empty;

            if ( !Page.IsPostBack )
            {
                if ( !canApprove )
                {
                    rFilter.SaveUserPreference( "Created By", string.Empty );
                }

                tbSubject.Text = rFilter.GetUserPreference( "Subject" );
                cpChannel.SelectedValue = rFilter.GetUserPreference( "Channel" );
                ddlStatus.SelectedValue = rFilter.GetUserPreference( "Status" );

                int personId = 0;
                if ( int.TryParse( rFilter.GetUserPreference( "Created By" ), out personId ) )
                {
                    var personService = new PersonService( new RockContext() );
                    var person = personService.Get( personId );
                    if ( person != null )
                    {
                        ppSender.SetValue( person );
                    }
                }

                tbContent.Text = rFilter.GetUserPreference( "Content" );
            }
        }

        private void BindGrid()
        {
            var rockContext = new RockContext();

            var communications = new CommunicationService( rockContext )
                    .Queryable( "ChannelEntityType,Sender,Reviewer" )
                    .Where( c => c.Status != CommunicationStatus.Transient );

            string subject = rFilter.GetUserPreference( "Subject" );
            if ( !string.IsNullOrWhiteSpace( subject ) )
            {
                communications = communications.Where( c => c.Subject.Contains( subject ) );
            }

            Guid entityTypeGuid = Guid.Empty;
            if ( Guid.TryParse( rFilter.GetUserPreference( "Channel" ), out entityTypeGuid ) )
            {
                communications = communications.Where( c => c.ChannelEntityType != null && c.ChannelEntityType.Guid.Equals( entityTypeGuid ) );
            }

            string status = rFilter.GetUserPreference( "Status" );
            if ( !string.IsNullOrWhiteSpace( status ) )
            {
                var communicationStatus = (CommunicationStatus)System.Enum.Parse( typeof( CommunicationStatus ), status );
                communications = communications.Where( c => c.Status == communicationStatus );
            }

            if ( canApprove )
            {
                int personId = 0;
                if ( int.TryParse( rFilter.GetUserPreference( "Created By" ), out personId ) && personId != 0 )
                {
                    communications = communications.Where( c => c.SenderPersonId.HasValue && c.SenderPersonId.Value == personId );
                }
            }
            else
            {
                communications = communications.Where( c => c.SenderPersonId.HasValue && c.SenderPersonId.Value == CurrentPersonId );
            }

            string content = rFilter.GetUserPreference( "Content" );
            if ( !string.IsNullOrWhiteSpace( content ) )
            {
                communications = communications.Where( c => c.ChannelDataJson.Contains( content ) );
            }

            var recipients = new CommunicationRecipientService( rockContext ).Queryable();

            var queryable = communications
                .Select( c => new CommunicationItem
                {
                    Id = c.Id,
                    Communication = c,
                    Recipients = recipients
                        .Where( r => r.CommunicationId == c.Id)
                        .Count(),
                    PendingRecipients = recipients
                        .Where( r => r.CommunicationId == c.Id && r.Status == CommunicationRecipientStatus.Pending)
                        .Count(),
                    CancelledRecipients = recipients
                        .Where( r => r.CommunicationId == c.Id && r.Status == CommunicationRecipientStatus.Cancelled)
                        .Count(),
                    FailedRecipients = recipients
                        .Where( r => r.CommunicationId == c.Id && r.Status == CommunicationRecipientStatus.Failed)
                        .Count(),
                    DeliveredRecipients = recipients
                        .Where( r => r.CommunicationId == c.Id && 
                            (r.Status == CommunicationRecipientStatus.Delivered || r.Status == CommunicationRecipientStatus.Opened))
                        .Count(),
                    OpenedRecipients = recipients
                        .Where( r => r.CommunicationId == c.Id && r.Status == CommunicationRecipientStatus.Opened)
                        .Count()
                } );

            var sortProperty = gCommunication.SortProperty;
            if ( sortProperty != null )
            {
                queryable = queryable.Sort( sortProperty );
            }
            else
            {
                queryable = queryable.OrderByDescending( c => c.Communication.Id );
            }

            // Get the channel names
            var channels = new Dictionary<int, string>();
            foreach ( var item in Rock.Communication.ChannelContainer.Instance.Components.Values )
            {
                var entityType = item.Value.EntityType;
                channels.Add( entityType.Id, item.Metadata.ComponentName );
            }

            var communicationItems = queryable.ToList();
            foreach( var c in communicationItems)
            {
                c.ChannelName = channels.ContainsKey( c.Communication.ChannelEntityTypeId ?? 0 ) ?
                    channels[c.Communication.ChannelEntityTypeId ?? 0] :
                    c.Communication.ChannelEntityType.FriendlyName;
            }

            gCommunication.DataSource = communicationItems;
            gCommunication.DataBind();

        }

        #endregion

        protected class CommunicationItem
        {
            public int Id { get; set; }
            public Rock.Model.Communication Communication { get; set; }
            public string ChannelName { get; set; }
            public int Recipients { get; set; }
            public int PendingRecipients { get; set; }
            public int CancelledRecipients { get; set; }
            public int FailedRecipients { get; set; }
            public int DeliveredRecipients { get; set; }
            public int OpenedRecipients { get; set; }
        }

    }
}