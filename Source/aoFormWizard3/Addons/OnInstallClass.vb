
Imports Contensive.Addon.aoFormWizard3.Models.Db
Imports Contensive.BaseClasses

Namespace Views
    '
    Public Class OnInstallClass
        Inherits AddonBaseClass
        '
        '=====================================================================================
        ''' <summary>
        ''' Executed on installation -- upgrade content.
        ''' </summary>
        ''' <param name="CP"></param>
        ''' <returns></returns>
        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Try
                If (CP.Site.GetInteger("Form Wizard Version", 0) < Constants.version) Then
                    '
                    ' -- version - 2, convert form.useauthmembercontent, form.useauthorgcontent, form.authcontent -> form.saveContentId (null=no-save, 1=no-save, 2=people-save, 3=org-save, 4=custom-save), saveCustomContent
                    For Each form As FormModel In BaseModel.createList(Of FormModel)(CP, "", "id")
                        form.saveCustomContent = If(form.authcontent.Equals(0), "", CP.Content.GetID(form.authcontent))
                        form.saveTypeId = If(form.useauthmembercontent, 2, If(form.useauthorgcontent, 3, If(Not String.IsNullOrWhiteSpace(form.saveCustomContent), 4, 1)))
                        form.save(CP)
                    Next
                End If
                Return String.Empty
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
                Throw
            End Try
        End Function
    End Class
End Namespace