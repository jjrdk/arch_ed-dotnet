'
'
'	component:   "openEHR Archetype Project"
'	description: "$DESCRIPTION"
'	keywords:    "Archetype, Clinical, Editor"
'	author:      "Peter Gummer"
'	support:     https://openehr.atlassian.net/browse/AEPR
'	copyright:   "Copyright (c) 2012 Ocean Informatics Pty Ltd"
'	license:     "See notice at bottom of class"
'
'	file:        "$URL: http://www.openehr.org/svn/knowledge_tools_dotnet/TRUNK/ArchetypeEditor/DataConstraintsView/ParsableViewControl.vb $"
'	revision:    "$LastChangedRevision: 676 $"
'	last_change: "$LastChangedDate: 2010-11-11 11:55:24 +1100 (Thu, 11 Nov 2010) $"
'
'

Option Strict On

Public Class ParsableViewControl : Inherits ElementViewControl

    Private WithEvents mComboBox As ComboBox

    Public Sub New(ByVal element As ArchetypeElement, ByVal filemanager As FileManagerLocal)
        MyBase.New(element, filemanager)
    End Sub

    Public Sub New(ByVal constraint As Constraint, ByVal filemanager As FileManagerLocal)
        MyBase.New(constraint, filemanager)
    End Sub

    Protected Overrides Sub InitialiseComponent(ByVal constraint As Constraint, ByVal location As System.Drawing.Point)
        Dim parsableConstraint As Constraint_Parsable = CType(constraint, Constraint_Parsable)
        Dim length As Integer = 150

        mComboBox = New ComboBox
        mComboBox.Location = location
        mComboBox.Height = 25
        mComboBox.Width = 150

        For Each s As String In parsableConstraint.AllowableFormalisms
            If length < s.Length And s.Length <= 250 Then
                length = s.Length
            End If

            mComboBox.Items.Add(s)
        Next

        mComboBox.Width = length
        Controls.Add(mComboBox)
    End Sub

    Private Sub ComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mComboBox.SelectedIndexChanged
        Tag = mComboBox.Text
        Value = mComboBox.Text
    End Sub

    Private mValue As String

    Public Overrides Property Value() As Object
        Get
            Return mValue
        End Get
        Set(ByVal Value As Object)
            mValue = CStr(Value)
            MyBase.OnValueChanged()
        End Set
    End Property

End Class

'
'***** BEGIN LICENSE BLOCK *****
'Version: MPL 1.1/GPL 2.0/LGPL 2.1
'
'The contents of this file are subject to the Mozilla Public License Version 
'1.1 (the "License"); you may not use this file except in compliance with 
'the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/
'
'Software distributed under the License is distributed on an "AS IS" basis,
'WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
'for the specific language governing rights and limitations under the
'License.
'
'The Original Code is TextViewControl.vb.
'
'The Initial Developer of the Original Code is
'Sam Heard, Ocean Informatics (www.oceaninformatics.biz).
'Portions created by the Initial Developer are Copyright (C) 2004
'the Initial Developer. All Rights Reserved.
'
'Contributor(s):
'	Heath Frankel
'
'Alternatively, the contents of this file may be used under the terms of
'either the GNU General Public License Version 2 or later (the "GPL"), or
'the GNU Lesser General Public License Version 2.1 or later (the "LGPL"),
'in which case the provisions of the GPL or the LGPL are applicable instead
'of those above. If you wish to allow use of your version of this file only
'under the terms of either the GPL or the LGPL, and not to allow others to
'use your version of this file under the terms of the MPL, indicate your
'decision by deleting the provisions above and replace them with the notice
'and other provisions required by the GPL or the LGPL. If you do not delete
'the provisions above, a recipient may use your version of this file under
'the terms of any one of the MPL, the GPL or the LGPL.
'
'***** END LICENSE BLOCK *****
'
