
Imports Contensive.Addon.aoFormWizard3.Controllers
Imports Contensive.Addon.aoFormWizard3.Models.View
Imports Contensive.Addon.aoFormWizard3.Models.Db
Imports Contensive.BaseClasses

Namespace Controllers
    Public Class FormWizardController


        ''' <summary>
        ''' Process submitted contact form. Returns true if the form has already been submitted, or successfully commits
        ''' </summary>
        ''' <param name="CP"></param>
        ''' <param name="settings"></param>
        ''' <param name="request"></param>
        ''' <returns></returns>
        Public Shared Function processRequest(ByVal CP As CPBaseClass, settings As Models.Db.FormSetModel, request As Views.DynamicFormClass.Request) As Boolean
            Dim returnHtml As String = String.Empty
            Try
                If (Not request.blockContactFormButton.Equals("Submit")) Then Return False








                If (CP.Visit.GetBoolean("contactFormSubmitted")) Then
                    If (request.blockContactFormButton = buttonValueResubmit) Then
                        CP.Visit.SetProperty("contactFormSubmitted", "0")
                        Return False
                    End If
                    Return True
                End If
                If (settings Is Nothing) Then Return False
                If (String.IsNullOrEmpty(request.blockContactFormButton)) Then Return False
                '
                ' -- set visit property to signal form submitted
                CP.Visit.SetProperty("contactFormSubmitted", "1")

                '
                ' -- authenticate
                If (Not CP.User.IsAuthenticated) Then
                    If (Not CP.User.IsRecognized) Then
                        '
                        ' -- guest, login
                        CP.User.LoginByID(CP.User.Id)
                    Else
                        '
                        ' -- recognized, logout, login
                        CP.User.Logout()
                        CP.User.LoginByID(CP.User.Id)
                    End If
                End If
                '
                ' -- update user record
                'Dim person As PeopleModel = PeopleModel.create(CP, CP.User.Id)
                'person.Email = request.contactEmail
                'If (settings.allowfirstname) Then person.FirstName = request.contactFirstName
                'If (settings.allowlastname) Then person.LastName = request.contactLastName
                'If (settings.allowfirstname Or settings.allowlastname) Then person.name = (person.FirstName & " " & person.LastName).Trim()
                'If (settings.allowcompany) Then person.Company = request.contactCountry
                'person.save(CP)
                ''
                '' -- add user to contact group
                'If (settings.addToGroupId > 0) Then
                '    Dim ruleList = MemberRuleModel.createList(CP, "(groupID=" & settings.addToGroupId & ")and(memberID=" & person.id & ")and((dateExpires is null)or(dateExpires>" & CP.Db.EncodeSQLDate(Now) & "))")
                '    If (ruleList.Count = 0) Then
                '        Dim rule As MemberRuleModel = MemberRuleModel.add(CP)
                '        rule.MemberID = person.id
                '        rule.GroupID = settings.addToGroupId
                '        rule.save(CP)
                '    End If
                'End If
                ''
                'If (request.contactGroupList.Count > 0) Then
                '    For Each contactGroup In request.contactGroupList
                '        Dim ruleList = MemberRuleModel.createList(CP, "(groupID=" & contactGroup.groupId & ")and(memberID=" & person.id & ")and((dateExpires is null)or(dateExpires>" & CP.Db.EncodeSQLDate(Now) & "))")
                '        If (contactGroup.checked) Then
                '            ' 
                '            ' -- add user to group
                '            If (ruleList.Count = 0) Then
                '                Dim rule As MemberRuleModel = MemberRuleModel.add(CP)
                '                rule.MemberID = person.id
                '                rule.GroupID = contactGroup.groupId
                '                rule.save(CP)
                '            End If
                '        Else
                '            '
                '            ' -- remove person from group
                '            For Each rule In ruleList
                '                MemberRuleModel.delete(CP, rule.id)
                '            Next
                '        End If
                '    Next
                'End If
                ''
                '' -- save message
                'Dim message As Models.Db.DbContactUsDataModel = Models.Db.DbContactUsDataModel.add(CP)
                'message.name = CP.User.Name & ", added " & Now.ToString()
                'message.message = request.contactMessage
                'message.userId = CP.User.Id
                'message.save(CP)
                ''
                '' -- send notification email to notification contact
                'If (settings.notificationemailid > 0) Then
                '    Dim additionalCopy As String = "" _
                '        & "<br>" _
                '        & "<br>User: " & person.name _
                '        & "<br>Posted: " & Now.ToString() _
                '        & "<br>Message: " & If(String.IsNullOrEmpty(request.contactMessage), "(no message included)", request.contactMessage)
                '    CP.Email.sendSystem(settings.notificationemailid.ToString(), additionalCopy, settings.sendNotificationToPeopleId)
                'End If
                ''
                '' -- redirect if inclcluded
                'If (Not String.IsNullOrEmpty(settings.RedirectUrl)) Then
                '    CP.Response.Redirect(settings.RedirectUrl)
                'End If
                Return True
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
                Return False
            End Try
        End Function
    End Class
End Namespace
