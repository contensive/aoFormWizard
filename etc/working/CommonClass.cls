VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CommonClass"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True

Option Explicit
'
'========================================================================
'
'========================================================================
'
' ----- Types
'
Type NameValueType
    Name As String
    Value As String
End Type
'
Type FormFieldType
    Id As Long
    Name As String
    ReplaceText As String
    ContentFieldID As Long
    Required As Boolean
    Caption As String
    InputType As String
    Deleted As Boolean
    ButtonActionID As Long
End Type
'
Type FormType
    Id As Long
    Name As String
    NextFormID As Long
    HTMLBody As String
    ContentID As Long
    NewContentName As String
    UseAuthMemberContent As Boolean
    UseAuthOrgContent As Boolean
    AddContinueButton As Boolean
    ContinueButtonName As String
    AddBackButton As Boolean
    BackButtonName As String
    AddCancelButton As Boolean
    CancelButtonName As String
    CreateContentFields As Boolean
    SortOrder As String
    Deleted As Boolean
    '
    ' Wizard values
    '
    Fields() As FormFieldType
    FieldCnt As Long
End Type
'
' Response to a form
'
Type FormResponseType
    Id As Long
    FormName As String
    FieldCnt As Long
    Fields() As NameValueType
End Type
'
' Response to a formset
'
Type FormsetResponseType
    Id As Long
    FormSetName As String
    FormResponseCnt As Long
    FormResponse() As FormResponseType
End Type
'
' Formset
'
Type FormSetType
    '
    ' Db values
    '
    Id As Long
    Active As Boolean
    Name As String
    ThankYouCopy As String
    JoinGroupID As Long
    NotificationEmailID As Long
    ResponseEmailID As Long
    FormsetResponse As FormsetResponseType
    '
    ' Wizard values
    '
    Forms() As FormType
    FormCnt As Long
    FormPtr As Long
End Type
'
' ----- Forms
'
Private Const SubFormSetName = 1
Private Const SubFormPageReview = 2
Private Const SubFormPasteHTML = 4
Private Const SubFormButtonAction = 5
Private Const SubFormSelectContent = 6
Private Const SubFormMapField = 7
Private Const SubFormMoreForms = 8
Private Const SubFormThankYouCopy = 9
Private Const SubFormActions = 10
Private Const SubFormFinished = 11
'
Private Const SubFormMax = 10
'
' ----- Buttons
'
' ----- local scope variables
'
' ----- Request Names
'
Private Const RequestNameSubForm = "SubFormID"
Private Const RequestNameFormSetName = "formsetname"
Private Const RequestNameFormSetID = "formsetid"
Private Const RequestNameFormName = "formname"
Private Const RequestNameHTMLBody = "htmlbody"
Private Const RequestNameJoinGroupID = "groupid"
Private Const RequestNameNotificationEmailID = "notificationemailid"
Private Const RequestNameResponseEmailID = "responsemailid"
Private Const RequestNameFormPtr = "formptr"
Private Const RequestNameUserFormResponseID = "responseid"
'
Private Const RequestNameContentSelectID = "contentselect"
Private Const RequestNameContentID = "contentid"
Private Const RequestNameNewContentName = "newcontent"
Private Const RequestNameCreateContentFields = "createnewfields"
Private Const RequestNameThankYouCopy = "thankyoucopy"
'
Private Const RequestNameButtonAction = "buttonaction"
Private Const RequestNameButtonActionID = "buttonactionid"
Private Const RequestNameButtonActionCnt = "buttonactioncnt"
Private Const RequestNameExtraButtonCancel = "buttonextracancel"
Private Const RequestNameExtraButtonBack = "buttonextraback"
Private Const RequestNameExtraButtonContinue = "buttonextracontinue"
Private Const RequestNameExtraButtonCancelName = "buttonextracancelname"
Private Const RequestNameExtraButtonBackName = "buttonextrabackname"
Private Const RequestNameExtraButtonContinueName = "buttonextracontinuename"
'
Private Const RequestNameOrderDirection = "orderdirection"
'
' ----- Order Direction
'
Const OrderDirectionUp = 1
Const OrderDirectionDown = 2
'
' ----- Content Select options
'
Const ContentSelectUseAuthMemberContent = 1
Const ContentSelectUseAuthOrgContent = 2
Const ContentSelectUseContentID = 3
Const ContentSelectUseNewContentName = 4
Const ContentSelectUseNone = 5
'
'
'
Private Const ButtonActionCancel = 1
Private Const ButtonActionBack = 2
Private Const ButtonActionContinue = 3
'
'=================================================================================
'   Aggregate Object Interface
'=================================================================================
'
Public Function GetForm(Main As Object) As String
'Public Function GetForm(Main As ccWeb3.MainClass) As String
    On Error GoTo ErrorTrap
    '
    Dim editor As String
    Dim TempError As String
    Dim NewFormSet As FormSetType
    Dim QSUp As String
    Dim OrderDirection As Long
    Dim QSDown As String
    Dim Style As String
    Dim QS As String
    Dim DataTableName As String
    Dim ReloadContentDefinitions As Boolean
    Dim DataContentName As String
    Dim DataContentID As Long
    Dim Ptr As Long
    Dim DynamicFormMenuID As Long
    Dim CS As Long
    Dim RowCnt As Long
    Dim FormBodySegments() As String
    Dim SegmentParts() As String
    Dim CSField As Long
    Dim SegmentPtr As Long
    Dim SQL As String
    Dim FormSetContentID As Long
    Dim criteria As String
    '
    Dim ContentSelectID As Long
    Dim FormNewContentName As String
    Dim CreateContentFields As Boolean
    Dim formContentId As Long
    '
    Dim FormSetName As String
    Dim HeaderCaption As String
    Dim Content As String
    Dim Formset As FormSetType
    Dim FormName As String
    Dim FormHTMLBody As String
    '
    Dim RowStart As String
    Dim RowStop As String
    Dim ValueText As String
    Dim ValueInteger As Long
    Dim ContentName As String
    Dim ContentID As Long
    Dim Description As String
    Dim WizardContent As String
    Dim SubFormID As Long
    Dim ProcessError As Boolean
    Dim TestMemberID As Long
    Dim EmailAddress As String
    Dim MemberName As String
    Dim HeadingContentID As Long
    Dim HeadingContentName As String
    Dim RecordName As String
    Dim RecordID As Long
    Dim IDList As String
    Dim CSList As Long
    Dim ButtonList As String
    Dim EmailScheduleStart As Date
    Dim EmailFinishID As Long
    Dim Button As String
    Dim FormSetID As Long
    '
    Dim HTML As New kmaHTML.ParseClass
    Dim ElementPtr As Long
    Dim Copy As String
    Dim ElementTag As String
    Dim fs As New FastStringClass
    Dim InputType As String
    Dim InputName As String
    Dim InputValue As String
    Dim FieldFound As Boolean
    Dim FieldPtr As Long
    Dim editorAddonId As Long
    '
    ProcessError = False
    Content = ""
    Button = Main.GetStreamText(RequestNameButton)
    If Button = ButtonCancel Then
        '
        ' Cancel
        '
        Call Main.Redirect(Main.ServerAppRootPath & Main.ServerAppPath)
    Else
        FormSetID = Main.GetStreamInteger(RequestNameFormSetID)
        If FormSetID <> 0 Then
            Formset = GetFormSet(Main, FormSetID)
            If Formset.Id = 0 Then
                FormSetID = 0
                'Call Main.setVisitProperty("FormWizardSetID", CStr(FormSetID))
            End If
            If FormSetID <> 0 Then
                If Formset.FormCnt > 0 Then
                    Formset.FormPtr = Main.GetStreamInteger(RequestNameFormPtr)
                End If
                Call Main.AddRefreshQueryString(RequestNameFormSetID, FormSetID)
            End If
        End If
        If (FormSetID = 0) Or (FormSetID <> kmaEncodeInteger(Main.GetVisitProperty("FormWizardSetID", "0"))) Then
            '
            ' formset is zero or it was changed since last hit, clear visit properties and use the new one
            '
            Call Main.SetVisitProperty("FormWizardSetID", CStr(FormSetID))
            Call Main.SetVisitProperty("FormWizard-UserResponseID", "")
        End If
        '
        '
        '
        SubFormID = Main.GetStreamInteger(RequestNameSubForm)
        If FormSetID = 0 Then
            '
            ' can only access the formsetname if no formsetid
            '
            If SubFormID <> SubFormSetName Then
                '
                ' if there was a submit from another form, block it (visit timed out)
                '
                Button = ""
            End If
            SubFormID = 0
        End If
        If SubFormID = 0 Then
            SubFormID = SubFormSetName
        End If
        '
        ' Process incoming form
        '
        'If Button <> "" Then
            Select Case SubFormID
                Case SubFormSetName
                    '
                    ' Pick Name, create Records
                    '
                    If Button = ButtonContinue Then
                        '
                        ValueText = Trim(Main.GetStreamText(RequestNameFormSetName))
                        ValueInteger = Main.GetStreamInteger(RequestNameFormSetID)
                        If (ValueText = "") And (ValueInteger = 0) Then
                            ProcessError = True
                            Content = Content & "<div class=ccError>A name or form selection is required.</div>"
                        ElseIf ValueInteger <> 0 Then
                            '
                            ' formset being assigned
                            '
                            FormSetID = ValueInteger
                            Call Main.AddRefreshQueryString(RequestNameFormSetID, FormSetID)
                            Call Main.SetVisitProperty("FormWizardSetID", FormSetID)
                            Formset = GetFormSet(Main, FormSetID)
                            SubFormID = SubFormPageReview
                        Else
                            CS = Main.openCsContent("Form Sets", "name=" & KmaEncodeSQLText(ValueText))
                            If Main.iscsok(CS) Then
                                '
                                ' name already used
                                '
                                ProcessError = True
                                Content = Content & "<div class=ccError>A form set with this name already exists. Either create a new name or select this form from the drop-down menu.</div>"
                            End If
                            Call Main.closeCs(CS)
                            Formset.Name = ValueText
                            Call SaveFormSet(Main, Formset)
                            FormSetID = Main.GetRecordID("Form Sets", ValueText)
                            Call Main.AddRefreshQueryString(RequestNameFormSetID, CStr(FormSetID))
                            SubFormID = SubFormPageReview
                        End If
                    End If
                Case SubFormPageReview
                    '
                    ' Review pages for this FormSet
                    '
Dim TargetPtr As Long
                    If Formset.FormCnt > 1 Then
                        '
                        ' Process re-order
                        '
                        OrderDirection = Main.GetStreamInteger(RequestNameOrderDirection)
                        If OrderDirection <> 0 Then
                            TargetPtr = Main.GetStreamInteger(RequestNameFormPtr)
                            Select Case OrderDirection
                                Case OrderDirectionUp
                                    NewFormSet = Formset
                                    For Ptr = 1 To NewFormSet.FormCnt - 1
                                        If Ptr = TargetPtr Then
                                            NewFormSet.Forms(Ptr - 1) = Formset.Forms(Ptr)
                                            NewFormSet.Forms(Ptr) = Formset.Forms(Ptr - 1)
                                            Exit For
                                        End If
                                    Next
                                    Formset = NewFormSet
                                    Call SaveFormSet(Main, Formset)
                                Case OrderDirectionDown
                                    NewFormSet = Formset
                                    For Ptr = 0 To NewFormSet.FormCnt - 2
                                        If Ptr = TargetPtr Then
                                            NewFormSet.Forms(Ptr + 1) = Formset.Forms(Ptr)
                                            NewFormSet.Forms(Ptr) = Formset.Forms(Ptr + 1)
                                            Exit For
                                        End If
                                    Next
                                    Formset = NewFormSet
                                    Call SaveFormSet(Main, Formset)
                            End Select
                        End If
                    End If
                    If Button = ButtonAdd Then
                        '
                        ' Add a new page
                        '
                        SubFormID = SubFormPasteHTML
                    ElseIf Button = ButtonContinue Then
                        '
                        ' Continue on to thank you
                        '
                        SubFormID = SubFormThankYouCopy
                    ElseIf Button = ButtonBack Then
                        '
                        ' Back
                        '
                        SubFormID = SubFormSetName
                    End If
                Case SubFormPasteHTML
                    '
                    ' Paste the HTML form
                    '
                    If (Button = ButtonContinue) Or (Button = ButtonSave) Then
                        ''
                        '' Continue - Create first form if needed
                        ''
                        'If Formset.FormCnt = 0 Then
                        '    Formset.FormCnt = 1
                        '    Formset.FormPtr = 0
                        '    ReDim Formset.Forms(0)
                        'End If
                        '
                        ' form name
                        '
                        ValueText = Trim(Main.GetStreamText(RequestNameFormName))
                        If ValueText = "" Then
                            ProcessError = True
                            Content = Content & "<div class=ccError>A name is required.</div>"
                        Else
                            Formset.Forms(Formset.FormPtr).Name = ValueText
                        End If
                        '
                        ' form HTML
                        '
                        ValueText = Trim(Main.GetStreamActiveContent(RequestNameHTMLBody))
                        If ValueText = "" Then
                            ProcessError = True
                            Content = Content & "<div class=ccError>A HTML Body is required.</div>"
                        Else
                            Formset.Forms(Formset.FormPtr).HTMLBody = ValueText
                        End If
                        If Not ProcessError Then
                            '
                            ' Parse the field names out of the HTML into fields
                            '
                        TempError = ProcessHTMLFormReturnError(Main, Formset)
                        If TempError <> "" Then
                            ProcessError = True
                            Content = Content & "<div class=ccError>" & TempError & "</div>"
                        End If
                        End If
                        If (Not ProcessError) Then
                            Call SaveFormSet(Main, Formset)
                            If (Button = ButtonContinue) Then
                                SubFormID = SubFormButtonAction
                            End If
                        End If
                    ElseIf Button = ButtonBack Then
                        '
                        ' Back
                        '
                        SubFormID = SubFormPageReview
                    ElseIf Button = ButtonDelete Then
                        '
                        ' Delete and go back to review
                        '
                        SubFormID = SubFormPageReview
                        Formset.Forms(Formset.FormPtr).Deleted = True
                        Call SaveFormSet(Main, Formset)
                        Formset = GetFormSet(Main, FormSetID)
                    End If
                Case SubFormButtonAction
                    '
                    '
                    '
