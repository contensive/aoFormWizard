
Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Contensive.Addon.AddonCollectionVb.Controllers
    '
    Public Class addonClass
        Inherits AddonBaseClass
        '
        ' - use NuGet to add Contentive.clib reference
        ' - Verify project root name space is empty
        ' - Change the namespace to the collection name
        ' - Change this class name to the addon name
        ' - Create a Contensive Addon record with the namespace apCollectionName.ad
        ' - add reference to CPBase.DLL, typically installed in c:\program files\kma\contensive\
        '
        '=====================================================================================
        ' addon api
        '=====================================================================================
        '
        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Dim returnHtml As String
            Try
                returnHtml = "Visual Studio 2005 Contensive Addon - OK response"
            Catch ex As Exception
                errorReport(CP, ex, "execute")
                returnHtml = "Visual Studio 2005 Contensive Addon - Error response"
            End Try
            Return returnHtml
        End Function

        Private Sub errorReport(cP As CPBaseClass, ex As Exception, v As String)
            Throw New NotImplementedException()
        End Sub
    End Class
End Namespace
