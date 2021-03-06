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
using System.Linq;
using System.Web.UI;

using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;
using Rock.Security;

namespace RockWeb.Blocks.Crm.PersonDetail
{
    /// <summary>
    /// Block for adding new families
    /// </summary>
    [DisplayName( "Add Family" )]
    [Category( "CRM > Person Detail" )]
    [Description( "Allows for adding new families." )]

    [GroupLocationTypeField( Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY, "Location Type",
        "The type of location that address should use", false, Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_HOME, "", 0 )]
    [DefinedValueField( Rock.SystemGuid.DefinedType.PERSON_CONNECTION_STATUS, "Default Connection Status",
        "The connection status that should be set by default", false, false, Rock.SystemGuid.DefinedValue.PERSON_CONNECTION_STATUS_VISITOR, "", 1 )]
    [BooleanField( "Gender", "Require a gender for each person", "Don't require", "Should Gender be required for each person added?", false, "", 2 )]
    [DefinedValueField( Rock.SystemGuid.DefinedType.PERSON_MARITAL_STATUS, "Adult Marital Status", "The default marital status for adults in the family.", false, false, "", "", 3)]
    [DefinedValueField( Rock.SystemGuid.DefinedType.PERSON_MARITAL_STATUS, "Child Marital Status", "The marital status to use for children in the family.", false, false,
        Rock.SystemGuid.DefinedValue.PERSON_MARITAL_STATUS_SINGLE, "", 4 )]
    [BooleanField( "Grade", "Require a grade for each child", "Don't require", "Should Grade be required for each child added?", false, "", 5 )]
    [AttributeCategoryField( "Attribute Categories", "The Attribute Categories to display attributes from", true, "Rock.Model.Person", false, "", "", 6 )]
    public partial class AddFamily : Rock.Web.UI.RockBlock
    {

        #region Fields

        private bool _requireGender = false;
        private bool _requireGrade = false;
        private int _childRoleId = 0;
        private List<NewFamilyAttributes> attributeControls = new List<NewFamilyAttributes>();
        private List<GroupMember> _groupMembers = null;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the index of the current category.
        /// </summary>
        /// <value>
        /// The index of the current category.
        /// </value>
        protected int CurrentCategoryIndex
        {
            get { return ViewState["CurrentCategoryIndex"] as int? ?? 0; }
            set { ViewState["CurrentCategoryIndex"] = value; }
        }

        #endregion

        #region Base Control Methods

        /// <summary>
        /// Restores the view-state information from a previous user control request that was saved by the <see cref="M:System.Web.UI.UserControl.SaveViewState" /> method.
        /// </summary>
        /// <param name="savedState">An <see cref="T:System.Object" /> that represents the user control state to be restored.</param>
        protected override void LoadViewState( object savedState )
        {
            base.LoadViewState( savedState );

            var familyMembers = new List<GroupMember>();
            List<string> jsonStrings = ViewState["FamilyMembers"] as List<string>;
            jsonStrings.ForEach( j => familyMembers.Add( GroupMember.FromJson( j ) ) );
            CreateControls( familyMembers, false );
        }
        
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            ddlMaritalStatus.BindToDefinedType( DefinedTypeCache.Read( Rock.SystemGuid.DefinedType.PERSON_MARITAL_STATUS.AsGuid() ) );
            var AdultMaritalStatus = DefinedValueCache.Read( GetAttributeValue( "AdultMaritalStatus" ).AsGuid() );
            if (AdultMaritalStatus != null)
            {
                ddlMaritalStatus.SetValue( AdultMaritalStatus.Id );
            }

            var campusi = new CampusService( new RockContext() ).Queryable().OrderBy( a => a.Name ).ToList();
            cpCampus.Campuses = campusi;
            cpCampus.Visible = campusi.Any();

            var familyGroupType = GroupTypeCache.GetFamilyGroupType();
            if (familyGroupType != null)
            {
                _childRoleId = familyGroupType.Roles
                    .Where( r => r.Guid.Equals( Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_CHILD.AsGuid() ) )
                    .Select( r => r.Id)
                    .FirstOrDefault();
            }

            bool.TryParse( GetAttributeValue( "Gender" ), out _requireGender );
            bool.TryParse( GetAttributeValue( "Grade" ), out _requireGrade );

