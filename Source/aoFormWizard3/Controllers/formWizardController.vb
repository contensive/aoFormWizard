
Imports Contensive.Addon.aoFormWizard3.Controllers
Imports Contensive.Addon.aoFormWizard3.Models.View
Imports Contensive.Addon.aoFormWizard3.Models.Db
Imports Contensive.BaseClasses

Namespace Controllers
    Public Class FormWizardController
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
            Dim cs As CPCSBaseClass = CP.CSNew
            Try
                If (Not request.blockContactFormButton.Equals("Submit")) Then Return False
                Dim Adddata As Models.Db.UserFormResponseModel = Models.Db.UserFormResponseModel.add(CP)
                Dim formlist As List(Of FormModel) = FormModel.createList(CP, "(formsetid=" & settings.id & ")", "sortorder")
                For Each form In formlist
                    Dim formsFieldList As List(Of FormFieldModel) = FormFieldModel.createList(CP, "(formid=" & form.id & ")", "sortOrder,id")
                    Dim optionList As New List(Of OptionClass)
                    For Each formsField In formsFieldList
                        Dim question As String = formsField.name
                        Dim answerList As New List(Of String)
                        Select Case formsField.inputtype.ToLower()
                            Case "checkbox", "radio"
                                Dim answerNumberCommaList As String = CP.Doc.GetText("formField_" & formsField.id)
                                Dim answerNumberList As List(Of String) = New List(Of String)(answerNumberCommaList.Split(","c))
                                Dim optionPtr As Integer = 1
                                For Each formfieldoption In formsField.optionList.Split(",")
                                    If answerNumberList.Contains(optionPtr.ToString()) Then
                                        answerList.Add(formfieldoption)
                                    End If
                                    optionPtr += 1
                                Next
                            Case "file"
                                Dim folder As LibraryFolderModel = LibraryFolderModel.createByName(CP, "Form Wizard Uploads")
                                If (folder Is Nothing) Then
                                    folder = LibraryFolderModel.add(CP)
                                    folder.name = "Form Wizard Uploads"
                                    folder.save(CP)
                                End If
                                cs.Insert("Library Files")
                                cs.SetFormInput("filename", "formField_" & formsField.id)
                                cs.SetField("folderid", folder.id)
                                cs.Save()
                                answerList.Add("<a href=""http://" & CP.Site.DomainPrimary & CP.Site.FilePath & cs.GetText("filename") & """>" & CP.Doc.GetText("formField_" & formsField.id) & "</a>")
                                cs.Close()
                            Case Else
                                answerList.Add(CP.Doc.GetText("formField_" & formsField.id))
                        End Select
                        Adddata.copy += "<div style=""padding-top:10px;""> Question:" & vbCrLf & question & "</div>"
                        For Each answer In answerList
                            Adddata.copy += "<div style=""padding-left:20px;"">" & vbCrLf & vbTab & answer & "</div>"
                        Next
                    Next
                Next
                'Dim imgfile As String = CP.Doc.GetText("formField_5")
                'Dim saveImage As Models.Db.LibraryFileModel = Models.Db.LibraryFileModel.add(CP)
                'saveImage.filename = imgfile
                'saveImage.save(CP)

                CP.Email.sendSystem(settings.notificationemailid, Adddata.copy)
                If (settings.joingroupid <> 0) Then
                    CP.Group.AddUser(settings.joingroupid, CP.User.Id)
                End If
                CP.Utils.AppendLog("Add group User,groupuser=" & settings.joingroupid & "," & CP.User.Id)
                CP.Utils.AppendLog("Notification email=" & Adddata.copy)
                Adddata.name = "Form Set " + settings.name + " completed on " + Date.Now.ToString("MM/dd/yyyy") + " by " + CP.User.Name
                Adddata.save(CP)
                Return True
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
                Return False
            End Try
        End Function
    End Class
End Namespace
