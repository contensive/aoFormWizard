using Contensive.Addon.aoFormWizard3.Controllers;
using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.BaseClasses;
using Contensive.DesignBlockBase.Models.View;
using Contensive.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contensive.Addon.aoFormWizard3.Models.View {
    /// <summary>
    /// Construct the view to be displayed (the current form)
    /// </summary>
    public class FormViewModel : DesignBlockViewBaseModel {
        // 
        public int id { get; set; }
        /// <summary>
        /// A short string that is unique to this form.
        /// </summary>
        public string formHtmlId { get; set; }
        /// <summary>
        /// has to be added to the form so it can be rendered in the callback
        /// </summary>
        public string instanceId { get; set; }
        public bool allowRecaptcha { get; set; }
        public string recaptchaHTML { get; set; }
        public string pageHeader { get; set; }
        public string pageDescription { get; set; }
        public List<FieldViewModel> listOfFieldsClass { get; set; } = new List<FieldViewModel>();
        public string fieldAddLink { get; set; }
        public string previousButton { get; set; }
        public string cancelButton { get; set; }
        public string submitButton { get; set; }
        public string continueButton { get; set; }
        public string formEditWrapper { get; set; }
        public string formdEditLink { get; set; }
        public string formAddLink { get; set; }
        public bool isEditing { get; set; }
        // 
        public class FieldViewModel {
            public string inputtype { get; set; }
            public string caption { get; set; }
            public string headline { get; set; }
            public string fielddescription { get; set; }
            public bool @required { get; set; }
            public string name { get; set; }
            public string currentValue { get; set; }
            public bool isCheckbox { get; set; }
            public bool isRadio { get; set; }
            public bool isSelect { get; set; }
            public bool isTextArea { get; set; }
            public bool isDefault { get; set; }
            public int id { get; set; }
            public List<OptionClass> optionList { get; set; } = new List<OptionClass>();
            public string fieldEditWrapper { get; set; }
            public string fieldEditLink { get; set; }
        }
        // 
        public class OptionClass {
            public string optionName { get; set; }
            public int optionPtr { get; set; }
        }
        // 
        public class ButtonClass {
            public string buttonCaption { get; set; }
            public bool isVisible { get; set; }
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// Populate the view model from the entity model
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static FormViewModel create(CPBaseClass cp, FormSetModel settings) {
            try {
                // 
                // 
                // -- process form request
                if (processRequest(cp, settings)) {
                    // 
                    // -- simple thank you content
                    cp.Doc.SetProperty("formwizardcomplete", true);
                } 
                // 
                // -- base fields
                string requestFormHtmlId = cp.Doc.GetText("formHtmlId");
                //
                var formViewData = create<FormViewModel>(cp, settings);
                formViewData.id = settings.id;
                formViewData.instanceId = settings.ccguid;
                formViewData.formHtmlId = string.IsNullOrEmpty(requestFormHtmlId) ? cp.Utils.GetRandomString(4) : requestFormHtmlId;
                formViewData.allowRecaptcha = settings.allowRecaptcha;
                formViewData.recaptchaHTML = settings.allowRecaptcha ? cp.Addon.Execute(Constants.guidAddonRecaptchav2) : "";

                // 
                var formlist = DbBaseModel.createList<FormModel>(cp, "(formsetid=" + settings.id + ")", "sortorder");
                if (formlist.Count > 0) {
                    var firstForm = formlist.First();
                    // 
                    // -- output one page with page one header
                    formViewData.isEditing = cp.User.IsEditing();
                    formViewData.pageHeader = firstForm.name;
                    formViewData.pageDescription = firstForm.description;
                    formViewData.previousButton = firstForm.addbackbutton ? "Previous" : "";
                    formViewData.cancelButton = firstForm.addcancelbutton ? firstForm.cancelbuttonname : "";
                    formViewData.submitButton = "";
                    formViewData.continueButton = firstForm.addcontinuebutton ? firstForm.continuebuttonname : "";
                    //
                    if (formViewData.isEditing) {
                        formViewData.formEditWrapper = "ccEditWrapper";
                        formViewData.formdEditLink = cp.Content.GetEditLink(FormModel.tableMetadata.contentName, firstForm.id.ToString(), false, "", formViewData.isEditing);
                    }
                    //
                    foreach (var form in formlist) {
                        var formFieldList = DbBaseModel.createList<FormFieldModel>(cp, "(formid=" + form.id + ")", "sortorder, id");
                        int fieldPtr = 0;
                        foreach (var formField in formFieldList) {
                            var optionList = new List<OptionClass>();
                            int optionPtr = 1;
                            foreach (var formfieldoption in formField.optionList.Split(',')) {
                                optionList.Add(new OptionClass() {
                                    optionName = formfieldoption,
                                    optionPtr = optionPtr
                                });
                                optionPtr += 1;
                            }
                            string fieldEditLink = cp.Content.GetEditLink(FormFieldModel.tableMetadata.contentName, formField.id.ToString(), false, "Edit Question", formViewData.isEditing);
                            switch (formField.inputtype.ToLower() ?? "") {
                                case "radio": {
                                        string caption = formField.caption;
                                        if (string.IsNullOrEmpty(caption)) {
                                            caption = formField.name;
                                        }
                                        formViewData.listOfFieldsClass.Add(new FieldViewModel() {
                                            caption = caption,
                                            name = formField.name,
                                            currentValue = "",
                                            inputtype = formField.inputtype,
                                            @required = formField.@required,
                                            headline = formField.headline,
                                            fielddescription = formField.description,
                                            isCheckbox = false,
                                            isDefault = false,
                                            isTextArea = false,
                                            isRadio = true,
                                            isSelect = false,
                                            id = formField.id,
                                            optionList = optionList,
                                            fieldEditLink  = fieldEditLink,
                                            fieldEditWrapper = formViewData.isEditing ? "ccEditWrapper" : ""
                                        });
                                        break;
                                    }
                                case "select": {
                                        string caption = formField.caption;
                                        if (string.IsNullOrEmpty(caption)) {
                                            caption = formField.name;
                                        }
                                        formViewData.listOfFieldsClass.Add(new FieldViewModel() {
                                            caption = caption,
                                            name = formField.name,
                                            currentValue = "",
                                            inputtype = formField.inputtype,
                                            @required = formField.@required,
                                            headline = formField.headline,
                                            fielddescription = formField.description,
                                            isCheckbox = false,
                                            isDefault = false,
                                            isTextArea = false,
                                            isRadio = false,
                                            isSelect = true,
                                            id = formField.id,
                                            optionList = optionList,
                                            fieldEditLink = fieldEditLink,
                                            fieldEditWrapper = formViewData.isEditing ? "ccEditWrapper" : ""
                                        });
                                        break;
                                    }
                                case "checkbox": {
                                        string caption = formField.caption;
                                        if (string.IsNullOrEmpty(caption)) {
                                            caption = formField.name;
                                        }
                                        formViewData.listOfFieldsClass.Add(new FieldViewModel() {
                                            caption = caption,
                                            name = formField.name,
                                            currentValue = "",
                                            inputtype = formField.inputtype,
                                            @required = formField.@required,
                                            headline = formField.headline,
                                            fielddescription = formField.description,
                                            isCheckbox = true,
                                            isDefault = false,
                                            isTextArea = false,
                                            isRadio = false,
                                            isSelect = false,
                                            id = formField.id,
                                            optionList = optionList,
                                            fieldEditLink = fieldEditLink,
                                            fieldEditWrapper = formViewData.isEditing ? "ccEditWrapper" : ""
                                        });
                                        break;
                                    }
                                case "textarea": {
                                        string caption = formField.caption;
                                        if (string.IsNullOrEmpty(caption)) {
                                            caption = formField.name;
                                        }
                                        formViewData.listOfFieldsClass.Add(new FieldViewModel() {
                                            caption = caption,
                                            name = formField.name,
                                            currentValue = "",
                                            inputtype = formField.inputtype,
                                            @required = formField.@required,
                                            headline = formField.headline,
                                            fielddescription = formField.description,
                                            isCheckbox = false,
                                            isDefault = false,
                                            isTextArea = true,
                                            isRadio = false,
                                            isSelect = false,
                                            id = formField.id,
                                            optionList = optionList,
                                            fieldEditLink = fieldEditLink,
                                            fieldEditWrapper = formViewData.isEditing ? "ccEditWrapper" : ""
                                        });
                                        break;
                                    }

                                default: {
                                        string caption = formField.caption;
                                        if (string.IsNullOrEmpty(caption)) {
                                            caption = formField.name;
                                        }
                                        formViewData.listOfFieldsClass.Add(new FieldViewModel() {
                                            caption = caption,
                                            name = formField.name,
                                            currentValue = "",
                                            inputtype = formField.inputtype,
                                            @required = formField.@required,
                                            headline = formField.headline,
                                            fielddescription = formField.description,
                                            isCheckbox = false,
                                            isDefault = true,
                                            isTextArea = false,
                                            isRadio = false,
                                            isSelect = false,
                                            id = formField.id,
                                            optionList = optionList,
                                            fieldEditLink = fieldEditLink,
                                            fieldEditWrapper = formViewData.isEditing ? "ccEditWrapper" : ""
                                        });
                                        break;
                                    }
                            }
                            fieldPtr += 1;
                        }
                        formViewData.fieldAddLink = cp.Content.GetAddLink(FormFieldModel.tableMetadata.contentName, "formid=" + form.id, false, formViewData.isEditing);
                    }
                    formViewData.formAddLink = cp.Content.GetAddLink(FormModel.tableMetadata.contentName, "formsetid=" + settings.id, false, formViewData.isEditing);
                }
                return formViewData;
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                return null;
            }
        }

        /// <summary>
        /// Process submitted contact form. Returns true if the form has already been submitted, or successfully commits
        /// </summary>
        /// <param name="CP"></param>
        /// <param name="formSet"></param>
        /// <returns></returns>
        public static bool processRequest(CPBaseClass CP, FormSetModel formSet) {
            string returnHtml = string.Empty;
            int hint = 0;
            try {
                string button = CP.Doc.GetText("button");
                if (string.IsNullOrWhiteSpace(button) || button.Equals("cancel"))
                    return false;
                UserFormResponseModel userFormResponse = DbBaseModel.createFirstOfList<UserFormResponseModel>(CP, $"visitid={CP.Visit.Id}", "id desc");
                if (userFormResponse is null) {
                    userFormResponse = DbBaseModel.addDefault<UserFormResponseModel>(CP);
                    userFormResponse.name = "Form Set " + formSet.name + " started " + DateTime.Now.ToString("MM/dd/yyyy") + " by " + CP.User.Name;
                }
                userFormResponse.visitid = CP.Visit.Id;
                userFormResponse.memberId = CP.User.Id;
                //
                var htmlVersion = new StringBuilder();
                var textVersion = new StringBuilder();
                //int currentAuthContentRecordId = 0;

                FormResponseDataModel savedAnswersForm = null;
                if (!string.IsNullOrEmpty(userFormResponse.formResponseData)) { savedAnswersForm = CP.JSON.Deserialize<FormResponseDataModel>(userFormResponse.formResponseData); }
                if (savedAnswersForm is null) { savedAnswersForm = new FormResponseDataModel(); }
                if (savedAnswersForm.pageDict is null) { savedAnswersForm.pageDict = []; }
                if (savedAnswersForm.activity is null) { savedAnswersForm.activity = []; }
                //
                foreach (var formPage in DbBaseModel.createList<FormModel>(CP, "(formsetid=" + formSet.id + ")", "sortorder")) {
                    if (!savedAnswersForm.pageDict.TryGetValue(formPage.id, out FormResponseDataPageModel savedAnswersForm_Page)) {
                        savedAnswersForm_Page = new FormResponseDataPageModel {
                            questionDict = [],
                            answer = ""
                        };
                        savedAnswersForm.pageDict.Add(formPage.id, savedAnswersForm_Page);
                    }


                    //// remove any bad characters from the custom content name
                    //string customContentName = form.saveCustomContent;
                    //// replace custom content name with no nonalphanumeric characters
                    //// includes spaces since this is content "[^A-Za-z0-9 ]+"
                    //customContentName = Regex.Replace(customContentName, "[^A-Za-z0-9 ]+", "");
                    //

                    foreach (var formPage_Question in DbBaseModel.createList<FormFieldModel>(CP, $"(formid={formPage.id})", "sortOrder,id")) {
                        if (!savedAnswersForm_Page.questionDict.TryGetValue(formPage_Question.id, out FormResponseDataPageQuestionModel savedAnswersForm_Page_Question)) {
                            savedAnswersForm_Page_Question = new FormResponseDataPageQuestionModel {
                                question = formPage_Question.name,
                                textAnswer = "",
                                choiceAnswerDict = [],
                            };
                            savedAnswersForm_Page.questionDict.Add(formPage_Question.id, savedAnswersForm_Page_Question);
                        }
                        string requestAnswer_Text = CP.Doc.GetText("formField_" + formPage_Question.id);



                        //
                        //// remove any bad characters from the customfieldname
                        //string customFieldName = formsField.name;
                        //// do not include spaces in the field name "[^A-Za-z0-9]+"
                        //customFieldName = Regex.Replace(customFieldName, "[^A-Za-z0-9]+", "");
                        //
                        //
                        // -- move this into a method called after save is complete, on the formResponseData field
                        textVersion.Append($"{Environment.NewLine}Question: " + formPage_Question.name);
                        htmlVersion.Append("<div style=\"padding-top:10px;\"> Question:" + formPage_Question.name + "</div>");
                        //
                        switch (formPage_Question.inputtype.ToLower() ?? "") {
                            case "checkbox":
                            case "radio":
                            case "select": {
                                    hint = 3;
                                    var requestAnswer_OptionsSelectedList = new List<string>(requestAnswer_Text.Split(','));
                                    int optionPtr = 1;
                                    var requestAnswer_OptionsSelectedList_Names = new List<string>();
                                    foreach (var formPage_Question_Option in formPage_Question.optionList.Split(',')) {
                                        bool isSelected = requestAnswer_OptionsSelectedList.Contains(optionPtr.ToString());
                                        savedAnswersForm_Page_Question.choiceAnswerDict.Add(formPage_Question_Option, isSelected);
                                        if (isSelected) {
                                            //
                                            // -- answer is true (found in request)
                                            //
                                            // -- move this into a method called after save is complete, on the formResponseData field
                                            textVersion.Append(Microsoft.VisualBasic.Constants.vbCrLf + Microsoft.VisualBasic.Constants.vbTab + formPage_Question_Option);
                                            htmlVersion.Append("<div style=\"padding-left:20px;\">" + formPage_Question_Option + "</div>");
                                            //
                                            requestAnswer_OptionsSelectedList_Names.Add(formPage_Question_Option);
                                        }
                                        optionPtr += 1;
                                    }
                                    // custom content
                                    //if (form.saveTypeId.Equals(4) & !string.IsNullOrWhiteSpace(customContentName)) {
                                    //    hint = 10;
                                    //    bool verifiedContent = CustomContentController.verifyCustomContent(CP, customContentName);
                                    //    if (verifiedContent) {
                                    //        // make sure the content has this field
                                    //        if (!CP.Content.IsField(customContentName, customFieldName)) {
                                    //            hint = 11;
                                    //            // create a text field
                                    //            CustomContentController.createCustomContentField(CP, customContentName, customFieldName, (int)Constants.FieldTypeIdEnum.Text);
                                    //        }

                                    //        using (var cs = CP.CSNew()) {
                                    //            // after the field should have been created, check again
                                    //            if (CP.Content.IsField(customContentName, customFieldName)) {
                                    //                hint = 12;
                                    //                if (currentAuthContentRecordId == 0) {
                                    //                    hint = 13;
                                    //                    cs.Insert(customContentName);
                                    //                    currentAuthContentRecordId = cs.GetInteger("id");
                                    //                }
                                    //                if (cs.Open(customContentName, "id=" + currentAuthContentRecordId)) {
                                    //                    hint = 14;
                                    //                    // list inot a string with commas
                                    //                    string value = string.Join(",", answerTextList);
                                    //                    cs.SetField(customFieldName, value);
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //}

                                    break;
                                }
                            case "file": {
                                    hint = 20;
                                    string fieldName = "formField_" + formPage_Question.id;
                                    string uploadFilename = CP.Doc.GetText(fieldName);
                                    if (!string.IsNullOrEmpty(uploadFilename)) {
                                        hint = 21;
                                        var folder = DbBaseModel.createByUniqueName<LibraryFolderModel>(CP, "Form Wizard Uploads");
                                        if (folder is null) {
                                            hint = 22;
                                            folder = DbBaseModel.addDefault<LibraryFolderModel>(CP);
                                            folder.name = "Form Wizard Uploads";
                                            folder.save(CP);
                                        }
                                        string pathFilename = "";
                                        using (var cs = CP.CSNew()) {
                                            hint = 23;
                                            cs.Insert("Library Files");
                                            cs.SetFormInput("filename", fieldName);
                                            cs.SetField("folderid", folder.id);
                                            cs.SetField("description", "Form wizard upload by " + CP.User.Name);
                                            cs.Save();
                                            // 
                                            pathFilename = cs.GetText("filename");
                                            var fileDetails = CP.CdnFiles.FileDetails(pathFilename);
                                            cs.SetField("name", fileDetails.Name);
                                            cs.SetField("filesize", fileDetails.Size);
                                            cs.Save();
                                        }
                                        // 
                                        //if (formPage.useauthorgcontent) {
                                        //    hint = 24;
                                        //    if (CP.Content.IsField("Organizations", formsField.name)) {
                                        //        // make sure the form's field exists in the people table
                                        //        using (var cs2 = CP.CSNew()) {
                                        //            if (cs2.Open("Organizations", "id=" + CP.User.OrganizationID)) {
                                        //                hint = 25;
                                        //                cs2.SetField(formsField.name, pathFilename);
                                        //                cs2.Save();
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                        // custom content
                                        //if (form.saveTypeId.Equals(4) & !string.IsNullOrWhiteSpace(customContentName)) {
                                        //    hint = 30;
                                        //    bool verifiedContent = CustomContentController.verifyCustomContent(CP, customContentName);
                                        //    if (verifiedContent) {
                                        //        // make sure the content has this field
                                        //        if (!CP.Content.IsField(customContentName, customFieldName)) {
                                        //            hint = 31;
                                        //            // create file field
                                        //            CustomContentController.createCustomContentField(CP, customContentName, customFieldName, (int)Constants.FieldTypeIdEnum.File);
                                        //        }
                                        //        using (var csContent = CP.CSNew()) {
                                        //            // after the field should have been created, check again
                                        //            if (CP.Content.IsField(customContentName, customFieldName)) {
                                        //                hint = 32;
                                        //                if (currentAuthContentRecordId == 0) {
                                        //                    hint = 33;
                                        //                    csContent.Insert(customContentName);
                                        //                    currentAuthContentRecordId = csContent.GetInteger("id");
                                        //                }
                                        //                if (csContent.Open(customContentName, "id=" + currentAuthContentRecordId)) {
                                        //                    csContent.SetField(customFieldName, pathFilename);
                                        //                    csContent.Save();
                                        //                }
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                        textVersion.Append(Microsoft.VisualBasic.Constants.vbCrLf + Microsoft.VisualBasic.Constants.vbTab + CP.Http.CdnFilePathPrefixAbsolute + pathFilename);
                                        htmlVersion.Append("<div style=\"padding-left:20px;\"><a href=\"" + CP.Http.CdnFilePathPrefixAbsolute + pathFilename + "\">" + uploadFilename + "</a></div>");

                                    }

                                    break;
                                }

                            default: {
                                    //if (form.saveTypeId.Equals(4) & !string.IsNullOrWhiteSpace(customContentName)) {
                                    //    hint = 50;
                                    //    hint = 10;
                                    //    bool verifiedContent = CustomContentController.verifyCustomContent(CP, customContentName);
                                    //    if (verifiedContent) {
                                    //        // make sure the content has this field
                                    //        if (!CP.Content.IsField(customContentName, customFieldName)) {
                                    //            hint = 11;
                                    //            // determine which kind of field to create
                                    //            switch (formsField.inputtype.ToLower() ?? "") {
                                    //                case "text": {
                                    //                        // create a text field if this type is for text
                                    //                        CustomContentController.createCustomContentField(CP, customContentName, customFieldName, (int)Constants.FieldTypeIdEnum.Text);
                                    //                        break;
                                    //                    }

                                    //                default: {
                                    //                        // this is a longtext or unknown so default to longtext
                                    //                        CustomContentController.createCustomContentField(CP, customContentName, customFieldName, (int)Constants.FieldTypeIdEnum.LongText);
                                    //                        break;
                                    //                    }
                                    //            }
                                    //        }

                                    //        using (var cs = CP.CSNew()) {
                                    //            // after the field should have been created, check again
                                    //            if (CP.Content.IsField(customContentName, customFieldName)) {
                                    //                hint = 12;
                                    //                if (currentAuthContentRecordId == 0) {
                                    //                    hint = 13;
                                    //                    cs.Insert(customContentName);
                                    //                    currentAuthContentRecordId = cs.GetInteger("id");
                                    //                }
                                    //                if (cs.Open(customContentName, "id=" + currentAuthContentRecordId)) {
                                    //                    cs.SetField(customFieldName, requestQuestionValue);
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //} else 
                                    if (formPage.saveTypeId.Equals(3)) {
                                        // 
                                        // -- save to organization
                                        if (CP.Content.IsField("organizations", formPage_Question.name)) {
                                            using (var cs = CP.CSNew()) {
                                                // make sure the form's field exists in the people table
                                                if (cs.Open("organizations", "id=" + CP.User.OrganizationID) & cs.FieldOK(formPage_Question.name)) {
                                                    cs.SetField(formPage_Question.name, requestAnswer_Text);
                                                    cs.Save();
                                                }
                                            }
                                        }
                                    } else if (formPage.saveTypeId.Equals(2)) {
                                        // 
                                        // -- save to people
                                        if (CP.Content.IsField("People", formPage_Question.name)) {
                                            using (var cs = CP.CSNew()) {
                                                // make sure the form's field exists in the people table
                                                if (cs.Open("People", "id=" + CP.User.Id) & cs.FieldOK(formPage_Question.name)) {
                                                    cs.SetField(formPage_Question.name, requestAnswer_Text);
                                                    cs.Save();
                                                }
                                            }
                                        }
                                    }
                                    textVersion.Append(Microsoft.VisualBasic.Constants.vbCrLf + Microsoft.VisualBasic.Constants.vbTab + requestAnswer_Text);
                                    htmlVersion.Append("<div style=\"padding-left:20px;\">" + requestAnswer_Text + "</div>");
                                    break;
                                }
                        }
                    }
                }
                CP.Email.sendSystem(formSet.notificationemailid, htmlVersion.ToString());
                if (formSet.responseemailid > 0) {
                    CP.Email.sendSystem(formSet.responseemailid, "", CP.User.Id);
                }
                if (formSet.joingroupid != 0) {
                    CP.Group.AddUser(formSet.joingroupid, CP.User.Id);
                }
                userFormResponse.formResponseData = CP.JSON.Serialize(savedAnswersForm);
                userFormResponse.copy = textVersion.ToString();
                userFormResponse.save(CP);
                return true;
            } catch (Exception ex) {
                CP.Site.ErrorReport("hint=" + hint.ToString() + " " + ex.ToString());
                return false;
            }
        }
    }
}
