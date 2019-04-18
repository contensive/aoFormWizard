
Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses
Imports Contensive.Addon.aoFormWizard3

Namespace Models.Db
    Public Class SystemEmailModel
        Inherits baseModel
        Implements ICloneable
        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "System Email"
        Public Const contentTableName As String = "ccEmail"
        Private Shadows Const contentDataSource As String = "default"
        '
        '====================================================================================================
        ' -- instance properties

        Public Property AddLinkEID As Boolean
        Public Property AllowSpamFooter As Boolean
        Public Property BlockSiteStyles As Boolean
        Public Property ConditionExpireDate As Date
        Public Property ConditionID As Integer
        Public Property ConditionPeriod As Integer
        Public Property CopyFilename As String
        Public Property EmailTemplateID As Integer
        Public Property EmailWizardID As Integer
        Public Property FromAddress As String
        Public Property InlineStyles As String
        Public Property LastSendTestDate As Date
        Public Property ScheduleDate As Date
        Public Property Sent As Boolean
        Public Property StylesFilename As String
        Public Property Subject As String
        Public Property Submitted As Boolean
        Public Property TestMemberID As Integer
        Public Property ToAll As Boolean

        '
        '====================================================================================================
        Public Overloads Shared Function add(cp As CPBaseClass) As SystemEmailModel
            Return add(Of SystemEmailModel)(cp)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function create(cp As CPBaseClass, recordId As Integer) As SystemEmailModel
            Return create(Of SystemEmailModel)(cp, recordId)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function create(cp As CPBaseClass, recordGuid As String) As SystemEmailModel
            Return create(Of SystemEmailModel)(cp, recordGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function createByName(cp As CPBaseClass, recordName As String) As SystemEmailModel
            Return createByName(Of SystemEmailModel)(cp, recordName)
        End Function
        '
        '====================================================================================================
        Public Overloads Sub save(cp As CPBaseClass)
            MyBase.save(cp)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Sub delete(cp As CPBaseClass, recordId As Integer)
            delete(Of SystemEmailModel)(cp, recordId)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Sub delete(cp As CPBaseClass, ccGuid As String)
            delete(Of SystemEmailModel)(cp, ccGuid)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Function createList(cp As CPBaseClass, sqlCriteria As String, Optional sqlOrderBy As String = "id") As List(Of SystemEmailModel)
            Return createList(Of SystemEmailModel)(cp, sqlCriteria, sqlOrderBy)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordName(cp As CPBaseClass, recordId As Integer) As String
            Return baseModel.getRecordName(Of SystemEmailModel)(cp, recordId)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordName(cp As CPBaseClass, ccGuid As String) As String
            Return baseModel.getRecordName(Of SystemEmailModel)(cp, ccGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordId(cp As CPBaseClass, ccGuid As String) As Integer
            Return baseModel.getRecordId(Of SystemEmailModel)(cp, ccGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getCount(cp As CPBaseClass, sqlCriteria As String) As Integer
            Return baseModel.getCount(Of SystemEmailModel)(cp, sqlCriteria)
        End Function
        '
        '====================================================================================================
        Public Overloads Function getUploadPath(fieldName As String) As String
            Return MyBase.getUploadPath(Of SystemEmailModel)(fieldName)
        End Function
        '
        '====================================================================================================
        '
        Public Function Clone(cp As CPBaseClass) As SystemEmailModel
            Dim result As SystemEmailModel = DirectCast(Me.Clone(), SystemEmailModel)
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
        '
        '====================================================================================================
        '
        'Public Shared Function createContactUsNotification(cp As CPBaseClass) As SystemEmailModel
        '    Dim result As SystemEmailModel = Nothing
        '    Try
        '        '
        '        ' -- this email gets sent by name. Look this up by guid, bu verify the name
        '        result = Models.Db.SystemEmailModel.create(cp, guidDbContactUsNotificationEmail)
        '        If (result Is Nothing) Then
        '            result = Models.Db.SystemEmailModel.add(cp)
        '            result.ccguid = guidDbContactUsNotificationEmail
        '            result.FromAddress = cp.Site.GetText("EMAILFROMADDRESS")
        '            result.Subject = "Contact Us form submitted"
        '            result.CopyFilename = "<p>The following contact-us form was submitted:</p>"
        '        End If
        '        result.name = "Contact Us Notification"
        '        result.save(cp)
        '    Catch ex As Exception
        '        cp.Site.ErrorReport(ex)
        '    End Try
        '    Return result
        'End Function

    End Class
End Namespace
