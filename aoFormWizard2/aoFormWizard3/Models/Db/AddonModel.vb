

Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Models.Db
    Public Class AddonModel
        Inherits baseModel
        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "Add-ons"
        Public Const contentTableName As String = "ccAggregateFunctions"
        Private Shadows Const contentDataSource As String = "default"
        '
        '====================================================================================================
        ' -- instance properties
        'instancePropertiesGoHere
        ' sample instance property -- Public Property DataSourceID As Integer

    End Class
End Namespace
