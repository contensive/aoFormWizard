
Public Module Constants
    '
    ' -- version used to upgrade content
    Public Const version As Integer = 2
    ' Field types. This enum is taken from contensive 5 code
    Public Enum FieldTypeIdEnum
        IntegerType = 1
        Text = 2
        LongText = 3
        BooleanType = 4
        DateType = 5
        File = 6
        Lookup = 7
        Redirect = 8
        Currency = 9
        FileText = 10
        FileImage = 11
        Float = 12
        AutoIdIncrement = 13
        ManyToMany = 14
        MemberSelect = 15
        FileCSS = 16
        FileXML = 17
        FileJavascript = 18
        Link = 19
        ResourceLink = 20
        HTML = 21
        FileHTML = 22
        HTMLCode = 23
        FileHTMLCode = 24
    End Enum
End Module