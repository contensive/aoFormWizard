
Imports Contensive.Addon.aoFormWizard3.Controllers
Imports Contensive.BaseClasses

Namespace Models.View

    Public Class ViewBaseModel
        Public Property styleBackgroundImage As String
        Public Property styleheight As String
        Public Property contentContainerClass As String
        '
        '====================================================================================================
        ''' <summary>
        ''' Populate the view model from the entity model
        ''' </summary>
        ''' <param name="cp"></param>
        ''' <param name="settings"></param>
        ''' <returns></returns>
        Public Shared Function create(Of T As ViewBaseModel)(cp As CPBaseClass, settings As Models.Db.DbBaseModel) As T
            Dim result As T = Nothing
            Try
                Dim instanceType As Type = GetType(T)
                result = DirectCast(Activator.CreateInstance(instanceType), T)
                '
                ' -- base fields
                result.styleheight = genericController.encodeStyleHeight(settings.styleheight)
                result.styleBackgroundImage = genericController.encodeStyleBackgroundImage(cp, settings.backgroundImageFilename)
                result.contentContainerClass = "" _
                    + If(settings.asFullBleed, " container", String.Empty) _
                    + If(settings.fontStyleId.Equals(0), String.Empty, " " + Models.Db.DesignBlockFontModel.getRecordName(cp, settings.fontStyleId)) _
                    + If(settings.themeStyleId.Equals(0), String.Empty, " " + Models.Db.DesignBlockThemeModel.getRecordName(cp, settings.themeStyleId)) _
                    + If(settings.padTop, " pt-5", "") _
                    + If(settings.padRight, " pr-4", "") _
                    + If(settings.padBottom, " pb-5", "") _
                    + If(settings.padLeft, " pl-4", "") _
                    + ""
            Catch ex As Exception
                cp.Site.ErrorReport(ex)
            End Try
            Return result
        End Function
    End Class

End Namespace