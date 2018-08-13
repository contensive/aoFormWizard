
Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses
Imports Newtonsoft.Json

Namespace Contensive.Addon.AddonCollectionVb.Controllers
    Public Class UserFormResponseModel
        Inherits baseModel
        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "User Form Response"                   '<------ set content name
        Public Const contentTableName As String = "ccUserFormResponse"            '<------ set to tablename for the primary content (used for cache names)
        Private Shadows Const contentDataSource As String = "default"   '<------ set to datasource if not default
        '
        '====================================================================================================
        ' -- instance properties
        Public Property DataSourceID As Integer                         '<------ replace this with a list all model fields not part of the base model
        '
        '====================================================================================================
        Public Overloads Shared Function add(cp As CPBaseClass) As UserFormResponseModel
            Return add(Of UserFormResponseModel)(cp)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function create(cp As CPBaseClass, recordId As Integer) As UserFormResponseModel
            Return create(Of UserFormResponseModel)(cp, recordId)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function create(cp As CPBaseClass, recordGuid As String) As UserFormResponseModel
            Return create(Of UserFormResponseModel)(cp, recordGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function createByName(cp As CPBaseClass, recordName As String) As UserFormResponseModel
            Return createByName(Of UserFormResponseModel)(cp, recordName)
        End Function
        '
        '====================================================================================================
        Public Overloads Sub save(cp As CPBaseClass)
            MyBase.save(cp)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Sub delete(cp As CPBaseClass, recordId As Integer)
            delete(Of UserFormResponseModel)(cp, recordId)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Sub delete(cp As CPBaseClass, ccGuid As String)
            delete(Of UserFormResponseModel)(cp, ccGuid)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Function createList(cp As CPBaseClass, sqlCriteria As String, Optional sqlOrderBy As String = "id") As List(Of UserFormResponseModel)
            Return createList(Of UserFormResponseModel)(cp, sqlCriteria, sqlOrderBy)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordName(cp As CPBaseClass, recordId As Integer) As String
            Return baseModel.getRecordName(Of UserFormResponseModel)(cp, recordId)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordName(cp As CPBaseClass, ccGuid As String) As String
            Return baseModel.getRecordName(Of UserFormResponseModel)(cp, ccGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordId(cp As CPBaseClass, ccGuid As String) As Integer
            Return baseModel.getRecordId(Of UserFormResponseModel)(cp, ccGuid)
        End Function
    End Class
End Namespace
