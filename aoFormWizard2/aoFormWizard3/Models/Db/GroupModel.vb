
Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Models.Db
    Public Class GroupModel
        Inherits baseModel
        Implements ICloneable
        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "Groups"
        Public Const contentTableName As String = "ccGroups"
        Private Shadows Const contentDataSource As String = "default"
        '
        '====================================================================================================
        ' -- instance properties

        Public Property AllowBulkEmail As Boolean
        Public Property Caption As String
        Public Property CopyFilename As String
        Public Property PublicJoin As Boolean
        '
        '====================================================================================================
        Public Overloads Shared Function add(cp As CPBaseClass) As GroupModel
            Return add(Of GroupModel)(cp)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function create(cp As CPBaseClass, recordId As Integer) As GroupModel
            Return create(Of GroupModel)(cp, recordId)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function create(cp As CPBaseClass, recordGuid As String) As GroupModel
            Return create(Of GroupModel)(cp, recordGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function createByName(cp As CPBaseClass, recordName As String) As GroupModel
            Return createByName(Of GroupModel)(cp, recordName)
        End Function
        '
        '====================================================================================================
        Public Overloads Sub save(cp As CPBaseClass)
            MyBase.save(cp)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Sub delete(cp As CPBaseClass, recordId As Integer)
            delete(Of GroupModel)(cp, recordId)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Sub delete(cp As CPBaseClass, ccGuid As String)
            delete(Of GroupModel)(cp, ccGuid)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Function createList(cp As CPBaseClass, sqlCriteria As String, Optional sqlOrderBy As String = "id") As List(Of GroupModel)
            Return createList(Of GroupModel)(cp, sqlCriteria, sqlOrderBy)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordName(cp As CPBaseClass, recordId As Integer) As String
            Return baseModel.getRecordName(Of GroupModel)(cp, recordId)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordName(cp As CPBaseClass, ccGuid As String) As String
            Return baseModel.getRecordName(Of GroupModel)(cp, ccGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordId(cp As CPBaseClass, ccGuid As String) As Integer
            Return baseModel.getRecordId(Of GroupModel)(cp, ccGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getCount(cp As CPBaseClass, sqlCriteria As String) As Integer
            Return baseModel.getCount(Of GroupModel)(cp, sqlCriteria)
        End Function
        '
        '====================================================================================================
        Public Overloads Function getUploadPath(fieldName As String) As String
            Return MyBase.getUploadPath(Of GroupModel)(fieldName)
        End Function
        '
        '====================================================================================================
        '
        Public Function Clone(cp As CPBaseClass) As GroupModel
            Dim result As GroupModel = DirectCast(Me.Clone(), GroupModel)
            result.id = cp.Content.AddRecord(contentName)
            result.ccguid = cp.Utils.CreateGuid()
            result.save(cp)
            Return result
        End Function
        ''' <summary>
        ''' returns a list of groups that are associated to a db contact us settings record
        ''' </summary>
        ''' <param name="cp"></param>
        ''' <param name="dbContactUsId"></param>
        ''' <returns></returns>
        Public Shared Function getContactUsGroupList(cp As CPBaseClass, dbContactUsId As Integer) As List(Of GroupModel)
            Return createList(cp, "(id in (select groupid from DBContactUsRules where dbContactUsId=" & dbContactUsId & "))")

        End Function
        '
        '====================================================================================================
        '
        Public Function Clone() As Object Implements ICloneable.Clone
            Return Me.MemberwiseClone()
        End Function
        ''
        ''====================================================================================================
        '''' <summary>
        '''' Get the id of the Site Managers Group
        '''' </summary>
        '''' <param name="cp"></param>
        '''' <returns></returns>
        'Public Shared Function getSiteManagersGroupId(cp As CPBaseClass) As Integer
        '    Dim result As Integer = 0
        '    Try
        '        Dim group As GroupModel = create(cp, guidSiteManagersGroup)
        '        If (group Is Nothing) Then
        '            group = add(cp)
        '            group.name = siteManagersGroupName
        '            group.Caption = group.name
        '            group.save(cp)
        '        End If
        '        result = group.id
        '    Catch ex As Exception
        '        cp.Site.ErrorReport(ex)
        '    End Try
        '    Return result
        'End Function
    End Class
End Namespace
