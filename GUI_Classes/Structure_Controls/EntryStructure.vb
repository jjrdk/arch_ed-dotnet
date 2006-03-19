'
'
'	component:   "openEHR Archetype Project"
'	description: "$DESCRIPTION"
'	keywords:    "Archetype, Clinical, Editor"
'	author:      "Sam Heard"
'	support:     "Ocean Informatics <support@OceanInformatics.biz>"
'	copyright:   "Copyright (c) 2004,2005 Ocean Informatics Pty Ltd"
'	license:     "See notice at bottom of class"
'
'	file:        "$Source: source/vb.net/archetype_editor/GUI_Classes/Structure_Controls/SCCS/s.EntryStructure.vb $"
'	revision:    "$LastChangedRevision$"
'	last_change: "$LastChangedDate$"
'
'

Option Explicit On 

Public Class EntryStructure
    Inherits System.Windows.Forms.UserControl

    Private mStructureType As StructureType   'implement as overrided property
    Protected MenuItemSpecialise As MenuItem
    Protected MenuItemAddReference As MenuItem
    Protected OKtoEditSpecialisation As Boolean
    Protected mNodeId As String
    Protected mIsState As Boolean
    Protected mConstraintMenu As ConstraintContextMenu
    Protected mOrdinalTable As DataTable
    Protected mCurrentItem As ArchetypeNode
    Protected mFileManager As FileManagerLocal
    Protected mCardinalityControl As OccurrencesPanel

    'implement as overrided property
    Protected mControl As Control  ' the GUI control in the inherited class e.g. tree, text etc

    Protected mDragArchetypeNode As ArchetypeNode
    Public Event CurrentItemChanged(ByVal sender As ArchetypeNode, ByVal e As EventArgs)


