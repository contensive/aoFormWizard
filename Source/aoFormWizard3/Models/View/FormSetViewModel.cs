using Contensive.Addon.aoFormWizard3.Controllers;
using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.BaseClasses;
using Contensive.DesignBlockBase.Models.View;
using Contensive.Models.Db;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
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
        public string formHtmlId { get; set; }//
        /// <summary>
        /// The 0-based index to the current page. Saved in the page for processing. Continue moves to the next page.
        /// </summary>
        public int srcPageId { get; set; }
        /// <summary>
        /// has to be added to the form so it can be rendered in the callback
        /// </summary>
        public string instanceId { get; set; }
        public bool allowRecaptcha { get; set; }
        public string recaptchaHTML { get; set; }
        public string pageDescription { get; set; }
        public List<FieldViewModel> listOfFieldsClass { get; set; } = new List<FieldViewModel>();
        public string fieldAddLink { get; set; }
        public string previousButton { get; set; }
        public string resetButton { get; set; }
        public string submitButton { get; set; }
        public string saveButton { get; set; }
        public string continueButton { get; set; }
        public string formEditWrapper { get; set; }
        public string formdEditLink { get; set; }
        public string formAddLink { get; set; }
        public bool isEditing { get; set; }
        public bool isThankYouPage { get; set; }
        public List<FormPagesModel> pageList {get; set;}
        public List<EditingPageData> pageListEditingData {get; set;}
        // 
        public class FieldViewModel {
            public string inputtype { get; set; }
            public string caption { get; set; }
            public string headline { get; set; }
            public string fielddescription { get; set; }
            public bool @required { get; set; }
            public string name { get; set; }
            public string currentValue { get; set; }
            public string currentFileUrl { get; set; }
            public bool isCheckbox { get; set; }
            public bool isRadio { get; set; }
            public bool isSelect { get; set; }
            public bool isTextArea { get; set; }
            public bool isFile { get; set; }
            public bool isDefault { get; set; }
            public int id { get; set; }
            public List<OptionClass> optionList { get; set; } = new List<OptionClass>();
            public string fieldEditWrapper { get; set; }
            public string fieldEditLink { get; set; }
            public bool invalidAnswer { get; set; }
        }
        // 
        public class OptionClass {
            public string optionName { get; set; }
            public int optionPtr { get; set; }
            public bool isSelected { get; set; }
        }
        // 
        public class ButtonClass {
            public string buttonCaption { get; set; }
            public bool isVisible { get; set; }
        }

        public class EditingPageData {
            public string pageDescription { get; set; }
            public List<FieldViewModel> listOfFieldsClass { get; set; } = new List<FieldViewModel>();
            public string fieldAddLink { get; set; }
            public string previousButton { get; set; }
            public string resetButton { get; set; }
            public string submitButton { get; set; }
            public string saveButton { get; set; }
            public string continueButton { get; set; }
            public string formEditWrapper { get; set; }
            public string formdEditLink { get; set; }
            public string formAddLink { get; set; }
            public bool isEditing { get; set; }
            public bool isThankYouPage { get; set; }
        }

        // 
        // ====================================================================================================
        /// <summary>
        /// Populate the view model from the entity model
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static FormViewModel create(CPBaseClass cp, FormWidgetsModel settings) {
            try {
                //
                cp.Log.Debug($"aoFormWizard.FormSetViewModel.create() start");
                // 
                // -- process form request
                // -- the request includes the srcPageId that needs to be processed
                //
                string userFormResponseSql = settings.useUserProperty ? $"memberid = {cp.User.Id}" : $"visitid={cp.Visit.Id}";
                
                FormResponseModel userFormResponse = DbBaseModel.createFirstOfList<FormResponseModel>(cp, userFormResponseSql, "id desc");
                //
                // -- process the request
                processRequest(cp, settings, ref userFormResponse);
                //
                // -- validate the savedAnswers object
                FormResponseDataModel savedAnswers = string.IsNullOrEmpty(userFormResponse?.formResponseData) ? new() : cp.JSON.Deserialize<FormResponseDataModel>(userFormResponse.formResponseData);
                savedAnswers.pageDict ??= [];
                savedAnswers.activity ??= [];
                //
                // -- form completee, show thank you and exit
                if (savedAnswers.isComplete) {
                    return new FormViewModel() {
                        pageDescription = settings.thankyoucopy,
                        isThankYouPage = true
                    };
                }
                //
                // -- validate the current page
                var pageList = FormPagesModel.getPageList(cp, settings.id);                
                var currentpage = pageList.Find((x) => x.id == savedAnswers.currentPageid);
                if (currentpage is null) {
                    savedAnswers.currentPageid = pageList.First().id;
                }
                // 
                // -- begin create output data
                var formViewData = create<FormViewModel>(cp, settings);
                formViewData.id = settings.id;
                formViewData.instanceId = settings.ccguid;
                formViewData.formHtmlId = string.IsNullOrEmpty(("formHtmlId")) ? cp.Utils.GetRandomString(4) : ("formHtmlId");
                formViewData.srcPageId = savedAnswers.currentPageid;
                formViewData.allowRecaptcha = false;
                formViewData.recaptchaHTML = "";
                formViewData.isEditing = cp.User.IsEditing();
                formViewData.pageList = pageList;
                formViewData.pageListEditingData = new List<EditingPageData>();
                
                if (pageList.Count <= 0) { return formViewData; }
                // 
                // -- output one page with page one header
                foreach (FormPagesModel page in pageList) {
                    //
                    //-- skip to the current page
                    if (page.id != savedAnswers.currentPageid && !formViewData.isEditing) { continue; }
                    //
                    // -- recapcha
                    if (page == pageList.First() && settings.allowRecaptcha && !savedAnswers.recaptchaSuccess) {
                        //
                        cp.Log.Debug($"aoFormWizard.FormSetViewModel.create(), add recaptcha");
                        // 
                        formViewData.allowRecaptcha = settings.allowRecaptcha;
                        formViewData.recaptchaHTML = cp.Addon.Execute(Constants.guidAddonRecaptchav2);
                        if (cp.UserError.OK()) {
                            savedAnswers.recaptchaSuccess = true;
                        }
                        //
                        cp.Log.Debug($"aoFormWizard.FormSetViewModel.create(), return recaptchaHTML [{cp.Utils.EncodeHTML(formViewData.recaptchaHTML)}]");
                        //
                    }
                    //
                    FormResponseDataPageModel savedAnswers_Page = savedAnswers.pageDict.TryGetValue(page.id, out var savedAnswers_Page_Result) ? savedAnswers_Page_Result : new FormResponseDataPageModel();
                    savedAnswers_Page.questionDict ??= [];
                    //
                    //formViewData.pageHeader = page.name;
                    formViewData.pageDescription = page.description;
                    formViewData.previousButton = page == pageList.First() ? "" : string.IsNullOrEmpty(settings.backButtonName) ? "Previous" : settings.backButtonName;
                    formViewData.resetButton = !settings.addResetButton ? "" : string.IsNullOrEmpty(settings.resetButtonName) ? "Reset" : settings.resetButtonName;
                    formViewData.submitButton = page != pageList.Last() ? "" : string.IsNullOrEmpty(settings.submitButtonName) ? "Submit" : settings.submitButtonName;
                    formViewData.continueButton = page == pageList.Last() ? "" : string.IsNullOrEmpty(settings.continueButtonName) ? "Continue" : settings.continueButtonName;
                    formViewData.saveButton = !settings.useUserProperty ? "" : string.IsNullOrEmpty(settings.saveButtonName) ? "Save" : settings.saveButtonName;
                    if (formViewData.isEditing) {
                        formViewData.formEditWrapper = "ccEditWrapper";
                        formViewData.formdEditLink = cp.Content.GetEditLink(FormPagesModel.tableMetadata.contentName, page.id.ToString(), false, "", formViewData.isEditing);
                    }
                    //

                    int questionPtr = 0;
                    var questionList = FormQuestionsModel.getQuestionList(cp, page.id);
                    foreach (var question in questionList) {
                        FormResponseDataPageQuestionModel savedAnswers_Page_Question = savedAnswers_Page.questionDict.TryGetValue(question.id, out var savedAnswers_Page_Question_Result) ? savedAnswers_Page_Question_Result : new FormResponseDataPageQuestionModel();
                        savedAnswers_Page_Question.choiceAnswerDict ??= [];
                        //
                        var optionList = new List<OptionClass>();
                        int optionPtr = 1;
                        foreach (var questionOptionName in question.optionList.Split(',')) {
                            if (string.IsNullOrEmpty(questionOptionName)) { continue; }
                            //
                            optionList.Add(new OptionClass() {
                                optionName = questionOptionName,
                                optionPtr = optionPtr,
                                isSelected = savedAnswers_Page_Question.choiceAnswerDict.ContainsKey(questionOptionName) ? savedAnswers_Page_Question.choiceAnswerDict[questionOptionName] : false
                            });
                            optionPtr += 1;
                        }
                        string fieldEditLink = cp.Content.GetEditLink(FormQuestionsModel.tableMetadata.contentName, question.id.ToString(), false, "Edit Question", formViewData.isEditing);
                        //
                        cp.Utils.AppendLog($"form-widget, FormViewModel.create, page [{page.id}], question [{question.id}], fieldEditLink [{fieldEditLink}]");
                        //
                        switch (question.inputTypeId) {
                            case (int)FormQuestionsModel.inputTypeEnum.radio: {
                                    string caption = question.caption;
                                    if (string.IsNullOrEmpty(caption)) {
                                        caption = question.name;
                                    }
                                    formViewData.listOfFieldsClass.Add(new FieldViewModel() {
                                        caption = caption,
                                        name = question.name,
                                        currentValue = "",
                                        inputtype = FormQuestionsModel.getInputTypeName(question.inputTypeId),
                                        @required = question.@required,
                                        headline = question.headline,
                                        fielddescription = question.description,
                                        isCheckbox = false,
                                        isDefault = false,
                                        isTextArea = false,
                                        isRadio = true,
                                        isSelect = false,
                                        isFile = false,
                                        id = question.id,
                                        optionList = optionList,
                                        fieldEditLink = fieldEditLink,
                                        fieldEditWrapper = formViewData.isEditing ? "ccEditWrapper" : "",
                                        invalidAnswer = savedAnswers_Page_Question.invalidAnswer
                                    });
                                    break;
                                }
                            case (int)FormQuestionsModel.inputTypeEnum.select: {
                                    string caption = question.caption;
                                    if (string.IsNullOrEmpty(caption)) {
                                        caption = question.name;
                                    }
                                    formViewData.listOfFieldsClass.Add(new FieldViewModel() {
                                        caption = caption,
                                        name = question.name,
                                        currentValue = "",
                                        inputtype = FormQuestionsModel.getInputTypeName(question.inputTypeId),
                                        @required = question.@required,
                                        headline = question.headline,
                                        fielddescription = question.description,
                                        isCheckbox = false,
                                        isDefault = false,
                                        isTextArea = false,
                                        isRadio = false,
                                        isSelect = true,
                                        isFile = false,
                                        id = question.id,
                                        optionList = optionList,
                                        fieldEditLink = fieldEditLink,
                                        fieldEditWrapper = formViewData.isEditing ? "ccEditWrapper" : "",
                                        invalidAnswer = savedAnswers_Page_Question.invalidAnswer
                                    });
                                    break;
                                }
                            case (int)FormQuestionsModel.inputTypeEnum.checkbox: {
                                    string caption = question.caption;
                                    if (string.IsNullOrEmpty(caption)) {
                                        caption = question.name;
                                    }
                                    formViewData.listOfFieldsClass.Add(new FieldViewModel() {
                                        caption = caption,
                                        name = question.name,
                                        currentValue = "",
                                        inputtype = FormQuestionsModel.getInputTypeName(question.inputTypeId),
                                        @required = question.@required,
                                        headline = question.headline,
                                        fielddescription = question.description,
                                        isCheckbox = true,
                                        isDefault = false,
                                        isTextArea = false,
                                        isRadio = false,
                                        isSelect = false,
                                        isFile = false,
                                        id = question.id,
                                        optionList = optionList,
                                        fieldEditLink = fieldEditLink,
                                        fieldEditWrapper = formViewData.isEditing ? "ccEditWrapper" : "",
                                        invalidAnswer = savedAnswers_Page_Question.invalidAnswer
                                    });
                                    break;
                                }
                            case (int)FormQuestionsModel.inputTypeEnum.textarea: {
                                    string caption = question.caption;
                                    if (string.IsNullOrEmpty(caption)) {
                                        caption = question.name;
                                    }
                                    formViewData.listOfFieldsClass.Add(new FieldViewModel() {
                                        caption = caption,
                                        name = question.name,
                                        currentValue = savedAnswers_Page_Question.textAnswer,
                                        inputtype = FormQuestionsModel.getInputTypeName(question.inputTypeId),
                                        @required = question.@required,
                                        headline = question.headline,
                                        fielddescription = question.description,
                                        isCheckbox = false,
                                        isDefault = false,
                                        isTextArea = true,
                                        isRadio = false,
                                        isSelect = false,
                                        isFile = false,
                                        id = question.id,
                                        optionList = optionList,
                                        fieldEditLink = fieldEditLink,
                                        fieldEditWrapper = formViewData.isEditing ? "ccEditWrapper" : "",
                                        invalidAnswer = savedAnswers_Page_Question.invalidAnswer
                                    });
                                    break;
                                }

                            case (int)FormQuestionsModel.inputTypeEnum.file: {
                                    string caption = question.caption;
                                    if (string.IsNullOrEmpty(caption)) {
                                        caption = question.name;
                                    }
                                    formViewData.listOfFieldsClass.Add(new FieldViewModel() {
                                        caption = caption,
                                        name = question.name,
                                        currentFileUrl = string.IsNullOrEmpty(savedAnswers_Page_Question.textAnswer) ? "" : cp.Http.CdnFilePathPrefixAbsolute + savedAnswers_Page_Question.textAnswer,
                                        currentValue = Path.GetFileName(savedAnswers_Page_Question.textAnswer),
                                        inputtype = FormQuestionsModel.getInputTypeName(question.inputTypeId),
                                        @required = question.@required,
                                        headline = question.headline,
                                        fielddescription = question.description,
                                        isCheckbox = false,
                                        isDefault = false,
                                        isTextArea = false,
                                        isRadio = false,
                                        isSelect = false,
                                        isFile = true,
                                        id = question.id,
                                        optionList = optionList,
                                        fieldEditLink = fieldEditLink,
                                        fieldEditWrapper = formViewData.isEditing ? "ccEditWrapper" : "",
                                        invalidAnswer = savedAnswers_Page_Question.invalidAnswer
                                    });
                                    break;
                                }

                            default: {
                                    string caption = question.caption;
                                    if (string.IsNullOrEmpty(caption)) {
                                        caption = question.name;
                                    }
                                    formViewData.listOfFieldsClass.Add(new FieldViewModel() {
                                        caption = caption,
                                        name = question.name,
                                        currentValue = savedAnswers_Page_Question.textAnswer,
                                        inputtype = FormQuestionsModel.getInputTypeName(question.inputTypeId),
                                        @required = question.@required,
                                        headline = question.headline,
                                        fielddescription = question.description,
                                        isCheckbox = false,
                                        isDefault = true,
                                        isTextArea = false,
                                        isRadio = false,
                                        isSelect = false,
                                        id = question.id,
                                        optionList = optionList,
                                        fieldEditLink = fieldEditLink,
                                        fieldEditWrapper = formViewData.isEditing ? "ccEditWrapper" : "",
                                        invalidAnswer = savedAnswers_Page_Question.invalidAnswer
                                    });
                                    break;
                                }
                        }
                        questionPtr += 1;
                    }
                    formViewData.fieldAddLink = cp.Content.GetAddLink(FormQuestionsModel.tableMetadata.contentName, "formid=" + page.id, false, formViewData.isEditing);
                    //
                    // -- the rendering of this form page is complete. If not editing, exit with just one page
                    if (!formViewData.isEditing) {
                        break;
                    }
                    else {
                        var currentEditingPage = new EditingPageData();
                        currentEditingPage.pageDescription = page.description;
                        currentEditingPage.listOfFieldsClass = formViewData.listOfFieldsClass;
                        currentEditingPage.formEditWrapper = formViewData.formEditWrapper;
                        currentEditingPage.formdEditLink = formViewData.formdEditLink;
                        currentEditingPage.fieldAddLink = formViewData.fieldAddLink;
                        currentEditingPage.formAddLink = cp.Content.GetAddLink(FormPagesModel.tableMetadata.contentName, "formsetid=" + settings.id, false, formViewData.isEditing);
                        formViewData.pageListEditingData.Add(currentEditingPage);
                    }
                }
                formViewData.pageListEditingData.Reverse();
                formViewData.formAddLink = cp.Content.GetAddLink(FormPagesModel.tableMetadata.contentName, "formsetid=" + settings.id, false, formViewData.isEditing);
                return formViewData;
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                return null;
            }
        }

        /// <summary>
        /// Process submitted contact form. 
        /// Returns true if the form has already been submitted, or successfully commits
        /// process the page in the request "srcPageId"
        /// set the savedAnswers.currentPageid to the next page
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="formSet"></param>
        /// <param name="userFormResponse"></param>
        /// <returns></returns>
        public static bool processRequest(CPBaseClass cp, FormWidgetsModel formSet, ref FormResponseModel userFormResponse) {
            string returnHtml = string.Empty;
            int hint = 0;
            try {
                string button = cp.Doc.GetText("button");
                int srcPageId = cp.Doc.GetInteger("srcPageId");
                //
                // -- handle no button
                if (string.IsNullOrWhiteSpace(button) || srcPageId.Equals(0)) { return false; }
                //
                // -- handle reset button
                string resetButton = string.IsNullOrEmpty(formSet.resetButtonName) ? "Reset" : formSet.resetButtonName;
                if (userFormResponse != null && formSet.addResetButton && !string.IsNullOrEmpty(resetButton) && button.Equals(resetButton)) {
                    DbBaseModel.delete<FormResponseModel>(cp, userFormResponse.ccguid);
                    userFormResponse = null;
                    return false;
                }
                //
                // -- verify the users response
                if (userFormResponse is null) {
                    userFormResponse = DbBaseModel.addDefault<FormResponseModel>(cp);
                    userFormResponse.name = "Form Set " + formSet.name + " started " + DateTime.Now.ToString("MM/dd/yyyy") + " by " + cp.User.Name;
                }
                userFormResponse.visitid = cp.Visit.Id;
                userFormResponse.memberId = cp.User.Id;
                //
                // -- determine the page to process
                List<FormPagesModel> pageList = FormPagesModel.getPageList(cp, formSet.id);
                var currentPage = pageList.Find((x) => x.id == srcPageId);
                if (currentPage is null) {
                    //
                    // -- invalid page, exit no processing
                    return false;
                }
                //
                // -- setup the savedAnswers
                FormResponseDataModel savedAnswers = null;
                if (!string.IsNullOrEmpty(userFormResponse.formResponseData)) { savedAnswers = cp.JSON.Deserialize<FormResponseDataModel>(userFormResponse.formResponseData); }
                if (savedAnswers is null) { savedAnswers = new FormResponseDataModel(); }
                if (savedAnswers.pageDict is null) { savedAnswers.pageDict = []; }
                if (savedAnswers.activity is null) { savedAnswers.activity = []; }
                if (!savedAnswers.pageDict.TryGetValue(currentPage.id, out FormResponseDataPageModel savedAnswersForm_Page)) {
                    savedAnswersForm_Page = new FormResponseDataPageModel {
                        questionDict = []
                    };
                    savedAnswers.pageDict.Add(currentPage.id, savedAnswersForm_Page);
                }
                //
                // -- handle previous button
                string backButton = string.IsNullOrEmpty(formSet.backButtonName) ? "Previous" : formSet.backButtonName;
                if (button == backButton) {
                    if (currentPage == pageList.First()) { return false; }
                    var previousPage = pageList.First();
                    foreach (var page in pageList) {
                        if (page == currentPage) { break; }
                        previousPage = page;
                    }
                    savedAnswers.currentPageid = previousPage.id;
                    // -- mark the previous page not complete
                    if (!savedAnswers.pageDict.TryGetValue(currentPage.id, out FormResponseDataPageModel savedAnswersForm_Page2)) {
                        savedAnswersForm_Page2 = new FormResponseDataPageModel {
                            questionDict = []
                        };
                        savedAnswers.pageDict.Add(currentPage.id, savedAnswersForm_Page);
                    }
                    savedAnswersForm_Page2.isCompleted = false;
                    savedAnswersForm_Page2.isStarted = true;
                    userFormResponse.formResponseData = cp.JSON.Serialize(savedAnswers);
                    userFormResponse.save(cp);
                    return true;
                }
                //
                // -- legacy results
                var htmlVersion = new StringBuilder();
                var textVersion = new StringBuilder();
                //
                //// remove any bad characters from the custom content name
                //string customContentName = form.saveCustomContent;
                //// replace custom content name with no nonalphanumeric characters
                //// includes spaces since this is content "[^A-Za-z0-9 ]+"
                //customContentName = Regex.Replace(customContentName, "[^A-Za-z0-9 ]+", "");
                //
                bool returnInvalidAnswer = false;
                foreach (var formPage_Question in FormQuestionsModel.getQuestionList(cp, currentPage.id)) {
                    if (!savedAnswersForm_Page.questionDict.TryGetValue(formPage_Question.id, out FormResponseDataPageQuestionModel savedAnswersForm_Page_Question)) {
                        savedAnswersForm_Page_Question = new FormResponseDataPageQuestionModel {
                            question = formPage_Question.name,
                            textAnswer = "",
                            choiceAnswerDict = [],
                        };
                        savedAnswersForm_Page.questionDict.Add(formPage_Question.id, savedAnswersForm_Page_Question);
                    }
                    string requestAnswer_Text = cp.Doc.GetText("formField_" + formPage_Question.id);
                    //
                    // -- invalid request. Mark the field and continue to check all the fields, then return
                    savedAnswersForm_Page_Question.invalidAnswer = false;
                    //
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
                    switch (formPage_Question.inputTypeId) {
                        case (int)FormQuestionsModel.inputTypeEnum.checkbox:
                        case (int)FormQuestionsModel.inputTypeEnum.radio:
                        case (int)FormQuestionsModel.inputTypeEnum.select: {
                                hint = 3;
                                if (formPage_Question.required && string.IsNullOrEmpty(requestAnswer_Text)) {
                                    //
                                    // -- invalid request. Mark the field and continue to check all the fields
                                    returnInvalidAnswer = true;
                                    savedAnswersForm_Page_Question.invalidAnswer = true;
                                }
                                var requestAnswer_OptionsSelectedList = new List<string>(requestAnswer_Text.Split(','));
                                int optionPtr = 1;
                                var requestAnswer_OptionsSelectedList_Names = new List<string>();
                                foreach (var formPage_Question_Option in formPage_Question.optionList.Split(',')) {
                                    bool isSelected = requestAnswer_OptionsSelectedList.Contains(optionPtr.ToString());
                                    //
                                    // -- save answer
                                    if (!savedAnswersForm_Page_Question.choiceAnswerDict.ContainsKey(formPage_Question_Option)) {
                                        savedAnswersForm_Page_Question.choiceAnswerDict.Add(formPage_Question_Option, isSelected);
                                    } else {
                                        savedAnswersForm_Page_Question.choiceAnswerDict[formPage_Question_Option] = isSelected;
                                    }
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
                                break;
                            }
                        case (int)FormQuestionsModel.inputTypeEnum.file: {
                                hint = 20;
                                string fieldName = "formField_" + formPage_Question.id;
                                string uploadFilename = cp.Doc.GetText(fieldName);
                                if (!string.IsNullOrEmpty(uploadFilename)) {
                                    hint = 21;
                                    var folder = DbBaseModel.createByUniqueName<LibraryFolderModel>(cp, "Form Wizard Uploads");
                                    if (folder is null) {
                                        hint = 22;
                                        folder = DbBaseModel.addDefault<LibraryFolderModel>(cp);
                                        folder.name = "Form Wizard Uploads";
                                        folder.save(cp);
                                    }
                                    string pathFilename = "";
                                    using (var cs = cp.CSNew()) {
                                        hint = 23;
                                        cs.Insert("Library Files");
                                        cs.SetFormInput("filename", fieldName);
                                        cs.SetField("folderid", folder.id);
                                        cs.SetField("description", "Form wizard upload by " + cp.User.Name);
                                        cs.Save();
                                        // 
                                        pathFilename = cs.GetText("filename");
                                        var fileDetails = cp.CdnFiles.FileDetails(pathFilename);
                                        cs.SetField("name", fileDetails.Name);
                                        cs.SetField("filesize", fileDetails.Size);
                                        cs.Save();
                                    }
                                    savedAnswersForm_Page_Question.textAnswer = pathFilename;
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
                                }
                                textVersion.Append(Microsoft.VisualBasic.Constants.vbCrLf);
                                if (!string.IsNullOrEmpty(savedAnswersForm_Page_Question.textAnswer)) {
                                    //
                                    // -- invalid request. Mark the field and continue to check all the fields
                                    textVersion.Append(Microsoft.VisualBasic.Constants.vbTab + cp.Http.CdnFilePathPrefixAbsolute + savedAnswersForm_Page_Question.textAnswer);
                                    string filename = Path.GetFileName(savedAnswersForm_Page_Question.textAnswer);
                                    htmlVersion.Append("<div style=\"padding-left:20px;\"><a href=\"" + cp.Http.CdnFilePathPrefixAbsolute + savedAnswersForm_Page_Question.textAnswer + "\">" + filename + "</a></div>");
                                }
                                //
                                // -- test if this is a required field
                                if (formPage_Question.required && string.IsNullOrEmpty(savedAnswersForm_Page_Question.textAnswer)) {
                                    //
                                    // -- invalid request. Mark the field and continue to check all the fields
                                    returnInvalidAnswer = true;
                                    savedAnswersForm_Page_Question.invalidAnswer = true;
                                }
                                break;
                            }

                        default: {
                                savedAnswersForm_Page_Question.textAnswer = requestAnswer_Text;
                                if (formPage_Question.required && string.IsNullOrEmpty(savedAnswersForm_Page_Question.textAnswer)) {
                                    //
                                    // -- invalid request. Mark the field and continue to check all the fields
                                    returnInvalidAnswer = true;
                                    savedAnswersForm_Page_Question.invalidAnswer = true;
                                }
                                //
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
                                if (currentPage.saveTypeId.Equals(3)) {
                                    // 
                                    // -- save to organization
                                    if (cp.Content.IsField("organizations", formPage_Question.name)) {
                                        using (var cs = cp.CSNew()) {
                                            // make sure the form's field exists in the people table
                                            if (cs.Open("organizations", "id=" + cp.User.OrganizationID) & cs.FieldOK(formPage_Question.name)) {
                                                cs.SetField(formPage_Question.name, requestAnswer_Text);
                                                cs.Save();
                                            }
                                        }
                                    }
                                } else if (currentPage.saveTypeId.Equals(2)) {
                                    // 
                                    // -- save to people
                                    if (cp.Content.IsField("People", formPage_Question.name)) {
                                        using (var cs = cp.CSNew()) {
                                            // make sure the form's field exists in the people table
                                            if (cs.Open("People", "id=" + cp.User.Id) & cs.FieldOK(formPage_Question.name)) {
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
                //
                if (returnInvalidAnswer) {
                    //
                    // -- one or more invalid answers. Save the invalid answer marker and return
                    userFormResponse.formResponseData = cp.JSON.Serialize(savedAnswers);
                    userFormResponse.save(cp);
                    return false;
                }
                //
                // -- continue to next page
                //
                // -- if last question done processing, mark this form-page complete
                string continueButton = string.IsNullOrEmpty(formSet.continueButtonName) ? "Continue" : formSet.continueButtonName;
                string saveButton = string.IsNullOrEmpty(formSet.saveButtonName) ? "Save" : formSet.saveButtonName;
                string submitButton = string.IsNullOrEmpty(formSet.submitButtonName) ? "Submit" : formSet.submitButtonName;
                if (!string.IsNullOrEmpty(button) && (button == continueButton || button == submitButton)) {
                    //
                    // -- mark this page complete
                    savedAnswersForm_Page.isCompleted = true;
                    if (currentPage == pageList.Last()) {
                        //
                        // -- this was the last page, go to thankyou page
                        userFormResponse.dateSubmitted = DateTime.Now;
                        savedAnswers.isComplete = true;
                        savedAnswers.currentPageid = 0;
                    } else {
                        //
                        // -- go to the next page
                        bool setNext = false;
                        foreach (var page in pageList) {
                            if (setNext) {
                                savedAnswers.currentPageid = page.id;
                                break;
                            }
                            if (page.id == currentPage.id) {
                                setNext = true;
                            }
                        }
                    }
                }
                if (currentPage == pageList.Last() && (button == submitButton)) {
                    //
                    // -- the entire form is complete
                    //
                    cp.Email.sendSystem(formSet.notificationemailid, htmlVersion.ToString());
                    if (formSet.responseemailid > 0) {
                        cp.Email.sendSystem(formSet.responseemailid, "", cp.User.Id);
                    }
                    if (formSet.joingroupid != 0) {
                        cp.Group.AddUser(formSet.joingroupid, cp.User.Id);
                    }
                }



                userFormResponse.formResponseData = cp.JSON.Serialize(savedAnswers);
                userFormResponse.copy = textVersion.ToString();
                userFormResponse.save(cp);
                return true;
            } catch (Exception ex) {
                cp.Site.ErrorReport("hint=" + hint.ToString() + " " + ex.ToString());
                return false;
            }
        }
    }
}
