'
'
'	component:   "openEHR Archetype Project"
'	description: "Builds all XML Archetypes"
'	keywords:    "Archetype, Clinical, Editor"
'	author:      "Sam Heard"
'	support:     "Ocean Informatics <support@OceanInformatics.biz>"
'	copyright:   "Copyright (c) 2004,2005,2006 Ocean Informatics Pty Ltd"
'	license:     "See notice at bottom of class"
'
'	file:        "$Source: source/vb.net/archetype_editor/ADL_Classes/SCCS/s.ADL_Archetype.vb $"
'	revision:    "$LastChangedRevision$"
'	last_change: "$LastChangedDate: 2006-05-17 18:54:30 +0930 (Wed, 17 May 2006) $"
'
'

Option Explicit On 

Namespace ArchetypeEditor.XML_Classes

    Public Class XML_Archetype
        Inherits Archetype

        'Builds all archetypes at present

        Private mXmlArchetype As XMLParser.ARCHETYPE
        Private mArchetypeParser As XMLParser.XmlArchetypeParser
        Private mAomFactory As XMLParser.AomFactory

        Private Structure ReferenceToResolve
            Dim Element As RmElement
            Dim Attribute As XMLParser.C_ATTRIBUTE
        End Structure

        Protected ReferencesToResolve As ArrayList = New ArrayList


        Public Overrides Property ConceptCode() As String
            Get
                Return mXmlArchetype.concept_code
            End Get
            Set(ByVal Value As String)
                mXmlArchetype.concept_code = Value
            End Set
        End Property

        Public Overrides ReadOnly Property ArchetypeAvailable() As Boolean
            Get
                Return mArchetypeParser.ArchetypeAvailable
            End Get
        End Property
        Public Overrides Property Archetype_ID() As ArchetypeID
            Get
                Try
                    Return mArchetypeID
                Catch
                    Debug.Assert(False)
                    Return Nothing
                End Try
            End Get
            Set(ByVal Value As ArchetypeID)
                SetArchetypeId(Value)
            End Set
        End Property
        Public Overrides Property LifeCycle() As String
            Get
                Return sLifeCycle
            End Get
            Set(ByVal Value As String)
                sLifeCycle = Value
            End Set
        End Property
        Public Overrides Property ParentArchetype() As String
            Get
                Return mXmlArchetype.parent_archetype_id
            End Get
            Set(ByVal Value As String)
                mXmlArchetype.parent_archetype_id = Value
            End Set
        End Property
        Public Overrides ReadOnly Property SourceCode() As String
            Get
                Return mArchetypeParser.Serialise
            End Get
        End Property
        Public Overrides ReadOnly Property SerialisedArchetype(ByVal a_format As String) As String
            Get
                If a_format.ToLowerInvariant() = "xml" Then
                    Me.MakeParseTree()
                    Return mArchetypeParser.Serialise
                Else
                    Debug.Assert(False, "Cannot return format '" + a_format + " from XML parser!")
                    Return "Error - " + a_format + " is not available for XML parser"
                End If
            End Get
        End Property
        Public Overrides ReadOnly Property Paths(ByVal LanguageCode As String, ByVal parserIsSynchronised As Boolean, Optional ByVal Logical As Boolean = False) As String()
            Get
                Dim list As System.Collections.ArrayList

                ' must call the prepareToSave to ensure it is accurate
                If (Not Filemanager.Master.FileLoading) AndAlso (Not parserIsSynchronised) Then
                    MakeParseTree()
                End If
                ' showing the task with logical paths takes a lot of space
                If Logical Then
                    list = mArchetypeParser.LogicalPaths(LanguageCode)
                Else
                    list = mArchetypeParser.PhysicalPaths()
                End If

                Return list.ToArray(GetType(String))

            End Get
        End Property

        Public Overrides Sub Specialise(ByVal ConceptShortName As String, ByRef The_Ontology As OntologyManager)
            Dim a_term As RmTerm

            mArchetypeParser.SpecialiseArchetype(ConceptShortName)
            ' Update the GUI tables with the new term
            a_term = New XML_Term(mArchetypeParser.Ontology.TermDefinition(The_Ontology.LanguageCode, mXmlArchetype.concept_code))
            The_Ontology.UpdateTerm(a_term)
            Me.mArchetypeID.Concept &= "-" & ConceptShortName
        End Sub

        Public Sub RemoveUnusedCodes()
            mArchetypeParser.Ontology.RemoveUnusedCodes()
        End Sub

        Protected Sub SetArchetypeId(ByVal an_archetype_id As ArchetypeID)
            Try
                If Not mArchetypeParser.ArchetypeAvailable Then
                    mArchetypeParser.NewArchetype(an_archetype_id.ToString(), sPrimaryLanguageCode, OceanArchetypeEditor.DefaultLanguageCodeSet)
                    mXmlArchetype = mArchetypeParser.Archetype
                    mArchetypeParser.SetDefinitionId(mXmlArchetype.concept_code)
                    setDefinition()
                Else
                    ' does this involve a change in the entity (affects the GUI a great deal!)
                    If mXmlArchetype.archetype_id.Contains(an_archetype_id.ReferenceModelEntity) Then
                        Debug.Assert(False, "Not handled")
                        ' will need to reset the GUI to the new entity
                        setDefinition()
                    End If
                    mXmlArchetype.archetype_id = an_archetype_id.ToString()
                End If
                ' set the internal variable last in case errors
                mArchetypeID = an_archetype_id
            Catch e As Exception
                Debug.Assert(False, "Error setting archetype id")
                Beep()
            End Try
        End Sub

        Protected Sub ArchetypeID_Changed(ByVal sender As Object, ByVal e As EventArgs) Handles mArchetypeID.ArchetypeID_Changed
            SetArchetypeId(CType(sender, ArchetypeID))
        End Sub

        Private Function MakeAssertion(ByVal id As String, ByVal expression As String) As XMLParser.ASSERTION
            Dim id_expression_leaf, id_pattern_expression_leaf As XMLParser.EXPR_LEAF
            Dim match_operator As XMLParser.EXPR_BINARY_OPERATOR
            Dim assert As New XMLParser.ASSERTION

            Debug.Assert((Not id Is Nothing) And (id <> ""))

            id_expression_leaf = New XMLParser.EXPR_LEAF()
            id_expression_leaf.type = "String"
            id_expression_leaf.item = id
            id_expression_leaf.reference_type = "attribute"

            id_pattern_expression_leaf = New XMLParser.EXPR_LEAF()
            id_pattern_expression_leaf.type = "C_STRING"
            id_pattern_expression_leaf.reference_type = "constraint"

            Dim c_s As New XMLParser.C_STRING()

            If expression = "*" Then
                c_s.pattern = "/.*/"
            Else
                c_s.pattern = "/" + expression + "/"
            End If
            id_pattern_expression_leaf.item = c_s

            match_operator = New XMLParser.EXPR_BINARY_OPERATOR()
            match_operator.type = "Boolean"
            match_operator.operator = Global.XMLParser.OPERATOR_KIND.Item2007
            match_operator.left_operand = id_expression_leaf
            match_operator.right_operand = id_pattern_expression_leaf

            assert.expression = match_operator

            Return assert

        End Function

        Private Function MakeCardinality(ByVal c As RmCardinality, Optional ByVal IsOrdered As Boolean = True) As XMLParser.CARDINALITY
            Dim cardObj As XMLParser.CARDINALITY

            cardObj = New XMLParser.CARDINALITY()
            cardObj.interval = New XMLParser.interval_of_integer()
            cardObj.interval.includes_maximum = True
            cardObj.interval.includes_minimum = True
            cardObj.interval.minimum = c.MinCount

            If Not c.IsUnbounded Then
                cardObj.interval.maximum = CStr(c.MaxCount)
            End If
            If c.Ordered Then
                cardObj.is_ordered = True
            Else
                cardObj.is_ordered = False
            End If
            Return cardObj

        End Function

        Private Function MakeOccurrences(ByVal c As RmCardinality) As XMLParser.interval_of_integer
            Dim an_interval As New XMLParser.interval_of_integer()

            an_interval.includes_maximum = True
            an_interval.minimum = 0
            If c.IsUnbounded Then
                an_interval.includes_minimum = c.IncludeLower
                an_interval.minimum = c.MinCount
            Else
                an_interval.includes_minimum = c.IncludeLower
                an_interval.minimum = c.MinCount
                an_interval.includes_maximum = c.IncludeUpper
                an_interval.maximum = c.MaxCount
            End If
            Return an_interval
        End Function

        Private Overloads Sub BuildCodedText(ByVal value_attribute As XMLParser.C_ATTRIBUTE, ByVal ConstraintID As String)
            Dim coded_text As XMLParser.C_COMPLEX_OBJECT
            Dim code_rel_node As XMLParser.C_ATTRIBUTE
            Dim ca_Term As XMLParser.CONSTRAINT_REF

            coded_text = mAomFactory.MakeComplexObject(value_attribute, "DV_CODED_TEXT")
            code_rel_node = mAomFactory.MakeSingleAttribute(coded_text, "defining_code")
            ca_Term = New XMLParser.CONSTRAINT_REF()
            ca_Term.rm_type_name = "External constraint"
            ca_Term.reference = ConstraintID
            mAomFactory.add_object(code_rel_node, ca_Term)
        End Sub

        Private Overloads Sub BuildCodedText(ByRef ObjNode As XMLParser.C_COMPLEX_OBJECT, ByVal RunTimeName As String)
            Dim coded_text As XMLParser.C_COMPLEX_OBJECT
            Dim code_rel_node, name_rel_node As XMLParser.C_ATTRIBUTE
            Dim ca_Term As XMLParser.CONSTRAINT_REF

            name_rel_node = mAomFactory.MakeSingleAttribute(ObjNode, "name")
            coded_text = mAomFactory.MakeComplexObject(name_rel_node, "DV_CODED_TEXT")
            code_rel_node = mAomFactory.MakeSingleAttribute(coded_text, "defining_code")
            ca_Term = New XMLParser.CONSTRAINT_REF()
            ca_Term.rm_type_name = "External constraint"
            ca_Term.reference = RunTimeName
            mAomFactory.add_object(code_rel_node, ca_Term)
        End Sub

        Private Overloads Sub BuildCodedText(ByVal value_attribute As XMLParser.C_ATTRIBUTE, ByVal a_CodePhrase As CodePhrase, Optional ByVal an_assumed_value As String = "")
            Dim coded_text As XMLParser.C_COMPLEX_OBJECT
            Dim code_rel_node As XMLParser.C_ATTRIBUTE
            Dim ca_Term As New XMLParser.C_CODE_PHRASE

            coded_text = mAomFactory.MakeComplexObject(value_attribute, "DV_CODED_TEXT")
            ca_Term.rm_type_name = "CODE_PHRASE"

            code_rel_node = mAomFactory.MakeSingleAttribute(coded_text, "defining_code")
            If a_CodePhrase.Codes.Count > 0 Then
                ca_Term.terminology = a_CodePhrase.TerminologyID
                ca_Term.code_list = Array.CreateInstance(GetType(String), a_CodePhrase.Codes.Count)
                For i As Integer = 0 To a_CodePhrase.Codes.Count - 1
                    ca_Term.code_list(i) = a_CodePhrase.Codes(i)
                Next
                If an_assumed_value <> "" Then
                    ca_Term.assumed_value = an_assumed_value
                End If
            Else
                ca_Term.terminology = a_CodePhrase.TerminologyID
            End If
            mAomFactory.add_object(code_rel_node, ca_Term)
        End Sub

        Private Sub BuildPlainText(ByVal value_attribute As XMLParser.C_ATTRIBUTE, ByVal TermList As Collections.Specialized.StringCollection)
            Dim plain_text As XMLParser.C_COMPLEX_OBJECT
            Dim value_rel_node As XMLParser.C_ATTRIBUTE
            Dim cString As XMLParser.C_STRING
            Dim xmlSimple As XMLParser.C_PRIMITIVE_OBJECT

            plain_text = mAomFactory.MakeComplexObject(value_attribute, "DV_TEXT")

            If TermList.Count > 0 Then
                Dim i As Integer
                value_rel_node = mAomFactory.MakeSingleAttribute(plain_text, "value")
                cString = New XMLParser.C_STRING()
                cString.list = Array.CreateInstance(GetType(String), TermList.Count)
                For i = 0 To TermList.Count - 1
                    cString.list(i) = TermList.Item(i)
                Next
                xmlSimple = mAomFactory.MakePrimitiveObject(value_rel_node, cString)
            Else
                plain_text.any_allowed = True
            End If

        End Sub

        Private Sub DuplicateHistory(ByVal rm As RmStructureCompound, ByRef RelNode As XMLParser.C_ATTRIBUTE)

            Dim xmlHistory, xmlEvent As XMLParser.C_COMPLEX_OBJECT
            Dim an_attribute As XMLParser.C_ATTRIBUTE
            Dim an_event As RmEvent
            Dim rm_1 As RmStructureCompound
            Dim a_history As RmHistory

            For Each rm_1 In CType(cDefinition, ArchetypeDefinition).Data
                If rm_1.Type = StructureType.History Then
                    a_history = CType(rm_1, RmHistory)
                    xmlHistory = mAomFactory.MakeComplexObject( _
                        RelNode, _
                        ReferenceModel.RM_StructureName(StructureType.History), _
                        a_history.NodeId, _
                        MakeOccurrences(a_history.Occurrences))

                    If Not a_history.HasNameConstraint Then
                        an_attribute = mAomFactory.MakeSingleAttribute(xmlHistory, "name")
                        BuildText(an_attribute, a_history.NameConstraint)
                    End If
                    If a_history.isPeriodic Then
                        Dim durationConstraint As New Constraint_Duration

                        an_attribute = mAomFactory.MakeSingleAttribute(xmlHistory, "period")
                        durationConstraint.MinMaxValueUnits = a_history.PeriodUnits
                        'Set max and min to offset value
                        durationConstraint.MinimumValue = a_history.Period
                        durationConstraint.HasMinimum = True
                        durationConstraint.MaximumValue = a_history.Period
                        durationConstraint.HasMaximum = True
                        BuildDuration(an_attribute, durationConstraint)
                    End If

                    ' now build the events
                    If a_history.Children.Count > 0 Then
                        an_attribute = mAomFactory.MakeMultipleAttribute( _
                            xmlHistory, _
                            "events", _
                            MakeCardinality(a_history.Children.Cardinality)) ', _
                        'a_history.Children.Count)

                        an_event = a_history.Children.Item(0)
                        xmlEvent = mAomFactory.MakeComplexObject( _
                            an_attribute, _
                            ReferenceModel.RM_StructureName(StructureType.Event), _
                            an_event.NodeId, _
                            MakeOccurrences(an_event.Occurrences))

                        Select Case an_event.EventType
                            Case RmEvent.ObservationEventType.PointInTime
                                If an_event.hasFixedOffset Then
                                    Dim durationConstraint As New Constraint_Duration

                                    an_attribute = mAomFactory.MakeSingleAttribute(xmlEvent, "offset")
                                    durationConstraint.MinMaxValueUnits = an_event.OffsetUnits
                                    'Set max and min to offset value
                                    durationConstraint.MinimumValue = an_event.Offset
                                    durationConstraint.HasMinimum = True
                                    durationConstraint.MaximumValue = an_event.Offset
                                    durationConstraint.HasMaximum = True
                                    BuildDuration(an_attribute, durationConstraint)
                                End If
                            Case RmEvent.ObservationEventType.Interval

                                If an_event.AggregateMathFunction <> "" Then
                                    an_attribute = mAomFactory.MakeSingleAttribute(xmlEvent, "math_function")
                                    Dim a_code_phrase As CodePhrase = New CodePhrase
                                    a_code_phrase.FirstCode = an_event.AggregateMathFunction
                                    a_code_phrase.TerminologyID = "openehr"
                                    BuildCodedText(an_attribute, a_code_phrase)
                                End If

                                If an_event.hasFixedDuration Then
                                    Dim durationConstraint As New Constraint_Duration

                                    an_attribute = mAomFactory.MakeSingleAttribute(xmlEvent, "width")
                                    durationConstraint.MinMaxValueUnits = an_event.WidthUnits
                                    'Set max and min to offset value
                                    durationConstraint.MinimumValue = an_event.Width
                                    durationConstraint.HasMinimum = True
                                    durationConstraint.MaximumValue = an_event.Width
                                    durationConstraint.HasMaximum = True
                                    BuildDuration(an_attribute, durationConstraint)
                                End If
                        End Select

                        ' runtime name
                        If an_event.HasNameConstraint Then
                            an_attribute = mAomFactory.MakeSingleAttribute(xmlEvent, "name")
                            BuildText(an_attribute, an_event.NameConstraint)
                        End If

                        ' data
                        an_attribute = mAomFactory.MakeSingleAttribute(xmlEvent, "data")
                        Dim objNode As XMLParser.C_COMPLEX_OBJECT

                        objNode = mAomFactory.MakeComplexObject( _
                            an_attribute, _
                            ReferenceModel.RM_StructureName(rm.Type), _
                            rm.NodeId)

                        BuildStructure(rm, objNode)

                        Exit Sub

                    End If ' at least one child
                End If
            Next

        End Sub

        Private Sub BuildHistory(ByVal a_history As RmHistory, ByRef RelNode As XMLParser.C_ATTRIBUTE, ByVal rmState As RmStructureCompound)
            Dim events As Object()
            Dim history_event As XMLParser.C_COMPLEX_OBJECT
            Dim an_attribute As XMLParser.C_ATTRIBUTE

            events = BuildHistory(a_history, RelNode)

            Dim a_rm As RmStructureCompound

            a_rm = rmState.Children.items(0)


            If events.Length > 0 AndAlso Not a_rm Is Nothing Then
                Dim path As String = "?"
                For i As Integer = 0 To events.Length - 1
                    history_event = CType(events(i), XMLParser.C_COMPLEX_OBJECT)
                    an_attribute = mAomFactory.MakeSingleAttribute(history_event, "state")

                    'First event has the structure
                    If i = 0 Then
                        Dim objNode As XMLParser.C_COMPLEX_OBJECT

                        objNode = mAomFactory.MakeComplexObject(an_attribute, ReferenceModel.RM_StructureName(a_rm.Type), a_rm.NodeId)
                        BuildStructure(a_rm, objNode)
                        path = Me.GetPathOfNode(a_rm.NodeId)
                    Else
                        'create a reference
                        Dim ref_xmlRefNode As XMLParser.ARCHETYPE_INTERNAL_REF
                        If Not path = "?" Then
                            ref_xmlRefNode = mAomFactory.MakeArchetypeRef(an_attribute, ReferenceModel.RM_StructureName(a_rm.Type), path)
                        Else
                            Debug.Assert(False, "Error with path")
                        End If
                    End If
                Next
            End If
        End Sub

        Private Function BuildHistory(ByVal a_history As RmHistory, ByRef RelNode As XMLParser.C_ATTRIBUTE) As Object()
            Dim xmlHistory, xmlEvent As XMLParser.C_COMPLEX_OBJECT
            Dim an_attribute As XMLParser.C_ATTRIBUTE
            Dim events_rel_node As New XMLParser.C_MULTIPLE_ATTRIBUTE()
            Dim an_event As RmEvent
            Dim data_processed As Boolean
            Dim data_path As String = ""
            Dim array_list_events As New ArrayList


            xmlHistory = mAomFactory.MakeComplexObject( _
                RelNode, _
                StructureType.History.ToString.ToUpper(System.Globalization.CultureInfo.InvariantCulture), _
                a_history.NodeId, _
                MakeOccurrences(a_history.Occurrences))

            If a_history.HasNameConstraint Then
                an_attribute = New XMLParser.C_SINGLE_ATTRIBUTE()
                an_attribute.rm_attribute_name = "name"
                BuildText(an_attribute, a_history.NameConstraint)
            End If

            If a_history.isPeriodic Then
                Dim durationConstraint As New Constraint_Duration

                an_attribute = mAomFactory.MakeSingleAttribute(xmlHistory, "period")
                durationConstraint.MinMaxValueUnits = a_history.PeriodUnits
                'Set max and min to offset value
                durationConstraint.MinimumValue = a_history.Period
                durationConstraint.HasMinimum = True
                durationConstraint.MaximumValue = a_history.Period
                durationConstraint.HasMaximum = True
                BuildDuration(an_attribute, durationConstraint)
            End If

            ' now build the events

            events_rel_node = mAomFactory.MakeMultipleAttribute( _
                xmlHistory, _
                "events", _
                MakeCardinality(a_history.Children.Cardinality)) ', _
            'a_history.Children.Count)

            For i As Integer = 0 To a_history.Children.Count - 1
                an_event = a_history.Children.Item(i)
                xmlEvent = mAomFactory.MakeComplexObject( _
                    ReferenceModel.RM_StructureName(an_event.Type), _
                    an_event.NodeId, _
                    MakeOccurrences(an_event.Occurrences))

                ' add to the array list to return from function
                array_list_events.Add(xmlEvent)

                'Add the object to the attribute
                mAomFactory.add_object(events_rel_node, xmlEvent)

                Select Case an_event.Type
                    Case StructureType.Event
                        ' do nothing...
                    Case StructureType.PointEvent
                        If an_event.hasFixedOffset Then
                            Dim durationConstraint As New Constraint_Duration

                            an_attribute = mAomFactory.MakeSingleAttribute(xmlEvent, "offset")
                            durationConstraint.MinMaxValueUnits = an_event.OffsetUnits
                            'Set max and min to offset value
                            durationConstraint.MinimumValue = an_event.Offset
                            durationConstraint.HasMinimum = True
                            durationConstraint.MaximumValue = an_event.Offset
                            durationConstraint.HasMaximum = True
                            BuildDuration(an_attribute, durationConstraint)
                        End If
                    Case StructureType.IntervalEvent

                        If an_event.AggregateMathFunction <> "" Then
                            an_attribute = mAomFactory.MakeSingleAttribute(xmlEvent, "math_function")
                            Dim a_code_phrase As CodePhrase = New CodePhrase
                            a_code_phrase.FirstCode = an_event.AggregateMathFunction
                            a_code_phrase.TerminologyID = "openehr"
                            BuildCodedText(an_attribute, a_code_phrase)
                        End If

                        If an_event.hasFixedDuration Then
                            Dim durationConstraint As New Constraint_Duration

                            an_attribute = mAomFactory.MakeSingleAttribute(xmlEvent, "width")
                            durationConstraint.MinMaxValueUnits = an_event.WidthUnits
                            'Set max and min to offset value
                            durationConstraint.MinimumValue = an_event.Width
                            durationConstraint.HasMinimum = True
                            durationConstraint.MaximumValue = an_event.Width
                            durationConstraint.HasMaximum = True
                            BuildDuration(an_attribute, durationConstraint)
                        End If
                End Select

                ' runtime name
                If an_event.HasNameConstraint Then
                    an_attribute = mAomFactory.MakeSingleAttribute(xmlEvent, "name")
                    BuildText(an_attribute, an_event.NameConstraint)
                End If

                ' data
                an_attribute = mAomFactory.MakeSingleAttribute(xmlEvent, "data")
                If Not data_processed Then
                    If Not a_history.Data Is Nothing Then
                        Dim objNode As XMLParser.C_COMPLEX_OBJECT

                        objNode = mAomFactory.MakeComplexObject( _
                            an_attribute, _
                            ReferenceModel.RM_StructureName(a_history.Data.Type), _
                            a_history.Data.NodeId)

                        BuildStructure(a_history.Data, objNode)

                        data_path = GetPathOfNode(a_history.Data.NodeId)
                    End If
                    data_processed = True
                Else
                    mAomFactory.MakeArchetypeRef(an_attribute, ReferenceModel.RM_StructureName(a_history.Data.Type), data_path)
                End If
            Next

            Return array_list_events.ToArray()
        End Function

        Private Sub BuildCluster(ByVal Cluster As RmCluster, ByRef RelNode As XMLParser.C_ATTRIBUTE)
            Dim cluster_xmlObj As XMLParser.C_COMPLEX_OBJECT
            Dim an_attribute As XMLParser.C_ATTRIBUTE
            Dim rm As RmStructure

            cluster_xmlObj = mAomFactory.MakeComplexObject( _
                RelNode, _
                ReferenceModel.RM_StructureName(StructureType.Cluster), _
                Cluster.NodeId, _
                MakeOccurrences(Cluster.Occurrences))

            If Cluster.HasNameConstraint Then
                an_attribute = mAomFactory.MakeSingleAttribute(cluster_xmlObj, "name")
                BuildText(an_attribute, Cluster.NameConstraint)
            End If

            If Cluster.Children.Count > 0 Then
                an_attribute = mAomFactory.MakeMultipleAttribute( _
                    cluster_xmlObj, _
                    "items", _
                    MakeCardinality(Cluster.Children.Cardinality, Cluster.Children.Cardinality.Ordered)) ', _
                'Cluster.Children.Count)

                For Each rm In Cluster.Children.items
                    If rm.Type = StructureType.Cluster Then
                        BuildCluster(rm, an_attribute)
                    ElseIf rm.Type = StructureType.Element Or rm.Type = StructureType.Reference Then
                        BuildElementOrReference(rm, an_attribute)
                    ElseIf rm.Type = StructureType.Slot Then
                        BuildSlot(an_attribute, rm)
                    Else
                        Debug.Assert(False, "Type not handled")
                    End If
                Next
            Else
                cluster_xmlObj.any_allowed = True
            End If
        End Sub

        Protected Sub BuildRootCluster(ByVal Cluster As RmCluster, ByVal xmlObj As XMLParser.C_COMPLEX_OBJECT)
            ' Build a section, runtimename is already done
            Dim an_attribute As XMLParser.C_ATTRIBUTE

            ' CadlObj.SetObjectId(openehr.base.kernel.Create.STRING.make_from_cil(Rm.NodeId))

            If Cluster.Children.Count > 0 Then
                an_attribute = mAomFactory.MakeMultipleAttribute(xmlObj, "items", MakeCardinality(Cluster.Children.Cardinality, Cluster.Children.Cardinality.Ordered))
                For Each Rm As RmStructure In Cluster.Children.items
                    If Rm.Type = StructureType.Cluster Then
                        BuildCluster(Rm, an_attribute)
                    ElseIf Rm.Type = StructureType.Element Or Rm.Type = StructureType.Reference Then
                        BuildElementOrReference(Rm, an_attribute)
                    ElseIf Rm.Type = StructureType.Slot Then
                        BuildSlot(an_attribute, Rm)
                    Else
                        Debug.Assert(False, "Type not handled")
                    End If
                Next
            End If

            If ReferencesToResolve.Count > 0 Then
                Dim ref_xmlRefNode As XMLParser.ARCHETYPE_INTERNAL_REF
                Dim path As String

                For Each ref As ReferenceToResolve In ReferencesToResolve

                    path = GetPathOfNode(ref.Element.NodeId)
                    If Not path Is Nothing Then
                        ref_xmlRefNode = mAomFactory.MakeArchetypeRef(ref.Attribute, "ELEMENT", path)
                        ref_xmlRefNode.occurrences = MakeOccurrences(ref.Element.Occurrences)
                    Else
                        'reference element no longer exists so build it as an element
                        Dim new_element As RmElement = ref.Element.Copy()
                        BuildElementOrReference(new_element, ref.Attribute)
                    End If

                Next
                ReferencesToResolve.Clear()
            End If

        End Sub

        Protected Sub BuildRootElement(ByVal an_element As RmElement, ByVal xmlObj As XMLParser.C_COMPLEX_OBJECT)
            ' Build a element

            If an_element.HasNameConstraint Then
                Dim an_attribute As XMLParser.C_ATTRIBUTE

                an_attribute = mAomFactory.MakeSingleAttribute(xmlObj, "name")
                BuildText(an_attribute, an_element.NameConstraint)
            End If

            If an_element.Constraint.Type = ConstraintType.Any Then
                If xmlObj.attributes Is Nothing OrElse xmlObj.attributes.Length = 0 Then
                    xmlObj.any_allowed = True
                End If
            Else
                Dim value_attribute As XMLParser.C_ATTRIBUTE

                value_attribute = mAomFactory.MakeSingleAttribute(xmlObj, "value")
                BuildElementConstraint(xmlObj, value_attribute, an_element.Constraint)
            End If

        End Sub

        Private Sub BuildProportion(ByVal value_attribute As XMLParser.C_ATTRIBUTE, ByVal cp As Constraint_Proportion)
            Dim RatioObject As XMLParser.C_COMPLEX_OBJECT
            Dim fraction_attribute As XMLParser.C_ATTRIBUTE

            RatioObject = mAomFactory.MakeComplexObject(value_attribute, ReferenceModel.RM_DataTypeName(cp.Type))

            If cp.Numerator.HasMaximum Or cp.Numerator.HasMinimum Then
                fraction_attribute = mAomFactory.MakeSingleAttribute(RatioObject, "numerator")
                BuildReal(fraction_attribute, cp.Numerator)
            End If
            If cp.Denominator.HasMaximum Or cp.Denominator.HasMinimum Then
                fraction_attribute = mAomFactory.MakeSingleAttribute(RatioObject, "denominator")
                BuildReal(fraction_attribute, cp.Denominator)
            End If

            If cp.IsIntegralSet Then
                'There is a restriction on whether the instance will be integral or not
                fraction_attribute = mAomFactory.MakeSingleAttribute(RatioObject, "is_integral")
                Dim boolConstraint As New Constraint_Boolean
                If cp.IsIntegral Then
                    boolConstraint.TrueAllowed = True
                Else
                    boolConstraint.FalseAllowed = True
                End If
                BuildBoolean(fraction_attribute, boolConstraint)
            End If

            If Not cp.AllowAllTypes Then
                Dim integerConstraint As New XMLParser.C_INTEGER
                
                fraction_attribute = mAomFactory.MakeSingleAttribute(RatioObject, "type")

                Dim allowedTypes As New ArrayList

                For i As Integer = 0 To 4
                    If cp.IsTypeAllowed(i) Then
                        allowedTypes.Add(i.ToString)
                    End If
                Next

                integerConstraint.list = allowedTypes.ToArray(GetType(String))

                mAomFactory.MakePrimitiveObject(fraction_attribute, integerConstraint)
            End If

        End Sub

        Private Sub BuildReal(ByVal value_attribute As XMLParser.C_ATTRIBUTE, ByVal ct As Constraint_Count)
            Dim magnitude As XMLParser.C_PRIMITIVE_OBJECT
            Dim cReal As New XMLParser.C_REAL

            If ct.HasMaximum Or ct.HasMinimum Then
                cReal.range = New XMLParser.interval_of_real
            End If

            If ct.HasMaximum And ct.HasMinimum Then
                cReal.range.minimum = ct.MinimumValue
                cReal.range.minimumSpecified = True
                cReal.range.maximum = ct.MaximumValue
                cReal.range.maximumSpecified = True
                cReal.range.includes_minimum = ct.IncludeMinimum
                cReal.range.includes_maximum = ct.IncludeMaximum
            ElseIf ct.HasMaximum Then
                cReal.range.maximum = ct.MaximumValue
                cReal.range.maximumSpecified = True
                cReal.range.includes_maximum = ct.IncludeMaximum
            ElseIf ct.HasMinimum Then
                cReal.range.minimum = ct.MinimumValue
                cReal.range.minimumSpecified = True
                cReal.range.includes_minimum = ct.IncludeMinimum
            End If
            If ct.HasAssumedValue Then
                cReal.assumed_valueSpecified = True
                cReal.assumed_value = ct.AssumedValue.ToString()
            End If

                magnitude = mAomFactory.MakePrimitiveObject(value_attribute, cReal)
        End Sub


        Private Sub BuildCount(ByVal value_attribute As XMLParser.C_ATTRIBUTE, ByVal ct As Constraint_Count)
            Dim an_attribute As XMLParser.C_ATTRIBUTE
            Dim xmlCount As XMLParser.C_COMPLEX_OBJECT
            Dim magnitude As XMLParser.C_PRIMITIVE_OBJECT

            xmlCount = mAomFactory.MakeComplexObject(value_attribute, ReferenceModel.RM_DataTypeName(ct.Type))

            If ct.HasMaximum Or ct.HasMinimum Then
                ' set the magnitude constraint
                an_attribute = mAomFactory.MakeSingleAttribute(xmlCount, "magnitude")
                Dim c_int As New XMLParser.C_INTEGER

                If ct.HasMaximum Or ct.HasMinimum Then
                    c_int.range = New XMLParser.interval_of_integer
                End If

                If ct.HasMaximum And ct.HasMinimum Then
                    c_int.range.minimum = ct.MinimumValue
                    c_int.range.maximum = ct.MaximumValue
                    c_int.range.includes_minimum = ct.IncludeMinimum
                    c_int.range.includes_maximum = ct.IncludeMaximum
                ElseIf ct.HasMaximum Then
                    c_int.range.maximum = ct.MaximumValue
                    c_int.range.includes_maximum = ct.IncludeMaximum
                ElseIf ct.HasMinimum Then
                    c_int.range.minimum = ct.MinimumValue
                    c_int.range.includes_minimum = ct.IncludeMinimum
                Else
                    Debug.Assert(False)
                    xmlCount.any_allowed = True
                    Return
                End If

                If ct.HasAssumedValue Then
                    c_int.assumed_value = ct.AssumedValue.ToString()
                End If

                magnitude = mAomFactory.MakePrimitiveObject(an_attribute, c_int)

            Else
                xmlCount.any_allowed = True
            End If
        End Sub

        Private Sub BuildDateTime(ByVal value_attribute As XMLParser.C_ATTRIBUTE, ByVal dt As Constraint_DateTime)

            Dim an_attribute As XMLParser.C_ATTRIBUTE
            Dim an_object As XMLParser.C_COMPLEX_OBJECT

            Dim cd As XMLParser.C_PRIMITIVE
            Dim xmlDateTime As New XMLParser.C_PRIMITIVE_OBJECT

            Select Case dt.TypeofDateTimeConstraint
                Case 11                 ' Allow all
                    Dim dtc As New XMLParser.C_DATE_TIME
                    dtc.pattern = "YYYY-??-??T??:??:??"
                    cd = dtc
                Case 12                 ' Full date time
                    Dim dtc As New XMLParser.C_DATE_TIME
                    dtc.pattern = "YYYY-MM-DDTHH:MM:SS"
                    cd = dtc
                Case 13                 'Partial Date time
                    Dim dtc As New XMLParser.C_DATE_TIME
                    dtc.pattern = "YYYY-MM-DDTHH:??:??"
                    cd = dtc
                Case 14                 'Date only
                    Dim dtc As New XMLParser.C_DATE
                    dtc.pattern = "YYYY-??-??"
                    cd = dtc
                Case 15                'Full date
                    Dim dtc As New XMLParser.C_DATE
                    dtc.pattern = "YYYY-MM-DD"
                    cd = dtc
                Case 16                'Partial date
                    Dim dtc As New XMLParser.C_DATE
                    dtc.pattern = "YYYY-??-XX"
                    cd = dtc
                Case 17                'Partial date with month
                    Dim dtc As New XMLParser.C_DATE
                    dtc.pattern = "YYYY-MM-??"
                    cd = dtc
                Case 18                'TimeOnly
                    Dim dtc As New XMLParser.C_TIME
                    dtc.pattern = "HH:??:??"
                    cd = dtc
                Case 19                 'Full time
                    Dim dtc As New XMLParser.C_TIME
                    dtc.pattern = "HH:MM:SS"
                    cd = dtc
                Case 20                'Partial time
                    Dim dtc As New XMLParser.C_TIME
                    dtc.pattern = "HH:??:XX"
                    cd = dtc
                Case 21                'Partial time with minutes
                    Dim dtc As New XMLParser.C_TIME
                    dtc.pattern = "HH:MM:??"
                    cd = dtc
                Case Else
                    Debug.Assert(False, "Not handled")
                    Return
            End Select

            Dim a_type As String = ""
            Select Case cd.GetType().ToString().ToLowerInvariant()
                Case "xmlparser.c_date_time"
                    a_type = "DV_DATE_TIME"
                Case "xmlparser.c_date"
                    a_type = "DV_DATE"
                Case "xmlparser.c_time"
                    a_type = "DV_TIME"
            End Select
            an_object = mAomFactory.MakeComplexObject(value_attribute, a_type)
            an_attribute = mAomFactory.MakeSingleAttribute(an_object, "value")
            mAomFactory.MakePrimitiveObject(an_attribute, cd)

        End Sub

        Private Sub BuildSlot(ByVal value_attribute As XMLParser.C_ATTRIBUTE, ByVal a_slot As RmSlot)
            BuildSlot(value_attribute, a_slot.SlotConstraint, a_slot.Occurrences)
        End Sub

        Private Sub BuildSlot(ByVal value_attribute As XMLParser.C_ATTRIBUTE, ByVal sl As Constraint_Slot, ByVal an_occurrence As RmCardinality)
            Dim slot As XMLParser.ARCHETYPE_SLOT

            slot = New XMLParser.ARCHETYPE_SLOT

            slot.rm_type_name = ReferenceModel.RM_StructureName(sl.RM_ClassType)
            slot.occurrences = MakeOccurrences(an_occurrence)

            If sl.hasSlots Then
                If sl.IncludeAll Then
                    mAomFactory.AddIncludeToSlot(slot, MakeAssertion("archetype_id/value", ".*"))
                Else
                    For Each s As String In sl.Include
                        Dim escapedString As String
                        Dim i As Integer
                        'Must have at least one escaped . or it is not valid unless it is the end
                        i = s.IndexOf("\")
                        If i > -1 AndAlso i <> (s.Length - 1) Then
                            escapedString = s
                        Else
                            escapedString = s.Replace(".", "\.")
                        End If
                        mAomFactory.AddIncludeToSlot(slot, MakeAssertion("archetype_id/value", escapedString))
                    Next
                    For Each s As String In sl.Include

                    Next
                End If
                If sl.ExcludeAll Then
                    mAomFactory.AddExcludeToSlot(slot, MakeAssertion("archetype_id/value", ".*"))
                Else
                    For Each s As String In sl.Exclude
                        Dim escapedString As String
                        Dim i As Integer
                        'Must have at least one escaped . or it is not valid unless it is the end
                        i = s.IndexOf("\")
                        If i > -1 AndAlso i <> (s.Length - 1) Then
                            escapedString = s
                        Else
                            escapedString = s.Replace(".", "\.")
                        End If
                        mAomFactory.AddExcludeToSlot(slot, MakeAssertion("archetype_id/value", escapedString))
                    Next
                End If
            Else
                mAomFactory.AddIncludeToSlot(slot, MakeAssertion("archetype_id/value", ".*"))
            End If

            mAomFactory.add_object(value_attribute, slot)

        End Sub

        Private Sub BuildDuration(ByVal value_attribute As XMLParser.C_ATTRIBUTE, ByVal c As Constraint_Duration)

            Dim an_object As XMLParser.C_COMPLEX_OBJECT = mAomFactory.MakeComplexObject(value_attribute, ReferenceModel.RM_DataTypeName(c.Type))
            Dim an_attribute As XMLParser.C_SINGLE_ATTRIBUTE = mAomFactory.MakeSingleAttribute(an_object, "value")

            Dim objNode As XMLParser.C_PRIMITIVE_OBJECT
            Dim d As New XMLParser.C_DURATION

            Dim durationISO As New Duration()

            If c.HasMaximum Or c.HasMinimum Then
                d.range = New XMLParser.interval_of_duration()
                durationISO.ISO_Units = OceanArchetypeEditor.ISO_TimeUnits.GetOptimalIsoUnit(c.MinMaxValueUnits)

                If c.HasMaximum And c.HasMinimum Then
                    durationISO.GUI_duration = CInt(c.MaximumValue)
                    d.range.maximum = durationISO.ISO_duration
                    durationISO.GUI_duration = CInt(c.MinimumValue)
                    d.range.minimum = durationISO.ISO_duration
                    d.range.includes_maximum = c.IncludeMaximum
                    d.range.includes_minimum = c.IncludeMinimum
                ElseIf c.HasMinimum Then
                    durationISO.GUI_duration = CInt(c.MinimumValue)
                    d.range.minimum = durationISO.ISO_duration
                    d.range.includes_minimum = c.IncludeMinimum
                Else 'Has maximum
                    durationISO.GUI_duration = CInt(c.MaximumValue)
                    d.range.maximum = durationISO.ISO_duration
                    d.range.includes_maximum = c.IncludeMaximum
                End If
            Else
                d.pattern = c.AllowableUnits
            End If

            objNode = mAomFactory.MakePrimitiveObject(an_attribute, d)

        End Sub

        Private Sub BuildQuantity(ByVal value_attribute As XMLParser.C_ATTRIBUTE, ByVal q As Constraint_Quantity)
            Dim cQuantity As New XMLParser.C_DV_QUANTITY
            cQuantity.rm_type_name = "QUANTITY"

            mAomFactory.add_object(value_attribute, cQuantity)

            ' set the property constraint - it should be present

            If Not q.IsNull Then

                Dim cp As New XMLParser.CODE_PHRASE

                Debug.Assert(q.IsCoded)

                cp.code_string = q.OpenEhrCode
                cp.terminology_id = "openehr"

                cQuantity.property = cp

                If q.has_units Then
                    Dim unit_constraint As Constraint_QuantityUnit
                    Dim cUnit As XMLParser.C_QUANTITY_ITEM

                    cQuantity.list = Array.CreateInstance(GetType(XMLParser.C_QUANTITY_ITEM), q.Units.Count)

                    For i As Integer = 1 To q.Units.Count
                        unit_constraint = q.Units(i)
                        Dim a_real As XMLParser.interval_of_real = Nothing
                        cUnit = New XMLParser.C_QUANTITY_ITEM

                        cUnit.units = unit_constraint.Unit

                        If unit_constraint.HasMaximum Or unit_constraint.HasMinimum Then
                            a_real = New XMLParser.interval_of_real
                            'a_real.has_maximum = unit_constraint.HasMaximum
                            'a_real.has_minimum = unit_constraint.HasMinimum
                            If unit_constraint.HasMaximum And unit_constraint.HasMinimum Then
                                a_real.minimum = unit_constraint.MinimumValue
                                a_real.minimumSpecified = True
                                a_real.maximum = unit_constraint.MaximumValue
                                a_real.maximumSpecified = True
                                If unit_constraint.IncludeMinimum = False Then
                                    a_real.includes_minimum = unit_constraint.IncludeMinimum
                                End If
                                If unit_constraint.IncludeMaximum = False Then
                                    a_real.includes_maximum = unit_constraint.IncludeMaximum
                                End If
                            ElseIf unit_constraint.HasMaximum Then
                                a_real.maximum = unit_constraint.MaximumValue
                                a_real.maximumSpecified = True
                                a_real.includes_maximum = unit_constraint.IncludeMaximum
                            ElseIf unit_constraint.HasMinimum Then
                                a_real.minimum = unit_constraint.MinimumValue
                                a_real.minimumSpecified = True
                                a_real.includes_minimum = unit_constraint.IncludeMinimum
                            End If
                        End If

                        If Not a_real Is Nothing Then
                            cUnit.magnitude = a_real
                        End If

                        If unit_constraint.Precision > -1 Then
                            cUnit.precision = unit_constraint.Precision
                        End If

                        If unit_constraint.HasAssumedValue Then
                            cUnit.assumed_value = unit_constraint.AssumedValue
                            cUnit.assumed_valueSpecified = True
                        End If
                        'vb collection is base 1, cQuantity.list is base 0
                        cQuantity.list(i - 1) = cUnit
                    Next
                End If

            Else
                cQuantity.any_allowed = True
            End If

        End Sub

        Private Sub BuildBoolean(ByVal value_attribute As XMLParser.C_ATTRIBUTE, ByVal b As Constraint_Boolean)

            Dim an_object As XMLParser.C_COMPLEX_OBJECT = mAomFactory.MakeComplexObject(value_attribute, ReferenceModel.RM_DataTypeName(b.Type))
            Dim an_attribute As XMLParser.C_SINGLE_ATTRIBUTE = mAomFactory.MakeSingleAttribute(an_object, "value")

            Dim c_value As XMLParser.C_PRIMITIVE_OBJECT
            Dim c_bool As New XMLParser.C_BOOLEAN


            If b.TrueFalseAllowed Then
                c_bool.false_valid = True
                c_bool.true_valid = True
                c_value = mAomFactory.MakePrimitiveObject(an_attribute, c_bool)
            ElseIf b.TrueAllowed Then
                c_bool.false_valid = False
                c_bool.true_valid = True
                c_value = mAomFactory.MakePrimitiveObject(an_attribute, c_bool)
            ElseIf b.FalseAllowed Then
                c_bool.false_valid = True
                c_bool.true_valid = False
                c_value = mAomFactory.MakePrimitiveObject(an_attribute, c_bool)
            End If

            If b.hasAssumedValue Then
                c_bool.assumed_value = b.AssumedValue
                c_bool.assumed_valueSpecified = True
            End If

        End Sub

        Private Sub BuildOrdinal(ByVal value_attribute As XMLParser.C_ATTRIBUTE, ByVal o As Constraint_Ordinal)
            Dim c_value As XMLParser.C_DV_ORDINAL
            Dim o_v As OrdinalValue

            c_value = New XMLParser.C_DV_ORDINAL()
            c_value.rm_type_name = "ORDINAL"
            c_value.list = Array.CreateInstance(GetType(XMLParser.ORDINAL), o.OrdinalValues.Count)

            If o.OrdinalValues.Count > 0 Then
                Dim i As Integer = 0
                For Each o_v In o.OrdinalValues
                    'SRH: Added as empty rows still give a count of 1
                    If o_v.InternalCode <> Nothing Then
                        Dim xmlO As New XMLParser.ORDINAL
                        xmlO.value = o_v.Ordinal.ToString()
                        xmlO.symbol = New XMLParser.CODE_PHRASE
                        xmlO.symbol.code_string = o_v.InternalCode
                        xmlO.symbol.terminology_id = "local"
                        c_value.list(i) = xmlO
                        i += 1
                    End If
                Next
            End If

            If o.HasAssumedValue Then
                c_value.assumed_value = CStr(o.AssumedValue)
            End If

            mAomFactory.add_object(value_attribute, c_value)

        End Sub

        Private Sub BuildText(ByVal value_attribute As XMLParser.C_ATTRIBUTE, ByVal t As Constraint_Text)

            Select Case t.TypeOfTextConstraint
                Case TextConstrainType.Terminology
                    If t.ConstraintCode <> "" Then
                        BuildCodedText(value_attribute, t.ConstraintCode)
                    End If
                Case TextConstrainType.Internal
                    BuildCodedText(value_attribute, t.AllowableValues, t.AssumedValue)
                Case TextConstrainType.Text
                    BuildPlainText(value_attribute, t.AllowableValues.Codes)
            End Select
        End Sub

        Protected Function GetPathOfNode(ByVal NodeId As String) As String
            Dim an_arraylist As System.Collections.ArrayList
            Dim s As String

            an_arraylist = mArchetypeParser.PhysicalPaths()

            For Each s In an_arraylist
                If s.EndsWith(NodeId & "]") Then
                    Return s
                End If
            Next
            Debug.Assert(False, "Should be a path for every node")
            Return ""
        End Function

        Private Sub BuildInterval(ByVal value_attribute As XMLParser.C_ATTRIBUTE, ByVal c As Constraint_Interval)

            Dim objNode As XMLParser.C_COMPLEX_OBJECT

            objNode = mAomFactory.MakeComplexObject(value_attribute, ReferenceModel.RM_DataTypeName(c.Type))

            'Upper of type T
            Dim an_attribute As XMLParser.C_ATTRIBUTE
            an_attribute = mAomFactory.MakeSingleAttribute(objNode, "upper")
            BuildElementConstraint(objNode, an_attribute, c.UpperLimit)

            'Lower of type T
            an_attribute = mAomFactory.MakeSingleAttribute(objNode, "lower")
            BuildElementConstraint(objNode, an_attribute, c.LowerLimit)
        End Sub

        Private Sub BuildMultiMedia(ByVal value_attribute As XMLParser.C_ATTRIBUTE, ByVal c As Constraint_MultiMedia)
            Dim objNode As XMLParser.C_COMPLEX_OBJECT
            Dim code_rel_node As XMLParser.C_ATTRIBUTE
            Dim ca_Term As XMLParser.C_CODE_PHRASE

            objNode = mAomFactory.MakeComplexObject(value_attribute, ReferenceModel.RM_DataTypeName(c.Type))

            code_rel_node = mAomFactory.MakeSingleAttribute(objNode, "media_type")
            ca_Term = New XMLParser.C_CODE_PHRASE
            ca_Term.rm_type_name = "CODE_PHRASE"
            ca_Term.terminology = c.AllowableValues.TerminologyID

            If c.AllowableValues.Codes.Count > 0 Then
                ca_Term.code_list = Array.CreateInstance(GetType(String), c.AllowableValues.Codes.Count)
                c.AllowableValues.Codes.CopyTo(ca_Term.code_list, 0)
            End If

            mAomFactory.add_object(code_rel_node, ca_Term)

        End Sub

        Private Sub BuildURI(ByVal value_attribute As XMLParser.C_ATTRIBUTE, ByVal c As Constraint_URI)
            Dim objNode As XMLParser.C_COMPLEX_OBJECT

            objNode = mAomFactory.MakeComplexObject(value_attribute, ReferenceModel.RM_DataTypeName(c.Type))
            objNode.any_allowed = True
        End Sub

        Private Sub BuildElementConstraint(ByVal parent As XMLParser.C_COMPLEX_OBJECT, ByVal value_attribute As XMLParser.C_ATTRIBUTE, ByVal c As Constraint)

            ' cannot have a value with no constraint on datatype
            Debug.Assert(c.Type <> ConstraintType.Any)

            Select Case c.Type
                Case ConstraintType.Quantity
                    BuildQuantity(value_attribute, c)

                Case ConstraintType.Boolean
                    BuildBoolean(value_attribute, c)

                Case ConstraintType.Text
                    BuildText(value_attribute, c)

                Case ConstraintType.Ordinal
                    BuildOrdinal(value_attribute, c)

                Case ConstraintType.Any
                    parent.any_allowed = True

                Case ConstraintType.Proportion
                    BuildProportion(value_attribute, c)

                Case ConstraintType.Count
                    BuildCount(value_attribute, c)

                Case ConstraintType.DateTime
                    BuildDateTime(value_attribute, c)

                Case ConstraintType.Slot
                    BuildSlot(value_attribute, c, New RmCardinality)

                Case ConstraintType.Multiple
                    For Each a_constraint As Constraint In CType(c, Constraint_Choice).Constraints
                        BuildElementConstraint(parent, value_attribute, a_constraint)
                    Next

                Case ConstraintType.Interval_Count, ConstraintType.Interval_Quantity, ConstraintType.Interval_DateTime
                    BuildInterval(value_attribute, c)

                Case ConstraintType.MultiMedia
                    BuildMultiMedia(value_attribute, c)

                Case ConstraintType.URI
                    BuildURI(value_attribute, c)

                Case ConstraintType.Duration
                    BuildDuration(value_attribute, c)

            End Select

        End Sub
        Private Sub BuildElementOrReference(ByVal Element As RmElement, ByRef RelNode As XMLParser.C_ATTRIBUTE)
            Dim value_attribute As XMLParser.C_ATTRIBUTE

            If Element.Type = StructureType.Reference Then
                Dim ref As ReferenceToResolve

                ref.Element = Element
                ref.Attribute = RelNode

                ReferencesToResolve.Add(ref)

            Else
                Dim element_xmlObj As XMLParser.C_COMPLEX_OBJECT

                element_xmlObj = mAomFactory.MakeComplexObject(RelNode, _
                    ReferenceModel.RM_StructureName(StructureType.Element), _
                    Element.NodeId, _
                    MakeOccurrences(Element.Occurrences))

                If Element.HasNameConstraint Then
                    Dim an_attribute As XMLParser.C_ATTRIBUTE

                    an_attribute = mAomFactory.MakeSingleAttribute(element_xmlObj, "name")
                    BuildText(an_attribute, Element.NameConstraint)
                End If

                If Element.Constraint.Type = ConstraintType.Any Then
                    If element_xmlObj.attributes Is Nothing OrElse element_xmlObj.attributes.Length = 0 Then
                        element_xmlObj.any_allowed = True
                    End If
                Else
                    value_attribute = mAomFactory.MakeSingleAttribute(element_xmlObj, "value")
                    BuildElementConstraint(element_xmlObj, value_attribute, Element.Constraint)
                End If

            End If
        End Sub

        Private Sub BuildStructure(ByVal rmStruct As RmStructureCompound, ByRef objNode As XMLParser.C_COMPLEX_OBJECT)
            Dim an_attribute As XMLParser.C_ATTRIBUTE
            Dim rm As RmStructure

            ' preconditions
            Debug.Assert(rmStruct.NodeId <> "") ' anonymous

            ' now make sure there are some contents to the structure
            ' and if not set it to anyallowed
            If rmStruct.Children.Count > 0 Then
                Select Case rmStruct.Type '.TypeName
                    Case StructureType.Single ' "SINGLE"
                        rm = rmStruct.Children.items(0)
                        an_attribute = mAomFactory.MakeSingleAttribute(objNode, "item")
                        If rm.Type = StructureType.Element Or rm.Type = StructureType.Reference Then
                            BuildElementOrReference(rm, an_attribute)
                        ElseIf rm.Type = StructureType.Slot Then
                            BuildSlot(an_attribute, rm)
                        Else
                            Debug.Assert(False, "Type not handled")
                        End If
                    Case StructureType.List ' "LIST"
                        an_attribute = mAomFactory.MakeMultipleAttribute( _
                            objNode, _
                            "items", _
                            MakeCardinality(CType(rmStruct, RmStructureCompound).Children.Cardinality, CType(rmStruct, RmStructureCompound).Children.Cardinality.Ordered)) ', _
                        'CType(rmStruct, RmStructureCompound).Children.Count)

                        For Each rm In rmStruct.Children.items
                            If rm.Type = StructureType.Element Or rm.Type = StructureType.Reference Then
                                BuildElementOrReference(rm, an_attribute)
                            ElseIf rm.Type = StructureType.Slot Then
                                BuildSlot(an_attribute, rm)
                            Else
                                Debug.Assert(False, "Type not handled")
                            End If
                        Next
                    Case StructureType.Tree ' "TREE"
                        an_attribute = mAomFactory.MakeMultipleAttribute( _
                            objNode, _
                            "items", _
                            MakeCardinality(CType(rmStruct, RmStructureCompound).Children.Cardinality, CType(rmStruct, RmStructureCompound).Children.Cardinality.Ordered)) ', _
                        'CType(rmStruct, RmStructureCompound).Children.Count)

                        For Each rm In rmStruct.Children.items
                            If rm.Type = StructureType.Cluster Then
                                BuildCluster(rm, an_attribute)
                            ElseIf rm.Type = StructureType.Element Or rm.Type = StructureType.Reference Then
                                BuildElementOrReference(rm, an_attribute)
                            ElseIf rm.Type = StructureType.Slot Then
                                BuildSlot(an_attribute, rm)
                            Else
                                Debug.Assert(False, "Type not handled")
                            End If
                        Next
                    Case StructureType.Table ' "TABLE"
                        Dim table As RmTable
                        Dim b As New XMLParser.C_BOOLEAN

                        b.assumed_valueSpecified = False

                        table = CType(rmStruct, RmTable)
                        ' set is rotated
                        an_attribute = mAomFactory.MakeSingleAttribute(objNode, "rotated")
                        If table.isRotated Then
                            b.true_valid = True
                            b.false_valid = False
                        Else
                            b.false_valid = True
                            b.true_valid = False
                        End If

                        mAomFactory.MakePrimitiveObject(an_attribute, b)

                        ' set number of row if not one
                        If table.NumberKeyColumns > 0 Then
                            Dim rh As New XMLParser.C_INTEGER
                            rh.range = New XMLParser.interval_of_integer

                            an_attribute = mAomFactory.MakeSingleAttribute(objNode, "number_key_columns")
                            rh.range.includes_maximum = True
                            rh.range.includes_minimum = True
                            rh.range.maximum = table.NumberKeyColumns
                            rh.range.minimum = table.NumberKeyColumns
                            rh.list_openSpecified = False

                            mAomFactory.MakePrimitiveObject(an_attribute, rh)
                        End If


                        an_attribute = mAomFactory.MakeMultipleAttribute( _
                            objNode, _
                            "rows", _
                            MakeCardinality(New RmCardinality(rmStruct.Occurrences), True)) ', _
                        'CType(rmStruct.Children.items(0), RmCluster).Children.Count)


                        BuildCluster(rmStruct.Children.items(0), an_attribute)

                End Select
            Else
                objNode.any_allowed = True
            End If

            If ReferencesToResolve.Count > 0 Then
                Dim ref_xmlRefNode As XMLParser.ARCHETYPE_INTERNAL_REF
                Dim path As String

                For Each ref As ReferenceToResolve In ReferencesToResolve

                    path = GetPathOfNode(ref.Element.NodeId)
                    If Not path Is Nothing Then
                        ref_xmlRefNode = mAomFactory.MakeArchetypeRef(ref.Attribute, "ELEMENT", path)
                        ref_xmlRefNode.occurrences = MakeOccurrences(ref.Element.Occurrences)
                    Else
                        'reference element no longer exists so build it as an element
                        Dim new_element As RmElement = ref.Element.Copy()

                        BuildElementOrReference(new_element, ref.Attribute)
                    End If
                Next
                ReferencesToResolve.Clear()
            End If

        End Sub

        Private Sub BuildSubjectOfData(ByVal subject As RelatedParty, ByVal root_node As XMLParser.C_COMPLEX_OBJECT)
            If subject.Relationship.Codes.Count = 0 Then
                Return
            Else
                Dim objnode As XMLParser.C_COMPLEX_OBJECT
                Dim an_attribute As XMLParser.C_ATTRIBUTE
                Dim a_relationship As XMLParser.C_ATTRIBUTE

                an_attribute = mAomFactory.MakeSingleAttribute(root_node, "subject")
                objnode = mAomFactory.MakeComplexObject(an_attribute, "PARTY_RELATED")
                a_relationship = mAomFactory.MakeSingleAttribute(objnode, "relationship")
                BuildCodedText(a_relationship, subject.Relationship)
            End If
        End Sub

        Private Sub BuildSection(ByVal rmChildren As Children, ByVal xmlObj As XMLParser.C_COMPLEX_OBJECT)
            ' Build a section, runtimename is already done
            Dim an_attribute As XMLParser.C_ATTRIBUTE

            an_attribute = mAomFactory.MakeMultipleAttribute( _
                xmlObj, _
                "items", _
                MakeCardinality(rmChildren.Cardinality, rmChildren.Cardinality.Ordered)) ', _
            'rmChildren.Count)

            For Each a_structure As RmStructure In rmChildren

                If a_structure.Type = StructureType.SECTION Then
                    Dim new_section As XMLParser.C_COMPLEX_OBJECT

                    new_section = mAomFactory.MakeComplexObject( _
                    an_attribute, _
                    "SECTION", _
                    a_structure.NodeId, _
                    MakeOccurrences(a_structure.Occurrences))

                    If a_structure.HasNameConstraint Then
                        an_attribute = mAomFactory.MakeSingleAttribute(new_section, "name")
                        BuildText(an_attribute, a_structure.NameConstraint)
                    End If

                    If CType(a_structure, RmSection).Children.Count > 0 Then
                        BuildSection(CType(a_structure, RmSection).Children, new_section)
                    Else
                        new_section.any_allowed = True
                    End If
                ElseIf a_structure.Type = StructureType.Slot Then
                    BuildSlot(an_attribute, a_structure)
                Else
                    Debug.Assert(False)
                End If
            Next
        End Sub

        Private Sub BuildComposition(ByVal Rm As RmComposition, ByVal xmlObj As XMLParser.C_COMPLEX_OBJECT)
            Dim an_attribute As XMLParser.C_ATTRIBUTE

            ' set the category
            an_attribute = mAomFactory.MakeSingleAttribute(xmlObj, "category")
            Dim t As New Constraint_Text
            t.TypeOfTextConstraint = TextConstrainType.Terminology ' coded_text
            t.AllowableValues.TerminologyID = "openehr"

            If Rm.IsPersistent Then
                t.AllowableValues.Codes.Add("431") ' persistent
            Else
                t.AllowableValues.Codes.Add("433") ' event
            End If

            BuildCodedText(an_attribute, t.AllowableValues)

            ' Deal with the content and context
            If Rm.Data.Count > 0 Then

                For Each a_structure As RmStructure In Rm.Data
                    Select Case a_structure.Type
                        Case StructureType.List, StructureType.Single, StructureType.Table, StructureType.Tree

                            Dim new_structure As XMLParser.C_COMPLEX_OBJECT

                            an_attribute = mAomFactory.MakeSingleAttribute(xmlObj, "context")
                            new_structure = mAomFactory.MakeComplexObject(an_attribute, "EVENT_CONTEXT")
                            an_attribute = mAomFactory.MakeSingleAttribute(new_structure, "other_context")
                            new_structure = mAomFactory.MakeComplexObject(an_attribute, ReferenceModel.RM_StructureName(a_structure.Type), a_structure.NodeId)
                            BuildStructure(a_structure, new_structure)

                        Case StructureType.SECTION

                            If CType(a_structure, RmSection).Children.Count > 0 Then

                                an_attribute = mAomFactory.MakeSingleAttribute(xmlObj, "content")

                                For Each slot As RmSlot In CType(a_structure, RmSection).Children

                                    BuildSlot(an_attribute, slot)
                                Next

                            End If

                        Case Else
                            Debug.Assert(False)
                    End Select
                Next
            Else
                xmlObj.any_allowed = True
            End If
        End Sub

        Private Sub BuildRootSection(ByVal Rm As RmSection, ByVal xmlObj As XMLParser.C_COMPLEX_OBJECT)
            ' Build a section, runtimename is already done
            Dim an_attribute As XMLParser.C_ATTRIBUTE

            ' xmlObj.SetObjectId(Rm.NodeId))

            If Rm.Children.Count > 0 Then
                an_attribute = mAomFactory.MakeMultipleAttribute( _
                    xmlObj, _
                    "items", _
                    MakeCardinality(Rm.Children.Cardinality, Rm.Children.Cardinality.Ordered)) ', _
                'Rm.Children.Count)

                For Each a_structure As RmStructure In Rm.Children
                    If a_structure.Type = StructureType.SECTION Then
                        Dim new_section As XMLParser.C_COMPLEX_OBJECT

                        new_section = mAomFactory.MakeComplexObject( _
                            "SECTION", _
                            a_structure.NodeId, _
                            MakeOccurrences(a_structure.Occurrences))

                        If a_structure.HasNameConstraint Then
                            Dim another_attribute As XMLParser.C_ATTRIBUTE
                            another_attribute = mAomFactory.MakeSingleAttribute(new_section, "name")
                            BuildText(another_attribute, a_structure.NameConstraint)
                        End If

                        If CType(a_structure, RmSection).Children.Count > 0 Then
                            BuildSection(CType(a_structure, RmSection).Children, new_section)
                        Else
                            new_section.any_allowed = True
                        End If
                        mAomFactory.add_object(an_attribute, new_section)
                    ElseIf a_structure.Type = StructureType.Slot Then
                        BuildSlot(an_attribute, a_structure)
                    Else
                        Debug.Assert(False)
                    End If
                Next
            Else
                xmlObj.any_allowed = True
            End If
        End Sub

        Private Sub BuildStructure(ByVal rm As RmStructureCompound, _
                ByVal an_adlArchetype As XMLParser.C_COMPLEX_OBJECT, _
                ByVal attribute_name As String)
            Dim an_attribute As XMLParser.C_ATTRIBUTE

            an_attribute = mAomFactory.MakeSingleAttribute(mXmlArchetype.definition, attribute_name)

            If CType(rm.Children.items(0), RmStructure).Type = StructureType.Slot Then
                BuildSlot(an_attribute, rm.Children.items(0))
            Else
                Dim objNode As XMLParser.C_COMPLEX_OBJECT

                objNode = mAomFactory.MakeComplexObject( _
                    an_attribute, _
                    ReferenceModel.RM_StructureName(rm.Children.items(0).Type), _
                    rm.Children.items(0).NodeId)

                BuildStructure(rm.Children.items(0), objNode)
            End If
        End Sub

        Private Sub BuildProtocol(ByVal rm As RmStructure, ByVal an_adlArchetype As XMLParser.C_COMPLEX_OBJECT)
            Dim an_attribute As XMLParser.C_ATTRIBUTE
            Dim rmStructComp As RmStructureCompound

            If rm.Type = StructureType.Slot Then
                an_attribute = mAomFactory.MakeSingleAttribute(mXmlArchetype.definition, "protocol")
                BuildSlot(an_attribute, rm)
            Else
                rmStructComp = CType(rm, RmStructureCompound)
                If rmStructComp.Children.Count > 0 Then
                    an_attribute = mAomFactory.MakeSingleAttribute(mXmlArchetype.definition, "protocol")
                    ' only 1 protocol allowed
                    Dim objNode As XMLParser.C_COMPLEX_OBJECT

                    objNode = mAomFactory.MakeComplexObject( _
                        an_attribute, _
                        ReferenceModel.RM_StructureName(rmStructComp.Children.items(0).Type), _
                        rmStructComp.Children.items(0).NodeId)

                    BuildStructure(rmStructComp.Children.items(0), objNode)
                End If
            End If

        End Sub

        Private Sub BuildWorkFlowStep(ByVal rm As RmPathwayStep, ByVal an_attribute As XMLParser.C_ATTRIBUTE)
            Dim a_state, a_step As XMLParser.C_ATTRIBUTE
            Dim objNode As XMLParser.C_COMPLEX_OBJECT
            Dim code_phrase As New CodePhrase

            objNode = mAomFactory.MakeComplexObject(an_attribute, "ISM_TRANSITION")
            a_state = mAomFactory.MakeSingleAttribute(objNode, "current_state")
            code_phrase.TerminologyID = "openehr"
            code_phrase.Codes.Add((CInt(rm.StateType)).ToString)
            If rm.HasAlternativeState Then
                code_phrase.Codes.Add(CInt(rm.AlternativeState).ToString)
            End If
            BuildCodedText(a_state, code_phrase)

            a_step = mAomFactory.MakeSingleAttribute(objNode, "careflow_step")
            code_phrase = New CodePhrase
            code_phrase.Codes.Add(rm.NodeId)  ' local is default terminology, node_id of rm is same as term code of name
            BuildCodedText(a_step, code_phrase)

        End Sub

        Private Sub BuildPathway(ByVal rm As RmStructureCompound, ByVal arch_def As XMLParser.C_COMPLEX_OBJECT)
            Dim an_attribute As XMLParser.C_ATTRIBUTE

            If rm.Children.Count > 0 Then
                an_attribute = mAomFactory.MakeSingleAttribute(mXmlArchetype.definition, "ism_transition")

                For Each pathway_step As RmPathwayStep In rm.Children
                    BuildWorkFlowStep(pathway_step, an_attribute)
                Next
            End If
        End Sub

        Private Sub BuildActivity(ByVal rm As RmActivity, ByVal an_attribute As XMLParser.C_ATTRIBUTE)
            Dim objNode As XMLParser.C_COMPLEX_OBJECT
            Dim objNodeSimple As XMLParser.C_PRIMITIVE_OBJECT

            objNode = mAomFactory.MakeComplexObject( _
                an_attribute, _
                "ACTIVITY", _
                rm.NodeId, _
                MakeOccurrences(rm.Occurrences))

            If rm.ArchetypeId <> "" Then
                an_attribute = mAomFactory.MakeSingleAttribute(objNode, "action_archetype_id")
                Dim c_s As New XMLParser.C_STRING
                c_s.pattern = "/" + rm.ArchetypeId + "/"
                objNodeSimple = mAomFactory.MakePrimitiveObject(an_attribute, c_s)
            End If

            For Each rm_struct As RmStructure In rm.Children
                an_attribute = mAomFactory.MakeMultipleAttribute(objNode, "description", MakeCardinality(rm.Children.Cardinality))
                Select Case rm_struct.Type
                    Case StructureType.List, StructureType.Single, StructureType.Tree, StructureType.Table
                        Dim EIF_struct As XMLParser.C_COMPLEX_OBJECT
                        EIF_struct = mAomFactory.MakeComplexObject(an_attribute, _
                            ReferenceModel.RM_StructureName(rm_struct.Type), _
                            rm_struct.NodeId)

                        BuildStructure(CType(rm_struct, RmStructureCompound), EIF_struct)

                    Case StructureType.Slot
                        ' this allows a structure to be archetyped at this point
                        Debug.Assert(CType(rm_struct, RmStructure).Type = StructureType.Slot)
                        BuildSlot(an_attribute, rm_struct)
                End Select
            Next

        End Sub

        Private Sub BuildInstruction(ByVal data As RmChildren)
            For Each rm As RmStructureCompound In data
                Select Case rm.Type
                    Case StructureType.Activities

                        'ToDo: Set cardinality on this attribute
                        Dim an_attribute As XMLParser.C_ATTRIBUTE
                        an_attribute = mAomFactory.MakeMultipleAttribute(mXmlArchetype.definition, _
                            "activities", _
                            MakeCardinality(New RmCardinality(0))) ', _
                        'rm.Children.Count)

                        ' only one activity allowed at present
                        Debug.Assert(rm.Children.Count < 2)

                        For Each activity As RmActivity In rm.Children
                            BuildActivity(activity, an_attribute)
                        Next
                    Case StructureType.Protocol
                        BuildProtocol(rm, mXmlArchetype.definition)
                    Case Else
                        Debug.Assert(False, rm.Type.ToString() & " - Type under INSTRUCTION not handled")
                End Select
            Next
        End Sub

        Private Sub BuildAction(ByVal rm As RmStructureCompound, ByVal a_definition As XMLParser.C_COMPLEX_OBJECT)
            Dim action_spec As RmStructure
            Dim an_attribute As XMLParser.C_ATTRIBUTE
            Dim objNode As XMLParser.C_COMPLEX_OBJECT

            If rm.Children.items.Length > 0 Then
                an_attribute = mAomFactory.MakeSingleAttribute(mXmlArchetype.definition, "description")
                action_spec = rm.Children.items(0)

                Select Case action_spec.Type
                    Case StructureType.Single, StructureType.List, StructureType.Tree, StructureType.Table
                        objNode = mAomFactory.MakeComplexObject(an_attribute, _
                            ReferenceModel.RM_StructureName(action_spec.Type), _
                            rm.Children.items(0).NodeId)

                        BuildStructure(action_spec, objNode)

                    Case StructureType.Slot
                        ' allows action to be specified in another archetype
                        Dim slot As RmSlot = CType(action_spec, RmSlot)

                        BuildSlot(an_attribute, slot)
                End Select
            End If
        End Sub

        Public Overridable Sub MakeParseTree()

            If Not mSynchronised Then
                Dim rm As RmStructureCompound
                Dim an_attribute As XMLParser.C_ATTRIBUTE

                'reset the ADL definition to make it again
                mXmlArchetype.definition.attributes = Nothing

                'pick up the description data
                If TypeOf mDescription Is ADL_Classes.ADL_Description Then
                    mXmlArchetype.description = New XML_Description(CType(mDescription, ADL_Classes.ADL_Description)).XML_Description
                Else
                    mXmlArchetype.description = (CType(mDescription, XML_Description).XML_Description)
                End If

                If Not mTranslationDetails Is Nothing AndAlso mTranslationDetails.Count > 0 Then
                    Dim xmlTranslationDetails As XMLParser.TRANSLATION_DETAILS() = Array.CreateInstance(GetType(XMLParser.TRANSLATION_DETAILS), mTranslationDetails.Count)
                    If TypeOf mTranslationDetails.Values(0) Is ADL_TranslationDetails Then
                        'Need to convert to XML
                        For i As Integer = 0 To mTranslationDetails.Count - 1
                            xmlTranslationDetails(i) = New XML_TranslationDetails(mTranslationDetails.Values(i)).XmlTranslation
                        Next
                    Else
                        For i As Integer = 0 To mTranslationDetails.Count - 1
                            xmlTranslationDetails(i) = CType(mTranslationDetails.Values(i), XML_TranslationDetails).XmlTranslation
                        Next

                    End If

                    mXmlArchetype.translations = xmlTranslationDetails
                End If

                If cDefinition Is Nothing Then
                    Err.Raise(vbObjectError + 512, "No archetype definition", _
                    "An archetype definition is required prior to saving")
                End If

                mAomFactory = New XMLParser.AomFactory()

                If cDefinition.hasNameConstraint Then
                    an_attribute = mAomFactory.MakeSingleAttribute(mXmlArchetype.definition, "name")
                    BuildText(an_attribute, cDefinition.NameConstraint)
                End If


                Debug.Assert(ReferenceModel.IsValidArchetypeDefinition(cDefinition.Type))

                Select Case cDefinition.Type

                    Case StructureType.Single, StructureType.List, StructureType.Tree, StructureType.Table
                        If mXmlArchetype.definition.any_allowed AndAlso CType(cDefinition, ArchetypeDefinition).Data.Count > 0 Then
                            'This can arise if the archetype has been saved with no children then
                            'items have been added later - this is percular to Tree, List and Table.
                            mXmlArchetype.definition.occurrences = MakeOccurrences(New RmCardinality(0))
                        End If
                        BuildStructure(cDefinition, mXmlArchetype.definition)

                    Case StructureType.Cluster
                        BuildRootCluster(cDefinition, mXmlArchetype.definition)

                    Case StructureType.Element
                        BuildRootElement(cDefinition, mXmlArchetype.definition)

                    Case StructureType.SECTION
                        BuildRootSection(cDefinition, mXmlArchetype.definition)

                    Case StructureType.COMPOSITION
                        BuildComposition(cDefinition, mXmlArchetype.definition)

                    Case StructureType.EVALUATION, StructureType.ENTRY

                        BuildSubjectOfData(CType(cDefinition, RmEntry).SubjectOfData, mXmlArchetype.definition)

                        For Each rm In CType(cDefinition, ArchetypeDefinition).Data
                            Select Case rm.Type
                                Case StructureType.State
                                    BuildStructure(rm, mXmlArchetype.definition, "state")

                                Case StructureType.Protocol
                                    BuildProtocol(rm, mXmlArchetype.definition)

                                Case StructureType.Data
                                    BuildStructure(rm, mXmlArchetype.definition, "data")

                            End Select
                        Next

                    Case StructureType.ADMIN_ENTRY

                        an_attribute = mAomFactory.MakeSingleAttribute(mXmlArchetype.definition, "data")
                        Try
                            Dim rm_struct As RmStructureCompound = CType(CType(cDefinition, ArchetypeDefinition).Data.items(0), RmStructureCompound).Children.items(0)

                            Dim objNode As XMLParser.C_COMPLEX_OBJECT
                            objNode = mAomFactory.MakeComplexObject(an_attribute, ReferenceModel.RM_StructureName(rm_struct.Type), rm_struct.NodeId)
                            BuildStructure(rm_struct, objNode)
                        Catch
                            'ToDo - process error
                            Debug.Assert(False, "Error building structure")
                        End Try

                    Case StructureType.OBSERVATION
                        BuildSubjectOfData(CType(cDefinition, RmEntry).SubjectOfData, mXmlArchetype.definition)

                        'Add state to each event so need to be sure of requirements
                        Dim state_to_be_added As Boolean = True
                        Dim rm_state As RmStructureCompound = Nothing
                        Dim rm_data As RmStructureCompound = Nothing
                        Dim rm_protocol As RmStructureCompound = Nothing

                        For Each rm In CType(cDefinition, ArchetypeDefinition).Data
                            Select Case rm.Type
                                'PROTOCOL
                                Case StructureType.Protocol
                                    rm_protocol = rm

                                    'DATA
                                Case StructureType.Data
                                    'remember the data structure
                                    rm_data = rm

                                    'STATE
                                Case StructureType.State

                                    'for the moment saving the state data on the first event EventSeries if there is one
                                    Dim a_rm As RmStructureCompound

                                    a_rm = rm.Children.items(0)

                                    If a_rm.Type = StructureType.History Then
                                        an_attribute = mAomFactory.MakeSingleAttribute(mXmlArchetype.definition, "state")

                                        ' can have EventSeries for each state
                                        BuildHistory(a_rm, an_attribute)
                                    Else
                                        rm_state = rm

                                    End If


                            End Select
                        Next

                        'Add the data
                        If Not rm_data Is Nothing Then
                            an_attribute = mAomFactory.MakeSingleAttribute(mXmlArchetype.definition, "data")

                            For Each a_rm As RmStructureCompound In rm_data.Children.items
                                Select Case a_rm.Type '.TypeName
                                    Case StructureType.History
                                        If Not rm_state Is Nothing Then
                                            BuildHistory(a_rm, an_attribute, rm_state)
                                        Else
                                            BuildHistory(a_rm, an_attribute)
                                        End If
                                    Case Else
                                        Debug.Assert(False) '?OBSOLETE
                                        Dim objNode As XMLParser.C_COMPLEX_OBJECT
                                        objNode = mAomFactory.MakeComplexObject(an_attribute, openehr.base.kernel.Create.STRING.make_from_cil(ReferenceModel.RM_StructureName(a_rm.Type)), a_rm.NodeId)
                                        BuildStructure(a_rm, objNode)
                                End Select
                            Next
                        End If

                        If Not rm_protocol Is Nothing Then
                            BuildProtocol(rm_protocol, mXmlArchetype.definition)
                        End If

                    Case StructureType.INSTRUCTION
                        BuildSubjectOfData(CType(cDefinition, RmEntry).SubjectOfData, mXmlArchetype.definition)

                        BuildInstruction(CType(cDefinition, ArchetypeDefinition).Data)

                    Case StructureType.ACTION
                        BuildSubjectOfData(CType(cDefinition, RmEntry).SubjectOfData, mXmlArchetype.definition)

                        For Each rm In CType(cDefinition, ArchetypeDefinition).Data
                            Select Case rm.Type
                                Case StructureType.ISM_TRANSITION
                                    BuildPathway(rm, mXmlArchetype.definition)
                                Case StructureType.ActivityDescription
                                    BuildAction(rm, mXmlArchetype.definition)
                                Case StructureType.Slot
                                    ' this allows a structure to be archetyped at this point
                                    Debug.Assert(CType(rm.Children.items(0), RmStructure).Type = StructureType.Slot)
                                    BuildStructure(rm, mXmlArchetype.definition)
                                Case StructureType.Protocol
                                    BuildProtocol(rm, mXmlArchetype.definition)
                            End Select
                        Next

                End Select
                mSynchronised = True
            End If
        End Sub

        Sub New(ByVal an_XML_Parser As XMLParser.XmlArchetypeParser, ByVal an_ArchetypeID As ArchetypeID, ByVal primary_language As String)
            ' call to create a brand new archetype
            MyBase.New(primary_language, an_ArchetypeID)

            mArchetypeParser = an_XML_Parser
            ' make the new archetype

            Try
                mArchetypeParser.NewArchetype(an_ArchetypeID.ToString, sPrimaryLanguageCode, OceanArchetypeEditor.DefaultLanguageCodeSet)
                mXmlArchetype = mArchetypeParser.Archetype
                mDescription = New XML_Description(mXmlArchetype.description, primary_language)
            Catch
                Debug.Assert(False)
                ''FIXME raise error
            End Try
        End Sub

        Sub New(ByVal a_parser As XMLParser.XmlArchetypeParser)
            ' Used in Export or SaveAs only
            MyBase.New(a_parser.Archetype.original_language.code_string)

            mXmlArchetype = a_parser.Archetype
            mArchetypeParser = a_parser
            mArchetypeID = New ArchetypeID(mXmlArchetype.archetype_id)
            
            ' get the parent ID
            If Not mXmlArchetype.parent_archetype_id Is Nothing Then
                sParentArchetypeID = mXmlArchetype.parent_archetype_id
            End If
            'this is the one
            mDescription = New XML_Description(mXmlArchetype.description, a_parser.Archetype.original_language.code_string)

            Select Case mArchetypeID.ReferenceModelEntity
                Case StructureType.COMPOSITION
                    cDefinition = New RmComposition()
                    cDefinition.RootNodeId = mXmlArchetype.concept_code
                Case StructureType.SECTION
                    cDefinition = New RmSection(mXmlArchetype.concept_code)
                Case StructureType.List, StructureType.Tree, StructureType.Single
                    cDefinition = New RmStructureCompound(mXmlArchetype.concept_code, mArchetypeID.ReferenceModelEntity)
                Case StructureType.Table
                    cDefinition = New RmTable(mXmlArchetype.concept_code)
                Case StructureType.ENTRY, StructureType.OBSERVATION, StructureType.EVALUATION, StructureType.INSTRUCTION, StructureType.ADMIN_ENTRY, StructureType.ACTION
                    cDefinition = New RmEntry(mArchetypeID.ReferenceModelEntity)
                    cDefinition.RootNodeId = mXmlArchetype.concept_code
                Case StructureType.Cluster
                    cDefinition = New RmCluster(mXmlArchetype.concept_code)
                Case StructureType.Element
                    cDefinition = New RmElement(mXmlArchetype.concept_code)
                Case Else
                    Debug.Assert(False)
            End Select

            sLifeCycle = mXmlArchetype.description.lifecycle_state

        End Sub

        Sub New(ByVal a_parser As XMLParser.XmlArchetypeParser, ByVal a_filemanager As FileManagerLocal)
            ' call to create an in memory archetype from the XDL parser
            MyBase.New(a_parser.Archetype.original_language.code_string)

            mXmlArchetype = a_parser.Archetype
            mArchetypeParser = a_parser
            mArchetypeID = New ArchetypeID(mXmlArchetype.archetype_id)
            ReferenceModel.SetArchetypedClass(mArchetypeID.ReferenceModelEntity)

            ' get the parent ID
            If Not mXmlArchetype.parent_archetype_id Is Nothing Then
                sParentArchetypeID = mXmlArchetype.parent_archetype_id
            End If

            'description and translation details
            mDescription = New XML_Description(mXmlArchetype.description, a_parser.Archetype.original_language.code_string)

            If Not mXmlArchetype.translations Is Nothing Then
                For Each t As XMLParser.TRANSLATION_DETAILS In mXmlArchetype.translations
                    mTranslationDetails.Add(t.language.code_string, New XML_TranslationDetails(t))
                Next
            End If

            Select Case mArchetypeID.ReferenceModelEntity
                Case StructureType.COMPOSITION
                    cDefinition = New XML_COMPOSITION(mXmlArchetype.definition, a_filemanager)
                Case StructureType.SECTION
                    cDefinition = New XML_SECTION(mXmlArchetype.definition, a_filemanager)
                Case StructureType.List, StructureType.Tree, StructureType.Single
                    cDefinition = New RmStructureCompound(mXmlArchetype.definition, a_filemanager)
                Case StructureType.Table
                    cDefinition = New RmTable(mXmlArchetype.definition, a_filemanager)
                Case StructureType.ENTRY, StructureType.OBSERVATION, StructureType.EVALUATION, StructureType.INSTRUCTION, StructureType.ADMIN_ENTRY, StructureType.ACTION
                    cDefinition = New XML_ENTRY(mXmlArchetype.definition, a_filemanager)
                Case StructureType.Cluster
                    cDefinition = New RmCluster(mXmlArchetype.definition, a_filemanager)
                Case StructureType.Element
                    cDefinition = New XML_RmElement(mXmlArchetype.definition, a_filemanager)
                Case Else
                    Debug.Assert(False)
            End Select

            sLifeCycle = mXmlArchetype.description.lifecycle_state

        End Sub

        Protected Sub New(ByVal primary_language As String)
            MyBase.New(primary_language)
            mDescription = New XML_Description(mXmlArchetype.description, primary_language)
        End Sub

    End Class
End Namespace



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
'The Original Code is XML_Archetype.vb.
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