Dim FormFieldPtr As Long
Dim FormFieldID As Long
Dim RowPtr As Long
                    If Button = ButtonContinue Then
                        '
                        With Formset.Forms(Formset.FormPtr)
                            .AddCancelButton = Main.GetStreamBoolean(RequestNameExtraButtonCancel)
                            .CancelButtonName = Main.GetStreamText(RequestNameExtraButtonCancelName)
                            .AddBackButton = Main.GetStreamBoolean(RequestNameExtraButtonBack)
                            .BackButtonName = Main.GetStreamText(RequestNameExtraButtonBackName)
                            .AddContinueButton = Main.GetStreamBoolean(RequestNameExtraButtonContinue)
                            .ContinueButtonName = Main.GetStreamText(RequestNameExtraButtonContinueName)
                            '
                            RowCnt = Main.GetStreamInteger(RequestNameButtonActionCnt)
                            If RowCnt > 0 Then
                                For RowPtr = 0 To RowCnt - 1
                                    FormFieldID = Main.GetStreamInteger(RequestNameButtonActionID & RowPtr)
                                    If FormFieldID <> 0 Then
                                        For FormFieldPtr = 0 To .FieldCnt - 1
                                            If .Fields(FormFieldPtr).Id = FormFieldID Then
                                                .Fields(FormFieldPtr).ButtonActionID = Main.GetStreamInteger(RequestNameButtonAction & RowPtr)
                                                Exit For
                                            End If
                                        Next
                                    End If
                                Next
                            End If
                        End With
                        If Not ProcessError Then
                            Call SaveFormSet(Main, Formset)
                            SubFormID = SubFormSelectContent
                        End If
                    ElseIf Button = ButtonBack Then
                        SubFormID = SubFormPasteHTML
                    End If
                Case SubFormSelectContent
                    '
                    ' Select content
                    '
                    If Button = ButtonContinue Then
                        '
                        ' Create first form if needed
                        '
                        If Formset.FormCnt = 0 Then
                            Formset.FormCnt = 1
                            Formset.FormPtr = 0
                            ReDim Formset.Forms(0)
                        End If
                        '
                        ' Content Select
                        '
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
                                        CS = Main.openCsContent("Content", "name=" & KmaEncodeSQLText(DataContentName))
                                        If Not Main.iscsok(CS) Then
                                            Call Main.closeCs(CS)
                                            Call Main.CreateContent("default", DataTableName, DataContentName)
                                            CS = Main.openCsContent("Content", "name=" & KmaEncodeSQLText(DataContentName))
                                        End If
                                        If Main.iscsok(CS) Then
                                            DataContentID = Main.GetCSInteger(CS, "ID")
                                        End If
                                        Call Main.closeCs(CS)
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
                                    DataContentID = Main.getContentId("people")
                                Case ContentSelectUseAuthOrgContent
                                    .ContentID = 0
                                    .NewContentName = ""
                                    .UseAuthMemberContent = False
                                    .UseAuthOrgContent = True
                                    DataContentID = Main.getContentId("organizations")
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
                                    FieldNeeded = Not Main.iscsok(CS)
                                    Call Main.closeCs(CS)
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
                                        FieldNeeded = Not Main.iscsok(CS)
                                        Call Main.closeCs(CS)
                                        
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
                    ElseIf Button = ButtonBack Then
                        '
                        ' Back
                        '
                        SubFormID = SubFormButtonAction
                    End If
'                Case SubFormMapField
'                    '
'                    '
'                    '
'                    If Button = ButtonContinue Then
'                        '
'                        '
'                        '
'                        If Not ProcessError Then
'                            Call SaveFormSet(Main,FormSet)
'                            SubFormID = SubFormThankYouCopy
'                        End If
'                    ElseIf Button = ButtonBack Then
'                        SubFormID = SubFormSelectContent
'                    End If
                Case SubFormThankYouCopy
                    '
                    '
                    '
                    If Button = ButtonContinue Then
                        ValueText = Main.GetStreamText(RequestNameThankYouCopy)
                        If ValueText = "" Then
                            ProcessError = True
                            Content = Content & "<div class=ccError>Please enter the thank you copy.</div>"
                        Else
                            Formset.ThankYouCopy = ValueText
                        End If
                        If Not ProcessError Then
                            Call SaveFormSet(Main, Formset)
                            SubFormID = SubFormActions
                        End If
                    ElseIf Button = ButtonBack Then
                        '
                        ' Back
                        '
                        SubFormID = SubFormPageReview
                    End If
                Case SubFormActions
                    '
                    '
                    '
                    If Button = ButtonContinue Then
                        Formset.JoinGroupID = Main.GetStreamInteger(RequestNameJoinGroupID)
                        Formset.NotificationEmailID = Main.GetStreamInteger(RequestNameNotificationEmailID)
                        Formset.ResponseEmailID = Main.GetStreamInteger(RequestNameResponseEmailID)
                        Call SaveFormSet(Main, Formset)
                        SubFormID = SubFormFinished
                    ElseIf Button = ButtonBack Then
                        '
                        ' Back
                        '
                        SubFormID = SubFormThankYouCopy
                    End If
                Case SubFormFinished
                    '
                    ' Set it active for use
                    '
                    Formset.Active = True
                    Call SaveFormSet(Main, Formset)
                    'Call Main.setVisitProperty("FormWizardSetID", "0")
                    Call Main.Redirect(Main.ServerAppRootPath & Main.ServerAppPath)
            End Select
        'End If
        '
        ' Get Next Form
        '
        HeaderCaption = "Form Wizard"
        Call Main.AddRefreshQueryString(RequestNameSubForm, SubFormID)
'        Content = Content & Main.GetFormInputHidden(RequestNameSubForm, SubFormID)
        If Main.IsUserError Then
            Content = Content & Main.GetUserError()
        End If
        Select Case SubFormID
            Case SubFormSetName
                '
                ' Formset Name
                '
                If Formset.Id <> 0 Then
                    FormSetName = ""
                Else
                    FormSetName = Main.GetStreamText(RequestNameFormSetName)
                End If
Dim Selector As String
                Selector = Main.GetFormInputSelect(RequestNameFormSetID, Formset.Id, "Form Sets")
Dim Pos As Long
Dim cnt As Long
                Pos = 1
                Do While Pos <> 0 And cnt < 2
                    Pos = InStr(Pos, Selector, "<option", vbTextCompare)
                    If Pos <> 0 Then
                        cnt = cnt + 1
                        Pos = Pos + 1
                    End If
                Loop
                Description = "<B>Form Set Name</B><BR><BR>Select a current formset to edit, or enter a name to create a new form set. A form set is a group of pages that makes up a multipage form. This name will be used only by you to identify this form. This name will not be visible to anyone on your website."
                Content = Content _
                    & "<div>" _
                    & "<TABLE border=0 cellpadding=10 cellspacing=0 width=""100%"">" _
                    & "<TR>" _
                    & "<TD align=left colspan=2>Create a new multi-page form</td>" _
                    & "</tr>" _
                    & "<TR>" _
                    & "<TD width=100 align=right>Name&nbsp;<br><img src=/cclib/images/spacer.gif width=100 height=1></td>" _
                    & "<td width=99% align=left>" & Main.GetFormInputText(RequestNameFormSetName, FormSetName, 1, 30) & "</td>" _
                    & "</tr>"
                If cnt > 1 Then
                    Content = Content _
                        & "<TR>" _
                        & "<TD align=left colspan=2>Edit a current multi-page form</td>" _
                        & "</tr>" _
                        & "<TR>" _
                        & "<TD width=100 align=right>Forms&nbsp;<br><img src=/cclib/images/spacer.gif width=100 height=1></td>" _
                        & "<td width=99% align=left>" & Selector & "</td>" _
                        & "</tr>"
                End If
                Content = Content _
                    & "</table>" _
                    & "</div>" _
                    & ""
                WizardContent = Main.GetWizardContent(HeaderCaption, ButtonCancel, ButtonContinue, True, True, Description, Content)
            Case SubFormPageReview
                '
                ' Review the pages so far
                '
                If Formset.Id <> 0 Then
                    FormSetName = ""
                Else
                    FormSetName = Main.GetStreamText(RequestNameFormSetName)
                End If
                Description = "<B>Form Pages in this Form Set</B><BR><BR>Review the form pages in this form set. When a visitor fills out your form set, they fill out one form page at a time. As each form page is completed, the next form page is presented."
                Content = Content _
                    & "<div>" _
                    & "<TABLE border=0 cellpadding=0 cellspacing=0 width=""100%"">" _
                    & "<TR>" _
                        & "<TD align=left width=""100%"">Form Pages</td>" _
                        & "<TD align=center width=50>Order<br><img src=""/cclib/images/spacer.gif"" width=50 height=1></td>" _
                    & "</TR>" _
                    & "<TR>" _
                        & "<TD align=left colspan=2><div class=""ccPanel3DReverse"" style=""background-color:white;"">" _
                    & "<TABLE border=0 cellpadding=4 cellspacing=0 width=""100%"">"
                If Formset.FormCnt = 0 Then
                    Content = Content & "<TR><TD align=left Style=""padding:4px;"" colspan=2>There are no form pages</td></TR>"
                    ButtonList = ButtonAdd
                Else
                    QS = Main.RefreshQueryString
                    QS = ModifyQueryString(QS, RequestNameSubForm, SubFormPasteHTML)
                    QSUp = Main.RefreshQueryString
                    QSUp = ModifyQueryString(QSUp, RequestNameSubForm, SubFormPageReview)
                    QSUp = ModifyQueryString(QSUp, RequestNameOrderDirection, OrderDirectionUp)
                    QSDown = Main.RefreshQueryString
                    QSDown = ModifyQueryString(QSDown, RequestNameSubForm, SubFormPageReview)
                    QSDown = ModifyQueryString(QSDown, RequestNameOrderDirection, OrderDirectionDown)
                    For Ptr = 0 To Formset.FormCnt - 1
                        If Ptr = 0 Then
                            Style = """padding:4px;"""
                        Else
                            Style = """padding:4px;border-top:1px solid #A0A0A0;"""
                        End If
                        QS = ModifyQueryString(QS, RequestNameFormPtr, CStr(Ptr))
                        QSUp = ModifyQueryString(QSUp, RequestNameFormPtr, CStr(Ptr))
                        QSDown = ModifyQueryString(QSDown, RequestNameFormPtr, CStr(Ptr))
                        If Formset.FormCnt = 1 Then
                            Content = Content _
                                & "<TR>" _
                                    & "<TD align=left Style=" & Style & "><a href=""?" & QS & """>" & Formset.Forms(Ptr).Name & "</a></td>" _
                                    & "<TD align=right Style=" & Style & ">&nbsp;</td>" _
                                & "</TR>"
                        ElseIf Ptr = 0 Then
                            Content = Content _
                                & "<TR>" _
                                    & "<TD align=left Style=" & Style & "><a href=""?" & QS & """>" & Formset.Forms(Ptr).Name & "</a></td>" _
                                    & "<TD align=right Style=" & Style & ">UP&nbsp;<a href=""?" & QSDown & """>Down</a></td>" _
                                & "</TR>"
                        ElseIf Ptr = Formset.FormCnt - 1 Then
                            Content = Content _
                                & "<TR>" _
                                    & "<TD align=left Style=" & Style & "><a href=""?" & QS & """>" & Formset.Forms(Ptr).Name & "</a></td>" _
                                    & "<TD align=right Style=" & Style & "><a href=""?" & QSUp & """>UP</a>&nbsp;Down</td>" _
                                & "</TR>"
                        Else
                            Content = Content _
                                & "<TR>" _
                                    & "<TD align=left Style=" & Style & "><a href=""?" & QS & """>" & Formset.Forms(Ptr).Name & "</a></td>" _
                                    & "<TD align=right Style=" & Style & "><a href=""?" & QSUp & """>UP</a>&nbsp;<a href=""?" & QSDown & """>Down</a></td>" _
                                & "</TR>"
                        End If
                    Next
                    ButtonList = ButtonAdd & "," & ButtonContinue
                End If
                Content = Content _
                    & "</table></div></td></TR></table>" _
                    & "</div>" _
                    & ""
                WizardContent = Main.GetWizardContent(HeaderCaption, ButtonCancel & "," & ButtonBack, ButtonList, True, True, Description, Content)
            Case SubFormPasteHTML
                '
                ' Form Name and HTML Page
                '
                Description = "<p><b>Form Name and HTML</b></p><p>Enter a name for your form page. A form page is a single page in a multipage form. This name will be used only by you to identify this form. The HTML body is the layout for the form. It should NOT include a form tag or buttons. If your data is to be saved into a database field, use the database field name as the name of the form element. Use the 'Full Screen' icon to enlarge the editor. You will be logged off after 60 minutes, so save often to prevent data loss.<BR><BR>To create required fields, include a '*' character in the name of the field. This character will NOT be included in the database field name. It will just be used to signal that the field is required on the form. For example, *FirstName requires that the field FirstName be included.</p>"
                If Formset.FormPtr = 0 Then
                    If Button = ButtonAdd Then
                        Formset.FormPtr = Formset.FormCnt
                    End If
                End If
                If Formset.FormPtr >= Formset.FormCnt Then
                    Formset.FormPtr = Formset.FormCnt
                    Formset.FormCnt = Formset.FormCnt + 1
                    ReDim Preserve Formset.Forms(Formset.FormPtr)
                    Call SaveFormSet(Main, Formset)
                End If
                'If Formset.FormCnt = 0 Then
                '    FormName = ""
                '    FormHTMLBody = ""
                'Else
                    FormName = Formset.Forms(Formset.FormPtr).Name
                    FormHTMLBody = Formset.Forms(Formset.FormPtr).HTMLBody
                'End If
                FormSetName = Formset.Name
                If Main.SiteProperty_BuildVersion < "4.1.372" Then
                    editor = Main.GetFormInputActiveContent(RequestNameHTMLBody, FormHTMLBody)
                Else
                    '
                    ' hack - new editor does not have input fields, force to default editor
                    '
                    editorAddonId = 0
                    SQL = "select editorAddonId from ccfieldtypes where name='html'"
                    CS = Main.OpenCSSQL("default", SQL)
                    If Main.iscsok(CS) Then
                        editorAddonId = Main.GetCSInteger(CS, "editorAddonId")
                    End If
                    Call Main.closeCs(CS)
                    SQL = "update ccfieldtypes set editorAddonId=0 where name='html'"
                    Call Main.ExecuteSQL("", SQL)
                    editor = Main.GetFormInputHTML3(RequestNameHTMLBody, FormHTMLBody)
                    SQL = "update ccfieldtypes set editorAddonId=" & editorAddonId & " where name='html'"
                    Call Main.ExecuteSQL("", SQL)
                End If
                Content = Content _
                    & cr & Main.GetFormInputHidden(RequestNameFormPtr, Formset.FormPtr) _
                    & cr & "<div class=""fwFormBody"">" _
                    & cr & "<div class=""fwEditCaption"">Name</div>" _
                    & cr & "<div class=""fwEditInput"">" & Main.GetFormInputText(RequestNameFormName, FormName, 1, 30) & "</div>" _
                    & cr & "<div class=""fwEditCaption"">HTML Body</div>" _
                    & cr & "<div class=""fwEditInput"">" & editor & "</div>" _
                    & cr & "</div>"
                