            lTitle.Text = ("Add Family").FormatAsHtmlTitle(); 
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
                CreateControls( new List<GroupMember>(), false );
                AddFamilyMember();
            }
            else
            {
                // Update the name on attribute panels
                if ( CurrentCategoryIndex == 0 )
                {
                    foreach ( var familyMemberRow in nfmMembers.FamilyMemberRows )
                    {
                        foreach ( var attributeControl in attributeControls )
                        {
                            var attributeRow = attributeControl.AttributesRows.FirstOrDefault( r => r.PersonGuid == familyMemberRow.PersonGuid );
                            if ( attributeRow != null )
                            {
                                attributeRow.PersonName = string.Format( "{0} {1}", familyMemberRow.FirstName, familyMemberRow.LastName );
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves any user control view-state changes that have occurred since the last page postback.
        /// </summary>
        /// <returns>
        /// Returns the user control's current view state. If there is no view state associated with the control, it returns null.
        /// </returns>
        protected override object SaveViewState()
        {
            if ( _groupMembers == null )
            {
                _groupMembers = GetControlData();
            }

            var groupMembers = new List<string>();
            _groupMembers.ForEach( m => groupMembers.Add( m.ToJson() ) );

            ViewState["FamilyMembers"] = groupMembers;

            return base.SaveViewState();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnPreRender( EventArgs e )
        {
            if (_groupMembers == null)
            {
                _groupMembers = GetControlData();
            }

            var adults = _groupMembers.Where( m => m.GroupRoleId != _childRoleId).ToList();
            if (adults.Any())
            {
                //if ( adults.Any( a => a.Person.FirstName == "") )
                //{
                    lAdultCaption.Text = "The adults in this family are ";
                //}
                //else
                //{
                //    var firstNames = adults.Select( a => "<span class='adult-name'>" + a.Person.FirstName + "</span>" ).ToList();
                //    if ( firstNames.Count() > 1 )
                //    {
                //        lAdultCaption.Text = firstNames.AsDelimited( " and " ) + " are ";
                //    }
                //    else
                //    {
                //        lAdultCaption.Text = firstNames.FirstOrDefault() + " is ";
                //    }
                //}

                lAdultCaption.Visible = true;
                ddlMaritalStatus.Visible = true;
            }
            else
            {
                lAdultCaption.Visible = false;
                ddlMaritalStatus.Visible = false;
            }

            base.OnPreRender( e );
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the AddFamilyMemberClick event of the nfmMembers control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void nfmMembers_AddFamilyMemberClick( object sender, EventArgs e )
        {
            AddFamilyMember();
        }

        /// <summary>
        /// Handles the RoleUpdated event of the familyMemberRow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        void familyMemberRow_RoleUpdated( object sender, EventArgs e )
        {
            NewFamilyMembersRow row = sender as NewFamilyMembersRow;
            row.ShowGrade = row.RoleId == _childRoleId;
        }

        /// <summary>
        /// Handles the DeleteClick event of the familyMemberRow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void familyMemberRow_DeleteClick( object sender, EventArgs e )
        {
            NewFamilyMembersRow row = sender as NewFamilyMembersRow;

            foreach ( var attributeControl in attributeControls )
            {
                var attributeRow = attributeControl.AttributesRows.FirstOrDefault( r => r.PersonGuid == row.PersonGuid );
                if ( attributeRow != null )
                {
                    attributeControl.Controls.Remove( attributeRow );
                }
            }

            nfmMembers.Controls.Remove( row );
        }

        protected void btnPrevious_Click( object sender, EventArgs e )
        {
            if ( CurrentCategoryIndex > 0 )
            {
                CurrentCategoryIndex--;
                ShowAttributeCategory( CurrentCategoryIndex );
            }
        }

        protected void btnNext_Click( object sender, EventArgs e )
        {
            if ( Page.IsValid )
            {
                if ( CurrentCategoryIndex < attributeControls.Count )
                {
                    CurrentCategoryIndex++;
                    ShowAttributeCategory( CurrentCategoryIndex );
                }
                else
                {
                    var familyMembers = GetControlData();
                    if ( familyMembers.Any() )
                    {
                        var rockContext = new RockContext();
                        RockTransactionScope.WrapTransaction( () =>
                        {
                            var familyGroup = GroupService.SaveNewFamily( rockContext, familyMembers, cpCampus.SelectedValueAsInt(), true );
                            if ( familyGroup != null )
                            {
                                GroupService.AddNewFamilyAddress( rockContext, familyGroup, GetAttributeValue( "LocationType" ),
                                    tbStreet1.Text, tbStreet2.Text, tbCity.Text, ddlState.SelectedValue, tbZip.Text );
                            }
                        } );

                        Response.Redirect( string.Format( "~/Person/{0}", familyMembers[0].Person.Id ), false );
                    }

                }
            }

        }

        #endregion

        #region Methods

        private void CreateControls( List<GroupMember> familyMembers, bool setSelection )
        {
            // Load all the attribute controls
            attributeControls.Clear();
            pnlAttributes.Controls.Clear();

            foreach ( string categoryGuid in GetAttributeValue( "AttributeCategories" ).SplitDelimitedValues( false ) )
            {
                Guid guid = Guid.Empty;
                if ( Guid.TryParse( categoryGuid, out guid ) )
                {
                    var category = CategoryCache.Read( guid );
                    if ( category != null )
                    {
                        var attributeControl = new NewFamilyAttributes();
                        attributeControl.ClearRows();
                        pnlAttributes.Controls.Add( attributeControl );
                        attributeControls.Add( attributeControl );
                        attributeControl.ID = "familyAttributes_" + category.Id.ToString();
                        attributeControl.CategoryId = category.Id;

                        foreach ( var attribute in new AttributeService( new RockContext() ).GetByCategoryId( category.Id ) )
                        {
                            if ( attribute.IsAuthorized( Authorization.EDIT, CurrentPerson ) )
                            {
                                attributeControl.AttributeList.Add( AttributeCache.Read( attribute ) );
                            }
                        }
                    }
                }
            }

            nfmMembers.ClearRows();

            foreach ( var familyMember in familyMembers )
            {
                var familyMemberRow = new NewFamilyMembersRow();
                nfmMembers.Controls.Add( familyMemberRow );
                familyMemberRow.ID = string.Format( "row_{0}", familyMember.Person.Guid.ToString().Replace( "-", "_" ) );
                familyMemberRow.RoleUpdated += familyMemberRow_RoleUpdated;
                familyMemberRow.DeleteClick += familyMemberRow_DeleteClick;
                familyMemberRow.PersonGuid = familyMember.Person.Guid;
                familyMemberRow.RequireGender = _requireGender;
                familyMemberRow.RequireGrade = _requireGrade;
                familyMemberRow.RoleId = familyMember.GroupRoleId;
                familyMemberRow.ShowGrade = familyMember.GroupRoleId == _childRoleId;

                if ( setSelection )
                {
                    if ( familyMember.Person != null )
                    {
                        familyMemberRow.TitleValueId = familyMember.Person.TitleValueId;
                        familyMemberRow.FirstName = familyMember.Person.FirstName;
                        familyMemberRow.LastName = familyMember.Person.LastName;
                        familyMemberRow.SuffixValueId = familyMember.Person.SuffixValueId;
                        familyMemberRow.Gender = familyMember.Person.Gender;
                        familyMemberRow.BirthDate = familyMember.Person.BirthDate;
                        familyMemberRow.ConnectionStatusValueId = familyMember.Person.ConnectionStatusValueId;
                        familyMemberRow.Grade = familyMember.Person.Grade;
                    }
                }

                foreach ( var attributeControl in attributeControls )
                {
                    var attributeRow = new NewFamilyAttributesRow();
                    attributeControl.Controls.Add( attributeRow );
                    attributeRow.ID = string.Format( "{0}_{1}", attributeControl.ID, familyMember.Person.Guid );
                    attributeRow.AttributeList = attributeControl.AttributeList;
                    attributeRow.PersonGuid = familyMember.Person.Guid;

                    if ( setSelection )
                    {
                        attributeRow.SetEditValues( familyMember.Person );
                    }
                }
            }

            ShowAttributeCategory( CurrentCategoryIndex );
        }

        private List<GroupMember> GetControlData()
        {
            var familyMembers = new List<GroupMember>();

            int? childMaritalStatusId = null;
            var childMaritalStatus = DefinedValueCache.Read( GetAttributeValue("ChildMaritalStatus").AsGuid() );
            if ( childMaritalStatus != null )
            {
                childMaritalStatusId = childMaritalStatus.Id;
            }
            int? adultMaritalStatusId = ddlMaritalStatus.SelectedValueAsInt();

            int recordTypePersonId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_RECORD_TYPE_PERSON.AsGuid() ).Id;
            int recordStatusActiveId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_RECORD_STATUS_ACTIVE.AsGuid() ).Id;

            foreach ( NewFamilyMembersRow row in nfmMembers.FamilyMemberRows )
            {
                var groupMember = new GroupMember();
                groupMember.Person = new Person();
                groupMember.Person.Guid = row.PersonGuid.Value;
                groupMember.Person.RecordTypeValueId = recordTypePersonId;
                groupMember.Person.RecordStatusValueId = recordStatusActiveId;

                if ( row.RoleId.HasValue )
                {
                    groupMember.GroupRoleId = row.RoleId.Value;

                    if (groupMember.GroupRoleId == _childRoleId)
                    {
                        groupMember.Person.MaritalStatusValueId = childMaritalStatusId;
                    }
                    else
                    {
                        groupMember.Person.MaritalStatusValueId = adultMaritalStatusId;
                    }
                }

                groupMember.Person.TitleValueId = row.TitleValueId;
                groupMember.Person.FirstName = row.FirstName;
                groupMember.Person.NickName = groupMember.Person.FirstName;
                groupMember.Person.LastName = row.LastName;
                groupMember.Person.SuffixValueId = row.SuffixValueId;
                groupMember.Person.Gender = row.Gender;
                groupMember.Person.BirthDate = row.BirthDate;
                groupMember.Person.ConnectionStatusValueId = row.ConnectionStatusValueId;
                groupMember.Person.Grade = row.Grade;

                groupMember.Person.EmailPreference = EmailPreference.EmailAllowed;

                groupMember.Person.LoadAttributes();

                foreach ( var attributeControl in attributeControls )
                {
                    NewFamilyAttributesRow attributeRow = attributeControl.AttributesRows.FirstOrDefault( r => r.PersonGuid == row.PersonGuid );
                    if ( attributeRow != null )
                    {
                        attributeRow.GetEditValues( groupMember.Person );
                    }
                }

                familyMembers.Add( groupMember );
            }

            return familyMembers;
        }

        private void AddFamilyMember()
        {
            var rows = nfmMembers.FamilyMemberRows;
            var familyMemberGuid = Guid.NewGuid();

            var familyMemberRow = new NewFamilyMembersRow();
            nfmMembers.Controls.Add( familyMemberRow );
            familyMemberRow.ID = string.Format( "row_{0}", familyMemberGuid.ToString().Replace( "-", "_" ) );
            familyMemberRow.RoleUpdated += familyMemberRow_RoleUpdated;
            familyMemberRow.DeleteClick += familyMemberRow_DeleteClick;
            familyMemberRow.PersonGuid = familyMemberGuid;
            familyMemberRow.Gender = Gender.Unknown;
            familyMemberRow.RequireGender = _requireGender;
            familyMemberRow.RequireGrade = _requireGrade;
            familyMemberRow.ValidationGroup = BlockValidationGroup;

            var familyGroupType = GroupTypeCache.GetFamilyGroupType();
            if ( familyGroupType != null && familyGroupType.DefaultGroupRoleId.HasValue )
            {
                familyMemberRow.RoleId = familyGroupType.DefaultGroupRoleId;
                familyMemberRow.ShowGrade = familyGroupType.DefaultGroupRoleId == _childRoleId;
            }
            else
            {
                familyMemberRow.ShowGrade = false;
            }

            var ConnectionStatusValue = DefinedValueCache.Read( GetAttributeValue( "DefaultConnectionStatus" ).AsGuid() );
            if ( ConnectionStatusValue != null )
            {
                familyMemberRow.ConnectionStatusValueId = ConnectionStatusValue.Id;
            }

            if ( rows.Count > 0 )
            {
                familyMemberRow.LastName = rows[0].LastName;
            }

            foreach ( var attributeControl in attributeControls )
            {
                var attributeRow = new NewFamilyAttributesRow();
                attributeControl.Controls.Add( attributeRow );
                attributeRow.ID = string.Format( "{0}_{1}", attributeControl.ID, familyMemberGuid );
                attributeRow.AttributeList = attributeControl.AttributeList;
                attributeRow.PersonGuid = familyMemberGuid;
            }
        }

        private void ShowAttributeCategory( int index )
        {
            pnlFamilyData.Visible = ( index == 0 );

            attributeControls.ForEach( c => c.Visible = false );
            if ( index > 0 && attributeControls.Count >= index )
            {
                attributeControls[index - 1].Visible = true;
            }

            btnPrevious.Visible = index > 0;
            btnNext.Text = index < attributeControls.Count ? "Next" : "Finish";
        }

        #endregion
    }
}