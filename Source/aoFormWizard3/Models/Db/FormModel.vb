﻿

Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Models.Db
    Public Class FormModel
        Inherits BaseModel
        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "Forms"
        Public Const contentTableName As String = "ccForms"
        Private Shadows Const contentDataSource As String = "default"
        '
        '====================================================================================================
        ' -- instance properties
        Public Property addbackbutton As Boolean
        Public Property backbuttonname As String
        Public Property addcancelbutton As Boolean
        Public Property cancelbuttonname As String
        Public Property addcontinuebutton As Boolean
        Public Property continuebuttonname As String
        Public Property contentid As Integer
        Public Property formsetid As Integer
        'Public Property htmlbody As String
        Public Property newcontentname As String
        Public Property nextformid As Integer
        Public Property description As String
        ''' <summary>
        ''' lookup (1=no-save, 2=people-save, 3=org-save, 4=custom-content-save)
        ''' </summary>
        ''' <returns></returns>
        Public Property saveTypeId As Integer
        ''' <summary>
        ''' text field to enter a custom content where data for this form should be saved
        ''' tablename for this content should be 'formWizard' + normalize(saveContent). normalize should validate the allowed characters for sql server tables.
        ''' </summary>
        ''' <returns></returns>
        Public Property saveCustomContent As String
        ''' <summary>
        ''' deprecated, see saveTypeId
        ''' </summary>
        ''' <returns></returns>
        Public Property authcontent As Integer
        ''' <summary>
        ''' deprecated, see saveTypeId
        ''' </summary>
        ''' <returns></returns>
        Public Property useauthmembercontent As Boolean
        ''' <summary>
        ''' deprecated, see saveTypeId
        ''' </summary>
        ''' <returns></returns>
        Public Property useauthorgcontent As Boolean
        '
        '====================================================================================================
        Public Overloads Shared Function add(cp As CPBaseClass) As FormModel
            Return add(Of FormModel)(cp)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function create(cp As CPBaseClass, recordId As Integer) As FormModel
            Return create(Of FormModel)(cp, recordId)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function create(cp As CPBaseClass, recordGuid As String) As FormModel
            Return create(Of FormModel)(cp, recordGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function createByName(cp As CPBaseClass, recordName As String) As FormModel
            Return createByName(Of FormModel)(cp, recordName)
        End Function
        '
        '====================================================================================================
        Public Overloads Sub save(cp As CPBaseClass)
            MyBase.save(cp)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Sub delete(cp As CPBaseClass, recordId As Integer)
            delete(Of FormModel)(cp, recordId)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Sub delete(cp As CPBaseClass, ccGuid As String)
            delete(Of FormModel)(cp, ccGuid)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Function createList(cp As CPBaseClass, sqlCriteria As String, Optional sqlOrderBy As String = "id") As List(Of FormModel)
            Return createList(Of FormModel)(cp, sqlCriteria, sqlOrderBy)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordName(cp As CPBaseClass, recordId As Integer) As String
            Return BaseModel.getRecordName(Of FormModel)(cp, recordId)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordName(cp As CPBaseClass, ccGuid As String) As String
            Return BaseModel.getRecordName(Of FormModel)(cp, ccGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordId(cp As CPBaseClass, ccGuid As String) As Integer
            Return BaseModel.getRecordId(Of FormModel)(cp, ccGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getCount(cp As CPBaseClass, sqlCriteria As String) As Integer
            Return BaseModel.getCount(Of FormModel)(cp, sqlCriteria)
        End Function
        '
        '====================================================================================================
        Public Overloads Function getUploadPath(fieldName As String) As String
            Return MyBase.getUploadPath(Of FormModel)(fieldName)
        End Function
    End Class
End Namespace