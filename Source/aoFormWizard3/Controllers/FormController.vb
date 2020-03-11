
Imports Contensive.Addon.aoFormWizard3.Controllers
Imports Contensive.Addon.aoFormWizard3.Models.View
Imports Contensive.Addon.aoFormWizard3.Models.Db
Imports Contensive.BaseClasses
Imports System.Text

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
                For Each form In FormModel.createList(CP, "(formsetid=" & settings.id & ")", "sortorder")
                    For Each formsField In FormFieldModel.createList(CP, "(formid=" & form.id & ")", "sortOrder,id")
                        textVersion.Append(vbCrLf & "Question: " & formsField.name)
                        htmlVersion.Append("<div style=""padding-top:10px;""> Question:" & formsField.name & "</div>")
                        Select Case formsField.inputtype.ToLower()
                            Case "checkbox", "radio"
                                Dim answerNumberCommaList As String = CP.Doc.GetText("formField_" & formsField.id)
                                Dim answerNumberList As List(Of String) = New List(Of String)(answerNumberCommaList.Split(","c))
                                Dim optionPtr As Integer = 1
                                For Each formfieldoption In formsField.optionList.Split(",")
                                    If answerNumberList.Contains(optionPtr.ToString()) Then
                                        textVersion.Append(vbCrLf & vbTab & formfieldoption)
                                        htmlVersion.Append("<div style=""padding-left:20px;"">" & formfieldoption & "</div>")
                                    End If
                                    optionPtr += 1
                                Next
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
                                        textVersion.Append(vbCrLf & vbTab & CP.Site.FilePath & pathFilename)
                                        htmlVersion.Append("<div style=""padding-left:20px;""><a href=""" & CP.Site.FilePath & pathFilename & """>" & uploadFilename & "</a></div>")
                                    End Using
                                End If
                            Case Else
                                'if the form is set to useauthmembercontent, then save the information from the form to their user record
                                If (form.useauthmembercontent) Then
                                    Using cs As CPCSBaseClass = CP.CSNew()
                                        'make sure the form's field exists in the people table
                                        If (cs.Open("People", "id=" & CP.User.Id) And cs.FieldOK(formsField.name)) Then
                                            cs.SetField(formsField.name, CP.Doc.GetText("formField_" & formsField.id))
                                            cs.Save()
                                        End If
                                    End Using
                                End If
                                textVersion.Append(vbCrLf & vbTab & CP.Doc.GetText("formField_" & formsField.id))
                                htmlVersion.Append("<div style=""padding-left:20px;"">" & CP.Doc.GetText("formField_" & formsField.id) & "</div>")
                        End Select
                    Next
                Next
                CP.Email.sendSystem(settings.notificationemailid, htmlVersion.ToString())
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
