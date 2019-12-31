
Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Models.Db
    Public Class FormSetModel        '<------ set set model Name and everywhere that matches this string
        Inherits DbBaseModel

        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "Form Sets"      '<------ set content name
        Public Const contentTableName As String = "ccFormSets"   '<------ set to tablename for the primary content (used for cache names)
        Private Shadows Const contentDataSource As String = "default"             '<------ set to datasource if not default
        '
        '====================================================================================================
        ' -- instance properties
        Public Property joingroupid As Integer
        Public Property notificationemailid As Integer
        Public Property responseemailid As Integer
        Public Property thankyoucopy As String
        '
        '====================================================================================================
        Public Overloads Shared Function createOrAddSettings(cp As CPBaseClass, settingsGuid As String) As FormSetModel
            Dim formSet As FormSetModel = create(Of FormSetModel)(cp, settingsGuid)
            If (formSet Is Nothing) Then
                '
                ' -- create default formset
                formSet = DbBaseModel.add(Of FormSetModel)(cp)
                formSet.name = "Dynamic Form " & formSet.id
                formSet.ccguid = settingsGuid
                formSet.save(cp)
                '
                ' -- add form one
                Dim formOne As FormModel = DbBaseModel.add(Of FormModel)(cp)
                formOne.name = "Form #1 of " & formSet.name
                formOne.formsetid = formSet.id
                formOne.addbackbutton = False
                formOne.addcancelbutton = False
                formOne.addcontinuebutton = True
                formOne.continuebuttonname = "Complete"
                formOne.description = "This form was automatically created by the dynamic form Design Block."
                formOne.htmlbody = "<h4>Dynamic Form Page 1</h4><p>This is the form body for page 1 of the sample form.</p>"
                formOne.save(cp)
                '
                ' -- form 1 field A
                Dim formOneFieldA As FormFieldModel = DbBaseModel.add(Of FormFieldModel)(cp)
                formOneFieldA.formid = formOne.id
                formOneFieldA.caption = "Text Field Caption"
                formOneFieldA.description = "Text Field Description"
                formOneFieldA.headline = "Text Field Headline"
                formOneFieldA.inputtype = "text"
                formOneFieldA.name = "Text Field Name"
                formOneFieldA.optionList = "a,b,c,d,e,f,g"
                formOneFieldA.replacetext = "replace-text"
                formOneFieldA.required = True
                formOneFieldA.sortOrder = "01"
                formOneFieldA.save(cp)
                '
                ' -- form 1 field B
                Dim formOneFieldB As FormFieldModel = DbBaseModel.add(Of FormFieldModel)(cp)
                formOneFieldB.formid = formOne.id
                formOneFieldB.caption = "Checkbox Field Caption"
                formOneFieldB.description = "Checkbox Field Description"
                formOneFieldB.headline = "Checkbox Field Headline"
                formOneFieldB.inputtype = "checkbox"
                formOneFieldB.name = "Checkbox Field Name"
                formOneFieldB.optionList = "a,b,c,d,e,f,g"
                formOneFieldB.replacetext = "replace-text"
                formOneFieldB.required = True
                formOneFieldB.sortOrder = "02"
                formOneFieldB.save(cp)
                '
                ' -- form 1 field C
                Dim formOneFieldC As FormFieldModel = DbBaseModel.add(Of FormFieldModel)(cp)
                formOneFieldC.formid = formOne.id
                formOneFieldC.caption = "Radio Field Caption"
                formOneFieldC.description = "Radio Field Description"
                formOneFieldC.headline = "Radio Field Headline"
                formOneFieldC.inputtype = "radio"
                formOneFieldC.name = "Radio Field Name"
                formOneFieldC.optionList = "a,b,c,d,e,f,g"
                formOneFieldC.replacetext = "replace-text"
                formOneFieldC.required = True
                formOneFieldC.sortOrder = "02"
                formOneFieldC.save(cp)
                '
                ' -- form 1 field D
                Dim formOneFieldD As FormFieldModel = DbBaseModel.add(Of FormFieldModel)(cp)
                formOneFieldD.formid = formOne.id
                formOneFieldD.caption = "File Field Caption"
                formOneFieldD.description = "File Field Description"
                formOneFieldD.headline = "File Field Headline"
                formOneFieldD.inputtype = "File"
                formOneFieldD.name = "File Field Name"
                formOneFieldD.optionList = "a,b,c,d,e,f,g"
                formOneFieldD.replacetext = "replace-text"
                formOneFieldD.required = True
                formOneFieldD.sortOrder = "02"
                formOneFieldD.save(cp)
                '
                ' -- add form two
                Dim formTwo As FormModel = DbBaseModel.add(Of FormModel)(cp)
                formTwo.name = "Form #2 of " & formSet.name
                formTwo.formsetid = formSet.id
                formTwo.addbackbutton = False
                formTwo.addcancelbutton = False
                formTwo.addcontinuebutton = True
                formTwo.continuebuttonname = "Complete"
                formTwo.description = "This form was automatically created by the dynamic form Design Block."
                formTwo.htmlbody = "<h4>Dynamic Form Page 1</h4><p>This is the form body for page 1 of the sample form.</p>"
                formTwo.save(cp)
                '
                ' -- form 2 field A
                Dim formTwoFieldA As FormFieldModel = DbBaseModel.add(Of FormFieldModel)(cp)
                formTwoFieldA.formid = formTwo.id
                formTwoFieldA.caption = "Text Field Caption"
                formTwoFieldA.description = "Text Field Description"
                formTwoFieldA.headline = "Text Field Headline"
                formTwoFieldA.inputtype = "text"
                formTwoFieldA.name = "Text Field Name"
                formTwoFieldA.optionList = "a,b,c,d,e,f,g"
                formTwoFieldA.replacetext = "replace-text"
                formTwoFieldA.required = True
                formTwoFieldA.sortOrder = "01"
                formTwoFieldA.save(cp)
                '
                formOne.save(cp)
                '
                ' -- create custom content
                '
            End If
            Return formSet
        End Function

    End Class
End Namespace