'                Content = Content _
'                    & CR & Main.GetFormInputHidden(RequestNameFormPtr, Formset.FormPtr) _
'                    & CR & "<div class=""fwFormBody"">" _
'                    & CR & "<div class=""fwEditCaption"">Name</div>" _
'                    & CR & "<div class=""fwEditInput"">" & Main.GetFormInputText(RequestNameFormName, FormName, 1, 30) & "</div>" _
'                    & CR & "<div class=""fwEditCaption"">HTML Body</div>" _
'                    & CR & "<div class=""fwEditInput"">" & Main.GetFormInputActiveContent(RequestNameHTMLBody, FormHTMLBody) & "</div>" _
'                    & CR & "</div>"


'                Content = Content _
'                    & Main.GetFormInputHidden(RequestNameFormPtr, Formset.FormPtr) _
'                    & "<div>" _
'                    & "<TABLE border=0 cellpadding=10 cellspacing=0 width=""100%"">" _
'                    & "<TR>" _
'                    & "<TD width=100 align=right>Name&nbsp;<br><img src=/cclib/images/spacer.gif width=100 height=1></td>" _
'                    & "<td width=99% align=left>" & Main.GetFormInputText(RequestNameFormName, FormName, 1, 30) & "</td>" _
'                    & "</tr>" _
'                    & "<TR>" _
'                    & "<TD width=100 align=right>HTML Body&nbsp;<br><img src=/cclib/images/spacer.gif width=100 height=1></td>" _
'                    & "<td width=99% align=left>" & Main.GetFormInputActiveContent(RequestNameHTMLBody, FormHTMLBody) & "</td>" _
'                    & "</tr>" _
'                    & "</table>" _
'                    & "</div>" _
'                    & ""
                WizardContent = Main.GetWizardContent(HeaderCaption, ButtonCancel & "," & ButtonBack, ButtonDelete & "," & ButtonSave & "," & ButtonContinue, True, True, Description, Content)
            Case SubFormButtonAction
                '
                ' Determine Button Actions (what will the provided buttons do)
                '
                Description = "<B>Form Buttons</B><BR><BR>Assign actions to the buttons on your form, and add any new buttons needed."
                If Formset.FormCnt = 0 Then
                    FormName = ""
                    FormHTMLBody = ""
                Else
                    FormName = Formset.Forms(Formset.FormPtr).Name
                    FormHTMLBody = Formset.Forms(Formset.FormPtr).HTMLBody
                End If
                FormSetName = Formset.Name
                Content = Content _
                    & Main.GetFormInputHidden(RequestNameFormPtr, Formset.FormPtr) _
                    & "<div>" _
                    & "<TABLE border=0 cellpadding=10 cellspacing=0 width=""100%"">" _
                    & "<TR>" _
                    & "<TD width=""100%"" colspan=2>Assign actions to the buttons on your form</td>" _
                    & "</TR>"
                RowCnt = 0
                For FieldPtr = 0 To Formset.Forms(Formset.FormPtr).FieldCnt - 1
                    With Formset.Forms(Formset.FormPtr).Fields(FieldPtr)
                        If .InputType = "SUBMIT" Then
                            Content = Content _
                                & "<TR>" _
                                & "<TD width=100 align=right><B>" & .Name & "</B>&nbsp;<br><img src=/cclib/images/spacer.gif width=100 height=1></td>" _
                                & "<TD width=99% align=left>" & Main.GetFormInputSelectList(RequestNameButtonAction & RowCnt, .ButtonActionID, "Cancel,Back,Submit") & Main.GetFormInputHidden(RequestNameButtonActionID & RowCnt, .Id) & "</td>" _
                                & "</TR>"
                            RowCnt = RowCnt + 1
                        End If
                    End With
                Next
                If RowCnt = 0 Then
                    Content = Content _
                        & "<TR>" _
                        & "<TD width=100 align=right>&nbsp</td>" _
                        & "<TD width=99% align=left>There are no submit buttons on your form</td>" _
                        & "</TR>"
                End If
                With Formset.Forms(Formset.FormPtr)
                    Content = Content _
                        & "<TR>" _
                        & "<TD width=""100%"" colspan=2>Add these buttons below your form</td>" _
                        & "</TR>" _
                        & "<TR>" _
                        & "<TD width=100 align=right><B>Cancel</B></td>" _
                        & "<TD width=99% align=left>" & Main.GetFormInputCheckBox(RequestNameExtraButtonCancel, .AddCancelButton) & "&nbsp;" & Main.GetFormInputText(RequestNameExtraButtonCancelName, .CancelButtonName, 1, 20) & "</td>" _
                        & "</TR>" _
                        & "<TR>" _
                        & "<TD width=100 align=right><B>Back</B></td>" _
                        & "<TD width=99% align=left>" & Main.GetFormInputCheckBox(RequestNameExtraButtonBack, .AddBackButton) & "&nbsp;" & Main.GetFormInputText(RequestNameExtraButtonBackName, .BackButtonName, 1, 20) & "</td>" _
                        & "<TR>" _
                        & "<TD width=100 align=right><B>Submit</B></td>" _
                        & "<TD width=99% align=left>" & Main.GetFormInputCheckBox(RequestNameExtraButtonContinue, .AddContinueButton) & "&nbsp;" & Main.GetFormInputText(RequestNameExtraButtonContinueName, .ContinueButtonName, 1, 20) & "</td>" _
                        & "</TR>"
                End With
                Content = Content & Main.GetFormInputHidden(RequestNameButtonActionCnt, RowCnt)
                Content = Content _
                    & "</table>" _
                    & "</div>" _
                    & ""
                WizardContent = Main.GetWizardContent(HeaderCaption, ButtonCancel & "," & ButtonBack, ButtonContinue, True, True, Description, Content)
            Case SubFormSelectContent
                '
                ' Select which Content the data is stored
                '
                Description = "<B>Form result storage</B><BR><BR>Select where the data from this form will be stored. All fields that exactly match the database fields will be stored. If you select 'Create New Fields', your database will be expanded to include the fields from this form. In either case, all the data from the response to this this form will be sent to the Response system email, and saved in the 'User Form Response' data."
                If Formset.FormCnt = 0 Then
                    FormNewContentName = ""
                    formContentId = 0
                    FormHTMLBody = ""
                    ContentSelectID = 0
                    CreateContentFields = False
                Else
                    ContentSelectID = ContentSelectUseNone
                    With Formset.Forms(Formset.FormPtr)
                        If .ContentID <> 0 Then
                            ContentSelectID = ContentSelectUseContentID
                        ElseIf .NewContentName <> "" Then
                            ContentSelectID = ContentSelectUseNewContentName
                        ElseIf .UseAuthMemberContent Then
                            ContentSelectID = ContentSelectUseAuthMemberContent
                        ElseIf .UseAuthOrgContent Then
                            ContentSelectID = ContentSelectUseAuthOrgContent
                        End If
                    End With
                    FormName = Formset.Forms(Formset.FormPtr).Name
                    FormHTMLBody = Formset.Forms(Formset.FormPtr).HTMLBody
                End If
                FormSetName = Formset.Name
                Content = Content _
                    & Main.GetFormInputHidden(RequestNameFormPtr, Formset.FormPtr) _
                    & "<div>" _
                    & "<TABLE border=0 cellpadding=10 cellspacing=0 width=""100%"">"
                Content = Content _
                    & "<TR>" _
                    & "<TD width=100 align=right valign=top>" & Main.GetFormInputRadioBox(RequestNameContentSelectID, ContentSelectUseNone, ContentSelectID) & "</td>" _
                    & "<td width=99% align=left>" _
                        & "<B>None</B>" _
                        & "<br>When the form is completed, data will not be stored in a separate data table. It will still be saved in the 'User Form Response' section and emailed with the notification system email." _
                        & "<br><img src=/cclib/images/spacer.gif width=100 height=1></td>" _
                    & "</tr>"
                Content = Content _
                    & "<TR>" _
                    & "<TD width=100 align=right valign=top>" & Main.GetFormInputRadioBox(RequestNameContentSelectID, ContentSelectUseAuthMemberContent, ContentSelectID) & "</td>" _
                    & "<td width=99% align=left>" _
                        & "<B>Update User</B>*&nbsp;" _
                        & "<br>When the form is completed, the data entered will be saved to the visitor's People record. If your website is configured to allow 'recognized' users and the visitor was recognized as a People record that contains a username, the visitor will be required to log-in, or logout to continue. See Security Settings on the Admin Navigator for details." _
                        & "<br><img src=/cclib/images/spacer.gif width=100 height=1></td>" _
                    & "</tr>"
                Content = Content _
                    & "<TR>" _
                    & "<TD width=100 align=right valign=top>" & Main.GetFormInputRadioBox(RequestNameContentSelectID, ContentSelectUseAuthOrgContent, ContentSelectID) & "</td>" _
                    & "<td width=99% align=left>" _
                        & "<B>Update Organization</B>*&nbsp;" _
                        & "<br>When the form is completed, the data entered will be written to the person's organization account. They will be required to login for this option. If they update their organization account, all other members with the same organization selection will be updated as well." _
                        & "<br>" _
                        & "<img src=/cclib/images/spacer.gif width=100 height=1></td>" _
                    & "</tr>"
                    criteria = ""
                    If Main.IsDeveloper Then
                        criteria = "(1=1)"
                    ElseIf Main.IsAdmin Then
                        criteria = "(DeveloperOnly=0)"
                    Else
                        criteria = "(DeveloperOnly=0)and(AdminOnly=0)"
                    End If
                Content = Content _
                    & "<TR>" _
                    & "<TD width=100 align=right valign=top>" & Main.GetFormInputRadioBox(RequestNameContentSelectID, ContentSelectUseContentID, ContentSelectID) & "</td>" _
                    & "<td width=99% align=left>" _
                        & "<B>Add to existing data table</B>" _
                        & "<br>When the form is completed, a new record in this data will be created for their response." _
                        & "<br>" _
                        & "<img src=/cclib/images/spacer.gif width=100 height=1></td>" _
                    & "</tr>" _
                    & "<TR>" _
                    & "<TD width=100 align=right style=""padding-top:0px;"">&nbsp;</td>" _
                    & "<td width=99% align=left style=""padding-top:0px;"">" & Main.GetFormInputSelect(RequestNameContentID, Formset.Forms(Formset.FormPtr).ContentID, "Content", criteria) & "</td>" _
                    & "</tr>"
                Content = Content _
                    & "<TR>" _
                    & "<TD width=100 align=right valign=top>" & Main.GetFormInputRadioBox(RequestNameContentSelectID, ContentSelectUseNewContentName, ContentSelectID) & "</td>" _
                    & "<td width=99% align=left valign=top>" _
                        & "<B>Add to new data table</B>" _
                        & "<br>A new data table will be added you your site when you hit continue. When the form is completed, a new record will be created in this data for their response." _
                        & "<br>" _
                        & "<img src=/cclib/images/spacer.gif width=100 height=1></td>" _
                    & "</tr>" _
                    & "<TR>" _
                    & "<TD width=100 align=right style=""padding-top:0px;"">&nbsp;</td>" _
                    & "<td width=99% align=left style=""padding-top:0px;"">" & Main.GetFormInputText(RequestNameNewContentName, FormNewContentName, 1, 30) & "</td>" _
                    & "</tr>"
                Content = Content _
                    & "<TR>" _
                    & "<TD width=100 align=right valign=top>" & Main.GetFormInputCheckBox(RequestNameCreateContentFields, CreateContentFields) & "</td>" _
                    & "<td width=99% align=left valign=top><B>Create New Database Fields</B>" _
                        & "<br>When checked, all the fields in your form will be added to the data table you selected above when you hit continue. Note that if you add fields to large tables, this operation may take a few minutes." _
                        & "<br><img src=/cclib/images/spacer.gif width=100 height=1></td>" _
                    & "</tr>"
                Content = Content _
                    & "<TR>" _
                    & "<td colspan=2 width=""100%"" align=left>* To use this data, the visitor will be required to login</td>" _
                    & "</tr>" _
                    & "</table>" _
                    & "</div>" _
                    & ""
                WizardContent = Main.GetWizardContent(HeaderCaption, ButtonCancel & "," & ButtonBack, ButtonContinue, True, True, Description, Content)
'            Case SubFormMapField
'                '
'                ' later
'                '
'                WizardContent = Main.GetWizardContent(HeaderCaption, ButtonCancel & "," & ButtonBack, ButtonContinue, True, True, "Description", "Content")
            Case SubFormThankYouCopy
                '
                ' Thank you Copy
                '
                Description = "<B>Thank You Copy</B><BR><BR>Enter the text that will appear when this form set is complete."
                Content = Content _
                    & "<div>" _
                    & "<TABLE border=0 cellpadding=10 cellspacing=0 width=""100%"">" _
                    & "<TR>" _
                    & "<td align=left>" & Main.GetFormInputHTML(RequestNameThankYouCopy, Formset.ThankYouCopy) & "</td>" _
                    & "</tr>" _
                    & "</table>" _
                    & "</div>" _
                    & ""
                WizardContent = Main.GetWizardContent(HeaderCaption, ButtonCancel & "," & ButtonBack, ButtonContinue, True, True, Description, Content)
            Case SubFormActions
                '
                ' Actions to take
                '
