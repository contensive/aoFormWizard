

Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Models.Db
    Public Class PageContentModel
        Inherits baseModel
        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "page content"
        Public Const contentTableName As String = "ccpagecontent"
        Private Shadows Const contentDataSource As String = "default"
        '
        '====================================================================================================
        ' -- instance properties
        Public Property allowbrief As Boolean
        Public Property allowchildlistdisplay As Boolean
        Public Property allowfeedback As Boolean
        Public Property allowhitnotification As Boolean
        Public Property allowinchildlists As Boolean
        Public Property allowinmenus As Boolean
        Public Property allowlastmodifiedfooter As Boolean
        Public Property allowmessagefooter As Boolean
        Public Property allowmetacontentnofollow As Boolean
        Public Property allowmoreinfo As Boolean
        Public Property allowreturnlinkdisplay As Boolean
        Public Property allowreviewedfooter As Boolean
        Public Property allowseealso As Boolean
        Public Property archiveparentid As Integer
        Public Property blockcontent As Boolean
        Public Property blockpage As Boolean
        Public Property blocksourceid As Integer
        Public Property brieffilename As String
        Public Property childlistinstanceoptions As String
        Public Property childlistsortmethodid As Integer
        Public Property childpagesfound As Boolean
        Public Property clicks As Integer
        Public Property collectionid As Integer
        Public Property contactmemberid As Integer
        Public Property contentpadding As Integer
        Public Property copyfilename As String
        Public Property customblockmessage As String
        Public Property datearchive As Date
        Public Property dateexpires As Date
        Public Property datereviewed As Date
        Public Property headline As String
        Public Property imagefilename As String
        Public Property issecure As Boolean
        Public Property jsendbody As String
        Public Property jsfilename As String
        Public Property jshead As String
        Public Property jsonload As String
        Public Property link As String
        Public Property linkalias As String
        Public Property linklabel As String
        Public Property menuclass As String
        Public Property menuheadline As String
        Public Property metadescription As String
        Public Property metakeywordlist As String
        Public Property otherheadtags As String
        Public Property pagelink As String
        Public Property pagetitle As String
        Public Property parentid As Integer
        Public Property parentlistname As String
        Public Property pubdate As Date
        Public Property registrationgroupid As Integer
        Public Property reviewedby As Integer
        Public Property templateid As Integer
        Public Property triggeraddgroupid As Integer
        Public Property triggerconditiongroupid As Integer
        Public Property triggerconditionid As Integer
        Public Property triggerremovegroupid As Integer
        Public Property triggersendsystememailid As Integer
        Public Property viewings As Integer
        '
        '
        '
        Public Shared Function getRootPageId(cp As CPBaseClass, pageId As Integer, usedIdList As List(Of Integer)) As Integer
            If (usedIdList.Contains(pageId)) Then Return 0
            usedIdList.Add(pageId)
            Dim page = DbBaseModel.create(Of PageContentModel)(cp, pageId)
            If (page Is Nothing) Then Return 0
            If (page.parentid = 0) Then Return pageId
            Return getRootPageId(cp, page.parentid, usedIdList)
        End Function
    End Class
End Namespace
