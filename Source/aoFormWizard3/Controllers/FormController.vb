
Imports Contensive.Addon.aoFormWizard3.Controllers
Imports Contensive.Addon.aoFormWizard3.Models.View
Imports Contensive.Addon.aoFormWizard3.Models.Db
Imports Contensive.BaseClasses
Imports System.Text
Imports System.Configuration

Namespace Controllers
    Public Class FormController
        Public Class OptionClass
            Public Property optionName As String
            Public Property optionPtr As Integer
        End Class

        ''' <summary>
        ''' Process submitted contact form. Returns true if the form has already been submitted, or successfully commits
        ''' </summary>
        ''' <param name="CP"></param>
        ''' <param name="settings"></param>
        ''' <param name="request"></param>
        ''' <returns></returns>
        Public Shared Function processRequest(ByVal CP As CPBaseClass, settings As Models.Db.FormSetModel, request As Views.DynamicFormClass.Request) As Boolean
            Dim returnHtml As String = String.Empty
            Try
                If (String.IsNullOrWhiteSpace(request.button) OrElse (request.button.Equals("cancel"))) Then Return False
                Dim htmlVersion As New StringBuilder()
                Dim textVersion As New StringBuilder()
                Dim currentAuthContentRecordId As Integer = 0
                For Each form In FormModel.createList(CP, "(formsetid=" & settings.id & ")", "sortorder")
                    For Each formsField In FormFieldModel.createList(CP, "(formid=" & form.id & ")", "sortOrder,id")
                        textVersion.Append(vbCrLf & "Question: " & formsField.name)
                        htmlVersion.Append("<div style=""padding-top:10px;""> Question:" & formsField.name & "</div>")
                        Select Case formsField.inputtype.ToLower()
                            Case "checkbox", "radio"
                                Dim answerNumberCommaList As String = CP.Doc.GetText("formField_" & formsField.id)
                                Dim answerNumberList As List(Of String) = New List(Of String)(answerNumberCommaList.Split(","c))
                                Dim optionPtr As Integer = 1
                                Dim answerTextList As List(Of String) = New List(Of String)
                                For Each formfieldoption In formsField.optionList.Split(",")
                                    If answerNumberList.Contains(optionPtr.ToString()) Then
                                        textVersion.Append(vbCrLf & vbTab & formfieldoption)
                                        htmlVersion.Append("<div style=""padding-left:20px;"">" & formfieldoption & "</div>")
                                        If (form.authcontent <> 0) Then
                                            answerTextList.Add(formfieldoption)
                                        End If
                                    End If
                                    optionPtr += 1
                                Next
                                If (form.authcontent <> 0) Then
                                    Using cs As CPCSBaseClass = CP.CSNew()
                                        ''make sure the form's field exists in the people table
                                        Dim contentName As String = ""
                                        If (cs.Open("Content", "id=" & form.authcontent)) Then
                                            contentName = cs.GetText("name")
                                        End If
                                        'make sure the content has this field
                                        If CP.Content.IsField(contentName, formsField.name) Then
                                            If currentAuthContentRecordId = 0 Then
                                                cs.Insert(contentName)
                                                currentAuthContentRecordId = cs.GetInteger("id")
                                            End If
                                            If cs.Open(contentName, "id=" & currentAuthContentRecordId) Then
                                                'list inot a string with commas
                                                Dim value As String = String.Join(",", answerTextList)
                                                cs.SetField(formsField.name, value)
                                            End If
                                        End If
                                    End Using
                                End If
                            Case "file"
                                Dim fieldName As String = "formField_" & formsField.id
                                Dim uploadFilename As String = CP.Doc.GetText(fieldName)
                                If (Not String.IsNullOrEmpty(uploadFilename)) Then
                                    Dim folder As LibraryFolderModel = LibraryFolderModel.createByName(CP, "Form Wizard Uploads")
                                    If (folder Is Nothing) Then
                                        folder = LibraryFolderModel.add(CP)
                                        folder.name = "Form Wizard Uploads"
                                        folder.save(CP)
                                    End If
                                    Using cs As CPCSBaseClass = CP.CSNew
                                        cs.Insert("Library Files")
                                        cs.SetFormInput("filename", fieldName)
                                        cs.SetField("folderid", folder.id)
                                        cs.Save()
                                        Dim pathFilename As String = cs.GetText("filename")
                                        If (form.useauthorgcontent) Then
                                            If CP.Content.IsField("Organizations", formsField.name) Then
                                                'make sure the form's field exists in the people table
                                                If (cs.Open("Organizations", "id=" & CP.User.OrganizationID)) Then
                                                    cs.SetField(formsField.name, pathFilename)
                                                    cs.Save()
                                                End If
                                            End If
                                        End If
                                        If (form.authcontent <> 0) Then
                                            Using csContent As CPCSBaseClass = CP.CSNew()
                                                ''make sure the form's field exists in the people table
                                                Dim contentName As String = ""
                                                If (csContent.Open("Content", "id=" & form.authcontent)) Then
                                                    contentName = csContent.GetText("name")
                                                End If
                                                'make sure the content has this field
                                                If CP.Content.IsField(contentName, formsField.name) Then
                                                    If currentAuthContentRecordId = 0 Then
                                                        csContent.Insert(contentName)
                                                        currentAuthContentRecordId = csContent.GetInteger("id")
                                                    End If
                                                    If csContent.Open(contentName, "id=" & currentAuthContentRecordId) Then
                                                        csContent.SetField(formsField.name, pathFilename)
                                                        csContent.Save()
                                                    End If
                                                End If
                                            End Using
                                        End If
                                        textVersion.Append(vbCrLf & vbTab & CP.Site.FilePath & pathFilename)
                                        htmlVersion.Append("<div style=""padding-left:20px;""><a href=""" & CP.Site.FilePath & pathFilename & """>" & uploadFilename & "</a></div>")
                                    End Using
                                End If
                            Case Else
                                'if the form is set to use authcontent, then save the information from the form into that table
                                If (form.authcontent <> 0) Then
                                    Using cs As CPCSBaseClass = CP.CSNew()
                                        'make sure the form's field exists in the people table
                                        Dim contentName As String = ""
                                        If (cs.Open("Content", "id=" & form.authcontent)) Then
                                            contentName = cs.GetText("name")
                                        End If
                                        'make sure the content has this field
                                        If CP.Content.IsField(contentName, formsField.name) Then
                                            If currentAuthContentRecordId = 0 Then
                                                cs.Insert(contentName)
                                                currentAuthContentRecordId = cs.GetInteger("id")
                                            End If
                                            If cs.Open(contentName, "id=" & currentAuthContentRecordId) Then
                                                cs.SetField(formsField.name, CP.Doc.GetText("formField_" & formsField.id))
                                            End If
                                        End If
                                    End Using
                                    'if the form is set to useauthmembercontent, then save the information from the form to their user record
                                ElseIf (form.useauthmembercontent) Then
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
                CP.Site.ErrorReport(ex)
                Return False
            End Try
        End Function
    End Class
End Namespace