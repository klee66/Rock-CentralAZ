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

using Rock;
using Rock.Attribute;
using Rock.Communication;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web.Cache;

namespace RockWeb.Blocks.Security
{
    /// <summary>
    /// Block for user to request a forgotten username.
    /// </summary>
    [DisplayName( "Forgot Username" )]
    [Category( "Security" )]
    [Description( "Allows a user to get their forgotten username information emailed to them." )]

    [TextField( "Heading Caption", "", false, "<div class='alert alert-info'>Enter your email address below and we'll send your account information to you right away.</div>", "Captions", 0 )]
    [TextField( "Invalid Email Caption", "", false, "Sorry, we could not find an account for the email address you entered.", "Captions", 1 )]
    [TextField("Success Caption", "", false, "Your user name has been sent with instructions on how to change your password if needed.", "Captions", 2)]
    [LinkedPage( "Confirmation Page", "Page for user to confirm their account (if blank will use 'ConfirmAccount' page route)", true, "", "", 3 )]
    [EmailTemplateField("Forgot Username Email Template", "Email Template to send", false, Rock.SystemGuid.SystemEmail.SECURITY_FORGOT_USERNAME, "", 4, "EmailTemplate" )]
    public partial class ForgotUserName : Rock.Web.UI.RockBlock
    {
        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            pnlEntry.Visible = true;
            pnlWarning.Visible = false;
            pnlSuccess.Visible = false;

            if ( !Page.IsPostBack )
            {
                lCaption.Text = GetAttributeValue( "HeadingCaption" );
                lWarning.Text = GetAttributeValue( "InvalidEmailCaption" );
                lSuccess.Text = GetAttributeValue( "SuccessCaption" );
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the Click event of the btnSend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnSend_Click( object sender, EventArgs e )
        {
            var mergeObjects = new Dictionary<string, object>();

            var url = LinkedPageUrl( "ConfirmationPage" );
            if ( string.IsNullOrWhiteSpace( url ) )
            {
                url = ResolveRockUrl( "~/ConfirmAccount" );
            }

            mergeObjects.Add( "ConfirmAccountUrl", RootPath + url.TrimStart( new char[] { '/' } ) );

            var personDictionaries = new List<IDictionary<string, object>>();

            var rockContext = new RockContext();
            var personService = new PersonService( rockContext );
            var userLoginService = new UserLoginService( rockContext );

            foreach ( Person person in personService.GetByEmail( tbEmail.Text )
                .Where( p => p.Users.Any()))
            {
                var users = new List<UserLogin>();
                foreach ( UserLogin user in userLoginService.GetByPersonId( person.Id ) )
                {
                    if ( user.EntityType != null )
                    {
                        var component = AuthenticationContainer.GetComponent( user.EntityType.Name );
                        if ( component.ServiceType == AuthenticationServiceType.Internal )
                        {
                            users.Add( user );
                        }
                    }
                }

                if ( users.Count > 0 )
                {
                    var personDictionary = person.ToLiquid() as Dictionary<string, object>;
                    if (personDictionary.Keys.Contains("Users"))
                    {
                        personDictionary["Users"] = users;
                    }
                    else
                    {
                        personDictionary.Add( "Users", users );
                    }

                    personDictionaries.Add( personDictionary );
                }
            }

            if ( personDictionaries.Count > 0 )
            {
                mergeObjects.Add( "Persons", personDictionaries.ToArray() );
                var recipients = new Dictionary<string, Dictionary<string, object>>();
                recipients.Add( tbEmail.Text, mergeObjects );

                Email.Send( GetAttributeValue( "EmailTemplate" ).AsGuid(), recipients, ResolveRockUrlIncludeRoot( "~/" ), ResolveRockUrlIncludeRoot( "~~/" ) );

                pnlEntry.Visible = false;
                pnlSuccess.Visible = true;
            }
            else
            {
                pnlWarning.Visible = true;
            }
        }

        #endregion
    }
}