Dim ReturnLink As String
Dim EmailCID As Long
                EmailCID = Main.getContentId("System Email")
                ReturnLink = Main.ServerLink
                ReturnLink = kmaModifyLinkQuery(ReturnLink, RequestNameSubForm, SubFormActions)
                ReturnLink = kmaEncodeRequestVariable(ReturnLink)
                Description = "<B>Actions to take</B><BR><BR>Enter the actions you would like to be taken when the form set is completed."
                Content = Content _
                    & "<div>" _
                    & "<TABLE border=0 cellpadding=10 cellspacing=0 width=""100%"">" _
                    & "<TR>" _
                    & "<TD width=100 align=right>Join&nbsp;Group&nbsp;<br><img src=/cclib/images/spacer.gif width=100 height=1></td>" _
                    & "<td width=99% align=left>" & Main.GetFormInputSelect(RequestNameJoinGroupID, Formset.JoinGroupID, "Groups") & "</td>" _
                    & "<TD width=50 align=center><a href=""" & Main.SiteProperty_AdminURL & "?af=4&cid=" & Main.getContentId("Groups") & "&EditReferer=" & ReturnLink & """><img src=""/cclib/images/IconContentAdd.gif"" border=0 width=18 height=22></a></td>" _
                    & "</tr>" _
                    & "<TR>" _
                    & "<TD width=100 align=right>Response&nbsp;Email&nbsp;<br><img src=/cclib/images/spacer.gif width=100 height=1></td>" _
                    & "<td width=99% align=left>" & Main.GetFormInputSelect(RequestNameResponseEmailID, Formset.ResponseEmailID, "System Email") & "</td>" _
                    & "<TD width=50 align=center><a href=""" & Main.SiteProperty_AdminURL & "?af=4&cid=" & EmailCID & "&EditReferer=" & ReturnLink & """><img src=""/cclib/images/IconContentAdd.gif"" border=0 width=18 height=22></a></td>" _
                    & "</tr>" _
                    & "<TR>" _
                    & "<TD width=100 align=right>Notification&nbsp;Email&nbsp;<br><img src=/cclib/images/spacer.gif width=100 height=1></td>" _
                    & "<td width=99% align=left>" & Main.GetFormInputSelect(RequestNameNotificationEmailID, Formset.NotificationEmailID, "System Email") & "</td>" _
                    & "<TD width=50 align=center><a href=""" & Main.SiteProperty_AdminURL & "?af=4&cid=" & EmailCID & "&EditReferer=" & ReturnLink & """><img src=""/cclib/images/IconContentAdd.gif"" border=0 width=18 height=22></a></td>" _
                    & "</tr>" _
                    & "</table>" _
                    & "</div>" _
                    & ""
                WizardContent = Main.GetWizardContent(HeaderCaption, ButtonCancel & "," & ButtonBack, ButtonContinue, True, True, Description, Content)
            Case SubFormFinished
                '
                ' Actions to take
                '
                Description = "<B>Finished</B><BR><BR>All the information about your form has been collected. click the Finish button to complete the setup, making your form ready to use. To use your form, edit a page, and use the 'Tag' menu to select the 'Dynamic Form' dropin. When dropped on the page, double click it and select this form (" & Formset.Name & ")"
                WizardContent = Main.GetWizardContent(HeaderCaption, "", ButtonFinish, True, True, Description, Content)
            Case Else
        End Select
        '
        WizardContent = "" _
            & cr & "<div class=""formWizard"">" _
            & kmaIndent(WizardContent) _
            & cr & "</div>"
        GetForm = Main.GetAdminFormBody("", "", "", True, True, "", "", 20, WizardContent)
    End If
    '
    Exit Function
ErrorTrap:
    Call HandleClassTrapError("GetForm")
End Function
'
'=================================================================================
'   Process the HTML body and create fields
'       returns the user error if there is a problem
'
'=================================================================================
'
Private Function ProcessHTMLFormReturnError(Main As Object, Formset As FormSetType) As String
    On Error GoTo ErrorTrap
    '
    Dim ChrPtr As Long
    Dim chr As String
    Dim CSField As Long
    Dim SQL As String
    Dim DataContentID As Long
    Dim InputRequired As Boolean
    Dim HTML As New kmaHTML.ParseClass
    Dim ElementPtr As Long
    Dim Copy As String
    Dim ElementTag As String
    Dim fs As New FastStringClass
    Dim InputType As String
    Dim InputName As String
    Dim InputValue As String
    Dim FieldFound As Boolean
    Dim FieldPtr As Long
    '
    With Formset.Forms(Formset.FormPtr)
        If .ContentID <> 0 Then
            '
            ' Insert into this content
            '
            DataContentID = .ContentID
        ElseIf .NewContentName <> "" Then
            '
            ' Create new content, use the new one
            '
'            Call Main.CreateContentFromSQLTable("default", "DynamicForm" & .NewContentName, "Dynamic Form " & .NewContentName)
 '           .ContentID = Main.GetContentID("Dynamic Form " & .NewContentName)
  '          DataContentID = .ContentID
        ElseIf .UseAuthMemberContent Then
            '
            ' Update the member record
            '
            DataContentID = Main.getContentId("people")
        ElseIf .UseAuthOrgContent Then
            '
            ' Update the organization record
            '
            DataContentID = Main.getContentId("Organizations")
        End If
        If InStr(1, .HTMLBody, "<") = 0 Then
            '
            ' No html tags
            '
        Else
            '
            ' parse the fieldnames out
            '
            Call HTML.Load(.HTMLBody)
            '
            ' mark all the loaded fields as deleted
            '
            For FieldPtr = 0 To .FieldCnt - 1
                .Fields(FieldPtr).Deleted = True
            Next
            '
            ' load all tags from form body, clear deleted flags
            '
            ElementPtr = 0
            If HTML.ElementCount > 0 Then
                ElementPtr = 0
                Do While ElementPtr < HTML.ElementCount
                    Copy = HTML.Text(ElementPtr)
                    If HTML.IsTag(ElementPtr) Then
                        ElementTag = UCase(HTML.TagName(ElementPtr))
                        Select Case ElementTag
                            Case "FORM", "/FORM"
                                Copy = ""
                            Case "INPUT", "SELECT", "TEXTAREA"
                                '
                                If ElementTag = "TEXTAREA" Then
                                    InputType = "TEXTAREA"
                                ElseIf ElementTag = "INPUT" Then
                                    InputType = UCase(HTML.ElementAttribute(ElementPtr, "type"))
                                    If InputType = "" Then
                                        InputType = "TEXT"
                                    End If
                                ElseIf ElementTag = "SELECT" Then
                                    InputType = "SELECT"
                                End If
                                '
                                ' Build fields for a text input
                                '
                                If InputType = "SUBMIT" Then
                                    '
                                    ' Buttons use the value and the name
                                    '
                                    InputName = Trim(HTML.ElementAttribute(ElementPtr, "VALUE"))
                                Else
                                    '
                                    ' All other inputs use the name
                                    '
                                    InputName = Trim(HTML.ElementAttribute(ElementPtr, "name"))
                                End If
                                InputValue = Trim(HTML.ElementAttribute(ElementPtr, "value"))
                                If (LCase(InputType) = "checkbox") And (InputValue = "") Then
                                    InputValue = "Yes"
                                    Copy = "<input type=""checkbox"" name=""" & InputName & """ value=""Yes"">"
                                End If
                                '
                                ' Test for name >=3 characters
                                '
                                If Len(InputName) < 3 Then
                                    ProcessHTMLFormReturnError = ProcessHTMLFormReturnError & "<div class=ccError>Field """ & InputName & """ is not allowed because field names must be at least 3 characters</div>"
                                    'Exit Function
                                Else
                                    '
                                    ' Test for a required marker (* in the fieldname)
                                    '
                                    InputRequired = False
                                    'FieldName = InputName
                                    If InStr(1, InputName, "*") <> 0 Then
                                        InputRequired = True
                                        InputName = Replace(InputName, "*", "")
                                    End If
                                    ''
                                    '' Test for >=3 chacters
                                    ''
                                    'If Len(InputName) < 3 Then
                                    '    ProcessHTMLFormReturnError = ProcessHTMLFormReturnError & "<div class=ccError>Field """ & InputName & """ is not allowed because field names must be at least 3 characters</div>"
                                    '    'Exit Function
                                    'End If
                                    '
                                    ' See if there is already a .field record for this name on this form
                                    '
                                    FieldFound = False
                                    For FieldPtr = 0 To .FieldCnt - 1
                                        If UCase(InputName) = UCase(.Fields(FieldPtr).Name) Then
                                            FieldFound = True
                                            '
                                            ' Update what might have changed
                                            '
                                            .Fields(FieldPtr).InputType = InputType
                                            .Fields(FieldPtr).Deleted = False
                                            .Fields(FieldPtr).Required = InputRequired
                                            Exit For
                                        End If
                                    Next
                                    If Not FieldFound Then
                                    'If Not FieldFound And (DataContentID <> 0) Then
                                        '
                                        ' Add a new field record if not found
                                        '
                                        FieldPtr = .FieldCnt
                                        ReDim Preserve .Fields(.FieldCnt + 1)
                                        With .Fields(FieldPtr)
                                            '
                                            ' Save field
                                            '
                                            .InputType = InputType
                                            .Name = InputName
                                            .Deleted = False
                                            .Required = InputRequired
                                            'Formset.Forms(Formset.FormPtr).FieldCnt = Formset.Forms(Formset.FormPtr).FieldCnt + 1
                                        End With
                                        .FieldCnt = .FieldCnt + 1
                                    End If
                                    If (DataContentID <> 0) Then
                                    'If (DataContentID <> 0) And (InputType <> "SUBMIT") Then
                                        '
                                        ' Test for only alphanumeric
                                        '
                                        For ChrPtr = 1 To Len(InputName)
                                            chr = Mid(InputName, ChrPtr, 1)
                                            If InStr(1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", chr, vbTextCompare) = 0 Then
                                                ProcessHTMLFormReturnError = ProcessHTMLFormReturnError & "<div class=ccError>Field """ & InputName & """ is not allowed because field names must only use characters A-Z and 0-9</div>"
                                                'Exit Function
                                            End If
                                        Next
                                        '
                                        '
                                        '
                                        With .Fields(FieldPtr)
                                            '
                                            ' Get Content information for the field
                                            '
                                            SQL = "select top 1 * from ccFields where ContentID=" & DataContentID & " and (name=" & KmaEncodeSQLText(.Name) & ") order by id"
                                            CSField = Main.OpenCSSQL("default", SQL, 1, 1)
                                            If Main.iscsok(CSField) Then
                                                '
                                                ' field matches a content field, save associations
                                                '
                                                .ContentFieldID = Main.GetCSInteger(CSField, "ID")
                                               ' .Required = Main.GetCSBoolean(CSField, "required")
                                            End If
                                            Call Main.closeCs(CSField)
                                        End With
                                    End If
                                
                                End If
                        End Select
                    End If
                    fs.Add Copy
                    ElementPtr = ElementPtr + 1
                Loop
            End If
            .HTMLBody = fs.Text
        End If
    End With
    '
    Exit Function
ErrorTrap:
    Call HandleClassTrapError("GetForm")
End Function
'
'
'
Private Sub Class_Initialize()
End Sub
'
'
'
Private Sub Class_Terminate()
End Sub
'
'
'
Private Sub HandleClassTrapError(MethodName As String, Optional ignore0 As String)
    Call HandleError("EmailWizardClass", MethodName, Err.Number, Err.Source, Err.Description, True, False)
End Sub
'
'
'
Private Sub HandleForumInternalError(MethodName As String, ErrDescription As String)
    Call Err.Raise(KmaErrorInternal, App.EXEName, ErrDescription)
End Sub
'
'
'
Private Function GetFormSet(Main As Object, FormSetID As Long) As FormSetType
    On Error GoTo ErrorTrap
    '
    Dim CSFormSet As Long
    Dim CSForm As Long
    Dim FormPtr As Long
    Dim FormSize As Long
    Dim CSField As Long
    Dim FieldSize As Long
    Dim FieldPtr As Long
    '
    CSFormSet = Main.OpenCSContentRecord("Form Sets", FormSetID)
    If Main.iscsok(CSFormSet) Then
        GetFormSet.Id = FormSetID
        GetFormSet.Name = Main.GetCSText(CSFormSet, "name")
        GetFormSet.Active = Main.GetCSBoolean(CSFormSet, "active")
        GetFormSet.JoinGroupID = Main.GetCSInteger(CSFormSet, "JoinGroupID")
        GetFormSet.NotificationEmailID = Main.GetCSInteger(CSFormSet, "NotificationEmailID")
        GetFormSet.ResponseEmailID = Main.GetCSInteger(CSFormSet, "ResponseEmailID")
        GetFormSet.ThankYouCopy = Main.GetCSText(CSFormSet, "ThankYouCopy")
    End If
    Call Main.closeCs(CSFormSet)
    '
    ' Load in the forms
    '
    If GetFormSet.Id <> 0 Then
        CSForm = Main.openCsContent("Forms", "FormSetID=" & GetFormSet.Id, "SortOrder,ID")
        FormSize = 0
        FormPtr = 0
        Do While Main.iscsok(CSForm)
            If FormPtr >= FormSize Then
                FormSize = FormSize + 5
                ReDim Preserve GetFormSet.Forms(FormSize)
            End If
            With GetFormSet.Forms(FormPtr)
                .Id = Main.GetCSInteger(CSForm, "ID")
                .Name = Main.GetCSText(CSForm, "name")
                If Trim(.Name) = "" Then
                    .Name = "Form " & CStr(.Id)
                End If
                .AddBackButton = Main.GetCSBoolean(CSForm, "AddBackButton")
                .BackButtonName = Main.GetCSText(CSForm, "BackButtonName")
                .AddCancelButton = Main.GetCSBoolean(CSForm, "AddCancelButton")
                .CancelButtonName = Main.GetCSText(CSForm, "CancelButtonName")
                .AddContinueButton = Main.GetCSBoolean(CSForm, "AddContinueButton")
                .ContinueButtonName = Main.GetCSText(CSForm, "ContinueButtonName")
                .HTMLBody = Main.GetCSText(CSForm, "HTMLBody")
                .NextFormID = Main.GetCSInteger(CSForm, "NextFormID")
                .ContentID = Main.GetCSInteger(CSForm, "ContentID")
                .NewContentName = Main.GetCSText(CSForm, "NewContentName")
                .UseAuthMemberContent = Main.GetCSBoolean(CSForm, "UseAuthMemberContent")
                .UseAuthOrgContent = Main.GetCSBoolean(CSForm, "UseAuthOrgContent")
                .SortOrder = Main.GetCSText(CSForm, "SortOrder")
                '
                ' Load in Form fields
                '
                CSField = Main.openCsContent("Form Fields", "FormID=" & .Id)
                FieldSize = 0
                .FieldCnt = 0
                Do While Main.iscsok(CSField)
                    If .FieldCnt >= FieldSize Then
                        FieldSize = FieldSize + 5
                        ReDim Preserve GetFormSet.Forms(FormPtr).Fields(FieldSize)
                    End If
                    With .Fields(.FieldCnt)
                        .Name = Main.GetCSText(CSField, "Name")
                        .Id = Main.GetCSInteger(CSField, "ID")
                        .Required = Main.GetCSBoolean(CSField, "Required")
                        .InputType = Main.GetCSText(CSField, "InputType")
                        .ContentFieldID = Main.GetCSInteger(CSField, "ContentFieldID")
                        .Caption = Main.GetCSText(CSField, "Caption")
                        .ReplaceText = Main.GetCSText(CSField, "ReplaceText")
                        .ButtonActionID = Main.GetCSInteger(CSField, "ButtonActionID")
                    End With
                    .FieldCnt = .FieldCnt + 1
                    Main.NextCSRecord (CSField)
                Loop
                Call Main.closeCs(CSField)
            End With
            
            FormPtr = FormPtr + 1
            Main.NextCSRecord (CSForm)
        Loop
        Call Main.closeCs(CSForm)
        GetFormSet.FormCnt = FormPtr
    End If
    '
    Exit Function
ErrorTrap:
    Call HandleClassTrapError("GetFormSet")
End Function
'
'
'
Private Sub SaveFormSet(Main As Object, Formset As FormSetType)
    On Error GoTo ErrorTrap
    '
    Dim SQL As String
    Dim CSFormSet As Long
    Dim CSForm As Long
    Dim FormPtr As Long
    Dim FormSize As Long
    Dim CSField As Long
    Dim FieldSize As Long
    Dim FieldPtr As Long
    '
    If Formset.Id = 0 Then
        Formset.Id = Main.InsertContentRecordGetID("Form Sets")
        Call Main.SetVisitProperty("FormWizardSetID", Formset.Id)
        '
        ' for now, mark them all active. problem is that it is not ready to go on the site. If active is used to block it, then you can not come back to the
        ' editor and change it, because it is not active until the form is completed. Leave and come back and everything is lost.
        '
        Formset.Active = True
        'Formset.Active = 0
    End If
    CSFormSet = Main.OpenCSContentRecord("Form Sets", Formset.Id)
    If Main.iscsok(CSFormSet) Then
        Call Main.SetCS(CSFormSet, "name", Formset.Name)
        Call Main.SetCS(CSFormSet, "active", Formset.Active)
        Call Main.SetCS(CSFormSet, "JoinGroupID", Formset.JoinGroupID)
        Call Main.SetCS(CSFormSet, "NotificationEmailID", Formset.NotificationEmailID)
        Call Main.SetCS(CSFormSet, "ResponseEmailID", Formset.ResponseEmailID)
        Call Main.SetCS(CSFormSet, "ThankYouCopy", Formset.ThankYouCopy)
        '
        ' Save the forms
        '
Dim FormID As Long
        For FormPtr = 0 To Formset.FormCnt - 1
            With Formset.Forms(FormPtr)
                If .Deleted Then
                    If .Id <> 0 Then
                        Call Main.DeleteContentRecord("Forms", .Id)
                    End If
                Else
                    If .Id = 0 Then
                        .Id = Main.InsertContentRecordGetID("Forms")
                    End If
                    FormID = .Id
                    CSForm = Main.OpenCSContentRecord("Forms", FormID)
                    If Main.iscsok(CSForm) Then
                        Call Main.SetCS(CSForm, "name", .Name)
                        Call Main.SetCS(CSForm, "FormSetID", Formset.Id)
                        Call Main.SetCS(CSForm, "AddBackButton", .AddBackButton)
                        Call Main.SetCS(CSForm, "BackButtonName", .BackButtonName)
                        Call Main.SetCS(CSForm, "AddCancelButton", .AddCancelButton)
                        Call Main.SetCS(CSForm, "CancelButtonName", .CancelButtonName)
                        Call Main.SetCS(CSForm, "AddContinueButton", .AddContinueButton)
                        Call Main.SetCS(CSForm, "ContinueButtonName", .ContinueButtonName)
                        Call Main.SetCS(CSForm, "HTMLBody", .HTMLBody)
                        Call Main.SetCS(CSForm, "NextFormID", .NextFormID)
                        Call Main.SetCS(CSForm, "ContentID", .ContentID)
                        Call Main.SetCS(CSForm, "NewContentName", .NewContentName)
                        Call Main.SetCS(CSForm, "UseAuthMemberContent", .UseAuthMemberContent)
                        Call Main.SetCS(CSForm, "UseAuthOrgContent", .UseAuthOrgContent)
                        Call Main.SetCS(CSForm, "SortOrder", GetIntegerString(FormPtr, 10))
                        '
                        ' Mark previous form fields inactive
                        '
                        SQL = "update ccFormFields set active=0 where formid=" & FormID
                        Call Main.ExecuteSQL("default", SQL)
                        '
                        ' Save Form fields
                        '
                        For FieldPtr = 0 To .FieldCnt - 1
                            With .Fields(FieldPtr)
                                If .Name <> "" And Not .Deleted Then
                                    If .Id = 0 Then
                                        CSField = Main.openCsContent("Form Fields", "(formid=" & FormID & ")and(Name=" & KmaEncodeSQLText(.Name) & ")", , False)
                                        If Main.iscsok(CSField) Then
                                            .Id = Main.GetCSInteger(CSField, "ID")
                                        End If
                                        Call Main.closeCs(CSField)
                                        If .Id = 0 Then
                                            .Id = Main.InsertContentRecordGetID("Form Fields")
                                        End If
                                    End If
                                    CSField = Main.OpenCSContentRecord("Form Fields", .Id)
                                    If Main.iscsok(CSField) Then
                                        Call Main.SetCS(CSField, "Name", .Name)
                                        Call Main.SetCS(CSField, "Active", True)
                                        Call Main.SetCS(CSField, "Caption", .Caption)
                                        Call Main.SetCS(CSField, "ContentFieldID", .ContentFieldID)
                                        Call Main.SetCS(CSField, "ReplaceText", .ReplaceText)
                                        Call Main.SetCS(CSField, "Required", .Required)
                                        Call Main.SetCS(CSField, "InputType", .InputType)
                                        Call Main.SetCS(CSField, "ButtonActionID", .ButtonActionID)
                                        'Call Main.SetCS(CSField, "Required", .Required)
                                        Call Main.SetCS(CSField, "FormID", FormID)
                                    End If
                                End If
                            End With
                            Call Main.closeCs(CSField)
                        Next
                        '
                        ' Delete inactive fields
                        '
                        SQL = "delete from ccFormFields where active<>1 and formid=" & FormID
                        Call Main.ExecuteSQL("default", SQL)
                    End If
                    Call Main.closeCs(CSForm)
                End If
            End With
        Next
        'Formset.FormCnt = FormPtr
    End If
    Call Main.closeCs(CSFormSet)
    '
    Exit Sub
ErrorTrap:
    Call HandleClassTrapError("SaveFormSet")
End Sub
'
'
'
Public Function GetDynamicForm(Main As Object, FormSetName As String) As String
    On Error GoTo ErrorTrap
    '
    Dim isAuthenticated As Boolean
    Dim isRecognized As Boolean
    Dim VirtualPath As String
    Dim Hint As String
    Dim RepeatFormForUserError As Boolean
    Dim RenderedHTMLBody As String
    Dim BlockForm As Boolean
    Dim ClientsWereSane As Boolean
    Dim OriginialName As String
    Dim DbValue As String
    Dim DbVAlueList As String
    Dim DbValues() As String
    Dim cnt As Long
    Dim Ptr As Long
    Dim OptionValue As String
    Dim ButtonBar As String
    Dim FirstName As String
    Dim LastName As String
    Dim Name As String
    Dim ErrorMsg As String
    Dim QS As String
    Dim CS As Long
    Dim FieldCnt As Long
    Dim FieldName As String
    Dim FieldValue As String
    Dim FormName As String
    Dim FormResponsePtr As Long
    Dim UserFormResponseID As Long
    Dim GroupName As String
    Dim EmailName As String
    Dim FormPtr As Long
    Dim EmailBody As String
    Dim InputValue As String
    Dim fs As New FastString.FastStringClass
    Dim SQL As String
    Dim FieldPtr As Long
    Dim FieldFound As Boolean
    Dim InputName As String
    Dim InputType As String
    Dim ElementTag As String
    Dim Copy As String
    Dim ElementPtr As Long
    Dim RequestNameDataRecordID As String
    Dim FormButtonCancel As String
    Dim FormButtonback As String
    Dim ButtonCnt As Long
    Dim DataFieldName As String
    Dim DataContentName As String
                            Dim formContentId As Integer
    Dim DataRecordID As Long
    Dim DataContentID As Long
    Dim ReplaceText As String
    Dim ReplaceFormINput As String
    Dim CSData As Long
    Dim CSFormSet As Long
    Dim CSForm As Long
    Dim CSFields As Long
    Dim FormID As Long
    Dim FormSetID As Long
    Dim AdminHint As String
    Dim Formset As FormSetType
    Dim HTML As New kmaHTML.ParseClass
    Dim Button As String
    Dim ButtonAction As Long
    Dim PeopleContentID As Long
    Dim usesAccountAccess As Boolean
    '
    ' Check for MenuID - if present, arguments are in the Dynamic Menu content - else it is old, and they are in the SegmentCMDargs
    '
Hint = "1"
    If Main.SiteProperty_BuildVersion <= "3.3.575" Then
        '
        ' Old Db
        '
        GetDynamicForm = "<p>Your website database needs to be upgraded before this feature can be used.</p>"
        Call Main.TestPoint("GetDynamicForm failed because database needs to be upgraded")
    Else
        '
        ' Load the formset
        '
        FormSetID = Main.GetRecordID("Form Sets", FormSetName)
        Formset = GetFormSet(Main, FormSetID)
        Formset.FormPtr = Main.GetStreamInteger(RequestNameFormPtr)
        UserFormResponseID = kmaEncodeInteger(Main.GetVisitProperty("FormWizard-UserResponseID"))
        'UserFormResponseID = Main.GetStreamInteger(RequestNameUserFormResponseID)
        Formset.FormsetResponse = GetFormsetResponse(Main, UserFormResponseID)
        RenderedHTMLBody = ""
        '
'        usesAccountAccess = False
'        For Ptr = 0 To Formset.FormCnt - 1
'            usesAccountAccess = usesAccountAccess Or Formset.Forms(Ptr).UseAuthMemberContent Or Formset.Forms(Ptr).UseAuthOrgContent
'        Next
'        isAuthenticated = Main.isAuthenticated
'        isRecognized = Main.isRecognized
'        If (Not isAuthenticated) And isRecognized Then
'        'If usesAccountAccess And (Not isAuthenticated) And isRecognized Then
'            '
'            ' if the formset uses AuthMemberContent, you must be either authenticated or not-recognized
'            '   if not authenticated and recognized, log out and log in as a real guest
'            '
'            Call Main.LogoutMember
'            Call Main.LoginMemberByID(Main.memberId)
'        End If
'        '
'        ' if the current account has a username, you must be authenticated to access it
'        '
'        BlockForm = False
'        If (Main.MemberUsername <> "") And Not Main.IsAuthenticated Then
'            PeopleContentID = Main.GetContentID("people")
'            For Ptr = 0 To Formset.FormCnt - 1
'                BlockForm = BlockForm Or Formset.Forms(Ptr).UseAuthMemberContent Or Formset.Forms(Ptr).UseAuthOrgContent
'            Next
'        End If
Hint = "2"
        If True Then
'        If BlockForm Then
'            '
'            ' Security Issue
'            '
'            QS = Main.RefreshQueryString
'            If QS = "" Then
'                QS = "?"
'            Else
'                QS = "?" & QS & "&"
'            End If
'            GetDynamicForm = "<p>To complete this form using your current website account, you must first log-in. <a href=""" & QS & "method=login"">Click here to log-in</a>. If you do not have an account, <a href=""" & QS & "method=logout"">click here</a> to start over with a new account.</p>"
'            Call Main.TestPoint("GetDynamicForm failed because database needs to be upgraded")
'        Else
            '
            ' ----- Process the incoming form
            '
            Button = Main.GetStreamText("Button")
            If Button <> "" Then
                '
                ' Determine button action
                '
                With Formset.Forms(Formset.FormPtr)
                    If .AddBackButton Then
                        If ((.BackButtonName = "") And (Button = ButtonBack)) Or (Button = .BackButtonName) Then
                            ButtonAction = ButtonActionBack
                        End If
                    End If
                    If (ButtonAction = 0) And .AddCancelButton Then
                        If ((.BackButtonName = "") And (Button = ButtonCancel)) Or (Button = .CancelButtonName) Then
                            ButtonAction = ButtonActionCancel
                        End If
                    End If
                    If (ButtonAction = 0) And .AddContinueButton Then
                        If ((.BackButtonName = "") And (Button = ButtonContinue)) Or (Button = .ContinueButtonName) Then
                            ButtonAction = ButtonActionContinue
                        End If
                    End If
                    If (ButtonAction = 0) Then
                        For FieldPtr = 0 To .FieldCnt - 1
                            With .Fields(FieldPtr)
                                If .InputType = "SUBMIT" And .Name = Button Then
                                'If .InputType = "SUBMIT" And .Name = Button Then
                                    ButtonAction = .ButtonActionID
                                    Exit For
                                End If
                            End With
                        Next
                    End If
                    Select Case ButtonAction
                        Case ButtonActionCancel
                            '
                            ' Cancel the form
                            '
                        Case ButtonActionBack
                            '
                            ' Go back to the previous form in the formset, no save
                            '
                            If Formset.FormPtr > 0 Then
                                Formset.FormPtr = Formset.FormPtr - 1
                            End If
                        Case ButtonActionContinue
                            '
                            ' Process the form input and move on
                            '
                            If Formset.FormsetResponse.Id = 0 Then
                                CS = Main.InsertCSRecord("user form response")
                                If Main.IsCSOK(CS) Then
                                    Formset.FormsetResponse.Id = Main.GetCSInteger(CS, "ID")
                                    Call Main.SetVisitProperty("FormWizard-UserResponseID", Formset.FormsetResponse.Id)
                                End If
                                Call Main.CloseCS(CS)
                            End If
                            DataContentID = .ContentID
                            CSData = -1
                            If (DataContentID <> 0) Or .UseAuthMemberContent Or .UseAuthOrgContent Then
                                DataRecordID = 0
                                If DataContentID = 0 Then
                                    '
                                    ' use auth content, like people or organizations
                                    '
                                    CSData = OpenCSData(Main, 0, 0, .UseAuthMemberContent, .UseAuthOrgContent)
                                Else
                                    '
                                    ' Use custom content (not people or organizations)
                                    '
                                    DataContentName = Main.GetContentNameByID(DataContentID)
                                    RequestNameDataRecordID = "C" & DataContentID & "ID"
