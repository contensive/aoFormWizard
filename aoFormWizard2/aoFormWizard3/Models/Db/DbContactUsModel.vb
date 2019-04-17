
Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Models.Db
    Public Class DbContactUsModel
        Inherits DbBaseModel
        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "DB Contact Us"
        Public Const contentTableName As String = "db Contact Us"
        Private Shadows Const contentDataSource As String = "default"
        '
        '====================================================================================================
        ' -- instance properties
        Public Property formDescription As String
        Public Property formButtonText As String
        Public Property addToGroupId As Integer
        Public Property notificationEmailId As Integer
        Public Property sendNotificationToPeopleId As Integer
        Public Property contactusgrouptypes As String
        Public Property thankYouTitle As String
        Public Property thankYouDescription As String
        Public Property groupListTitle As String
        Public Property RedirectUrl As String
        Public Property allowfirstname As Boolean
        Public Property allowlastname As Boolean
        'Public Property allowemail As Boolean
        Public Property allowcompany As Boolean
        'Public Property allowoptionalgroups As Boolean
        Public Property allowMessage As Boolean
        '
        '====================================================================================================
        Public Overloads Shared Function createOrAddSettings(cp As CPBaseClass, settingsGuid As String) As DbContactUsModel
            Dim result As DbContactUsModel = create(Of DbContactUsModel)(cp, settingsGuid)
            If (result Is Nothing) Then
                '
                ' -- create default content
                result = DbBaseModel.add(Of DbContactUsModel)(cp)
                result.name = contentName & " Instance " & result.id & ", created " & Now.ToString()
                result.contactusgrouptypes = ""
                result.ccguid = settingsGuid
                result.fontStyleId = 0
                result.themeStyleId = 0
                result.padTop = False
                result.padBottom = False
                result.padRight = False
                result.padLeft = False
                result.groupListTitle = "GroupList Title"

                '
                ' -- create custom content
                result.formDescription = "<h2>Contact Us</h2><p>Thank you for taking the time to let us know what you think. We appreciate your input.</p>"
                result.formButtonText = "Submit"
                result.addToGroupId = 0
                'result.notificationEmailId = Models.Db.SystemEmailModel.createContactUsNotification(cp).id
                result.sendNotificationToPeopleId = 0
                result.thankYouTitle = "Thank You"
                result.thankYouDescription = "Thank you for your comments."
                result.allowfirstname = True
                result.allowlastname = True
                result.allowcompany = True
                result.allowMessage = True
                result.RedirectUrl = "/"
                '
                result.save(cp)
            End If
            Return result
        End Function
    End Class
End Namespace
