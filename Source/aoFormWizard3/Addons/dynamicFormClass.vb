
Option Strict On
Option Explicit On

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
            Const designBlockName As String = "Dynamic Form"
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
                Dim request As New Request() With {
                    .button = CP.Doc.GetText("button")
                }
                If (FormController.processRequest(CP, settings, request)) Then
                    '
                    CP.Doc.SetProperty("formwizardcomplete", True)
                    Return CP.Html.div(settings.thankyoucopy)
                Else
                    '
                    ' -- display the form
                    '
                    ' -- translate the Db model to a view model and mustache it into the layout
                    Dim viewModel = Models.View.FormViewModel.create(CP, settings)
                    If (viewModel Is Nothing) Then Throw New ApplicationException("Could not create design block view model.")
                    '
                    ' -- translate view model into html
                    result = CP.Html.Form(Nustache.Core.Render.StringToString(My.Resources.FormWizard, viewModel))
                End If
                '
                ' -- if editing enabled, add the link and wrapperwrapper
                Return genericController.addEditWrapper(CP, result, settings.id, settings.name, FormSetModel.contentName)
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
                Return "<!-- " & designBlockName & ", Unexpected Exception -->"
            End Try
        End Function
        '
        '
        Public Class Request
            Public button As String
        End Class
        '
        '
    End Class
End Namespace