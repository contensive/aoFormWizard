
Imports Contensive.Addon.aoFormWizard3.Controllers
Imports Contensive.Addon.aoFormWizard3.Models.Db
Imports Contensive.BaseClasses

Namespace Views
    '
    Public Class DynamicFormClass
        Inherits AddonBaseClass
        '
        '=====================================================================================
        ''' <summary>
        ''' Addon api - Dynamic Form addon, Render dynamic form
        ''' </summary>
        ''' <param name="CP"></param>
        ''' <returns></returns>
        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Const designBlockName As String = "Form Wizard"
            Try
                '
                ' -- read instanceId, guid created uniquely for this instance of the addon on a page
                Dim result = String.Empty
                Dim settingsGuid = InstanceIdController.getSettingsGuid(CP, designBlockName, result)
                If (String.IsNullOrEmpty(settingsGuid)) Then Return result
                '
                ' -- locate or create a data record for this guid
                Dim settings = FormSetModel.createOrAddSettings(CP, settingsGuid)
                If (settings Is Nothing) Then Throw New ApplicationException("Could not create the design block settings record.")
                '
                '
                ' -- process form request
                Dim request As New FormRequest() With {
                    .button = CP.Doc.GetText("button")
                }
                If (FormController.processRequest(CP, settings, request)) Then
                    '
                    ' -- simple thank you content
                    CP.Doc.SetProperty("formwizardcomplete", True)
                    result = CP.Html.div(settings.thankyoucopy)
                Else
                    '
                    ' -- translate the Db model to a view model and mustache it into the layout
                    Dim viewModel = Models.View.FormViewModel.create(CP, settings)
                    If (viewModel Is Nothing) Then Throw New ApplicationException("Could not create design block view model.")
                    '
                    ' -- translate view model into html
                    result = CP.Html.Form(CP.Mustache.Render(My.Resources.FormWizard, viewModel))
                End If
                '
                ' -- if editing enabled, add the link and wrapperwrapper
                Return genericController.addEditWrapper(CP, result, settings.id, settings.name, FormSetModel.contentName)
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
                Return "<!-- " & designBlockName & ", Unexpected Exception -->"
            End Try
        End Function
    End Class
    '
    '=====================================================================================
    ''' <summary>
    ''' Request object for main
    ''' </summary>
    Public Class FormRequest
        Public button As String
    End Class
End Namespace