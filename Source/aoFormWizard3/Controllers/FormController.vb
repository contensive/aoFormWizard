
Imports System.Text
Imports System.Text.RegularExpressions
Imports Contensive.Addon.aoFormWizard3.Models.Db
Imports Contensive.BaseClasses

Namespace Controllers
    Public NotInheritable Class FormController
        ''' <summary>
        ''' Process submitted contact form. Returns true if the form has already been submitted, or successfully commits
        ''' </summary>
        ''' <param name="CP"></param>
        ''' <param name="settings"></param>
        ''' <param name="request"></param>
        ''' <returns></returns>
        Public Shared Function processRequest(ByVal CP As CPBaseClass, settings As Models.Db.FormSetModel, request As Views.FormRequest) As Boolean
            Dim returnHtml As String = String.Empty
            Dim hint As Integer = 0
            Try
                If (String.IsNullOrWhiteSpace(request.button) OrElse (request.button.Equals("cancel"))) Then Return False
                Dim htmlVersion As New StringBuilder()
                Dim textVersion As New StringBuilder()
                Dim currentAuthContentRecordId As Integer = 0

                For Each form In FormModel.createList(CP, "(formsetid=" & settings.id & ")", "sortorder")
                    'remove any bad characters from the custom content name
                    Dim customContentName As String = form.saveCustomContent
                    'replace custom content name with no nonalphanumeric characters
                    'includes spaces since this is content "[^A-Za-z0-9 ]+"
                    customContentName = Regex.Replace(customContentName, "[^A-Za-z0-9 ]+", "")

                    hint = 1
                    For Each formsField In FormFieldModel.createList(CP, "(formid=" & form.id & ")", "sortOrder,id")
                        'remove any bad characters from the customfieldname
                        Dim customFieldName As String = formsField.name
                        'do not include spaces in the field name "[^A-Za-z0-9]+"
                        customFieldName = Regex.Replace(customFieldName, "[^A-Za-z0-9]+", "")

                        hint = 2
                        textVersion.Append(vbCrLf & "Question: " & formsField.name)
                        htmlVersion.Append("<div style=""padding-top:10px;""> Question:" & formsField.name & "</div>")
                        Select Case formsField.inputtype.ToLower()
                            Case "checkbox", "radio", "select"
                                hint = 3
                                Dim answerNumberCommaList As String = CP.Doc.GetText("formField_" & formsField.id)
                                Dim answerNumberList As List(Of String) = New List(Of String)(answerNumberCommaList.Split(","c))
                                Dim optionPtr As Integer = 1
                                Dim answerTextList As List(Of String) = New List(Of String)
                                For Each formfieldoption In formsField.optionList.Split(",")
                                    If answerNumberList.Contains(optionPtr.ToString()) Then
                                        hint = 4
                                        textVersion.Append(vbCrLf & vbTab & formfieldoption)
                                        htmlVersion.Append("<div style=""padding-left:20px;"">" & formfieldoption & "</div>")
                                        If (Not String.IsNullOrWhiteSpace(customContentName)) Then
                                            answerTextList.Add(formfieldoption)
                                        End If
                                    End If
                                    optionPtr += 1
                                Next
                                'custom content
                                If (form.saveTypeId.Equals(4)) And (Not String.IsNullOrWhiteSpace(customContentName)) Then
                                    hint = 10
                                    Dim verifiedContent As Boolean = CustomContentController.verifyCustomContent(CP, customContentName)
                                    If verifiedContent Then
                                        'make sure the content has this field
                                        If Not CP.Content.IsField(customContentName, customFieldName) Then
                                            hint = 11
                                            'create a text field
                                            CustomContentController.createCustomContentField(CP, customContentName, customFieldName, Constants.FieldTypeIdEnum.Text)
                                        End If

                                        Using cs As CPCSBaseClass = CP.CSNew()
                                            'after the field should have been created, check again
                                            If CP.Content.IsField(customContentName, customFieldName) Then
                                                hint = 12
                                                If currentAuthContentRecordId = 0 Then
                                                    hint = 13
                                                    cs.Insert(customContentName)
                                                    currentAuthContentRecordId = cs.GetInteger("id")
                                                End If
                                                If cs.Open(customContentName, "id=" & currentAuthContentRecordId) Then
                                                    hint = 14
                                                    'list inot a string with commas
                                                    Dim value As String = String.Join(",", answerTextList)
                                                    cs.SetField(customFieldName, value)
                                                End If
                                            End If
                                        End Using
                                    End If
                                End If
                            Case "file"
                                hint = 20
                                Dim fieldName As String = "formField_" & formsField.id
                                Dim uploadFilename As String = CP.Doc.GetText(fieldName)
                                If (Not String.IsNullOrEmpty(uploadFilename)) Then
                                    hint = 21
                                    Dim folder As LibraryFolderModel = LibraryFolderModel.createByName(CP, "Form Wizard Uploads")
                                    If (folder Is Nothing) Then
                                        hint = 22
                                        folder = LibraryFolderModel.add(CP)
                                        folder.name = "Form Wizard Uploads"
                                        folder.save(CP)
                                    End If
                                    Using cs As CPCSBaseClass = CP.CSNew
                                        hint = 23
                                        cs.Insert("Library Files")
                                        cs.SetFormInput("filename", fieldName)
                                        cs.SetField("folderid", folder.id)
                                        cs.Save()
                                        Dim pathFilename As String = cs.GetText("filename")
                                        If (form.useauthorgcontent) Then
                                            hint = 24
                                            If CP.Content.IsField("Organizations", formsField.name) Then
                                                'make sure the form's field exists in the people table
                                                If (cs.Open("Organizations", "id=" & CP.User.OrganizationID)) Then
                                                    hint = 25
                                                    cs.SetField(formsField.name, pathFilename)
                                                    cs.Save()
                                                End If
                                            End If
                                        End If
                                        'custom content
                                        If (form.saveTypeId.Equals(4)) And (Not String.IsNullOrWhiteSpace(customContentName)) Then
                                            hint = 30
                                            Dim verifiedContent As Boolean = CustomContentController.verifyCustomContent(CP, customContentName)
                                            If verifiedContent Then
                                                'make sure the content has this field
                                                If Not CP.Content.IsField(customContentName, customFieldName) Then
                                                    hint = 31
                                                    'create file field
                                                    CustomContentController.createCustomContentField(CP, customContentName, customFieldName, Constants.FieldTypeIdEnum.File)
                                                End If
                                                Using csContent As CPCSBaseClass = CP.CSNew()
                                                    'after the field should have been created, check again
                                                    If CP.Content.IsField(customContentName, customFieldName) Then
                                                        hint = 32
                                                        If currentAuthContentRecordId = 0 Then
                                                            hint = 33
                                                            csContent.Insert(customContentName)
                                                            currentAuthContentRecordId = csContent.GetInteger("id")
                                                        End If
                                                        If csContent.Open(customContentName, "id=" & currentAuthContentRecordId) Then
                                                            csContent.SetField(customFieldName, pathFilename)
                                                            csContent.Save()
                                                        End If
                                                    End If
                                                End Using
                                            End If
                                        End If
                                        textVersion.Append(vbCrLf & vbTab & CP.Http.CdnFilePathPrefixAbsolute & pathFilename)
                                        htmlVersion.Append("<div style=""padding-left:20px;""><a href=""" & CP.Http.CdnFilePathPrefixAbsolute & pathFilename & """>" & uploadFilename & "</a></div>")
                                    End Using
                                End If
                            Case Else
                                If (form.saveTypeId.Equals(4)) And (Not String.IsNullOrWhiteSpace(customContentName)) Then
                                    hint = 50
                                    hint = 10
                                    Dim verifiedContent As Boolean = CustomContentController.verifyCustomContent(CP, customContentName)
                                    If verifiedContent Then
                                        'make sure the content has this field
                                        If Not CP.Content.IsField(customContentName, customFieldName) Then
                                            hint = 11
                                            'determine which kind of field to create
                                            Select Case formsField.inputtype.ToLower()
                                                Case "text"
                                                    'create a text field if this type is for text
                                                    CustomContentController.createCustomContentField(CP, customContentName, customFieldName, Constants.FieldTypeIdEnum.Text)
                                                Case Else
                                                    'this is a longtext or unknown so default to longtext
                                                    CustomContentController.createCustomContentField(CP, customContentName, customFieldName, Constants.FieldTypeIdEnum.LongText)
                                            End Select
                                        End If

                                        Using cs As CPCSBaseClass = CP.CSNew()
                                            'after the field should have been created, check again
                                            If CP.Content.IsField(customContentName, customFieldName) Then
                                                hint = 12
                                                If currentAuthContentRecordId = 0 Then
                                                    hint = 13
                                                    cs.Insert(customContentName)
                                                    currentAuthContentRecordId = cs.GetInteger("id")
                                                End If
                                                If cs.Open(customContentName, "id=" & currentAuthContentRecordId) Then
                                                    cs.SetField(customFieldName, CP.Doc.GetText("formField_" & formsField.id))
                                                End If
                                            End If
                                        End Using
                                    End If
                                ElseIf (form.saveTypeId.Equals(3)) Then
                                    hint = 60
                                    '
                                    ' save to organization
                                    If CP.Content.IsField("organizations", formsField.name) Then
                                        Using cs As CPCSBaseClass = CP.CSNew()
                                            'make sure the form's field exists in the people table
                                            If (cs.Open("organizations", "id=" & CP.User.OrganizationID) And cs.FieldOK(formsField.name)) Then
                                                cs.SetField(formsField.name, CP.Doc.GetText("formField_" & formsField.id))
                                                cs.Save()
                                            End If
                                        End Using
                                    End If
                                ElseIf (form.saveTypeId.Equals(2)) Then
                                    hint = 70
                                    '
                                    ' -- save to people table
                                    If CP.Content.IsField("People", formsField.name) Then
                                        Using cs As CPCSBaseClass = CP.CSNew()
                                            'make sure the form's field exists in the people table
                                            If (cs.Open("People", "id=" & CP.User.Id) And cs.FieldOK(formsField.name)) Then
                                                cs.SetField(formsField.name, CP.Doc.GetText("formField_" & formsField.id))
                                                cs.Save()
                                            End If
                                        End Using
                                    End If
                                End If
                                textVersion.Append(vbCrLf & vbTab & CP.Doc.GetText("formField_" & formsField.id))
                                htmlVersion.Append("<div style=""padding-left:20px;"">" & CP.Doc.GetText("formField_" & formsField.id) & "</div>")
                        End Select
                    Next
                Next
                CP.Email.sendSystem(settings.notificationemailid, htmlVersion.ToString())
                If settings.responseemailid > 0 Then
                    CP.Email.sendSystem(settings.responseemailid, "", CP.User.Id)
                End If
                If (settings.joingroupid <> 0) Then
                    CP.Group.AddUser(settings.joingroupid, CP.User.Id)
                End If
                Dim userFormResponse As UserFormResponseModel = UserFormResponseModel.add(CP)
                userFormResponse.visitid = CP.Visit.Id
                userFormResponse.copy = textVersion.ToString()
                userFormResponse.name = "Form Set " + settings.name + " completed on " + Date.Now.ToString("MM/dd/yyyy") + " by " + CP.User.Name
                userFormResponse.save(CP)
                Return True
            Catch ex As Exception
                CP.Site.ErrorReport("hint=" & hint.ToString() & +" " & ex.ToString())
                Return False
            End Try
        End Function
    End Class
    Public Class FormControllerOptionClass
        Public Property optionName As String
        Public Property optionPtr As Integer
    End Class
End Namespace