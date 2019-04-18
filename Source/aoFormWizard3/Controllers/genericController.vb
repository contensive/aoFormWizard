

Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses
Imports Contensive.Addon.aoFormWizard3

Namespace Controllers
    Public NotInheritable Class genericController
        Private Sub New()
        End Sub
        '
        '====================================================================================================
        ''' <summary>
        ''' if date is invalid, set to minValue
        ''' </summary>
        ''' <param name="srcDate"></param>
        ''' <returns></returns>
        Public Shared Function encodeMinDate(srcDate As DateTime) As DateTime
            Dim returnDate As DateTime = srcDate
            If srcDate < New DateTime(1900, 1, 1) Then
                returnDate = DateTime.MinValue
            End If
            Return returnDate
        End Function
        '
        '====================================================================================================
        ''' <summary>
        ''' if valid date, return the short date, else return blank string 
        ''' </summary>
        ''' <param name="srcDate"></param>
        ''' <returns></returns>
        Public Shared Function getShortDateString(srcDate As DateTime) As String
            Dim returnString As String = ""
            Dim workingDate As DateTime = encodeMinDate(srcDate)
            If Not isDateEmpty(srcDate) Then
                returnString = workingDate.ToShortDateString()
            End If
            Return returnString
        End Function
        '
        '====================================================================================================
        Public Shared Function isDateEmpty(srcDate As DateTime) As Boolean
            Return (srcDate < New DateTime(1900, 1, 1))
        End Function
        '
        '====================================================================================================
        Public Shared Function getSortOrderFromInteger(id As Integer) As String
            Return id.ToString().PadLeft(7, "0"c)
        End Function
        '
        '====================================================================================================
        Public Shared Function getDateForHtmlInput(source As DateTime) As String
            If isDateEmpty(source) Then
                Return ""
            Else
                Return source.Year + "-" + source.Month.ToString().PadLeft(2, "0"c) + "-" + source.Day.ToString().PadLeft(2, "0"c)
            End If
        End Function
        '
        '====================================================================================================
        ''' <summary>
        ''' convert string into a style "height: {styleHeight};", if value is numeric it adds "px"
        ''' </summary>
        ''' <param name="styleheight"></param>
        ''' <returns></returns>
        Public Shared Function encodeStyleHeight(styleheight As String) As String
            Return If(String.IsNullOrWhiteSpace(styleheight), String.Empty, "overflow:hidden;height:" & styleheight & If(IsNumeric(styleheight), "px", String.Empty) & ";")
        End Function
        '
        '====================================================================================================
        ''' <summary>
        ''' convert string into a style "background-image: url(backgroundImage)
        ''' </summary>
        ''' <param name="backgroundImage"></param>
        ''' <returns></returns>
        Public Shared Function encodeStyleBackgroundImage(cp As CPBaseClass, backgroundImage As String) As String
            Return If(String.IsNullOrWhiteSpace(backgroundImage), String.Empty, "background-image: url(" & cp.Site.FilePath & backgroundImage & ");")
        End Function
        '
        '
        '
        Public Shared Function addEditWrapper(ByVal cp As CPBaseClass, ByVal innerHtml As String, ByVal instanceId As Integer, ByVal instanceName As String, ByVal contentName As String, ByVal designBlockCaption As String) As String
            If (Not cp.User.IsEditingAnything) Then Return innerHtml
            Dim editLink As String = getEditLink(cp, contentName, instanceId, designBlockCaption)
            Dim settingContent As String = cp.Html.div(innerHtml, "", "dbSettingWrapper")
            Dim settingHeader As String = cp.Html.div(editLink, "", "dbSettingHeader")
            Return cp.Html.div(settingHeader + settingContent)
        End Function
        '
        '
        Public Shared Function getEditLink(ByVal cp As CPBaseClass, ByVal contentName As String, ByVal recordId As Integer, Caption As String) As String
            Dim contentId As Integer = cp.Content.GetID(contentName)
            If contentId = 0 Then Return String.Empty
            Return "<a href=""/admin?af=4&aa=2&ad=1&cid=" & contentId & "&id=" & recordId & """ class=""ccRecordEditLink""><span style=""color:#0c0""><i title=""edit"" class=""fas fa-cog""></i></span>&nbsp;" & Caption & "</a>"
        End Function
    End Class
End Namespace