Dim vpFormSetFormContentRecordId As String
                                    vpFormSetFormContentRecordId = "formsetResponse" & Formset.FormsetResponse.Id & "Content" & DataContentID
                                    DataRecordID = kmaEncodeInteger(Main.GetVisitProperty(vpFormSetFormContentRecordId))
                                    'DataRecordID = Main.GetStreamInteger(RequestNameDataRecordID)
                                    If DataRecordID = 0 Then
                                        CSData = Main.InsertCSRecord(DataContentName)
                                        If Main.IsCSOK(CSData) Then
                                            Call Main.SetCS(CSData, "name", "Dynamic Form Response " & Formset.FormsetResponse.Id)
                                            DataRecordID = Main.GetCSInteger(CSData, "id")
                                            '
                                            '   Save the recordID to a visit property - that way outside add-ons can access the newly created data
                                            '
                                            Call Main.SetVisitProperty(vpFormSetFormContentRecordId, DataRecordID)
                                            'Call Main.SetVisitProperty("Dynamic Form Data Record ID", DataRecordID)
                                        End If
                                    Else
                                        CSData = Main.OpenCSContentRecord(DataContentName, DataRecordID)
                                    End If
                                End If
                                If Not Main.iscsok(CSData) Then
                                    Call Main.TestPoint("GetDynamicForm, form field mapping source, contentid [" & DataContentID & "] did not open correctly")
                                End If
                            End If
                            '
                            ' Setup the Form Response storage for the form input
                            '
                            formContentId = .ContentID
                            FormName = .Name
                            FieldCnt = .FieldCnt
                            With Formset.FormsetResponse
                                If .FormResponseCnt > 0 Then
                                    For FormResponsePtr = 0 To .FormResponseCnt - 1
                                        If FormName = .FormResponse(FormResponsePtr).FormName Then
                                            Exit For
                                        End If
                                    Next
                                End If
                                If FormResponsePtr >= .FormResponseCnt Then
                                    ReDim Preserve .FormResponse(FormResponsePtr)
                                    .FormResponse(FormResponsePtr).FormName = FormName
                                    .FormResponseCnt = FormResponsePtr + 1
                                End If
                                ReDim Preserve .FormResponse(FormResponsePtr).Fields(FieldCnt)
                                .FormResponse(FormResponsePtr).FieldCnt = FieldCnt
                            End With
                            '
                            ' check if all fields are ok
                            '
                            Dim csUser As Integer
                            Dim peopleCid As Integer
                            Dim UseAuthMemberContent As Boolean
                            UseAuthMemberContent = .UseAuthMemberContent
                            peopleCid = Main.getContentId("people")
                            For FieldPtr = 0 To FieldCnt - 1
                                With .Fields(FieldPtr)
                                    FieldName = .Name
                                    FieldValue = Main.GetStreamText(.Name)
                                    If ((UseAuthMemberContent) Or (formContentId = peopleCid)) And (LCase(FieldName) = "username") Then
                                        '
                                        ' this is the username or a people record
                                        '
                                        csUser = Main.openCsContent("people", "(username=" & KmaEncodeSQLText(FieldValue) & ")and(id<>" & Main.memberId & ")")
                                        If Main.iscsok(csUser) Then
                                            Call Main.AddUserError("The username you entered is in use. Please enter a different username. If this is your account, please login first.")
                                        End If
                                        Call Main.closeCs(csUser)
                                    End If
                                End With
                            Next
                            If Not Main.IsUserError Then
                                '
                                ' must be logged in or logged out (cannot be recognized)
                                '
                                isAuthenticated = Main.isAuthenticated
                                isRecognized = Main.isRecognized
                                If (Not isAuthenticated) And ((.UseAuthMemberContent) Or (.UseAuthOrgContent)) Then
                                    '
                                    ' you are not authenticated and not recognized, but it is going to update your people record
                                    '
                                    If isRecognized Then
                                        Call Main.LogoutMember
                                    End If
                                    Call Main.LoginMemberByID(Main.memberId)
                                End If
                                '
                                ' save the response
                                '
                                For FieldPtr = 0 To FieldCnt - 1
                                    With .Fields(FieldPtr)
                                        FieldName = .Name
                                        FieldValue = Main.GetStreamText(.Name)
                                        If .InputType = "FILE" Then
                                            FieldName = FieldName
                                            VirtualPath = GetRandomInteger()
                                            VirtualPath = "Upload\FormWizard\" & String(12 - Len(VirtualPath), "0") & VirtualPath & "\"
                                            FieldValue = Main.ProcessFormInputFile(FieldName, VirtualPath)
                                        End If
                                        If LCase(FieldName) = "firstname" Then
                                            FirstName = FieldValue
                                        ElseIf LCase(FieldName) = "lastname" Then
                                            LastName = FieldValue
                                        ElseIf LCase(FieldName) = "name" Then
                                            Name = FieldValue
                                        End If
                                        If .Required And FieldValue = "" Then
                                            '
                                            ' Required field is missing
                                            '
                                            Call Main.AddUserError(FieldName & " is a required field.")
                                            'ErrorMsg = ErrorMsg & "<div class=ccError>" & FieldName & " is a required field.</div>"
                                        Else
                                            '
                                            ' Update the User Response Fiels
                                            '
                                            Formset.FormsetResponse.FormResponse(FormResponsePtr).Fields(FieldPtr).Name = FieldName
                                            Formset.FormsetResponse.FormResponse(FormResponsePtr).Fields(FieldPtr).Value = FieldValue
                                            '
                                            ' Update the content
                                            '
                                            If Main.iscsok(CSData) And .ContentFieldID <> 0 Then
                                                Call Main.SetCS(CSData, FieldName, FieldValue)
                                            End If
                                        End If
                                    End With
                                Next
                                '
                                ' set name from firstname and lastname
                                '
                                If Main.iscsok(CSData) And (FirstName <> "") And (LastName <> "") And (Name = "") Then
                                    Call Main.SetCS(CSData, "name", FirstName & " " & LastName)
                                End If
                            End If
                            '
                            ' REnder the HTML copy for the page so any Add-ons get a chance to post there errors, throw away result
                            '
                            RenderedHTMLBody = Main.EncodeContentForWeb(.HTMLBody, "Forms", .Id, "", kmaEncodeInteger(Main.GetSiteProperty("DefaultWrapperID", "0")))
                            '
                            RepeatFormForUserError = Main.IsUserError()
                            If Not RepeatFormForUserError Then
                            'If ErrorMsg = "" Then
                                '
                                ' Save response and move on to the next form, or go to the thankyou copy (Ptr=Cnt)
                                '
                                Call SaveFormsetResponse(Main, Formset, CSData)
                                Formset.FormPtr = Formset.FormPtr + 1
                            End If
                            Call Main.closeCs(CSData)
                    End Select
                End With
            End If
            '
            ' ----- Get the next form
            '
            Hint = "3"
            If Formset.FormPtr >= Formset.FormCnt Then
                '
                ' Done, Send Notification
                '
                Call Main.SetVisitProperty("FormWizard-UserResponseID", "")
                If Formset.NotificationEmailID <> 0 Then
                    Hint = "31"
                    EmailName = Main.GetRecordName("System Email", Formset.NotificationEmailID)
                    If EmailName <> "" Then
                        EmailBody = EmailBody _
                            & "<BR>" _
                            & "<BR>The Formset [" & Formset.Name & "] was completed " & Now() _
                            & "<BR>By " & Main.MemberName & " (member #" & Main.memberId & ")" _
                            & "<BR>" _
                            & "<BR>"
                        If ClientsWereSane Then
                            '
                            ' Send the response
                            '
                            CS = Main.OpenCSContentRecord("User Form Response", Formset.FormsetResponse.Id)
                            If Main.iscsok(CS) Then
                                Copy = Main.GetCS(CS, "copy")
                                Copy = Replace(Copy, "\n", vbCrLf)
                                Copy = Replace(Copy, vbCrLf, "<BR>")
                                EmailBody = EmailBody & Copy
                            End If
                            Call Main.closeCs(CS)
                        Else
                            Hint = "32"
                            '
                            ' Send the response to each form, in the original form
                            '
                            Dim HTMLBody As String
                            FormSetName = Formset.Name
                            For FormPtr = 0 To Formset.FormCnt - 1
                                Hint = "33"
                                With Formset.Forms(FormPtr)
                                    FormName = Formset.Forms(FormPtr).Name
                                    '
                                    ' Set the FormResponsePtr for this form
                                    '
                                    With Formset.FormsetResponse
                                        If .FormResponseCnt > 0 Then
                                            For FormResponsePtr = 0 To .FormResponseCnt - 1
                                                If FormName = .FormResponse(FormResponsePtr).FormName Then
                                                    Exit For
                                                End If
                                            Next
                                        End If
                                    End With
                                    HTMLBody = .HTMLBody
                                    
                                    Call HTML.Load(HTMLBody)
                                    '
                                    ' load all tags from form body, clear deleted flags
                                    '
                                    ElementPtr = 0
                                    Hint = "34"
                                    If HTML.ElementCount > 0 Then
                                        ElementPtr = 0
                                        Set fs = New FastStringClass
                                        Do While ElementPtr < HTML.ElementCount
                                            Hint = "341"
                                            Copy = HTML.Text(ElementPtr)
                                            If HTML.IsTag(ElementPtr) Then
                                                ElementTag = UCase(HTML.TagName(ElementPtr))
                                                Select Case ElementTag
                                                    Case "FORM", "/FORM"
                                                        Hint = "342"
                                                        Copy = ""
                                                    Case "INPUT", "SELECT", "TEXTAREA"
                                                        Hint = "343"
                                                        '
                                                        If ElementTag = "TEXTAREA" Then
                                                            InputType = "TEXTAREA"
                                                        ElseIf ElementTag = "INPUT" Then
                                                            InputType = UCase(HTML.ElementAttribute(ElementPtr, "type"))
                                                            If InputType = "" Then
                                                                InputType = "TEXT"
                                                            End If
                                                        ElseIf ElementTag = "SELECT" Then
                                                            InputType = "SELECT"
                                                        End If
                                                        '
                                                        ' Build fields for a text input
                                                        '
                                                        InputName = Trim(HTML.ElementAttribute(ElementPtr, "name"))
                                                        If InputName <> "" Then
                                                            '
                                                            ' remove required marker (* in the fieldname)
                                                            '
                                                            InputName = Replace(InputName, "*", "")
                                                            '
                                                            ' Find the response for this form field and replace the HTML tag
                                                            '
                                                            Select Case InputType
                                                                Case "SUBMIT"
                                                                    Hint = "344"
                                                                    Copy = "<span style=""border:1px solid #808080;font-weight:bold;"">Button</span>"
                                                                Case "CHECKBOX"
                                                                        Hint = "345"
                                                                        With Formset.FormsetResponse.FormResponse(FormResponsePtr)
                                                                            For FieldPtr = 0 To .FieldCnt - 1
                                                                                If UCase(InputName) = UCase(.Fields(FieldPtr).Name) Then
                                                                                    If (.Fields(FieldPtr).Value <> "") Then
                                                                                        Copy = "<input type=checkbox checked disabled />"
                                                                                        'Copy = "Yes"
                                                                                    Else
                                                                                        Copy = "<input type=checkbox disabled />"
                                                                                        'Copy = "No"
                                                                                    End If
                                                                                    'Copy = "<span style=""font-weight:bold;"">&nbsp;" & Copy & "&nbsp;</span>"
                                                                                    Exit For
                                                                                End If
                                                                            Next
                                                                            If FieldPtr >= .FieldCnt Then
                                                                                FieldPtr = FieldPtr
                                                                            End If
                                                                        End With
                                                                Case "SELECT"
                                                                        Hint = "346"
                                                                        With Formset.FormsetResponse.FormResponse(FormResponsePtr)
                                                                            '
                                                                            ' replace the tag with the answer
                                                                            '
                                                                            For FieldPtr = 0 To .FieldCnt - 1
                                                                                If UCase(InputName) = UCase(.Fields(FieldPtr).Name) Then
                                                                                    Copy = "<span style=""font-weight:bold;"">&nbsp;" & .Fields(FieldPtr).Value & "&nbsp;</span>"
                                                                                    Exit For
                                                                                End If
                                                                            Next
                                                                            '
                                                                            ' Skip to the /SELECT
                                                                            '
                                                                            Do While (ElementPtr < HTML.ElementCount) And (ElementTag <> "/SELECT")
                                                                                ElementPtr = ElementPtr + 1
                                                                                'Copy = HTML.Text(ElementPtr)
                                                                                ElementTag = ""
                                                                                If HTML.IsTag(ElementPtr) Then
                                                                                    ElementTag = UCase(HTML.TagName(ElementPtr))
                                                                                End If
                                                                            Loop
                                                                        End With
                                                                Case "RADIO"
                                                                        Hint = "347"
                                                                        InputValue = Trim(HTML.ElementAttribute(ElementPtr, "value"))
                                                                        With Formset.FormsetResponse.FormResponse(FormResponsePtr)
                                                                            For FieldPtr = 0 To .FieldCnt - 1
                                                                                If UCase(InputName) = UCase(.Fields(FieldPtr).Name) Then
                                                                                    If UCase(InputValue) = UCase(.Fields(FieldPtr).Value) Then
                                                                                        Copy = "<input type=radio checked disabled  />"
                                                                                        'Copy = "<span style=""font-weight:bold;"">&nbsp;Yes&nbsp;</span>"
                                                                                    Else
                                                                                        Copy = "<input type=radio disabled />"
                                                                                        'Copy = "&nbsp;No&nbsp;"
                                                                                    End If
                                                                                    Exit For
                                                                                End If
                                                                            Next
                                                                            If FieldPtr >= .FieldCnt Then
                                                                                FieldPtr = FieldPtr
                                                                            End If
                                                                        End With
                                                                Case "FILE"
                                                                        Hint = "348"
                                                                        With Formset.FormsetResponse.FormResponse(FormResponsePtr)
                                                                            For FieldPtr = 0 To .FieldCnt - 1
                                                                                If UCase(InputName) = UCase(.Fields(FieldPtr).Name) Then
                                                                                    Copy = "click to download <a href=""http://" & Main.ServerDomainPrimary & Main.serverFilePath & Replace(.Fields(FieldPtr).Value, "\", "/") & """><span style=""font-weight:bold;"">&nbsp;" & .Fields(FieldPtr).Value & "&nbsp;</span></a>"
                                                                                    Exit For
                                                                                End If
                                                                            Next
                                                                            If FieldPtr >= .FieldCnt Then
                                                                                FieldPtr = FieldPtr
                                                                            End If
                                                                        End With
                                                                Case Else
                                                                        Hint = "349, inputtype=" & InputType & ", inputname=" & InputName & ", FormResponsePtr=" & FormResponsePtr
                                                                        With Formset.FormsetResponse.FormResponse(FormResponsePtr)
                                                                            Hint = Hint & ", .FieldCnt=" & .FieldCnt
                                                                            For FieldPtr = 0 To .FieldCnt - 1
                                                                                Hint = Hint & ", FieldPtr=" & FieldPtr
                                                                                If UCase(InputName) = UCase(.Fields(FieldPtr).Name) Then
                                                                                    Hint = Hint & ", match"
                                                                                    Copy = "<span style=""font-weight:bold;"">&nbsp;" & .Fields(FieldPtr).Value & "&nbsp;</span>"
                                                                                    Exit For
                                                                                End If
                                                                            Next
                                                                            If FieldPtr >= .FieldCnt Then
                                                                                FieldPtr = FieldPtr
                                                                            End If
                                                                        End With
                                                            End Select
                                                        End If
                                                End Select
                                            End If
                                            fs.Add Copy
                                            ElementPtr = ElementPtr + 1
                                        Loop
                                    End If
                                End With
                                EmailBody = EmailBody _
                                    & "<div style=""margin-bottom:10px;border:1px solid #888;padding:0;"">" _
                                    & "<div style=""background-color:#dde;padding:10px;"">Form page <b>" & FormName & "</b> as it appeared on the page</div>" _
                                    & "<div style=""padding:10px;"">" & fs.Text & "</div>" _
                                    & "</div>"
Hint = "35"
                            Next
                        End If
Hint = "36"
                        Call Main.SendSystemEmail(EmailName, EmailBody)
                    End If
                End If
                '
                ' Send Response
                '
Hint = "4"
                If Formset.ResponseEmailID <> 0 Then
                    EmailName = Main.GetRecordName("System Email", Formset.ResponseEmailID)
                    If EmailName <> "" Then
                        Call Main.SendSystemEmail(EmailName, , Main.memberId)
                    End If
                End If
                '
                ' Join Group
                '
Hint = "5"
                If Formset.JoinGroupID <> 0 Then
                    GroupName = Main.GetGroupByID(Formset.JoinGroupID)
                    If GroupName <> "" Then
                        Call Main.AddGroupMember(GroupName)
                    End If
                End If
                '
                ' Thank you page
                '
Hint = "6"
                GetDynamicForm = GetDynamicForm & Formset.ThankYouCopy
            Else
                '
                ' Output the form
                '
Hint = "7"
                With Formset.Forms(Formset.FormPtr)
                    If Main.IsAdmin Then
                        If .UseAuthMemberContent Or .UseAuthOrgContent Then
                        '
                        ' Admin Warning
                        '
                        QS = ModifyQueryString(Main.RefreshQueryString, "method", "logout", True)
                        GetDynamicForm = GetDynamicForm & Main.GetAdminHintWrapper("" _
                            & "<div>Security Warning: This form will update your account. Administrators and Developers should not complete forms for other users. Your account may be changed, or you may accidentally grant admin access to others. Please log out before proceeding if you are completing this form for someone else.</div>" _
                            & "<div>&nbsp;</div>" _
                            & "<div style=""text-align:center;""><strong><a href=""?" & QS & """>Logout</a></strong></div>" _
                            & "")
                        End If
                    End If
                    ErrorMsg = Main.GetUserError()
                    If ErrorMsg <> "" Then
                        '
                        ' Error Message
                        '
                        GetDynamicForm = GetDynamicForm & "<div>" & ErrorMsg & "</div>"
                            
                        'GetDynamicForm = GetDynamicForm _
                        '    & "<div>&nbsp;</div>" _
                        '    & "<div class=ccError>There was a problem processing the form. Please make the necessary changes and try again.</div>" _
                        '    & "<div>&nbsp;</div>" _
                        '    & "<div style=""padding-left:20px;"">" & ErrorMsg & "</div>" _
                        '    & "<div>&nbsp;</div>" _
                        '    & ""
                    End If
                    '
                    ' Open the form's custom content to prepopulate the form
                    '   if the record has not been created, the CSData will be -1 and prepopulate will be skipped
                    '
                    If RepeatFormForUserError Then
                        '
                        ' Use the DataContent and DataRecordID from the save
                        '
                    Else
                        DataContentID = .ContentID
                        If DataContentID <> 0 Then
                            RequestNameDataRecordID = "C" & DataContentID & "ID"
                            DataRecordID = Main.GetStreamInteger(RequestNameDataRecordID)
                        End If
                    End If
                    CSData = OpenCSData(Main, DataContentID, DataRecordID, .UseAuthMemberContent, .UseAuthOrgContent)
                    '
                    ' Replace all form elements with the data content form inputs
                    '
                    Hint = "8"
                    If True Then
                        If RepeatFormForUserError And RenderedHTMLBody <> "" Then
                            '
                            ' Display the copy rendered during the input processing
                            '
                            GetDynamicForm = GetDynamicForm & RenderedHTMLBody
                        Else
                            '
                            ' Process the next forms copy
                            '
                            GetDynamicForm = GetDynamicForm & Main.EncodeContentForWeb(.HTMLBody, "Forms", .Id, "", kmaEncodeInteger(Main.GetSiteProperty("DefaultWrapperID", "0")))
                        End If
                        If InStr(1, GetDynamicForm, "<") = 0 Then
                            '
                            ' No html tags
                            '
                        Else
                            '
                            ' parse the field values in
                            '
                            Call HTML.Load(GetDynamicForm)
                            '
                            ElementPtr = 0
                            If HTML.ElementCount > 0 Then
                                ElementPtr = 0
                                Do While ElementPtr < HTML.ElementCount
                                    Copy = HTML.Text(ElementPtr)
                                    ElementTag = ""
                                    If HTML.IsTag(ElementPtr) Then
                                        ElementTag = UCase(HTML.TagName(ElementPtr))
                                        OriginialName = Trim(HTML.ElementAttribute(ElementPtr, "name"))
                                        InputName = Replace(OriginialName, "*", "")
                                        If InputName <> OriginialName Then
                                            Copy = Replace(Copy, OriginialName, InputName)
                                        End If
                                        InputValue = ""
    Dim LcaseInputName As String
    Dim fieldType As Long
                                        LcaseInputName = LCase(InputName)
                                        
                                        Select Case ElementTag
                                            Case "TEXTAREA"
                                                Copy = Copy
                                                'Copy = Replace(Copy, "value=", "xvalue=", , , vbTextCompare)
                                                If Main.IsCSFieldSupported(CSData, InputName) Then
                                                    fs.Add Copy
                                                    ElementPtr = ElementPtr + 1
                                                    '
                                                    ' client request - just gonna hope Mr. Guest Throkmorton stays away!
                                                    '
                                                    If Main.IsCSFieldSupported(CSData, InputName) Then
                                                        InputValue = Main.GetCSText(CSData, InputName)
                                                        fieldType = Main.GetCSFieldType(CSData, InputName)
                                                        Select Case fieldType
                                                            Case FieldTypeTextFile, FieldTypeHTMLFile
                                                                InputValue = Main.ReadVirtualFile(InputValue)
                                                            Case Else
                                                        End Select
                                                    End If
                                                    If (LcaseInputName = "name" Or LcaseInputName = "firstname") And (LCase(InputValue) = "guest") Then
                                                        InputValue = ""
                                                    End If
                                                    If HTML.IsTag(ElementPtr) Then
                                                        '
                                                        ' textarea was empty, add copy to fs object and continue
                                                        '
                                                        Call fs.Add(InputValue)
                                                        Copy = HTML.Text(ElementPtr)
                                                    Else
                                                        '
                                                        ' remove default copy, add our copy to fs object and continue
                                                        '
                                                        Copy = InputValue
                                                    End If
                                                End If
                                            Case "FORM", "/FORM"
                                                Copy = ""
                                            Case "INPUT"
                                                '
                                                'Copy = "<input type=hidden name=ContensiveUserForm value=1>"
                                                If InputName <> "" Then
                                                    InputType = UCase(HTML.ElementAttribute(ElementPtr, "type"))
                                                    InputValue = Trim(HTML.ElementAttribute(ElementPtr, "value"))
                                                    If LCase(InputType) = "submit" Then
                                                        '
                                                        ' Buttons match off input value, not name
                                                        '
                                                        InputName = InputValue
                                                    End If
                                                    '
                                                    ' Match the HTML input element name with a formfield
                                                    '
                                                    FieldFound = False
                                                    For FieldPtr = 0 To .FieldCnt - 1
                                                        If UCase(InputName) = UCase(.Fields(FieldPtr).Name) Then
                                                            FieldFound = True
                                                            '
                                                            ' Get the value from the Dataset and store it in the HTML input
                                                            '
                                                            Select Case InputType
                                                                Case "TEXT", ""
                                                                    '
                                                                    ' Build fields for a text input
                                                                    '
                                                                    Copy = Replace(Copy, "value=", "xvalue=", , , vbTextCompare)
                                                                    If Main.IsCSFieldSupported(CSData, InputName) Then
                                                                        InputValue = Main.GetCSText(CSData, InputName)
                                                                        fieldType = Main.GetCSFieldType(CSData, InputName)
                                                                        Select Case fieldType
                                                                            Case FieldTypeTextFile, FieldTypeHTMLFile
                                                                                InputValue = Main.ReadVirtualFile(InputValue)
                                                                            Case Else
                                                                        End Select
                                                                        '
                                                                        ' client request - just gonna home Mr. Guest Throkmorton stays away!
                                                                        '
                                                                        If (LcaseInputName = "name" Or LcaseInputName = "firstname") And (LCase(InputValue) = "guest") Then
                                                                            InputValue = ""
                                                                        End If
                                                                        Copy = Replace(Copy, ">", " value=""" & InputValue & """>", , , vbTextCompare)
                                                                    End If
                                                                Case "SUBMIT"
                                                                    '
                                                                    ' Button
                                                                    '
                                                                    Copy = Main.GetFormButton(InputName)
                                                                Case "RADIO"
                                                                    '
                                                                    ' Radio
                                                                    '
                                                                    Copy = Copy
                                                                    If Main.IsCSFieldSupported(CSData, InputName) Then
                                                                        DbValue = Trim(Main.GetCSText(CSData, InputName))
                                                                        If LCase(DbValue) = LCase(InputValue) Then
                                                                            Copy = Replace(Copy, ">", " checked>", , , vbTextCompare)
                                                                        End If
                                                                    End If
                                                                Case "CHECKBOX"
                                                                    '
                                                                    ' Checkbox
                                                                    '
                                                                    Copy = Copy
                                                                    Copy = Replace(Copy, "checked", "", , , vbTextCompare)
                                                                    If Main.IsCSFieldSupported(CSData, InputName) Then
                                                                        DbVAlueList = Trim(Main.GetCSText(CSData, InputName))
                                                                        If DbVAlueList <> "" Then
                                                                            DbValues = Split(DbVAlueList, ",")
                                                                            cnt = UBound(DbValues) + 1
                                                                            For Ptr = 0 To cnt - 1
                                                                                DbValue = Trim(DbValues(Ptr))
                                                                                If LCase(DbValue) = LCase(InputValue) Then
                                                                                    Copy = Replace(Copy, ">", " checked>", , , vbTextCompare)
                                                                                End If
                                                                            Next
                                                                        End If
                                                                    End If
                                                            End Select
                                                            Exit For
                                                        End If
                                                    Next
                                                End If
                                            Case "SELECT"
                                                '
                                                InputValue = Trim(HTML.ElementAttribute(ElementPtr, "value"))
                                                If InputName <> "" Then
                                                    '
                                                    ' Match the HTML input element name with a formfield
                                                    '
                                                    FieldFound = False
                                                    For FieldPtr = 0 To .FieldCnt - 1
                                                        If UCase(InputName) = UCase(.Fields(FieldPtr).Name) Then
                                                            FieldFound = True
                                                            If Main.IsCSFieldSupported(CSData, InputName) Then
                                                                DbValue = Main.GetCSText(CSData, InputName)
                                                                '
                                                                ' Go through all tags to the /select, marking all options that match
                                                                '
                                                                Do While (ElementPtr < HTML.ElementCount) And (UCase(ElementTag) <> "/SELECT")
                                                                    If HTML.IsTag(ElementPtr) Then
                                                                        Select Case ElementTag
                                                                            Case "OPTION"
                                                                                Copy = Replace(Copy, "selected", "", , , vbTextCompare)
                                                                                If InStr(1, Copy, "value=", vbTextCompare) <> 0 Then
                                                                                    '
                                                                                    ' option contains a value attribute
                                                                                    '
                                                                                    OptionValue = Trim(HTML.ElementAttribute(ElementPtr, "value"))
                                                                                    If LCase(OptionValue) = LCase(DbValue) Then
                                                                                        Copy = Replace(Copy, ">", " selected>")
                                                                                    End If
                                                                                Else
                                                                                    '
                                                                                    ' option has no value attribute, check the text following the option
                                                                                    '
                                                                                    If LCase(HTML.Text(ElementPtr + 1)) = LCase(DbValue) Then
                                                                                        Copy = Replace(Copy, ">", " selected>")
                                                                                    End If
                                                                                End If
                                                                                'Copy = Copy
                                                                        End Select
                                                                    End If
                                                                    If (ElementPtr < HTML.ElementCount) And (UCase(ElementTag) <> "/SELECT") Then
                                                                        fs.Add Copy
                                                                        ElementPtr = ElementPtr + 1
                                                                        Copy = HTML.Text(ElementPtr)
                                                                        If HTML.IsTag(ElementPtr) Then
                                                                            ElementTag = UCase(HTML.TagName(ElementPtr))
                                                                        Else
                                                                            ElementTag = ""
                                                                        End If
                                                                    End If
                                                                Loop
                                                                Exit For
                                                            End If
                                                        End If
                                                    Next
                                                End If
                                        End Select
                                    End If
                                    fs.Add Copy
                                    ElementPtr = ElementPtr + 1
                                Loop
                            End If
                            GetDynamicForm = fs.Text
                        End If
Hint = "9"
    '    '------------------------------------------------------------------------------------
    '    ' remove
    '    '------------------------------------------------------------------------------------
    '                    CSFields = Main.OpenCSContent("Form Fields", "(FormID=" & FormID & ")")
    '                    Do While Main.IsCSOK(CSFields)
    '                        DataFieldName = Main.GetCStext(CSFields, "Name")
    '                        ReplaceText = Main.GetCStext(CSFields, "ReplaceText")
    '                        If DataFieldName = "" Then
    '                            DataFieldName = ReplaceText
    '                        End If
    '                        If Main.IsCSFieldSupported(CSData, DataFieldName) Then
    '                            ReplaceFormINput = Main.GetFormCSInput(CSData, DataFieldName)
    '                            GetDynamicForm = GetDynamicForm &  Replace(GetDynamicForm, "{{" & ReplaceText & "}}", ReplaceFormINput, , , vbTextCompare)
    '                        End If
    '                        Call Main.NextCSRecord(CSFields)
    '                    Loop
    '                    Call Main.CloseCS(CSFields)
    '    '------------------------------------------------------------------------------------
    '    '
    '    '------------------------------------------------------------------------------------
                    End If
                    Call Main.closeCs(CSData)
                    '
                    ' Add the extra buttons
                    '
                    If .AddCancelButton Then
                        Name = .CancelButtonName
                        If Trim(Name) = "" Then
                            Name = ButtonCancel
                        End If
                        ButtonBar = ButtonBar & Main.GetFormButton(Name) & "&nbsp;"
                    End If
                    If .AddBackButton Then
                        Name = .BackButtonName
                        If Trim(Name) = "" Then
                            Name = ButtonBack
                        End If
                        ButtonBar = ButtonBar & Main.GetFormButton(Name) & "&nbsp;"
                    End If
                    If .AddContinueButton Then
                        Name = .ContinueButtonName
                        If Trim(Name) = "" Then
                            Name = ButtonContinue
                        End If
                        ButtonBar = ButtonBar & Main.GetFormButton(Name) & "&nbsp;"
                    End If
                    If ButtonBar <> "" Then
                        GetDynamicForm = GetDynamicForm & "<div>" & ButtonBar & "</div>"
                    End If
                    '
                    ' Complete the form
                    '
                    DataRecordID = DataRecordID
                    GetDynamicForm = "" _
                        & Main.GetUploadFormStart() _
                        & GetDynamicForm _
                        & Main.GetFormInputHidden(RequestNameDataRecordID, DataRecordID) _
                        & Main.GetFormInputHidden(RequestNameFormPtr, Formset.FormPtr) _
                        & Main.GetFormEnd
                End With
            End If
        End If
    End If

    '
    Exit Function
ErrorTrap:
    Call HandleClassTrapError("GetDynamicForm, Hint = " & Hint)
End Function
'
'
'
Private Function OpenCSData(Main As Object, DataContentID As Long, DataRecordID As Long, UseAuthMemberContent As Boolean, UseAuthOrgContent As Boolean) As Long
    On Error GoTo ErrorTrap
    '
    Dim DataContentName As String
    'Dim DataRecordID As Long
    '
    ' Form Found, Open the Data record
    '
    OpenCSData = -1
    If (DataContentID <> 0) And (DataRecordID <> 0) Then
        DataContentName = Main.GetContentNameByID(DataContentID)
        OpenCSData = Main.OpenCSContentRecord(DataContentName, DataRecordID)
    ElseIf UseAuthMemberContent Then
        OpenCSData = Main.OpenCSContentRecord("people", Main.memberId)
    ElseIf UseAuthOrgContent Then
        OpenCSData = Main.OpenCSContentRecord("organizations", Main.MemberOrganizationID)
    End If
    '
    Exit Function
ErrorTrap:
    Call HandleClassTrapError("OpenCSData")
End Function
'
'
'
Private Function GetFormsetResponse(Main As Object, UserFormResponseID As Long) As FormsetResponseType
    On Error GoTo ErrorTrap
    '
    Dim Copy As String
    Dim Rows() As String
    Dim RowPtr As Long
    Dim RowCopy As String
    Dim RowName As String
    Dim RowValue As String
    Dim Pos As Long
    Dim CS As Long
    Dim FormPtr As Long
    Dim FieldPtr As Long
    '
    If UserFormResponseID <> 0 Then
        With GetFormsetResponse
            '
            ' Get the text results
            '
            .Id = UserFormResponseID
            CS = Main.OpenCSContentRecord("user form response", .Id)
            If Not Main.iscsok(CS) Then
                Call Main.closeCs(CS)
                CS = Main.InsertCSRecord("user form response")
                Call Main.SetVisitProperty("FormWizard-UserResponseID", Main.GetCSInteger(CS, "ID"))
            Else
                Copy = Main.GetCSText(CS, "copy")
            End If
            Call Main.closeCs(CS)
            '
            ' Parse it out into the fields
            '
            If Copy <> "" Then
                Rows = Split(Copy, vbCrLf)
                '
                ' Check for formset name in first row
                '
                RowCopy = Rows(0)
                If InStr(1, RowCopy, "Form set: ", vbTextCompare) = 1 Then
                    .FormSetName = Mid(RowCopy, 11)
                    If UBound(Rows) > 0 Then
                        '
                        ' Parse the text into the FormResponse values
                        '
                        For RowPtr = 1 To UBound(Rows)
                            RowCopy = Rows(RowPtr)
                            If RowCopy <> "" Then
                                If InStr(1, RowCopy, "Form: ", vbTextCompare) = 1 Then
                                    '
                                    ' New form
                                    '
'                                    RowCopy = Mid(RowCopy, 7)
                                    FormPtr = .FormResponseCnt
                                    .FormResponseCnt = .FormResponseCnt + 1
                                    FieldPtr = 0
                                    ReDim Preserve .FormResponse(FormPtr)
                                    .FormResponse(FormPtr).FormName = Mid(RowCopy, 7)
                                ElseIf FormPtr >= 0 Then
                                    '
                                    ' Parse out form fields
                                    '
                                    If .FormResponseCnt > 0 Then
                                        With .FormResponse(FormPtr)
                                            Pos = InStr(1, RowCopy, "=")
                                            If Pos > 1 Then
                                                FieldPtr = .FieldCnt
                                                ReDim Preserve .Fields(FieldPtr)
                                                .Fields(FieldPtr).Name = Mid(RowCopy, 1, Pos - 1)
                                                .Fields(FieldPtr).Value = Replace(Mid(RowCopy, Pos + 1), "\n", vbCrLf)
                                                .FieldCnt = FieldPtr + 1
                                            End If
                                        End With
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        End With
    End If
    '
    Exit Function
ErrorTrap:
    Call HandleClassTrapError("GetFormsetResponse")
End Function

'
'
'
Private Sub SaveFormsetResponse(Main As Object, Formset As FormSetType, CSData As Long)
    On Error GoTo ErrorTrap
    '
    Dim FieldValue As String
    Dim FieldName As String
    Dim CS As Long
    Dim Ptr As Long
    Dim FieldPtr As Long
    Dim Copy As String
    Dim FormSetName As String
    Dim SQLLength As Long
    '
    FormSetName = Formset.Name
    With Formset.FormsetResponse
        If .Id = 0 Then
            .Id = Main.InsertContentRecordGetID("user form response")
            Call Main.SetVisitProperty("FormWizard-UserResponseID", CStr(.Id))
        End If
        If .Id <> 0 Then
            '
            ' Build the text result
            '
            Copy = "Form set: " & FormSetName
            For Ptr = 0 To .FormResponseCnt - 1
                With .FormResponse(Ptr)
                    Copy = Copy & vbCrLf & vbCrLf & "Form: " & .FormName
                    For FieldPtr = 0 To .FieldCnt - 1
                        Copy = Copy & vbCrLf & .Fields(FieldPtr).Name & "=" & Replace(.Fields(FieldPtr).Value, vbCrLf, "\n")
                    Next
                End With
            Next
            '
            ' save the copy
            '
            CS = Main.OpenCSContentRecord("user form response", .Id)
            If Not Main.iscsok(CS) Then
                Call Main.closeCs(CS)
                CS = Main.InsertCSRecord("user Form REsponse")
                .Id = Main.GetCSInteger(CS, "id")
                Call Main.SetVisitProperty("FormWizard-UserResponseID", CStr(.Id))
            End If
            If Main.iscsok(CS) Then
                Call Main.SetCS(CS, "name", "Form set '" & FormSetName & "' completed " & Now() & " by " & Main.MemberName)
                Call Main.SetCS(CS, "copy", Copy)
                If (Main.GetSiteProperty("BuildVersion", "0") > "3.4.221") Then
                    Call Main.SetCS(CS, "visitid", Main.VisitID)
                End If
            End If
            Call Main.closeCs(CS)
            '
            ' Save the data into the current record
            '
Dim fieldType As Long

            If Main.iscsok(CSData) Then
                For Ptr = 0 To .FormResponseCnt - 1
                    With .FormResponse(Ptr)
                        For FieldPtr = 0 To .FieldCnt - 1
                            FieldName = .Fields(FieldPtr).Name
                            If Main.IsCSFieldSupported(CSData, FieldName) Then
                                fieldType = Main.GetCSFieldType(CSData, FieldName)
                                If fieldType <> FieldTypeManyToMany And fieldType <> FieldTypeRedirect Then
                                    Select Case Main.GetCSFieldType(CSData, FieldName)
                                        Case FieldTypeLongText
                                            FieldValue = Left(.Fields(FieldPtr).Value, 4000)
                                        Case FieldTypeAutoIncrement, FieldTypeCurrency, FieldTypeInteger, FieldTypeFloat, FieldTypeLookup
                                            FieldValue = KmaEncodeNumber(.Fields(FieldPtr).Value)
                                        Case FieldTypeBoolean
                                            FieldValue = CStr(kmaEncodeBoolean(.Fields(FieldPtr).Value))
                                        Case FieldTypeDate
                                            FieldValue = CStr(KmaEncodeDate(.Fields(FieldPtr).Value))
                                        Case Else
                                            FieldValue = Left(.Fields(FieldPtr).Value, 255)
                                    End Select
                                    If SQLLength + Len(FieldValue) > 4000 Then
                                        Call Main.SaveCSRecord(CSData)
                                        SQLLength = 0
                                    End If
                                    SQLLength = SQLLength + Len(FieldValue)
                                    Call Main.SetCS(CSData, FieldName, FieldValue)
                                End If
                            End If
                        Next
                    End With
                Next
            End If
        End If
    End With
    Exit Sub
ErrorTrap:
    Call HandleClassTrapError("SaveFormsetResponse")
End Sub
'
'
'
Private Sub VerifyMenuEntry(Main As Object, MenuName As String, ParentMenuID As Long, ContentID As Long)
    On Error GoTo ErrorTrap
    '
    Dim CS As Long
    '
    CS = Main.openCsContent("Menu Entries", "name=" & KmaEncodeSQLText(MenuName))
    If Not Main.iscsok(CS) Then
        Call Main.closeCs(CS)
        CS = Main.InsertCSContent("Menu Entries")
        If Main.iscsok(CS) Then
            Call Main.SetCS(CS, "Name", MenuName)
            Call Main.SetCS(CS, "ParentID", ParentMenuID)
            Call Main.SetCS(CS, "ContentID", ContentID)
        End If
    End If
    Call Main.closeCs(CS)
    '
    Exit Sub
ErrorTrap:
    Call HandleClassTrapError("VerifyMenuEntry")
End Sub

