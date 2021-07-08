Imports System.Text.RegularExpressions
Imports Contensive.BaseClasses

Namespace Controllers
    Public Class CustomContentController
        Public Shared Function verifyCustomContent(cp As CPBaseClass, customContentName As String) As Boolean
            Try
                Dim status As Boolean = False
                Dim createTable As Boolean = False
                Using cs As CPCSBaseClass = cp.CSNew()
                    If (Not cs.Open("Content", "name=" + cp.Db.EncodeSQLText(customContentName))) Then
                        createTable = True
                    Else
                        'this table exists
                        status = True
                    End If
                End Using

                If createTable Then
                    Dim tableName As String = ""
                    'remove any spaces since this is for sql table "[^A-Za-z0-9]+"
                    Dim sqlContentName = Regex.Replace(customContentName, "[^A-Za-z0-9]+", "")
                    tableName = "formwizard" & sqlContentName
                    Dim tableid As Integer = cp.Content.AddContent(customContentName, tableName)
                    If tableid <= 0 Then
                        cp.Site.ErrorReport("formwizard verifyCustomContent: could not create content for " + customContentName)
                        cp.Site.LogAlarm("formwizard verifyCustomContent: could not create content for " + customContentName)
                        Return False
                    End If
                    status = True
                End If

                Return status
            Catch ex As Exception
                cp.Site.ErrorReport(ex)
                cp.Site.LogAlarm("formwizard verifyCustomContent: " + ex.ToString())
                Return False
            End Try
        End Function
        Public Shared Function createCustomContentField(cp As CPBaseClass, customContentName As String, fieldName As String, fieldType As Integer) As Boolean
            Try
                Dim status As Boolean = False
                Dim tableExists As Boolean = False
                Using cs As CPCSBaseClass = cp.CSNew()
                    If (cs.Open("Content", "name=" + cp.Db.EncodeSQLText(customContentName))) Then
                        tableExists = True
                    End If
                End Using

                If tableExists Then
                    Dim fieldid As Integer = cp.Content.AddContentField(customContentName, fieldName, fieldType)
                    If fieldid <= 0 Then
                        cp.Site.ErrorReport("formwizard createCustomContentField: could not create content field for content:" + customContentName + " and field:" + fieldName)
                        cp.Site.LogAlarm("formwizard createCustomContentField: could not create content field for content:" + customContentName + " and field:" + fieldName)
                        Return False
                    End If
                    status = True
                End If

                Return status
            Catch ex As Exception
                cp.Site.ErrorReport(ex)
                cp.Site.LogAlarm("formwizard createCustomContentField: " + ex.ToString())
                Return False
            End Try
        End Function
    End Class
End Namespace