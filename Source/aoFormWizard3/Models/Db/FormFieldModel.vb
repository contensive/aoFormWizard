

Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Models.Db
    Public Class FormFieldModel        '<------ set set model Name and everywhere that matches this string
        Inherits BaseModel
        Implements ICloneable
        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "Form Fields"      '<------ set content name
        Public Const contentTableName As String = "ccFormFields"   '<------ set to tablename for the primary content (used for cache names)
        Private Shadows Const contentDataSource As String = "default"             '<------ set to datasource if not default
        '
        '====================================================================================================
        ' -- instance properties
        Public Property buttonactionid As Integer
        Public Property caption As String
        Public Property headline As String
        Public Property description As String
        Public Property contentfieldid As Integer
        Public Property formid As Integer
        ''' <summary>
        ''' Field type, string, can be "checkbox", "radio", "file", "text"
        ''' </summary>
        ''' <returns></returns>
        Public Property inputtype As String
        Public Property replacetext As String
        Public Property required As Boolean
        ''' <summary>
        ''' Comma delimited list of options
        ''' </summary>
        ''' <returns></returns>
        Public Property optionList As String
        '
        '====================================================================================================
        Public Overloads Shared Function add(cp As CPBaseClass) As FormFieldModel
            Return add(Of FormFieldModel)(cp)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function create(cp As CPBaseClass, recordId As Integer) As FormFieldModel
            Return create(Of FormFieldModel)(cp, recordId)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function create(cp As CPBaseClass, recordGuid As String) As FormFieldModel
            Return create(Of FormFieldModel)(cp, recordGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function createByName(cp As CPBaseClass, recordName As String) As FormFieldModel
            Return createByName(Of FormFieldModel)(cp, recordName)
        End Function
        '
        '====================================================================================================
        Public Overloads Sub save(cp As CPBaseClass)
            MyBase.save(cp)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Sub delete(cp As CPBaseClass, recordId As Integer)
            delete(Of FormFieldModel)(cp, recordId)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Sub delete(cp As CPBaseClass, ccGuid As String)
            delete(Of FormFieldModel)(cp, ccGuid)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Function createList(cp As CPBaseClass, sqlCriteria As String, Optional sqlOrderBy As String = "id") As List(Of FormFieldModel)
            Return createList(Of FormFieldModel)(cp, sqlCriteria, sqlOrderBy)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordName(cp As CPBaseClass, recordId As Integer) As String
            Return BaseModel.getRecordName(Of FormFieldModel)(cp, recordId)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordName(cp As CPBaseClass, ccGuid As String) As String
            Return BaseModel.getRecordName(Of FormFieldModel)(cp, ccGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordId(cp As CPBaseClass, ccGuid As String) As Integer
            Return BaseModel.getRecordId(Of FormFieldModel)(cp, ccGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getCount(cp As CPBaseClass, sqlCriteria As String) As Integer
            Return BaseModel.getCount(Of FormFieldModel)(cp, sqlCriteria)
        End Function
        '
        '====================================================================================================
        Public Overloads Function getUploadPath(fieldName As String) As String
            Return MyBase.getUploadPath(Of FormFieldModel)(fieldName)
        End Function
        '
        '====================================================================================================
        '
        Public Function clone(cp As CPBaseClass) As FormFieldModel
            Dim result As FormFieldModel = DirectCast(Me.Clone(), FormFieldModel)
            result.id = cp.Content.AddRecord(contentName)
            result.ccguid = cp.Utils.CreateGuid()
            result.save(cp)
            Return result
        End Function
        '
        '====================================================================================================
        '
        Public Function Clone() As Object Implements ICloneable.Clone
            Return Me.MemberwiseClone()
        End Function

    End Class
End Namespace
