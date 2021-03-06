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
using System.ComponentModel;
using System.Collections.Generic;
using System.Web.UI;
using Rock;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI;
using CronExpressionDescriptor;
using Rock.Security;

namespace RockWeb.Blocks.Administration
{
    /// <summary>
    /// User controls for managing scheduled jobs.
    /// </summary>
    [DisplayName( "Scheduled Job Detail" )]
    [Category( "Core" )]
    [Description( "Displays the details of the given scheduled job." )]
    public partial class ScheduledJobDetail : RockBlock
    {
        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                string itemId = PageParameter( "serviceJobId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    ShowDetail( "serviceJobId", int.Parse( itemId ) );
                }
                else
                {
                    pnlDetails.Visible = false;
                }
            }

            if ( pnlDetails.Visible )
            {
                var job = new ServiceJob { Id = int.Parse( hfId.Value ), Class = ddlJobTypes.SelectedValue ?? "Rock.Jobs.JobPulse" };
                if ( job.Id > 0 )
                {
                    job.LoadAttributes();
                    phAttributes.Controls.Clear();
                    Rock.Attribute.Helper.AddEditControls( job, phAttributes, true );
                }
            }
        }

        #endregion

        #region Edit Events

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnCancel_Click( object sender, EventArgs e )
        {
            NavigateToParentPage();
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            ServiceJob job;
            var rockContext = new RockContext();
            ServiceJobService jobService = new ServiceJobService( rockContext );

            int jobId = int.Parse( hfId.Value );

            if ( jobId == 0 )
            {
                job = new ServiceJob();
                jobService.Add( job );
            }
            else
            {
                job = jobService.Get( jobId );
            }

            job.Name = tbName.Text;
            job.Description = tbDescription.Text;
            job.IsActive = cbActive.Checked;
            job.Class = ddlJobTypes.SelectedValue;
            job.NotificationEmails = tbNotificationEmails.Text;
            job.NotificationStatus = (JobNotificationStatus)int.Parse( ddlNotificationStatus.SelectedValue );
            job.CronExpression = tbCronExpression.Text;

            if ( !job.IsValid )
            {
                // Controls will render the error messages                    
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                rockContext.SaveChanges();

                job.LoadAttributes( rockContext );
                Rock.Attribute.Helper.GetEditValues( phAttributes, job );
                job.SaveAttributeValues( rockContext );

            } );

            NavigateToParentPage();
        }

        protected void ddlJobTypes_SelectedIndexChanged( object sender, EventArgs e )
        {
            ServiceJob job;
            var itemId = int.Parse( PageParameter( "serviceJobId" ) );
            if ( itemId == 0 )
            {
                job = new ServiceJob { Id = 0, IsActive = true };
            }
            else
            {
                job = new ServiceJobService( new RockContext() ).Get( itemId );
            }

            job.Class = ddlJobTypes.SelectedValue;
            job.LoadAttributes();
            phAttributes.Controls.Clear();
            Rock.Attribute.Helper.AddEditControls( job, phAttributes, true );
            
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        public void ShowDetail( string itemKey, int itemKeyValue )
        {
            // return if unexpected itemKey 
            if ( itemKey != "serviceJobId" )
            {
                return;
            }

            pnlDetails.Visible = true;
            LoadDropDowns();

            // Load depending on Add(0) or Edit
            ServiceJob job;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                job = new ServiceJobService( new RockContext() ).Get( itemKeyValue );
                lActionTitle.Text = ActionTitle.Edit( ServiceJob.FriendlyTypeName ).FormatAsHtmlTitle();
            }
            else
            {
                job = new ServiceJob { Id = 0, IsActive = true };
                lActionTitle.Text = ActionTitle.Add( ServiceJob.FriendlyTypeName ).FormatAsHtmlTitle();
            }

            hfId.Value = job.Id.ToString();
            tbName.Text = job.Name;
            tbDescription.Text = job.Description;
            cbActive.Checked = job.IsActive.HasValue ? job.IsActive.Value : false;
            ddlJobTypes.SelectedValue = job.Class;
            tbNotificationEmails.Text = job.NotificationEmails;
            ddlNotificationStatus.SetValue( (int)job.NotificationStatus );
            tbCronExpression.Text = job.CronExpression;

            if (job.Id == 0)
            {
                job.Class = ddlJobTypes.SelectedValue;
                lCronExpressionDesc.Visible = false;
            }
            else
            {
                lCronExpressionDesc.Text = ExpressionDescriptor.GetDescription( job.CronExpression );
                lCronExpressionDesc.Visible = true;
            }

            job.LoadAttributes();
            phAttributes.Controls.Clear();
            Rock.Attribute.Helper.AddEditControls( job, phAttributes, true );

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( Authorization.EDIT ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( ServiceJob.FriendlyTypeName );
            }

            if ( job.IsSystem )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlySystem( ServiceJob.FriendlyTypeName );
            }

            if ( readOnly )
            {
                lActionTitle.Text = ActionTitle.View( ServiceJob.FriendlyTypeName ).FormatAsHtmlTitle();
                btnCancel.Text = "Close";
                Rock.Attribute.Helper.AddDisplayControls( job, phAttributesReadOnly );
                phAttributesReadOnly.Visible = true;
                phAttributes.Visible = false;
                tbCronExpression.Text = job.CronExpression;
            }

            
            tbName.ReadOnly = readOnly;
            tbDescription.ReadOnly = readOnly;
            cbActive.Enabled = !readOnly;
            ddlJobTypes.Enabled = !readOnly;
            tbNotificationEmails.ReadOnly = readOnly;
            ddlNotificationStatus.Enabled = !readOnly;
            tbCronExpression.ReadOnly = readOnly;

            btnSave.Visible = !readOnly;
        }

        /// <summary>
        /// Loads the drop downs.
        /// </summary>
        private void LoadDropDowns()
        {
            ddlNotificationStatus.BindToEnum( typeof( JobNotificationStatus ) );

            int? jobEntityTypeId = Rock.Web.Cache.EntityTypeCache.Read( "Rock.Model.ServiceJob" ).Id;

            var jobs = Rock.Reflection.FindTypes( typeof( Quartz.IJob ) ).Values;

            var rockContext = new RockContext();
            foreach ( var job in jobs )
            {
                Rock.Attribute.Helper.UpdateAttributes( job, jobEntityTypeId, "Class", job.FullName, rockContext );
            }

            ddlJobTypes.DataSource = jobs;
            ddlJobTypes.DataBind();
        }

        #endregion

    }
}