
Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Models.Db
    Public Class FormSetModel        '<------ set set model Name and everywhere that matches this string
        Inherits DbBaseModel

        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "Form Sets"      '<------ set content name
        Public Const contentTableName As String = "ccFormSets"   '<------ set to tablename for the primary content (used for cache names)
        Private Shadows Const contentDataSource As String = "default"             '<------ set to datasource if not default
        '
        '====================================================================================================
        ' -- instance properties
        Public Property joingroupid As Integer
        Public Property notificationemailid As Integer
        Public Property responseemailid As Integer
        Public Property thankyoucopy As String
        '
        '====================================================================================================
        Public Overloads Shared Function createOrAddSettings(cp As CPBaseClass, settingsGuid As String) As FormSetModel
            Dim result As FormSetModel = create(Of FormSetModel)(cp, settingsGuid)
            If (result Is Nothing) Then
                '
                ' -- create default content
                result = DbBaseModel.add(Of FormSetModel)(cp)
                result.name = contentName & " Instance " & result.id & ", created " & Now.ToString()
                result.ccguid = settingsGuid
                '
                ' -- create custom content

                '
                result.save(cp)
            End If
            Return result
        End Function

    End Class
End Namespace
