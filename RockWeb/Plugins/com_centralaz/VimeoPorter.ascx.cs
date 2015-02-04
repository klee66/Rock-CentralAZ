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
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;
using Rock.Attribute;

namespace RockWeb.Plugins.com_centralaz
{
    /// <summary>
    /// Template block for developers to use to start a new block.
    /// </summary>
    [DisplayName( "Vimeo Porter" )]
    [Category( "WebBlocks" )]
    [Description( "Template block for developers to use to start a new detail block." )]
    [EmailField("Email")]
    [IntegerField("Vimeo Vid ID","vimeo.com/XXXXX, this block uses this value to fetch embeded video from Vimeo", true)]
    public partial class VimeoPorter : Rock.Web.UI.RockBlock
    {
        #region Fields

        // used for private variables

        #endregion

        #region Properties

        // used for public / protected properties

        #endregion

        #region Base Control Methods

        //  overrides of the base RockBlock methods (i.e. OnInit, OnLoad)

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {


                // added for your convenience
            }
        }

        public override List<Control> GetAdministrateControls(bool canConfig, bool canEdit)
        {
            List<Control> configControls = new List<Control>();
            // add edit icon to config controls if user has edit permission
            if (canConfig || canEdit)
            {
                LinkButton lbEdit = new LinkButton();
                lbEdit.CssClass = "edit";
                lbEdit.ToolTip = "Edit HTML";
                lbEdit.Click += new EventHandler(mdDialog_Show);
                configControls.Add(lbEdit);
                HtmlGenericControl iEdit = new HtmlGenericControl("i");
                lbEdit.Controls.Add(iEdit);
                lbEdit.CausesValidation = false;
                iEdit.Attributes.Add("class", "fa fa-pencil-square-o");
                ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl(lbEdit);
            }
            configControls.AddRange(base.GetAdministrateControls(canConfig, canEdit));
            return configControls;
        }

        #endregion

        #region Events

        // handlers called by the controls on your block

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {

        }

        protected void mdDialog_Show(object sender, EventArgs e)
        {
            mdDialog.Show();
            //pnlView.Visible = false;
            //pnlEdit.Visible = true;
            tbVimeoVidID.Text = GetAttributeValue("VimeoVidID");
            
        }

        protected void mdDialog_SaveClick(object sender, EventArgs e)
        {
            int number = 0;
            if (Int32.TryParse(tbVimeoVidID.Text, out number))
            {
                mdDialog.Hide();
                SetAttributeValue("VimeoVidID", tbVimeoVidID.Text);
                SaveAttributeValues();
                //pnlView.Visible = true;
                //pnlEdit.Visible = false;
            }
            else
            {
                mdDialog.Show();
            }
        }
}
        #endregion

        #region Methods

        // helper functional methods (like BindGrid(), etc.)

        #endregion

}