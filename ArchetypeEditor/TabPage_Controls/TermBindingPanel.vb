'
'
'	component:   "openEHR Archetype Project"
'	description: "$DESCRIPTION"
'	keywords:    "Archetype, Clinical, Editor"
'	author:      "Sam Heard"
'	support:     https://openehr.atlassian.net/browse/AEPR
'	copyright:   "Copyright (c) 2004,2005,2006 Ocean Informatics Pty Ltd"
'	license:     "See notice at bottom of class"
'
'

Option Strict On


Public Class TermBindingPanel
    Inherits System.Windows.Forms.UserControl

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        If Not DesignMode Then
            mFileManager = Filemanager.Master


            '' Term Binding data view
            mTermBindingView = New DataView(mFileManager.OntologyManager.TermBindingsTable)
            mTermBindingView.AllowNew = False

            mTermBindingCriteriaView = New DataView(mFileManager.OntologyManager.TermBindingCriteriaTable)
            mTermBindingCriteriaView.AllowNew = False
        End If

    End Sub

    'UserControl overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents PathsTreeView As System.Windows.Forms.TreeView
    Friend WithEvents AddBindingButton As System.Windows.Forms.Button
    Friend WithEvents BindingImageList As System.Windows.Forms.ImageList
    Friend WithEvents DeleteBindingButton As System.Windows.Forms.Button
    Friend WithEvents AddCriteriaButton As System.Windows.Forms.Button
    Friend WithEvents DeleteCriteriaButton As System.Windows.Forms.Button
    Friend WithEvents BindingCriteriaListBox As System.Windows.Forms.ListBox
    Friend WithEvents BindingList As System.Windows.Forms.ListView
    Friend WithEvents BindingOkButton As System.Windows.Forms.Button
    Friend WithEvents ReleaseTextBox As System.Windows.Forms.TextBox
    Friend WithEvents CodeTextBox As System.Windows.Forms.TextBox
    Friend WithEvents NodeLabel As System.Windows.Forms.Label
    Friend WithEvents PathRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents NodeRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents AddBindingGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents AddBindingCriteriaGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents BindingGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents CancelAddBindingButton As System.Windows.Forms.Button
    Friend WithEvents TopPanel As System.Windows.Forms.Panel
    Friend WithEvents AddTerminologyButton As System.Windows.Forms.Button
    Friend WithEvents OperatorComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents CriteriaValueTextBox As System.Windows.Forms.TextBox
    Friend WithEvents CriteriaTermLabel As System.Windows.Forms.Label
    Friend WithEvents BindingCriteriaCancelButton As System.Windows.Forms.Button
    Friend WithEvents CriteriaPathRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents CriteriaNodeRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents CriteriaOkButton As System.Windows.Forms.Button
    Friend WithEvents TerminologyComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents PanelBindings As System.Windows.Forms.Panel
    Friend WithEvents gbCriteria As System.Windows.Forms.GroupBox
    Friend WithEvents TerminologyLabel As System.Windows.Forms.Label
    Friend WithEvents lblCode As System.Windows.Forms.Label
    Friend WithEvents BindingCodeListColumnHeader As System.Windows.Forms.ColumnHeader
    Friend WithEvents ContextMenuTermBinding As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmBindingPastePath As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmBindingPastePathLogical As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmBindingPastePathPhysical As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblRelease As System.Windows.Forms.Label
    Friend WithEvents tabBindings As Crownwood.Magic.Controls.TabControl
    Friend WithEvents tpSimple As Crownwood.Magic.Controls.TabPage
    Friend WithEvents dgTermBindings As System.Windows.Forms.DataGridView
    Friend WithEvents BindingToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents PathLabel As System.Windows.Forms.Label
    Friend WithEvents TerminologyColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NodeColumn As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents CodeColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ReleaseColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents tpComplex As Crownwood.Magic.Controls.TabPage

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TermBindingPanel))
        Me.PathsTreeView = New System.Windows.Forms.TreeView
        Me.ContextMenuTermBinding = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmBindingPastePath = New System.Windows.Forms.ToolStripMenuItem
        Me.cmBindingPastePathLogical = New System.Windows.Forms.ToolStripMenuItem
        Me.cmBindingPastePathPhysical = New System.Windows.Forms.ToolStripMenuItem
        Me.BindingImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.BindingGroupBox = New System.Windows.Forms.GroupBox
        Me.BindingList = New System.Windows.Forms.ListView
        Me.BindingCodeListColumnHeader = New System.Windows.Forms.ColumnHeader
        Me.DeleteBindingButton = New System.Windows.Forms.Button
        Me.AddBindingButton = New System.Windows.Forms.Button
        Me.DeleteCriteriaButton = New System.Windows.Forms.Button
        Me.BindingCriteriaListBox = New System.Windows.Forms.ListBox
        Me.AddCriteriaButton = New System.Windows.Forms.Button
        Me.AddBindingGroupBox = New System.Windows.Forms.GroupBox
        Me.PathLabel = New System.Windows.Forms.Label
        Me.CancelAddBindingButton = New System.Windows.Forms.Button
        Me.BindingOkButton = New System.Windows.Forms.Button
        Me.lblRelease = New System.Windows.Forms.Label
        Me.lblCode = New System.Windows.Forms.Label
        Me.ReleaseTextBox = New System.Windows.Forms.TextBox
        Me.CodeTextBox = New System.Windows.Forms.TextBox
        Me.NodeLabel = New System.Windows.Forms.Label
        Me.PathRadioButton = New System.Windows.Forms.RadioButton
        Me.NodeRadioButton = New System.Windows.Forms.RadioButton
        Me.AddBindingCriteriaGroupBox = New System.Windows.Forms.GroupBox
        Me.CriteriaPathRadioButton = New System.Windows.Forms.RadioButton
        Me.CriteriaNodeRadioButton = New System.Windows.Forms.RadioButton
        Me.CriteriaTermLabel = New System.Windows.Forms.Label
        Me.OperatorComboBox = New System.Windows.Forms.ComboBox
        Me.BindingCriteriaCancelButton = New System.Windows.Forms.Button
        Me.CriteriaOkButton = New System.Windows.Forms.Button
        Me.CriteriaValueTextBox = New System.Windows.Forms.TextBox
        Me.BindingToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.TopPanel = New System.Windows.Forms.Panel
        Me.AddTerminologyButton = New System.Windows.Forms.Button
        Me.TerminologyComboBox = New System.Windows.Forms.ComboBox
        Me.TerminologyLabel = New System.Windows.Forms.Label
        Me.PanelBindings = New System.Windows.Forms.Panel
        Me.gbCriteria = New System.Windows.Forms.GroupBox
        Me.tabBindings = New Crownwood.Magic.Controls.TabControl
        Me.tpSimple = New Crownwood.Magic.Controls.TabPage
        Me.dgTermBindings = New System.Windows.Forms.DataGridView
        Me.TerminologyColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.NodeColumn = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.CodeColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ReleaseColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.tpComplex = New Crownwood.Magic.Controls.TabPage
        Me.ContextMenuTermBinding.SuspendLayout()
        Me.BindingGroupBox.SuspendLayout()
        Me.AddBindingGroupBox.SuspendLayout()
        Me.AddBindingCriteriaGroupBox.SuspendLayout()
        Me.TopPanel.SuspendLayout()
        Me.PanelBindings.SuspendLayout()
        Me.gbCriteria.SuspendLayout()
        Me.tpSimple.SuspendLayout()
        CType(Me.dgTermBindings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpComplex.SuspendLayout()
        Me.SuspendLayout()
        '
        'PathsTreeView
        '
        Me.PathsTreeView.ContextMenuStrip = Me.ContextMenuTermBinding
        Me.PathsTreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PathsTreeView.HideSelection = False
        Me.PathsTreeView.ImageIndex = 0
        Me.PathsTreeView.ImageList = Me.BindingImageList
        Me.PathsTreeView.Location = New System.Drawing.Point(0, 0)
        Me.PathsTreeView.Name = "PathsTreeView"
        Me.PathsTreeView.PathSeparator = "/"
        Me.PathsTreeView.SelectedImageIndex = 0
        Me.PathsTreeView.ShowRootLines = False
        Me.PathsTreeView.Size = New System.Drawing.Size(880, 161)
        Me.PathsTreeView.TabIndex = 1
        '
        'ContextMenuTermBinding
        '
        Me.ContextMenuTermBinding.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmBindingPastePath})
        Me.ContextMenuTermBinding.Name = "ContextMenuTermBinding"
        Me.ContextMenuTermBinding.Size = New System.Drawing.Size(184, 26)
        '
        'cmBindingPastePath
        '
        Me.cmBindingPastePath.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmBindingPastePathLogical, Me.cmBindingPastePathPhysical})
        Me.cmBindingPastePath.Name = "cmBindingPastePath"
        Me.cmBindingPastePath.Size = New System.Drawing.Size(183, 22)
        Me.cmBindingPastePath.Text = "Copy path to clipboard"
        '
        'cmBindingPastePathLogical
        '
        Me.cmBindingPastePathLogical.Name = "cmBindingPastePathLogical"
        Me.cmBindingPastePathLogical.Size = New System.Drawing.Size(137, 22)
        Me.cmBindingPastePathLogical.Text = "Logical path"
        '
        'cmBindingPastePathPhysical
        '
        Me.cmBindingPastePathPhysical.Name = "cmBindingPastePathPhysical"
        Me.cmBindingPastePathPhysical.Size = New System.Drawing.Size(137, 22)
        Me.cmBindingPastePathPhysical.Text = "Physical path"
        '
        'BindingImageList
        '
        Me.BindingImageList.ImageStream = CType(resources.GetObject("BindingImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.BindingImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.BindingImageList.Images.SetKeyName(0, "")
        Me.BindingImageList.Images.SetKeyName(1, "")
        Me.BindingImageList.Images.SetKeyName(2, "")
        Me.BindingImageList.Images.SetKeyName(3, "")
        Me.BindingImageList.Images.SetKeyName(4, "")
        Me.BindingImageList.Images.SetKeyName(5, "")
        Me.BindingImageList.Images.SetKeyName(6, "")
        '
        'BindingGroupBox
        '
        Me.BindingGroupBox.Controls.Add(Me.BindingList)
        Me.BindingGroupBox.Controls.Add(Me.DeleteBindingButton)
        Me.BindingGroupBox.Controls.Add(Me.AddBindingButton)
        Me.BindingGroupBox.Dock = System.Windows.Forms.DockStyle.Left
        Me.BindingGroupBox.Location = New System.Drawing.Point(0, 0)
        Me.BindingGroupBox.Name = "BindingGroupBox"
        Me.BindingGroupBox.Size = New System.Drawing.Size(224, 96)
        Me.BindingGroupBox.TabIndex = 1
        Me.BindingGroupBox.TabStop = False
        Me.BindingGroupBox.Text = "Bindings"
        '
        'BindingList
        '
        Me.BindingList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.BindingCodeListColumnHeader})
        Me.BindingList.FullRowSelect = True
        Me.BindingList.HideSelection = False
        Me.BindingList.Location = New System.Drawing.Point(56, 16)
        Me.BindingList.MultiSelect = False
        Me.BindingList.Name = "BindingList"
        Me.BindingList.Size = New System.Drawing.Size(160, 69)
        Me.BindingList.SmallImageList = Me.BindingImageList
        Me.BindingList.TabIndex = 8
        Me.BindingList.UseCompatibleStateImageBehavior = False
        Me.BindingList.View = System.Windows.Forms.View.Details
        '
        'BindingCodeListColumnHeader
        '
        Me.BindingCodeListColumnHeader.Text = "Code"
        Me.BindingCodeListColumnHeader.Width = 156
        '
        'DeleteBindingButton
        '
        Me.DeleteBindingButton.Image = CType(resources.GetObject("DeleteBindingButton.Image"), System.Drawing.Image)
        Me.DeleteBindingButton.Location = New System.Drawing.Point(24, 49)
        Me.DeleteBindingButton.Name = "DeleteBindingButton"
        Me.DeleteBindingButton.Size = New System.Drawing.Size(26, 27)
        Me.DeleteBindingButton.TabIndex = 7
        Me.DeleteBindingButton.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'AddBindingButton
        '
        Me.AddBindingButton.Image = CType(resources.GetObject("AddBindingButton.Image"), System.Drawing.Image)
        Me.AddBindingButton.Location = New System.Drawing.Point(24, 22)
        Me.AddBindingButton.Name = "AddBindingButton"
        Me.AddBindingButton.Size = New System.Drawing.Size(26, 27)
        Me.AddBindingButton.TabIndex = 6
        Me.AddBindingButton.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'DeleteCriteriaButton
        '
        Me.DeleteCriteriaButton.Image = CType(resources.GetObject("DeleteCriteriaButton.Image"), System.Drawing.Image)
        Me.DeleteCriteriaButton.Location = New System.Drawing.Point(15, 51)
        Me.DeleteCriteriaButton.Name = "DeleteCriteriaButton"
        Me.DeleteCriteriaButton.Size = New System.Drawing.Size(26, 27)
        Me.DeleteCriteriaButton.TabIndex = 10
        '
        'BindingCriteriaListBox
        '
        Me.BindingCriteriaListBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BindingCriteriaListBox.Location = New System.Drawing.Point(48, 16)
        Me.BindingCriteriaListBox.Name = "BindingCriteriaListBox"
        Me.BindingCriteriaListBox.Size = New System.Drawing.Size(600, 30)
        Me.BindingCriteriaListBox.TabIndex = 11
        Me.BindingCriteriaListBox.TabStop = False
        '
        'AddCriteriaButton
        '
        Me.AddCriteriaButton.Image = CType(resources.GetObject("AddCriteriaButton.Image"), System.Drawing.Image)
        Me.AddCriteriaButton.Location = New System.Drawing.Point(16, 22)
        Me.AddCriteriaButton.Name = "AddCriteriaButton"
        Me.AddCriteriaButton.Size = New System.Drawing.Size(26, 27)
        Me.AddCriteriaButton.TabIndex = 9
        '
        'AddBindingGroupBox
        '
        Me.AddBindingGroupBox.Controls.Add(Me.PathLabel)
        Me.AddBindingGroupBox.Controls.Add(Me.CancelAddBindingButton)
        Me.AddBindingGroupBox.Controls.Add(Me.BindingOkButton)
        Me.AddBindingGroupBox.Controls.Add(Me.lblRelease)
        Me.AddBindingGroupBox.Controls.Add(Me.lblCode)
        Me.AddBindingGroupBox.Controls.Add(Me.ReleaseTextBox)
        Me.AddBindingGroupBox.Controls.Add(Me.CodeTextBox)
        Me.AddBindingGroupBox.Controls.Add(Me.NodeLabel)
        Me.AddBindingGroupBox.Controls.Add(Me.PathRadioButton)
        Me.AddBindingGroupBox.Controls.Add(Me.NodeRadioButton)
        Me.AddBindingGroupBox.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.AddBindingGroupBox.Location = New System.Drawing.Point(0, 329)
        Me.AddBindingGroupBox.Name = "AddBindingGroupBox"
        Me.AddBindingGroupBox.Size = New System.Drawing.Size(880, 90)
        Me.AddBindingGroupBox.TabIndex = 4
        Me.AddBindingGroupBox.TabStop = False
        Me.AddBindingGroupBox.Text = "Add Binding"
        Me.AddBindingGroupBox.Visible = False
        '
        'PathLabel
        '
        Me.PathLabel.Location = New System.Drawing.Point(88, 39)
        Me.PathLabel.Name = "PathLabel"
        Me.PathLabel.Size = New System.Drawing.Size(784, 18)
        Me.PathLabel.TabIndex = 4
        '
        'CancelAddBindingButton
        '
        Me.CancelAddBindingButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelAddBindingButton.Location = New System.Drawing.Point(808, 57)
        Me.CancelAddBindingButton.Name = "CancelAddBindingButton"
        Me.CancelAddBindingButton.Size = New System.Drawing.Size(64, 28)
        Me.CancelAddBindingButton.TabIndex = 10
        Me.CancelAddBindingButton.Text = "Cancel"
        '
        'BindingOkButton
        '
        Me.BindingOkButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BindingOkButton.Location = New System.Drawing.Point(737, 57)
        Me.BindingOkButton.Name = "BindingOkButton"
        Me.BindingOkButton.Size = New System.Drawing.Size(64, 28)
        Me.BindingOkButton.TabIndex = 9
        Me.BindingOkButton.Text = "OK"
        '
        'lblRelease
        '
        Me.lblRelease.Location = New System.Drawing.Point(216, 63)
        Me.lblRelease.Name = "lblRelease"
        Me.lblRelease.Size = New System.Drawing.Size(72, 16)
        Me.lblRelease.TabIndex = 7
        Me.lblRelease.Text = "Release"
        Me.lblRelease.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCode
        '
        Me.lblCode.Location = New System.Drawing.Point(10, 63)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.Size = New System.Drawing.Size(88, 16)
        Me.lblCode.TabIndex = 5
        Me.lblCode.Text = "Code"
        Me.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ReleaseTextBox
        '
        Me.ReleaseTextBox.Location = New System.Drawing.Point(296, 63)
        Me.ReleaseTextBox.Name = "ReleaseTextBox"
        Me.ReleaseTextBox.Size = New System.Drawing.Size(100, 21)
        Me.ReleaseTextBox.TabIndex = 8
        '
        'CodeTextBox
        '
        Me.CodeTextBox.Location = New System.Drawing.Point(104, 63)
        Me.CodeTextBox.Name = "CodeTextBox"
        Me.CodeTextBox.Size = New System.Drawing.Size(100, 21)
        Me.CodeTextBox.TabIndex = 6
        '
        'NodeLabel
        '
        Me.NodeLabel.Location = New System.Drawing.Point(88, 19)
        Me.NodeLabel.Name = "NodeLabel"
        Me.NodeLabel.Size = New System.Drawing.Size(784, 18)
        Me.NodeLabel.TabIndex = 2
        '
        'PathRadioButton
        '
        Me.PathRadioButton.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.PathRadioButton.Checked = True
        Me.PathRadioButton.Location = New System.Drawing.Point(8, 38)
        Me.PathRadioButton.Name = "PathRadioButton"
        Me.PathRadioButton.Size = New System.Drawing.Size(74, 20)
        Me.PathRadioButton.TabIndex = 3
        Me.PathRadioButton.TabStop = True
        Me.PathRadioButton.Text = "Path"
        Me.PathRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'NodeRadioButton
        '
        Me.NodeRadioButton.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.NodeRadioButton.Location = New System.Drawing.Point(8, 18)
        Me.NodeRadioButton.Name = "NodeRadioButton"
        Me.NodeRadioButton.Size = New System.Drawing.Size(74, 18)
        Me.NodeRadioButton.TabIndex = 1
        Me.NodeRadioButton.Text = "Node"
        Me.NodeRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'AddBindingCriteriaGroupBox
        '
        Me.AddBindingCriteriaGroupBox.Controls.Add(Me.CriteriaPathRadioButton)
        Me.AddBindingCriteriaGroupBox.Controls.Add(Me.CriteriaNodeRadioButton)
        Me.AddBindingCriteriaGroupBox.Controls.Add(Me.CriteriaTermLabel)
        Me.AddBindingCriteriaGroupBox.Controls.Add(Me.OperatorComboBox)
        Me.AddBindingCriteriaGroupBox.Controls.Add(Me.BindingCriteriaCancelButton)
        Me.AddBindingCriteriaGroupBox.Controls.Add(Me.CriteriaOkButton)
        Me.AddBindingCriteriaGroupBox.Controls.Add(Me.CriteriaValueTextBox)
        Me.AddBindingCriteriaGroupBox.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.AddBindingCriteriaGroupBox.Location = New System.Drawing.Point(0, 257)
        Me.AddBindingCriteriaGroupBox.Name = "AddBindingCriteriaGroupBox"
        Me.AddBindingCriteriaGroupBox.Size = New System.Drawing.Size(880, 72)
        Me.AddBindingCriteriaGroupBox.TabIndex = 3
        Me.AddBindingCriteriaGroupBox.TabStop = False
        Me.AddBindingCriteriaGroupBox.Text = "Add Binding Criteria"
        Me.AddBindingCriteriaGroupBox.Visible = False
        '
        'CriteriaPathRadioButton
        '
        Me.CriteriaPathRadioButton.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CriteriaPathRadioButton.Location = New System.Drawing.Point(8, 44)
        Me.CriteriaPathRadioButton.Name = "CriteriaPathRadioButton"
        Me.CriteriaPathRadioButton.Size = New System.Drawing.Size(74, 22)
        Me.CriteriaPathRadioButton.TabIndex = 2
        Me.CriteriaPathRadioButton.Text = "Path"
        Me.CriteriaPathRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'CriteriaNodeRadioButton
        '
        Me.CriteriaNodeRadioButton.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CriteriaNodeRadioButton.Checked = True
        Me.CriteriaNodeRadioButton.Location = New System.Drawing.Point(8, 22)
        Me.CriteriaNodeRadioButton.Name = "CriteriaNodeRadioButton"
        Me.CriteriaNodeRadioButton.Size = New System.Drawing.Size(74, 22)
        Me.CriteriaNodeRadioButton.TabIndex = 1
        Me.CriteriaNodeRadioButton.TabStop = True
        Me.CriteriaNodeRadioButton.Text = "Node"
        Me.CriteriaNodeRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'CriteriaTermLabel
        '
        Me.CriteriaTermLabel.BackColor = System.Drawing.Color.White
        Me.CriteriaTermLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.CriteriaTermLabel.Location = New System.Drawing.Point(91, 20)
        Me.CriteriaTermLabel.Name = "CriteriaTermLabel"
        Me.CriteriaTermLabel.Size = New System.Drawing.Size(381, 40)
        Me.CriteriaTermLabel.TabIndex = 3
        '
        'OperatorComboBox
        '
        Me.OperatorComboBox.Items.AddRange(New Object() {"=", "<", "<=", ">", ">=", "!="})
        Me.OperatorComboBox.Location = New System.Drawing.Point(488, 20)
        Me.OperatorComboBox.Name = "OperatorComboBox"
        Me.OperatorComboBox.Size = New System.Drawing.Size(64, 21)
        Me.OperatorComboBox.TabIndex = 4
        Me.OperatorComboBox.Text = "="
        '
        'BindingCriteriaCancelButton
        '
        Me.BindingCriteriaCancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BindingCriteriaCancelButton.Location = New System.Drawing.Point(808, 42)
        Me.BindingCriteriaCancelButton.Name = "BindingCriteriaCancelButton"
        Me.BindingCriteriaCancelButton.Size = New System.Drawing.Size(64, 28)
        Me.BindingCriteriaCancelButton.TabIndex = 7
        Me.BindingCriteriaCancelButton.Text = "Cancel"
        '
        'CriteriaOkButton
        '
        Me.CriteriaOkButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CriteriaOkButton.Location = New System.Drawing.Point(737, 42)
        Me.CriteriaOkButton.Name = "CriteriaOkButton"
        Me.CriteriaOkButton.Size = New System.Drawing.Size(64, 28)
        Me.CriteriaOkButton.TabIndex = 6
        Me.CriteriaOkButton.Text = "OK"
        '
        'CriteriaValueTextBox
        '
        Me.CriteriaValueTextBox.Location = New System.Drawing.Point(568, 20)
        Me.CriteriaValueTextBox.Name = "CriteriaValueTextBox"
        Me.CriteriaValueTextBox.Size = New System.Drawing.Size(200, 21)
        Me.CriteriaValueTextBox.TabIndex = 5
        Me.CriteriaValueTextBox.Visible = False
        '
        'BindingToolTip
        '
        Me.BindingToolTip.Active = False
        Me.BindingToolTip.AutoPopDelay = 5000
        Me.BindingToolTip.InitialDelay = 1000
        Me.BindingToolTip.IsBalloon = True
        Me.BindingToolTip.ReshowDelay = 100
        Me.BindingToolTip.ShowAlways = True
        Me.BindingToolTip.ToolTipTitle = "Archetype path"
        '
        'TopPanel
        '
        Me.TopPanel.Controls.Add(Me.AddTerminologyButton)
        Me.TopPanel.Controls.Add(Me.TerminologyComboBox)
        Me.TopPanel.Controls.Add(Me.TerminologyLabel)
        Me.TopPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.TopPanel.Location = New System.Drawing.Point(4, 0)
        Me.TopPanel.Name = "TopPanel"
        Me.TopPanel.Size = New System.Drawing.Size(880, 40)
        Me.TopPanel.TabIndex = 14
        '
        'AddTerminologyButton
        '
        Me.AddTerminologyButton.Image = CType(resources.GetObject("AddTerminologyButton.Image"), System.Drawing.Image)
        Me.AddTerminologyButton.Location = New System.Drawing.Point(695, 7)
        Me.AddTerminologyButton.Name = "AddTerminologyButton"
        Me.AddTerminologyButton.Size = New System.Drawing.Size(26, 27)
        Me.AddTerminologyButton.TabIndex = 3
        '
        'TerminologyComboBox
        '
        Me.TerminologyComboBox.DisplayMember = "Description"
        Me.TerminologyComboBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TerminologyComboBox.Location = New System.Drawing.Point(256, 5)
        Me.TerminologyComboBox.Name = "TerminologyComboBox"
        Me.TerminologyComboBox.Size = New System.Drawing.Size(433, 23)
        Me.TerminologyComboBox.TabIndex = 2
        Me.TerminologyComboBox.ValueMember = "Terminology"
        '
        'TerminologyLabel
        '
        Me.TerminologyLabel.Location = New System.Drawing.Point(8, 3)
        Me.TerminologyLabel.Name = "TerminologyLabel"
        Me.TerminologyLabel.Size = New System.Drawing.Size(240, 29)
        Me.TerminologyLabel.TabIndex = 1
        Me.TerminologyLabel.Text = "Terminology"
        Me.TerminologyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'PanelBindings
        '
        Me.PanelBindings.Controls.Add(Me.gbCriteria)
        Me.PanelBindings.Controls.Add(Me.BindingGroupBox)
        Me.PanelBindings.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelBindings.Location = New System.Drawing.Point(0, 161)
        Me.PanelBindings.Name = "PanelBindings"
        Me.PanelBindings.Size = New System.Drawing.Size(880, 96)
        Me.PanelBindings.TabIndex = 2
        '
        'gbCriteria
        '
        Me.gbCriteria.Controls.Add(Me.BindingCriteriaListBox)
        Me.gbCriteria.Controls.Add(Me.AddCriteriaButton)
        Me.gbCriteria.Controls.Add(Me.DeleteCriteriaButton)
        Me.gbCriteria.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbCriteria.Location = New System.Drawing.Point(224, 0)
        Me.gbCriteria.Name = "gbCriteria"
        Me.gbCriteria.Size = New System.Drawing.Size(656, 96)
        Me.gbCriteria.TabIndex = 2
        Me.gbCriteria.TabStop = False
        Me.gbCriteria.Text = "Criteria"
        Me.gbCriteria.Visible = False
        '
        'tabBindings
        '
        Me.tabBindings.BackColor = System.Drawing.Color.OldLace
        Me.tabBindings.BoldSelectedPage = True
        Me.tabBindings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabBindings.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.ShowAlways
        Me.tabBindings.Location = New System.Drawing.Point(4, 40)
        Me.tabBindings.Name = "tabBindings"
        Me.tabBindings.PositionTop = True
        Me.tabBindings.SelectedIndex = 0
        Me.tabBindings.SelectedTab = Me.tpSimple
        Me.tabBindings.Size = New System.Drawing.Size(880, 444)
        Me.tabBindings.TabIndex = 16
        Me.tabBindings.TabPages.AddRange(New Crownwood.Magic.Controls.TabPage() {Me.tpSimple, Me.tpComplex})
        '
        'tpSimple
        '
        Me.tpSimple.Controls.Add(Me.dgTermBindings)
        Me.tpSimple.Location = New System.Drawing.Point(0, 0)
        Me.tpSimple.Name = "tpSimple"
        Me.tpSimple.Size = New System.Drawing.Size(880, 419)
        Me.tpSimple.TabIndex = 0
        Me.tpSimple.Title = "Node"
        '
        'dgTermBindings
        '
        Me.dgTermBindings.AllowUserToAddRows = False
        Me.dgTermBindings.AllowUserToOrderColumns = True
        Me.dgTermBindings.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.TerminologyColumn, Me.NodeColumn, Me.CodeColumn, Me.ReleaseColumn})
        Me.dgTermBindings.Dock = System.Windows.Forms.DockStyle.Left
        Me.dgTermBindings.Location = New System.Drawing.Point(0, 0)
        Me.dgTermBindings.Name = "dgTermBindings"
        Me.dgTermBindings.RowTemplate.Height = 24
        Me.dgTermBindings.Size = New System.Drawing.Size(657, 419)
        Me.dgTermBindings.TabIndex = 0
        '
        'TerminologyColumn
        '
        Me.TerminologyColumn.DataPropertyName = "Terminology"
        Me.TerminologyColumn.HeaderText = "Terminology"
        Me.TerminologyColumn.Name = "TerminologyColumn"
        Me.TerminologyColumn.Visible = False
        '
        'NodeColumn
        '
        Me.NodeColumn.DataPropertyName = "Path"
        Me.NodeColumn.HeaderText = "Node"
        Me.NodeColumn.MinimumWidth = 50
        Me.NodeColumn.Name = "NodeColumn"
        Me.NodeColumn.Width = 300
        '
        'CodeColumn
        '
        Me.CodeColumn.DataPropertyName = "Code"
        Me.CodeColumn.HeaderText = "Code"
        Me.CodeColumn.MinimumWidth = 50
        Me.CodeColumn.Name = "CodeColumn"
        Me.CodeColumn.Width = 150
        '
        'ReleaseColumn
        '
        Me.ReleaseColumn.DataPropertyName = "Release"
        Me.ReleaseColumn.HeaderText = "Release"
        Me.ReleaseColumn.Name = "ReleaseColumn"
        '
        'tpComplex
        '
        Me.tpComplex.BackColor = System.Drawing.Color.FloralWhite
        Me.tpComplex.Controls.Add(Me.PathsTreeView)
        Me.tpComplex.Controls.Add(Me.PanelBindings)
        Me.tpComplex.Controls.Add(Me.AddBindingCriteriaGroupBox)
        Me.tpComplex.Controls.Add(Me.AddBindingGroupBox)
        Me.tpComplex.Location = New System.Drawing.Point(0, 0)
        Me.tpComplex.Name = "tpComplex"
        Me.tpComplex.Selected = False
        Me.tpComplex.Size = New System.Drawing.Size(880, 419)
        Me.tpComplex.TabIndex = 1
        Me.tpComplex.Title = "Complex"
        '
        'TermBindingPanel
        '
        Me.BackColor = System.Drawing.Color.FloralWhite
        Me.Controls.Add(Me.tabBindings)
        Me.Controls.Add(Me.TopPanel)
        Me.Name = "TermBindingPanel"
        Me.Padding = New System.Windows.Forms.Padding(4, 0, 4, 4)
        Me.Size = New System.Drawing.Size(888, 488)
        Me.ContextMenuTermBinding.ResumeLayout(False)
        Me.BindingGroupBox.ResumeLayout(False)
        Me.AddBindingGroupBox.ResumeLayout(False)
        Me.AddBindingGroupBox.PerformLayout()
        Me.AddBindingCriteriaGroupBox.ResumeLayout(False)
        Me.AddBindingCriteriaGroupBox.PerformLayout()
        Me.TopPanel.ResumeLayout(False)
        Me.PanelBindings.ResumeLayout(False)
        Me.gbCriteria.ResumeLayout(False)
        Me.tpSimple.ResumeLayout(False)
        CType(Me.dgTermBindings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpComplex.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private WithEvents mTermBindingView As DataView
    Private WithEvents mTermBindingCriteriaView As DataView
    Private mFileManager As FileManagerLocal
    Private mCurrentTermNode As TermNode
    Private mCurrentBindingCriteria As BindingCriteria
    Private mCriteriaMode As Boolean
    Private WithEvents mCriteriaElementView As ElementViewControl
    Private mCriteriaNewRow As DataRow

    Private Sub BindingTerminologyComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TerminologyComboBox.SelectedIndexChanged
        If Not TerminologyComboBox.SelectedValue Is Nothing Then
            ShowNodesImage(PathsTreeView.Nodes)
            SetTermBindingFilter()  ' for the tree view
            SetDvTermBindingsFilter() ' for the node only view
            dgTermBindings.AllowUserToAddRows = True
        Else
            dgTermBindings.AllowUserToAddRows = False
        End If
    End Sub

    Private Function GetTextFromPath(ByVal path As String, ByVal insideSqBrackets As Boolean) As String
        Dim splitNode As String() = path.Split("[]".ToCharArray)

        If splitNode.Length = 3 Then
            If insideSqBrackets Then
                Return splitNode(1)
            Else
                Return splitNode(0)
            End If
        Else
            Return path
        End If
    End Function

    Public Sub PopulatePathTree()
        PathsTreeView.SuspendLayout()
        PathsTreeView.Nodes.Clear()

        Dim languageCode As String = mFileManager.OntologyManager.LanguageCode

        ' get the paths with the NodeId labels
        Dim physicalPaths As String() = mFileManager.Archetype.Paths(languageCode, mFileManager.ParserSynchronised, False)

        'Temporary fix for paths notation which included value[unknown_1] and such
        'Match number, bracket at end of string - but no underscore
        'Dim regularExp As New System.Text.RegularExpressions.Regex("(?<!_)\d\]\Z")
        Dim regularExp As New System.Text.RegularExpressions.Regex(".+\d\]\Z")

        For i As Integer = 0 To physicalPaths.Length - 1
            'ignore value paths and empty attributes
            If regularExp.Match(physicalPaths(i)).Success Then
                Dim z As String() = physicalPaths(i).Split("/"c)
                Dim nodes As TreeNodeCollection = PathsTreeView.Nodes

                'Ignore the first string as always ""
                Debug.Assert(z(0) = "", "String is not empty as expected")

                For j As Integer = 1 To z.Length - 1
                    Dim nodeFound As Boolean = False
                    Dim node As TermNode = Nothing
                    Dim splitPathSegment As String() = z(j).Split("[]".ToCharArray())

                    If splitPathSegment.Length > 1 Then
                        Dim nId As String = splitPathSegment(1)

                        ' does this node exist elsewhere?
                        For Each n As TermNode In nodes
                            If n.NodeId = nId Then
                                node = n
                                nodeFound = True
                                Exit For
                            End If
                        Next
                    End If

                    If Not nodeFound Then
                        node = New TermNode(z(j), mFileManager)

                        If z(j).StartsWith("data") Then
                            nodes.Insert(0, node)
                        ElseIf z(j).StartsWith("events") Then
                            If z.Length > 3 Then
                                ' the data node
                                nodes.Insert(0, node)
                            Else
                                nodes.Add(node)
                            End If
                        Else
                            nodes.Add(node)
                        End If
                    End If

                    nodes = node.Nodes
                Next
            End If
        Next

        ShowNodesImage(PathsTreeView.Nodes)

        If PathsTreeView.GetNodeCount(False) > 0 Then
            PathsTreeView.ExpandAll()
            PathsTreeView.SelectedNode = PathsTreeView.Nodes(0)
            AddBindingButton.Enabled = True
            DeleteBindingButton.Enabled = True
        Else
            AddBindingButton.Enabled = False
            DeleteBindingButton.Enabled = False
        End If

        SetDvTermBindingsFilter()
        PathsTreeView.ResumeLayout()
    End Sub

    Private Sub ShowNodesImage(ByVal aNodes As TreeNodeCollection)
        If imageTermBindingDataView Is Nothing Then
            imageTermBindingDataView = New DataView(mFileManager.OntologyManager.TermBindingsTable)
        End If

        For Each node As TermNode In aNodes
            node.ImageIndex = 0
            node.SelectedImageIndex = 0

            If Not TerminologyComboBox.SelectedValue Is Nothing Then
                SetTermBindingFilter(imageTermBindingDataView, CStr(TerminologyComboBox.SelectedValue), node.NodeId, node.PhysicalPath)

                If imageTermBindingDataView.Count > 0 Then
                    node.ImageIndex = 1
                    node.SelectedImageIndex = 1
                End If
            End If

            If Not node.Parent Is Nothing AndAlso node.Parent.ImageIndex <= 0 Then
                node.Parent.ImageIndex = 4
                node.Parent.SelectedImageIndex = 4
            End If

            ShowNodesImage(node.Nodes)
        Next
    End Sub

    Private imageTermBindingDataView As DataView
    Private dvTermBindings As Data.DataView
    Private dvTermDefinitions As Data.DataView

    Private Sub SetDvTermBindingsFilter()
        If TerminologyComboBox.SelectedIndex > -1 Then
            dvTermBindings.RowFilter = "Terminology = '" & CStr(TerminologyComboBox.SelectedValue) & "' AND Path LIKE 'at%'"
            dvTermBindings.Table.Columns(0).DefaultValue = CStr(TerminologyComboBox.SelectedValue)
        End If

        dgTermBindings.Enabled = mFileManager.OntologyManager.TerminologiesTable.Rows.Count > 0
    End Sub

    Private Sub SetDvTermDefinitionsFilter()
        dvTermDefinitions.RowFilter = "id = '" & mFileManager.OntologyManager.LanguageCode & "'"
    End Sub

    Private Sub TermBindingPanel_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not DesignMode Then
            TranslateGUI()

            'set up the text display for the terms in the current language
            dvTermDefinitions = New Data.DataView(mFileManager.OntologyManager.TermDefinitionTable)
            SetDvTermDefinitionsFilter()
            NodeColumn.DataSource = dvTermDefinitions
            NodeColumn.DisplayMember = "Text"
            NodeColumn.ValueMember = "Code"

            'Set up the view of the node (only) term bindings for that terminology
            dvTermBindings = New Data.DataView(mFileManager.OntologyManager.TermBindingsTable)

            'Now set the terminology source which calls SetDvTermBindingsFilter if there are any terminologies
            TerminologyComboBox.DataSource = mFileManager.OntologyManager.TerminologiesTable

            dgTermBindings.DataSource = dvTermBindings

            BindingToolTip.SetToolTip(AddBindingButton, Filemanager.GetOpenEhrTerm(99, "Add binding"))
            BindingToolTip.SetToolTip(DeleteBindingButton, Filemanager.GetOpenEhrTerm(152, "Remove"))
        End If
    End Sub

    Public Sub Reset()
        PathsTreeView.Nodes.Clear()
    End Sub

    Private Sub NodeScopeCheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PathRadioButton.CheckedChanged, NodeRadioButton.CheckedChanged
        CodeTextBox.Focus()
    End Sub

    Private Sub PathsTreeView_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles PathsTreeView.AfterSelect
        Debug.Assert(TypeOf PathsTreeView.SelectedNode Is TermNode)

        If Not CriteriaMode Then
            SetTermBindingFilter()

            If AddBindingGroupBox.Visible Then
                PopulateNewBindingPath()
            End If
        Else
            PopulateCriteriaPath()
            ShowCriteriaElementView()
        End If

        Dim selectedNode As TermNode = CType(PathsTreeView.SelectedNode, TermNode)
        BindingToolTip.SetToolTip(PathsTreeView, selectedNode.PhysicalPath)
    End Sub

    Private Sub SetTermBindingFilter()
        Dim selectedNode As TermNode = CType(PathsTreeView.SelectedNode, TermNode)

        If Not selectedNode Is Nothing Then
            SetTermBindingFilter(mTermBindingView, CStr(TerminologyComboBox.SelectedValue), selectedNode.NodeId, selectedNode.PhysicalPath)
        End If
    End Sub

    Private Sub SetTermBindingFilter(ByVal TermBindingView As DataView, ByVal TerminologyId As String, ByVal NodeId As String, ByVal NodePath As String)
        TermBindingView.RowFilter = String.Format("Terminology = '{0}' AND (Path = '{1}' OR Path = '{2}')", TerminologyId, NodeId, NodePath)
    End Sub

    Private Sub AddBindingButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddBindingButton.Click
        If TerminologyComboBox.SelectedValue Is Nothing Then
            MsgBox("Select a terminology")
        Else
            AddBindingGroupBox.Show()
            PanelBindings.Enabled = False
            CodeTextBox.Text = ""
            ReleaseTextBox.Text = ""
            PopulateNewBindingPath()
        End If
    End Sub

    Private Sub PopulateNewBindingPath()
        Dim node As TermNode = TryCast(PathsTreeView.SelectedNode, TermNode)

        If Not node Is Nothing Then
            NodeLabel.Text = node.NodeId
            PathLabel.Text = node.PhysicalPath
        End If
    End Sub

    Private Sub AddPathToBindings(ByVal path As String, ByVal code As String)
        Dim yes As Boolean = True

        If mFileManager.OntologyManager.Ontology.HasTermBinding(CStr(TerminologyComboBox.SelectedValue), path) Then
            ' already has this path so add criteria
            yes = MessageBox.Show("Term has binding, add anyway?", AE_Constants.Instance.MessageBoxCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes
        End If

        If yes Then
            Dim key(2) As Object
            key(0) = TerminologyComboBox.SelectedValue
            key(1) = path
            key(2) = code
            Dim row As DataRow = mFileManager.OntologyManager.TermBindingsTable.Rows.Find(key)

            If row Is Nothing Then
                mCriteriaNewRow = mFileManager.OntologyManager.TermBindingsTable.NewRow
                mCriteriaNewRow(0) = TerminologyComboBox.SelectedValue
                mCriteriaNewRow(1) = path
                mCriteriaNewRow("code") = code
                mFileManager.OntologyManager.TermBindingsTable.Rows.Add(mCriteriaNewRow)
            Else
                row.BeginEdit()
                row(0) = TerminologyComboBox.SelectedValue
                row(1) = path
                row("code") = code
                row.EndEdit()
            End If

            mFileManager.FileEdited = True

            Debug.Assert(BindingList.Items.Count > 0, "BindingList not updated")

            If BindingList.Items.Count > 0 Then
                BindingList.Items(BindingList.Items.Count - 1).Selected = True
            End If
        End If
    End Sub

    Private Sub RemovePathFromBindings(ByVal aPath As String, ByVal aCode As String)
        Dim SelectedRows As DataRow()

        SelectedRows = mFileManager.OntologyManager.TermBindingsTable.Select _
            ("[Terminology]='" & CStr(TerminologyComboBox.SelectedValue) & "'" _
            & " AND [Path]='" & aPath & "'" _
            & " AND [Code]='" & aCode & "'")

        For Each d_row As DataRow In SelectedRows
            d_row.Delete()
        Next

        mFileManager.OntologyManager.TermBindingsTable.AcceptChanges()
        mFileManager.FileEdited = True

        If BindingList.Items.Count > 0 Then
            BindingList.Items(BindingList.Items.Count - 1).Selected = True
        End If
    End Sub

    Private Sub AddTerminologyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddTerminologyButton.Click
        If Main.Instance.AddTerminology() Then
            If mFileManager.OntologyManager.TerminologiesTable.Rows.Count = 1 Then
                SetTermBindingFilter()  ' for the tree view
                SetDvTermBindingsFilter() ' for the node only view
                dgTermBindings.Enabled = True
                dgTermBindings.AllowUserToAddRows = True
            Else
                TerminologyComboBox.SelectedIndex = TerminologyComboBox.Items.Count - 1
            End If

            mFileManager.FileEdited = True
        End If
    End Sub

    Private Sub BindingList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BindingList.SelectedIndexChanged
        Debug.Assert(BindingList.SelectedIndices.Count <= 1)

        If BindingList.SelectedIndices.Count = 1 Then
            Dim index As Integer = BindingList.SelectedIndices.Item(0)
            gbCriteria.Show()

            If index >= 0 Then
                Dim bindingRowView As DataRowView = mTermBindingView.Item(index)

                ' populate binding criteria
                Dim path As String = CStr(bindingRowView.Item("Path"))
                Dim code As String = CStr(bindingRowView.Item("Code"))
                SetBindingCriteriaFilter(path, code)

                ' set binding list tooltip
                BindingToolTip.SetToolTip(BindingList, path)
            Else
                Debug.Assert(False)
            End If
        Else
            If BindingList.SelectedIndices.Count = 0 Then
                gbCriteria.Hide()
            End If

            BindingToolTip.SetToolTip(BindingList, "")
        End If
    End Sub

    Private Sub BindingOkButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BindingOkButton.Click
        Debug.Assert(NodeLabel.Text <> "", "Node not set")
        Debug.Assert(PathLabel.Text <> "", "Path not set")

        If CodeTextBox.Text = "" Then
            MessageBox.Show("Binding code required", AE_Constants.Instance.MessageBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf TerminologyComboBox.SelectedValue Is Nothing Then
            MessageBox.Show("Binding terminology required", AE_Constants.Instance.MessageBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            Dim path As String

            If NodeRadioButton.Checked Then
                path = "//" & NodeLabel.Text
            Else
                path = PathLabel.Text
            End If

            If mFileManager.OntologyManager.HasTermBinding(CStr(TerminologyComboBox.SelectedValue), CodeTextBox.Text, path) Then
                If MessageBox.Show(AE_Constants.Instance.Must_add_criteria, AE_Constants.Instance.MessageBoxCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.OK Then
                    AddBindingGroupBox.Hide()
                    AddCriteriaButton_Click(sender, e)
                    mCriteriaMode = True
                Else
                    PanelBindings.Enabled = True
                    AddBindingGroupBox.Hide()
                End If
            Else
                Try
                    AddPathToBindings(path, CodeTextBox.Text)
                    ShowNodesImage(PathsTreeView.Nodes)
                    PanelBindings.Enabled = True
                    AddBindingGroupBox.Hide()
                Catch ex As Exception
                    MessageBox.Show(AE_Constants.Instance.Duplicate_name, AE_Constants.Instance.MessageBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End If
    End Sub

    Private Sub mTermBindingView_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles mTermBindingView.ListChanged
        PopulateBindingList()
    End Sub

    Private Sub PopulateBindingList()
        BindingList.Items.Clear()

        For Each bindingRow As DataRowView In mTermBindingView
            Dim imageIndex As Integer

            If bindingRow.Item("Path").ToString.IndexOf("/") > 0 Then
                ' Path
                imageIndex = 6
            Else
                ' Node
                imageIndex = 5
            End If

            BindingList.Items.Add(CStr(bindingRow.Item("code")), imageIndex)
        Next

        If BindingList.Items.Count > 0 Then
            BindingList.Items(0).Selected = True
        Else
            BindingToolTip.SetToolTip(BindingList, "")
            ' hide the criteria as there are no nodes
            gbCriteria.Hide()
            SetBindingCriteriaFilter("", "")
        End If
    End Sub

    Private Sub SetBindingCriteriaFilter(ByVal Path As String, ByVal Code As String)
        ' Called during load before terminologyComboBox has a selected value so...
        If Not TerminologyComboBox.SelectedValue Is Nothing Then
            mTermBindingCriteriaView.RowFilter = String.Format("Terminology = '{0}' AND Path = '{1}' AND Code = '{2}'", CStr(TerminologyComboBox.SelectedValue), Path, Code)
        End If
    End Sub

    Private Sub CancelAddBindingButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelAddBindingButton.Click
        PanelBindings.Enabled = True
        AddBindingGroupBox.Hide()
    End Sub

    Private Sub DeleteBindingButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteBindingButton.Click
        Debug.Assert(BindingList.SelectedIndices.Count <= 1)

        If BindingList.SelectedIndices.Count = 1 Then
            Dim index As Integer = BindingList.SelectedIndices.Item(0)

            If index >= 0 Then
                Dim a_code As String = CStr(mTermBindingView.Item(index).Item("Code"))
                Dim a_path As String = CStr(mTermBindingView.Item(index).Item("Path"))

                If mFileManager.OntologyManager.HasTermBinding(CStr(TerminologyComboBox.SelectedValue), a_code, a_path) Then
                    Try
                        RemovePathFromBindings(a_path, a_code)
                    Catch ex As Exception
                        Debug.Assert(False, ex.ToString)
                        MessageBox.Show(AE_Constants.Instance.ErrorSaving, AE_Constants.Instance.MessageBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End If

            ShowNodesImage(PathsTreeView.Nodes)
        End If
    End Sub

    Private Sub AddCriteriaButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddCriteriaButton.Click
        If BindingList.SelectedIndices.Count > 0 Then
            'Preconditions
            Debug.Assert(Not PathsTreeView.SelectedNode Is Nothing)
            Debug.Assert(TypeOf PathsTreeView.SelectedNode Is TermNode)

            'Empty the criteria text box
            CriteriaValueTextBox.Text = ""

            'Make the right panel visible
            AddBindingCriteriaGroupBox.Visible = True
            PanelBindings.Enabled = False

            mCurrentTermNode = CType(PathsTreeView.SelectedNode, TermNode)
            mCurrentBindingCriteria = New BindingCriteria(mFileManager)
            PopulateCriteriaPath()
            mCriteriaMode = True
            ShowCriteriaElementView()
        Else
            MsgBox("Select a term binding")
        End If
    End Sub

    Private ReadOnly Property CriteriaMode() As Boolean
        Get
            Return AddBindingCriteriaGroupBox.Visible
        End Get
    End Property

    Private Sub BindingCriteriaCancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BindingCriteriaCancelButton.Click
        AddBindingCriteriaGroupBox.Visible = False
        PanelBindings.Enabled = True
        mCriteriaMode = False
        PathsTreeView.SelectedNode = mCurrentTermNode
    End Sub

    Private Sub CriteriaOkButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CriteriaOkButton.Click
        mCurrentBindingCriteria.[Operator] = CStr(OperatorComboBox.Text)
        mCurrentBindingCriteria.ValueOperand = CStr(CriteriaValueTextBox.Text)

        If mCriteriaMode Then
            If BindingList.SelectedItems.Count = 0 Or BindingList.SelectedIndices.Count = 0 Then
                MessageBox.Show("Binding code required", AE_Constants.Instance.MessageBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                Dim bindingRowView As DataRowView = mTermBindingView.Item(BindingList.SelectedIndices.Item(0))

                ' populate binding criteria
                Dim path As String = CStr(bindingRowView.Item("Path"))
                Dim terminology As String = CStr(bindingRowView.Item("Terminology"))
                Dim code As String = CStr(bindingRowView.Item("Code"))

                Dim key(3) As Object
                key(0) = terminology
                key(1) = path
                key(2) = code
                key(3) = mCurrentBindingCriteria.ToPhysicalCriteria
                Dim row As DataRow = mFileManager.OntologyManager.TermBindingCriteriaTable.Rows.Find(key)

                If row Is Nothing Then
                    row = mFileManager.OntologyManager.TermBindingCriteriaTable.NewRow
                    row(0) = terminology
                    row(1) = path
                    row("code") = code
                    row("Criteria") = mCurrentBindingCriteria.ToPhysicalCriteria
                    mFileManager.OntologyManager.TermBindingCriteriaTable.Rows.Add(row)
                Else
                    row.BeginEdit()
                    row(0) = terminology
                    row(1) = path
                    row("code") = code
                    row("Criteria") = mCurrentBindingCriteria.ToPhysicalCriteria
                    row.EndEdit()
                End If

                mFileManager.FileEdited = True
                AddPathToBindings(CodeTextBox.Text & "{" & mCurrentBindingCriteria.ToPhysicalCriteria & "}", BindingList.SelectedItems(0).Text)
                ShowNodesImage(PathsTreeView.Nodes)
            End If
        End If

        BindingCriteriaCancelButton_Click(sender, e)
    End Sub

    Private Sub PopulateCriteriaPath()
        Dim selectedNode As TermNode = CType(PathsTreeView.SelectedNode, TermNode)

        If CriteriaPathRadioButton.Checked Then
            mCurrentBindingCriteria.NodeOperand = selectedNode.PhysicalPath
        Else
            Debug.Assert(CriteriaNodeRadioButton.Checked, "No criteria scope option selected")

            mCurrentBindingCriteria.NodeOperand = selectedNode.NodeId
        End If

        CriteriaTermLabel.Text = mCurrentBindingCriteria.NodeText
    End Sub

    Private Sub ShowCriteriaElementView()
        Dim selectedNode As TermNode = CType(PathsTreeView.SelectedNode, TermNode)
        Dim termConstraint As Constraint = selectedNode.Constraint

        If Not mCriteriaElementView Is Nothing Then
            AddBindingCriteriaGroupBox.Controls.Remove(mCriteriaElementView)
        End If

        If Not termConstraint Is Nothing Then
            mCriteriaElementView = ArchetypeView.ConstraintView(termConstraint, mFileManager)
            mCriteriaElementView.Location = CriteriaValueTextBox.Location
            AddBindingCriteriaGroupBox.Controls.Add(mCriteriaElementView)
        End If
    End Sub

    Private Sub CriteriaNodeRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CriteriaNodeRadioButton.CheckedChanged, CriteriaPathRadioButton.CheckedChanged
        If Not PathsTreeView.SelectedNode Is Nothing Then
            PopulateCriteriaPath()
        End If
    End Sub

    Public Sub Translate()
        TranslateNodes(PathsTreeView.Nodes)
        SetDvTermDefinitionsFilter()
    End Sub

    Private Sub TranslateNodes(ByVal nodes As TreeNodeCollection)
        For Each t As TermNode In nodes
            t.Translate()

            If t.GetNodeCount(False) > 0 Then
                TranslateNodes(t.Nodes)
            End If
        Next
    End Sub

    Private Sub TranslateGUI()
        TerminologyLabel.Text = Filemanager.GetOpenEhrTerm(47, TerminologyLabel.Text)
        lblCode.Text = Filemanager.GetOpenEhrTerm(90, lblCode.Text)
        lblRelease.Text = Filemanager.GetOpenEhrTerm(97, lblRelease.Text)
        BindingGroupBox.Text = Filemanager.GetOpenEhrTerm(93, BindingGroupBox.Text)
        AddBindingGroupBox.Text = Filemanager.GetOpenEhrTerm(99, AddBindingGroupBox.Text)
        gbCriteria.Text = Filemanager.GetOpenEhrTerm(622, gbCriteria.Text)
        NodeRadioButton.Text = Filemanager.GetOpenEhrTerm(621, NodeRadioButton.Text)
        PathRadioButton.Text = Filemanager.GetOpenEhrTerm(96, PathRadioButton.Text)
        CriteriaNodeRadioButton.Text = NodeRadioButton.Text
        CriteriaPathRadioButton.Text = PathRadioButton.Text
        CancelAddBindingButton.Text = Filemanager.GetOpenEhrTerm(166, CancelAddBindingButton.Text)
        BindingCriteriaCancelButton.Text = CancelAddBindingButton.Text
        BindingOkButton.Text = Filemanager.GetOpenEhrTerm(165, BindingOkButton.Text)
        CriteriaOkButton.Text = BindingOkButton.Text
        BindingCodeListColumnHeader.Text = Filemanager.GetOpenEhrTerm(90, BindingCodeListColumnHeader.Text)
        cmBindingPastePath.Text = Filemanager.GetOpenEhrTerm(639, cmBindingPastePath.Text)
        tpSimple.Title = Filemanager.GetOpenEhrTerm(621, tpSimple.Title)
        tpComplex.Title = Filemanager.GetOpenEhrTerm(102, tpComplex.Title)
        NodeColumn.HeaderText = Filemanager.GetOpenEhrTerm(621, NodeColumn.HeaderText)
        CodeColumn.HeaderText = Filemanager.GetOpenEhrTerm(90, CodeColumn.HeaderText)
        ReleaseColumn.HeaderText = Filemanager.GetOpenEhrTerm(97, ReleaseColumn.HeaderText)
    End Sub

    Private Sub DeleteCriteriaButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteCriteriaButton.Click
        If BindingCriteriaListBox.SelectedIndex >= 0 Then
            mTermBindingCriteriaView.Item(BindingCriteriaListBox.SelectedIndex).Delete()
        Else
            MsgBox("Select a binding criteria to remove")
        End If
    End Sub

    Private Sub mTermBindingCriteriaView_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles mTermBindingCriteriaView.ListChanged
        PopulateBindingCriteriaList()
    End Sub

    Private Sub PopulateBindingCriteriaList()
        BindingCriteriaListBox.Items.Clear()

        For Each criteriaRow As DataRowView In mTermBindingCriteriaView
            Dim criteriaExpression As String = CStr(criteriaRow.Item("criteria"))
            Dim criteria As New BindingCriteria(criteriaExpression, mFileManager)
            BindingCriteriaListBox.Items.Add(criteria)
        Next

        BindingToolTip.SetToolTip(BindingCriteriaListBox, "")
    End Sub

    Private Sub CriteriaValue_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mCriteriaElementView.ValueChanged
        CriteriaValueTextBox.Text = CStr(mCriteriaElementView.Value)
    End Sub

    Class TermNode : Inherits TreeNode
        Private mFileManager As FileManagerLocal
        Private mPhysicalPathPart As String

        Public Sub New(ByVal physicalPathPart As String, ByVal fileManager As FileManagerLocal)
            MyBase.New("?")
            mPhysicalPathPart = physicalPathPart
            mFileManager = fileManager
            Debug.Assert(physicalPathPart.IndexOf("/") = -1, "Path is wrong:" & physicalPathPart)
            Dim i As Integer = physicalPathPart.IndexOf("[")

            If i > -1 Then
                mNodeId = physicalPathPart.Substring(i + 1, physicalPathPart.Length - i - 2)
            Else
                Debug.Assert(physicalPathPart = "context", String.Format("Possible error in path: {0}", physicalPathPart))
            End If

            Translate()
        End Sub

        Private mNodeId As String

        ReadOnly Property NodeId() As String
            Get
                Return mNodeId
            End Get
        End Property

        ReadOnly Property PhysicalPathPart() As String
            Get
                Return mPhysicalPathPart
            End Get
        End Property

        Shadows ReadOnly Property Parent() As TermNode
            Get
                Return TryCast(MyBase.Parent, TermNode)
            End Get
        End Property

        ReadOnly Property LogicalPath() As String
            Get
                Return FullPath
            End Get
        End Property

        Public Function PhysicalPath() As String
            If Parent Is Nothing Then
                Return "/" & PhysicalPathPart
            Else
                Return Parent.PhysicalPath & "/" & PhysicalPathPart
            End If
        End Function

        Public Sub Translate()
            Dim i As Integer = PhysicalPathPart.IndexOf("[")
            Dim txt As String

            If i > -1 Then
                txt = PhysicalPathPart.Substring(0, i)
            Else
                txt = PhysicalPathPart ' For context in Composition
            End If

            Select Case txt.ToLower(System.Globalization.CultureInfo.InvariantCulture)
                Case "history"
                    txt = mFileManager.OntologyManager.GetOpenEHRTerm(275, txt).ToLower(System.Globalization.CultureInfo.InvariantCulture)
                Case "data"
                    txt = mFileManager.OntologyManager.GetOpenEHRTerm(80, txt).ToLower(System.Globalization.CultureInfo.InvariantCulture)
                Case "protocol"
                    txt = mFileManager.OntologyManager.GetOpenEHRTerm(78, txt).ToLower(System.Globalization.CultureInfo.InvariantCulture)
                Case "state"
                    txt = mFileManager.OntologyManager.GetOpenEHRTerm(177, txt).ToLower(System.Globalization.CultureInfo.InvariantCulture)
                Case "context"
                    txt = mFileManager.OntologyManager.GetOpenEHRTerm(514, txt).ToLower(System.Globalization.CultureInfo.InvariantCulture)
                Case Else
                    txt = mFileManager.OntologyManager.GetTerm(NodeId).Text
            End Select

            Text = txt
        End Sub

        ReadOnly Property Constraint() As Constraint
            Get
                Dim result As Constraint = Nothing

                If mFileManager.Archetype.Definition.Type <> StructureType.Element Then
                    Dim rmClass As RmStructure = CType(mFileManager.Archetype.Definition, ArchetypeDefinition).GetChildByNodeId(mNodeId)

                    If TypeOf rmClass Is RmElement Then
                        result = CType(rmClass, RmElement).Constraint
                    End If
                End If

                Return result
            End Get
        End Property

        Public Function Copy() As TermNode
            Return New TermNode(mPhysicalPathPart, mFileManager)
        End Function
    End Class

    Public Class BindingCriteria
        Public NodeOperand As String
        Public [Operator] As String = "="
        Public ValueOperand As String
        Private mFilemanager As FileManagerLocal

        Public ReadOnly Property NodeText() As String
            Get
                If NodeOperand.IndexOf("/") >= 0 Then
                    Dim path As String = NodeOperand.TrimEnd("/"c)
                    Dim pathParts() As String = path.Split("/"c)
                    path = ""

                    For Each part As String In pathParts
                        Dim i As Integer = part.IndexOf("[") + 1

                        If i > 0 Then
                            Dim j As Integer = part.IndexOf("]")
                            Debug.Assert(j > 0)
                            Dim partTerm As String = part.Substring(i, j - i)
                            Dim term As RmTerm = mFilemanager.OntologyManager.GetTerm(partTerm)

                            If term.Text <> "" Then
                                part = term.Text
                            Else
                                ' TODO: what are valid values/mappings here?
                                part = part.Substring(0, i - 1)
                            End If
                        Else
                            ' TODO: what are valid values/mappings here?
                            part = "structure"
                        End If

                        path &= "/" & part
                    Next

                    Return path
                Else
                    If mFilemanager.OntologyManager.Ontology.HasTermCode(NodeOperand) Then
                        Dim nodeTerm As RmTerm = mFilemanager.OntologyManager.GetTerm(NodeOperand)
                        Return nodeTerm.Text
                    Else
                        Return NodeOperand
                    End If
                End If
            End Get
        End Property

        Public BooleanOperator As String = "OR"

        Public Overrides Function ToString() As String
            Return ToLogicalCriteria()
        End Function

        Public Function ToPhysicalCriteria() As String
            ' Computable criteria expression
            Return NodeOperand + " " + [Operator] + " " + ValueOperand
        End Function

        Public Function ToLogicalCriteria() As String
            ' Readable criteria expression
            Return NodeText + " " + [Operator] + " " + ValueText
        End Function

        Public ReadOnly Property ValueText() As String
            Get
                Dim result As String = ValueOperand

                If ValueOperand <> "" Then
                    Dim nodeId As String = NodeOperand

                    If nodeId.IndexOf("/") >= 0 Then
                        Dim path As String = nodeId.TrimEnd("/"c)
                        Dim pathParts() As String = path.Split("/"c)
                        nodeId = pathParts(pathParts.Length - 1)

                        Dim i As Integer = nodeId.IndexOf("[") + 1
                        Debug.Assert(i > 0)

                        Dim j As Integer = nodeId.IndexOf("]")
                        Debug.Assert(j > 0)

                        nodeId = nodeId.Substring(i, j - i)
                    End If

                    Dim rmClass As RmStructure = CType(mFilemanager.Archetype.Definition, ArchetypeDefinition).Data.GetChildByNodeId(nodeId)
                    Dim valueConstraint As Constraint = CType(rmClass, RmElement).Constraint
                    Dim ordinalConstraint As Constraint_Ordinal = TryCast(valueConstraint, Constraint_Ordinal)

                    If Not ordinalConstraint Is Nothing Then
                        Dim rows() As DataRow = ordinalConstraint.OrdinalValues.Select("Ordinal = " & ValueOperand)
                        Debug.Assert(rows.Length = 1)
                        Dim ordinal As New OrdinalValue(rows(0))
                        result = ordinal.Text
                    End If
                End If

                Return result
            End Get
        End Property

        Public Sub New(ByVal a_filemanager As FileManagerLocal)
            mFilemanager = a_filemanager
        End Sub

        Public Sub New(ByVal CriteriaExpression As String, ByVal a_filemanager As FileManagerLocal)
            mFilemanager = a_filemanager
            Dim criteriaParts() As String = CriteriaExpression.Split(" "c)
            Debug.Assert(criteriaParts.Length >= 3)

            NodeOperand = criteriaParts(0)
            [Operator] = criteriaParts(1)
            ValueOperand = String.Join(" ", criteriaParts, 2, criteriaParts.Length - 2)
        End Sub
    End Class

    Private Sub AddBindingGroupBox_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddBindingGroupBox.VisibleChanged
        If AddBindingGroupBox.Visible Then
            CodeTextBox.Focus()
        End If
    End Sub

    Private Sub TermBindingPanel_RightToLeftChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.RightToLeftChanged
        Main.Reflect(Me)
    End Sub

    Dim mCurrentLogicalPath As String
    Dim mCurrentPhysicalPath As String

    Private Sub PathsTreeView_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PathsTreeView.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Dim pathNode As TermNode = CType(PathsTreeView.GetNodeAt(New Drawing.Point(e.X, e.Y)), TermNode)

            If pathNode Is Nothing Then
                mCurrentLogicalPath = ""
                mCurrentPhysicalPath = ""
            Else
                mCurrentLogicalPath = pathNode.LogicalPath
                mCurrentPhysicalPath = pathNode.PhysicalPath
            End If
        End If
    End Sub

    Private Sub ContextMenuTermBinding_Popup(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuTermBinding.Opening
        If mCurrentLogicalPath <> "" Then
            cmBindingPastePath.Enabled = True
            cmBindingPastePathLogical.Text = mCurrentLogicalPath
            cmBindingPastePathPhysical.Text = mCurrentPhysicalPath
        Else
            cmBindingPastePath.Enabled = False
        End If
    End Sub

    Private Sub cmBindingPastePathLogical_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmBindingPastePathLogical.Click, cmBindingPastePathPhysical.Click
        Clipboard.SetDataObject(CType(sender, ToolStripMenuItem).Text)
    End Sub

    Private Sub dgTermBindings_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dgTermBindings.DataError
        e.ThrowException = False
    End Sub

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
'The Original Code is TermBindingPanel.vb.
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