#Region " Windows Form Designer generated code "


    Public Sub New(ByVal rm As RmStructureCompound, ByVal a_file_manager As FileManagerLocal)
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        'Add any initialization after the InitializeComponent() call

        ' need to set nodeID prior to setting structure type
        mNodeId = rm.NodeId

        mFileManager = a_file_manager

        ' layout the buttons on the icons panel
        Select Case rm.Type
            Case StructureType.Single, StructureType.List, _
                StructureType.Tree, StructureType.Table
                mStructureType = rm.Type
                SetHelpTopic(mStructureType)
            Case Else
                Debug.Assert(False)
        End Select

        ShowIcons()

        SetCardinality(rm)

    End Sub

    Public Sub New(ByVal a_structure_as_string As String, ByVal a_file_manager As FileManagerLocal)
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        'Add any initialization after the InitializeComponent() call
        mFileManager = a_file_manager

        ' also sets the node Id if it is not already set
        Me.StructureType = StructureType.Parse(StructureType.GetType, a_structure_as_string)

        ' layout the buttons on the icons panel
        Select Case mStructureType
            Case StructureType.Single, StructureType.List, _
                StructureType.Tree, StructureType.Table
                SetHelpTopic(mStructureType)
            Case Else
                Debug.Assert(False)
        End Select

        ShowIcons()

        SetCardinality(mStructureType)

    End Sub

    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        'Add any initialization after the InitializeComponent() call
        If Not Me.DesignMode Then
            Debug.Assert(False)
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
    Private components As System.ComponentModel.IContainer

    'Required by the Windows Form Designer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents PanelIcons As System.Windows.Forms.Panel
    Friend WithEvents ButAddElement As System.Windows.Forms.Button
    Friend WithEvents butRemoveElement As System.Windows.Forms.Button
    Friend WithEvents butListUp As System.Windows.Forms.Button
    Friend WithEvents butListDown As System.Windows.Forms.Button
    Friend WithEvents pbText As System.Windows.Forms.PictureBox
    Friend WithEvents pbQuantity As System.Windows.Forms.PictureBox
    Friend WithEvents pbCount As System.Windows.Forms.PictureBox
    Friend WithEvents pbDateTime As System.Windows.Forms.PictureBox
    Friend WithEvents pbOrdinal As System.Windows.Forms.PictureBox
    Friend WithEvents pbBoolean As System.Windows.Forms.PictureBox
    Friend WithEvents pbAny As System.Windows.Forms.PictureBox
    Friend WithEvents pbCluster As System.Windows.Forms.PictureBox
    Friend WithEvents butChangeDataType As System.Windows.Forms.Button
    Friend WithEvents PanelStructureHeader As System.Windows.Forms.Panel
    Friend WithEvents ilSmall As System.Windows.Forms.ImageList
    Friend WithEvents ttElement As System.Windows.Forms.ToolTip
    Friend WithEvents ToolTipSpecialisation As System.Windows.Forms.ToolTip
    Friend WithEvents helpEntryStructure As System.Windows.Forms.HelpProvider
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents lblAtcode As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(EntryStructure))
        Me.PanelIcons = New System.Windows.Forms.Panel
        Me.ButAddElement = New System.Windows.Forms.Button
        Me.butRemoveElement = New System.Windows.Forms.Button
        Me.butListUp = New System.Windows.Forms.Button
        Me.butListDown = New System.Windows.Forms.Button
        Me.pbText = New System.Windows.Forms.PictureBox
        Me.pbQuantity = New System.Windows.Forms.PictureBox
        Me.pbCount = New System.Windows.Forms.PictureBox
        Me.pbDateTime = New System.Windows.Forms.PictureBox
        Me.pbOrdinal = New System.Windows.Forms.PictureBox
        Me.pbBoolean = New System.Windows.Forms.PictureBox
        Me.pbAny = New System.Windows.Forms.PictureBox
        Me.pbCluster = New System.Windows.Forms.PictureBox
        Me.butChangeDataType = New System.Windows.Forms.Button
        Me.PanelStructureHeader = New System.Windows.Forms.Panel
        Me.lblAtcode = New System.Windows.Forms.Label
        Me.ilSmall = New System.Windows.Forms.ImageList(Me.components)
        Me.ttElement = New System.Windows.Forms.ToolTip(Me.components)
        Me.ToolTipSpecialisation = New System.Windows.Forms.ToolTip(Me.components)
        Me.helpEntryStructure = New System.Windows.Forms.HelpProvider
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.PanelIcons.SuspendLayout()
        Me.PanelStructureHeader.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelIcons
        '
        Me.PanelIcons.Controls.Add(Me.ButAddElement)
        Me.PanelIcons.Controls.Add(Me.butRemoveElement)
        Me.PanelIcons.Controls.Add(Me.butListUp)
        Me.PanelIcons.Controls.Add(Me.butListDown)
        Me.PanelIcons.Controls.Add(Me.pbText)
        Me.PanelIcons.Controls.Add(Me.pbQuantity)
        Me.PanelIcons.Controls.Add(Me.pbCount)
        Me.PanelIcons.Controls.Add(Me.pbDateTime)
        Me.PanelIcons.Controls.Add(Me.pbOrdinal)
        Me.PanelIcons.Controls.Add(Me.pbBoolean)
        Me.PanelIcons.Controls.Add(Me.pbAny)
        Me.PanelIcons.Controls.Add(Me.pbCluster)
        Me.PanelIcons.Controls.Add(Me.butChangeDataType)
        Me.PanelIcons.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelIcons.Location = New System.Drawing.Point(0, 27)
        Me.PanelIcons.Name = "PanelIcons"
        Me.PanelIcons.Size = New System.Drawing.Size(40, 333)
        Me.PanelIcons.TabIndex = 36
        '
        'ButAddElement
        '
        Me.ButAddElement.BackColor = System.Drawing.Color.PaleGoldenrod
        Me.ButAddElement.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButAddElement.ForeColor = System.Drawing.SystemColors.ControlText
        Me.helpEntryStructure.SetHelpNavigator(Me.ButAddElement, System.Windows.Forms.HelpNavigator.Topic)
        Me.ButAddElement.Image = CType(resources.GetObject("ButAddElement.Image"), System.Drawing.Image)
        Me.ButAddElement.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.ButAddElement.Location = New System.Drawing.Point(8, 4)
        Me.ButAddElement.Name = "ButAddElement"
        Me.helpEntryStructure.SetShowHelp(Me.ButAddElement, True)
        Me.ButAddElement.Size = New System.Drawing.Size(24, 25)
        Me.ButAddElement.TabIndex = 32
        '
        'butRemoveElement
        '
        Me.butRemoveElement.BackColor = System.Drawing.Color.PaleGoldenrod
        Me.butRemoveElement.ForeColor = System.Drawing.SystemColors.ControlText
        Me.butRemoveElement.Image = CType(resources.GetObject("butRemoveElement.Image"), System.Drawing.Image)
        Me.butRemoveElement.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.butRemoveElement.Location = New System.Drawing.Point(8, 31)
        Me.butRemoveElement.Name = "butRemoveElement"
        Me.butRemoveElement.Size = New System.Drawing.Size(24, 25)
        Me.butRemoveElement.TabIndex = 33
        '
        'butListUp
        '
        Me.butListUp.BackColor = System.Drawing.Color.PaleGoldenrod
        Me.butListUp.Image = CType(resources.GetObject("butListUp.Image"), System.Drawing.Image)
        Me.butListUp.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.butListUp.Location = New System.Drawing.Point(8, 58)
        Me.butListUp.Name = "butListUp"
        Me.butListUp.Size = New System.Drawing.Size(24, 25)
        Me.butListUp.TabIndex = 30
        '
        'butListDown
        '
        Me.butListDown.BackColor = System.Drawing.Color.PaleGoldenrod
        Me.butListDown.Image = CType(resources.GetObject("butListDown.Image"), System.Drawing.Image)
        Me.butListDown.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.butListDown.Location = New System.Drawing.Point(8, 85)
        Me.butListDown.Name = "butListDown"
        Me.butListDown.Size = New System.Drawing.Size(24, 25)
        Me.butListDown.TabIndex = 31
        '
        'pbText
        '
        Me.pbText.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbText.Image = CType(resources.GetObject("pbText.Image"), System.Drawing.Image)
        Me.pbText.Location = New System.Drawing.Point(8, 112)
        Me.pbText.Name = "pbText"
        Me.pbText.Size = New System.Drawing.Size(24, 25)
        Me.pbText.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbText.TabIndex = 32
        Me.pbText.TabStop = False
        '
        'pbQuantity
        '
        Me.pbQuantity.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbQuantity.Image = CType(resources.GetObject("pbQuantity.Image"), System.Drawing.Image)
        Me.pbQuantity.Location = New System.Drawing.Point(8, 139)
        Me.pbQuantity.Name = "pbQuantity"
        Me.pbQuantity.Size = New System.Drawing.Size(24, 25)
        Me.pbQuantity.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbQuantity.TabIndex = 33
        Me.pbQuantity.TabStop = False
        '
        'pbCount
        '
        Me.pbCount.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbCount.Image = CType(resources.GetObject("pbCount.Image"), System.Drawing.Image)
        Me.pbCount.Location = New System.Drawing.Point(8, 166)
        Me.pbCount.Name = "pbCount"
        Me.pbCount.Size = New System.Drawing.Size(24, 25)
        Me.pbCount.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbCount.TabIndex = 39
        Me.pbCount.TabStop = False
        '
        'pbDateTime
        '
        Me.pbDateTime.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbDateTime.Image = CType(resources.GetObject("pbDateTime.Image"), System.Drawing.Image)
        Me.pbDateTime.Location = New System.Drawing.Point(8, 193)
        Me.pbDateTime.Name = "pbDateTime"
        Me.pbDateTime.Size = New System.Drawing.Size(24, 25)
        Me.pbDateTime.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbDateTime.TabIndex = 34
        Me.pbDateTime.TabStop = False
        '
        'pbOrdinal
        '
        Me.pbOrdinal.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbOrdinal.Image = CType(resources.GetObject("pbOrdinal.Image"), System.Drawing.Image)
        Me.pbOrdinal.Location = New System.Drawing.Point(8, 220)
        Me.pbOrdinal.Name = "pbOrdinal"
        Me.pbOrdinal.Size = New System.Drawing.Size(24, 25)
        Me.pbOrdinal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbOrdinal.TabIndex = 35
        Me.pbOrdinal.TabStop = False
        '
        'pbBoolean
        '
        Me.pbBoolean.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbBoolean.Image = CType(resources.GetObject("pbBoolean.Image"), System.Drawing.Image)
        Me.pbBoolean.Location = New System.Drawing.Point(8, 247)
        Me.pbBoolean.Name = "pbBoolean"
        Me.pbBoolean.Size = New System.Drawing.Size(24, 25)
        Me.pbBoolean.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbBoolean.TabIndex = 37
        Me.pbBoolean.TabStop = False
        '
        'pbAny
        '
        Me.pbAny.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbAny.Image = CType(resources.GetObject("pbAny.Image"), System.Drawing.Image)
        Me.pbAny.Location = New System.Drawing.Point(8, 274)
        Me.pbAny.Name = "pbAny"
        Me.pbAny.Size = New System.Drawing.Size(24, 25)
        Me.pbAny.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbAny.TabIndex = 36
        Me.pbAny.TabStop = False
        '
        'pbCluster
        '
        Me.pbCluster.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbCluster.Image = CType(resources.GetObject("pbCluster.Image"), System.Drawing.Image)
        Me.pbCluster.Location = New System.Drawing.Point(8, 301)
        Me.pbCluster.Name = "pbCluster"
        Me.pbCluster.Size = New System.Drawing.Size(24, 25)
        Me.pbCluster.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbCluster.TabIndex = 38
        Me.pbCluster.TabStop = False
        '
        'butChangeDataType
        '
        Me.helpEntryStructure.SetHelpKeyword(Me.butChangeDataType, "HowTo/Edit data/change_datatype.htm")
        Me.helpEntryStructure.SetHelpNavigator(Me.butChangeDataType, System.Windows.Forms.HelpNavigator.Topic)
        Me.butChangeDataType.Image = CType(resources.GetObject("butChangeDataType.Image"), System.Drawing.Image)
        Me.butChangeDataType.Location = New System.Drawing.Point(8, 328)
        Me.butChangeDataType.Name = "butChangeDataType"
        Me.helpEntryStructure.SetShowHelp(Me.butChangeDataType, True)
        Me.butChangeDataType.Size = New System.Drawing.Size(24, 25)
        Me.butChangeDataType.TabIndex = 34
        Me.ttElement.SetToolTip(Me.butChangeDataType, "Change data type")
        Me.butChangeDataType.Visible = False
        '
        'PanelStructureHeader
        '
        Me.PanelStructureHeader.Controls.Add(Me.lblAtcode)
        Me.PanelStructureHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelStructureHeader.Location = New System.Drawing.Point(0, 0)
        Me.PanelStructureHeader.Name = "PanelStructureHeader"
        Me.PanelStructureHeader.Size = New System.Drawing.Size(384, 24)
        Me.PanelStructureHeader.TabIndex = 37
        '
        'lblAtcode
        '
        Me.lblAtcode.Dock = System.Windows.Forms.DockStyle.Right
        Me.lblAtcode.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblAtcode.Location = New System.Drawing.Point(312, 0)
        Me.lblAtcode.Name = "lblAtcode"
        Me.lblAtcode.Size = New System.Drawing.Size(72, 24)
        Me.lblAtcode.TabIndex = 0
        Me.lblAtcode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ilSmall
        '
        Me.ilSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit
        Me.ilSmall.ImageSize = New System.Drawing.Size(20, 20)
        Me.ilSmall.ImageStream = CType(resources.GetObject("ilSmall.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ilSmall.TransparentColor = System.Drawing.Color.Transparent
        '
        'helpEntryStructure
        '
        Me.helpEntryStructure.HelpNamespace = ""
        '
        'Splitter1
        '
        Me.Splitter1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Splitter1.Location = New System.Drawing.Point(0, 24)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(384, 3)
        Me.Splitter1.TabIndex = 38
        Me.Splitter1.TabStop = False
        '
        'EntryStructure
        '
        Me.Controls.Add(Me.PanelIcons)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.PanelStructureHeader)
        Me.helpEntryStructure.SetHelpKeyword(Me, "Edit an archetype")
        Me.helpEntryStructure.SetHelpNavigator(Me, System.Windows.Forms.HelpNavigator.TableOfContents)
        Me.Name = "EntryStructure"
        Me.helpEntryStructure.SetShowHelp(Me, True)
        Me.Size = New System.Drawing.Size(384, 360)
        Me.PanelIcons.ResumeLayout(False)
        Me.PanelStructureHeader.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public ReadOnly Property SelectedImageOffset() As Integer
        Get
            Return 26
        End Get
    End Property
    'implement as overrided property
    Public Property StructureType() As StructureType
        Get
            Return mStructureType
        End Get
        Set(ByVal Value As StructureType)
            mStructureType = Value
            Dim s As String

            Select Case Value
                Case StructureType.Single
                    s = Filemanager.GetOpenEhrTerm(105, "Single")
                Case StructureType.List
                    s = Filemanager.GetOpenEhrTerm(106, "List")
                Case StructureType.Tree
                    s = Filemanager.GetOpenEhrTerm(107, "Tree")
                Case StructureType.Table
                    s = Filemanager.GetOpenEhrTerm(108, "Table")
            End Select
            If mNodeId = "" Then
                mNodeId = mFileManager.OntologyManager.AddTerm(s, "@ internal @").Code
            Else
                mFileManager.OntologyManager.SetText(s, mNodeId)
            End If
            ShowIcons()
        End Set
    End Property

    Public Overridable ReadOnly Property InterfaceBuilder() As Object
        Get
            Throw New NotImplementedException("Subclass must override this property")
        End Get
    End Property

    Public Overridable ReadOnly Property Elements() As ArchetypeElement()
        Get
            Throw New NotImplementedException("Subclass must override this property")
        End Get
    End Property

    Public Overridable Property Archetype() As RmStructureCompound
        Get
            Throw New NotImplementedException("Subclass must override this property")
        End Get
        Set(ByVal Value As RmStructureCompound)
            Throw New NotImplementedException("Subclass must override this property")
        End Set
    End Property

    Protected Sub SetCardinality(ByVal rm As RmStructureCompound)
        SetCardinality(rm.Type)
        mCardinalityControl.Cardinality = rm.Children.Cardinality
    End Sub

    Protected Sub SetCardinality(ByVal a_structure_type As StructureType)
        mCardinalityControl = New OccurrencesPanel(mFileManager)
        mCardinalityControl.LocalFileManager = mFileManager
        If a_structure_type = StructureType.Single Then
            mCardinalityControl.SetSingle = True
        Else
            mCardinalityControl.IsContainer = True
            mCardinalityControl.Location = New Drawing.Point(0, 0)
            Me.PanelStructureHeader.Controls.Add(mCardinalityControl)
        End If
    End Sub


    Protected Sub SetHelpTopic(ByVal a_structure_type As StructureType)

        Me.helpEntryStructure.SetHelpNavigator(Me, HelpNavigator.Topic)

        Select Case a_structure_type
            Case StructureType.Single
                Me.helpEntryStructure.SetHelpKeyword(Me, "Screens/structure_simple.htm")
            Case StructureType.List
                Me.helpEntryStructure.SetHelpKeyword(Me, "Screens/structure_list.htm")
            Case StructureType.Tree
                Me.helpEntryStructure.SetHelpKeyword(Me, "Screens/structure_tree.htm")
            Case StructureType.Table
                Me.helpEntryStructure.SetHelpKeyword(Me, "Screens/structure_table.htm")
        End Select
    End Sub

    Public Overridable Sub Translate()
        RaiseEvent CurrentItemChanged(mCurrentItem, New EventArgs)
    End Sub

    Protected Overridable Sub SpecialiseCurrentItem(ByVal sender As Object, ByVal e As EventArgs)
        Throw New NotImplementedException("Subclass must override this method")
    End Sub

    Public Overridable Sub Reset()
        Throw New NotImplementedException("Subclass must override this method")
    End Sub

    Protected Overridable Sub SetUpAddElementMenu()
        Throw New NotImplementedException("Subclass must override this method")
    End Sub

    Protected Overridable Sub AddNewElement(ByVal a_constraint As Constraint)
        Throw New NotImplementedException("Subclass must override this method")
    End Sub

    Protected Overridable Sub AddReference(ByVal sender As Object, ByVal e As EventArgs)
        Throw New NotImplementedException("Subclass must override this method")
    End Sub

    Protected Overridable Sub RemoveItemAndReferences(ByVal sender As Object, ByVal e As EventArgs)
        Throw New NotImplementedException("Subclass must override this method")
    End Sub

    Public Overridable Function ToRichText(ByVal indentlevel As Integer, ByVal new_line As String) As String
        Throw New NotImplementedException("Subclass must override this method")
    End Function

    Public Overridable Function ToHTML(ByVal BackGroundColour As String) As String
        Throw New NotImplementedException("Subclass must override this method")
    End Function

    Protected Overridable Sub ButListUp_Click(ByVal sender As Object, ByVal e As EventArgs)
        Throw New NotImplementedException("Subclass must override this method")
    End Sub

    Protected Overridable Sub ButListDown_Click(ByVal sender As Object, ByVal e As EventArgs)
        Throw New NotImplementedException("Subclass must override this method")
    End Sub

    Protected Overridable Sub RefreshIcons()
        Throw New NotImplementedException("Subclass must override this method")
    End Sub

    Protected Overloads Sub SetCurrentItem(ByVal a_node As ArchetypeNode)
        ' if nothing this hides panelDetails
        mCurrentItem = a_node
        Me.lblAtcode.Text = a_node.RM_Class.NodeId
        If Not a_node Is Nothing Then
            SetButtonVisibility(a_node)
        End If
        RaiseEvent CurrentItemChanged(a_node, New EventArgs)
    End Sub

    Protected Overloads Sub SetCurrentItem(ByVal a_node As ArchetypeNode, ByVal cardinality As RmCardinality)
        ' if nothing this hides panelDetails
        mCurrentItem = a_node
        If Not a_node Is Nothing Then
            SetButtonVisibility(a_node)
        End If
        RaiseEvent CurrentItemChanged(a_node, New EventArgs)

    End Sub


    Protected Sub SetButtonVisibility(ByVal a_node As ArchetypeNode)

        'Hide the icons if simple to stop drag and drop
        If mStructureType = StructureType.Single AndAlso Me.pbText.Visible Then
            Me.pbAny.Visible = False
            Me.pbBoolean.Visible = False
            Me.pbCount.Visible = False
            Me.pbDateTime.Visible = False
            Me.pbOrdinal.Visible = False
            Me.pbQuantity.Visible = False
            Me.pbText.Visible = False
            LayoutIcons()
        End If

        If a_node.RM_Class.Type = StructureType.Element Then
            If CType(a_node, ArchetypeElement).IsReference Then
                Me.butChangeDataType.Enabled = False
            Else
                Me.butChangeDataType.Enabled = True
            End If

            If mFileManager.OntologyManager.NumberOfSpecialisations > 0 Then
                ' ensure that datatypes cannot be changed in specialisations
                'except for any
                If CType(a_node, ArchetypeElement).Constraint.Type = ConstraintType.Any Then
                    Me.butChangeDataType.Visible = True
                Else
                    If mFileManager.OntologyManager.NumberOfSpecialisations = OceanArchetypeEditor.Instance.CountInString(a_node.RM_Class.NodeId, ".") Then
                        Me.butChangeDataType.Visible = True
                    Else
                        Me.butChangeDataType.Visible = False
                    End If
                End If
            Else
                Me.butChangeDataType.Visible = True
            End If
        Else
            ' a cluster
            Me.butChangeDataType.Visible = False
        End If
    End Sub

    Protected Sub SetToolTipSpecialisation(ByVal Ctrl As Control, ByVal Item As ArchetypeNode)

        If Item Is Nothing Then
            Me.ToolTipSpecialisation.RemoveAll()
            Return
        End If

        If mFileManager.OntologyManager.NumberOfSpecialisations > 0 Then
            Dim s As String
            Dim nl As String
            Dim ct() As CodeAndTerm
            Dim i As Integer

            nl = Chr(10) & Chr(13)

            If Not Item.IsAnonymous Then
                ct = OceanArchetypeEditor.Instance.GetSpecialisationChain(CType(Item, ArchetypeNodeAbstract).NodeId)

                If ct.Length = 1 Then
                    Me.ToolTipSpecialisation.RemoveAll()
                    Return
                End If
                s = "Specialised:" & nl
                For i = 0 To ct.Length - 1
                    s = s & Space((i * 2) + 2) & "- " & ct(i).Text
                    If i < ct.Length - 1 Then
                        s = s & nl
                    End If
                Next
                Me.ToolTipSpecialisation.SetToolTip(Ctrl, s)
            End If
        End If

    End Sub

    Protected Function ImageIndexForConstraintType(ByVal ct As ConstraintType, Optional ByVal isReference As Boolean = False, _
        Optional ByVal isSelected As Boolean = False) As Integer

        Dim offset As Integer

        If isReference Then offset = 13
        If isSelected Then offset += Me.SelectedImageOffset

        Select Case ct
            Case ConstraintType.Quantity
                Return 0 + offset
            Case ConstraintType.Text
                Return 1 + offset
            Case ConstraintType.Boolean
                Return 2 + offset
            Case ConstraintType.Ordinal
                Return 3 + offset
            Case ConstraintType.Count
                Return 4 + offset
            Case ConstraintType.DateTime
                Return 5 + offset
            Case ConstraintType.Any
                Return 6 + offset
            Case ConstraintType.Multiple
                Return 7 + offset
            Case ConstraintType.Interval_Count, ConstraintType.Interval_Quantity, ConstraintType.Interval_DateTime
                Return 8 + offset
            Case ConstraintType.MultiMedia
                Return 9 + offset
            Case ConstraintType.URI
                Return 10 + offset
            Case ConstraintType.Ratio
                Return 11 + offset
            Case ConstraintType.Duration
                Return 12 + offset
            Case Else
                Debug.Assert(False, "Constraint not handled")
        End Select
    End Function

    Private Sub LayoutIcons()
        ' now space the buttons consistently
        Dim ctrl As Control
        Dim loc As New System.Drawing.Point(8, 4)
        For Each ctrl In PanelIcons.Controls
            If ctrl.Visible Then
                ctrl.Location = loc
                loc.Y += 27
            End If
        Next
        'tag the change datatype button on the end
        Me.butChangeDataType.Location = loc

    End Sub

    Protected Sub ShowIcons()
        ' turn off any inappropriate buttons
        Select Case mStructureType
            Case StructureType.Single
                Me.butListUp.Visible = False
                Me.butListDown.Visible = False
                Me.pbCluster.Visible = False
                Me.ButAddElement.Visible = False
                Me.butRemoveElement.Visible = False
            Case StructureType.List
                Me.pbCluster.Visible = False
            Case StructureType.Table
                Me.butListUp.Visible = False
                Me.butListDown.Visible = False
                Me.pbCluster.Visible = False
        End Select

        LayoutIcons()

    End Sub

    Private Sub ButAddElement_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButAddElement.Click
        mConstraintMenu = New ConstraintContextMenu(AddressOf AddNewElement, mFileManager)
        mConstraintMenu.ShowHeader(Filemanager.GetOpenEhrTerm(155, "Add"))
        SetUpAddElementMenu()
    End Sub

    Private Sub butRemoveElement_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butRemoveElement.Click
        RemoveItemAndReferences(sender, e)
    End Sub

    Private Sub ChangeConstraint(ByVal a_constraint As Constraint)
        Debug.Assert(mCurrentItem.RM_Class.Type = StructureType.Element)
        If a_constraint.Type = ConstraintType.Multiple Then
            'Add the current constraint to the multiple constraint before setting the current item to the multiple
            CType(a_constraint, Constraint_Choice).Constraints.Add(CType(mCurrentItem, ArchetypeElement).Constraint)
        ElseIf CType(mCurrentItem, ArchetypeElement).Constraint.Type = ConstraintType.Multiple Then
            'Or if the current item is multiple
            Dim m As Constraint_Choice
            m = CType(mCurrentItem, ArchetypeElement).Constraint
            For Each c As Constraint In m.Constraints
                'find the constraint that is of the same type as a_constraint if there is one
                If c.Type = a_constraint.Type Then
                    a_constraint = c
                End If
            Next
        End If
        'now set the current item to the new constraint
        CType(mCurrentItem, ArchetypeElement).Constraint = a_constraint
        mFileManager.FileEdited = True
        RefreshIcons()
        RaiseEvent CurrentItemChanged(mCurrentItem, New EventArgs)
    End Sub

    Private Sub butChangeDataType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butChangeDataType.Click
        Debug.Assert(Not mCurrentItem Is Nothing, "Button should not be available")
        Debug.Assert(mCurrentItem.RM_Class.Type = StructureType.Element, "Button should not be available")
        mConstraintMenu = New ConstraintContextMenu(New ConstraintContextMenu.ProcessMenuClick(AddressOf ChangeConstraint), mFileManager)
        ' hide the current constraint type
        mConstraintMenu.HideMenuItem(CType(mCurrentItem, ArchetypeElement).Constraint.Type)
        mConstraintMenu.ShowHeader(Filemanager.GetOpenEhrTerm(60, "Change data type"))
        mConstraintMenu.Show(butChangeDataType, New System.Drawing.Point(5, 5))
    End Sub

    Private Sub cbOrdered_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If mCurrentItem Is Nothing Then Return

        mFileManager.FileEdited = True
    End Sub

    Protected Overrides Sub OnBackColorChanged(ByVal e As System.EventArgs)
        ' changes the colour of some buttons when the background colour changes
        If Me.BackColor.Equals(System.Drawing.Color.LightSteelBlue) Then
            Me.ButAddElement.BackColor = System.Drawing.Color.CornflowerBlue
            Me.butRemoveElement.BackColor = System.Drawing.Color.CornflowerBlue
            Me.butListUp.BackColor = System.Drawing.Color.CornflowerBlue
            Me.butListDown.BackColor = System.Drawing.Color.CornflowerBlue
        End If
    End Sub


    Private Sub EntryStructure_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Me.DesignMode Then
            ' add the tooltips to the buttons on the left
            Me.ttElement.SetToolTip(Me.pbText, AE_Constants.Instance.Text)
            Me.ttElement.SetToolTip(Me.pbQuantity, AE_Constants.Instance.Quantity)
            Me.ttElement.SetToolTip(Me.pbAny, AE_Constants.Instance.Any)
            Me.ttElement.SetToolTip(Me.pbBoolean, AE_Constants.Instance.Boolean_)
            Me.ttElement.SetToolTip(Me.pbOrdinal, AE_Constants.Instance.Ordinal)
            Me.ttElement.SetToolTip(Me.pbCount, AE_Constants.Instance.Count)
            Me.ttElement.SetToolTip(Me.pbDateTime, AE_Constants.Instance.DateTime)
            Me.ttElement.SetToolTip(Me.pbCluster, AE_Constants.Instance.Cluster)

            Me.helpEntryStructure.HelpNamespace = OceanArchetypeEditor.Instance.Options.HelpLocationPath

        End If
    End Sub

    Private Sub ChangeStructure(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

#Region "Drag and Drop operations for toolbar on left"

    Private Sub pbGroup_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) _
                Handles pbAny.MouseUp, pbBoolean.MouseUp, pbCluster.MouseUp, pbCount.MouseUp, pbDateTime.MouseUp, _
                    pbOrdinal.MouseUp, pbText.MouseUp, pbQuantity.MouseUp

        'cancel drag and drop operation
        mDragArchetypeNode = Nothing
    End Sub

    Private Sub pbGroup_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) _
                Handles pbAny.MouseDown, pbBoolean.MouseDown, pbCluster.MouseDown, pbCount.MouseDown, _
                        pbDateTime.MouseDown, pbOrdinal.MouseDown, pbQuantity.MouseDown, pbText.MouseDown

        Dim rm As RmElement
        Dim pb As PictureBox

        ' create mDragArchetype node with the correct constraint

        If sender.name = "pbCluster" Then
            mDragArchetypeNode = New ArchetypeComposite(Filemanager.GetOpenEhrTerm(322, "New cluster"), StructureType.Cluster, mFileManager)
        Else
            Dim c As New Constraint

            mDragArchetypeNode = New ArchetypeElement(Filemanager.GetOpenEhrTerm(109, "New element"), mFileManager)

            Select Case sender.Name
                Case "pbAny"
                    c = New Constraint
                Case "pbBoolean"
                    c = New Constraint_Boolean
                Case "pbCount"
                    c = New Constraint_Count
                Case "pbDateTime"
                    c = New Constraint_DateTime
                Case "pbOrdinal"
                    c = New Constraint_Ordinal(True, mFileManager)
                Case "pbQuantity"
                    c = New Constraint_Quantity
                Case "pbText"
                    c = New Constraint_Text
            End Select
            CType(mDragArchetypeNode, ArchetypeElement).Constraint = c
        End If

        If mControl.Enabled = False Then
            mControl.Enabled = True
        End If

        mControl.AllowDrop = True

        sender.DoDragDrop(mDragArchetypeNode, DragDropEffects.Copy)

    End Sub


#End Region

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
'The Original Code is EntryStructure.vb.
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
