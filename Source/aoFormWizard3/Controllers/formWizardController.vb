
Imports Contensive.Addon.aoFormWizard3.Controllers
Imports Contensive.Addon.aoFormWizard3.Models.View
Imports Contensive.Addon.aoFormWizard3.Models.Db
Imports Contensive.BaseClasses

Namespace Controllers
    Public Class FormWizardController


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
                If (Not request.blockContactFormButton.Equals("Submit")) Then Return False
                Dim Adddata As Models.Db.UserFormResponseModel = Models.Db.UserFormResponseModel.add(CP)
                Dim formlist As List(Of FormModel) = FormModel.createList(CP, "(formsetid=" & settings.id & ")")
                For Each form In formlist
                    Dim formsFieldList As List(Of FormFieldModel) = FormFieldModel.createList(CP, "(formid=" & form.id & ")")
                    For Each formsField In formsFieldList
                        Adddata.copy += vbCrLf & formsField.name & "=" & CP.Doc.GetText("formField_" & formsField.id)
                    Next
                    CP.Email.sendSystem(settings.notificationemailid, Adddata.copy)
                    CP.Group.AddUser(settings.joingroupid, CP.User.Id)
                Next
                Adddata.save(CP)

                Return True
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
                Return False
            End Try
        End Function
    End Class
End Namespace
