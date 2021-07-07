
                        With Formset.Forms(Formset.FormPtr)
                            ValueInteger = Main.GetStreamInteger(RequestNameContentSelectID)
                            CreateContentFields = Main.GetStreamBoolean(RequestNameCreateContentFields)
                            '
                            .CreateContentFields = CreateContentFields
                            Select Case ValueInteger
                                Case ContentSelectUseNone
                                    DataContentID = ValueInteger
                                    .ContentID = 0
                                    .NewContentName = ""
                                    .UseAuthMemberContent = False
                                    .UseAuthOrgContent = False
                                    CreateContentFields = False
                                Case ContentSelectUseContentID
                                    ValueInteger = Main.GetStreamInteger(RequestNameContentID)
                                    If ValueInteger = 0 Then
                                        ProcessError = True
                                        Content = Content & "<div class=ccError>Select an existing data source.</div>"
                                    Else
                                        DataContentID = ValueInteger
                                        .ContentID = ValueInteger
                                        .NewContentName = ""
                                        .UseAuthMemberContent = False
                                        .UseAuthOrgContent = False
                                    End If
                                Case ContentSelectUseNewContentName
                                    ValueText = Main.GetStreamText(RequestNameNewContentName)
                                    If ValueText = "" Then
                                        ProcessError = True
                                        Content = Content & "<div class=ccError>Enter a new data source name.</div>"
                                    ElseIf (Main.GetRecordID("content", ValueText) <> 0) Then
                                        ProcessError = True
                                        Content = Content & "<div class=ccError>The name '" & ValueText & "' is taken. Select it from the existing data, or enter a different name.</div>"
                                    Else
                                        .ContentID = 0
                                        .NewContentName = ValueText
                                        .UseAuthMemberContent = False
                                        .UseAuthOrgContent = False
                                        '
                                        ' Create New Content from the new Content Name and the field values
                                        '
                                        DataContentName = ValueText
                                        DataTableName = "DynamicForm" & DataContentName
                                        DataTableName = Replace(DataTableName, " ", "")
                                        DataTableName = Replace(DataTableName, ",", "")
                                        DataTableName = Replace(DataTableName, ",", "")
                                        DataTableName = Replace(DataTableName, ";", "")
                                        DataTableName = Replace(DataTableName, """", "")
                                        DataTableName = Replace(DataTableName, "'", "")
                                        DataTableName = Replace(DataTableName, "/", "")
                                        DataTableName = Replace(DataTableName, "/", "")
                                        CS = Main.OpenCSContent("Content", "name=" & KmaEncodeSQLText(DataContentName))
                                        If Not Main.IsCSOK(CS) Then
                                            Call Main.CloseCS(CS)
                                            Call Main.CreateContent("default", DataTableName, DataContentName)
                                            CS = Main.OpenCSContent("Content", "name=" & KmaEncodeSQLText(DataContentName))
                                        End If
                                        If Main.IsCSOK(CS) Then
                                            DataContentID = Main.GetCSInteger(CS, "ID")
                                        End If
                                        Call Main.CloseCS(CS)
                                        .ContentID = DataContentID
                                        If DataContentID = 0 Then
                                            ProcessError = True
                                            Content = Content & "<div class=ccError>The data '" & ValueText & "' could not be created. Please try again, or enter a different name.</div>"
                                        Else
                                            '
                                            ' Verify the admin menu link
                                            '
                                            Call VerifyMenuEntry(Main, "Online Forms", Main.GetRecordID("Menu Entries", "Site Content"), 0)
                                            Call VerifyMenuEntry(Main, .NewContentName, Main.GetRecordID("Menu Entries", "Online Forms"), DataContentID)
                                            '
                                            ' Requires Reload
                                            '
                                            ReloadContentDefinitions = True
                                        End If
                                    End If
                                Case ContentSelectUseAuthMemberContent
                                    .ContentID = 0
                                    .NewContentName = ""
                                    .UseAuthMemberContent = True
                                    .UseAuthOrgContent = False
                                    DataContentID = Main.GetContentID("people")
                                Case ContentSelectUseAuthOrgContent
                                    .ContentID = 0
                                    .NewContentName = ""
                                    .UseAuthMemberContent = False
                                    .UseAuthOrgContent = True
                                    DataContentID = Main.GetContentID("organizations")
                                Case Else
                                    ProcessError = True
                                    Content = Content & "<div class=ccError>Please select the appropriate checkbox.</div>"
                            End Select
                            '
                            ' Process the fields again, this time checking it against the new content
                            '
                            TempError = ProcessHTMLFormReturnError(Main, Formset)
                            If TempError <> "" Then
                                ProcessError = True
                                Content = Content & TempError
                            End If
                            '
                            ' Create fields if needed
                            '
                            If CreateContentFields Then
                                '
                                ' Create all the content fields
                                '
Dim FieldNeeded As Boolean
                                DataContentName = Main.GetContentNameByID(DataContentID)
                                For Ptr = 0 To .FieldCnt - 1
                                    SQL = "select id from ccfields where name=" & KmaEncodeSQLText(.Fields(Ptr).Name) & " and contentid=" & DataContentID
                                    CS = Main.OpenCSSQL("default", SQL)
                                    FieldNeeded = Not Main.IsCSOK(CS)
                                    Call Main.CloseCS(CS)
                                    If FieldNeeded Then
                                        ReloadContentDefinitions = True
                                        'With .Fields(Ptr)
                                        On Error Resume Next
                                        If .Fields(Ptr).InputType = "TEXTAREA" Then
                                            Call Main.CreateContentField(DataContentName, .Fields(Ptr).Name, FieldTypeLongText)
                                        Else
                                            Call Main.CreateContentField(DataContentName, .Fields(Ptr).Name, FieldTypeText)
                                        End If
                                        On Error GoTo ErrorTrap
                                        '
                                        ' now verify the field was added
                                        '
                                        SQL = "select id from ccfields where name=" & KmaEncodeSQLText(.Fields(Ptr).Name) & " and contentid=" & DataContentID
                                        CS = Main.OpenCSSQL("default", SQL)
                                        FieldNeeded = Not Main.IsCSOK(CS)
                                        Call Main.CloseCS(CS)

                                        If FieldNeeded Then
                                            ProcessError = True
                                            Content = Content & "<div class=ccError>There was a problem creating field """ & .Fields(Ptr).Name & """. It may be a reserved name. Please try a different name</div>"
                                        Else
                                            If .UseAuthMemberContent Or .UseAuthOrgContent Then
                                                SQL = "update ccfields set EditTab='Online Form Response',EditSortPriority=ID where name=" & KmaEncodeSQLText(.Fields(Ptr).Name) & " and contentid=" & DataContentID
                                            Else
                                                SQL = "update ccfields set EditSortPriority=ID where name=" & KmaEncodeSQLText(.Fields(Ptr).Name) & " and contentid=" & DataContentID
                                            End If
                                            Call Main.ExecuteSQL("default", SQL)
                                        End If
                                        'End With
                                    End If
                                Next
                            End If
                            '
                            ' Reload CDef
                            '
                            If ReloadContentDefinitions Then
                                Call Main.LoadContentDefinitions
                            End If
                            '
                            '
                            '
                            If Not ProcessError Then
                                Call SaveFormSet(Main, Formset)
                                SubFormID = SubFormPageReview
                                'SubFormID = SubFormThankYouCopy
                                'SubFormID = SubFormMapField
                            End If
                        End With
                  
                  
