
Imports System.Linq
Imports Contensive.Addon.aoFormWizard3.Models.Db
Imports Contensive.BaseClasses

Namespace Models.View
    ''' <summary>
    ''' Construct the view to be displayed (the current form)
    ''' </summary>
    Public Class FormViewModel
        Inherits ViewBaseModel
        '       
        Public Property id As Integer
        Public Property pageHeader As String
        Public Property pageDescription As String
        Public Property listOfFieldsClass As New List(Of FieldViewModel)
        Public Property fieldAddLink As String
        Public Property previousButton As String
        Public Property cancelButton As String
        Public Property submitButton As String
        Public Property continueButton As String
        Public Property formEditWrapper As String
        Public Property formdEditLink As String
        Public Property formAddLink As String
        ' 
        Public Class FieldViewModel
            Public Property inputtype As String
            Public Property caption As String
            Public Property headline As String
            Public Property fielddescription As String
            Public Property required As Boolean
            Public Property name As String
            Public Property isCheckbox As Boolean
            Public Property isRadio As Boolean
            Public Property isSelect As Boolean
            Public Property isTextArea As Boolean
            Public Property isDefault As Boolean
            Public Property id As Integer
            Public Property optionList As New List(Of OptionClass)
            Public Property fieldEditWrapper As String
            Public Property fieldEditLink As String
        End Class
        '
        Public Class OptionClass
            Public Property optionName As String
            Public Property optionPtr As Integer
        End Class
        '
        Public Class ButtonClass
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
        Public Overloads Shared Function create(cp As CPBaseClass, settings As FormSetModel) As FormViewModel
            Try
                '
                ' -- base fields
                Dim formViewData = create(Of FormViewModel)(cp, settings)
                formViewData.id = settings.id
                ' 
                Dim formlist As List(Of FormModel) = FormModel.createList(cp, "(formsetid=" & settings.id & ")", "sortorder")
                If (formlist.Count > 0) Then
                    Dim firstForm As FormModel = formlist.First
                    '
                    ' -- output one page with page one header
                    Dim isEditing As Boolean = cp.User.IsEditingAnything()
                    formViewData.pageHeader = firstForm.name
                    formViewData.pageDescription = firstForm.description
                    formViewData.previousButton = If(firstForm.addbackbutton, "Previous", "")
                    formViewData.cancelButton = If(firstForm.addcancelbutton, firstForm.cancelbuttonname, "")
                    formViewData.submitButton = ""
                    formViewData.continueButton = If(firstForm.addcontinuebutton, firstForm.continuebuttonname, "")


                    Dim fieldEditLink As String = ""
                    If (isEditing) Then
                        formViewData.formEditWrapper = "ccEditWrapper"
                        formViewData.formdEditLink = cp.Content.GetEditLink(FormModel.contentName, firstForm.id, False, "", isEditing)
                        fieldEditLink = cp.Content.GetEditLink(FormFieldModel.contentName, 99999, False, "", isEditing)
                    End If

                    For Each form In formlist
                        Dim formFieldList As List(Of FormFieldModel) = FormFieldModel.createList(cp, "(formid=" & form.id & ")", "sortorder, id")
                        Dim fieldPtr As Integer = 0
                        For Each formField In formFieldList
                            Dim optionList As New List(Of OptionClass)
                            Dim optionPtr As Integer = 1
                            For Each formfieldoption In formField.optionList.Split(",")
                                optionList.Add(New OptionClass() With {
                                        .optionName = formfieldoption,
                                        .optionPtr = optionPtr
                                })
                                optionPtr += 1
                            Next
                            Select Case formField.inputtype.ToLower()
                                Case "radio"
                                    Dim caption = formField.caption
                                    If (String.IsNullOrEmpty(caption)) Then
                                        caption = formField.name
                                    End If
                                    formViewData.listOfFieldsClass.Add(New FieldViewModel() With {
                                        .caption = caption,
                                        .inputtype = formField.inputtype,
                                        .required = formField.required,
                                        .name = formField.name,
                                        .headline = formField.headline,
                                        .fielddescription = formField.description,
                                        .isCheckbox = False,
                                        .isDefault = False,
                                        .isTextArea = False,
                                        .isRadio = True,
                                        .isSelect = False,
                                        .id = formField.id,
                                        .optionList = optionList,
                                        .fieldEditLink = fieldEditLink.Replace("99999", formField.id),
                                        .fieldEditWrapper = If(isEditing, "ccEditWrapper", "")
                                    })
                                Case "select"
                                    Dim caption = formField.caption
                                    If (String.IsNullOrEmpty(caption)) Then
                                        caption = formField.name
                                    End If
                                    formViewData.listOfFieldsClass.Add(New FieldViewModel() With {
                                        .caption = caption,
                                        .inputtype = formField.inputtype,
                                        .required = formField.required,
                                        .name = formField.name,
                                        .headline = formField.headline,
                                        .fielddescription = formField.description,
                                        .isCheckbox = False,
                                        .isDefault = False,
                                        .isTextArea = False,
                                        .isRadio = False,
                                        .isSelect = True,
                                        .id = formField.id,
                                        .optionList = optionList,
                                        .fieldEditLink = fieldEditLink.Replace("99999", formField.id),
                                        .fieldEditWrapper = If(isEditing, "ccEditWrapper", "")
                                    })
                                Case "checkbox"
                                    Dim caption = formField.caption
                                    If (String.IsNullOrEmpty(caption)) Then
                                        caption = formField.name
                                    End If
                                    formViewData.listOfFieldsClass.Add(New FieldViewModel() With {
                                        .caption = caption,
                                        .inputtype = formField.inputtype,
                                        .required = formField.required,
                                        .name = formField.name,
                                        .headline = formField.headline,
                                        .fielddescription = formField.description,
                                        .isCheckbox = True,
                                        .isDefault = False,
                                        .isTextArea = False,
                                        .isRadio = False,
                                        .isSelect = False,
                                        .id = formField.id,
                                        .optionList = optionList,
                                        .fieldEditLink = fieldEditLink.Replace("99999", formField.id),
                                        .fieldEditWrapper = If(isEditing, "ccEditWrapper", "")
                                    })
                                Case "textarea"
                                    Dim caption = formField.caption
                                    If (String.IsNullOrEmpty(caption)) Then
                                        caption = formField.name
                                    End If
                                    formViewData.listOfFieldsClass.Add(New FieldViewModel() With {
                                        .caption = caption,
                                        .inputtype = formField.inputtype,
                                        .required = formField.required,
                                        .name = formField.name,
                                        .headline = formField.headline,
                                        .fielddescription = formField.description,
                                        .isCheckbox = False,
                                        .isDefault = False,
                                        .isTextArea = True,
                                        .isRadio = False,
                                        .isSelect = False,
                                        .id = formField.id,
                                        .optionList = optionList,
                                        .fieldEditLink = fieldEditLink.Replace("99999", formField.id),
                                        .fieldEditWrapper = If(isEditing, "ccEditWrapper", "")
                                    })
                                Case Else
                                    Dim caption = formField.caption
                                    If (String.IsNullOrEmpty(caption)) Then
                                        caption = formField.name
                                    End If
                                    formViewData.listOfFieldsClass.Add(New FieldViewModel() With {
                                        .caption = caption,
                                        .inputtype = formField.inputtype,
                                        .required = formField.required,
                                        .name = formField.name,
                                        .headline = formField.headline,
                                        .fielddescription = formField.description,
                                        .isCheckbox = False,
                                        .isDefault = True,
                                        .isTextArea = False,
                                        .isRadio = False,
                                        .isSelect = False,
                                        .id = formField.id,
                                        .optionList = optionList,
                                        .fieldEditLink = fieldEditLink.Replace("99999", formField.id),
                                        .fieldEditWrapper = If(isEditing, "ccEditWrapper", "")
                                    })
                            End Select
                            fieldPtr += 1
                        Next
                        formViewData.fieldAddLink = cp.Content.GetAddLink(FormFieldModel.contentName, "formid=" & form.id, False, isEditing)
                    Next
                    formViewData.formAddLink = cp.Content.GetAddLink(FormModel.contentName, "formsetid=" & settings.id, False, isEditing)
                End If
                Return formViewData
            Catch ex As Exception
                cp.Site.ErrorReport(ex)
                Return Nothing
            End Try
        End Function
    End Class
End Namespace