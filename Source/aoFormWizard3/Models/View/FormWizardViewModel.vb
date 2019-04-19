Imports Contensive.Addon.aoFormWizard3
Imports Contensive.Addon.aoFormWizard3.Models.Db
Imports Contensive.BaseClasses

Namespace Models.View

    Public Class FormWizardViewModel
        Inherits ViewBaseModel
        '       
        Public Property id As Integer
        Public Property headerline As String
        Public Property description As String
        Public Property listOfFieldsClass As New List(Of fieldsClass)
        Public Property listOfButtons As New List(Of buttonClass)
        Public Property previousButton As buttonClass
        Public Property cancelButton As buttonClass
        Public Property submitButton As buttonClass
        Public Property continueButtonName As buttonClass
        '
        ' 
        Public Class fieldsClass
            Public Property inputtype As String
            Public Property caption As String
            Public Property headline As String
            Public Property fielddescription As String
            Public Property required As Boolean
            Public Property name As String
            Public Property isCheckbox As Boolean
            Public Property isRadio As Boolean
            Public Property isDefault As Boolean
            Public Property id As Integer
            Public Property optionList As New List(Of OptionClass)
        End Class
        '
        Public Class OptionClass
            Public Property optionName As String
            Public Property optionPtr As Integer
        End Class
        '
        Public Class buttonClass
            Public Property buttonCaption As String
            Public Property isVisible As Boolean
        End Class
        '
        '====================================================================================================
        ''' <summary>
        ''' Populate the view model from the entity model
        ''' </summary>
        ''' <param name="cp"></param>
        ''' <param name="settings"></param>
        ''' <returns></returns>
        Public Overloads Shared Function create(cp As CPBaseClass, settings As Models.Db.FormSetModel) As FormWizardViewModel
            Try
                '
                ' -- base fields
                Dim result = ViewBaseModel.create(Of FormWizardViewModel)(cp, settings)
                result.id = settings.id
                ' 
                Dim formlist As List(Of FormModel) = FormModel.createList(cp, "(formsetid=" & settings.id & ")")
                For Each form In formlist
                    result.headerline = form.name
                    result.description = form.description
                    Dim formsFieldList As List(Of FormFieldModel) = FormFieldModel.createList(cp, "(formid=" & form.id & ")")
                    Dim fieldPtr As Integer = 0
                    For Each formsField In formsFieldList
                        Dim optionList As New List(Of OptionClass)
                        Dim optionPtr As Integer = 1
                        For Each formfieldoption In formsField.optionList.Split(",")
                            optionList.Add(New OptionClass() With {
                                    .optionName = formfieldoption,
                                    .optionPtr = optionPtr
                            })
                            optionPtr += 1
                        Next
                        Select Case formsField.inputtype.ToLower()
                            Case "radio"
                                result.listOfFieldsClass.Add(New fieldsClass() With {
                                    .caption = formsField.caption,
                                    .inputtype = formsField.inputtype,
                                    .required = formsField.required,
                                    .name = formsField.name,
                                    .headline = formsField.headline,
                                    .fielddescription = formsField.description,
                                    .isCheckbox = False,
                                    .isDefault = False,
                                    .isRadio = True,
                                    .id = formsField.id,
                                    .optionList = optionList
                                })
                            Case "checkbox"
                                result.listOfFieldsClass.Add(New fieldsClass() With {
                                    .caption = formsField.caption,
                                    .inputtype = formsField.inputtype,
                                    .required = formsField.required,
                                    .name = formsField.name,
                                    .headline = formsField.headline,
                                    .fielddescription = formsField.description,
                                    .isCheckbox = True,
                                    .isDefault = False,
                                    .isRadio = False,
                                    .id = formsField.id,
                                    .optionList = optionList
                                })
                            Case Else
                                result.listOfFieldsClass.Add(New fieldsClass() With {
                                    .caption = formsField.caption,
                                    .inputtype = formsField.inputtype,
                                    .required = formsField.required,
                                    .name = formsField.name,
                                    .headline = formsField.headline,
                                    .fielddescription = formsField.description,
                                    .isCheckbox = False,
                                    .isDefault = True,
                                    .isRadio = False,
                                    .id = formsField.id,
                                    .optionList = optionList
                                })
                        End Select
                    Next
                Next
                Return result
            Catch ex As Exception
                cp.Site.ErrorReport(ex)
                Return Nothing
            End Try
        End Function
    End Class
End Namespace